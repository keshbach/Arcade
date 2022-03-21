/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Arcade
{
    namespace Forms
    {
        /// <summary>
        /// Summary description for PartDetailsForm.
        /// </summary>
        public partial class PartDetailsForm : System.Windows.Forms.Form
        {
            private System.Int32 m_nPartId = -1;
            private System.String m_sSelectedPartName = "";

            public PartDetailsForm()
            {
                //
                // Required for Windows Form Designer support
                //
                InitializeComponent();
            }

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

            private void PartDetailsForm_Load(object sender, System.EventArgs e)
            {
                System.Windows.Forms.ListViewItem SelectedItem = null;
                System.Int32 nPartPinoutId;
                System.String sErrorMessage, sPartPinouts;
                System.Collections.Generic.List<DatabaseDefs.TPart> PartList;
                System.Windows.Forms.ListViewItem Item;
                DatabaseDefs.TPartLens PartLens;

                if (Database.GetPartMaxLens(out PartLens))
                {
                    textBoxPinouts.MaxLength = PartLens.nPartPinoutsLen;
                }

                using (new Common.Forms.WaitCursor(this))
                {
                    if (Database.GetPartPinouts(m_nPartId, out nPartPinoutId,
                                                out sPartPinouts, out sErrorMessage))
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

                    if (Database.GetPartsWithSamePinouts(m_nPartId, out PartList,
                                                         out sErrorMessage))
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

                        listViewKeywords.AutosizeColumns();
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
                }

                if (SelectedItem != null)
                {
                    SelectedItem.Selected = true;
                    SelectedItem.Focused = true;

                    SelectedItem.EnsureVisible();
                }
            }

            private void buttonAdd_Click(object sender, System.EventArgs e)
            {
                PartEntryForm EntryForm = new PartEntryForm();
                System.Int32 nIndex = listViewKeywords.SelectedIndices[0];
                System.Int32 nNewPartId;
                System.String sErrorMessage;
                DatabaseDefs.TPart Part;
                System.Windows.Forms.ListViewItem Item;

                Part = (DatabaseDefs.TPart)listViewKeywords.Items[nIndex].Tag;

                EntryForm.PartEntryFormType = PartEntryForm.EPartEntryFormType.AddPartName;
                EntryForm.PartCategoryName = Part.sPartCategoryName;
                EntryForm.PartTypeName = Part.sPartTypeName;
                EntryForm.PartPackageName = Part.sPartPackageName;
                EntryForm.DefaultPartName = false;
                EntryForm.PartPinouts = textBoxPinouts.Text;

                if (System.Windows.Forms.DialogResult.OK == EntryForm.ShowDialog(this))
                {
                    if (Database.AddPart(m_nPartId, EntryForm.PartName,
                                         EntryForm.PartPackageName,
                                         EntryForm.PartDatasheetColl,
                                         out nNewPartId,
                                         out sErrorMessage))
                    {
                        Part = new DatabaseDefs.TPart();

                        Part.nPartId = nNewPartId;
                        Part.sPartName = EntryForm.PartName;
                        Part.sPartCategoryName = EntryForm.PartCategoryName;
                        Part.sPartTypeName = EntryForm.PartTypeName;
                        Part.sPartPackageName = EntryForm.PartPackageName;
                        Part.bPartIsDefault = EntryForm.DefaultPartName;
                        Part.PartDatasheetColl = EntryForm.PartDatasheetColl;

                        Item = new System.Windows.Forms.ListViewItem();

                        Item.Text = EntryForm.PartName;
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

                        listViewKeywords.Items.Add(Item);

                        Item.Selected = true;
                        Item.Focused = true;

                        Item.EnsureVisible();

                        listViewKeywords.AutosizeColumns();
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }
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

                Part = (DatabaseDefs.TPart)listViewKeywords.Items[nIndex].Tag;

                if (Database.DeletePart(Part.nPartId, out nNewDefPartId,
                                        out sErrorMessage))
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
            }

            private void buttonDatasheets_Click(object sender, EventArgs e)
            {
                System.Int32 nIndex = listViewKeywords.SelectedIndices[0];
                ViewPartDatasheetsForm DatasheetsForm = new ViewPartDatasheetsForm();
                DatabaseDefs.TPart Part;

                Part = (DatabaseDefs.TPart)listViewKeywords.Items[nIndex].Tag;

                DatasheetsForm.PartDatasheetColl = Part.PartDatasheetColl;

                DatasheetsForm.ShowDialog(this);
            }

            private void buttonClose_Click(object sender, System.EventArgs e)
            {
                Close();
            }

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

            private void EditPart()
            {
                PartEntryForm EntryForm = new PartEntryForm();
                System.Int32 nIndex = listViewKeywords.SelectedIndices[0];
                System.String sErrorMessage;
                DatabaseDefs.TPart Part, TmpPart;

                Part = (DatabaseDefs.TPart)listViewKeywords.Items[nIndex].Tag;

                EntryForm.PartEntryFormType = PartEntryForm.EPartEntryFormType.EditPart;
                EntryForm.PartName = Part.sPartName;
                EntryForm.PartCategoryName = Part.sPartCategoryName;
                EntryForm.PartTypeName = Part.sPartTypeName;
                EntryForm.PartPackageName = Part.sPartPackageName;
                EntryForm.DefaultPartName = Part.bPartIsDefault;
                EntryForm.PartPinouts = textBoxPinouts.Text;
                EntryForm.PartDatasheetColl = Part.PartDatasheetColl;

                if (System.Windows.Forms.DialogResult.OK == EntryForm.ShowDialog(this))
                {
                    if (Database.EditPart(Part.nPartId, EntryForm.PartName,
                                          EntryForm.PartCategoryName,
                                          EntryForm.PartTypeName,
                                          EntryForm.PartPackageName,
                                          EntryForm.PartPinouts,
                                          EntryForm.DefaultPartName,
                                          EntryForm.PartDatasheetColl,
                                          out sErrorMessage))
                    {
                        if (Part.bPartIsDefault == false &&
                            EntryForm.DefaultPartName == true)
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

                        Part.sPartName = EntryForm.PartName;
                        Part.sPartCategoryName = EntryForm.PartCategoryName;
                        Part.sPartTypeName = EntryForm.PartTypeName;
                        Part.sPartPackageName = EntryForm.PartPackageName;
                        Part.bPartIsDefault = EntryForm.DefaultPartName;
                        Part.PartDatasheetColl = EntryForm.PartDatasheetColl;

                        textBoxCategory.Text = EntryForm.PartCategoryName;
                        textBoxType.Text = EntryForm.PartTypeName;
                        textBoxPackage.Text = EntryForm.PartPackageName;
                        textBoxPinouts.Text = EntryForm.PartPinouts;

                        listViewKeywords.Items[nIndex].Text = EntryForm.PartName;
                        listViewKeywords.Items[nIndex].Tag = Part;

                        if (EntryForm.PartDatasheetColl.Count > 0)
                        {
                            listViewKeywords.Items[nIndex].SubItems[1].Text = "*";

                            buttonDatasheets.Enabled = true;
                        }
                        else
                        {
                            listViewKeywords.Items[nIndex].SubItems[1].Text = "";

                            buttonDatasheets.Enabled = false;
                        }

                        listViewKeywords.AutosizeColumns();
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
