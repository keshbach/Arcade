/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;

namespace Arcade.Forms
{
    public partial class ViewPartDatasheetsForm : Common.Forms.Form
    {
        #region "Member Variables"
        private System.Collections.Specialized.StringCollection m_PartDatasheetColl;
        #endregion

        #region "Constructor"
        public ViewPartDatasheetsForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Properties"
        public System.Collections.Specialized.StringCollection PartDatasheetColl
        {
            set
            {
                m_PartDatasheetColl = value;
            }
        }
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { listViewDatasheets };
            }
        }
        #endregion

        #region "View Part Datasheets Event Handlers"
        private void ViewPartDatasheetsForm_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.ListViewItem Item;

            listViewDatasheets.BeginUpdate();

            foreach (System.String sValue in m_PartDatasheetColl)
            {
                Item = new System.Windows.Forms.ListViewItem();

                Item.Text = sValue;

                listViewDatasheets.Items.Add(Item);
            }

            listViewDatasheets.EndUpdate();

            if (listViewDatasheets.Items.Count > 0)
            {
                Item = listViewDatasheets.Items[0];

                Item.Selected = true;
                Item.Focused = true;

                Item.EnsureVisible();
            }
            else
            {
                listViewDatasheets.Enabled = false;
                buttonView.Enabled = false;
            }
        }
        #endregion

        #region "List View Event Handlers"
        private void listViewDatasheets_Click(object sender, EventArgs e)
        {
            buttonView.Enabled = true;
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonView_Click(object sender, EventArgs e)
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

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
