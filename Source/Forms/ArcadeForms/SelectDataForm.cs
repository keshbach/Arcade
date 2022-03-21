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

namespace Arcade
{
    namespace Forms
    {
        public partial class SelectDataForm : System.Windows.Forms.Form
        {
            private System.Collections.Specialized.StringCollection m_DataColl = new System.Collections.Specialized.StringCollection();
            private System.String m_sSelectedData = "";
            private System.String m_sDataType = "";

            public System.Collections.Specialized.StringCollection DataColl
            {
                set
                {
                    m_DataColl = value;
                }
            }

            public System.String SelectedData
            {
                get
                {
                    return m_sSelectedData;
                }
            }

            public System.String DataType
            {
                set
                {
                    m_sDataType = value;
                }
            }

            public SelectDataForm()
            {
                InitializeComponent();
            }

            private void SelectDataForm_Load(object sender, EventArgs e)
            {
                listViewData.BeginUpdate();

                foreach (System.String sValue in m_DataColl)
                {
                    listViewData.Items.Add(sValue);
                }

                listViewData.AutosizeColumns();
                listViewData.EndUpdate();

                if (listViewData.Items.Count == 0)
                {
                    listViewData.Enabled = false;
                }

                buttonOK.Enabled = false;

                Text = "Select " + m_sDataType;

                labelData.Text = "&" + m_sDataType + ":";
            }

            private void listViewGameData_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
            {
                buttonOK.Enabled = true;
            }

            private void listViewGameData_DoubleClick(object sender, EventArgs e)
            {
                OnOK();
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

            private void OnOK()
            {
                System.Int32 nIndex = listViewData.SelectedIndices[0];

                m_sSelectedData = listViewData.Items[nIndex].Text;

                DialogResult = DialogResult.OK;

                Close();
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
