//using LocalObjTracker.sforce;
//using LocalObjTracker.sforce_sandbox;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LocalObjTracker.ManagementDatabase;


namespace LocalObjTracker
{
    internal class TrackWorker
    {
        private SalesforceConnection dbSalesforceConnection;
        private SalesforceDatabase dbSalesforceDatabase;
        private object salesforceSoap;
        private List<TrackObject> trackObjects;

        public TrackWorker(SalesforceConnection dbSalesforceConnection, SalesforceDatabase dbSalesforceDatabase, object salesforceSoap, List<TrackObject> trackObjects)
        {
            this.dbSalesforceConnection = dbSalesforceConnection;
            this.dbSalesforceDatabase = dbSalesforceDatabase;
            this.salesforceSoap = salesforceSoap;
            this.trackObjects = trackObjects;
        }

        /// <summary>
        /// Tracks changes in Salesforce objects and updates the local database accordingly.
        /// </summary>
        public int Track()
        {
            int totalChanges = 0;
            foreach (var trackObject in trackObjects)
            {
                // Get the latest LastModifiedDate from the local database
                DateTime lastModifiedDate = dbSalesforceDatabase.GetLastModifiedDate(trackObject.SqliteTableName);

                // Get the fields for the current object
                var fields = ManagementDatabase.Instance.GetFields(trackObject.ObjectId);

                // Retrieve data from Salesforce that has been modified since the last retrieved date
                var queryResult = GetSalesforceData(trackObject.ObjectName, lastModifiedDate);

                // id list for LastModifiedById field
                List<string> lastModifiedByIdList = new List<string>();

                if (queryResult == null)
                    continue;

                if (queryResult is sforce.QueryResult)
                {
                    if (((sforce.QueryResult)queryResult).records == null)
                        continue;

                    totalChanges += ((sforce.QueryResult)queryResult).records.Length;

                    foreach (sforce.sObject record in ((sforce.QueryResult)queryResult).records)
                    {
                        var recordData = new Dictionary<string, object>();
                        foreach (var field in fields)
                        {
                            var fieldValue = record.Any.FirstOrDefault(x => x.LocalName == field.FieldName)?.InnerText;
                            recordData[field.FieldName] = ConvertFieldValue(field.FieldType, fieldValue);
                        }

                        string recordId = recordData["Id"].ToString();
                        bool isDeleted = recordData.ContainsKey("IsDeleted") && (bool)recordData["IsDeleted"];

                        // Add LastModifiedById to the list
                        if (recordData.ContainsKey("LastModifiedById"))
                        {
                            if (!lastModifiedByIdList.Contains(recordData["LastModifiedById"].ToString()))
                                lastModifiedByIdList.Add(recordData["LastModifiedById"].ToString());
                        }

                        if (!dbSalesforceDatabase.RecordExists(trackObject.SqliteTableName, recordId))
                        {
                            // Insert new record and log the operation in the history table
                            dbSalesforceDatabase.InsertRecord(trackObject.SqliteTableName, recordData);
                            dbSalesforceDatabase.InsertHistoryRecord(trackObject.SqliteTableName, recordData, "INSERT");
                        }
                        else if (isDeleted)
                        {
                            // Delete the record and log the operation in the history table
                            dbSalesforceDatabase.DeleteRecord(trackObject.SqliteTableName, recordId);
                            dbSalesforceDatabase.InsertHistoryRecord(trackObject.SqliteTableName, recordData, "DELETE");
                        }
                        else
                        {
                            // Update the existing record and log the operation in the history table
                            Dictionary<string, object> existingRecord = dbSalesforceDatabase.GetRecordById(trackObject.SqliteTableName, recordId);
                            // remove SqliteId from existing record
                            existingRecord.Remove("SqliteId");
                            dbSalesforceDatabase.InsertHistoryRecord(trackObject.SqliteTableName, existingRecord, "UPDATE_BEFORE");
                            dbSalesforceDatabase.InsertHistoryRecord(trackObject.SqliteTableName, recordData, "UPDATE_AFTER");
                            dbSalesforceDatabase.UpdateRecordById(trackObject.SqliteTableName, recordData, recordId);
                        }
                    }
                }


                if (queryResult is sforce_sandbox.QueryResult)
                {
                    if (((sforce_sandbox.QueryResult)queryResult).records == null)
                        continue;

                    totalChanges += ((sforce_sandbox.QueryResult)queryResult).records.Length;

                    // Implement tracking for sandbox
                    foreach (sforce_sandbox.sObject record in ((sforce_sandbox.QueryResult)queryResult).records)
                    {
                        var recordData = new Dictionary<string, object>();
                        foreach (var field in fields)
                        {
                            var fieldValue = record.Any.FirstOrDefault(x => x.LocalName == field.FieldName)?.InnerText;
                            recordData[field.FieldName] = ConvertFieldValue(field.FieldType, fieldValue);
                        }
                        string recordId = recordData["Id"].ToString();
                        bool isDeleted = recordData.ContainsKey("IsDeleted") && (bool)recordData["IsDeleted"];

                        // Add LastModifiedById to the list
                        if (recordData.ContainsKey("LastModifiedById"))
                        {
                            if (!lastModifiedByIdList.Contains(recordData["LastModifiedById"].ToString()))
                                lastModifiedByIdList.Add(recordData["LastModifiedById"].ToString());
                        }

                        if (!dbSalesforceDatabase.RecordExists(trackObject.SqliteTableName, recordId))
                        {
                            // Insert new record and log the operation in the history table
                            dbSalesforceDatabase.InsertRecord(trackObject.SqliteTableName, recordData);
                            dbSalesforceDatabase.InsertHistoryRecord(trackObject.SqliteTableName, recordData, "INSERT");
                        }
                        else if (isDeleted)
                        {
                            // Delete the record and log the operation in the history table
                            dbSalesforceDatabase.DeleteRecord(trackObject.SqliteTableName, recordId);
                            dbSalesforceDatabase.InsertHistoryRecord(trackObject.SqliteTableName, recordData, "DELETE");
                        }
                        else
                        {
                            // Update the existing record and log the operation in the history table
                            Dictionary<string, object> existingRecord = dbSalesforceDatabase.GetRecordById(trackObject.SqliteTableName, recordId);
                            // remove SqliteId from existing record
                            existingRecord.Remove("SqliteId");
                            dbSalesforceDatabase.InsertHistoryRecord(trackObject.SqliteTableName, existingRecord, "UPDATE_BEFORE");
                            dbSalesforceDatabase.InsertHistoryRecord(trackObject.SqliteTableName, recordData, "UPDATE_AFTER");
                            dbSalesforceDatabase.UpdateRecordById(trackObject.SqliteTableName, recordData, recordId);
                        }
                    }
                }

                AddLastModifiedByIdList(lastModifiedByIdList);

            }

            return totalChanges;
        }

