/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2006 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade
{
    namespace Forms
    {
        partial class ListGameManualsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListGameManualsForm));
            this.labelManuals = new System.Windows.Forms.Label();
            this.listViewManuals = new Common.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStorageBox = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPrintEdition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonView = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelManuals
            // 
            this.labelManuals.AutoSize = true;
            this.labelManuals.Location = new System.Drawing.Point(7, 8);
            this.labelManuals.Name = "labelManuals";
            this.labelManuals.Size = new System.Drawing.Size(50, 13);
            this.labelManuals.TabIndex = 0;
            this.labelManuals.Text = "&Manuals:";
            // 
            // listViewManuals
            // 
            this.listViewManuals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewManuals.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderStorageBox,
            this.columnHeaderPrintEdition});
            this.listViewManuals.ComboBoxItems = ((System.Collections.Specialized.StringCollection)(resources.GetObject("listViewManuals.ComboBoxItems")));
            this.listViewManuals.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewManuals.HideSelection = false;
            this.listViewManuals.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
            this.listViewManuals.LabelWrap = false;
            this.listViewManuals.Location = new System.Drawing.Point(7, 25);
            this.listViewManuals.MultiSelect = false;
            this.listViewManuals.Name = "listViewManuals";
            this.listViewManuals.ShowItemToolTips = true;
            this.listViewManuals.Size = new System.Drawing.Size(491, 135);
            this.listViewManuals.SortArrow = Common.Forms.ListView.ESortArrow.None;
            this.listViewManuals.Sorting = Common.Forms.ListView.ESortOrder.GroupSequential;
            this.listViewManuals.TabIndex = 1;
            this.listViewManuals.UseCompatibleStateImageBehavior = false;
            this.listViewManuals.View = System.Windows.Forms.View.Details;
            this.listViewManuals.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewManuals_ItemSelectionChanged);
            this.listViewManuals.DoubleClick += new System.EventHandler(this.listViewManuals_DoubleClick);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            // 
            // columnHeaderStorageBox
            // 
            this.columnHeaderStorageBox.Text = "Storage Box";
            // 
            // columnHeaderPrintEdition
            // 
            this.columnHeaderPrintEdition.Text = "Print Edition";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAdd.Location = new System.Drawing.Point(7, 167);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 2;
            this.buttonAdd.Text = "&Add...";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDelete.Location = new System.Drawing.Point(89, 167);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 3;
            this.buttonDelete.Text = "&Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonView
            // 
            this.buttonView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonView.Location = new System.Drawing.Point(171, 167);
            this.buttonView.Name = "buttonView";
            this.buttonView.Size = new System.Drawing.Size(75, 23);
            this.buttonView.TabIndex = 4;
            this.buttonView.Text = "&View...";
            this.buttonView.UseVisualStyleBackColor = true;
            this.buttonView.Click += new System.EventHandler(this.buttonView_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(423, 167);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // ListGameManualsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(506, 196);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonView);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.listViewManuals);
            this.Controls.Add(this.labelManuals);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(514, 230);
            this.Name = "ListGameManualsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manuals...";
            this.Load += new System.EventHandler(this.ListGameManualsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

            }

            #endregion

            private System.Windows.Forms.Label labelManuals;
            private Common.Forms.ListView listViewManuals;
            private System.Windows.Forms.ColumnHeader columnHeaderName;
            private System.Windows.Forms.ColumnHeader columnHeaderStorageBox;
            private System.Windows.Forms.ColumnHeader columnHeaderPrintEdition;
            private System.Windows.Forms.Button buttonAdd;
            private System.Windows.Forms.Button buttonDelete;
            private System.Windows.Forms.Button buttonView;
            private System.Windows.Forms.Button buttonClose;
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2006 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
