using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static LocalObjTracker.MainForm;


namespace LocalObjTracker
{
    public class SalesforceDatabase
    {
        private string databaseFileName;
        private string connectionString;

        public SalesforceDatabase(int connectionId)
        {
            databaseFileName = $"SalesforceData{connectionId}.db";
            connectionString = $"Data Source={databaseFileName};";
            EnsureDatabaseExists();
        }

        private void EnsureDatabaseExists()
        {
            if (!File.Exists(databaseFileName))
            {
                SQLiteConnection.CreateFile(databaseFileName);
            }
        }

        public void CreateTables(Dictionary<string, List<ManagementDatabase.Field>> dbFields)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                foreach (var table in dbFields)
                {
                    string tableName = ManagementDatabase.IsSqliteKeyword(table.Key) ? "t_" + table.Key : table.Key;

                    List<ManagementDatabase.Field> fields = table.Value;

                    // Create main table
                    string createTableQuery = $"CREATE TABLE IF NOT EXISTS {tableName} (";
                    createTableQuery += "SqliteId INTEGER PRIMARY KEY AUTOINCREMENT, ";
                    foreach (var field in fields)
                    {
                        createTableQuery += $"{field.FieldName} {GetSQLiteType(field.FieldType)}, ";
                    }
                    createTableQuery = createTableQuery.TrimEnd(',', ' ') + ");";

                    using (var command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Create history table
                    string historyTableName = $"{tableName}__History";
                    string createHistoryTableQuery = $"CREATE TABLE IF NOT EXISTS {historyTableName} (";
                    createHistoryTableQuery += "HistoryId INTEGER PRIMARY KEY AUTOINCREMENT, ";
                    createHistoryTableQuery += "Operation TEXT, ";
                    createHistoryTableQuery += "LastRetrievedAt DATETIME, ";
                    foreach (var field in fields)
                    {
                        createHistoryTableQuery += $"{field.FieldName} {GetSQLiteType(field.FieldType)}, ";
                    }
                    createHistoryTableQuery = createHistoryTableQuery.TrimEnd(',', ' ') + ");";

                    using (var command = new SQLiteCommand(createHistoryTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                // create table of t__UserOrGroupName with
                //SqliteId
                //Id
                //SourceObject
                //Name
                //Email
                //LastRetrievedAt
                string userOrGroupNameTable = "t__UserOrGroupName";
                string createUserOrGroupNameTableQuery = $"CREATE TABLE IF NOT EXISTS {userOrGroupNameTable} (";
                createUserOrGroupNameTableQuery += "SqliteId INTEGER PRIMARY KEY AUTOINCREMENT, ";
                createUserOrGroupNameTableQuery += "Id TEXT, ";
                createUserOrGroupNameTableQuery += "SourceObject TEXT, ";
                createUserOrGroupNameTableQuery += "Name TEXT, ";
                createUserOrGroupNameTableQuery += "Email TEXT, ";
                createUserOrGroupNameTableQuery += "LastRetrievedAt DATETIME);";

                using (var command = new SQLiteCommand(createUserOrGroupNameTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }


            }
        }

        private string GetSQLiteType(string fieldType)
        {
            switch (fieldType.ToLower())
            {
                case "string":
                case "picklist":
                case "reference":
                case "textarea":
                case "phone":
                case "url":
                case "email":
                case "id":
                    return "TEXT";
                case "boolean":
                    return "BOOLEAN";
                case "int":
                case "integer":
                    return "INTEGER";
                case "double":
                case "currency":
                case "percent":
                    return "REAL";
                case "date":
                case "datetime":
                    return "DATETIME";
                default:
                    return "TEXT";
            }
        }

        public void InsertRecord(string objName, Dictionary<string, object> record)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string tableName = ManagementDatabase.IsSqliteKeyword(objName) ? "t_" + objName : objName;

                // Insert into main table
                string insertQuery = $"INSERT INTO {tableName} (";
                string valuesPart = "VALUES (";
                foreach (var field in record)
                {
                    insertQuery += $"{field.Key}, ";
                    valuesPart += $"@{field.Key}, ";
                }
                insertQuery = insertQuery.TrimEnd(',', ' ') + ") ";
                valuesPart = valuesPart.TrimEnd(',', ' ') + ");";
                insertQuery += valuesPart;

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    foreach (var field in record)
                    {
                        command.Parameters.AddWithValue($"@{field.Key}", field.Value ?? DBNull.Value);
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertHistoryRecord(string objName, Dictionary<string, object> record, string operation)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string tableName = ManagementDatabase.IsSqliteKeyword(objName) ? "t_" + objName : objName;

                // Insert into history table
                string historyTableName = $"{tableName}__History";
                string insertHistoryQuery = $"INSERT INTO {historyTableName} (Operation, LastRetrievedAt, ";
                string valuesPart = "VALUES (@Operation, @LastRetrievedAt, ";
                foreach (var field in record)
                {
                    insertHistoryQuery += $"{field.Key}, ";
                    valuesPart += $"@{field.Key}, ";
                }
                insertHistoryQuery = insertHistoryQuery.TrimEnd(',', ' ') + ") ";
                valuesPart = valuesPart.TrimEnd(',', ' ') + ");";
                insertHistoryQuery += valuesPart;

                using (var command = new SQLiteCommand(insertHistoryQuery, connection))
                {
                    command.Parameters.AddWithValue("@Operation", operation);
                    command.Parameters.AddWithValue("@LastRetrievedAt", DateTime.Now);
                    foreach (var field in record)
                    {
                        command.Parameters.AddWithValue($"@{field.Key}", field.Value ?? DBNull.Value);
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertUserOrGroupName(string objName, sforce.QueryResult queryResult)
        {
            if (queryResult == null || queryResult.records == null)
            {
                return;
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    string tableName = ManagementDatabase.IsSqliteKeyword(objName) ? "t_" + objName : objName;
                    string insertQuery = $"INSERT INTO t__UserOrGroupName (Id, SourceObject, Name, Email, LastRetrievedAt) VALUES (@Id, @SourceObject, @Name, @Email, @LastRetrievedAt);";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        foreach (var record in queryResult.records)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@Id", record.Id);
                            command.Parameters.AddWithValue("@SourceObject", objName);
                            command.Parameters.AddWithValue("@Name", record.Any.FirstOrDefault(x => x.LocalName == "Name")?.InnerText);
                            command.Parameters.AddWithValue("@Email", record.Any.FirstOrDefault(x => x.LocalName == "Email")?.InnerText);
                            command.Parameters.AddWithValue("@LastRetrievedAt", DateTime.Now);
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
            }

        }

        public void InsertUserOrGroupName(string objName, sforce_sandbox.QueryResult queryResult)
        {
            if (queryResult == null || queryResult.records == null)
            {
                return;
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    string tableName = ManagementDatabase.IsSqliteKeyword(objName) ? "t_" + objName : objName;
                    string insertQuery = $"INSERT INTO t__UserOrGroupName (Id, SourceObject, Name, Email, LastRetrievedAt) VALUES (@Id, @SourceObject, @Name, @Email, @LastRetrievedAt);";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        foreach (var record in queryResult.records)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@Id", record.Id);
                            command.Parameters.AddWithValue("@SourceObject", objName);
                            command.Parameters.AddWithValue("@Name", record.Any.FirstOrDefault(x => x.LocalName == "Name")?.InnerText);
                            command.Parameters.AddWithValue("@Email", record.Any.FirstOrDefault(x => x.LocalName == "Email")?.InnerText);
                            command.Parameters.AddWithValue("@LastRetrievedAt", DateTime.Now);
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
            }

        }
        public void InsertRecordsFromQueryResult(string objName, List<ManagementDatabase.Field> fields, sforce.QueryResult queryResult)
        {
            if (queryResult == null || queryResult.records == null)
                return;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    string tableName = ManagementDatabase.IsSqliteKeyword(objName) ? "t_" + objName : objName;

                    string insertQuery = $"INSERT INTO {tableName} (";
                    string valuesPart = "VALUES (";
                    foreach (var field in fields)
                    {
                        insertQuery += $"{field.FieldName}, ";
                        valuesPart += $"@{field.FieldName}, ";
                    }
                    insertQuery = insertQuery.TrimEnd(',', ' ') + ") ";
                    valuesPart = valuesPart.TrimEnd(',', ' ') + ");";
                    insertQuery += valuesPart;

                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        foreach (var record in queryResult.records)
                        {
                            var recordData = new Dictionary<string, object>();

                            foreach (var field in fields)
                            {
                                var fieldValue = record.Any.FirstOrDefault(x => x.LocalName == field.FieldName)?.InnerText;
                                recordData[field.FieldName] = ConvertFieldValue(field.FieldType, fieldValue);
                            }

                            command.Parameters.Clear();
                            foreach (var field in recordData)
                            {
                                command.Parameters.AddWithValue($"@{field.Key}", field.Value ?? DBNull.Value);
                            }
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
            }
        }

        public void InsertRecordsFromQueryResult(string objName, List<ManagementDatabase.Field> fields, sforce_sandbox.QueryResult queryResult)
        {
            if (queryResult == null || queryResult.records == null)
                return;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    string tableName = ManagementDatabase.IsSqliteKeyword(objName) ? "t_" + objName : objName;

                    string insertQuery = $"INSERT INTO {tableName} (";
                    string valuesPart = "VALUES (";
                    foreach (var field in fields)
                    {
                        insertQuery += $"{field.FieldName}, ";
                        valuesPart += $"@{field.FieldName}, ";
                    }
                    insertQuery = insertQuery.TrimEnd(',', ' ') + ") ";
                    valuesPart = valuesPart.TrimEnd(',', ' ') + ");";
                    insertQuery += valuesPart;

                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        foreach (var record in queryResult.records)
                        {
                            var recordData = new Dictionary<string, object>();

                            foreach (var field in fields)
                            {
                                var fieldValue = record.Any.FirstOrDefault(x => x.LocalName == field.FieldName)?.InnerText;
                                recordData[field.FieldName] = ConvertFieldValue(field.FieldType, fieldValue);
                            }

                            command.Parameters.Clear();
                            foreach (var field in recordData)
                            {
                                command.Parameters.AddWithValue($"@{field.Key}", field.Value ?? DBNull.Value);
                            }
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
            }
        }

        public int InsertRecordsFromBulkCsvResult(string objName, string csvWithHeaderLine)
        {
            int totalRecords = 0;
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    string tableName = ManagementDatabase.IsSqliteKeyword(objName) ? "t_" + objName : objName;

                    // Split the CSV string into lines by CRLF not CR nor LF
                    string[] lines = csvWithHeaderLine.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    if(lines.Length < 2)
                    {
                        return 0;
                    }
                    string[] headers = lines[0].Split(',');
                    string insertQuery = $"INSERT INTO {tableName} (";
                    string valuesPart = "VALUES (";

                    for (int i = 0; i < headers.Length; i++)
                    {
                        headers[i] = headers[i].Trim();

                        // remove double quotes in the first and last characters if exists
                        if (headers[i].Length > 1 && headers[i][0] == '"' && headers[i][headers[i].Length - 1] == '"')
                        {
                            headers[i] = headers[i].Substring(1, headers[i].Length - 2);
                        }
                    }

                    foreach (var header in headers)
                    {
                        insertQuery += $"{header}, ";
                        valuesPart += $"@{header}, ";
                    }
                    insertQuery = insertQuery.TrimEnd(',', ' ') + ") ";
                    valuesPart = valuesPart.TrimEnd(',', ' ') + ");";
                    insertQuery += valuesPart;
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        string mergedLine = "";
                        for (int i = 1; i < lines.Length; i++)
                        {
                            string[] values = lines[i].Split(',');

                            if (values.Length != headers.Length)
                            {
                                mergedLine += mergedLine == "" ? lines[i] : "\r\n" + lines[i];
                                values = mergedLine.Split(',');

                                if (values.Length != headers.Length)
                                    continue;
                            }

                            for (int j = 0; j < headers.Length; j++)
                            {
                                // remove double quotes in the first and last characters if exists
                                if (values[j].Length > 1 && values[j][0] == '"' && values[j][values[j].Length - 1] == '"')
                                {
                                    values[j] = values[j].Substring(1, values[j].Length - 2);
                                }
                                command.Parameters.AddWithValue($"@{headers[j]}", values[j]);
                            }
                            command.ExecuteNonQuery();
                            totalRecords++;
                            mergedLine = "";
                        }
                    }
                    transaction.Commit();
                }
            }
            return totalRecords;
        }

        public List<string> GetLastModifiedByIdListFromObject(string objName, List<string> idList)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string tableName = ManagementDatabase.IsSqliteKeyword(objName) ? "t_" + objName : objName;
                string query = $"SELECT LastModifiedById FROM {tableName} GROUP BY LastModifiedById;";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // if not exists, add to the list
                            if (!idList.Contains(reader.GetString(0)))
                                idList.Add(reader.GetString(0));
                        }
                        return idList;
                    }
                }
            }
        }

        public List<string> GetNotExistIds(List<string> idList)
        {
            if (idList.Count == 0)
            {
                return new List<string>();
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT Id FROM t__UserOrGroupName WHERE Id IN ({string.Join(",", idList.Select(x => $"'{x}'"))});";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // if exists, remove from the list
                            idList.Remove(reader.GetString(0));
                        }
                        return idList;
                    }
                }
            }
        }

        public bool RecordExists(string tableName, string id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT COUNT(*) FROM {tableName} WHERE Id = @Id;";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    return (long)command.ExecuteScalar() > 0;
                }
            }
        }

        private object ConvertFieldValue(string fieldType, string fieldValue)
        {
            if (fieldValue == null || fieldValue == "")
            {
                return DBNull.Value;
            }

            switch (fieldType.ToLower())
            {
                case "string":
                case "picklist":
                case "reference":
                case "textarea":
                case "phone":
                case "url":
                case "email":
                case "id":
                    return fieldValue;
                case "boolean":
                    return bool.Parse(fieldValue);
                case "int":
                case "integer":
                    return int.Parse(fieldValue);
                case "double":
                case "currency":
                case "percent":
                    return double.Parse(fieldValue);
                case "date":
                case "datetime":
                    return DateTime.Parse(fieldValue).ToUniversalTime();
                default:
                    return fieldValue;
            }
        }

        private string FieldValueToStringForApexUpdate(object fieldValue)
        {
            if (fieldValue == null || fieldValue == DBNull.Value)
            {
                return "null";
            }
            if (fieldValue is string)
            {
                return $"'{fieldValue}'";
            }
            if (fieldValue is bool)
            {
                return (bool)fieldValue ? "true" : "false";
            }
            if (fieldValue is DateTime)
            {
                // DateTime.newInstanceGMT(...) in Apex
                return $"DateTime.newInstanceGMT({((DateTime)fieldValue).Year}, {((DateTime)fieldValue).Month}, {((DateTime)fieldValue).Day}, {((DateTime)fieldValue).Hour}, {((DateTime)fieldValue).Minute}, {((DateTime)fieldValue).Second})";
            }
            return fieldValue.ToString();
        }

        public DateTime GetLastModifiedDate(string tableName)
        {
            // Get the last modified date from the data table by field name "LastModifiedDate"
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT MAX(LastModifiedDate) FROM {tableName};";
                using (var command = new SQLiteCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result == DBNull.Value)
                    {
                        return DateTime.MinValue;
                    }
                    return DateTime.Parse(result.ToString()).ToUniversalTime();
                }
            }
        }

        public void DeleteRecord(string sqliteTableName, string recordId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $"DELETE FROM {sqliteTableName} WHERE Id = @Id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", recordId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateRecordById(string sqliteTableName, Dictionary<string, object> recordData, string recordId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $"UPDATE {sqliteTableName} SET ";
                foreach (var field in recordData)
                {
                    query += $"{field.Key} = @{field.Key}, ";
                }
                query = query.TrimEnd(',', ' ') + $" WHERE Id = @Id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    foreach (var field in recordData)
                    {
                        command.Parameters.AddWithValue($"@{field.Key}", field.Value ?? DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@Id", recordId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Dictionary<string, object> GetRecordById(string sqliteTableName, string recordId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT * FROM {sqliteTableName} WHERE Id = @Id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", recordId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var recordData = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                recordData[reader.GetName(i)] = reader.GetValue(i);
                            }
                            return recordData;
                        }
                        return null;
                    }
                }
            }
        }

        public List<Dictionary<string, object>> GetTrackHistories(string sqliteTableName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT * FROM {sqliteTableName}__History";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var histories = new List<Dictionary<string, object>>();
                        while (reader.Read())
                        {
                            var historyData = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                historyData[reader.GetName(i)] = reader.GetValue(i);
                            }
                            histories.Add(historyData);
                        }
                        return histories;
                    }
                }
            }
        }

        internal int GetCount(string sqliteTableName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT COUNT(*) FROM {sqliteTableName}";
                using (var command = new SQLiteCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public (DateTime, string, string) GetLastModifiedDateAndIdAndName(string sqliteTableName)
        {
            // sqliteTableName join with t__UserOrGroupName to get the LastModifiedById and Name
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT {sqliteTableName}.LastModifiedDate, t__UserOrGroupName.Id, t__UserOrGroupName.Name FROM {sqliteTableName} JOIN t__UserOrGroupName ON {sqliteTableName}.LastModifiedById = t__UserOrGroupName.Id ORDER BY {sqliteTableName}.LastModifiedDate DESC LIMIT 1;";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (DateTime.Parse(reader.GetString(0)).ToUniversalTime(), reader.GetString(1), reader.GetString(2));
                        }
                        return (DateTime.MinValue, "", "");
                    }
                }
            }
        }

        public string GetUserOrGroupNameById(string id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT Name FROM t__UserOrGroupName WHERE Id = @Id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    return command.ExecuteScalar()?.ToString();
                }
            }
        }

        internal string DescribeDifference(string objName, int historyIdBeforce, int historyIdAfter)
        {
            string tableName = ManagementDatabase.IsSqliteKeyword(objName) ? "t_" + objName : objName;

            // Insert into history table
            string historyTableName = $"{tableName}__History";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string queryBefore = $"SELECT * FROM {historyTableName} WHERE HistoryId = {historyIdBeforce}";
                var before = new Dictionary<string, object>();

                using (var command = new SQLiteCommand(queryBefore, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                // skip HistoryId, Operation, LastRetrievedAt, LastModifiedById, LastModifiedDate, SystemModstamp, LastViewedDate, LastReferencedDate
                                if (reader.GetName(i) == "HistoryId" || reader.GetName(i) == "Operation" || reader.GetName(i) == "LastRetrievedAt" || reader.GetName(i) == "LastModifiedById" || reader.GetName(i) == "LastModifiedDate" || reader.GetName(i) == "SystemModstamp" || reader.GetName(i) == "LastViewedDate" || reader.GetName(i) == "LastReferencedDate")
                                {
                                    continue;
                                }


                                before[reader.GetName(i)] = reader.GetValue(i);
                            }
                        }
                    }
                }

                string queryAfter = $"SELECT * FROM {historyTableName} WHERE HistoryId = {historyIdAfter}";
                var after = new Dictionary<string, object>();

                using (var command = new SQLiteCommand(queryAfter, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                // skip HistoryId, Operation, LastRetrievedAt, LastModifiedById, LastModifiedDate, SystemModstamp, LastViewedDate, LastReferencedDate
                                if (reader.GetName(i) == "HistoryId" || reader.GetName(i) == "Operation" || reader.GetName(i) == "LastRetrievedAt" || reader.GetName(i) == "LastModifiedById" || reader.GetName(i) == "LastModifiedDate" || reader.GetName(i) == "SystemModstamp" || reader.GetName(i) == "LastViewedDate" || reader.GetName(i) == "LastReferencedDate")
                                {
                                    continue;
                                }
                                after[reader.GetName(i)] = reader.GetValue(i);
                            }
                        }
                    }
                }

                var diff = new StringBuilder();
                foreach (var key in before.Keys)
                {
                    if (!after.ContainsKey(key))
                    {
                        diff.AppendLine($"{key}: {before[key]} => (null)  ");
                    }
                    else if (!before[key].Equals(after[key]))
                    {
                        diff.AppendLine($"{key}: {before[key]} => {after[key]}  ");
                    }
                }

                return diff.ToString();

            }
        }

        internal string GetRestoreApexUpdate(string objName, List<ManagementDatabase.Field> fields, string id, int historyIdBeforce, int historyIdAfter)
        {
            string tableName = ManagementDatabase.IsSqliteKeyword(objName) ? "t_" + objName : objName;

            // Insert into history table
            string historyTableName = $"{tableName}__History";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string queryBefore = $"SELECT * FROM {historyTableName} WHERE HistoryId = {historyIdBeforce}";
                var before = new Dictionary<string, object>();

                using (var command = new SQLiteCommand(queryBefore, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                // skip HistoryId, Operation, LastRetrievedAt, LastModifiedById, LastModifiedDate, SystemModstamp, LastViewedDate, LastReferencedDate
                                if (reader.GetName(i) == "HistoryId" || reader.GetName(i) == "Operation" || reader.GetName(i) == "LastRetrievedAt" || reader.GetName(i) == "LastModifiedById" || reader.GetName(i) == "LastModifiedDate" || reader.GetName(i) == "SystemModstamp" || reader.GetName(i) == "LastViewedDate" || reader.GetName(i) == "LastReferencedDate")
                                {
                                    continue;
                                }

                                // find reader.GetName(i) in fields, then skip if it is not updateable
                                if (!fields.Any(x => x.FieldName == reader.GetName(i) && x.Updateable))
                                {
                                    continue;
                                }


                                before[reader.GetName(i)] = reader.GetValue(i);
                            }
                        }
                    }
                }

                string queryAfter = $"SELECT * FROM {historyTableName} WHERE HistoryId = {historyIdAfter}";
                var after = new Dictionary<string, object>();

                using (var command = new SQLiteCommand(queryAfter, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                // skip HistoryId, Operation, LastRetrievedAt, LastModifiedById, LastModifiedDate, SystemModstamp, LastViewedDate, LastReferencedDate
                                if (reader.GetName(i) == "HistoryId" || reader.GetName(i) == "Operation" || reader.GetName(i) == "LastRetrievedAt" || reader.GetName(i) == "LastModifiedById" || reader.GetName(i) == "LastModifiedDate" || reader.GetName(i) == "SystemModstamp" || reader.GetName(i) == "LastViewedDate" || reader.GetName(i) == "LastReferencedDate")
                                {
                                    continue;
                                }
                                
                                // find reader.GetName(i) in fields, then skip if it is not updateable
                                if (!fields.Any(x => x.FieldName == reader.GetName(i) && x.Updateable))
                                {
                                    continue;
                                }

                                after[reader.GetName(i)] = reader.GetValue(i);
                            }
                        }
                    }
                }

                Dictionary<string, string> diffFieldValue = new Dictionary<string, string>();
                foreach (var key in before.Keys)
                {
                    if (!after.ContainsKey(key) || !before[key].Equals(after[key]))
                    {
                        diffFieldValue[key] = FieldValueToStringForApexUpdate(before[key]);
                    }
                }

                if(diffFieldValue.Count == 0)
                {
                    return "";
                }

                // build update statement by objName, id and before, for example:
                //{
                //    Account rec = [SELECT BillingCity FROM Account WHERE ID = '001GA000050fAYUYA2' LIMIT 1];
                //    if (rec != null)
                //    {
                //        rec.BillingCity = 'New York1';
                //        update rec;
                //    }
                //}
                string allFields = string.Join(", ", diffFieldValue.Select(x => x.Key));
                var update = new StringBuilder();
                update.AppendLine($"{{");
                update.AppendLine($"\t{objName} rec = [SELECT {allFields} FROM {objName} WHERE ID = '{id}' LIMIT 1];");
                update.AppendLine($"\tif (rec != null)");
                update.AppendLine($"\t{{");
                foreach (var field in diffFieldValue)
                {
                    update.AppendLine($"\t\trec.{field.Key} = {field.Value};");
                }
                update.AppendLine($"\t\tupdate rec;");
                update.AppendLine($"\t}}");
                update.AppendLine($"}}");

                return update.ToString();
            }
        }
    }
}

