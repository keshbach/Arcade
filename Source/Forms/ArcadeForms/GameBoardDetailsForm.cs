/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class GameBoardDetailsForm : Arcade.Forms.Form
    {
        #region "Member Variables"
        private System.Int32 m_nGameId = -1;
        #endregion

        #region "Constructor"
        public GameBoardDetailsForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Properties"
        public System.Int32 GameId
        {
            set
            {
                m_nGameId = value;
            }
        }
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { listViewBoards };
            }
        }
        #endregion

        #region "Game Board Details Event Handlers"
        private void GameBoardDetailsForm_Shown(object sender, EventArgs e)
        {
            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                InitializeControls();

                RunOnUIThreadWait(() =>
                {
                    this.BusyControlVisible = false;
                });
            }, "Game Board Details Form Initialize Thread");
        }
        #endregion

        #region "List View Event Handlers"
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
        #endregion

        #region "Button Event Handlers"
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            GameBoardEntryForm GameBoardEntry = new GameBoardEntryForm();
            System.Int32 nNewBoardId;
            System.String sErrorMessage;
            DatabaseDefs.TBoard Board;
            System.Windows.Forms.ListViewItem Item;
            System.Boolean bResult;

            new Common.Forms.FormLocation(GameBoardEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            GameBoardEntry.GameBoardEntryFormType = GameBoardEntryForm.EGameBoardEntryFormType.NewBoard;

            if (System.Windows.Forms.DialogResult.OK == GameBoardEntry.ShowDialog(this))
            {
                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.AddGameBoard(m_nGameId, GameBoardEntry.BoardName,
                                                    GameBoardEntry.BoardSize,
                                                    GameBoardEntry.BoardDescription,
                                                    GameBoardEntry.BoardTypeName,
                                                    out nNewBoardId, out sErrorMessage);

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
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
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }

                        this.BusyControlVisible = false;

                        GameBoardEntry.Dispose();
                    });
                }, "Game Board Details Form Add Thread");
            }
            else
            {
                GameBoardEntry.Dispose();
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
            System.Boolean bResult;

            Board = (DatabaseDefs.TBoard)listViewBoards.Items[nIndex].Tag;

            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                bResult = Database.DeleteGameBoard(Board.nBoardId, out sErrorMessage);

                RunOnUIThreadWait(() =>
                {
                    if (bResult)
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

                    this.BusyControlVisible = false;
                });
            }, "Game Board Details Form Delete Thread");
        }

        private void buttonParts_Click(object sender, EventArgs e)
        {
            Arcade.Forms.BoardPartDetailsForm BoardPartDetails = new Arcade.Forms.BoardPartDetailsForm();
            System.Int32 nIndex = listViewBoards.SelectedIndices[0];
            DatabaseDefs.TBoard Board;

            new Common.Forms.FormLocation(BoardPartDetails, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            Board = (DatabaseDefs.TBoard)listViewBoards.Items[nIndex].Tag;

            BoardPartDetails.BoardId = Board.nBoardId;

            BoardPartDetails.ShowDialog(this);

            BoardPartDetails.Dispose();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region "Internal Helpers"
        private void EditBoard()
        {
            GameBoardEntryForm GameBoardEntry = new GameBoardEntryForm();
            System.Int32 nIndex = listViewBoards.SelectedIndices[0];
            System.String sErrorMessage;
            DatabaseDefs.TBoard Board;
            System.Boolean bResult;

            new Common.Forms.FormLocation(GameBoardEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            Board = (DatabaseDefs.TBoard)listViewBoards.Items[nIndex].Tag;

            GameBoardEntry.GameBoardEntryFormType = GameBoardEntryForm.EGameBoardEntryFormType.EditBoard;
            GameBoardEntry.BoardTypeName = Board.sBoardTypeName;
            GameBoardEntry.BoardName = Board.sBoardName;
            GameBoardEntry.BoardSize = Board.sBoardSize;
            GameBoardEntry.BoardDescription = Board.sBoardDescription;

            if (System.Windows.Forms.DialogResult.OK == GameBoardEntry.ShowDialog(this))
            {
                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.EditGameBoard(Board.nBoardId,
                                                     GameBoardEntry.BoardTypeName,
                                                     GameBoardEntry.BoardName,
                                                     GameBoardEntry.BoardSize,
                                                     GameBoardEntry.BoardDescription,
                                                     out sErrorMessage);

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
                        {
                            Board.sBoardTypeName = GameBoardEntry.BoardTypeName;
                            Board.sBoardName = GameBoardEntry.BoardName;
                            Board.sBoardSize = GameBoardEntry.BoardSize;
                            Board.sBoardDescription = GameBoardEntry.BoardDescription;

                            listViewBoards.Items[nIndex].Text = GameBoardEntry.BoardTypeName;
                            listViewBoards.Items[nIndex].SubItems[1].Text = GameBoardEntry.BoardName;
                            listViewBoards.Items[nIndex].Tag = Board;
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }

                        this.BusyControlVisible = false;

                        GameBoardEntry.Dispose();
                    });
                }, "Game Board Details Form Edit Thread");
            }
            else
            {
                GameBoardEntry.Dispose();
            }
        }

        private void InitializeControls()
        {
            System.Collections.Generic.List<DatabaseDefs.TBoard> BoardsList;
            System.String sErrorMessage;
            System.Windows.Forms.ListViewItem Item;
            System.Boolean bResult;

            Common.Debug.Thread.IsWorkerThread();

            bResult = Database.GetBoardsForGame(m_nGameId, out BoardsList,
                                                out sErrorMessage);

            RunOnUIThreadWait(() =>
            {
                if (bResult)
                {
                    listViewBoards.BeginUpdate();

                    foreach (DatabaseDefs.TBoard Board in BoardsList)
                    {
                        Item = listViewBoards.Items.Add(Board.sBoardTypeName);

                        Item.Tag = Board;
                        Item.SubItems.Add(Board.sBoardName);
                    }

                    listViewBoards.EndUpdate();
                }
                else
                {
                    Common.Forms.MessageBox.Show(this, sErrorMessage,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);

                    buttonAdd.Enabled = false;
                }

                if (listViewBoards.Items.Count == 0)
                {
                    listViewBoards.Enabled = false;
                }

                buttonEdit.Enabled = false;
                buttonDelete.Enabled = false;
                buttonParts.Enabled = false;
            });
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
