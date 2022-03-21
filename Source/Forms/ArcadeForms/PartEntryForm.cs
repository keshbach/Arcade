/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2016 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Arcade.Forms
{
    /// <summary>
    /// Form to allow the entry of a new part or to edit an existing part.
    /// </summary>
    public partial class PartEntryForm : System.Windows.Forms.Form
    {
        public enum EPartEntryFormType
        {
            AddPartName,
            NewPart,
            EditPart
        };

        private EPartEntryFormType m_PartEntryFormType = EPartEntryFormType.AddPartName;
        private System.String m_sPartName = "";
        private System.String m_sPartCategoryName = "";
        private System.String m_sPartTypeName = "";
        private System.String m_sPartPackageName = "";
        private System.Boolean m_bDefaultPartName = false;
        private System.String m_sPartPinouts = "";
        private Common.Collections.StringCollection m_PartDatasheetColl = new Common.Collections.StringCollection();

        private Common.Collections.StringSortedList<System.Int32> m_PartCategoryList = null;
        private Common.Collections.StringSortedList<System.Int32> m_PartTypeList = null;
        private Common.Collections.StringSortedList<System.Int32> m_PartPackageList = null;

        private System.Boolean m_IgnoreChange = false;

        public PartEntryForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        public EPartEntryFormType PartEntryFormType
        {
            set
            {
                m_PartEntryFormType = value;
            }
        }

        public System.String PartName
        {
            get
            {
                return m_sPartName;
            }
            set
            {
                m_sPartName = value;
            }
        }

        public System.String PartCategoryName
        {
            get
            {
                return m_sPartCategoryName;
            }
            set
            {
                m_sPartCategoryName = value;
            }
        }

        public System.String PartTypeName
        {
            get
            {
                return m_sPartTypeName;
            }
            set
            {
                m_sPartTypeName = value;
            }
        }

        public System.String PartPackageName
        {
            get
            {
                return m_sPartPackageName;
            }
            set
            {
                m_sPartPackageName = value;
            }
        }

        public System.Boolean DefaultPartName
        {
            get
            {
                return m_bDefaultPartName;
            }
            set
            {
                m_bDefaultPartName = value;
            }
        }

        public System.String PartPinouts
        {
            get
            {
                return m_sPartPinouts;
            }
            set
            {
                m_sPartPinouts = value;
            }
        }

        public Common.Collections.StringCollection PartDatasheetColl
        {
            get
            {
                return m_PartDatasheetColl;
            }
            set
            {
                if (value != null)
                {
                    m_PartDatasheetColl = value;
                }
                else
                {
                    m_PartDatasheetColl.Clear();
                }
            }
        }

        private void PartEntryForm_Load(object sender, EventArgs e)
        {
            DatabaseDefs.TPartLens PartLens;

            using (new Common.Forms.WaitCursor(this))
            {
                m_IgnoreChange = true;

                Database.GetPartCategoryList(DatabaseDefs.EPartDataType.Category,
                                             out m_PartCategoryList);
                Database.GetPartCategoryList(DatabaseDefs.EPartDataType.Type,
                                             out m_PartTypeList);
                Database.GetPartCategoryList(DatabaseDefs.EPartDataType.Package,
                                             out m_PartPackageList);

                comboBoxCategory.BeginUpdate();

                foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_PartCategoryList)
                {
                    comboBoxCategory.Items.Add(Pair.Key);
                }

                comboBoxCategory.EndUpdate();

                comboBoxCategory.AutosizeDropDown();

                comboBoxType.BeginUpdate();

                foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_PartTypeList)
                {
                    comboBoxType.Items.Add(Pair.Key);
                }

                comboBoxType.EndUpdate();

                comboBoxType.AutosizeDropDown();

                comboBoxPackage.BeginUpdate();

                foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_PartPackageList)
                {
                    comboBoxPackage.Items.Add(Pair.Key);
                }

                comboBoxPackage.EndUpdate();

                comboBoxPackage.AutosizeDropDown();

                buttonDeleteDatasheet.Enabled = false;
                buttonViewDatasheet.Enabled = false;

                switch (m_PartEntryFormType)
                {
                    case EPartEntryFormType.AddPartName:
                        Text = "Add...";

                        textBoxPartPinouts.ReadOnly = true;
                        textBoxPartPinouts.Text = m_sPartPinouts;

                        comboBoxCategory.SelectedIndex = m_PartCategoryList.IndexOfKey(m_sPartCategoryName);
                        comboBoxType.SelectedIndex = m_PartTypeList.IndexOfKey(m_sPartTypeName);
                        comboBoxPackage.SelectedIndex = m_PartPackageList.IndexOfKey(m_sPartPackageName);

                        comboBoxCategory.Enabled = false;
                        comboBoxType.Enabled = false;

                        checkBoxDefault.Enabled = false;
                        break;
                    case EPartEntryFormType.NewPart:
                        Text = "New Part...";

                        checkBoxDefault.Checked = true;
                        checkBoxDefault.Enabled = false;
                        break;
                    case EPartEntryFormType.EditPart:
                        Text = "Edit...";

                        textBoxName.Text = m_sPartName;
                        textBoxPartPinouts.Text = m_sPartPinouts;

                        comboBoxCategory.SelectedIndex = m_PartCategoryList.IndexOfKey(m_sPartCategoryName);
                        comboBoxType.SelectedIndex = m_PartTypeList.IndexOfKey(m_sPartTypeName);
                        comboBoxPackage.SelectedIndex = m_PartPackageList.IndexOfKey(m_sPartPackageName);

                        checkBoxDefault.Checked = m_bDefaultPartName;

                        listViewDatasheets.BeginUpdate();

                        foreach (System.String sValue in m_PartDatasheetColl)
                        {
                            listViewDatasheets.Items.Add(sValue);
                        }

                        listViewDatasheets.EndUpdate();

                        if (m_PartDatasheetColl.Count > 0)
                        {
                            listViewDatasheets.AutosizeColumns();
                        }
                        else
                        {
                            listViewDatasheets.Enabled = false;
                        }
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                }

                if (textBoxName.Text.Length == 0)
                {
                    buttonOK.Enabled = false;
                }

                if (Database.GetPartMaxLens(out PartLens))
                {
                    textBoxName.MaxLength = PartLens.nPartNameLen;
                    textBoxPartPinouts.MaxLength = PartLens.nPartPinoutsLen;
                }

                m_IgnoreChange = false;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_sPartName = textBoxName.Text;
            m_sPartPinouts = textBoxPartPinouts.Text;
            m_bDefaultPartName = checkBoxDefault.Checked;

            m_sPartCategoryName = (System.String)comboBoxCategory.SelectedItem;
            m_sPartTypeName = (System.String)comboBoxType.SelectedItem;
            m_sPartPackageName = (System.String)comboBoxPackage.SelectedItem;

            DialogResult = DialogResult.OK;

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            UpdateOKButton();
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '\'':
                case '\"':
                case '*':
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        private void textBoxPartPinouts_TextChanged(object sender, EventArgs e)
        {
            UpdateOKButton();
        }

        private void comboBoxCategory_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxCategory.SelectedIndex == -1)
            {
                e.Cancel = true;
            }

            UpdateOKButton();
        }

        private void comboBoxType_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxType.SelectedIndex == -1)
            {
                e.Cancel = true;
            }

            UpdateOKButton();
        }

        private void comboBoxPackage_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxPackage.SelectedIndex == -1)
            {
                e.Cancel = true;
            }

            UpdateOKButton();
        }

        private void checkBoxDefault_CheckedChanged(object sender, EventArgs e)
        {
            UpdateOKButton();
        }

        private void listViewDatasheets_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            buttonDeleteDatasheet.Enabled = true;
            buttonViewDatasheet.Enabled = true;
        }

        private void listViewDatasheets_DoubleClick(object sender, EventArgs e)
        {
            ViewDatasheet();
        }

        private void buttonAddDatasheet_Click(object sender, EventArgs e)
        {
            System.Boolean bStringFound = false;
            System.Windows.Forms.ListViewItem Item;

            if (System.Windows.Forms.DialogResult.OK == openFileDialog.ShowDialog(this))
            {
                for (System.Int32 nIndex = 0; nIndex < listViewDatasheets.Items.Count;
                        ++nIndex)
                {
                    if (0 == System.String.Compare(listViewDatasheets.Items[nIndex].Text,
                                                    openFileDialog.FileName, true))
                    {
                        bStringFound = true;
                    }
                }

                if (bStringFound == false)
                {
                    listViewDatasheets.Enabled = true;

                    Item = listViewDatasheets.Items.Add(openFileDialog.FileName);

                    m_PartDatasheetColl.Add(openFileDialog.FileName);

                    listViewDatasheets.AutosizeColumns();

                    Item.Selected = true;
                    Item.Focused = true;

                    Item.EnsureVisible();

                    UpdateOKButton();
                }
                else
                {
                    Common.Forms.MessageBox.Show(this,
                        "This datasheet file already exists for this part.",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
        }

        private void buttonDeleteDatasheet_Click(object sender, EventArgs e)
        {
            System.Int32 nIndex = listViewDatasheets.SelectedIndices[0];
            System.Windows.Forms.ListViewItem Item;

            m_PartDatasheetColl.RemoveAt(nIndex);

            listViewDatasheets.Items.RemoveAt(nIndex);

            if (nIndex == listViewDatasheets.Items.Count)
            {
                --nIndex;
            }

            if (listViewDatasheets.Items.Count > 0)
            {
                Item = listViewDatasheets.Items[nIndex];

                Item.Selected = true;
                Item.Focused = true;

                Item.EnsureVisible();
            }
            else
            {
                listViewDatasheets.Enabled = false;

                buttonDeleteDatasheet.Enabled = false;
                buttonViewDatasheet.Enabled = false;
            }

            UpdateOKButton();
        }

        private void buttonViewDatasheet_Click(object sender, EventArgs e)
        {
            ViewDatasheet();
        }

        private void ViewDatasheet()
        {
            System.Int32 nIndex = listViewDatasheets.SelectedIndices[0];
            System.Diagnostics.ProcessStartInfo StartInfo = new System.Diagnostics.ProcessStartInfo();

            try
            {
                StartInfo.FileName = listViewDatasheets.Items[nIndex].Text;
                StartInfo.UseShellExecute = true;
                StartInfo.Verb = "Open";
                StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;

                System.Diagnostics.Process.Start(StartInfo);
            }
            catch (System.Exception Exception)
            {
                Common.Forms.MessageBox.Show(this,
                    "The file could not be opened.\n\n(" + Exception.Message + ")",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information);
            }
        }

        private void UpdateOKButton()
        {
            bool bEnabled = false;

            if (m_IgnoreChange == false &&
                textBoxName.TextLength > 0 &&
                comboBoxCategory.SelectedIndex != -1 &&
                comboBoxType.SelectedIndex != -1 &&
                comboBoxPackage.SelectedIndex != -1)
            {
                bEnabled = true;
            }

            buttonOK.Enabled = bEnabled;
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2016 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
