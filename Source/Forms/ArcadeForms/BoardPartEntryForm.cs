/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
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
        /// <summary>
        /// Summary description for BoardPartEntryForm.
        /// </summary>
        public partial class BoardPartEntryForm : System.Windows.Forms.Form
        {
            public enum EBoardPartEntryFormType
            {
                AddBoardPart,
                EditBoardPart
            }

            private EBoardPartEntryFormType m_BoardPartEntryFormType = EBoardPartEntryFormType.AddBoardPart;
            private System.String m_sPartPosition = "";
            private System.String m_sPartLocation = "";
            private System.String m_sPartDescription = "";
            private System.String m_sPartName = "";
            private System.String m_sPartCategoryName = "";
            private System.String m_sPartTypeName = "";
            private System.String m_sPartPackageName = "";

            private Common.Collections.StringSortedList<System.Int32> m_BoardPartLocationList = null;

            public BoardPartEntryForm()
            {
                //
                // Required for Windows Form Designer support
                //
                InitializeComponent();

                //
                // TODO: Add any constructor code after InitializeComponent call
                //
            }

            public EBoardPartEntryFormType BoardPartEntryFormType
            {
                set
                {
                    m_BoardPartEntryFormType = value;
                }
            }

            public System.String PartPosition
            {
                get
                {
                    return m_sPartPosition;
                }
                set
                {
                    m_sPartPosition = value;
                }
            }

            public System.String PartLocation
            {
                get
                {
                    return m_sPartLocation;
                }
                set
                {
                    m_sPartLocation = value;
                }
            }

            public System.String PartDescription
            {
                get
                {
                    return m_sPartDescription;
                }
                set
                {
                    m_sPartDescription = value;
                }
            }

            public System.String PartName
            {
                get
                {
                    return m_sPartName;
                }
                set
                {
                    m_sPartName = value;
                }
            }

            public System.String PartCategoryName
            {
                get
                {
                    return m_sPartCategoryName;
                }
                set
                {
                    m_sPartCategoryName = value;
                }
            }

            public System.String PartTypeName
            {
                get
                {
                    return m_sPartTypeName;
                }
                set
                {
                    m_sPartTypeName = value;
                }
            }

            public System.String PartPackageName
            {
                get
                {
                    return m_sPartPackageName;
                }
                set
                {
                    m_sPartPackageName = value;
                }
            }

            private void BoardPartEntryForm_Load(object sender, EventArgs e)
            {
                DatabaseDefs.TBoardPartLocationLens BoardPartLocationLens;
                DatabaseDefs.TPartLens PartLens;

                using (new Common.Forms.WaitCursor(this))
                {
                    if (Database.GetBoardPartLocationMaxLens(out BoardPartLocationLens))
                    {
                        textBoxPosition.MaxLength = BoardPartLocationLens.nBoardPartPositionLen;
                        textBoxDescription.MaxLength = BoardPartLocationLens.nBoardPartDescriptionLen;
                    }

                    if (Database.GetPartMaxLens(out PartLens))
                    {
                        textBoxTimerFind.MaxLength = PartLens.nPartNameLen;
                    }

                    Database.GetBoardPartLocationList(out m_BoardPartLocationList);

                    comboBoxLocation.BeginUpdate();

                    foreach (System.Collections.Generic.KeyValuePair<System.String, System.Int32> Pair in m_BoardPartLocationList)
                    {
                        comboBoxLocation.Items.Add(Pair.Key);
                    }

                    comboBoxLocation.EndUpdate();
                }

                switch (m_BoardPartEntryFormType)
                {
                    case EBoardPartEntryFormType.AddBoardPart:
                        Text = "Add...";

                        m_sPartLocation = Database.DefBoardPartLocation;

                        buttonOK.Enabled = false;
                        break;
                    case EBoardPartEntryFormType.EditBoardPart:
                        Text = "Edit...";

                        textBoxPosition.Text = m_sPartPosition;
                        textBoxDescription.Text = m_sPartDescription;

                        textBoxTimerFind.Text = m_sPartName;

                        FindPartsFromPartName();
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                }

                comboBoxLocation.SelectedIndex = m_BoardPartLocationList.IndexOfKey(m_sPartLocation);
            }

            private void buttonOK_Click(object sender, EventArgs e)
            {
                System.Int32 nIndex = listViewParts.SelectedIndices[0];

                if (textBoxPosition.Text.Length > 0)
                {
                    m_sPartPosition = textBoxPosition.Text;
                }
                else
                {
                    m_sPartPosition = " ";
                }

                m_sPartLocation = (System.String)comboBoxLocation.SelectedItem;
                m_sPartDescription = textBoxDescription.Text;
                m_sPartName = listViewParts.Items[nIndex].SubItems[0].Text;
                m_sPartCategoryName = listViewParts.Items[nIndex].SubItems[1].Text;
                m_sPartTypeName = listViewParts.Items[nIndex].SubItems[2].Text;
                m_sPartPackageName = listViewParts.Items[nIndex].SubItems[3].Text;

                DialogResult = DialogResult.OK;

                Close();
            }

            private void buttonCancel_Click(object sender, EventArgs e)
            {
                DialogResult = DialogResult.Cancel;

                Close();
            }

            private void textBoxTimerFind_KeyPressTimerExpired(object sender, EventArgs e)
            {
                FindPartsFromPartName();
            }

            private void listViewParts_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
            {
                buttonOK.Enabled = true;
            }

            private void listViewParts_DoubleClick(object sender, EventArgs e)
            {
                buttonOK.PerformClick();
            }

            private void buttonFindPart_Click(object sender, EventArgs e)
            {
                FindPartForm PartForm = new FindPartForm();

                PartForm.ShowDialog(this);

                FindPartsFromPartName();

                PartForm.Dispose();
            }

            private void buttonNewPart_Click(object sender, EventArgs e)
            {
                Arcade.Forms.PartEntryForm PartEntry = new Arcade.Forms.PartEntryForm();
                System.Int32 nPartId;
                System.String sErrorMessage;

                PartEntry.PartEntryFormType = Arcade.Forms.PartEntryForm.EPartEntryFormType.NewPart;

                if (PartEntry.ShowDialog(this) == DialogResult.OK)
                {
                    using (new Common.Forms.WaitCursor(this))
                    {
                        if (true == Database.AddPartGroup(PartEntry.PartName,
                                                          PartEntry.PartCategoryName,
                                                          PartEntry.PartTypeName,
                                                          PartEntry.PartPackageName,
                                                          PartEntry.PartPinouts,
                                                          PartEntry.PartDatasheetColl,
                                                          out nPartId, out sErrorMessage))
                        {
                            FindPartsFromPartName();
                        }
                        else
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                }

                PartEntry.Dispose();
            }

            private void FindPartsFromPartName()
            {
                System.Collections.Generic.List<DatabaseDefs.TPart> PartList;
                System.String sErrorMessage;
                System.Windows.Forms.ListViewItem Item;

                buttonOK.Enabled = false;
                listViewParts.Enabled = false;

                if (textBoxTimerFind.Text.Length > 0)
                {
                    using (new Common.Forms.WaitCursor(this))
                    {
                        if (false == Database.GetPartsMatchingKeyword(textBoxTimerFind.Text,
                                            DatabaseDefs.EKeywordMatchingCriteria.Start,
                                            out PartList,
                                            out sErrorMessage))
                        {
                            Common.Forms.MessageBox.Show(this, sErrorMessage,
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);

                            return;
                        }
                    }

                    listViewParts.BeginUpdate();

                    listViewParts.Sorting = Common.Forms.ListView.ESortOrder.None;

                    listViewParts.Items.Clear();

                    foreach (DatabaseDefs.TPart Part in PartList)
                    {
                        Item = listViewParts.Items.Add(Part.sPartName);

                        Item.SubItems.Add(Part.sPartCategoryName);
                        Item.SubItems.Add(Part.sPartTypeName);
                        Item.SubItems.Add(Part.sPartPackageName);

                        if (0 == System.String.Compare(m_sPartName, Part.sPartName, true) &&
                            0 == System.String.Compare(m_sPartCategoryName, Part.sPartCategoryName, true) &&
                            0 == System.String.Compare(m_sPartTypeName, Part.sPartTypeName, true) &&
                            0 == System.String.Compare(m_sPartPackageName, Part.sPartPackageName, true))
                        {
                            listViewParts.Enabled = true;

                            Item.EnsureVisible();

                            Item.Selected = true;
                        }
                    }

                    listViewParts.Sorting = Common.Forms.ListView.ESortOrder.GroupSequential;

                    listViewParts.AutosizeColumns();
                    listViewParts.EndUpdate();

                    if (listViewParts.Items.Count > 0)
                    {
                        listViewParts.Enabled = true;

                        if (listViewParts.Items.Count == 1)
                        {
                            listViewParts.Items[0].Selected = true;

                            buttonOK.Enabled = true;
                        }
                    }
                    else
                    {
                        listViewParts.Enabled = false;
                    }
                }
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
