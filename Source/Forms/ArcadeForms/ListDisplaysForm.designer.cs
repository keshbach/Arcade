/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade.Forms
{
    partial class ListDisplaysForm
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
            this.labelDisplays = new System.Windows.Forms.Label();
            this.listViewDisplays = new Common.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonAdd = new Arcade.Forms.Button();
            this.buttonEdit = new Arcade.Forms.Button();
            this.buttonDelete = new Arcade.Forms.Button();
            this.buttonOK = new Arcade.Forms.Button();
            this.buttonCancel = new Arcade.Forms.Button();
            this.SuspendLayout();
            // 
            // labelDisplays
            // 
            this.labelDisplays.AutoSize = true;
            this.labelDisplays.Location = new System.Drawing.Point(5, 9);
            this.labelDisplays.Name = "labelDisplays";
            this.labelDisplays.Size = new System.Drawing.Size(49, 13);
            this.labelDisplays.TabIndex = 0;
            this.labelDisplays.Text = "&Displays:";
            // 
            // listViewDisplays
            // 
            this.listViewDisplays.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewDisplays.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName});
            this.listViewDisplays.ComboBoxItems = null;
            this.listViewDisplays.FullRowSelect = true;
            this.listViewDisplays.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewDisplays.HideSelection = false;
            this.listViewDisplays.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
            this.listViewDisplays.LabelWrap = false;
            this.listViewDisplays.Location = new System.Drawing.Point(8, 25);
            this.listViewDisplays.MultiSelect = false;
            this.listViewDisplays.Name = "listViewDisplays";
            this.listViewDisplays.ShowGroups = false;
            this.listViewDisplays.Size = new System.Drawing.Size(402, 98);
            this.listViewDisplays.SortArrow = Common.Forms.ListView.ESortArrow.None;
            this.listViewDisplays.Sorting = Common.Forms.ListView.ESortOrder.None;
            this.listViewDisplays.TabIndex = 1;
            this.listViewDisplays.UseCompatibleStateImageBehavior = false;
            this.listViewDisplays.View = System.Windows.Forms.View.Details;
            this.listViewDisplays.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewDisplays_ItemSelectionChanged);
            this.listViewDisplays.DoubleClick += new System.EventHandler(this.listViewDisplays_DoubleClick);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 100;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAdd.Location = new System.Drawing.Point(8, 130);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 2;
            this.buttonAdd.Text = "&Add...";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonEdit.Location = new System.Drawing.Point(89, 130);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(75, 23);
            this.buttonEdit.TabIndex = 3;
            this.buttonEdit.Text = "&Edit...";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDelete.Location = new System.Drawing.Point(170, 130);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 4;
            this.buttonDelete.Text = "&Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(253, 130);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(335, 129);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // ListDisplaysForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(418, 160);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.listViewDisplays);
            this.Controls.Add(this.labelDisplays);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(426, 194);
            this.Name = "ListDisplaysForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find Display...";
            this.Shown += new System.EventHandler(this.ListDisplaysForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDisplays;
        private Common.Forms.ListView listViewDisplays;
        private Arcade.Forms.Button buttonAdd;
        private Arcade.Forms.Button buttonEdit;
        private Arcade.Forms.Button buttonDelete;
        private Arcade.Forms.Button buttonOK;
        private Arcade.Forms.Button buttonCancel;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
