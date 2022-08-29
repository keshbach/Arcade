/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;

namespace Arcade.Forms
{
    public partial class ViewPartDatasheetsForm : Arcade.Forms.Form
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
            System.String sFile = listViewDatasheets.Items[nIndex].Text;
            System.Boolean bResult;
            System.String sErrorMessage;

            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                sErrorMessage = null;
                bResult = OpenFile(sFile, ref sErrorMessage);

                RunOnUIThreadWait(() =>
                {
                    if (!bResult)
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }

                    this.BusyControlVisible = false;
                });
            }, "View Part Datasheets Form View Thread");
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
