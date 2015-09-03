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
using System.Net;

namespace Technic_Modpack_Creator
{
    public partial class ModManager : Form
    {
        private List<ModInfo> modList = new List<ModInfo>();
        private string cd = Directory.GetCurrentDirectory();
        private ModUriDatabase uriDatabase = new ModUriDatabase();

        private ModInfo selectedMod;
        private int selectedIndex = -1;
        private bool action = false;
        private bool updatingFields = false;
        private bool updatingList = false;
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
            uriDatabase.LoadDatabases();
            LoadModList();
            UpdateModList();
        }

        private void ModManager_Shown(object sender, EventArgs e)
        {
            // Download uri database
            WebClient client = new WebClient();
            client.DownloadFileAsync(new Uri("https://dl.dropboxusercontent.com/u/46484032/TMC/uridatabase.dat"), cd + "\\settings\\globaldatabase.dat");
        }

        private void ModManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveModList();
            uriDatabase.SaveDatabases();
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
            foreach (ModInfo mod in modList)
            {
                uriDatabase.SetSite(mod.modFileName, mod.website);
            }
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
                    if (file == info.modFileName)
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
                    if (modList[i].modFileName == file)
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
                    if (modList[i].uriState == "")
                    {
                        modList[i].website = uriDatabase.GetSuperiorSite(modList[i].modFileName, modList[i].website);
                    }
                    modList[i].UpdateModValues();
                }
            }

            modList.Sort();
            SaveModList();
            UpdateListView();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            bool allDone = true;
            bool allDownloadDone = true;

            for (int i = 0; i < modList.Count; i++)
            {
                ModInfo mod = modList[i];

                if (mod.updateState != modListView.Items[i].SubItems[5].Text)
                {
                    modListView.Items[i].SubItems[5].Text = mod.updateState;
                }

                if (mod.findQueued && !mod.isWorking)
                {
                    LockButtons();
                    mod.FindWebsiteUri();
                }
                else if (mod.checkQueued && !mod.isWorking)
                {
                    LockButtons();
                    mod.CheckForUpdate(Main.acces.mcVersion);
                }
                else if (mod.downloadQueued && !mod.isWorking && !downloadBusy)
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

                    if (selectedIndex == i)
                    {
                        UpdateModFields();
                    }

                    modListView.Items[i].SubItems[1].Text = mod.uriState;
                    modListView.Items[i].SubItems[2].Text = mod.versionLocal;
                    modListView.Items[i].SubItems[3].Text = mod.versionLatest;
                    modListView.Items[i].SubItems[4].Text = mod.releaseDate;

                    modListView.Items[i].SubItems[6].Text = mod.newFileName;
                    modListView.Items[i].SubItems[7].Text = mod.website1;
                    modListView.Items[i].SubItems[8].Text = mod.website2;
                    modListView.Items[i].SubItems[9].Text = mod.website3;
                    modListView.Items[i].SubItems[10].Text = mod.website4;
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
            updatingList = true;
            modListView.Items.Clear();

            foreach (ModInfo mod in modList)
            {
                ListViewItem lvi = new ListViewItem(mod.modFileName);

                lvi.SubItems.Add(mod.uriState);
                lvi.SubItems.Add(mod.versionLocal);
                lvi.SubItems.Add(mod.versionLatest);
                lvi.SubItems.Add(mod.releaseDate);
                lvi.SubItems.Add(mod.updateState);
                lvi.SubItems.Add(mod.newFileName);
                lvi.SubItems.Add(mod.website1);
                lvi.SubItems.Add(mod.website2);
                lvi.SubItems.Add(mod.website3);
                lvi.SubItems.Add(mod.website4);

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
                    if (item == selectedMod.modFileName)
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
                modListView.Select();
            }
            else
            {
                ClearSelection();
            }

            updatingList = false;
        }

        private void UpdateModFields()
        {
            updatingFields = true;
            modNameBox.Text = selectedMod.modFileName;
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
            if (selectedMod.website != "NONE")
            {
                Process.Start(selectedMod.website);
            }
        }

        private void googleButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.google.com/?gws_rd=ssl#q=" + selectedMod.modFileName.Replace(" ","") + "+minecraft");
            Process.Start("https://www.google.com/?gws_rd=ssl#q=" + MiscFunctions.CleanString(selectedMod.modFileName) + "+minecraft");
        }

        private void findSiteButton_Click(object sender, EventArgs e)
        {
            foreach (ModInfo mod in modList)
            {
                if (mod.uriState == "")
                {
                    mod.findQueued = true;
                }
            }
        }

        private void updateCheckButton_Click(object sender, EventArgs e)
        {
            foreach (ModInfo mod in modList)
            {
                if (mod.uriState != "")
                {
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
                    mod.downloadQueued = true;
                }
            }
        }

        private void updateSelectedButton_Click(object sender, EventArgs e)
        {
            if (selectedMod.canUpdate)
            {
                selectedMod.downloadQueued = true;
                selectedMod.downloadBusy = false;
            }
        }

        private void findSelectedButton_Click(object sender, EventArgs e)
        {
            selectedMod.findQueued = true;
        }

        private void checkSelectedButton_Click(object sender, EventArgs e)
        {
            selectedMod.checkQueued = true;
        }

        private void modListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!updatingList)
            {
                modList[e.Item.Index].disabled = !e.Item.Checked;
                modList[e.Item.Index].SetFileNames();
                e.Item.SubItems[0].Text = modList[e.Item.Index].modFileName;
            }
        }

        private void loadDataButton_Click(object sender, EventArgs e)
        {
            foreach (ModInfo mod in modList)
            {
                mod.website = uriDatabase.GetSuperiorSite(mod.modFileName, mod.website);
                mod.UpdateModValues();
            }
            UpdateModList();
        }

        private void writeDataButton_Click(object sender, EventArgs e)
        {
            foreach (ModInfo mod in modList)
            {
                uriDatabase.ForceSetSite(mod.modFileName, mod.website);
            }
            uriDatabase.SaveDatabases();
        }
    }
}
