/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade
{
    namespace Forms
    {
        public partial class ViewGamePartLocationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewGamePartLocationForm));
            this.listViewGames = new Common.Forms.ListView();
            this.columnHeaderGames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewBoard = new Common.Forms.ListView();
            this.columnHeaderCartridgeFlag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBoardType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBoardName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewPositionsAndLocations = new Common.Forms.ListView();
            this.columnHeaderPosition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonClose = new System.Windows.Forms.Button();
            this.checkBoxIncludeAllParts = new System.Windows.Forms.CheckBox();
            this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
            this.splitContainerRight = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).BeginInit();
            this.splitContainerLeft.Panel1.SuspendLayout();
            this.splitContainerLeft.Panel2.SuspendLayout();
            this.splitContainerLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).BeginInit();
            this.splitContainerRight.Panel1.SuspendLayout();
            this.splitContainerRight.Panel2.SuspendLayout();
            this.splitContainerRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewGames
            // 
            this.listViewGames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderGames});
            this.listViewGames.ComboBoxItems = ((System.Collections.Specialized.StringCollection)(resources.GetObject("listViewGames.ComboBoxItems")));
            this.listViewGames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewGames.FullRowSelect = true;
            this.listViewGames.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewGames.HideSelection = false;
            this.listViewGames.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
            this.listViewGames.LabelWrap = false;
            this.listViewGames.Location = new System.Drawing.Point(0, 0);
            this.listViewGames.MultiSelect = false;
            this.listViewGames.Name = "listViewGames";
            this.listViewGames.Size = new System.Drawing.Size(178, 271);
            this.listViewGames.SortArrow = Common.Forms.ListView.ESortArrow.None;
            this.listViewGames.Sorting = Common.Forms.ListView.ESortOrder.None;
            this.listViewGames.TabIndex = 0;
            this.listViewGames.UseCompatibleStateImageBehavior = false;
            this.listViewGames.View = System.Windows.Forms.View.Details;
            this.listViewGames.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewGames_ItemSelectionChanged);
            // 
            // columnHeaderGames
            // 
            this.columnHeaderGames.Text = "Games";
            this.columnHeaderGames.Width = 200;
            // 
            // listViewBoard
            // 
            this.listViewBoard.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderCartridgeFlag,
            this.columnHeaderBoardType,
            this.columnHeaderBoardName});
            this.listViewBoard.ComboBoxItems = ((System.Collections.Specialized.StringCollection)(resources.GetObject("listViewBoard.ComboBoxItems")));
            this.listViewBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewBoard.FullRowSelect = true;
            this.listViewBoard.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewBoard.HideSelection = false;
            this.listViewBoard.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
            this.listViewBoard.LabelWrap = false;
            this.listViewBoard.Location = new System.Drawing.Point(0, 0);
            this.listViewBoard.MultiSelect = false;
            this.listViewBoard.Name = "listViewBoard";
            this.listViewBoard.Size = new System.Drawing.Size(471, 271);
            this.listViewBoard.SortArrow = Common.Forms.ListView.ESortArrow.None;
            this.listViewBoard.Sorting = Common.Forms.ListView.ESortOrder.None;
            this.listViewBoard.TabIndex = 0;
            this.listViewBoard.UseCompatibleStateImageBehavior = false;
            this.listViewBoard.View = System.Windows.Forms.View.Details;
            this.listViewBoard.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewBoard_ItemSelectionChanged);
            // 
            // columnHeaderCartridgeFlag
            // 
            this.columnHeaderCartridgeFlag.Text = "Cartridge";
            // 
            // columnHeaderBoardType
            // 
            this.columnHeaderBoardType.Text = "Type";
            this.columnHeaderBoardType.Width = 200;
            // 
            // columnHeaderBoardName
            // 
            this.columnHeaderBoardName.Text = "Name";
            this.columnHeaderBoardName.Width = 200;
            // 
            // listViewPositionsAndLocations
            // 
            this.listViewPositionsAndLocations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderPosition,
            this.columnHeaderLocation});
            this.listViewPositionsAndLocations.ComboBoxItems = ((System.Collections.Specialized.StringCollection)(resources.GetObject("listViewPositionsAndLocations.ComboBoxItems")));
            this.listViewPositionsAndLocations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewPositionsAndLocations.FullRowSelect = true;
            this.listViewPositionsAndLocations.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewPositionsAndLocations.HideSelection = false;
            this.listViewPositionsAndLocations.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
            this.listViewPositionsAndLocations.LabelWrap = false;
            this.listViewPositionsAndLocations.Location = new System.Drawing.Point(0, 0);
            this.listViewPositionsAndLocations.MultiSelect = false;
            this.listViewPositionsAndLocations.Name = "listViewPositionsAndLocations";
            this.listViewPositionsAndLocations.Size = new System.Drawing.Size(168, 271);
            this.listViewPositionsAndLocations.SortArrow = Common.Forms.ListView.ESortArrow.None;
            this.listViewPositionsAndLocations.Sorting = Common.Forms.ListView.ESortOrder.None;
            this.listViewPositionsAndLocations.TabIndex = 0;
            this.listViewPositionsAndLocations.UseCompatibleStateImageBehavior = false;
            this.listViewPositionsAndLocations.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderPosition
            // 
            this.columnHeaderPosition.Text = "Position";
            // 
            // columnHeaderLocation
            // 
            this.columnHeaderLocation.Text = "Location";
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(758, 314);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // checkBoxIncludeAllParts
            // 
            this.checkBoxIncludeAllParts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxIncludeAllParts.AutoSize = true;
            this.checkBoxIncludeAllParts.Location = new System.Drawing.Point(8, 289);
            this.checkBoxIncludeAllParts.Name = "checkBoxIncludeAllParts";
            this.checkBoxIncludeAllParts.Size = new System.Drawing.Size(146, 17);
            this.checkBoxIncludeAllParts.TabIndex = 0;
            this.checkBoxIncludeAllParts.Text = "&Include all matching parts";
            this.checkBoxIncludeAllParts.UseVisualStyleBackColor = true;
            this.checkBoxIncludeAllParts.CheckedChanged += new System.EventHandler(this.checkBoxIncludeAllParts_CheckedChanged);
            // 
            // splitContainerLeft
            // 
            this.splitContainerLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerLeft.Location = new System.Drawing.Point(8, 12);
            this.splitContainerLeft.Name = "splitContainerLeft";
            // 
            // splitContainerLeft.Panel1
            // 
            this.splitContainerLeft.Panel1.Controls.Add(this.listViewGames);
            // 
            // splitContainerLeft.Panel2
            // 
            this.splitContainerLeft.Panel2.Controls.Add(this.splitContainerRight);
            this.splitContainerLeft.Size = new System.Drawing.Size(825, 271);
            this.splitContainerLeft.SplitterDistance = 178;
            this.splitContainerLeft.TabIndex = 8;
            // 
            // splitContainerRight
            // 
            this.splitContainerRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRight.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRight.Name = "splitContainerRight";
            // 
            // splitContainerRight.Panel1
            // 
            this.splitContainerRight.Panel1.Controls.Add(this.listViewBoard);
            // 
            // splitContainerRight.Panel2
            // 
            this.splitContainerRight.Panel2.Controls.Add(this.listViewPositionsAndLocations);
            this.splitContainerRight.Size = new System.Drawing.Size(643, 271);
            this.splitContainerRight.SplitterDistance = 471;
            this.splitContainerRight.TabIndex = 0;
            // 
            // ViewGamePartLocationForm
            // 
            this.AcceptButton = this.buttonClose;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(840, 344);
            this.Controls.Add(this.splitContainerLeft);
            this.Controls.Add(this.checkBoxIncludeAllParts);
            this.Controls.Add(this.buttonClose);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 382);
            this.Name = "ViewGamePartLocationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Games...";
            this.Load += new System.EventHandler(this.ViewGamePartLocationForm_Load);
            this.splitContainerLeft.Panel1.ResumeLayout(false);
            this.splitContainerLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).EndInit();
            this.splitContainerLeft.ResumeLayout(false);
            this.splitContainerRight.Panel1.ResumeLayout(false);
            this.splitContainerRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).EndInit();
            this.splitContainerRight.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

            }
            #endregion

            private Common.Forms.ListView listViewGames;
            private System.Windows.Forms.ColumnHeader columnHeaderGames;
            private Common.Forms.ListView listViewBoard;
            private System.Windows.Forms.ColumnHeader columnHeaderBoardType;
            private Common.Forms.ListView listViewPositionsAndLocations;
            private System.Windows.Forms.ColumnHeader columnHeaderPosition;
            private System.Windows.Forms.Button buttonClose;
            private System.Windows.Forms.ColumnHeader columnHeaderCartridgeFlag;
            private System.Windows.Forms.ColumnHeader columnHeaderLocation;
            private System.Windows.Forms.CheckBox checkBoxIncludeAllParts;
            private System.Windows.Forms.ColumnHeader columnHeaderBoardName;
            private System.Windows.Forms.SplitContainer splitContainerLeft;
            private System.Windows.Forms.SplitContainer splitContainerRight;
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
