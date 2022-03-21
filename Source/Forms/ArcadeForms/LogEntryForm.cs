/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2016-2016 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Arcade.Forms
{
    /// <summary>
    /// Form to allow the entry of a new log or to edit an existing log.
    /// </summary>
    public partial class LogEntryForm : System.Windows.Forms.Form
    {
        public enum ELogEntryFormType
        {
            NewLog,
            EditLog
        };

        private ELogEntryFormType m_LogEntryFormType = ELogEntryFormType.NewLog;

        private System.DateTime m_LogDateTime = new System.DateTime();
        private System.String m_sLogType = "";
        private System.String m_sLogDescription = "";

        private Common.Collections.StringSortedList<System.Int32> m_LogTypeList = null;

        public LogEntryForm()
        {
            InitializeComponent();
        }

        public ELogEntryFormType LogEntryFormType
        {
            set
            {
                m_LogEntryFormType = value;
            }
        }

        public System.DateTime LogDateTime
        {
            get
            {
                return m_LogDateTime;
            }
            set
            {
                m_LogDateTime = value;
            }
        }

        public System.String LogType
        {
            get
            {
                return m_sLogType;
            }
            set
            {
                m_sLogType = value;
            }
        }

        public System.String LogDescription
        {
            get
            {
                return m_sLogDescription;
            }
            set
            {
                m_sLogDescription = value;
            }
        }

        private void LogEntryForm_Load(object sender, EventArgs e)
        {
            DatabaseDefs.TLogLens LogLens;

            using (new Common.Forms.WaitCursor(this))
            {
                Database.GetLogTypeList(out m_LogTypeList);

                comboBoxType.BeginUpdate();

                foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_LogTypeList)
                {
                    comboBoxType.Items.Add(Pair.Key);
                }

                comboBoxType.EndUpdate();

                comboBoxType.AutosizeDropDown();

                switch (m_LogEntryFormType)
                {
                    case ELogEntryFormType.NewLog:
                        Text = "New Log...";

                        comboBoxType.SelectedIndex = m_LogTypeList.IndexOfKey(m_sLogType);
                        break;
                    case ELogEntryFormType.EditLog:
                        Text = "Edit Log...";

                        dateTimePickerLog.Value = m_LogDateTime;

                        textBoxDescription.Text = m_sLogDescription;

                        comboBoxType.SelectedIndex = m_LogTypeList.IndexOfKey(m_sLogType);

                        ValidateData();
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                }

                if (comboBoxType.SelectedIndex == -1)
                {
                    buttonOK.Enabled = false;
                }

                if (Arcade.Database.GetLogMaxLens(out LogLens))
                {
                    textBoxDescription.MaxLength = LogLens.nLogDescriptionLen;
                }
            }
        }

        private void comboBoxType_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxType.SelectedIndex == -1)
            {
                e.Cancel = true;
            }

            ValidateData();
        }

        private void textBoxDescription_TextChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_LogDateTime = dateTimePickerLog.Value;
            m_sLogType = (System.String)comboBoxType.SelectedItem;
            m_sLogDescription = textBoxDescription.Text;

            DialogResult = DialogResult.OK;

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void ValidateData()
        {
            if (comboBoxType.SelectedIndex != -1 && textBoxDescription.TextLength > 0)
            {
                buttonOK.Enabled = true;
            }
            else
            {
                buttonOK.Enabled = false;
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2016-2016 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
