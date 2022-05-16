/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class SelectDataForm : Common.Forms.Form
    {
        #region "Member Variables"
        private System.Collections.Specialized.StringCollection m_DataColl = new System.Collections.Specialized.StringCollection();
        private System.String m_sSelectedData = "";
        private System.String m_sDataType = "";
        #endregion

        #region "Properties"
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
        #endregion

        #region "Constructor"
        public SelectDataForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { listViewData };
            }
        }
        #endregion

        #region "Select Data Event Handlers"
        private void SelectDataForm_Load(object sender, EventArgs e)
        {
            listViewData.BeginUpdate();

            foreach (System.String sValue in m_DataColl)
            {
                listViewData.Items.Add(sValue);
            }

            listViewData.EndUpdate();

            if (listViewData.Items.Count == 0)
            {
                listViewData.Enabled = false;
            }

            buttonOK.Enabled = false;

            Text = "Select " + m_sDataType;

            labelData.Text = "&" + m_sDataType + ":";
        }
        #endregion

        #region "List View Event Handlers"
        private void listViewGameData_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            buttonOK.Enabled = true;
        }

        private void listViewGameData_DoubleClick(object sender, EventArgs e)
        {
            OnOK();
        }
        #endregion

        #region "Button Event Handlers"
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
        private void OnOK()
        {
            System.Int32 nIndex = listViewData.SelectedIndices[0];

            m_sSelectedData = listViewData.Items[nIndex].Text;

            DialogResult = DialogResult.OK;

            Close();
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
