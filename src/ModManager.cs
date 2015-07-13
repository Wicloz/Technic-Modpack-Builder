﻿using System;
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

        public ModManager()
        {
            InitializeComponent();
            LoadModList();
            UpdateModList();

            // Initiate timer
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 100;
            timer.Start();
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
                if (modList[i].updateState != modListView.Items[i].SubItems[4].Text)
                {
                    modListView.Items[i].SubItems[4].Text = modList[i].updateState;
                }
            }
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
                lvi.SubItems.Add(mod.updateState);

                modListView.Items.Add(lvi);
            }

            // Selection
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

        private void UpdateModFields()
        {
            modNameBox.Text = selectedMod.modFilename;
            modSiteBox.Text = selectedMod.website;
            modDownloadBox.Text = selectedMod.dlSite;
            linkStatusLabel.Text = selectedMod.uriState;
        }

        private void ClearSelection()
        {
            selectedMod = null;
            modNameBox.Text = "";
            modSiteBox.Text = "";
            modDownloadBox.Text = "";
            linkStatusLabel.Text = "";
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
            selectedMod.website = modSiteBox.Text;
            selectedMod.UpdateModValues();
            modListView.Items[selectedIndex].SubItems[1].Text = selectedMod.uriState;
            linkStatusLabel.Text = selectedMod.uriState;
            modDownloadBox.Text = selectedMod.dlSite;
        }

        private void openSiteButton_Click(object sender, EventArgs e)
        {
            Process.Start(selectedMod.website);
        }

        private void updateCheckButton_Click(object sender, EventArgs e)
        {
            foreach (ModInfo mod in modList)
            {
                if (mod.siteMode != "NONE")
                {
                    mod.checkQueued = true;
                    mod.CheckForUpdate();
                }
            }
        }

        private void updateModsButton_Click(object sender, EventArgs e)
        {
            foreach (ModInfo mod in modList)
            {
                if (mod.siteMode != "NONE" && mod.versionLocalRaw != mod.versionLatestRaw)
                {
                    mod.downloadQueued = true;
                    mod.UpdateMod();
                }
            }
        }
    }
}
