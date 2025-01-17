//using LocalObjTracker.sforce;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LocalObjTracker.ManagementDatabase;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using Excel = Microsoft.Office.Interop.Excel;

namespace LocalObjTracker
{
    public partial class InitWizardForm : Form
    {
        // list to save saleforce object information including label, name, count and last modified date
        private List<(string label, string name, bool custom, int count, DateTime lastModified, object[] fields, object[] records)> allObjectList = new List<(string label, string name, bool custom, int count, DateTime lastModified, object[] fields, object[] records)>();
        private List<(string label, string name, bool custom, int count, DateTime lastModified, object[] fields, object[] records)> selectedObjectList = new List<(string label, string name, bool custom, int count, DateTime lastModified, object[] fields, object[] records)>();

        // list to filter objects by name
        private List<string> filterObjectList = new List<string> { "Account", "Contact", "Opportunity", "Lead", "Case", "Order", "Quote", "Campaign", 
            "User", "Product2", "Pricebook2" };

        // Salesforce SOAP API object
        //private sforce.SalesforceSoap salesforceSoap = new sforce.SalesforceSoap();
        //private sforce_sandbox.SalesforceSoapSand salesforceSoapSand = new sforce_sandbox.SalesforceSoapSand();
        public object salesforceSoap = null;

        // step 1 to 4
        private int step = 1;

        // ManagementDatabase
        public List<ManagementDatabase.SalesforceConnection> connections = new List<ManagementDatabase.SalesforceConnection>();
        public ManagementDatabase.SalesforceConnection dbSalesforceConnection = new ManagementDatabase.SalesforceConnection();
        private Dictionary<string, ManagementDatabase.TrackObject> dbTrackObjects = new Dictionary<string, ManagementDatabase.TrackObject>();
        private Dictionary<string, List<ManagementDatabase.Field>> dbFields = new Dictionary<string, List<ManagementDatabase.Field>>();

        // SalesforceDatabase
        public SalesforceDatabase dbSalesforceDatabase = null;

        // list for Ids including CreatedById, LastModifiedById, OwnerId
        private List<string> userOrGroupIds = new List<string>();

        private DateTime? processStartTime = null;
        private string estimateTime = "";

        // update controls based on step
        private void UpdateControls()
        {
            // hide all panels
            panelStep1.Visible = step == 1;
            panelStep2.Visible = step == 2;
            panelStep3.Visible = step == 3;
            panelStep4.Visible = step == 4;

            // update buttons
            buttonPrev.Visible = step != 1;
            buttonNext.Visible = step != 4;

            // update title
            labelStep1.Font = step == 1 ? new System.Drawing.Font(labelStep1.Font, FontStyle.Bold) : new System.Drawing.Font(labelStep1.Font, FontStyle.Regular);
            labelStep2.Font = step == 2 ? new System.Drawing.Font(labelStep2.Font, FontStyle.Bold) : new System.Drawing.Font(labelStep2.Font, FontStyle.Regular);
            labelStep3.Font = step == 3 ? new System.Drawing.Font(labelStep3.Font, FontStyle.Bold) : new System.Drawing.Font(labelStep3.Font, FontStyle.Regular);
            labelStep4.Font = step == 4 ? new System.Drawing.Font(labelStep4.Font, FontStyle.Bold) : new System.Drawing.Font(labelStep4.Font, FontStyle.Regular);

            switch (step)
            {
                case 1:
                    buttonNext.Text = "Next";
                    break;
                case 2:
                    buttonPrev.Text = "Prev";
                    buttonNext.Text = "Next";
                    break;
                case 3:
                    buttonPrev.Text = "Prev";
                    buttonNext.Text = "Finish and Init";
                    break;
                case 4:
                    buttonPrev.Text = "Cancel";
                    break;

                default:
                    break;
            }

            labelReuseCredential.Visible = bShowExitingConnectionList && step == 1;
            listBoxConnection.Visible = bShowExitingConnectionList && step == 1;
        }

