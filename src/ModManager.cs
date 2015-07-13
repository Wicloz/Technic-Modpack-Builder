using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Technic_Modpack_Creator
{
    public partial class ModManager : Form
    {
        private List<ModInfo> modList = new List<ModInfo>();
        private string cd = Directory.GetCurrentDirectory();

        private string selectedModName = "";
        private int selectedIndex = -1;

        public ModManager()
        {
            InitializeComponent();
            LoadModList();
            UpdateModList();
        }

        private void ModManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveModList();
        }

        private void LoadModList()
        {
            modList = SaveLoad.LoadFileBf<List<ModInfo>>(cd + "\\settings\\modlist.dat");
            if (modList == null)
            {
                modList = new List<ModInfo>();
            }
        }

        private void SaveModList()
        {
            SaveLoad.SaveFileBf(modList, cd + "\\settings\\modlist.dat");
        }

        private void UpdateModList()
        {
            List<string> fileList = new List<string>();

            foreach (string file in Directory.GetFiles(cd + "\\modpack\\mods", "*.jar", SearchOption.TopDirectoryOnly))
            {
                fileList.Add(Path.GetFileName(file));
            }
            foreach (string file in Directory.GetFiles(cd + "\\modpack\\mods", "*.zip", SearchOption.TopDirectoryOnly))
            {
                fileList.Add(Path.GetFileName(file));
            }

            foreach (string file in fileList)
            {
                bool exists = false;
                foreach (ModInfo info in modList)
                {
                    if (file == info.modFile)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    modList.Add(new ModInfo(file));
                }
            }

            for (int i = 0; i < modList.Count; i++)
            {
                bool exists = false;
                foreach (string file in fileList)
                {
                    if (modList[i].modFile == file)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    modList.RemoveAt(i);
                    i--;
                }
                else
                {
                    modList[i].UpdateModValues();
                }
            }

            modList.Sort();
            UpdateListView();
            SaveModList();
        }

        private void UpdateListView()
        {
            modListView.Items.Clear();

            foreach (ModInfo mod in modList)
            {
                ListViewItem lvi = new ListViewItem(mod.modFile);

                lvi.SubItems.Add(mod.uriState);
                lvi.SubItems.Add(mod.versionLocal);
                lvi.SubItems.Add(mod.versionLatest);
                lvi.SubItems.Add(mod.updateState);

                modListView.Items.Add(lvi);
            }

            // Selection
            modListView.SelectedIndices.Clear();
            int index = 0;

            if (selectedModName != "")
            {
                for (int i = 0; i < modListView.Items.Count; i++)
                {
                    string item = modListView.Items[i].SubItems[0].Text;
                    if (item == selectedModName)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index >= 0)
            {
                modListView.SelectedIndices.Add(index);
                SelectItem(index);
            }
            else
            {
                ClearSelection();
            }
        }

        private void UpdateModFields(int index)
        {
            ModInfo mod = modList[index];

            modNameBox.Text = mod.modFile;
            modSiteBox.Text = mod.website;
            modDownloadBox.Text = mod.dlSite;
            linkStatusLabel.Text = mod.uriState;
        }

        private void ClearSelection()
        {
            selectedModName = "";
            selectedIndex = -1;

            modNameBox.Text = "";
            modSiteBox.Text = "";
            modDownloadBox.Text = "";
            linkStatusLabel.Text = "";
        }

        private void SelectItem(int index)
        {
            selectedModName = modListView.Items[index].SubItems[0].Text;
            selectedIndex = index;
            UpdateModFields(selectedIndex);
        }

        private void modListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (modListView.SelectedIndices.Count > 0)
            {
                SelectItem(modListView.SelectedIndices[0]);
            }
        }

        private void updateListButton_Click(object sender, EventArgs e)
        {
            UpdateModList();
        }

        private void modSiteBox_TextChanged(object sender, EventArgs e)
        {
            modList[selectedIndex].website = modSiteBox.Text;
            modListView.Items[selectedIndex].SubItems[1].Text = modList[selectedIndex].uriState;
        }
    }
}
