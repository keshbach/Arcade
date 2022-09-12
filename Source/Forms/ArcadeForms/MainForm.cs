/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;

namespace Arcade.Forms
{
    public partial class MainForm : Common.Forms.MainForm
    {
        #region "Constants"
        private const string CEndOfLine = "\r\n";
        #endregion

        #region "Enumerations"

        public enum DatabaseMode
        {
            Access,
            SQLServer
        }

        private enum State
        {
            Initialize,
            Initializing,
            InitializatedSuccessDatabaseAvailable,
            InitializatedSuccessDatabaseNotAvailable,
            InitializatedFailed,
            Running,
            Uninitializing,
            UninitializatedSuccess,
            UninitializatedFailed
        }
        #endregion

        #region "Member Variables"
        private System.String m_sRegistryKey;
        private System.String m_sFormLocationsRegistryKey;
        private System.String m_sDatabaseRegistryKey;

        private DatabaseMode m_DatabaseMode;

        private State m_State = State.Initialize;

        private bool m_bAllowClose = true;

        private static System.Boolean s_bInitializeImages = true;
        #endregion

        #region "Constructor"
        private MainForm()
        {
        }

        public MainForm(
            System.String sRegistryKey,
            System.String sFormLocationsRegistryKey,
            System.String sDatabaseRegistryKey,
            DatabaseMode DatabaseMode)
        {
            m_sRegistryKey = sRegistryKey;
            m_sFormLocationsRegistryKey = sFormLocationsRegistryKey;
            m_sDatabaseRegistryKey = sDatabaseRegistryKey;
            m_DatabaseMode = DatabaseMode;

            InitializeComponent();
        }
        #endregion

        #region "Properties"
        public System.String FormLocationsRegistryKey
        {
            get
            {
                return m_sFormLocationsRegistryKey;
            }
        }
        #endregion

