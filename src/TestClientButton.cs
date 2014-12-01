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

        public void StartTesting()
        {
            string[] deleteFolders = {
                                        "\\.technic\\modpacks\\vanilla\\config",
                                        "\\.technic\\modpacks\\vanilla\\coremods",
                                        "\\.technic\\modpacks\\vanilla\\Flan",
                                        "\\.technic\\modpacks\\vanilla\\hats",
                                        "\\.technic\\modpacks\\vanilla\\mods"
                                     };

            foreach (string folder in deleteFolders)
            {
                if (Directory.Exists(appdata + folder))
                {
                    Directory.Delete(appdata + folder, true);
                }
            }

            foreach (string file in Directory.GetFiles(cd + "\\modpack", "*.*", SearchOption.AllDirectories))
            {
                string newFile = file.Replace(cd + "\\modpack", appdata + "\\.technic\\modpacks\\vanilla");

                Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                File.Delete(newFile);
                File.Copy(file, newFile, true);
            }

            if (File.Exists(cd + "\\plugins\\idfixer\\idfixminus.jar"))
            {
                File.Copy(cd + "\\plugins\\idfixer\\idfixminus.jar", appdata + "\\.technic\\modpacks\\vanilla\\mods\\idfixminus.jar", true);
            }

            Process launcher = new Process();

            launcher.StartInfo.FileName = cd + "\\plugins\\TechnicLauncher.exe";
            launcher.StartInfo.Arguments = "";

            launcher.EnableRaisingEvents = true;
            launcher.Exited += new EventHandler(techniclauncher_Exited);

            launcher.Start();
        }

        public void DoneTesting ()
        {
            Directory.Delete(cd + "\\modpack\\config", true);
            Directory.CreateDirectory(cd + "\\modpack\\config");

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

            foreach (string file in Directory.GetFiles(appdata + "\\.technic\\modpacks\\vanilla\\mods", "*.cfg", SearchOption.AllDirectories))
            {
                string newFile = file.Replace(appdata + "\\.technic\\modpacks\\vanilla", cd + "\\modpack");

                Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                File.Delete(newFile);
                File.Copy(file, newFile, true);
            }
        }

        private void techniclauncher_Exited(object sender, EventArgs e)
        {
            MessageBox.Show("Client Closed");
        }
    }
}
