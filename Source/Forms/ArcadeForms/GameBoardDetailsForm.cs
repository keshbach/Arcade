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
        public partial class GameBoardDetailsForm : System.Windows.Forms.Form
        {
            private System.Int32 m_nGameId = -1;

            public GameBoardDetailsForm()
            {
                InitializeComponent();
            }

            public System.Int32 GameId
            {
                set
                {
                    m_nGameId = value;
                }
            }

            private void GameBoardDetailsForm_Load(object sender, EventArgs e)
            {
                System.Collections.Generic.List<DatabaseDefs.TBoard> BoardsList;
                System.String sErrorMessage;
                System.Windows.Forms.ListViewItem Item;

                using (new Common.Forms.WaitCursor(this))
                {
                    if (Database.GetBoardsForGame(m_nGameId, out BoardsList,
                                                  out sErrorMessage))
                    {
                        listViewBoards.BeginUpdate();

                        foreach (DatabaseDefs.TBoard Board in BoardsList)
                        {
                            Item = listViewBoards.Items.Add(Board.sBoardTypeName);

                            Item.Tag = Board;
                            Item.SubItems.Add(Board.sBoardName);
                        }

                        listViewBoards.AutosizeColumns();
                        listViewBoards.EndUpdate();
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);

                        buttonAdd.Enabled = false;
                    }
                }

                if (listViewBoards.Items.Count == 0)
                {
                    listViewBoards.Enabled = false;
                }

                buttonEdit.Enabled = false;
                buttonDelete.Enabled = false;
                buttonParts.Enabled = false;
            }

            private void listViewBoards_DoubleClick(object sender, EventArgs e)
            {
                EditBoard();
            }

            private void listViewBoards_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
            {
                buttonEdit.Enabled = true;
                buttonDelete.Enabled = true;
                buttonParts.Enabled = true;
            }

            private void buttonAdd_Click(object sender, EventArgs e)
            {
                GameBoardEntryForm GameBoardEntry = new GameBoardEntryForm();
                System.Int32 nNewBoardId;
                System.String sErrorMessage;
                DatabaseDefs.TBoard Board;
                System.Windows.Forms.ListViewItem Item;

                GameBoardEntry.GameBoardEntryFormType = GameBoardEntryForm.EGameBoardEntryFormType.NewBoard;

                if (System.Windows.Forms.DialogResult.OK == GameBoardEntry.ShowDialog(this))
                {
                    using (new Common.Forms.WaitCursor(this))
                    {
                        if (Database.AddGameBoard(m_nGameId, GameBoardEntry.BoardName,
                                                  GameBoardEntry.BoardSize,
                                                  GameBoardEntry.BoardDescription,
                                                  GameBoardEntry.BoardTypeName,
                                                  out nNewBoardId, out sErrorMessage))
                        {
                            Board = new DatabaseDefs.TBoard();

                            Board.nBoardId = nNewBoardId;
                            Board.sBoardName = GameBoardEntry.BoardName;
                            Board.sBoardDescription = GameBoardEntry.BoardDescription;
                            Board.sBoardSize = GameBoardEntry.BoardSize;
                            Board.sBoardTypeName = GameBoardEntry.BoardTypeName;

                            listViewBoards.Enabled = true;

                            Item = listViewBoards.Items.Add(Board.sBoardTypeName);

                            Item.Tag = Board;
                            Item.SubItems.Add(Board.sBoardName);

                            Item.Selected = true;
                            Item.Focused = true;

                            Item.EnsureVisible();

                            listViewBoards.AutosizeColumns();
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
                EditBoard();
            }

            private void buttonDelete_Click(object sender, EventArgs e)
            {
                System.Int32 nIndex = listViewBoards.SelectedIndices[0];
                System.String sErrorMessage;
                DatabaseDefs.TBoard Board;

                Board = (DatabaseDefs.TBoard)listViewBoards.Items[nIndex].Tag;

                using (new Common.Forms.WaitCursor(this))
                {
                    if (Database.DeleteGameBoard(Board.nBoardId, out sErrorMessage))
                    {
                        listViewBoards.Items.RemoveAt(nIndex);

                        if (listViewBoards.Items.Count > 0)
                        {
                            if (nIndex == listViewBoards.Items.Count)
                            {
                                --nIndex;
                            }

                            listViewBoards.Items[nIndex].Selected = true;
                            listViewBoards.Items[nIndex].Focused = true;

                            listViewBoards.Items[nIndex].EnsureVisible();
                        }
                        else
                        {
                            listViewBoards.Enabled = false;

                            buttonEdit.Enabled = false;
                            buttonDelete.Enabled = false;
                            buttonParts.Enabled = false;
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

            private void buttonParts_Click(object sender, EventArgs e)
            {
                Arcade.Forms.BoardPartDetailsForm BoardPartDetails = new Arcade.Forms.BoardPartDetailsForm();
                System.Int32 nIndex = listViewBoards.SelectedIndices[0];
                DatabaseDefs.TBoard Board;

                Board = (DatabaseDefs.TBoard)listViewBoards.Items[nIndex].Tag;

                BoardPartDetails.BoardId = Board.nBoardId;

                BoardPartDetails.ShowDialog(this);
            }

            private void buttonClose_Click(object sender, EventArgs e)
            {
                Close();
            }

            private void EditBoard()
            {
                GameBoardEntryForm GameBoardEntry = new GameBoardEntryForm();
                System.Int32 nIndex = listViewBoards.SelectedIndices[0];
                System.String sErrorMessage;
                DatabaseDefs.TBoard Board;

                Board = (DatabaseDefs.TBoard)listViewBoards.Items[nIndex].Tag;

                GameBoardEntry.GameBoardEntryFormType = GameBoardEntryForm.EGameBoardEntryFormType.EditBoard;
                GameBoardEntry.BoardTypeName = Board.sBoardTypeName;
                GameBoardEntry.BoardName = Board.sBoardName;
                GameBoardEntry.BoardSize = Board.sBoardSize;
                GameBoardEntry.BoardDescription = Board.sBoardDescription;

                if (System.Windows.Forms.DialogResult.OK == GameBoardEntry.ShowDialog(this))
                {
                    using (new Common.Forms.WaitCursor(this))
                    {
                        if (Database.EditGameBoard(Board.nBoardId,
                                                   GameBoardEntry.BoardTypeName,
                                                   GameBoardEntry.BoardName,
                                                   GameBoardEntry.BoardSize,
                                                   GameBoardEntry.BoardDescription,
                                                   out sErrorMessage))
                        {
                            Board.sBoardTypeName = GameBoardEntry.BoardTypeName;
                            Board.sBoardName = GameBoardEntry.BoardName;
                            Board.sBoardSize = GameBoardEntry.BoardSize;
                            Board.sBoardDescription = GameBoardEntry.BoardDescription;

                            listViewBoards.Items[nIndex].Text = GameBoardEntry.BoardTypeName;
                            listViewBoards.Items[nIndex].SubItems[1].Text = GameBoardEntry.BoardName;
                            listViewBoards.Items[nIndex].Tag = Board;

                            listViewBoards.AutosizeColumns();
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
