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
        /// Summary description for ListGamesForm.
        /// </summary>
        public partial class ListGamesForm : System.Windows.Forms.Form
        {
            public ListGamesForm()
            {
                InitializeComponent();
            }

            private void ListGamesForm_Load(object sender, EventArgs e)
            {
                System.Collections.Generic.List<DatabaseDefs.TGame> GamesList;
                System.String sErrorMessage;
                System.Windows.Forms.ListViewItem Item;

                buttonEdit.Enabled = false;
                buttonDelete.Enabled = false;

                using (new Common.Forms.WaitCursor(this))
                {
                    if (Database.GetGames(out GamesList, out sErrorMessage))
                    {
                        listViewGames.BeginUpdate();

                        foreach (DatabaseDefs.TGame Game in GamesList)
                        {
                            Item = listViewGames.Items.Add(Game.sGameName);

                            Item.Tag = Game;
                        }

                        listViewGames.AutosizeColumns();
                        listViewGames.EndUpdate();
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }
                }

                if (listViewGames.Items.Count == 0)
                {
                    listViewGames.Enabled = false;
                }
            }

            private void listViewGames_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
            {
                buttonEdit.Enabled = true;
                buttonDelete.Enabled = true;
            }

            private void listViewGames_DoubleClick(object sender, EventArgs e)
            {
                EditGame();
            }

            private void buttonAdd_Click(object sender, EventArgs e)
            {
                Arcade.Forms.GameEntryForm GameForm = new Arcade.Forms.GameEntryForm();
                System.Int32 nNewGameId;
                System.String sErrorMessage;
                DatabaseDefs.TGame Game;
                System.Windows.Forms.ListViewItem Item;

                GameForm.GameEntryFormType = Arcade.Forms.GameEntryForm.EGameEntryFormType.NewGame;

                if (System.Windows.Forms.DialogResult.OK == GameForm.ShowDialog(this))
                {
                    using (new Common.Forms.WaitCursor(this))
                    {
                        if (Database.AddGame(GameForm.GameName,
                                             GameForm.Manufacturer,
                                             GameForm.GameWiringHarness,
                                             GameForm.GameHaveWiringHarness,
                                             GameForm.GameNeedPowerOnReset,
                                             GameForm.GameCocktail,
                                             GameForm.GameDescription,
                                             GameForm.GamePinouts,
                                             GameForm.GameDipSwitches,
                                             GameForm.GameAudioColl,
                                             GameForm.GameVideoColl,
                                             GameForm.GameControlsColl,
                                             out nNewGameId,
                                             out sErrorMessage))
                        {
                            Game = new DatabaseDefs.TGame();

                            Game.nGameId = nNewGameId;
                            Game.sGameName = GameForm.GameName;
                            Game.sManufacturer = GameForm.Manufacturer;
                            Game.sGameWiringHarness = GameForm.GameWiringHarness;
                            Game.bGameHaveWiringHarness = GameForm.GameHaveWiringHarness;
                            Game.sGameCocktail = GameForm.GameCocktail;
                            Game.sGameDescription = GameForm.GameDescription;
                            Game.sGamePinouts = GameForm.GamePinouts;
                            Game.sGameDipSwitches = GameForm.GameDipSwitches;
                            Game.GameAudioColl = GameForm.GameAudioColl;
                            Game.GameVideoColl = GameForm.GameVideoColl;
                            Game.GameControlsColl = GameForm.GameControlsColl;

                            listViewGames.Enabled = true;

                            Item = listViewGames.Items.Add(Game.sGameName);

                            Item.Tag = Game;
                            Item.Selected = true;
                            Item.Focused = true;

                            Item.EnsureVisible();

                            listViewGames.AutosizeColumns();
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
                EditGame();
            }

            private void buttonDelete_Click(object sender, EventArgs e)
            {
                System.Int32 nIndex = listViewGames.SelectedIndices[0];
                System.String sErrorMessage;
                DatabaseDefs.TGame Game;

                Game = (DatabaseDefs.TGame)listViewGames.Items[nIndex].Tag;

                using (new Common.Forms.WaitCursor(this))
                {
                    if (Database.DeleteGame(Game.nGameId, out sErrorMessage))
                    {
                        listViewGames.Items.RemoveAt(nIndex);

                        if (listViewGames.Items.Count > 0)
                        {
                            if (listViewGames.Items.Count == nIndex)
                            {
                                --nIndex;
                            }

                            listViewGames.Items[nIndex].Selected = true;
                            listViewGames.Items[nIndex].Focused = true;

                            listViewGames.Items[nIndex].EnsureVisible();
                        }
                        else
                        {
                            listViewGames.Enabled = false;

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

            private void buttonClose_Click(object sender, EventArgs e)
            {
                Close();
            }

            private void EditGame()
            {
                System.Int32 nIndex = listViewGames.SelectedIndices[0];
                Arcade.Forms.GameEntryForm GameForm = new Arcade.Forms.GameEntryForm();
                System.String sErrorMessage;
                DatabaseDefs.TGame Game;

                Game = (DatabaseDefs.TGame)listViewGames.Items[nIndex].Tag;

                GameForm.GameEntryFormType = Arcade.Forms.GameEntryForm.EGameEntryFormType.EditGame;
                GameForm.GameId = Game.nGameId;
                GameForm.GameName = Game.sGameName;
                GameForm.Manufacturer = Game.sManufacturer;
                GameForm.GameWiringHarness = Game.sGameWiringHarness;
                GameForm.GameHaveWiringHarness = Game.bGameHaveWiringHarness;
                GameForm.GameNeedPowerOnReset = Game.bGameNeedPowerOnReset;
                GameForm.GameCocktail = Game.sGameCocktail;
                GameForm.GameDescription = Game.sGameDescription;
                GameForm.GamePinouts = Game.sGamePinouts;
                GameForm.GameDipSwitches = Game.sGameDipSwitches;
                GameForm.GameAudioColl = Game.GameAudioColl;
                GameForm.GameVideoColl = Game.GameVideoColl;
                GameForm.GameControlsColl = Game.GameControlsColl;

                if (System.Windows.Forms.DialogResult.OK == GameForm.ShowDialog(this))
                {
                    using (new Common.Forms.WaitCursor(this))
                    {
                        if (Database.EditGame(Game.nGameId,
                                              GameForm.GameName,
                                              GameForm.Manufacturer,
                                              GameForm.GameWiringHarness,
                                              GameForm.GameHaveWiringHarness,
                                              GameForm.GameNeedPowerOnReset,
                                              GameForm.GameCocktail,
                                              GameForm.GameDescription,
                                              GameForm.GamePinouts,
                                              GameForm.GameDipSwitches,
                                              GameForm.GameAudioColl,
                                              GameForm.GameVideoColl,
                                              GameForm.GameControlsColl,
                                              out sErrorMessage))
                        {
                            Game.sGameName = GameForm.GameName;
                            Game.sManufacturer = GameForm.Manufacturer;
                            Game.sGameWiringHarness = GameForm.GameWiringHarness;
                            Game.bGameHaveWiringHarness = GameForm.GameHaveWiringHarness;
                            Game.bGameNeedPowerOnReset = GameForm.GameNeedPowerOnReset;
                            Game.sGameCocktail = GameForm.GameCocktail;
                            Game.sGameDescription = GameForm.GameDescription;
                            Game.sGamePinouts = GameForm.GamePinouts;
                            Game.sGameDipSwitches = GameForm.GameDipSwitches;
                            Game.GameAudioColl = GameForm.GameAudioColl;
                            Game.GameVideoColl = GameForm.GameVideoColl;
                            Game.GameControlsColl = GameForm.GameControlsColl;

                            listViewGames.Items[nIndex].Text = GameForm.GameName;
                            listViewGames.Items[nIndex].Tag = Game;

                            listViewGames.AutosizeColumns();
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
