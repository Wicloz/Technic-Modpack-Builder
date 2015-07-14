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
        private bool finished = false;
        private bool taskDone = true;
        private int pbValue = 0;

        private string[] labelText = {
                                        "Creating Directory's ...",
                                        "Cleaning Up ...",
                                        "Creating Files ...",
                                        "Downloading TechnicLauncher ...",
                                        "Downloading Server Files ...",
                                        "Downloading Server Files ...",
                                        "Downloading Client Files ...",
                                        "Downloading Client Files ...",
                                        "Downloading Client Files ..."
                                     };

        private ProgressBarStyle[] pbStyle = {
                                                ProgressBarStyle.Marquee,
                                                ProgressBarStyle.Marquee,
                                                ProgressBarStyle.Marquee,
                                                ProgressBarStyle.Continuous,
                                                ProgressBarStyle.Continuous,
                                                ProgressBarStyle.Continuous,
                                                ProgressBarStyle.Continuous,
                                                ProgressBarStyle.Continuous,
                                                ProgressBarStyle.Continuous
                                             };

        private Thread stuff;

        private void EmptyFunction()
        { }

        public Startup()
        {
            InitializeComponent();
        }

        private void Startup_Shown(object sender, EventArgs e)
        {
            stuff = new Thread(EmptyFunction);

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 20;
            timer.Start();

            if (File.Exists(cd + "\\temp\\modpack.jar"))
            {
                File.Delete(cd + "\\modpack\\bin\\modpack.jar");
                File.Move(cd + "\\temp\\modpack.jar", cd + "\\modpack\\bin\\modpack.jar");
            }

            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 10;
        }

        private void DoStuff()
        {
            stuffIndex++;

            if (stuffIndex == 0)
            {
                stuff = new Thread(CreateFolders);
                stuff.Start();

                return;
            }

            else if (stuffIndex == 1)
            {
                stuff = new Thread(Cleanup);
                stuff.Start();

                return;
            }

            else if (stuffIndex == 2)
            {
                stuff = new Thread(CreateFiles);
                stuff.Start();

                return;
            }

            else if (stuffIndex <= 8)
            {
                stuff = new Thread(DownloadFile);
                stuff.Start();

                return;
            }

            else
            {
                EndStartup();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (stuffIndex >= 0 && stuffIndex <= 8)
            {
                label.Text = labelText[stuffIndex];
                progressBar.Style = pbStyle[stuffIndex];
            }

            progressBar.Value = pbValue;

            if (taskDone)
            {
                taskDone = false;
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
            Directory.CreateDirectory(cd + "\\modpack\\scripts");

            Directory.CreateDirectory(cd + "\\plugins\\forgemodloader");
            Directory.CreateDirectory(cd + "\\plugins\\idfixer");
            Directory.CreateDirectory(cd + "\\plugins\\server_template");
            Directory.CreateDirectory(cd + "\\plugins\\client_template");
            Directory.CreateDirectory(cd + "\\plugins\\mergedjar");

            Directory.CreateDirectory(cd + "\\export\\versions");
            Directory.CreateDirectory(cd + "\\export\\icons");
            Directory.CreateDirectory(cd + "\\export\\zipfiles");

            Directory.CreateDirectory(cd + "\\settings");
            Directory.CreateDirectory(cd + "\\tests\\ClientBuild");

            Directory.CreateDirectory(cd + "\\resourcepack\\addons");

            taskDone = true;
        }

        private void CreateFiles()
        {
            if (!File.Exists(cd + "\\settings\\mpexceptions.txt"))
            {
                File.WriteAllText(cd + "\\settings\\mpexceptions.txt", "damageindicator" + "\n" + "bacr" + "\n" + "neiaddons" + "\n" + "minimap" + "\n" + "inventorytweaks" + "\n" + "mapwriter" + "\n" + "dynamiclights" + "\n" + "shatter" + "\n" + "bettertitlescreen" + "\n" + "journeymap" + "\n" + "notenoughkeys" + "\n" + "optifine" + "\n" + "java7checker");
            }

            if (!File.Exists(cd + "\\modpack\\bin\\modpack.jar"))
            {
                string readme = "README:";

                Directory.CreateDirectory(cd + "\\temp");
                File.WriteAllText(cd + "\\temp\\README.txt", readme);

                ZipFile.CreateFromDirectory(cd + "\\temp", cd + "\\modpack\\bin\\modpack.jar");

                Directory.Delete(cd + "\\temp", true);
            }

            if (!File.Exists(cd + "\\resourcepack\\distribute.txt"))
            {
                File.WriteAllText(cd + "\\resourcepack\\distribute.txt", "");
            }

            taskDone = true;
        }

        private void Cleanup()
        {
            if (Directory.Exists(cd + "\\temp"))
            {
                Directory.Delete(cd + "\\temp", true);
            }

            if (Directory.Exists(cd + "\\downloads") && Directory.GetDirectories(cd + "\\downloads").Length == 0)
            {
                Directory.Delete(cd + "\\downloads", true);
            }

            taskDone = true;
        }

        private void DownloadFile()
        {
            SettingManager settings = new SettingManager();
            string version = settings.minecraftVersionLoad;

            if (stuffIndex == 3)
            {
                if (!File.Exists(cd + "\\plugins\\TechnicLauncher.exe"))
                {
                    WebClient client = new WebClient();
                    client.DownloadFileAsync(new Uri("http://launcher.technicpack.net/launcher4/308/TechnicLauncher.exe"), cd + "\\plugins\\TechnicLauncher.exe");
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                }
                else
                {
                    taskDone = true;
                }

                return;
            }

            else if (stuffIndex == 4)
            {
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
                    taskDone = true;
                }

                return;
            }

            else if (stuffIndex == 5)
            {
                if (version != "" && !File.Exists(cd + "\\plugins\\server_template\\minecraft_server." + version + ".jar"))
                {
                    WebClient client = new WebClient();
                    client.DownloadFileAsync(new Uri("https://s3.amazonaws.com/Minecraft.Download/versions/" + version + "/minecraft_server." + version + ".jar"), cd + "\\plugins\\server_template\\minecraft_server." + version + ".jar");
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                }
                else
                {
                    taskDone = true;
                }

                return;
            }

            else if (stuffIndex == 6)
            {
                if (!Directory.Exists(cd + "\\plugins\\client_template\\bin\\natives"))
                {
                    Directory.CreateDirectory(cd + "\\temp");
                    WebClient client = new WebClient();
                    client.DownloadFileAsync(new Uri("https://dl.dropboxusercontent.com/u/46484032/TMC/ClientTemplate.zip"), cd + "\\temp\\client.zip");
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                }
                else
                {
                    taskDone = true;
                }

                return;
            }

            else if (stuffIndex == 7)
            {
                if (version != "" && !File.Exists(cd + "\\plugins\\client_template\\bin\\minecraft.jar"))
                {
                    WebClient client = new WebClient();
                    client.DownloadFileAsync(new Uri("https://s3.amazonaws.com/Minecraft.Download/versions/" + version + "/" + version + ".jar"), cd + "\\plugins\\client_template\\bin\\minecraft.jar");
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                }
                else
                {
                    taskDone = true;
                }

                return;
            }

            else if (stuffIndex == 8)
            {
                if (version != "" && !File.Exists(cd + "\\plugins\\client_template\\bin\\" + version + ".json"))
                {
                    WebClient client = new WebClient();
                    client.DownloadFileAsync(new Uri("https://s3.amazonaws.com/Minecraft.Download/versions/" + version + "/" + version + ".json"), cd + "\\plugins\\client_template\\bin\\" + version + ".json");
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                }
                else
                {
                    taskDone = true;
                }

                return;
            }
        }

        private void ProcessFile()
        {
            if (stuffIndex == 4)
            {
                ZipFile.ExtractToDirectory(cd + "\\temp\\server.zip", cd + "\\plugins\\server_template");
                Directory.Delete(cd + "\\temp", true);
            }

            if (stuffIndex == 6)
            {
                ZipFile.ExtractToDirectory(cd + "\\temp\\client.zip", cd + "\\plugins\\client_template");
                Directory.Delete(cd + "\\temp", true);
            }

            pbValue = 0;
            taskDone = true;
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            pbValue = e.ProgressPercentage;
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

            finished = true;
            this.Close();
        }

        private void Startup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!finished)
            {
                Application.Exit();
            }
        }
    }
}
