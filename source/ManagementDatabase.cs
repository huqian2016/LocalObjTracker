using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalObjTracker
{
    public class ManagementDatabase
    {
        private static ManagementDatabase instance;
        private const string ManagementDatabaseFileName = "SalesforceManagement.db";
        private const string ConnectionString = "Data Source=" + ManagementDatabaseFileName + ";";
        private static readonly HashSet<string> sqliteKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ABORT", "ACTION", "ADD", "AFTER", "ALL", "ALTER", "ALWAYS", "ANALYZE", "AND", "AS", "ASC", "ATTACH", "AUTOINCREMENT",
            "BEFORE", "BEGIN", "BETWEEN", "BY", "CASCADE", "CASE", "CAST", "CHECK", "COLLATE", "COLUMN", "COMMIT", "CONFLICT",
            "CONSTRAINT", "CREATE", "CROSS", "CURRENT", "CURRENT_DATE", "CURRENT_TIME", "CURRENT_TIMESTAMP", "DATABASE", "DEFAULT",
            "DEFERRABLE", "DEFERRED", "DELETE", "DESC", "DETACH", "DISTINCT", "DO", "DROP", "EACH", "ELSE", "END", "ESCAPE", "EXCEPT",
            "EXCLUDE", "EXCLUSIVE", "EXISTS", "EXPLAIN", "FAIL", "FILTER", "FIRST", "FOLLOWING", "FOR", "FOREIGN", "FROM", "FULL",
            "GENERATED", "GLOB", "GROUP", "GROUPS", "HAVING", "IF", "IGNORE", "IMMEDIATE", "IN", "INDEX", "INDEXED", "INITIALLY",
            "INNER", "INSERT", "INSTEAD", "INTERSECT", "INTO", "IS", "ISNULL", "JOIN", "KEY", "LAST", "LEFT", "LIKE", "LIMIT", "MATCH",
            "MATERIALIZED", "NATURAL", "NO", "NOT", "NOTHING", "NOTNULL", "NULL", "NULLS", "OF", "OFFSET", "ON", "OR", "ORDER", "OTHERS",
            "OUTER", "OVER", "PARTITION", "PLAN", "PRAGMA", "PRECEDING", "PRIMARY", "QUERY", "RAISE", "RANGE", "RECURSIVE", "REFERENCES",
            "REGEXP", "REINDEX", "RELEASE", "RENAME", "REPLACE", "RESTRICT", "RETURNING", "RIGHT", "ROLLBACK", "ROW", "ROWS", "SAVEPOINT",
            "SELECT", "SET", "TABLE", "TEMP", "TEMPORARY", "THEN", "TIES", "TO", "TRANSACTION", "TRIGGER", "UNBOUNDED", "UNION", "UNIQUE",
            "UPDATE", "USING", "VACUUM", "VALUES", "VIEW", "VIRTUAL", "WHEN", "WHERE", "WINDOW", "WITH", "WITHOUT"
        };

        public static bool IsSqliteKeyword(string input)
        {
            return sqliteKeywords.Contains(input);
        }

        // 文字列をBase64エンコードする
        public static string EncodeToBase64(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        // Base64エンコードされた文字列をデコードする
        public static string DecodeFromBase64(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private ManagementDatabase()
        {
            EnsureDatabaseExists();
        }

        public static ManagementDatabase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ManagementDatabase();
                }
                return instance;
            }
        }

        public class SalesforceConnection
        {
            public int ConnectionId { get; set; }
            public string ConnectionName { get; set; } = "Salesforce";
            public string Username { get; set; }
            public string Password { get; set; }
            public string SecurityToken { get; set; }
            public string ServerUrl { get; set; }
            public string MyDomain { get; set; }
            public string OrgnaizationType { get; set; }
            public string UserEmail { get; set; }
            public string UserFullName { get; set; }
            public string UserID { get; set; }
            public string OrganizationId { get; set; }
            public string OrganizationName { get; set; }
            public string ProfileId { get; set; }
            public string RoleId { get; set; }
            public string UserType { get; set; }
            public string UserTimeZone { get; set; }
            public int IntervalTime { get; set; } = 60;
            public DateTime? LastRetrievedAt { get; set; }
        }


        public List<SalesforceConnection> GetSalesforceConnections()
        {
            List<SalesforceConnection> connections = new List<SalesforceConnection>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string selectConnections = @"
            SELECT ConnectionId, ConnectionName, Username, Password, SecurityToken, ServerUrl, MyDomain, OrgnaizationType, UserEmail, UserFullName, UserID, OrganizationId, OrganizationName, ProfileId, RoleId, UserType, UserTimeZone, IntervalTime, LastRetrievedAt
            FROM SalesforceConnections ORDER BY ConnectionId DESC;";
                using (var command = new SQLiteCommand(selectConnections, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            connections.Add(new SalesforceConnection
                            {
                                ConnectionId = reader.GetInt32(0),
                                ConnectionName = reader.GetString(1),
                                Username = reader.GetString(2),
                                Password = DecodeFromBase64(reader.GetString(3)),
                                SecurityToken = reader.IsDBNull(4) ? null : reader.GetString(4),
                                ServerUrl = reader.IsDBNull(5) ? null : reader.GetString(5),
                                MyDomain = reader.IsDBNull(6) ? null : reader.GetString(6),
                                OrgnaizationType = reader.IsDBNull(7) ? null : reader.GetString(7),
                                UserEmail = reader.IsDBNull(8) ? null : reader.GetString(8),
                                UserFullName = reader.IsDBNull(9) ? null : reader.GetString(9),
                                UserID = reader.IsDBNull(10) ? null : reader.GetString(10),
                                OrganizationId = reader.IsDBNull(11) ? null : reader.GetString(11),
                                OrganizationName = reader.IsDBNull(12) ? null : reader.GetString(12),
                                ProfileId = reader.IsDBNull(13) ? null : reader.GetString(13),
                                RoleId = reader.IsDBNull(14) ? null : reader.GetString(14),
                                UserType = reader.IsDBNull(15) ? null : reader.GetString(15),
                                UserTimeZone = reader.IsDBNull(16) ? null : reader.GetString(16),
                                IntervalTime = reader.IsDBNull(17) ? 0 : reader.GetInt32(17),
                                LastRetrievedAt = reader.IsDBNull(18) ? (DateTime?)null : reader.GetDateTime(18)
                            });
                        }
                    }
                }
            }
            return connections;
        }

        public int InsertSalesforceConnection(SalesforceConnection connection)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string insertConnection = @"
            INSERT INTO SalesforceConnections (ConnectionName, Username, Password, SecurityToken, ServerUrl, MyDomain, OrgnaizationType, UserEmail, UserFullName, UserID, OrganizationId, OrganizationName, ProfileId, RoleId, UserType, UserTimeZone, IntervalTime, LastRetrievedAt)
            VALUES (@ConnectionName, @Username, @Password, @SecurityToken, @ServerUrl, @MyDomain, @OrgnaizationType, @UserEmail, @UserFullName, @UserID, @OrganizationId, @OrganizationName, @ProfileId, @RoleId, @UserType, @UserTimeZone, @IntervalTime, @LastRetrievedAt);
            SELECT last_insert_rowid();";
                using (var command = new SQLiteCommand(insertConnection, conn))
                {
                    command.Parameters.AddWithValue("@ConnectionName", connection.ConnectionName);
                    command.Parameters.AddWithValue("@Username", connection.Username);
                    command.Parameters.AddWithValue("@Password", EncodeToBase64(connection.Password));
                    command.Parameters.AddWithValue("@SecurityToken", connection.SecurityToken);
                    command.Parameters.AddWithValue("@ServerUrl", connection.ServerUrl);
                    command.Parameters.AddWithValue("@MyDomain", connection.MyDomain);
                    command.Parameters.AddWithValue("@OrgnaizationType", connection.OrgnaizationType);
                    command.Parameters.AddWithValue("@UserEmail", connection.UserEmail);
                    command.Parameters.AddWithValue("@UserFullName", connection.UserFullName);
                    command.Parameters.AddWithValue("@UserID", connection.UserID);
                    command.Parameters.AddWithValue("@OrganizationId", connection.OrganizationId);
                    command.Parameters.AddWithValue("@OrganizationName", connection.OrganizationName);
                    command.Parameters.AddWithValue("@ProfileId", connection.ProfileId);
                    command.Parameters.AddWithValue("@RoleId", connection.RoleId);
                    command.Parameters.AddWithValue("@UserType", connection.UserType);
                    command.Parameters.AddWithValue("@UserTimeZone", connection.UserTimeZone);
                    command.Parameters.AddWithValue("@IntervalTime", connection.IntervalTime);
                    command.Parameters.AddWithValue("@LastRetrievedAt", connection.LastRetrievedAt.HasValue ? (object)connection.LastRetrievedAt.Value : DBNull.Value);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public void UpdateSalesforceConnection(SalesforceConnection connection)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string updateConnection = @"
            UPDATE SalesforceConnections
            SET ConnectionName = @ConnectionName,
                Username = @Username,
                Password = @Password,
                SecurityToken = @SecurityToken,
                ServerUrl = @ServerUrl,
                MyDomain = @MyDomain,
                OrgnaizationType = @OrgnaizationType,
                UserEmail = @UserEmail,
                UserFullName = @UserFullName,
                UserID = @UserID,
                OrganizationId = @OrganizationId,
                OrganizationName = @OrganizationName,
                ProfileId = @ProfileId,
                RoleId = @RoleId,
                UserType = @UserType,
                UserTimeZone = @UserTimeZone,
                IntervalTime = @IntervalTime,
                LastRetrievedAt = @LastRetrievedAt
            WHERE ConnectionId = @ConnectionId;";
                using (var command = new SQLiteCommand(updateConnection, conn))
                {
                    command.Parameters.AddWithValue("@ConnectionName", connection.ConnectionName);
                    command.Parameters.AddWithValue("@Username", connection.Username);
                    command.Parameters.AddWithValue("@Password", EncodeToBase64(connection.Password));
                    command.Parameters.AddWithValue("@SecurityToken", connection.SecurityToken);
                    command.Parameters.AddWithValue("@ServerUrl", connection.ServerUrl);
                    command.Parameters.AddWithValue("@MyDomain", connection.MyDomain);
                    command.Parameters.AddWithValue("@OrgnaizationType", connection.OrgnaizationType);
                    command.Parameters.AddWithValue("@UserEmail", connection.UserEmail);
                    command.Parameters.AddWithValue("@UserFullName", connection.UserFullName);
                    command.Parameters.AddWithValue("@UserID", connection.UserID);
                    command.Parameters.AddWithValue("@OrganizationId", connection.OrganizationId);
                    command.Parameters.AddWithValue("@OrganizationName", connection.OrganizationName);
                    command.Parameters.AddWithValue("@ProfileId", connection.ProfileId);
                    command.Parameters.AddWithValue("@RoleId", connection.RoleId);
                    command.Parameters.AddWithValue("@UserType", connection.UserType);
                    command.Parameters.AddWithValue("@UserTimeZone", connection.UserTimeZone);
                    command.Parameters.AddWithValue("@IntervalTime", connection.IntervalTime);
                    command.Parameters.AddWithValue("@LastRetrievedAt", connection.LastRetrievedAt.HasValue ? (object)connection.LastRetrievedAt.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@ConnectionId", connection.ConnectionId);
                    command.ExecuteNonQuery();
                }
            }
        }



        public void EnsureDatabaseExists()
        {
            if (!System.IO.File.Exists(ManagementDatabaseFileName))
            {
                SQLiteConnection.CreateFile(ManagementDatabaseFileName);
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    string createSalesforceConnectionsTable = @"
                        CREATE TABLE IF NOT EXISTS SalesforceConnections (
                            ConnectionId INTEGER PRIMARY KEY AUTOINCREMENT,
                            ConnectionName TEXT NOT NULL,
                            Username TEXT NOT NULL,
                            Password TEXT NOT NULL,
                            SecurityToken TEXT,
                            ServerUrl TEXT,
                            MyDomain TEXT,
                            OrgnaizationType TEXT,
                            UserEmail TEXT,
                            UserFullName TEXT,
                            UserID TEXT,
                            OrganizationId TEXT,
                            OrganizationName TEXT,
                            ProfileId TEXT,
                            RoleId TEXT,
                            UserType TEXT,
                            UserTimeZone TEXT,
                            IntervalTime INTEGER,
                            LastRetrievedAt DATETIME
                        );";

                    string createTrackObjectsTable = @"
                        CREATE TABLE IF NOT EXISTS TrackObjects (
                            ObjectId INTEGER PRIMARY KEY AUTOINCREMENT,
                            ConnectionId INTEGER,
                            ObjectName TEXT NOT NULL,
                            ObjectLabel TEXT,
                            SqliteTableName TEXT NOT NULL,
                            Custom BOOLEAN,
                            Createable BOOLEAN,
                            Deletable BOOLEAN,
                            Queryable BOOLEAN,
                            Replicateable BOOLEAN,
                            Retrieveable BOOLEAN,
                            Searchable BOOLEAN,
                            Undeletable BOOLEAN,
                            Updateable BOOLEAN,
                            InitCount INTEGER,
                            InitModifiedDate DATETIME,
                            LastRetrievedAt DATETIME,
                            FOREIGN KEY (ConnectionId) REFERENCES SalesforceConnections(ConnectionId)
                        );";

                    string createSettingsTable = @"
                        CREATE TABLE IF NOT EXISTS Settings (
                            SettingId INTEGER PRIMARY KEY AUTOINCREMENT,
                            ConnectionId INTEGER,
                            Key TEXT NOT NULL,
                            Value TEXT NOT NULL,
                            FOREIGN KEY (ConnectionId) REFERENCES SalesforceConnections(ConnectionId)
                        );";

                    string createFieldsTable = @"
                        CREATE TABLE IF NOT EXISTS Fields (
                            FieldId INTEGER PRIMARY KEY AUTOINCREMENT,
                            ObjectId INTEGER,
                            FieldName TEXT NOT NULL,
                            FieldLabel TEXT,
                            FieldType TEXT,
                            NameField BOOLEAN,
                            RestrictedPicklist BOOLEAN,
                            Length INTEGER,
                            Scale INTEGER,
                            Precision INTEGER,
                            Digits INTEGER,
                            Custom BOOLEAN,
                            Nillable BOOLEAN,
                            Createable BOOLEAN,
                            Filterable BOOLEAN,
                            Updateable BOOLEAN,
                            ReferenceTo TEXT,
                            LastRetrievedAt DATETIME,
                            FOREIGN KEY (ObjectId) REFERENCES TrackObjects(ObjectId)
                        );";

                    string createRetrieveLogTable = @"
                        CREATE TABLE IF NOT EXISTS RetrieveLog (
                            LogId INTEGER PRIMARY KEY AUTOINCREMENT,
                            ObjectId INTEGER,
                            Type TEXT,
                            TotalRecordCount INTEGER,
                            UpdateCount INTEGER,
                            DeleteCount INTEGER,
                            InsertCount INTEGER,
                            RetrievedAt DATETIME,
                            FOREIGN KEY (ObjectId) REFERENCES TrackObjects(ObjectId)
                        );";

                    using (var command = new SQLiteCommand(createSalesforceConnectionsTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (var command = new SQLiteCommand(createTrackObjectsTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (var command = new SQLiteCommand(createSettingsTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (var command = new SQLiteCommand(createFieldsTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (var command = new SQLiteCommand(createRetrieveLogTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public class Setting
        {
            public int SettingId { get; set; }
            public int ConnectionId { get; set; }
            public string Key { get; set; }
            public string Value { get; set; }
        }

        // Settings テーブルのレコードを取得
        public List<Setting> GetSettings()
        {
            List<Setting> settings = new List<Setting>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string selectSettings = @"
            SELECT SettingId, ConnectionId, Key, Value
            FROM Settings;";
                using (var command = new SQLiteCommand(selectSettings, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            settings.Add(new Setting
                            {
                                SettingId = reader.GetInt32(0),
                                ConnectionId = reader.GetInt32(1),
                                Key = reader.GetString(2),
                                Value = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            return settings;
        }

        // Settings テーブルにレコードを挿入し、挿入したレコードの SettingId を返す
        public int InsertSetting(Setting setting)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string insertSetting = @"
            INSERT INTO Settings (ConnectionId, Key, Value)
            VALUES (@ConnectionId, @Key, @Value);
            SELECT last_insert_rowid();";
                using (var command = new SQLiteCommand(insertSetting, conn))
                {
                    command.Parameters.AddWithValue("@ConnectionId", setting.ConnectionId);
                    command.Parameters.AddWithValue("@Key", setting.Key);
                    command.Parameters.AddWithValue("@Value", setting.Value);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        // Settings テーブルのレコードを更新
        public void UpdateSetting(Setting setting)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string updateSetting = @"
            UPDATE Settings
            SET ConnectionId = @ConnectionId,
                Key = @Key,
                Value = @Value
            WHERE SettingId = @SettingId;";
                using (var command = new SQLiteCommand(updateSetting, conn))
                {
                    command.Parameters.AddWithValue("@ConnectionId", setting.ConnectionId);
                    command.Parameters.AddWithValue("@Key", setting.Key);
                    command.Parameters.AddWithValue("@Value", setting.Value);
                    command.Parameters.AddWithValue("@SettingId", setting.SettingId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Settings テーブルのレコードを削除
        public void DeleteSetting(int settingId)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string deleteSetting = @"
            DELETE FROM Settings
            WHERE SettingId = @SettingId;";
                using (var command = new SQLiteCommand(deleteSetting, conn))
                {
                    command.Parameters.AddWithValue("@SettingId", settingId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public class TrackObject
        {
            public int ObjectId { get; set; }
            public int ConnectionId { get; set; }
            public string ObjectName { get; set; }
            public string ObjectLabel { get; set; }
            public string SqliteTableName { get; set; }
            public bool Custom { get; set; }
            public bool Createable { get; set; }
            public bool Deletable { get; set; }
            public bool Queryable { get; set; }
            public bool Replicateable { get; set; }
            public bool Retrieveable { get; set; }
            public bool Searchable { get; set; }
            public bool Undeletable { get; set; }
            public bool Updateable { get; set; }
            public int InitCount { get; set; }
            public DateTime? InitModifiedDate { get; set; }
            public DateTime? LastRetrievedAt { get; set; }
        }

        // TrackObjects テーブルのレコードを取得
        public List<TrackObject> GetTrackObjects(int connectionId)
        {
            List<TrackObject> trackObjects = new List<TrackObject>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string selectTrackObjects = @"
            SELECT ObjectId, ConnectionId, ObjectName, ObjectLabel, SqliteTableName, Custom, Createable, Deletable, Queryable, Replicateable, Retrieveable, Searchable, Undeletable, Updateable, InitCount, InitModifiedDate, LastRetrievedAt
            FROM TrackObjects WHERE ConnectionId = " + connectionId.ToString() + ";";
                using (var command = new SQLiteCommand(selectTrackObjects, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            trackObjects.Add(new TrackObject
                            {
                                ObjectId = reader.GetInt32(0),
                                ConnectionId = reader.GetInt32(1),
                                ObjectName = reader.GetString(2),
                                ObjectLabel = reader.GetString(3),
                                SqliteTableName = reader.GetString(4),
                                Custom = reader.GetBoolean(5),
                                Createable = reader.GetBoolean(6),
                                Deletable = reader.GetBoolean(7),
                                Queryable = reader.GetBoolean(8),
                                Replicateable = reader.GetBoolean(9),
                                Retrieveable = reader.GetBoolean(10),
                                Searchable = reader.GetBoolean(11),
                                Undeletable = reader.GetBoolean(12),
                                Updateable = reader.GetBoolean(13),
                                InitCount = reader.GetInt32(14),
                                InitModifiedDate = reader.IsDBNull(15) ? (DateTime?)null : reader.GetDateTime(15),
                                LastRetrievedAt = reader.IsDBNull(16) ? (DateTime?)null : reader.GetDateTime(16)
                            });
                        }
                    }
                }
            }
            return trackObjects;
        }

        // TrackObjects テーブルにレコードを挿入し、挿入したレコードの ObjectId を返す
        public int InsertTrackObject(TrackObject trackObject)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string insertTrackObject = @"
            INSERT INTO TrackObjects (ConnectionId, ObjectName, ObjectLabel, SqliteTableName, Custom, Createable, Deletable, Queryable, Replicateable, Retrieveable, Searchable, Undeletable, Updateable, InitCount, InitModifiedDate, LastRetrievedAt)
            VALUES (@ConnectionId, @ObjectName, @ObjectLabel, @SqliteTableName, @Custom, @Createable, @Deletable, @Queryable, @Replicateable, @Retrieveable, @Searchable, @Undeletable, @Updateable, @InitCount, @InitModifiedDate, @LastRetrievedAt);
            SELECT last_insert_rowid();";
                using (var command = new SQLiteCommand(insertTrackObject, conn))
                {
                    command.Parameters.AddWithValue("@ConnectionId", trackObject.ConnectionId);
                    command.Parameters.AddWithValue("@ObjectName", trackObject.ObjectName);
                    command.Parameters.AddWithValue("@ObjectLabel", trackObject.ObjectLabel);
                    command.Parameters.AddWithValue("@SqliteTableName", trackObject.SqliteTableName);
                    command.Parameters.AddWithValue("@Custom", trackObject.Custom);
                    command.Parameters.AddWithValue("@Createable", trackObject.Createable);
                    command.Parameters.AddWithValue("@Deletable", trackObject.Deletable);
                    command.Parameters.AddWithValue("@Queryable", trackObject.Queryable);
                    command.Parameters.AddWithValue("@Replicateable", trackObject.Replicateable);
                    command.Parameters.AddWithValue("@Retrieveable", trackObject.Retrieveable);
                    command.Parameters.AddWithValue("@Searchable", trackObject.Searchable);
                    command.Parameters.AddWithValue("@Undeletable", trackObject.Undeletable);
                    command.Parameters.AddWithValue("@Updateable", trackObject.Updateable);
                    command.Parameters.AddWithValue("@InitCount", trackObject.InitCount);
                    command.Parameters.AddWithValue("@InitModifiedDate", trackObject.InitModifiedDate.HasValue ? (object)trackObject.InitModifiedDate.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@LastRetrievedAt", trackObject.LastRetrievedAt.HasValue ? (object)trackObject.LastRetrievedAt.Value : DBNull.Value);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        // TrackObjects テーブルのレコードを更新
        public void UpdateTrackObject(TrackObject trackObject)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string updateTrackObject = @"
            UPDATE TrackObjects
            SET ConnectionId = @ConnectionId,
                ObjectName = @ObjectName,
                ObjectLabel = @ObjectLabel,
                SqliteTableName = @SqliteTableName,
                Custom = @Custom,
                Createable = @Createable,
                Deletable = @Deletable,
                Queryable = @Queryable,
                Replicateable = @Replicateable,
                Retrieveable = @Retrieveable,
                Searchable = @Searchable,
                Undeletable = @Undeletable,
                Updateable = @Updateable,
                InitCount = @InitCount,
                InitModifiedDate = @InitModifiedDate,
                LastRetrievedAt = @LastRetrievedAt
            WHERE ObjectId = @ObjectId;";
                using (var command = new SQLiteCommand(updateTrackObject, conn))
                {
                    command.Parameters.AddWithValue("@ConnectionId", trackObject.ConnectionId);
                    command.Parameters.AddWithValue("@ObjectName", trackObject.ObjectName);
                    command.Parameters.AddWithValue("@ObjectLabel", trackObject.ObjectLabel);
                    command.Parameters.AddWithValue("@SqliteTableName", trackObject.SqliteTableName);
                    command.Parameters.AddWithValue("@Custom", trackObject.Custom);
                    command.Parameters.AddWithValue("@Createable", trackObject.Createable);
                    command.Parameters.AddWithValue("@Deletable", trackObject.Deletable);
                    command.Parameters.AddWithValue("@Queryable", trackObject.Queryable);
                    command.Parameters.AddWithValue("@Replicateable", trackObject.Replicateable);
                    command.Parameters.AddWithValue("@Retrieveable", trackObject.Retrieveable);
                    command.Parameters.AddWithValue("@Searchable", trackObject.Searchable);
                    command.Parameters.AddWithValue("@Undeletable", trackObject.Undeletable);
                    command.Parameters.AddWithValue("@Updateable", trackObject.Updateable);
                    command.Parameters.AddWithValue("@InitCount", trackObject.InitCount);
                    command.Parameters.AddWithValue("@InitModifiedDate", trackObject.InitModifiedDate.HasValue ? (object)trackObject.InitModifiedDate.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@LastRetrievedAt", trackObject.LastRetrievedAt.HasValue ? (object)trackObject.LastRetrievedAt.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@ObjectId", trackObject.ObjectId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // TrackObjects テーブルのレコードを削除
        public void DeleteTrackObject(int objectId)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string deleteTrackObject = @"
            DELETE FROM TrackObjects
            WHERE ObjectId = @ObjectId;";
                using (var command = new SQLiteCommand(deleteTrackObject, conn))
                {
                    command.Parameters.AddWithValue("@ObjectId", objectId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public class Field
        {
            public int FieldId { get; set; }
            public int ObjectId { get; set; }
            public string FieldName { get; set; }
            public string FieldLabel { get; set; }
            public string FieldType { get; set; }
            public bool NameField { get; set; }
            public bool RestrictedPicklist { get; set; }
            public int Length { get; set; }
            public int Scale { get; set; }
            public int Precision { get; set; }
            public int Digits { get; set; }
            public bool Custom { get; set; }
            public bool Nillable { get; set; }
            public bool Createable { get; set; }
            public bool Filterable { get; set; }
            public string ReferenceTo { get; set; }
            public bool Updateable { get; set; }
            public DateTime? LastRetrievedAt { get; set; }
        }

        // Fields テーブルのレコードを取得
        public List<Field> GetFields()
        {
            List<Field> fields = new List<Field>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string selectFields = @"
            SELECT FieldId, ObjectId, FieldName, FieldLabel, FieldType, NameField, RestrictedPicklist, Length, Scale, Precision, Digits, Custom, Nillable, Createable, Filterable, Updateable, LastRetrievedAt
            FROM Fields;";
                using (var command = new SQLiteCommand(selectFields, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fields.Add(new Field
                            {
                                FieldId = reader.GetInt32(0),
                                ObjectId = reader.GetInt32(1),
                                FieldName = reader.GetString(2),
                                FieldLabel = reader.GetString(3),
                                FieldType = reader.GetString(4),
                                NameField = reader.GetBoolean(5),
                                RestrictedPicklist = reader.GetBoolean(6),
                                Length = reader.GetInt32(7),
                                Scale = reader.GetInt32(8),
                                Precision = reader.GetInt32(9),
                                Digits = reader.GetInt32(10),
                                Custom = reader.GetBoolean(11),
                                Nillable = reader.GetBoolean(12),
                                Createable = reader.GetBoolean(13),
                                Filterable = reader.GetBoolean(14),
                                Updateable = reader.GetBoolean(15),
                                LastRetrievedAt = reader.IsDBNull(16) ? (DateTime?)null : reader.GetDateTime(16)
                            });
                        }
                    }
                }
            }
            return fields;
        }

        public List<Field> GetFields(int ObjectId)
        {
            List<Field> fields = new List<Field>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string selectFields = @"
            SELECT FieldId, ObjectId, FieldName, FieldLabel, FieldType, NameField, RestrictedPicklist, Length, Scale, Precision, Digits, Custom, Nillable, Createable, Filterable, Updateable, LastRetrievedAt
            FROM Fields WHERE ObjectId = "+ ObjectId.ToString() + ";";
                using (var command = new SQLiteCommand(selectFields, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fields.Add(new Field
                            {
                                FieldId = reader.GetInt32(0),
                                ObjectId = reader.GetInt32(1),
                                FieldName = reader.GetString(2),
                                FieldLabel = reader.GetString(3),
                                FieldType = reader.GetString(4),
                                NameField = reader.GetBoolean(5),
                                RestrictedPicklist = reader.GetBoolean(6),
                                Length = reader.GetInt32(7),
                                Scale = reader.GetInt32(8),
                                Precision = reader.GetInt32(9),
                                Digits = reader.GetInt32(10),
                                Custom = reader.GetBoolean(11),
                                Nillable = reader.GetBoolean(12),
                                Createable = reader.GetBoolean(13),
                                Filterable = reader.GetBoolean(14),
                                Updateable = reader.GetBoolean(15),
                                LastRetrievedAt = reader.IsDBNull(16) ? (DateTime?)null : reader.GetDateTime(16)
                            });
                        }
                    }
                }
            }
            return fields;
        }

        // Fields テーブルにレコードを挿入し、挿入したレコードの FieldId を返す
        public void InsertField(List<Field> field)
        {
            // use transaction to insert multiple records
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    string insertField = @"
            INSERT INTO Fields (ObjectId, FieldName, FieldLabel, FieldType, NameField, RestrictedPicklist, Length, Scale, Precision, Digits, Custom, Nillable, Createable, Filterable, Updateable, ReferenceTo, LastRetrievedAt)
            VALUES (@ObjectId, @FieldName, @FieldLabel, @FieldType, @NameField, @RestrictedPicklist, @Length, @Scale, @Precision, @Digits, @Custom, @Nillable, @Createable, @Filterable, @Updateable, @ReferenceTo, @LastRetrievedAt);";
                    foreach (var f in field)
                    {
                        using (var command = new SQLiteCommand(insertField, conn))
                        {
                            command.Parameters.AddWithValue("@ObjectId", f.ObjectId);
                            command.Parameters.AddWithValue("@FieldName", f.FieldName);
                            command.Parameters.AddWithValue("@FieldLabel", f.FieldLabel);
                            command.Parameters.AddWithValue("@FieldType", f.FieldType);
                            command.Parameters.AddWithValue("@NameField", f.NameField);
                            command.Parameters.AddWithValue("@RestrictedPicklist", f.RestrictedPicklist);
                            command.Parameters.AddWithValue("@Length", f.Length);
                            command.Parameters.AddWithValue("@Scale", f.Scale);
                            command.Parameters.AddWithValue("@Precision", f.Precision);
                            command.Parameters.AddWithValue("@Digits", f.Digits);
                            command.Parameters.AddWithValue("@Custom", f.Custom);
                            command.Parameters.AddWithValue("@Nillable", f.Nillable);
                            command.Parameters.AddWithValue("@Createable", f.Createable);
                            command.Parameters.AddWithValue("@Filterable", f.Filterable);
                            command.Parameters.AddWithValue("@Updateable", f.Updateable);
                            command.Parameters.AddWithValue("@ReferenceTo", f.ReferenceTo);
                            command.Parameters.AddWithValue("@LastRetrievedAt", f.LastRetrievedAt.HasValue ? (object)f.LastRetrievedAt.Value : DBNull.Value);
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
            }
        }

        // Fields テーブルのレコードを更新
        public void UpdateField(Field field)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string updateField = @"
            UPDATE Fields
            SET ObjectId = @ObjectId,
                FieldName = @FieldName,
                FieldLabel = @FieldLabel,
                FieldType = @FieldType,
                NameField = @NameField,
                RestrictedPicklist = @RestrictedPicklist,
                Length = @Length,
                Scale = @Scale,
                Precision = @Precision,
                Digits = @Digits,
                Custom = @Custom,
                Nillable = @Nillable,
                Createable = @Createable,
                Filterable = @Filterable,
                Updateable = @Updateable,
                LastRetrievedAt = @LastRetrievedAt
            WHERE FieldId = @FieldId;";
                using (var command = new SQLiteCommand(updateField, conn))
                {
                    command.Parameters.AddWithValue("@ObjectId", field.ObjectId);
                    command.Parameters.AddWithValue("@FieldName", field.FieldName);
                    command.Parameters.AddWithValue("@FieldLabel", field.FieldLabel);
                    command.Parameters.AddWithValue("@FieldType", field.FieldType);
                    command.Parameters.AddWithValue("@NameField", field.NameField);
                    command.Parameters.AddWithValue("@RestrictedPicklist", field.RestrictedPicklist);
                    command.Parameters.AddWithValue("@Length", field.Length);
                    command.Parameters.AddWithValue("@Scale", field.Scale);
                    command.Parameters.AddWithValue("@Precision", field.Precision);
                    command.Parameters.AddWithValue("@Digits", field.Digits);
                    command.Parameters.AddWithValue("@Custom", field.Custom);
                    command.Parameters.AddWithValue("@Nillable", field.Nillable);
                    command.Parameters.AddWithValue("@Createable", field.Createable);
                    command.Parameters.AddWithValue("@Filterable", field.Filterable);
                    command.Parameters.AddWithValue("@Updateable", field.Updateable);
                    command.Parameters.AddWithValue("@LastRetrievedAt", field.LastRetrievedAt.HasValue ? (object)field.LastRetrievedAt.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@FieldId", field.FieldId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Fields テーブルのレコードを削除
        public void DeleteField(int fieldId)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string deleteField = @"
            DELETE FROM Fields
            WHERE FieldId = @FieldId;";
                using (var command = new SQLiteCommand(deleteField, conn))
                {
                    command.Parameters.AddWithValue("@FieldId", fieldId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public class RetrieveLog
        {
            public int LogId { get; set; }
            public int ObjectId { get; set; }
            public string Type { get; set; }
            public int TotalRecordCount { get; set; }
            public int UpdateCount { get; set; }
            public int DeleteCount { get; set; }
            public int InsertCount { get; set; }
            public DateTime? RetrievedAt { get; set; }
        }

        // RetrieveLog テーブルのレコードを取得
        public List<RetrieveLog> GetRetrieveLogs()
        {
            List<RetrieveLog> retrieveLogs = new List<RetrieveLog>();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string selectRetrieveLogs = @"
            SELECT LogId, ObjectId, Type, TotalRecordCount, UpdateCount, DeleteCount, InsertCount, RetrievedAt
            FROM RetrieveLog;";
                using (var command = new SQLiteCommand(selectRetrieveLogs, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retrieveLogs.Add(new RetrieveLog
                            {
                                LogId = reader.GetInt32(0),
                                ObjectId = reader.GetInt32(1),
                                Type = reader.GetString(2),
                                TotalRecordCount = reader.GetInt32(3),
                                UpdateCount = reader.GetInt32(4),
                                DeleteCount = reader.GetInt32(5),
                                InsertCount = reader.GetInt32(6),
                                RetrievedAt = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7)
                            });
                        }
                    }
                }
            }
            return retrieveLogs;
        }

        // RetrieveLog テーブルにレコードを挿入し、挿入したレコードの LogId を返す
        public int InsertRetrieveLog(RetrieveLog retrieveLog)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string insertRetrieveLog = @"
            INSERT INTO RetrieveLog (ObjectId, Type, TotalRecordCount, UpdateCount, DeleteCount, InsertCount, RetrievedAt)
            VALUES (@ObjectId, @Type, @TotalRecordCount, @UpdateCount, @DeleteCount, @InsertCount, @RetrievedAt);
            SELECT last_insert_rowid();";
                using (var command = new SQLiteCommand(insertRetrieveLog, conn))
                {
                    command.Parameters.AddWithValue("@ObjectId", retrieveLog.ObjectId);
                    command.Parameters.AddWithValue("@Type", retrieveLog.Type);
                    command.Parameters.AddWithValue("@TotalRecordCount", retrieveLog.TotalRecordCount);
                    command.Parameters.AddWithValue("@UpdateCount", retrieveLog.UpdateCount);
                    command.Parameters.AddWithValue("@DeleteCount", retrieveLog.DeleteCount);
                    command.Parameters.AddWithValue("@InsertCount", retrieveLog.InsertCount);
                    command.Parameters.AddWithValue("@RetrievedAt", retrieveLog.RetrievedAt.HasValue ? (object)retrieveLog.RetrievedAt.Value : DBNull.Value);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        // RetrieveLog テーブルのレコードを更新
        public void UpdateRetrieveLog(RetrieveLog retrieveLog)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string updateRetrieveLog = @"
            UPDATE RetrieveLog
            SET ObjectId = @ObjectId,
                Type = @Type,
                TotalRecordCount = @TotalRecordCount,
                UpdateCount = @UpdateCount,
                DeleteCount = @DeleteCount,
                InsertCount = @InsertCount,
                RetrievedAt = @RetrievedAt
            WHERE LogId = @LogId;";
                using (var command = new SQLiteCommand(updateRetrieveLog, conn))
                {
                    command.Parameters.AddWithValue("@ObjectId", retrieveLog.ObjectId);
                    command.Parameters.AddWithValue("@Type", retrieveLog.Type);
                    command.Parameters.AddWithValue("@TotalRecordCount", retrieveLog.TotalRecordCount);
                    command.Parameters.AddWithValue("@UpdateCount", retrieveLog.UpdateCount);
                    command.Parameters.AddWithValue("@DeleteCount", retrieveLog.DeleteCount);
                    command.Parameters.AddWithValue("@InsertCount", retrieveLog.InsertCount);
                    command.Parameters.AddWithValue("@RetrievedAt", retrieveLog.RetrievedAt.HasValue ? (object)retrieveLog.RetrievedAt.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@LogId", retrieveLog.LogId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // RetrieveLog テーブルのレコードを削除
        public void DeleteRetrieveLog(int logId)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string deleteRetrieveLog = @"
            DELETE FROM RetrieveLog
            WHERE LogId = @LogId;";
                using (var command = new SQLiteCommand(deleteRetrieveLog, conn))
                {
                    command.Parameters.AddWithValue("@LogId", logId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}


