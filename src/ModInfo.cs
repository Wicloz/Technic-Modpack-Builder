﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Diagnostics;

namespace Technic_Modpack_Creator {
    [Serializable]
    class ModInfo : System.IComparable<ModInfo> {
        private string cd = Directory.GetCurrentDirectory();
        private string downloadFolder {
            get {
                return cd + "\\downloads\\" + MiscFunctions.CleanString(modFileName);
            }
        }
        private string downloadedFile {
            get {
                return downloadFolder + "\\" + newFileName;
            }
        }
        private string curseIdentifier = "minecraft.curseforge.com/";
        private string forumIdentifier = "minecraftforum.net/forums/mapping-and-modding/minecraft-mods/";
        private string githubIdentifier = "github.com";
        private string mcVersion = "";

        public string modFileName = "";
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

        public string website1 = "NONE";
        public string website2 = "NONE";
        public string website3 = "NONE";
        public string website4 = "NONE";

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

        public bool findBusy {
            get {
                return _findBusy;
            }
            set {
                _findBusy = value;
            }
        }
        public bool checkBusy {
            get {
                return _checkBusy;
            }
            set {
                _checkBusy = value;
            }
        }
        public bool downloadBusy {
            get {
                return _downloadBusy;
            }
            set {
                _downloadBusy = value;
            }
        }
        public bool isWorking {
            get {
                return (findBusy || checkBusy || downloadBusy);
            }
        }

        public string uriState {
            get {
                if (website == "NONE") {
                    return "";
                }
                else if (dlSite != "NONE") {
                    return "Automatic";
                }
                else if (siteMode != "NONE") {
                    return "Manual";
                }
                else {
                    return "Unsupported";
                }
            }
        }
        public string updateState {
            get {
                if (checkQueued) {
                    return "Checking for Update ...";
                }
                else if (findQueued) {
                    return "Searching for Website ...";
                }
                else if (downloadQueued && progress != 0) {
                    return "Downloading: " + progress + "%";
                }
                else if (downloadQueued) {
                    return "Awaiting Download ...";
                }
                else if (canUpdate) {
                    return "Update Available";
                }
                else {
                    return "";
                }
            }
        }
        public bool canUpdate {
            get {
                return _canUpdate;
            }
            set {
                _canUpdate = value;
                if (canUpdate) {
                    Directory.CreateDirectory(downloadFolder);
                }
                else {
                    try {
                        Directory.Delete(downloadFolder, true);
                    }
                    catch { }
                }
            }
        }

        public ModInfo () { }

        public ModInfo (string fileName) {
            modFileName = fileName;
        }

        public void UpdateModValues () {
            cd = Directory.GetCurrentDirectory();

            //Manage local version
            versionLocal = MiscFunctions.RemoveLetters(modFileName);

            //Determine site mode
            if (website == "") {
                website = "NONE";
            }

            if (website.Contains(curseIdentifier)) {
                siteMode = "curse";
                website = ParseCurseUri(website);
            }
            else if (website.Contains(forumIdentifier)) {
                siteMode = "forum";
                website = ParseForumUri(website);
            }
            else if (website.Contains(githubIdentifier)) {
                siteMode = "github";
                website = ParseGithubUri(website);
            }
            else {
                siteMode = "NONE";
            }

            //Manage download site
            if (siteMode == "curse") {
                char[] startCharList = new char[] { 'f', 'i', 'l', 'e', 's', '/' };
                char[] endCharList = new char[] { '\"' };
                string modBit = MiscFunctions.ExtractSection(versionLatestRaw, endCharList, startCharList);

                dlSite = website + "/files/" + modBit + "/download";
            }

            else if (siteMode == "github") {
                string bit = website.Replace("https://github.com", "").Replace("/latest", "/download");
                string appendage = versionLatestRaw.Replace("<a href=\"", "").Replace(bit, "").Replace("\" rel=\"nofollow\">", "");
                dlSite = website.Replace("/latest", "/download") + appendage;
            }

            else {
                dlSite = "NONE";
            }
        }

        public void SetFileNames () {
            if (disabled && !newFileName.Contains(".disabled")) {
                newFileName += ".disabled";
            }
            if (disabled && !modFileName.Contains(".disabled")) {
                string modPath = cd + "\\modpack\\mods\\" + modFileName;
                File.Move(modPath, modPath + ".disabled");
                modFileName += ".disabled";
            }

            if (!disabled) {
                newFileName = newFileName.Replace(".disabled", "");
                modFileName = modFileName.Replace(".disabled", "");
                string modPath = cd + "\\modpack\\mods\\" + modFileName;

                if (File.Exists(modPath + ".disabled")) {
                    File.Move(modPath + ".disabled", modPath);
                }
            }
        }

