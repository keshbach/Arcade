/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace Arcade.Forms
{
    /// <summary>
    /// Summary description for BoardPartDetailsForm.
    /// </summary>
    public partial class BoardPartDetailsForm : Arcade.Forms.Form
    {
        #region "Member Variables"
        private System.Int32 m_nBoardId = -1;

        private System.Collections.Generic.Dictionary<String, Common.Forms.ListView.ESortOrder> m_LabelSortOrderDict = new System.Collections.Generic.Dictionary<String, Common.Forms.ListView.ESortOrder>();
        #endregion

        #region "Constructor"
        public BoardPartDetailsForm()
        {
            InitializeComponent();

            InitializeData();
        }
        #endregion

        #region "Properties"
        public System.Int32 BoardId
        {
            set
            {
                m_nBoardId = value;
            }
        }
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { listViewBoardParts, comboBoxSorting };
            }
        }

        protected override Common.Forms.ListView.ESortOrder GetListViewDefaultSortOrderFormLocationSetting(
            Common.Forms.ListView ListView)
        {
            return Common.Forms.ListView.ESortOrder.GroupSequential;
        }

        protected override System.Int32 GetComboBoxDefaultSelectedIndexSetting(
            System.Windows.Forms.ComboBox ComboBox)
        {
            System.Int32 nIndex = 0;

            foreach (System.String sLabel in m_LabelSortOrderDict.Keys)
            {
                if (sLabel == "Group Sequential")
                {
                    return nIndex;
                }

                ++nIndex;
            }

            return 0;
        }
        #endregion

        #region "Board Part Details Event Handlers"
        private void BoardPartDetailsForm_Shown(object sender, EventArgs e)
        {
            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                InitializeControls();

                RunOnUIThreadWait(() =>
                {
                    this.BusyControlVisible = false;
                });
            }, "Board Part Details Form Initialize Thread");
        }
        #endregion

        #region "List View Event Handlers"
        private void listViewBoardParts_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            buttonEdit.Enabled = true;
            buttonDelete.Enabled = true;
            buttonExport.Enabled = true;
        }

        private void listViewBoardParts_DoubleClick(object sender, EventArgs e)
        {
            EditBoardPart();
        }
        #endregion

        #region "Combo Box Event Handlers"
        private void comboBoxSorting_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewBoardParts.Sorting = m_LabelSortOrderDict[(System.String)comboBoxSorting.SelectedItem];

            if (listViewBoardParts.Items.Count > 0)
            {
                listViewBoardParts.SelectedItems[0].EnsureVisible();
            }
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            BoardPartEntryForm BoardPartEntry = new BoardPartEntryForm();
            System.Int32 nNewBoardPartId, nPartId;
            System.String sErrorMessage;
            DatabaseDefs.TBoardPart BoardPart;
            System.Windows.Forms.ListViewItem Item;
            System.Boolean bResult;

            new Common.Forms.FormLocation(BoardPartEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            BoardPartEntry.BoardPartEntryFormType = BoardPartEntryForm.EBoardPartEntryFormType.AddBoardPart;

            if (BoardPartEntry.ShowDialog(this) == DialogResult.OK)
            {
                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.AddGameBoardPart(m_nBoardId,
                                                        BoardPartEntry.PartPosition,
                                                        BoardPartEntry.PartLocation,
                                                        BoardPartEntry.PartDescription,
                                                        BoardPartEntry.PartName,
                                                        BoardPartEntry.PartCategoryName,
                                                        BoardPartEntry.PartTypeName,
                                                        BoardPartEntry.PartPackageName,
                                                        out nNewBoardPartId,
                                                        out nPartId,
                                                        out sErrorMessage);

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
                        {
                            BoardPart = new DatabaseDefs.TBoardPart();

                            BoardPart.BoardPartLocation.nBoardPartId = nNewBoardPartId;
                            BoardPart.BoardPartLocation.sBoardPartPosition = BoardPartEntry.PartPosition;
                            BoardPart.BoardPartLocation.sBoardPartLocation = BoardPartEntry.PartLocation;
                            BoardPart.BoardPartLocation.sBoardPartDescription = BoardPartEntry.PartDescription;
                            BoardPart.Part.nPartId = nPartId;
                            BoardPart.Part.sPartName = BoardPartEntry.PartName;
                            BoardPart.Part.sPartCategoryName = BoardPartEntry.PartCategoryName;
                            BoardPart.Part.sPartTypeName = BoardPartEntry.PartTypeName;
                            BoardPart.Part.sPartPackageName = BoardPartEntry.PartPackageName;

                            listViewBoardParts.Enabled = true;

                            Item = listViewBoardParts.Items.Add(BoardPart.BoardPartLocation.sBoardPartPosition);

                            Item.Tag = BoardPart;

                            Item.SubItems.Add(BoardPart.Part.sPartName);
                            Item.SubItems.Add(BoardPart.Part.sPartCategoryName);
                            Item.SubItems.Add(BoardPart.Part.sPartTypeName);
                            Item.SubItems.Add(BoardPart.Part.sPartPackageName);
                            Item.SubItems.Add(BoardPart.BoardPartLocation.sBoardPartLocation);

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

                        BoardPartEntry.Dispose();
                    });
                }, "Board Part Details Form Add Thread");
            }
            else
            {
                BoardPartEntry.Dispose();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditBoardPart();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            System.Int32 nIndex = listViewBoardParts.SelectedIndices[0];
            System.String sErrorMessage;
            DatabaseDefs.TBoardPart BoardPart;
            System.Windows.Forms.ListViewItem Item;
            System.Boolean bResult;

            BoardPart = (DatabaseDefs.TBoardPart)listViewBoardParts.Items[nIndex].Tag;

            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                bResult = Database.DeleteGameBoardPart(BoardPart.BoardPartLocation.nBoardPartId,
                                                       out sErrorMessage);

                RunOnUIThreadWait(() =>
                {
                    if (bResult)
                    {
                        listViewBoardParts.Items.RemoveAt(nIndex);

                        if (listViewBoardParts.Items.Count > 0)
                        {
                            if (nIndex == listViewBoardParts.Items.Count)
                            {
                                --nIndex;
                            }

                            Item = listViewBoardParts.Items[nIndex];

                            Item.Selected = true;
                            Item.Focused = true;

                            Item.EnsureVisible();
                        }
                        else
                        {
                            listViewBoardParts.Enabled = false;

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
            }, "Board Part Details Form Delete Thread");
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            System.String sFile, sErrorMessage;
            System.Boolean bResult;

            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                bResult = CreateExportFile(out sFile, out sErrorMessage) &&
                          OpenFile(sFile, ref sErrorMessage);

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
            }, "Board Part Details Form Export Thread");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region "Internal Helpers"
        private void EditBoardPart()
        {
            BoardPartEntryForm BoardPartEntry = new BoardPartEntryForm();
            System.Int32 nIndex = listViewBoardParts.SelectedIndices[0];
            System.String sErrorMessage;
            DatabaseDefs.TBoardPart BoardPart;
            System.Windows.Forms.ListViewItem Item;
            System.Boolean bResult;

            Common.Debug.Thread.IsUIThread();

            new Common.Forms.FormLocation(BoardPartEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            BoardPart = (DatabaseDefs.TBoardPart)listViewBoardParts.Items[nIndex].Tag;

            BoardPartEntry.BoardPartEntryFormType = BoardPartEntryForm.EBoardPartEntryFormType.EditBoardPart;
            BoardPartEntry.PartPosition = BoardPart.BoardPartLocation.sBoardPartPosition;
            BoardPartEntry.PartLocation = BoardPart.BoardPartLocation.sBoardPartLocation;
            BoardPartEntry.PartDescription = BoardPart.BoardPartLocation.sBoardPartDescription;
            BoardPartEntry.PartName = BoardPart.Part.sPartName;
            BoardPartEntry.PartCategoryName = BoardPart.Part.sPartCategoryName;
            BoardPartEntry.PartTypeName = BoardPart.Part.sPartTypeName;
            BoardPartEntry.PartPackageName = BoardPart.Part.sPartPackageName;

            if (BoardPartEntry.ShowDialog(this) == DialogResult.OK)
            {
                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.EditGameBoardPart(BoardPart.BoardPartLocation.nBoardPartId,
                                                         BoardPartEntry.PartPosition,
                                                         BoardPartEntry.PartLocation,
                                                         BoardPartEntry.PartDescription,
                                                         BoardPartEntry.PartName,
                                                         BoardPartEntry.PartCategoryName,
                                                         BoardPartEntry.PartTypeName,
                                                         BoardPartEntry.PartPackageName,
                                                         out BoardPart.Part.nPartId,
                                                         out sErrorMessage);

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
                        {
                            BoardPart.BoardPartLocation.sBoardPartPosition = BoardPartEntry.PartPosition;
                            BoardPart.BoardPartLocation.sBoardPartLocation = BoardPartEntry.PartLocation;
                            BoardPart.BoardPartLocation.sBoardPartDescription = BoardPartEntry.PartDescription;
                            BoardPart.Part.sPartName = BoardPartEntry.PartName;
                            BoardPart.Part.sPartCategoryName = BoardPartEntry.PartCategoryName;
                            BoardPart.Part.sPartTypeName = BoardPartEntry.PartTypeName;
                            BoardPart.Part.sPartPackageName = BoardPartEntry.PartPackageName;

                            Item = listViewBoardParts.Items[nIndex];

                            Item.Text = BoardPart.BoardPartLocation.sBoardPartPosition;
                            Item.Tag = BoardPart;

                            Item.SubItems[1].Text = BoardPart.Part.sPartName;
                            Item.SubItems[2].Text = BoardPart.Part.sPartCategoryName;
                            Item.SubItems[3].Text = BoardPart.Part.sPartTypeName;
                            Item.SubItems[4].Text = BoardPart.Part.sPartPackageName;
                            Item.SubItems[5].Text = BoardPart.BoardPartLocation.sBoardPartLocation;
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }

                        this.BusyControlVisible = false;

                        BoardPartEntry.Dispose();
                    });
                }, "Board Part Details Form Edit Thread");
            }
            else
            {
                BoardPartEntry.Dispose();
            }
        }

        private void InitializeData()
        {
            Common.Debug.Thread.IsUIThread();

            m_LabelSortOrderDict["Ascending"] = Common.Forms.ListView.ESortOrder.Ascending;
            m_LabelSortOrderDict["Group"] = Common.Forms.ListView.ESortOrder.Group;
            m_LabelSortOrderDict["Sequential"] = Common.Forms.ListView.ESortOrder.Sequential;
            m_LabelSortOrderDict["Group Sequential"] = Common.Forms.ListView.ESortOrder.GroupSequential;
        }

        private void InitializeControls()
        {
            System.Collections.Generic.List<DatabaseDefs.TBoardPart> GameBoardPartsList;
            System.String sErrorMessage;
            System.Windows.Forms.ListViewItem Item;
            System.Boolean bResult;

            Common.Debug.Thread.IsWorkerThread();

            bResult = Database.GetGameBoardParts(m_nBoardId,
                                                 out GameBoardPartsList,
                                                 out sErrorMessage);

            RunOnUIThreadWait(() =>
            {
                comboBoxSorting.BeginUpdate();

                foreach (System.String sLabel in m_LabelSortOrderDict.Keys)
                {
                    comboBoxSorting.Items.Add(sLabel);
                }

                comboBoxSorting.EndUpdate();

                if (bResult)
                {
                    listViewBoardParts.BeginUpdate();

                    foreach (DatabaseDefs.TBoardPart BoardPart in GameBoardPartsList)
                    {
                        Item = listViewBoardParts.Items.Add(BoardPart.BoardPartLocation.sBoardPartPosition);

                        Item.Tag = BoardPart;

                        Item.SubItems.Add(BoardPart.Part.sPartName);
                        Item.SubItems.Add(BoardPart.Part.sPartCategoryName);
                        Item.SubItems.Add(BoardPart.Part.sPartTypeName);
                        Item.SubItems.Add(BoardPart.Part.sPartPackageName);
                        Item.SubItems.Add(BoardPart.BoardPartLocation.sBoardPartLocation);
                    }

                    listViewBoardParts.EndUpdate();
                }
                else
                {
                    Common.Forms.MessageBox.Show(this, sErrorMessage,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);

                    buttonAdd.Enabled = false;
                }

                if (listViewBoardParts.Items.Count > 0)
                {
                    listViewBoardParts.Items[0].Selected = true;
                    listViewBoardParts.Items[0].Focused = true;
                }
                else
                {
                    listViewBoardParts.Enabled = false;
                    buttonEdit.Enabled = false;
                    buttonDelete.Enabled = false;
                    buttonExport.Enabled = false;
                }
            });
        }

        private System.Boolean CreateExportFile(out System.String sFile, out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Collections.Generic.List<DatabaseDefs.TBoardPart> GameBoardPartsList;
            System.IO.StreamWriter StreamWriter;

            Common.Debug.Thread.IsWorkerThread();

            sFile = null;
            sErrorMessage = null;

            Common.Debug.Thread.IsWorkerThread();

            if (!Database.GetGameBoardParts(m_nBoardId,
                                            out GameBoardPartsList,
                                            out sErrorMessage))
            {
                return false;
            }

            try
            {
                if (Common.IO.TempFileManager.CreateTempFile(".txt", ref sFile))
                {
                    StreamWriter = new System.IO.StreamWriter(sFile);

                    StreamWriter.Write("\"Position\",\"Keyword\",\"Category\",\"Type\",\"Package\",\"Location\"");
                    StreamWriter.WriteLine();

                    foreach (DatabaseDefs.TBoardPart BoardPart in GameBoardPartsList)
                    {
                        StreamWriter.Write("\"");
                        StreamWriter.Write(BoardPart.BoardPartLocation.sBoardPartPosition);
                        StreamWriter.Write("\"");
                        StreamWriter.Write(",");
                        StreamWriter.Write("\"");
                        StreamWriter.Write(BoardPart.Part.sPartName);
                        StreamWriter.Write("\"");
                        StreamWriter.Write(",");
                        StreamWriter.Write("\"");
                        StreamWriter.Write(BoardPart.Part.sPartCategoryName);
                        StreamWriter.Write("\"");
                        StreamWriter.Write(",");
                        StreamWriter.Write("\"");
                        StreamWriter.Write(BoardPart.Part.sPartTypeName);
                        StreamWriter.Write("\"");
                        StreamWriter.Write(",");
                        StreamWriter.Write("\"");
                        StreamWriter.Write(BoardPart.Part.sPartPackageName);
                        StreamWriter.Write("\"");
                        StreamWriter.Write(",");
                        StreamWriter.Write("\"");
                        StreamWriter.Write(BoardPart.BoardPartLocation.sBoardPartLocation);
                        StreamWriter.Write("\"");
                        StreamWriter.WriteLine();
                    }

                    StreamWriter.Close();

                    bResult = true;
                }
                else
                {
                    sErrorMessage = "A temporary file could not be created.";
                }
            }
            catch (System.SystemException exception)
            {
                sErrorMessage = exception.Message;
            }

            return bResult;
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