        public InitWizardForm()
        {
            InitializeComponent();

            // set form height
            this.Height = 600;
            panelStep2.Location = panelStep1.Location;
            panelStep3.Location = panelStep1.Location;
            panelStep4.Location = panelStep1.Location;

            panelStep2.Size = new Size(panelStep2.Size.Width, this.Height - 100);
            listObjectFrom.Size = new Size(listObjectFrom.Size.Width, panelStep2.Size.Height - 50);
            listObjectTo.Size = new Size(listObjectTo.Size.Width, panelStep2.Size.Height - 50);

            panelStep3.Size = new Size(panelStep3.Size.Width, this.Height - 100);
            listViewObject.Size = new Size(listViewObject.Size.Width, panelStep3.Size.Height - 50);


            var config = ConfigManager.Instance.Config;
            textUsername.Text = config.Username;
            textPassword.Text = config.Password;
            textSecurityToken.Text = config.SecurityToken;
            if (config.OrgType == "Sandbox")
            {
                radioSandbox.Checked = true;
            }
            else
            {
                radioProduction.Checked = true;
            }

            // SalesforceConnections テーブルのレコードを取得
            ManagementDatabase db = ManagementDatabase.Instance;
            connections = db.GetSalesforceConnections();
            
            // listBoxConnection に接続情報を表示
            foreach (ManagementDatabase.SalesforceConnection connection in connections)
            {
                listBoxConnection.Items.Add(connection.ConnectionId.ToString() + ". " + connection.Username + " (" + connection.OrgnaizationType + ", " +
                    connection.LastRetrievedAt?.ToString("yyyy/MM/dd ddd HH:mm:ss") + ")");
            }
            listBoxConnection.Location = new System.Drawing.Point(panelStep1.Location.X, listBoxConnection.Location.Y);
            labelReuseCredential.Location = new System.Drawing.Point(panelStep1.Location.X, labelReuseCredential.Location.Y);
            listBoxConnection.Size = new Size(panelStep1.Size.Width, listBoxConnection.Size.Height);

            UpdateControls();
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (step > 1)
                step--;
            UpdateControls();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            switch (step)
            {
                case 1:
                    {
                        var config = ConfigManager.Instance.Config;
                        config.Username = textUsername.Text;
                        config.Password = textPassword.Text;
                        config.SecurityToken = textSecurityToken.Text;
                        config.OrgType = radioSandbox.Checked ? "Sandbox" : "Production";
                        ConfigManager.Instance.SaveConfig();

                        if (radioProduction.Checked)
                        {
                            salesforceSoap = new sforce.SalesforceSoap();
                            if (((sforce.SalesforceSoap)salesforceSoap).Login(textUsername.Text, textPassword.Text, textSecurityToken.Text))
                            {
                                MessageBox.Show("Login successful");
                                step++;


                                // get object list and update controls of listObjectFrom and listObjectTo
                                sforce.DescribeGlobalResult objectListResult = ((sforce.SalesforceSoap)salesforceSoap).GetObjectList();
                                sforce.GetUserInfoResult userInfo = ((sforce.SalesforceSoap)salesforceSoap).getUserInfoResult();

                                // save dbSalesforceConnection
                                dbSalesforceConnection.Username = textUsername.Text;
                                dbSalesforceConnection.Password = textPassword.Text;
                                dbSalesforceConnection.SecurityToken = textSecurityToken.Text;
                                dbSalesforceConnection.ServerUrl = ((sforce.SalesforceSoap)salesforceSoap).GetServerUrl();
                                Uri uri = new Uri(dbSalesforceConnection.ServerUrl);
                                dbSalesforceConnection.MyDomain = uri.Host;
                                dbSalesforceConnection.OrgnaizationType = "Production";
                                dbSalesforceConnection.UserEmail = userInfo.userEmail;
                                dbSalesforceConnection.UserFullName = userInfo.userFullName;
                                dbSalesforceConnection.UserID = userInfo.userId;
                                dbSalesforceConnection.OrganizationId = userInfo.organizationId;
                                dbSalesforceConnection.OrganizationName = userInfo.organizationName;
                                dbSalesforceConnection.ProfileId = userInfo.profileId;
                                dbSalesforceConnection.RoleId = userInfo.roleId;
                                dbSalesforceConnection.UserType = userInfo.userType;
                                dbSalesforceConnection.UserTimeZone = userInfo.userTimeZone;
                                dbSalesforceConnection.LastRetrievedAt = DateTime.Now;

                                //dbSalesforceConnection.ConnectionId = ManagementDatabase.Instance.InsertSalesforceConnection(dbSalesforceConnection);

                                listObjectFrom.Items.Clear();
                                listObjectTo.Items.Clear();

                                allObjectList.Clear();
                                selectedObjectList.Clear();

                                dbTrackObjects.Clear();
                                dbFields.Clear();

                                foreach (var obj in objectListResult.sobjects)
                                {
                                    // add to allObjectList
                                    allObjectList.Add((obj.label, obj.name, obj.custom, 0, DateTime.MinValue, null, null));

                                    // add to listObjectFrom if name is in filterObjectList or the object is custom object
                                    if (checkShowAllObject.Checked || filterObjectList.Contains(obj.name) || obj.name.EndsWith("__c"))
                                        listObjectFrom.Items.Add(obj.name);

                                    // add all to dbTrackObjects
                                    dbTrackObjects.Add(obj.name, new ManagementDatabase.TrackObject
                                    {
                                        //ConnectionId = dbSalesforceConnection.ConnectionId,
                                        ObjectName = obj.name,
                                        ObjectLabel = obj.label,
                                        SqliteTableName = ManagementDatabase.IsSqliteKeyword(obj.name) ? "t_" + obj.name : obj.name,
                                        Custom = obj.custom,
                                        Createable = obj.createable,
                                        Deletable = obj.deletable,
                                        Queryable = obj.queryable,
                                        Replicateable = obj.replicateable,
                                        Retrieveable = obj.retrieveable,
                                        Searchable = obj.searchable,
                                        Undeletable = obj.undeletable,
                                        Updateable = obj.updateable,
                                        InitCount = 0,
                                        InitModifiedDate = DateTime.MinValue,
                                        LastRetrievedAt = DateTime.Now
                                    });
                                }
                            }
                            else
                            {
                                MessageBox.Show("Login failed");
                            }
                        }
                        else if (radioSandbox.Checked)
                        {
                            salesforceSoap = new sforce_sandbox.SalesforceSoapSand();
                            if (((sforce_sandbox.SalesforceSoapSand)salesforceSoap).Login(textUsername.Text, textPassword.Text, textSecurityToken.Text))
                            {
                                MessageBox.Show("Login successful");
                                step++;

                                // get object list and update controls of listObjectFrom and listObjectTo
                                sforce_sandbox.DescribeGlobalResult objectListResult = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).GetObjectList();
                                sforce_sandbox.GetUserInfoResult userInfo = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).getUserInfoResult();

                                // save dbSalesforceConnection
                                dbSalesforceConnection.Username = textUsername.Text;
                                dbSalesforceConnection.Password = textPassword.Text;
                                dbSalesforceConnection.SecurityToken = textSecurityToken.Text;
                                dbSalesforceConnection.ServerUrl = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).GetServerUrl();
                                Uri uri = new Uri(dbSalesforceConnection.ServerUrl);
                                dbSalesforceConnection.MyDomain = uri.Host;
                                dbSalesforceConnection.OrgnaizationType = "Sandbox";
                                dbSalesforceConnection.UserEmail = userInfo.userEmail;
                                dbSalesforceConnection.UserFullName = userInfo.userFullName;
                                dbSalesforceConnection.UserID = userInfo.userId;
                                dbSalesforceConnection.OrganizationId = userInfo.organizationId;
                                dbSalesforceConnection.OrganizationName = userInfo.organizationName;
                                dbSalesforceConnection.ProfileId = userInfo.profileId;
                                dbSalesforceConnection.RoleId = userInfo.roleId;
                                dbSalesforceConnection.UserType = userInfo.userType;
                                dbSalesforceConnection.UserTimeZone = userInfo.userTimeZone;
                                dbSalesforceConnection.LastRetrievedAt = DateTime.Now;

                                //dbSalesforceConnection.ConnectionId = ManagementDatabase.Instance.InsertSalesforceConnection(dbSalesforceConnection);


                                listObjectFrom.Items.Clear();
                                listObjectTo.Items.Clear();

                                allObjectList.Clear();
                                selectedObjectList.Clear();

                                dbTrackObjects.Clear();
                                dbFields.Clear();

                                foreach (var obj in objectListResult.sobjects)
                                {
                                    // add to allObjectList
                                    allObjectList.Add((obj.label, obj.name, obj.custom, 0, DateTime.MinValue, null, null));

                                    // add to listObjectFrom if name is in filterObjectList or the object is custom object
                                    if (checkShowAllObject.Checked || filterObjectList.Contains(obj.name) || obj.custom)
                                        listObjectFrom.Items.Add(obj.name);

                                    // add all to dbTrackObjects
                                    dbTrackObjects.Add(obj.name, new ManagementDatabase.TrackObject
                                    {
                                        //ConnectionId = dbSalesforceConnection.ConnectionId,
                                        ObjectName = obj.name,
                                        ObjectLabel = obj.label,
                                        SqliteTableName = ManagementDatabase.IsSqliteKeyword(obj.name) ? "t_" + obj.name : obj.name,
                                        Custom = obj.custom,
                                        Createable = obj.createable,
                                        Deletable = obj.deletable,
                                        Queryable = obj.queryable,
                                        Replicateable = obj.replicateable,
                                        Retrieveable = obj.retrieveable,
                                        Searchable = obj.searchable,
                                        Undeletable = obj.undeletable,
                                        Updateable = obj.updateable,
                                        InitCount = 0,
                                        InitModifiedDate = DateTime.MinValue,
                                        LastRetrievedAt = DateTime.Now
                                    });
                                }
                            }
                            else
                            {
                                MessageBox.Show("Login failed");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please select environment");
                            return;
                        }
                    }
                    break;
                case 2:
                    {
                        if (listObjectTo.Items.Count == 0)
                        {
                            MessageBox.Show("Please select at least one object to track");
                            return;
                        }

                        // get count and last modified date of selected objects
                        foreach (var obj in selectedObjectList.ToList())
                        {
                            var countAndLastModified = radioProduction.Checked ? 
                                ((sforce.SalesforceSoap)salesforceSoap).GetObjectCountAndLastModified(obj.name) :
                                ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).GetObjectCountAndLastModified(obj.name);
                            var updatedObj = obj;
                            updatedObj.count = countAndLastModified.count;
                            updatedObj.lastModified = countAndLastModified.lastModified;

                            // Update the object in the list
                            int index = selectedObjectList.IndexOf(obj);
                            selectedObjectList[index] = updatedObj;

                            // Update the object in the dbTrackObjects
                            dbTrackObjects[obj.name].InitCount = countAndLastModified.count;
                            dbTrackObjects[obj.name].InitModifiedDate = countAndLastModified.lastModified;
                        }

