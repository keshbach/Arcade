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
        /// Summary description for FindPartForm.
        /// </summary>
        public partial class FindPartForm : System.Windows.Forms.Form
        {
            public FindPartForm()
            {
                //
                // Required for Windows Form Designer support
                //
                InitializeComponent();
            }

            private void FindPartForm_Load(object sender, System.EventArgs e)
            {
                Common.Collections.StringSortedList<System.Int32> PartTypeList;
                DatabaseDefs.TPartLens PartLens;

                if (Database.GetPartMaxLens(out PartLens))
                {
                    textBoxKeyword.MaxLength = PartLens.nPartNameLen;
                }

                Database.GetPartCategoryList(DatabaseDefs.EPartDataType.Type,
                                             out PartTypeList);

                comboBoxType.BeginUpdate();

                foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in PartTypeList)
                {
                    comboBoxType.Items.Add(Pair.Key);
                }

                comboBoxType.EndUpdate();

                comboBoxType.AutosizeDropDown();

                radioButtonKeyword.Checked = true;
                comboBoxType.Enabled = false;
                buttonSearch.Enabled = false;
                listViewParts.Enabled = false;
                buttonDetails.Enabled = false;
                buttonGames.Enabled = false;
            }

            private void radioButtonKeyword_CheckedChanged(object sender, EventArgs e)
            {
                textBoxKeyword.Enabled = radioButtonKeyword.Checked;

                if (radioButtonKeyword.Checked == true)
                {
                    ValidateKeywordTextBox();
                }
            }

            private void textBoxKeyword_TextChanged(object sender, System.EventArgs e)
            {
                ValidateKeywordTextBox();
            }

            private void buttonSearch_Click(object sender, System.EventArgs e)
            {
                System.Collections.Generic.List<DatabaseDefs.TPart> PartList;
                System.String sErrorMessage;
                System.Windows.Forms.ListViewItem Item;
                System.Boolean bResult;

                listViewParts.Sorting = Common.Forms.ListView.ESortOrder.None;

                listViewParts.Items.Clear();

                buttonDetails.Enabled = false;
                buttonGames.Enabled = false;

                using (new Common.Forms.WaitCursor(this))
                {
                    if (radioButtonKeyword.Checked == true)
                    {
                        bResult = Database.GetPartsMatchingKeyword(textBoxKeyword.Text,
                                                                   DatabaseDefs.EKeywordMatchingCriteria.Anywhere,
                                                                   out PartList,
                                                                   out sErrorMessage);
                    }
                    else
                    {
                        bResult = Database.GetPartsMatchingType((System.String)comboBoxType.SelectedItem,
                                                                out PartList,
                                                                out sErrorMessage);
                    }

                    if (bResult == true)
                    {
                        if (PartList.Count > 0)
                        {
                            listViewParts.Enabled = true;

                            listViewParts.BeginUpdate();

                            foreach (DatabaseDefs.TPart Part in PartList)
                            {
                                Item = new System.Windows.Forms.ListViewItem();

                                Item.Text = Part.sPartName;
                                Item.Tag = Part;

                                Item.SubItems.Add(Part.sPartCategoryName);
                                Item.SubItems.Add(Part.sPartTypeName);
                                Item.SubItems.Add(Part.sPartPackageName);

                                listViewParts.Items.Add(Item);
                            }

                            listViewParts.Sorting = Common.Forms.ListView.ESortOrder.GroupSequential;

                            listViewParts.AutosizeColumns();
                            listViewParts.EndUpdate();
                        }
                        else
                        {
                            listViewParts.Enabled = false;

                            Common.Forms.MessageBox.Show(this,
                                "No parts were found that match the given search criteria.",
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information,
                                System.Windows.Forms.MessageBoxDefaultButton.Button1);

                            textBoxKeyword.Focus();
                        }
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }
                }
            }

            private void buttonDetails_Click(object sender, System.EventArgs e)
            {
                ShowPartDetailsForm();
            }

            private void buttonGames_Click(object sender, System.EventArgs e)
            {
                System.Int32 nIndex = listViewParts.SelectedIndices[0];
                ViewGamePartLocationForm ViewGamePartLocation = new ViewGamePartLocationForm();
                DatabaseDefs.TPart Part;

                Part = (DatabaseDefs.TPart)listViewParts.Items[nIndex].Tag;

                ViewGamePartLocation.PartId = Part.nPartId;

                ViewGamePartLocation.ShowDialog(this);
            }

            private void buttonClose_Click(object sender, System.EventArgs e)
            {
                Close();
            }

            private void listViewParts_DoubleClick(object sender, System.EventArgs e)
            {
                ShowPartDetailsForm();
            }

            private void ShowPartDetailsForm()
            {
                System.Int32 nIndex = listViewParts.SelectedIndices[0];
                PartDetailsForm PartDetails = new PartDetailsForm();
                DatabaseDefs.TPart Part;

                Part = (DatabaseDefs.TPart)listViewParts.Items[nIndex].Tag;

                PartDetails.PartId = Part.nPartId;
                PartDetails.SelectedPartName = Part.sPartName;

                PartDetails.ShowDialog(this);
            }

            private void listViewParts_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
            {
                buttonDetails.Enabled = true;
                buttonGames.Enabled = true;
            }

            private void radioButtonType_CheckedChanged(object sender, EventArgs e)
            {
                comboBoxType.Enabled = radioButtonType.Checked;

                if (radioButtonType.Checked == true)
                {
                    if (comboBoxType.SelectedIndex != -1)
                    {
                        buttonSearch.Enabled = true;
                    }
                    else
                    {
                        buttonSearch.Enabled = false;
                    }
                }
            }

            private void comboBoxType_Validating(object sender, CancelEventArgs e)
            {
                if (comboBoxType.SelectedIndex != -1)
                {
                    buttonSearch.Enabled = true;
                }
                else
                {
                    e.Cancel = true;

                    buttonSearch.Enabled = false;
                }
            }

            private void ValidateKeywordTextBox()
            {
                if (textBoxKeyword.Text.Length > 0)
                {
                    buttonSearch.Enabled = true;
                }
                else
                {
                    buttonSearch.Enabled = false;
                }
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
