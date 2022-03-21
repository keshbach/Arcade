/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade
{
    namespace Forms
    {
        partial class BoardPartEntryForm
        {
            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
            this.labelPosition = new System.Windows.Forms.Label();
            this.textBoxPosition = new System.Windows.Forms.TextBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.groupBoxPart = new System.Windows.Forms.GroupBox();
            this.labelParts = new System.Windows.Forms.Label();
            this.listViewParts = new Common.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPackage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBoxTimerFind = new Common.Forms.TextBoxTimer();
            this.labelFind = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonFindPart = new System.Windows.Forms.Button();
            this.comboBoxLocation = new System.Windows.Forms.ComboBox();
            this.labelLocation = new System.Windows.Forms.Label();
            this.buttonNewPart = new System.Windows.Forms.Button();
            this.groupBoxPart.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPosition
            // 
            this.labelPosition.Location = new System.Drawing.Point(6, 11);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(48, 16);
            this.labelPosition.TabIndex = 0;
            this.labelPosition.Text = "&Position:";
            // 
            // textBoxPosition
            // 
            this.textBoxPosition.Location = new System.Drawing.Point(60, 9);
            this.textBoxPosition.MaxLength = 20;
            this.textBoxPosition.Name = "textBoxPosition";
            this.textBoxPosition.Size = new System.Drawing.Size(60, 20);
            this.textBoxPosition.TabIndex = 1;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(6, 35);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(64, 16);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "&Description:";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.AcceptsReturn = true;
            this.textBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDescription.HideSelection = false;
            this.textBoxDescription.Location = new System.Drawing.Point(6, 51);
            this.textBoxDescription.MaxLength = 1000000;
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(522, 56);
            this.textBoxDescription.TabIndex = 5;
            this.textBoxDescription.WordWrap = false;
            // 
            // groupBoxPart
            // 
            this.groupBoxPart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPart.Controls.Add(this.labelParts);
            this.groupBoxPart.Controls.Add(this.listViewParts);
            this.groupBoxPart.Controls.Add(this.textBoxTimerFind);
            this.groupBoxPart.Controls.Add(this.labelFind);
            this.groupBoxPart.Location = new System.Drawing.Point(6, 115);
            this.groupBoxPart.Name = "groupBoxPart";
            this.groupBoxPart.Size = new System.Drawing.Size(522, 193);
            this.groupBoxPart.TabIndex = 6;
            this.groupBoxPart.TabStop = false;
            this.groupBoxPart.Text = "Part:";
            // 
            // labelParts
            // 
            this.labelParts.AutoSize = true;
            this.labelParts.Location = new System.Drawing.Point(3, 50);
            this.labelParts.Name = "labelParts";
            this.labelParts.Size = new System.Drawing.Size(40, 13);
            this.labelParts.TabIndex = 2;
            this.labelParts.Text = "&Part(s):";
            // 
            // listViewParts
            // 
            this.listViewParts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewParts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderCategory,
            this.columnHeaderType,
            this.columnHeaderPackage});
            this.listViewParts.ComboBoxItems = null;
            this.listViewParts.Enabled = false;
            this.listViewParts.FullRowSelect = true;
            this.listViewParts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewParts.HideSelection = false;
            this.listViewParts.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
            this.listViewParts.LabelWrap = false;
            this.listViewParts.Location = new System.Drawing.Point(6, 67);
            this.listViewParts.MultiSelect = false;
            this.listViewParts.Name = "listViewParts";
            this.listViewParts.Size = new System.Drawing.Size(510, 120);
            this.listViewParts.SortArrow = Common.Forms.ListView.ESortArrow.None;
            this.listViewParts.Sorting = Common.Forms.ListView.ESortOrder.None;
            this.listViewParts.TabIndex = 3;
            this.listViewParts.UseCompatibleStateImageBehavior = false;
            this.listViewParts.View = System.Windows.Forms.View.Details;
            this.listViewParts.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewParts_ItemSelectionChanged);
            this.listViewParts.DoubleClick += new System.EventHandler(this.listViewParts_DoubleClick);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            // 
            // columnHeaderCategory
            // 
            this.columnHeaderCategory.Text = "Category";
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Type";
            // 
            // columnHeaderPackage
            // 
            this.columnHeaderPackage.Text = "Package";
            // 
            // textBoxTimerFind
            // 
            this.textBoxTimerFind.KeyPressTimerDelay = 0.5D;
            this.textBoxTimerFind.Location = new System.Drawing.Point(39, 19);
            this.textBoxTimerFind.Name = "textBoxTimerFind";
            this.textBoxTimerFind.Size = new System.Drawing.Size(119, 20);
            this.textBoxTimerFind.TabIndex = 1;
            this.textBoxTimerFind.KeyPressTimerExpired += new Common.Forms.TextBoxTimer.KeyPressTimerExpiredHandler(this.textBoxTimerFind_KeyPressTimerExpired);
            // 
            // labelFind
            // 
            this.labelFind.AutoSize = true;
            this.labelFind.Location = new System.Drawing.Point(3, 22);
            this.labelFind.Name = "labelFind";
            this.labelFind.Size = new System.Drawing.Size(30, 13);
            this.labelFind.TabIndex = 0;
            this.labelFind.Text = "&Find:";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(453, 314);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(374, 314);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 9;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonFindPart
            // 
            this.buttonFindPart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonFindPart.Location = new System.Drawing.Point(6, 313);
            this.buttonFindPart.Name = "buttonFindPart";
            this.buttonFindPart.Size = new System.Drawing.Size(75, 23);
            this.buttonFindPart.TabIndex = 7;
            this.buttonFindPart.Text = "F&ind Part...";
            this.buttonFindPart.UseVisualStyleBackColor = true;
            this.buttonFindPart.Click += new System.EventHandler(this.buttonFindPart_Click);
            // 
            // comboBoxLocation
            // 
            this.comboBoxLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLocation.FormattingEnabled = true;
            this.comboBoxLocation.Location = new System.Drawing.Point(227, 8);
            this.comboBoxLocation.Name = "comboBoxLocation";
            this.comboBoxLocation.Size = new System.Drawing.Size(144, 21);
            this.comboBoxLocation.TabIndex = 3;
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Location = new System.Drawing.Point(170, 11);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(51, 13);
            this.labelLocation.TabIndex = 2;
            this.labelLocation.Text = "&Location:";
            // 
            // buttonNewPart
            // 
            this.buttonNewPart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNewPart.Location = new System.Drawing.Point(88, 312);
            this.buttonNewPart.Name = "buttonNewPart";
            this.buttonNewPart.Size = new System.Drawing.Size(75, 23);
            this.buttonNewPart.TabIndex = 8;
            this.buttonNewPart.Text = "&New Part...";
            this.buttonNewPart.UseVisualStyleBackColor = true;
            this.buttonNewPart.Click += new System.EventHandler(this.buttonNewPart_Click);
            // 
            // BoardPartEntryForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(534, 342);
            this.Controls.Add(this.buttonNewPart);
            this.Controls.Add(this.labelLocation);
            this.Controls.Add(this.comboBoxLocation);
            this.Controls.Add(this.buttonFindPart);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBoxPart);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.textBoxPosition);
            this.Controls.Add(this.labelPosition);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(480, 380);
            this.Name = "BoardPartEntryForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Board Part Entry";
            this.Load += new System.EventHandler(this.BoardPartEntryForm_Load);
            this.groupBoxPart.ResumeLayout(false);
            this.groupBoxPart.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

            #endregion

            private System.Windows.Forms.Label labelPosition;
            private System.Windows.Forms.TextBox textBoxPosition;
            private System.Windows.Forms.Label labelDescription;
            private System.Windows.Forms.TextBox textBoxDescription;
            private System.Windows.Forms.GroupBox groupBoxPart;
            private System.Windows.Forms.Button buttonCancel;
            private System.Windows.Forms.Button buttonOK;
            private Common.Forms.ListView listViewParts;
            private System.Windows.Forms.ColumnHeader columnHeaderName;
            private System.Windows.Forms.ColumnHeader columnHeaderCategory;
            private System.Windows.Forms.ColumnHeader columnHeaderType;
            private System.Windows.Forms.ColumnHeader columnHeaderPackage;
            private Common.Forms.TextBoxTimer textBoxTimerFind;
            private System.Windows.Forms.Label labelFind;
            private System.Windows.Forms.Label labelParts;
            private System.Windows.Forms.Button buttonFindPart;
            private System.Windows.Forms.ComboBox comboBoxLocation;
            private System.Windows.Forms.Label labelLocation;
            private System.Windows.Forms.Button buttonNewPart;
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
