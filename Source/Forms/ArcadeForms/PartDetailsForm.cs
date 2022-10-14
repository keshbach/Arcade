/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class PartDetailsForm : Arcade.Forms.Form
    {
        #region "Member Variables"
        private System.Int32 m_nPartId = -1;
        private System.String m_sSelectedPartName = "";
        #endregion

        #region "Constructor"
        public PartDetailsForm()
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

        public System.String SelectedPartName
        {
            set
            {
                m_sSelectedPartName = value;
            }
        }
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { listViewKeywords, splitContainerPart };
            }
        }

        protected override System.Windows.Forms.Control[] ControlClearSelection
        {
            get
            {
                return new System.Windows.Forms.Control[] { textBoxCategory, textBoxType, textBoxPackage, textBoxPinouts };
            }
        }
        #endregion

        #region "Part Details Event Handlers"
        private void PartDetailsForm_Shown(object sender, EventArgs e)
        {
            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                InitializeControls();

                RunOnUIThreadWait(() =>
                {
                    this.BusyControlVisible = false;
                });
            }, "Part Details Form Initialize Thread");
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonAdd_Click(object sender, System.EventArgs e)
        {
            PartEntryForm PartEntry = new PartEntryForm();
            System.Int32 nIndex = listViewKeywords.SelectedIndices[0];
            System.Int32 nNewPartId;
            System.String sErrorMessage;
            DatabaseDefs.TPart Part;
            System.Windows.Forms.ListViewItem Item;
            System.Boolean bResult;

            new Common.Forms.FormLocation(PartEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            Part = (DatabaseDefs.TPart)listViewKeywords.Items[nIndex].Tag;

            PartEntry.PartEntryFormType = PartEntryForm.EPartEntryFormType.AddPartName;
            PartEntry.PartCategoryName = Part.sPartCategoryName;
            PartEntry.PartTypeName = Part.sPartTypeName;
            PartEntry.PartPackageName = Part.sPartPackageName;
            PartEntry.DefaultPartName = false;
            PartEntry.PartPinouts = textBoxPinouts.Text;

            if (System.Windows.Forms.DialogResult.OK == PartEntry.ShowDialog(this))
            {
                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.AddPart(m_nPartId, PartEntry.PartName,
                                               PartEntry.PartPackageName,
                                               PartEntry.PartDatasheetColl,
                                               out nNewPartId,
                                               out sErrorMessage);

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
                        {
                            Part = new DatabaseDefs.TPart();

                            Part.nPartId = nNewPartId;
                            Part.sPartName = PartEntry.PartName;
                            Part.sPartCategoryName = PartEntry.PartCategoryName;
                            Part.sPartTypeName = PartEntry.PartTypeName;
                            Part.sPartPackageName = PartEntry.PartPackageName;
                            Part.bPartIsDefault = PartEntry.DefaultPartName;
                            Part.PartDatasheetColl = PartEntry.PartDatasheetColl;

                            Item = new System.Windows.Forms.ListViewItem();

                            Item.Text = PartEntry.PartName;
                            Item.Tag = Part;

                            if (Part.PartDatasheetColl.Count > 0)
                            {
                                Item.SubItems.Add("*");

                                buttonDatasheets.Enabled = true;
                            }
                            else
                            {
                                Item.SubItems.Add("");

                                buttonDatasheets.Enabled = false;
                            }

                            Item.SubItems.Add("-");

                            listViewKeywords.Items.Add(Item);

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

                        PartEntry.Dispose();
                    });
                }, "Part Details Form Add Thread");
            }
            else
            {
                PartEntry.Dispose();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditPart();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            System.Int32 nIndex = listViewKeywords.SelectedIndices[0];
            System.Boolean bResetPartId = false;
            System.Int32 nNewDefPartId;
            System.String sErrorMessage;
            DatabaseDefs.TPart Part;
            System.Boolean bResult;

            Part = (DatabaseDefs.TPart)listViewKeywords.Items[nIndex].Tag;

            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                bResult = Database.DeletePart(Part.nPartId, out nNewDefPartId,
                                              out sErrorMessage);

                RunOnUIThreadWait(() =>
                {
                    if (bResult)
                    {
                        if (m_nPartId == Part.nPartId)
                        {
                            bResetPartId = true;
                        }

                        listViewKeywords.Items.RemoveAt(nIndex);

                        if (listViewKeywords.Items.Count > 0)
                        {
                            for (System.Int32 nItemIndex = 0;
                                    nItemIndex < listViewKeywords.Items.Count;
                                    ++nItemIndex)
                            {
                                Part = (DatabaseDefs.TPart)listViewKeywords.Items[nItemIndex].Tag;

                                if (Part.nPartId != nNewDefPartId)
                                {
                                    listViewKeywords.Items[nItemIndex].Font = new System.Drawing.Font(listViewKeywords.Items[nItemIndex].Font,
                                                                                                      FontStyle.Regular);
                                }
                                else
                                {
                                    Part.bPartIsDefault = true;

                                    listViewKeywords.Items[nItemIndex].Font = new System.Drawing.Font(listViewKeywords.Items[nItemIndex].Font,
                                                                                                      FontStyle.Bold);

                                    listViewKeywords.Items[nItemIndex].Tag = Part;
                                }
                            }

                            if (nIndex == listViewKeywords.Items.Count)
                            {
                                --nIndex;
                            }

                            listViewKeywords.Items[nIndex].Selected = true;
                            listViewKeywords.Items[nIndex].Focused = true;

                            listViewKeywords.Items[nIndex].EnsureVisible();

                            listViewKeywords.AutosizeColumns();

                            if (bResetPartId == true)
                            {
                                Part = (DatabaseDefs.TPart)listViewKeywords.Items[nIndex].Tag;

                                m_nPartId = Part.nPartId;
                            }
                        }
                        else
                        {
                            m_nPartId = -1;

                            listViewKeywords.Enabled = false;

                            buttonAdd.Enabled = false;
                            buttonEdit.Enabled = false;
                            buttonDelete.Enabled = false;
                            buttonDatasheets.Enabled = false;
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
            }, "Part Details Form Delete Thread");
        }

        private void buttonDatasheets_Click(object sender, EventArgs e)
        {
            System.Int32 nIndex = listViewKeywords.SelectedIndices[0];
            ViewPartDatasheetsForm ViewPartDatasheets = new ViewPartDatasheetsForm();
            DatabaseDefs.TPart Part;

            new Common.Forms.FormLocation(ViewPartDatasheets, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            Part = (DatabaseDefs.TPart)listViewKeywords.Items[nIndex].Tag;

            ViewPartDatasheets.PartDatasheetColl = Part.PartDatasheetColl;

            ViewPartDatasheets.ShowDialog(this);
        }

        private void buttonInventory_Click(object sender, EventArgs e)
        {
            System.Int32 nIndex = listViewKeywords.SelectedIndices[0];
            System.Windows.Forms.ListViewItem Item = listViewKeywords.Items[nIndex];
            DatabaseDefs.TPart Part = (DatabaseDefs.TPart)Item.Tag;
            System.Boolean bResult;
            System.Int32 nInventoryTotal;
            System.String sErrorMessage;

            ListPartInventoryForm ListPartInventory = new ListPartInventoryForm();

            new Common.Forms.FormLocation(ListPartInventory, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            ListPartInventory.PartId = Part.nPartId;

            ListPartInventory.ShowDialog();

            if (ListPartInventory.InventoryChanged)
            {
                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.GetInventoryTotalForPart(Part.nPartId,
                                                                out nInventoryTotal,
                                                                out sErrorMessage);

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
                        {
                            Part.nPartTotalInventory = nInventoryTotal;

                            Item.SubItems[2].Text = FormatInventory(nInventoryTotal);
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }

                        this.BusyControlVisible = false;
                    });
                }, "Part Details Form Inventory Thread");
            }

            ListPartInventory.Dispose();
        }

        private void buttonClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }
        #endregion

        #region "List View Event Handlers"
        private void listViewKeywords_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            DatabaseDefs.TPart Part;
            System.Int32 nIndex;

            if (listViewKeywords.SelectedIndices.Count > 0)
            {
                nIndex = listViewKeywords.SelectedIndices[0];
                Part = (DatabaseDefs.TPart)listViewKeywords.Items[nIndex].Tag;

                textBoxCategory.Text = Part.sPartCategoryName;
                textBoxType.Text = Part.sPartTypeName;
                textBoxPackage.Text = Part.sPartPackageName;

                buttonDatasheets.Enabled = (Part.PartDatasheetColl.Count > 0) ? true : false;

                buttonEdit.Enabled = true;
            }
        }

        private void listViewKeywords_DoubleClick(object sender, EventArgs e)
        {
            EditPart();
        }
        #endregion

        #region "Internal Helpers"
        private void EditPart()
        {
            PartEntryForm PartEntry = new PartEntryForm();
            System.Int32 nIndex = listViewKeywords.SelectedIndices[0];
            System.Boolean bResult;
            System.String sErrorMessage;
            DatabaseDefs.TPart Part, TmpPart;

            Common.Debug.Thread.IsUIThread();

            new Common.Forms.FormLocation(PartEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            Part = (DatabaseDefs.TPart)listViewKeywords.Items[nIndex].Tag;

            PartEntry.PartEntryFormType = PartEntryForm.EPartEntryFormType.EditPart;
            PartEntry.PartId = Part.nPartId;
            PartEntry.PartName = Part.sPartName;
            PartEntry.PartCategoryName = Part.sPartCategoryName;
            PartEntry.PartTypeName = Part.sPartTypeName;
            PartEntry.PartPackageName = Part.sPartPackageName;
            PartEntry.DefaultPartName = Part.bPartIsDefault;
            PartEntry.PartPinouts = textBoxPinouts.Text;
            PartEntry.PartDatasheetColl = Part.PartDatasheetColl;

            if (System.Windows.Forms.DialogResult.OK == PartEntry.ShowDialog(this))
            {
                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.EditPart(Part.nPartId, PartEntry.PartName,
                                                PartEntry.PartCategoryName,
                                                PartEntry.PartTypeName,
                                                PartEntry.PartPackageName,
                                                PartEntry.PartPinouts,
                                                PartEntry.DefaultPartName,
                                                PartEntry.PartDatasheetColl,
                                                out sErrorMessage);

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
                        {
                            if (Part.bPartIsDefault == false &&
                                PartEntry.DefaultPartName == true)
                            {
                                for (System.Int32 nItemIndex = 0;
                                     nItemIndex < listViewKeywords.Items.Count;
                                     ++nItemIndex)
                                {
                                    if (nItemIndex != nIndex)
                                    {
                                        TmpPart = (DatabaseDefs.TPart)listViewKeywords.Items[nItemIndex].Tag;

                                        TmpPart.bPartIsDefault = false;

                                        listViewKeywords.Items[nItemIndex].Font = new System.Drawing.Font(listViewKeywords.Items[nItemIndex].Font,
                                                                                                          FontStyle.Regular);
                                        listViewKeywords.Items[nItemIndex].Tag = TmpPart;
                                    }
                                    else
                                    {
                                        listViewKeywords.Items[nItemIndex].Font = new System.Drawing.Font(listViewKeywords.Items[nItemIndex].Font,
                                                                                                          FontStyle.Bold);
                                    }
                                }
                            }

                            Part.sPartName = PartEntry.PartName;
                            Part.sPartCategoryName = PartEntry.PartCategoryName;
                            Part.sPartTypeName = PartEntry.PartTypeName;
                            Part.sPartPackageName = PartEntry.PartPackageName;
                            Part.bPartIsDefault = PartEntry.DefaultPartName;
                            Part.PartDatasheetColl = PartEntry.PartDatasheetColl;

                            textBoxCategory.Text = PartEntry.PartCategoryName;
                            textBoxType.Text = PartEntry.PartTypeName;
                            textBoxPackage.Text = PartEntry.PartPackageName;
                            textBoxPinouts.Text = PartEntry.PartPinouts;

                            listViewKeywords.Items[nIndex].Text = PartEntry.PartName;
                            listViewKeywords.Items[nIndex].Tag = Part;

                            if (PartEntry.PartDatasheetColl.Count > 0)
                            {
                                listViewKeywords.Items[nIndex].SubItems[1].Text = "*";

                                buttonDatasheets.Enabled = true;
                            }
                            else
                            {
                                listViewKeywords.Items[nIndex].SubItems[1].Text = "";

                                buttonDatasheets.Enabled = false;
                            }
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }

                        this.BusyControlVisible = false;

                        PartEntry.Dispose();
                    });
                }, "Part Details Form Edit Thread");
            }
            else
            {
                PartEntry.Dispose();
            }
        }

        private void InitializeControls()
        {
            System.Windows.Forms.ListViewItem SelectedItem = null;
            System.Int32 nPartPinoutId;
            System.String sErrorMessage, sPartPinouts;
            System.Collections.Generic.List<DatabaseDefs.TPart> PartList;
            System.Windows.Forms.ListViewItem Item;
            DatabaseDefs.TPartLens PartLens;
            System.Boolean bPartMaxLensResult, bGetPartPinoutsResult, bGetPartsWithSamePinoutsResult;

            Common.Debug.Thread.IsWorkerThread();

            bPartMaxLensResult = Database.GetPartMaxLens(out PartLens);
            bGetPartPinoutsResult = Database.GetPartPinouts(m_nPartId, out nPartPinoutId,
                                                            out sPartPinouts, out sErrorMessage);

            if (bGetPartPinoutsResult)
            {
                bGetPartsWithSamePinoutsResult = Database.GetPartsWithSamePinouts(m_nPartId, out PartList,
                                                                                  out sErrorMessage);
            }
            else
            {
                bGetPartsWithSamePinoutsResult = false;
                PartList = new System.Collections.Generic.List<DatabaseDefs.TPart>();
            }

            RunOnUIThreadWait(() =>
            {
                if (bPartMaxLensResult)
                {
                    textBoxPinouts.MaxLength = PartLens.nPartPinoutsLen;
                }

                if (bGetPartPinoutsResult)
                {
                    textBoxPinouts.Text = sPartPinouts;
                }
                else
                {
                    Common.Forms.MessageBox.Show(this, sErrorMessage,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);

                    buttonAdd.Enabled = false;
                    buttonEdit.Enabled = false;
                    buttonDelete.Enabled = false;
                    buttonDatasheets.Enabled = false;

                    return;
                }

                if (bGetPartsWithSamePinoutsResult)
                {
                    listViewKeywords.BeginUpdate();

                    foreach (DatabaseDefs.TPart Part in PartList)
                    {
                        Item = new System.Windows.Forms.ListViewItem();

                        if (Part.bPartIsDefault)
                        {
                            Item.Font = new System.Drawing.Font(Item.Font, FontStyle.Bold);
                        }

                        Item.Text = Part.sPartName;
                        Item.Tag = Part;

                        if (Part.PartDatasheetColl.Count > 0)
                        {
                            Item.SubItems.Add("*");
                        }
                        else
                        {
                            Item.SubItems.Add("");
                        }

                        Item.SubItems.Add(FormatInventory(Part.nPartTotalInventory));

                        listViewKeywords.Items.Add(Item);

                        if (Part.sPartName == m_sSelectedPartName)
                        {
                            SelectedItem = Item;
                        }
                    }

                    if (SelectedItem == null && listViewKeywords.Items.Count > 0)
                    {
                        SelectedItem = listViewKeywords.Items[0];
                    }

                    listViewKeywords.EndUpdate();
                }
                else
                {
                    Common.Forms.MessageBox.Show(this, sErrorMessage,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);

                    buttonAdd.Enabled = false;
                    buttonEdit.Enabled = false;
                    buttonDelete.Enabled = false;
                    buttonDatasheets.Enabled = false;
                }

                if (SelectedItem != null)
                {
                    SelectedItem.Selected = true;
                    SelectedItem.Focused = true;

                    SelectedItem.EnsureVisible();
                }
            });
        }

        private static System.String FormatInventory(
            System.Int32 nInventoryTotal)
        {
            if (nInventoryTotal > 0)
            {
                return nInventoryTotal.ToString();
            }

            return "-";
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
