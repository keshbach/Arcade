/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class ListPartInventoryForm : Arcade.Forms.Form
    {
        #region "Member Variables"
        private System.Int32 m_nPartId = -1;

        private System.Boolean m_bInventoryChanged = false;
        #endregion

        #region "Constructor"
        public ListPartInventoryForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Properties"
        public System.Int32 PartId
        {
            set
            {
                m_nPartId = value;
            }
        }

        public System.Boolean InventoryChanged
        {
            get
            {
                return m_bInventoryChanged;
            }
        }
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { listViewPartInventory };
            }
        }
        #endregion

        #region "List Part Inventory Event Handlers"
        private void ListPartInventoryForm_Shown(object sender, EventArgs e)
        {
            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                InitializeControls();

                RunOnUIThreadWait(() =>
                {
                    this.BusyControlVisible = false;
                });
            }, "List Part Inventory Form Initialize Thread");
        }
        #endregion

        #region "List View Event Handlers"
        private void listViewPartInventory_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            buttonEdit.Enabled = true;
            buttonDelete.Enabled = true;
        }

        private void listViewPartInventory_DoubleClick(object sender, EventArgs e)
        {
            EditPartInventory();
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            InventoryEntryForm InventoryEntry = new InventoryEntryForm();
            System.Boolean bResult;
            System.String sErrorMessage;
            System.Int32 nInventoryId;
            System.Windows.Forms.ListViewItem Item;
            DatabaseDefs.TInventory Inventory;

            new Common.Forms.FormLocation(InventoryEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            InventoryEntry.InventoryEntryFormType = InventoryEntryForm.EInventoryEntryFormType.NewInventory;

            if (System.Windows.Forms.DialogResult.OK == InventoryEntry.ShowDialog(this))
            {
                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.AddPartInventoryEntry(m_nPartId,
                                                             InventoryEntry.InventoryDateTime,
                                                             InventoryEntry.InventoryCount,
                                                             InventoryEntry.InventoryDescription,
                                                             out nInventoryId,
                                                             out sErrorMessage);

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
                        {
                            listViewPartInventory.BeginUpdate();

                            Item = listViewPartInventory.Items.Add(InventoryEntry.InventoryDateTime.ToShortDateString());

                            Item.SubItems.Add(InventoryEntry.InventoryCount.ToString());

                            Inventory = new DatabaseDefs.TInventory();

                            Inventory.nInventoryId = nInventoryId;
                            Inventory.DateTime = InventoryEntry.InventoryDateTime;
                            Inventory.sInventoryDescription = InventoryEntry.InventoryDescription;
                            Inventory.nCount = InventoryEntry.InventoryCount;

                            Item.Tag = Inventory;
                            Item.Selected = true;
                            Item.Focused = true;

                            Item.EnsureVisible();

                            listViewPartInventory.Enabled = true;

                            listViewPartInventory.EndUpdate();

                            m_bInventoryChanged = true;
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }

                        this.BusyControlVisible = false;

                        InventoryEntry.Dispose();
                    });
                }, "List Part Inventory Form Add Thread");
            }
            else
            {
                InventoryEntry.Dispose();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditPartInventory();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            System.Int32 nIndex = listViewPartInventory.SelectedIndices[0];
            System.Boolean bResult;
            System.String sErrorMessage;
            DatabaseDefs.TInventory Inventory;

            Inventory = (DatabaseDefs.TInventory)listViewPartInventory.Items[nIndex].Tag;

            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                bResult = Database.DeletePartInventoryEntry(Inventory.nInventoryId, out sErrorMessage);

                RunOnUIThreadWait(() =>
                {
                    if (bResult)
                    {
                        listViewPartInventory.BeginUpdate();
                        listViewPartInventory.Items.RemoveAt(nIndex);

                        if (listViewPartInventory.Items.Count > 0)
                        {
                            if (listViewPartInventory.Items.Count == nIndex)
                            {
                                --nIndex;
                            }

                            listViewPartInventory.Items[nIndex].Selected = true;
                            listViewPartInventory.Items[nIndex].Focused = true;

                            listViewPartInventory.Items[nIndex].EnsureVisible();
                        }
                        else
                        {
                            listViewPartInventory.Enabled = false;

                            buttonEdit.Enabled = false;
                            buttonDelete.Enabled = false;
                        }

                        listViewPartInventory.EndUpdate();

                        m_bInventoryChanged = true;
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }

                    this.BusyControlVisible = false;
                });
            }, "List Part Inventory Form Delete Thread");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region "Internal Helpers"
        private void EditPartInventory()
        {
            System.Int32 nIndex = listViewPartInventory.SelectedIndices[0];
            InventoryEntryForm InventoryEntry = new InventoryEntryForm();
            System.Boolean bResult;
            System.String sErrorMessage;
            DatabaseDefs.TInventory Inventory;

            new Common.Forms.FormLocation(InventoryEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            Inventory = (DatabaseDefs.TInventory)listViewPartInventory.Items[nIndex].Tag;

            InventoryEntry.InventoryEntryFormType = InventoryEntryForm.EInventoryEntryFormType.EditInventory;
            InventoryEntry.InventoryDateTime = Inventory.DateTime;
            InventoryEntry.InventoryCount = Inventory.nCount;
            InventoryEntry.InventoryDescription = Inventory.sInventoryDescription;

            if (System.Windows.Forms.DialogResult.OK == InventoryEntry.ShowDialog(this))
            {
                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.EditPartInventoryEntry(Inventory.nInventoryId,
                                                              InventoryEntry.InventoryDateTime,
                                                              InventoryEntry.InventoryCount,
                                                              InventoryEntry.InventoryDescription,
                                                              out sErrorMessage);

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
                        {
                            Inventory.DateTime = InventoryEntry.InventoryDateTime;
                            Inventory.nCount = InventoryEntry.InventoryCount;
                            Inventory.sInventoryDescription = InventoryEntry.InventoryDescription;

                            listViewPartInventory.BeginUpdate();

                            listViewPartInventory.Items[nIndex].Text = InventoryEntry.InventoryDateTime.ToShortDateString();
                            listViewPartInventory.Items[nIndex].SubItems[1].Text = InventoryEntry.InventoryCount.ToString();
                            listViewPartInventory.Items[nIndex].Tag = Inventory;

                            listViewPartInventory.EndUpdate();

                            m_bInventoryChanged = true;
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }

                        this.BusyControlVisible = false;

                        InventoryEntry.Dispose();
                    });
                }, "List Part Inventory Form Edit Thread");
            }
            else
            {
                InventoryEntry.Dispose();
            }
        }

        private void InitializeControls()
        {
            System.Collections.Generic.List<DatabaseDefs.TInventory> InventoryList;
            System.Boolean bResult;
            System.String sErrorMessage;
            System.Windows.Forms.ListViewItem Item;

            Common.Debug.Thread.IsWorkerThread();

            bResult = Database.GetInventoryForPart(m_nPartId, out InventoryList, out sErrorMessage);

            RunOnUIThreadWait(() =>
            {
                buttonEdit.Enabled = false;
                buttonDelete.Enabled = false;
                listViewPartInventory.Enabled = false;

                if (bResult)
                {
                    listViewPartInventory.BeginUpdate();

                    foreach (DatabaseDefs.TInventory Inventory in InventoryList)
                    {
                        Item = listViewPartInventory.Items.Add(Inventory.DateTime.ToShortDateString());

                        Item.SubItems.Add(Inventory.nCount.ToString());

                        Item.Tag = Inventory;
                    }

                    if (listViewPartInventory.Items.Count > 0)
                    {
                        listViewPartInventory.Enabled = true;
                    }

                    listViewPartInventory.EndUpdate();
                }
                else
                {
                    Common.Forms.MessageBox.Show(this, sErrorMessage,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);

                    buttonAdd.Enabled = false;
                }
            });
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
