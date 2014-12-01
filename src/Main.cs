using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.IO.Compression;
using System.Diagnostics;
using Microsoft.Win32;

namespace Technic_Modpack_Creator
{
    public partial class Main : Form
    {
        public static Main acces;

        private SettingManager settings = new SettingManager();
        private TestClientButton testClient = new TestClientButton();
        private TestServerButton testServer = new TestServerButton();
        private BuildPackButton buildPack = new BuildPackButton();

        private string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string cd = Directory.GetCurrentDirectory();

        private int testModeA = 1;
        private int testModeB = 1;

        public Main()
        {
            InitializeComponent();
            acces = this;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            minecraftVersionBox.Text = settings.minecraftVersionLoad;
            modpackVersionBox.Text = settings.modpackVersionLoad;
            siteBox.Text = settings.nameLoad;
            folderBox.Text = settings.locationLoad;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            settings.SaveSettings(minecraftVersionBox.Text, modpackVersionBox.Text, siteBox.Text, folderBox.Text);
            RestoreModpackJar();

            if (Directory.Exists(cd + "\\temp"))
            {
                Directory.Delete(cd + "\\temp", true);
            }
        }

        private void testButtonClient_Click(object sender, EventArgs e)
        {
            if (testModeA == 1)
            {
                if (PreClientCheck())
                {
                    testButtonClient.Text = "Done Testing";
                    buildButton.Enabled = false;
                    setupButton.Enabled = false;
                    testModeA = 2;

                    MakeLowerCases();
                    //MergeModpackJar();

                    testClient.StartTesting();
                }
            }
            else if (testModeA == 2)
            {
                RestoreModpackJar();
                testClient.DoneTesting();

                testButtonClient.Text = "Test Modpack Client";
                buildButton.Enabled = true;
                setupButton.Enabled = true;
                testModeA = 1;
            }
        }

        private void testButtonServer_Click(object sender, EventArgs e)
        {
            if (testModeB == 1)
            {
                if (PreServerCheck())
                {
                    testButtonServer.Text = "Done Testing";
                    buildButton.Enabled = false;
                    testModeB = 2;

                    testServer.BuildServer(modpackVersionBox.Text);
                    testServer.RunServer();
                }
            }
            else if (testModeB == 2)
            {
                testServer.DoneTesting();

                testButtonServer.Text = "Test Modpack Server";
                buildButton.Enabled = true;
                testModeB = 1;
            }
        }

        private void buildButton_Click(object sender, EventArgs e)
        {
            if (PreClientCheck() && PreServerCheck())
            {
                MakeLowerCases();
                MergeModpackJar();
                testServer.BuildServer(modpackVersionBox.Text);

                MakeModList();
                buildPack.CreateZipFiles();
                buildPack.CopyZipFiles(folderBox.Text, modpackVersionBox.Text);

                RestoreModpackJar();
                testServer.DoneTesting();

                OpenSite();
            }
        }

        private void setupButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(appdata + "\\.technic\\modpacks\\vanilla"))
            {
                Directory.Delete(appdata + "\\.technic\\modpacks\\vanilla", true);
            }

            Process launcher = new Process();
            launcher.StartInfo.FileName = cd + "\\plugins\\TechnicLauncher.exe";
            launcher.StartInfo.Arguments = "";
            launcher.Start();