                        // show the result to listViewObject
                        listViewObject.Items.Clear();
                        int no = 1;
                        foreach (var obj in selectedObjectList)
                        {
                            ListViewItem item = new ListViewItem(no++.ToString());
                            item.SubItems.Add(obj.label);

                            if (ManagementDatabase.IsSqliteKeyword(obj.name))
                            {
                                item.SubItems.Add(obj.name + " -> t_" + obj.name);
                            }
                            else
                            {
                                item.SubItems.Add(obj.name);
                            }

                            // add comma into count number
                            item.SubItems.Add(obj.count.ToString("N0"));

                            // add weekday into last modified date
                            item.SubItems.Add(obj.lastModified.ToLocalTime().ToString("yyyy/MM/dd ddd HH:mm:ss"));
                            listViewObject.Items.Add(item);
                        }

                        step++;
                    }
                    break;
                case 3:
                    step++;
                    
                    // start time of the process
                    processStartTime = DateTime.Now;
                    timer1.Start();

                    // create a background thread to get fields of selected objects and all records of selected objects
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += (s, ev) =>
                    {
                        SaveManagementDatabase();

                        int totalCount = 0;
                        int finishedCount = 0;
                        foreach (var obj in selectedObjectList)
                        {
                            totalCount += obj.count;
                        }
                        DateTime startTime = DateTime.Now;

                        // get fields of selected objects
                        for (int i = 0; i<selectedObjectList.Count; i++)
                        {
                            var obj = selectedObjectList[i];

                            // show current object name and progress in labelRetrieveData like "xxx (20/30 Objects, 2000 Records)"
                            this.Invoke((MethodInvoker)delegate
                            {
                                labelRetrieveData.Text = $"{obj.name} ({i + 1}/{selectedObjectList.Count} Objects, {obj.count.ToString("N0")} Records)";

                                // show progress bar
                                progressBarRetrieveData.Maximum = selectedObjectList.Count;
                                progressBarRetrieveData.Value = i + 1;
                            });

                            if (obj.count > 10000)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    labelRetrieveData.Text = $"{obj.name} ({i + 1}/{selectedObjectList.Count} Objects, {obj.count.ToString("N0")} Records - Bulk)";
                                });

