using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.IO.Compression;
using System.Diagnostics;
using System.Windows.Forms;

namespace Technic_Modpack_Creator
{
    class TestClientButton
    {
        private string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string cd = Directory.GetCurrentDirectory();

        public bool StartTesting()
        {
            string[] deleteFolders = {
                                        "\\.technic\\modpacks\\vanilla\\config",
                                        "\\.technic\\modpacks\\vanilla\\coremods",
                                        "\\.technic\\modpacks\\vanilla\\Flan",
                                        "\\.technic\\modpacks\\vanilla\\hats",
                                        "\\.technic\\modpacks\\vanilla\\mods",
                                        "\\.technic\\modpacks\\vanilla\\journeymap"
                                     };

            foreach (string folder in deleteFolders)
            {
                if (Directory.Exists(appdata + folder))
                {
                    bool succes = false;

                    while (!succes)
                    {
                        try
                        {
                            Directory.Delete(appdata + folder, true);
                            succes = true;
                        }
                        catch
                        { }
                    }
                }
            }

            if (File.Exists(appdata + "\\.technic\\modpacks\\vanilla\\bin\\modpack.jar"))
            {
                File.Delete(appdata + "\\.technic\\modpacks\\vanilla\\bin\\modpack.jar");
            }

            foreach (string file in Directory.GetFiles(cd + "\\modpack", "*.*", SearchOption.AllDirectories))
            {
                if (file != cd + "\\modpack\\bin\\modpack.jar")
                {
                    string newFile = file.Replace(cd + "\\modpack", appdata + "\\.technic\\modpacks\\vanilla");

                    Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                    File.Delete(newFile);
                    File.Copy(file, newFile, true);
                }
            }

            try
            {
                string file = cd + "\\plugins\\mergedjar\\modpack.jar";
                string newFile = appdata + "\\.technic\\modpacks\\vanilla\\bin\\modpack.jar";
                File.Copy(file, newFile, true);
            }
            catch
            {
                return false;
            }

            if (File.Exists(cd + "\\plugins\\idfixer\\idfixminus.jar"))
            {
                File.Copy(cd + "\\plugins\\idfixer\\idfixminus.jar", appdata + "\\.technic\\modpacks\\vanilla\\mods\\idfixminus.jar", true);
            }

            foreach (string file in Directory.GetFiles(cd + "\\resourcepack", "*.zip", SearchOption.TopDirectoryOnly))
            {
                string newFile = file.Replace(cd + "\\resourcepack", appdata + "\\.technic\\modpacks\\vanilla\\resourcepacks");

                Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                File.Delete(newFile);
                File.Copy(file, newFile, true);
            }

            Process launcher = new Process();

            launcher.StartInfo.FileName = cd + "\\plugins\\TechnicLauncher.exe";
            launcher.StartInfo.Arguments = "";

            launcher.EnableRaisingEvents = true;
            launcher.Exited += new EventHandler(techniclauncher_Exited);

            launcher.Start();
            return true;
        }

        public void DoneTesting ()
        {
            Directory.Delete(cd + "\\modpack\\config", true);
            Directory.CreateDirectory(cd + "\\modpack\\config");

            foreach (string file in Directory.GetFiles(appdata + "\\.technic\\modpacks\\vanilla", "*.cfg", SearchOption.AllDirectories))
            {
                if (!file.Contains("\\modpacks\\vanilla\\asm") && !file.Contains("\\modpacks\\vanilla\\saves") && !file.Contains("\\modpacks\\vanilla\\crash-reports") && !file.Contains("\\modpacks\\vanilla\\backups") && !file.Contains("\\modpacks\\vanilla\\compendiumunlocks"))
                {
                    string newFile = file.Replace(appdata + "\\.technic\\modpacks\\vanilla", cd + "\\modpack");

                    Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                    File.Delete(newFile);
                    File.Copy(file, newFile, true);
                }
            }

            foreach (string file in Directory.GetFiles(appdata + "\\.technic\\modpacks\\vanilla", "*.txt", SearchOption.AllDirectories))
            {
                if (!file.Contains("\\modpacks\\vanilla\\asm") && !file.Contains("\\modpacks\\vanilla\\saves") && !file.Contains("\\modpacks\\vanilla\\crash-reports") && !file.Contains("\\modpacks\\vanilla\\backups") && !file.Contains("\\modpacks\\vanilla\\compendiumunlocks"))
                {
                    string newFile = file.Replace(appdata + "\\.technic\\modpacks\\vanilla", cd + "\\modpack");

                    Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                    File.Delete(newFile);
                    File.Copy(file, newFile, true);
                }
            }

            if (File.Exists(appdata + "\\.technic\\modpacks\\vanilla\\options.txt"))
            {
                File.Copy(appdata + "\\.technic\\modpacks\\vanilla\\options.txt", cd + "\\modpack\\options.txt", true);
            }

            foreach (string file in Directory.GetFiles(appdata + "\\.technic\\modpacks\\vanilla\\config", "*.*", SearchOption.AllDirectories))
            {
                string newFile = file.Replace(appdata + "\\.technic\\modpacks\\vanilla", cd + "\\modpack");

                Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                File.Copy(file, newFile, true);
            }
        }

        private void techniclauncher_Exited(object sender, EventArgs e)
        {
            //MessageBox.Show("Client Closed");
        }
    }
}
