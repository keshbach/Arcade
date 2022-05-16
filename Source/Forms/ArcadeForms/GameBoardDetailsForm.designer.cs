/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade.Forms
{
    partial class GameBoardDetailsForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameBoardDetailsForm));
        this.labelBoards = new System.Windows.Forms.Label();
        this.listViewBoards = new Common.Forms.ListView();
        this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
        this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
        this.buttonClose = new System.Windows.Forms.Button();
        this.buttonAdd = new System.Windows.Forms.Button();
        this.buttonEdit = new System.Windows.Forms.Button();
        this.buttonDelete = new System.Windows.Forms.Button();
        this.buttonParts = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // labelBoards
        // 
        this.labelBoards.AutoSize = true;
        this.labelBoards.Location = new System.Drawing.Point(4, 4);
        this.labelBoards.Name = "labelBoards";
        this.labelBoards.Size = new System.Drawing.Size(43, 13);
        this.labelBoards.TabIndex = 0;
        this.labelBoards.Text = "&Boards:";
        // 
        // listViewBoards
        // 
        this.listViewBoards.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.listViewBoards.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
        this.columnHeaderType,
        this.columnHeaderName});
        this.listViewBoards.ComboBoxItems = ((System.Collections.Specialized.StringCollection)(resources.GetObject("listViewBoards.ComboBoxItems")));
        this.listViewBoards.FullRowSelect = true;
        this.listViewBoards.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
        this.listViewBoards.HideSelection = false;
        this.listViewBoards.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
        this.listViewBoards.LabelWrap = false;
        this.listViewBoards.Location = new System.Drawing.Point(7, 21);
        this.listViewBoards.MultiSelect = false;
        this.listViewBoards.Name = "listViewBoards";
        this.listViewBoards.ShowItemToolTips = true;
        this.listViewBoards.Size = new System.Drawing.Size(402, 178);
        this.listViewBoards.SortArrow = Common.Forms.ListView.ESortArrow.None;
        this.listViewBoards.Sorting = Common.Forms.ListView.ESortOrder.None;
        this.listViewBoards.TabIndex = 1;
        this.listViewBoards.UseCompatibleStateImageBehavior = false;
        this.listViewBoards.View = System.Windows.Forms.View.Details;
        this.listViewBoards.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewBoards_ItemSelectionChanged);
        this.listViewBoards.DoubleClick += new System.EventHandler(this.listViewBoards_DoubleClick);
        // 
        // columnHeaderType
        // 
        this.columnHeaderType.Text = "Type";
        // 
        // columnHeaderName
        // 
        this.columnHeaderName.Text = "Name";
        // 
        // buttonClose
        // 
        this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.buttonClose.Location = new System.Drawing.Point(334, 206);
        this.buttonClose.Name = "buttonClose";
        this.buttonClose.Size = new System.Drawing.Size(75, 23);
        this.buttonClose.TabIndex = 6;
        this.buttonClose.Text = "Close";
        this.buttonClose.UseVisualStyleBackColor = true;
        this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
        // 
        // buttonAdd
        // 
        this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.buttonAdd.Location = new System.Drawing.Point(7, 206);
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
        this.buttonEdit.Location = new System.Drawing.Point(89, 206);
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
        this.buttonDelete.Location = new System.Drawing.Point(171, 206);
        this.buttonDelete.Name = "buttonDelete";
        this.buttonDelete.Size = new System.Drawing.Size(75, 23);
        this.buttonDelete.TabIndex = 4;
        this.buttonDelete.Text = "&Delete";
        this.buttonDelete.UseVisualStyleBackColor = true;
        this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
        // 
        // buttonParts
        // 
        this.buttonParts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.buttonParts.Location = new System.Drawing.Point(252, 206);
        this.buttonParts.Name = "buttonParts";
        this.buttonParts.Size = new System.Drawing.Size(75, 23);
        this.buttonParts.TabIndex = 5;
        this.buttonParts.Text = "&Parts...";
        this.buttonParts.UseVisualStyleBackColor = true;
        this.buttonParts.Click += new System.EventHandler(this.buttonParts_Click);
        // 
        // GameBoardDetailsForm
        // 
        this.AcceptButton = this.buttonClose;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.buttonClose;
        this.ClientSize = new System.Drawing.Size(418, 234);
        this.Controls.Add(this.buttonParts);
        this.Controls.Add(this.buttonDelete);
        this.Controls.Add(this.buttonEdit);
        this.Controls.Add(this.buttonAdd);
        this.Controls.Add(this.buttonClose);
        this.Controls.Add(this.listViewBoards);
        this.Controls.Add(this.labelBoards);
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.MinimumSize = new System.Drawing.Size(426, 268);
        this.Name = "GameBoardDetailsForm";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Boards...";
        this.Load += new System.EventHandler(this.GameBoardDetailsForm_Load);
        this.ResumeLayout(false);
        this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelBoards;
        private Common.Forms.ListView listViewBoards;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonParts;
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