        public void FindWebsiteUri () {
            findBusy = true;
            WebClient client1 = new WebClient();
            client1.DownloadStringCompleted += new DownloadStringCompletedEventHandler(findCurseCompleted);
            WebClient client2 = new WebClient();
            client2.DownloadStringCompleted += new DownloadStringCompletedEventHandler(findLuckyCompleted);
            WebClient client3 = new WebClient();
            client3.DownloadStringCompleted += new DownloadStringCompletedEventHandler(findGoogleCompleted);
            WebClient client4 = new WebClient();
            client4.DownloadStringCompleted += new DownloadStringCompletedEventHandler(findYahooCompleted);
            progress = 0;
            findMode++;

            switch (findMode) {
                case 1:
                    website = "NONE"; //debug
                    client1.DownloadStringAsync(new Uri("http://minecraft.curseforge.com/search?search=" + MiscFunctions.CleanModName(modFileName)));
                    break;
                case 2:
                    website1 = website;
                    website = "NONE"; //debug
                    client3.DownloadStringAsync(new Uri("http://www.google.com/search?sourceid=navclient&btnI=I&q=" + modFileName.Replace(" ", "") + "+minecraft"));
                    break;
                case 3:
                    website2 = website;
                    website = "NONE"; //debug
                    client4.DownloadStringAsync(new Uri("https://search.yahoo.com/search?p=" + modFileName.Replace(" ", "") + "+minecraft"));
                    break;
                case 4:
                    website3 = website;
                    website = "NONE"; //debug
                    client4.DownloadStringAsync(new Uri("https://search.yahoo.com/search?p=" + MiscFunctions.CleanString(modFileName) + "+minecraft"));
                    break;

                default:
                    website4 = website;
                    website = "NONE"; //debug
                    if (website1 != "NONE") {
                        website = website1;
                    }
                    else if (website2 != "NONE") {
                        website = website2;
                    }
                    else if (website3 != "NONE") {
                        website = website3;
                    }
                    else if (website4 != "NONE") {
                        website = website4;
                    }
                    website1 = website1.Replace(forumIdentifier, "").Replace(curseIdentifier, "").Replace("http://", "").Replace("www.", "");
                    website2 = website2.Replace(forumIdentifier, "").Replace(curseIdentifier, "").Replace("http://", "").Replace("www.", "");
                    website3 = website3.Replace(forumIdentifier, "").Replace(curseIdentifier, "").Replace("http://", "").Replace("www.", "");
                    website4 = website4.Replace(forumIdentifier, "").Replace(curseIdentifier, "").Replace("http://", "").Replace("www.", "");

                    //client2.DownloadStringAsync(new Uri("http://www.google.com/search?&sourceid=navclient&btnI=I&q=" + MiscFunctions.CleanString(modFilename) + "+curseforge"));
                    //client3.DownloadStringAsync(new Uri("http://www.google.com/search?sourceid=navclient&btnI=I&q=" + MiscFunctions.CleanString(modFilename) + "+minecraft"));
                    progress = 0;
                    findMode = 0;
                    findQueued = false;
                    findBusy = false;
                    updateList = true; //debug
                    break;
            }
        }

        private void findCurseCompleted (object sender, DownloadStringCompletedEventArgs e) {
            if (e != null && e.Error == null && !String.IsNullOrEmpty(e.Result)) {
                using (StringReader sr = new StringReader(e.Result)) {
                    List<string> results = new List<string>();

                    while (true) {
                        try {
                            string currentline = sr.ReadLine().Trim();
                            if (currentline.Contains("<a href=\"/mc-mods/")) {
                                results.Add(currentline);
                            }
                        }
                        catch {
                            break;
                        }
                    }

                    foreach (string result in results) {
                        char[] startCharList = new char[] { '"', '>' };
                        char[] endCharList = new char[] { '<' };
                        string foundModname = MiscFunctions.ExtractSection(result, endCharList, startCharList);

                        if (MiscFunctions.CleanString(foundModname) == MiscFunctions.CleanString(modFileName)) {
                            startCharList = new char[] { '=', '"' };
                            endCharList = new char[] { '"' };
                            string linkSection = MiscFunctions.ExtractSection(result, endCharList, startCharList);
                            website = ParseCurseUri("http://minecraft.curseforge.com" + linkSection);
                            break;
                        }
                    }
                }

                UpdateModValues();
                FindWebsiteUri(); //debug
                return; //debug

                if (siteMode == "NONE") {
                    FindWebsiteUri();
                }
                else {
                    progress = 0;
                    findMode = 0;
                    findQueued = false;
                    findBusy = false;
                    updateList = true;
                }
            }
            else {
                FindWebsiteUri();
            }
        }

