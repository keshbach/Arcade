/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class ListManualsForm : Common.Forms.Form
    {
        #region "Enumerations"
        public enum EListManualsFormType
        {
            ListManuals,
            EditManuals
        };
        #endregion

        #region "Member Variables"
        private EListManualsFormType m_ListManualsFormType = EListManualsFormType.EditManuals;
        private System.Int32 m_nManualId = -1;
        private System.String m_sManualName = "";
        private System.String m_sStorageBox = "";
        private System.String m_sPartNumber = "";
        private System.Int32 m_nYearPrinted = 0;
        private System.String m_sPrintEdition = "";
        private System.String m_sCondition = "";
        private System.String m_sManufacturer = "";
        private System.Boolean m_bComplete = true;
        private System.Boolean m_bOriginal = true;
        private System.String m_sDescription = "";
        #endregion

        #region "Constructor"
        public ListManualsForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Properties"
        public EListManualsFormType ListManualsFormType
        {
            set
            {
                m_ListManualsFormType = value;
            }
        }

        public System.Int32 ManualId
        {
            get
            {
                return m_nManualId;
            }
        }

        public System.String ManualName
        {
            get
            {
                return m_sManualName;
            }
        }

        public System.String StorageBox
        {
            get
            {
                return m_sStorageBox;
            }
        }

        public System.String PartNumber
        {
            get
            {
                return m_sPartNumber;
            }
        }

        public System.Int32 YearPrinted
        {
            get
            {
                return m_nYearPrinted;
            }
        }

        public System.String PrintEdition
        {
            get
            {
                return m_sPrintEdition;
            }
        }

        public System.String Condition
        {
            get
            {
                return m_sCondition;
            }
        }

        public System.String Manufacturer
        {
            get
            {
                return m_sManufacturer;
            }
        }

        public System.Boolean Complete
        {
            get
            {
                return m_bComplete;
            }
        }

        public System.Boolean Original
        {
            get
            {
                return m_bOriginal;
            }
        }

        public System.String Description
        {
            get
            {
                return m_sDescription;
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

        #region "List Manuals Event Handlers"
        private void ListManualsForm_Load(object sender, EventArgs e)
        {
            DatabaseDefs.TManualLens ManualLens;

            ReadManuals(null);

            if (Database.GetManualMaxLens(out ManualLens))
            {
                textBoxKeyword.MaxLength = ManualLens.nManualNameLen;
            }

            if (m_ListManualsFormType == EListManualsFormType.EditManuals)
            {
                buttonEdit.Enabled = false;
                buttonDelete.Enabled = false;

                buttonOK.Visible = false;

                buttonCancel.Text = "Close";
            }
            else
            {
                buttonAdd.Visible = false;
                buttonEdit.Visible = false;
                buttonDelete.Visible = false;

                buttonOK.Enabled = false;
            }

            buttonSearch.Enabled = false;
            buttonClear.Enabled = false;
        }
        #endregion

        #region "List View Event Handlers"
        private void listViewManuals_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            buttonEdit.Enabled = true;
            buttonDelete.Enabled = true;

            buttonOK.Enabled = true;
        }

        private void listViewManuals_DoubleClick(object sender, EventArgs e)
        {
            if (m_ListManualsFormType == EListManualsFormType.EditManuals)
            {
                EditManual();
            }
            else
            {
                OnOK();
            }
        }
        #endregion

        #region "Text Box Event Handlers"
        private void textBoxKeyword_TextChanged(object sender, EventArgs e)
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

        #region "Button Event Handlers"
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            buttonClear.Enabled = true;

            ReadManuals(textBoxKeyword.Text);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            buttonClear.Enabled = false;

            ReadManuals(null);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            ManualEntryForm ManualEntry = new ManualEntryForm();
            System.String sErrorMessage;
            System.Int32 nNewManualId;
            DatabaseDefs.TManual Manual;
            System.Windows.Forms.ListViewItem Item;

            new Common.Forms.FormLocation(ManualEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            ManualEntry.ManualEntryFormType = ManualEntryForm.EManualEntryFormType.NewManual;

            if (ManualEntry.ShowDialog(this) == DialogResult.OK)
            {
                using (new Common.Forms.WaitCursor(this))
                {
                    if (Database.AddManual(ManualEntry.ManualName, ManualEntry.StorageBox,
                                           ManualEntry.PartNumber, ManualEntry.YearPrinted,
                                           ManualEntry.PrintEdition, ManualEntry.Condition,
                                           ManualEntry.Manufacturer, ManualEntry.Complete,
                                           ManualEntry.Original, ManualEntry.Description,
                                           out nNewManualId, out sErrorMessage))
                    {
                        Manual = new DatabaseDefs.TManual();

                        Manual.nManualId = nNewManualId;
                        Manual.sManualName = ManualEntry.ManualName;
                        Manual.sManualPartNumber = ManualEntry.PartNumber;
                        Manual.nManualYearPrinted = ManualEntry.YearPrinted;
                        Manual.bManualComplete = ManualEntry.Complete;
                        Manual.bManualOriginal = ManualEntry.Original;
                        Manual.sManualDescription = ManualEntry.Description;
                        Manual.sManualPrintEdition = ManualEntry.PrintEdition;
                        Manual.sManualCondition = ManualEntry.Condition;
                        Manual.sManufacturer = ManualEntry.Manufacturer;
                        Manual.sManualStorageBox = ManualEntry.StorageBox;

                        listViewManuals.Enabled = true;

                        Item = listViewManuals.Items.Add(Manual.sManualName);

                        Item.Tag = Manual;

                        Item.SubItems.Add(Manual.sManualPrintEdition);
                        Item.SubItems.Add(Manual.sManufacturer);
                        Item.SubItems.Add(Manual.sManualStorageBox);

                        Item.Selected = true;
                        Item.Focused = true;

                        Item.EnsureVisible();

                        listViewManuals.AutosizeColumns();
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

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditManual();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            System.Int32 nIndex = listViewManuals.SelectedIndices[0];
            System.String sErrorMessage;
            DatabaseDefs.TManual Manual;
            System.Windows.Forms.ListViewItem Item;

            Manual = (DatabaseDefs.TManual)listViewManuals.Items[nIndex].Tag;

            using (new Common.Forms.WaitCursor(this))
            {
                if (Database.DeleteManual(Manual.nManualId, out sErrorMessage))
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

                        buttonEdit.Enabled = false;
                        buttonDelete.Enabled = false;
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

        private void buttonOK_Click(object sender, EventArgs e)
        {
            OnOK();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }
        #endregion

        #region "Internal Helpers"
        private void EditManual()
        {
            ManualEntryForm ManualEntry = new ManualEntryForm();
            System.Int32 nIndex = listViewManuals.SelectedIndices[0];
            System.String sErrorMessage;
            DatabaseDefs.TManual Manual;
            System.Windows.Forms.ListViewItem Item;

            Manual = (DatabaseDefs.TManual)listViewManuals.Items[nIndex].Tag;

            new Common.Forms.FormLocation(ManualEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            ManualEntry.ManualEntryFormType = ManualEntryForm.EManualEntryFormType.EditManual;

            ManualEntry.ManualName = Manual.sManualName;
            ManualEntry.PartNumber = Manual.sManualPartNumber;
            ManualEntry.YearPrinted = Manual.nManualYearPrinted;
            ManualEntry.Complete = Manual.bManualComplete;
            ManualEntry.Original = Manual.bManualOriginal;
            ManualEntry.Description = Manual.sManualDescription;
            ManualEntry.PrintEdition = Manual.sManualPrintEdition;
            ManualEntry.Condition = Manual.sManualCondition;
            ManualEntry.Manufacturer = Manual.sManufacturer;
            ManualEntry.StorageBox = Manual.sManualStorageBox;

            if (ManualEntry.ShowDialog(this) == DialogResult.OK)
            {
                using (new Common.Forms.WaitCursor(this))
                {
                    if (Database.EditManual(Manual.nManualId,
                                            ManualEntry.ManualName,
                                            ManualEntry.StorageBox,
                                            ManualEntry.PartNumber,
                                            ManualEntry.YearPrinted,
                                            ManualEntry.PrintEdition,
                                            ManualEntry.Condition,
                                            ManualEntry.Manufacturer,
                                            ManualEntry.Complete,
                                            ManualEntry.Original,
                                            ManualEntry.Description,
                                            out sErrorMessage))
                    {
                        Manual.sManualName = ManualEntry.ManualName;
                        Manual.sManualPartNumber = ManualEntry.PartNumber;
                        Manual.nManualYearPrinted = ManualEntry.YearPrinted;
                        Manual.bManualComplete = ManualEntry.Complete;
                        Manual.bManualOriginal = ManualEntry.Original;
                        Manual.sManualDescription = ManualEntry.Description;
                        Manual.sManualPrintEdition = ManualEntry.PrintEdition;
                        Manual.sManualCondition = ManualEntry.Condition;
                        Manual.sManufacturer = ManualEntry.Manufacturer;
                        Manual.sManualStorageBox = ManualEntry.StorageBox;

                        Item = listViewManuals.Items[nIndex];

                        Item.Text = Manual.sManualName;
                        Item.Tag = Manual;

                        Item.SubItems[1].Text = Manual.sManualPrintEdition;
                        Item.SubItems[2].Text = Manual.sManufacturer;
                        Item.SubItems[3].Text = Manual.sManualStorageBox;
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

        private System.Boolean ReadManuals(System.String sKeyword)
        {
            System.Collections.Generic.List<DatabaseDefs.TManual> ManualsList;
            System.String sErrorMessage;
            System.Windows.Forms.ListViewItem Item;

            listViewManuals.Items.Clear();

            buttonOK.Enabled = false;

            using (new Common.Forms.WaitCursor(this))
            {
                if (false == Database.GetManuals(sKeyword,
                                                 out ManualsList,
                                                 out sErrorMessage))
                {
                    Common.Forms.MessageBox.Show(this, sErrorMessage,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);

                    listViewManuals.Enabled = false;

                    return false;
                }

                listViewManuals.BeginUpdate();

                foreach (DatabaseDefs.TManual Manual in ManualsList)
                {
                    Item = listViewManuals.Items.Add(Manual.sManualName);

                    Item.Tag = Manual;

                    Item.SubItems.Add(Manual.sManualPrintEdition);
                    Item.SubItems.Add(Manual.sManufacturer);
                    Item.SubItems.Add(Manual.sManualStorageBox);
                }

                listViewManuals.EndUpdate();

                if (ManualsList.Count > 0)
                {
                    listViewManuals.Enabled = true;
                }
                else
                {
                    Common.Forms.MessageBox.Show(this,
                        "No manuals were found that match the given search criteria.",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information,
                        System.Windows.Forms.MessageBoxDefaultButton.Button1);

                    textBoxKeyword.Focus();

                    listViewManuals.Enabled = false;

                    buttonOK.Enabled = false;
                }
            }

            return true;
        }

        private void OnOK()
        {
            System.Int32 nIndex = listViewManuals.SelectedIndices[0];
            DatabaseDefs.TManual Manual;

            Manual = (DatabaseDefs.TManual)listViewManuals.Items[nIndex].Tag;

            m_nManualId = Manual.nManualId;
            m_sManualName = Manual.sManualName;
            m_sStorageBox = Manual.sManualStorageBox;
            m_sPartNumber = Manual.sManualPartNumber;
            m_nYearPrinted = Manual.nManualYearPrinted;
            m_sPrintEdition = Manual.sManualPrintEdition;
            m_sCondition = Manual.sManualCondition;
            m_sManufacturer = Manual.sManufacturer;
            m_bComplete = Manual.bManualComplete;
            m_bOriginal = Manual.bManualOriginal;
            m_sDescription = Manual.sManualDescription;

            DialogResult = DialogResult.OK;

            Close();
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
