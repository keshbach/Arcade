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
        /// Summary description for ViewGamePartLocationForm.
        /// </summary>
        public partial class ViewGamePartLocationForm : System.Windows.Forms.Form
        {
            private System.Int32 m_nPartId = -1;

            public ViewGamePartLocationForm()
            {
                //
                // Required for Windows Form Designer support
                //
                InitializeComponent();
            }

            public System.Int32 PartId
            {
                set
                {
                    m_nPartId = value;
                }
            }

            private void ViewGamePartLocationForm_Load(object sender, System.EventArgs e)
            {
                RefreshGames();
            }

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

            private void buttonClose_Click(object sender, System.EventArgs e)
            {
                Close();
            }

            private void checkBoxIncludeAllParts_CheckedChanged(object sender, EventArgs e)
            {
                RefreshGames();
            }

            private System.Boolean RefreshGames()
            {
                System.Collections.Generic.List<DatabaseDefs.TGame> GamesList;
                System.String sErrorMessage;
                System.Windows.Forms.ListViewItem Item;

                using (new Common.Forms.WaitCursor(this))
                {
                    listViewGames.Items.Clear();
                    listViewBoard.Items.Clear();
                    listViewPositionsAndLocations.Items.Clear();

                    listViewGames.Enabled = false;
                    listViewBoard.Enabled = false;
                    listViewPositionsAndLocations.Enabled = false;

                    if (checkBoxIncludeAllParts.CheckState == CheckState.Unchecked)
                    {
                        if (false == Database.GetGamesWithPart(m_nPartId,
                                                               out GamesList,
                                                               out sErrorMessage))
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        if (false == Database.GetGamesWithPartIncludeAllMatchingParts(
                                         m_nPartId, out GamesList,
                                         out sErrorMessage))
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }

                    listViewGames.BeginUpdate();

                    foreach (DatabaseDefs.TGame Game in GamesList)
                    {
                        Item = listViewGames.Items.Add(Game.sGameName);

                        Item.Tag = Game;
                    }

                    listViewGames.AutosizeColumns();
                    listViewGames.EndUpdate();
                }

                if (listViewGames.Items.Count > 0)
                {
                    listViewGames.Enabled = true;
                }

                return true;
            }

            private System.Boolean RefreshBoards(
                System.Int32 nGameId)
            {
                System.Collections.Generic.List<DatabaseDefs.TBoard> BoardsList;
                System.String sErrorMessage;
                System.Windows.Forms.ListViewItem Item;

                using (new Common.Forms.WaitCursor(this))
                {
                    listViewBoard.Items.Clear();
                    listViewPositionsAndLocations.Items.Clear();

                    listViewPositionsAndLocations.Enabled = false;

                    if (checkBoxIncludeAllParts.CheckState == CheckState.Unchecked)
                    {
                        if (false == Database.GetBoardsWithPart(nGameId,
                                                                m_nPartId,
                                                                out BoardsList,
                                                                out sErrorMessage))
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);

                            return false;
                        }
                    }
                    else
                    {
                        if (false == Database.GetBoardsWithPartIncludeAllMatchingParts(
                                         nGameId, m_nPartId,
                                         out BoardsList, out sErrorMessage))
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);

                            return false;
                        }
                    }

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

                    listViewBoard.AutosizeColumns();
                    listViewBoard.EndUpdate();
                }

                return true;
            }

            private System.Boolean RefreshPosAndLoc(
                System.Int32 nBoardId)
            {
                System.Collections.Generic.List<DatabaseDefs.TBoardPartLocation> PosAndLocList;
                System.String sErrorMessage;
                System.Windows.Forms.ListViewItem Item;

                using (new Common.Forms.WaitCursor(this))
                {
                    listViewPositionsAndLocations.Items.Clear();

                    if (checkBoxIncludeAllParts.CheckState == CheckState.Unchecked)
                    {
                        if (false == Database.GetBoardPartLocations(
                                         nBoardId, m_nPartId,
                                         out PosAndLocList,
                                         out sErrorMessage))
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);

                            return false;
                        }
                    }
                    else
                    {
                        if (false == Database.GetBoardPartLocationsIncludeAllMatchingParts(
                                         nBoardId, m_nPartId,
                                         out PosAndLocList,
                                         out sErrorMessage))
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);

                            return false;
                        }
                    }

                    listViewPositionsAndLocations.BeginUpdate();

                    listViewPositionsAndLocations.Enabled = true;

                    foreach (DatabaseDefs.TBoardPartLocation Location in PosAndLocList)
                    {
                        Item = listViewPositionsAndLocations.Items.Add(Location.sBoardPartPosition);

                        Item.SubItems.Add(Location.sBoardPartLocation);

                        Item.Tag = Location;
                    }

                    listViewPositionsAndLocations.AutosizeColumns();
                    listViewPositionsAndLocations.EndUpdate();
                }

                return true;
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
