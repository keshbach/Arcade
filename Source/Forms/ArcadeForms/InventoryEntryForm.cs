/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2016-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class InventoryEntryForm : Arcade.Forms.Form
    {
        #region "Enumerations"
        public enum EInventoryEntryFormType
        {
            NewInventory,
            EditInventory
        };
        #endregion

        #region "Member Variables"
        private EInventoryEntryFormType m_InventoryEntryFormType = EInventoryEntryFormType.NewInventory;

        private System.DateTime m_InventoryDateTime = new System.DateTime();
        private System.Int32 m_nInventoryCount = 0;
        private System.String m_sInventoryDescription = "";
        #endregion

        #region "Constructor"
        public InventoryEntryForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Properties"
        public EInventoryEntryFormType InventoryEntryFormType
        {
            set
            {
                m_InventoryEntryFormType = value;
            }
        }

        public System.DateTime InventoryDateTime
        {
            get
            {
                return m_InventoryDateTime;
            }
            set
            {
                m_InventoryDateTime = value;
            }
        }

        public System.Int32 InventoryCount
        {
            get
            {
                return m_nInventoryCount;
            }
            set
            {
                m_nInventoryCount = value;
            }
        }

        public System.String InventoryDescription
        {
            get
            {
                return m_sInventoryDescription;
            }
            set
            {
                m_sInventoryDescription = value;
            }
        }



        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlClearSelection
        {
            get
            {
                return new System.Windows.Forms.Control[] { textBoxDescription };
            }
        }
        #endregion

        #region "Log Entry Event Handlers"
        private void InventoryEntryForm_Shown(object sender, EventArgs e)
        {
            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                InitializeControls();

                RunOnUIThreadWait(() =>
                {
                    this.BusyControlVisible = false;
                });
            }, "Inventory Entry Form Initialize Thread");
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_InventoryDateTime = dateTimePickerInventory.Value;
            m_nInventoryCount = System.Convert.ToInt32(numericUpDownInventory.Value);
            m_sInventoryDescription = textBoxDescription.Text;

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
        private void InitializeControls()
        {
            DatabaseDefs.TInventoryLens InventoryLens;
            System.Boolean bResult;

            Common.Debug.Thread.IsWorkerThread();

            bResult = Database.GetInventoryMaxLens(out InventoryLens);

            RunOnUIThreadWait(() =>
            {
                switch (m_InventoryEntryFormType)
                {
                    case EInventoryEntryFormType.NewInventory:
                        Text = "New Inventory...";
                        break;
                    case EInventoryEntryFormType.EditInventory:
                        Text = "Edit Inventory...";

                        dateTimePickerInventory.Value = m_InventoryDateTime;

                        numericUpDownInventory.Value = m_nInventoryCount;

                        textBoxDescription.Text = m_sInventoryDescription;
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                }

                if (bResult)
                {
                    textBoxDescription.MaxLength = InventoryLens.nInventoryDescriptionLen;
                }
            });
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2016-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
