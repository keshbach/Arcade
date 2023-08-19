/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade.Forms
{
    partial class FindGameBoardForm
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
            this.labelKeyword = new System.Windows.Forms.Label();
            this.textBoxKeyword = new Common.Forms.TextBox();
            this.listViewGames = new Common.Forms.ListView();
            this.columnHeaderGameName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonClose = new Arcade.Forms.Button();
            this.buttonSearch = new Arcade.Forms.Button();
            this.listViewBoard = new Common.Forms.ListView();
            this.columnHeaderCartridge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBoardName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainerGameBoards = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGameBoards)).BeginInit();
            this.splitContainerGameBoards.Panel1.SuspendLayout();
            this.splitContainerGameBoards.Panel2.SuspendLayout();
            this.splitContainerGameBoards.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelKeyword
            // 
            this.labelKeyword.AutoSize = true;
            this.labelKeyword.Location = new System.Drawing.Point(10, 15);
            this.labelKeyword.Name = "labelKeyword";
            this.labelKeyword.Size = new System.Drawing.Size(51, 13);
            this.labelKeyword.TabIndex = 0;
            this.labelKeyword.Text = "&Keyword:";
            // 
            // textBoxKeyword
            // 
            this.textBoxKeyword.Location = new System.Drawing.Point(67, 12);
            this.textBoxKeyword.MaxLength = 200;
            this.textBoxKeyword.Name = "textBoxKeyword";
            this.textBoxKeyword.Size = new System.Drawing.Size(160, 20);
            this.textBoxKeyword.TabIndex = 1;
            this.textBoxKeyword.TextChanged += new System.EventHandler(this.textBoxKeyword_TextChanged);
            // 
            // listViewGames
            // 
            this.listViewGames.AutoArrange = false;
            this.listViewGames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderGameName});
            this.listViewGames.ComboBoxItems = null;
            this.listViewGames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewGames.FullRowSelect = true;
            this.listViewGames.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewGames.HideSelection = false;
            this.listViewGames.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
            this.listViewGames.LabelWrap = false;
            this.listViewGames.Location = new System.Drawing.Point(0, 0);
            this.listViewGames.MultiSelect = false;
            this.listViewGames.Name = "listViewGames";
            this.listViewGames.ShowGroups = false;
            this.listViewGames.Size = new System.Drawing.Size(219, 186);
            this.listViewGames.SortArrow = Common.Forms.ListView.ESortArrow.None;
            this.listViewGames.Sorting = Common.Forms.ListView.ESortOrder.None;
            this.listViewGames.TabIndex = 0;
            this.listViewGames.UseCompatibleStateImageBehavior = false;
            this.listViewGames.View = System.Windows.Forms.View.Details;
            this.listViewGames.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewGames_ItemSelectionChanged);
            // 
            // columnHeaderGameName
            // 
            this.columnHeaderGameName.Text = "Name";
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(527, 230);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(526, 10);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 2;
            this.buttonSearch.Text = "&Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // listViewBoard
            // 
            this.listViewBoard.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderCartridge,
            this.columnHeaderType,
            this.columnHeaderBoardName});
            this.listViewBoard.ComboBoxItems = null;
            this.listViewBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewBoard.FullRowSelect = true;
            this.listViewBoard.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewBoard.HideSelection = false;
            this.listViewBoard.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
            this.listViewBoard.LabelWrap = false;
            this.listViewBoard.Location = new System.Drawing.Point(0, 0);
            this.listViewBoard.MultiSelect = false;
            this.listViewBoard.Name = "listViewBoard";
            this.listViewBoard.ShowGroups = false;
            this.listViewBoard.Size = new System.Drawing.Size(366, 186);
            this.listViewBoard.SortArrow = Common.Forms.ListView.ESortArrow.None;
            this.listViewBoard.Sorting = Common.Forms.ListView.ESortOrder.None;
            this.listViewBoard.TabIndex = 0;
            this.listViewBoard.UseCompatibleStateImageBehavior = false;
            this.listViewBoard.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderCartridge
            // 
            this.columnHeaderCartridge.Text = "Cartridge";
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Type";
            // 
            // columnHeaderBoardName
            // 
            this.columnHeaderBoardName.Text = "Name";
            // 
            // splitContainerGameBoards
            // 
            this.splitContainerGameBoards.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerGameBoards.Location = new System.Drawing.Point(12, 38);
            this.splitContainerGameBoards.Name = "splitContainerGameBoards";
            // 
            // splitContainerGameBoards.Panel1
            // 
            this.splitContainerGameBoards.Panel1.Controls.Add(this.listViewGames);
            // 
            // splitContainerGameBoards.Panel2
            // 
            this.splitContainerGameBoards.Panel2.Controls.Add(this.listViewBoard);
            this.splitContainerGameBoards.Size = new System.Drawing.Size(589, 186);
            this.splitContainerGameBoards.SplitterDistance = 219;
            this.splitContainerGameBoards.TabIndex = 3;
            // 
            // FindGameBoardForm
            // 
            this.AcceptButton = this.buttonSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(614, 262);
            this.Controls.Add(this.splitContainerGameBoards);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.textBoxKeyword);
            this.Controls.Add(this.labelKeyword);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(630, 300);
            this.Name = "FindGameBoardForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find Game Board";
            this.Load += new System.EventHandler(this.FindGameBoardForm_Load);
            this.splitContainerGameBoards.Panel1.ResumeLayout(false);
            this.splitContainerGameBoards.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGameBoards)).EndInit();
            this.splitContainerGameBoards.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelKeyword;
        private Common.Forms.TextBox textBoxKeyword;
        private Common.Forms.ListView listViewGames;
        private Arcade.Forms.Button buttonClose;
        private Arcade.Forms.Button buttonSearch;
        private System.Windows.Forms.ColumnHeader columnHeaderGameName;
        private Common.Forms.ListView listViewBoard;
        private System.Windows.Forms.ColumnHeader columnHeaderCartridge;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderBoardName;
        private System.Windows.Forms.SplitContainer splitContainerGameBoards;
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2014-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
