namespace LocalObjTracker
{
    partial class RestoreApexForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxApexCode = new System.Windows.Forms.TextBox();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabelDeveloperConsole = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // textBoxApexCode
            // 
            this.textBoxApexCode.AcceptsReturn = true;
            this.textBoxApexCode.AcceptsTab = true;
            this.textBoxApexCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxApexCode.Location = new System.Drawing.Point(12, 64);
            this.textBoxApexCode.Multiline = true;
            this.textBoxApexCode.Name = "textBoxApexCode";
            this.textBoxApexCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxApexCode.Size = new System.Drawing.Size(1182, 451);
            this.textBoxApexCode.TabIndex = 0;
            // 
            // buttonCopy
            // 
            this.buttonCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopy.Location = new System.Drawing.Point(1100, 23);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(94, 37);
            this.buttonCopy.TabIndex = 1;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(291, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Use the following apex code to restore data.";
            // 
            // linkLabelDeveloperConsole
            // 
            this.linkLabelDeveloperConsole.AutoSize = true;
            this.linkLabelDeveloperConsole.Location = new System.Drawing.Point(12, 9);
            this.linkLabelDeveloperConsole.Name = "linkLabelDeveloperConsole";
            this.linkLabelDeveloperConsole.Size = new System.Drawing.Size(128, 15);
            this.linkLabelDeveloperConsole.TabIndex = 3;
            this.linkLabelDeveloperConsole.TabStop = true;
            this.linkLabelDeveloperConsole.Text = "Developer Console";
            this.linkLabelDeveloperConsole.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDeveloperConsole_LinkClicked);
            // 
            // RestoreApexForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1206, 527);
            this.Controls.Add(this.linkLabelDeveloperConsole);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this.textBoxApexCode);
            this.Name = "RestoreApexForm";
            this.Text = "Restore Data";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxApexCode;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabelDeveloperConsole;
    }
}