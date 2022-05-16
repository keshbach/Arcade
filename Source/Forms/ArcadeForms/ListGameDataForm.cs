/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class ListGameDataForm : Common.Forms.Form
    {
        #region "Enumerations"
        public enum EListGameDataFormType
        {
            GameControls,
            GameAudio,
            GameVideo,
            GameDisplay
        };
        #endregion

        #region "Member Variables"
        private EListGameDataFormType m_ListGameDataFormType = EListGameDataFormType.GameControls;
        private Common.Collections.StringCollection m_GamePropertiesColl = new Common.Collections.StringCollection();
        private System.Boolean m_bAllowDuplicates = false;

        private Common.Collections.StringCollection m_GameDataColl = new Common.Collections.StringCollection();
        #endregion

        #region "Properties"
        public EListGameDataFormType ListGameDataFormType
        {
            set
            {
                m_ListGameDataFormType = value;
            }
        }

        public Common.Collections.StringCollection GamePropertiesColl
        {
            get
            {
                return m_GamePropertiesColl;
            }
            set
            {
                m_GamePropertiesColl = value.MakeCopy();
            }
        }

        public System.Boolean AllowDuplicates
        {
            set
            {
                m_bAllowDuplicates = value;
            }
        }
        #endregion

        #region "Constructor"
        public ListGameDataForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { listViewGameData };
            }
        }
        #endregion

        #region "List Game Data Event Handlers"
        private void ListGameDataForm_Load(object sender, EventArgs e)
        {
            Common.Collections.StringSortedList<System.Int32> GameDataList = new Common.Collections.StringSortedList<System.Int32>();
            System.Collections.Generic.List<DatabaseDefs.TDisplay> DisplayList = new System.Collections.Generic.List<DatabaseDefs.TDisplay>();
            System.String sErrorMessage;

            listViewGameData.BeginUpdate();

            foreach (System.String sValue in m_GamePropertiesColl)
            {
                listViewGameData.Items.Add(sValue);
            }

            listViewGameData.AutosizeColumns();
            listViewGameData.EndUpdate();

            if (listViewGameData.Items.Count == 0)
            {
                listViewGameData.Enabled = false;
            }

            buttonDelete.Enabled = false;

            switch (m_ListGameDataFormType)
            {
                case EListGameDataFormType.GameControls:
                    Text = "Controls";

                    labelGameData.Text = "&Controls:";

                    Database.GetGameCategoryList(DatabaseDefs.EGameDataType.ControlProperty,
                                                    out GameDataList);
                    break;
                case EListGameDataFormType.GameAudio:
                    Text = "Audio";

                    labelGameData.Text = "&Audio:";

                    Database.GetGameCategoryList(DatabaseDefs.EGameDataType.AudioProperty,
                                                    out GameDataList);
                    break;
                case EListGameDataFormType.GameVideo:
                    Text = "Video";

                    labelGameData.Text = "&Video:";

                    Database.GetGameCategoryList(DatabaseDefs.EGameDataType.VideoProperty,
                                                    out GameDataList);
                    break;
                case EListGameDataFormType.GameDisplay:
                    Text = "Displays";

                    labelGameData.Text = "&Displays:";

                    Database.GetDisplays(out DisplayList, out sErrorMessage);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
            }

            if (GameDataList.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in GameDataList)
                {
                    m_GameDataColl.Add(Pair.Key);
                }
            }
            else if (DisplayList.Count > 0)
            {
                foreach (DatabaseDefs.TDisplay Display in DisplayList)
                {
                    m_GameDataColl.Add(Display.sDisplayName);
                }
            }
        }
        #endregion

        #region "List View Event Handlers"
        private void listViewGameData_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            buttonDelete.Enabled = true;
        }

        private void listViewGameData_DoubleClick(object sender, EventArgs e)
        {
            DeleteCurrentItem();
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            SelectDataForm SelectData = new SelectDataForm();
            System.Collections.Specialized.StringCollection TmpColl = new System.Collections.Specialized.StringCollection();

            if (m_bAllowDuplicates)
            {
                TmpColl = m_GameDataColl.MakeCopy();
            }
            else
            {
                TmpColl = m_GameDataColl.Merge(m_GamePropertiesColl);
            }

            SelectData.DataColl = TmpColl;

            switch (m_ListGameDataFormType)
            {
                case EListGameDataFormType.GameControls:
                    SelectData.DataType = "Controls";
                    break;
                case EListGameDataFormType.GameAudio:
                    SelectData.DataType = "Audio";
                    break;
                case EListGameDataFormType.GameVideo:
                    SelectData.DataType = "Video";
                    break;
                case EListGameDataFormType.GameDisplay:
                    SelectData.DataType = "Displays";
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
            }

            if (SelectData.ShowDialog(this) == DialogResult.OK)
            {
                m_GamePropertiesColl.Add(SelectData.SelectedData);

                listViewGameData.BeginUpdate();

                listViewGameData.Items.Add(SelectData.SelectedData);
                listViewGameData.AutosizeColumns();

                listViewGameData.Enabled = true;

                listViewGameData.EndUpdate();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteCurrentItem();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
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
        private void DeleteCurrentItem()
        {
            System.Int32 nIndex = listViewGameData.SelectedIndices[0];

            m_GamePropertiesColl.RemoveAt(nIndex);

            listViewGameData.Items.RemoveAt(nIndex);

            if (listViewGameData.Items.Count == 0)
            {
                listViewGameData.Enabled = false;
            }

            buttonDelete.Enabled = false;
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
