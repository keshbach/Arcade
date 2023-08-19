/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade.Forms
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
            this.textBoxPosition = new Common.Forms.TextBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxDescription = new Common.Forms.TextBox();
            this.groupBoxPart = new System.Windows.Forms.GroupBox();
            this.labelParts = new System.Windows.Forms.Label();
            this.listViewParts = new Common.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPackage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBoxTimerFind = new Common.Forms.TextBoxTimer();
            this.labelFind = new System.Windows.Forms.Label();
            this.buttonCancel = new Arcade.Forms.Button();
            this.buttonOK = new Arcade.Forms.Button();
            this.buttonFindPart = new Arcade.Forms.Button();
            this.comboBoxLocation = new System.Windows.Forms.ComboBox();
            this.labelLocation = new System.Windows.Forms.Label();
            this.buttonNewPart = new Arcade.Forms.Button();
            this.splitContainerBoardPart = new System.Windows.Forms.SplitContainer();
            this.groupBoxPart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBoardPart)).BeginInit();
            this.splitContainerBoardPart.Panel1.SuspendLayout();
            this.splitContainerBoardPart.Panel2.SuspendLayout();
            this.splitContainerBoardPart.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPosition
            // 
            this.labelPosition.Location = new System.Drawing.Point(3, 7);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(48, 16);
            this.labelPosition.TabIndex = 0;
            this.labelPosition.Text = "&Position:";
            // 
            // textBoxPosition
            // 
            this.textBoxPosition.Location = new System.Drawing.Point(57, 4);
            this.textBoxPosition.MaxLength = 20;
            this.textBoxPosition.Name = "textBoxPosition";
            this.textBoxPosition.Size = new System.Drawing.Size(60, 20);
            this.textBoxPosition.TabIndex = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(3, 36);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(64, 16);
            this.labelDescription.TabIndex = 3;
            this.labelDescription.Text = "&Description:";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.AcceptsReturn = true;
            this.textBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDescription.HideSelection = false;
            this.textBoxDescription.Location = new System.Drawing.Point(3, 55);
            this.textBoxDescription.MaxLength = 1000000;
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(513, 46);
            this.textBoxDescription.TabIndex = 4;
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
            this.groupBoxPart.Location = new System.Drawing.Point(0, 0);
            this.groupBoxPart.Name = "groupBoxPart";
            this.groupBoxPart.Size = new System.Drawing.Size(522, 189);
            this.groupBoxPart.TabIndex = 0;
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
            this.listViewParts.Size = new System.Drawing.Size(510, 116);
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
            this.buttonCancel.TabIndex = 4;
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
            this.buttonOK.TabIndex = 3;
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
            this.buttonFindPart.TabIndex = 1;
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
            this.comboBoxLocation.Location = new System.Drawing.Point(224, 4);
            this.comboBoxLocation.Name = "comboBoxLocation";
            this.comboBoxLocation.Size = new System.Drawing.Size(144, 21);
            this.comboBoxLocation.TabIndex = 2;
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Location = new System.Drawing.Point(167, 7);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(51, 13);
            this.labelLocation.TabIndex = 1;
            this.labelLocation.Text = "&Location:";
            // 
            // buttonNewPart
            // 
            this.buttonNewPart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNewPart.Location = new System.Drawing.Point(88, 312);
            this.buttonNewPart.Name = "buttonNewPart";
            this.buttonNewPart.Size = new System.Drawing.Size(75, 23);
            this.buttonNewPart.TabIndex = 2;
            this.buttonNewPart.Text = "&New Part...";
            this.buttonNewPart.UseVisualStyleBackColor = true;
            this.buttonNewPart.Click += new System.EventHandler(this.buttonNewPart_Click);
            // 
            // splitContainerBoardPart
            // 
            this.splitContainerBoardPart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerBoardPart.Location = new System.Drawing.Point(6, 8);
            this.splitContainerBoardPart.Name = "splitContainerBoardPart";
            this.splitContainerBoardPart.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerBoardPart.Panel1
            // 
            this.splitContainerBoardPart.Panel1.Controls.Add(this.labelPosition);
            this.splitContainerBoardPart.Panel1.Controls.Add(this.textBoxPosition);
            this.splitContainerBoardPart.Panel1.Controls.Add(this.labelLocation);
            this.splitContainerBoardPart.Panel1.Controls.Add(this.labelDescription);
            this.splitContainerBoardPart.Panel1.Controls.Add(this.comboBoxLocation);
            this.splitContainerBoardPart.Panel1.Controls.Add(this.textBoxDescription);
            // 
            // splitContainerBoardPart.Panel2
            // 
            this.splitContainerBoardPart.Panel2.Controls.Add(this.groupBoxPart);
            this.splitContainerBoardPart.Size = new System.Drawing.Size(522, 300);
            this.splitContainerBoardPart.SplitterDistance = 104;
            this.splitContainerBoardPart.TabIndex = 0;
            // 
            // BoardPartEntryForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(534, 342);
            this.Controls.Add(this.splitContainerBoardPart);
            this.Controls.Add(this.buttonNewPart);
            this.Controls.Add(this.buttonFindPart);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(480, 380);
            this.Name = "BoardPartEntryForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Board Part Entry";
            this.Shown += new System.EventHandler(this.BoardPartEntryForm_Shown);
            this.groupBoxPart.ResumeLayout(false);
            this.groupBoxPart.PerformLayout();
            this.splitContainerBoardPart.Panel1.ResumeLayout(false);
            this.splitContainerBoardPart.Panel1.PerformLayout();
            this.splitContainerBoardPart.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBoardPart)).EndInit();
            this.splitContainerBoardPart.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelPosition;
        private Common.Forms.TextBox textBoxPosition;
        private System.Windows.Forms.Label labelDescription;
        private Common.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.GroupBox groupBoxPart;
        private Arcade.Forms.Button buttonCancel;
        private Arcade.Forms.Button buttonOK;
        private Common.Forms.ListView listViewParts;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderCategory;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderPackage;
        private Common.Forms.TextBoxTimer textBoxTimerFind;
        private System.Windows.Forms.Label labelFind;
        private System.Windows.Forms.Label labelParts;
        private Arcade.Forms.Button buttonFindPart;
        private System.Windows.Forms.ComboBox comboBoxLocation;
        private System.Windows.Forms.Label labelLocation;
        private Arcade.Forms.Button buttonNewPart;
        private System.Windows.Forms.SplitContainer splitContainerBoardPart;
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
