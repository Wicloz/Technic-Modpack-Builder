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
    class Proci
    {
        private static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string cd = Directory.GetCurrentDirectory();

        public static void StartLauncher()
        {
            Process launcher = new Process();

            launcher.StartInfo.FileName = cd + "\\plugins\\TechnicLauncher.exe";
            launcher.StartInfo.Arguments = "";

            launcher.Start();
            launcher.Dispose();
        }
    }
}
