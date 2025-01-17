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
    public partial class RestoreApexForm : Form
    {
        private string developerConsoleUrl = "";
        public RestoreApexForm(string apexCode, string developerConsoleUrl)
        {
            InitializeComponent();
            textBoxApexCode.Text = apexCode;
            this.developerConsoleUrl = developerConsoleUrl;

            // select all text in the textbox
            textBoxApexCode.SelectAll();
        }

        private void linkLabelDeveloperConsole_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Open the developer console in the default browser
            System.Diagnostics.Process.Start(developerConsoleUrl);

        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            // copy the text in the textbox to the clipboard
            Clipboard.SetText(textBoxApexCode.Text);
            buttonCopy.Text = "Copied!";
        }
    }
}
