using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Technic_Modpack_Creator
{
    class SettingManager
    {
        public string minecraftVersionLoad;
        public string modpackVersionLoad;
        public string nameLoad;
        public string locationLoad;

        private string cd = Directory.GetCurrentDirectory();

        public SettingManager ()
        {
            if (File.Exists(cd + "\\settings\\settings.cfg"))
            {
                FileStream fs = File.Open(cd + "\\settings\\settings.cfg", FileMode.Open, FileAccess.ReadWrite);
                BinaryFormatter bf = new BinaryFormatter();
                SettingData data = (SettingData)bf.Deserialize(fs);
                fs.Close();

                minecraftVersionLoad = data.minecraftVersion;
                modpackVersionLoad = data.modpackVersion;
                nameLoad = data.name;
                locationLoad = data.location;
            }
            else
            {
                minecraftVersionLoad = "";
                modpackVersionLoad = "";
                nameLoad = "";
                locationLoad = "";

                SaveSettings(minecraftVersionLoad, modpackVersionLoad, nameLoad, locationLoad);
            }
        }

        public void SaveSettings(string MinecraftVersion, string ModpackVersion, string Name, string Location)
        {
            FileStream fs = File.Create(cd + "\\settings\\settings.cfg");
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(fs, new SettingData(MinecraftVersion, ModpackVersion, Name, Location));
            fs.Close();
        }
    }

    [Serializable]
    public class SettingData
    {
        public string minecraftVersion;
        public string modpackVersion;
        public string name;
        public string location;

        public SettingData(string MinecraftVersion, string ModpackVersion, string Name, string Location)
        {
            minecraftVersion = MinecraftVersion;
            modpackVersion = ModpackVersion;
            name = Name;
            location = Location;
        }
    }
}
