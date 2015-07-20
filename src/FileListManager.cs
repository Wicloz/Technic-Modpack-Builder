using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Technic_Modpack_Creator
{
    class FileListManager
    {
        public List<FileList> fileLists = new List<FileList>();

        private string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string cd = Directory.GetCurrentDirectory();

        public void LoadLists()
        {
            foreach (string dir in Directory.GetDirectories(cd + "\\export\\versions", "*", SearchOption.TopDirectoryOnly))
            {
                int order = SaveLoad.LoadFileBf<int>(dir + "\\properties.dat");
                fileLists.Add(new FileList(order, dir.Replace(Path.GetDirectoryName(dir) + "\\","")));
            }

            fileLists.Sort();

            foreach (FileList list in fileLists)
            {
                string directory = cd + "\\export\\versions\\" + list.version;

                list.fileList = SaveLoad.LoadFileXml<List<string>>(directory + "\\filelist.txt");

                list.modList = SaveLoad.LoadFileBf<List<string>>(directory + "\\modlist.dat");
                list.changelog = SaveLoad.LoadFileBf<List<string>>(directory + "\\changelog.dat");
            }

            fileLists.Sort();
        }

        public void SaveLists(string currentVersion)
        {
            if (currentVersion != "")
            {
                fileLists.Sort();
                CreateNewestList(currentVersion);

                foreach (FileList list in fileLists)
                {
                    string directory = cd + "\\export\\versions\\" + list.version;

                    Directory.CreateDirectory(directory);
                    SaveLoad.SaveFileBf(list.order, directory + "\\properties.dat");

                    SaveLoad.SaveFileXml(list.fileList, directory + "\\filelist.txt");

                    List<string> modList = new List<string>();

                    foreach (string mod in list.modList)
                    {
                        modList.Add(" - " + ProcessModName(mod));
                    }

                    File.WriteAllLines(directory + "\\modlist.txt", modList);
                    File.WriteAllLines(directory + "\\changelog.txt", list.changelog);

                    SaveLoad.SaveFileBf(list.modList, directory + "\\modlist.dat");
                    SaveLoad.SaveFileBf(list.changelog, directory + "\\changelog.dat");
                }
            }
        }

        private void CreateNewestList (string currentVersion)
        {
            int thisOrder = 0;

            if (fileLists.Count > 0)
            {
                bool delete = false;
                for (int i = 0; i < fileLists.Count; i++)
                {
                    FileList list = fileLists[i];

                    if (list.version == currentVersion)
                    {
                        thisOrder = i;
                        delete = true;
                    }

                    if (delete)
                    {
                        Directory.Delete(cd + "\\export\\versions\\" + list.version, true);
                        fileLists.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        thisOrder = i + 1;
                    }
                }
            }

            fileLists.Add(new FileList(thisOrder, currentVersion));

            fileLists[thisOrder].CreateFileList();
            fileLists[thisOrder].CreateModList();

            if (fileLists.Count > 1)
            {
                fileLists[thisOrder].CreateUpdateList(fileLists[thisOrder - 1]);
            }
            else
            {
                fileLists[thisOrder].CreateUpdateList(null);
            }
        }

        public static string ProcessModName(string mod)
        {
            string cleanMod = mod.
                Replace(".jar", "").
                Replace(".zip", "");

            char[] modChars = cleanMod.ToCharArray();
            List<char> nonLetters = new List<char>();

            nonLetters.Add('0');
            nonLetters.Add('1');
            nonLetters.Add('2');
            nonLetters.Add('3');
            nonLetters.Add('4');
            nonLetters.Add('5');
            nonLetters.Add('6');
            nonLetters.Add('7');
            nonLetters.Add('8');
            nonLetters.Add('9');

            nonLetters.Add('-');
            nonLetters.Add('_');
            nonLetters.Add('.');
            nonLetters.Add('[');
            nonLetters.Add(']');

            cleanMod = "";
            bool makeUpper = true;

            for (int i = 0; i < modChars.Length; i++)
            {
                if (nonLetters.Contains(modChars[i]))
                {
                    makeUpper = true;
                }
                else if (makeUpper)
                {
                    modChars[i] = modChars[i].ToString().ToUpper().ToCharArray()[0];
                    makeUpper = false;
                }

                cleanMod += modChars[i].ToString();
            }

            cleanMod = cleanMod.
                Replace("Rc", "rc").
                Replace("Mc", "mc");

            return cleanMod;
        }
    }

    [Serializable]
    public class FileList : System.IComparable<FileList>
    {
        private string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string cd = Directory.GetCurrentDirectory();

        public int order;
        public string version;
        public List<string> fileList = new List<string>();
        public List<string> modList = new List<string>();
        public List<string> changelog = new List<string>();

        public FileList()
        { }

        public FileList(int order, string version)
        {
            this.order = order;
            this.version = version;
        }

        public void CreateFileList()
        {
            fileList = new List<string>();

            foreach (string file in Directory.GetFiles(cd + "\\modpack", "*.*", SearchOption.AllDirectories))
            {
                fileList.Add(file.Replace(cd + "\\modpack\\", ""));
            }
        }

        public void CreateModList()
        {
            modList = new List<string>();

            foreach (string file in this.fileList)
            {
                if ((file.Contains("mods\\") || file.Contains("coremods\\") || file.Contains("Flan\\")) && Path.GetExtension(file) != ".disabled")
                {
                    modList.Add(file.Replace("mods\\", "").Replace("coremods\\", ""));
                }
            }
        }

        public void CreateUpdateList(FileList other)
        {
            if (other == null)
            {
                other = new FileList(0, "0");
            }

            foreach (string mod in this.modList)
            {
                if (!other.modList.Contains(mod))
                {
                    changelog.Add("+ Added: " + FileListManager.ProcessModName(mod));
                }
            }

            foreach (string mod in other.modList)
            {
                if (!this.modList.Contains(mod))
                {
                    changelog.Add("- Removed: " + FileListManager.ProcessModName(mod));
                }
            }
        }

        public int CompareTo(FileList other)
        {
            return this.order - other.order;
        }
    }
}