                                int count = 0;
                                if (radioProduction.Checked)
                                {
                                    var (finished, csv, locator, jobId) = ((sforce.SalesforceSoap)salesforceSoap).GetBulkQueryResult(obj.name);
                                    count += dbSalesforceDatabase.InsertRecordsFromBulkCsvResult(obj.name, csv);

                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        labelRetrieveData.Text = $"{obj.name} ({i + 1}/{selectedObjectList.Count} Objects, {count.ToString("N0")}/{obj.count.ToString("N0")} Records - Bulk)";

                                        if (finishedCount + count > 0)
                                        {
                                            double estimateSeconds = (DateTime.Now - startTime).TotalSeconds / (finishedCount + count) * totalCount + 10;
                                            estimateTime = $"Estimate: {DateTime.Now.AddSeconds(estimateSeconds).ToString("yyyy/MM/dd ddd HH:mm:ss")}";
                                        }
                                    });
                                    while (!finished)
                                    {
                                        var (finished2, csv2, locator2) = ((sforce.SalesforceSoap)salesforceSoap).BulkQueryMore(locator, jobId);
                                        finished = finished2;
                                        locator = locator2;
                                        count += dbSalesforceDatabase.InsertRecordsFromBulkCsvResult(obj.name, csv2);
                                        this.Invoke((MethodInvoker)delegate
                                        {
                                            labelRetrieveData.Text = $"{obj.name} ({i + 1}/{selectedObjectList.Count} Objects, {count.ToString("N0")}/{obj.count.ToString("N0")} Records - Bulk)";
                                            if (finishedCount + count > 0)
                                            {
                                                double estimateSeconds = (DateTime.Now - startTime).TotalSeconds / (finishedCount + count) * totalCount + 10;
                                                estimateTime = $"Estimate: {DateTime.Now.AddSeconds(estimateSeconds).ToString("yyyy/MM/dd ddd HH:mm:ss")}";
                                            }
                                        });
                                    }
                                }
                                else
                                {
                                    var (finished, csv, locator, jobId) = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).GetBulkQueryResult(obj.name);
                                    count += dbSalesforceDatabase.InsertRecordsFromBulkCsvResult(obj.name, csv);
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        labelRetrieveData.Text = $"{obj.name} ({i + 1}/{selectedObjectList.Count} Objects, {count.ToString("N0")}/{obj.count.ToString("N0")} Records - Bulk)";
                                        if (finishedCount + count > 0)
                                        {
                                            double estimateSeconds = (DateTime.Now - startTime).TotalSeconds / (finishedCount + count) * totalCount + 10;
                                            estimateTime = $"Estimate: {DateTime.Now.AddSeconds(estimateSeconds).ToString("yyyy/MM/dd ddd HH:mm:ss")}";
                                        }
                                    });
                                    while (!finished)
                                    {
                                        var (finished2, csv2, locator2) = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).BulkQueryMore(locator, jobId);
                                        finished = finished2;
                                        locator = locator2;
                                        count += dbSalesforceDatabase.InsertRecordsFromBulkCsvResult(obj.name, csv2);
                                        this.Invoke((MethodInvoker)delegate
                                        {
                                            labelRetrieveData.Text = $"{obj.name} ({i + 1}/{selectedObjectList.Count} Objects, {count.ToString("N0")}/{obj.count.ToString("N0")} Records - Bulk)";
                                            if (finishedCount + count > 0)
                                            {
                                                double estimateSeconds = (DateTime.Now - startTime).TotalSeconds / (finishedCount + count) * totalCount + 10;
                                                estimateTime = $"Estimate: {DateTime.Now.AddSeconds(estimateSeconds).ToString("yyyy/MM/dd ddd HH:mm:ss")}";
                                            }
                                        });
                                    }
                                }
                            }
                            else
                            {
                                if (radioProduction.Checked)
                                {
                                    //obj.fields = ((sforce.SalesforceSoap)salesforceSoap).GetObjectFields(obj.name).fields;
                                    //obj.records = ((sforce.SalesforceSoap)salesforceSoap).GetAllRecords(obj.name).ToArray();

                                    sforce.QueryResult queryResult = ((sforce.SalesforceSoap)salesforceSoap).GetQueryResult(obj.name);
                                    dbSalesforceDatabase.InsertRecordsFromQueryResult(obj.name, dbFields[obj.name], queryResult);

                                    int count = queryResult.records == null ? 0 : queryResult.records.Count();
                                    while (queryResult.done == false)
                                    {
                                        this.Invoke((MethodInvoker)delegate
                                        {
                                            labelRetrieveData.Text = $"{obj.name} ({i + 1}/{selectedObjectList.Count} Objects, {count.ToString("N0")}/{obj.count.ToString("N0")} Records)";
                                            if (finishedCount + count > 0)
                                            {
                                                double estimateSeconds = (DateTime.Now - startTime).TotalSeconds / (finishedCount + count) * totalCount + 10;
                                                estimateTime = $"Estimate: {DateTime.Now.AddSeconds(estimateSeconds).ToString("yyyy/MM/dd ddd HH:mm:ss")}";
                                            }
                                        });

                                        queryResult = ((sforce.SalesforceSoap)salesforceSoap).QueryMore(queryResult.queryLocator);
                                        dbSalesforceDatabase.InsertRecordsFromQueryResult(obj.name, dbFields[obj.name], queryResult);

                                        count += queryResult.records.Count();
                                    }
                                }
                                else
                                {
                                    //obj.fields = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).GetObjectFields(obj.name).fields;
                                    //obj.records = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).GetAllRecords(obj.name).ToArray();

                                    sforce_sandbox.QueryResult queryResult = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).GetQueryResult(obj.name);
                                    dbSalesforceDatabase.InsertRecordsFromQueryResult(obj.name, dbFields[obj.name], queryResult);

                                    int count = queryResult.records == null ? 0 : queryResult.records.Count();
                                    while (queryResult.done == false)
                                    {
                                        this.Invoke((MethodInvoker)delegate
                                        {
                                            labelRetrieveData.Text = $"{obj.name} ({i + 1}/{selectedObjectList.Count} Objects, {count.ToString("N0")}/{obj.count.ToString("N0")} Records)";
                                            if (finishedCount + count > 0)
                                            {
                                                double estimateSeconds = (DateTime.Now - startTime).TotalSeconds / (finishedCount + count) * totalCount + 10;
                                                estimateTime = $"Estimate: {DateTime.Now.AddSeconds(estimateSeconds).ToString("yyyy/MM/dd ddd HH:mm:ss")}";
                                            }
                                        });

                                        queryResult = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).QueryMore(queryResult.queryLocator);
                                        dbSalesforceDatabase.InsertRecordsFromQueryResult(obj.name, dbFields[obj.name], queryResult);

                                        count += queryResult.records.Count();
                                    }
                                }
                            }
                            userOrGroupIds = dbSalesforceDatabase.GetLastModifiedByIdListFromObject(obj.name, userOrGroupIds);


                            // estimate finish time by count, totalCount and elapsed time
                            finishedCount += obj.count;

                            this.Invoke((MethodInvoker)delegate
                            {
                                if (finishedCount > 0)
                                {
                                    double estimateSeconds = (DateTime.Now - startTime).TotalSeconds / finishedCount * totalCount + 10;
                                    estimateTime = $"Estimate: {DateTime.Now.AddSeconds(estimateSeconds).ToString("yyyy/MM/dd ddd HH:mm:ss")}";
                                }
                            });

                        }

                        // query user and group information
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


                        // show current object name and progress in labelRetrieveData like "xxx (20/30 Objects, 2000 Records)"
                        this.Invoke((MethodInvoker)delegate
                        {
                            labelRetrieveData.Text = $"Done ({selectedObjectList.Count} Objects)";

                            if (processStartTime != null)
                            {
                                timer1.Stop();
                                estimateTime = "";
                                labelEstimateTime.Text = $"Finished! {(DateTime.Now - processStartTime.Value).ToString(@"hh\:mm\:ss")}";
                                processStartTime = null;
                            }
                        });

                        this.Invoke((MethodInvoker)delegate
                        {
                            // sleep 2 seconds
                            System.Threading.Thread.Sleep(2000);

                            // OK
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        });
                    };
                    worker.RunWorkerAsync();
                    break;
                case 4:
                    break;
                default:
                    break;
            }

            UpdateControls();
        }

        private void SaveManagementDatabase()
        {
            dbSalesforceConnection.ConnectionId = ManagementDatabase.Instance.InsertSalesforceConnection(dbSalesforceConnection);

            dbFields.Clear();
            dbSalesforceDatabase = new SalesforceDatabase(dbSalesforceConnection.ConnectionId);

            // save dbTrackObjects
            for (int i = 0; i < selectedObjectList.Count; i++)
            {
                // show current object name and progress in labelParseField like "xxx (20/30 Objects)"
                this.Invoke((MethodInvoker)delegate
                {
                    labelParseField.Text = $"{selectedObjectList[i].label} ({i + 1}/{selectedObjectList.Count} Objects)";
                    labelRetrieveData.Text = "";

                    // show progress bar
                    progressBarParseField.Maximum = selectedObjectList.Count;
                    progressBarParseField.Value = i + 1;
                });

                var obj = selectedObjectList[i];
                dbTrackObjects[obj.name].ConnectionId = dbSalesforceConnection.ConnectionId;
                dbTrackObjects[obj.name].ObjectId = ManagementDatabase.Instance.InsertTrackObject(dbTrackObjects[obj.name]);

                // save dbFields
                dbFields.Add(obj.name, new List<ManagementDatabase.Field>());

                // retrieve fields of the object
                if (radioProduction.Checked)
                {
                    obj.fields = ((sforce.SalesforceSoap)salesforceSoap).GetObjectFields(obj.name).fields;
                    foreach (sforce.Field field in obj.fields)
                    {
                        dbFields[obj.name].Add(new ManagementDatabase.Field
                        {
                            ObjectId = dbTrackObjects[obj.name].ObjectId,
                            FieldName = field.name,
                            FieldLabel = field.label,
                            FieldType = field.type.ToString(),
                            NameField = field.nameField,
                            RestrictedPicklist = field.restrictedPicklist,
                            Length = field.length,
                            Scale = field.scale,
                            Precision = field.precision,
                            Digits = field.digits,
                            Custom = field.custom,
                            Nillable = field.nillable,
                            Createable = field.createable,
                            Filterable = field.filterable,
                            Updateable = field.updateable,
                            ReferenceTo = field.referenceTo == null ? "" : string.Join(";", field.referenceTo),
                            LastRetrievedAt = DateTime.Now
                        });
                    }
                }
                else
                {
                    obj.fields = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).GetObjectFields(obj.name).fields;
                    foreach (sforce_sandbox.Field field in obj.fields)
                    {
                        dbFields[obj.name].Add(new ManagementDatabase.Field
                        {
                            ObjectId = dbTrackObjects[obj.name].ObjectId,
                            FieldName = field.name,
                            FieldLabel = field.label,
                            FieldType = field.type.ToString(),
                            NameField = field.nameField,
                            RestrictedPicklist = field.restrictedPicklist,
                            Length = field.length,
                            Scale = field.scale,
                            Precision = field.precision,
                            Digits = field.digits,
                            Custom = field.custom,
                            Nillable = field.nillable,
                            Createable = field.createable,
                            Filterable = field.filterable,
                            Updateable = field.updateable,
                            ReferenceTo = field.referenceTo == null ? "" : string.Join(";", field.referenceTo),
                            LastRetrievedAt = DateTime.Now
                        });
                    }
                }


                // save dbFields
                ManagementDatabase.Instance.InsertField(dbFields[obj.name]);


            }

            // show current object name and progress in labelParseField like "xxx (20/30 Objects)"
            this.Invoke((MethodInvoker)delegate
            {
                labelParseField.Text = $"Done ({selectedObjectList.Count} Objects)";
            });

            dbSalesforceDatabase.CreateTables(dbFields);


        }

        private void buttonAddObject_Click(object sender, EventArgs e)
        {
            // Move selected items from listObjectFrom to listObjectTo
            var selectedItems = listObjectFrom.SelectedItems.Cast<string>().ToList();
            foreach (var selectedItem in selectedItems)
            {
                listObjectFrom.Items.Remove(selectedItem);
                listObjectTo.Items.Add(selectedItem);
                // Update selectedObjectList
                var obj = allObjectList.Find(o => o.name == selectedItem);
                selectedObjectList.Add(obj);
            }
        }

        private void buttonRemoveObject_Click(object sender, EventArgs e)
        {
            // Move selected items from listObjectTo to listObjectFrom
            var selectedItems = listObjectTo.SelectedItems.Cast<string>().ToList();
            foreach (var selectedItem in selectedItems)
            {
                listObjectTo.Items.Remove(selectedItem);
                listObjectFrom.Items.Add(selectedItem);
                // Update selectedObjectList
                var obj = selectedObjectList.Find(o => o.name == selectedItem);
                selectedObjectList.Remove(obj);
            }
        }

        private void checkShowAllObject_CheckedChanged(object sender, EventArgs e)
        {
            // show all objects or only filtered objects and custom objects, but remove objects already in listObjectTo
            listObjectFrom.Items.Clear();
            foreach (var obj in allObjectList)
            {
                if (checkShowAllObject.Checked || filterObjectList.Contains(obj.name) || obj.custom)
                {
                    if (!listObjectTo.Items.Contains(obj.name))
                        listObjectFrom.Items.Add(obj.name);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ExportListViewToExcel(listViewObject);
        }


        private void ExportListViewToExcel(ListView listView)
        {
            // Create a new Excel application instance
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = true;

            // Create a new workbook
            Excel.Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];
            worksheet.Name = "ExportedData";

            // Add column headers
            for (int i = 0; i < listView.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = listView.Columns[i].Text;
            }

            // Add rows
            for (int i = 0; i < listView.Items.Count; i++)
            {
                for (int j = 0; j < listView.Items[i].SubItems.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = listView.Items[i].SubItems[j].Text;
                }
            }

            // Autofit columns
            worksheet.Columns.AutoFit();

            // Release resources
            workbook = null;
            worksheet = null;
            excelApp = null;
        }

        private void listObjectFrom_DoubleClick(object sender, EventArgs e)
        {
            buttonAddObject_Click(sender, e);
        }

        private void listObjectTo_DoubleClick(object sender, EventArgs e)
        {
            buttonRemoveObject_Click(sender, e);
        }

        private bool bShowExitingConnectionList = false;
        private void pictureBoxReuseLogin_Click(object sender, EventArgs e)
        {
            bShowExitingConnectionList = !bShowExitingConnectionList;
            labelReuseCredential.Visible = bShowExitingConnectionList && step == 1;
            listBoxConnection.Visible = bShowExitingConnectionList && step == 1;

        }

        private void listBoxConnection_DoubleClick(object sender, EventArgs e)
        {
            // get selected connection and set to textUsername, textPassword, textSecurityToken, radioProduction, radioSandbox
            if (listBoxConnection.SelectedIndex >= 0)
            {
                var connection = connections[listBoxConnection.SelectedIndex];
                textUsername.Text = connection.Username;
                textPassword.Text = connection.Password;
                textSecurityToken.Text = connection.SecurityToken;
                radioProduction.Checked = connection.OrgnaizationType == "Production";
                radioSandbox.Checked = connection.OrgnaizationType == "Sandbox";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // current date and time
            if (processStartTime != null)
            {
                labelEstimateTime.Text = $"Processing...  Elapsed: {(DateTime.Now - processStartTime.Value).ToString(@"hh\:mm\:ss")} " + estimateTime;
            }
            
        }
    }
}
