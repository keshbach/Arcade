/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade.Forms
{
    partial class BoardPartDetailsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoardPartDetailsForm));
            this.labelBoardParts = new System.Windows.Forms.Label();
            this.buttonAdd = new Arcade.Forms.Button();
            this.buttonEdit = new Arcade.Forms.Button();
            this.buttonDelete = new Arcade.Forms.Button();
            this.buttonClose = new Arcade.Forms.Button();
            this.listViewBoardParts = new Common.Forms.ListView();
            this.columnHeaderPosition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderKeyword = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPackage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelSorting = new System.Windows.Forms.Label();
            this.comboBoxSorting = new System.Windows.Forms.ComboBox();
            this.buttonExport = new Arcade.Forms.Button();
            this.SuspendLayout();
            // 
            // labelBoardParts
            // 
            this.labelBoardParts.AutoSize = true;
            this.labelBoardParts.Location = new System.Drawing.Point(8, 8);
            this.labelBoardParts.Name = "labelBoardParts";
            this.labelBoardParts.Size = new System.Drawing.Size(65, 13);
            this.labelBoardParts.TabIndex = 0;
            this.labelBoardParts.Text = "&Board Parts:";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAdd.Location = new System.Drawing.Point(8, 263);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 4;
            this.buttonAdd.Text = "&Add...";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonEdit.Location = new System.Drawing.Point(90, 263);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(75, 23);
            this.buttonEdit.TabIndex = 5;
            this.buttonEdit.Text = "&Edit...";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDelete.Location = new System.Drawing.Point(172, 263);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 6;
            this.buttonDelete.Text = "&Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(639, 263);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 8;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // listViewBoardParts
            // 
            this.listViewBoardParts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewBoardParts.AutoArrange = false;
            this.listViewBoardParts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderPosition,
            this.columnHeaderKeyword,
            this.columnHeaderCategory,
            this.columnHeaderType,
            this.columnHeaderPackage,
            this.columnHeaderLocation});
            this.listViewBoardParts.ComboBoxItems = ((System.Collections.Specialized.StringCollection)(resources.GetObject("listViewBoardParts.ComboBoxItems")));
            this.listViewBoardParts.FullRowSelect = true;
            this.listViewBoardParts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewBoardParts.HideSelection = false;
            this.listViewBoardParts.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
            this.listViewBoardParts.LabelWrap = false;
            this.listViewBoardParts.Location = new System.Drawing.Point(8, 27);
            this.listViewBoardParts.MultiSelect = false;
            this.listViewBoardParts.Name = "listViewBoardParts";
            this.listViewBoardParts.ShowItemToolTips = true;
            this.listViewBoardParts.Size = new System.Drawing.Size(706, 198);
            this.listViewBoardParts.SortArrow = Common.Forms.ListView.ESortArrow.None;
            this.listViewBoardParts.Sorting = Common.Forms.ListView.ESortOrder.None;
            this.listViewBoardParts.TabIndex = 1;
            this.listViewBoardParts.UseCompatibleStateImageBehavior = false;
            this.listViewBoardParts.View = System.Windows.Forms.View.Details;
            this.listViewBoardParts.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewBoardParts_ItemSelectionChanged);
            this.listViewBoardParts.DoubleClick += new System.EventHandler(this.listViewBoardParts_DoubleClick);
            // 
            // columnHeaderPosition
            // 
            this.columnHeaderPosition.Text = "Position";
            // 
            // columnHeaderKeyword
            // 
            this.columnHeaderKeyword.Text = "Keyword";
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
            // columnHeaderLocation
            // 
            this.columnHeaderLocation.Text = "Location";
            // 
            // labelSorting
            // 
            this.labelSorting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSorting.AutoSize = true;
            this.labelSorting.Location = new System.Drawing.Point(5, 234);
            this.labelSorting.Name = "labelSorting";
            this.labelSorting.Size = new System.Drawing.Size(43, 13);
            this.labelSorting.TabIndex = 2;
            this.labelSorting.Text = "&Sorting:";
            // 
            // comboBoxSorting
            // 
            this.comboBoxSorting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxSorting.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSorting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSorting.FormattingEnabled = true;
            this.comboBoxSorting.Location = new System.Drawing.Point(50, 231);
            this.comboBoxSorting.Name = "comboBoxSorting";
            this.comboBoxSorting.Size = new System.Drawing.Size(111, 21);
            this.comboBoxSorting.TabIndex = 3;
            this.comboBoxSorting.SelectedIndexChanged += new System.EventHandler(this.comboBoxSorting_SelectedIndexChanged);
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonExport.Location = new System.Drawing.Point(253, 263);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(75, 23);
            this.buttonExport.TabIndex = 7;
            this.buttonExport.Text = "E&xport";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // BoardPartDetailsForm
            // 
            this.AcceptButton = this.buttonClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(722, 294);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.comboBoxSorting);
            this.Controls.Add(this.labelSorting);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.listViewBoardParts);
            this.Controls.Add(this.labelBoardParts);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(730, 328);
            this.Name = "BoardPartDetailsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Board Parts...";
            this.Load += new System.EventHandler(this.BoardPartDetailsForm_Load);
            this.Shown += new System.EventHandler(this.BoardPartDetailsForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelBoardParts;
        private Common.Forms.ListView listViewBoardParts;
        private System.Windows.Forms.ColumnHeader columnHeaderPosition;
        private System.Windows.Forms.ColumnHeader columnHeaderKeyword;
        private System.Windows.Forms.ColumnHeader columnHeaderCategory;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderPackage;
        private Arcade.Forms.Button buttonAdd;
        private Arcade.Forms.Button buttonEdit;
        private Arcade.Forms.Button buttonDelete;
        private Arcade.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelSorting;
        private System.Windows.Forms.ComboBox comboBoxSorting;
        private System.Windows.Forms.ColumnHeader columnHeaderLocation;
        private Arcade.Forms.Button buttonExport;
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
