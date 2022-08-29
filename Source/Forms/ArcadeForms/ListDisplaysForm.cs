/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class ListDisplaysForm : Arcade.Forms.Form
    {
        #region "Enumerations"
        public enum EListDisplaysFormType
        {
            ListDisplays,
            EditDisplays
        };
        #endregion

        #region "Member Variables"
        private EListDisplaysFormType m_ListDisplaysFormType = EListDisplaysFormType.EditDisplays;
        private System.Int32 m_nDisplayId = -1;
        private System.String m_sDisplayType;
        private System.String m_sDisplayResolution;
        private System.String m_sDisplayColors;
        private System.String m_sDisplayOrientation;
        #endregion

        #region "Constructor"
        public ListDisplaysForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Properties"
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
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { listViewDisplays };
            }
        }
        #endregion

        #region "List Displays Event Handlers"
        private void ListDisplaysForm_Shown(object sender, EventArgs e)
        {
            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                InitializeControls();

                RunOnUIThreadWait(() =>
                {
                    this.BusyControlVisible = false;
                });
            }, "List Displays Form Initialize Thread");
        }
        #endregion

        #region "List View Event Handlers"
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
        #endregion

        #region "Button Event Handlers"
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            DisplayEntryForm DisplayEntry = new DisplayEntryForm();
            System.String sErrorMessage;
            System.Int32 nNewDisplayId;
            DatabaseDefs.TDisplay Display;
            System.Windows.Forms.ListViewItem Item;
            System.Boolean bResult;

            new Common.Forms.FormLocation(DisplayEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            DisplayEntry.DisplayEntryFormType = Arcade.Forms.DisplayEntryForm.EDisplayEntryFormType.NewDisplay;

            if (DisplayEntry.ShowDialog(this) == DialogResult.OK)
            {
                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.AddDisplay(DisplayEntry.DisplayName,
                                                  DisplayEntry.DisplayType,
                                                  DisplayEntry.DisplayResolution,
                                                  DisplayEntry.DisplayColors,
                                                  DisplayEntry.DisplayOrientation,
                                                  out nNewDisplayId,
                                                  out sErrorMessage);

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
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
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }

                        this.BusyControlVisible = false;

                        DisplayEntry.Dispose();
                    });
                }, "List Data Form Details Thread");
            }
            else
            {
                DisplayEntry.Dispose();
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
            System.Boolean bResult;

            Display = (DatabaseDefs.TDisplay)listViewDisplays.Items[nIndex].Tag;

            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                bResult = Database.DeleteDisplay(Display.nDisplayId, out sErrorMessage);

                RunOnUIThreadWait(() =>
                {
                    if (bResult)
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

                    this.BusyControlVisible = false;
                });
            }, "List Displays Form Delete Thread");
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
        #endregion

        #region "Internal Helpers"
        private void EditDisplay()
        {
            DisplayEntryForm DisplayEntry = new DisplayEntryForm();
            System.Int32 nIndex = listViewDisplays.SelectedIndices[0];
            System.String sErrorMessage;
            DatabaseDefs.TDisplay Display;
            System.Windows.Forms.ListViewItem Item;
            System.Boolean bResult;

            new Common.Forms.FormLocation(DisplayEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            Display = (DatabaseDefs.TDisplay)listViewDisplays.Items[nIndex].Tag;

            DisplayEntry.DisplayEntryFormType = Arcade.Forms.DisplayEntryForm.EDisplayEntryFormType.EditDisplay;

            DisplayEntry.DisplayName = Display.sDisplayName;
            DisplayEntry.DisplayType = Display.sDisplayType;
            DisplayEntry.DisplayResolution = Display.sDisplayResolution;
            DisplayEntry.DisplayColors = Display.sDisplayColors;
            DisplayEntry.DisplayOrientation = Display.sDisplayOrientation;

            if (DisplayEntry.ShowDialog(this) == DialogResult.OK)
            {
                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.EditDisplay(Display.nDisplayId,
                                                   DisplayEntry.DisplayName,
                                                   DisplayEntry.DisplayType,
                                                   DisplayEntry.DisplayResolution,
                                                   DisplayEntry.DisplayColors,
                                                   DisplayEntry.DisplayOrientation,
                                                   out sErrorMessage);

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
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

                        this.BusyControlVisible = false;

                        DisplayEntry.Dispose();
                    });
                }, "List Displays Form Edit Thread");
            }
            else
            {
                DisplayEntry.Dispose();
            }
        }

        private void OnOK()
        {
            System.Int32 nIndex = listViewDisplays.SelectedIndices[0];
            DatabaseDefs.TDisplay Display;

            Display = (DatabaseDefs.TDisplay)listViewDisplays.Items[nIndex].Tag;

            m_nDisplayId = Display.nDisplayId;
            m_sDisplayType = Display.sDisplayType;
            m_sDisplayResolution = Display.sDisplayResolution;
            m_sDisplayColors = Display.sDisplayColors;
            m_sDisplayOrientation = Display.sDisplayOrientation;

            DialogResult = DialogResult.OK;

            Close();
        }

        private void InitializeControls()
        {
            System.Collections.Generic.List<DatabaseDefs.TDisplay> DisplaysList;
            System.Windows.Forms.ListViewItem Item;
            System.String sErrorMessage;
            System.Boolean bResult;

            Common.Debug.Thread.IsWorkerThread();

            bResult = Database.GetDisplays(out DisplaysList,
                                           out sErrorMessage);

            RunOnUIThreadWait(() =>
            {
                buttonOK.Enabled = false;

                if (bResult)
                {
                    listViewDisplays.BeginUpdate();

                    foreach (DatabaseDefs.TDisplay Display in DisplaysList)
                    {
                        Item = listViewDisplays.Items.Add(Display.sDisplayName);

                        Item.Tag = Display;
                    }

                    listViewDisplays.EndUpdate();
                }

                if (listViewDisplays.Items.Count == 0)
                {
                    listViewDisplays.Enabled = false;
                }

                switch (this.m_ListDisplaysFormType)
                {
                    case EListDisplaysFormType.ListDisplays:
                        UpdateControlVisibility(buttonAdd, false);
                        UpdateControlVisibility(buttonEdit, false);
                        UpdateControlVisibility(buttonDelete, false);
                        break;
                    case EListDisplaysFormType.EditDisplays:
                        buttonEdit.Enabled = false;
                        buttonDelete.Enabled = false;

                        UpdateControlVisibility(buttonOK, false);

                        buttonCancel.Text = "Close";
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                }

                if (!bResult)
                {
                    Common.Forms.MessageBox.Show(this, sErrorMessage,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);
                }

            });
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
