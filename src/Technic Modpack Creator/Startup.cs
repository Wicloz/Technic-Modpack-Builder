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
        private int stuffIndex = -1;
        private int downloadIndex = -1;
        private Thread stuff;

        private void EmptyFunction()
        {
        }

        public Startup()
        {
            InitializeComponent();
        }

        private void Startup_Shown(object sender, EventArgs e)
        {
            stuff = new Thread(EmptyFunction);

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 200;
            timer.Start();

            label.Text = "Creating Directory's ...";

            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 10;
        }

        private void DoStuff()
        {
            stuffIndex++;

            if(stuffIndex == 0)
            {
                stuff = new Thread(CreateFolders);
                stuff.Start();

                label.Text = "Moving Files ...";
                return;
            }

            else if(stuffIndex == 1)
            {
                stuff = new Thread(MoveFiles);
                stuff.Start();

                label.Text = "Creating Files ...";
                return;
            }

            else if(stuffIndex == 2)
            {
                stuff = new Thread(CreateFiles);
                stuff.Start();

                label.Text = "Downloading TechnicLauncher ...";
                return;
            }

            else if(stuffIndex == 3)
            {
                DownloadFile();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!stuff.IsAlive && downloadIndex < 0)
            {
                DoStuff();
            }
        }

        private void CreateFolders()
        {
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
        }

        private void CreateFiles()
        {
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
        }

        private void MoveFiles()
        {
            if (File.Exists(cd + "\\temp\\modpack.jar"))
            {
                File.Delete(cd + "\\modpack\\bin\\modpack.jar");
                File.Move(cd + "\\temp\\modpack.jar", cd + "\\modpack\\bin\\modpack.jar");
                Directory.Delete(cd + "\\temp", true);
            }
        }

        private void DownloadFile()
        {
            downloadIndex++;
            progressBar.Style = ProgressBarStyle.Blocks;

            if(downloadIndex == 0)
            {
                if (!File.Exists(cd + "\\plugins\\TechnicLauncher.exe"))
                {
                    WebClient client = new WebClient();
                    client.DownloadFileAsync(new Uri("http://launcher.technicpack.net/launcher/439/TechnicLauncher.exe"), cd + "\\plugins\\TechnicLauncher.exe");
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                }
                else
                {
                    DownloadFile();
                }

                return;
            }

            else if(downloadIndex == 1)
            {
                label.Text = "Downloading Server Files ...";

                if (!File.Exists(cd + "\\plugins\\server_template\\RUN.bat"))
                {
                    Directory.Delete(cd + "\\plugins\\server_template", true);
                    Directory.CreateDirectory(cd + "\\plugins\\server_template");
                    Directory.CreateDirectory(cd + "\\temp");

                    WebClient client = new WebClient();
                    client.DownloadFileAsync(new Uri("https://dl.dropboxusercontent.com/u/46484032/TMC/ServerTemplate.zip"), cd + "\\temp\\server.zip");
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                }
                else
                {
                    DownloadFile();
                }

                return;
            }

            else
            {
                EndStartup();
            }
        }

        private void ProcessFile()
        {
            if (downloadIndex == 0)
            {
            }

            else if (downloadIndex == 1)
            {
                ZipFile.ExtractToDirectory(cd + "\\temp\\server.zip", cd + "\\plugins\\server_template");
                Directory.Delete(cd + "\\temp", true);
            }

            progressBar.Value = 0;
            DownloadFile();
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ProcessFile();
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