        private void findLuckyCompleted (object sender, DownloadStringCompletedEventArgs e) {
            if (e != null && e.Error == null && !String.IsNullOrEmpty(e.Result)) {
                using (StringReader sr = new StringReader(e.Result)) {
                    try {
                        string currentline = "";
                        while (!currentline.Contains("<a href=\"/mc-mods/")) {
                            currentline = sr.ReadLine().Trim();
                        }

                        char[] startCharList = new char[] { '=', '"' };
                        char[] endCharList = new char[] { '"' };
                        string linkSection = MiscFunctions.ExtractSection(currentline, endCharList, startCharList);

                        startCharList = new char[] { '"', '>' };
                        endCharList = new char[] { '<' };
                        string foundModname = MiscFunctions.ExtractSection(currentline, endCharList, startCharList);

                        if (!MiscFunctions.PartialMatch(modFileName, foundModname)) {
                            website = "No Patial Match: "; //debug
                        }
                        website += ParseCurseUri("http://minecraft.curseforge.com" + linkSection);
                    }
                    catch {
                        website = "NONE";
                    }
                }

                UpdateModValues();
                FindWebsiteUri(); //debug
                return; //debug

                if (siteMode == "NONE") {
                    FindWebsiteUri();
                }
                else {
                    progress = 0;
                    findMode = 0;
                    findQueued = false;
                    findBusy = false;
                    updateList = true;
                }
            }
            else {
                FindWebsiteUri();
            }
        }

        private void findGoogleCompleted (object sender, DownloadStringCompletedEventArgs e) {
            if (e != null && e.Error == null && !String.IsNullOrEmpty(e.Result)) {
                char[] startCharList = new char[] { 'q', '=' };
                char[] endCharList = new char[] { '&' };
                website = GetWebsiteFromResults(e.Result, "<a href=\"", startCharList, endCharList);
                UpdateModValues();

                FindWebsiteUri(); //debug
                return; //debug

                if (siteMode == "NONE") {
                    FindWebsiteUri();
                }
                else {
                    progress = 0;
                    findMode = 0;
                    findQueued = false;
                    findBusy = false;
                    updateList = true;
                }
            }
            else {
                FindWebsiteUri();
            }
        }

        private void findYahooCompleted (object sender, DownloadStringCompletedEventArgs e) {
            if (e != null && e.Error == null && !String.IsNullOrEmpty(e.Result)) {
                char[] startCharList = new char[] { };
                char[] endCharList = new char[] { '"' };
                website = GetWebsiteFromResults(e.Result, "href=\"", startCharList, endCharList);
                UpdateModValues();

                FindWebsiteUri(); //debug
                return; //debug

                if (siteMode == "NONE") {
                    FindWebsiteUri();
                }
                else {
                    progress = 0;
                    findMode = 0;
                    findQueued = false;
                    findBusy = false;
                    updateList = true;
                }
            }
            else {
                FindWebsiteUri();
            }
        }

        private string GetWebsiteFromResults (string webpage, string delim, char[] startChars, char[] endChars) {
            List<string> results = new List<string>();
            using (StringReader sr = new StringReader(webpage)) {
                string currentLine = "";
                while (true) {
                    currentLine = sr.ReadLine();

                    if (currentLine != null) {
                        if (currentLine.Contains(delim)) {
                            string[] delims = new string[] { delim };
                            results.AddRange(currentLine.Split(delims, StringSplitOptions.RemoveEmptyEntries));
                        }
                    }
                    else {
                        break;
                    }
                }

                string curseLink = "NONE";
                string forumLink = "NONE";

                for (int i = 1; i < results.Count; i++) {
                    results[i] = MiscFunctions.ExtractSection(results[i], endChars, startChars).Replace("%2f", "/").Replace("%3a", ":");

                    char[] startCharList = new char[] { 'm', 'o', 'd', 's', '/' };
                    char[] endCharList = new char[] { };
                    string foundModname = MiscFunctions.ExtractSection(results[i], endCharList, startCharList);

                    if (curseLink == "NONE" && results[i].Contains(curseIdentifier) && MiscFunctions.PartialMatch(modFileName, foundModname)) {
                        curseLink = ParseCurseUri(results[i]);
                    }
                    if (forumLink == "NONE" && results[i].Contains(forumIdentifier) && MiscFunctions.PartialMatch(modFileName, foundModname)) {
                        forumLink = ParseForumUri(results[i]);
                    }
                }

                if (curseLink != "NONE") {
                    return curseLink;
                }
                else {
                    return forumLink;
                }
            }
        }

