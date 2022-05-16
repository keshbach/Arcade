/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class FindPartForm : Common.Forms.Form
    {
        #region "Constructor"
        public FindPartForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { listViewParts, comboBoxType };
            }
        }

        protected override RadioButton[][] RadioButtonLocationSettings
        {
            get
            {
                RadioButton[] RadioButtons1 = new System.Windows.Forms.RadioButton[] { radioButtonKeyword, radioButtonType };

                return new System.Windows.Forms.RadioButton[][] { RadioButtons1 };
            }
        }

        protected override Common.Forms.ListView.ESortOrder GetListViewDefaultSortOrderFormLocationSetting(
            Common.Forms.ListView ListView)
        {
            return Common.Forms.ListView.ESortOrder.GroupSequential;
        }
        #endregion

        #region "Find Part Event Handlers"
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
        #endregion

        #region "Radio Button Event Handlers"
        private void radioButtonKeyword_CheckedChanged(object sender, EventArgs e)
        {
            textBoxKeyword.Enabled = radioButtonKeyword.Checked;

            if (radioButtonKeyword.Checked == true)
            {
                ValidateKeywordTextBox();
            }
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
        #endregion

        #region "Text Box Event Handlers"
        private void textBoxKeyword_TextChanged(object sender, System.EventArgs e)
        {
            ValidateKeywordTextBox();
        }
        #endregion

        #region "List View Event Handlers"
        private void listViewParts_DoubleClick(object sender, System.EventArgs e)
        {
            ShowPartDetailsForm();
        }

        private void listViewParts_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            buttonDetails.Enabled = true;
            buttonGames.Enabled = true;
        }
        #endregion

        #region "Combo Box Event Handlers"
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
        #endregion

        #region "Button Event Handlers"
        private void buttonSearch_Click(object sender, System.EventArgs e)
        {
            System.Collections.Generic.List<DatabaseDefs.TPart> PartList;
            System.String sErrorMessage;
            System.Windows.Forms.ListViewItem Item;
            System.Boolean bResult;

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

            new Common.Forms.FormLocation(ViewGamePartLocation, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            Part = (DatabaseDefs.TPart)listViewParts.Items[nIndex].Tag;

            ViewGamePartLocation.PartId = Part.nPartId;

            ViewGamePartLocation.ShowDialog(this);
        }

        private void buttonClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }
        #endregion

        #region "Internal Helpers"
        private void ShowPartDetailsForm()
        {
            System.Int32 nIndex = listViewParts.SelectedIndices[0];
            PartDetailsForm PartDetails = new PartDetailsForm();
            DatabaseDefs.TPart Part;

            new Common.Forms.FormLocation(PartDetails, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            Part = (DatabaseDefs.TPart)listViewParts.Items[nIndex].Tag;

            PartDetails.PartId = Part.nPartId;
            PartDetails.SelectedPartName = Part.sPartName;

            PartDetails.ShowDialog(this);
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
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
