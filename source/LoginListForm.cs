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
    public partial class LoginListForm : Form
    {
        public List<ManagementDatabase.SalesforceConnection> connections = new List<ManagementDatabase.SalesforceConnection>();
        public ManagementDatabase.SalesforceConnection selectedConnection = null;
        public LoginListForm()
        {
            InitializeComponent();

            // SalesforceConnections テーブルのレコードを取得
            ManagementDatabase db = ManagementDatabase.Instance;
            connections = db.GetSalesforceConnections();

            // listBoxConnection に接続情報を表示
            foreach (ManagementDatabase.SalesforceConnection connection in connections)
            {
                listBoxConnection.Items.Add(connection.ConnectionId.ToString() + ". " + connection.Username + " (" + connection.OrgnaizationType + ", " + 
                    connection.LastRetrievedAt?.ToString("yyyy/MM/dd ddd HH:mm:ss") + ")");
            }

            listBoxConnection.SelectedIndex = 0;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            // show confirm dialog
            DialogResult result = MessageBox.Show("Are you sure you want to reset all local data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // find and delete all *.db files in the current directory
                string[] files = System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory(), "*.db");
                foreach (string file in files)
                {
                    System.IO.File.Delete(file);
                }
                listBoxConnection.Items.Clear();
                buttonLogin.Enabled = false;
                buttonNew.Enabled = true;
                buttonReset.Enabled = false;

                ManagementDatabase.Instance.EnsureDatabaseExists();
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            selectedConnection = null;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            // get selected connection
            selectedConnection = connections[listBoxConnection.SelectedIndex];
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void listBoxConnection_DoubleClick(object sender, EventArgs e)
        {
            buttonLogin_Click(sender, e);
        }
    }
}
