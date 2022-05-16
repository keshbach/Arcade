/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class DataEntryForm : System.Windows.Forms.Form
    {
        #region "Enumerations"
        public enum EDataEntryFormType
        {
            NewData,
            EditData
        };
        #endregion

        #region "Member Variables"
        private EDataEntryFormType m_DataEntryFormType = EDataEntryFormType.NewData;
        private System.String m_sDataName = "";
        private System.Int32 m_nMaxDataNameLen = 0;
        #endregion

        #region "Constructor"
        public DataEntryForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Properties"
        public EDataEntryFormType DataEntryFormType
        {
            set
            {
                m_DataEntryFormType = value;
            }
        }

        public System.String DataName
        {
            get
            {
                return m_sDataName;
            }
            set
            {
                m_sDataName = value;
            }
        }

        public System.Int32 MaxDataNameLen
        {
            set
            {
                m_nMaxDataNameLen = value;
            }
        }
        #endregion

        #region "Data Entry Event Handlers"
        private void DataEntryForm_Load(object sender, EventArgs e)
        {
            switch (m_DataEntryFormType)
            {
                case EDataEntryFormType.NewData:
                    Text = "Add...";
                    break;
                case EDataEntryFormType.EditData:
                    Text = "Edit...";
                    textBoxName.Text = m_sDataName;
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
            }

            if (textBoxName.Text.Length == 0)
            {
                buttonOK.Enabled = false;
            }

            textBoxName.MaxLength = m_nMaxDataNameLen;
        }
        #endregion

        #region "Text Box Event Handlers"
        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (textBoxName.TextLength > 0)
            {
                buttonOK.Enabled = true;
            }
            else
            {
                buttonOK.Enabled = false;
            }
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_sDataName = textBoxName.Text;

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
