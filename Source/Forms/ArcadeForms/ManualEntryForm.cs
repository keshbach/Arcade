/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class ManualEntryForm : System.Windows.Forms.Form
    {
        #region "Enumerations"
        public enum EManualEntryFormType
        {
            NewManual,
            EditManual,
            ViewManual
        };
        #endregion

        #region "Member Variables"
        private EManualEntryFormType m_ManualEntryFormType = EManualEntryFormType.NewManual;
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

        private Common.Collections.StringSortedList<System.Int32> m_ManufacturerList = null;
        private Common.Collections.StringSortedList<System.Int32> m_ManualStorageBoxList = null;
        private Common.Collections.StringSortedList<System.Int32> m_ManualPrintEditionList = null;
        private Common.Collections.StringSortedList<System.Int32> m_ManualConditionList = null;
        #endregion

        #region "Constructor"
        public ManualEntryForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Properties"
        public EManualEntryFormType ManualEntryFormType
        {
            set
            {
                m_ManualEntryFormType = value;
            }
        }

        public System.String ManualName
        {
            get
            {
                return m_sManualName;
            }
            set
            {
                m_sManualName = value;
            }
        }

        public System.String StorageBox
        {
            get
            {
                return m_sStorageBox;
            }
            set
            {
                m_sStorageBox = value;
            }
        }

        public System.String PartNumber
        {
            get
            {
                return m_sPartNumber;
            }
            set
            {
                m_sPartNumber = value;
            }
        }

        public System.Int32 YearPrinted
        {
            get
            {
                return m_nYearPrinted;
            }
            set
            {
                m_nYearPrinted = value;
            }
        }

        public System.String PrintEdition
        {
            get
            {
                return m_sPrintEdition;
            }
            set
            {
                m_sPrintEdition = value;
            }
        }

        public System.String Condition
        {
            get
            {
                return m_sCondition;
            }
            set
            {
                m_sCondition = value;
            }
        }

        public System.String Manufacturer
        {
            get
            {
                return m_sManufacturer;
            }
            set
            {
                m_sManufacturer = value;
            }
        }

        public System.Boolean Complete
        {
            get
            {
                return m_bComplete;
            }
            set
            {
                m_bComplete = value;
            }
        }

        public System.Boolean Original
        {
            get
            {
                return m_bOriginal;
            }
            set
            {
                m_bOriginal = value;
            }
        }

        public System.String Description
        {
            get
            {
                return m_sDescription;
            }
            set
            {
                m_sDescription = value;
            }
        }
        #endregion

        #region "Manual Entry Event Handlers"
        private void ManualEntryForm_Load(object sender, EventArgs e)
        {
            DatabaseDefs.TManualLens ManualLens;

            if (Database.GetManualMaxLens(out ManualLens))
            {
                textBoxName.MaxLength = ManualLens.nManualNameLen;
                textBoxPartNumber.MaxLength = ManualLens.nManualPartNumberLen;
                textBoxDescription.MaxLength = ManualLens.nManualDescriptionLen;
            }

            Database.GetManufacturerList(out m_ManufacturerList);
            Database.GetManualCategoryList(DatabaseDefs.EManualDataType.StorageBox,
                                           out m_ManualStorageBoxList);
            Database.GetManualCategoryList(DatabaseDefs.EManualDataType.PrintEdition,
                                           out m_ManualPrintEditionList);
            Database.GetManualCategoryList(DatabaseDefs.EManualDataType.Condition,
                                           out m_ManualConditionList);

            comboBoxManufacturer.BeginUpdate();

            foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_ManufacturerList)
            {
                comboBoxManufacturer.Items.Add(Pair.Key);
            }

            comboBoxManufacturer.EndUpdate();

            comboBoxStorageBox.BeginUpdate();

            foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_ManualStorageBoxList)
            {
                comboBoxStorageBox.Items.Add(Pair.Key);
            }

            comboBoxStorageBox.EndUpdate();

            comboBoxPrintEdition.BeginUpdate();

            foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_ManualPrintEditionList)
            {
                comboBoxPrintEdition.Items.Add(Pair.Key);
            }

            comboBoxPrintEdition.EndUpdate();

            comboBoxCondition.BeginUpdate();

            foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_ManualConditionList)
            {
                comboBoxCondition.Items.Add(Pair.Key);
            }

            comboBoxCondition.EndUpdate();

            textBoxName.Text = m_sManualName;
            textBoxPartNumber.Text = m_sPartNumber;
            textBoxDescription.Text = m_sDescription;

            if (m_nYearPrinted > 0)
            {
                maskedTextBoxYearPrinted.Text = System.Convert.ToString(m_nYearPrinted);
            }
            else
            {
                maskedTextBoxYearPrinted.Text = "";
            }

            checkBoxComplete.Checked = m_bComplete;
            checkBoxOriginal.Checked = m_bOriginal;

            if (m_ManualEntryFormType == EManualEntryFormType.NewManual)
            {
                Text = "Add...";

                buttonOK.Enabled = false;
            }
            else
            {
                comboBoxManufacturer.SelectedIndex = m_ManufacturerList.IndexOfKey(m_sManufacturer);
                comboBoxStorageBox.SelectedIndex = m_ManualStorageBoxList.IndexOfKey(m_sStorageBox);
                comboBoxPrintEdition.SelectedIndex = m_ManualPrintEditionList.IndexOfKey(m_sPrintEdition);
                comboBoxCondition.SelectedIndex = m_ManualConditionList.IndexOfKey(m_sCondition);

                if (m_ManualEntryFormType == EManualEntryFormType.EditManual)
                {
                    Text = "Edit...";

                    buttonOK.Enabled = true;
                }
                else
                {
                    Text = "View...";

                    buttonOK.Visible = false;
                    buttonCancel.Text = "Close";

                    textBoxName.ReadOnly = true;
                    textBoxPartNumber.ReadOnly = true;
                    textBoxDescription.ReadOnly = true;
                    maskedTextBoxYearPrinted.Enabled = false;
                    checkBoxComplete.Enabled = false;
                    checkBoxOriginal.Enabled = false;

                    comboBoxStorageBox.Enabled = false;
                    comboBoxPrintEdition.Enabled = false;
                    comboBoxCondition.Enabled = false;
                    comboBoxManufacturer.Enabled = false;
                }
            }
        }
        #endregion

        #region "Text Box Event Handlers"
        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (textBoxName.Text.Length > 0)
            {
                ValidateFields();
            }
            else
            {
                buttonCancel.Enabled = false;
            }
        }
        #endregion

        #region "Combo Box Event Handlers"
        private void comboBoxStorageBox_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxStorageBox.SelectedIndex != -1)
            {
                ValidateFields();
            }
            else
            {
                e.Cancel = true;

                buttonOK.Enabled = false;
            }
        }

        private void comboBoxPrintEdition_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxPrintEdition.SelectedIndex != -1)
            {
                ValidateFields();
            }
            else
            {
                e.Cancel = true;

                buttonOK.Enabled = false;
            }
        }

        private void comboBoxCondition_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxCondition.SelectedIndex != -1)
            {
                ValidateFields();
            }
            else
            {
                e.Cancel = true;

                buttonOK.Enabled = false;
            }
        }

        private void comboBoxManufacturer_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxManufacturer.SelectedIndex != -1)
            {
                ValidateFields();
            }
            else
            {
                e.Cancel = true;

                buttonOK.Enabled = false;
            }
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_sManualName = textBoxName.Text;
            m_sPartNumber = textBoxPartNumber.Text;
            m_sDescription = textBoxDescription.Text;

            if (maskedTextBoxYearPrinted.Text.Length > 0)
            {
                m_nYearPrinted = System.Convert.ToInt32(maskedTextBoxYearPrinted.Text);
            }
            else
            {
                m_nYearPrinted = 0;
            }

            m_bComplete = checkBoxComplete.Checked;
            m_bOriginal = checkBoxOriginal.Checked;

            m_sStorageBox = (System.String)comboBoxStorageBox.SelectedItem;
            m_sPrintEdition = (System.String)comboBoxPrintEdition.SelectedItem;
            m_sCondition = (System.String)comboBoxCondition.SelectedItem;
            m_sManufacturer = (System.String)comboBoxManufacturer.SelectedItem;

            DialogResult = DialogResult.OK;

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }
        #endregion

        #region "Internal Helpers"
        private void ValidateFields()
        {
            if (textBoxName.Text.Length > 0 &&
                comboBoxStorageBox.SelectedIndex != -1 &&
                comboBoxPrintEdition.SelectedIndex != -1 &&
                comboBoxCondition.SelectedIndex != -1 &&
                comboBoxManufacturer.SelectedIndex != -1)
            {
                buttonOK.Enabled = true;
            }
            else
            {
                buttonOK.Enabled = false;
            }
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
