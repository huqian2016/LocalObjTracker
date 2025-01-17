using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using static LocalObjTracker.MainForm;
using static LocalObjTracker.ManagementDatabase;
using Excel = Microsoft.Office.Interop.Excel;


namespace LocalObjTracker
{

    public interface IMainForm
    {
        void OpenObjectLink(string objName);
        void OpenRecordLink(string objId);
    }

    public partial class MainForm : Form, IMainForm
    {
        // Salesforce SOAP API object
        //private sforce.SalesforceSoap salesforceSoap = new sforce.SalesforceSoap();
        //private sforce_sandbox.SalesforceSoapSand salesforceSoapSand = new sforce_sandbox.SalesforceSoapSand();
        private object salesforceSoap = null;

        // ManagementDatabase
        ManagementDatabase.SalesforceConnection dbSalesforceConnection = new ManagementDatabase.SalesforceConnection();
        List<TrackObject> trackObjects = new List<TrackObject>();
        //Dictionary<string, List<ManagementDatabase.Field>> dbFields = new Dictionary<string, List<ManagementDatabase.Field>>();

        // SalesforceDatabase
        SalesforceDatabase dbSalesforceDatabase = null;

        private bool loopTrack = false;
        private bool isTracking = false;

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private InfoDockingForm infoDockingForm;
        private DataDockingForm dataDockingForm;

        public MainForm()
        {
            InitializeComponent();

            var config = ConfigManager.Instance.Config;
            this.Width = config.MainFormWidth;
            this.Height = config.MainFormHeight;
            this.Location = new Point(config.MainFormX, config.MainFormY);

            // SalesforceConnections テーブルのレコードを取得
            ManagementDatabase db = ManagementDatabase.Instance;
            List<ManagementDatabase.SalesforceConnection> connections = db.GetSalesforceConnections();

            bool createNew = false;
            if (connections.Count > 0)
            {
                // Show login list form
                LoginListForm loginListForm = new LoginListForm();
                if (loginListForm.ShowDialog() == DialogResult.OK)
                {
                    if (loginListForm.selectedConnection != null)
                    {
                        dbSalesforceConnection = loginListForm.selectedConnection;
                        if (dbSalesforceConnection.OrgnaizationType == "Production")
                        {
                            salesforceSoap = new sforce.SalesforceSoap();
                            if (!((sforce.SalesforceSoap)salesforceSoap).Login(dbSalesforceConnection.Username, dbSalesforceConnection.Password, dbSalesforceConnection.SecurityToken))
                            {
                                // show error dialog
                                MessageBox.Show("Login failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                createNew = true;
                            }
                        }
                        else // Sandbox
                        {
                            salesforceSoap = new sforce_sandbox.SalesforceSoapSand();
                            if (!((sforce_sandbox.SalesforceSoapSand)salesforceSoap).Login(dbSalesforceConnection.Username, dbSalesforceConnection.Password, dbSalesforceConnection.SecurityToken))
                            {
                                // show error dialog
                                MessageBox.Show("Login failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                createNew = true;
                            }
                        }

                        if (!createNew)
                            dbSalesforceDatabase = new SalesforceDatabase(dbSalesforceConnection.ConnectionId);
                    }
                    else
                    {
                        createNew = true;
                    }
                }
                else
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += (s, ev) =>
                    {
                        // wait 1 second
                        System.Threading.Thread.Sleep(1000);

                        this.Invoke((MethodInvoker)delegate
                        {
                            this.Close();
                        });
                    };
                    worker.RunWorkerAsync();
                }
            }
            else
            {
                createNew = true;
            }

            if (createNew)
            {
                // Show initial wizard form
                InitWizardForm initWizardForm = new InitWizardForm();
                if (initWizardForm.ShowDialog() == DialogResult.OK)
                {
                    salesforceSoap = initWizardForm.salesforceSoap;
                    dbSalesforceConnection = initWizardForm.dbSalesforceConnection;
                    dbSalesforceDatabase = initWizardForm.dbSalesforceDatabase;
                }
                else
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += (s, ev) =>
                    {
                        // wait 1 second
                        System.Threading.Thread.Sleep(1000);

                        this.Invoke((MethodInvoker)delegate
                        {
                            this.Close();
                        });
                    };
                    worker.RunWorkerAsync();
                }
            }

            trackObjects = db.GetTrackObjects(dbSalesforceConnection.ConnectionId);


            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            //this.dockPanel.DocumentStyle = DocumentStyle.DockingSdi;
            //this.dockPanel.DockTopPortion = 0.3;
            this.dockPanel.DockLeftPortion = 0.3;
            var theme = new VS2015BlueTheme();
            this.dockPanel.Theme = theme;
            this.dockPanel.Dock = DockStyle.Fill;
            this.Controls.Add(this.dockPanel);

            infoDockingForm = new InfoDockingForm(this);
            infoDockingForm.Show(this.dockPanel, DockState.DockLeft);
            infoDockingForm.UpdateInfo(dbSalesforceConnection);

            dataDockingForm = new DataDockingForm(this, dbSalesforceConnection, dbSalesforceDatabase, trackObjects);
            dataDockingForm.Show(this.dockPanel, DockState.Document);

            // Adjust the dockPanel to be below the toolstrip
            this.Controls.SetChildIndex(this.dockPanel, 1);


            UpdatePropertyGridInfo();
            dataDockingForm.UpdateListViewObject();
            dataDockingForm.UpdateListViewTrackHistory();

            toolStripButtonStart.Enabled = loopTrack ? false : true;
            toolStripButtonStop.Enabled = loopTrack ? true : false;

            if (salesforceSoap != null)
            {
                if (salesforceSoap is sforce.SalesforceSoap)
                {
                    sforce.LimitInfoHeader limitInfo = ((sforce.SalesforceSoap)salesforceSoap).GetLimitInfoHeader();
                    if (limitInfo == null || limitInfo.limitInfo == null || limitInfo.limitInfo.Length == 0)
                    {

                    }
                    else
                    {
                        infoDockingForm.labelAPILimit.Text = limitInfo.limitInfo[0].limit.ToString("N0");
                        infoDockingForm.labelCurrentAPICalls.Text = limitInfo.limitInfo[0].current.ToString("N0");
                    }
                }
                else
                {
                    sforce_sandbox.LimitInfoHeader limitInfo = ((sforce_sandbox.SalesforceSoapSand)salesforceSoap).GetLimitInfoHeader();
                    if (limitInfo == null || limitInfo.limitInfo == null || limitInfo.limitInfo.Length == 0)
                    {

                    }
                    else
                    {
                        infoDockingForm.labelAPILimit.Text = limitInfo.limitInfo[0].limit.ToString("N0");
                        infoDockingForm.labelCurrentAPICalls.Text = limitInfo.limitInfo[0].current.ToString("N0");
                    }
                }
            }

            // show version info to the title bar
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (!string.IsNullOrEmpty(version))
            {
                this.Text = "Local Object Tracker - " + version;
            }

            timer1.Start();
        }


