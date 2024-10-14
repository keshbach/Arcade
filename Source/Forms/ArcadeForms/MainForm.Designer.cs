namespace Arcade.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuAppStrip = new Common.Forms.MenuStrip();
            this.menuItemFile = new Common.Forms.ToolStripMenuItem();
            this.menuItemFileExit = new Common.Forms.ToolStripMenuItem();
            this.menuItemEdit = new Common.Forms.ToolStripMenuItem();
            this.menuItemEditCopy = new Common.Forms.ToolStripMenuItem();
            this.menuItemEditDelete = new Common.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemEditSelectAll = new Common.Forms.ToolStripMenuItem();
            this.menuItemParts = new Common.Forms.ToolStripMenuItem();
            this.menuItemPartsFind = new Common.Forms.ToolStripMenuItem();
            this.menuItemPartsNewPart = new Common.Forms.ToolStripMenuItem();
            this.dividerToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemPartsCategoryList = new Common.Forms.ToolStripMenuItem();
            this.menuItemPartsTypeList = new Common.Forms.ToolStripMenuItem();
            this.menuItemPartsPackageList = new Common.Forms.ToolStripMenuItem();
            this.menuItemManuals = new Common.Forms.ToolStripMenuItem();
            this.menuItemManualsFind = new Common.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemManualsStorageBox = new Common.Forms.ToolStripMenuItem();
            this.menuItemManualsPrintEdition = new Common.Forms.ToolStripMenuItem();
            this.menuItemManualsCondition = new Common.Forms.ToolStripMenuItem();
            this.menuItemManualsManufacturerList = new Common.Forms.ToolStripMenuItem();
            this.menuItemGames = new Common.Forms.ToolStripMenuItem();
            this.menuItemGamesFind = new Common.Forms.ToolStripMenuItem();
            this.menuItemGamesFindBoardName = new Common.Forms.ToolStripMenuItem();
            this.wiringHarnessListToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemGamesWiringHarnessList = new Common.Forms.ToolStripMenuItem();
            this.menuItemGamesCocktailList = new Common.Forms.ToolStripMenuItem();
            this.menuItemGamesControlList = new Common.Forms.ToolStripMenuItem();
            this.menuItemGamesBoardTypeList = new Common.Forms.ToolStripMenuItem();
            this.menuItemGamesBoardPartLocationList = new Common.Forms.ToolStripMenuItem();
            this.menuItemGamesVideoList = new Common.Forms.ToolStripMenuItem();
            this.menuItemGamesAudioList = new Common.Forms.ToolStripMenuItem();
            this.menuItemGamesManufacturerList = new Common.Forms.ToolStripMenuItem();
            this.menuItemGamesLogTypeList = new Common.Forms.ToolStripMenuItem();
            this.menuItemDisplays = new Common.Forms.ToolStripMenuItem();
            this.menuItemDisplaysFind = new Common.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemDisplaysTypeList = new Common.Forms.ToolStripMenuItem();
            this.menuItemDisplaysResolutionList = new Common.Forms.ToolStripMenuItem();
            this.menuItemDisplaysColorsList = new Common.Forms.ToolStripMenuItem();
            this.menuItemDisplaysOrientationList = new Common.Forms.ToolStripMenuItem();
            this.menuItemTools = new Common.Forms.ToolStripMenuItem();
            this.menuItemToolsOptions = new Common.Forms.ToolStripMenuItem();
            this.menuItemHelp = new Common.Forms.ToolStripMenuItem();
            this.menuItemHelpAbout = new Common.Forms.ToolStripMenuItem();
            this.statusStrip = new Common.Forms.StatusStrip();
            this.toolStripDatabaseModeStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDatabaseConnectionStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.textBoxMessages = new System.Windows.Forms.TextBox();
            this.contextMenuMessageWindowStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuItemCopy = new Common.Forms.ToolStripMenuItem();
            this.contextMenuItemDelete = new Common.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuItemSelectAll = new Common.Forms.ToolStripMenuItem();
            this.timerUpdater = new System.Windows.Forms.Timer(this.components);
            this.menuAppStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.contextMenuMessageWindowStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuAppStrip
            // 
            this.menuAppStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFile,
            this.menuItemEdit,
            this.menuItemParts,
            this.menuItemManuals,
            this.menuItemGames,
            this.menuItemDisplays,
            this.menuItemTools,
            this.menuItemHelp});
            this.menuAppStrip.Location = new System.Drawing.Point(0, 0);
            this.menuAppStrip.Name = "menuAppStrip";
            this.menuAppStrip.Size = new System.Drawing.Size(430, 24);
            this.menuAppStrip.TabIndex = 0;
            this.menuAppStrip.Text = "menuAppStrip";
            // 
            // menuItemFile
            // 
            this.menuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFileExit});
            this.menuItemFile.HelpText = null;
            this.menuItemFile.Name = "menuItemFile";
            this.menuItemFile.Size = new System.Drawing.Size(37, 20);
            this.menuItemFile.Text = "&File";
            // 
            // menuItemFileExit
            // 
            this.menuItemFileExit.HelpText = null;
            this.menuItemFileExit.Name = "menuItemFileExit";
            this.menuItemFileExit.Size = new System.Drawing.Size(93, 22);
            this.menuItemFileExit.Text = "E&xit";
            this.menuItemFileExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuItemEdit
            // 
            this.menuItemEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemEditCopy,
            this.menuItemEditDelete,
            this.toolStripSeparator4,
            this.menuItemEditSelectAll});
            this.menuItemEdit.HelpText = "Options for messages.";
            this.menuItemEdit.Name = "menuItemEdit";
            this.menuItemEdit.Size = new System.Drawing.Size(39, 20);
            this.menuItemEdit.Text = "&Edit";
            this.menuItemEdit.DropDownOpening += new System.EventHandler(this.menuItemEdit_DropDownOpening);
            // 
            // menuItemEditCopy
            // 
            this.menuItemEditCopy.HelpText = "Copy the selected message(s) into the clipboard.";
            this.menuItemEditCopy.Name = "menuItemEditCopy";
            this.menuItemEditCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.menuItemEditCopy.Size = new System.Drawing.Size(164, 22);
            this.menuItemEditCopy.Text = "&Copy";
            this.menuItemEditCopy.Click += new System.EventHandler(this.menuItemEditCopy_Click);
            // 
            // menuItemEditDelete
            // 
            this.menuItemEditDelete.HelpText = "Remove all messages.";
            this.menuItemEditDelete.Name = "menuItemEditDelete";
            this.menuItemEditDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.menuItemEditDelete.Size = new System.Drawing.Size(164, 22);
            this.menuItemEditDelete.Text = "&Delete";
            this.menuItemEditDelete.Click += new System.EventHandler(this.menuItemEditDelete_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(161, 6);
            // 
            // menuItemEditSelectAll
            // 
            this.menuItemEditSelectAll.HelpText = "Select all messages.";
            this.menuItemEditSelectAll.Name = "menuItemEditSelectAll";
            this.menuItemEditSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.menuItemEditSelectAll.Size = new System.Drawing.Size(164, 22);
            this.menuItemEditSelectAll.Text = "Select &All";
            this.menuItemEditSelectAll.Click += new System.EventHandler(this.menuItemEditSelectAll_Click);
            // 
            // menuItemParts
            // 
            this.menuItemParts.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemPartsFind,
            this.menuItemPartsNewPart,
            this.dividerToolStripMenuItem,
            this.menuItemPartsCategoryList,
            this.menuItemPartsTypeList,
            this.menuItemPartsPackageList});
            this.menuItemParts.HelpText = "Options for parts.";
            this.menuItemParts.Name = "menuItemParts";
            this.menuItemParts.Size = new System.Drawing.Size(45, 20);
            this.menuItemParts.Text = "&Parts";
            // 
            // menuItemPartsFind
            // 
            this.menuItemPartsFind.HelpText = null;
            this.menuItemPartsFind.Name = "menuItemPartsFind";
            this.menuItemPartsFind.Size = new System.Drawing.Size(152, 22);
            this.menuItemPartsFind.Text = "&Find...";
            this.menuItemPartsFind.Click += new System.EventHandler(this.menuPartsFind_Click);
            // 
            // menuItemPartsNewPart
            // 
            this.menuItemPartsNewPart.HelpText = null;
            this.menuItemPartsNewPart.Name = "menuItemPartsNewPart";
            this.menuItemPartsNewPart.Size = new System.Drawing.Size(152, 22);
            this.menuItemPartsNewPart.Text = "&New Part...";
            this.menuItemPartsNewPart.Click += new System.EventHandler(this.menuPartsNewPart_Click);
            // 
            // dividerToolStripMenuItem
            // 
            this.dividerToolStripMenuItem.Name = "dividerToolStripMenuItem";
            this.dividerToolStripMenuItem.Size = new System.Drawing.Size(149, 6);
            // 
            // menuItemPartsCategoryList
            // 
            this.menuItemPartsCategoryList.HelpText = null;
            this.menuItemPartsCategoryList.Name = "menuItemPartsCategoryList";
            this.menuItemPartsCategoryList.Size = new System.Drawing.Size(152, 22);
            this.menuItemPartsCategoryList.Text = "&Category List...";
            this.menuItemPartsCategoryList.Click += new System.EventHandler(this.menuPartsCategoryList_Click);
            // 
            // menuItemPartsTypeList
            // 
            this.menuItemPartsTypeList.HelpText = null;
            this.menuItemPartsTypeList.Name = "menuItemPartsTypeList";
            this.menuItemPartsTypeList.Size = new System.Drawing.Size(152, 22);
            this.menuItemPartsTypeList.Text = "&Type List...";
            this.menuItemPartsTypeList.Click += new System.EventHandler(this.menuPartsTypeList_Click);
            // 
            // menuItemPartsPackageList
            // 
            this.menuItemPartsPackageList.HelpText = null;
            this.menuItemPartsPackageList.Name = "menuItemPartsPackageList";
            this.menuItemPartsPackageList.Size = new System.Drawing.Size(152, 22);
            this.menuItemPartsPackageList.Text = "&Package List...";
            this.menuItemPartsPackageList.Click += new System.EventHandler(this.menuPartsPackageList_Click);
            // 
            // menuItemManuals
            // 
            this.menuItemManuals.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemManualsFind,
            this.toolStripSeparator1,
            this.menuItemManualsStorageBox,
            this.menuItemManualsPrintEdition,
            this.menuItemManualsCondition,
            this.menuItemManualsManufacturerList});
            this.menuItemManuals.HelpText = "Options for manuals.";
            this.menuItemManuals.Name = "menuItemManuals";
            this.menuItemManuals.Size = new System.Drawing.Size(64, 20);
            this.menuItemManuals.Text = "&Manuals";
            // 
            // menuItemManualsFind
            // 
            this.menuItemManualsFind.HelpText = null;
            this.menuItemManualsFind.Name = "menuItemManualsFind";
            this.menuItemManualsFind.Size = new System.Drawing.Size(176, 22);
            this.menuItemManualsFind.Text = "&Find...";
            this.menuItemManualsFind.Click += new System.EventHandler(this.menuManualsFind_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(173, 6);
            // 
            // menuItemManualsStorageBox
            // 
            this.menuItemManualsStorageBox.HelpText = null;
            this.menuItemManualsStorageBox.Name = "menuItemManualsStorageBox";
            this.menuItemManualsStorageBox.Size = new System.Drawing.Size(176, 22);
            this.menuItemManualsStorageBox.Text = "&Storage Box List...";
            this.menuItemManualsStorageBox.Click += new System.EventHandler(this.menuManualsStorageBox_Click);
            // 
            // menuItemManualsPrintEdition
            // 
            this.menuItemManualsPrintEdition.HelpText = null;
            this.menuItemManualsPrintEdition.Name = "menuItemManualsPrintEdition";
            this.menuItemManualsPrintEdition.Size = new System.Drawing.Size(176, 22);
            this.menuItemManualsPrintEdition.Text = "&Print Edition List...";
            this.menuItemManualsPrintEdition.Click += new System.EventHandler(this.menuManualsPrintEdition_Click);
            // 
            // menuItemManualsCondition
            // 
            this.menuItemManualsCondition.HelpText = null;
            this.menuItemManualsCondition.Name = "menuItemManualsCondition";
            this.menuItemManualsCondition.Size = new System.Drawing.Size(176, 22);
            this.menuItemManualsCondition.Text = "&Condition List...";
            this.menuItemManualsCondition.Click += new System.EventHandler(this.menuManualsCondition_Click);
            // 
            // menuItemManualsManufacturerList
            // 
            this.menuItemManualsManufacturerList.HelpText = null;
            this.menuItemManualsManufacturerList.Name = "menuItemManualsManufacturerList";
            this.menuItemManualsManufacturerList.Size = new System.Drawing.Size(176, 22);
            this.menuItemManualsManufacturerList.Text = "&Manufacturer List...";
            this.menuItemManualsManufacturerList.Click += new System.EventHandler(this.menuManualsManufacturerList_Click);
            // 
            // menuItemGames
            // 
            this.menuItemGames.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemGamesFind,
            this.menuItemGamesFindBoardName,
            this.wiringHarnessListToolStripMenuItem,
            this.menuItemGamesWiringHarnessList,
            this.menuItemGamesCocktailList,
            this.menuItemGamesControlList,
            this.menuItemGamesBoardTypeList,
            this.menuItemGamesBoardPartLocationList,
            this.menuItemGamesVideoList,
            this.menuItemGamesAudioList,
            this.menuItemGamesManufacturerList,
            this.menuItemGamesLogTypeList});
            this.menuItemGames.HelpText = "Options for games.";
            this.menuItemGames.Name = "menuItemGames";
            this.menuItemGames.Size = new System.Drawing.Size(55, 20);
            this.menuItemGames.Text = "&Games";
            // 
            // menuItemGamesFind
            // 
            this.menuItemGamesFind.HelpText = null;
            this.menuItemGamesFind.Name = "menuItemGamesFind";
            this.menuItemGamesFind.Size = new System.Drawing.Size(208, 22);
            this.menuItemGamesFind.Text = "&Find...";
            this.menuItemGamesFind.Click += new System.EventHandler(this.menuGamesFind_Click);
            // 
            // menuItemGamesFindBoardName
            // 
            this.menuItemGamesFindBoardName.HelpText = null;
            this.menuItemGamesFindBoardName.Name = "menuItemGamesFindBoardName";
            this.menuItemGamesFindBoardName.Size = new System.Drawing.Size(208, 22);
            this.menuItemGamesFindBoardName.Text = "Find Board &Name...";
            this.menuItemGamesFindBoardName.Click += new System.EventHandler(this.menuItemGamesFindBoardName_Click);
            // 
            // wiringHarnessListToolStripMenuItem
            // 
            this.wiringHarnessListToolStripMenuItem.Name = "wiringHarnessListToolStripMenuItem";
            this.wiringHarnessListToolStripMenuItem.Size = new System.Drawing.Size(205, 6);
            // 
            // menuItemGamesWiringHarnessList
            // 
            this.menuItemGamesWiringHarnessList.HelpText = null;
            this.menuItemGamesWiringHarnessList.Name = "menuItemGamesWiringHarnessList";
            this.menuItemGamesWiringHarnessList.Size = new System.Drawing.Size(208, 22);
            this.menuItemGamesWiringHarnessList.Text = "&Wiring Harness List...";
            this.menuItemGamesWiringHarnessList.Click += new System.EventHandler(this.menuGamesWiringHarnessList_Click);
            // 
            // menuItemGamesCocktailList
            // 
            this.menuItemGamesCocktailList.HelpText = null;
            this.menuItemGamesCocktailList.Name = "menuItemGamesCocktailList";
            this.menuItemGamesCocktailList.Size = new System.Drawing.Size(208, 22);
            this.menuItemGamesCocktailList.Text = "C&ocktail List...";
            this.menuItemGamesCocktailList.Click += new System.EventHandler(this.menuGamesCocktailList_Click);
            // 
            // menuItemGamesControlList
            // 
            this.menuItemGamesControlList.HelpText = null;
            this.menuItemGamesControlList.Name = "menuItemGamesControlList";
            this.menuItemGamesControlList.Size = new System.Drawing.Size(208, 22);
            this.menuItemGamesControlList.Text = "&Control List...";
            this.menuItemGamesControlList.Click += new System.EventHandler(this.menuGamesControlList_Click);
            // 
            // menuItemGamesBoardTypeList
            // 
            this.menuItemGamesBoardTypeList.HelpText = null;
            this.menuItemGamesBoardTypeList.Name = "menuItemGamesBoardTypeList";
            this.menuItemGamesBoardTypeList.Size = new System.Drawing.Size(208, 22);
            this.menuItemGamesBoardTypeList.Text = "&Board Type List...";
            this.menuItemGamesBoardTypeList.Click += new System.EventHandler(this.menuGamesBoardTypeList_Click);
            // 
            // menuItemGamesBoardPartLocationList
            // 
            this.menuItemGamesBoardPartLocationList.HelpText = null;
            this.menuItemGamesBoardPartLocationList.Name = "menuItemGamesBoardPartLocationList";
            this.menuItemGamesBoardPartLocationList.Size = new System.Drawing.Size(208, 22);
            this.menuItemGamesBoardPartLocationList.Text = "Board &Part Location List...";
            this.menuItemGamesBoardPartLocationList.Click += new System.EventHandler(this.menuGamesBoardPartLocationList_Click);
            // 
            // menuItemGamesVideoList
            // 
            this.menuItemGamesVideoList.HelpText = null;
            this.menuItemGamesVideoList.Name = "menuItemGamesVideoList";
            this.menuItemGamesVideoList.Size = new System.Drawing.Size(208, 22);
            this.menuItemGamesVideoList.Text = "&Video List...";
            this.menuItemGamesVideoList.Click += new System.EventHandler(this.menuGamesVideoList_Click);
            // 
            // menuItemGamesAudioList
            // 
            this.menuItemGamesAudioList.HelpText = null;
            this.menuItemGamesAudioList.Name = "menuItemGamesAudioList";
            this.menuItemGamesAudioList.Size = new System.Drawing.Size(208, 22);
            this.menuItemGamesAudioList.Text = "&Audio List...";
            this.menuItemGamesAudioList.Click += new System.EventHandler(this.menuGamesAudioList_Click);
            // 
            // menuItemGamesManufacturerList
            // 
            this.menuItemGamesManufacturerList.HelpText = null;
            this.menuItemGamesManufacturerList.Name = "menuItemGamesManufacturerList";
            this.menuItemGamesManufacturerList.Size = new System.Drawing.Size(208, 22);
            this.menuItemGamesManufacturerList.Text = "&Manufacturer List...";
            this.menuItemGamesManufacturerList.Click += new System.EventHandler(this.menuGamesManufacturerList_Click);
            // 
            // menuItemGamesLogTypeList
            // 
            this.menuItemGamesLogTypeList.HelpText = null;
            this.menuItemGamesLogTypeList.Name = "menuItemGamesLogTypeList";
            this.menuItemGamesLogTypeList.Size = new System.Drawing.Size(208, 22);
            this.menuItemGamesLogTypeList.Text = "&Log Type List...";
            this.menuItemGamesLogTypeList.Click += new System.EventHandler(this.menuItemGamesLogTypeList_Click);
            // 
            // menuItemDisplays
            // 
            this.menuItemDisplays.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemDisplaysFind,
            this.toolStripSeparator2,
            this.menuItemDisplaysTypeList,
            this.menuItemDisplaysResolutionList,
            this.menuItemDisplaysColorsList,
            this.menuItemDisplaysOrientationList});
            this.menuItemDisplays.HelpText = "Options for displays.";
            this.menuItemDisplays.Name = "menuItemDisplays";
            this.menuItemDisplays.Size = new System.Drawing.Size(62, 20);
            this.menuItemDisplays.Text = "&Displays";
            // 
            // menuItemDisplaysFind
            // 
            this.menuItemDisplaysFind.HelpText = null;
            this.menuItemDisplaysFind.Name = "menuItemDisplaysFind";
            this.menuItemDisplaysFind.Size = new System.Drawing.Size(164, 22);
            this.menuItemDisplaysFind.Text = "&Find...";
            this.menuItemDisplaysFind.Click += new System.EventHandler(this.menuDisplaysFind_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(161, 6);
            // 
            // menuItemDisplaysTypeList
            // 
            this.menuItemDisplaysTypeList.HelpText = null;
            this.menuItemDisplaysTypeList.Name = "menuItemDisplaysTypeList";
            this.menuItemDisplaysTypeList.Size = new System.Drawing.Size(164, 22);
            this.menuItemDisplaysTypeList.Text = "&Type List...";
            this.menuItemDisplaysTypeList.Click += new System.EventHandler(this.menuDisplaysTypeList_Click);
            // 
            // menuItemDisplaysResolutionList
            // 
            this.menuItemDisplaysResolutionList.HelpText = null;
            this.menuItemDisplaysResolutionList.Name = "menuItemDisplaysResolutionList";
            this.menuItemDisplaysResolutionList.Size = new System.Drawing.Size(164, 22);
            this.menuItemDisplaysResolutionList.Text = "&Resolution List...";
            this.menuItemDisplaysResolutionList.Click += new System.EventHandler(this.menuDisplaysResolutionList_Click);
            // 
            // menuItemDisplaysColorsList
            // 
            this.menuItemDisplaysColorsList.HelpText = null;
            this.menuItemDisplaysColorsList.Name = "menuItemDisplaysColorsList";
            this.menuItemDisplaysColorsList.Size = new System.Drawing.Size(164, 22);
            this.menuItemDisplaysColorsList.Text = "&Colors List...";
            this.menuItemDisplaysColorsList.Click += new System.EventHandler(this.menuDisplaysColorsList_Click);
            // 
            // menuItemDisplaysOrientationList
            // 
            this.menuItemDisplaysOrientationList.HelpText = null;
            this.menuItemDisplaysOrientationList.Name = "menuItemDisplaysOrientationList";
            this.menuItemDisplaysOrientationList.Size = new System.Drawing.Size(164, 22);
            this.menuItemDisplaysOrientationList.Text = "&Orientation List...";
            this.menuItemDisplaysOrientationList.Click += new System.EventHandler(this.menuDisplaysOrientationList_Click);
            // 
            // menuItemTools
            // 
            this.menuItemTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolsOptions});
            this.menuItemTools.HelpText = "Options for configuring the application.";
            this.menuItemTools.Name = "menuItemTools";
            this.menuItemTools.Size = new System.Drawing.Size(46, 20);
            this.menuItemTools.Text = "&Tools";
            // 
            // menuItemToolsOptions
            // 
            this.menuItemToolsOptions.HelpText = null;
            this.menuItemToolsOptions.Name = "menuItemToolsOptions";
            this.menuItemToolsOptions.Size = new System.Drawing.Size(125, 22);
            this.menuItemToolsOptions.Text = "&Options...";
            this.menuItemToolsOptions.Click += new System.EventHandler(this.menuToolsOptions_Click);
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemHelpAbout});
            this.menuItemHelp.HelpText = "Options for messages.";
            this.menuItemHelp.Name = "menuItemHelp";
            this.menuItemHelp.Size = new System.Drawing.Size(44, 20);
            this.menuItemHelp.Text = "&Help";
            // 
            // menuItemHelpAbout
            // 
            this.menuItemHelpAbout.HelpText = null;
            this.menuItemHelpAbout.Name = "menuItemHelpAbout";
            this.menuItemHelpAbout.Size = new System.Drawing.Size(181, 22);
            this.menuItemHelpAbout.Text = "&About Arcade App...";
            this.menuItemHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ActiveGroup = "";
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDatabaseModeStatusLabel,
            this.toolStripDatabaseConnectionStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 159);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(430, 22);
            this.statusStrip.TabIndex = 2;
            // 
            // toolStripDatabaseModeStatusLabel
            // 
            this.toolStripDatabaseModeStatusLabel.Name = "toolStripDatabaseModeStatusLabel";
            this.toolStripDatabaseModeStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripDatabaseConnectionStatusLabel
            // 
            this.toolStripDatabaseConnectionStatusLabel.Name = "toolStripDatabaseConnectionStatusLabel";
            this.toolStripDatabaseConnectionStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // textBoxMessages
            // 
            this.textBoxMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMessages.ContextMenuStrip = this.contextMenuMessageWindowStrip;
            this.textBoxMessages.HideSelection = false;
            this.textBoxMessages.Location = new System.Drawing.Point(12, 27);
            this.textBoxMessages.MaxLength = 2000000000;
            this.textBoxMessages.Multiline = true;
            this.textBoxMessages.Name = "textBoxMessages";
            this.textBoxMessages.ReadOnly = true;
            this.textBoxMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxMessages.Size = new System.Drawing.Size(406, 122);
            this.textBoxMessages.TabIndex = 1;
            this.textBoxMessages.WordWrap = false;
            // 
            // contextMenuMessageWindowStrip
            // 
            this.contextMenuMessageWindowStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuItemCopy,
            this.contextMenuItemDelete,
            this.toolStripSeparator3,
            this.contextMenuItemSelectAll});
            this.contextMenuMessageWindowStrip.Name = "contextMenuMessageWindowStrip";
            this.contextMenuMessageWindowStrip.Size = new System.Drawing.Size(165, 76);
            this.contextMenuMessageWindowStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuMessageWindowStrip_Opening);
            // 
            // contextMenuItemCopy
            // 
            this.contextMenuItemCopy.HelpText = "Copy the selected message(s) into the clipboard.";
            this.contextMenuItemCopy.Name = "contextMenuItemCopy";
            this.contextMenuItemCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.contextMenuItemCopy.Size = new System.Drawing.Size(164, 22);
            this.contextMenuItemCopy.Text = "&Copy";
            this.contextMenuItemCopy.Click += new System.EventHandler(this.contextMenuItemCopy_Click);
            // 
            // contextMenuItemDelete
            // 
            this.contextMenuItemDelete.HelpText = "Remove all messages.";
            this.contextMenuItemDelete.Name = "contextMenuItemDelete";
            this.contextMenuItemDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.contextMenuItemDelete.Size = new System.Drawing.Size(164, 22);
            this.contextMenuItemDelete.Text = "&Delete";
            this.contextMenuItemDelete.Click += new System.EventHandler(this.contextMenuItemDelete_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(161, 6);
            // 
            // contextMenuItemSelectAll
            // 
            this.contextMenuItemSelectAll.HelpText = "Select all messages.";
            this.contextMenuItemSelectAll.Name = "contextMenuItemSelectAll";
            this.contextMenuItemSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.contextMenuItemSelectAll.Size = new System.Drawing.Size(164, 22);
            this.contextMenuItemSelectAll.Text = "Select &All";
            this.contextMenuItemSelectAll.Click += new System.EventHandler(this.contextMenuItemSelectAll_Click);
            // 
            // timerUpdater
            // 
            this.timerUpdater.Interval = 1000;
            this.timerUpdater.Tick += new System.EventHandler(this.timerUpdater_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(430, 181);
            this.Controls.Add(this.textBoxMessages);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuAppStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuAppStrip;
            this.MinimumSize = new System.Drawing.Size(430, 220);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Arcade App";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuAppStrip.ResumeLayout(false);
            this.menuAppStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.contextMenuMessageWindowStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Common.Forms.MenuStrip menuAppStrip;
        private Common.Forms.ToolStripMenuItem menuItemFile;
        private Common.Forms.ToolStripMenuItem menuItemHelp;
        private Common.Forms.ToolStripMenuItem menuItemFileExit;
        private Common.Forms.ToolStripMenuItem menuItemEdit;
        private Common.Forms.ToolStripMenuItem menuItemEditCopy;
        private Common.Forms.ToolStripMenuItem menuItemEditDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private Common.Forms.ToolStripMenuItem menuItemEditSelectAll;
        private Common.Forms.ToolStripMenuItem menuItemParts;
        private Common.Forms.ToolStripMenuItem menuItemManuals;
        private Common.Forms.ToolStripMenuItem menuItemGames;
        private Common.Forms.ToolStripMenuItem menuItemManualsFind;
        private Common.Forms.ToolStripMenuItem menuItemGamesFind;
        private Common.Forms.ToolStripMenuItem menuItemPartsFind;
        private Common.Forms.ToolStripMenuItem menuItemPartsNewPart;
        private Common.Forms.ToolStripMenuItem menuItemHelpAbout;
        private Common.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripSeparator dividerToolStripMenuItem;
        private Common.Forms.ToolStripMenuItem menuItemPartsCategoryList;
        private Common.Forms.ToolStripMenuItem menuItemPartsTypeList;
        private Common.Forms.ToolStripMenuItem menuItemPartsPackageList;
        private System.Windows.Forms.ToolStripSeparator wiringHarnessListToolStripMenuItem;
        private Common.Forms.ToolStripMenuItem menuItemGamesWiringHarnessList;
        private Common.Forms.ToolStripMenuItem menuItemGamesControlList;
        private Common.Forms.ToolStripMenuItem menuItemGamesBoardTypeList;
        private Common.Forms.ToolStripMenuItem menuItemGamesManufacturerList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Common.Forms.ToolStripMenuItem menuItemManualsManufacturerList;
        private Common.Forms.ToolStripMenuItem menuItemGamesVideoList;
        private Common.Forms.ToolStripMenuItem menuItemGamesAudioList;
        private Common.Forms.ToolStripMenuItem menuItemManualsStorageBox;
        private Common.Forms.ToolStripMenuItem menuItemManualsPrintEdition;
        private Common.Forms.ToolStripMenuItem menuItemManualsCondition;
        private Common.Forms.ToolStripMenuItem menuItemDisplays;
        private Common.Forms.ToolStripMenuItem menuItemDisplaysFind;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private Common.Forms.ToolStripMenuItem menuItemDisplaysTypeList;
        private Common.Forms.ToolStripMenuItem menuItemDisplaysResolutionList;
        private Common.Forms.ToolStripMenuItem menuItemDisplaysColorsList;
        private Common.Forms.ToolStripMenuItem menuItemDisplaysOrientationList;
        private Common.Forms.ToolStripMenuItem menuItemGamesBoardPartLocationList;
        private Common.Forms.ToolStripMenuItem menuItemGamesCocktailList;
        private Common.Forms.ToolStripMenuItem menuItemGamesFindBoardName;
        private Common.Forms.ToolStripMenuItem menuItemGamesLogTypeList;
        private Common.Forms.ToolStripMenuItem menuItemTools;
        private System.Windows.Forms.TextBox textBoxMessages;
        private System.Windows.Forms.ContextMenuStrip contextMenuMessageWindowStrip;
        private Common.Forms.ToolStripMenuItem contextMenuItemCopy;
        private Common.Forms.ToolStripMenuItem contextMenuItemDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private Common.Forms.ToolStripMenuItem contextMenuItemSelectAll;
        private Common.Forms.ToolStripMenuItem menuItemToolsOptions;
        private System.Windows.Forms.ToolStripStatusLabel toolStripDatabaseModeStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripDatabaseConnectionStatusLabel;
        private System.Windows.Forms.Timer timerUpdater;
    }
}
