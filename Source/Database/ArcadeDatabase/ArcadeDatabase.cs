/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2016 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;

namespace Arcade
{
    /// <summary>
    /// Utility functions to interact with a database.
    /// </summary>
	public sealed class Database
    {
        // Table names
        private static System.String CManualTableName = "Manual";
        private static System.String CGameTableName = "Game";
        private static System.String CBoardTableName = "Board";
        private static System.String CBoardPartTableName = "BoardPart";
        private static System.String CDisplayTableName = "Display";
        private static System.String CPartTableName = "Part";
        private static System.String CPartPinoutsTableName = "PartPinouts";
        private static System.String CPartPropertyTableName = "PartProperty";
        private static System.String CManufacturerTableName = "Manufacturer";
        private static System.String CBoardTypeTableName = "BoardType";
        private static System.String CBoardPartLocationTableName = "BoardPartLocation";
        private static System.String CLogTypeTableName = "LogType";
        private static System.String CLogTableName = "Log";

        // Game Property Names
        private static System.String CGameAudioName = "Audio";
        private static System.String CGameWiringName = "Wiring";
        private static System.String CGameControlsName = "Controls";
        private static System.String CGameVideoName = "Video";
        private static System.String CGameCocktailName = "Cocktail";

        // Display Property Names
        private static System.String CDisplayTypeName = "Type";
        private static System.String CDisplayResolutionName = "Resolution";
        private static System.String CDisplayColorsName = "Colors";
        private static System.String CDisplayOrientationName = "Orientation";

        // Part Property Names
        private static System.String CPartCategoryName = "Category";
        private static System.String CPartTypeName = "Type";
        private static System.String CPartPackageName = "Package";
        private static System.String CPartDatasheetName = "Datasheet";

        // Manual Property Names
        private static System.String CManualStorageBoxName = "Storage Box";
        private static System.String CManualPrintEditionName = "Print Edition";
        private static System.String CManualConditionName = "Condition";

        private static Common.Data.IDbAdapter s_DbAdapter = null;

        private static Common.Collections.StringSortedList<System.Int32> s_ManufacturerList = new Common.Collections.StringSortedList<System.Int32>();
        private static System.Threading.Mutex s_ManufacturerMutex = new System.Threading.Mutex();

        private static Common.Collections.StringSortedList<System.Int32> s_BoardTypeList = new Common.Collections.StringSortedList<System.Int32>();
        private static System.Threading.Mutex s_BoardTypeMutex = new System.Threading.Mutex();

        private static Common.Collections.StringSortedList<System.Int32> s_BoardPartLocationList = new Common.Collections.StringSortedList<System.Int32>();
        private static System.Threading.Mutex s_BoardPartLocationMutex = new System.Threading.Mutex();

        private static Common.Collections.StringSortedList<System.Int32> s_LogTypeList = new Common.Collections.StringSortedList<System.Int32>();
        private static System.Threading.Mutex s_LogTypeMutex = new System.Threading.Mutex();

        private static System.String[] s_GameDataName = {CGameAudioName,
                                                         CGameWiringName,
                                                         CGameControlsName,
                                                         CGameVideoName,
                                                         CGameCocktailName};
        private static Common.Collections.StringSortedList<System.Int32>[] s_GameDataList = {
            new Common.Collections.StringSortedList<System.Int32>(),
            new Common.Collections.StringSortedList<System.Int32>(),
            new Common.Collections.StringSortedList<System.Int32>(),
            new Common.Collections.StringSortedList<System.Int32>(),
            new Common.Collections.StringSortedList<System.Int32>()};
        private static System.Threading.Mutex[] s_GameDataMutex = {new System.Threading.Mutex(),
                                                                   new System.Threading.Mutex(),
                                                                   new System.Threading.Mutex(),
                                                                   new System.Threading.Mutex(),
                                                                   new System.Threading.Mutex()};
        private static Common.Collections.StringDictionary<System.Int32> s_GamePropertyNameDictionary = new Common.Collections.StringDictionary<System.Int32>();
        private static System.Boolean[] s_GamePropertyDupsAllowed = {false,
                                                                     false,
                                                                     true,
                                                                     false,
                                                                     false};

        private static System.String[] s_DisplayDataName = {CDisplayTypeName,
                                                            CDisplayResolutionName,
                                                            CDisplayColorsName,
                                                            CDisplayOrientationName};
        private static Common.Collections.StringSortedList<System.Int32>[] s_DisplayDataList = {
            new Common.Collections.StringSortedList<System.Int32>(),
            new Common.Collections.StringSortedList<System.Int32>(),
            new Common.Collections.StringSortedList<System.Int32>(),
            new Common.Collections.StringSortedList<System.Int32>()};
        private static System.Threading.Mutex[] s_DisplayDataMutex = {new System.Threading.Mutex(),
                                                                      new System.Threading.Mutex(),
                                                                      new System.Threading.Mutex(),
                                                                      new System.Threading.Mutex()};
        private static Common.Collections.StringDictionary<System.Int32> s_DisplayPropertyNameDictionary = new Common.Collections.StringDictionary<System.Int32>();

        private static System.String[] s_PartDataName = {CPartCategoryName,
                                                         CPartTypeName,
                                                         CPartPackageName};
        private static Common.Collections.StringSortedList<System.Int32>[] s_PartDataList = {
            new Common.Collections.StringSortedList<System.Int32>(),
            new Common.Collections.StringSortedList<System.Int32>(),
            new Common.Collections.StringSortedList<System.Int32>()};
        private static System.Threading.Mutex[] s_PartDataMutex = {new System.Threading.Mutex(),
                                                                   new System.Threading.Mutex(),
                                                                   new System.Threading.Mutex()};
        private static Common.Collections.StringDictionary<System.Int32> s_PartPropertyNameDictionary = new Common.Collections.StringDictionary<System.Int32>();

        private static System.String[] s_ManualDataName = {CManualStorageBoxName,
                                                           CManualPrintEditionName,
                                                           CManualConditionName};
        private static Common.Collections.StringSortedList<System.Int32>[] s_ManualDataList = {
            new Common.Collections.StringSortedList<System.Int32>(),
            new Common.Collections.StringSortedList<System.Int32>(),
            new Common.Collections.StringSortedList<System.Int32>()};
        private static System.Threading.Mutex[] s_ManualDataMutex = {new System.Threading.Mutex(),
                                                                     new System.Threading.Mutex(),
                                                                     new System.Threading.Mutex()};
        private static Common.Collections.StringDictionary<System.Int32> s_ManualPropertyNameDictionary = new Common.Collections.StringDictionary<System.Int32>();

        private static System.Boolean s_bInitialized = false;
        private static System.String s_sDatabasePluginFile = "";

        /// <summary>
        /// Initializes the database.
        /// <param name="sRegistryKey">
        /// A registry path under Local Machine that contains a string
        /// value called "Database" that contains the database file to use.
        /// </param>
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean Init(
            System.String sRegistryKey,
            out System.String sErrorMessage)
        {
            System.Boolean bInitSuccessful = true;
            System.Boolean bSnapshotSupported = false;
            Microsoft.Win32.RegistryKey RegKey;
            System.String sTmpErrorMessage;

            sErrorMessage = "";

            RegKey = Common.Registry.OpenLocalMachineRegKey(sRegistryKey, false);
                        
            if (RegKey == null)
            {
                sErrorMessage = "The Local Machine registry key could not be opened.";

                return false;
            }

            s_sDatabasePluginFile = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                                                           (System.String)RegKey.GetValue("Plugin"));

            s_sDatabasePluginFile += ".dll";

#if USE_DATABASE_PLUGIN
            s_DbAdapter = CreateDatabasePlugin(s_sDatabasePluginFile, out sErrorMessage);
#endif

            if ((string)RegKey.GetValue("Plugin") == "DbAdapterSQLServerNet")
            {
                s_DbAdapter = new Common.Data.DbAdapterSQLServer();
            }
            else if ((string)RegKey.GetValue("Plugin") == "DbAdapterAccessNet")
            {
                s_DbAdapter = new Common.Data.DbAdapterAccess();
            }
            else
            {
                sErrorMessage = "Unrecognized database plugin.";
            }

            if (s_DbAdapter == null)
            {
                RegKey.Close();

                return false;
            }

            if (false == s_DbAdapter.Initialize(RegKey, ref sErrorMessage))
            {
                RegKey.Close();

                return false;
            }

            RegKey.Close();

            if (bInitSuccessful == true &&
                false == s_DbAdapter.GetSnapshotIsolationSupported(ref bSnapshotSupported,
                                                                   ref sErrorMessage))
            {
                sErrorMessage = "Could not check if the database supports snapshot isolation.";

                bInitSuccessful = false;
            }

            if (bInitSuccessful == true &&
                bSnapshotSupported == false)
            {
                sErrorMessage = "Database does not support snapshot isolation.";

                bInitSuccessful = false;
            }

            if (bInitSuccessful == true &&
                false == LoadDatabaseData(out sErrorMessage))
            {
                bInitSuccessful = false;
            }

            if (bInitSuccessful == false)
            {
                sTmpErrorMessage = "";

                s_DbAdapter.Uninitialize(ref sTmpErrorMessage);

                s_DbAdapter = null;

                return false;
            }
            
            s_bInitialized = true;

            return true;
        }

