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
        public partial class ListDisplaysForm : System.Windows.Forms.Form
        {
            public enum EListDisplaysFormType
            {
                ListDisplays,
                EditDisplays
            };

            private EListDisplaysFormType m_ListDisplaysFormType = EListDisplaysFormType.EditDisplays;
            private System.Int32 m_nDisplayId = -1;
            private System.String m_sDisplayName;
            private System.String m_sDisplayType;
            private System.String m_sDisplayResolution;
            private System.String m_sDisplayColors;
            private System.String m_sDisplayOrientation;

            public ListDisplaysForm()
            {
                InitializeComponent();
            }

            public EListDisplaysFormType ListDisplaysFormType
            {
                set
                {
                    m_ListDisplaysFormType = value;
                }
            }

            public System.Int32 DisplayId
            {
                get
                {
                    return m_nDisplayId;
                }
            }

            public System.String DisplayType
            {
                get
                {
                    return m_sDisplayType;
                }
            }

            public System.String DisplayResolution
            {
                get
                {
                    return m_sDisplayResolution;
                }
            }

            public System.String DisplayColors
            {
                get
                {
                    return m_sDisplayColors;
                }
            }

            public System.String DisplayOrientation
            {
                get
                {
                    return m_sDisplayOrientation;
                }
            }

            private void ListDisplaysForm_Load(object sender, EventArgs e)
            {
                System.Collections.Generic.List<DatabaseDefs.TDisplay> DisplaysList;
                System.String sErrorMessage;
                System.Windows.Forms.ListViewItem Item;

                buttonOK.Enabled = false;

                using (new Common.Forms.WaitCursor(this))
                {
                    if (true == Database.GetDisplays(out DisplaysList,
                                                     out sErrorMessage))
                    {
                        listViewDisplays.BeginUpdate();

                        foreach (DatabaseDefs.TDisplay Display in DisplaysList)
                        {
                            Item = listViewDisplays.Items.Add(Display.sDisplayName);

                            Item.Tag = Display;
                        }

                        listViewDisplays.AutosizeColumns();
                        listViewDisplays.EndUpdate();
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }
                }

                if (listViewDisplays.Items.Count == 0)
                {
                    listViewDisplays.Enabled = false;
                }

                switch (this.m_ListDisplaysFormType)
                {
                    case EListDisplaysFormType.ListDisplays:
                        buttonAdd.Visible = false;
                        buttonEdit.Visible = false;
                        buttonDelete.Visible = false;
                        break;
                    case EListDisplaysFormType.EditDisplays:
                        buttonEdit.Enabled = false;
                        buttonDelete.Enabled = false;

                        buttonOK.Visible = false;
                        buttonCancel.Text = "Close";
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                }
            }

            private void listViewDisplays_DoubleClick(object sender, EventArgs e)
            {
                if (m_ListDisplaysFormType == EListDisplaysFormType.EditDisplays)
                {
                    EditDisplay();
                }
                else
                {
                    OnOK();
                }
            }

            private void listViewDisplays_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
            {
                buttonEdit.Enabled = true;
                buttonDelete.Enabled = true;

                buttonOK.Enabled = true;
            }

            private void buttonAdd_Click(object sender, EventArgs e)
            {
                Arcade.Forms.DisplayEntryForm DisplayEntry = new Arcade.Forms.DisplayEntryForm();
                System.String sErrorMessage;
                System.Int32 nNewDisplayId;
                DatabaseDefs.TDisplay Display;
                System.Windows.Forms.ListViewItem Item;

                DisplayEntry.DisplayEntryFormType = Arcade.Forms.DisplayEntryForm.EDisplayEntryFormType.NewDisplay;

                if (DisplayEntry.ShowDialog(this) == DialogResult.OK)
                {
                    using (new Common.Forms.WaitCursor(this))
                    {
                        if (Database.AddDisplay(DisplayEntry.DisplayName,
                                                DisplayEntry.DisplayType,
                                                DisplayEntry.DisplayResolution,
                                                DisplayEntry.DisplayColors,
                                                DisplayEntry.DisplayOrientation,
                                                out nNewDisplayId,
                                                out sErrorMessage))
                        {
                            Display = new DatabaseDefs.TDisplay();

                            Display.nDisplayId = nNewDisplayId;
                            Display.sDisplayName = DisplayEntry.DisplayName;
                            Display.sDisplayType = DisplayEntry.DisplayType;
                            Display.sDisplayResolution = DisplayEntry.DisplayResolution;
                            Display.sDisplayColors = DisplayEntry.DisplayColors;
                            Display.sDisplayOrientation = DisplayEntry.DisplayOrientation;

                            listViewDisplays.Enabled = true;

                            Item = listViewDisplays.Items.Add(Display.sDisplayName);

                            Item.Tag = Display;

                            Item.Selected = true;
                            Item.Focused = true;

                            Item.EnsureVisible();

                            listViewDisplays.AutosizeColumns();
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                }
            }

            private void buttonEdit_Click(object sender, EventArgs e)
            {
                EditDisplay();
            }

            private void buttonDelete_Click(object sender, EventArgs e)
            {
                System.Int32 nIndex = listViewDisplays.SelectedIndices[0];
                System.String sErrorMessage;
                DatabaseDefs.TDisplay Display;
                System.Windows.Forms.ListViewItem Item;

                Display = (DatabaseDefs.TDisplay)listViewDisplays.Items[nIndex].Tag;

                using (new Common.Forms.WaitCursor(this))
                {
                    if (Database.DeleteDisplay(Display.nDisplayId, out sErrorMessage))
                    {
                        listViewDisplays.Items.RemoveAt(nIndex);

                        if (listViewDisplays.Items.Count > 0)
                        {
                            if (nIndex == listViewDisplays.Items.Count)
                            {
                                --nIndex;
                            }

                            Item = listViewDisplays.Items[nIndex];

                            Item.Selected = true;
                            Item.Focused = true;

                            Item.EnsureVisible();
                        }
                        else
                        {
                            listViewDisplays.Enabled = false;

                            buttonEdit.Enabled = false;
                            buttonDelete.Enabled = false;
                        }
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }
                }
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

            private void EditDisplay()
            {
                Arcade.Forms.DisplayEntryForm DisplayEntry = new Arcade.Forms.DisplayEntryForm();
                System.Int32 nIndex = listViewDisplays.SelectedIndices[0];
                System.String sErrorMessage;
                DatabaseDefs.TDisplay Display;
                System.Windows.Forms.ListViewItem Item;

                Display = (DatabaseDefs.TDisplay)listViewDisplays.Items[nIndex].Tag;

                DisplayEntry.DisplayEntryFormType = Arcade.Forms.DisplayEntryForm.EDisplayEntryFormType.EditDisplay;

                DisplayEntry.DisplayName = Display.sDisplayName;
                DisplayEntry.DisplayType = Display.sDisplayType;
                DisplayEntry.DisplayResolution = Display.sDisplayResolution;
                DisplayEntry.DisplayColors = Display.sDisplayColors;
                DisplayEntry.DisplayOrientation = Display.sDisplayOrientation;

                if (DisplayEntry.ShowDialog(this) == DialogResult.OK)
                {
                    using (new Common.Forms.WaitCursor(this))
                    {
                        if (Database.EditDisplay(Display.nDisplayId,
                                                 DisplayEntry.DisplayName,
                                                 DisplayEntry.DisplayType,
                                                 DisplayEntry.DisplayResolution,
                                                 DisplayEntry.DisplayColors,
                                                 DisplayEntry.DisplayOrientation,
                                                 out sErrorMessage))
                        {
                            Display.sDisplayName = DisplayEntry.DisplayName;
                            Display.sDisplayType = DisplayEntry.DisplayType;
                            Display.sDisplayResolution = DisplayEntry.DisplayResolution;
                            Display.sDisplayColors = DisplayEntry.DisplayColors;
                            Display.sDisplayOrientation = DisplayEntry.DisplayOrientation;

                            Item = listViewDisplays.Items[nIndex];

                            Item.Text = Display.sDisplayName;
                            Item.Tag = Display;
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                }
            }

            private void OnOK()
            {
                System.Int32 nIndex = listViewDisplays.SelectedIndices[0];
                DatabaseDefs.TDisplay Display;

                Display = (DatabaseDefs.TDisplay)listViewDisplays.Items[nIndex].Tag;

                m_nDisplayId = Display.nDisplayId;
                m_sDisplayName = Display.sDisplayName;
                m_sDisplayType = Display.sDisplayType;
                m_sDisplayResolution = Display.sDisplayResolution;
                m_sDisplayColors = Display.sDisplayColors;
                m_sDisplayOrientation = Display.sDisplayOrientation;

                DialogResult = DialogResult.OK;

                Close();
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
