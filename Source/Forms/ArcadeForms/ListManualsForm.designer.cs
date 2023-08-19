/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade.Forms
{
    partial class ListManualsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListManualsForm));
            this.labelManuals = new System.Windows.Forms.Label();
            this.listViewManuals = new Common.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPrintEdition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderManufacturer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStorageBox = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonAdd = new Arcade.Forms.Button();
            this.buttonEdit = new Arcade.Forms.Button();
            this.buttonDelete = new Arcade.Forms.Button();
            this.buttonCancel = new Arcade.Forms.Button();
            this.buttonOK = new Arcade.Forms.Button();
            this.labelKeyword = new System.Windows.Forms.Label();
            this.textBoxKeyword = new Common.Forms.TextBox();
            this.buttonSearch = new Arcade.Forms.Button();
            this.buttonClear = new Arcade.Forms.Button();
            this.SuspendLayout();
            // 
            // labelManuals
            // 
            this.labelManuals.AutoSize = true;
            this.labelManuals.Location = new System.Drawing.Point(6, 38);
            this.labelManuals.Name = "labelManuals";
            this.labelManuals.Size = new System.Drawing.Size(50, 13);
            this.labelManuals.TabIndex = 4;
            this.labelManuals.Text = "&Manuals:";
            // 
            // listViewManuals
            // 
            this.listViewManuals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewManuals.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderPrintEdition,
            this.columnHeaderManufacturer,
            this.columnHeaderStorageBox});
            this.listViewManuals.ComboBoxItems = ((System.Collections.Specialized.StringCollection)(resources.GetObject("listViewManuals.ComboBoxItems")));
            this.listViewManuals.FullRowSelect = true;
            this.listViewManuals.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewManuals.HideSelection = false;
            this.listViewManuals.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
            this.listViewManuals.LabelWrap = false;
            this.listViewManuals.Location = new System.Drawing.Point(9, 54);
            this.listViewManuals.MultiSelect = false;
            this.listViewManuals.Name = "listViewManuals";
            this.listViewManuals.ShowItemToolTips = true;
            this.listViewManuals.Size = new System.Drawing.Size(400, 178);
            this.listViewManuals.SortArrow = Common.Forms.ListView.ESortArrow.None;
            this.listViewManuals.Sorting = Common.Forms.ListView.ESortOrder.None;
            this.listViewManuals.TabIndex = 5;
            this.listViewManuals.UseCompatibleStateImageBehavior = false;
            this.listViewManuals.View = System.Windows.Forms.View.Details;
            this.listViewManuals.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewManuals_ItemSelectionChanged);
            this.listViewManuals.DoubleClick += new System.EventHandler(this.listViewManuals_DoubleClick);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            // 
            // columnHeaderPrintEdition
            // 
            this.columnHeaderPrintEdition.Text = "Print Edition";
            // 
            // columnHeaderManufacturer
            // 
            this.columnHeaderManufacturer.Text = "Manufacturer";
            // 
            // columnHeaderStorageBox
            // 
            this.columnHeaderStorageBox.Text = "Storage Box";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAdd.Location = new System.Drawing.Point(8, 238);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 6;
            this.buttonAdd.Text = "&Add...";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonEdit.Location = new System.Drawing.Point(90, 238);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(75, 23);
            this.buttonEdit.TabIndex = 7;
            this.buttonEdit.Text = "&Edit...";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDelete.Location = new System.Drawing.Point(172, 238);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 8;
            this.buttonDelete.Text = "&Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(334, 238);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(253, 238);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 9;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelKeyword
            // 
            this.labelKeyword.AutoSize = true;
            this.labelKeyword.Location = new System.Drawing.Point(6, 11);
            this.labelKeyword.Name = "labelKeyword";
            this.labelKeyword.Size = new System.Drawing.Size(51, 13);
            this.labelKeyword.TabIndex = 0;
            this.labelKeyword.Text = "&Keyword:";
            // 
            // textBoxKeyword
            // 
            this.textBoxKeyword.Location = new System.Drawing.Point(56, 8);
            this.textBoxKeyword.MaxLength = 150;
            this.textBoxKeyword.Name = "textBoxKeyword";
            this.textBoxKeyword.Size = new System.Drawing.Size(175, 20);
            this.textBoxKeyword.TabIndex = 1;
            this.textBoxKeyword.WordWrap = false;
            this.textBoxKeyword.TextChanged += new System.EventHandler(this.textBoxKeyword_TextChanged);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(237, 7);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 2;
            this.buttonSearch.Text = "&Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(318, 7);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "&Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // ListManualsForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(418, 268);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.textBoxKeyword);
            this.Controls.Add(this.labelKeyword);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.listViewManuals);
            this.Controls.Add(this.labelManuals);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(426, 302);
            this.Name = "ListManualsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find Manual...";
            this.Shown += new System.EventHandler(this.ListManualsForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelManuals;
        private Common.Forms.ListView listViewManuals;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderPrintEdition;
        private System.Windows.Forms.ColumnHeader columnHeaderManufacturer;
        private System.Windows.Forms.ColumnHeader columnHeaderStorageBox;
        private Arcade.Forms.Button buttonAdd;
        private Arcade.Forms.Button buttonEdit;
        private Arcade.Forms.Button buttonDelete;
        private Arcade.Forms.Button buttonCancel;
        private Arcade.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelKeyword;
        private Common.Forms.TextBox textBoxKeyword;
        private Arcade.Forms.Button buttonSearch;
        private Arcade.Forms.Button buttonClear;
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
