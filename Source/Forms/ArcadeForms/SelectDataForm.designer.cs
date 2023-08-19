/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade.Forms
{
    partial class SelectDataForm
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
        this.labelData = new System.Windows.Forms.Label();
        this.listViewData = new Common.Forms.ListView();
        this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
        this.buttonOK = new Arcade.Forms.Button();
        this.buttonCancel = new Arcade.Forms.Button();
        this.SuspendLayout();
        // 
        // labelData
        // 
        this.labelData.AutoSize = true;
        this.labelData.Location = new System.Drawing.Point(4, 8);
        this.labelData.Name = "labelData";
        this.labelData.Size = new System.Drawing.Size(33, 13);
        this.labelData.TabIndex = 0;
        this.labelData.Text = "&Data:";
        // 
        // listViewData
        // 
        this.listViewData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.listViewData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
        this.columnHeaderName});
        this.listViewData.ComboBoxItems = null;
        this.listViewData.FullRowSelect = true;
        this.listViewData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
        this.listViewData.HideSelection = false;
        this.listViewData.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
        this.listViewData.LabelWrap = false;
        this.listViewData.Location = new System.Drawing.Point(7, 24);
        this.listViewData.MultiSelect = false;
        this.listViewData.Name = "listViewData";
        this.listViewData.ShowGroups = false;
        this.listViewData.Size = new System.Drawing.Size(262, 120);
        this.listViewData.SortArrow = Common.Forms.ListView.ESortArrow.None;
        this.listViewData.Sorting = Common.Forms.ListView.ESortOrder.None;
        this.listViewData.TabIndex = 1;
        this.listViewData.UseCompatibleStateImageBehavior = false;
        this.listViewData.View = System.Windows.Forms.View.Details;
        this.listViewData.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewGameData_ItemSelectionChanged);
        this.listViewData.DoubleClick += new System.EventHandler(this.listViewGameData_DoubleClick);
        // 
        // columnHeaderName
        // 
        this.columnHeaderName.Text = "Name";
        this.columnHeaderName.Width = 100;
        // 
        // buttonOK
        // 
        this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.buttonOK.Location = new System.Drawing.Point(113, 150);
        this.buttonOK.Name = "buttonOK";
        this.buttonOK.Size = new System.Drawing.Size(75, 23);
        this.buttonOK.TabIndex = 2;
        this.buttonOK.Text = "OK";
        this.buttonOK.UseVisualStyleBackColor = true;
        this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
        // 
        // buttonCancel
        // 
        this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.buttonCancel.Location = new System.Drawing.Point(194, 150);
        this.buttonCancel.Name = "buttonCancel";
        this.buttonCancel.Size = new System.Drawing.Size(75, 23);
        this.buttonCancel.TabIndex = 3;
        this.buttonCancel.Text = "Cancel";
        this.buttonCancel.UseVisualStyleBackColor = true;
        this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        // 
        // SelectDataForm
        // 
        this.AcceptButton = this.buttonOK;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.buttonCancel;
        this.ClientSize = new System.Drawing.Size(278, 180);
        this.Controls.Add(this.buttonCancel);
        this.Controls.Add(this.buttonOK);
        this.Controls.Add(this.listViewData);
        this.Controls.Add(this.labelData);
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.MinimumSize = new System.Drawing.Size(180, 214);
        this.Name = "SelectDataForm";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "SelectDataForm";
        this.Load += new System.EventHandler(this.SelectDataForm_Load);
        this.ResumeLayout(false);
        this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelData;
        private Common.Forms.ListView listViewData;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private Arcade.Forms.Button buttonOK;
        private Arcade.Forms.Button buttonCancel;
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
