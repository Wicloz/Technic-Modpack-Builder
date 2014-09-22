using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.IO.Compression;

namespace Technic_Modpack_Creator
{
    class BuildPackButton
    {
        private string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string cd = Directory.GetCurrentDirectory();

        private string zipDirectory;

        public BuildPackButton()
        {
            zipDirectory = cd + "\\export\\ZipFiles";
        }

        public void CreateZipFiles()
        {
            if (Directory.Exists(zipDirectory))
            {
                Directory.Delete(zipDirectory, true);
            }
            Directory.CreateDirectory(zipDirectory);

            ZipFile.CreateFromDirectory(cd + "\\modpack", zipDirectory + "\\ClientFiles.zip");
            ZipFile.CreateFromDirectory(cd + "\\tests\\ServerBuild", zipDirectory + "\\ServerFiles.zip");
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

            File.Copy(zipDirectory + "\\ClientFiles.zip", clientFiles);
            File.Copy(zipDirectory + "\\ServerFiles.zip", serverFiles);

            File.WriteAllText(outputFolder + "\\version.dat", version);
        }
    }
}
