/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Arcade
{
    namespace Forms
    {
        public partial class GameBoardEntryForm : System.Windows.Forms.Form
        {
            public enum EGameBoardEntryFormType
            {
                NewBoard,
                EditBoard
            };

            private EGameBoardEntryFormType m_GameBoardEntryFormType = EGameBoardEntryFormType.NewBoard;
            private System.String m_sBoardTypeName = "";
            private System.String m_sBoardName = "";
            private System.String m_sBoardSize = "";
            private System.String m_sBoardDescription = "";

            public GameBoardEntryForm()
            {
                InitializeComponent();
            }

            public EGameBoardEntryFormType GameBoardEntryFormType
            {
                set
                {
                    m_GameBoardEntryFormType = value;
                }
            }

            public System.String BoardTypeName
            {
                get
                {
                    return m_sBoardTypeName;
                }
                set
                {
                    m_sBoardTypeName = value;
                }
            }

            public System.String BoardName
            {
                get
                {
                    return m_sBoardName;
                }
                set
                {
                    m_sBoardName = value;
                }
            }

            public System.String BoardSize
            {
                get
                {
                    return m_sBoardSize;
                }
                set
                {
                    m_sBoardSize = value;
                }
            }

            public System.String BoardDescription
            {
                get
                {
                    return m_sBoardDescription;
                }
                set
                {
                    m_sBoardDescription = value;
                }
            }

            private void GameBoardEntryForm_Load(object sender, EventArgs e)
            {
                Common.Collections.StringSortedList<System.Int32> BoardTypeList;
                DatabaseDefs.TBoardLens BoardLens;

                if (Database.GetBoardMaxLens(out BoardLens))
                {
                    textBoxName.MaxLength = BoardLens.nBoardNameLen;
                    textBoxSize.MaxLength = BoardLens.nBoardSizeLen;
                    textBoxDescription.MaxLength = BoardLens.nBoardDescriptionLen;
                }

                Database.GetBoardTypeList(out BoardTypeList);

                comboBoxBoardType.BeginUpdate();

                foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in BoardTypeList)
                {
                    comboBoxBoardType.Items.Add(Pair.Key);
                }

                comboBoxBoardType.EndUpdate();

                if (m_GameBoardEntryFormType == EGameBoardEntryFormType.NewBoard)
                {
                    this.Text = "Add...";

                    buttonOK.Enabled = false;
                }
                else
                {
                    this.Text = "Edit...";

                    comboBoxBoardType.SelectedIndex = BoardTypeList.IndexOfKey(m_sBoardTypeName);

                    textBoxName.Text = m_sBoardName;
                    textBoxSize.Text = m_sBoardSize;
                    textBoxDescription.Text = m_sBoardDescription;
                }
            }

            private void comboBoxBoardType_Validating(object sender, CancelEventArgs e)
            {
                if (comboBoxBoardType.SelectedIndex != -1)
                {
                    VerifyFields();
                }
                else
                {
                    e.Cancel = true;

                    buttonOK.Enabled = false;
                }
            }

            private void textBoxName_TextChanged(object sender, EventArgs e)
            {
                VerifyFields();
            }

            private void buttonOK_Click(object sender, EventArgs e)
            {
                m_sBoardTypeName = (System.String)comboBoxBoardType.SelectedItem;
                m_sBoardName = textBoxName.Text;
                m_sBoardSize = textBoxSize.Text;
                m_sBoardDescription = textBoxDescription.Text;

                DialogResult = DialogResult.OK;

                Close();
            }

            private void buttonCancel_Click(object sender, EventArgs e)
            {
                DialogResult = DialogResult.Cancel;

                Close();
            }

            private void VerifyFields()
            {
                if (comboBoxBoardType.SelectedIndex != -1)
                {
                    if (DatabaseDefs.CCartridgeName == (System.String)comboBoxBoardType.SelectedItem)
                    {
                        if (textBoxName.Text.Length > 0)
                        {
                            buttonOK.Enabled = true;
                        }
                        else
                        {
                            buttonOK.Enabled = false;
                        }
                    }
                    else
                    {
                        buttonOK.Enabled = true;
                    }
                }
                else
                {
                    buttonOK.Enabled = false;
                }
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
