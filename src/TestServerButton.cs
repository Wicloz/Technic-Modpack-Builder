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
    class TestServerButton
    {
        private string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string cd = Directory.GetCurrentDirectory();

        public void BuildServer(string version)
        {
            //Delete old server
            if (Directory.Exists(cd + "\\tests\\ServerBuild"))
            {
                bool succes = false;

                while (!succes)
                {
                    try
                    {
                        Directory.Delete(cd + "\\tests\\ServerBuild", true);
                        succes = true;
                    }
                    catch
                    { }
                }
            }

            //Copy server_template
            foreach (string file in Directory.GetFiles(cd + "\\plugins\\server_template", "*.*", SearchOption.AllDirectories))
            {
                string newFile = file.Replace("\\plugins\\server_template", "\\tests\\ServerBuild");

                Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                File.Copy(file, newFile, true);
            }

            //Setup directory and version
            Directory.CreateDirectory(cd + "\\tests\\ServerBuild\\backups");
            File.WriteAllText(cd + "\\tests\\ServerBuild\\currentversion.dat", version);

            //Copy forgemodloader
            foreach (string file in Directory.GetFiles(cd + "\\plugins\\forgemodloader", "*forge*.jar"))
            {
                File.Copy(file, cd + "\\tests\\ServerBuild\\modpack.jar");
            }

            List<string> fileList = new List<string>();
            List<string> exceptionList = new List<string>();

            //Load exeption list
            using (FileStream fs = File.Open(cd + "\\settings\\mpexceptions.txt", FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (true)
                    {
                        string nextLine = sr.ReadLine();

                        if (nextLine != null)
                        {
                            exceptionList.Add(nextLine);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            //Determine files to copy
            foreach (string file in Directory.GetFiles(cd + "\\modpack", "*.*", SearchOption.AllDirectories))
            {
                string path = Path.GetDirectoryName(file);

                if (path.Contains("\\modpack\\config") || path.Contains("\\modpack\\coremods") || path.Contains("\\modpack\\Flan") || path.Contains("\\modpack\\mods") || path.Contains("\\modpack\\scripts"))
                {
                    bool isException = false;

                    foreach (string exception in exceptionList)
                    {
                        if (Path.GetFileName(file).Contains(exception))
                        {
                            isException = true;
                            break;
                        }
                    }

                    if (!isException)
                    {
                        fileList.Add(file);
                    }
                }
            }

            //Copy mod files
            foreach (string file in fileList)
            {
                string newFile = file.Replace("\\modpack", "\\tests\\ServerBuild");

                Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                File.Copy(file, newFile);
            }
        }

        public void DeleteBackupper()
        {
            if (File.Exists(cd + "\\tests\\ServerBuild\\BackUpper.bat"))
            {
                File.Delete(cd + "\\tests\\ServerBuild\\BackUpper.bat");
            }
        }

        public void RunServer()
        {
            Process server = new Process();

            server.StartInfo.FileName = "java.exe";
            server.StartInfo.WorkingDirectory = cd + "\\tests\\ServerBuild";
            server.StartInfo.Arguments = "-XX:PermSize=256m -jar modpack.jar nogui";

            server.EnableRaisingEvents = true;
            server.Exited += new EventHandler(server_Exited);

            server.Start();
        }


        private void server_Exited(object sender, EventArgs e)
        {
            MessageBox.Show("Server Closed");
        }

        public void DoneTesting()
        {
            //Close running server
            foreach (Process p in Process.GetProcessesByName("cmd"))
            {
                p.Kill();
            }

            //Copy server properties
            string properties1 = cd + "\\tests\\ServerBuild\\server.properties";
            string properties2 = cd + "\\plugins\\server_template\\server.properties";
            if (File.Exists(properties1))
            {
                File.Delete(properties2);
                File.Copy(properties1, properties2, true);
            }

            //Delete server folder
            bool succeed = false;
            while (!succeed)
            {
                try
                {
                    Directory.Delete(cd + "\\tests\\ServerBuild", true);
                    succeed = true;
                }
                catch
                { }
            }
        }
    }
}