        private void toolStripButtonBrowseDB_Click(object sender, EventArgs e)
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

        private void UpdatePropertyGridInfo()
        {
            // show dbSalesforceConnection in propertyGridInfo
            //propertyGridInfo.SelectedObject = dbSalesforceConnection;

        }


        private void toolStripButtonManualExecute_Click(object sender, EventArgs e)
        {
            toolStripButtonManualExecute.Enabled = false;
            ExecuteRetrieveChanges();
        }

        private void ExecuteRetrieveChanges()
        {
            isTracking = true;

            // create a background thread to execute track
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, ev) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    toolStripStatusLabelRetrieve.Text = "Retrieving changes from Salesforce... at " + DateTime.Now.ToString("yyyy/MM/dd ddd HH:mm:ss");
                });

                // track worker object
                TrackWorker trackWorker = new TrackWorker(dbSalesforceConnection, dbSalesforceDatabase, salesforceSoap, trackObjects);
                int totalChanges = trackWorker.Track();


                // update API usage info
                this.Invoke((MethodInvoker)delegate
                {
                    infoDockingForm.labelAPILimit.Text = trackWorker.GetAPILimit().ToString("N0");
                    infoDockingForm.labelCurrentAPICalls.Text = trackWorker.GetAPICurrent().ToString("N0");

                    if (totalChanges > 0)
                    {
                        dataDockingForm.UpdateListViewObject();
                        dataDockingForm.UpdateListViewTrackHistory();
                    }
                    toolStripStatusLabelRetrieve.Text = "Last retrieved at " + DateTime.Now.ToString("yyyy/MM/dd ddd HH:mm:ss");
                });

                if (loopTrack)
                {
                    System.Threading.Thread.Sleep(1000 * dbSalesforceConnection.IntervalTime);
                }

                // update ListViewTrackHistory
                this.Invoke((MethodInvoker)delegate
                {
                    if (!loopTrack)
                        toolStripButtonManualExecute.Enabled = true;

                    isTracking = false;
                });
            };
            worker.RunWorkerAsync();
        }

        public class TrackHistory
        {
            public int HistoryId { get; set; }
            public string Operation { get; set; }
            public string Id { get; set; }
            public string ObjectName { get; set; }
            public DateTime LastModifiedDate { get; set; }
            public string RecordName { get; set; }
            public string LastModifiedById { get; set; }
            public string LastModifiedByName { get; set; }
            public string Description { get; set; }
            public DateTime LastRetrievedAt { get; set; }

        }

        private void toolStripButtonStart_Click(object sender, EventArgs e)
        {
            loopTrack = true;
            toolStripButtonStart.Enabled = false;
            toolStripButtonStop.Enabled = true;
            toolStripButtonManualExecute.Enabled = false;

            ExecuteRetrieveChanges();
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            loopTrack = false;
            toolStripButtonStart.Enabled = true;
            toolStripButtonStop.Enabled = false;

            if(!isTracking)
                toolStripButtonManualExecute.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (loopTrack && isTracking == false)
            {
                ExecuteRetrieveChanges();
            }
        }

        public void OpenObjectLink(string objName)
        {
            System.Diagnostics.Process.Start("https://" + dbSalesforceConnection.MyDomain + "/lightning/o/" + objName + "/list");
        }

        public void OpenRecordLink(string objId)
        {
            System.Diagnostics.Process.Start("https://" + dbSalesforceConnection.MyDomain + "/" + objId);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var config = ConfigManager.Instance.Config;
            config.MainFormWidth = this.Width;
            config.MainFormHeight = this.Height;
            config.MainFormX = this.Location.X;
            config.MainFormY = this.Location.Y;
            ConfigManager.Instance.SaveConfig();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // get AssemblyVersion
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (!string.IsNullOrEmpty(version))
            {
                MessageBox.Show("Local Object Tracker\n\nVersion: " + version + "\nAuthor: ----, ----, ----", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

}
