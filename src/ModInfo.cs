﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.IO.Compression;

namespace Technic_Modpack_Creator
{
    [Serializable]
    class ModInfo : System.IComparable<ModInfo>
    {
        private string cd = Directory.GetCurrentDirectory();
        public string modFilename = "";

        public string versionLocalRaw = "";
        public string versionLatestRaw = "";
        public string versionLocal = "N/A";
        public string versionLatest = "N/A";
        public string newFileName = "";

        public string website = "NONE";
        public string siteMode = "NONE";
        public string dlSite = "NONE";

        // Dowload + Check info
        public int progress = 0;
        public bool checkQueued = false;
        public bool checkDone = false;
        public bool downloadQueued = false;
        public bool downloadDone = false;

        public string uriState
        {
            get
            {
                if (website == "NONE")
                {
                    return "No Link";
                }
                else if (dlSite != "NONE")
                {
                    return "Automatic";
                }
                else if (siteMode != "NONE")
                {
                    return "Manual";
                }
                else
                {
                    return "Unsupported";
                }
            }
        }

        public string updateState
        {
            get
            {
                if (checkQueued)
                {
                    return "Checking for Update ...";
                }
                else if (downloadQueued && progress != 0)
                {
                    return "Downloading: " + progress + "%";
                }
                else if (downloadQueued)
                {
                    return "Awaiting Download ...";
                }
                else if (versionLatestRaw == versionLocalRaw)
                {
                    return "";
                }
                else
                {
                    return "Update Available";
                }
            }
        }

        public ModInfo()
        {}

        public ModInfo(string fileName)
        {
            modFilename = fileName;
        }

        public void UpdateModValues()
        {
            // Manage local version
            versionLocal = MiscFunctions.RemoveLetters(modFilename);

            // Determine site mode
            if (website.Contains("minecraft.curseforge.com"))
            {
                siteMode = "curse";
            }
            else if (website.Contains("github.com"))
            {
                siteMode = "github";
            }
            else
            {
                siteMode = "NONE";
            }

            // Manage download site
            if (siteMode == "curse")
            {
                dlSite = website + "/files/latest";
            }

            else if (siteMode == "github")
            {
                string bit = website.Replace("https://github.com", "").Replace("/latest", "/download");
                string appendage = versionLatestRaw.Replace("<a href=\"", "").Replace(bit, "").Replace("\" rel=\"nofollow\">", "");
                dlSite = website.Replace("/latest", "/download") + appendage;
            }

            else
            {
                dlSite = "NONE";
            }
        }

        public void CheckForUpdate()
        {
            if (siteMode != "NONE")
            {
                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(checkDownloadCompleted);
                progress = 10;
                client.DownloadStringAsync(new Uri(website));
            }
            else
            {
                checkDone = true;
            }
        }

        private void checkDownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e != null && !String.IsNullOrEmpty(e.Result))
            {
                progress = 100;
                StringReader sr = new StringReader(e.Result);
                string newVersion = "";
                string newFile = "N/A";

                if (siteMode == "curse")
                {
                    while (!newVersion.Contains("<a class=\"overflow-tip\" href=\"/mc-mods/"))
                    {
                        newVersion = sr.ReadLine().Trim();
                    }

                    char[] endCharList = new char[] {'<','"'};
                    char[] startCharList = new char[] {'>'};
                    newFile = MiscFunctions.ExtractVersion(newVersion, endCharList, startCharList);
                }

                if (siteMode == "forum")
                {
                    while (!newVersion.Contains("<title>"))
                    {
                        newVersion = sr.ReadLine().Trim();
                    }
                    newFile = MiscFunctions.RemoveLetters(newVersion);
                }

                if (siteMode == "github")
                {
                    while (!newVersion.Contains("<a href=\"" + website.Replace("https://github.com", "").Replace("/latest", "/download/")))
                    {
                        newVersion = sr.ReadLine().Trim();
                    }

                    char[] endCharList = new char[] {'"'};
                    char[] startCharList = new char[] {'/'};
                    newFile = MiscFunctions.ExtractVersion(newVersion, endCharList, startCharList);
                }

                versionLatestRaw = newVersion;
                newFileName = Path.GetFileName(newFile);
                versionLatest = MiscFunctions.RemoveLetters(newFileName);
                UpdateModValues();
                progress = 0;
                checkQueued = false;
                checkDone = true;
            }
        }

        public void UpdateMod()
        {
            if (siteMode != "NONE")
            {
                string downloadFolder = cd + "\\temp\\downloads\\" + MiscFunctions.CleanName(newFileName);
                Directory.CreateDirectory(downloadFolder);

                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(modDownloadCompleted);
                progress = 10;
                client.DownloadFileAsync(new Uri(dlSite), downloadFolder + "\\" + newFileName);
            }
            else
            {
                downloadDone = true;
            }
        }

        private void modDownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            string downloadFolder = cd + "\\temp\\downloads\\" + MiscFunctions.CleanName(newFileName);

            try
            {
                string newFilePath = downloadFolder + "\\" + newFileName;

                if (File.Exists(newFilePath))
                {
                    ZipFile.ExtractToDirectory(newFilePath, downloadFolder + "\\temp");
                    progress = 100;

                    string newModLocation = cd + "\\modpack\\mods\\" + newFileName;
                    string oldModLocation = cd + "\\modpack\\mods\\" + modFilename;
                    File.Delete(newModLocation);
                    File.Delete(oldModLocation);

                    File.Move(newFilePath, newModLocation);
                    modFilename = newFileName;

                    try
                    {
                        Directory.Delete(downloadFolder, true);
                    }
                    catch
                    {}

                    versionLocalRaw = versionLatestRaw;
                    UpdateModValues();
                    progress = 0;
                    downloadQueued = false;
                    downloadDone = true;
                }
            }
            catch
            {}
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progress = 10 + Convert.ToInt32(e.ProgressPercentage * 0.8);
        }

        public int CompareTo(ModInfo other)
        {
            if (other == null)
            {
                return 1;
            }

            char[] charArray1 = (this.modFilename).ToCharArray();
            char[] charArray2 = (other.modFilename).ToCharArray();

            if (charArray1.Length == 0 || charArray2.Length == 0)
            {
                return 0;
            }

            for (int i = 0; i < charArray1.Length; i++)
            {
                if (charArray1[i].GetHashCode() != charArray2[i].GetHashCode())
                {
                    return charArray1[i].GetHashCode() - charArray2[i].GetHashCode();
                }

                if (i + 1 >= charArray2.Length)
                {
                    return -1;
                }
            }

            return 0;
        }
    }
}
