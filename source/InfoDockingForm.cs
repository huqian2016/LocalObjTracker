using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocalObjTracker
{
    public partial class InfoDockingForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        ManagementDatabase.SalesforceConnection dbSalesforceConnection;
        private IMainForm mainForm = null;
        public InfoDockingForm(IMainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;

            CloseButton = false;

            // Hide the close button so the user isn't even tempted
            CloseButtonVisible = false;
        }

        public void UpdateInfo(ManagementDatabase.SalesforceConnection dbSalesforceConnection)
        {
            this.dbSalesforceConnection = dbSalesforceConnection;

            linkLabelOrg.Text = dbSalesforceConnection.MyDomain;
            labelOrgType.Text = dbSalesforceConnection.OrgnaizationType;
            linkLabelUsername.Text = dbSalesforceConnection.Username;
            labelEmail.Text = dbSalesforceConnection.UserEmail;
            labelUserTimeZone.Text = dbSalesforceConnection.UserTimeZone;
            labelIntervalTime.Text = dbSalesforceConnection.IntervalTime.ToString("N0");
            labelCreatedDate.Text = dbSalesforceConnection.LastRetrievedAt?.ToString("yyyy/MM/dd ddd HH:mm:ss");

            var dbFilename = "SalesforceData" + dbSalesforceConnection.ConnectionId + ".db";

            // if file does not exist, show "Not found"
            if (!System.IO.File.Exists(dbFilename))
            {
                linkLabelSQLiteFile.Text = "Not found";
                return;
            }
            else
            {
                // get file size
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(dbFilename);
                // if the size is less than 1MB, show KB with 2 decimal places, otherwise show MB with 2 decimal places
                if (fileInfo.Length < 1024 * 1024)
                {
                    linkLabelSQLiteFile.Text = dbFilename + " (" + (fileInfo.Length / 1024.0).ToString("0.0") + " KB)";
                }
                else
                {
                    linkLabelSQLiteFile.Text = dbFilename + " (" + (fileInfo.Length / 1024.0 / 1024.0).ToString("0.0") + " MB)";
                }
            }

        }

        private void linkLabelOrg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://" + dbSalesforceConnection.MyDomain + "/");
        }

        private void linkLabelUsername_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://" + dbSalesforceConnection.MyDomain + "/lightning/settings/personal/PersonalInformation/home");

        }

        private void linkLabelSetupHome_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://" + dbSalesforceConnection.MyDomain + "/lightning/setup/SetupOneHome/home");

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://" + dbSalesforceConnection.MyDomain + "/lightning/setup/ObjectManager/home");

        }

        private void linkLabelSQLiteFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // get current folder
            string currentFolder = System.IO.Directory.GetCurrentDirectory();

            // add \SQLiteStudio\SQLiteStudio.exe
            string sqliteStudioPath = currentFolder + "\\SQLiteStudio\\SQLiteStudio.exe";

            // get SalesforceData[x].db file path, x is ConnectionId
            string dbFilePath = currentFolder + "\\SalesforceData" + dbSalesforceConnection.ConnectionId + ".db";

            // open SQLiteStudio
            System.Diagnostics.Process.Start(sqliteStudioPath, dbFilePath);
        }
    }
}
