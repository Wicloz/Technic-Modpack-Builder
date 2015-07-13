namespace Technic_Modpack_Creator
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
            this.updateState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.updateListButton = new System.Windows.Forms.Button();
            this.modInfoBox = new System.Windows.Forms.GroupBox();
            this.linkStatusLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.modDownloadBox = new System.Windows.Forms.TextBox();
            this.modSiteBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.modNameBox = new System.Windows.Forms.TextBox();
            this.modInfoBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // modListView
            // 
            this.modListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.modName,
            this.uriState,
            this.versionLocal,
            this.versionLatest,
            this.updateState});
            this.modListView.FullRowSelect = true;
            this.modListView.GridLines = true;
            this.modListView.HideSelection = false;
            this.modListView.Location = new System.Drawing.Point(12, 167);
            this.modListView.MultiSelect = false;
            this.modListView.Name = "modListView";
            this.modListView.Size = new System.Drawing.Size(638, 414);
            this.modListView.TabIndex = 0;
            this.modListView.UseCompatibleStateImageBehavior = false;
            this.modListView.View = System.Windows.Forms.View.Details;
            this.modListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.modListView_ItemSelectionChanged);
            // 
            // modName
            // 
            this.modName.Text = "Mod Name";
            this.modName.Width = 202;
            // 
            // uriState
            // 
            this.uriState.Text = "URI State";
            this.uriState.Width = 86;
            // 
            // versionLocal
            // 
            this.versionLocal.Text = "Local Version";
            this.versionLocal.Width = 81;
            // 
            // versionLatest
            // 
            this.versionLatest.Text = "Latest Version";
            this.versionLatest.Width = 82;
            // 
            // updateState
            // 
            this.updateState.Text = "Update State";
            this.updateState.Width = 114;
            // 
            // updateListButton
            // 
            this.updateListButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.updateListButton.Location = new System.Drawing.Point(13, 122);
            this.updateListButton.Name = "updateListButton";
            this.updateListButton.Size = new System.Drawing.Size(637, 39);
            this.updateListButton.TabIndex = 1;
            this.updateListButton.Text = "Update Modlist";
            this.updateListButton.UseVisualStyleBackColor = true;
            this.updateListButton.Click += new System.EventHandler(this.updateListButton_Click);
            // 
            // modInfoBox
            // 
            this.modInfoBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modInfoBox.Controls.Add(this.linkStatusLabel);
            this.modInfoBox.Controls.Add(this.label3);
            this.modInfoBox.Controls.Add(this.modDownloadBox);
            this.modInfoBox.Controls.Add(this.modSiteBox);
            this.modInfoBox.Controls.Add(this.label2);
            this.modInfoBox.Controls.Add(this.label1);
            this.modInfoBox.Controls.Add(this.modNameBox);
            this.modInfoBox.Location = new System.Drawing.Point(13, 13);
            this.modInfoBox.Name = "modInfoBox";
            this.modInfoBox.Size = new System.Drawing.Size(637, 103);
            this.modInfoBox.TabIndex = 2;
            this.modInfoBox.TabStop = false;
            this.modInfoBox.Text = "Mod Information";
            // 
            // linkStatusLabel
            // 
            this.linkStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkStatusLabel.AutoSize = true;
            this.linkStatusLabel.Location = new System.Drawing.Point(563, 48);
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
            this.modDownloadBox.Size = new System.Drawing.Size(534, 20);
            this.modDownloadBox.TabIndex = 4;
            // 
            // modSiteBox
            // 
            this.modSiteBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modSiteBox.Location = new System.Drawing.Point(97, 45);
            this.modSiteBox.Name = "modSiteBox";
            this.modSiteBox.Size = new System.Drawing.Size(460, 20);
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
            this.modNameBox.Size = new System.Drawing.Size(534, 20);
            this.modNameBox.TabIndex = 0;
            // 
            // ModManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 593);
            this.Controls.Add(this.modInfoBox);
            this.Controls.Add(this.updateListButton);
            this.Controls.Add(this.modListView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ModManager";
            this.Text = "Mod Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModManager_FormClosing);
            this.modInfoBox.ResumeLayout(false);
            this.modInfoBox.PerformLayout();
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
    }
}