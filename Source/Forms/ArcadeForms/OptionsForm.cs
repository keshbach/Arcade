/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class OptionsForm : System.Windows.Forms.Form
    {
        private int m_nItemEdit;

        public OptionsForm()
        {
            InitializeComponent();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            System.Int32[] nColumnOrder = {1, 0};
            Dictionary<string, object> SettingsDict;
            Dictionary<string, object>.Enumerator Enum;
            System.Windows.Forms.ListViewItem ListViewItem;
            System.String sErrorMessage;

            buttonOK.Enabled = false;

            listViewSettings.BeginUpdate();

            using (new Common.Forms.WaitCursor(this))
            {
                if (Database.ReadSettings(out SettingsDict, out sErrorMessage))
                {
                    Enum = SettingsDict.GetEnumerator();

                    while (Enum.MoveNext())
                    {
                        ListViewItem = listViewSettings.Items.Add(Enum.Current.Value.ToString());

                        ListViewItem.SubItems.Add(Enum.Current.Key);

                        ListViewItem.Tag = Enum.Current.Value.GetType();
                    }

                    buttonOK.Enabled = listViewSettings.Items.Count > 0;
                }
                else
                {
                    Common.Forms.MessageBox.Show(this, sErrorMessage,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);
                }
            }

            listViewSettings.ChangeColumnDisplayOrder(nColumnOrder);
            listViewSettings.AutosizeColumns();
            listViewSettings.EndUpdate();
        }

        private void listViewSettings_KeyPressLabelEdit(object sender, KeyPressEventArgs e)
        {
            if ((Type)listViewSettings.Items[m_nItemEdit].Tag == typeof(System.UInt16))
            {
                if (e.KeyChar < '0' || e.KeyChar > '9')
                {
                    e.Handled = true;
                }
            }
        }

        private void listViewSettings_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if ((Type)listViewSettings.Items[e.Item].Tag == typeof(System.UInt16))
            {
                System.UInt16 nValue;

                if (!System.UInt16.TryParse(e.Label, out nValue))
                {
                    e.CancelEdit = true;
                }
            }
        }

        private void listViewSettings_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            m_nItemEdit = e.Item;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> SettingsDict = new Dictionary<string, object>();
            System.String sErrorMessage;

            foreach (System.Windows.Forms.ListViewItem ListViewItem in listViewSettings.Items)
            {
                if ((Type)ListViewItem.Tag == typeof(System.UInt16))
                {
                    System.UInt16 nValue;

                    if (System.UInt16.TryParse(ListViewItem.Text, out nValue))
                    {
                        SettingsDict.Add(ListViewItem.SubItems[1].Text, nValue);
                    }
                    else
                    {
                        SettingsDict.Add(ListViewItem.SubItems[1].Text, 0);
                    }
                }
                else if ((Type)ListViewItem.Tag == typeof(System.String))
                {
                    SettingsDict.Add(ListViewItem.SubItems[1].Text, ListViewItem.Text);
                }
            }

            if (Database.WriteSettings(SettingsDict, out sErrorMessage))
            {
                Close();
            }
            else
            {
                Common.Forms.MessageBox.Show(this, sErrorMessage,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
