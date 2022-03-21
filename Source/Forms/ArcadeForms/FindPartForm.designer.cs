/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade
{
    namespace Forms
    {
        partial class FindPartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindPartForm));
            this.textBoxKeyword = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.labelParts = new System.Windows.Forms.Label();
            this.listViewParts = new Common.Forms.ListView();
            this.columnHeaderKeyword = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPackage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonDetails = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonGames = new System.Windows.Forms.Button();
            this.radioButtonKeyword = new System.Windows.Forms.RadioButton();
            this.radioButtonType = new System.Windows.Forms.RadioButton();
            this.comboBoxType = new Common.Forms.ComboBox();
            this.groupBoxSearchParameter = new System.Windows.Forms.GroupBox();
            this.groupBoxSearchParameter.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxKeyword
            // 
            this.textBoxKeyword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxKeyword.Location = new System.Drawing.Point(76, 19);
            this.textBoxKeyword.MaxLength = 100;
            this.textBoxKeyword.Name = "textBoxKeyword";
            this.textBoxKeyword.Size = new System.Drawing.Size(356, 20);
            this.textBoxKeyword.TabIndex = 2;
            this.textBoxKeyword.TextChanged += new System.EventHandler(this.textBoxKeyword_TextChanged);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(452, 12);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 1;
            this.buttonSearch.Text = "&Search";
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // labelParts
            // 
            this.labelParts.Location = new System.Drawing.Point(5, 88);
            this.labelParts.Name = "labelParts";
            this.labelParts.Size = new System.Drawing.Size(48, 16);
            this.labelParts.TabIndex = 2;
            this.labelParts.Text = "&Part(s):";
            // 
            // listViewParts
            // 
            this.listViewParts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewParts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderKeyword,
            this.columnHeaderCategory,
            this.columnHeaderType,
            this.columnHeaderPackage});
            this.listViewParts.ComboBoxItems = ((System.Collections.Specialized.StringCollection)(resources.GetObject("listViewParts.ComboBoxItems")));
            this.listViewParts.FullRowSelect = true;
            this.listViewParts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewParts.HideSelection = false;
            this.listViewParts.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
            this.listViewParts.LabelWrap = false;
            this.listViewParts.Location = new System.Drawing.Point(8, 107);
            this.listViewParts.MultiSelect = false;
            this.listViewParts.Name = "listViewParts";
            this.listViewParts.Size = new System.Drawing.Size(516, 139);
            this.listViewParts.SortArrow = Common.Forms.ListView.ESortArrow.None;
            this.listViewParts.Sorting = Common.Forms.ListView.ESortOrder.None;
            this.listViewParts.TabIndex = 3;
            this.listViewParts.UseCompatibleStateImageBehavior = false;
            this.listViewParts.View = System.Windows.Forms.View.Details;
            this.listViewParts.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewParts_ItemSelectionChanged);
            this.listViewParts.DoubleClick += new System.EventHandler(this.listViewParts_DoubleClick);
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
            // buttonDetails
            // 
            this.buttonDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDetails.Location = new System.Drawing.Point(8, 254);
            this.buttonDetails.Name = "buttonDetails";
            this.buttonDetails.Size = new System.Drawing.Size(75, 23);
            this.buttonDetails.TabIndex = 4;
            this.buttonDetails.Text = "&Details...";
            this.buttonDetails.Click += new System.EventHandler(this.buttonDetails_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(452, 254);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 6;
            this.buttonClose.Text = "Close";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonGames
            // 
            this.buttonGames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonGames.Location = new System.Drawing.Point(90, 254);
            this.buttonGames.Name = "buttonGames";
            this.buttonGames.Size = new System.Drawing.Size(75, 23);
            this.buttonGames.TabIndex = 5;
            this.buttonGames.Text = "&Games...";
            this.buttonGames.Click += new System.EventHandler(this.buttonGames_Click);
            // 
            // radioButtonKeyword
            // 
            this.radioButtonKeyword.AutoSize = true;
            this.radioButtonKeyword.Checked = true;
            this.radioButtonKeyword.Location = new System.Drawing.Point(7, 19);
            this.radioButtonKeyword.Name = "radioButtonKeyword";
            this.radioButtonKeyword.Size = new System.Drawing.Size(69, 17);
            this.radioButtonKeyword.TabIndex = 0;
            this.radioButtonKeyword.TabStop = true;
            this.radioButtonKeyword.Text = "&Keyword:";
            this.radioButtonKeyword.UseVisualStyleBackColor = true;
            this.radioButtonKeyword.CheckedChanged += new System.EventHandler(this.radioButtonKeyword_CheckedChanged);
            // 
            // radioButtonType
            // 
            this.radioButtonType.AutoSize = true;
            this.radioButtonType.Location = new System.Drawing.Point(7, 45);
            this.radioButtonType.Name = "radioButtonType";
            this.radioButtonType.Size = new System.Drawing.Size(52, 17);
            this.radioButtonType.TabIndex = 1;
            this.radioButtonType.Text = "&Type:";
            this.radioButtonType.UseVisualStyleBackColor = true;
            this.radioButtonType.CheckedChanged += new System.EventHandler(this.radioButtonType_CheckedChanged);
            // 
            // comboBoxType
            // 
            this.comboBoxType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(76, 44);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(356, 21);
            this.comboBoxType.TabIndex = 3;
            this.comboBoxType.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxType_Validating);
            // 
            // groupBoxSearchParameter
            // 
            this.groupBoxSearchParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchParameter.Controls.Add(this.textBoxKeyword);
            this.groupBoxSearchParameter.Controls.Add(this.comboBoxType);
            this.groupBoxSearchParameter.Controls.Add(this.radioButtonKeyword);
            this.groupBoxSearchParameter.Controls.Add(this.radioButtonType);
            this.groupBoxSearchParameter.Location = new System.Drawing.Point(8, 7);
            this.groupBoxSearchParameter.Name = "groupBoxSearchParameter";
            this.groupBoxSearchParameter.Size = new System.Drawing.Size(438, 73);
            this.groupBoxSearchParameter.TabIndex = 0;
            this.groupBoxSearchParameter.TabStop = false;
            this.groupBoxSearchParameter.Text = "Search Parameter:";
            // 
            // FindPartForm
            // 
            this.AcceptButton = this.buttonSearch;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(534, 286);
            this.Controls.Add(this.groupBoxSearchParameter);
            this.Controls.Add(this.buttonGames);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonDetails);
            this.Controls.Add(this.listViewParts);
            this.Controls.Add(this.labelParts);
            this.Controls.Add(this.buttonSearch);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 320);
            this.Name = "FindPartForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find Part...";
            this.Load += new System.EventHandler(this.FindPartForm_Load);
            this.groupBoxSearchParameter.ResumeLayout(false);
            this.groupBoxSearchParameter.PerformLayout();
            this.ResumeLayout(false);

            }
            #endregion

            private System.Windows.Forms.TextBox textBoxKeyword;
            private System.Windows.Forms.Button buttonSearch;
            private System.Windows.Forms.Label labelParts;
            private Common.Forms.ListView listViewParts;
            private System.Windows.Forms.ColumnHeader columnHeaderKeyword;
            private System.Windows.Forms.ColumnHeader columnHeaderCategory;
            private System.Windows.Forms.ColumnHeader columnHeaderType;
            private System.Windows.Forms.ColumnHeader columnHeaderPackage;
            private System.Windows.Forms.Button buttonDetails;
            private System.Windows.Forms.Button buttonClose;
            private System.Windows.Forms.Button buttonGames;
            private System.Windows.Forms.RadioButton radioButtonKeyword;
            private System.Windows.Forms.RadioButton radioButtonType;
            private Common.Forms.ComboBox comboBoxType;
            private System.Windows.Forms.GroupBox groupBoxSearchParameter;
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
