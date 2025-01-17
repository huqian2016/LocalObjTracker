using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LocalObjTracker.MainForm;
using static LocalObjTracker.ManagementDatabase;
using Excel = Microsoft.Office.Interop.Excel;

namespace LocalObjTracker
{
    public partial class DataDockingForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        private SalesforceConnection dbSalesforceConnection = null;
        private SalesforceDatabase dbSalesforceDatabase = null;
        private List<TrackObject> trackObjects = new List<TrackObject>();
        private List<TrackHistory> trackHistories = new List<TrackHistory>();
        private Dictionary<string, List<ManagementDatabase.Field>> dbFields = new Dictionary<string, List<ManagementDatabase.Field>>();

        private IMainForm mainForm = null;
        public DataDockingForm(IMainForm mainForm, SalesforceConnection dbSalesforceConnection, SalesforceDatabase dbSalesforceDatabase, List<TrackObject> trackObjects)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.dbSalesforceConnection = dbSalesforceConnection;
            this.dbSalesforceDatabase = dbSalesforceDatabase;
            this.trackObjects = trackObjects;

            // get all fields of all objects in trackObjects
            foreach (TrackObject trackObject in trackObjects)
            {
                dbFields[trackObject.ObjectName] = ManagementDatabase.Instance.GetFields(trackObject.ObjectId);
            }

            CloseButton = false;

            // Hide the close button so the user isn't even tempted
            CloseButtonVisible = false;
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

        private void pictureBoxExportObject_Click(object sender, EventArgs e)
        {
            ExportListViewToExcel(listViewObject);

        }

        private void pictureBoxExportTrackHistory_Click(object sender, EventArgs e)
        {
            ExportListViewToExcel(listViewTrackHistory);

        }

        private void listViewObject_DoubleClick(object sender, EventArgs e)
        {
            // Get the selected item's Name
            string objName = listViewObject.SelectedItems[0].SubItems[1].Text;
            mainForm.OpenObjectLink(objName);
        }

        private void listViewTrackHistory_DoubleClick(object sender, EventArgs e)
        {
            // Get the selected item's ID
            string objId = listViewTrackHistory.SelectedItems[0].SubItems[2].Text;
            mainForm.OpenRecordLink(objId);


        }

