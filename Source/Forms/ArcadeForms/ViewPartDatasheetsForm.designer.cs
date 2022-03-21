/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade
{
    namespace Forms
    {
        partial class ViewPartDatasheetsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewPartDatasheetsForm));
            this.listViewDatasheets = new Common.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonView = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelDatasheets = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listViewDatasheets
            // 
            this.listViewDatasheets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewDatasheets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName});
            this.listViewDatasheets.ComboBoxItems = ((System.Collections.Specialized.StringCollection)(resources.GetObject("listViewDatasheets.ComboBoxItems")));
            this.listViewDatasheets.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewDatasheets.HideSelection = false;
            this.listViewDatasheets.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
            this.listViewDatasheets.Location = new System.Drawing.Point(9, 26);
            this.listViewDatasheets.MultiSelect = false;
            this.listViewDatasheets.Name = "listViewDatasheets";
            this.listViewDatasheets.Size = new System.Drawing.Size(451, 93);
            this.listViewDatasheets.SortArrow = Common.Forms.ListView.ESortArrow.None;
            this.listViewDatasheets.Sorting = Common.Forms.ListView.ESortOrder.None;
            this.listViewDatasheets.TabIndex = 1;
            this.listViewDatasheets.UseCompatibleStateImageBehavior = false;
            this.listViewDatasheets.View = System.Windows.Forms.View.Details;
            this.listViewDatasheets.Click += new System.EventHandler(this.listViewDatasheets_Click);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            // 
            // buttonView
            // 
            this.buttonView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonView.Location = new System.Drawing.Point(9, 125);
            this.buttonView.Name = "buttonView";
            this.buttonView.Size = new System.Drawing.Size(75, 23);
            this.buttonView.TabIndex = 2;
            this.buttonView.Text = "&View";
            this.buttonView.UseVisualStyleBackColor = true;
            this.buttonView.Click += new System.EventHandler(this.buttonView_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonClose.Location = new System.Drawing.Point(385, 125);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelDatasheets
            // 
            this.labelDatasheets.AutoSize = true;
            this.labelDatasheets.Location = new System.Drawing.Point(6, 9);
            this.labelDatasheets.Name = "labelDatasheets";
            this.labelDatasheets.Size = new System.Drawing.Size(64, 13);
            this.labelDatasheets.TabIndex = 0;
            this.labelDatasheets.Text = "&Datasheets:";
            // 
            // ViewPartDatasheetsForm
            // 
            this.AcceptButton = this.buttonClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(468, 154);
            this.Controls.Add(this.labelDatasheets);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonView);
            this.Controls.Add(this.listViewDatasheets);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(476, 188);
            this.Name = "ViewPartDatasheetsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Datasheets...";
            this.Load += new System.EventHandler(this.ViewPartDatasheetsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

            }

            #endregion

            private Common.Forms.ListView listViewDatasheets;
            private System.Windows.Forms.Button buttonView;
            private System.Windows.Forms.Button buttonClose;
            private System.Windows.Forms.Label labelDatasheets;
            private System.Windows.Forms.ColumnHeader columnHeaderName;
        }
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
