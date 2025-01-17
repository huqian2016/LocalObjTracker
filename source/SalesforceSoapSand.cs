using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LocalObjTracker.sforce_sandbox
{
    internal class SalesforceSoapSand
    {
        private SforceService binding;
        private LoginResult loginResult;

        public SalesforceSoapSand()
        {
            binding = new SforceService();
        }

        /// <summary>
        /// Logs in to Salesforce using the provided credentials.
        /// </summary>
        /// <param name="username">The Salesforce username.</param>
        /// <param name="password">The Salesforce password.</param>
        /// <param name="securityToken">The Salesforce security token.</param>
        /// <returns>True if login is successful, otherwise false.</returns>
        public bool Login(string username, string password, string securityToken)
        {
            try
            {
                loginResult = binding.login(username, password + securityToken);
                binding.Url = loginResult.serverUrl;
                binding.SessionHeaderValue = new SessionHeader { sessionId = loginResult.sessionId };
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login failed: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Get the server URL of the Salesforce instance.
        /// </summary>
        /// <returns>The server URL.</returns>
        public string GetServerUrl()
        {
            return loginResult.serverUrl;
        }

        /// <summary>
        /// Get the user info of the logged-in user.
        /// </summary>
        /// <returns>The user info.</returns>
        public GetUserInfoResult getUserInfoResult()
        {
            return loginResult.userInfo;
        }

        /// <summary>
        /// Retrieves the list of all objects available in Salesforce.
        /// </summary>
        /// <returns>A DescribeGlobalResult object containing the list of objects.</returns>
        public DescribeGlobalResult GetObjectList()
        {
            try
            {
                GetUserInfoResult userInfo = loginResult.userInfo;
                return binding.describeGlobal();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get object list: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrieves the count and last modified date of the specified object.
        /// </summary>
        /// <param name="objectName">The name of the Salesforce object.</param>
        /// <returns>A tuple containing the count and last modified date.</returns>
        public (int count, DateTime lastModified) GetObjectCountAndLastModified(string objectName)
        {
            try
            {
                var queryResult = binding.queryAll($"SELECT COUNT(Id), MAX(LastModifiedDate) FROM {objectName}");
                int count = int.Parse(queryResult.records[0].Any[0].InnerText);

                if (count == 0)
                {
                    return (0, DateTime.MinValue);
                }

                DateTime lastModified = DateTime.Parse(queryResult.records[0].Any[1].InnerText).ToUniversalTime();
                return (count, lastModified);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get object count and last modified date: " + ex.Message);
                return (0, DateTime.MinValue);
            }
        }

        /// <summary>
        /// Retrieves the fields of the specified object.
        /// </summary>
        /// <param name="objectName">The name of the Salesforce object.</param>
        /// <returns>A DescribeSObjectResult object containing the fields of the object.</returns>
        public DescribeSObjectResult GetObjectFields(string objectName)
        {
            try
            {
                return binding.describeSObject(objectName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get object fields: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrieves all records of the specified object.
        /// </summary>
        /// <param name="objectName">The name of the Salesforce object.</param>
        /// <returns>An array of SObject containing all records of the object.</returns>
        public sObject[] GetAllRecords(string objectName)
        {
            try
            {
                // Retrieve the fields of the object to construct the query
                var describeResult = GetObjectFields(objectName);
                if (describeResult == null)
                {
                    throw new Exception("Failed to describe object fields.");
                }

                // Construct the query with specific field names
                var fieldNames = describeResult.fields.Select(f => f.name).ToArray();
                var query = $"SELECT {string.Join(", ", fieldNames)} FROM {objectName}";

                binding.QueryOptionsValue = new sforce_sandbox.QueryOptions();
                binding.QueryOptionsValue.batchSize = 2000;
                binding.QueryOptionsValue.batchSizeSpecified = true;

                var queryResult = binding.queryAll(query);
                return queryResult.records;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get all records: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retrieves all records of the specified object.
        /// </summary>
        /// <param name="objectName">The name of the Salesforce object.</param>
        /// <returns>An array of SObject containing all records of the object.</returns>
        public QueryResult GetQueryResult(string objectName)
        {
            try
            {
                // Retrieve the fields of the object to construct the query
                var describeResult = GetObjectFields(objectName);
                if (describeResult == null)
                {
                    throw new Exception("Failed to describe object fields.");
                }

                // Construct the query with specific field names
                var fieldNames = describeResult.fields.Select(f => f.name).ToArray();
                var query = $"SELECT {string.Join(", ", fieldNames)} FROM {objectName}";

                binding.QueryOptionsValue = new sforce_sandbox.QueryOptions();
                binding.QueryOptionsValue.batchSize = 2000;
                binding.QueryOptionsValue.batchSizeSpecified = true;

                var queryResult = binding.queryAll(query);
                return queryResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get all records: " + ex.Message);
                return null;
            }
        }

        public QueryResult GetQueryResult(string objectName, string whereCondition)
        {
            try
            {
                // Retrieve the fields of the object to construct the query
                var describeResult = GetObjectFields(objectName);
                if (describeResult == null)
                {
                    throw new Exception("Failed to describe object fields.");
                }

                // Construct the query with specific field names
                var fieldNames = describeResult.fields.Select(f => f.name).ToArray();
                var query = $"SELECT {string.Join(", ", fieldNames)} FROM {objectName} {whereCondition}";

                binding.QueryOptionsValue = new sforce_sandbox.QueryOptions();
                binding.QueryOptionsValue.batchSize = 2000;
                binding.QueryOptionsValue.batchSizeSpecified = true;

                var queryResult = binding.queryAll(query);
                return queryResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get all records: " + ex.Message);
                return null;
            }
        }

        public QueryResult QueryMore(string queryLocator)
        {
            try
            {
                return binding.queryMore(queryLocator);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to query more: " + ex.Message);
                return null;
            }
        }

        public LimitInfoHeader GetLimitInfoHeader()
        {
            return binding.LimitInfoHeaderValue;
        }

        private class SalesforceJob
        {
            public string Id { get; set; }
            public string Operation { get; set; }
            public string Object { get; set; }
            public string CreatedById { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime SystemModstamp { get; set; }
            public string State { get; set; }
            public string ConcurrencyMode { get; set; }
            public string ContentType { get; set; }
            public double ApiVersion { get; set; }
            public string LineEnding { get; set; }
            public string ColumnDelimiter { get; set; }
        }
        private SalesforceJob CreateBulkJob(string query)
        {
            try
            {
                var domain = new Uri(binding.Url).Host;
                var url = $"https://{domain}/services/data/v62.0/jobs/query";
                string contentType = "application/json";
                string body = $"{{\"operation\":\"query\",\"contentType\" : \"CSV\",\"lineEnding\" : \"CRLF\",\"query\":\"{query}\"}}";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + loginResult.sessionId);
                    var content = new StringContent(body, Encoding.UTF8, contentType);
                    var response = client.PostAsync(url, content).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Failed to create bulk job: " + response.ReasonPhrase);
                    }

                    // Parse the json response to SalesforceJob
                    // response sample:
                    // {"id":"750GA00000fWadJYAS","operation":"query","object":"Account","createdById":"005GA00000AS1keYAD","createdDate":"2025-01-04T15:31:48.000+0000","systemModstamp":"2025-01-04T15:31:48.000+0000","state":"UploadComplete","concurrencyMode":"Parallel","contentType":"CSV","apiVersion":50.0,"lineEnding":"CRLF","columnDelimiter":"COMMA"}
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var job = new SalesforceJob();
                    var json = JObject.Parse(responseContent);
                    job.Id = json["id"].ToString();
                    job.Operation = json["operation"].ToString();
                    job.Object = json["object"].ToString();
                    job.CreatedById = json["createdById"].ToString();
                    job.CreatedDate = DateTime.Parse(json["createdDate"].ToString()).ToUniversalTime();
                    job.SystemModstamp = DateTime.Parse(json["systemModstamp"].ToString()).ToUniversalTime();
                    job.State = json["state"].ToString();
                    job.ConcurrencyMode = json["concurrencyMode"].ToString();
                    job.ContentType = json["contentType"].ToString();
                    job.ApiVersion = double.Parse(json["apiVersion"].ToString());
                    job.LineEnding = json["lineEnding"].ToString();
                    job.ColumnDelimiter = json["columnDelimiter"].ToString();

                    return job;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to create bulk job: " + ex.Message);
                return null;
            }
        }

        private class SalesforceJobStatus
        {
            public string Id { get; set; }
            public string Operation { get; set; }
            public string Object { get; set; }
            public string CreatedById { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime SystemModstamp { get; set; }
            //The current state of processing for the job.Possible values are:
            //  UploadComplete — All job data has been uploaded and the job is ready to be processed. Salesforce has put the job in the queue.
            //  InProgress — Salesforce is processing the job.
            //  Aborted — The job has been aborted.See Abort a Query Job.
            //  JobComplete — Salesforce has finished processing the job.
            //  Failed — The job failed.
            public string State { get; set; } 
            public string ConcurrencyMode { get; set; }
            public string ContentType { get; set; }
            public double ApiVersion { get; set; }
            public string JobType { get; set; }
            public string LineEnding { get; set; }
            public string ColumnDelimiter { get; set; }
            public int NumberRecordsProcessed { get; set; }
            public int Retries { get; set; }
            public int TotalProcessingTime { get; set; }
        }

        private SalesforceJobStatus GetSalesforceJobStatus(SalesforceJob job)
        {
            if (job == null)
            {
                return null;
            }


            try
            {
                var domain = new Uri(binding.Url).Host;
                var url = $"https://{domain}/services/data/v62.0/jobs/query/{job.Id}";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + loginResult.sessionId);
                    // use HTTP methods GET to get status
                    var response = client.GetAsync(url).Result;



                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Failed to get bulk job info: " + response.ReasonPhrase);
                    }

                    // Parse the json response to SalesforceJob
                    // response sample:
                    // {
                    //   "id" : "750R0000000zlh9IAA",
                    //   "operation" : "query",
                    //   "object" : "Account",
                    //   "createdById" : "005R0000000GiwjIAC",
                    //   "createdDate" : "2018-12-10T17:50:19.000+0000",
                    //   "systemModstamp" : "2018-12-10T17:51:27.000+0000",
                    //   "state" : "JobComplete",
                    //   "concurrencyMode" : "Parallel",
                    //   "contentType" : "CSV",
                    //   "apiVersion" : 46.0,
                    //   "jobType" : "V2Query",
                    //   "lineEnding" : "LF",
                    //   "columnDelimiter" : "COMMA",
                    //   "numberRecordsProcessed" : 500,
                    //   "retries" : 0,
                    //   "totalProcessingTime" : 334
                    //}

                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var jobStatus = new SalesforceJobStatus();
                    var json = JObject.Parse(responseContent);

                    jobStatus.Id = json["id"].ToString();
                    jobStatus.Operation = json["operation"].ToString();
                    jobStatus.Object = json["object"].ToString();
                    jobStatus.CreatedById = json["createdById"].ToString();
                    jobStatus.CreatedDate = DateTime.Parse(json["createdDate"].ToString()).ToUniversalTime();
                    jobStatus.SystemModstamp = DateTime.Parse(json["systemModstamp"].ToString()).ToUniversalTime();
                    jobStatus.State = json["state"].ToString();
                    jobStatus.ConcurrencyMode = json["concurrencyMode"].ToString();
                    jobStatus.ContentType = json["contentType"].ToString();
                    jobStatus.ApiVersion = double.Parse(json["apiVersion"].ToString());
                    jobStatus.JobType = json["jobType"].ToString();
                    jobStatus.LineEnding = json["lineEnding"].ToString();
                    jobStatus.ColumnDelimiter = json["columnDelimiter"].ToString();
                    //jobStatus.NumberRecordsProcessed = int.Parse(json["numberRecordsProcessed"].ToString());
                    jobStatus.Retries = int.Parse(json["retries"].ToString());
                    jobStatus.TotalProcessingTime = int.Parse(json["totalProcessingTime"].ToString());

                    return jobStatus;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to create bulk job: " + ex.Message);
                return null;
            }

        }


        public (bool, string, string, string) GetBulkQueryResult(string objectName, string whereCondition = "")
        {
            string locator = "";
            try
            {
                // Retrieve the fields of the object to construct the query
                var describeResult = GetObjectFields(objectName);
                if (describeResult == null)
                {
                    throw new Exception("Failed to describe object fields.");
                }

                // Construct the query with specific field names except fieldType.address and fieldType.location
                var fieldNames = describeResult.fields.Where(f => f.type != fieldType.address && f.type != fieldType.location).Select(f => f.name).ToArray();
                var query = $"SELECT {string.Join(", ", fieldNames)} FROM {objectName} {whereCondition}";

                SalesforceJob job = CreateBulkJob(query);
                SalesforceJobStatus jobStatus = null;
                while (true)
                {
                    jobStatus = GetSalesforceJobStatus(job);
                    if (jobStatus == null)
                    {
                        return (true, "", locator, "");
                    }
                    if (jobStatus.State == "JobComplete")
                    {
                        break;
                    }
                    else if (jobStatus.State == "Failed")
                    {
                        return (true, "Failed to get bulk query result.", locator, job.Id);
                    }
                    else if (jobStatus.State == "Aborted")
                    {
                        return (true, "The job has been aborted.", locator, job.Id);
                    }
                    System.Threading.Thread.Sleep(1000);
                }

                if (jobStatus.State == "JobComplete")
                {
                    var domain = new Uri(binding.Url).Host;
                    var url = $"https://{domain}/services/data/v62.0/jobs/query/{job.Id}/results?maxRecords=50000";
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + loginResult.sessionId);
                        var response = client.GetAsync(url).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception("Failed to get bulk query result: " + response.ReasonPhrase);
                        }

                        // get Sforce-Locator: from response header
                        locator = response.Headers.GetValues("Sforce-Locator").FirstOrDefault();

                        return (locator == "null", response.Content.ReadAsStringAsync().Result, locator, job.Id);
                    }
                }

                return (true, "", locator, job.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get all records: " + ex.Message);
                return (true, "", "", "");
            }
        }

        public (bool, string, string) BulkQueryMore(string queryLocator, string jobId)
        {
            try
            {
                var domain = new Uri(binding.Url).Host;
                var url = $"https://{domain}/services/data/v62.0/jobs/query/{jobId}/results?locator={queryLocator}&maxRecords=50000";
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + loginResult.sessionId);
                    var response = client.GetAsync(url).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Failed to get bulk query result: " + response.ReasonPhrase);
                    }

                    // get Sforce-Locator: from response header
                    var locator = response.Headers.GetValues("Sforce-Locator").FirstOrDefault();

                    return (locator == "null", response.Content.ReadAsStringAsync().Result, locator);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to query more: " + ex.Message);
                return (true, "", "");
            }
        }
    }
}
