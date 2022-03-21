/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2016-2016 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class ListLogsForm : System.Windows.Forms.Form
    {
        private System.Int32 m_nGameId = -1;

        public ListLogsForm()
        {
            InitializeComponent();
        }

        public System.Int32 GameId
        {
            set
            {
                m_nGameId = value;
            }
        }

        private void ListLogsForm_Load(object sender, EventArgs e)
        {
            System.Collections.Generic.List<DatabaseDefs.TLog> LogsList;
            System.String sErrorMessage;
            System.Windows.Forms.ListViewItem Item;

            buttonEdit.Enabled = false;
            buttonDelete.Enabled = false;
            listViewLogs.Enabled = false;

            using (new Common.Forms.WaitCursor(this))
            {
                if (Database.GetLogsForGame(m_nGameId, out LogsList, out sErrorMessage))
                {
                    listViewLogs.BeginUpdate();

                    foreach (DatabaseDefs.TLog Log in LogsList)
                    {
                        Item = listViewLogs.Items.Add(Log.DateTime.ToShortDateString());

                        Item.SubItems.Add(Log.sLogType);

                        Item.Tag = Log;
                    }

                    if (listViewLogs.Items.Count > 0)
                    {
                        listViewLogs.Enabled = true;
                    }

                    listViewLogs.AutosizeColumns();
                    listViewLogs.EndUpdate();
                }
                else
                {
                    Common.Forms.MessageBox.Show(this, sErrorMessage,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);

                    buttonAdd.Enabled = false;
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Arcade.Forms.LogEntryForm LogForm = new Arcade.Forms.LogEntryForm();
            System.String sErrorMessage = "";
            System.Int32 nLogId;
            System.Windows.Forms.ListViewItem Item;
            DatabaseDefs.TLog Log;

            LogForm.LogEntryFormType = LogEntryForm.ELogEntryFormType.NewLog;

            if (System.Windows.Forms.DialogResult.OK == LogForm.ShowDialog(this))
            {
                using (new Common.Forms.WaitCursor(this))
                {
                    if (Database.AddGameLogEntry(m_nGameId, LogForm.LogDateTime, LogForm.LogType,
                                                 LogForm.LogDescription,
                                                 out nLogId, out sErrorMessage))
                    {
                        listViewLogs.BeginUpdate();

                        Item = listViewLogs.Items.Add(LogForm.LogDateTime.ToShortDateString());

                        Item.SubItems.Add(LogForm.LogType);

                        Log = new DatabaseDefs.TLog();

                        Log.nLogId = nLogId;
                        Log.DateTime = LogForm.LogDateTime;
                        Log.sLogDescription = LogForm.LogDescription;
                        Log.sLogType = LogForm.LogType;

                        Item.Tag = Log;
                        Item.Selected = true;
                        Item.Focused = true;

                        Item.EnsureVisible();

                        listViewLogs.Enabled = true;

                        listViewLogs.AutosizeColumns();
                        listViewLogs.EndUpdate();
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditLog();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            System.Int32 nIndex = listViewLogs.SelectedIndices[0];
            System.String sErrorMessage;
            DatabaseDefs.TLog Log;

            Log = (DatabaseDefs.TLog)listViewLogs.Items[nIndex].Tag;

            using (new Common.Forms.WaitCursor(this))
            {
                if (Database.DeleteGameLogEntry(Log.nLogId, out sErrorMessage))
                {
                    listViewLogs.BeginUpdate();
                    listViewLogs.Items.RemoveAt(nIndex);

                    if (listViewLogs.Items.Count > 0)
                    {
                        if (listViewLogs.Items.Count == nIndex)
                        {
                            --nIndex;
                        }

                        listViewLogs.Items[nIndex].Selected = true;
                        listViewLogs.Items[nIndex].Focused = true;

                        listViewLogs.Items[nIndex].EnsureVisible();
                    }
                    else
                    {
                        listViewLogs.Enabled = false;

                        buttonEdit.Enabled = false;
                        buttonDelete.Enabled = false;
                    }

                    listViewLogs.EndUpdate();
                }
                else
                {
                    Common.Forms.MessageBox.Show(this, sErrorMessage,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listViewLogs_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            buttonEdit.Enabled = true;
            buttonDelete.Enabled = true;
        }

        private void listViewLogs_DoubleClick(object sender, EventArgs e)
        {
            EditLog();
        }

        private void EditLog()
        {
            System.Int32 nIndex = listViewLogs.SelectedIndices[0];
            Arcade.Forms.LogEntryForm LogForm = new Arcade.Forms.LogEntryForm();
            System.String sErrorMessage;
            DatabaseDefs.TLog Log;

            Log = (DatabaseDefs.TLog)listViewLogs.Items[nIndex].Tag;

            LogForm.LogEntryFormType = LogEntryForm.ELogEntryFormType.EditLog;
            LogForm.LogDateTime = Log.DateTime;
            LogForm.LogType = Log.sLogType;
            LogForm.LogDescription = Log.sLogDescription;

            if (System.Windows.Forms.DialogResult.OK == LogForm.ShowDialog(this))
            {
                using (new Common.Forms.WaitCursor(this))
                {
                    if (Database.EditGameLogEntry(Log.nLogId,
                                                  LogForm.LogDateTime,
                                                  LogForm.LogType,
                                                  LogForm.LogDescription,
                                                  out sErrorMessage))
                    {
                        Log.DateTime = LogForm.LogDateTime;
                        Log.sLogType = LogForm.LogType;
                        Log.sLogDescription = LogForm.LogDescription;

                        listViewLogs.BeginUpdate();

                        listViewLogs.Items[nIndex].Text = LogForm.LogDateTime.ToShortDateString();
                        listViewLogs.Items[nIndex].SubItems[1].Text = LogForm.LogType;
                        listViewLogs.Items[nIndex].Tag = Log;

                        listViewLogs.AutosizeColumns();
                        listViewLogs.EndUpdate();
                    }
                    else
                    {
                        Common.Forms.MessageBox.Show(this, sErrorMessage,
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2016-2016 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
