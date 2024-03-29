﻿/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2016-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class ListLogsForm : Arcade.Forms.Form
    {
        #region "Member Variables"
        private System.Int32 m_nGameId = -1;
        #endregion

        #region "Constructor"
        public ListLogsForm()
        {
            InitializeComponent();
        }
        #endregion

        #region "Properties"
        public System.Int32 GameId
        {
            set
            {
                m_nGameId = value;
            }
        }
        #endregion

        #region "Common.Forms.Form Overrides"
        protected override System.Windows.Forms.Control[] ControlLocationSettings
        {
            get
            {
                return new System.Windows.Forms.Control[] { listViewLogs };
            }
        }
        #endregion

        #region "List Logs Event Handlers"
        private void ListLogsForm_Shown(object sender, EventArgs e)
        {
            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                InitializeControls();

                RunOnUIThreadWait(() =>
                {
                    this.BusyControlVisible = false;
                });
            }, "List Logs Form Initialize Thread");
        }
        #endregion

        #region "Button Event Handlers"
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            LogEntryForm LogEntry = new LogEntryForm();
            System.Boolean bResult;
            System.String sErrorMessage;
            System.Int32 nLogId;
            System.Windows.Forms.ListViewItem Item;
            DatabaseDefs.TLog Log;

            new Common.Forms.FormLocation(LogEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            LogEntry.LogEntryFormType = LogEntryForm.ELogEntryFormType.NewLog;

            if (System.Windows.Forms.DialogResult.OK == LogEntry.ShowDialog(this))
            {
                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.AddGameLogEntry(m_nGameId, LogEntry.LogDateTime, LogEntry.LogType,
                                                       LogEntry.LogDescription,
                                                       out nLogId, out sErrorMessage);

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
                        {
                            listViewLogs.BeginUpdate();

                            Item = listViewLogs.Items.Add(LogEntry.LogDateTime.ToShortDateString());

                            Item.SubItems.Add(LogEntry.LogType);

                            Log = new DatabaseDefs.TLog();

                            Log.nLogId = nLogId;
                            Log.DateTime = LogEntry.LogDateTime;
                            Log.sLogDescription = LogEntry.LogDescription;
                            Log.sLogType = LogEntry.LogType;

                            Item.Tag = Log;
                            Item.Selected = true;
                            Item.Focused = true;

                            Item.EnsureVisible();

                            listViewLogs.Enabled = true;

                            listViewLogs.EndUpdate();
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }

                        this.BusyControlVisible = false;

                        LogEntry.Dispose();
                    });
                }, "List Logs Form Add Thread");
            }
            else
            {
                LogEntry.Dispose();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditLog();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            System.Int32 nIndex = listViewLogs.SelectedIndices[0];
            System.Boolean bResult;
            System.String sErrorMessage;
            DatabaseDefs.TLog Log;

            Log = (DatabaseDefs.TLog)listViewLogs.Items[nIndex].Tag;

            this.BusyControlVisible = true;

            Common.Threading.Thread.RunWorkerThread(() =>
            {
                bResult = Database.DeleteGameLogEntry(Log.nLogId, out sErrorMessage);

                RunOnUIThreadWait(() =>
                {
                    if (bResult)
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

                    this.BusyControlVisible = false;
                });
            }, "List Logs Form Delete Thread");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region "List View Event Handlers"
        private void listViewLogs_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            buttonEdit.Enabled = true;
            buttonDelete.Enabled = true;
        }

        private void listViewLogs_DoubleClick(object sender, EventArgs e)
        {
            EditLog();
        }
        #endregion

        #region "Internal Helpers"
        private void EditLog()
        {
            System.Int32 nIndex = listViewLogs.SelectedIndices[0];
            LogEntryForm LogEntry = new LogEntryForm();
            System.Boolean bResult;
            System.String sErrorMessage;
            DatabaseDefs.TLog Log;

            new Common.Forms.FormLocation(LogEntry, ((Arcade.Forms.MainForm)Common.Forms.Application.MainForm).FormLocationsRegistryKey);

            Log = (DatabaseDefs.TLog)listViewLogs.Items[nIndex].Tag;

            LogEntry.LogEntryFormType = LogEntryForm.ELogEntryFormType.EditLog;
            LogEntry.LogDateTime = Log.DateTime;
            LogEntry.LogType = Log.sLogType;
            LogEntry.LogDescription = Log.sLogDescription;

            if (System.Windows.Forms.DialogResult.OK == LogEntry.ShowDialog(this))
            {
                this.BusyControlVisible = true;

                Common.Threading.Thread.RunWorkerThread(() =>
                {
                    bResult = Database.EditGameLogEntry(Log.nLogId,
                                                        LogEntry.LogDateTime,
                                                        LogEntry.LogType,
                                                        LogEntry.LogDescription,
                                                        out sErrorMessage);

                    RunOnUIThreadWait(() =>
                    {
                        if (bResult)
                        {
                            Log.DateTime = LogEntry.LogDateTime;
                            Log.sLogType = LogEntry.LogType;
                            Log.sLogDescription = LogEntry.LogDescription;

                            listViewLogs.BeginUpdate();

                            listViewLogs.Items[nIndex].Text = LogEntry.LogDateTime.ToShortDateString();
                            listViewLogs.Items[nIndex].SubItems[1].Text = LogEntry.LogType;
                            listViewLogs.Items[nIndex].Tag = Log;

                            listViewLogs.EndUpdate();
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }

                        this.BusyControlVisible = false;

                        LogEntry.Dispose();
                    });
                }, "List Logs Form Edit Thread");
            }
            else
            {
                LogEntry.Dispose();
            }
        }

        private void InitializeControls()
        {
            System.Collections.Generic.List<DatabaseDefs.TLog> LogsList;
            System.Boolean bResult;
            System.String sErrorMessage;
            System.Windows.Forms.ListViewItem Item;

            Common.Debug.Thread.IsWorkerThread();

            bResult = Database.GetLogsForGame(m_nGameId, out LogsList, out sErrorMessage);

            RunOnUIThreadWait(() =>
            {
                buttonEdit.Enabled = false;
                buttonDelete.Enabled = false;
                listViewLogs.Enabled = false;

                if (bResult)
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

                    listViewLogs.EndUpdate();
                }
                else
                {
                    Common.Forms.MessageBox.Show(this, sErrorMessage,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);

                    buttonAdd.Enabled = false;
                }
            });
        }
        #endregion
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2016-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
