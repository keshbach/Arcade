/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class DisplayEntryForm : System.Windows.Forms.Form
    {
        public enum EDisplayEntryFormType
        {
            NewDisplay,
            EditDisplay
        };

        private EDisplayEntryFormType m_DisplayEntryFormType = EDisplayEntryFormType.NewDisplay;
        private System.Int32 m_nDisplayId = -1;
        private System.String m_sDisplayName = "";
        private System.String m_sDisplayType = "";
        private System.String m_sDisplayResolution = "";
        private System.String m_sDisplayColors = "";
        private System.String m_sDisplayOrientation = "";

        private Common.Collections.StringSortedList<System.Int32> m_DisplayTypeList = null;
        private Common.Collections.StringSortedList<System.Int32> m_DisplayResolutionList = null;
        private Common.Collections.StringSortedList<System.Int32> m_DisplayColorsList = null;
        private Common.Collections.StringSortedList<System.Int32> m_DisplayOrientationList = null;

        public DisplayEntryForm()
        {
            InitializeComponent();
        }

        public EDisplayEntryFormType DisplayEntryFormType
        {
            set
            {
                m_DisplayEntryFormType = value;
            }
        }

        public System.Int32 DisplayId
        {
            set
            {
                m_nDisplayId = value;
            }
        }

        public System.String DisplayName
        {
            get
            {
                return m_sDisplayName;
            }
            set
            {
                m_sDisplayName = value;
            }
        }

        public System.String DisplayType
        {
            get
            {
                return m_sDisplayType;
            }
            set
            {
                m_sDisplayType = value;
            }
        }

        public System.String DisplayResolution
        {
            get
            {
                return m_sDisplayResolution;
            }
            set
            {
                m_sDisplayResolution = value;
            }
        }

        public System.String DisplayColors
        {
            get
            {
                return m_sDisplayColors;
            }
            set
            {
                m_sDisplayColors = value;
            }
        }

        public System.String DisplayOrientation
        {
            get
            {
                return m_sDisplayOrientation;
            }
            set
            {
                m_sDisplayOrientation = value;
            }
        }

        private void DisplayEntryForm_Load(object sender, EventArgs e)
        {
            DatabaseDefs.TDisplayLens DisplayLens;

            if (Database.GetDisplayMaxLens(out DisplayLens))
            {
                textBoxName.MaxLength = DisplayLens.nDisplayNameLen;
            }

            Database.GetDisplayCategoryList(DatabaseDefs.EDisplayDataType.Type,
                                            out m_DisplayTypeList);
            Database.GetDisplayCategoryList(DatabaseDefs.EDisplayDataType.Resolution,
                                            out m_DisplayResolutionList);
            Database.GetDisplayCategoryList(DatabaseDefs.EDisplayDataType.Colors,
                                            out m_DisplayColorsList);
            Database.GetDisplayCategoryList(DatabaseDefs.EDisplayDataType.Orientation,
                                            out m_DisplayOrientationList);

            comboBoxType.BeginUpdate();

            foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_DisplayTypeList)
            {
                comboBoxType.Items.Add(Pair.Key);
            }

            comboBoxType.EndUpdate();

            comboBoxResolution.BeginUpdate();

            foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_DisplayResolutionList)
            {
                comboBoxResolution.Items.Add(Pair.Key);
            }

            comboBoxResolution.EndUpdate();

            comboBoxColors.BeginUpdate();

            foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_DisplayColorsList)
            {
                comboBoxColors.Items.Add(Pair.Key);
            }

            comboBoxColors.EndUpdate();

            comboBoxOrientation.BeginUpdate();

            foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_DisplayOrientationList)
            {
                comboBoxOrientation.Items.Add(Pair.Key);
            }

            comboBoxOrientation.EndUpdate();

            textBoxName.Text = m_sDisplayName;

            if (m_DisplayEntryFormType == EDisplayEntryFormType.NewDisplay)
            {
                Text = "Add...";

                buttonOK.Enabled = false;
            }
            else
            {
                comboBoxType.SelectedIndex = m_DisplayTypeList.IndexOfKey(m_sDisplayType);
                comboBoxResolution.SelectedIndex = m_DisplayResolutionList.IndexOfKey(m_sDisplayResolution);
                comboBoxColors.SelectedIndex = m_DisplayColorsList.IndexOfKey(m_sDisplayColors);
                comboBoxOrientation.SelectedIndex = m_DisplayOrientationList.IndexOfKey(m_sDisplayOrientation);

                Text = "Edit...";

                buttonOK.Enabled = true;
            }
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (textBoxName.Text.Length > 0)
            {
                ValidateFields();
            }
            else
            {
                buttonOK.Enabled = false;
            }
        }

        private void comboBoxType_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxType.SelectedIndex != -1)
            {
                ValidateFields();
            }
            else
            {
                e.Cancel = true;

                buttonOK.Enabled = false;
            }
        }

        private void comboBoxResolution_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxResolution.SelectedIndex != -1)
            {
                ValidateFields();
            }
            else
            {
                e.Cancel = true;

                buttonOK.Enabled = false;
            }
        }

        private void comboBoxColors_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxColors.SelectedIndex != -1)
            {
                ValidateFields();
            }
            else
            {
                e.Cancel = true;

                buttonOK.Enabled = false;
            }
        }

        private void comboBoxOrientation_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxOrientation.SelectedIndex != -1)
            {
                ValidateFields();
            }
            else
            {
                e.Cancel = true;

                buttonOK.Enabled = false;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_sDisplayName = textBoxName.Text;

            m_sDisplayType = (System.String)comboBoxType.SelectedItem;
            m_sDisplayResolution = (System.String)comboBoxResolution.SelectedItem;
            m_sDisplayColors = (System.String)comboBoxColors.SelectedItem;
            m_sDisplayOrientation = (System.String)comboBoxOrientation.SelectedItem;

            DialogResult = DialogResult.OK;

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void ValidateFields()
        {
            if (textBoxName.Text.Length > 0 &&
                comboBoxType.SelectedIndex != -1 &&
                comboBoxResolution.SelectedIndex != -1 &&
                comboBoxColors.SelectedIndex != -1 &&
                comboBoxOrientation.SelectedIndex != -1)
            {
                buttonOK.Enabled = true;
            }
            else
            {
                buttonOK.Enabled = false;
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
