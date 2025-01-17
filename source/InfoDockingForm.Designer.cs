namespace LocalObjTracker
{
    partial class InfoDockingForm
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
            this.labelOrg = new System.Windows.Forms.Label();
            this.linkLabelOrg = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelOrgType = new System.Windows.Forms.Label();
            this.linkLabelUsername = new System.Windows.Forms.LinkLabel();
            this.labelEmail = new System.Windows.Forms.Label();
            this.linkLabelSetupHome = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelAPILimit = new System.Windows.Forms.Label();
            this.labelCurrentAPICalls = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.linkLabelSQLiteFile = new System.Windows.Forms.LinkLabel();
            this.labelCreatedDate = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.labelUserTimeZone = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelIntervalTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelOrg
            // 
            this.labelOrg.Location = new System.Drawing.Point(12, 10);
            this.labelOrg.Name = "labelOrg";
            this.labelOrg.Size = new System.Drawing.Size(126, 15);
            this.labelOrg.TabIndex = 0;
            this.labelOrg.Text = "Organization:";
            this.labelOrg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // linkLabelOrg
            // 
            this.linkLabelOrg.AutoSize = true;
            this.linkLabelOrg.Location = new System.Drawing.Point(144, 10);
            this.linkLabelOrg.Name = "linkLabelOrg";
            this.linkLabelOrg.Size = new System.Drawing.Size(34, 15);
            this.linkLabelOrg.TabIndex = 1;
            this.linkLabelOrg.TabStop = true;
            this.linkLabelOrg.Text = "URL";
            this.linkLabelOrg.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelOrg_LinkClicked);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Org Type:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Username:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Email:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "Setup Home:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "Setup Object:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelOrgType
            // 
            this.labelOrgType.AutoSize = true;
            this.labelOrgType.Location = new System.Drawing.Point(144, 35);
            this.labelOrgType.Name = "labelOrgType";
            this.labelOrgType.Size = new System.Drawing.Size(34, 15);
            this.labelOrgType.TabIndex = 7;
            this.labelOrgType.Text = "type";
            // 
            // linkLabelUsername
            // 
            this.linkLabelUsername.AutoSize = true;
            this.linkLabelUsername.Location = new System.Drawing.Point(144, 60);
            this.linkLabelUsername.Name = "linkLabelUsername";
            this.linkLabelUsername.Size = new System.Drawing.Size(34, 15);
            this.linkLabelUsername.TabIndex = 8;
            this.linkLabelUsername.TabStop = true;
            this.linkLabelUsername.Text = "URL";
            this.linkLabelUsername.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUsername_LinkClicked);
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(144, 85);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(39, 15);
            this.labelEmail.TabIndex = 9;
            this.labelEmail.Text = "email";
            // 
            // linkLabelSetupHome
            // 
            this.linkLabelSetupHome.AutoSize = true;
            this.linkLabelSetupHome.Location = new System.Drawing.Point(144, 110);
            this.linkLabelSetupHome.Name = "linkLabelSetupHome";
            this.linkLabelSetupHome.Size = new System.Drawing.Size(34, 15);
            this.linkLabelSetupHome.TabIndex = 10;
            this.linkLabelSetupHome.TabStop = true;
            this.linkLabelSetupHome.Text = "URL";
            this.linkLabelSetupHome.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSetupHome_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(144, 135);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(34, 15);
            this.linkLabel1.TabIndex = 11;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "URL";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "API Limit (24H):";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 185);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(126, 15);
            this.label7.TabIndex = 13;
            this.label7.Text = "Current API #:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAPILimit
            // 
            this.labelAPILimit.AutoSize = true;
            this.labelAPILimit.Location = new System.Drawing.Point(144, 160);
            this.labelAPILimit.Name = "labelAPILimit";
            this.labelAPILimit.Size = new System.Drawing.Size(193, 19);
            this.labelAPILimit.TabIndex = 14;
            this.labelAPILimit.Text = "(Show after soap calls)";
            // 
            // labelCurrentAPICalls
            // 
            this.labelCurrentAPICalls.AutoSize = true;
            this.labelCurrentAPICalls.Location = new System.Drawing.Point(144, 185);
            this.labelCurrentAPICalls.Name = "labelCurrentAPICalls";
            this.labelCurrentAPICalls.Size = new System.Drawing.Size(193, 19);
            this.labelCurrentAPICalls.TabIndex = 15;
            this.labelCurrentAPICalls.Text = "(Show after soap calls)";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(12, 210);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(126, 15);
            this.label8.TabIndex = 16;
            this.label8.Text = "SQLite File:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(12, 235);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(126, 15);
            this.label9.TabIndex = 17;
            this.label9.Text = "Created Date:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // linkLabelSQLiteFile
            // 
            this.linkLabelSQLiteFile.AutoSize = true;
            this.linkLabelSQLiteFile.Location = new System.Drawing.Point(144, 210);
            this.linkLabelSQLiteFile.Name = "linkLabelSQLiteFile";
            this.linkLabelSQLiteFile.Size = new System.Drawing.Size(34, 15);
            this.linkLabelSQLiteFile.TabIndex = 18;
            this.linkLabelSQLiteFile.TabStop = true;
            this.linkLabelSQLiteFile.Text = "URL";
            this.linkLabelSQLiteFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSQLiteFile_LinkClicked);
            // 
            // labelCreatedDate
            // 
            this.labelCreatedDate.AutoSize = true;
            this.labelCreatedDate.Location = new System.Drawing.Point(144, 235);
            this.labelCreatedDate.Name = "labelCreatedDate";
            this.labelCreatedDate.Size = new System.Drawing.Size(55, 15);
            this.labelCreatedDate.TabIndex = 20;
            this.labelCreatedDate.Text = "------";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(12, 260);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(126, 15);
            this.label11.TabIndex = 22;
            this.label11.Text = "User TimeZone:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelUserTimeZone
            // 
            this.labelUserTimeZone.AutoSize = true;
            this.labelUserTimeZone.Location = new System.Drawing.Point(144, 260);
            this.labelUserTimeZone.Name = "labelUserTimeZone";
            this.labelUserTimeZone.Size = new System.Drawing.Size(55, 15);
            this.labelUserTimeZone.TabIndex = 23;
            this.labelUserTimeZone.Text = "------";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(12, 285);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(126, 15);
            this.label10.TabIndex = 24;
            this.label10.Text = "Interval Time (s):";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelIntervalTime
            // 
            this.labelIntervalTime.AutoSize = true;
            this.labelIntervalTime.Location = new System.Drawing.Point(144, 285);
            this.labelIntervalTime.Name = "labelIntervalTime";
            this.labelIntervalTime.Size = new System.Drawing.Size(55, 15);
            this.labelIntervalTime.TabIndex = 23;
            this.labelIntervalTime.Text = "------";
            // 
            // InfoDockingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 392);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.labelIntervalTime);
            this.Controls.Add(this.labelUserTimeZone);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.labelCreatedDate);
            this.Controls.Add(this.linkLabelSQLiteFile);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labelCurrentAPICalls);
            this.Controls.Add(this.labelAPILimit);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.linkLabelSetupHome);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.linkLabelUsername);
            this.Controls.Add(this.labelOrgType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabelOrg);
            this.Controls.Add(this.labelOrg);
            this.Name = "InfoDockingForm";
            this.Text = "Info";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelOrg;
        private System.Windows.Forms.LinkLabel linkLabelOrg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelOrgType;
        private System.Windows.Forms.LinkLabel linkLabelUsername;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.LinkLabel linkLabelSetupHome;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label labelAPILimit;
        public System.Windows.Forms.Label labelCurrentAPICalls;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.LinkLabel linkLabelSQLiteFile;
        public System.Windows.Forms.Label labelCreatedDate;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.Label labelUserTimeZone;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.Label labelIntervalTime;
    }
}