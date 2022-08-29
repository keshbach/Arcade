/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class BoardPartEntryForm : Arcade.Forms.Form
    {
        #region "Enumerations"
        public enum EBoardPartEntryFormType
        {
            AddBoardPart,
            EditBoardPart
        }
        #endregion

        #region "Member Variables"
        private EBoardPartEntryFormType m_BoardPartEntryFormType = EBoardPartEntryFormType.AddBoardPart;
        private System.String m_sPartPosition = "";
        private System.String m_sPartLocation = "";
        private System.String m_sPartDescription = "";
        private System.String m_sPartName = "";
        private System.String m_sPartCategoryName = "";
        private System.String m_sPartTypeName = "";
        private System.String m_sPartPackageName = "";

        private Common.Collections.StringSortedList<System.Int32> m_BoardPartLocationList = null;

        private System.Boolean m_bTimerExpiredDuringFind = false;
        #endregion

        #region "Constructor"
        public BoardPartEntryForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Properties"
        public EBoardPartEntryFormType BoardPartEntryFormType
        {
            set
            {
                m_BoardPartEntryFormType = value;
            }
        }

        public System.String PartPosition
        {
            get
            {
                return m_sPartPosition;
            }
            set
            {
                m_sPartPosition = value;
            }
        }

        public System.String PartLocation
        {
            get
            {
                return m_sPartLocation;
            }
            set
            {
                m_sPartLocation = value;
            }
        }

        public System.String PartDescription
        {
            get
            {
                return m_sPartDescription;
            }
            set
            {
                m_sPartDescription = value;
            }
        }

        public System.String PartName
        {
            get
            {
                return m_sPartName;
            }
            set
            {
                m_sPartName = value;
            }
        }

        public System.String PartCategoryName
        {
            get
            {
                return m_sPartCategoryName;
            }
            set
            {
                m_sPartCategoryName = value;
            }
        }

        public System.String PartTypeName
        {
            get
            {
                return m_sPartTypeName;
            }
            set
            {
                m_sPartTypeName = value;
            }
        }

        public System.String PartPackageName
        {
            get
            {
                return m_sPartPackageName;
            }
            set
            {
                m_sPartPackageName = value;
            }
        }
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { listViewParts, splitContainerBoardPart };
            }
        }

        protected override System.Windows.Forms.Control[] ControlClearSelection
        {
            get
            {
                return new System.Windows.Forms.Control[] { textBoxPosition, textBoxDescription, textBoxTimerFind };
            }
        }

        protected override Common.Forms.ListView.ESortOrder GetListViewDefaultSortOrderFormLocationSetting(
            Common.Forms.ListView ListView)
        {
            return Common.Forms.ListView.ESortOrder.GroupSequential;
        }
        #endregion

        #region "Board Part Entry Event Handlers"
        private void BoardPartEntryForm_Shown(object sender, EventArgs e)
        {
            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                InitializeControls();

                RunOnUIThreadWait(() =>
                {
                    this.BusyControlVisible = false;
                });
            }, "Board Part Entry Form Initialize Thread");
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonFindPart_Click(object sender, EventArgs e)
        {
            FindPartForm PartForm = new FindPartForm();

            new Common.Forms.FormLocation(PartForm, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            PartForm.ShowDialog(this);

            FindPartsFromPartName();

            PartForm.Dispose();
        }

        private void buttonNewPart_Click(object sender, EventArgs e)
        {
            Arcade.Forms.PartEntryForm PartEntry = new Arcade.Forms.PartEntryForm();
            System.Int32 nPartId;
            System.String sErrorMessage;
            System.Boolean bResult;

            new Common.Forms.FormLocation(PartEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            PartEntry.PartEntryFormType = Arcade.Forms.PartEntryForm.EPartEntryFormType.NewPart;

            if (PartEntry.ShowDialog(this) == DialogResult.OK)
            {
                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.AddPartGroup(PartEntry.PartName,
                                                    PartEntry.PartCategoryName,
                                                    PartEntry.PartTypeName,
                                                    PartEntry.PartPackageName,
                                                    PartEntry.PartPinouts,
                                                    PartEntry.PartDatasheetColl,
                                                    out nPartId,
                                                    out sErrorMessage);

                    RunOnUIThreadWait(() =>
                    {
                        this.BusyControlVisible = false;

                        if (bResult)
                        {
                            FindPartsFromPartName();
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    });
                }, "Board Part Entry Form New Thread");
            }

            PartEntry.Dispose();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            System.Int32 nIndex = listViewParts.SelectedIndices[0];

            if (textBoxPosition.Text.Length > 0)
            {
                m_sPartPosition = textBoxPosition.Text;
            }
            else
            {
                m_sPartPosition = " ";
            }

            m_sPartLocation = (System.String)comboBoxLocation.SelectedItem;
            m_sPartDescription = textBoxDescription.Text;
            m_sPartName = listViewParts.Items[nIndex].SubItems[0].Text;
            m_sPartCategoryName = listViewParts.Items[nIndex].SubItems[1].Text;
            m_sPartTypeName = listViewParts.Items[nIndex].SubItems[2].Text;
            m_sPartPackageName = listViewParts.Items[nIndex].SubItems[3].Text;

            DialogResult = DialogResult.OK;

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }
        #endregion

        #region "Text Box Timer Event Handlers"
        private void textBoxTimerFind_KeyPressTimerExpired(object sender, EventArgs e)
        {
            if (!this.BusyControlVisible)
            {
                FindPartsFromPartName();
            }
            else
            {
                m_bTimerExpiredDuringFind = true;
            }
        }
        #endregion

        #region "List View Event Handlers"
        private void listViewParts_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            buttonOK.Enabled = true;
        }

        private void listViewParts_DoubleClick(object sender, EventArgs e)
        {
            buttonOK.PerformClick();
        }
        #endregion

        #region "Internal Helpers"
        private void FindPartsFromPartName()
        {
            Common.Debug.Thread.IsUIThread();

            buttonOK.Enabled = false;
            listViewParts.Enabled = false;

            if (textBoxTimerFind.Text.Length > 0)
            {
                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    RefreshMatchingKeywords(textBoxTimerFind.Text);

                    RunOnUIThreadWait(() =>
                    {
                        this.BusyControlVisible = false;

                        if (m_bTimerExpiredDuringFind)
                        {
                            m_bTimerExpiredDuringFind = false;

                            FindPartsFromPartName();
                        }
                    });
                }, "Board Part Entry Form Find Thread");
            }
        }

        private void RefreshMatchingKeywords(System.String sKeyword)
        {
            System.Collections.Generic.List<DatabaseDefs.TPart> PartList;
            System.String sErrorMessage;
            System.Windows.Forms.ListViewItem Item;
            System.Boolean bResult;

            Common.Debug.Thread.IsWorkerThread();

            bResult = Database.GetPartsMatchingKeyword(sKeyword,
                                                       DatabaseDefs.EKeywordMatchingCriteria.Start,
                                                       out PartList,
                                                       out sErrorMessage);

            RunOnUIThreadWait(() => {
                if (bResult)
                {
                    listViewParts.BeginUpdate();

                    listViewParts.Sorting = Common.Forms.ListView.ESortOrder.None;

                    listViewParts.Items.Clear();

                    foreach (DatabaseDefs.TPart Part in PartList)
                    {
                        Item = listViewParts.Items.Add(Part.sPartName);

                        Item.SubItems.Add(Part.sPartCategoryName);
                        Item.SubItems.Add(Part.sPartTypeName);
                        Item.SubItems.Add(Part.sPartPackageName);

                        if (0 == System.String.Compare(m_sPartName, Part.sPartName, true) &&
                            0 == System.String.Compare(m_sPartCategoryName, Part.sPartCategoryName, true) &&
                            0 == System.String.Compare(m_sPartTypeName, Part.sPartTypeName, true) &&
                            0 == System.String.Compare(m_sPartPackageName, Part.sPartPackageName, true))
                        {
                            listViewParts.Enabled = true;

                            Item.EnsureVisible();

                            Item.Selected = true;
                        }
                    }

                    listViewParts.EndUpdate();

                    if (listViewParts.Items.Count > 0)
                    {
                        listViewParts.Enabled = true;

                        if (listViewParts.Items.Count == 1)
                        {
                            listViewParts.Items[0].Selected = true;

                            buttonOK.Enabled = true;
                        }
                    }
                    else
                    {
                        listViewParts.Enabled = false;
                    }
                }
                else
                {
                    Common.Forms.MessageBox.Show(this, sErrorMessage,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);
                }
            });
        }

        private void InitializeControls()
        {
            System.String sKeyword = null;
            DatabaseDefs.TBoardPartLocationLens BoardPartLocationLens;
            DatabaseDefs.TPartLens PartLens;

            Common.Debug.Thread.IsWorkerThread();

            if (Database.GetBoardPartLocationMaxLens(out BoardPartLocationLens))
            {
                RunOnUIThreadWait(() =>
                {
                    textBoxPosition.MaxLength = BoardPartLocationLens.nBoardPartPositionLen;
                    textBoxDescription.MaxLength = BoardPartLocationLens.nBoardPartDescriptionLen;
                });
            }

            if (Database.GetPartMaxLens(out PartLens))
            {
                RunOnUIThreadWait(() =>
                {
                    textBoxTimerFind.MaxLength = PartLens.nPartNameLen;
                });
            }

            if (Database.GetBoardPartLocationList(out m_BoardPartLocationList))
            {
                RunOnUIThreadWait(() =>
                {
                    comboBoxLocation.BeginUpdate();

                    foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_BoardPartLocationList)
                    {
                        comboBoxLocation.Items.Add(Pair.Key);
                    }

                    comboBoxLocation.EndUpdate();
                });
            }

            RunOnUIThreadWait(() =>
            {
                switch (m_BoardPartEntryFormType)
                {
                    case EBoardPartEntryFormType.AddBoardPart:
                        Text = "Add...";

                        m_sPartLocation = Database.DefBoardPartLocation;

                        buttonOK.Enabled = false;
                        break;
                    case EBoardPartEntryFormType.EditBoardPart:
                        Text = "Edit...";

                        textBoxPosition.Text = m_sPartPosition;
                        textBoxDescription.Text = m_sPartDescription;

                        textBoxTimerFind.Text = m_sPartName;

                        sKeyword = textBoxTimerFind.Text;
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                }

                comboBoxLocation.SelectedIndex = m_BoardPartLocationList.IndexOfKey(m_sPartLocation);
            });

            if (!System.String.IsNullOrEmpty(sKeyword))
            {
                RefreshMatchingKeywords(sKeyword);
            }
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
