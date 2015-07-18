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
        private string testFolder;

        public bool BuildClient()
        {
            testFolder = appdata + "\\.technic\\modpacks\\dummy-pack-1710";
            //testFolder = cd + "\\tests\\ClientBuild";

            //Delete folders
            string[] deleteFolders = {
                                        testFolder + "\\config",
                                        testFolder + "\\coremods",
                                        testFolder + "\\Flan",
                                        testFolder + "\\hats",
                                        testFolder + "\\mods",
                                        testFolder + "\\scripts",
                                        testFolder + "\\betterrecords"
                                     };

            foreach (string folder in deleteFolders)
            {
                if (Directory.Exists(folder))
                {
                    bool succes = false;

                    while (!succes)
                    {
                        try
                        {
                            Directory.Delete(folder, true);
                            succes = true;
                        }
                        catch
                        { }
                    }
                }
            }

            //Copy client_template
            if (!File.Exists(testFolder + "\\bin\\minecraft.jar"))
            {
                foreach (string file in Directory.GetFiles(cd + "\\plugins\\client_template", "*.*", SearchOption.AllDirectories))
                {
                    string newFile = file.Replace(cd + "\\plugins\\client_template", testFolder);

                    Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                    File.Delete(newFile);
                    File.Copy(file, newFile, true);
                }
            }

            //Delete modpack.jar
            if (File.Exists(testFolder + "\\bin\\modpack.jar"))
            {
                File.Delete(testFolder + "\\bin\\modpack.jar");
            }

            //Copy files
            foreach (string file in Directory.GetFiles(cd + "\\modpack", "*.*", SearchOption.AllDirectories))
            {
                if (file != cd + "\\modpack\\bin\\modpack.jar")
                {
                    string newFile = file.Replace(cd + "\\modpack", testFolder);

                    Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                    File.Delete(newFile);
                    File.Copy(file, newFile, true);
                }
            }

            //Copy modpack.jar
            try
            {
                string file = cd + "\\plugins\\mergedjar\\modpack.jar";
                string newFile = testFolder + "\\bin\\modpack.jar";
                File.Delete(newFile);
                File.Copy(file, newFile, true);
            }
            catch
            {
                return false;
            }

            //Copy potionidhelper
            foreach (string file in Directory.GetFiles(cd + "\\plugins\\potionidhelper"))
            {
                File.Copy(file, testFolder + "\\mods\\" + Path.GetFileName(file), true);
            }

            //Copy idfixminus.jar
            if (File.Exists(cd + "\\plugins\\idfixer\\idfixminus.jar"))
            {
                File.Copy(cd + "\\plugins\\idfixer\\idfixminus.jar", testFolder + "\\mods\\idfixminus.jar", true);
            }

            //Copy resourcepack
            foreach (string file in Directory.GetFiles(cd + "\\resourcepack", "*.zip", SearchOption.TopDirectoryOnly))
            {
                string newFile = file.Replace(cd + "\\resourcepack", testFolder + "\\resourcepacks");

                Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                File.Delete(newFile);
                File.Copy(file, newFile, true);
            }

            //End
            return true;
        }

        public void StartClientBuild()
        {
            Process minecraft = new Process();
            string libString = "";

            foreach (string file in Directory.GetFiles(appdata + "\\.technic\\cache", "*.jar", SearchOption.AllDirectories))
            {
                if (!file.Contains("minecraft_"))
                {
                    libString += file + ";";
                }
            }

            //minecraft.StartInfo.FileName = "java.exe";
            //minecraft.StartInfo.WorkingDirectory = cd + "\\tests\\ClientBuild";
            //minecraft.StartInfo.Arguments = "-Djava.library.path=\"bin\\natives\" -cp \""+libString+"bin\\modpack.jar;bin\\minecraft.jar\" net.minecraft.client.main.Main --username myusername --accessToken myaccesstoken --userProperties {} --version 1.7.10";

            //minecraft.Start();

            Proci.StartLauncher();
        }

        public void DoneTesting ()
        {
            //Prepare build folder
            //Directory.Delete(cd + "\\modpack\\config", true);
            Directory.CreateDirectory(cd + "\\modpack\\config");

            //Copy all configs
            foreach (string file in Directory.GetFiles(testFolder, "*.cfg", SearchOption.AllDirectories))
            {
                if (!file.Contains(testFolder + "\\asm") && !file.Contains(testFolder + "\\saves") && !file.Contains(testFolder + "\\crash-reports") && !file.Contains(testFolder + "\\backups") && !file.Contains(testFolder + "\\compendiumunlocks"))
                {
                    string newFile = file.Replace(testFolder, cd + "\\modpack");

                    Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                    File.Delete(newFile);
                    File.Copy(file, newFile, true);
                }
            }

            foreach (string file in Directory.GetFiles(testFolder, "*.config", SearchOption.AllDirectories))
            {
                if (!file.Contains(testFolder + "\\asm") && !file.Contains(testFolder + "\\saves") && !file.Contains(testFolder + "\\crash-reports") && !file.Contains(testFolder + "\\backups") && !file.Contains(testFolder + "\\compendiumunlocks"))
                {
                    string newFile = file.Replace(testFolder, cd + "\\modpack");

                    Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                    File.Delete(newFile);
                    File.Copy(file, newFile, true);
                }
            }

            foreach (string file in Directory.GetFiles(testFolder, "*.txt", SearchOption.AllDirectories))
            {
                if (!file.Contains(testFolder + "\\asm") && !file.Contains(testFolder + "\\saves") && !file.Contains(testFolder + "\\crash-reports") && !file.Contains(testFolder + "\\backups") && !file.Contains(testFolder + "\\compendiumunlocks") && !file.Contains(testFolder + "\\betterrain\\null.txt"))
                {
                    string newFile = file.Replace(testFolder, cd + "\\modpack");

                    Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                    File.Delete(newFile);
                    File.Copy(file, newFile, true);
                }
            }

            //Copy options
            if (File.Exists(testFolder + "\\options.txt"))
            {
                File.Copy(testFolder + "\\options.txt", cd + "\\modpack\\options.txt", true);
            }

            if (File.Exists(testFolder + "\\optionsof.txt"))
            {
                File.Copy(testFolder + "\\optionsof.txt", cd + "\\modpack\\optionsof.txt", true);
            }

            //Copy specific folders
            foreach (string file in Directory.GetFiles(testFolder + "\\config", "*.*", SearchOption.AllDirectories))
            {
                string newFile = file.Replace(testFolder, cd + "\\modpack");

                Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                File.Copy(file, newFile, true);
            }

            if (Directory.Exists(testFolder + "\\scripts"))
            {
                foreach (string file in Directory.GetFiles(testFolder + "\\scripts", "*.*", SearchOption.AllDirectories))
                {
                    string newFile = file.Replace(testFolder, cd + "\\modpack");

                    Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                    File.Copy(file, newFile, true);
                }
            }
        }
    }
}
