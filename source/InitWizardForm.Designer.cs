namespace LocalObjTracker
{
    partial class InitWizardForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitWizardForm));
            this.labelStep1 = new System.Windows.Forms.Label();
            this.labelStep2 = new System.Windows.Forms.Label();
            this.labelStep3 = new System.Windows.Forms.Label();
            this.labelStep4 = new System.Windows.Forms.Label();
            this.panelStep1 = new System.Windows.Forms.Panel();
            this.pictureBoxReuseLogin = new System.Windows.Forms.PictureBox();
            this.radioSandbox = new System.Windows.Forms.RadioButton();
            this.radioProduction = new System.Windows.Forms.RadioButton();
            this.labelOrgType = new System.Windows.Forms.Label();
            this.textSecurityToken = new System.Windows.Forms.TextBox();
            this.labelSecurityToken = new System.Windows.Forms.Label();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textUsername = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.buttonPrev = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.panelStep2 = new System.Windows.Forms.Panel();
            this.checkShowAllObject = new System.Windows.Forms.CheckBox();
            this.buttonRemoveObject = new System.Windows.Forms.Button();
            this.buttonAddObject = new System.Windows.Forms.Button();
            this.listObjectTo = new System.Windows.Forms.ListBox();
            this.listObjectFrom = new System.Windows.Forms.ListBox();
            this.panelStep3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelConfirm = new System.Windows.Forms.Label();
            this.listViewObject = new System.Windows.Forms.ListView();
            this.No = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Label = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ApiName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Count = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LastModifiedDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelStep4 = new System.Windows.Forms.Panel();
            this.labelEstimateTime = new System.Windows.Forms.Label();
            this.labelRetrieveData = new System.Windows.Forms.Label();
            this.progressBarRetrieveData = new System.Windows.Forms.ProgressBar();
            this.labelParseField = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBarParseField = new System.Windows.Forms.ProgressBar();
            this.labelRetrieving = new System.Windows.Forms.Label();
            this.listBoxConnection = new System.Windows.Forms.ListBox();
            this.labelReuseCredential = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panelStep1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxReuseLogin)).BeginInit();
            this.panelStep2.SuspendLayout();
            this.panelStep3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelStep4.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelStep1
            // 
            this.labelStep1.AutoSize = true;
            this.labelStep1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelStep1.Location = new System.Drawing.Point(12, 27);
            this.labelStep1.Name = "labelStep1";
            this.labelStep1.Size = new System.Drawing.Size(104, 15);
            this.labelStep1.TabIndex = 0;
            this.labelStep1.Text = "Step 1. Login";
            // 
            // labelStep2
            // 
            this.labelStep2.AutoSize = true;
            this.labelStep2.Location = new System.Drawing.Point(13, 55);
            this.labelStep2.Name = "labelStep2";
            this.labelStep2.Size = new System.Drawing.Size(149, 15);
            this.labelStep2.TabIndex = 1;
            this.labelStep2.Text = "Step 2. Select objects";
            // 
            // labelStep3
            // 
            this.labelStep3.AutoSize = true;
            this.labelStep3.Location = new System.Drawing.Point(13, 83);
            this.labelStep3.Name = "labelStep3";
            this.labelStep3.Size = new System.Drawing.Size(107, 15);
            this.labelStep3.TabIndex = 2;
            this.labelStep3.Text = "Step 3. Confirm";
            // 
            // labelStep4
            // 
            this.labelStep4.AutoSize = true;
            this.labelStep4.Location = new System.Drawing.Point(13, 112);
            this.labelStep4.Name = "labelStep4";
            this.labelStep4.Size = new System.Drawing.Size(108, 15);
            this.labelStep4.TabIndex = 3;
            this.labelStep4.Text = "Step 4. Initialize";
            // 
            // panelStep1
            // 
            this.panelStep1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelStep1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStep1.Controls.Add(this.pictureBoxReuseLogin);
            this.panelStep1.Controls.Add(this.radioSandbox);
            this.panelStep1.Controls.Add(this.radioProduction);
            this.panelStep1.Controls.Add(this.labelOrgType);
            this.panelStep1.Controls.Add(this.textSecurityToken);
            this.panelStep1.Controls.Add(this.labelSecurityToken);
            this.panelStep1.Controls.Add(this.textPassword);
            this.panelStep1.Controls.Add(this.labelPassword);
            this.panelStep1.Controls.Add(this.textUsername);
            this.panelStep1.Controls.Add(this.labelUsername);
            this.panelStep1.Location = new System.Drawing.Point(227, 12);
            this.panelStep1.Name = "panelStep1";
            this.panelStep1.Size = new System.Drawing.Size(749, 135);
            this.panelStep1.TabIndex = 4;
            // 
            // pictureBoxReuseLogin
            // 
            this.pictureBoxReuseLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxReuseLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxReuseLogin.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxReuseLogin.Image")));
            this.pictureBoxReuseLogin.Location = new System.Drawing.Point(706, 102);
            this.pictureBoxReuseLogin.Name = "pictureBoxReuseLogin";
            this.pictureBoxReuseLogin.Size = new System.Drawing.Size(21, 17);
            this.pictureBoxReuseLogin.TabIndex = 12;
            this.pictureBoxReuseLogin.TabStop = false;
            this.pictureBoxReuseLogin.Click += new System.EventHandler(this.pictureBoxReuseLogin_Click);
            // 
            // radioSandbox
            // 
            this.radioSandbox.AutoSize = true;
            this.radioSandbox.Location = new System.Drawing.Point(256, 98);
            this.radioSandbox.Name = "radioSandbox";
            this.radioSandbox.Size = new System.Drawing.Size(81, 19);
            this.radioSandbox.TabIndex = 8;
            this.radioSandbox.TabStop = true;
            this.radioSandbox.Text = "Sandbox";
            this.radioSandbox.UseVisualStyleBackColor = true;
            // 
            // radioProduction
            // 
            this.radioProduction.AutoSize = true;
            this.radioProduction.Checked = true;
            this.radioProduction.Location = new System.Drawing.Point(140, 98);
            this.radioProduction.Name = "radioProduction";
            this.radioProduction.Size = new System.Drawing.Size(97, 19);
            this.radioProduction.TabIndex = 7;
            this.radioProduction.TabStop = true;
            this.radioProduction.Text = "Production";
            this.radioProduction.UseVisualStyleBackColor = true;
            // 
            // labelOrgType
            // 
            this.labelOrgType.AutoSize = true;
            this.labelOrgType.Location = new System.Drawing.Point(24, 100);
            this.labelOrgType.Name = "labelOrgType";
            this.labelOrgType.Size = new System.Drawing.Size(66, 15);
            this.labelOrgType.TabIndex = 6;
            this.labelOrgType.Text = "Org Type";
            // 
            // textSecurityToken
            // 
            this.textSecurityToken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSecurityToken.Location = new System.Drawing.Point(140, 70);
            this.textSecurityToken.Name = "textSecurityToken";
            this.textSecurityToken.Size = new System.Drawing.Size(587, 22);
            this.textSecurityToken.TabIndex = 5;
            // 
            // labelSecurityToken
            // 
            this.labelSecurityToken.AutoSize = true;
            this.labelSecurityToken.Location = new System.Drawing.Point(24, 73);
            this.labelSecurityToken.Name = "labelSecurityToken";
            this.labelSecurityToken.Size = new System.Drawing.Size(105, 15);
            this.labelSecurityToken.TabIndex = 4;
            this.labelSecurityToken.Text = "Security Token";
            // 
            // textPassword
            // 
            this.textPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textPassword.Location = new System.Drawing.Point(140, 42);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(587, 22);
            this.textPassword.TabIndex = 3;
            this.textPassword.UseSystemPasswordChar = true;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(24, 45);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(67, 15);
            this.labelPassword.TabIndex = 2;
            this.labelPassword.Text = "Password";
            // 
            // textUsername
            // 
            this.textUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textUsername.Location = new System.Drawing.Point(140, 14);
            this.textUsername.Name = "textUsername";
            this.textUsername.Size = new System.Drawing.Size(587, 22);
            this.textUsername.TabIndex = 1;
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(24, 17);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(71, 15);
            this.labelUsername.TabIndex = 0;
            this.labelUsername.Text = "Username";
            // 
            // buttonPrev
            // 
            this.buttonPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPrev.Location = new System.Drawing.Point(726, 747);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new System.Drawing.Size(122, 34);
            this.buttonPrev.TabIndex = 5;
            this.buttonPrev.Text = "Prev";
            this.buttonPrev.UseVisualStyleBackColor = true;
            this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNext.Location = new System.Drawing.Point(854, 747);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(122, 34);
            this.buttonNext.TabIndex = 6;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // panelStep2
            // 
            this.panelStep2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelStep2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStep2.Controls.Add(this.checkShowAllObject);
            this.panelStep2.Controls.Add(this.buttonRemoveObject);
            this.panelStep2.Controls.Add(this.buttonAddObject);
            this.panelStep2.Controls.Add(this.listObjectTo);
            this.panelStep2.Controls.Add(this.listObjectFrom);
            this.panelStep2.Location = new System.Drawing.Point(227, 171);
            this.panelStep2.Name = "panelStep2";
            this.panelStep2.Size = new System.Drawing.Size(749, 194);
            this.panelStep2.TabIndex = 9;
            // 
            // checkShowAllObject
            // 
            this.checkShowAllObject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkShowAllObject.AutoSize = true;
            this.checkShowAllObject.Location = new System.Drawing.Point(14, 160);
            this.checkShowAllObject.Name = "checkShowAllObject";
            this.checkShowAllObject.Size = new System.Drawing.Size(133, 19);
            this.checkShowAllObject.TabIndex = 4;
            this.checkShowAllObject.Text = "Show all objects";
            this.checkShowAllObject.UseVisualStyleBackColor = true;
            this.checkShowAllObject.CheckedChanged += new System.EventHandler(this.checkShowAllObject_CheckedChanged);
            // 
            // buttonRemoveObject
            // 
            this.buttonRemoveObject.Location = new System.Drawing.Point(352, 84);
            this.buttonRemoveObject.Name = "buttonRemoveObject";
            this.buttonRemoveObject.Size = new System.Drawing.Size(48, 30);
            this.buttonRemoveObject.TabIndex = 3;
            this.buttonRemoveObject.Text = "<-";
            this.buttonRemoveObject.UseVisualStyleBackColor = true;
            this.buttonRemoveObject.Click += new System.EventHandler(this.buttonRemoveObject_Click);
            // 
            // buttonAddObject
            // 
            this.buttonAddObject.Location = new System.Drawing.Point(352, 48);
            this.buttonAddObject.Name = "buttonAddObject";
            this.buttonAddObject.Size = new System.Drawing.Size(48, 30);
            this.buttonAddObject.TabIndex = 2;
            this.buttonAddObject.Text = "->";
            this.buttonAddObject.UseVisualStyleBackColor = true;
            this.buttonAddObject.Click += new System.EventHandler(this.buttonAddObject_Click);
            // 
            // listObjectTo
            // 
            this.listObjectTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listObjectTo.FormattingEnabled = true;
            this.listObjectTo.ItemHeight = 15;
            this.listObjectTo.Location = new System.Drawing.Point(415, 15);
            this.listObjectTo.Name = "listObjectTo";
            this.listObjectTo.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listObjectTo.Size = new System.Drawing.Size(312, 139);
            this.listObjectTo.TabIndex = 1;
            this.listObjectTo.DoubleClick += new System.EventHandler(this.listObjectTo_DoubleClick);
            // 
            // listObjectFrom
            // 
            this.listObjectFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listObjectFrom.FormattingEnabled = true;
            this.listObjectFrom.ItemHeight = 15;
            this.listObjectFrom.Location = new System.Drawing.Point(14, 15);
            this.listObjectFrom.Name = "listObjectFrom";
            this.listObjectFrom.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listObjectFrom.Size = new System.Drawing.Size(323, 139);
            this.listObjectFrom.TabIndex = 0;
            this.listObjectFrom.DoubleClick += new System.EventHandler(this.listObjectFrom_DoubleClick);
            // 
            // panelStep3
            // 
            this.panelStep3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelStep3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStep3.Controls.Add(this.pictureBox1);
            this.panelStep3.Controls.Add(this.labelConfirm);
            this.panelStep3.Controls.Add(this.listViewObject);
            this.panelStep3.Location = new System.Drawing.Point(227, 386);
            this.panelStep3.Name = "panelStep3";
            this.panelStep3.Size = new System.Drawing.Size(749, 201);
            this.panelStep3.TabIndex = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(706, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(21, 17);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // labelConfirm
            // 
            this.labelConfirm.AutoSize = true;
            this.labelConfirm.Location = new System.Drawing.Point(19, 13);
            this.labelConfirm.Name = "labelConfirm";
            this.labelConfirm.Size = new System.Drawing.Size(662, 15);
            this.labelConfirm.TabIndex = 1;
            this.labelConfirm.Text = "Please confirm the select track objects. (If the api name is a SQLite keyword, a " +
    "prefix t_ will be added)";
            // 
            // listViewObject
            // 
            this.listViewObject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewObject.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.No,
            this.Label,
            this.ApiName,
            this.Count,
            this.LastModifiedDate});
            this.listViewObject.FullRowSelect = true;
            this.listViewObject.GridLines = true;
            this.listViewObject.HideSelection = false;
            this.listViewObject.Location = new System.Drawing.Point(14, 41);
            this.listViewObject.Name = "listViewObject";
            this.listViewObject.Size = new System.Drawing.Size(713, 139);
            this.listViewObject.TabIndex = 0;
            this.listViewObject.UseCompatibleStateImageBehavior = false;
            this.listViewObject.View = System.Windows.Forms.View.Details;
            // 
            // No
            // 
            this.No.Text = "NO";
            this.No.Width = 40;
            // 
            // Label
            // 
            this.Label.Text = "LABEL";
            this.Label.Width = 120;
            // 
            // ApiName
            // 
            this.ApiName.Text = "API NAME";
            this.ApiName.Width = 150;
            // 
            // Count
            // 
            this.Count.Text = "COUNT";
            this.Count.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Count.Width = 70;
            // 
            // LastModifiedDate
            // 
            this.LastModifiedDate.Text = "UPATE DATE";
            this.LastModifiedDate.Width = 150;
            // 
            // panelStep4
            // 
            this.panelStep4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelStep4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStep4.Controls.Add(this.labelEstimateTime);
            this.panelStep4.Controls.Add(this.labelRetrieveData);
            this.panelStep4.Controls.Add(this.progressBarRetrieveData);
            this.panelStep4.Controls.Add(this.labelParseField);
            this.panelStep4.Controls.Add(this.label1);
            this.panelStep4.Controls.Add(this.progressBarParseField);
            this.panelStep4.Controls.Add(this.labelRetrieving);
            this.panelStep4.Location = new System.Drawing.Point(227, 593);
            this.panelStep4.Name = "panelStep4";
            this.panelStep4.Size = new System.Drawing.Size(749, 170);
            this.panelStep4.TabIndex = 11;
            // 
            // labelEstimateTime
            // 
            this.labelEstimateTime.AutoSize = true;
            this.labelEstimateTime.Location = new System.Drawing.Point(19, 139);
            this.labelEstimateTime.Name = "labelEstimateTime";
            this.labelEstimateTime.Size = new System.Drawing.Size(103, 19);
            this.labelEstimateTime.TabIndex = 9;
            this.labelEstimateTime.Text = "Eclipsed: --";
            // 
            // labelRetrieveData
            // 
            this.labelRetrieveData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRetrieveData.AutoSize = true;
            this.labelRetrieveData.Location = new System.Drawing.Point(363, 89);
            this.labelRetrieveData.Name = "labelRetrieveData";
            this.labelRetrieveData.Size = new System.Drawing.Size(237, 15);
            this.labelRetrieveData.TabIndex = 8;
            this.labelRetrieveData.Text = "xxx (20/30 Objects, 2,000 Records)";
            // 
            // progressBarRetrieveData
            // 
            this.progressBarRetrieveData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarRetrieveData.Location = new System.Drawing.Point(37, 84);
            this.progressBarRetrieveData.Name = "progressBarRetrieveData";
            this.progressBarRetrieveData.Size = new System.Drawing.Size(320, 23);
            this.progressBarRetrieveData.TabIndex = 7;
            // 
            // labelParseField
            // 
            this.labelParseField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelParseField.AutoSize = true;
            this.labelParseField.Location = new System.Drawing.Point(363, 35);
            this.labelParseField.Name = "labelParseField";
            this.labelParseField.Size = new System.Drawing.Size(137, 15);
            this.labelParseField.TabIndex = 6;
            this.labelParseField.Text = "xxx (20/30 Objects)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Parsing Object Fields";
            // 
            // progressBarParseField
            // 
            this.progressBarParseField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarParseField.Location = new System.Drawing.Point(37, 32);
            this.progressBarParseField.Name = "progressBarParseField";
            this.progressBarParseField.Size = new System.Drawing.Size(320, 23);
            this.progressBarParseField.TabIndex = 4;
            // 
            // labelRetrieving
            // 
            this.labelRetrieving.AutoSize = true;
            this.labelRetrieving.Location = new System.Drawing.Point(20, 66);
            this.labelRetrieving.Name = "labelRetrieving";
            this.labelRetrieving.Size = new System.Drawing.Size(601, 15);
            this.labelRetrieving.TabIndex = 0;
            this.labelRetrieving.Text = "Retrieving Records (LDV will use Bulk API without Compound address and geolocatio" +
    "n fields)";
            // 
            // listBoxConnection
            // 
            this.listBoxConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxConnection.FormattingEnabled = true;
            this.listBoxConnection.ItemHeight = 15;
            this.listBoxConnection.Location = new System.Drawing.Point(71, 198);
            this.listBoxConnection.Name = "listBoxConnection";
            this.listBoxConnection.Size = new System.Drawing.Size(138, 334);
            this.listBoxConnection.TabIndex = 12;
            this.listBoxConnection.Visible = false;
            this.listBoxConnection.DoubleClick += new System.EventHandler(this.listBoxConnection_DoubleClick);
            // 
            // labelReuseCredential
            // 
            this.labelReuseCredential.AutoSize = true;
            this.labelReuseCredential.Location = new System.Drawing.Point(68, 180);
            this.labelReuseCredential.Name = "labelReuseCredential";
            this.labelReuseCredential.Size = new System.Drawing.Size(262, 15);
            this.labelReuseCredential.TabIndex = 13;
            this.labelReuseCredential.Text = "Double click to reuse existing login data";
            this.labelReuseCredential.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // InitWizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(988, 793);
            this.Controls.Add(this.labelReuseCredential);
            this.Controls.Add(this.listBoxConnection);
            this.Controls.Add(this.panelStep4);
            this.Controls.Add(this.panelStep3);
            this.Controls.Add(this.panelStep2);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPrev);
            this.Controls.Add(this.panelStep1);
            this.Controls.Add(this.labelStep4);
            this.Controls.Add(this.labelStep3);
            this.Controls.Add(this.labelStep2);
            this.Controls.Add(this.labelStep1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InitWizardForm";
            this.Text = "InitWizardForm";
            this.panelStep1.ResumeLayout(false);
            this.panelStep1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxReuseLogin)).EndInit();
            this.panelStep2.ResumeLayout(false);
            this.panelStep2.PerformLayout();
            this.panelStep3.ResumeLayout(false);
            this.panelStep3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelStep4.ResumeLayout(false);
            this.panelStep4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelStep1;
        private System.Windows.Forms.Label labelStep2;
        private System.Windows.Forms.Label labelStep3;
        private System.Windows.Forms.Label labelStep4;
        private System.Windows.Forms.Panel panelStep1;
        private System.Windows.Forms.Button buttonPrev;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.TextBox textSecurityToken;
        private System.Windows.Forms.Label labelSecurityToken;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textUsername;
        private System.Windows.Forms.RadioButton radioSandbox;
        private System.Windows.Forms.RadioButton radioProduction;
        private System.Windows.Forms.Label labelOrgType;
        private System.Windows.Forms.Panel panelStep2;
        private System.Windows.Forms.CheckBox checkShowAllObject;
        private System.Windows.Forms.Button buttonRemoveObject;
        private System.Windows.Forms.Button buttonAddObject;
        private System.Windows.Forms.ListBox listObjectTo;
        private System.Windows.Forms.ListBox listObjectFrom;
        private System.Windows.Forms.Panel panelStep3;
        private System.Windows.Forms.Label labelConfirm;
        private System.Windows.Forms.ListView listViewObject;
        private System.Windows.Forms.ColumnHeader No;
        private System.Windows.Forms.ColumnHeader Label;
        private System.Windows.Forms.ColumnHeader ApiName;
        private System.Windows.Forms.ColumnHeader Count;
        private System.Windows.Forms.ColumnHeader LastModifiedDate;
        private System.Windows.Forms.Panel panelStep4;
        private System.Windows.Forms.Label labelRetrieving;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBarParseField;
        private System.Windows.Forms.Label labelEstimateTime;
        private System.Windows.Forms.Label labelRetrieveData;
        private System.Windows.Forms.ProgressBar progressBarRetrieveData;
        private System.Windows.Forms.Label labelParseField;
        private System.Windows.Forms.PictureBox pictureBoxReuseLogin;
        private System.Windows.Forms.ListBox listBoxConnection;
        private System.Windows.Forms.Label labelReuseCredential;
        private System.Windows.Forms.Timer timer1;
    }
}