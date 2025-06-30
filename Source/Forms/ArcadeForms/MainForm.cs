/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2024 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;

namespace Arcade.Forms
{
    public partial class MainForm : Common.Forms.MainForm,
                                    Arcade.IArcadeDatabaseLogging
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
            AddingNewPart,
            AddedNewPart,
            Uninitializing,
            UninitializatedSuccess,
            UninitializatedFailed
        }
        #endregion

        #region "Structures"
        private struct TToolStripData
        {
            public TToolStripData(
                Common.Forms.ToolStripMenuItem ToolStripMenuItem,
                System.Windows.Forms.ToolStrip ToolStrip)
            {
                this.ToolStripMenuItem = ToolStripMenuItem;
                this.ToolStrip = ToolStrip;
            }

            public Common.Forms.ToolStripMenuItem ToolStripMenuItem;
            public System.Windows.Forms.ToolStrip ToolStrip;
        };
        #endregion

        #region "Member Variables"
        private System.String m_sRegistryKey;
        private System.String m_sFormLocationsRegistryKey;
        private System.String m_sDatabaseRegistryKey;

        private DatabaseMode m_DatabaseMode;

        private State m_State = State.Initialize;

        private bool m_bAllowClose = true;

        private System.Collections.Generic.List<System.String> m_MessageCacheList = new System.Collections.Generic.List<System.String>();
        private System.Threading.Mutex m_MessageCacheMutex = new System.Threading.Mutex();

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

        #region "Arcade.IArcadeDatabaseLogging"
        public void ArcadeDatabaseMessage(System.String sMessage)
        {
            Common.Debug.Thread.IsWorkerThread();

            AddCachedMessage(sMessage);
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

            InitToolStripItems();

            InitContextMenuStripItems();

            BeginUpdateTimer();

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                InitDatabase();
            }, "Main Form Initialize Database Thread");
        }

        private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            e.Cancel = !m_bAllowClose;

            if (!m_bAllowClose)
            {
                return;
            }

            if (m_State == State.Running)
            {
                e.Cancel = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    UninitDatabase();
                }, "Main Form Uninitialize Database Thread");
            }
            else
            {
                EndUpdateTimer();

                m_MessageCacheMutex.Close();

                m_MessageCacheMutex = null;
            }
        }

        private void MainForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            UninitImageLists();

            UninitToolStripItems();

            UninitContextMenuStripItems();
        }
        #endregion

        #region "MenuItem Event Handlers"
        private void menuExit_Click(object sender, EventArgs e)
        {
            Common.Debug.Thread.IsUIThread();

            Close();
        }

        private void menuItemEditCopy_Click(object sender, EventArgs e)
        {
            Common.Debug.Thread.IsUIThread();

            System.Windows.Forms.Clipboard.SetText(textBoxMessages.SelectedText);
        }

        private void menuItemEditDelete_Click(object sender, EventArgs e)
        {
            Common.Debug.Thread.IsUIThread();

            textBoxMessages.Text = "";
        }

        private void menuItemEditSelectAll_Click(object sender, EventArgs e)
        {
            Common.Debug.Thread.IsUIThread();

            textBoxMessages.SelectionStart = 0;
            textBoxMessages.SelectionLength = textBoxMessages.TextLength;
        }

        private void menuPartsFind_Click(object sender, EventArgs e)
        {
            FindPartForm FindPart = new Arcade.Forms.FindPartForm();

            new Common.Forms.FormLocation(FindPart, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            FindPart.ShowDialog(this);

            FindPart.Dispose();

            BeginUpdateTimer();
        }

        private void menuPartsNewPart_Click(object sender, EventArgs e)
        {
            PartEntryForm PartEntry = new Arcade.Forms.PartEntryForm();
            System.Int32 nPartId;
            System.String sErrorMessage;
            System.Boolean bResult;

            new Common.Forms.FormLocation(PartEntry, m_sFormLocationsRegistryKey);

            PartEntry.PartEntryFormType = Arcade.Forms.PartEntryForm.EPartEntryFormType.NewPart;

            EndUpdateTimer();

            if (PartEntry.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                UpdateState(State.AddingNewPart);

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.AddPartGroup(PartEntry.PartName,
                                                    PartEntry.PartCategoryName,
                                                    PartEntry.PartTypeName,
                                                    PartEntry.PartPackageName,
                                                    PartEntry.PartPinouts,
                                                    PartEntry.PartDatasheetColl,
                                                    out nPartId, 
                                                    out sErrorMessage);
                    RunOnUIThreadWait(() =>
                    {
                        if (!bResult)
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                                         System.Windows.Forms.MessageBoxButtons.OK,
                                                         System.Windows.Forms.MessageBoxIcon.Information);
                        }

                        UpdateState(State.AddedNewPart);

                        PartEntry.Dispose();
                    });
                }, "Main Form Adding New Part Thread");
            }

            BeginUpdateTimer();
        }

        private void menuPartsCategoryList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.PartCategoryData;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuPartsTypeList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.PartTypeData;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuPartsPackageList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.PartPackageData;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuManualsFind_Click(object sender, EventArgs e)
        {
            ListManualsForm ListManuals = new Arcade.Forms.ListManualsForm();

            new Common.Forms.FormLocation(ListManuals, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListManuals.ShowDialog(this);

            ListManuals.Dispose();

            BeginUpdateTimer();
        }

        private void menuManualsStorageBox_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.ManualStorageBox;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuManualsPrintEdition_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.ManualPrintEdition;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuManualsCondition_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.ManualCondition;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuManualsManufacturerList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.ManufacturerData;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuGamesFind_Click(object sender, EventArgs e)
        {
            ListGamesForm ListGames = new Arcade.Forms.ListGamesForm();

            new Common.Forms.FormLocation(ListGames, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListGames.ShowDialog(this);

            ListGames.Dispose();

            BeginUpdateTimer();
        }

        private void menuItemGamesFindBoardName_Click(object sender, EventArgs e)
        {
            FindGameBoardForm FindGameBoard = new Arcade.Forms.FindGameBoardForm();

            new Common.Forms.FormLocation(FindGameBoard, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            FindGameBoard.ShowDialog(this);

            FindGameBoard.Dispose();

            BeginUpdateTimer();
        }

        private void menuGamesWiringHarnessList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.GameWiringHarnessData;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuGamesCocktailList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.GameCocktailData;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuGamesControlList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.GameControlData;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuGamesBoardTypeList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.GameBoardTypeData;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuGamesBoardPartLocationList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.GameBoardPartLocation;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuGamesManufacturerList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.ManufacturerData;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuGamesVideoList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.GameVideo;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuGamesAudioList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.GameAudio;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuItemGamesLogTypeList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.LogTypeData;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuDisplaysFind_Click(object sender, EventArgs e)
        {
            ListDisplaysForm ListDisplays = new Arcade.Forms.ListDisplaysForm();

            new Common.Forms.FormLocation(ListDisplays, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListDisplays.ListDisplaysFormType = Arcade.Forms.ListDisplaysForm.EListDisplaysFormType.EditDisplays;

            ListDisplays.ShowDialog(this);

            ListDisplays.Dispose();

            BeginUpdateTimer();
        }

        private void menuDisplaysTypeList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.DisplayType;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuDisplaysResolutionList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.DisplayResolution;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuDisplaysColorsList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.DisplayColors;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuDisplaysOrientationList_Click(object sender, EventArgs e)
        {
            ListDataForm ListData = new Arcade.Forms.ListDataForm();

            new Common.Forms.FormLocation(ListData, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            ListData.ListDataFormType = Arcade.Forms.ListDataForm.EListDataFormType.DisplayOrientation;

            ListData.ShowDialog(this);

            ListData.Dispose();

            BeginUpdateTimer();
        }

        private void menuToolsOptions_Click(object sender, EventArgs e)
        {
            OptionsForm Options = new OptionsForm();

            new Common.Forms.FormLocation(Options, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            if (Options.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Common.Forms.MessageBox.Show(this,
                    "Please restart the application to utilize the new database settings.",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information);
            }

            Options.Dispose();

            BeginUpdateTimer();
        }

        private void menuHelpAbout_Click(object sender, EventArgs e)
        {
            AboutForm About = new AboutForm();

            new Common.Forms.FormLocation(About, m_sFormLocationsRegistryKey);

            EndUpdateTimer();

            About.ShowDialog(this);

            About.Dispose();

            BeginUpdateTimer();
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
                case State.AddingNewPart:
                    menuAppStrip.SaveItemsEnableState();
                    menuAppStrip.DisableAllItems(NoneExcludedMenuItems);

                    m_bAllowClose = false;
                    break;
                case State.AddedNewPart:
                    menuAppStrip.RestoreItemsEnableState();

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

            if (Database.Init(DatabaseAdapter, m_sDatabaseRegistryKey, this, out bDatabaseAvailable, out sErrorMessage))
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

        private void UninitDatabase()
        {
            System.String sErrorMessage;
            System.Boolean bError;
    
            Common.Debug.Thread.IsWorkerThread();

            RunOnUIThreadWait(() =>
            {
                UpdateState(State.Uninitializing);

                LogMessage("Uninitializing the database.");
            });

            bError = Database.Uninit(out sErrorMessage);

            RunOnUIThreadNoWait(() =>
            {
                if (bError)
                {
                    UpdateState(State.UninitializatedSuccess);

                    this.Close();
                }
                else
                {
                    UpdateState(State.UninitializatedFailed);

                    LogMessage("An error occurred during the uninitialization of the database.");
                    LogMessage(sErrorMessage);
                }
            });
        }

        private void InitImageLists()
        {
            System.Windows.Forms.ToolStrip[] ToolStrips = {
                    menuAppStrip,
                    contextMenuMessageWindowStrip,
                    toolStripEdit};

            Common.Debug.Thread.IsUIThread();

            if (s_bInitializeImages)
            {
                Common.Forms.ImageManager.AddToolbarSmallImages(Arcade.Forms.Resources.Resources.ResourceManager);

                s_bInitializeImages = false;
            }

            foreach (System.Windows.Forms.ToolStrip ToolStrip in ToolStrips)
            {
                ToolStrip.ImageList = Common.Forms.ImageManager.ToolbarSmallImageList;
            }

            menuAppStrip.RefreshToolStripItemsImageList();
        }

        private void UninitImageLists()
        {
            System.Windows.Forms.ToolStrip[] ToolStrips = {
                    menuAppStrip,
                    contextMenuMessageWindowStrip,
                    toolStripEdit};

            Common.Debug.Thread.IsUIThread();

            foreach (System.Windows.Forms.ToolStrip ToolStrip in ToolStrips)
            {
                ToolStrip.ImageList = null;
            }

            menuAppStrip.RefreshToolStripItemsImageList();
        }

        private void InitImageKeys()
        {
            Common.Debug.Thread.IsUIThread();

            menuItemEditCopy.ImageKey = Common.Forms.ToolbarImageKey.Copy;
            menuItemEditDelete.ImageKey = Common.Forms.ToolbarImageKey.Delete;
            menuItemEditSelectAll.ImageKey = Common.Forms.ToolbarImageKey.Select;
        }

        private void InitToolStripItems()
        {
            TToolStripData[] ToolStripDataArray = {
                new TToolStripData(menuItemEdit, toolStripEdit)};

            Common.Debug.Thread.IsUIThread();

            foreach (TToolStripData ToolStripData in ToolStripDataArray)
            {
                InitToolStripItems(ToolStripData.ToolStripMenuItem,
                                   ToolStripData.ToolStrip);
            }
        }

        private void UninitToolStripItems()
        {
            System.Windows.Forms.ToolStrip[] ToolStrips = {
                toolStripEdit};

            Common.Debug.Thread.IsUIThread();

            foreach (System.Windows.Forms.ToolStrip ToolStrip in ToolStrips)
            {
                UninitToolStripItems(ToolStrip);
            }
        }

        private void InitContextMenuStripItems()
        {
            Common.Debug.Thread.IsUIThread();

            InitContextMenuItems(menuItemEdit, contextMenuMessageWindowStrip);
        }

        private void UninitContextMenuStripItems()
        {
            Common.Debug.Thread.IsUIThread();

            UninitContextMenuItems(contextMenuMessageWindowStrip);
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

        private void AddCachedMessage(
            System.String sMessage)
        {
            Common.Debug.Thread.IsWorkerThread();

            m_MessageCacheMutex.WaitOne();

            m_MessageCacheList.Add(sMessage);

            m_MessageCacheMutex.ReleaseMutex();
        }

        private void FlushMessages()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.Int32 nLength = 0;

            Common.Debug.Thread.IsUIThread();

            m_MessageCacheMutex.WaitOne();

            foreach (System.String sValue in m_MessageCacheList)
            {
                nLength += sValue.Length;
                nLength += CEndOfLine.Length;
            }

            sb.EnsureCapacity(nLength);

            foreach (System.String sValue in m_MessageCacheList)
            {
                sb.Append(sValue);
                sb.Append(CEndOfLine);
            }

            m_MessageCacheList.Clear();

            textBoxMessages.AppendText(sb.ToString());

            m_MessageCacheMutex.ReleaseMutex();
        }

        private void BeginUpdateTimer()
        {
            Common.Debug.Thread.IsUIThread();

            timerUpdater.Start();
        }

        private void EndUpdateTimer()
        {
            Common.Debug.Thread.IsUIThread();

            timerUpdater.Stop();

            FlushMessages();
        }
        #endregion

        #region "Timer Event Handlers"
        private void timerUpdater_Tick(object sender, EventArgs e)
        {
            Common.Debug.Thread.IsUIThread();

            FlushMessages();
        }
        #endregion

        #region "Text Box Messages Event Handlers"
        private void textBoxMessages_TextChanged(object sender, EventArgs e)
        {
            if (textBoxMessages.Text.Length > 0)
            {
                menuItemEditDelete.Enabled = true;
                menuItemEditSelectAll.Enabled = true;
            }
            else
            {
                menuItemEditDelete.Enabled = false;
                menuItemEditSelectAll.Enabled = false;
            }
        }

        private void textBoxMessages_TextSelected(object sender, Common.Forms.TextBoxMessagesTextSelectedEventArgs e)
        {
            menuItemEditCopy.Enabled = e.TextSelected;
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2024 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
