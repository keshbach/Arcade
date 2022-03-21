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
        /// Summary description for BoardPartDetailsForm.
        /// </summary>
        public partial class BoardPartDetailsForm : System.Windows.Forms.Form
        {
            private System.Int32 m_nBoardId = -1;

            public BoardPartDetailsForm()
            {
                InitializeComponent();
            }

            public System.Int32 BoardId
            {
                set
                {
                    m_nBoardId = value;
                }
            }

            private void BoardPartDetailsForm_Load(object sender, EventArgs e)
            {
                System.Collections.Generic.List<DatabaseDefs.TBoardPart> GameBoardPartsList;
                System.String sErrorMessage;
                System.Windows.Forms.ListViewItem Item;

                using (new Common.Forms.WaitCursor(this))
                {
                    if (Database.GetGameBoardParts(m_nBoardId,
                                                    out GameBoardPartsList,
                                                    out sErrorMessage))
                    {
                        listViewBoardParts.BeginUpdate();

                        listViewBoardParts.Sorting = Common.Forms.ListView.ESortOrder.None;

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

                        listViewBoardParts.Sorting = Common.Forms.ListView.ESortOrder.GroupSequential;

                        listViewBoardParts.AutosizeColumns();

                        listViewBoardParts.EndUpdate();
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);

                        buttonAdd.Enabled = false;
                    }
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

                comboBoxSorting.SelectedItem = "Group Sequential";
            }

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

            private void comboBoxSorting_SelectedIndexChanged(object sender, EventArgs e)
            {
                switch ((System.String)comboBoxSorting.SelectedItem)
                {
                    case "Ascending":
                        listViewBoardParts.Sorting = Common.Forms.ListView.ESortOrder.Ascending;
                        break;
                    case "Group":
                        listViewBoardParts.Sorting = Common.Forms.ListView.ESortOrder.Group;
                        break;
                    case "Sequential":
                        listViewBoardParts.Sorting = Common.Forms.ListView.ESortOrder.Sequential;
                        break;
                    case "Group Sequential":
                        listViewBoardParts.Sorting = Common.Forms.ListView.ESortOrder.GroupSequential;
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                }

                if (listViewBoardParts.Items.Count > 0)
                {
                    listViewBoardParts.SelectedItems[0].EnsureVisible();
                }
            }

            private void buttonAdd_Click(object sender, EventArgs e)
            {
                BoardPartEntryForm BoardPartEntry = new BoardPartEntryForm();
                System.Int32 nNewBoardPartId, nPartId;
                System.String sErrorMessage;
                DatabaseDefs.TBoardPart BoardPart;
                System.Windows.Forms.ListViewItem Item;

                BoardPartEntry.BoardPartEntryFormType = BoardPartEntryForm.EBoardPartEntryFormType.AddBoardPart;

                if (BoardPartEntry.ShowDialog(this) == DialogResult.OK)
                {
                    using (new Common.Forms.WaitCursor(this))
                    {
                        if (Database.AddGameBoardPart(m_nBoardId,
                                                      BoardPartEntry.PartPosition,
                                                      BoardPartEntry.PartLocation,
                                                      BoardPartEntry.PartDescription,
                                                      BoardPartEntry.PartName,
                                                      BoardPartEntry.PartCategoryName,
                                                      BoardPartEntry.PartTypeName,
                                                      BoardPartEntry.PartPackageName,
                                                      out nNewBoardPartId,
                                                      out nPartId,
                                                      out sErrorMessage))
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

                            listViewBoardParts.AutosizeColumns();
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
                EditBoardPart();
            }

            private void buttonDelete_Click(object sender, EventArgs e)
            {
                System.Int32 nIndex = listViewBoardParts.SelectedIndices[0];
                System.String sErrorMessage;
                DatabaseDefs.TBoardPart BoardPart;
                System.Windows.Forms.ListViewItem Item;

                BoardPart = (DatabaseDefs.TBoardPart)listViewBoardParts.Items[nIndex].Tag;

                using (new Common.Forms.WaitCursor(this))
                {
                    if (Database.DeleteGameBoardPart(BoardPart.BoardPartLocation.nBoardPartId,
                                                     out sErrorMessage))
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
                }
            }

            private void buttonExport_Click(object sender, EventArgs e)
            {
                System.String sTempFile = "";
                System.IO.StreamWriter StreamWriter;
                System.Diagnostics.ProcessStartInfo StartInfo;

                try
                {
                    if (Common.IO.TempFileManager.CreateTempFile(".txt", ref sTempFile))
                    {
                        StreamWriter = new System.IO.StreamWriter(sTempFile);

                        for (System.Int32 nIndex = 0;
                                nIndex < listViewBoardParts.Columns.Count; ++nIndex)
                        {
                            StreamWriter.Write("\"");
                            StreamWriter.Write(listViewBoardParts.Columns[nIndex].Text);
                            StreamWriter.Write("\"");

                            if (nIndex + 1 < listViewBoardParts.Columns.Count)
                            {
                                StreamWriter.Write(",");
                            }
                        }

                        StreamWriter.WriteLine();

                        for (System.Int32 nIndex = 0;
                                nIndex < listViewBoardParts.Items.Count; ++nIndex)
                        {
                            for (System.Int32 nIndex2 = 0;
                                nIndex2 < listViewBoardParts.Items[nIndex].SubItems.Count;
                                ++nIndex2)
                            {
                                StreamWriter.Write("\"");
                                StreamWriter.Write(listViewBoardParts.Items[nIndex].SubItems[nIndex2].Text);
                                StreamWriter.Write("\"");

                                if (nIndex2 + 1 < listViewBoardParts.Items[nIndex].SubItems.Count)
                                {
                                    StreamWriter.Write(",");
                                }
                            }

                            StreamWriter.WriteLine();
                        }

                        StreamWriter.Close();

                        StartInfo = new System.Diagnostics.ProcessStartInfo();

                        StartInfo.FileName = sTempFile;
                        StartInfo.UseShellExecute = true;
                        StartInfo.Verb = "Open";
                        StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;

                        System.Diagnostics.Process.Start(StartInfo);
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this, "A temporary file could not be created.",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }
                }

                catch (System.SystemException exception)
                {
                    Common.Forms.MessageBox.Show(this, exception.Message,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);
                }
            }

            private void buttonClose_Click(object sender, EventArgs e)
            {
                Close();
            }

            private void EditBoardPart()
            {
                BoardPartEntryForm BoardPartEntry = new BoardPartEntryForm();
                System.Int32 nIndex = listViewBoardParts.SelectedIndices[0];
                System.String sErrorMessage;
                DatabaseDefs.TBoardPart BoardPart;
                System.Windows.Forms.ListViewItem Item;

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
                    using (new Common.Forms.WaitCursor(this))
                    {
                        if (Database.EditGameBoardPart(BoardPart.BoardPartLocation.nBoardPartId,
                                                       BoardPartEntry.PartPosition,
                                                       BoardPartEntry.PartLocation,
                                                       BoardPartEntry.PartDescription,
                                                       BoardPartEntry.PartName,
                                                       BoardPartEntry.PartCategoryName,
                                                       BoardPartEntry.PartTypeName,
                                                       BoardPartEntry.PartPackageName,
                                                       out BoardPart.Part.nPartId,
                                                       out sErrorMessage))
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

                            listViewBoardParts.AutosizeColumns();
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
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
