/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class ListDataForm : Arcade.Forms.Form
    {
        #region "Enumerations"
        public enum EListDataFormType
        {
            PartCategoryData,
            PartTypeData,
            PartPackageData,
            GameWiringHarnessData,
            GameCocktailData,
            GameControlData,
            GameBoardTypeData,
            GameAudio,
            GameVideo,
            GameBoardPartLocation,
            ManualStorageBox,
            ManualPrintEdition,
            ManualCondition,
            ManufacturerData,
            LogTypeData,
            DisplayType,
            DisplayResolution,
            DisplayColors,
            DisplayOrientation
        };
        #endregion

        #region "Member Variables"
        private EListDataFormType m_ListDataFormType = EListDataFormType.PartCategoryData;
        private Common.Collections.StringSortedList<System.Int32> m_DataList = null;
        private System.Int32 m_nMaxDataValueLen = 0;
        #endregion

        #region "Constructor"
        public ListDataForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Properties"
        public EListDataFormType ListDataFormType
        {
            set
            {
                m_ListDataFormType = value;
            }
        }
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { listViewData };
            }
        }
        #endregion

        #region "List Data Event Handlers"
        private void ListDataForm_Shown(object sender, EventArgs e)
        {
            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                InitializeControls();

                RunOnUIThreadWait(() =>
                {
                    this.BusyControlVisible = false;
                });
            }, "List Data Form Initialize Thread");
        }
        #endregion

        #region "List View Event Handlers"
        private void listViewData_DoubleClick(object sender, EventArgs e)
        {
            EditData();
        }

        private void listViewData_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            buttonEdit.Enabled = true;
            buttonDelete.Enabled = true;
            buttonDetails.Enabled = true;
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            DataEntryForm DataEntry = new DataEntryForm();
            System.Boolean bResult;
            System.Int32 nNewDataId;
            System.String sErrorMessage;
            System.Int32 nIndex;
            System.Windows.Forms.ListViewItem Item;

            new Common.Forms.FormLocation(DataEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            DataEntry.DataEntryFormType = Arcade.Forms.DataEntryForm.EDataEntryFormType.NewData;
            DataEntry.MaxDataNameLen = m_nMaxDataValueLen;

            if (DataEntry.ShowDialog(this) == DialogResult.OK)
            {
                if (m_DataList.ContainsKey(DataEntry.DataName))
                {
                    Common.Forms.MessageBox.Show(this, "Cannot add this name because it already exists.",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);

                    DataEntry.Dispose();

                    return;
                }

                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    switch (m_ListDataFormType)
                    {
                        case EListDataFormType.PartCategoryData:
                            bResult = Database.AddPartDataName(DatabaseDefs.EPartDataType.Category,
                                                               DataEntry.DataName,
                                                               out nNewDataId,
                                                               out sErrorMessage);
                            break;
                        case EListDataFormType.PartTypeData:
                            bResult = Database.AddPartDataName(DatabaseDefs.EPartDataType.Type,
                                                               DataEntry.DataName,
                                                               out nNewDataId,
                                                               out sErrorMessage);
                            break;
                        case EListDataFormType.PartPackageData:
                            bResult = Database.AddPartDataName(DatabaseDefs.EPartDataType.Package,
                                                               DataEntry.DataName,
                                                               out nNewDataId,
                                                               out sErrorMessage);
                            break;
                        case EListDataFormType.GameWiringHarnessData:
                            bResult = Database.AddGameDataName(DatabaseDefs.EGameDataType.WiringHarness,
                                                               DataEntry.DataName,
                                                               out nNewDataId,
                                                               out sErrorMessage);
                            break;
                        case EListDataFormType.GameControlData:
                            bResult = Database.AddGameDataName(DatabaseDefs.EGameDataType.ControlProperty,
                                                               DataEntry.DataName,
                                                               out nNewDataId,
                                                               out sErrorMessage);
                            break;
                        case EListDataFormType.GameBoardTypeData:
                            bResult = Database.AddBoardTypeDataName(DataEntry.DataName,
                                                                    out nNewDataId,
                                                                    out sErrorMessage);
                            break;
                        case EListDataFormType.GameAudio:
                            bResult = Database.AddGameDataName(DatabaseDefs.EGameDataType.AudioProperty,
                                                               DataEntry.DataName,
                                                               out nNewDataId,
                                                               out sErrorMessage);
                            break;
                        case EListDataFormType.GameVideo:
                            bResult = Database.AddGameDataName(DatabaseDefs.EGameDataType.VideoProperty,
                                                               DataEntry.DataName,
                                                               out nNewDataId,
                                                               out sErrorMessage);
                            break;
                        case EListDataFormType.GameCocktailData:
                            bResult = Database.AddGameDataName(DatabaseDefs.EGameDataType.Cocktail,
                                                               DataEntry.DataName,
                                                               out nNewDataId,
                                                               out sErrorMessage);
                            break;
                        case EListDataFormType.GameBoardPartLocation:
                            bResult = Database.AddBoardPartLocationDataName(
                                            DataEntry.DataName, out nNewDataId,
                                            out sErrorMessage);
                            break;
                        case EListDataFormType.ManualStorageBox:
                            bResult = Database.AddManualDataName(DatabaseDefs.EManualDataType.StorageBox,
                                                                 DataEntry.DataName,
                                                                 out nNewDataId,
                                                                 out sErrorMessage);
                            break;
                        case EListDataFormType.ManualPrintEdition:
                            bResult = Database.AddManualDataName(DatabaseDefs.EManualDataType.PrintEdition,
                                                                 DataEntry.DataName,
                                                                 out nNewDataId,
                                                                 out sErrorMessage);
                            break;
                        case EListDataFormType.ManualCondition:
                            bResult = Database.AddManualDataName(DatabaseDefs.EManualDataType.Condition,
                                                                 DataEntry.DataName,
                                                                 out nNewDataId,
                                                                 out sErrorMessage);
                            break;
                        case EListDataFormType.DisplayType:
                            bResult = Database.AddDisplayDataName(DatabaseDefs.EDisplayDataType.Type,
                                                                  DataEntry.DataName,
                                                                  out nNewDataId,
                                                                  out sErrorMessage);
                            break;
                        case EListDataFormType.DisplayResolution:
                            bResult = Database.AddDisplayDataName(DatabaseDefs.EDisplayDataType.Resolution,
                                                                  DataEntry.DataName,
                                                                  out nNewDataId,
                                                                  out sErrorMessage);
                            break;
                        case EListDataFormType.DisplayColors:
                            bResult = Database.AddDisplayDataName(DatabaseDefs.EDisplayDataType.Colors,
                                                                  DataEntry.DataName,
                                                                  out nNewDataId,
                                                                  out sErrorMessage);
                            break;
                        case EListDataFormType.DisplayOrientation:
                            bResult = Database.AddDisplayDataName(DatabaseDefs.EDisplayDataType.Orientation,
                                                                  DataEntry.DataName,
                                                                  out nNewDataId,
                                                                  out sErrorMessage);
                            break;
                        case EListDataFormType.ManufacturerData:
                            bResult = Database.AddManufacturerName(DataEntry.DataName,
                                                                   out nNewDataId,
                                                                   out sErrorMessage);
                            break;
                        case EListDataFormType.LogTypeData:
                            bResult = Database.AddLogTypeDataName(DataEntry.DataName,
                                                                  out nNewDataId,
                                                                  out sErrorMessage);
                            break;
                        default:
                            System.Diagnostics.Debug.Assert(false, "Unknown list data form type.");

                            bResult = false;
                            nNewDataId = -1;
                            sErrorMessage = "Unknown list data form type";
                            break;
                    }

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
                        {
                            m_DataList.Add(DataEntry.DataName, nNewDataId);

                            nIndex = m_DataList.IndexOfValue(nNewDataId);

                            Item = listViewData.Items.Insert(nIndex, DataEntry.DataName);

                            Item.Selected = true;
                            Item.Focused = true;

                            Item.EnsureVisible();

                            buttonEdit.Enabled = true;
                            buttonDelete.Enabled = true;
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }

                        this.BusyControlVisible = false;

                        DataEntry.Dispose();
                    });
                }, "List Data Form Add Thread");
            }
            else
            {
                DataEntry.Dispose();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditData();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            System.Int32 nIndex = listViewData.SelectedIndices[0];
            System.Boolean bResult;
            System.String sErrorMessage;

            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                switch (m_ListDataFormType)
                {
                    case EListDataFormType.PartCategoryData:
                        bResult = Database.DeletePartDataName(DatabaseDefs.EPartDataType.Category,
                                                              (System.Int32)m_DataList.GetValueList()[nIndex],
                                                              out sErrorMessage);
                        break;
                    case EListDataFormType.PartTypeData:
                        bResult = Database.DeletePartDataName(DatabaseDefs.EPartDataType.Type,
                                                              (System.Int32)m_DataList.GetValueList()[nIndex],
                                                              out sErrorMessage);
                        break;
                    case EListDataFormType.PartPackageData:
                        bResult = Database.DeletePartDataName(DatabaseDefs.EPartDataType.Package,
                                                              (System.Int32)m_DataList.GetValueList()[nIndex],
                                                              out sErrorMessage);
                        break;
                    case EListDataFormType.GameWiringHarnessData:
                        bResult = Database.DeleteGameDataName(DatabaseDefs.EGameDataType.WiringHarness,
                                                              (System.Int32)m_DataList.GetValueList()[nIndex],
                                                              out sErrorMessage);
                        break;
                    case EListDataFormType.GameControlData:
                        bResult = Database.DeleteGameDataName(DatabaseDefs.EGameDataType.ControlProperty,
                                                              (System.Int32)m_DataList.GetValueList()[nIndex],
                                                              out sErrorMessage);
                        break;
                    case EListDataFormType.GameBoardTypeData:
                        bResult = Database.DeleteBoardTypeDataName(
                                        (System.Int32)m_DataList.GetValueList()[nIndex],
                                        out sErrorMessage);
                        break;
                    case EListDataFormType.GameAudio:
                        bResult = Database.DeleteGameDataName(DatabaseDefs.EGameDataType.AudioProperty,
                                                              (System.Int32)m_DataList.GetValueList()[nIndex],
                                                              out sErrorMessage);
                        break;
                    case EListDataFormType.GameVideo:
                        bResult = Database.DeleteGameDataName(DatabaseDefs.EGameDataType.VideoProperty,
                                                              (System.Int32)m_DataList.GetValueList()[nIndex],
                                                              out sErrorMessage);
                        break;
                    case EListDataFormType.GameCocktailData:
                        bResult = Database.DeleteGameDataName(DatabaseDefs.EGameDataType.Cocktail,
                                                              (System.Int32)m_DataList.GetValueList()[nIndex],
                                                              out sErrorMessage);
                        break;
                    case EListDataFormType.GameBoardPartLocation:
                        bResult = Database.DeleteBoardPartLocationDataName(
                                        (System.Int32)m_DataList.GetValueList()[nIndex],
                                        out sErrorMessage);
                        break;
                    case EListDataFormType.ManualStorageBox:
                        bResult = Database.DeleteManualDataName(DatabaseDefs.EManualDataType.StorageBox,
                                                                (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                out sErrorMessage);
                        break;
                    case EListDataFormType.ManualPrintEdition:
                        bResult = Database.DeleteManualDataName(DatabaseDefs.EManualDataType.PrintEdition,
                                                                (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                out sErrorMessage);
                        break;
                    case EListDataFormType.ManualCondition:
                        bResult = Database.DeleteManualDataName(DatabaseDefs.EManualDataType.Condition,
                                                                (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                out sErrorMessage);
                        break;
                    case EListDataFormType.DisplayOrientation:
                        bResult = Database.DeleteDisplayDataName(DatabaseDefs.EDisplayDataType.Orientation,
                                                                 (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                 out sErrorMessage);
                        break;
                    case EListDataFormType.DisplayResolution:
                        bResult = Database.DeleteDisplayDataName(DatabaseDefs.EDisplayDataType.Resolution,
                                                                 (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                 out sErrorMessage);
                        break;
                    case EListDataFormType.DisplayType:
                        bResult = Database.DeleteDisplayDataName(DatabaseDefs.EDisplayDataType.Type,
                                                                 (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                 out sErrorMessage);
                        break;
                    case EListDataFormType.DisplayColors:
                        bResult = Database.DeleteDisplayDataName(DatabaseDefs.EDisplayDataType.Colors,
                                                                 (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                 out sErrorMessage);
                        break;
                    case EListDataFormType.ManufacturerData:
                        bResult = Database.DeleteManufacturerName((System.Int32)m_DataList.GetValueList()[nIndex],
                                                                  out sErrorMessage);
                        break;
                    case EListDataFormType.LogTypeData:
                        bResult = Database.DeleteLogTypeDataName((System.Int32)m_DataList.GetValueList()[nIndex],
                                                                 out sErrorMessage);
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false, "Unknown list data form type.");

                        bResult = false;
                        sErrorMessage = "Unknown list data form type";
                        break;
                }

                RunOnUIThreadWait(() =>
                {
                    if (bResult)
                    {
                        m_DataList.RemoveAt(nIndex);

                        listViewData.Items.RemoveAt(nIndex);

                        if (listViewData.Items.Count > 0)
                        {
                            if (nIndex == listViewData.Items.Count)
                            {
                                --nIndex;
                            }

                            listViewData.Items[nIndex].Selected = true;
                            listViewData.Items[nIndex].Focused = true;

                            listViewData.Items[nIndex].EnsureVisible();
                        }
                        else
                        {
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

                    this.BusyControlVisible = false;
                });
            }, "List Data Form Delete Thread");
        }

        private void buttonDetails_Click(object sender, EventArgs e)
        {
            System.Int32 nIndex = listViewData.SelectedIndices[0];
            System.Int32 nCategoryNameId = (System.Int32)m_DataList.GetValueList()[nIndex];
            System.String sCategoryValue = (System.String)m_DataList.GetKeyList()[nIndex];
            System.Collections.Specialized.StringCollection DataCollection = new System.Collections.Specialized.StringCollection();
            System.Boolean bResult = false;
            System.String sErrorMessage;
            System.String sFile;

            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                switch (m_ListDataFormType)
                {
                    case EListDataFormType.PartCategoryData:
                        bResult = Database.GetPartsUsingCategoryItemList(
                                        nCategoryNameId, sCategoryValue,
                                        out DataCollection, out sErrorMessage);
                        break;
                    case EListDataFormType.PartTypeData:
                        bResult = Database.GetPartsUsingCategoryItemList(
                                        nCategoryNameId, sCategoryValue,
                                        out DataCollection, out sErrorMessage);
                        break;
                    case EListDataFormType.PartPackageData:
                        bResult = Database.GetPartsUsingCategoryItemList(
                                        nCategoryNameId, sCategoryValue,
                                        out DataCollection, out sErrorMessage);
                        break;
                    case EListDataFormType.GameWiringHarnessData:
                        bResult = Database.GetGamesUsingCategoryItemList(
                                        nCategoryNameId, sCategoryValue,
                                        out DataCollection, out sErrorMessage);
                        break;
                    case EListDataFormType.GameCocktailData:
                        bResult = Database.GetGamesUsingCategoryItemList(
                                        nCategoryNameId, sCategoryValue,
                                        out DataCollection, out sErrorMessage);
                        break;
                    case EListDataFormType.GameControlData:
                        bResult = Database.GetGamesUsingCategoryItemList(
                                        nCategoryNameId, sCategoryValue,
                                        out DataCollection, out sErrorMessage);
                        break;
                    case EListDataFormType.GameAudio:
                        bResult = Database.GetGamesUsingCategoryItemList(
                                        nCategoryNameId, sCategoryValue,
                                        out DataCollection, out sErrorMessage);
                        break;
                    case EListDataFormType.GameVideo:
                        bResult = Database.GetGamesUsingCategoryItemList(
                                        nCategoryNameId, sCategoryValue,
                                        out DataCollection, out sErrorMessage);
                        break;
                    case EListDataFormType.ManualStorageBox:
                        bResult = Database.GetManualsUsingCategoryItemList(
                                        nCategoryNameId, sCategoryValue,
                                        out DataCollection, out sErrorMessage);
                        break;
                    case EListDataFormType.ManualPrintEdition:
                        bResult = Database.GetManualsUsingCategoryItemList(
                                        nCategoryNameId, sCategoryValue,
                                        out DataCollection, out sErrorMessage);
                        break;
                    case EListDataFormType.ManualCondition:
                        bResult = Database.GetManualsUsingCategoryItemList(
                                        nCategoryNameId, sCategoryValue,
                                        out DataCollection, out sErrorMessage);
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false, "Unknown list data form type.");

                        bResult = false;
                        sErrorMessage = "Unknown list data form type";
                        break;
                }

                if (bResult)
                {
                    bResult = CreateDetailsFile(DataCollection, out sFile, out sErrorMessage) &&
                              OpenFile(sFile, ref sErrorMessage);
                }

                RunOnUIThreadWait(() =>
                {
                    if (!bResult)
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }

                    this.BusyControlVisible = false;
                });
            }, "List Data Form Details Thread");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region "Internal Helpers"
        private void EditData()
        {
            DataEntryForm DataEntry = new DataEntryForm();
            System.Int32 nIndex = listViewData.SelectedIndices[0];
            System.Boolean bResult;
            System.String sErrorMessage;
            System.Int32 nDataNameId;
            System.Windows.Forms.ListViewItem Item;

            Common.Debug.Thread.IsUIThread();

            new Common.Forms.FormLocation(DataEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            DataEntry.DataEntryFormType = Arcade.Forms.DataEntryForm.EDataEntryFormType.EditData;
            DataEntry.DataName = (System.String)m_DataList.GetKeyList()[nIndex];
            DataEntry.MaxDataNameLen = m_nMaxDataValueLen;

            if (DataEntry.ShowDialog(this) == DialogResult.OK)
            {
                if (m_DataList.ContainsKey(DataEntry.DataName))
                {
                    Common.Forms.MessageBox.Show(this, "Cannot change this name because it already exists.",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);

                    DataEntry.Dispose();

                    return;
                }

                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    switch (m_ListDataFormType)
                    {
                        case EListDataFormType.PartCategoryData:
                            bResult = Database.EditPartDataName(DatabaseDefs.EPartDataType.Category,
                                                                (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                DataEntry.DataName,
                                                                out sErrorMessage);
                            break;
                        case EListDataFormType.PartTypeData:
                            bResult = Database.EditPartDataName(DatabaseDefs.EPartDataType.Type,
                                                                (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                DataEntry.DataName,
                                                                out sErrorMessage);
                            break;
                        case EListDataFormType.PartPackageData:
                            bResult = Database.EditPartDataName(DatabaseDefs.EPartDataType.Package,
                                                                (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                DataEntry.DataName,
                                                                out sErrorMessage);
                            break;
                        case EListDataFormType.GameWiringHarnessData:
                            bResult = Database.EditGameDataName(DatabaseDefs.EGameDataType.WiringHarness,
                                                                (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                DataEntry.DataName,
                                                                out sErrorMessage);
                            break;
                        case EListDataFormType.GameControlData:
                            bResult = Database.EditGameDataName(DatabaseDefs.EGameDataType.ControlProperty,
                                                                (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                DataEntry.DataName,
                                                                out sErrorMessage);
                            break;
                        case EListDataFormType.GameBoardTypeData:
                            bResult = Database.EditBoardTypeDataName(
                                            (System.Int32)m_DataList.GetValueList()[nIndex],
                                            DataEntry.DataName, out sErrorMessage);
                            break;
                        case EListDataFormType.GameAudio:
                            bResult = Database.EditGameDataName(DatabaseDefs.EGameDataType.AudioProperty,
                                                                (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                DataEntry.DataName,
                                                                out sErrorMessage);
                            break;
                        case EListDataFormType.GameVideo:
                            bResult = Database.EditGameDataName(DatabaseDefs.EGameDataType.VideoProperty,
                                                                (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                DataEntry.DataName,
                                                                out sErrorMessage);
                            break;
                        case EListDataFormType.GameCocktailData:
                            bResult = Database.EditGameDataName(DatabaseDefs.EGameDataType.Cocktail,
                                                                (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                DataEntry.DataName,
                                                                out sErrorMessage);
                            break;
                        case EListDataFormType.GameBoardPartLocation:
                            bResult = Database.EditBoardPartLocationDataName(
                                            (System.Int32)m_DataList.GetValueList()[nIndex],
                                            DataEntry.DataName, out sErrorMessage);
                            break;
                        case EListDataFormType.ManualStorageBox:
                            bResult = Database.EditManualDataName(DatabaseDefs.EManualDataType.StorageBox,
                                                                  (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                  DataEntry.DataName,
                                                                  out sErrorMessage);
                            break;
                        case EListDataFormType.ManualPrintEdition:
                            bResult = Database.EditManualDataName(DatabaseDefs.EManualDataType.PrintEdition,
                                                                  (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                  DataEntry.DataName,
                                                                  out sErrorMessage);
                            break;
                        case EListDataFormType.ManualCondition:
                            bResult = Database.EditManualDataName(DatabaseDefs.EManualDataType.Condition,
                                                                  (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                  DataEntry.DataName,
                                                                  out sErrorMessage);
                            break;
                        case EListDataFormType.DisplayOrientation:
                            bResult = Database.EditDisplayDataName(DatabaseDefs.EDisplayDataType.Orientation,
                                                                   (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                   DataEntry.DataName,
                                                                   out sErrorMessage);
                            break;
                        case EListDataFormType.DisplayResolution:
                            bResult = Database.EditDisplayDataName(DatabaseDefs.EDisplayDataType.Resolution,
                                                                   (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                   DataEntry.DataName,
                                                                   out sErrorMessage);
                            break;
                        case EListDataFormType.DisplayColors:
                            bResult = Database.EditDisplayDataName(DatabaseDefs.EDisplayDataType.Colors,
                                                                   (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                   DataEntry.DataName,
                                                                   out sErrorMessage);
                            break;
                        case EListDataFormType.DisplayType:
                            bResult = Database.EditDisplayDataName(DatabaseDefs.EDisplayDataType.Type,
                                                                   (System.Int32)m_DataList.GetValueList()[nIndex],
                                                                   DataEntry.DataName,
                                                                   out sErrorMessage);
                            break;
                        case EListDataFormType.ManufacturerData:
                            bResult = Database.EditManufacturerName((System.Int32)m_DataList.GetValueList()[nIndex],
                                                                    DataEntry.DataName,
                                                                    out sErrorMessage);
                            break;
                        case EListDataFormType.LogTypeData:
                            bResult = Database.EditLogTypeDataName((System.Int32)m_DataList.GetValueList()[nIndex],
                                                                   DataEntry.DataName,
                                                                   out sErrorMessage);
                            break;
                        default:
                            System.Diagnostics.Debug.Assert(false, "Unknown list data form type.");

                            bResult = false;
                            sErrorMessage = "Unknown list data form type";
                            break;
                    }

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
                        {
                            listViewData.Items.RemoveAt(nIndex);

                            nDataNameId = (System.Int32)m_DataList.GetValueList()[nIndex];

                            m_DataList.RemoveAt(nIndex);
                            m_DataList.Add(DataEntry.DataName, nDataNameId);

                            nIndex = m_DataList.IndexOfValue(nDataNameId);

                            Item = listViewData.Items.Insert(nIndex, DataEntry.DataName);

                            Item.Selected = true;
                            Item.Focused = true;

                            Item.EnsureVisible();
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }

                        this.BusyControlVisible = false;

                        DataEntry.Dispose();
                    });
                }, "List Data Form Edit Thread");
            }
            else
            {
                DataEntry.Dispose();
            }
        }

        private void InitializeControls()
        {
            DatabaseDefs.TGameLens GameLens;
            DatabaseDefs.TBoardLens BoardLens;
            DatabaseDefs.TBoardPartLocationLens BoardPartLocationLens;
            DatabaseDefs.TPartLens PartLens;
            DatabaseDefs.TManualLens ManualLens;
            DatabaseDefs.TDisplayLens DisplayLens;
            DatabaseDefs.TLogLens LogLens;
            System.Boolean bResult;

            Common.Debug.Thread.IsWorkerThread();

            // Suppress error CS0170: Use of possibly unassigned field '???'

            GameLens = new DatabaseDefs.TGameLens();
            BoardLens = new DatabaseDefs.TBoardLens();
            BoardPartLocationLens = new DatabaseDefs.TBoardPartLocationLens();
            PartLens = new DatabaseDefs.TPartLens();
            ManualLens = new DatabaseDefs.TManualLens();
            DisplayLens = new DatabaseDefs.TDisplayLens();
            LogLens = new DatabaseDefs.TLogLens();

            switch (m_ListDataFormType)
            {
                case EListDataFormType.PartCategoryData:
                    Database.GetPartCategoryList(DatabaseDefs.EPartDataType.Category,
                                                 out m_DataList);

                    bResult = Database.GetPartMaxLens(out PartLens);
                    break;
                case EListDataFormType.PartTypeData:
                    Database.GetPartCategoryList(DatabaseDefs.EPartDataType.Type,
                                                 out m_DataList);

                    bResult = Database.GetPartMaxLens(out PartLens);
                    break;
                case EListDataFormType.PartPackageData:
                    Database.GetPartCategoryList(DatabaseDefs.EPartDataType.Package,
                                                 out m_DataList);

                    bResult = Database.GetPartMaxLens(out PartLens);
                    break;
                case EListDataFormType.GameWiringHarnessData:
                    Database.GetGameCategoryList(DatabaseDefs.EGameDataType.WiringHarness,
                                                 out m_DataList);

                    bResult = Database.GetGameMaxLens(out GameLens);
                    break;
                case EListDataFormType.GameCocktailData:
                    Database.GetGameCategoryList(DatabaseDefs.EGameDataType.Cocktail,
                                                 out m_DataList);

                    bResult = Database.GetGameMaxLens(out GameLens);
                    break;
                case EListDataFormType.GameControlData:
                    Database.GetGameCategoryList(DatabaseDefs.EGameDataType.ControlProperty,
                                                 out m_DataList);

                    bResult = Database.GetGameMaxLens(out GameLens);
                    break;
                case EListDataFormType.GameBoardTypeData:
                    Database.GetBoardTypeList(out m_DataList);

                    bResult = Database.GetBoardMaxLens(out BoardLens);
                    break;
                case EListDataFormType.GameAudio:
                    Database.GetGameCategoryList(DatabaseDefs.EGameDataType.AudioProperty,
                                                 out m_DataList);

                    bResult = Database.GetGameMaxLens(out GameLens);
                    break;
                case EListDataFormType.GameVideo:
                    Database.GetGameCategoryList(DatabaseDefs.EGameDataType.VideoProperty,
                                                 out m_DataList);

                    bResult = Database.GetGameMaxLens(out GameLens);
                    break;
                case EListDataFormType.GameBoardPartLocation:
                    Database.GetBoardPartLocationList(out m_DataList);

                    bResult = Database.GetBoardPartLocationMaxLens(out BoardPartLocationLens);
                    break;
                case EListDataFormType.ManualStorageBox:
                    Database.GetManualCategoryList(DatabaseDefs.EManualDataType.StorageBox,
                                                   out m_DataList);

                    bResult = Database.GetManualMaxLens(out ManualLens);
                    break;
                case EListDataFormType.ManualPrintEdition:
                    Database.GetManualCategoryList(DatabaseDefs.EManualDataType.PrintEdition,
                                                   out m_DataList);

                    bResult = Database.GetManualMaxLens(out ManualLens);
                    break;
                case EListDataFormType.ManualCondition:
                    Database.GetManualCategoryList(DatabaseDefs.EManualDataType.Condition,
                                                   out m_DataList);

                    bResult = Database.GetManualMaxLens(out ManualLens);
                    break;
                case EListDataFormType.DisplayType:
                    Database.GetDisplayCategoryList(DatabaseDefs.EDisplayDataType.Type,
                                                    out m_DataList);

                    bResult = Database.GetDisplayMaxLens(out DisplayLens);
                    break;
                case EListDataFormType.DisplayResolution:
                    Database.GetDisplayCategoryList(DatabaseDefs.EDisplayDataType.Resolution,
                                                    out m_DataList);

                    bResult = Database.GetDisplayMaxLens(out DisplayLens);
                    break;
                case EListDataFormType.DisplayColors:
                    Database.GetDisplayCategoryList(DatabaseDefs.EDisplayDataType.Colors,
                                                    out m_DataList);

                    bResult = Database.GetDisplayMaxLens(out DisplayLens);
                    break;
                case EListDataFormType.DisplayOrientation:
                    Database.GetDisplayCategoryList(DatabaseDefs.EDisplayDataType.Orientation,
                                                    out m_DataList);

                    bResult = Database.GetDisplayMaxLens(out DisplayLens);
                    break;
                case EListDataFormType.ManufacturerData:
                    Database.GetManufacturerList(out m_DataList);

                    bResult = Database.GetGameMaxLens(out GameLens);
                    break;
                case EListDataFormType.LogTypeData:
                    Database.GetLogTypeList(out m_DataList);

                    bResult = Database.GetLogMaxLens(out LogLens);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "Unknown list data form type.");

                    bResult = false;
                    break;
            }

            RunOnUIThreadWait(() =>
            {
                switch (m_ListDataFormType)
                {
                    case EListDataFormType.PartCategoryData:
                        this.Text = "Part Category List...";

                        labelData.Text = "&Part Category:";
                        buttonDetails.Text = "Parts";

                        if (bResult)
                        {
                            m_nMaxDataValueLen = PartLens.nPartCategoryNameLen;
                        }
                        break;
                    case EListDataFormType.PartTypeData:
                        this.Text = "Part Type List...";

                        labelData.Text = "&Part Type:";
                        buttonDetails.Text = "Parts";

                        if (bResult)
                        {
                            m_nMaxDataValueLen = PartLens.nPartTypeNameLen;
                        }
                        break;
                    case EListDataFormType.PartPackageData:
                        this.Text = "Part Package List...";

                        labelData.Text = "&Part Package:";
                        buttonDetails.Text = "Parts";

                        if (bResult)
                        {
                            m_nMaxDataValueLen = PartLens.nPartPackageNameLen;
                        }
                        break;
                    case EListDataFormType.GameWiringHarnessData:
                        this.Text = "Game Wiring Harness List...";

                        labelData.Text = "&Game Wiring Harness:";
                        buttonDetails.Text = "Games";

                        if (bResult)
                        {
                            m_nMaxDataValueLen = GameLens.nGameWiringHarnessLen;
                        }
                        break;
                    case EListDataFormType.GameCocktailData:
                        this.Text = "Game Cocktail List...";

                        labelData.Text = "&Game Cocktail:";
                        buttonDetails.Text = "Games";

                        if (bResult)
                        {
                            m_nMaxDataValueLen = GameLens.nGameCocktailLen;
                        }
                        break;
                    case EListDataFormType.GameControlData:
                        this.Text = "Game Control List...";

                        labelData.Text = "&Game Control:";
                        buttonDetails.Text = "Games";

                        if (bResult)
                        {
                            m_nMaxDataValueLen = GameLens.nGameControlLen;
                        }
                        break;
                    case EListDataFormType.GameBoardTypeData:
                        this.Text = "Game Board Type List...";

                        labelData.Text = "&Game Board Type:";

                        UpdateControlVisibility(buttonDetails, false);

                        if (bResult)
                        {
                            m_nMaxDataValueLen = BoardLens.nBoardTypeNameLen;
                        }
                        break;
                    case EListDataFormType.GameAudio:
                        this.Text = "Game Audio List...";

                        labelData.Text = "&Game Audio:";
                        buttonDetails.Text = "Games";

                        if (bResult)
                        {
                            m_nMaxDataValueLen = GameLens.nGameAudioLen;
                        }
                        break;
                    case EListDataFormType.GameVideo:
                        this.Text = "Game Video List...";

                        labelData.Text = "&Game Video:";
                        buttonDetails.Text = "Games";

                        if (bResult)
                        {
                            m_nMaxDataValueLen = GameLens.nGameVideoLen;
                        }
                        break;
                    case EListDataFormType.GameBoardPartLocation:
                        this.Text = "Game Board Part Location List...";

                        labelData.Text = "&Game Board Part Location:";

                        UpdateControlVisibility(buttonDetails, false);

                        if (bResult)
                        {
                            m_nMaxDataValueLen = BoardPartLocationLens.nBoardPartLocationLen;
                        }
                        break;
                    case EListDataFormType.ManualStorageBox:
                        this.Text = "Manual Storage Box List...";

                        labelData.Text = "&Manual Storage Box:";
                        buttonDetails.Text = "Manuals";

                        if (bResult)
                        {
                            m_nMaxDataValueLen = ManualLens.nManualStorageBoxLen;
                        }
                        break;
                    case EListDataFormType.ManualPrintEdition:
                        this.Text = "Manual Print Edition List...";

                        labelData.Text = "&Manual Print Edition:";
                        buttonDetails.Text = "Manuals";

                        if (bResult)
                        {
                            m_nMaxDataValueLen = ManualLens.nManualPrintEditionLen;
                        }
                        break;
                    case EListDataFormType.ManualCondition:
                        this.Text = "Manual Condition List...";

                        labelData.Text = "&Manual Condition:";
                        buttonDetails.Text = "Manuals";

                        if (bResult)
                        {
                            m_nMaxDataValueLen = ManualLens.nManualConditionLen;
                        }
                        break;
                    case EListDataFormType.DisplayType:
                        this.Text = "Display Type List...";

                        labelData.Text = "&Display Type:";

                        UpdateControlVisibility(buttonDetails, false);

                        if (bResult)
                        {
                            m_nMaxDataValueLen = DisplayLens.nDisplayTypeLen;
                        }
                        break;
                    case EListDataFormType.DisplayResolution:
                        this.Text = "Display Resolution List...";

                        labelData.Text = "&Display Resolution:";

                        UpdateControlVisibility(buttonDetails, false);

                        if (bResult)
                        {
                            m_nMaxDataValueLen = DisplayLens.nDisplayResolutionLen;
                        }
                        break;
                    case EListDataFormType.DisplayColors:
                        this.Text = "Display Colors List...";

                        labelData.Text = "&Display Colors:";

                        UpdateControlVisibility(buttonDetails, false);

                        if (bResult)
                        {
                            m_nMaxDataValueLen = DisplayLens.nDisplayColorsLen;
                        }
                        break;
                    case EListDataFormType.DisplayOrientation:
                        this.Text = "Display Orientation List...";

                        labelData.Text = "&Display Orientation:";

                        UpdateControlVisibility(buttonDetails, false);

                        if (bResult)
                        {
                            m_nMaxDataValueLen = DisplayLens.nDisplayOrientationLen;
                        }
                        break;
                    case EListDataFormType.ManufacturerData:
                        this.Text = "Manufacturer List...";

                        labelData.Text = "&Manufacturer:";

                        UpdateControlVisibility(buttonDetails, false);

                        if (bResult)
                        {
                            m_nMaxDataValueLen = GameLens.nManufacturerLen;
                        }
                        break;
                    case EListDataFormType.LogTypeData:
                        this.Text = "Log Type List...";

                        labelData.Text = "&Log Type:";

                        UpdateControlVisibility(buttonDetails, false);

                        if (bResult)
                        {
                            m_nMaxDataValueLen = LogLens.nLogTypeLen;
                        }
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false, "Unknown list data form type.");
                        break;
                }

                listViewData.BeginUpdate();

                foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_DataList)
                {
                    listViewData.Items.Add(Pair.Key);
                }

                listViewData.EndUpdate();
            });
        }

        private System.Boolean CreateDetailsFile(
            System.Collections.Specialized.StringCollection DataCollection,
            out System.String sFile,
            out System.String sErrorMessage)
        {
            System.Boolean bResult = false;
            System.IO.StreamWriter StreamWriter;

            Common.Debug.Thread.IsWorkerThread();

            sFile = null;
            sErrorMessage = null;

            Common.Debug.Thread.IsWorkerThread();

            try
            {
                if (Common.IO.TempFileManager.CreateTempFile(".txt", ref sFile))
                {
                    StreamWriter = new System.IO.StreamWriter(sFile);

                    foreach (System.String sValue in DataCollection)
                    {
                        StreamWriter.WriteLine(sValue);
                    }

                    StreamWriter.Close();

                    bResult = true;
                }
            }

            catch (System.SystemException exception)
            {
                sErrorMessage = exception.Message;
            }

            return bResult;
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
