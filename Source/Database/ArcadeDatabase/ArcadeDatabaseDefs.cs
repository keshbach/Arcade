/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;

namespace Arcade
{
    /// <summary>
    /// Database types and definitions.
    /// </summary>

    public sealed class DatabaseDefs
    {
        public struct TPart
        {
            public System.Int32 nPartId;
            public System.String sPartName;
            public System.String sPartCategoryName;
            public System.String sPartTypeName;
            public System.String sPartPackageName;
            public System.Boolean bPartIsDefault;
            public System.Int32 nPartTotalInventory;
            public Common.Collections.StringCollection PartDatasheetColl;
        };

        public struct TPartLens
        {
            public System.Int32 nPartNameLen;
            public System.Int32 nPartCategoryNameLen;
            public System.Int32 nPartTypeNameLen;
            public System.Int32 nPartPackageNameLen;
            public System.Int32 nPartDatasheetLen;
            public System.Int32 nPartPinoutsLen;
        };

        public struct TGame
        {
            public System.Int32 nGameId;
            public System.String sGameName;
            public System.String sGamePinouts;
            public System.String sGameDipSwitches;
            public System.String sGameDescription;
            public System.String sManufacturer;
            public System.String sGameWiringHarness;
            public System.Boolean bGameHaveWiringHarness;
            public System.Boolean bGameNeedPowerOnReset;
            public System.String sGameCocktail;
            public Common.Collections.StringCollection GameAudioColl;
            public Common.Collections.StringCollection GameControlsColl;
            public Common.Collections.StringCollection GameVideoColl;
        };

        public struct TGameLens
        {
            public System.Int32 nGameNameLen;
            public System.Int32 nGamePinoutsLen;
            public System.Int32 nGameDipSwitchesLen;
            public System.Int32 nGameDescriptionLen;
            public System.Int32 nManufacturerLen;
            public System.Int32 nGameWiringHarnessLen;
            public System.Int32 nGameCocktailLen;
            public System.Int32 nGameAudioLen;
            public System.Int32 nGameVideoLen;
            public System.Int32 nGameControlLen;
        };

        public struct TBoard
        {
            public System.Int32 nBoardId;
            public System.String sBoardName;
            public System.String sBoardDescription;
            public System.String sBoardSize;
            public System.String sBoardTypeName;
        };

        public struct TBoardLens
        {
            public System.Int32 nBoardNameLen;
            public System.Int32 nBoardDescriptionLen;
            public System.Int32 nBoardSizeLen;
            public System.Int32 nBoardTypeNameLen;
        };

        public struct TBoardPart
        {
            public TBoardPartLocation BoardPartLocation;
            public DatabaseDefs.TPart Part;
        };

        public struct TBoardPartLocation
        {
            public System.Int32 nBoardPartId;
            public System.String sBoardPartPosition;
            public System.String sBoardPartLocation;
            public System.String sBoardPartDescription;
        };

        public struct TBoardPartLocationLens
        {
            public System.Int32 nBoardPartPositionLen;
            public System.Int32 nBoardPartLocationLen;
            public System.Int32 nBoardPartDescriptionLen;
        };

        public struct TManual
        {
            public System.Int32 nManualId;
            public System.String sManualName;
            public System.String sManualPartNumber;
            public System.Int32 nManualYearPrinted;
            public System.Boolean bManualComplete;
            public System.Boolean bManualOriginal;
            public System.String sManualDescription;
            public System.String sManualPrintEdition;
            public System.String sManualCondition;
            public System.String sManualStorageBox;
            public System.String sManufacturer;
        };

        public struct TManualLens
        {
            public System.Int32 nManualNameLen;
            public System.Int32 nManualPartNumberLen;
            public System.Int32 nManualDescriptionLen;
            public System.Int32 nManualPrintEditionLen;
            public System.Int32 nManualConditionLen;
            public System.Int32 nManualStorageBoxLen;
            public System.Int32 nManufacturerLen;
        };

        public struct TDisplay
        {
            public System.Int32 nDisplayId;
            public System.String sDisplayName;
            public System.String sDisplayType;
            public System.String sDisplayResolution;
            public System.String sDisplayColors;
            public System.String sDisplayOrientation;
        };

        public struct TDisplayLens
        {
            public System.Int32 nDisplayNameLen;
            public System.Int32 nDisplayTypeLen;
            public System.Int32 nDisplayResolutionLen;
            public System.Int32 nDisplayColorsLen;
            public System.Int32 nDisplayOrientationLen;
        };

        public struct TLog
        {
            public System.Int32 nLogId;
            public System.DateTime DateTime;
            public System.String sLogType;
            public System.String sLogDescription;
        };

        public struct TLogLens
        {
            public System.Int32 nLogDescriptionLen;
            public System.Int32 nLogTypeLen;
        };

        public struct TInventory
        {
            public System.Int32 nInventoryId;
            public System.Int32 nCount;
            public System.DateTime DateTime;
            public System.String sInventoryDescription;
        }

        public struct TInventoryLens
        {
            public System.Int32 nInventoryDescriptionLen;
        };

        #region "Enumerations"
        public enum EKeywordMatchingCriteria
        {
            Start = 0,
            Anywhere = 1
        };

        public enum EPartDataType
        {
            Category = 0,
            Type = 1,
            Package = 2
        };

        public enum EGameDataType
        {
            AudioProperty = 0,
            WiringHarness = 1,
            ControlProperty = 2,
            VideoProperty = 3,
            Cocktail = 4
        };

        public enum EDisplayDataType
        {
            Type = 0,
            Resolution = 1,
            Colors = 2,
            Orientation = 3
        };

        public enum EManualDataType
        {
            StorageBox = 0,
            PrintEdition = 1,
            Condition = 2
        }
        #endregion

        #region "Constants"
        public static System.String CCartridgeName = "Cartridge";
        #endregion

        #region "Constructor"
        private DatabaseDefs()
        {
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
