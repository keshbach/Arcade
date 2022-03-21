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
        /// Summary description for DataEntryForm.
        /// </summary>
        public partial class DataEntryForm : System.Windows.Forms.Form
        {
            public enum EDataEntryFormType
            {
                NewData,
                EditData
            };

            private EDataEntryFormType m_DataEntryFormType = EDataEntryFormType.NewData;
            private System.String m_sDataName = "";
            private System.Int32 m_nMaxDataNameLen = 0;

            public DataEntryForm()
            {
                InitializeComponent();
            }

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

            private void buttonOK_Click(object sender, EventArgs e)
            {
                m_sDataName = textBoxName.Text;

                Close();
            }

            private void buttonCancel_Click(object sender, EventArgs e)
            {
                Close();
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
