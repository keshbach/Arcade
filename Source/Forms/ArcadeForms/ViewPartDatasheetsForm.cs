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
        /// Summary description for ViewPartDatasheetsForm.
        /// </summary>
        public partial class ViewPartDatasheetsForm : System.Windows.Forms.Form
        {
            private System.Collections.Specialized.StringCollection m_PartDatasheetColl;

            public ViewPartDatasheetsForm()
            {
                InitializeComponent();
            }

            public System.Collections.Specialized.StringCollection PartDatasheetColl
            {
                set
                {
                    m_PartDatasheetColl = value;
                }
            }

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

                listViewDatasheets.AutosizeColumns();
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

            private void listViewDatasheets_Click(object sender, EventArgs e)
            {
                buttonView.Enabled = true;
            }

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
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