        private string ParseCurseUri (string uri) {
            if (!uri.Contains(curseIdentifier) || uri == "http://minecraft.curseforge.com/mc-mods/minecraft") {
                uri = "NONE";
            }

            if (uri.EndsWith("/files")) {
                uri = uri.Replace("/files", "");
            }
            else if (uri.Contains("/files/")) {
                char[] startCharList = new char[] { 'f', 'i', 'l', 'e', 's', '/' };
                char[] endCharList = new char[] { };
                string garbage = "/files/" + MiscFunctions.ExtractSection(uri, endCharList, startCharList);
                uri = uri.Replace(garbage, "");
            }

            return uri;
        }

        private string ParseForumUri (string uri) {
            if (!uri.Contains(forumIdentifier) || uri == "http://www.minecraftforum.net/forums/mapping-and-modding/minecraft-mods") {
                uri = "NONE";
            }

            if (uri.Contains("?page=")) {
                char[] endCharList = new char[] { '?' };
                uri = MiscFunctions.ExtractSection(uri, endCharList);
            }

            return uri;
        }

        private string ParseGithubUri (string uri) {
            if (!uri.Contains(githubIdentifier)) {
                uri = "NONE";
            }

            return uri;
        }

        public void CheckForUpdate (string mcVersion) {
            this.mcVersion = mcVersion;

            if (siteMode != "NONE") {
                checkBusy = true;
                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(checkDownloadCompleted);
                progress = 10;
                client.DownloadStringAsync(new Uri(website));
            }
            else {
                checkQueued = false;
                versionLatest = "N/A";
                releaseDate = "N/A";
                newFileName = "";
            }
        }

        private void checkDownloadCompleted (object sender, DownloadStringCompletedEventArgs e) {
            if (e != null && e.Error == null && !String.IsNullOrEmpty(e.Result)) {
                progress = 100;
                using (StringReader sr = new StringReader(e.Result)) {
                    string newVersion = "";
                    string newFile = "";
                    releaseDate = "N/A";
                    versionLatestRaw = "N/A";
                    versionLatest = "N/A";
                    newFileName = "";

                    if (siteMode == "curse") {
                        while (true) {
                            while (!newVersion.Contains("<h4 class=\"e-sidebar-subheader overflow-tip\">Minecraft")) {
                                newVersion = sr.ReadLine();
                                if (newVersion == null) {
                                    versionLatest = "MC version not found";
                                    progress = 0;
                                    checkQueued = false;
                                    checkBusy = false;
                                    updateList = true;
                                    return;
                                }
                                newVersion = newVersion.Trim();
                            }

                            char[] startCharList = new char[] { '>', 'm', 'i', 'n', 'e', 'c', 'r', 'a', 'f', 't' };
                            char[] endCharList = new char[] { '<' };
                            string siteVersion = MiscFunctions.ExtractSection(newVersion.ToLower(), endCharList, startCharList).Trim();

                            string thisVersion = "";
                            try {
                                char[] delim = new char[] { '.' };
                                thisVersion = mcVersion.Split(delim)[0] + "." + mcVersion.Split(delim)[1];
                            }
                            catch { }

                            if (siteVersion == thisVersion) {
                                while (!newVersion.Contains("<a class=\"overflow-tip\" href=\"/projects/")) {
                                    newVersion = sr.ReadLine().Trim();
                                }

                                startCharList = new char[] { '>' };
                                endCharList = new char[] { '<' };
                                newFile = MiscFunctions.ExtractSection(newVersion, endCharList, startCharList).Replace(" ", "").ToLower().Replace("&#x27;", "").Replace("+", "");

                                string[] modNames = new string[] { "reliquary", "carpentersblocks", "hardcoreenderexpansion" };
                                foreach (string mod in modNames) {
                                    if (MiscFunctions.CleanString(website).Contains(mod) && !MiscFunctions.CleanString(newFile).Contains(mod)) {
                                        newFile = mod + "-" + newFile;
                                    }
                                }

                                string dateLine = sr.ReadLine().Trim();
                                startCharList = new char[] { '"', '>' };
                                endCharList = new char[] { '<' };
                                releaseDate = MiscFunctions.ExtractSection(dateLine, endCharList, startCharList);
                                break;
                            }
                            else {
                                newVersion = sr.ReadLine().Trim();
                            }
                        }
                    }

                    if (siteMode == "forum") {
                        while (!newVersion.Contains("<title>")) {
                            newVersion = sr.ReadLine().Trim();
                        }
                        newFile = "undefined-" + MiscFunctions.RemoveLetters(newVersion);
                    }

                    if (siteMode == "github") {
                        while (!newVersion.Contains("<a href=\"" + website.Replace("https://github.com", "").Replace("/latest", "/download/"))) {
                            newVersion = sr.ReadLine().Trim();
                        }

                        char[] startCharList = new char[] { '/' };
                        char[] endCharList = new char[] { '"' };
                        newFile = MiscFunctions.ExtractSection(newVersion, endCharList, startCharList).Replace(" ", "").ToLower();
                    }

                    if (!newFile.Contains(".zip") && !newFile.Contains(".jar")) {
                        newFile += ".jar";
                    }
                    if (disabled && !newFile.Contains(".disabled")) {
                        newFile += ".disabled";
                    }

                    versionLatestRaw = newVersion;
                    newFileName = Path.GetFileName(newFile);
                    versionLatest = MiscFunctions.RemoveLetters(newFileName);

                    if (versionLatestRaw != versionLocalRaw) {
                        canUpdate = true;
                    }
                    else {
                        canUpdate = false;
                    }
                }

                UpdateModValues();
                progress = 0;
                checkQueued = false;
                checkBusy = false;
                updateList = true;
            }
            else {
                progress = 0;
                checkQueued = false;
                checkBusy = false;
            }
        }

