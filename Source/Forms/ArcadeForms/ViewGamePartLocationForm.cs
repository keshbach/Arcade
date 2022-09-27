/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class ViewGamePartLocationForm : Arcade.Forms.Form
    {
        #region "Member Variables"
        private System.Int32 m_nPartId = -1;
        #endregion

        #region "Constructor"
        public ViewGamePartLocationForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Properties"
        public System.Int32 PartId
        {
            set
            {
                m_nPartId = value;
            }
        }
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { listViewGames, listViewBoard, listViewPositionsAndLocations, checkBoxIncludeAllParts, splitContainerLeft, splitContainerRight };
            }
        }
        #endregion

        #region "View Game Part Location Event Handlers"
        private void ViewGamePartLocationForm_Shown(object sender, EventArgs e)
        {
            RefreshGames();
        }
        #endregion

        #region "List View Event Handlers"
        private void listViewGames_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            DatabaseDefs.TGame Game;

            if (e.IsSelected)
            {
                Game = (DatabaseDefs.TGame)listViewGames.Items[e.ItemIndex].Tag;

                RefreshBoards(Game.nGameId);
            }
        }

        private void listViewBoard_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            DatabaseDefs.TBoard Board;

            if (e.IsSelected)
            {
                Board = (DatabaseDefs.TBoard)listViewBoard.Items[e.ItemIndex].Tag;

                RefreshPosAndLoc(Board.nBoardId);
            }
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }
        #endregion

        #region "Check Box Event Handlers"
        private void checkBoxIncludeAllParts_CheckedChanged(object sender, EventArgs e)
        {
            RefreshGames();
        }
        #endregion

        #region "Internal Helpers"
        private void RefreshGames()
        {
            System.Boolean bIncludeAllParts = (checkBoxIncludeAllParts.CheckState == CheckState.Checked);
            System.Collections.Generic.List<DatabaseDefs.TGame> GamesList;
            System.Boolean bResult;
            System.String sErrorMessage;
            System.Windows.Forms.ListViewItem Item;

            Common.Debug.Thread.IsUIThread();

            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                if (bIncludeAllParts)
                {
                    bResult = Database.GetGamesWithPartIncludeAllMatchingParts(
                                  m_nPartId, out GamesList,
                                  out sErrorMessage);
                }
                else
                {
                    bResult = Database.GetGamesWithPart(m_nPartId,
                                                        out GamesList,
                                                        out sErrorMessage);
                }

                RunOnUIThreadWait(() =>
                {
                    if (!bResult)
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }

                    listViewGames.Items.Clear();
                    listViewBoard.Items.Clear();
                    listViewPositionsAndLocations.Items.Clear();

                    listViewGames.Enabled = false;
                    listViewBoard.Enabled = false;
                    listViewPositionsAndLocations.Enabled = false;

                    listViewGames.BeginUpdate();

                    foreach (DatabaseDefs.TGame Game in GamesList)
                    {
                        Item = listViewGames.Items.Add(Game.sGameName);

                        Item.Tag = Game;
                    }

                    listViewGames.EndUpdate();

                    if (listViewGames.Items.Count > 0)
                    {
                        listViewGames.Enabled = true;
                    }

                    this.BusyControlVisible = false;
                });
            }, "View Game Part Location Form Refresh Games Thread");
        }

        private void RefreshBoards(
            System.Int32 nGameId)
        {
            System.Boolean bIncludeAllParts = (checkBoxIncludeAllParts.CheckState == CheckState.Unchecked);
            System.Collections.Generic.List<DatabaseDefs.TBoard> BoardsList;
            System.Boolean bResult;
            System.String sErrorMessage;
            System.Windows.Forms.ListViewItem Item;

            Common.Debug.Thread.IsUIThread();

            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                if (bIncludeAllParts)
                {
                    bResult = Database.GetBoardsWithPartIncludeAllMatchingParts(
                                           nGameId, m_nPartId,
                                           out BoardsList, out sErrorMessage);
                }
                else
                {
                    bResult = Database.GetBoardsWithPart(nGameId,
                                                         m_nPartId,
                                                         out BoardsList,
                                                         out sErrorMessage);
                }

                RunOnUIThreadWait(() =>
                {
                    listViewBoard.Items.Clear();
                    listViewPositionsAndLocations.Items.Clear();

                    listViewPositionsAndLocations.Enabled = false;

                    if (bResult)
                    {
                        listViewBoard.BeginUpdate();

                        listViewBoard.Enabled = true;

                        foreach (DatabaseDefs.TBoard Board in BoardsList)
                        {
                            Item = new System.Windows.Forms.ListViewItem();

                            Item.Tag = Board;
                            Item.Text = (Board.sBoardTypeName != DatabaseDefs.CCartridgeName) ? "" : "*";
                            Item.SubItems.Add(Board.sBoardTypeName);
                            Item.SubItems.Add(Board.sBoardName);

                            listViewBoard.Items.Add(Item);
                        }

                        listViewBoard.EndUpdate();
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                                                     System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Information);
                    }

                    this.BusyControlVisible = false;
                });
            }, "View Game Part Location Form Refresh Boards Thread");
        }

        private void RefreshPosAndLoc(
            System.Int32 nBoardId)
        {
            System.Boolean bIncludeAllParts = (checkBoxIncludeAllParts.CheckState == CheckState.Unchecked);
            System.Collections.Generic.List<DatabaseDefs.TBoardPartLocation> PosAndLocList;
            System.Boolean bResult;
            System.String sErrorMessage;
            System.Windows.Forms.ListViewItem Item;

            Common.Debug.Thread.IsUIThread();

            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                if (bIncludeAllParts)
                {
                    bResult = Database.GetBoardPartLocationsIncludeAllMatchingParts(
                                           nBoardId, m_nPartId,
                                           out PosAndLocList,
                                           out sErrorMessage);
                }
                else
                {
                    bResult = Database.GetBoardPartLocations(
                                           nBoardId, m_nPartId,
                                           out PosAndLocList,
                                           out sErrorMessage);
                }

                RunOnUIThreadWait(() =>
                {
                    listViewPositionsAndLocations.Items.Clear();

                    if (bResult)
                    {
                        listViewPositionsAndLocations.BeginUpdate();

                        listViewPositionsAndLocations.Enabled = true;

                        foreach (DatabaseDefs.TBoardPartLocation Location in PosAndLocList)
                        {
                            Item = listViewPositionsAndLocations.Items.Add(Location.sBoardPartPosition);

                            Item.SubItems.Add(Location.sBoardPartLocation);

                            Item.Tag = Location;
                        }

                        listViewPositionsAndLocations.EndUpdate();
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                                                     System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Information);
                    }

                    this.BusyControlVisible = false;
                });
            }, "View Game Part Location Form Refresh Position and Location Thread");
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
