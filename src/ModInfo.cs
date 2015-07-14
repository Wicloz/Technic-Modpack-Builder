using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Diagnostics;

namespace Technic_Modpack_Creator
{
    [Serializable]
    class ModInfo : System.IComparable<ModInfo>
    {
        private string cd = Directory.GetCurrentDirectory();
        private string downloadFolder;
        public string modFilename = "";
        public bool disabled = false;

        public string versionLocalRaw = "";
        public string versionLatestRaw = "";
        public string versionLocal = "N/A";
        public string versionLatest = "N/A";
        public string releaseDate = "N/A";
        public string newFileName = "";
        public bool _canUpdate = false;

        public string website = "NONE";
        public string siteMode = "NONE";
        public string dlSite = "NONE";

        //Dowload + Check info
        public bool updateList = false;
        public int progress = 0;
        public int findMode = 0;
        public bool findQueued = false;
        public bool checkQueued = false;
        public bool downloadQueued = false;
        private bool _findBusy = false;
        private bool _checkBusy = false;
        private bool _downloadBusy = false;

        public bool findBusy
        {
            get
            {
                return _findBusy;
            }
            set
            {
                _findBusy = value;
            }
        }
        public bool checkBusy
        {
            get
            {
                return _checkBusy;
            }
            set
            {
                _checkBusy = value;
            }
        }
        public bool downloadBusy
        {
            get
            {
                return _downloadBusy;
            }
            set
            {
                _downloadBusy = value;
            }
        }

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
                else if (findQueued)
                {
                    return "Searching for Website ...";
                }
                else if (downloadQueued && progress != 0)
                {
                    return "Downloading: " + progress + "%";
                }
                else if (downloadQueued)
                {
                    return "Awaiting Download ...";
                }
                else if (canUpdate)
                {
                    return "Update Available";
                }
                else
                {
                    return "";
                }
            }
        }
        public bool canUpdate
        {
            get
            {
                return _canUpdate;
            }
            set
            {
                _canUpdate = value;
                if (canUpdate)
                {
                    Directory.CreateDirectory(downloadFolder);
                }
                else
                {
                    Directory.Delete(downloadFolder, true);
                }
            }
        }

        public ModInfo()
        { }

        public ModInfo(string fileName)
        {
            modFilename = fileName;
        }

        public void UpdateModValues()
        {
            //Manage local version
            versionLocal = MiscFunctions.RemoveLetters(modFilename);

            //Initialise variables
            downloadFolder = cd + "\\downloads\\" + MiscFunctions.CleanName(modFilename);

            //Determine site mode
            if (website == "")
            {
                website = "NONE";
            }

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

            //Manage download site
            if (website == "http://minecraft.curseforge.com/mc-mods/minecraft")
            {
                website = "NONE";
            }

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

        public void SetFileNames()
        {
            if (disabled && !newFileName.Contains(".disabled"))
            {
                newFileName += ".disabled";
            }
            if (disabled && !modFilename.Contains(".disabled"))
            {
                string modPath = cd + "\\modpack\\mods\\" + modFilename;
                File.Move(modPath, modPath + ".disabled");
                modFilename += ".disabled";
            }

            if (!disabled)
            {
                newFileName = newFileName.Replace(".disabled", "");
                modFilename = modFilename.Replace(".disabled", "");
                string modPath = cd + "\\modpack\\mods\\" + modFilename;

                if (File.Exists(modPath + ".disabled"))
                {
                    File.Move(modPath + ".disabled", modPath);
                }
            }
        }

        public void FindWebsiteUri()
        {
            findBusy = true;
            WebClient client1 = new WebClient();
            client1.DownloadStringCompleted += new DownloadStringCompletedEventHandler(findCurseCompleted);
            WebClient client2 = new WebClient();
            client2.DownloadStringCompleted += new DownloadStringCompletedEventHandler(findGoogleCompleted);
            progress = 0;
            findMode++;

            switch (findMode)
            {
                case 1:
                    client1.DownloadStringAsync(new Uri("http://minecraft.curseforge.com/search?search=" + MiscFunctions.CleanName(modFilename)));
                    break;
                case 2:
                    client2.DownloadStringAsync(new Uri("http://www.google.com/search?&sourceid=navclient&btnI=I&q=" + MiscFunctions.CleanName(modFilename) + "+curseforge"));
                    break;

                default:
                    UpdateModValues();
                    progress = 0;
                    findMode = 0;
                    findQueued = false;
                    findBusy = false;
                    updateList = true;
                    break;
            }
        }

        private void findCurseCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e != null && !String.IsNullOrEmpty(e.Result))
            {
                using (StringReader sr = new StringReader(e.Result))
                {
                    List<string> results = new List<string>();

                    while (true)
                    {
                        try
                        {
                            string currentline = sr.ReadLine().Trim();
                            if (currentline.Contains("<a href=\"/mc-mods/"))
                            {
                                results.Add(currentline);
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }

                    foreach (string result in results)
                    {
                        if (MiscFunctions.CleanName(result).Contains(MiscFunctions.CleanName(modFilename)))
                        {
                            char[] endCharList = new char[] { '"' };
                            char[] startCharList = new char[] { '=', '"' };
                            string linkSection = MiscFunctions.ExtractSection(result, endCharList, startCharList);
                            website = "http://minecraft.curseforge.com" + linkSection;
                            break;
                        }
                    }
                }

                UpdateModValues();

                if (siteMode == "NONE")
                {
                    FindWebsiteUri();
                }
                else
                {
                    progress = 0;
                    findMode = 0;
                    findQueued = false;
                    findBusy = false;
                    updateList = true;
                }
            }
        }

        private void findGoogleCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e != null && !String.IsNullOrEmpty(e.Result))
            {
                using (StringReader sr = new StringReader(e.Result))
                {
                    string currentline = "";

                    try
                    {
                        while (!currentline.Contains("<a href=\"/mc-mods/"))
                        {
                            currentline = sr.ReadLine().Trim();
                        }

                        char[] endCharList = new char[] {'"'};
                        char[] startCharList = new char[] {'=','"'};
                        string linkSection = MiscFunctions.ExtractSection(currentline, endCharList, startCharList);
                        website = "http://minecraft.curseforge.com" + linkSection;
                    }
                    catch
                    { }
                }

                UpdateModValues();

                if (siteMode == "NONE")
                {
                    FindWebsiteUri();
                }
                else
                {
                    progress = 0;
                    findMode = 0;
                    findQueued = false;
                    findBusy = false;
                    updateList = true;
                }
            }
        }

        public void CheckForUpdate()
        {
            if (siteMode != "NONE")
            {
                checkBusy = true;
                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(checkDownloadCompleted);
                progress = 10;
                client.DownloadStringAsync(new Uri(website));
            }
            else
            {
                checkQueued = false;
            }
        }

        private void checkDownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e != null && !String.IsNullOrEmpty(e.Result))
                {
                    progress = 100;
                    using (StringReader sr = new StringReader(e.Result))
                    {
                        string newVersion = "";
                        string newFile = "";

                        if (siteMode == "curse")
                        {
                            while (!newVersion.Contains("<a class=\"overflow-tip\" href=\"/mc-mods/"))
                            {
                                newVersion = sr.ReadLine().Trim();
                            }

                            char[] endCharList = new char[] { '<' };
                            char[] startCharList = new char[] { '>' };
                            newFile = MiscFunctions.ExtractSection(newVersion, endCharList, startCharList);

                            string dateLine = sr.ReadLine().Trim();
                            endCharList = new char[] { '<' };
                            startCharList = new char[] { '"', '>' };
                            releaseDate = MiscFunctions.ExtractSection(dateLine, endCharList, startCharList);
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

                            char[] endCharList = new char[] { '"' };
                            char[] startCharList = new char[] { '/' };
                            newFile = MiscFunctions.ExtractSection(newVersion, endCharList, startCharList);
                        }

                        if (!newFile.Contains(".zip") && !newFile.Contains(".jar"))
                        {
                            newFile += ".jar";
                        }
                        if (disabled && !newFile.Contains(".disabled"))
                        {
                            newFile += ".disabled";
                        }

                        versionLatestRaw = newVersion;
                        newFileName = Path.GetFileName(newFile);
                        versionLatest = MiscFunctions.RemoveLetters(newFileName);

                        if (versionLatestRaw != versionLocalRaw)
                        {
                            canUpdate = true;
                        }
                    }

                    UpdateModValues();
                    progress = 0;
                    checkQueued = false;
                    checkBusy = false;
                    updateList = true;
                }
            }
            catch
            {
                progress = 0;
                checkQueued = false;
                checkBusy = false;
            }
        }

        public void UpdateMod()
        {
            if (siteMode != "NONE")
            {
                downloadBusy = true;
                Directory.CreateDirectory(downloadFolder);

                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(modDownloadCompleted);
                progress = 10;
                client.DownloadFileAsync(new Uri(dlSite), downloadFolder + "\\" + newFileName);
            }

            else if (Directory.GetFiles(downloadFolder).Length > 0)
            {
                newFileName = Directory.GetFiles(downloadFolder)[0];
                if (newFileName.EndsWith(".zip") || newFileName.EndsWith(".jar"))
                {
                    downloadBusy = true;
                    MoveDownloadedMod();
                }
                else
                {
                    downloadQueued = false;
                }
            }

            else
            {
                downloadQueued = false;
            }
        }

        private void modDownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                string downloadedFilePath = downloadFolder + "\\" + newFileName;

                if (File.Exists(downloadedFilePath))
                {
                    ZipFile.ExtractToDirectory(downloadedFilePath, downloadFolder + "\\extract");
                    progress = 100;
                    MoveDownloadedMod();
                }
            }
            catch
            { }
        }

        private void MoveDownloadedMod()
        {
            string newModLocation = cd + "\\modpack\\mods\\" + newFileName;
            string oldModLocation = cd + "\\modpack\\mods\\" + modFilename;
            File.Delete(newModLocation);
            File.Delete(oldModLocation);

            string downloadedFilePath = downloadFolder + "\\" + newFileName;
            File.Move(downloadedFilePath, newModLocation);
            modFilename = newFileName.ToLower();

            try
            {
                Directory.Delete(downloadFolder, true);
            }
            catch
            { }

            versionLocalRaw = versionLatestRaw;
            UpdateModValues();
            progress = 0;
            downloadQueued = false;
            downloadBusy = false;
            updateList = true;
            canUpdate = false;
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
