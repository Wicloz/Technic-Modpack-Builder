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
        private ModManager modManager = new ModManager();

        private SettingManager settings = new SettingManager();
        private TestClientButton testClient = new TestClientButton();
        private TestServerButton testServer = new TestServerButton();
        private BuildPackButton buildPack = new BuildPackButton();
        private FileListManager fileLists = new FileListManager();

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
            includeOptionsBox.Checked = settings.includeOptionsLoad;
            fileLists.LoadLists();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            settings.SaveSettings(minecraftVersionBox.Text, modpackVersionBox.Text, siteBox.Text, folderBox.Text, includeOptionsBox.Checked);
            DeleteTempFolder();
            fileLists.SaveLists(modpackVersionBox.Text);
        }

        private void testButtonClient_Click(object sender, EventArgs e)
        {
            if (testModeA == 1)
            {
                if (PreClientCheck())
                {
                    testButtonClient.Text = "Done Testing";
                    buttonStartLauncher.Enabled = true;
                    buildButton.Enabled = false;
                    setupButton.Enabled = false;
                    manageModsButton.Enabled = false;
                    testModeA = 2;

                    MakeLowerCases();
                    MergeModpackJar();
                    fileLists.SaveLists(modpackVersionBox.Text);

                    if (!testClient.StartTesting())
                    {
                        testButtonClient.Text = "Test Modpack Client";
                        buttonStartLauncher.Enabled = false;
                        buildButton.Enabled = true;
                        setupButton.Enabled = true;
                        manageModsButton.Enabled = true;
                        testModeA = 1;
                    }
                }
            }
            else if (testModeA == 2)
            {
                testClient.DoneTesting();

                testButtonClient.Text = "Test Modpack Client";
                buttonStartLauncher.Enabled = false;
                buildButton.Enabled = true;
                setupButton.Enabled = true;
                manageModsButton.Enabled = true;
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

                    MakeLowerCases();
                    fileLists.SaveLists(modpackVersionBox.Text);

                    testServer.BuildServer(modpackVersionBox.Text);
                    testServer.DeleteBackupper();
                    //testServer.RunServer();
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

                buildPack.CreateZipFiles();
                buildPack.CopyZipFiles(folderBox.Text, modpackVersionBox.Text);

                testServer.DoneTesting();

                fileLists.SaveLists(modpackVersionBox.Text);
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

            MessageBox.Show("Do this:\n1) Select 'Vanilla' in the list on the left under 'Modpacks'.\n2) Click 'Modpack Options' (not 'Launcher Options') in the upper right corner.\n3) Select the version that matches the version of your modpack under 'A Specific Version' and click on 'Reinstall Pack'.\n4) Accept the dialog box, close the option screen and press 'Install'.\n5) After Minecraft has finished downloading, press 'Start'.\n6) After minecraft started, close it again.\n7) Done!", "Do These Things !!!");
        }

        private void OpenSite()
        {
            if (siteBox.Text != "")
            {
                Process.Start("http://www.technicpack.net/" + siteBox.Text);
            }

            if (modpackVersionBox.Text != "")
            {
                Process.Start(cd + "\\export\\versions\\" + modpackVersionBox.Text);
            }
        }

        private void MergeModpackJar()
        {
            DeleteTempFolder();

            Directory.CreateDirectory(cd + "\\temp");
            File.Delete(cd + "\\plugins\\mergedjar\\modpack.jar");

            foreach (string file in Directory.GetFiles(cd + "\\plugins\\forgemodloader", "*forge*.jar"))
            {
                File.Copy(file, cd + "\\plugins\\mergedjar\\modpack.jar");
                break;
            }

            ZipFile.ExtractToDirectory(cd + "\\modpack\\bin\\modpack.jar", cd + "\\temp\\extract");

            using (Ionic.Zip.ZipFile jar = Ionic.Zip.ZipFile.Read(cd + "\\plugins\\mergedjar\\modpack.jar"))
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

                    bool succeed = false;

                    while (!succeed)
                    {
                        try
                        {
                            jar.Save();
                            succeed = true;
                        }
                        catch
                        { }
                    }
                }
            }

            DeleteTempFolder();
        }

        private void DeleteTempFolder()
        {
            if (Directory.Exists(cd + "\\temp"))
            {
                Directory.Delete(cd + "\\temp", true);
            }
        }

        public void MakeLowerCases ()
        {
            foreach (string file in Directory.GetFiles(cd + "\\modpack\\mods", "*.zip", SearchOption.AllDirectories))
            {
                bool succes = false;

                while (!succes)
                {
                    try
                    {
                        File.Move(file, file.ToLower());
                        succes = true;
                    }
                    catch
                    { }
                }
            }

            foreach (string file in Directory.GetFiles(cd + "\\modpack\\mods", "*.jar", SearchOption.AllDirectories))
            {
                bool succes = false;

                while (!succes)
                {
                    try
                    {
                        File.Move(file, file.ToLower());
                        succes = true;
                    }
                    catch
                    { }
                }
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
            modManager.ShowDialog();
        }

        private void openModpackFolder_Click(object sender, EventArgs e)
        {
            Process.Start(appdata + "\\.technic\\modpacks\\vanilla");
        }

        private void openThisFolder_Click(object sender, EventArgs e)
        {
            Process.Start(cd);
        }

        private void buttonStartLauncher_Click(object sender, EventArgs e)
        {
            Proci.StartLauncher();
        }
    }
}