        private void AddLastModifiedByIdList(List<string> lastModifiedByIdList)
        {
            var userOrGroupIds = dbSalesforceDatabase.GetNotExistIds(lastModifiedByIdList);
            if (userOrGroupIds.Count > 0)
            {
                if (salesforceSoap is sforce.SalesforceSoap)
                {
                    string objectName = "User";

                    // build where clause for Ids
                    string whereClause = string.Join("','", userOrGroupIds);
                    whereClause = "WHERE Id IN ('" + whereClause + "')";

                    sforce.QueryResult queryResult = ((sforce.SalesforceSoap)salesforceSoap).GetQueryResult(objectName, whereClause);
                    dbSalesforceDatabase.InsertUserOrGroupName(objectName, queryResult);

                    while (queryResult.done == false)
                    {
                        queryResult = ((sforce.SalesforceSoap)salesforceSoap).QueryMore(queryResult.queryLocator);
                        dbSalesforceDatabase.InsertUserOrGroupName(objectName, queryResult);
                    }

                    objectName = "Group";
                    queryResult = ((sforce.SalesforceSoap)salesforceSoap).GetQueryResult(objectName, whereClause);
                    dbSalesforceDatabase.InsertUserOrGroupName(objectName, queryResult);

                    while (queryResult.done == false)
                    {
                        queryResult = ((sforce.SalesforceSoap)salesforceSoap).QueryMore(queryResult.queryLocator);
                        dbSalesforceDatabase.InsertUserOrGroupName(objectName, queryResult);
                    }
                }
                else
                {
                    string objectName = "User";
                    // build where clause for Ids
                    string whereClause = string.Join("','", userOrGroupIds);
                    whereClause = "WHERE Id IN ('" + whereClause + "')";
                    sforce_sandbox.QueryResult queryResult = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).GetQueryResult(objectName, whereClause);
                    dbSalesforceDatabase.InsertUserOrGroupName(objectName, queryResult);
                    while (queryResult.done == false)
                    {
                        queryResult = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).QueryMore(queryResult.queryLocator);
                        dbSalesforceDatabase.InsertUserOrGroupName(objectName, queryResult);
                    }
                    objectName = "Group";
                    queryResult = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).GetQueryResult(objectName, whereClause);
                    dbSalesforceDatabase.InsertUserOrGroupName(objectName, queryResult);
                    while (queryResult.done == false)
                    {
                        queryResult = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).QueryMore(queryResult.queryLocator);
                        dbSalesforceDatabase.InsertUserOrGroupName(objectName, queryResult);
                    }
                }
            }

        }

        /// <summary>
        /// Retrieves data from Salesforce that has been modified since the specified date.
        /// </summary>
        /// <param name="objectName">The name of the Salesforce object.</param>
        /// <param name="lastModifiedDate">The date to retrieve records modified since.</param>
        /// <returns>A QueryResult containing the retrieved records.</returns>
        private object GetSalesforceData(string objectName, DateTime? lastModifiedDate)
        {
            string whereCondition = $" WHERE LastModifiedDate > {lastModifiedDate?.ToString("yyyy-MM-ddTHH:mm:ssZ")}";
            if (salesforceSoap is sforce.SalesforceSoap)
            {
                return ((sforce.SalesforceSoap)salesforceSoap).GetQueryResult(objectName, whereCondition);
            }
            else
            {
                return ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).GetQueryResult(objectName, whereCondition);
            }
        }



        /// <summary>
        /// Converts the field value to the appropriate type based on the field type.
        /// </summary>
        /// <param name="fieldType">The type of the field.</param>
        /// <param name="fieldValue">The value of the field.</param>
        /// <returns>The converted field value.</returns>
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

        public long GetAPILimit()
        {
            if (salesforceSoap is sforce.SalesforceSoap)
            {
                sforce.LimitInfoHeader limitInfo = ((sforce.SalesforceSoap)salesforceSoap).GetLimitInfoHeader();
                if (limitInfo == null || limitInfo.limitInfo == null || limitInfo.limitInfo.Length == 0)
                {
                    return 0;
                }
                return limitInfo.limitInfo[0].limit;
            }
            else
            {
                sforce_sandbox.LimitInfoHeader limitInfo = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).GetLimitInfoHeader();
                if (limitInfo == null || limitInfo.limitInfo == null || limitInfo.limitInfo.Length == 0)
                {
                    return 0;
                }
                return limitInfo.limitInfo[0].limit;
            }
        }
        public long GetAPICurrent()
        {
            if (salesforceSoap is sforce.SalesforceSoap)
            {
                sforce.LimitInfoHeader limitInfo = ((sforce.SalesforceSoap)salesforceSoap).GetLimitInfoHeader();
                if (limitInfo == null || limitInfo.limitInfo == null || limitInfo.limitInfo.Length == 0)
                {
                    return 0;
                }
                return limitInfo.limitInfo[0].current;
            }
            else
            {
                sforce_sandbox.LimitInfoHeader limitInfo = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).GetLimitInfoHeader();
                if (limitInfo == null || limitInfo.limitInfo == null || limitInfo.limitInfo.Length == 0)
                {
                    return 0;
                }
                return limitInfo.limitInfo[0].current;
            }
        }
    }
}