        /// <summary>
        /// Uninitializes the database by performing various clean ups.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean Uninit(
            out System.String sErrorMessage)
        {
            System.Boolean bResult = true;

            sErrorMessage = "";

            if (s_bInitialized == true)
            {
                bResult = s_DbAdapter.Uninitialize(ref sErrorMessage);

                s_DbAdapter = null;

                s_bInitialized = false;
            }

            return bResult;
        }

        /// <summary>
        /// Retrieves the database plugin file being used.
        /// </summary>

        public static System.String GetDatabasePluginFile()
        {
            return s_sDatabasePluginFile;
        }

        /// <summary>
        /// Retrieves the default board part location.
        /// </summary>

        public static System.String GetDefBoardPartLocation()
        {
            return "Top (Parts)";
        }

        /// <summary>
        /// Retrieves the maximum length of the strings in a part.
        /// <param name="PartLens">
        /// On return will contain the maximum lengths.
        /// </param>
        /// </summary>

        public static System.Boolean GetPartMaxLens(
            out DatabaseDefs.TPartLens PartLens)
        {
            System.String sErrorMessage = "";
            System.Int32 nTableTotal = 0;
            System.Collections.Generic.List<Common.Data.DbTableColumn> TableColumnList = new System.Collections.Generic.List<Common.Data.DbTableColumn>();

            PartLens = new DatabaseDefs.TPartLens();

            if (s_DbAdapter.GetTableSchema(CPartTableName,
                                           ref TableColumnList,
                                           ref sErrorMessage))
            {
                foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                {
                    switch (TableColumn.ColumnName)
                    {
                        case "Name":
                            PartLens.nPartNameLen = TableColumn.ColumnLength;
                            break;
                    }
                }

                ++nTableTotal;
            }

            if (s_DbAdapter.GetTableSchema("PartPropertyValue",
                                           ref TableColumnList,
                                           ref sErrorMessage))
            {
                foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                {
                    switch (TableColumn.ColumnName)
                    {
                        case "Name":
                            PartLens.nPartCategoryNameLen = TableColumn.ColumnLength;
                            PartLens.nPartTypeNameLen = TableColumn.ColumnLength;
                            PartLens.nPartPackageNameLen = TableColumn.ColumnLength;
                            PartLens.nPartDatasheetLen = TableColumn.ColumnLength;
                            break;
                    }
                }

                ++nTableTotal;
            }

            if (s_DbAdapter.GetTableSchema(CPartPinoutsTableName,
                                           ref TableColumnList,
                                           ref sErrorMessage))
            {
                foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                {
                    switch (TableColumn.ColumnName)
                    {
                        case "Description":
                            PartLens.nPartPinoutsLen = TableColumn.ColumnLength;
                            break;
                    }
                }

                ++nTableTotal;
            }

            return nTableTotal == 3 ? true : false;
        }

        /// <summary>
        /// Retrieves the maximum length of the strings in a game.
        /// <param name="GameLens">
        /// On return will contain the maximum lengths.
        /// </param>
        /// </summary>

        public static System.Boolean GetGameMaxLens(
            out DatabaseDefs.TGameLens GameLens)
        {
            System.String sErrorMessage = "";
            System.Int32 nTableTotal = 0;
            System.Collections.Generic.List<Common.Data.DbTableColumn> TableColumnList = new System.Collections.Generic.List<Common.Data.DbTableColumn>();

            GameLens = new DatabaseDefs.TGameLens();

            if (s_DbAdapter.GetTableSchema(CGameTableName,
                                           ref TableColumnList,
                                           ref sErrorMessage))
            {
                foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                {
                    switch (TableColumn.ColumnName)
                    {
                        case "Name":
                            GameLens.nGameNameLen = TableColumn.ColumnLength;
                            break;
                        case "Description":
                            GameLens.nGameDescriptionLen = TableColumn.ColumnLength;
                            break;
                        case "Pinouts":
                            GameLens.nGamePinoutsLen = TableColumn.ColumnLength;
                            break;
                        case "DipSwitches":
                            GameLens.nGameDipSwitchesLen = TableColumn.ColumnLength;
                            break;
                    }
                }

                ++nTableTotal;
            }

            if (s_DbAdapter.GetTableSchema(CManufacturerTableName,
                                           ref TableColumnList,
                                           ref sErrorMessage))
            {
                foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                {
                    switch (TableColumn.ColumnName)
                    {
                        case "Name":
                            GameLens.nManufacturerLen = TableColumn.ColumnLength;
                            break;
                    }
                }

                ++nTableTotal;
            }

            if (s_DbAdapter.GetTableSchema("GamePropertyValue",
                                           ref TableColumnList,
                                           ref sErrorMessage))
            {
                foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                {
                    switch (TableColumn.ColumnName)
                    {
                        case "Name":
                            GameLens.nGameWiringHarnessLen = TableColumn.ColumnLength;
                            GameLens.nGameAudioLen = TableColumn.ColumnLength;
                            GameLens.nGameVideoLen = TableColumn.ColumnLength;
                            GameLens.nGameControlLen = TableColumn.ColumnLength;
                            GameLens.nGameCocktailLen = TableColumn.ColumnLength;
                            break;
                    }
                }

                ++nTableTotal;
            }

            return nTableTotal == 3 ? true : false;
        }

        /// <summary>
        /// Retrieves the maximum length of the strings in a board.
        /// <param name="BoardLens">
        /// On return will contain the maximum lengths.
        /// </param>
        /// </summary>

        public static System.Boolean GetBoardMaxLens(
            out DatabaseDefs.TBoardLens BoardLens)
        {
            System.String sErrorMessage = "";
            System.Boolean bResult = false;
            System.Collections.Generic.List<Common.Data.DbTableColumn> TableColumnList = new System.Collections.Generic.List<Common.Data.DbTableColumn>();

            BoardLens = new DatabaseDefs.TBoardLens();

            if (s_DbAdapter.GetTableSchema(CBoardTableName,
                                           ref TableColumnList,
                                           ref sErrorMessage))
            {
                foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                {
                    switch (TableColumn.ColumnName)
                    {
                        case "Name":
                            BoardLens.nBoardNameLen = TableColumn.ColumnLength;
                            break;
                        case "Description":
                            BoardLens.nBoardDescriptionLen = TableColumn.ColumnLength;
                            break;
                        case "Size":
                            BoardLens.nBoardSizeLen = TableColumn.ColumnLength;
                            break;
                    }
                }

                if (s_DbAdapter.GetTableSchema(CBoardTypeTableName,
                                               ref TableColumnList,
                                               ref sErrorMessage))
                {
                    foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                    {
                        switch (TableColumn.ColumnName)
                        {
                            case "Name":
                                BoardLens.nBoardTypeNameLen = TableColumn.ColumnLength;
                                break;
                        }
                    }

                    bResult = true;
                }                
            }

            return bResult;
        }

        /// <summary>
        /// Retrieves the maximum length of the strings in a board part location.
        /// <param name="BoardPartLocationLens">
        /// On return will contain the maximum lengths.
        /// </param>
        /// </summary>

        public static System.Boolean GetBoardPartLocationMaxLens(
            out DatabaseDefs.TBoardPartLocationLens BoardPartLocationLens)
        {
            System.String sErrorMessage = "";
            System.Int32 nTableTotal = 0;
            System.Collections.Generic.List<Common.Data.DbTableColumn> TableColumnList = new System.Collections.Generic.List<Common.Data.DbTableColumn>();

            BoardPartLocationLens = new DatabaseDefs.TBoardPartLocationLens();

            if (s_DbAdapter.GetTableSchema(CBoardPartTableName,
                                           ref TableColumnList,
                                           ref sErrorMessage))
            {
                foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                {
                    switch (TableColumn.ColumnName)
                    {
                        case "Position":
                            BoardPartLocationLens.nBoardPartPositionLen = TableColumn.ColumnLength;

                            ++nTableTotal;
                            break;
                        case "Description":
                            BoardPartLocationLens.nBoardPartDescriptionLen = TableColumn.ColumnLength;

                            ++nTableTotal;
                            break;
                    }
                }
            }

            if (s_DbAdapter.GetTableSchema(CBoardPartLocationTableName,
                                           ref TableColumnList,
                                           ref sErrorMessage))
            {
                foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                {
                    switch (TableColumn.ColumnName)
                    {
                        case "Name":
                            BoardPartLocationLens.nBoardPartLocationLen = TableColumn.ColumnLength;

                            ++nTableTotal;
                            break;
                    }
                }
            }

            return (nTableTotal == 3) ? true : false;
        }

        /// <summary>
        /// Retrieves the maximum length of the strings in a manual.
        /// <param name="ManualLens">
        /// On return will contain the maximum lengths.
        /// </param>
        /// </summary>

        public static System.Boolean GetManualMaxLens(
            out DatabaseDefs.TManualLens ManualLens)
        {
            System.String sErrorMessage = "";
            System.Int32 nTableTotal = 0;
            System.Collections.Generic.List<Common.Data.DbTableColumn> TableColumnList = new System.Collections.Generic.List<Common.Data.DbTableColumn>();

            ManualLens = new DatabaseDefs.TManualLens();

            if (s_DbAdapter.GetTableSchema(CManualTableName,
                                           ref TableColumnList,
                                           ref sErrorMessage))
            {
                foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                {
                    switch (TableColumn.ColumnName)
                    {
                        case "Name":
                            ManualLens.nManualNameLen = TableColumn.ColumnLength;
                            break;
                        case "PartNumber":
                            ManualLens.nManualPartNumberLen = TableColumn.ColumnLength;
                            break;
                        case "Description":
                            ManualLens.nManualDescriptionLen = TableColumn.ColumnLength;
                            break;
                    }
                }

                ++nTableTotal;
            }

            if (s_DbAdapter.GetTableSchema("ManualPropertyValue",
                                           ref TableColumnList,
                                           ref sErrorMessage))
            {
                foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                {
                    switch (TableColumn.ColumnName)
                    {
                        case "Name":
                            ManualLens.nManualPrintEditionLen = TableColumn.ColumnLength;
                            ManualLens.nManualConditionLen = TableColumn.ColumnLength;
                            ManualLens.nManualStorageBoxLen = TableColumn.ColumnLength;
                            break;
                    }
                }

                ++nTableTotal;
            }

            if (s_DbAdapter.GetTableSchema("Manufacturer",
                                           ref TableColumnList,
                                           ref sErrorMessage))
            {
                foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                {
                    switch (TableColumn.ColumnName)
                    {
                        case "Name":
                            ManualLens.nManufacturerLen = TableColumn.ColumnLength;
                            break;
                    }
                }

                ++nTableTotal;
            }

            return nTableTotal == 3 ? true : false;
        }

        /// <summary>
        /// Retrieves the maximum length of the strings in a display.
        /// <param name="DisplayLens">
        /// On return will contain the maximum lengths.
        /// </param>
        /// </summary>

        public static System.Boolean GetDisplayMaxLens(
            out DatabaseDefs.TDisplayLens DisplayLens)
        {
            System.String sErrorMessage = "";
            System.Int32 nTableTotal = 0;
            System.Collections.Generic.List<Common.Data.DbTableColumn> TableColumnList = new System.Collections.Generic.List<Common.Data.DbTableColumn>();

            DisplayLens = new DatabaseDefs.TDisplayLens();

            if (s_DbAdapter.GetTableSchema(CDisplayTableName,
                                           ref TableColumnList,
                                           ref sErrorMessage))
            {
                foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                {
                    switch (TableColumn.ColumnName)
                    {
                        case "Name":
                            DisplayLens.nDisplayNameLen = TableColumn.ColumnLength;
                            break;
                    }
                }

                ++nTableTotal;
            }

            if (s_DbAdapter.GetTableSchema("DisplayPropertyValue",
                                           ref TableColumnList,
                                           ref sErrorMessage))
            {
                foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                {
                    switch (TableColumn.ColumnName)
                    {
                        case "Name":
                            DisplayLens.nDisplayTypeLen = TableColumn.ColumnLength;
                            DisplayLens.nDisplayResolutionLen = TableColumn.ColumnLength;
                            DisplayLens.nDisplayColorsLen = TableColumn.ColumnLength;
                            DisplayLens.nDisplayOrientationLen = TableColumn.ColumnLength;
                            break;
                    }
                }

                ++nTableTotal;
            }

            return nTableTotal == 2 ? true : false;
        }

        /// <summary>
        /// Retrieves the maximum length of the strings in a log.
        /// <param name="LogLens">
        /// On return will contain the maximum lengths.
        /// </param>
        /// </summary>

        public static System.Boolean GetLogMaxLens(
            out DatabaseDefs.TLogLens LogLens)
        {
            System.String sErrorMessage = "";
            System.Boolean bResult = false;
            System.Collections.Generic.List<Common.Data.DbTableColumn> TableColumnList = new System.Collections.Generic.List<Common.Data.DbTableColumn>();

            LogLens = new DatabaseDefs.TLogLens();

            if (s_DbAdapter.GetTableSchema(CLogTypeTableName,
                                           ref TableColumnList,
                                           ref sErrorMessage))
            {
                foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                {
                    switch (TableColumn.ColumnName)
                    {
                        case "Name":
                            LogLens.nLogTypeLen = TableColumn.ColumnLength;
                            break;
                    }
                }

                if (s_DbAdapter.GetTableSchema(CLogTableName,
                                               ref TableColumnList,
                                               ref sErrorMessage))
                {
                    foreach (Common.Data.DbTableColumn TableColumn in TableColumnList)
                    {
                        switch (TableColumn.ColumnName)
                        {
                            case "Description":
                                LogLens.nLogDescriptionLen = TableColumn.ColumnLength;
                                break;
                        }
                    }

                    bResult = true;
                }
            }

            return bResult;
        }

        /// <summary>
        /// Retrieves all of the available manufacturers.
        /// <param name="ManufacturerList">
        /// List of the manufacturers.
        /// </param>
        /// </summary>

        public static System.Boolean GetManufacturerList(
            out Common.Collections.StringSortedList<System.Int32> ManufacturerList)
        {
            ManufacturerList = new Common.Collections.StringSortedList<System.Int32>();

            s_ManufacturerMutex.WaitOne();

            ManufacturerList = s_ManufacturerList.MakeCopy();

            s_ManufacturerMutex.ReleaseMutex();

            return true;
        }

        /// <summary>
        /// Retrieves all of the available board types.
        /// <param name="BoardTypeList">
        /// List of the board types.
        /// </param>
        /// </summary>

        public static System.Boolean GetBoardTypeList(
            out Common.Collections.StringSortedList<System.Int32> BoardTypeList)
        {
            s_BoardTypeMutex.WaitOne();

            BoardTypeList = s_BoardTypeList.MakeCopy();

            s_BoardTypeMutex.ReleaseMutex();

            return true;
        }

        /// <summary>
        /// Retrieves all of the available board part locations.
        /// <param name="BoardTypeList">
        /// List of the board types.
        /// </param>
        /// </summary>

        public static System.Boolean GetBoardPartLocationList(
            out Common.Collections.StringSortedList<System.Int32> BoardPartLocationList)
        {
            s_BoardPartLocationMutex.WaitOne();

            BoardPartLocationList = s_BoardPartLocationList.MakeCopy();

            s_BoardPartLocationMutex.ReleaseMutex();

            return true;
        }

        /// <summary>
        /// Retrieves all of the available log types.
        /// <param name="LogTypeList">
        /// List of the log types.
        /// </param>
        /// </summary>

        public static System.Boolean GetLogTypeList(
            out Common.Collections.StringSortedList<System.Int32> LogTypeList)
        {
            s_LogTypeMutex.WaitOne();

            LogTypeList = s_LogTypeList.MakeCopy();

            s_LogTypeMutex.ReleaseMutex();

            return true;
        }

        /// <summary>
        /// Retrieves all of the available data for the given game data type.
        /// <param name="GameDataList">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetGameCategoryList(
            DatabaseDefs.EGameDataType GameDataType,
            out Common.Collections.StringSortedList<System.Int32> GameDataList)
        {
            System.Int32 nIndex = (System.Int32)GameDataType;

            s_GameDataMutex[nIndex].WaitOne();

            GameDataList = s_GameDataList[nIndex].MakeCopy();

            s_GameDataMutex[nIndex].ReleaseMutex();

            return true;
        }

        /// <summary>
        /// Retrieves all of the available data for the given part data type.
        /// <param name="PartDataList">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetPartCategoryList(
            DatabaseDefs.EPartDataType PartDataType,
            out Common.Collections.StringSortedList<System.Int32> PartDataList)
        {
            System.Int32 nIndex = (System.Int32)PartDataType;

            s_PartDataMutex[nIndex].WaitOne();

            PartDataList = s_PartDataList[nIndex].MakeCopy();

            s_PartDataMutex[nIndex].ReleaseMutex();

            return true;
        }

        /// <summary>
        /// Retrieves all of the available data for the given display data type.
        /// <param name="DisplayDataList">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetDisplayCategoryList(
            DatabaseDefs.EDisplayDataType DisplayDataType,
            out Common.Collections.StringSortedList<System.Int32> DisplayDataList)
        {
            System.Int32 nIndex = (System.Int32)DisplayDataType;

            s_DisplayDataMutex[nIndex].WaitOne();

            DisplayDataList = s_DisplayDataList[nIndex].MakeCopy();

            s_DisplayDataMutex[nIndex].ReleaseMutex();

            return true;
        }

        /// <summary>
        /// Retrieves all of the available data for the given manual data type.
        /// <param name="ManualDataList">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetManualCategoryList(
            DatabaseDefs.EManualDataType ManualDataType,
            out Common.Collections.StringSortedList<System.Int32> ManualDataList)
        {
            System.Int32 nIndex = (System.Int32)ManualDataType;

            s_ManualDataMutex[nIndex].WaitOne();

            ManualDataList = s_ManualDataList[nIndex].MakeCopy();

            s_ManualDataMutex[nIndex].ReleaseMutex();

            return true;
        }

        /// <summary>
        /// Retrieves all of the parts that uses the part category item.
        /// <param name="GameDataList">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetPartsUsingCategoryItemList(
            System.Int32 nCategoryNameId,
            System.String sCategoryValue,
            out System.Collections.Specialized.StringCollection GameColl,
            out System.String sErrorMessage)
        {
            return GetDataForPropertyNameAndValue(
                       CPartTableName, nCategoryNameId, sCategoryValue,
                       out GameColl, out sErrorMessage);
        }

        /// <summary>
        /// Retrieves all of the games that uses the game category item.
        /// <param name="GameDataList">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetGamesUsingCategoryItemList(
            System.Int32 nCategoryNameId,
            System.String sCategoryValue,
            out System.Collections.Specialized.StringCollection GameColl,
            out System.String sErrorMessage)
        {
            return GetDataForPropertyNameAndValue(
                       CGameTableName, nCategoryNameId, sCategoryValue,
                       out GameColl, out sErrorMessage);
        }

        /// <summary>
        /// Retrieves all of the manuals that uses the manual category item.
        /// <param name="GameDataList">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetManualsUsingCategoryItemList(
            System.Int32 nCategoryNameId,
            System.String sCategoryValue,
            out System.Collections.Specialized.StringCollection GameColl,
            out System.String sErrorMessage)
        {
            return GetDataForPropertyNameAndValue(
                       CManualTableName, nCategoryNameId, sCategoryValue,
                       out GameColl, out sErrorMessage);
        }

        /// <summary>
        /// Find the identifier of the given manufacturer.
        /// </summary>

        public static System.Int32 GetManufacturerId(
            System.String sManufacturer)
        {
            System.Int32 nResult;

            s_ManufacturerMutex.WaitOne();

            nResult = GetListDataId(sManufacturer, s_ManufacturerList);

            s_ManufacturerMutex.ReleaseMutex();

            return nResult;
        }

        /// <summary>
        /// Find the identifier of the given board type.
        /// </summary>

        public static System.Int32 GetBoardTypeId(
            System.String sBoardType)
        {
            System.Int32 nResult;

            s_BoardTypeMutex.WaitOne();

            nResult = GetListDataId(sBoardType, s_BoardTypeList);

            s_BoardTypeMutex.ReleaseMutex();

            return nResult;
        }

        /// <summary>
        /// Find the identifier of the given board part location.
        /// </summary>

        public static System.Int32 GetBoardPartLocationId(
            System.String sBoardPartLocation)
        {
            System.Int32 nResult;

            s_BoardPartLocationMutex.WaitOne();

            nResult = GetListDataId(sBoardPartLocation, s_BoardPartLocationList);

            s_BoardPartLocationMutex.ReleaseMutex();

            return nResult;
        }

        /// <summary>
        /// Find the identifier of the given log type.
        /// </summary>

        public static System.Int32 GetLogTypeId(
            System.String sLogType)
        {
            System.Int32 nResult;

            s_LogTypeMutex.WaitOne();

            nResult = GetListDataId(sLogType, s_LogTypeList);

            s_LogTypeMutex.ReleaseMutex();

            return nResult;
        }

        /// <summary>
        /// Find the identifier of the given game data.
        /// </summary>

        public static System.Int32 GetGameDataId(
            DatabaseDefs.EGameDataType GameDataType,
            System.String sGameData)
        {
            System.Int32 nIndex = (System.Int32)GameDataType;
            System.Int32 nResult;

            s_GameDataMutex[nIndex].WaitOne();

            nResult = GetListDataId(sGameData, s_GameDataList[nIndex]);

            s_GameDataMutex[nIndex].ReleaseMutex();

            return nResult;
        }

        /// <summary>
        /// Find the identifier of the given part data.
        /// </summary>

        public static System.Int32 GetPartDataId(
            DatabaseDefs.EPartDataType PartDataType,
            System.String sPartData)
        {
            System.Int32 nIndex = (System.Int32)PartDataType;
            System.Int32 nResult;

            s_PartDataMutex[nIndex].WaitOne();

            nResult = GetListDataId(sPartData,
                                    s_PartDataList[nIndex]);

            s_PartDataMutex[nIndex].ReleaseMutex();

            return nResult;
        }

        /// <summary>
        /// Find the identifier of the given manual data.
        /// </summary>

        public static System.Int32 GetManualDataId(
            DatabaseDefs.EManualDataType ManualDataType,
            System.String sManualData)
        {
            System.Int32 nIndex = (System.Int32)ManualDataType;
            System.Int32 nResult;

            s_ManualDataMutex[nIndex].WaitOne();

            nResult = GetListDataId(sManualData,
                                    s_ManualDataList[nIndex]);

            s_ManualDataMutex[nIndex].ReleaseMutex();

            return nResult;
        }

        /// <summary>
        /// Find the identifier of the given display data.
        /// </summary>

        public static System.Int32 GetDisplayDataId(
            DatabaseDefs.EDisplayDataType DisplayDataType,
            System.String sDisplayData)
        {
            System.Int32 nIndex = (System.Int32)DisplayDataType;
            System.Int32 nResult;

            s_DisplayDataMutex[nIndex].WaitOne();

            nResult = GetListDataId(sDisplayData,
                                    s_DisplayDataList[nIndex]);

            s_DisplayDataMutex[nIndex].ReleaseMutex();

            return nResult;
        }

        /// <summary>
        /// Retrieves whether a game property can have duplicates.
        /// </summary>

        public static System.Boolean GetGamePropertyDupsAllowed(
            DatabaseDefs.EGameDataType GameDataType)
        {
            System.Int32 nIndex = (System.Int32)GameDataType;

            return s_GamePropertyDupsAllowed[nIndex];
        }

        /// <summary>
        /// Retrieves whether a game can have duplicates displays.
        /// </summary>

        public static System.Boolean GetGameDisplayDupsAllowed()
        {
            return true;
        }

        /// <summary>
        /// Adds a new name to the given part data type.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddPartDataName(
            DatabaseDefs.EPartDataType PartDataType,
            System.String sNewPartDataName,
            out System.Int32 nNewPartDataId,
            out System.String sErrorMessage)
        {
            System.Int32 nIndex = (System.Int32)PartDataType;
            System.String sPropertyName = s_PartDataName[nIndex];            
            System.Boolean bResult;
            System.Text.StringBuilder sb;

            s_PartDataMutex[nIndex].WaitOne();

            if (s_PartDataList[nIndex].ContainsKey(sNewPartDataName))
            {
                sb = new System.Text.StringBuilder();

                sb.Append("The part data name \"");
                sb.Append(sNewPartDataName);
                sb.Append("\" already exists.");

                nNewPartDataId = -1;
                sErrorMessage = sb.ToString();

                bResult = false;
            }
            else
            {
                bResult = AddTableProperty(CPartTableName,
                                           sNewPartDataName,
                                           s_PartPropertyNameDictionary[sPropertyName],
                                           ref s_PartDataList[nIndex],
                                           out nNewPartDataId,
                                           out sErrorMessage);
            }                       

            s_PartDataMutex[nIndex].ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Changes the name of an existing part data type.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditPartDataName(
            DatabaseDefs.EPartDataType PartDataType,
            System.Int32 nPartDataId,
            System.String sPartDataName,
            out System.String sErrorMessage)
        {
            System.Int32 nIndex = (System.Int32)PartDataType;
            System.Boolean bResult;

            s_PartDataMutex[nIndex].WaitOne();

            bResult = UpdateTableProperty(CPartTableName, nPartDataId,
                                          sPartDataName,
                                          ref s_PartDataList[nIndex],
                                          out sErrorMessage);

            s_PartDataMutex[nIndex].ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Deletes an existing part data type name.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeletePartDataName(
            DatabaseDefs.EPartDataType PartDataType,
            System.Int32 nPartDataId,
            out System.String sErrorMessage)
        {
            System.Int32 nIndex = (System.Int32)PartDataType;
            System.Boolean bResult;

            s_PartDataMutex[nIndex].WaitOne();

            bResult = DeletePartPropertyValue(nPartDataId,
                                              ref s_PartDataList[nIndex],
                                              out sErrorMessage);

            s_PartDataMutex[nIndex].ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Adds a new name to the board type.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddBoardTypeDataName(
            System.String sNewBoardTypeName,
            out System.Int32 nNewBoardTypeId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult;

            s_BoardTypeMutex.WaitOne();

            bResult = AddNameToTable(CBoardTypeTableName,
                                     sNewBoardTypeName,
                                     ref s_BoardTypeList,
                                     out nNewBoardTypeId,
                                     out sErrorMessage);

            s_BoardTypeMutex.ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Changes the name of an existing board type.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditBoardTypeDataName(
            System.Int32 nBoardTypeId,
            System.String sBoardTypeName,
            out System.String sErrorMessage)
        {
            System.Boolean bResult;

            s_BoardTypeMutex.WaitOne();

            bResult = UpdateNameOfTable(CBoardTypeTableName,
                                        nBoardTypeId, sBoardTypeName,
                                        ref s_BoardTypeList,
                                        out sErrorMessage);

            s_BoardTypeMutex.ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Deletes an existing board type name.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeleteBoardTypeDataName(
            System.Int32 nBoardTypeId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult;

            s_BoardTypeMutex.WaitOne();

            bResult = DeleteNameFromTable(CBoardTypeTableName,
                                          nBoardTypeId,
                                          ref s_BoardTypeList,
                                          out sErrorMessage);

            s_BoardTypeMutex.ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Adds a new name to the board part location.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddBoardPartLocationDataName(
            System.String sNewBoardPartLocationName,
            out System.Int32 nNewBoardPartLocationId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult;

            s_BoardPartLocationMutex.WaitOne();

            bResult = AddNameToTable(CBoardPartLocationTableName,
                                     sNewBoardPartLocationName,
                                     ref s_BoardPartLocationList,
                                     out nNewBoardPartLocationId,
                                     out sErrorMessage);

            s_BoardPartLocationMutex.ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Changes the name of an existing board part location.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditBoardPartLocationDataName(
            System.Int32 nBoardPartLocationId,
            System.String sBoardPartLocationName,
            out System.String sErrorMessage)
        {
            System.Boolean bResult;

            s_BoardPartLocationMutex.WaitOne();

            bResult = UpdateNameOfTable(CBoardPartLocationTableName,
                                        nBoardPartLocationId,
                                        sBoardPartLocationName,
                                        ref s_BoardPartLocationList,
                                        out sErrorMessage);

            s_BoardPartLocationMutex.ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Deletes an existing board part location name.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeleteBoardPartLocationDataName(
            System.Int32 nBoardPartLocationId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult;

            s_BoardPartLocationMutex.WaitOne();

            bResult = DeleteNameFromTable(CBoardPartLocationTableName,
                                          nBoardPartLocationId,
                                          ref s_BoardPartLocationList,
                                          out sErrorMessage);

            s_BoardPartLocationMutex.ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Adds a new name to the log type.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddLogTypeDataName(
            System.String sNewLogTypeName,
            out System.Int32 nNewLogTypeId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult;

            s_LogTypeMutex.WaitOne();

            bResult = AddNameToTable(CLogTypeTableName,
                                     sNewLogTypeName,
                                     ref s_LogTypeList,
                                     out nNewLogTypeId,
                                     out sErrorMessage);

            s_LogTypeMutex.ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Changes the name of an existing log type.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditLogTypeDataName(
            System.Int32 nLogTypeId,
            System.String sLogTypeName,
            out System.String sErrorMessage)
        {
            System.Boolean bResult;

            s_LogTypeMutex.WaitOne();

            bResult = UpdateNameOfTable(CLogTypeTableName,
                                        nLogTypeId,
                                        sLogTypeName,
                                        ref s_LogTypeList,
                                        out sErrorMessage);

            s_LogTypeMutex.ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Deletes an existing log type name.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeleteLogTypeDataName(
            System.Int32 nLogTypeId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult;

            s_LogTypeMutex.WaitOne();

            bResult = DeleteNameFromTable(CLogTypeTableName,
                                          nLogTypeId,
                                          ref s_LogTypeList,
                                          out sErrorMessage);

            s_LogTypeMutex.ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Adds a new name to the given game data type.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddGameDataName(
            DatabaseDefs.EGameDataType GameDataType,
            System.String sNewGameDataName,
            out System.Int32 nNewGameDataId,
            out System.String sErrorMessage)
        {
            System.Int32 nIndex = (System.Int32)GameDataType;
            System.String sPropertyName = s_GameDataName[nIndex];
            System.Boolean bResult;
            System.Text.StringBuilder sb;

            s_GameDataMutex[nIndex].WaitOne();

            if (s_GameDataList[nIndex].ContainsKey(sNewGameDataName))
            {
                sb = new System.Text.StringBuilder();

                sb.Append("The game data name \"");
                sb.Append(sNewGameDataName);
                sb.Append("\" already exists.");

                nNewGameDataId = -1;
                sErrorMessage = sb.ToString();

                bResult = false;
            }
            else
            {
                bResult = AddTableProperty(CGameTableName,
                                           sNewGameDataName,
                                           s_GamePropertyNameDictionary[sPropertyName],
                                           ref s_GameDataList[nIndex],
                                           out nNewGameDataId,
                                           out sErrorMessage);
            }

            s_GameDataMutex[nIndex].ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Changes the name of an existing game data type.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditGameDataName(
            DatabaseDefs.EGameDataType GameDataType,
            System.Int32 nGameDataId,
            System.String sGameDataName,
            out System.String sErrorMessage)
        {
            System.Int32 nIndex = (System.Int32)GameDataType;
            System.Boolean bResult;

            s_GameDataMutex[nIndex].WaitOne();

            bResult = UpdateTableProperty(CGameTableName, nGameDataId,
                                          sGameDataName,
                                          ref s_GameDataList[nIndex],
                                          out sErrorMessage);

            s_GameDataMutex[nIndex].ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Deletes an existing game data type name.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeleteGameDataName(
            DatabaseDefs.EGameDataType GameDataType,
            System.Int32 nGameDataId,
            out System.String sErrorMessage)
        {
            System.Int32 nIndex = (System.Int32)GameDataType;
            System.Boolean bResult;

            s_GameDataMutex[nIndex].WaitOne();

            bResult = DeleteTableProperty(CGameTableName, nGameDataId,
                                          ref s_GameDataList[nIndex],
                                          out sErrorMessage);

            s_GameDataMutex[nIndex].ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Adds a new name to the given manual data type.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddManualDataName(
            DatabaseDefs.EManualDataType ManualDataType,
            System.String sNewManualDataName,
            out System.Int32 nNewManualDataId,
            out System.String sErrorMessage)
        {
            System.Int32 nIndex = (System.Int32)ManualDataType;
            System.String sPropertyName = s_ManualDataName[nIndex];
            System.Boolean bResult;
            System.Text.StringBuilder sb;

            s_ManualDataMutex[nIndex].WaitOne();

            if (s_ManualDataList[nIndex].ContainsKey(sNewManualDataName))
            {
                sb = new System.Text.StringBuilder();

                sb.Append("The manual data name \"");
                sb.Append(sNewManualDataName);
                sb.Append("\" already exists.");

                nNewManualDataId = -1;
                sErrorMessage = sb.ToString();

                bResult = false;
            }
            else
            {
                bResult = AddTableProperty(CManualTableName,
                                           sNewManualDataName,
                                           s_ManualPropertyNameDictionary[sPropertyName],
                                           ref s_ManualDataList[nIndex],
                                           out nNewManualDataId,
                                           out sErrorMessage);
            }

            s_ManualDataMutex[nIndex].ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Changes the name of an existing manual data type.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditManualDataName(
            DatabaseDefs.EManualDataType ManualDataType,
            System.Int32 nManualDataId,
            System.String sManualDataName,
            out System.String sErrorMessage)
        {

            System.Int32 nIndex = (System.Int32)ManualDataType;
            System.Boolean bResult;

            s_ManualDataMutex[nIndex].WaitOne();

            bResult = UpdateTableProperty(CManualTableName, nManualDataId,
                                          sManualDataName,
                                          ref s_ManualDataList[nIndex],
                                          out sErrorMessage);

            s_ManualDataMutex[nIndex].ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Deletes an existing manual data type name.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeleteManualDataName(
            DatabaseDefs.EManualDataType ManualDataType,
            System.Int32 nManualDataId,
            out System.String sErrorMessage)
        {
            System.Int32 nIndex = (System.Int32)ManualDataType;
            System.Boolean bResult;

            s_ManualDataMutex[nIndex].WaitOne();

            bResult = DeleteNameFromTable(s_ManualDataName[nIndex],
                                          nManualDataId,
                                          ref s_ManualDataList[nIndex],
                                          out sErrorMessage);

            s_ManualDataMutex[nIndex].ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Adds a new name to the given display data type.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddDisplayDataName(
            DatabaseDefs.EDisplayDataType DisplayDataType,
            System.String sNewDisplayDataName,
            out System.Int32 nNewDisplayDataId,
            out System.String sErrorMessage)
        {
            System.Int32 nIndex = (System.Int32)DisplayDataType;
            System.String sPropertyName = s_DisplayDataName[nIndex];
            System.Boolean bResult;
            System.Text.StringBuilder sb;

            s_DisplayDataMutex[nIndex].WaitOne();

            if (s_DisplayDataList[nIndex].ContainsKey(sNewDisplayDataName))
            {
                sb = new System.Text.StringBuilder();

                sb.Append("The display data name \"");
                sb.Append(sNewDisplayDataName);
                sb.Append("\" already exists.");

                nNewDisplayDataId = -1;
                sErrorMessage = sb.ToString();

                bResult = false;
            }
            else
            {
                bResult = AddTableProperty(CDisplayTableName,
                                           sNewDisplayDataName,
                                           s_DisplayPropertyNameDictionary[sPropertyName],
                                           ref s_DisplayDataList[nIndex],
                                           out nNewDisplayDataId,
                                           out sErrorMessage);
            }

            s_DisplayDataMutex[nIndex].ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Changes the name of an existing display data type.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditDisplayDataName(
            DatabaseDefs.EDisplayDataType DisplayDataType,
            System.Int32 nDisplayDataId,
            System.String sDisplayDataName,
            out System.String sErrorMessage)
        {
            System.Int32 nIndex = (System.Int32)DisplayDataType;
            System.Boolean bResult;

            s_DisplayDataMutex[nIndex].WaitOne();

            bResult = UpdateTableProperty(CDisplayTableName, nDisplayDataId,
                                          sDisplayDataName,
                                          ref s_DisplayDataList[nIndex],
                                          out sErrorMessage);

            s_DisplayDataMutex[nIndex].ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Deletes an existing display data type name.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeleteDisplayDataName(
            DatabaseDefs.EDisplayDataType DisplayDataType,
            System.Int32 nDisplayDataId,
            out System.String sErrorMessage)
        {
            System.Int32 nIndex = (System.Int32)DisplayDataType;
            System.Boolean bResult;

            s_DisplayDataMutex[nIndex].WaitOne();

            bResult = DeleteTableProperty(CDisplayTableName, nDisplayDataId,
                                          ref s_DisplayDataList[nIndex],
                                          out sErrorMessage);

            s_DisplayDataMutex[nIndex].ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Adds a new name to the given manufacturer data.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddManufacturerName(
            System.String sNewManufacturerName,
            out System.Int32 nNewManufacturerId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult;

            s_ManufacturerMutex.WaitOne();

            bResult = AddNameToTable(CManufacturerTableName,
                                     sNewManufacturerName,
                                     ref s_ManufacturerList,
                                     out nNewManufacturerId,
                                     out sErrorMessage);

            s_ManufacturerMutex.ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Changes the name of an existing manufacturer.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditManufacturerName(
            System.Int32 nManufacturerId,
            System.String sManufacturerName,
            out System.String sErrorMessage)
        {
            System.Boolean bResult;

            s_ManufacturerMutex.WaitOne();

            bResult = UpdateNameOfTable(CManufacturerTableName,
                                        nManufacturerId,
                                        sManufacturerName,
                                        ref s_ManufacturerList,
                                        out sErrorMessage);

            s_ManufacturerMutex.ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Deletes an existing manufacturer name.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeleteManufacturerName(
            System.Int32 nManufacturerId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult;

            s_ManufacturerMutex.WaitOne();

            bResult = DeleteNameFromTable(CManufacturerTableName,
                                          nManufacturerId,
                                          ref s_ManufacturerList,
                                          out sErrorMessage);

            s_ManufacturerMutex.ReleaseMutex();

            return bResult;
        }

        /// <summary>
        /// Retrieves part specific data on a part.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetPartData(
            System.Int32 nPartId,
            ref System.String sPartCategoryName,
            ref System.String sPartTypeName,
            ref System.String sPartPackageName,
            ref System.String sErrorMessage)
        {
            sPartCategoryName = "";
            sPartTypeName = "";
            sPartPackageName = "";
            sErrorMessage = "";

            if (true == GetTablePropertyValue(CPartTableName, nPartId,
                                              CPartCategoryName,
                                              out sPartCategoryName,
                                              out sErrorMessage) &&
                true == GetTablePropertyValue(CPartTableName, nPartId,
                                              CPartTypeName,
                                              out sPartTypeName,
                                              out sErrorMessage) &&
                true == GetTablePropertyValue(CPartTableName, nPartId,
                                              CPartPackageName,
                                              out sPartPackageName,
                                              out sErrorMessage))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Finds all of the parts that match the given keyword.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetPartsMatchingKeyword(
            System.String sKeyword,
            DatabaseDefs.EKeywordMatchingCriteria KeywordMatchingCriteria,
            out System.Collections.Generic.List<DatabaseDefs.TPart> PartList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            DatabaseDefs.TPart Part;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            PartList = new System.Collections.Generic.List<DatabaseDefs.TPart>();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT Part.PartID, Part.Name, Part.IsDefault ");
                sb.Append("FROM Part ");
                sb.Append("WHERE ((Part.Name Like '");

                switch (KeywordMatchingCriteria)
                {
                    case DatabaseDefs.EKeywordMatchingCriteria.Start:
                        sb.Append(sKeyword.Replace("'", "''"));
                        sb.Append("%");
                        break;
                    case DatabaseDefs.EKeywordMatchingCriteria.Anywhere:
                        sb.Append("%");
                        sb.Append(sKeyword.Replace("'", "''"));
                        sb.Append("%");
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false, "Unknown keyword matching criteria");
                        break;
                }
                
                sb.Append("')) ");
                sb.Append("ORDER BY Part.Name;");

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    Part = new DatabaseDefs.TPart();

                    Part.nPartId = DataReader.GetInt32(0);
                    Part.sPartName = DataReader.GetString(1);
                    Part.bPartIsDefault = DataReader.GetBoolean(2);

                    if (false == GetTablePropertyValue(CPartTableName,
                                                       Part.nPartId,
                                                       CPartCategoryName,
                                                       out Part.sPartCategoryName,
                                                       out sErrorMessage) ||
                        false == GetTablePropertyValue(CPartTableName,
                                                       Part.nPartId,
                                                       CPartTypeName,
                                                       out Part.sPartTypeName,
                                                       out sErrorMessage) ||
                        false == GetTablePropertyValue(CPartTableName,
                                                       Part.nPartId,
                                                       CPartPackageName,
                                                       out Part.sPartPackageName,
                                                       out sErrorMessage) ||
                        false == GetTablePropertyValues(CPartTableName,
                                                        Part.nPartId,
                                                        CPartDatasheetName,
                                                        out Part.PartDatasheetColl,
                                                        out sErrorMessage))
                    {
                        DataReader.Close();

                        s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

                        PartList.Clear();

                        return false;
                    }

                    PartList.Add(Part);
                }

                DataReader.Close();

                PartList.Sort(new PartComparer());

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Finds all of the parts that match the given type.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetPartsMatchingType(
            System.String sPartType,
            out System.Collections.Generic.List<DatabaseDefs.TPart> PartList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            DatabaseDefs.TPart Part;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            PartList = new System.Collections.Generic.List<DatabaseDefs.TPart>();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT Part.PartID, Part.Name, Part.IsDefault ");
                sb.Append("FROM PartPropertyValue INNER JOIN ");
                sb.Append("     (PartPropertyName INNER JOIN ");
                sb.Append("     (Part INNER JOIN (PartProperty INNER JOIN ");
                sb.Append("     PartPartProperty ON ");
                sb.Append("     PartProperty.PartPropertyID = PartPartProperty.PartPropertyID) ");
                sb.Append("     ON Part.PartID = PartPartProperty.PartID) ON ");
                sb.Append("     PartPropertyName.PartPropertyNameID = PartProperty.PartPropertyNameID) ");
                sb.Append("     ON PartPropertyValue.PartPropertyValueID = PartProperty.PartPropertyValueID ");
                sb.Append("WHERE ((Part.IsDefault = ");
                sb.Append(s_DbAdapter.GetSQLBooleanValue(true));
                sb.Append(") AND ");
                sb.Append("       (PartPropertyName.Name = ?) AND ");
                sb.Append("       (PartPropertyValue.Name = ?)) ");
                sb.Append("ORDER BY Part.Name;");

                s_DbAdapter.AddCommandParameter(Command, "@PartTypeName", CPartTypeName);
                s_DbAdapter.AddCommandParameter(Command, "@PartType", sPartType);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    Part = new DatabaseDefs.TPart();

                    Part.nPartId = DataReader.GetInt32(0);
                    Part.sPartName = DataReader.GetString(1);
                    Part.bPartIsDefault = DataReader.GetBoolean(2);
                    Part.sPartTypeName = sPartType;

                    if (false == GetTablePropertyValue(CPartTableName,
                                                       Part.nPartId,
                                                       CPartCategoryName,
                                                       out Part.sPartCategoryName,
                                                       out sErrorMessage) ||
                        false == GetTablePropertyValue(CPartTableName,
                                                       Part.nPartId,
                                                       CPartPackageName,
                                                       out Part.sPartPackageName,
                                                       out sErrorMessage) ||
                        false == GetTablePropertyValues(CPartTableName,
                                                        Part.nPartId,
                                                        CPartDatasheetName,
                                                        out Part.PartDatasheetColl,
                                                        out sErrorMessage))
                    {
                        DataReader.Close();

                        s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

                        PartList.Clear();

                        return false;
                    }

                    PartList.Add(Part);
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Retrieves parts that have the same pinouts as the given part.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetPartsWithSamePinouts(
            System.Int32 nPartId,
            out System.Collections.Generic.List<DatabaseDefs.TPart> PartList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Int32 nPartPinoutsId;
            System.String sPinouts;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;
            DatabaseDefs.TPart Part;

            PartList = new System.Collections.Generic.List<DatabaseDefs.TPart>();
            sErrorMessage = "";

            if (false == GetPartPinouts(nPartId, out nPartPinoutsId, out sPinouts,
                                        out sErrorMessage))
            {
                return false;
            }

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT Part.PartID, ");
                sb.Append("       Part.Name, ");
                sb.Append("       Part.IsDefault ");
                sb.Append("FROM Part ");
                sb.Append("WHERE Part.PartPinoutsID = ? ");
                sb.Append("ORDER BY Part.Name;");

                s_DbAdapter.AddCommandParameter(Command, "@PartPinoutsID", nPartPinoutsId);
 
                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    Part = new DatabaseDefs.TPart();

                    Part.nPartId = DataReader.GetInt32(0);
                    Part.sPartName = DataReader.GetString(1);
                    Part.bPartIsDefault = DataReader.GetBoolean(2);

                    GetTablePropertyValue(CPartTableName, Part.nPartId,
                                          CPartCategoryName,
                                          out Part.sPartCategoryName,
                                          out sErrorMessage);

                    GetTablePropertyValue(CPartTableName, Part.nPartId,
                                          CPartTypeName,
                                          out Part.sPartTypeName,
                                          out sErrorMessage);

                    GetTablePropertyValue(CPartTableName, Part.nPartId,
                                          CPartPackageName,
                                          out Part.sPartPackageName,
                                          out sErrorMessage);

                    GetTablePropertyValues(CPartTableName, Part.nPartId,
                                           CPartDatasheetName,
                                           out Part.PartDatasheetColl,
                                           out sErrorMessage);

                    PartList.Add(Part);
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Retrieves the pinouts for the given part.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetPartPinouts(
            System.Int32 nPartId,
            out System.Int32 nPartPinoutsId,
            out System.String sPartPinouts,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            nPartPinoutsId = -1;
            sPartPinouts = "";
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT PartPinouts.PartPinoutsID, PartPinouts.Description ");
                sb.Append("FROM PartPinouts INNER JOIN Part ON PartPinouts.PartPinoutsID = Part.PartPinoutsID ");
                sb.Append("WHERE Part.PartID = ?;");

                s_DbAdapter.AddCommandParameter(Command, "@PartID", nPartId);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                if (DataReader.Read())
                {
                    nPartPinoutsId = DataReader.GetInt32(0);

                    if (DataReader.IsDBNull(1) == false)
                    {
                        sPartPinouts = DataReader.GetString(1);
                    }
                    else
                    {
                        sPartPinouts = "";
                    }

                    bResult = true;
                }
                else
                {
                    sErrorMessage = "No pinouts found for the given part.  (Does this part exist?)";
                }

                DataReader.Close();
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Adds a new part group.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddPartGroup(
            System.String sPartName,
            System.String sPartCategory,
            System.String sPartType,
            System.String sPartPackage,
            System.String sPartPinouts,
            System.Collections.Specialized.StringCollection PartDatasheetColl,
            out System.Int32 nNewPartId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.Int32 nNewPartPinoutsId = -1;
            System.String sTmpErrorMessage = null;

            nNewPartId = -1;
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == AddPartPinouts(Command, sPartPinouts,
                                           out nNewPartPinoutsId, out sErrorMessage) &&
                    true == AddPart(Command, sPartName, nNewPartPinoutsId, true,
                                    out nNewPartId, out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CPartTableName,
                                                  nNewPartId,
                                                  GetPartDataId(DatabaseDefs.EPartDataType.Category,
                                                                sPartCategory),
                                                  out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CPartTableName,
                                                  nNewPartId,
                                                  GetPartDataId(DatabaseDefs.EPartDataType.Type,
                                                                sPartType),
                                                  out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CPartTableName,
                                                  nNewPartId,
                                                  GetPartDataId(DatabaseDefs.EPartDataType.Package,
                                                                sPartPackage),
                                                  out sErrorMessage) &&
                    true == AddPartDatasheets(Command, nNewPartId,
                                              PartDatasheetColl,
                                              out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Adds a new part to a the cross reference of equivalent parts.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddPart(
            System.Int32 nPartId,
            System.String sPartName,
            System.String sPartPackage,
            System.Collections.Specialized.StringCollection PartDatasheetColl,
            out System.Int32 nNewPartId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.Boolean bExists = false;
            System.Int32 nPartPinoutsId;
            System.String sPartCategory;
            System.String sPartType;
            System.String sPartPinouts;
            System.String sTmpErrorMessage = null;

            nNewPartId = -1;
            sErrorMessage = "";

            if (false == GetTablePropertyValue(CPartTableName, nPartId,
                                               CPartCategoryName,
                                               out sPartCategory,
                                               out sErrorMessage) ||
                false == GetTablePropertyValue(CPartTableName, nPartId,
                                               CPartTypeName,
                                               out sPartType,
                                               out sErrorMessage) ||
                false == GetPartPinouts(nPartId, out nPartPinoutsId,
                                        out sPartPinouts, out sErrorMessage) ||
                false == DoesPartNameExist(nPartPinoutsId, sPartName, 
                                           out bExists, out sErrorMessage))
            {
                return false;
            }

            if (bExists == true)
            {
                sErrorMessage = "This part name already exists.";

                return false;
            }

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == AddPart(Command, sPartName, nPartPinoutsId, false,
                                    out nNewPartId, out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CPartTableName,
                                                  nNewPartId,
                                                  GetPartDataId(DatabaseDefs.EPartDataType.Category,
                                                                sPartCategory),
                                                  out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CPartTableName,
                                                  nNewPartId,
                                                  GetPartDataId(DatabaseDefs.EPartDataType.Type,
                                                                sPartType),
                                                  out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CPartTableName,
                                                  nNewPartId,
                                                  GetPartDataId(DatabaseDefs.EPartDataType.Package,
                                                                sPartPackage),
                                                  out sErrorMessage) &&
                    true == AddPartDatasheets(Command, nNewPartId,
                                              PartDatasheetColl,
                                              out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Changes the data associated with a part.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditPart(
            System.Int32 nPartId,
            System.String sPartName,
            System.String sPartCategory,
            System.String sPartType,
            System.String sPartPackage,
            System.String sPartPinouts,
            System.Boolean bIsDefaultPart,
            Common.Collections.StringCollection PartDatasheetColl,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;
            System.Int32 nPartPinoutsId, nDefPartId;
            System.String sTmpPinouts, sOriginalPartCategory;
            System.String sOriginalPartType, sOriginalPartPackage;

            sErrorMessage = "";

            if (false == GetPartPinouts(nPartId, out nPartPinoutsId,
                                        out sTmpPinouts, out sErrorMessage) ||
                false == GetDefaultPart(nPartId, out nDefPartId,
                                        out sErrorMessage) ||
                false == GetTablePropertyValue(CPartTableName, nPartId,
                                               CPartCategoryName,
                                               out sOriginalPartCategory,
                                               out sErrorMessage) ||
                false == GetTablePropertyValue(CPartTableName, nPartId,
                                               CPartTypeName,
                                               out sOriginalPartType,
                                               out sErrorMessage) ||
                false == GetTablePropertyValue(CPartTableName, nPartId,
                                               CPartPackageName,
                                               out sOriginalPartPackage,
                                               out sErrorMessage))
            {
                return false;
            }

            if (nPartId == nDefPartId && bIsDefaultPart == false)
            {
                sErrorMessage = "This is the default part and it cannot be changed.";

                return false;
            }

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == UpdatePartPinouts(Command, nPartPinoutsId, sPartPinouts,
                                              out sErrorMessage) &&
                    true == UpdatePart(Command, nPartId, sPartName, bIsDefaultPart,
                                       out sErrorMessage) &&
                    true == UpdateTableTableProperty(Command, CPartTableName,
                                nPartId,
                                GetPartDataId(DatabaseDefs.EPartDataType.Category,
                                              sOriginalPartCategory),
                                GetPartDataId(DatabaseDefs.EPartDataType.Category,
                                              sPartCategory),
                                out sErrorMessage) &&
                    true == UpdateTableTableProperty(Command, CPartTableName,
                                nPartId,
                                GetPartDataId(DatabaseDefs.EPartDataType.Type,
                                              sOriginalPartType),
                                GetPartDataId(DatabaseDefs.EPartDataType.Type,
                                              sPartType),
                                out sErrorMessage) &&
                    true == UpdateTableTableProperty(Command, CPartTableName,
                                nPartId,
                                GetPartDataId(DatabaseDefs.EPartDataType.Package,
                                              sOriginalPartPackage),
                                GetPartDataId(DatabaseDefs.EPartDataType.Package,
                                              sPartPackage),
                                out sErrorMessage) &&
                    true == UpdatePartDatasheets(Command, nPartId,
                                                 PartDatasheetColl,
                                                 out sErrorMessage))
                {
                    if (bIsDefaultPart == true && nDefPartId != nPartId)
                    {
                        if (true == UpdateIsDefaultPartFlag(Command, nDefPartId,
                                                            false,
                                                            out sErrorMessage))
                        {
                            Transaction.Commit();

                            bResult = true;
                        }
                        else
                        {
                            Transaction.Rollback();
                        }
                    }
                    else
                    {
                        Transaction.Commit();

                        bResult = true;
                    }
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Changes the data associated with a part.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeletePart(
            System.Int32 nPartId,
            out System.Int32 nNewDefPartId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;
            System.String sPartPinouts;
            System.Int32 nPartPinoutsId, nDefPartId, nNextPartId;

            nNewDefPartId = -1;
            sErrorMessage = "";

            if (false == GetPartPinouts(nPartId, out nPartPinoutsId,
                                        out sPartPinouts, out sErrorMessage) ||
                false == GetFirstAvailablePart(nPartId, nPartPinoutsId,
                                               out nNextPartId, out sErrorMessage) ||
                false == GetDefaultPart(nPartId, out nDefPartId,
                                        out sErrorMessage))
            {
                return false;
            }

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == DeletePartDatasheets(Command, nPartId,
                                                 out sErrorMessage) &&
                    true == DeleteTableTableProperties(Command, CPartTableName,
                                                       nPartId,
                                                       out sErrorMessage) &&
                    true == DeleteNameFromTable(Command, CPartTableName,
                                                nPartId, out sErrorMessage) &&
                    true == DeleteUnusedProperties(Command, CPartTableName,
                                                   out sErrorMessage))
                {
                    if (nNextPartId == -1)
                    {
                        if (true == DeleteNameFromTable(Command,
                                                        CPartPinoutsTableName,
                                                        nPartPinoutsId,
                                                        out sErrorMessage))
                        {
                            Transaction.Commit();

                            bResult = true;
                        }
                        else
                        {
                            Transaction.Rollback();
                        }
                    }
                    else if (nPartId == nDefPartId)
                    {
                        if (true == UpdateIsDefaultPartFlag(Command, nNextPartId,
                                                            true, out sErrorMessage))
                        {
                            Transaction.Commit();

                            nNewDefPartId = nNextPartId;

                            bResult = true;
                        }
                        else
                        {
                            Transaction.Rollback();
                        }
                    }
                    else 
                    {
                        Transaction.Commit();

                        nNewDefPartId = nDefPartId;

                        bResult = true;
                    }
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);
            
            return bResult;
        }
        
        /// <summary>
        /// Finds the games that use the given part.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetGamesWithPart(
            System.Int32 nPartId,
            out System.Collections.Generic.List<DatabaseDefs.TGame> GamesList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            DatabaseDefs.TGame Game;
            System.String sTmpErrorMessage = null;

            GamesList = new System.Collections.Generic.List<DatabaseDefs.TGame>();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT DISTINCT Game.GameID, ");
                sb.Append("                Game.Name, ");
                sb.Append("                Game.Description, ");
                sb.Append("                Game.Pinouts, ");
                sb.Append("                Game.DipSwitches, ");
                sb.Append("                Game.HaveWiringHarness, ");
                sb.Append("                Game.NeedPowerOnReset ");
                sb.Append("FROM Game INNER JOIN ((Board INNER JOIN BoardPart ");
                sb.Append("    ON Board.BoardID = BoardPart.BoardID) INNER JOIN ");
                sb.Append("    GameBoard ON Board.BoardID = GameBoard.BoardID) ON ");
                sb.Append("    Game.GameID = GameBoard.GameID ");
                sb.Append("WHERE BoardPart.PartID = ? ");
                sb.Append("ORDER BY Game.Name;");

                s_DbAdapter.AddCommandParameter(Command, "@PartID", nPartId);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    Game = new DatabaseDefs.TGame();

                    Game.nGameId = DataReader.GetInt32(0);
                    Game.sGameName = DataReader.GetString(1);

                    if (DataReader.IsDBNull(3) == false)
                    {
                        Game.sGamePinouts = DataReader.GetString(3);
                    }
                    else
                    {
                        Game.sGamePinouts = "";
                    }

                    if (DataReader.IsDBNull(4) == false)
                    {
                        Game.sGameDipSwitches = DataReader.GetString(4);
                    }
                    else
                    {
                        Game.sGameDipSwitches = "";
                    }

                    if (DataReader.IsDBNull(2) == false)
                    {
                        Game.sGameDescription = DataReader.GetString(2);
                    }
                    else
                    {
                        Game.sGameDescription = "";
                    }

                    if (DataReader.IsDBNull(5) == false)
                    {
                        Game.bGameHaveWiringHarness = DataReader.GetBoolean(5);
                    }
                    else
                    {
                        Game.bGameHaveWiringHarness = false;
                    }

                    if (DataReader.IsDBNull(6) == false)
                    {
                        Game.bGameNeedPowerOnReset = DataReader.GetBoolean(6);
                    }
                    else
                    {
                        Game.bGameNeedPowerOnReset = false;
                    }
                    
                    GamesList.Add(Game);
                }
                
                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Finds all of the games that use the given part and it's matching variants.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetGamesWithPartIncludeAllMatchingParts(
            System.Int32 nPartId,
            out System.Collections.Generic.List<DatabaseDefs.TGame> GamesList,
            out System.String sErrorMessage)
        {
            System.Collections.Generic.List<DatabaseDefs.TPart> TmpPartList;
            System.Collections.Generic.List<DatabaseDefs.TGame> TmpGamesList;

            GamesList = new System.Collections.Generic.List<DatabaseDefs.TGame>();
            sErrorMessage = "";

            if (false == GetPartsWithSamePinouts(nPartId, out TmpPartList,
                                                 out sErrorMessage))
            {
                return false;
            }

            foreach (DatabaseDefs.TPart Part in TmpPartList)
            {
                if (false == GetGamesWithPart(Part.nPartId,
                                              out TmpGamesList,
                                              out sErrorMessage))
                {
                    GamesList.Clear();

                    return false;
                }

                foreach (DatabaseDefs.TGame Game in TmpGamesList)
                {
                    if (false == DoesGameAlreadyExist(Game.nGameId,
                                                      GamesList))
                    {
                        GamesList.Add(Game);
                    }
                }
            }

            GamesList.Sort(new GameComparer());

            return true;
        }

        /// <summary>
        /// Gets all of the games in the database.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetGames(
            out System.Collections.Generic.List<DatabaseDefs.TGame> GamesList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Boolean bQuit = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;
            Common.Collections.StringCollection WiringHarnessColl;
            Common.Collections.StringCollection CocktailColl;

            GamesList = new System.Collections.Generic.List<DatabaseDefs.TGame>();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT Game.GameID, ");
                sb.Append("       Game.Name, ");
                sb.Append("       Game.Description, ");
                sb.Append("       Game.Pinouts, ");
                sb.Append("       Game.DipSwitches, ");
                sb.Append("       Game.HaveWiringHarness, ");
                sb.Append("       Game.NeedPowerOnReset, ");
                sb.Append("       Manufacturer.Name ");
                sb.Append("FROM Manufacturer INNER JOIN Game ON ");
                sb.Append("     Manufacturer.ManufacturerID = Game.ManufacturerID ");
                sb.Append("ORDER BY Game.Name;");

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (bQuit == false && DataReader.Read())
                {
                    DatabaseDefs.TGame Game = new DatabaseDefs.TGame();

                    Game.nGameId = DataReader.GetInt32(0);
                    Game.sGameName = DataReader.GetString(1);

                    if (DataReader.IsDBNull(2) == false)
                    {
                        Game.sGameDescription = DataReader.GetString(2);
                    }
                    else
                    {
                        Game.sGameDescription = "";
                    }

                    if (DataReader.IsDBNull(3) == false)
                    {
                        Game.sGamePinouts = DataReader.GetString(3);
                    }
                    else
                    {
                        Game.sGamePinouts = "";
                    }

                    if (DataReader.IsDBNull(4) == false)
                    {
                        Game.sGameDipSwitches = DataReader.GetString(4);
                    }
                    else
                    {
                        Game.sGameDipSwitches = "";
                    }

                    if (DataReader.IsDBNull(5) == false)
                    {
                        Game.bGameHaveWiringHarness = DataReader.GetBoolean(5);
                    }
                    else
                    {
                        Game.bGameHaveWiringHarness = false;
                    }

                    if (DataReader.IsDBNull(6) == false)
                    {
                        Game.bGameNeedPowerOnReset = DataReader.GetBoolean(6);
                    }
                    else
                    {
                        Game.bGameNeedPowerOnReset = false;
                    }

                    Game.sManufacturer = DataReader.GetString(7);

                    WiringHarnessColl = null;
                    CocktailColl = null;

                    if (true == GetTableProperties(CGameTableName,
                                                   Game.nGameId,
                                                   s_GamePropertyNameDictionary[CGameControlsName],
                                                   out Game.GameControlsColl,
                                                   out sErrorMessage) &&
                        true == GetTableProperties(CGameTableName,
                                                   Game.nGameId,
                                                   s_GamePropertyNameDictionary[CGameVideoName],
                                                   out Game.GameVideoColl,
                                                   out sErrorMessage) &&
                        true == GetTableProperties(CGameTableName,
                                                   Game.nGameId,
                                                   s_GamePropertyNameDictionary[CGameAudioName],
                                                   out Game.GameAudioColl,
                                                   out sErrorMessage) &&
                        true == GetTableProperties(CGameTableName,
                                                   Game.nGameId,
                                                   s_GamePropertyNameDictionary[CGameWiringName],
                                                   out WiringHarnessColl,
                                                   out sErrorMessage) &&
                        true == GetTableProperties(CGameTableName,
                                                   Game.nGameId,
                                                   s_GamePropertyNameDictionary[CGameCocktailName],
                                                   out CocktailColl,
                                                   out sErrorMessage) &&
                        WiringHarnessColl.Count == 1 &&
                        CocktailColl.Count == 1)
                    {
                        Game.sGameWiringHarness = WiringHarnessColl[0];
                        Game.sGameCocktail = CocktailColl[0];

                        GamesList.Add(Game);
                    }
                    else
                    {
                        if (sErrorMessage.Length == 0)
                        {
                            if (WiringHarnessColl != null &&
                                WiringHarnessColl.Count != 1)
                            {
                                sb.Length = 0;

                                sb.Append("The Game \"");
                                sb.Append(Game.sGameName);
                                sb.Append("\" has ");
                                sb.Append(WiringHarnessColl.Count);
                                sb.Append(" wiring harnesses.  ");
                                sb.Append("(The game should have only one.)");

                                sErrorMessage = sb.ToString();
                            }
                            else if (CocktailColl != null &&
                                     CocktailColl.Count != 1)
                            {
                                sb.Length = 0;

                                sb.Append("The Game \"");
                                sb.Append(Game.sGameName);
                                sb.Append("\" has ");
                                sb.Append(CocktailColl.Count);
                                sb.Append(" cocktails.  ");
                                sb.Append("(The game should have only one.)");

                                sErrorMessage = sb.ToString();
                            }
                        }
                       
                        GamesList.Clear();

                        bQuit = true;
                    }
                }

                DataReader.Close();

                if (bQuit == false)
                {
                    bResult = true;
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Finds the displays that make up the given game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetDisplaysForGame(
            System.Int32 nGameId,
            out System.Collections.Generic.List<DatabaseDefs.TDisplay> DisplaysList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage =null;

            DisplaysList = new System.Collections.Generic.List<DatabaseDefs.TDisplay>();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT Display.DisplayID, ");
                sb.Append("       Display.Name ");
                sb.Append("FROM Display INNER JOIN GameDisplay ON ");
                sb.Append("     Display.DisplayID = GameDisplay.DisplayID ");
                sb.Append("WHERE (GameDisplay.GameID = ?) ");
                sb.Append("ORDER BY Display.Name;");

                s_DbAdapter.AddCommandParameter(Command, "@GameID", nGameId);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    DatabaseDefs.TDisplay Display = new DatabaseDefs.TDisplay();

                    Display.nDisplayId = DataReader.GetInt32(0);
                    Display.sDisplayName = DataReader.GetString(1);

                    if (GetTablePropertyValue(CDisplayTableName,
                                              Display.nDisplayId,
                                              CDisplayTypeName,
                                              out Display.sDisplayType,
                                              out sErrorMessage) &&
                        GetTablePropertyValue(CDisplayTableName,
                                              Display.nDisplayId,
                                              CDisplayResolutionName,
                                              out Display.sDisplayResolution,
                                              out sErrorMessage) &&
                        GetTablePropertyValue(CDisplayTableName,
                                              Display.nDisplayId,
                                              CDisplayColorsName,
                                              out Display.sDisplayColors,
                                              out sErrorMessage) &&
                        GetTablePropertyValue(CDisplayTableName,
                                              Display.nDisplayId,
                                              CDisplayOrientationName,
                                              out Display.sDisplayOrientation,
                                              out sErrorMessage))
                    {
                        DisplaysList.Add(Display);
                    }
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Finds the boards that make up the given game and also the cartridges it uses.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetBoardsForGame(
            System.Int32 nGameId,
            out System.Collections.Generic.List<DatabaseDefs.TBoard> BoardsList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            BoardsList = new System.Collections.Generic.List<DatabaseDefs.TBoard>();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT Board.BoardID, ");
                sb.Append("       Board.Name as BoardName, ");
                sb.Append("       Board.Description as BoardDescription, ");
                sb.Append("       Board.[Size] as BoardSize, ");
                sb.Append("       BoardType.Name as BoardTypeName ");
                sb.Append("FROM BoardType INNER JOIN (Board INNER JOIN ");
                sb.Append("    GameBoard ON Board.BoardID = GameBoard.BoardID) ");
                sb.Append("    ON BoardType.BoardTypeID = Board.BoardTypeID ");
                sb.Append("WHERE GameBoard.GameID = ? ");
                sb.Append("ORDER BY BoardType.Name, Board.Name;");

                s_DbAdapter.AddCommandParameter(Command, "@GameID", nGameId);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    DatabaseDefs.TBoard Board = new DatabaseDefs.TBoard();

                    Board.nBoardId = DataReader.GetInt32(0);

                    if (DataReader.IsDBNull(1) == false)
                    {
                        Board.sBoardName = DataReader.GetString(1);
                    }
                    else
                    {
                        Board.sBoardName = "";
                    }

                    if (DataReader.IsDBNull(2) == false)
                    {
                        Board.sBoardDescription = DataReader.GetString(2);
                    }
                    else
                    {
                        Board.sBoardDescription = "";
                    }

                    if (DataReader.IsDBNull(3) == false)
                    {
                        Board.sBoardSize = DataReader.GetString(3);
                    }
                    else
                    {
                        Board.sBoardSize = "";
                    }

                    Board.sBoardTypeName = DataReader.GetString(4);

                    BoardsList.Add(Board);
                }
                
                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Finds the boards that use the given part.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetBoardsWithPart(
            System.Int32 nGameId,
            System.Int32 nPartId,
            out System.Collections.Generic.List<DatabaseDefs.TBoard> BoardsList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;
            DatabaseDefs.TBoard Board;

            BoardsList = new System.Collections.Generic.List<DatabaseDefs.TBoard>();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT DISTINCT Board.BoardID, ");
                sb.Append("       Board.Name AS BoardName, ");
                sb.Append("       Board.Description AS BoardDescription, ");
                sb.Append("       Board.[Size] AS BoardSize, ");
                sb.Append("       BoardType.Name AS BoardTypeName ");
                sb.Append("FROM (BoardType INNER JOIN (Board INNER JOIN GameBoard ");
                sb.Append("     ON Board.BoardID = GameBoard.BoardID) ON ");
                sb.Append("     BoardType.BoardTypeID = Board.BoardTypeID) ");
                sb.Append("     INNER JOIN BoardPart ON Board.BoardID = BoardPart.BoardID ");
                sb.Append("WHERE (GameBoard.GameID = ?) AND ");
                sb.Append("      (BoardPart.PartID = ?);");

                s_DbAdapter.AddCommandParameter(Command, "@GameID", nGameId);
                s_DbAdapter.AddCommandParameter(Command, "@PartID", nPartId);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    Board = new DatabaseDefs.TBoard();

                    Board.nBoardId = DataReader.GetInt32(0);

                    if (DataReader.IsDBNull(1) == false)
                    {
                        Board.sBoardName = DataReader.GetString(1);
                    }
                    else
                    {
                        Board.sBoardName = "";
                    }

                    if (DataReader.IsDBNull(2) == false)
                    {
                        Board.sBoardDescription = DataReader.GetString(2);
                    }
                    else
                    {
                        Board.sBoardDescription = "";
                    }

                    if (DataReader.IsDBNull(3) == false)
                    {
                        Board.sBoardSize = DataReader.GetString(3);
                    }
                    else
                    {
                        Board.sBoardSize = "";
                    }

                    Board.sBoardTypeName = DataReader.GetString(4);

                    BoardsList.Add(Board);
                }
                
                DataReader.Close();

                BoardsList.Sort(new BoardComparer());

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Finds the boards that use the given part and it's matching parts.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetBoardsWithPartIncludeAllMatchingParts(
            System.Int32 nGameId,
            System.Int32 nPartId,
            out System.Collections.Generic.List<DatabaseDefs.TBoard> BoardsList,
            out System.String sErrorMessage)
        {
            System.Collections.Generic.List<DatabaseDefs.TPart> TmpPartList;
            System.Collections.Generic.List<DatabaseDefs.TBoard> TmpBoardsList;

            BoardsList = new System.Collections.Generic.List<DatabaseDefs.TBoard>();
            sErrorMessage = "";

            if (false == GetPartsWithSamePinouts(nPartId, out TmpPartList,
                                                 out sErrorMessage))
            {
                return false;
            }

            foreach (DatabaseDefs.TPart Part in TmpPartList)
            {
                if (false == GetBoardsWithPart(nGameId, Part.nPartId,
                                               out TmpBoardsList,
                                               out sErrorMessage))
                {
                    BoardsList.Clear();

                    return false;
                }

                foreach (DatabaseDefs.TBoard Board in TmpBoardsList)
                {
                    if (false == DoesBoardAlreadyExist(Board.nBoardId,
                                                       BoardsList))
                    {
                        BoardsList.Add(Board);
                    }
                }
            }

            BoardsList.Sort(new BoardComparer());

            return true;
        }

        /// <summary>
        /// Finds the location(s) for a part on the given board.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetBoardPartLocations(
            System.Int32 nBoardId,
            System.Int32 nPartId,
            out System.Collections.Generic.List<DatabaseDefs.TBoardPartLocation> LocationsList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;
            DatabaseDefs.TBoardPartLocation Location;

            LocationsList = new System.Collections.Generic.List<DatabaseDefs.TBoardPartLocation>();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT BoardPart.BoardPartID, ");
                sb.Append("       BoardPart.[Position], ");
                sb.Append("       BoardPartLocation.Name, ");
                sb.Append("       BoardPart.Description ");
                sb.Append("FROM BoardPartLocation INNER JOIN BoardPart ON ");
                sb.Append("    BoardPartLocation.BoardPartLocationID = BoardPart.BoardPartLocationID ");
                sb.Append("WHERE (BoardPart.BoardID = ?) AND ");
                sb.Append("      (BoardPart.PartID = ?);");

                s_DbAdapter.AddCommandParameter(Command, "@BoardID", nBoardId);
                s_DbAdapter.AddCommandParameter(Command, "@PartID", nPartId);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    Location = new DatabaseDefs.TBoardPartLocation();

                    Location.nBoardPartId = DataReader.GetInt32(0);

                    if (DataReader.IsDBNull(1) == false)
                    {
                        Location.sBoardPartPosition = DataReader.GetString(1);
                    }
                    else
                    {
                        Location.sBoardPartPosition = "";
                    }

                    Location.sBoardPartLocation = DataReader.GetString(2);

                    if (DataReader.IsDBNull(3) == false)
                    {
                        Location.sBoardPartDescription = DataReader.GetString(3);
                    }
                    else
                    {
                        Location.sBoardPartDescription = "";
                    }

                    LocationsList.Add(Location);
                }
                
                DataReader.Close();

                LocationsList.Sort(new PosAndLocComparer());

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Finds the location(s) for a part (and it's matching parts) on the given board.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetBoardPartLocationsIncludeAllMatchingParts(
            System.Int32 nBoardId,
            System.Int32 nPartId,
            out System.Collections.Generic.List<DatabaseDefs.TBoardPartLocation> LocationsList,
            out System.String sErrorMessage)
        {
            System.Collections.Generic.List<DatabaseDefs.TPart> TmpPartList;
            System.Collections.Generic.List<DatabaseDefs.TBoardPartLocation> TmpLocationsList;

            LocationsList = new System.Collections.Generic.List<DatabaseDefs.TBoardPartLocation>();
            sErrorMessage = "";

            if (false == GetPartsWithSamePinouts(nPartId, out TmpPartList,
                                                 out sErrorMessage))
            {
                return false;
            }

            foreach (DatabaseDefs.TPart Part in TmpPartList)
            {
                if (false == GetBoardPartLocations(nBoardId, Part.nPartId,
                                                   out TmpLocationsList,
                                                   out sErrorMessage))
                {
                    LocationsList.Clear();

                    return false;
                }

                foreach (DatabaseDefs.TBoardPartLocation BoardPartLocation in TmpLocationsList)
                {
                    LocationsList.Add(BoardPartLocation);
                }
            }

            LocationsList.Sort(new PosAndLocComparer());

            return true;
        }

        /// <summary>
        /// Gets all of the game boards that match a keyword.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetGameBoardsMatchingKeyword(
            System.String sKeyword,
            out System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<DatabaseDefs.TBoard>> GameBoardListDict,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;
            DatabaseDefs.TBoard Board;
            System.Collections.Generic.List<DatabaseDefs.TBoard> BoardList;

            GameBoardListDict = new System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<DatabaseDefs.TBoard>>();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT Game.Name, ");
                sb.Append("       Board.BoardID, ");
                sb.Append("       Board.Name, ");
                sb.Append("       Board.Description, ");
                sb.Append("       Board.[Size], ");
                sb.Append("       BoardType.Name ");
                sb.Append("FROM Game INNER JOIN ");
                sb.Append("    (GameBoard INNER JOIN ");
                sb.Append("        (Board INNER JOIN ");
                sb.Append("            BoardType ON Board.BoardTypeID = BoardType.BoardTypeId) ");
                sb.Append("        ON GameBoard.BoardID = Board.BoardID) ");
                sb.Append("ON Game.GameID = GameBoard.GameID ");
                sb.Append("WHERE (Board.Name LIKE '%");
                sb.Append(sKeyword.Replace("'", "''"));
                sb.Append("%') OR ");
                sb.Append("      (Board.Description LIKE '%");
                sb.Append(sKeyword.Replace("'", "''"));
                sb.Append("%') ");
                sb.Append("ORDER BY Game.Name, ");
                sb.Append("         BoardType.Name, ");
                sb.Append("         Board.Name;");

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    Board = new DatabaseDefs.TBoard();

                    Board.nBoardId = DataReader.GetInt32(1);

                    if (DataReader.IsDBNull(2) == false)
                    {
                        Board.sBoardName = DataReader.GetString(2);
                    }
                    else
                    {
                        Board.sBoardName = "";
                    }

                    if (DataReader.IsDBNull(3) == false)
                    {
                        Board.sBoardDescription = DataReader.GetString(3);
                    }
                    else
                    {
                        Board.sBoardDescription = "";
                    }

                    if (DataReader.IsDBNull(4) == false)
                    {
                        Board.sBoardSize = DataReader.GetString(4);
                    }
                    else
                    {
                        Board.sBoardSize = "";
                    }

                    Board.sBoardTypeName = DataReader.GetString(5);

                    if (!GameBoardListDict.TryGetValue(DataReader.GetString(0), out BoardList))
                    {
                        BoardList = new System.Collections.Generic.List<DatabaseDefs.TBoard>();
                        
                        GameBoardListDict.Add(DataReader.GetString(0), BoardList);
                    }

                    BoardList.Add(Board);
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Finds the manuals used by the given game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetManualsForGame(
            System.Int32 nGameId,
            out System.Collections.Generic.List<DatabaseDefs.TManual> ManualsList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;
            Common.Collections.StringCollection PropertiesColl;
            DatabaseDefs.TManual Manual;

            ManualsList = new System.Collections.Generic.List<DatabaseDefs.TManual>();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT Manual.ManualID, ");
                sb.Append("       Manual.Name, ");
                sb.Append("       Manual.PartNumber, ");
                sb.Append("       Manual.YearPrinted, ");
                sb.Append("       Manual.Complete, ");
                sb.Append("       Manual.Original, ");
                sb.Append("       Manual.Description, ");
                sb.Append("       Manufacturer.Name ");
                sb.Append("FROM Manufacturer INNER JOIN (Manual INNER JOIN ");
                sb.Append("     GameManual ON Manual.ManualID = GameManual.ManualID) ");
                sb.Append("     ON Manufacturer.ManufacturerID = Manual.ManufacturerID ");
                sb.Append("WHERE GameManual.GameID = ? ");
                sb.Append("ORDER BY Manual.Name;");

                s_DbAdapter.AddCommandParameter(Command, "@GameID", nGameId);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    Manual = new DatabaseDefs.TManual();

                    Manual.nManualId = DataReader.GetInt32(0);
                    Manual.sManualName = DataReader.GetString(1);

                    if (DataReader.IsDBNull(2) == false)
                    {
                        Manual.sManualPartNumber = DataReader.GetString(2);
                    }
                    else
                    {
                        Manual.sManualPartNumber = "";
                    }

                    if (DataReader.IsDBNull(3) == false)
                    {
                        Manual.nManualYearPrinted = DataReader.GetInt32(3);
                    }
                    else
                    {
                        Manual.nManualYearPrinted = 0;
                    }
            
                    Manual.bManualComplete = DataReader.GetBoolean(4);
                    Manual.bManualOriginal = DataReader.GetBoolean(5);

                    if (DataReader.IsDBNull(6) == false)
                    {
                        Manual.sManualDescription = DataReader.GetString(6);
                    }
                    else
                    {
                        Manual.sManualDescription = "";
                    }

                    Manual.sManufacturer = DataReader.GetString(7);

                    if (true == GetTableProperties(CManualTableName,
                                                   Manual.nManualId,
                                                   s_ManualPropertyNameDictionary[CManualPrintEditionName],
                                                   out PropertiesColl,
                                                   out sErrorMessage) &&
                        PropertiesColl.Count == 1)
                    {
                        Manual.sManualPrintEdition = PropertiesColl[0];
                    }

                    if (true == GetTableProperties(CManualTableName,
                                                   Manual.nManualId,
                                                   s_ManualPropertyNameDictionary[CManualConditionName],
                                                   out PropertiesColl,
                                                   out sErrorMessage) &&
                        PropertiesColl.Count == 1)
                    {
                        Manual.sManualCondition = PropertiesColl[0];
                    }

                    if (true == GetTableProperties(CManualTableName,
                                                   Manual.nManualId,
                                                   s_ManualPropertyNameDictionary[CManualStorageBoxName],
                                                   out PropertiesColl,
                                                   out sErrorMessage) &&
                        PropertiesColl.Count == 1)
                    {
                        Manual.sManualStorageBox = PropertiesColl[0];
                    }

                    ManualsList.Add(Manual);
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Finds the logs for the given game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetLogsForGame(
            System.Int32 nGameId,
            out System.Collections.Generic.List<DatabaseDefs.TLog> LogsList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;
            DatabaseDefs.TLog Log;

            LogsList = new System.Collections.Generic.List<DatabaseDefs.TLog>();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT Log.LogID, ");
                sb.Append("       Log.DateTime, ");
                sb.Append("       LogType.Name, ");
                sb.Append("       Log.Description ");
                sb.Append("FROM LogType INNER JOIN Log ON LogType.LogTypeID = Log.LogTypeID ");
                sb.Append("WHERE Log.GameID = ? ");
                sb.Append("ORDER BY Log.DateTime DESC;");

                s_DbAdapter.AddCommandParameter(Command, "@GameID", nGameId);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    Log = new DatabaseDefs.TLog();

                    Log.nLogId = DataReader.GetInt32(0);
                    Log.DateTime = DataReader.GetDateTime(1);
                    Log.sLogType = DataReader.GetString(2);
                    Log.sLogDescription = DataReader.GetString(3);

                    LogsList.Add(Log);
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Adds a new game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddGame(
            System.String sGameName,
            System.String sManufacturer,
            System.String sGameWiringHarness,
            System.Boolean bGameHaveWiringHarness,
            System.Boolean bGameNeedPowerOnReset,
            System.String sGameCocktail,
            System.String sGameDescription,
            System.String sGamePinouts,
            System.String sGameDipSwitches,
            Common.Collections.StringCollection GameAudioColl,
            Common.Collections.StringCollection GameVideoColl,
            Common.Collections.StringCollection GameControlsColl,
            out System.Int32 nNewGameId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            nNewGameId = -1;
            sErrorMessage = "";

            if (!AreGamePropertyDupsValid(GameAudioColl, GameVideoColl,
                                          GameControlsColl, out sErrorMessage))
            {
                return false;
            }

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == AddGame(Command, sGameName, sManufacturer,
                                    sGameDescription, sGamePinouts,
                                    sGameDipSwitches, bGameHaveWiringHarness,
                                    bGameNeedPowerOnReset, out nNewGameId,
                                    out sErrorMessage) &&
                    true == AddGameProperties(Command,
                                              DatabaseDefs.EGameDataType.AudioProperty,
                                              nNewGameId, GameAudioColl,
                                              out sErrorMessage) &&
                    true == AddGameProperties(Command,
                                              DatabaseDefs.EGameDataType.VideoProperty,
                                              nNewGameId, GameVideoColl,
                                              out sErrorMessage) &&
                    true == AddGameProperties(Command,
                                              DatabaseDefs.EGameDataType.ControlProperty,
                                              nNewGameId, GameControlsColl,
                                              out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CGameTableName,
                                                  nNewGameId,
                                                  GetGameDataId(DatabaseDefs.EGameDataType.WiringHarness,
                                                                sGameWiringHarness),
                                                  out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CGameTableName,
                                                  nNewGameId,
                                                  GetGameDataId(DatabaseDefs.EGameDataType.Cocktail,
                                                                sGameCocktail),
                                                  out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                nNewGameId = -1;

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Changes the data associated with a game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditGame(
            System.Int32 nGameId,
            System.String sGameName,
            System.String sManufacturer,
            System.String sGameWiringHarness,
            System.Boolean bGameHaveWiringHarness,
            System.Boolean bGameNeedPowerOnReset,
            System.String sGameCocktail,
            System.String sGameDescription,
            System.String sGamePinouts,
            System.String sGameDipSwitches,
            Common.Collections.StringCollection GameAudioColl,
            Common.Collections.StringCollection GameVideoColl,
            Common.Collections.StringCollection GameControlsColl,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            if (!AreGamePropertyDupsValid(GameAudioColl, GameVideoColl,
                                          GameControlsColl, out sErrorMessage))
            {
                return false;
            }

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == UpdateGame(Command, nGameId, sGameName,
                                       GetManufacturerId(sManufacturer),
                                       sGameDescription, sGamePinouts,
                                       sGameDipSwitches, bGameHaveWiringHarness,
                                       bGameNeedPowerOnReset, out sErrorMessage) &&
                    true == DeleteTableTableProperties(Command, CGameTableName,
                                                       nGameId,
                                                       out sErrorMessage) &&
                    true == AddGameProperties(Command,
                                              DatabaseDefs.EGameDataType.AudioProperty,
                                              nGameId, GameAudioColl,
                                              out sErrorMessage) &&
                    true == AddGameProperties(Command,
                                              DatabaseDefs.EGameDataType.VideoProperty,
                                              nGameId, GameVideoColl,
                                              out sErrorMessage) &&
                    true == AddGameProperties(Command,
                                              DatabaseDefs.EGameDataType.ControlProperty,
                                              nGameId, GameControlsColl,
                                              out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CGameTableName,
                                                  nGameId,
                                                  GetGameDataId(DatabaseDefs.EGameDataType.WiringHarness,
                                                                sGameWiringHarness),
                                                  out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CGameTableName,
                                                  nGameId,
                                                  GetGameDataId(DatabaseDefs.EGameDataType.Cocktail,
                                                                sGameCocktail),
                                                  out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Changes the data associated with a game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeleteGame(
            System.Int32 nGameId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.Boolean bQuitDeleting = false;
            System.Collections.Generic.List<System.Int32> BoardIdsList;
            System.Collections.IEnumerator Enum;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            if (false == GetGameBoardIds(nGameId, out BoardIdsList,
                                         out sErrorMessage))
            {
                return false;
            }

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                Enum = BoardIdsList.GetEnumerator();

                while (Enum.MoveNext() && bQuitDeleting == false)
                {
                    // BoardId contained within enum.current

                    if (false == DeleteBoardParts(Command,
                                     (System.Int32)Enum.Current,
                                     out sErrorMessage) ||
                        false == DeleteTable1Table2ByTable2Id(
                                     Command, CGameTableName, CBoardTableName,
                                     (System.Int32)Enum.Current,
                                     out sErrorMessage) ||
                        false == DeleteNameFromTable(Command, CBoardTableName,
                                                     (System.Int32)Enum.Current,
                                                     out sErrorMessage))
                    {
                        bQuitDeleting = true;
                    }
                }

                if (bQuitDeleting == false)
                {
                    if (true == DeleteTable1Table2ByTable1Id(Command,
                                                             CGameTableName,
                                                             CManualTableName,
                                                             nGameId,
                                                             out sErrorMessage) &&
                        true == DeleteTableTableProperties(Command,
                                                           CGameTableName,
                                                           nGameId,
                                                           out sErrorMessage) &&
                        true == DeleteNameFromTable(Command, CGameTableName,
                                                    nGameId, out sErrorMessage))
                    {
                        Transaction.Commit();

                        bResult = true;
                    }
                    else
                    {
                        Transaction.Rollback();
                    }
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Adds a new board to an existing game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddGameBoard(
            System.Int32 nGameId,
            System.String sBoardName,
            System.String sSize,
            System.String sDescription,
            System.String sBoardTypeName,
            out System.Int32 nNewBoardId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            nNewBoardId = -1;
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == AddBoard(Command, sBoardName, sSize, sDescription,
                                     GetBoardTypeId(sBoardTypeName),
                                     out nNewBoardId, out sErrorMessage) &&
                    true == AddTable1Table2Value(Command, CGameTableName,
                                                 CBoardTableName, nGameId,
                                                 nNewBoardId,
                                                 out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    nNewBoardId = -1;

                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                nNewBoardId = -1;

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Changes the data associated with a game board.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditGameBoard(
            System.Int32 nBoardId,
            System.String sBoardTypeName,
            System.String sBoardName,
            System.String sSize,
            System.String sDescription,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == UpdateGameBoard(Command, nBoardId, sBoardName,
                                            sDescription, sSize,
                                            GetBoardTypeId(sBoardTypeName),
                                            out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Deletes the data associated with a game board.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeleteGameBoard(
            System.Int32 nBoardId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == DeleteTable1Table2ByTable1Id(
                                Command, CBoardTableName, CPartTableName,
                                nBoardId, out sErrorMessage) &&
                    true == DeleteTable1Table2ByTable2Id(
                                Command, CGameTableName, CBoardTableName,
                                nBoardId, out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Adds a new part to an existing game board.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddGameBoardPart(
            System.Int32 nBoardId,
            System.String sPosition,
            System.String sLocation,
            System.String sDescription,
            System.String sPartName,
            System.String sPartCategoryName,
            System.String sPartTypeName,
            System.String sPartPackageName,
            out System.Int32 nNewBoardPartId,
            out System.Int32 nPartId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            nNewBoardPartId = -1;
            nPartId = -1;
            sErrorMessage = "";

            if (false == GetPartId(sPartName, sPartCategoryName, sPartTypeName,
                                   sPartPackageName, out nPartId,
                                   out sErrorMessage))
            {
                return false;
            }

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == AddGameBoardPart(Command, sPosition, sLocation,
                                             sDescription, nBoardId, nPartId,
                                             out nNewBoardPartId,
                                             out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                nNewBoardPartId = -1;

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Changes the part data associated with a game board.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditGameBoardPart(
            System.Int32 nBoardPartId,
            System.String sPosition,
            System.String sLocation,
            System.String sDescription,
            System.String sPartName,
            System.String sPartCategoryName,
            System.String sPartTypeName,
            System.String sPartPackageName,
            out System.Int32 nPartId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            nPartId = -1;
            sErrorMessage = "";

            if (false == GetPartId(sPartName, sPartCategoryName, sPartTypeName,
                                   sPartPackageName, out nPartId,
                                   out sErrorMessage))
            {
                return false;
            }

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == UpdateGameBoardPart(Command, nBoardPartId, sPosition,
                                                sLocation, sDescription, nPartId,
                                                out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Deletes a part associated with a game board.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeleteGameBoardPart(
            System.Int32 nBoardPartId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == DeleteNameFromTable(Command, CBoardPartTableName,
                                                nBoardPartId, out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Associates an existing manual with a game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddGameManual(
            System.Int32 nGameId,
            System.Int32 nManualId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == AddTable1Table2Value(Command, CGameTableName,
                                                 CManualTableName, nGameId,
                                                 nManualId, out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Deletes the association of a manual to a game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeleteGameManual(
            System.Int32 nGameId,
            System.Int32 nManualId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == DeleteGameManual(Command, nGameId, nManualId,
                                             out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Changes the displays associated with a game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditGameDisplays(
            System.Int32 nGameId,
            System.Collections.Specialized.StringCollection DisplayColl,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Boolean bDisplaysAdded = true;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;
            System.Collections.Generic.List<DatabaseDefs.TDisplay> DisplaysList = new System.Collections.Generic.List<DatabaseDefs.TDisplay>();
            System.Collections.Hashtable DisplaysHashTable = new System.Collections.Hashtable();
            System.Collections.Specialized.StringEnumerator StringEnum;
            System.Int32 nGameDisplayId;

            sErrorMessage = "";

            if (false == GetDisplays(out DisplaysList, out sErrorMessage))
            {
                return false;
            }

            foreach (DatabaseDefs.TDisplay Display in DisplaysList)
            {
                DisplaysHashTable.Add(Display.sDisplayName, Display.nDisplayId);
            }

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == DeleteTable1Table2ByTable1Id(Command,
                                                         CGameTableName,
                                                         CDisplayTableName,
                                                         nGameId,
                                                         out sErrorMessage))
                {
                    StringEnum = DisplayColl.GetEnumerator();

                    while (bDisplaysAdded == true && StringEnum.MoveNext())
                    {
                        if (false == DisplaysHashTable.ContainsKey(StringEnum.Current))
                        {
                            sErrorMessage = "The display \"";
                            sErrorMessage += StringEnum.Current;
                            sErrorMessage += "\" was not found.";

                            bDisplaysAdded = false;
                        }

                        if (bDisplaysAdded == true &&
                            false == AddTable1Table2Value(
                                         Command, CGameTableName,
                                         CDisplayTableName, nGameId,
                                         (System.Int32)DisplaysHashTable[StringEnum.Current],
                                         out nGameDisplayId,
                                         out sErrorMessage))
                        {
                            bDisplaysAdded = false;
                        }
                    }                    
                }
                else
                {
                    bDisplaysAdded = false;
                }

                if (bDisplaysAdded == true)
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Adds a new log entry for a game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddGameLogEntry(
            System.Int32 nGameId,
            System.DateTime DateTime,
            System.String sLogType,
            System.String sDescription,
            out System.Int32 nLogId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            nLogId = -1;
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == AddLog(Command, DateTime, sDescription,
                                   GetLogTypeId(sLogType), nGameId,
                                   out nLogId, out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }
        
        /// <summary>
        /// Edits an existing log entry for a game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditGameLogEntry(
            System.Int32 nLogId,
            System.DateTime DateTime,
            System.String sLogType,
            System.String sDescription,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == EditLog(Command, nLogId, DateTime, sDescription,
                                    GetLogTypeId(sLogType),
                                    out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Deletes an existing log entry for a game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeleteGameLogEntry(
            System.Int32 nLogId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == DeleteLog(Command, nLogId, out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Retrieves all of the parts associated with a game's board.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetGameBoardParts(
            System.Int32 nBoardId,
            out System.Collections.Generic.List<DatabaseDefs.TBoardPart> GameBoardPartsList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Boolean bQuit = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            GameBoardPartsList = new System.Collections.Generic.List<DatabaseDefs.TBoardPart>();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT BoardPart.BoardPartID, ");
                sb.Append("       BoardPart.[Position], ");
                sb.Append("       BoardPartLocation.Name, ");
                sb.Append("       BoardPart.Description, ");
                sb.Append("       Part.PartID, ");
                sb.Append("       Part.Name, ");
                sb.Append("       Part.IsDefault ");
                sb.Append("FROM BoardPartLocation INNER JOIN (Part INNER JOIN ");
                sb.Append("    BoardPart ON Part.PartID = BoardPart.PartID) ");
                sb.Append("    ON BoardPartLocation.BoardPartLocationID = BoardPart.BoardPartLocationID ");
                sb.Append("WHERE BoardPart.BoardID = ? ");
                sb.Append("ORDER BY BoardPartLocation.Name, BoardPart.[Position];");

                s_DbAdapter.AddCommandParameter(Command, "@BoardID", nBoardId);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (bQuit == false && DataReader.Read())
                {
                    DatabaseDefs.TBoardPart BoardPart = new DatabaseDefs.TBoardPart();

                    BoardPart.BoardPartLocation.nBoardPartId = DataReader.GetInt32(0);

                    if (DataReader.IsDBNull(1) == false)
                    {
                        BoardPart.BoardPartLocation.sBoardPartPosition = DataReader.GetString(1);
                    }
                    else
                    {
                        BoardPart.BoardPartLocation.sBoardPartPosition = "";
                    }

                    BoardPart.BoardPartLocation.sBoardPartLocation = DataReader.GetString(2);

                    if (DataReader.IsDBNull(3) == false)
                    {
                        BoardPart.BoardPartLocation.sBoardPartDescription = DataReader.GetString(3);
                    }
                    else
                    {
                        BoardPart.BoardPartLocation.sBoardPartDescription = "";
                    }

                    BoardPart.Part.nPartId = DataReader.GetInt32(4);
                    BoardPart.Part.sPartName = DataReader.GetString(5);
                    BoardPart.Part.bPartIsDefault = DataReader.GetBoolean(6);
                    BoardPart.Part.PartDatasheetColl = new Common.Collections.StringCollection();

                    if (true == GetTablePropertyValue(CPartTableName,
                                                      BoardPart.Part.nPartId,
                                                      CPartCategoryName,
                                                      out BoardPart.Part.sPartCategoryName,
                                                      out sErrorMessage) &&
                        true == GetTablePropertyValue(CPartTableName,
                                                      BoardPart.Part.nPartId,
                                                      CPartTypeName,
                                                      out BoardPart.Part.sPartTypeName,
                                                      out sErrorMessage) &&
                        true == GetTablePropertyValue(CPartTableName,
                                                      BoardPart.Part.nPartId,
                                                      CPartPackageName,
                                                      out BoardPart.Part.sPartPackageName,
                                                      out sErrorMessage) &&
                        true == GetTablePropertyValues(CPartTableName,
                                                       BoardPart.Part.nPartId,
                                                       CPartDatasheetName,
                                                       out BoardPart.Part.PartDatasheetColl,
                                                       out sErrorMessage))
                    {
                        GameBoardPartsList.Add(BoardPart);
                    }
                    else
                    {
                        GameBoardPartsList.Clear();

                        bQuit = true;
                    }
                }

                DataReader.Close();

                if (bQuit == false)
                {
                    bResult = true;
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Gets all of the manuals in the database. A keyword can be specified to
        /// limit the the results.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetManuals(
            System.String sKeyword,
            out System.Collections.Generic.List<DatabaseDefs.TManual> ManualsList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;
            Common.Collections.StringCollection PropertiesColl;

            ManualsList = new System.Collections.Generic.List<DatabaseDefs.TManual>();
            sErrorMessage = "";

            PropertiesColl = new Common.Collections.StringCollection();

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT Manual.ManualID, ");
                sb.Append("       Manual.Name, ");
                sb.Append("       Manual.PartNumber, ");
                sb.Append("       Manual.YearPrinted, ");
                sb.Append("       Manual.Complete, ");
                sb.Append("       Manual.Original, ");
                sb.Append("       Manual.Description, ");
                sb.Append("       Manufacturer.Name ");
                sb.Append("FROM Manufacturer INNER JOIN Manual ON ");
                sb.Append("     Manufacturer.ManufacturerID = Manual.ManufacturerID ");

                if (sKeyword != null)
                {
                    sb.Append("WHERE ((Manual.Name Like '%");
                    sb.Append(sKeyword.Replace("'", "''"));
                    sb.Append("%')) ");
                }

                sb.Append("ORDER BY Manual.Name, ");
                sb.Append("         Manufacturer.Name;");

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    DatabaseDefs.TManual Manual = new DatabaseDefs.TManual();

                    Manual.nManualId = DataReader.GetInt32(0);
                    Manual.sManualName = DataReader.GetString(1);

                    if (DataReader.IsDBNull(2) == false)
                    {
                        Manual.sManualPartNumber = DataReader.GetString(2);
                    }
                    else
                    {
                        Manual.sManualPartNumber = "";
                    }

                    if (DataReader.IsDBNull(3) == false)
                    {
                        Manual.nManualYearPrinted = DataReader.GetInt32(3);
                    }
                    else
                    {
                        Manual.nManualYearPrinted = 0;
                    }

                    Manual.bManualComplete = DataReader.GetBoolean(4);
                    Manual.bManualOriginal = DataReader.GetBoolean(5);

                    if (DataReader.IsDBNull(6) == false)
                    {
                        Manual.sManualDescription = DataReader.GetString(6);
                    }
                    else
                    {
                        Manual.sManualDescription = "";
                    }

                    Manual.sManufacturer = DataReader.GetString(7);

                    if (true == GetTableProperties(CManualTableName,
                                                   Manual.nManualId,
                                                   s_ManualPropertyNameDictionary[CManualPrintEditionName],
                                                   out PropertiesColl,
                                                   out sErrorMessage) &&
                        PropertiesColl.Count == 1)
                    {
                        Manual.sManualPrintEdition = PropertiesColl[0];
                    }

                    if (true == GetTableProperties(CManualTableName,
                                                   Manual.nManualId,
                                                   s_ManualPropertyNameDictionary[CManualConditionName],
                                                   out PropertiesColl,
                                                   out sErrorMessage) &&
                        PropertiesColl.Count == 1)
                    {
                        Manual.sManualCondition = PropertiesColl[0];
                    }

                    if (true == GetTableProperties(CManualTableName,
                                                   Manual.nManualId,
                                                   s_ManualPropertyNameDictionary[CManualStorageBoxName],
                                                   out PropertiesColl,
                                                   out sErrorMessage) &&
                        PropertiesColl.Count == 1)
                    {
                        Manual.sManualStorageBox = PropertiesColl[0];
                    }

                    ManualsList.Add(Manual);
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Adds a new manual.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddManual(
            System.String sManualName,
            System.String sManualStorageBox,
            System.String sManualPartNumber,
            System.Int32 nManualYearPrinted,
            System.String sManualPrintEdition,
            System.String sManualCondition,
            System.String sManualManufacturer,
            System.Boolean bManualComplete,
            System.Boolean bManualOriginal,
            System.String sManualDescription,
            out System.Int32 nNewManualId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            nNewManualId = -1;
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == AddManual(Command, sManualName, sManualPartNumber,
                                      nManualYearPrinted, bManualComplete,
                                      bManualOriginal, sManualDescription,
                                      GetManufacturerId(sManualManufacturer),
                                      out nNewManualId,
                                      out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CManualTableName,
                                                  nNewManualId,
                                                  GetManualDataId(DatabaseDefs.EManualDataType.StorageBox,
                                                                  sManualStorageBox),
                                                  out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CManualTableName,
                                                  nNewManualId,
                                                  GetManualDataId(DatabaseDefs.EManualDataType.PrintEdition,
                                                                  sManualPrintEdition),
                                                  out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CManualTableName,
                                                  nNewManualId,
                                                  GetManualDataId(DatabaseDefs.EManualDataType.Condition,
                                                                  sManualCondition),
                                                  out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Edits an existing manual.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditManual(
            System.Int32 nManualId,
            System.String sManualName,
            System.String sManualStorageBox,
            System.String sManualPartNumber,
            System.Int32 nManualYearPrinted,
            System.String sManualPrintEdition,
            System.String sManualCondition,
            System.String sManualManufacturer,
            System.Boolean bManualComplete,
            System.Boolean bManualOriginal,
            System.String sManualDescription,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;
            System.String sOriginalManualStorageBox, sOriginalManualPrintEdition;
            System.String sOriginalManualCondition;

            sErrorMessage = "";

            if (false == GetTablePropertyValue(CManualTableName, nManualId,
                                               CManualStorageBoxName,
                                               out sOriginalManualStorageBox,
                                               out sErrorMessage) ||
                false == GetTablePropertyValue(CManualTableName, nManualId,
                                               CManualPrintEditionName,
                                               out sOriginalManualPrintEdition,
                                               out sErrorMessage) ||
                false == GetTablePropertyValue(CManualTableName, nManualId,
                                               CManualConditionName,
                                               out sOriginalManualCondition,
                                               out sErrorMessage))
            {
                return false;
            }

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == UpdateManual(Command, nManualId, sManualName,
                                         sManualPartNumber, nManualYearPrinted,
                                         bManualComplete, bManualOriginal,
                                         sManualDescription,
                                         GetManufacturerId(sManualManufacturer),
                                         out sErrorMessage) &&
                    true == UpdateTableTableProperty(Command, CManualTableName,
                                nManualId,
                                GetManualDataId(DatabaseDefs.EManualDataType.StorageBox,
                                                sOriginalManualStorageBox),
                                GetManualDataId(DatabaseDefs.EManualDataType.StorageBox,
                                                sManualStorageBox),
                                out sErrorMessage) &&
                    true == UpdateTableTableProperty(Command, CManualTableName,
                                nManualId,
                                GetManualDataId(DatabaseDefs.EManualDataType.PrintEdition,
                                                sOriginalManualPrintEdition),
                                GetManualDataId(DatabaseDefs.EManualDataType.PrintEdition,
                                                sManualPrintEdition),
                                out sErrorMessage) &&
                    true == UpdateTableTableProperty(Command, CManualTableName,
                                nManualId,
                                GetManualDataId(DatabaseDefs.EManualDataType.Condition,
                                                sOriginalManualCondition),
                                GetManualDataId(DatabaseDefs.EManualDataType.Condition,
                                                sManualCondition),
                                out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Deletes an existing manual.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeleteManual(
            System.Int32 nManualId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == DeleteTable1Table2ByTable2Id(Command,
                                                         CGameTableName,
                                                         CManualTableName,
                                                         nManualId,
                                                         out sErrorMessage) &&
                    true == DeleteTableTableProperties(Command,
                                                       CManualTableName,
                                                       nManualId,
                                                       out sErrorMessage) &&
                    true == DeleteNameFromTable(Command, CManualTableName,
                                                nManualId, out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// Gets all of the displays in the database.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean GetDisplays(
            out System.Collections.Generic.List<DatabaseDefs.TDisplay> DisplaysList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Boolean bQuit = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            DisplaysList = new System.Collections.Generic.List<DatabaseDefs.TDisplay>();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT Display.DisplayID, Display.Name ");
                sb.Append("FROM Display ");
                sb.Append("ORDER BY Display.Name;");

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (bQuit == false && DataReader.Read())
                {
                    DatabaseDefs.TDisplay Display = new DatabaseDefs.TDisplay();

                    Display.nDisplayId = DataReader.GetInt32(0);
                    Display.sDisplayName = DataReader.GetString(1);

                    if (true == GetTablePropertyValue(CDisplayTableName,
                                                      Display.nDisplayId,
                                                      CDisplayTypeName,
                                                      out Display.sDisplayType,
                                                      out sErrorMessage) &&
                        true == GetTablePropertyValue(CDisplayTableName,
                                                      Display.nDisplayId,
                                                      CDisplayResolutionName,
                                                      out Display.sDisplayResolution,
                                                      out sErrorMessage) &&
                        true == GetTablePropertyValue(CDisplayTableName,
                                                      Display.nDisplayId,
                                                      CDisplayColorsName,
                                                      out Display.sDisplayColors,
                                                      out sErrorMessage) &&
                        true == GetTablePropertyValue(CDisplayTableName,
                                                      Display.nDisplayId,
                                                      CDisplayOrientationName,
                                                      out Display.sDisplayOrientation,
                                                      out sErrorMessage))
                    {
                        DisplaysList.Add(Display);
                    }
                    else
                    {
                        DisplaysList.Clear();

                        bQuit = true;
                    }
                }

                DataReader.Close();

                if (bQuit == false)
                {
                    bResult = true;
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Adds a new display.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AddDisplay(
            System.String sDisplayName,
            System.String sDisplayType,
            System.String sDisplayResolution,
            System.String sDisplayColors,
            System.String sDisplayOrientation,
            out System.Int32 nNewDisplayId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            nNewDisplayId = -1;
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == AddNameToTable(Command, CDisplayTableName,
                                           sDisplayName,
                                           out nNewDisplayId,
                                           out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CDisplayTableName,
                                                  nNewDisplayId,
                                                  GetDisplayDataId(DatabaseDefs.EDisplayDataType.Type,
                                                                   sDisplayType),
                                                  out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CDisplayTableName,
                                                  nNewDisplayId,
                                                  GetDisplayDataId(DatabaseDefs.EDisplayDataType.Resolution,
                                                                   sDisplayResolution),
                                                  out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CDisplayTableName,
                                                  nNewDisplayId,
                                                  GetDisplayDataId(DatabaseDefs.EDisplayDataType.Colors,
                                                                   sDisplayColors),
                                                  out sErrorMessage) &&
                    true == AddTableTableProperty(Command, CDisplayTableName,
                                                  nNewDisplayId,
                                                  GetDisplayDataId(DatabaseDefs.EDisplayDataType.Orientation,
                                                                   sDisplayOrientation),
                                                  out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Edits an existing display.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean EditDisplay(
            System.Int32 nDisplayId,
            System.String sDisplayName,
            System.String sDisplayType,
            System.String sDisplayResolution,
            System.String sDisplayColors,
            System.String sDisplayOrientation,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;
            System.String sOriginalDisplayType, sOriginalDisplayResolution;
            System.String sOriginalDisplayColors, sOriginalDisplayOrientation;

            sErrorMessage = "";

            if (false == GetTablePropertyValue(CDisplayTableName, nDisplayId,
                                               CDisplayTypeName,
                                               out sOriginalDisplayType,
                                               out sErrorMessage) ||
                false == GetTablePropertyValue(CDisplayTableName, nDisplayId,
                                               CDisplayResolutionName,
                                               out sOriginalDisplayResolution,
                                               out sErrorMessage) ||
                false == GetTablePropertyValue(CDisplayTableName, nDisplayId,
                                               CDisplayColorsName,
                                               out sOriginalDisplayColors,
                                               out sErrorMessage) ||
                false == GetTablePropertyValue(CDisplayTableName, nDisplayId,
                                               CDisplayOrientationName,
                                               out sOriginalDisplayOrientation,
                                               out sErrorMessage))
            {
                return false;
            }

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == UpdateNameOfTable(Command, CDisplayTableName,
                                              nDisplayId, sDisplayName,
                                              out sErrorMessage) &&
                    true == UpdateTableTableProperty(Command, CDisplayTableName,
                                nDisplayId,
                                GetDisplayDataId(DatabaseDefs.EDisplayDataType.Type,
                                                 sOriginalDisplayType),
                                GetDisplayDataId(DatabaseDefs.EDisplayDataType.Type,
                                                 sDisplayType),
                                out sErrorMessage) &&
                    true == UpdateTableTableProperty(Command, CDisplayTableName,
                                nDisplayId,
                                GetDisplayDataId(DatabaseDefs.EDisplayDataType.Resolution,
                                                 sOriginalDisplayResolution),
                                GetDisplayDataId(DatabaseDefs.EDisplayDataType.Resolution,
                                                 sDisplayResolution),
                                out sErrorMessage) &&
                    true == UpdateTableTableProperty(Command, CDisplayTableName,
                                nDisplayId,
                                GetDisplayDataId(DatabaseDefs.EDisplayDataType.Colors,
                                                 sOriginalDisplayColors),
                                GetDisplayDataId(DatabaseDefs.EDisplayDataType.Colors,
                                                 sDisplayColors),
                                out sErrorMessage) &&
                    true == UpdateTableTableProperty(Command, CDisplayTableName,
                                nDisplayId,
                                GetDisplayDataId(DatabaseDefs.EDisplayDataType.Orientation,
                                                 sOriginalDisplayOrientation),
                                GetDisplayDataId(DatabaseDefs.EDisplayDataType.Orientation,
                                                 sDisplayOrientation),
                                out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Deletes an existing display.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean DeleteDisplay(
            System.Int32 nDisplayId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == DeleteTable1Table2ByTable2Id(
                                Command, CGameTableName, CDisplayTableName,
                                nDisplayId, out sErrorMessage) &&
                    true == DeleteTableTableProperties(Command,
                                                       CDisplayTableName,
                                                       nDisplayId,
                                                       out sErrorMessage) &&
                    true == DeleteNameFromTable(Command, CDisplayTableName,
                                                nDisplayId, out sErrorMessage))
                {
                    Transaction.Commit();

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Loads the database data after the database has been Initialized.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean LoadDatabaseData(
            out System.String sErrorMessage)
        {
            sErrorMessage = "";

            if (false == InitTablePropertyNames(CGameTableName,
                                                ref s_GamePropertyNameDictionary,
                                                out sErrorMessage) ||
                false == InitTablePropertyNames(CDisplayTableName,
                                                ref s_DisplayPropertyNameDictionary,
                                                out sErrorMessage) ||
                false == InitTablePropertyNames(CPartTableName,
                                                ref s_PartPropertyNameDictionary,
                                                out sErrorMessage) ||
                false == InitTablePropertyNames(CManualTableName,
                                                ref s_ManualPropertyNameDictionary,
                                                out sErrorMessage) ||
                false == InitTablePropertyValues(CGameTableName,
                                                 s_GameDataName[(System.Int32)DatabaseDefs.EGameDataType.AudioProperty],
                                                 ref s_GameDataList[(System.Int32)DatabaseDefs.EGameDataType.AudioProperty],
                                                 out sErrorMessage) ||
                false == InitTablePropertyValues(CGameTableName,
                                                 s_GameDataName[(System.Int32)DatabaseDefs.EGameDataType.WiringHarness],
                                                 ref s_GameDataList[(System.Int32)DatabaseDefs.EGameDataType.WiringHarness],
                                                 out sErrorMessage) ||
                false == InitTablePropertyValues(CGameTableName,
                                                 s_GameDataName[(System.Int32)DatabaseDefs.EGameDataType.Cocktail],
                                                 ref s_GameDataList[(System.Int32)DatabaseDefs.EGameDataType.Cocktail],
                                                 out sErrorMessage) ||
                false == InitTablePropertyValues(CGameTableName,
                                                 s_GameDataName[(System.Int32)DatabaseDefs.EGameDataType.ControlProperty],
                                                 ref s_GameDataList[(System.Int32)DatabaseDefs.EGameDataType.ControlProperty],
                                                 out sErrorMessage) ||
                false == InitTablePropertyValues(CGameTableName,
                                                 s_GameDataName[(System.Int32)DatabaseDefs.EGameDataType.VideoProperty],
                                                 ref s_GameDataList[(System.Int32)DatabaseDefs.EGameDataType.VideoProperty],
                                                 out sErrorMessage) ||
                false == InitTablePropertyValues(CPartTableName,
                                                 s_PartDataName[(System.Int32)DatabaseDefs.EPartDataType.Category],
                                                 ref s_PartDataList[(System.Int32)DatabaseDefs.EPartDataType.Category],
                                                 out sErrorMessage) ||
                false == InitTablePropertyValues(CPartTableName,
                                                 s_PartDataName[(System.Int32)DatabaseDefs.EPartDataType.Type],
                                                 ref s_PartDataList[(System.Int32)DatabaseDefs.EPartDataType.Type],
                                                 out sErrorMessage) ||
                false == InitTablePropertyValues(CPartTableName,
                                                 s_PartDataName[(System.Int32)DatabaseDefs.EPartDataType.Package],
                                                 ref s_PartDataList[(System.Int32)DatabaseDefs.EPartDataType.Package],
                                                 out sErrorMessage) ||
                false == InitTablePropertyValues(CDisplayTableName,
                                                 s_DisplayDataName[(System.Int32)DatabaseDefs.EDisplayDataType.Type],
                                                 ref s_DisplayDataList[(System.Int32)DatabaseDefs.EDisplayDataType.Type],
                                                 out sErrorMessage) ||
                false == InitTablePropertyValues(CDisplayTableName,
                                                 s_DisplayDataName[(System.Int32)DatabaseDefs.EDisplayDataType.Resolution],
                                                 ref s_DisplayDataList[(System.Int32)DatabaseDefs.EDisplayDataType.Resolution],
                                                 out sErrorMessage) ||
                false == InitTablePropertyValues(CDisplayTableName,
                                                 s_DisplayDataName[(System.Int32)DatabaseDefs.EDisplayDataType.Colors],
                                                 ref s_DisplayDataList[(System.Int32)DatabaseDefs.EDisplayDataType.Colors],
                                                 out sErrorMessage) ||
                false == InitTablePropertyValues(CDisplayTableName,
                                                 s_DisplayDataName[(System.Int32)DatabaseDefs.EDisplayDataType.Orientation],
                                                 ref s_DisplayDataList[(System.Int32)DatabaseDefs.EDisplayDataType.Orientation],
                                                 out sErrorMessage) ||
                false == InitTablePropertyValues(CManualTableName,
                                                 s_ManualDataName[(System.Int32)DatabaseDefs.EManualDataType.StorageBox],
                                                 ref s_ManualDataList[(System.Int32)DatabaseDefs.EManualDataType.StorageBox],
                                                 out sErrorMessage) ||
                false == InitTablePropertyValues(CManualTableName,
                                                 s_ManualDataName[(System.Int32)DatabaseDefs.EManualDataType.PrintEdition],
                                                 ref s_ManualDataList[(System.Int32)DatabaseDefs.EManualDataType.PrintEdition],
                                                 out sErrorMessage) ||
                false == InitTablePropertyValues(CManualTableName,
                                                 s_ManualDataName[(System.Int32)DatabaseDefs.EManualDataType.Condition],
                                                 ref s_ManualDataList[(System.Int32)DatabaseDefs.EManualDataType.Condition],
                                                 out sErrorMessage) ||
                false == InitTableDataList(CManufacturerTableName,
                                           ref s_ManufacturerList,
                                           out sErrorMessage) ||
                false == InitTableDataList(CBoardTypeTableName,
                                           ref s_BoardTypeList,
                                           out sErrorMessage) ||
                false == InitTableDataList(CBoardPartLocationTableName,
                                           ref s_BoardPartLocationList,
                                           out sErrorMessage) ||
                false == InitTableDataList(CLogTypeTableName,
                                           ref s_LogTypeList,
                                           out sErrorMessage))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the given part name already exists for a group of parts.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DoesPartNameExist(
            System.Int32 nPartPinoutsId,
            System.String sPartName,
            out System.Boolean bExists,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            bExists = false;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT Part.PartID ");
                sb.Append("FROM Part ");
                sb.Append("WHERE (Part.PartPinoutsID = ?) AND ");
                sb.Append("      (Part.Name = ?);");

                s_DbAdapter.AddCommandParameter(Command, "@PartPinoutsID", nPartPinoutsId);
                s_DbAdapter.AddCommandParameter(Command, "@PartName", sPartName);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                if (DataReader.Read())
                {
                    bExists = true;
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Adds a new row to the Part table as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddPart(
            System.Data.Common.DbCommand Command,
            System.String sName,
            System.Int32 nPartPinoutsId,
            System.Boolean bIsDefault,
            out System.Int32 nNewPartId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            nNewPartId = -1;
            sErrorMessage = "";

            try
            {
                sb.Append("INSERT INTO Part (Name, PartPinoutsID, IsDefault) ");
                sb.Append("VALUES (?, ?, ?);");

                s_DbAdapter.AddCommandParameter(Command, "@Name", sName);
                s_DbAdapter.AddCommandParameter(Command, "@PartPinoutsID", nPartPinoutsId);
                s_DbAdapter.AddCommandParameter(Command, "@IsDefault", bIsDefault);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                if (s_DbAdapter.GetIdentityValue(Command, ref nNewPartId,
                                                 ref sErrorMessage))
                {
                    bResult = true;
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Gets the first available part in a group of parts excluding the given part.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean GetFirstAvailablePart(
            System.Int32 nSkipPartId,
            System.Int32 nPartPinoutsId,
            out System.Int32 nNextPartId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.String sTmpErrorMessage = null;

            nNextPartId = -1;
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb.Append("SELECT Part.PartID ");
                sb.Append("FROM Part ");
                sb.Append("WHERE ((Part.PartPinoutsID = ?) AND ");
                sb.Append("       (Part.PartID <> ?)) ");
                sb.Append("ORDER BY Part.Name;");

                s_DbAdapter.AddCommandParameter(Command, "@PartPinoutsID", nPartPinoutsId);
                s_DbAdapter.AddCommandParameter(Command, "@SkipPartID", nSkipPartId);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                if (DataReader.Read())
                {
                    nNextPartId = DataReader.GetInt32(0);
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Find the default part out of a group.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean GetDefaultPart(
            System.Int32 nPartId,
            out System.Int32 nDefPartId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            nDefPartId = -1;
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT Part.PartID ");
                sb.Append("FROM Part ");
                sb.Append("WHERE Part.IsDefault = ");
                sb.Append(s_DbAdapter.GetSQLBooleanValue(true));
                sb.Append(" AND Part.PartPinoutsID IN ");
                sb.Append("      (SELECT Part.PartPinoutsID ");
                sb.Append("       FROM Part ");
                sb.Append("       WHERE Part.PartID = ?);");

                s_DbAdapter.AddCommandParameter(Command, "@PartID", nPartId);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                if (DataReader.Read())
                {
                    nDefPartId = DataReader.GetInt32(0);

                    bResult = true;
                }
                else
                {
                    sErrorMessage = "No default part was found.";
                }

                DataReader.Close();
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Adds the given datasheets to a part.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddPartDatasheets(
            System.Data.Common.DbCommand Command,
            System.Int32 nPartId,
            System.Collections.Specialized.StringCollection PartDatasheetColl,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.Int32 nPartPropertyId;

            sErrorMessage = "";

            try
            {
                foreach (System.String sValue in PartDatasheetColl)
                {
                    if (false == GetPartPropertyId(CPartDatasheetName,
                                                   sValue,
                                                   out nPartPropertyId,
                                                   out sErrorMessage))
                    {
                        return false;
                    }

                    if (nPartPropertyId == -1)
                    {
                        if (false == AddTableProperty(Command,
                                                      CPartTableName,
                                                      s_PartPropertyNameDictionary[CPartDatasheetName],
                                                      sValue,
                                                      out nPartPropertyId,
                                                      out sErrorMessage))
                        {
                            return false;
                        }
                    }

                    sb.Append("INSERT INTO PartPartProperty (PartID, PartPropertyID) ");
                    sb.Append("VALUES (?, ?);");

                    s_DbAdapter.AddCommandParameter(Command, "@PartID", nPartId);
                    s_DbAdapter.AddCommandParameter(Command, "@PartPropertyID", nPartPropertyId);

                    Command.CommandText = sb.ToString();

                    Command.ExecuteNonQuery();

                    Command.Parameters.Clear();

                    sb.Length = 0;
                }

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Updates the datasheets associated with a part.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean UpdatePartDatasheets(
            System.Data.Common.DbCommand Command,
            System.Int32 nPartId,
            Common.Collections.StringCollection NewPartDatasheetColl,
            out System.String sErrorMessage)
        {
            Common.Collections.StringCollection CurPartDatasheetColl;
            Common.Collections.StringCollection AddPartDatasheetColl;
            Common.Collections.StringCollection DeletePartDatasheetColl;

            CurPartDatasheetColl = new Common.Collections.StringCollection();
            AddPartDatasheetColl = new Common.Collections.StringCollection();
            DeletePartDatasheetColl = new Common.Collections.StringCollection();

            if (false == GetTablePropertyValues(CPartTableName, nPartId,
                                                CPartDatasheetName,
                                                out CurPartDatasheetColl,
                                                out sErrorMessage))
            {
                return false;
            }

            // Find the delete part datasheets

            foreach (System.String sValue in CurPartDatasheetColl)
            {
                if (false == NewPartDatasheetColl.Contains(sValue))
                {
                    DeletePartDatasheetColl.Add(sValue);
                }
            }

            // Find the new part datasheets

            foreach (System.String sValue in NewPartDatasheetColl)
            {
                if (false == CurPartDatasheetColl.Contains(sValue))
                {
                    AddPartDatasheetColl.Add(sValue);
                }
            }

            if (true == DeletePartDatasheets(Command, nPartId,
                                             DeletePartDatasheetColl,
                                             out sErrorMessage) &&
                true == DeleteUnusedProperties(Command, CPartTableName,
                                               out sErrorMessage) &&
                true == AddPartDatasheets(Command, nPartId,
                                          AddPartDatasheetColl,
                                          out sErrorMessage))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes the given datasheets associated with a part.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeletePartDatasheets(
            System.Data.Common.DbCommand Command,
            System.Int32 nPartId,
            System.Collections.Specialized.StringCollection PartDatasheetColl,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                foreach (System.String sValue in PartDatasheetColl)
                {
                    sb.Append("DELETE ");
                    sb.Append("FROM PartPartProperty ");
                    sb.Append("WHERE (PartPartProperty.PartID = ?) AND ");
                    sb.Append("PartPartProperty.PartPropertyID IN ");
                    sb.Append("    (SELECT PartProperty.PartPropertyID ");
                    sb.Append("     FROM PartPropertyValue INNER JOIN PartProperty ");
                    sb.Append("          ON PartPropertyValue.PartPropertyValueID = PartProperty.PartPropertyValueID ");
                    sb.Append("     WHERE ((PartProperty.PartPropertyNameID = ?) AND ");
                    sb.Append("(PartPropertyValue.Name = ?)));");

                    s_DbAdapter.AddCommandParameter(Command, "@PartID", nPartId);
                    s_DbAdapter.AddCommandParameter(Command, "@PartPropertyNameID",
                                                    s_PartPropertyNameDictionary[CPartDatasheetName]);
                    s_DbAdapter.AddCommandParameter(Command, "@PartPropertyValueName",
                                                    sValue);

                    Command.CommandText = sb.ToString();

                    Command.ExecuteNonQuery();

                    Command.Parameters.Clear();
                }

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Deletes all datasheets associated with a part.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeletePartDatasheets(
            System.Data.Common.DbCommand Command,
            System.Int32 nPartId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("DELETE ");
                sb.Append("FROM PartPartProperty ");
                sb.Append("WHERE (PartPartProperty.PartID = ?) AND ");
                sb.Append("PartPartProperty.PartPropertyID IN ");
                sb.Append("    (SELECT PartProperty.PartPropertyID FROM PartProperty ");
                sb.Append("     WHERE PartProperty.PartPropertyNameID = ?);");

                s_DbAdapter.AddCommandParameter(Command, "@PartID", nPartId);
                s_DbAdapter.AddCommandParameter(Command, "@PartPropertyNameID",
                                                s_PartPropertyNameDictionary[CPartDatasheetName]);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Adds the given part pinouts as a new row to the PartPinouts table.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddPartPinouts(
            System.Data.Common.DbCommand Command,
            System.String sPartPinouts,
            out System.Int32 nNewPartPinoutsId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            nNewPartPinoutsId = -1;
            sErrorMessage = "";

            try
            {
                sb.Append("INSERT INTO PartPinouts (Description) VALUES (?);");

                s_DbAdapter.AddCommandParameter(Command, "@PartPinouts", sPartPinouts);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                if (s_DbAdapter.GetIdentityValue(Command, ref nNewPartPinoutsId,
                                                 ref sErrorMessage))
                {
                    bResult = true;
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Updates the given part pinouts.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean UpdatePartPinouts(
            System.Data.Common.DbCommand Command,
            System.Int32 nPartPinoutsId,
            System.String sPartPinouts,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("UPDATE PartPinouts ");
                sb.Append("SET PartPinouts.Description = ? ");
                sb.Append("WHERE PartPinouts.PartPinoutsID = ?;");

                s_DbAdapter.AddCommandParameter(Command, "@PartPinouts", sPartPinouts);
                s_DbAdapter.AddCommandParameter(Command, "@PartPinoutsID", nPartPinoutsId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Updates the given part.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean UpdatePart(
            System.Data.Common.DbCommand Command,
            System.Int32 nPartId,
            System.String sPartName,
            System.Boolean bIsDefaultPart,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("UPDATE Part ");
                sb.Append("SET Part.Name = ?, ");
                sb.Append("    Part.IsDefault = ? ");
                sb.Append("WHERE Part.PartID = ?;");

                s_DbAdapter.AddCommandParameter(Command, "@PartName", sPartName);
                s_DbAdapter.AddCommandParameter(Command, "@IsDefaultPart", bIsDefaultPart);
                s_DbAdapter.AddCommandParameter(Command, "@PartID", nPartId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Sets the value of the default part flag for the given part.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean UpdateIsDefaultPartFlag(
            System.Data.Common.DbCommand Command,
            System.Int32 nPartId,
            System.Boolean bIsDefaultPart,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("UPDATE Part ");
                sb.Append("SET Part.IsDefault = ? ");
                sb.Append("WHERE Part.PartID = ?;");

                s_DbAdapter.AddCommandParameter(Command, "@IsDefaultPart", bIsDefaultPart);
                s_DbAdapter.AddCommandParameter(Command, "@PartID", nPartId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Adds a new row to the Game table as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddGame(
            System.Data.Common.DbCommand Command,
            System.String sGameName,
            System.String sManufacturer,
            System.String sGameDescription,
            System.String sGamePinouts,
            System.String sGameDipSwitches,
            System.Boolean bGameHaveWiringHarness,
            System.Boolean bGameNeedPowerOnReset,
            out System.Int32 nNewGameId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            nNewGameId = -1;
            sErrorMessage = "";

            try
            {
                sb.Append("INSERT INTO Game (Name, Description, ");
                sb.Append("    Pinouts, DipSwitches, HaveWiringHarness, ");
                sb.Append("    NeedPowerOnReset, ManufacturerID) ");
                sb.Append("VALUES (?, ?, ?, ?, ?, ?, ?);");

                s_DbAdapter.AddCommandParameter(Command, "@GameName", sGameName);
                s_DbAdapter.AddCommandParameter(Command, "@GameDescription", sGameDescription);
                s_DbAdapter.AddCommandParameter(Command, "@GamePinouts", sGamePinouts);
                s_DbAdapter.AddCommandParameter(Command, "@GameDipSwitches", sGameDipSwitches);
                s_DbAdapter.AddCommandParameter(Command, "@GameHaveWiringHarness",
                                                bGameHaveWiringHarness);
                s_DbAdapter.AddCommandParameter(Command, "@GameNeedPowerOnReset",
                                                bGameNeedPowerOnReset);
                s_DbAdapter.AddCommandParameter(Command, "@ManufacturerID",
                                                GetManufacturerId(sManufacturer));

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                if (s_DbAdapter.GetIdentityValue(Command, ref nNewGameId,
                                                 ref sErrorMessage))
                {
                    bResult = true;
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Updates the given game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean UpdateGame(
            System.Data.Common.DbCommand Command,
            System.Int32 nGameId,
            System.String sGameName,
            System.Int32 nManufacturerId,
            System.String sGameDescription,
            System.String sGamePinouts,
            System.String sGameDipSwitches,
            System.Boolean bGameHaveWiringHarness,
            System.Boolean bGameNeedPowerOnReset,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("UPDATE Game SET ");
                sb.Append("    Game.Name = ?, ");
                sb.Append("    Game.Description = ?, ");
                sb.Append("    Game.Pinouts = ?, ");
                sb.Append("    Game.DipSwitches = ?, ");
                sb.Append("    Game.HaveWiringHarness = ?, ");
                sb.Append("    Game.NeedPowerOnReset = ?, ");
                sb.Append("    Game.ManufacturerID = ? ");
                sb.Append("WHERE Game.GameID = ?;");

                s_DbAdapter.AddCommandParameter(Command, "@Name", sGameName);
                s_DbAdapter.AddCommandParameter(Command, "@Description", sGameDescription);
                s_DbAdapter.AddCommandParameter(Command, "@Pinouts", sGamePinouts);
                s_DbAdapter.AddCommandParameter(Command, "@DipSwitches", sGameDipSwitches);
                s_DbAdapter.AddCommandParameter(Command, "@HaveWiringHarness",
                                                bGameHaveWiringHarness);
                s_DbAdapter.AddCommandParameter(Command, "@NeedPowerOnReset",
                                                bGameNeedPowerOnReset);
                s_DbAdapter.AddCommandParameter(Command, "@ManufacturerID", nManufacturerId);
                s_DbAdapter.AddCommandParameter(Command, "@GameID", nGameId);
                
                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Adds a new row to the Board table as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddBoard(
            System.Data.Common.DbCommand Command,
            System.String sBoardName,
            System.String sSize,
            System.String sDescription,
            System.Int32 nBoardTypeId,
            out System.Int32 nNewBoardId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            nNewBoardId = -1;
            sErrorMessage = "";

            try
            {
                sb.Append("INSERT INTO Board (Name, Description, [Size], BoardTypeID) ");
                sb.Append("VALUES (?, ?, ?, ?);");

                s_DbAdapter.AddCommandParameter(Command, "@BoardName", sBoardName);
                s_DbAdapter.AddCommandParameter(Command, "@Description", sDescription);
                s_DbAdapter.AddCommandParameter(Command, "@Size", sSize);
                s_DbAdapter.AddCommandParameter(Command, "@BoardTypeID", nBoardTypeId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                if (s_DbAdapter.GetIdentityValue(Command, ref nNewBoardId,
                                                 ref sErrorMessage))
                {
                    bResult = true;
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Updates the given game board.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean UpdateGameBoard(
            System.Data.Common.DbCommand Command,
            System.Int32 nBoardId,
            System.String sBoardName,
            System.String sBoardDescription,
            System.String sBoardSize,
            System.Int32 nBoardTypeId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("UPDATE Board SET ");
                sb.Append("    Board.Name = ?, ");
                sb.Append("    Board.Description = ?, ");
                sb.Append("    Board.[Size] = ?, ");
                sb.Append("    Board.BoardTypeID = ? ");
                sb.Append("WHERE Board.BoardID = ?;");

                s_DbAdapter.AddCommandParameter(Command, "@BoardName", sBoardName);
                s_DbAdapter.AddCommandParameter(Command, "@BoardDescription", sBoardDescription);
                s_DbAdapter.AddCommandParameter(Command, "@BoardSize", sBoardSize);
                s_DbAdapter.AddCommandParameter(Command, "@BoardTypeID", nBoardTypeId);
                s_DbAdapter.AddCommandParameter(Command, "@BoardID", nBoardId);
 
                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Adds a new row to the BoardPart table as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddGameBoardPart(
            System.Data.Common.DbCommand Command,
            System.String sPosition,
            System.String sLocation,
            System.String sDescription,
            System.Int32 nBoardId,
            System.Int32 nPartId,
            out System.Int32 nNewBoardPartId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            nNewBoardPartId = -1;
            sErrorMessage = "";

            try
            {
                sb.Append("INSERT INTO BoardPart ([Position], Description, BoardID, ");
                sb.Append("    BoardPartLocationID, PartID) ");
                sb.Append("VALUES (?, ?, ?, ?, ?);");

                s_DbAdapter.AddCommandParameter(Command, "@Position", sPosition);
                s_DbAdapter.AddCommandParameter(Command, "@Description", sDescription);
                s_DbAdapter.AddCommandParameter(Command, "@BoardID", nBoardId);
                s_DbAdapter.AddCommandParameter(Command, "@BoardPartLocationID", 
                                                GetBoardPartLocationId(sLocation));
                s_DbAdapter.AddCommandParameter(Command, "@PartID", nPartId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                if (s_DbAdapter.GetIdentityValue(Command, ref nNewBoardPartId,
                                                 ref sErrorMessage))
                {
                    bResult = true;
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Updates the given board part's data as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean UpdateGameBoardPart(
            System.Data.Common.DbCommand Command,
            System.Int32 nBoardPartId,
            System.String sPosition,
            System.String sLocation,
            System.String sDescription,
            System.Int32 nPartId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("UPDATE BoardPart SET ");
                sb.Append("    BoardPart.[Position] = ?, ");
                sb.Append("    BoardPart.Description = ?, ");
                sb.Append("    BoardPart.BoardPartLocationID = ?, ");
                sb.Append("    BoardPart.PartID = ? ");
                sb.Append("WHERE BoardPart.BoardPartID = ?;");

                s_DbAdapter.AddCommandParameter(Command, "@Position", sPosition);
                s_DbAdapter.AddCommandParameter(Command, "@Description", sDescription);
                s_DbAdapter.AddCommandParameter(Command, "@BoardPartLocationID",
                                                GetBoardPartLocationId(sLocation));
                s_DbAdapter.AddCommandParameter(Command, "@PartID", nPartId);
                s_DbAdapter.AddCommandParameter(Command, "@BoardPartID", nBoardPartId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Deletes a row from the GameManual table as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeleteGameManual(
            System.Data.Common.DbCommand Command,
            System.Int32 nGameId,
            System.Int32 nManualId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("DELETE FROM GameManual ");
                sb.Append("WHERE (GameManual.GameID = ? AND ");
                sb.Append("       GameManual.ManualID = ?);");

                s_DbAdapter.AddCommandParameter(Command, "@GameID", nGameId);
                s_DbAdapter.AddCommandParameter(Command, "@ManualID", nManualId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Adds a new row to the Manual table as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddManual(
            System.Data.Common.DbCommand Command,
            System.String sManualName,
            System.String sManualPartNumber,
            System.Int32 nManualYearPrinted,
            System.Boolean bManualComplete,
            System.Boolean bManualOriginal,
            System.String sManualDescription,
            System.Int32 nManualManufacturerId,
            out System.Int32 nNewManualId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            nNewManualId = -1;
            sErrorMessage = "";

            try
            {
                sb.Append("INSERT INTO Manual (Name, PartNumber, Complete, ");
                sb.Append("                    Original, Description, ");
                sb.Append("                    ManufacturerID");

                if (nManualYearPrinted > 0)
                {
                    sb.Append(", ");
                    sb.Append("YearPrinted");
                }
                
                sb.Append(") VALUES (?, ?, ?, ?, ?, ?");

                if (nManualYearPrinted > 0)
                {
                    sb.Append(", ?");
                }

                sb.Append(");");

                s_DbAdapter.AddCommandParameter(Command, "@ManualName", sManualName);
                s_DbAdapter.AddCommandParameter(Command, "@ManualPartNumber", sManualPartNumber);
                s_DbAdapter.AddCommandParameter(Command, "@ManualComplete", bManualComplete);
                s_DbAdapter.AddCommandParameter(Command, "@ManualOriginal", bManualOriginal);
                s_DbAdapter.AddCommandParameter(Command, "@ManualDescription", sManualDescription);
                s_DbAdapter.AddCommandParameter(Command, "@ManualManufacturerID", nManualManufacturerId);

                if (nManualYearPrinted > 0)
                {
                    s_DbAdapter.AddCommandParameter(Command, "@ManualYearPrinted", nManualYearPrinted);
                }

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                if (s_DbAdapter.GetIdentityValue(Command, ref nNewManualId,
                                                 ref sErrorMessage))
                {
                    bResult = true;
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Updates the given manual.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean UpdateManual(
            System.Data.Common.DbCommand Command,
            System.Int32 nManualId,
            System.String sManualName,
            System.String sManualPartNumber,
            System.Int32 nManualYearPrinted,
            System.Boolean bManualComplete,
            System.Boolean bManualOriginal,
            System.String sManualDescription,
            System.Int32 nManualManufacturerId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("UPDATE Manual SET ");
                sb.Append("    Manual.Name = ?, ");
                sb.Append("    Manual.PartNumber = ?, ");

                if (nManualYearPrinted > 0)
                {
                    sb.Append("    Manual.YearPrinted = ?, ");
                }

                sb.Append("    Manual.Complete = ?, ");
                sb.Append("    Manual.Original = ?, ");
                sb.Append("    Manual.Description = ?, ");
                sb.Append("    Manual.ManufacturerID = ? ");
                sb.Append("WHERE Manual.ManualID = ?;");

                s_DbAdapter.AddCommandParameter(Command, "@ManualName", sManualName);
                s_DbAdapter.AddCommandParameter(Command, "@ManualPartNumber", sManualPartNumber);

                if (nManualYearPrinted > 0)
                {
                    s_DbAdapter.AddCommandParameter(Command, "@ManualYearPrinted", nManualYearPrinted);
                }

                s_DbAdapter.AddCommandParameter(Command, "@ManualComplete", bManualComplete);
                s_DbAdapter.AddCommandParameter(Command, "@ManualOriginal", bManualOriginal);
                s_DbAdapter.AddCommandParameter(Command, "@ManualDescription", sManualDescription);
                s_DbAdapter.AddCommandParameter(Command, "@ManufacturerID", nManualManufacturerId);
                s_DbAdapter.AddCommandParameter(Command,"@ManualID", nManualId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Add the given properties to a game through a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddGameProperties(
            System.Data.Common.DbCommand Command,
            DatabaseDefs.EGameDataType GameDataType,
            System.Int32 nGameId,
            System.Collections.Specialized.StringCollection PropertiesColl,
            out System.String sErrorMessage)
        {
            sErrorMessage = "";

            for (System.Int32 nIndex = 0; nIndex < PropertiesColl.Count;
                 ++nIndex)
            {
                if (false == AddTableTableProperty(Command, CGameTableName,
                                                   nGameId,
                                                   GetGameDataId(GameDataType,
                                                                 PropertiesColl[nIndex]),
                                                   out sErrorMessage))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets all of the board ids associated with a game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean GetGameBoardIds(
            System.Int32 nGameId,
            out System.Collections.Generic.List<System.Int32> BoardIdsList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            BoardIdsList = new System.Collections.Generic.List<System.Int32>();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT GameBoard.BoardID ");
                sb.Append("FROM GameBoard ");
                sb.Append("WHERE GameBoard.GameID = ?;");

                s_DbAdapter.AddCommandParameter(Command, "@GameID", nGameId);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    BoardIdsList.Add(DataReader.GetInt32(0));
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Adds a new game.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        public static System.Boolean AreGamePropertyDupsValid(
            Common.Collections.StringCollection GameAudioColl,
            Common.Collections.StringCollection GameVideoColl,
            Common.Collections.StringCollection GameControlsColl,
            out System.String sErrorMessage)
        {
            sErrorMessage = "";

            if (!s_GamePropertyDupsAllowed[(System.Int32)DatabaseDefs.EGameDataType.AudioProperty] &&
                GameAudioColl.HasDuplicates)
            {
                sErrorMessage = "The audio property cannot contain duplicates.";

                return false;
            }

            if (!s_GamePropertyDupsAllowed[(System.Int32)DatabaseDefs.EGameDataType.VideoProperty] &&
                GameVideoColl.HasDuplicates)
            {
                sErrorMessage = "The video property cannot contain duplicates.";

                return false;
            }

            if (!s_GamePropertyDupsAllowed[(System.Int32)DatabaseDefs.EGameDataType.ControlProperty] &&
                GameControlsColl.HasDuplicates)
            {
                sErrorMessage = "The control property cannot contain duplicates.";

                return false;
            }

            return true;
        }

        /// <summary>
        /// Adds a new row to the Log table as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddLog(
            System.Data.Common.DbCommand Command,
            System.DateTime DateTime,
            System.String sDescription,
            System.Int32 nLogTypeId,
            System.Int32 nGameId,
            out System.Int32 nNewLogId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            nNewLogId = -1;
            sErrorMessage = "";

            try
            {
                sb.Append("INSERT INTO Log ([DateTime], Description, LogTypeID, GameID) ");
                sb.Append("VALUES (?, ?, ?, ?);");

                s_DbAdapter.AddCommandParameter(Command, "@DateTime", DateTime.ToShortDateString());
                s_DbAdapter.AddCommandParameter(Command, "@Description", sDescription);
                s_DbAdapter.AddCommandParameter(Command, "@LogTypeID", nLogTypeId);
                s_DbAdapter.AddCommandParameter(Command, "@GameID", nGameId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                if (s_DbAdapter.GetIdentityValue(Command, ref nNewLogId,
                                                 ref sErrorMessage))
                {
                    bResult = true;
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Edits an existing row in the Log table as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean EditLog(
            System.Data.Common.DbCommand Command,
            System.Int32 nLogId,
            System.DateTime DateTime,
            System.String sDescription,
            System.Int32 nLogTypeId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("UPDATE LOG ");
                sb.Append("SET [DateTime] = ?, ");
                sb.Append("Description = ?, ");
                sb.Append("LogTypeID = ? ");
                sb.Append("WHERE LogID = ?");

                s_DbAdapter.AddCommandParameter(Command, "@DateTime", DateTime.ToShortDateString());
                s_DbAdapter.AddCommandParameter(Command, "@Description", sDescription);
                s_DbAdapter.AddCommandParameter(Command, "@LogTypeID", nLogTypeId);
                s_DbAdapter.AddCommandParameter(Command, "@LogID", nLogId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Deletes a row from the Log table as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeleteLog(
            System.Data.Common.DbCommand Command,
            System.Int32 nLogId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
           
            sErrorMessage = "";

            try
            {
                sb.Append("DELETE FROM Log WHERE LogID = ?;");

                s_DbAdapter.AddCommandParameter(Command, "@LogID", nLogId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Find the given part's identifier from it's name, category, type and package.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean GetPartId(
            System.String sPartName,
            System.String sPartCategoryName,
            System.String sPartTypeName,
            System.String sPartPackageName,
            out System.Int32 nPartId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Boolean bQuit = false;
            System.String sTmpPartCategoryName = "";
            System.String sTmpPartTypeName = "";
            System.String sTmpPartPackageName = "";
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbCommand Command = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;
            System.Int32 nTmpPartId;

            nPartId = -1;
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT Part.PartID ");
                sb.Append("FROM Part ");
                sb.Append("WHERE Part.Name = ?;");

                s_DbAdapter.AddCommandParameter(Command, "@PartName", sPartName);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read() && bQuit == false)
                {
                    nTmpPartId = DataReader.GetInt32(0);

                    if (false == GetTablePropertyValue(CPartTableName, nTmpPartId,
                                                       CPartCategoryName,
                                                       out sTmpPartCategoryName,
                                                       out sErrorMessage) ||
                        false == GetTablePropertyValue(CPartTableName, nTmpPartId,
                                                       CPartTypeName,
                                                       out sTmpPartTypeName,
                                                       out sErrorMessage) ||
                        false == GetTablePropertyValue(CPartTableName, nTmpPartId,
                                                       CPartPackageName,
                                                       out sTmpPartPackageName,
                                                       out sErrorMessage))
                    {
                        bQuit = true;
                    }

                    if (bQuit == false &&
                        sPartCategoryName.CompareTo(sTmpPartCategoryName) == 0 &&
                        sPartTypeName.CompareTo(sTmpPartTypeName) == 0 &&
                        sPartPackageName.CompareTo(sTmpPartPackageName) == 0)
                    {
                        nPartId = nTmpPartId;

                        bQuit = true;
                        bResult = true;
                    }
                }

                DataReader.Close();
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }

                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Checks if the given part property name and value exist.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean GetPartPropertyId(
            System.String sPartPropertyName,
            System.String sPartPropertyValue,
            out System.Int32 nPartPropertyId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            nPartPropertyId = -1;
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT PartProperty.PartPropertyID AS PartPropertyID ");
                sb.Append("FROM PartPropertyValue INNER JOIN (PartPropertyName ");
                sb.Append("     INNER JOIN PartProperty ON PartPropertyName.PartPropertyNameID = PartProperty.PartPropertyNameID) ");
                sb.Append("     ON PartPropertyValue.PartPropertyValueID = PartProperty.PartPropertyValueID ");
                sb.Append("WHERE (PartPropertyName.Name = ?) AND ");
                sb.Append("      (PartPropertyValue.Name = ?);");

                s_DbAdapter.AddCommandParameter(Command, "@PartPropertyName", sPartPropertyName);
                s_DbAdapter.AddCommandParameter(Command, "@PartPropertyValue", sPartPropertyValue);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                if (DataReader.Read())
                {
                    nPartPropertyId = DataReader.GetInt32(0);
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }

                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Delete's the given rows from the BoardPart table as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeleteBoardParts(
            System.Data.Common.DbCommand Command,
            System.Int32 nBoardId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("DELETE FROM BoardPart ");
                sb.Append("WHERE (BoardPart.BoardID = ?);");

                s_DbAdapter.AddCommandParameter(Command, "@BoardID", nBoardId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Determines if a game already exists in an list using the
        /// game's unique identifier.
        /// </summary>

        private static System.Boolean DoesGameAlreadyExist(
            System.Int32 nGameId,
            System.Collections.Generic.List<DatabaseDefs.TGame> GamesList)
        {
            foreach (DatabaseDefs.TGame Game in GamesList)
            {
                if (Game.nGameId == nGameId)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if a board already exists in an list using the
        /// board's unique identifier.
        /// </summary>

        private static System.Boolean DoesBoardAlreadyExist(
            System.Int32 nBoardId,
            System.Collections.Generic.List<DatabaseDefs.TBoard> BoardsList)
        {
            foreach (DatabaseDefs.TBoard Board in BoardsList)
            {
                if (Board.nBoardId == nBoardId)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets all of the properties associated with a table name.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean GetTableProperties(
            System.String sTableName,
            System.Int32 nTableId,
            System.Int32 nPropertyNameId,
            out Common.Collections.StringCollection PropertiesColl,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            PropertiesColl = new Common.Collections.StringCollection();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT ");
                sb.Append(sTableName);
                sb.Append("PropertyValue.Name FROM ");
                sb.Append(sTableName);
                sb.Append("PropertyValue INNER JOIN (");
                sb.Append(sTableName);
                sb.Append("PropertyName INNER JOIN (");
                sb.Append(sTableName);
                sb.Append("Property INNER JOIN (");
                sb.Append(sTableName);
                sb.Append(" INNER JOIN ");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property ON ");
                sb.Append(sTableName);
                sb.Append(".");
                sb.Append(sTableName);
                sb.Append("ID = ");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("ID) ON ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyID = ");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyID) ON ");
                sb.Append(sTableName);
                sb.Append("PropertyName.");
                sb.Append(sTableName);
                sb.Append("PropertyNameID = ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyNameID) ON ");
                sb.Append(sTableName);
                sb.Append("PropertyValue.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID = ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID WHERE ((");
                sb.Append(sTableName);
                sb.Append(".");
                sb.Append(sTableName);
                sb.Append("ID = ?) AND (");
                sb.Append(sTableName);
                sb.Append("PropertyName.");
                sb.Append(sTableName);
                sb.Append("PropertyNameID = ?)) ORDER BY ");
                sb.Append(sTableName);
                sb.Append("PropertyValue.Name;");

                s_DbAdapter.AddCommandParameter(Command, "@TableID", nTableId);
                s_DbAdapter.AddCommandParameter(Command, "@PropertyNameID", nPropertyNameId);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    PropertiesColl.Add(DataReader.GetString(0));
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Adds a new row to the Table1 Table2 table.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddTable1Table2Value(
            System.Data.Common.DbCommand Command,
            System.String sTable1Name,
            System.String sTable2Name,
            System.Int32 nTable1Id,
            System.Int32 nTable2Id,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("INSERT INTO ");
                sb.Append(sTable1Name);
                sb.Append(sTable2Name);
                sb.Append(" (");
                sb.Append(sTable1Name);
                sb.Append("ID, ");
                sb.Append(sTable2Name);
                sb.Append("ID) VALUES (?, ?);");

                s_DbAdapter.AddCommandParameter(Command, "@Table1Id", nTable1Id);
                s_DbAdapter.AddCommandParameter(Command, "@Table2Id", nTable2Id);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Adds a new row to the Table1 Table2 table.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddTable1Table2Value(
            System.Data.Common.DbCommand Command,
            System.String sTable1Name,
            System.String sTable2Name,
            System.Int32 nTable1Id,
            System.Int32 nTable2Id,
            out System.Int32 nNewTable1Table2Id,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            nNewTable1Table2Id = -1;
            sErrorMessage = "";

            try
            {
                sb.Append("INSERT INTO ");
                sb.Append(sTable1Name);
                sb.Append(sTable2Name);
                sb.Append(" (");
                sb.Append(sTable1Name);
                sb.Append("ID, ");
                sb.Append(sTable2Name);
                sb.Append("ID) VALUES (?, ?);");

                s_DbAdapter.AddCommandParameter(Command, "@Table1Id", nTable1Id);
                s_DbAdapter.AddCommandParameter(Command, "@Table2Id", nTable2Id);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                if (s_DbAdapter.GetIdentityValue(Command, ref nNewTable1Table2Id,
                                                 ref sErrorMessage))
                {
                    bResult = true;
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Adds a new name to the given table name's property.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddTableProperty(
            System.String sTableName,
            System.String sNewValueName,
            System.Int32 nPropertyNameId,
            ref Common.Collections.StringSortedList<System.Int32> PropertyList,
            out System.Int32 nNewPropertyId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";
            nNewPropertyId = -1;

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == AddTableProperty(Command,
                                             sTableName,
                                             nPropertyNameId,
                                             sNewValueName,
                                             out nNewPropertyId,
                                             out sErrorMessage))
                {
                    Transaction.Commit();

                    PropertyList.Add(sNewValueName, nNewPropertyId);

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Adds a new row to the table name's property table.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddTableProperty(
            System.Data.Common.DbCommand Command,
            System.String sTableName,
            System.Int32 nPropertyNameId,
            System.String sPropertyValue,
            out System.Int32 nNewPropertyId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.Int32 nNewPropertyValueId = -1;

            nNewPropertyId = -1;
            sErrorMessage = "";

            try
            {
                if (false == AddNameToTable(Command,
                                            sTableName + "PropertyValue",
                                            sPropertyValue,
                                            out nNewPropertyValueId,
                                            out sErrorMessage))
                {
                    return false;
                }

                sb.Append("INSERT INTO ");
                sb.Append(sTableName);
                sb.Append("Property (");
                sb.Append(sTableName);
                sb.Append("PropertyNameID, ");
                sb.Append(sTableName);
                sb.Append("PropertyValueID) VALUES (?, ");
                sb.Append("?);");

                s_DbAdapter.AddCommandParameter(Command, "@PropertyNameId",
                                                nPropertyNameId);
                s_DbAdapter.AddCommandParameter(Command, "@NewPropertyValueId",
                                                nNewPropertyValueId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                if (s_DbAdapter.GetIdentityValue(Command, ref nNewPropertyId,
                                                 ref sErrorMessage))
                {
                    bResult = true;
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Changes the name of an existing table name property.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean UpdateTableProperty(
            System.String sTableName,
            System.Int32 nPropertyId,
            System.String sValueName,
            ref Common.Collections.StringSortedList<System.Int32> PropertyList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == UpdateTableProperty(Command, sTableName,
                                                nPropertyId, sValueName,
                                                out sErrorMessage))
                {
                    Transaction.Commit();

                    PropertyList.RemoveAt(PropertyList.IndexOfValue(nPropertyId));
                    PropertyList.Add(sValueName, nPropertyId);

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Updates the given table name's property.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean UpdateTableProperty(
            System.Data.Common.DbCommand Command,
            System.String sTableName,
            System.Int32 nPropertyId,
            System.String sPropertyValue,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("UPDATE ");
                sb.Append(sTableName);
                sb.Append("PropertyValue ");
                sb.Append("SET ");
                sb.Append(sTableName);
                sb.Append("PropertyValue.Name = ? ");
                sb.Append("WHERE ");
                sb.Append(sTableName);
                sb.Append("PropertyValue.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID IN (SELECT ");
                sb.Append(sTableName);
                sb.Append("PropertyValue.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID FROM ");
                sb.Append(sTableName);
                sb.Append("PropertyValue INNER JOIN ");
                sb.Append(sTableName);
                sb.Append("Property ON ");
                sb.Append(sTableName);
                sb.Append("PropertyValue.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID = ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID WHERE ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyID = ");
                sb.Append(s_DbAdapter.GetUpdateWithParameterizedSubQuerySupported() ? "?" : nPropertyId.ToString());
                sb.Append(");");

                s_DbAdapter.AddCommandParameter(Command, "@PropertyValue",
                                                sPropertyValue);

                if (s_DbAdapter.GetUpdateWithParameterizedSubQuerySupported())
                {
                    s_DbAdapter.AddCommandParameter(Command, "@PropertyID",
                                                    nPropertyId);
                }

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Deletes a part property value.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeletePartPropertyValue(
            System.Int32 nPartPropertyId,
            ref Common.Collections.StringSortedList<System.Int32> PartPropertyList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;
            System.Int32 nIndex;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == DeleteNameFromTable(Command, CPartPropertyTableName,
                                                nPartPropertyId,
                                                out sErrorMessage) &&
                    true == DeleteTablePropertyValues(Command, CPartTableName,
                                                      out sErrorMessage))
                {
                    Transaction.Commit();

                    nIndex = PartPropertyList.IndexOfValue(nPartPropertyId);

                    PartPropertyList.RemoveAt(nIndex);

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Finds the property value for the given table's property name.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean GetTablePropertyValue(
            System.String sTableName,
            System.Int32 nTableId,
            System.String sPropertyName,
            out System.String sPropertyValue,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            Common.Collections.StringCollection StringColl;

            sPropertyValue = "";
            sErrorMessage = "";

            if (true == GetTablePropertyValues(sTableName, nTableId,
                                               sPropertyName,
                                               out StringColl,
                                               out sErrorMessage))
            {
                if (StringColl.Count == 1)
                {
                    bResult = true;

                    sPropertyValue = StringColl[0];
                }
            }

            if (bResult == false)
            {
                sErrorMessage = "This " + sTableName + " does not exist.";
            }

            return bResult;
        }

        /// <summary>
        /// Finds all of the property values for the given table's property name.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean GetTablePropertyValues(
            System.String sTableName,
            System.Int32 nTableId,
            System.String sPropertyName,
            out Common.Collections.StringCollection StringColl,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            StringColl = new Common.Collections.StringCollection();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT ");
                sb.Append(sTableName);
                sb.Append("PropertyValue.Name FROM (");
                sb.Append(sTableName);
                sb.Append("PropertyValue INNER JOIN (");
                sb.Append(sTableName);
                sb.Append("PropertyName INNER JOIN ");
                sb.Append(sTableName);
                sb.Append("Property ON ");
                sb.Append(sTableName);
                sb.Append("PropertyName.");
                sb.Append(sTableName);
                sb.Append("PropertyNameID = ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyNameID) ON ");
                sb.Append(sTableName);
                sb.Append("PropertyValue.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID = ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID) INNER JOIN ");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property ON ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyID = ");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyID WHERE (");
                sb.Append(sTableName);
                sb.Append("PropertyName.Name = ?) AND (");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("ID = ?);");

                s_DbAdapter.AddCommandParameter(Command, "@PropertyName", sPropertyName);
                s_DbAdapter.AddCommandParameter(Command, "@TableID", nTableId);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    StringColl.Add(DataReader.GetString(0));
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Retrieves all of the data that uses the given part property value.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean GetDataForPropertyNameAndValue(
            System.String sTableName,
            System.Int32 nPropertyNameId,
            System.String sPropertyValue,
            out System.Collections.Specialized.StringCollection StringColl,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            StringColl = new System.Collections.Specialized.StringCollection();
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT ");
                sb.Append(sTableName);
                sb.Append(".Name ");
                sb.Append("FROM ");
                sb.Append(sTableName);
                sb.Append("PropertyValue INNER JOIN (");
                sb.Append(sTableName);
                sb.Append("Property INNER JOIN (");
                sb.Append(sTableName);
                sb.Append(" INNER JOIN ");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property ON ");
                sb.Append(sTableName);
                sb.Append(".");
                sb.Append(sTableName);
                sb.Append("ID = ");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("ID) ON ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyID = ");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyID) ON ");
                sb.Append(sTableName);
                sb.Append("PropertyValue.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID = ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID ");
                sb.Append("WHERE ((");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyID = ?) AND (");
                sb.Append(sTableName);
                sb.Append("PropertyValue.Name = ?)) ");
                sb.Append("ORDER BY ");
                sb.Append(sTableName);
                sb.Append(".Name;");

                s_DbAdapter.AddCommandParameter(Command, "@" + sTableName + "PropertyNameID", nPropertyNameId);
                s_DbAdapter.AddCommandParameter(Command, "@" + sTableName + "PropertyValue", sPropertyValue);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    StringColl.Add(DataReader.GetString(0));
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }

                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Adds a new name to the given table.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddNameToTable(
            System.String sTableName,
            System.String sNewName,
            ref Common.Collections.StringSortedList<System.Int32> NameList,
            out System.Int32 nNewNameId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            nNewNameId = -1;
            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == AddNameToTable(Command, sTableName, sNewName,
                                           out nNewNameId, out sErrorMessage))
                {
                    Transaction.Commit();

                    NameList.Add(sNewName, nNewNameId);

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Adds a new row to the given table.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddNameToTable(
            System.Data.Common.DbCommand Command,
            System.String sTableName,
            System.String sNewName,
            out System.Int32 nNewNameId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb;

            nNewNameId = -1;
            sErrorMessage = "";

            try
            {
                sb = new System.Text.StringBuilder();

                sb.Append("INSERT INTO ");
                sb.Append(sTableName);
                sb.Append(" (Name) VALUES (?);");

                s_DbAdapter.AddCommandParameter(Command, "@NewName", sNewName);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                if (s_DbAdapter.GetIdentityValue(Command, ref nNewNameId,
                                                 ref sErrorMessage))
                {
                    bResult = true;
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Adds a new row to the TableTableProperty table as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean AddTableTableProperty(
            System.Data.Common.DbCommand Command,
            System.String sTableName,
            System.Int32 nTableId,
            System.Int32 nTablePropertyId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("INSERT INTO ");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property (");
                sb.Append(sTableName);
                sb.Append("ID, ");
                sb.Append(sTableName);
                sb.Append("PropertyID) VALUES (?, ?);");

                s_DbAdapter.AddCommandParameter(Command, "@TableID", nTableId);
                s_DbAdapter.AddCommandParameter(Command, "@TablePropertyID", nTablePropertyId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Updates the given TableTableProperty row as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean UpdateTableTableProperty(
            System.Data.Common.DbCommand Command,
            System.String sTableName,
            System.Int32 nTableId,
            System.Int32 nOriginalTablePropertyId,
            System.Int32 nNewTablePropertyId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("UPDATE ");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property SET ");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyID = ? WHERE (");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("ID = ?) AND (");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyID = ?);");

                s_DbAdapter.AddCommandParameter(Command, "@NewTablePropertyID", nNewTablePropertyId);
                s_DbAdapter.AddCommandParameter(Command, "@TableID", nTableId);
                s_DbAdapter.AddCommandParameter(Command, "@OriginalTablePropertyID", nOriginalTablePropertyId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Deletes all of the given table's associated properties.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeleteTableTableProperties(
            System.Data.Common.DbCommand Command,
            System.String sTableName,
            System.Int32 nTableId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("DELETE FROM ");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property WHERE ");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("ID = ?;");

                s_DbAdapter.AddCommandParameter(Command, "@TableID", nTableId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Changes the name of a row within a table.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean UpdateNameOfTable(
            System.String sTableName,
            System.Int32 nNameId,
            System.String sNewName,
            ref Common.Collections.StringSortedList<System.Int32> NamePropertyList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == UpdateNameOfTable(Command, sTableName, nNameId,
                                              sNewName, out sErrorMessage))
                {
                    Transaction.Commit();

                    NamePropertyList.RemoveAt(NamePropertyList.IndexOfValue(nNameId));
                    NamePropertyList.Add(sNewName, nNameId);

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Updates the given name value of a row within the given table.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean UpdateNameOfTable(
            System.Data.Common.DbCommand Command,
            System.String sTableName,
            System.Int32 nNameId,
            System.String sNameValue,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("UPDATE ");
                sb.Append(sTableName);
                sb.Append(" ");
                sb.Append("SET ");
                sb.Append(sTableName);
                sb.Append(".Name = ? ");
                sb.Append("WHERE ");
                sb.Append(sTableName);
                sb.Append(".");
                sb.Append(sTableName);
                sb.Append("ID = ?;");

                s_DbAdapter.AddCommandParameter(Command, "@Name", sNameValue);
                s_DbAdapter.AddCommandParameter(Command, "@NameID", nNameId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Delete's the given rows from the Table1 Table2 table as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeleteTable1Table2ByTable1Id(
            System.Data.Common.DbCommand Command,
            System.String sTable1Name,
            System.String sTable2Name,
            System.Int32 nTable1Id,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("DELETE FROM ");
                sb.Append(sTable1Name);
                sb.Append(sTable2Name);
                sb.Append(" WHERE ");
                sb.Append(sTable1Name);
                sb.Append(sTable2Name);
                sb.Append(".");
                sb.Append(sTable1Name);
                sb.Append("ID = ?;");
                
                s_DbAdapter.AddCommandParameter(Command, "@Table1ID", nTable1Id);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Delete's all unused rows from a property table, name table and
        /// value table as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeleteUnusedProperties(
            System.Data.Common.DbCommand Command,
            System.String sTableName,
            out System.String sErrorMessage)
        {
            if (true == DeleteUnusedFromTableProperty(Command, sTableName,
                                                      out sErrorMessage) &&
                true == DeleteUnusedFromTablePropertyName(Command, sTableName,
                                                          out sErrorMessage) &&
                true == DeleteUnusedFromTablePropertyValue(Command, sTableName,
                                                           out sErrorMessage))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Delete's all unused rows from a property table as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeleteUnusedFromTableProperty(
            System.Data.Common.DbCommand Command,
            System.String sTableName,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("DELETE FROM ");
                sb.Append(sTableName);
                sb.Append("Property WHERE ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyID NOT IN (SELECT ");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyID FROM ");
                sb.Append(sTableName);
                sb.Append(sTableName);
                sb.Append("Property);");

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Delete's all unused rows from a property name table as part of
        /// a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeleteUnusedFromTablePropertyName(
            System.Data.Common.DbCommand Command,
            System.String sTableName,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("DELETE FROM ");
                sb.Append(sTableName);
                sb.Append("PropertyName WHERE ");
                sb.Append(sTableName);
                sb.Append("PropertyName.");
                sb.Append(sTableName);
                sb.Append("PropertyNameID NOT IN (SELECT ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyNameID FROM ");
                sb.Append(sTableName);
                sb.Append("Property);");

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Delete's all unused rows from a property value table as part of
        /// a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeleteUnusedFromTablePropertyValue(
            System.Data.Common.DbCommand Command,
            System.String sTableName,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("DELETE FROM ");
                sb.Append(sTableName);
                sb.Append("PropertyValue WHERE ");
                sb.Append(sTableName);
                sb.Append("PropertyValue.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID NOT IN (SELECT ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID FROM ");
                sb.Append(sTableName);
                sb.Append("Property);");

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Delete's the given rows from the Table1 Table2 table as part of a transaction.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeleteTable1Table2ByTable2Id(
            System.Data.Common.DbCommand Command,
            System.String sTable1Name,
            System.String sTable2Name,
            System.Int32 nTable2Id,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("DELETE FROM ");
                sb.Append(sTable1Name);
                sb.Append(sTable2Name);
                sb.Append(" WHERE ");
                sb.Append(sTable1Name);
                sb.Append(sTable2Name);
                sb.Append(".");
                sb.Append(sTable2Name);
                sb.Append("ID = ?;");

                s_DbAdapter.AddCommandParameter(Command, "@Table2ID", nTable2Id);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Deletes a name from a table.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeleteNameFromTable(
            System.String sTableName,
            System.Int32 nNameId,
            ref Common.Collections.StringSortedList<System.Int32> NameList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;
            System.Int32 nIndex;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == DeleteNameFromTable(Command, sTableName, nNameId,
                                                out sErrorMessage))
                {
                    Transaction.Commit();

                    nIndex = NameList.IndexOfValue(nNameId);

                    NameList.RemoveAt(nIndex);

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Deletes a row from the given table.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeleteNameFromTable(
            System.Data.Common.DbCommand Command,
            System.String sTableName,
            System.Int32 nNameId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("DELETE FROM ");
                sb.Append(sTableName);
                sb.Append(" ");
                sb.Append("WHERE (");
                sb.Append(sTableName);
                sb.Append(".");
                sb.Append(sTableName);
                sb.Append("ID = ?);");

                s_DbAdapter.AddCommandParameter(Command, "@NameID", nNameId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Deletes a name from a property table and it's corresponding property value.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeleteTableProperty(
            System.String sTableName,
            System.Int32 nPropertyId,
            ref Common.Collections.StringSortedList<System.Int32> DataList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbTransaction Transaction = null;
            System.Data.Common.DbCommand Command = null;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Transaction = Connection.BeginTransaction();

                Command = Connection.CreateCommand();

                Command.Transaction = Transaction;

                if (true == DeleteTablePropertyName(Command, sTableName,
                                                    nPropertyId,
                                                    out sErrorMessage) &&
                    true == DeleteTablePropertyValues(Command,
                                                      sTableName,
                                                      out sErrorMessage))
                {
                    Transaction.Commit();

                    DataList.RemoveAt(DataList.IndexOfValue(nPropertyId));

                    bResult = true;
                }
                else
                {
                    Transaction.Rollback();
                }
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (Transaction != null)
                {
                    try
                    {
                        Transaction.Rollback();
                    }
                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Deletes all unused rows from the Table Property table for a given Table Property Name Id.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeleteTablePropertyName(
            System.Data.Common.DbCommand Command,
            System.String sTableName,
            System.Int32 nPropertyNameId,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("DELETE ");
                sb.Append(sTableName);
                sb.Append("Property FROM ");
                sb.Append(sTableName);
                sb.Append("Property WHERE (");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyID = ?);");

                s_DbAdapter.AddCommandParameter(Command, "@PropertyID", nPropertyNameId);

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Deletes all unused rows from the Table PropertyValue table.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean DeleteTablePropertyValues(
            System.Data.Common.DbCommand Command,
            System.String sTableName,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sErrorMessage = "";

            try
            {
                sb.Append("DELETE FROM ");
                sb.Append(sTableName);
                sb.Append("PropertyValue WHERE ");
                sb.Append(sTableName);
                sb.Append("PropertyValue.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID NOT IN (SELECT ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID FROM ");
                sb.Append(sTableName);
                sb.Append("Property);");

                Command.CommandText = sb.ToString();

                Command.ExecuteNonQuery();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                sErrorMessage = Exception.Message;
            }

            Command.Parameters.Clear();

            return bResult;
        }

        /// <summary>
        /// Retrieves and populates cache with the with the given table's property's name.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean InitTablePropertyValues(
            System.String sTableName,
            System.String sPropertyName,
            ref Common.Collections.StringSortedList<System.Int32> PropertyList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyID, ");
                sb.Append(sTableName);
                sb.Append("PropertyValue.Name ");
                sb.Append("FROM ");
                sb.Append(sTableName);
                sb.Append("PropertyValue INNER JOIN (");
                sb.Append(sTableName);
                sb.Append("PropertyName ");
                sb.Append("INNER JOIN ");
                sb.Append(sTableName);
                sb.Append("Property ON ");
                sb.Append(sTableName);
                sb.Append("PropertyName.");
                sb.Append(sTableName);
                sb.Append("PropertyNameID = ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyNameID) ON ");
                sb.Append(sTableName);
                sb.Append("PropertyValue.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID = ");
                sb.Append(sTableName);
                sb.Append("Property.");
                sb.Append(sTableName);
                sb.Append("PropertyValueID WHERE (");
                sb.Append(sTableName);
                sb.Append("PropertyName.Name = ?) ORDER BY ");
                sb.Append(sTableName);
                sb.Append("PropertyValue.Name;");

                s_DbAdapter.AddCommandParameter(Command, "@" + sTableName + "PropertyName", sPropertyName);

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    PropertyList.Add(DataReader.GetString(1), DataReader.GetInt32(0));
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }

                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Retrieves and populates a cache with the data from the given table.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean InitTableDataList(
            System.String sTableName,
            ref Common.Collections.StringSortedList<System.Int32> DataList,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT ");
                sb.Append(sTableName);
                sb.Append(".");
                sb.Append(sTableName);
                sb.Append("ID, ");
                sb.Append(sTableName);
                sb.Append(".Name ");
                sb.Append(" FROM ");
                sb.Append(sTableName);
                sb.Append(" ORDER BY ");
                sb.Append(sTableName);
                sb.Append(".Name;");

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    DataList.Add(DataReader.GetString(1), DataReader.GetInt32(0));
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }

                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Retrieves and populates a cache with the available property names and their identifiers.
        /// <param name="sErrorMessage">
        /// On return will contain a message if an error occurred.
        /// </param>
        /// </summary>

        private static System.Boolean InitTablePropertyNames(
            System.String sTableName,
            ref Common.Collections.StringDictionary<System.Int32> PropertyNameDictionary,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.Data.Common.DbConnection Connection = null;
            System.Data.Common.DbDataReader DataReader = null;
            System.Data.Common.DbCommand Command;
            System.Text.StringBuilder sb;
            System.String sTmpErrorMessage = null;

            sErrorMessage = "";

            try
            {
                if (!s_DbAdapter.AllocConnection(ref Connection,
                                                 ref sErrorMessage))
                {
                    return false;
                }

                Command = Connection.CreateCommand();

                sb = new System.Text.StringBuilder();

                sb.Append("SELECT ");
                sb.Append(sTableName);
                sb.Append("PropertyName.");
                sb.Append(sTableName);
                sb.Append("PropertyNameID, ");
                sb.Append(sTableName);
                sb.Append("PropertyName.Name FROM ");
                sb.Append(sTableName);
                sb.Append("PropertyName;");

                Command.CommandText = sb.ToString();

                DataReader = Command.ExecuteReader();

                while (DataReader.Read())
                {
                    PropertyNameDictionary.Add(DataReader.GetString(1), DataReader.GetInt32(0));
                }

                DataReader.Close();

                bResult = true;
            }

            catch (System.Data.Common.DbException Exception)
            {
                if (DataReader != null)
                {
                    try
                    {
                        DataReader.Close();
                    }

                    catch (System.Data.Common.DbException)
                    {
                    }
                }

                sErrorMessage = Exception.Message;
            }

            s_DbAdapter.FreeConnection(Connection, ref sTmpErrorMessage);

            return bResult;
        }

        /// <summary>
        /// Find the identifier of the given data.
        /// </summary>

        private static System.Int32 GetListDataId(
            System.String sListData,
            Common.Collections.StringSortedList<System.Int32> List)
        {
            System.Int32 nIndex = List.IndexOfKey(sListData);

            if (nIndex == -1)
            {
                return -1;
            }

            return (System.Int32)List.GetValueList()[nIndex];
        }

#if USE_DATABASE_PLUGIN
        /// <summary>
        /// Creates the database accessor from the given plugin.
        /// </summary>

        private static Common.Data.IDbAdapter CreateDatabasePlugin(
            System.String sPlugin,
            out System.String sErrorMessage)
        {
            System.Reflection.Assembly assembly;
            System.Type[] types;
            System.Object obj;

            sErrorMessage = "";

            try
            {
                assembly = System.Reflection.Assembly.LoadFile(sPlugin);
            }

            catch (System.Exception exception)
            {
                sErrorMessage = exception.Message;

                return null;
            }

            try
            {
                types = assembly.GetExportedTypes();

                for (System.Int32 nIndex = 0; nIndex < types.Length; ++nIndex)
                {
                    if (types[nIndex].IsClass == true &&
                        types[nIndex].IsAbstract == false &&
                        DoesDatabaseInterfaceExist(types[nIndex].GetInterfaces()))
                    {
                        obj = System.Activator.CreateInstance(types[nIndex], false);

                        return (Common.Data.IDbAdapter)obj;
                    }
                }

                sErrorMessage = "Object implementing the Common.Data.IDbAdapter interface was not found.";
            }

            catch (System.Exception exception)
            {
                sErrorMessage = exception.Message;
            }

            return null;
        }

        /// <summary>
        /// Checks if the database interface is present.
        /// </summary>

        private static System.Boolean DoesDatabaseInterfaceExist(
            System.Type[] types)
        {
            foreach (System.Type Type in types)
            {
                if (Type == typeof(Common.Data.IDbAdapter))
                {
                    return true;
                }
            }

            return false;
        }
#endif
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2016 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
