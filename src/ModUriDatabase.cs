using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Technic_Modpack_Creator
{
    class ModUriDatabase
    {
        private string cd = Directory.GetCurrentDirectory();

        private string userDatabaseFile = Directory.GetCurrentDirectory() + "\\settings\\userdatabase.dat";
        private Dictionary<string, string> userDatabase = new Dictionary<string, string>();
        private string globalDatabaseFile = Directory.GetCurrentDirectory() + "\\settings\\globaldatabase.dat";
        private Dictionary<string, string> globalDatabase = new Dictionary<string, string>();

        public void LoadDatabases()
        {
            if (File.Exists(userDatabaseFile))
            {
                userDatabase = SaveLoad.LoadFileBf<Dictionary<string, string>>(userDatabaseFile);
            }
            if (File.Exists(globalDatabaseFile))
            {
                globalDatabase = SaveLoad.LoadFileBf<Dictionary<string, string>>(globalDatabaseFile);
            }
            SaveDatabases();
        }

        public void SaveDatabases()
        {
            SaveLoad.SaveFileBf(userDatabase, userDatabaseFile);
        }

        public string GetSite(string modName)
        {
            string cleanName = MiscFunctions.CleanString(modName);
            if (userDatabase.ContainsKey(cleanName) && userDatabase[cleanName] != "NONE")
            {
                return userDatabase[cleanName];
            }
            else if (globalDatabase.ContainsKey(cleanName) && globalDatabase[cleanName] != "NONE")
            {
                return globalDatabase[cleanName];
            }
            else
            {
                return "NONE";
            }
        }

        public string GetSuperiorSite(string modName, string currentUri)
        {
            string dataSite = GetSite(modName);
            if (dataSite == "NONE")
            {
                return currentUri;
            }
            else
            {
                return dataSite;
            }
        }

        public void SetSite(string modName, string uri)
        {
            if (uri != "NONE")
            {
                ForceSetSite(modName, uri);
            }
        }

        public void ForceSetSite(string modName, string uri)
        {
            string cleanName = MiscFunctions.CleanString(modName);
            if (userDatabase.ContainsKey(cleanName))
            {
                userDatabase[cleanName] = uri;
            }
            else
            {
                userDatabase.Add(cleanName, uri);
            }
        }
    }
}
