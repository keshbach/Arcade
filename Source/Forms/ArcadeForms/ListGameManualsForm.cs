/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class ListGameManualsForm : Arcade.Forms.Form
    {
        #region "Member Variables"
        private System.Int32 m_nGameId = -1;
        #endregion

        #region "Constructor"
        public ListGameManualsForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Properties"
        public System.Int32 GameId
        {
            set
            {
                m_nGameId = value;
            }
        }
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { listViewManuals };
            }
        }
        #endregion

        #region "List Game Manuals Event Handlers"
        private void ListGameManualsForm_Shown(object sender, EventArgs e)
        {
            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                InitializeControls();

                RunOnUIThreadWait(() =>
                {
                    this.BusyControlVisible = false;
                });
            }, "List Game Manuals Form Initialize Thread");
        }
        #endregion

        #region "List View Event Handlers"
        private void listViewManuals_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            buttonDelete.Enabled = true;
            buttonView.Enabled = true;
        }

        private void listViewManuals_DoubleClick(object sender, EventArgs e)
        {
            ViewManual();
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            ListManualsForm ListManuals = new ListManualsForm();
            System.Boolean bResult;
            System.String sErrorMessage;
            DatabaseDefs.TManual Manual;
            System.Windows.Forms.ListViewItem Item;

            new Common.Forms.FormLocation(ListManuals, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            ListManuals.ListManualsFormType = ListManualsForm.EListManualsFormType.ListManuals;

            if (ListManuals.ShowDialog(this) == DialogResult.OK)
            {
                if (DoesGameHaveManual(ListManuals.ManualId))
                {
                    Common.Forms.MessageBox.Show(this, "This game already has that manual.",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);

                    ListManuals.Dispose();

                    return;
                }

                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.AddGameManual(m_nGameId, ListManuals.ManualId,
                                                     out sErrorMessage);

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
                        {
                            Manual = new DatabaseDefs.TManual();

                            Manual.nManualId = ListManuals.ManualId;
                            Manual.sManualName = ListManuals.ManualName;
                            Manual.sManualPartNumber = ListManuals.PartNumber;
                            Manual.nManualYearPrinted = ListManuals.YearPrinted;
                            Manual.bManualComplete = ListManuals.Complete;
                            Manual.bManualOriginal = ListManuals.Original;
                            Manual.sManualDescription = ListManuals.Description;
                            Manual.sManualPrintEdition = ListManuals.PrintEdition;
                            Manual.sManualCondition = ListManuals.Condition;
                            Manual.sManufacturer = ListManuals.Manufacturer;
                            Manual.sManualStorageBox = ListManuals.StorageBox;

                            listViewManuals.Enabled = true;

                            Item = listViewManuals.Items.Add(Manual.sManualName);

                            Item.Tag = Manual;

                            Item.SubItems.Add(Manual.sManualStorageBox);
                            Item.SubItems.Add(Manual.sManualPrintEdition);

                            Item.Selected = true;
                            Item.Focused = true;

                            Item.EnsureVisible();
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }

                        this.BusyControlVisible = false;

                        ListManuals.Dispose();
                    });
                }, "List Game Manuals Form Add Thread");
            }
            else
            {
                ListManuals.Dispose();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            System.Int32 nIndex = listViewManuals.SelectedIndices[0];
            DatabaseDefs.TManual Manual = (DatabaseDefs.TManual)listViewManuals.Items[nIndex].Tag;
            System.Boolean bResult;
            System.String sErrorMessage;
            System.Windows.Forms.ListViewItem Item;

            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                bResult = Database.DeleteGameManual(m_nGameId, Manual.nManualId,
                                                    out sErrorMessage);

                RunOnUIThreadWait(() =>
                {
                    if (bResult)
                    {
                        listViewManuals.Items.RemoveAt(nIndex);

                        if (listViewManuals.Items.Count > 0)
                        {
                            if (nIndex == listViewManuals.Items.Count)
                            {
                                --nIndex;
                            }

                            Item = listViewManuals.Items[nIndex];

                            Item.Selected = true;
                            Item.Focused = true;

                            Item.EnsureVisible();
                        }
                        else
                        {
                            listViewManuals.Enabled = false;

                            buttonDelete.Enabled = false;
                            buttonView.Enabled = false;
                        }
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }

                    this.BusyControlVisible = false;
                });
            }, "List Game Manuals Form Delete Thread");
        }

        private void buttonView_Click(object sender, EventArgs e)
        {
            ViewManual();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region "Internal Helpers"
        private void ViewManual()
        {
            System.Int32 nIndex = listViewManuals.SelectedIndices[0];
            ManualEntryForm ManualEntry = new ManualEntryForm();
            DatabaseDefs.TManual Manual;

            Common.Debug.Thread.IsUIThread();

            Manual = (DatabaseDefs.TManual)listViewManuals.Items[nIndex].Tag;

            new Common.Forms.FormLocation(ManualEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            ManualEntry.ManualEntryFormType = ManualEntryForm.EManualEntryFormType.ViewManual;
            ManualEntry.ManualName = Manual.sManualName;
            ManualEntry.StorageBox = Manual.sManualStorageBox;
            ManualEntry.PartNumber = Manual.sManualPartNumber;
            ManualEntry.YearPrinted = Manual.nManualYearPrinted;
            ManualEntry.PrintEdition = Manual.sManualPrintEdition;
            ManualEntry.Condition = Manual.sManualCondition;
            ManualEntry.Manufacturer = Manual.sManufacturer;
            ManualEntry.Complete = Manual.bManualComplete;
            ManualEntry.Original = Manual.bManualOriginal;
            ManualEntry.Description = Manual.sManualDescription;

            ManualEntry.ShowDialog(this);

            ManualEntry.Dispose();
        }

        private System.Boolean DoesGameHaveManual(
            System.Int32 nManualId)
        {
            DatabaseDefs.TManual Manual;

            Common.Debug.Thread.IsUIThread();

            foreach (System.Windows.Forms.ListViewItem Item in listViewManuals.Items)
            {
                Manual = (DatabaseDefs.TManual)Item.Tag;

                if (nManualId == Manual.nManualId)
                {
                    return true;
                }
            }

            return false;
        }

        private void InitializeControls()
        {
            System.Collections.Generic.List<DatabaseDefs.TManual> ManualsList;
            System.Boolean bResult;
            System.String sErrorMessage;
            System.Windows.Forms.ListViewItem Item;

            Common.Debug.Thread.IsWorkerThread();

            bResult = Database.GetManualsForGame(m_nGameId, out ManualsList,
                                                 out sErrorMessage);

            RunOnUIThreadWait(() =>
            {
                buttonDelete.Enabled = false;
                buttonView.Enabled = false;

                if (bResult)
                {
                    listViewManuals.BeginUpdate();

                    foreach (DatabaseDefs.TManual Manual in ManualsList)
                    {
                        Item = listViewManuals.Items.Add(Manual.sManualName);

                        Item.Tag = Manual;

                        Item.SubItems.Add(Manual.sManualStorageBox);
                        Item.SubItems.Add(Manual.sManualPrintEdition);
                    }

                    listViewManuals.EndUpdate();
                }
                else
                {
                    Common.Forms.MessageBox.Show(this, sErrorMessage,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);

                    buttonAdd.Enabled = false;
                }

                if (listViewManuals.Items.Count == 0)
                {
                    listViewManuals.Enabled = false;
                }
            });
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
