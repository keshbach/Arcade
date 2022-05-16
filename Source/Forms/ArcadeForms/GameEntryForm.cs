/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class GameEntryForm : System.Windows.Forms.Form
    {
        #region "Enumerations"
        public enum EGameEntryFormType
        {
            NewGame,
            EditGame
        };
        #endregion

        #region "Constants"
        private static System.String CJAMMAWiringHarness = "JAMMA";
        #endregion

        #region "Member Variables"
        private EGameEntryFormType m_GameEntryFormType = EGameEntryFormType.NewGame;
        private System.Int32 m_nGameId = -1;
        private System.String m_sGameName = "";
        private System.String m_sManufacturer = "";
        private System.String m_sGameWiringHarness = "";
        private System.Boolean m_bGameHaveWiringHarness = false;
        private System.Boolean m_bGameNeedPowerOnReset = false;
        private System.String m_sGameCocktail = "";
        private System.String m_sGameDescription = "";
        private System.String m_sGamePinouts = "";
        private System.String m_sGameDipSwitches = "";
        private Common.Collections.StringCollection m_GameAudioColl = new Common.Collections.StringCollection();
        private Common.Collections.StringCollection m_GameControlsColl = new Common.Collections.StringCollection();
        private Common.Collections.StringCollection m_GameVideoColl = new Common.Collections.StringCollection();
        private Common.Collections.StringCollection m_GameDisplaysColl = new Common.Collections.StringCollection();

        private Common.Collections.StringSortedList<System.Int32> m_ManufacturerList = null;
        private Common.Collections.StringSortedList<System.Int32> m_GameWiringHarnessList = null;
        private Common.Collections.StringSortedList<System.Int32> m_GameCocktailList = null;
        #endregion

        #region "Constructor"
        public GameEntryForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Properties"
        public EGameEntryFormType GameEntryFormType
        {
            set
            {
                m_GameEntryFormType = value;
            }
        }

        public System.Int32 GameId
        {
            set
            {
                m_nGameId = value;
            }
        }

        public System.String GameName
        {
            get
            {
                return m_sGameName;
            }
            set
            {
                m_sGameName = value;
            }
        }

        public System.String Manufacturer
        {
            get
            {
                return m_sManufacturer;
            }
            set
            {
                m_sManufacturer = value;
            }
        }

        public Common.Collections.StringCollection GameAudioColl
        {
            get
            {
                return m_GameAudioColl;
            }
            set
            {
                m_GameAudioColl = value;
            }
        }

        public Common.Collections.StringCollection GameVideoColl
        {
            get
            {
                return m_GameVideoColl;
            }
            set
            {
                m_GameVideoColl = value;
            }
        }

        public Common.Collections.StringCollection GameControlsColl
        {
            get
            {
                return m_GameControlsColl;
            }
            set
            {
                m_GameControlsColl = value;
            }
        }

        public System.String GameWiringHarness
        {
            get
            {
                return m_sGameWiringHarness;
            }
            set
            {
                m_sGameWiringHarness = value;
            }
        }

        public System.Boolean GameHaveWiringHarness
        {
            get
            {
                return m_bGameHaveWiringHarness;
            }
            set
            {
                m_bGameHaveWiringHarness = value;
            }
        }

        public System.Boolean GameNeedPowerOnReset
        {
            get
            {
                return m_bGameNeedPowerOnReset;
            }
            set
            {
                m_bGameNeedPowerOnReset = value;
            }
        }

        public System.String GameCocktail
        {
            get
            {
                return m_sGameCocktail;
            }
            set
            {
                m_sGameCocktail = value;
            }
        }

        public System.String GameDescription
        {
            get
            {
                return m_sGameDescription;
            }
            set
            {
                m_sGameDescription = value;
            }
        }

        public System.String GamePinouts
        {
            get
            {
                return m_sGamePinouts;
            }
            set
            {
                m_sGamePinouts = value;
            }
        }

        public System.String GameDipSwitches
        {
            get
            {
                return m_sGameDipSwitches;
            }
            set
            {
                m_sGameDipSwitches = value;
            }
        }
        #endregion

        #region "Game Entry Event Handlers"
        private void GameEntryForm_Load(object sender, EventArgs e)
        {
            System.Collections.Generic.List<DatabaseDefs.TDisplay> DisplayList = new System.Collections.Generic.List<DatabaseDefs.TDisplay>();
            System.Collections.SortedList GamePropertyList = new System.Collections.SortedList();
            DatabaseDefs.TGameLens GameLens;
            System.String sErrorMessage;

            using (new Common.Forms.WaitCursor(this))
            {
                if (Database.GetGameMaxLens(out GameLens))
                {
                    textBoxName.MaxLength = GameLens.nGameNameLen;
                    textBoxDescription.MaxLength = GameLens.nGameDescriptionLen;
                    textBoxPinouts.MaxLength = GameLens.nGamePinoutsLen;
                    textBoxDipSwitches.MaxLength = GameLens.nGameDipSwitchesLen;
                }

                Database.GetManufacturerList(out m_ManufacturerList);
                Database.GetGameCategoryList(DatabaseDefs.EGameDataType.WiringHarness,
                                                out m_GameWiringHarnessList);
                Database.GetGameCategoryList(DatabaseDefs.EGameDataType.Cocktail,
                                                out m_GameCocktailList);

                comboBoxManufacturer.BeginUpdate();

                foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_ManufacturerList)
                {
                    comboBoxManufacturer.Items.Add(Pair.Key);
                }

                comboBoxManufacturer.EndUpdate();

                comboBoxWiringHarness.BeginUpdate();

                foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_GameWiringHarnessList)
                {
                    if (Pair.Key != CJAMMAWiringHarness)
                    {
                        comboBoxWiringHarness.Items.Add(Pair.Key);
                    }
                }

                comboBoxWiringHarness.EndUpdate();

                comboBoxCocktail.BeginUpdate();

                foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_GameCocktailList)
                {
                    comboBoxCocktail.Items.Add(Pair.Key);
                }

                comboBoxCocktail.EndUpdate();

                textBoxName.Text = m_sGameName;
                textBoxDescription.Text = m_sGameDescription;
                textBoxPinouts.Text = m_sGamePinouts;
                textBoxDipSwitches.Text = m_sGameDipSwitches;

                if (m_sGameWiringHarness == CJAMMAWiringHarness)
                {
                    checkBoxNotJAMMA.Checked = false;
                    checkBoxHaveWiringHarness.Enabled = false;
                    checkBoxNeedPowerOnReset.Enabled = false;
                    comboBoxWiringHarness.Enabled = false;
                }
                else
                {
                    checkBoxNotJAMMA.Checked = true;
                }

                checkBoxHaveWiringHarness.Checked = m_bGameHaveWiringHarness;
                checkBoxNeedPowerOnReset.Checked = m_bGameNeedPowerOnReset;

                if (m_GameEntryFormType == EGameEntryFormType.NewGame)
                {
                    Text = "Add...";

                    buttonBoards.Enabled = false;
                    buttonManuals.Enabled = false;
                    buttonControls.Enabled = false;
                    buttonVideo.Enabled = false;
                    buttonAudio.Enabled = false;
                    buttonDisplays.Enabled = false;
                    buttonLogs.Enabled = false;
                    buttonOK.Enabled = false;
                }
                else
                {
                    Text = "Edit...";

                    comboBoxManufacturer.SelectedIndex = m_ManufacturerList.IndexOfKey(m_sManufacturer);
                    comboBoxCocktail.SelectedIndex = m_GameCocktailList.IndexOfKey(m_sGameCocktail);

                    if (m_sGameWiringHarness != CJAMMAWiringHarness)
                    {
                        comboBoxWiringHarness.SelectedItem = m_sGameWiringHarness;
                    }

                    buttonOK.Enabled = true;

                    if (Database.GetDisplaysForGame(m_nGameId, out DisplayList,
                                                    out sErrorMessage))
                    {
                        foreach (DatabaseDefs.TDisplay Display in DisplayList)
                        {
                            m_GameDisplaysColl.Add(Display.sDisplayName);
                        }
                    }
                }
            }
        }
        #endregion

        #region "Text Box Event Handlers"
        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (textBoxName.Text.Length > 0)
            {
                ValidateFields();
            }
            else
            {
                buttonCancel.Enabled = false;
            }
        }
        #endregion

        #region "Combo Box Event Handlers"
        private void comboBoxManufacturer_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxManufacturer.SelectedIndex != -1)
            {
                ValidateFields();
            }
            else
            {
                e.Cancel = true;

                buttonOK.Enabled = false;
            }
        }

        private void comboBoxScreenOrientation_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxWiringHarness.SelectedIndex != -1)
            {
                ValidateFields();
            }
            else
            {
                e.Cancel = true;

                buttonOK.Enabled = false;
            }
        }

        private void comboBoxWiringHarness_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxWiringHarness.SelectedIndex != -1)
            {
                ValidateFields();
            }
            else
            {
                e.Cancel = true;

                buttonOK.Enabled = false;
            }
        }

        private void comboBoxCocktail_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxCocktail.SelectedIndex != -1)
            {
                ValidateFields();
            }
            else
            {
                e.Cancel = true;

                buttonOK.Enabled = false;
            }
        }
        #endregion

        #region "Check Box Event Handlers"
        private void checkBoxNotJAMMA_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxNotJAMMA.Checked == true)
            {
                comboBoxWiringHarness.Enabled = true;
                checkBoxHaveWiringHarness.Enabled = true;
                checkBoxNeedPowerOnReset.Enabled = true;
            }
            else
            {
                comboBoxWiringHarness.Enabled = false;
                checkBoxHaveWiringHarness.Enabled = false;
                checkBoxNeedPowerOnReset.Enabled = false;
            }

            ValidateFields();
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonBoards_Click(object sender, EventArgs e)
        {
            GameBoardDetailsForm GameBoardDetails = new GameBoardDetailsForm();

            new Common.Forms.FormLocation(GameBoardDetails, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            GameBoardDetails.GameId = m_nGameId;

            GameBoardDetails.ShowDialog(this);
        }

        private void buttonManuals_Click(object sender, EventArgs e)
        {
            ListGameManualsForm ListGameManuals = new ListGameManualsForm();

            new Common.Forms.FormLocation(ListGameManuals, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            ListGameManuals.GameId = m_nGameId;

            ListGameManuals.ShowDialog(this);
        }

        private void buttonControls_Click(object sender, EventArgs e)
        {
            EditGameDataColl(ListGameDataForm.EListGameDataFormType.GameControls);
        }

        private void buttonVideo_Click(object sender, EventArgs e)
        {
            EditGameDataColl(ListGameDataForm.EListGameDataFormType.GameVideo);
        }

        private void buttonAudio_Click(object sender, EventArgs e)
        {
            EditGameDataColl(ListGameDataForm.EListGameDataFormType.GameAudio);
        }

        private void buttonDisplays_Click(object sender, EventArgs e)
        {
            EditGameDataColl(ListGameDataForm.EListGameDataFormType.GameDisplay);
        }

        private void buttonLogs_Click(object sender, EventArgs e)
        {
            ListLogsForm ListLogs = new ListLogsForm();

            new Common.Forms.FormLocation(ListLogs, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            ListLogs.GameId = m_nGameId;

            ListLogs.ShowDialog();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_sGameName = textBoxName.Text;
            m_sManufacturer = (System.String)comboBoxManufacturer.SelectedItem;
            m_sGameCocktail = (System.String)comboBoxCocktail.SelectedItem;
            m_sGameDescription = textBoxDescription.Text;
            m_sGamePinouts = textBoxPinouts.Text;
            m_sGameDipSwitches = textBoxDipSwitches.Text;
            m_bGameHaveWiringHarness = checkBoxHaveWiringHarness.Checked;
            m_bGameNeedPowerOnReset = checkBoxNeedPowerOnReset.Checked;

            if (checkBoxNotJAMMA.Checked == false)
            {
                m_sGameWiringHarness = CJAMMAWiringHarness;
            }
            else
            {
                m_sGameWiringHarness = (System.String)comboBoxWiringHarness.SelectedItem;
            }

            DialogResult = DialogResult.OK;

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }
        #endregion

        #region "Internal Helpers"
        private void ValidateFields()
        {
            System.Boolean bWiringHarnessValid = false;

            if (checkBoxNotJAMMA.Checked == true &&
                comboBoxWiringHarness.SelectedIndex != -1)
            {
                bWiringHarnessValid = true;
            }
            else if (checkBoxNotJAMMA.Checked == false)
            {
                bWiringHarnessValid = true;
            }

            if (textBoxName.Text.Length > 0 &&
                comboBoxManufacturer.SelectedIndex != -1 &&
                comboBoxCocktail.SelectedIndex != -1 &&
                bWiringHarnessValid == true)
            {
                buttonOK.Enabled = true;
            }
            else
            {
                buttonOK.Enabled = false;
            }
        }

        private void EditGameDataColl(
            ListGameDataForm.EListGameDataFormType ListGameDataFormType)
        {
            ListGameDataForm ListGameData = new ListGameDataForm();
            System.String sErrorMessage;

            new Common.Forms.FormLocation(ListGameData, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            ListGameData.ListGameDataFormType = ListGameDataFormType;

            switch (ListGameDataFormType)
            {
                case ListGameDataForm.EListGameDataFormType.GameControls:
                    ListGameData.GamePropertiesColl = m_GameControlsColl;
                    ListGameData.AllowDuplicates = Database.GetGamePropertyDupsAllowed(
                                                        DatabaseDefs.EGameDataType.ControlProperty);
                    break;
                case ListGameDataForm.EListGameDataFormType.GameAudio:
                    ListGameData.GamePropertiesColl = m_GameAudioColl;
                    ListGameData.AllowDuplicates = Database.GetGamePropertyDupsAllowed(
                                                        DatabaseDefs.EGameDataType.AudioProperty);
                    break;
                case ListGameDataForm.EListGameDataFormType.GameVideo:
                    ListGameData.GamePropertiesColl = m_GameVideoColl;
                    ListGameData.AllowDuplicates = Database.GetGamePropertyDupsAllowed(
                                                        DatabaseDefs.EGameDataType.VideoProperty);
                    break;
                case ListGameDataForm.EListGameDataFormType.GameDisplay:
                    ListGameData.GamePropertiesColl = m_GameDisplaysColl;
                    ListGameData.AllowDuplicates = Database.GetGameDisplayDupsAllowed();
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
            }

            if (ListGameData.ShowDialog(this) == DialogResult.OK)
            {
                switch (ListGameDataFormType)
                {
                    case ListGameDataForm.EListGameDataFormType.GameControls:
                        m_GameControlsColl = ListGameData.GamePropertiesColl;
                        break;
                    case ListGameDataForm.EListGameDataFormType.GameAudio:
                        m_GameAudioColl = ListGameData.GamePropertiesColl;
                        break;
                    case ListGameDataForm.EListGameDataFormType.GameVideo:
                        m_GameVideoColl = ListGameData.GamePropertiesColl;
                        break;
                    case ListGameDataForm.EListGameDataFormType.GameDisplay:
                        m_GameDisplaysColl = ListGameData.GamePropertiesColl;
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                }

                using (new Common.Forms.WaitCursor(this))
                {
                    if (ListGameDataFormType != ListGameDataForm.EListGameDataFormType.GameDisplay)
                    {
                        if (false == Database.EditGame(m_nGameId,
                                                        m_sGameName,
                                                        m_sManufacturer,
                                                        m_sGameWiringHarness,
                                                        m_bGameHaveWiringHarness,
                                                        m_bGameNeedPowerOnReset,
                                                        m_sGameCocktail,
                                                        m_sGameDescription,
                                                        m_sGamePinouts,
                                                        m_sGameDipSwitches,
                                                        m_GameAudioColl,
                                                        m_GameVideoColl,
                                                        m_GameControlsColl,
                                                        out sErrorMessage))
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        if (false == Database.EditGameDisplays(m_nGameId,
                                                                m_GameDisplaysColl,
                                                                out sErrorMessage))
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }
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
