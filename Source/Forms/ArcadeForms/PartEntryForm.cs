/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class PartEntryForm : Arcade.Forms.Form
    {
        #region "Enumerations"
        public enum EPartEntryFormType
        {
            AddPartName,
            NewPart,
            EditPart
        };
        #endregion

        #region "Constants"
        private const string CFileOpenDialogAllFilesClientGuid = "{76E8474E-6999-41CC-A3F3-F815AF424886}";
        #endregion

        #region "Member Variables"
        private EPartEntryFormType m_PartEntryFormType = EPartEntryFormType.AddPartName;
        private System.Int32 m_nPartId = -1;
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

        private System.Int32 m_nMaxPartDatasheetLen = 0;
        #endregion

        #region "Constructor"
        public PartEntryForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Properties"
        public EPartEntryFormType PartEntryFormType
        {
            set
            {
                m_PartEntryFormType = value;
            }
        }

        public System.Int32 PartId
        {
            get
            {
                return m_nPartId;
            }
            set
            {
                m_nPartId = value;
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
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { splitContainerParts };
            }
        }

        protected override System.Windows.Forms.Control[] ControlClearSelection
        {
            get
            {
                return new System.Windows.Forms.Control[] { textBoxName, textBoxPartPinouts, comboBoxCategory, comboBoxType, comboBoxPackage };
            }
        }
        #endregion

        #region "Part Entry Event Handlers"
        private void PartEntryForm_Shown(object sender, EventArgs e)
        {
            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                InitializeControls();

                RunOnUIThreadWait(() =>
                {
                    this.BusyControlVisible = false;
                });
            }, "Part Entry Form Initialize Thread");
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_sPartName = textBoxName.Text;
            m_sPartPinouts = textBoxPartPinouts.Text;
            m_bDefaultPartName = checkBoxDefault.Checked;

            m_sPartCategoryName = (System.String)comboBoxCategory.SelectedItem;
            m_sPartTypeName = (System.String)comboBoxType.SelectedItem;
            m_sPartPackageName = (System.String)comboBoxPackage.SelectedItem;

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonAddDatasheet_Click(object sender, EventArgs e)
        {
            Common.Forms.FileOpenDialog OpenFileDlg = new Common.Forms.FileOpenDialog();
            System.Boolean bStringFound = false;
            System.Windows.Forms.ListViewItem Item;

            OpenFileDlg.Title = "Add...";
            OpenFileDlg.AddToRecentList = false;
            OpenFileDlg.AllowReadOnly = false;
            OpenFileDlg.FileTypes = BuildFileTypeList();
            OpenFileDlg.PickFolders = false;
            OpenFileDlg.SelectedFileType = 1;
            OpenFileDlg.SelectMultipleItems = false;
            OpenFileDlg.ShowHidden = false;
            OpenFileDlg.ClientGuid = System.Guid.ParseExact(CFileOpenDialogAllFilesClientGuid, "B");

            if (OpenFileDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                for (System.Int32 nIndex = 0; nIndex < listViewDatasheets.Items.Count;
                     ++nIndex)
                {
                    if (0 == System.String.Compare(listViewDatasheets.Items[nIndex].Text,
                                                   OpenFileDlg.FileName, true))
                    {
                        bStringFound = true;
                    }
                }

                if (bStringFound == false)
                {
                    if (OpenFileDlg.FileName.Length <= m_nMaxPartDatasheetLen)
                    {
                        listViewDatasheets.Enabled = true;

                        Item = listViewDatasheets.Items.Add(OpenFileDlg.FileName);

                        m_PartDatasheetColl.Add(OpenFileDlg.FileName);

                        Item.Selected = true;
                        Item.Focused = true;

                        Item.EnsureVisible();

                        listViewDatasheets.AutosizeColumns();

                        UpdateOKButton();
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this,
                            System.String.Format("This datasheet file path is to long.  (Maximum path length is {0} characters.)", m_nMaxPartDatasheetLen),
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }
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

                listViewDatasheets.AutosizeColumns();
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
        #endregion

        #region "Text Box Event Handlers"
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
        #endregion

        #region "Combo Box Event Handlers"
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
        #endregion

        #region "Check Box Event Handlers"
        private void checkBoxDefault_CheckedChanged(object sender, EventArgs e)
        {
            UpdateOKButton();
        }
        #endregion

        #region "List View Event Handlers"
        private void listViewDatasheets_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            buttonDeleteDatasheet.Enabled = true;
            buttonViewDatasheet.Enabled = true;
        }

        private void listViewDatasheets_DoubleClick(object sender, EventArgs e)
        {
            ViewDatasheet();
        }
        #endregion

        #region "Internal Helpers"
        private void ViewDatasheet()
        {
            System.Int32 nIndex = listViewDatasheets.SelectedIndices[0];
            System.String sFile = listViewDatasheets.Items[nIndex].Text;
            System.Boolean bResult;
            System.String sErrorMessage;

            Common.Debug.Thread.IsUIThread();

            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                sErrorMessage = null;
                bResult = OpenFile(sFile, ref sErrorMessage);

                RunOnUIThreadWait(() =>
                {
                    if (!bResult)
                    {
                        Common.Forms.MessageBox.Show(this,
                            String.Format("The file could not be opened.\n\n({0})", sErrorMessage),
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }

                    this.BusyControlVisible = false;
                });
            }, "Part Entry Form View Datasheet Thread");
        }

        private void UpdateOKButton()
        {
            bool bEnabled = false;

            Common.Debug.Thread.IsUIThread();

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

        private System.Collections.Generic.List<Common.Forms.FileTypeItem> BuildFileTypeList()
        {
            System.Collections.Generic.List<Common.Forms.FileTypeItem> FileTypeList = new System.Collections.Generic.List<Common.Forms.FileTypeItem>();
            Common.Forms.FileTypeItem FileTypeItem;

            Common.Debug.Thread.IsUIThread();

            FileTypeItem = new Common.Forms.FileTypeItem("Adobe PDF Files", "*.pdf");

            FileTypeList.Add(FileTypeItem);

            FileTypeItem = new Common.Forms.FileTypeItem("Text Files", "*.txt");

            FileTypeList.Add(FileTypeItem);

            FileTypeItem = new Common.Forms.FileTypeItem("All Files", "*.*");

            FileTypeList.Add(FileTypeItem);

            return FileTypeList;
        }

        private void InitializeControls()
        {
            DatabaseDefs.TPartLens PartLens;
            System.Boolean bPartMaxLensResult;

            Common.Debug.Thread.IsWorkerThread();

            Database.GetPartCategoryList(DatabaseDefs.EPartDataType.Category,
                                         out m_PartCategoryList);
            Database.GetPartCategoryList(DatabaseDefs.EPartDataType.Type,
                                         out m_PartTypeList);
            Database.GetPartCategoryList(DatabaseDefs.EPartDataType.Package,
                                         out m_PartPackageList);

            bPartMaxLensResult = Database.GetPartMaxLens(out PartLens);

            RunOnUIThreadWait(() =>
            {
                m_IgnoreChange = true;

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

                        listViewDatasheets.AutosizeColumns();

                        listViewDatasheets.EndUpdate();

                        listViewDatasheets.Enabled = (m_PartDatasheetColl.Count > 0);
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                }

                if (textBoxName.Text.Length == 0)
                {
                    buttonOK.Enabled = false;
                }

                if (bPartMaxLensResult)
                {
                    textBoxName.MaxLength = PartLens.nPartNameLen;
                    textBoxPartPinouts.MaxLength = PartLens.nPartPinoutsLen;

                    m_nMaxPartDatasheetLen = PartLens.nPartDatasheetLen;
                }

                m_IgnoreChange = false;
            });
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
