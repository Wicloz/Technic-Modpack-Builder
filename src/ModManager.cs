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
using System.Diagnostics;

namespace Technic_Modpack_Creator
{
    public partial class ModManager : Form
    {
        private List<ModInfo> modList = new List<ModInfo>();
        private string cd = Directory.GetCurrentDirectory();

        private ModInfo selectedMod;
        private int selectedIndex = -1;
        private bool action = false;
        private bool updatingFields = false;
        private bool downloadBusy = false;

        public ModManager()
        {
            InitializeComponent();
            //Initiate timer
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 100;
            timer.Start();
        }

        private void ModManager_Load(object sender, EventArgs e)
        {
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
            Main.acces.MakeLowerCases();
            List<string> fileList = new List<string>();

            foreach (string file in Directory.GetFiles(cd + "\\modpack\\mods", "*.jar", SearchOption.TopDirectoryOnly))
            {
                fileList.Add(Path.GetFileName(file));
            }
            foreach (string file in Directory.GetFiles(cd + "\\modpack\\mods", "*.zip", SearchOption.TopDirectoryOnly))
            {
                fileList.Add(Path.GetFileName(file));
            }
            foreach (string file in Directory.GetFiles(cd + "\\modpack\\mods", "*.disabled", SearchOption.TopDirectoryOnly))
            {
                fileList.Add(Path.GetFileName(file));
            }

            foreach (string file in fileList)
            {
                bool exists = false;
                foreach (ModInfo info in modList)
                {
                    if (file == info.modFilename)
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
                    if (modList[i].modFilename == file)
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

        private void timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < modList.Count; i++)
            {
                if (modList[i].updateState != modListView.Items[i].SubItems[5].Text)
                {
                    modListView.Items[i].SubItems[5].Text = modList[i].updateState;
                }
            }

            bool allDone = true;
            bool allDownloadDone = true;
            foreach (ModInfo mod in modList)
            {
                if (mod.findQueued && !mod.findBusy)
                {
                    LockButtons();
                    mod.FindWebsiteUri();
                }
                else if (mod.checkQueued && !mod.checkBusy)
                {
                    LockButtons();
                    mod.CheckForUpdate();
                }
                else if (mod.downloadQueued && !mod.downloadBusy && !downloadBusy)
                {
                    LockButtons();
                    downloadBusy = true;
                    mod.UpdateMod();
                }

                if (mod.downloadBusy)
                {
                    allDownloadDone = false;
                }
                if (mod.checkQueued || mod.downloadQueued || mod.findQueued)
                {
                    allDone = false;
                }

                if (mod.updateList)
                {
                    mod.updateList = false;
                    UpdateModList();
                    break;
                }
            }

            if (allDownloadDone)
            {
                downloadBusy = false;
            }
            if (allDone && action)
            {
                UpdateModList();
                UnlockButtons();
            }
        }

        private void LockButtons()
        {
            action = true;
            findSiteButton.Enabled = false;
            updateCheckButton.Enabled = false;
            updateModsButton.Enabled = false;
            updateSelectedButton.Enabled = false;
        }

        private void UnlockButtons()
        {
            findSiteButton.Enabled = true;
            updateCheckButton.Enabled = true;
            updateModsButton.Enabled = true;
            updateSelectedButton.Enabled = selectedMod.canUpdate;
            action = false;
        }

        private void UpdateListView()
        {
            modListView.Items.Clear();

            foreach (ModInfo mod in modList)
            {
                ListViewItem lvi = new ListViewItem(mod.modFilename);

                lvi.SubItems.Add(mod.uriState);
                lvi.SubItems.Add(mod.versionLocal);
                lvi.SubItems.Add(mod.versionLatest);
                lvi.SubItems.Add(mod.releaseDate);
                lvi.SubItems.Add(mod.updateState);

                lvi.Checked = !mod.disabled;
                modListView.Items.Add(lvi);
            }

            //Selection
            modListView.SelectedIndices.Clear();
            int index = 0;

            if (selectedMod != null)
            {
                for (int i = 0; i < modListView.Items.Count; i++)
                {
                    string item = modListView.Items[i].SubItems[0].Text;
                    if (item == selectedMod.modFilename)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index >= 0 && modListView.Items.Count > 0)
            {
                modListView.SelectedIndices.Add(index);
                SelectItem(index);
            }
            else
            {
                ClearSelection();
            }
        }

        private void UpdateModFields()
        {
            updatingFields = true;
            modNameBox.Text = selectedMod.modFilename;
            modSiteBox.Text = selectedMod.website;
            modDownloadBox.Text = selectedMod.dlSite;
            linkStatusLabel.Text = selectedMod.uriState;
            canUpdateBox.Checked = selectedMod.canUpdate;
            updateSelectedButton.Enabled = selectedMod.canUpdate;
            updatingFields = false;
        }

        private void ClearSelection()
        {
            updatingFields = true;
            selectedMod = null;
            modNameBox.Text = "";
            modSiteBox.Text = "";
            modDownloadBox.Text = "";
            linkStatusLabel.Text = "";
            canUpdateBox.Checked = false;
            updatingFields = false;
        }

        private void SelectItem(int index)
        {
            selectedMod = modList[index];
            selectedIndex = index;
            UpdateModFields();
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
            if (selectedMod != null && !updatingFields)
            {
                selectedMod.website = modSiteBox.Text;
                selectedMod.UpdateModValues();
                modListView.Items[selectedIndex].SubItems[1].Text = selectedMod.uriState;
                linkStatusLabel.Text = selectedMod.uriState;
                modDownloadBox.Text = selectedMod.dlSite;
            }
        }

        private void canUpdateBox_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedMod != null && !updatingFields)
            {
                selectedMod.canUpdate = canUpdateBox.Checked;
                updateSelectedButton.Enabled = selectedMod.canUpdate;
                modListView.Items[selectedIndex].SubItems[5].Text = selectedMod.updateState;
            }
        }

        private void openSiteButton_Click(object sender, EventArgs e)
        {
            Process.Start(selectedMod.website);
        }

        private void findSiteButton_Click(object sender, EventArgs e)
        {
            foreach (ModInfo mod in modList)
            {
                if (mod.siteMode == "NONE")
                {
                    //LockButtons();
                    mod.findQueued = true;
                }
            }
        }

        private void updateCheckButton_Click(object sender, EventArgs e)
        {
            foreach (ModInfo mod in modList)
            {
                if (mod.siteMode != "NONE")
                {
                    //LockButtons();
                    mod.checkQueued = true;
                }
            }
        }

        private void updateModsButton_Click(object sender, EventArgs e)
        {
            foreach (ModInfo mod in modList)
            {
                if (mod.canUpdate)
                {
                    //LockButtons();
                    mod.downloadQueued = true;
                }
            }
        }

        private void updateSelectedButton_Click(object sender, EventArgs e)
        {
            if (selectedMod.canUpdate)
            {
                //LockButtons();
                selectedMod.downloadQueued = true;
            }
        }

        private void modListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            modList[e.Item.Index].disabled = !e.Item.Checked;
            modList[e.Item.Index].SetFileNames();
            e.Item.SubItems[0].Text = modList[e.Item.Index].modFilename;
        }
    }
}
