using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.IO.Compression;

namespace Technic_Modpack_Creator
{
    public partial class Startup : Form
    {
        private string cd = Directory.GetCurrentDirectory();

        public Startup()
        {
            InitializeComponent();
        }

        private void Startup_Shown(object sender, EventArgs e)
        {
            label.Text = "Creating Directory's ...";

            Directory.CreateDirectory(cd + "\\modpack\\bin");
            Directory.CreateDirectory(cd + "\\modpack\\config");
            Directory.CreateDirectory(cd + "\\modpack\\coremods");
            Directory.CreateDirectory(cd + "\\modpack\\Flan");
            Directory.CreateDirectory(cd + "\\modpack\\hats");
            Directory.CreateDirectory(cd + "\\modpack\\mods");

            Directory.CreateDirectory(cd + "\\plugins\\forgemodloader");
            Directory.CreateDirectory(cd + "\\plugins\\idfixer");
            Directory.CreateDirectory(cd + "\\plugins\\server_template");

            Directory.CreateDirectory(cd + "\\export\\texturepack");
            Directory.CreateDirectory(cd + "\\export\\modlist");
            Directory.CreateDirectory(cd + "\\export\\icons");

            Directory.CreateDirectory(cd + "\\settings");
            Directory.CreateDirectory(cd + "\\tests\\ClientBuild");

            label.Text = "Moving Files ...";

            if (File.Exists(cd + "\\temp\\modpack.jar"))
            {
                File.Delete(cd + "\\modpack\\bin\\modpack.jar");
                File.Move(cd + "\\temp\\modpack.jar", cd + "\\modpack\\bin\\modpack.jar");
                Directory.Delete(cd + "\\temp", true);
            }

            label.Text = "Creating Files ...";

            if (!File.Exists(cd + "\\settings\\mpexceptions.txt"))
            {
                File.WriteAllText(cd + "\\settings\\mpexceptions.txt", "damageindicator" + "\n" + "bacr" + "\n" + "neiplugins" + "\n" + "minimap" + "\n" + "inventorytweaks" + "\n" + "animationapi" + "\n" + "mapwriter" + "\n" + "dynamiclights" + "\n" + "shatter");
            }

            if (!File.Exists(cd + "\\modpack\\bin\\modpack.jar"))
            {
                string readme = "README:";

                Directory.CreateDirectory(cd + "\\temp");
                File.WriteAllText(cd + "\\temp\\README.txt", readme);

                ZipFile.CreateFromDirectory(cd + "\\temp", cd + "\\modpack\\bin\\modpack.jar");

                Directory.Delete(cd + "\\temp", true);
            }

            DownloadFiles();
        }

        private void DownloadFiles()
        {
            label.Text = "Downloading TechnicLauncher ...";

            if (!File.Exists(cd + "\\plugins\\TechnicLauncher.exe"))
            {
                WebClient client = new WebClient();
                client.DownloadFile(new Uri("http://launcher.technicpack.net/launcher/439/TechnicLauncher.exe"), cd + "\\plugins\\TechnicLauncher.exe");
            }

            label.Text = "Downloading Server Files ...";

            if (!File.Exists(cd + "\\plugins\\server_template\\RUN.bat"))
            {
                Directory.Delete(cd + "\\plugins\\server_template", true);
                Directory.CreateDirectory(cd + "\\plugins\\server_template");
                Directory.CreateDirectory(cd + "\\temp");

                WebClient client = new WebClient();
                client.DownloadFileAsync(new Uri("https://dl.dropboxusercontent.com/u/46484032/TMC/ServerTemplate.zip"), cd + "\\temp\\server.zip");
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client2_DownloadFileCompleted);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client2_DownloadProgressChanged);
            }
            else
            {
                EndStartup();
            }
        }

        void client2_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        void client2_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ZipFile.ExtractToDirectory(cd + "\\temp\\server.zip", cd + "\\plugins\\server_template");
            Directory.Delete(cd + "\\temp", true);

            EndStartup();
        }

        void EndStartup()
        {
            if (Directory.Exists(cd + "\\temp"))
            {
                Directory.Delete(cd + "\\temp", true);
            }

            this.Close();
        }
    }
}