        private void pictureBoxRestoreApex_Click(object sender, EventArgs e)
        {
            // get all selected item indexes in listViewTrackHistory and find crospending items in trackHistories
            List<TrackHistory> selectedTrackHistories = new List<TrackHistory>();
            foreach (int index in listViewTrackHistory.SelectedIndices)
            {
                selectedTrackHistories.Add(trackHistories[index]);
            }

            if (selectedTrackHistories.Count == 0)
            {
                MessageBox.Show("Please select at least one record to restore.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // group selectedTrackHistories by ObjectName and Operation. for deplicate id, keep the latest one by LastModifiedDate
            Dictionary<string, Dictionary<string, List<TrackHistory>>> dict = new Dictionary<string, Dictionary<string, List<TrackHistory>>>();
            foreach (TrackHistory trackHistory in selectedTrackHistories)
            {
                if (!dict.ContainsKey(trackHistory.ObjectName))
                {
                    dict[trackHistory.ObjectName] = new Dictionary<string, List<TrackHistory>>();
                }
                if (!dict[trackHistory.ObjectName].ContainsKey(trackHistory.Operation))
                {
                    dict[trackHistory.ObjectName][trackHistory.Operation] = new List<TrackHistory>();
                }
                dict[trackHistory.ObjectName][trackHistory.Operation].Add(trackHistory);
            }

            // for each ObjectName with INSERT Operation, build restore apex code with "delete [SELECT Id FROM ... WHERE Id IN ('...')];"
            string apexCodeDelete = "";
            foreach (string objectName in dict.Keys)
            {
                if (dict[objectName].ContainsKey("INSERT"))
                {
                    List<string> ids = new List<string>();
                    foreach (TrackHistory trackHistory in dict[objectName]["INSERT"])
                    {
                        ids.Add(trackHistory.Id);
                    }
                    apexCodeDelete += "delete [SELECT Id FROM " + objectName + " WHERE Id IN ('" + string.Join("', '", ids) + "')];\r\n";
                }
            }
            if (apexCodeDelete != "")
            {
                apexCodeDelete = "// remove inserted data\r\n" + apexCodeDelete + "\r\n";
            }

            // for each ObjectName with DELETE Operation, build restore apex code with "undelete [SELECT ... FROM ... WHERE Id IN ('...')];"
            string apexCodeUndelete = "";
            foreach (string objectName in dict.Keys)
            {
                if (dict[objectName].ContainsKey("DELETE"))
                {
                    List<string> ids = new List<string>();
                    foreach (TrackHistory trackHistory in dict[objectName]["DELETE"])
                    {
                        ids.Add(trackHistory.Id);
                    }
                    apexCodeUndelete += "undelete [SELECT * FROM " + objectName + " WHERE Id IN ('" + string.Join("', '", ids) + "')];\r\n";
                }
            }
            if (apexCodeUndelete != "")
            {
                apexCodeUndelete = "// restore deleted data\r\n" + apexCodeUndelete + "\r\n";
            }

            // for each ObjectName with UPDATE_AFTER Operation, find the corresponding UPDATE_BEFORE Operation with the same Id and HistoryId is 1 less than the record, then compare the two records and build restore apex code
            string apexCodeUpdate = "";
            foreach (string objectName in dict.Keys)
            {
                if (dict[objectName].ContainsKey("UPDATE_AFTER"))
                {
                    foreach (TrackHistory trackHistory in dict[objectName]["UPDATE_AFTER"])
                    {
                        string id = trackHistory.Id;
                        int historyId = trackHistory.HistoryId;

                        if (dbFields.ContainsKey(objectName))
                        {
                            apexCodeUpdate += dbSalesforceDatabase.GetRestoreApexUpdate(objectName, dbFields[objectName], id, historyId-1, historyId);
                        }

                    }
                }
            }
            if (apexCodeUpdate != "")
            {
                apexCodeUpdate = "// restore updated data \r\n" + apexCodeUpdate + "\r\n";
            }

            //{
            //    Account accountToUpdate = [SELECT BillingCity FROM Account WHERE ID = '001GA000050fAYUYA2'LIMIT 1];
            //    if (accountToUpdate != null)
            //    {
            //        accountToUpdate.BillingCity = 'New York1';
            //        update accountToUpdate;
            //    }
            //}

            // show RestoreApexForm
            string apexCode = apexCodeDelete + apexCodeUndelete + apexCodeUpdate;
            string developerConsoleUrl = "https://" + dbSalesforceConnection.MyDomain + "/_ui/common/apex/debug/ApexCSIPage";
            RestoreApexForm restoreApexForm = new RestoreApexForm(apexCode, developerConsoleUrl);
            restoreApexForm.ShowDialog();
        }


        public void UpdateListViewObject()
        {
            // add listViewObject columns
            listViewObject.Columns.Clear();
            listViewObject.Columns.Add("Label", 100);
            listViewObject.Columns.Add("Name", 100);
            listViewObject.Columns.Add("LocalName", 100);
            listViewObject.Columns.Add("Count", 100);
            listViewObject.Columns.Add("Last Modified Date", 150);
            listViewObject.Columns.Add("Last Modified By", 200);

            // add listViewObject items
            listViewObject.Items.Clear();
            foreach (TrackObject trackObject in trackObjects)
            {
                ListViewItem item = new ListViewItem();
                item.Text = trackObject.ObjectLabel;
                item.SubItems.Add(trackObject.ObjectName);
                item.SubItems.Add(trackObject.SqliteTableName);

                // get count from sqlite table of dbSalesforceDatabase
                int count = dbSalesforceDatabase.GetCount(trackObject.SqliteTableName);
                item.SubItems.Add(count.ToString("N0"));

                // call dbSalesforceDatabase.GetLastModifiedDateAndIdAndName()
                (DateTime lastModifiedDate, string lastModifiedById, string lastModifiedByName) = dbSalesforceDatabase.GetLastModifiedDateAndIdAndName(trackObject.SqliteTableName);
                item.SubItems.Add(lastModifiedDate.ToLocalTime().ToString("yyyy/MM/dd ddd HH:mm:ss"));
                item.SubItems.Add(lastModifiedByName + " (" + lastModifiedById + ")");

                listViewObject.Items.Add(item);
            }
        }


        public void UpdateListViewTrackHistory()
        {
            // add listViewTrackHistory columns
            listViewTrackHistory.Columns.Clear();
            listViewTrackHistory.Columns.Add("ObjectName", 100);
            listViewTrackHistory.Columns.Add("Operation", 150);
            listViewTrackHistory.Columns.Add("Id", 140);
            listViewTrackHistory.Columns.Add("Name", 150);
            listViewTrackHistory.Columns.Add("Last Modified Date", 150);
            listViewTrackHistory.Columns.Add("Last Modified By", 200);
            listViewTrackHistory.Columns.Add("Description", 400);

            // get history data from each hisotry table according to trackObjects, save them in a list
            trackHistories = new List<TrackHistory>();
            foreach (TrackObject trackObject in trackObjects)
            {
                // call dbSalesforceDatabase.GetTrackHistories
                List<Dictionary<string, object>> records = dbSalesforceDatabase.GetTrackHistories(trackObject.SqliteTableName);

                // convert records to TrackHistory objects
                foreach (var record in records)
                {
                    TrackHistory trackHistory = new TrackHistory();
                    trackHistory.ObjectName = trackObject.ObjectName;
                    trackHistory.HistoryId = record.ContainsKey("HistoryId") && record["HistoryId"] is IConvertible ? Convert.ToInt32(record["HistoryId"]) : 0;
                    trackHistory.Operation = record.ContainsKey("Operation") ? record["Operation"].ToString() : "";
                    trackHistory.Id = record.ContainsKey("Id") ? record["Id"].ToString() : "";
                    trackHistory.RecordName = record.ContainsKey("Name") ? record["Name"].ToString() : "";
                    trackHistory.LastModifiedDate = record.ContainsKey("LastModifiedDate") ? (DateTime)record["LastModifiedDate"] : DateTime.MinValue;
                    trackHistory.LastModifiedById = record.ContainsKey("LastModifiedById") ? record["LastModifiedById"].ToString() : "";
                    trackHistory.LastModifiedByName = dbSalesforceDatabase.GetUserOrGroupNameById(trackHistory.LastModifiedById);
                    trackHistories.Add(trackHistory);
                }
            }

            // sort trackHistories by LastModifiedDate descending
            trackHistories.Sort((a, b) => b.LastModifiedDate.CompareTo(a.LastModifiedDate));

            // rearrange trackHistories, for records with Operation of "UPDATE_AFTER", "INSERT", "DELETE", sort them by LastModifiedDate descending
            // but for records with Operation of "UPDATE_AFTER", move the following "UPDATE_BEFORE" record to the next record postion
            // who has the same Id to the same record and HistoryId is 1 less than the record 
            List<TrackHistory> trackHistories2 = new List<TrackHistory>();
            for (int i = 0; i < trackHistories.Count; i++)
            {
                if (trackHistories[i].Operation == "UPDATE_AFTER")
                {
                    trackHistories2.Add(trackHistories[i]);
                    for (int j = i + 1; j < trackHistories.Count; j++)
                    {
                        if (trackHistories[j].Operation == "UPDATE_BEFORE" && trackHistories[j].Id == trackHistories[i].Id && trackHistories[j].HistoryId == trackHistories[i].HistoryId - 1)
                        {
                            trackHistories[j].ObjectName = "";

                            trackHistories2.Add(trackHistories[j]);

                            trackHistories[i].Description = dbSalesforceDatabase.DescribeDifference(trackHistories[i].ObjectName, trackHistories[j].HistoryId, trackHistories[i].HistoryId);

                            break;
                        }
                    }
                }
                else if (trackHistories[i].Operation == "INSERT" || trackHistories[i].Operation == "DELETE")
                {
                    trackHistories2.Add(trackHistories[i]);
                }
            }
            trackHistories = trackHistories2;

            // add listViewTrackHistory items
            listViewTrackHistory.Items.Clear();
            foreach (TrackHistory trackHistory in trackHistories)
            {
                ListViewItem item = new ListViewItem();
                item.Text = trackHistory.ObjectName;
                item.SubItems.Add(trackHistory.Operation);
                item.SubItems.Add(trackHistory.Id);
                item.SubItems.Add(trackHistory.RecordName);
                item.SubItems.Add(trackHistory.LastModifiedDate.ToLocalTime().ToString("yyyy/MM/dd ddd HH:mm:ss"));
                item.SubItems.Add(trackHistory.LastModifiedByName + " (" + trackHistory.LastModifiedById + ")");
                item.SubItems.Add(trackHistory.Description);
                listViewTrackHistory.Items.Add(item);
            }
        }

    }
}
