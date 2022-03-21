/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class FindGameBoardForm : System.Windows.Forms.Form
    {
        public FindGameBoardForm()
        {
            InitializeComponent();
        }

        private void FindGameBoardForm_Load(object sender, EventArgs e)
        {
            buttonSearch.Enabled = false;
            listViewGames.Enabled = false;
            listViewBoard.Enabled = false;
        }

        private void textBoxKeyword_TextChanged(object sender, EventArgs e)
        {
            buttonSearch.Enabled = (textBoxKeyword.TextLength > 0) ? true : false;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<DatabaseDefs.TBoard>> GameBoardListDict;
            System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.List<DatabaseDefs.TBoard>>.Enumerator Enum;
            System.String sErrorMessage;
            System.Windows.Forms.ListViewItem ListViewItem;

            listViewGames.BeginUpdate();
            listViewBoard.BeginUpdate();

            listViewGames.Items.Clear();
            listViewBoard.Items.Clear();

            listViewBoard.Enabled = false;

            using (new Common.Forms.WaitCursor(this))
            {
                if (Database.GetGameBoardsMatchingKeyword(textBoxKeyword.Text,
                                                          out GameBoardListDict,
                                                          out sErrorMessage))
                {
                    Enum = GameBoardListDict.GetEnumerator();

                    while (Enum.MoveNext())
                    {
                        ListViewItem = listViewGames.Items.Add(Enum.Current.Key);

                        ListViewItem.Tag = Enum.Current.Value;
                    }

                    listViewGames.AutosizeColumns();
                }
                else
                {
                    Common.Forms.MessageBox.Show(this, sErrorMessage,
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);
                }
            }

            listViewGames.Enabled = (listViewGames.Items.Count > 0) ? true : false;

            listViewGames.EndUpdate();
            listViewBoard.EndUpdate();
        }

        private void listViewGames_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            System.Collections.Generic.List<DatabaseDefs.TBoard> BoardList;
            System.Windows.Forms.ListViewItem ListViewItem;

            if (e.IsSelected)
            {
                BoardList = (System.Collections.Generic.List<DatabaseDefs.TBoard>)listViewGames.SelectedItems[0].Tag;

                listViewBoard.BeginUpdate();
                listViewBoard.Items.Clear();

                listViewBoard.Enabled = true;

                foreach (DatabaseDefs.TBoard Board in BoardList)
                {
                    ListViewItem = listViewBoard.Items.Add((Board.sBoardTypeName == DatabaseDefs.CCartridgeName) ? "*" : "");

                    ListViewItem.SubItems.Add(Board.sBoardTypeName);
                    ListViewItem.SubItems.Add(Board.sBoardName);
                }

                listViewBoard.AutosizeColumns();
                listViewBoard.EndUpdate();
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
