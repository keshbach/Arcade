/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace Arcade.Forms
{
    /// <summary>
    /// Summary description for ListGamesForm.
    /// </summary>
    public partial class ListGamesForm : Common.Forms.Form
    {
        #region "Constructor"
        public ListGamesForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { listViewGames };
            }
        }
        #endregion

        #region "List Games Event Handlers"
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
        #endregion

        #region "List View Event Handlers"
        private void listViewGames_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            buttonEdit.Enabled = true;
            buttonDelete.Enabled = true;
        }

        private void listViewGames_DoubleClick(object sender, EventArgs e)
        {
            EditGame();
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Arcade.Forms.GameEntryForm GameEntry = new Arcade.Forms.GameEntryForm();
            System.Int32 nNewGameId;
            System.String sErrorMessage;
            DatabaseDefs.TGame Game;
            System.Windows.Forms.ListViewItem Item;

            new Common.Forms.FormLocation(GameEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            GameEntry.GameEntryFormType = Arcade.Forms.GameEntryForm.EGameEntryFormType.NewGame;

            if (System.Windows.Forms.DialogResult.OK == GameEntry.ShowDialog(this))
            {
                using (new Common.Forms.WaitCursor(this))
                {
                    if (Database.AddGame(GameEntry.GameName,
                                         GameEntry.Manufacturer,
                                         GameEntry.GameWiringHarness,
                                         GameEntry.GameHaveWiringHarness,
                                         GameEntry.GameNeedPowerOnReset,
                                         GameEntry.GameCocktail,
                                         GameEntry.GameDescription,
                                         GameEntry.GamePinouts,
                                         GameEntry.GameDipSwitches,
                                         GameEntry.GameAudioColl,
                                         GameEntry.GameVideoColl,
                                         GameEntry.GameControlsColl,
                                         out nNewGameId,
                                         out sErrorMessage))
                    {
                        Game = new DatabaseDefs.TGame();

                        Game.nGameId = nNewGameId;
                        Game.sGameName = GameEntry.GameName;
                        Game.sManufacturer = GameEntry.Manufacturer;
                        Game.sGameWiringHarness = GameEntry.GameWiringHarness;
                        Game.bGameHaveWiringHarness = GameEntry.GameHaveWiringHarness;
                        Game.sGameCocktail = GameEntry.GameCocktail;
                        Game.sGameDescription = GameEntry.GameDescription;
                        Game.sGamePinouts = GameEntry.GamePinouts;
                        Game.sGameDipSwitches = GameEntry.GameDipSwitches;
                        Game.GameAudioColl = GameEntry.GameAudioColl;
                        Game.GameVideoColl = GameEntry.GameVideoColl;
                        Game.GameControlsColl = GameEntry.GameControlsColl;

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
        #endregion

        #region "Internal Helpers"
        private void EditGame()
        {
            System.Int32 nIndex = listViewGames.SelectedIndices[0];
            Arcade.Forms.GameEntryForm GameEntry = new Arcade.Forms.GameEntryForm();
            System.String sErrorMessage;
            DatabaseDefs.TGame Game;

            new Common.Forms.FormLocation(GameEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            Game = (DatabaseDefs.TGame)listViewGames.Items[nIndex].Tag;

            GameEntry.GameEntryFormType = Arcade.Forms.GameEntryForm.EGameEntryFormType.EditGame;
            GameEntry.GameId = Game.nGameId;
            GameEntry.GameName = Game.sGameName;
            GameEntry.Manufacturer = Game.sManufacturer;
            GameEntry.GameWiringHarness = Game.sGameWiringHarness;
            GameEntry.GameHaveWiringHarness = Game.bGameHaveWiringHarness;
            GameEntry.GameNeedPowerOnReset = Game.bGameNeedPowerOnReset;
            GameEntry.GameCocktail = Game.sGameCocktail;
            GameEntry.GameDescription = Game.sGameDescription;
            GameEntry.GamePinouts = Game.sGamePinouts;
            GameEntry.GameDipSwitches = Game.sGameDipSwitches;
            GameEntry.GameAudioColl = Game.GameAudioColl;
            GameEntry.GameVideoColl = Game.GameVideoColl;
            GameEntry.GameControlsColl = Game.GameControlsColl;

            if (System.Windows.Forms.DialogResult.OK == GameEntry.ShowDialog(this))
            {
                using (new Common.Forms.WaitCursor(this))
                {
                    if (Database.EditGame(Game.nGameId,
                                          GameEntry.GameName,
                                          GameEntry.Manufacturer,
                                          GameEntry.GameWiringHarness,
                                          GameEntry.GameHaveWiringHarness,
                                          GameEntry.GameNeedPowerOnReset,
                                          GameEntry.GameCocktail,
                                          GameEntry.GameDescription,
                                          GameEntry.GamePinouts,
                                          GameEntry.GameDipSwitches,
                                          GameEntry.GameAudioColl,
                                          GameEntry.GameVideoColl,
                                          GameEntry.GameControlsColl,
                                          out sErrorMessage))
                    {
                        Game.sGameName = GameEntry.GameName;
                        Game.sManufacturer = GameEntry.Manufacturer;
                        Game.sGameWiringHarness = GameEntry.GameWiringHarness;
                        Game.bGameHaveWiringHarness = GameEntry.GameHaveWiringHarness;
                        Game.bGameNeedPowerOnReset = GameEntry.GameNeedPowerOnReset;
                        Game.sGameCocktail = GameEntry.GameCocktail;
                        Game.sGameDescription = GameEntry.GameDescription;
                        Game.sGamePinouts = GameEntry.GamePinouts;
                        Game.sGameDipSwitches = GameEntry.GameDipSwitches;
                        Game.GameAudioColl = GameEntry.GameAudioColl;
                        Game.GameVideoColl = GameEntry.GameVideoColl;
                        Game.GameControlsColl = GameEntry.GameControlsColl;

                        listViewGames.Items[nIndex].Text = GameEntry.GameName;
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
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