        public void UpdateMod () {
            if (Directory.Exists(downloadFolder) && Directory.GetFiles(downloadFolder).Length > 0) {
                newFileName = Path.GetFileName(Directory.GetFiles(downloadFolder)[0]);
                if (newFileName.EndsWith(".zip") || newFileName.EndsWith(".jar")) {
                    downloadBusy = true;
                    MoveDownloadedMod();
                }
                else {
                    downloadQueued = false;
                }
            }

            else if (uriState == "Automatic") {
                downloadBusy = true;
                if (Directory.Exists(downloadFolder)) {
                    Directory.Delete(downloadFolder, true);
                }
                Directory.CreateDirectory(downloadFolder);

                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(modDownloadCompleted);
                progress = 10;
                client.DownloadFileAsync(new Uri(dlSite), downloadedFile);
            }

            else {
                downloadQueued = false;
            }
        }

        private void modDownloadCompleted (object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
            try {
                if (File.Exists(downloadedFile)) {
                    //ZipFile.ExtractToDirectory(downloadedFile, downloadFolder + "\\extract");
                    progress = 100;
                    MoveDownloadedMod();
                }
            }
            catch {
                if (Directory.Exists(downloadFolder)) {
                    Directory.Delete(downloadFolder, true);
                }
                Directory.CreateDirectory(downloadFolder);

                progress = 0;
                downloadQueued = false;
                downloadBusy = false;
            }
        }

        private void MoveDownloadedMod () {
            string newModLocation = cd + "\\modpack\\mods\\" + newFileName;
            string oldModLocation = cd + "\\modpack\\mods\\" + modFileName;

            File.Delete(newModLocation);
            File.Delete(oldModLocation);
            File.Move(downloadedFile, newModLocation);

            modFileName = newFileName.ToLower();
            versionLocalRaw = versionLatestRaw;

            UpdateModValues();
            progress = 0;
            downloadQueued = false;
            downloadBusy = false;
            updateList = true;
            canUpdate = false;
        }

        private void client_DownloadProgressChanged (object sender, DownloadProgressChangedEventArgs e) {
            progress = 10 + Convert.ToInt32(e.ProgressPercentage * 0.8);
        }

        public int CompareTo (ModInfo other) {
            if (other == null) {
                return 1;
            }

            char[] charArray1 = (this.modFileName).ToCharArray();
            char[] charArray2 = (other.modFileName).ToCharArray();

            if (charArray1.Length == 0 || charArray2.Length == 0) {
                return 0;
            }

            for (int i = 0; i < charArray1.Length; i++) {
                if (charArray1[i].GetHashCode() != charArray2[i].GetHashCode()) {
                    return charArray1[i].GetHashCode() - charArray2[i].GetHashCode();
                }

                if (i + 1 >= charArray2.Length) {
                    return -1;
                }
            }

            return 0;
        }
    }
}
