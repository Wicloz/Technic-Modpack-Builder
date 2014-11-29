namespace Technic_Modpack_Creator
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.testButtonClient = new System.Windows.Forms.Button();
            this.buildButton = new System.Windows.Forms.Button();
            this.testButtonServer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.siteBox = new System.Windows.Forms.TextBox();
            this.folderBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.minecraftVersionBox = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.modpackVersionBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.setupButton = new System.Windows.Forms.Button();
            this.getForgeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // testButtonClient
            // 
            this.testButtonClient.Location = new System.Drawing.Point(15, 92);
            this.testButtonClient.Name = "testButtonClient";
            this.testButtonClient.Size = new System.Drawing.Size(257, 45);
            this.testButtonClient.TabIndex = 0;
            this.testButtonClient.Text = "Test Modpack Client";
            this.testButtonClient.UseVisualStyleBackColor = true;
            this.testButtonClient.Click += new System.EventHandler(this.testButtonClient_Click);
            // 
            // buildButton
            // 
            this.buildButton.Location = new System.Drawing.Point(12, 245);
            this.buildButton.Name = "buildButton";
            this.buildButton.Size = new System.Drawing.Size(260, 55);
            this.buildButton.TabIndex = 1;
            this.buildButton.Text = "Build Modpack Release";
            this.buildButton.UseVisualStyleBackColor = true;
            this.buildButton.Click += new System.EventHandler(this.buildButton_Click);
            // 
            // testButtonServer
            // 
            this.testButtonServer.Location = new System.Drawing.Point(15, 143);
            this.testButtonServer.Name = "testButtonServer";
            this.testButtonServer.Size = new System.Drawing.Size(257, 45);
            this.testButtonServer.TabIndex = 2;
            this.testButtonServer.Text = "Test Modpack Server";
            this.testButtonServer.UseVisualStyleBackColor = true;
            this.testButtonServer.Click += new System.EventHandler(this.testButtonServer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Minecraft Version:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Site Suffix";
            // 
            // siteBox
            // 
            this.siteBox.Location = new System.Drawing.Point(72, 66);
            this.siteBox.Name = "siteBox";
            this.siteBox.Size = new System.Drawing.Size(200, 20);
            this.siteBox.TabIndex = 7;
            // 
            // folderBox
            // 
            this.folderBox.Location = new System.Drawing.Point(101, 219);
            this.folderBox.Name = "folderBox";
            this.folderBox.Size = new System.Drawing.Size(171, 20);
            this.folderBox.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 222);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Output Location";
            // 
            // minecraftVersionBox
            // 
            this.minecraftVersionBox.FormattingEnabled = true;
            this.minecraftVersionBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.minecraftVersionBox.Items.AddRange(new object[] {
            "1.6.4",
            "1.7.2",
            "1.7.10",
            "1.8.0"});
            this.minecraftVersionBox.Location = new System.Drawing.Point(111, 13);
            this.minecraftVersionBox.Name = "minecraftVersionBox";
            this.minecraftVersionBox.Size = new System.Drawing.Size(161, 21);
            this.minecraftVersionBox.TabIndex = 10;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(278, 72);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(248, 232);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // modpackVersionBox
            // 
            this.modpackVersionBox.Location = new System.Drawing.Point(111, 40);
            this.modpackVersionBox.Name = "modpackVersionBox";
            this.modpackVersionBox.Size = new System.Drawing.Size(161, 20);
            this.modpackVersionBox.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Modpack Version:";
            // 
            // setupButton
            // 
            this.setupButton.Location = new System.Drawing.Point(278, 12);
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(248, 25);
            this.setupButton.TabIndex = 14;
            this.setupButton.Text = "Setup Testing Eniroment";
            this.setupButton.UseVisualStyleBackColor = true;
            this.setupButton.Click += new System.EventHandler(this.setupButton_Click);
            // 
            // getForgeButton
            // 
            this.getForgeButton.Location = new System.Drawing.Point(278, 43);
            this.getForgeButton.Name = "getForgeButton";
            this.getForgeButton.Size = new System.Drawing.Size(248, 23);
            this.getForgeButton.TabIndex = 15;
            this.getForgeButton.Text = "Get Minecraft Forge";
            this.getForgeButton.UseVisualStyleBackColor = true;
            this.getForgeButton.Click += new System.EventHandler(this.getForgeButton_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 312);
            this.Controls.Add(this.getForgeButton);
            this.Controls.Add(this.setupButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.modpackVersionBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.minecraftVersionBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.folderBox);
            this.Controls.Add(this.siteBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.testButtonServer);
            this.Controls.Add(this.buildButton);
            this.Controls.Add(this.testButtonClient);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Technic Modpack Creator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button testButtonClient;
        private System.Windows.Forms.Button buildButton;
        private System.Windows.Forms.Button testButtonServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox siteBox;
        private System.Windows.Forms.TextBox folderBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox minecraftVersionBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox modpackVersionBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button setupButton;
        private System.Windows.Forms.Button getForgeButton;
    }
}