        #region "MainForm Event Handlers"
        private void MainForm_Load(object sender, EventArgs e)
        {
            switch (m_DatabaseMode)
            {
                case DatabaseMode.Access:
                    toolStripDatabaseModeStatusLabel.Text = "Access";
                    break;
                case DatabaseMode.SQLServer:
                    toolStripDatabaseModeStatusLabel.Text = "SQL Server";
                    break;
            }

            InitImageLists();
            InitImageKeys();

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                InitDatabase();
            }, "Main Form Initialize Database Thread");
        }

        private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            System.String sErrorMessage;

            e.Cancel = !m_bAllowClose;

            if (!m_bAllowClose)
            {
                return;
            }

            if (m_State == State.Running)
            {
                UpdateState(State.Uninitializing);

                if (Database.Uninit(out sErrorMessage))
                {
                    UpdateState(State.UninitializatedSuccess);
                }
                else
                {
                    UpdateState(State.UninitializatedFailed);

                    LogMessage("An error occurred during the uninitialization of the database.");
                    LogMessage(sErrorMessage);

                    e.Cancel = true;
                }
            }
        }

        private void MainForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            contextMenuMessageWindowStrip.ImageList = null;
            menuAppStrip.ImageList = null;
        }
        #endregion

        #region "MenuItem Event Handlers"
        private void menuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuItemEdit_DropDownOpening(object sender, EventArgs e)
        {
            UpdateEditMenu(menuItemEditCopy, menuItemEditDelete,
                           menuItemEditSelectAll);
        }

        private void menuItemEditCopy_Click(object sender, EventArgs e)
        {
            ExecuteCopy();
        }

        private void menuItemEditDelete_Click(object sender, EventArgs e)
        {
            ExecuteDelete();
        }

        private void menuItemEditSelectAll_Click(object sender, EventArgs e)
        {
            ExecuteSelectAll();
        }

        private void menuPartsFind_Click(object sender, EventArgs e)
        {
            FindPartForm FindPart = new Arcade.Forms.FindPartForm();

            new Common.Forms.FormLocation(FindPart, m_sFormLocationsRegistryKey);

            FindPart.ShowDialog(this);

            FindPart.Dispose();
        }

        private void menuPartsNewPart_Click(object sender, EventArgs e)
        {
            PartEntryForm PartEntry = new Arcade.Forms.PartEntryForm();
            System.Int32 nPartId;
            System.String sErrorMessage;

            new Common.Forms.FormLocation(PartEntry, m_sFormLocationsRegistryKey);

            PartEntry.PartEntryFormType = Arcade.Forms.PartEntryForm.EPartEntryFormType.NewPart;

            if (PartEntry.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                using (new Common.Forms.WaitCursor(this))
                {
                    if (false == Database.AddPartGroup(PartEntry.PartName,
                                                       PartEntry.PartCategoryName,
                                                       PartEntry.PartTypeName,
                                                       PartEntry.PartPackageName,
                                                       PartEntry.PartPinouts,
                                                       PartEntry.PartDatasheetColl,
                                                       out nPartId, out sErrorMessage))
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }
                }
            }

            PartEntry.Dispose();
        }

        private void menuPartsCategoryList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.PartCategoryData;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuPartsTypeList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.PartTypeData;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuPartsPackageList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.PartPackageData;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuManualsFind_Click(object sender, EventArgs e)
        {
            ListManualsForm ListManuals = new Arcade.Forms.ListManualsForm();

            new Common.Forms.FormLocation(ListManuals, m_sFormLocationsRegistryKey);

            ListManuals.ShowDialog(this);

            ListManuals.Dispose();
        }

        private void menuManualsStorageBox_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.ManualStorageBox;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuManualsPrintEdition_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.ManualPrintEdition;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuManualsCondition_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.ManualCondition;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuManualsManufacturerList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.ManufacturerData;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuGamesFind_Click(object sender, EventArgs e)
        {
            ListGamesForm ListGames = new Arcade.Forms.ListGamesForm();

            new Common.Forms.FormLocation(ListGames, m_sFormLocationsRegistryKey);

            ListGames.ShowDialog(this);

            ListGames.Dispose();
        }

        private void menuItemGamesFindBoardName_Click(object sender, EventArgs e)
        {
            FindGameBoardForm FindGameBoard = new Arcade.Forms.FindGameBoardForm();

            new Common.Forms.FormLocation(FindGameBoard, m_sFormLocationsRegistryKey);

            FindGameBoard.ShowDialog(this);

            FindGameBoard.Dispose();
        }

        private void menuGamesWiringHarnessList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.GameWiringHarnessData;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuGamesCocktailList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.GameCocktailData;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuGamesControlList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.GameControlData;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuGamesBoardTypeList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.GameBoardTypeData;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuGamesBoardPartLocationList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.GameBoardPartLocation;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuGamesManufacturerList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.ManufacturerData;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuGamesVideoList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.GameVideo;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuGamesAudioList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.GameAudio;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuItemGamesLogTypeList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.LogTypeData;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuDisplaysFind_Click(object sender, EventArgs e)
        {
            ListDisplaysForm ListDisplays = new Arcade.Forms.ListDisplaysForm();

            new Common.Forms.FormLocation(ListDisplays, m_sFormLocationsRegistryKey);

            ListDisplays.ListDisplaysFormType = Arcade.Forms.ListDisplaysForm.EListDisplaysFormType.EditDisplays;

            ListDisplays.ShowDialog(this);

            ListDisplays.Dispose();
        }

        private void menuDisplaysTypeList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.DisplayType;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuDisplaysResolutionList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.DisplayResolution;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuDisplaysColorsList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.DisplayColors;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuDisplaysOrientationList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.DisplayOrientation;

            ListData.ShowDialog(this);

            ListData.Dispose();
        }

        private void menuToolsOptions_Click(object sender, EventArgs e)
        {
            OptionsForm Options = new OptionsForm();

            new Common.Forms.FormLocation(Options, m_sFormLocationsRegistryKey);

            if (Options.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Common.Forms.MessageBox.Show(this,
                    "Please restart the application to utilize the new database settings.",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information);
            }

            Options.Dispose();
        }

        private void menuHelpAbout_Click(object sender, EventArgs e)
        {
            AboutForm About = new AboutForm();

            new Common.Forms.FormLocation(About, m_sFormLocationsRegistryKey);

            About.ShowDialog(this);

            About.Dispose();
        }
        #endregion

        #region "ContextMenu Event Handlers"
        private void contextMenuMessageWindowStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UpdateEditMenu(contextMenuItemCopy, contextMenuItemDelete,
                            contextMenuItemSelectAll);
        }

        private void contextMenuItemCopy_Click(object sender, EventArgs e)
        {
            ExecuteCopy();
        }

        private void contextMenuItemDelete_Click(object sender, EventArgs e)
        {
            ExecuteDelete();
        }

        private void contextMenuItemSelectAll_Click(object sender, EventArgs e)
        {
            ExecuteSelectAll();
        }
        #endregion

        #region "Internal Functions"
        private void UpdateState(
            State State)
        {
            System.Windows.Forms.ToolStripItem[] NoneExcludedMenuItems = { };
            System.Windows.Forms.ToolStripItem[] InitSuccessfulDatabaseNotAvailableExcludedMenuItems = {
                menuItemFileExit,
                menuItemToolsOptions,
                menuItemHelpAbout};
            System.Windows.Forms.ToolStripItem[] InitFailedExcludedMenuItems = {
                menuItemFileExit,
                menuItemHelpAbout};

            Common.Debug.Thread.IsUIThread();

            m_State = State;

            switch (State)
            {
                case State.Initializing:
                    menuAppStrip.SaveItemsEnableState();
                    menuAppStrip.DisableAllItems(NoneExcludedMenuItems);

                    m_bAllowClose = false;
                    break;
                case State.InitializatedSuccessDatabaseAvailable:
                    menuAppStrip.RestoreItemsEnableState();

                    toolStripDatabaseConnectionStatusLabel.Text = "Connected";

                    m_State = State.Running;
                    m_bAllowClose = true;
                    break;
                case State.InitializatedSuccessDatabaseNotAvailable:
                    menuAppStrip.RestoreItemsEnableState();
                    menuAppStrip.DisableAllItems(InitSuccessfulDatabaseNotAvailableExcludedMenuItems);

                    toolStripDatabaseConnectionStatusLabel.Text = "Not Connected";

                    m_State = State.Running;
                    m_bAllowClose = true;
                    break;
                case State.InitializatedFailed:
                    menuAppStrip.RestoreItemsEnableState();
                    menuAppStrip.DisableAllItems(InitFailedExcludedMenuItems);

                    toolStripDatabaseConnectionStatusLabel.Text = "Failed";

                    m_State = State.Running;
                    m_bAllowClose = true;
                    break;
                case State.Uninitializing:
                    menuAppStrip.SaveItemsEnableState();
                    menuAppStrip.DisableAllItems(NoneExcludedMenuItems);

                    m_bAllowClose = false;
                    break;
                case State.UninitializatedSuccess:
                    m_bAllowClose = true;
                    break;
                case State.UninitializatedFailed:
                    menuAppStrip.RestoreItemsEnableState();
                    menuAppStrip.DisableAllItems(InitFailedExcludedMenuItems);

                    m_bAllowClose = true;
                    break;
            }
        }

        private void LogMessage(
            System.String sMessage)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            Common.Debug.Thread.IsUIThread();

            for (System.Int32 nIndex = 0; nIndex < sMessage.Length; ++nIndex)
            {
                if (sMessage[nIndex] != '\n')
                {
                    sb.Append(sMessage[nIndex]);
                }
                else
                {
                    sb.Append(CEndOfLine);
                }
            }

            sb.Append(CEndOfLine);

            textBoxMessages.AppendText(sb.ToString());
        }

        private void InitDatabase()
        {
            System.String sErrorMessage;
            Database.EDatabaseAdapter DatabaseAdapter;
            System.Boolean bDatabaseAvailable;

            Common.Debug.Thread.IsWorkerThread();

            RunOnUIThreadWait(() =>
            {
                UpdateState(State.Initializing);

                LogMessage("Initializing the database.");
            });

            switch (m_DatabaseMode)
            {
                case DatabaseMode.Access:
                    DatabaseAdapter = Database.EDatabaseAdapter.Access;
                    break;
                case DatabaseMode.SQLServer:
                default:
                    DatabaseAdapter = Database.EDatabaseAdapter.SQLServer;
                    break;
            }

            if (Database.Init(DatabaseAdapter, m_sDatabaseRegistryKey, out bDatabaseAvailable, out sErrorMessage))
            {
                if (bDatabaseAvailable)
                {
                    RunOnUIThreadWait(() =>
                    {
                        LogMessage("Finished initializing the database.");

                        UpdateState(State.InitializatedSuccessDatabaseAvailable);
                    });
                }
                else
                {
                    RunOnUIThreadWait(() =>
                    {
                        LogMessage("Finished initializing but the database is not available.");

                        UpdateState(State.InitializatedSuccessDatabaseNotAvailable);
                    });
                }
            }
            else
            {
                RunOnUIThreadWait(() =>
                {
                    LogMessage("Failed to initialize the database.");
                    LogMessage(sErrorMessage);

                    UpdateState(State.InitializatedFailed);
                });
            }
        }

        private void InitImageLists()
        {
            if (s_bInitializeImages)
            {
                Common.Forms.ImageManager.AddToolbarSmallImages(Arcade.Forms.Resources.Resources.ResourceManager);

                s_bInitializeImages = false;
            }

            contextMenuMessageWindowStrip.ImageList = Common.Forms.ImageManager.ToolbarSmallImageList;
            menuAppStrip.ImageList = Common.Forms.ImageManager.ToolbarSmallImageList;
        }

        private void InitImageKeys()
        {
            Common.Debug.Thread.IsUIThread();

            contextMenuItemCopy.ImageKey = Common.Forms.ToolbarImageKey.Copy;
            contextMenuItemDelete.ImageKey = Common.Forms.ToolbarImageKey.Delete;
            contextMenuItemSelectAll.ImageKey = Common.Forms.ToolbarImageKey.Select;

            menuItemEditCopy.ImageKey = Common.Forms.ToolbarImageKey.Copy;
            menuItemEditDelete.ImageKey = Common.Forms.ToolbarImageKey.Delete;
            menuItemEditSelectAll.ImageKey = Common.Forms.ToolbarImageKey.Select;
        }

        private void UpdateEditMenu(
            System.Windows.Forms.ToolStripMenuItem menuItemCopy,
            System.Windows.Forms.ToolStripMenuItem menuItemClear,
            System.Windows.Forms.ToolStripMenuItem menuItemSelectAll)
        {
            Common.Debug.Thread.IsUIThread();

            if (textBoxMessages.Text.Length > 0)
            {
                menuItemClear.Enabled = true;
                menuItemSelectAll.Enabled = true;
            }
            else
            {
                menuItemClear.Enabled = false;
                menuItemSelectAll.Enabled = false;
            }

            if (textBoxMessages.SelectionLength > 0)
            {
                menuItemCopy.Enabled = true;
            }
            else
            {
                menuItemCopy.Enabled = false;
            }
        }

        private void ExecuteCopy()
        {
            Common.Debug.Thread.IsUIThread();

            System.Windows.Forms.Clipboard.SetText(textBoxMessages.SelectedText);
        }

        private void ExecuteDelete()
        {
            Common.Debug.Thread.IsUIThread();

            textBoxMessages.Text = "";
        }

        private void ExecuteSelectAll()
        {
            Common.Debug.Thread.IsUIThread();

            textBoxMessages.SelectionStart = 0;
            textBoxMessages.SelectionLength = textBoxMessages.TextLength;
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
