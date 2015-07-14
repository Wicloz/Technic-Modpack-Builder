using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Ionic.Zip;

namespace Technic_Modpack_Creator
{
    class BuildPackButton
    {
        private string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string cd = Directory.GetCurrentDirectory();

        private string zipDirectory;

        public BuildPackButton()
        {
            zipDirectory = cd + "\\export\\zipfiles";
        }

        public void CreateZipFiles()
        {
            //Delete old zipfiles
            if (Directory.Exists(zipDirectory))
            {
                Directory.Delete(zipDirectory, true);
            }
            Directory.CreateDirectory(zipDirectory);

            //Create empty zipfiles
            using (ZipOutputStream zos = new ZipOutputStream(zipDirectory + "\\ClientFiles.zip"))
            { }
            using (ZipOutputStream zos = new ZipOutputStream(zipDirectory + "\\ServerFiles.zip"))
            { }

            //Create list of resourcepacks
            List<string> rpackList = new List<string>();
            using (FileStream fs = File.Open(cd + "\\resourcepack\\distribute.txt", FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (true)
                    {
                        string nextLine = sr.ReadLine();

                        if (nextLine != null)
                        {
                            rpackList.Add(nextLine);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            //Create client zipfile
            using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(zipDirectory + "\\ClientFiles.zip"))
            {
                //Add modpack folders
                foreach (string folder in Directory.GetDirectories(cd + "\\modpack", "*", SearchOption.AllDirectories))
                {
                    if (zip.ContainsEntry(folder))
                    {
                        zip.RemoveEntry(folder);
                    }
                    zip.AddDirectoryByName(folder.Replace(cd + "\\modpack", ""));
                }
                //Add resourcepacks
                foreach (string file in Directory.GetFiles(cd + "\\resourcepack", "*.zip", SearchOption.TopDirectoryOnly))
                {
                    if (rpackList.Contains(Path.GetFileName(file)))
                    {
                        if (zip.ContainsEntry(file))
                        {
                            zip.RemoveEntry(file);
                        }
                        zip.AddFile(file, file.Replace(Path.GetFileName(file), "").Replace(cd + "\\resourcepack", "resourcepacks"));
                    }
                }
                //Add modpack files
                foreach (string file in Directory.GetFiles(cd + "\\modpack", "*.*", SearchOption.AllDirectories))
                {
                    if ((Main.acces.includeOptionsBox.Checked || (!file.Contains("\\modpack\\options.txt") && !file.Contains("\\modpack\\optionsof.txt"))) && file != cd + "\\modpack\\bin\\modpack.jar")
                    {
                        if (zip.ContainsEntry(file))
                        {
                            zip.RemoveEntry(file);
                        }
                        zip.AddFile(file, file.Replace(Path.GetFileName(file), "").Replace(cd + "\\modpack", ""));
                    }
                }

                //Add modpack.jar
                try
                {
                    string file = cd + "\\plugins\\mergedjar\\modpack.jar";
                    string zipFolder = "bin";

                    if (zip.ContainsEntry(file))
                    {
                        zip.RemoveEntry(file);
                    }
                    zip.AddFile(file, zipFolder);
                }
                catch
                { }
                //Add idfixminus.jar
                if (File.Exists(cd + "\\plugins\\idfixer\\idfixminus.jar"))
                {
                    string file = cd + "\\plugins\\idfixer\\idfixminus.jar";
                    string zipFolder = "mods";

                    if (zip.ContainsEntry(file))
                    {
                        zip.RemoveEntry(file);
                    }
                    zip.AddFile(file, zipFolder);
                }

                //Save zipfile
                bool succeed = false;
                while (!succeed)
                {
                    try
                    {
                        zip.Save();
                        succeed = true;
                    }
                    catch
                    { }
                }
            }

            //Create server zipfile
            using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(zipDirectory + "\\ServerFiles.zip"))
            {
                //Add server folders
                foreach (string folder in Directory.GetDirectories(cd + "\\tests\\ServerBuild", "*", SearchOption.AllDirectories))
                {
                    if (zip.ContainsEntry(folder))
                    {
                        zip.RemoveEntry(folder);
                    }
                    zip.AddDirectoryByName(folder.Replace(cd + "\\tests\\ServerBuild", ""));
                }
                //Add server files
                foreach (string file in Directory.GetFiles(cd + "\\tests\\ServerBuild", "*.*", SearchOption.AllDirectories))
                {
                    if (zip.ContainsEntry(file))
                    {
                        zip.RemoveEntry(file);
                    }
                    zip.AddFile(file, file.Replace(Path.GetFileName(file), "").Replace(cd + "\\tests\\ServerBuild", ""));
                }

                //Save zipfile
                bool succeed = false;
                while (!succeed)
                {
                    try
                    {
                        zip.Save();
                        succeed = true;
                    }
                    catch
                    { }
                }
            }
        }

        public void CopyZipFiles(string outputFolder, string version)
        {
            Directory.CreateDirectory(outputFolder);

            string clientFiles = outputFolder + "\\ClientFiles.zip";
            string serverFiles = outputFolder + "\\ServerFiles.zip";

            if (File.Exists(clientFiles))
            {
                File.Delete(clientFiles);
            }
            if (File.Exists(serverFiles))
            {
                File.Delete(serverFiles);
            }

            bool succeed = false;

            while (!succeed)
            {
                try
                {
                    File.Copy(zipDirectory + "\\ClientFiles.zip", clientFiles);
                    File.Copy(zipDirectory + "\\ServerFiles.zip", serverFiles);
                    succeed = true;
                }
                catch
                { }
            }

            File.WriteAllText(outputFolder + "\\version.dat", version);
        }
    }
}