            MessageBox.Show("Do this:\n1) Select 'Minecraft'.\n2) Click the cog under the picture.\n3) Select the build that matches the version of your modpack and click save.\n4) Close the option screen and press play.\n5) After Minecraft has finished downloading and started, close it.", "Do These Things !!!");
        }

        private void OpenSite()
        {
            if (siteBox.Text != "")
            {
                Process.Start("http://www.technicpack.net/modpack/details/" + siteBox.Text + "/edit");
            }
        }

        private void MakeModList()
        {
            string[] jarFiles = Directory.GetFiles(cd + "\\modpack", "*.jar", SearchOption.AllDirectories);
            string[] zipFiles = Directory.GetFiles(cd + "\\modpack", "*.zip", SearchOption.AllDirectories);
            List<string> allFiles = new List<string>();

            foreach (string file in jarFiles)
            {
                allFiles.Add(file);
            }
            foreach (string file in zipFiles)
            {
                allFiles.Add(file);
            }

            string[] content = new string[allFiles.Count];
            for (int i = 0; i < allFiles.Count; i++)
            {
                content[i] = Path.GetFileName(allFiles[i]);
            }

            File.WriteAllLines(cd + "\\export\\modlist\\Modlist.txt", content);
        }

        private void MergeModpackJar()
        {
            Directory.CreateDirectory(cd + "\\temp");
            File.Move(cd + "\\modpack\\bin\\modpack.jar", cd + "\\temp\\modpack.jar");

            foreach (string file in Directory.GetFiles(cd + "\\plugins\\forgemodloader", "*forge*.jar"))
            {
                File.Copy(file, cd + "\\modpack\\bin\\modpack.jar");
            }

            ZipFile.ExtractToDirectory(cd + "\\temp\\modpack.jar", cd + "\\temp\\extract");

            using (Ionic.Zip.ZipFile jar = Ionic.Zip.ZipFile.Read(cd + "\\modpack\\bin\\modpack.jar"))
            {
                if (Directory.GetFiles(cd + "\\temp\\extract", "*.*", SearchOption.AllDirectories).Length > 0)
                {
                    foreach (string file in Directory.GetFiles(cd + "\\temp\\extract", "*.*", SearchOption.AllDirectories))
                    {
                        if (jar.ContainsEntry(file))
                        {
                            jar.RemoveEntry(file);
                        }
                        jar.AddFile(file, file.Replace(Path.GetFileName(file), "").Replace(cd + "\\temp\\extract", ""));
                    }

                    jar.Save();
                }
            }
        }

        private void RestoreModpackJar()
        {
            if (File.Exists(cd + "\\temp\\modpack.jar"))
            {
                File.Delete(cd + "\\modpack\\bin\\modpack.jar");
                File.Move(cd + "\\temp\\modpack.jar", cd + "\\modpack\\bin\\modpack.jar");
                Directory.Delete(cd + "\\temp", true);
            }
        }

        private void MakeLowerCases ()
        {
            foreach (string file in Directory.GetFiles(cd + "\\modpack\\mods", "*.zip", SearchOption.AllDirectories))
            {
                File.Move(file, file.ToLower());
            }

            foreach (string file in Directory.GetFiles(cd + "\\modpack\\mods", "*.jar", SearchOption.AllDirectories))
            {
                File.Move(file, file.ToLower());
            }
        }

        private bool PreClientCheck ()
        {
            if (Directory.GetFiles(cd + "\\plugins\\forgemodloader", "*forge*.jar").Length <= 0)
            {
                MessageBox.Show("Download a version of Minecraft Forge first!", "ERROR!");
                return false;
            }

            return true;
        }

        private bool PreServerCheck()
        {
            if (Directory.GetFiles(cd + "\\plugins\\server_template", "minecraft_server*.jar").Length <= 0)
            {
                MessageBox.Show("Download the minecraft_server.jar first!", "ERROR!");
                return false;
            }

            return true;
        }

        private void getForgeButton_Click(object sender, EventArgs e)
        {
            switch (minecraftVersionBox.Text)
            {
                case "1.8.1":
                    Process.Start("http://files.minecraftforge.net/minecraftforge//1.8");
                    break;

                case "1.8.0":
                     Process.Start("http://files.minecraftforge.net/minecraftforge//1.8");
                    break;

                default:
                    Process.Start("http://files.minecraftforge.net/");
                    break;
            }
        }

        private void getServerButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (minecraftVersionBox.Text.Substring(1, 1) == "." && minecraftVersionBox.Text.Substring(3, 1) == ".")
                {
                    Process.Start("https://s3.amazonaws.com/Minecraft.Download/versions/" + minecraftVersionBox.Text + "/minecraft_server." + minecraftVersionBox.Text + ".jar");
                }
                else
                {
                    Process.Start("https://mcversions.net/");
                }
            }
            catch
            {
                Process.Start("https://mcversions.net/");
            }
        }

        private void manageModsButton_Click(object sender, EventArgs e)
        {

        }

        private void openModpackFolder_Click(object sender, EventArgs e)
        {
            Process.Start(appdata + "\\.technic\\modpacks\\vanilla");
        }

        private void openThisFolder_Click(object sender, EventArgs e)
        {
            Process.Start(cd);
        }
    }
}
