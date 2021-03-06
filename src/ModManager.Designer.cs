﻿namespace Technic_Modpack_Creator
{
    partial class ModManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModManager));
            this.modListView = new System.Windows.Forms.ListView();
            this.modName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.uriState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.versionLocal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.versionLatest = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.releaseDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.updateState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.debug1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.updateListButton = new System.Windows.Forms.Button();
            this.modInfoBox = new System.Windows.Forms.GroupBox();
            this.googleButton = new System.Windows.Forms.Button();
            this.canUpdateBox = new System.Windows.Forms.CheckBox();
            this.updateSelectedButton = new System.Windows.Forms.Button();
            this.openSiteButton = new System.Windows.Forms.Button();
            this.linkStatusLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.modDownloadBox = new System.Windows.Forms.TextBox();
            this.modSiteBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.modNameBox = new System.Windows.Forms.TextBox();
            this.updateCheckButton = new System.Windows.Forms.Button();
            this.updateModsButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.findSiteButton = new System.Windows.Forms.Button();
            this.loadDataButton = new System.Windows.Forms.Button();
            this.writeDataButton = new System.Windows.Forms.Button();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.findSelectedButton = new System.Windows.Forms.Button();
            this.checkSelectedButton = new System.Windows.Forms.Button();
            this.modInfoBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // modListView
            // 
            this.modListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modListView.CheckBoxes = true;
            this.modListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.modName,
            this.uriState,
            this.versionLocal,
            this.versionLatest,
            this.releaseDate,
            this.updateState,
            this.debug1,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.modListView.FullRowSelect = true;
            this.modListView.GridLines = true;
            this.modListView.HideSelection = false;
            this.modListView.Location = new System.Drawing.Point(12, 205);
            this.modListView.MultiSelect = false;
            this.modListView.Name = "modListView";
            this.modListView.Size = new System.Drawing.Size(1359, 442);
            this.modListView.TabIndex = 0;
            this.modListView.UseCompatibleStateImageBehavior = false;
            this.modListView.View = System.Windows.Forms.View.Details;
            this.modListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.modListView_ItemChecked);
            this.modListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.modListView_ItemSelectionChanged);
            // 
            // modName
            // 
            this.modName.Text = "Mod Name";
            this.modName.Width = 216;
            // 
            // uriState
            // 
            this.uriState.Text = "URI State";
            this.uriState.Width = 91;
            // 
            // versionLocal
            // 
            this.versionLocal.Text = "Local Version";
            this.versionLocal.Width = 114;
            // 
            // versionLatest
            // 
            this.versionLatest.Text = "Latest Version";
            this.versionLatest.Width = 114;
            // 
            // releaseDate
            // 
            this.releaseDate.Text = "Release Date";
            this.releaseDate.Width = 100;
            // 
            // updateState
            // 
            this.updateState.Text = "Update State";
            this.updateState.Width = 141;
            // 
            // debug1
            // 
            this.debug1.Text = "New Filename";
            this.debug1.Width = 130;
            // 
            // updateListButton
            // 
            this.updateListButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.updateListButton.Location = new System.Drawing.Point(12, 155);
            this.updateListButton.Name = "updateListButton";
            this.updateListButton.Size = new System.Drawing.Size(895, 44);
            this.updateListButton.TabIndex = 1;
            this.updateListButton.Text = "Update Modlist";
            this.updateListButton.UseVisualStyleBackColor = true;
            this.updateListButton.Click += new System.EventHandler(this.updateListButton_Click);
            // 
            // modInfoBox
            // 
            this.modInfoBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modInfoBox.Controls.Add(this.checkSelectedButton);
            this.modInfoBox.Controls.Add(this.findSelectedButton);
            this.modInfoBox.Controls.Add(this.googleButton);
            this.modInfoBox.Controls.Add(this.canUpdateBox);
            this.modInfoBox.Controls.Add(this.updateSelectedButton);
            this.modInfoBox.Controls.Add(this.openSiteButton);
            this.modInfoBox.Controls.Add(this.linkStatusLabel);
            this.modInfoBox.Controls.Add(this.label3);
            this.modInfoBox.Controls.Add(this.modDownloadBox);
            this.modInfoBox.Controls.Add(this.modSiteBox);
            this.modInfoBox.Controls.Add(this.label2);
            this.modInfoBox.Controls.Add(this.label1);
            this.modInfoBox.Controls.Add(this.modNameBox);
            this.modInfoBox.Location = new System.Drawing.Point(12, 12);
            this.modInfoBox.Name = "modInfoBox";
            this.modInfoBox.Size = new System.Drawing.Size(1126, 137);
            this.modInfoBox.TabIndex = 2;
            this.modInfoBox.TabStop = false;
            this.modInfoBox.Text = "Mod Information";
            // 
            // googleButton
            // 
            this.googleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.googleButton.Location = new System.Drawing.Point(1027, 19);
            this.googleButton.Name = "googleButton";
            this.googleButton.Size = new System.Drawing.Size(93, 20);
            this.googleButton.TabIndex = 10;
            this.googleButton.Text = "Google Search";
            this.googleButton.UseVisualStyleBackColor = true;
            this.googleButton.Click += new System.EventHandler(this.googleButton_Click);
            // 
            // canUpdateBox
            // 
            this.canUpdateBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.canUpdateBox.AutoSize = true;
            this.canUpdateBox.Location = new System.Drawing.Point(1013, 107);
            this.canUpdateBox.Name = "canUpdateBox";
            this.canUpdateBox.Size = new System.Drawing.Size(107, 17);
            this.canUpdateBox.TabIndex = 9;
            this.canUpdateBox.Text = "Update Available";
            this.canUpdateBox.UseVisualStyleBackColor = true;
            this.canUpdateBox.CheckedChanged += new System.EventHandler(this.canUpdateBox_CheckedChanged);
            // 
            // updateSelectedButton
            // 
            this.updateSelectedButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.updateSelectedButton.Location = new System.Drawing.Point(9, 97);
            this.updateSelectedButton.Name = "updateSelectedButton";
            this.updateSelectedButton.Size = new System.Drawing.Size(690, 34);
            this.updateSelectedButton.TabIndex = 8;
            this.updateSelectedButton.Text = "Update Mod";
            this.updateSelectedButton.UseVisualStyleBackColor = true;
            this.updateSelectedButton.Click += new System.EventHandler(this.updateSelectedButton_Click);
            // 
            // openSiteButton
            // 
            this.openSiteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openSiteButton.Location = new System.Drawing.Point(990, 45);
            this.openSiteButton.Name = "openSiteButton";
            this.openSiteButton.Size = new System.Drawing.Size(56, 20);
            this.openSiteButton.TabIndex = 7;
            this.openSiteButton.Text = "Open";
            this.openSiteButton.UseVisualStyleBackColor = true;
            this.openSiteButton.Click += new System.EventHandler(this.openSiteButton_Click);
            // 
            // linkStatusLabel
            // 
            this.linkStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkStatusLabel.AutoSize = true;
            this.linkStatusLabel.Location = new System.Drawing.Point(1052, 48);
            this.linkStatusLabel.Name = "linkStatusLabel";
            this.linkStatusLabel.Size = new System.Drawing.Size(68, 13);
            this.linkStatusLabel.TabIndex = 6;
            this.linkStatusLabel.Text = "Unsupported";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Download Link:";
            // 
            // modDownloadBox
            // 
            this.modDownloadBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modDownloadBox.Location = new System.Drawing.Point(97, 71);
            this.modDownloadBox.Name = "modDownloadBox";
            this.modDownloadBox.ReadOnly = true;
            this.modDownloadBox.Size = new System.Drawing.Size(1023, 20);
            this.modDownloadBox.TabIndex = 4;
            // 
            // modSiteBox
            // 
            this.modSiteBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modSiteBox.Location = new System.Drawing.Point(97, 45);
            this.modSiteBox.Name = "modSiteBox";
            this.modSiteBox.Size = new System.Drawing.Size(887, 20);
            this.modSiteBox.TabIndex = 3;
            this.modSiteBox.TextChanged += new System.EventHandler(this.modSiteBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Website Link:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "File Name:";
            // 
            // modNameBox
            // 
            this.modNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modNameBox.Location = new System.Drawing.Point(97, 19);
            this.modNameBox.Name = "modNameBox";
            this.modNameBox.ReadOnly = true;
            this.modNameBox.Size = new System.Drawing.Size(924, 20);
            this.modNameBox.TabIndex = 0;
            // 
            // updateCheckButton
            // 
            this.updateCheckButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.updateCheckButton.Location = new System.Drawing.Point(6, 19);
            this.updateCheckButton.Name = "updateCheckButton";
            this.updateCheckButton.Size = new System.Drawing.Size(215, 31);
            this.updateCheckButton.TabIndex = 3;
            this.updateCheckButton.Text = "Check For Updates";
            this.updateCheckButton.UseVisualStyleBackColor = true;
            this.updateCheckButton.Click += new System.EventHandler(this.updateCheckButton_Click);
            // 
            // updateModsButton
            // 
            this.updateModsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.updateModsButton.Location = new System.Drawing.Point(6, 56);
            this.updateModsButton.Name = "updateModsButton";
            this.updateModsButton.Size = new System.Drawing.Size(215, 31);
            this.updateModsButton.TabIndex = 4;
            this.updateModsButton.Text = "Update All Mods";
            this.updateModsButton.UseVisualStyleBackColor = true;
            this.updateModsButton.Click += new System.EventHandler(this.updateModsButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.findSiteButton);
            this.groupBox1.Controls.Add(this.updateCheckButton);
            this.groupBox1.Controls.Add(this.updateModsButton);
            this.groupBox1.Location = new System.Drawing.Point(1144, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 137);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // findSiteButton
            // 
            this.findSiteButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.findSiteButton.Location = new System.Drawing.Point(6, 93);
            this.findSiteButton.Name = "findSiteButton";
            this.findSiteButton.Size = new System.Drawing.Size(215, 31);
            this.findSiteButton.TabIndex = 6;
            this.findSiteButton.Text = "Find Mod Websites";
            this.findSiteButton.UseVisualStyleBackColor = true;
            this.findSiteButton.Click += new System.EventHandler(this.findSiteButton_Click);
            // 
            // loadDataButton
            // 
            this.loadDataButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadDataButton.Location = new System.Drawing.Point(913, 155);
            this.loadDataButton.Name = "loadDataButton";
            this.loadDataButton.Size = new System.Drawing.Size(226, 44);
            this.loadDataButton.TabIndex = 6;
            this.loadDataButton.Text = "Force Load Missing Sites from Database";
            this.loadDataButton.UseVisualStyleBackColor = true;
            this.loadDataButton.Click += new System.EventHandler(this.loadDataButton_Click);
            // 
            // writeDataButton
            // 
            this.writeDataButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.writeDataButton.Location = new System.Drawing.Point(1145, 155);
            this.writeDataButton.Name = "writeDataButton";
            this.writeDataButton.Size = new System.Drawing.Size(226, 44);
            this.writeDataButton.TabIndex = 7;
            this.writeDataButton.Text = "Overwrite all Sites to Database";
            this.writeDataButton.UseVisualStyleBackColor = true;
            this.writeDataButton.Click += new System.EventHandler(this.writeDataButton_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 113;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 106;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Width = 107;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Width = 112;
            // 
            // findSelectedButton
            // 
            this.findSelectedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.findSelectedButton.Location = new System.Drawing.Point(859, 97);
            this.findSelectedButton.Name = "findSelectedButton";
            this.findSelectedButton.Size = new System.Drawing.Size(148, 34);
            this.findSelectedButton.TabIndex = 11;
            this.findSelectedButton.Text = "Find Website";
            this.findSelectedButton.UseVisualStyleBackColor = true;
            this.findSelectedButton.Click += new System.EventHandler(this.findSelectedButton_Click);
            // 
            // checkSelectedButton
            // 
            this.checkSelectedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkSelectedButton.Location = new System.Drawing.Point(705, 97);
            this.checkSelectedButton.Name = "checkSelectedButton";
            this.checkSelectedButton.Size = new System.Drawing.Size(148, 34);
            this.checkSelectedButton.TabIndex = 12;
            this.checkSelectedButton.Text = "Check For Update";
            this.checkSelectedButton.UseVisualStyleBackColor = true;
            this.checkSelectedButton.Click += new System.EventHandler(this.checkSelectedButton_Click);
            // 
            // ModManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1383, 659);
            this.Controls.Add(this.writeDataButton);
            this.Controls.Add(this.loadDataButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.modInfoBox);
            this.Controls.Add(this.updateListButton);
            this.Controls.Add(this.modListView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ModManager";
            this.Text = "Mod Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModManager_FormClosing);
            this.Load += new System.EventHandler(this.ModManager_Load);
            this.Shown += new System.EventHandler(this.ModManager_Shown);
            this.modInfoBox.ResumeLayout(false);
            this.modInfoBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView modListView;
        private System.Windows.Forms.ColumnHeader modName;
        private System.Windows.Forms.ColumnHeader uriState;
        private System.Windows.Forms.ColumnHeader versionLocal;
        private System.Windows.Forms.ColumnHeader versionLatest;
        private System.Windows.Forms.ColumnHeader updateState;
        private System.Windows.Forms.Button updateListButton;
        private System.Windows.Forms.GroupBox modInfoBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox modNameBox;
        private System.Windows.Forms.TextBox modSiteBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox modDownloadBox;
        private System.Windows.Forms.Label linkStatusLabel;
        private System.Windows.Forms.Button openSiteButton;
        private System.Windows.Forms.Button updateCheckButton;
        private System.Windows.Forms.Button updateModsButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button findSiteButton;
        private System.Windows.Forms.Button updateSelectedButton;
        private System.Windows.Forms.CheckBox canUpdateBox;
        private System.Windows.Forms.ColumnHeader releaseDate;
        private System.Windows.Forms.ColumnHeader debug1;
        private System.Windows.Forms.Button loadDataButton;
        private System.Windows.Forms.Button writeDataButton;
        private System.Windows.Forms.Button googleButton;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button checkSelectedButton;
        private System.Windows.Forms.Button findSelectedButton;
    }
}