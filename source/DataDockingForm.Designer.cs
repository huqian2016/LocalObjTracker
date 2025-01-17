namespace LocalObjTracker
{
    partial class DataDockingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataDockingForm));
            this.pictureBoxExportTrackHistory = new System.Windows.Forms.PictureBox();
            this.pictureBoxExportObject = new System.Windows.Forms.PictureBox();
            this.labelTrackHistory = new System.Windows.Forms.Label();
            this.listViewTrackHistory = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.listViewObject = new System.Windows.Forms.ListView();
            this.pictureBoxRestoreApex = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExportTrackHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExportObject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRestoreApex)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxExportTrackHistory
            // 
            this.pictureBoxExportTrackHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxExportTrackHistory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxExportTrackHistory.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxExportTrackHistory.Image")));
            this.pictureBoxExportTrackHistory.Location = new System.Drawing.Point(800, 316);
            this.pictureBoxExportTrackHistory.Name = "pictureBoxExportTrackHistory";
            this.pictureBoxExportTrackHistory.Size = new System.Drawing.Size(21, 17);
            this.pictureBoxExportTrackHistory.TabIndex = 14;
            this.pictureBoxExportTrackHistory.TabStop = false;
            this.pictureBoxExportTrackHistory.Click += new System.EventHandler(this.pictureBoxExportTrackHistory_Click);
            // 
            // pictureBoxExportObject
            // 
            this.pictureBoxExportObject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxExportObject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxExportObject.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxExportObject.Image")));
            this.pictureBoxExportObject.Location = new System.Drawing.Point(800, 7);
            this.pictureBoxExportObject.Name = "pictureBoxExportObject";
            this.pictureBoxExportObject.Size = new System.Drawing.Size(21, 17);
            this.pictureBoxExportObject.TabIndex = 13;
            this.pictureBoxExportObject.TabStop = false;
            this.pictureBoxExportObject.Click += new System.EventHandler(this.pictureBoxExportObject_Click);
            // 
            // labelTrackHistory
            // 
            this.labelTrackHistory.AutoSize = true;
            this.labelTrackHistory.Location = new System.Drawing.Point(9, 318);
            this.labelTrackHistory.Name = "labelTrackHistory";
            this.labelTrackHistory.Size = new System.Drawing.Size(323, 15);
            this.labelTrackHistory.TabIndex = 12;
            this.labelTrackHistory.Text = "Track History (Double-click to open record page)";
            // 
            // listViewTrackHistory
            // 
            this.listViewTrackHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewTrackHistory.FullRowSelect = true;
            this.listViewTrackHistory.GridLines = true;
            this.listViewTrackHistory.HideSelection = false;
            this.listViewTrackHistory.Location = new System.Drawing.Point(12, 339);
            this.listViewTrackHistory.Name = "listViewTrackHistory";
            this.listViewTrackHistory.Size = new System.Drawing.Size(809, 110);
            this.listViewTrackHistory.TabIndex = 11;
            this.listViewTrackHistory.UseCompatibleStateImageBehavior = false;
            this.listViewTrackHistory.View = System.Windows.Forms.View.Details;
            this.listViewTrackHistory.DoubleClick += new System.EventHandler(this.listViewTrackHistory_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(320, 19);
            this.label1.TabIndex = 10;
            this.label1.Text = "Object (Double-click to open list page)";
            // 
            // listViewObject
            // 
            this.listViewObject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewObject.FullRowSelect = true;
            this.listViewObject.GridLines = true;
            this.listViewObject.HideSelection = false;
            this.listViewObject.Location = new System.Drawing.Point(12, 30);
            this.listViewObject.Name = "listViewObject";
            this.listViewObject.Size = new System.Drawing.Size(809, 277);
            this.listViewObject.TabIndex = 9;
            this.listViewObject.UseCompatibleStateImageBehavior = false;
            this.listViewObject.View = System.Windows.Forms.View.Details;
            this.listViewObject.DoubleClick += new System.EventHandler(this.listViewObject_DoubleClick);
            // 
            // pictureBoxRestoreApex
            // 
            this.pictureBoxRestoreApex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxRestoreApex.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxRestoreApex.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxRestoreApex.Image")));
            this.pictureBoxRestoreApex.Location = new System.Drawing.Point(767, 316);
            this.pictureBoxRestoreApex.Name = "pictureBoxRestoreApex";
            this.pictureBoxRestoreApex.Size = new System.Drawing.Size(21, 17);
            this.pictureBoxRestoreApex.TabIndex = 15;
            this.pictureBoxRestoreApex.TabStop = false;
            this.pictureBoxRestoreApex.Click += new System.EventHandler(this.pictureBoxRestoreApex_Click);
            // 
            // DataDockingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 473);
            this.Controls.Add(this.pictureBoxRestoreApex);
            this.Controls.Add(this.pictureBoxExportTrackHistory);
            this.Controls.Add(this.pictureBoxExportObject);
            this.Controls.Add(this.labelTrackHistory);
            this.Controls.Add(this.listViewTrackHistory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewObject);
            this.Name = "DataDockingForm";
            this.Text = "Data";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExportTrackHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExportObject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRestoreApex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox pictureBoxExportTrackHistory;
        public System.Windows.Forms.PictureBox pictureBoxExportObject;
        public System.Windows.Forms.Label labelTrackHistory;
        public System.Windows.Forms.ListView listViewTrackHistory;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.ListView listViewObject;
        public System.Windows.Forms.PictureBox pictureBoxRestoreApex;
    }
}