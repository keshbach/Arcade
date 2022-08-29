/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Arcade.Forms
{
    public partial class PartEntryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PartEntryForm));
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new Common.Forms.TextBox();
            this.labelCategory = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.labelPackage = new System.Windows.Forms.Label();
            this.textBoxPartPinouts = new Common.Forms.TextBox();
            this.labelPinouts = new System.Windows.Forms.Label();
            this.comboBoxCategory = new Common.Forms.ComboBox();
            this.comboBoxPackage = new Common.Forms.ComboBox();
            this.comboBoxType = new Common.Forms.ComboBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.splitContainerParts = new System.Windows.Forms.SplitContainer();
            this.groupBoxDatasheets = new System.Windows.Forms.GroupBox();
            this.listViewDatasheets = new Common.Forms.ListView();
            this.columnHeaderFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonViewDatasheet = new System.Windows.Forms.Button();
            this.buttonAddDatasheet = new System.Windows.Forms.Button();
            this.buttonDeleteDatasheet = new System.Windows.Forms.Button();
            this.checkBoxDefault = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerParts)).BeginInit();
            this.splitContainerParts.Panel1.SuspendLayout();
            this.splitContainerParts.Panel2.SuspendLayout();
            this.splitContainerParts.SuspendLayout();
            this.groupBoxDatasheets.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(1, 6);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(40, 16);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "&Name:";
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(65, 3);
            this.textBoxName.MaxLength = 50;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(202, 20);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            this.textBoxName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxName_KeyPress);
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(3, 32);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(56, 16);
            this.labelCategory.TabIndex = 2;
            this.labelCategory.Text = "&Category:";
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(3, 61);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(40, 16);
            this.labelType.TabIndex = 4;
            this.labelType.Text = "&Type:";
            // 
            // labelPackage
            // 
            this.labelPackage.Location = new System.Drawing.Point(3, 91);
            this.labelPackage.Name = "labelPackage";
            this.labelPackage.Size = new System.Drawing.Size(56, 16);
            this.labelPackage.TabIndex = 6;
            this.labelPackage.Text = "&Package:";
            // 
            // textBoxPartPinouts
            // 
            this.textBoxPartPinouts.AcceptsReturn = true;
            this.textBoxPartPinouts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPartPinouts.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPartPinouts.HideSelection = false;
            this.textBoxPartPinouts.Location = new System.Drawing.Point(6, 17);
            this.textBoxPartPinouts.MaxLength = 2000000;
            this.textBoxPartPinouts.Multiline = true;
            this.textBoxPartPinouts.Name = "textBoxPartPinouts";
            this.textBoxPartPinouts.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxPartPinouts.Size = new System.Drawing.Size(407, 314);
            this.textBoxPartPinouts.TabIndex = 1;
            this.textBoxPartPinouts.WordWrap = false;
            this.textBoxPartPinouts.TextChanged += new System.EventHandler(this.textBoxPartPinouts_TextChanged);
            // 
            // labelPinouts
            // 
            this.labelPinouts.AutoSize = true;
            this.labelPinouts.Location = new System.Drawing.Point(0, 0);
            this.labelPinouts.Name = "labelPinouts";
            this.labelPinouts.Size = new System.Drawing.Size(45, 13);
            this.labelPinouts.TabIndex = 0;
            this.labelPinouts.Text = "P&inouts:";
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.Location = new System.Drawing.Point(65, 29);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(202, 21);
            this.comboBoxCategory.TabIndex = 3;
            this.comboBoxCategory.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxCategory_Validating);
            // 
            // comboBoxPackage
            // 
            this.comboBoxPackage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPackage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxPackage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxPackage.FormattingEnabled = true;
            this.comboBoxPackage.Location = new System.Drawing.Point(65, 88);
            this.comboBoxPackage.Name = "comboBoxPackage";
            this.comboBoxPackage.Size = new System.Drawing.Size(202, 21);
            this.comboBoxPackage.TabIndex = 7;
            this.comboBoxPackage.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxPackage_Validating);
            // 
            // comboBoxType
            // 
            this.comboBoxType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(65, 58);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(202, 21);
            this.comboBoxType.TabIndex = 5;
            this.comboBoxType.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxType_Validating);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(546, 352);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(627, 352);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // splitContainerParts
            // 
            this.splitContainerParts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerParts.Location = new System.Drawing.Point(11, 12);
            this.splitContainerParts.Name = "splitContainerParts";
            // 
            // splitContainerParts.Panel1
            // 
            this.splitContainerParts.Panel1.Controls.Add(this.groupBoxDatasheets);
            this.splitContainerParts.Panel1.Controls.Add(this.checkBoxDefault);
            this.splitContainerParts.Panel1.Controls.Add(this.labelPackage);
            this.splitContainerParts.Panel1.Controls.Add(this.labelName);
            this.splitContainerParts.Panel1.Controls.Add(this.textBoxName);
            this.splitContainerParts.Panel1.Controls.Add(this.labelCategory);
            this.splitContainerParts.Panel1.Controls.Add(this.comboBoxType);
            this.splitContainerParts.Panel1.Controls.Add(this.labelType);
            this.splitContainerParts.Panel1.Controls.Add(this.comboBoxPackage);
            this.splitContainerParts.Panel1.Controls.Add(this.comboBoxCategory);
            // 
            // splitContainerParts.Panel2
            // 
            this.splitContainerParts.Panel2.Controls.Add(this.labelPinouts);
            this.splitContainerParts.Panel2.Controls.Add(this.textBoxPartPinouts);
            this.splitContainerParts.Size = new System.Drawing.Size(691, 334);
            this.splitContainerParts.SplitterDistance = 270;
            this.splitContainerParts.TabIndex = 0;
            // 
            // groupBoxDatasheets
            // 
            this.groupBoxDatasheets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDatasheets.Controls.Add(this.listViewDatasheets);
            this.groupBoxDatasheets.Controls.Add(this.buttonViewDatasheet);
            this.groupBoxDatasheets.Controls.Add(this.buttonAddDatasheet);
            this.groupBoxDatasheets.Controls.Add(this.buttonDeleteDatasheet);
            this.groupBoxDatasheets.Location = new System.Drawing.Point(1, 147);
            this.groupBoxDatasheets.Name = "groupBoxDatasheets";
            this.groupBoxDatasheets.Size = new System.Drawing.Size(263, 184);
            this.groupBoxDatasheets.TabIndex = 9;
            this.groupBoxDatasheets.TabStop = false;
            this.groupBoxDatasheets.Text = "Data&sheets:";
            // 
            // listViewDatasheets
            // 
            this.listViewDatasheets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewDatasheets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFile});
            this.listViewDatasheets.ComboBoxItems = ((System.Collections.Specialized.StringCollection)(resources.GetObject("listViewDatasheets.ComboBoxItems")));
            this.listViewDatasheets.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewDatasheets.HideSelection = false;
            this.listViewDatasheets.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
            this.listViewDatasheets.LabelWrap = false;
            this.listViewDatasheets.Location = new System.Drawing.Point(6, 19);
            this.listViewDatasheets.MultiSelect = false;
            this.listViewDatasheets.Name = "listViewDatasheets";
            this.listViewDatasheets.Size = new System.Drawing.Size(249, 129);
            this.listViewDatasheets.SortArrow = Common.Forms.ListView.ESortArrow.None;
            this.listViewDatasheets.Sorting = Common.Forms.ListView.ESortOrder.None;
            this.listViewDatasheets.TabIndex = 0;
            this.listViewDatasheets.UseCompatibleStateImageBehavior = false;
            this.listViewDatasheets.View = System.Windows.Forms.View.Details;
            this.listViewDatasheets.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewDatasheets_ItemSelectionChanged);
            this.listViewDatasheets.DoubleClick += new System.EventHandler(this.listViewDatasheets_DoubleClick);
            // 
            // columnHeaderFile
            // 
            this.columnHeaderFile.Text = "File";
            // 
            // buttonViewDatasheet
            // 
            this.buttonViewDatasheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonViewDatasheet.Location = new System.Drawing.Point(168, 155);
            this.buttonViewDatasheet.Name = "buttonViewDatasheet";
            this.buttonViewDatasheet.Size = new System.Drawing.Size(75, 23);
            this.buttonViewDatasheet.TabIndex = 3;
            this.buttonViewDatasheet.Text = "&View";
            this.buttonViewDatasheet.UseVisualStyleBackColor = true;
            this.buttonViewDatasheet.Click += new System.EventHandler(this.buttonViewDatasheet_Click);
            // 
            // buttonAddDatasheet
            // 
            this.buttonAddDatasheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddDatasheet.Location = new System.Drawing.Point(6, 155);
            this.buttonAddDatasheet.Name = "buttonAddDatasheet";
            this.buttonAddDatasheet.Size = new System.Drawing.Size(75, 23);
            this.buttonAddDatasheet.TabIndex = 1;
            this.buttonAddDatasheet.Text = "&Add...";
            this.buttonAddDatasheet.UseVisualStyleBackColor = true;
            this.buttonAddDatasheet.Click += new System.EventHandler(this.buttonAddDatasheet_Click);
            // 
            // buttonDeleteDatasheet
            // 
            this.buttonDeleteDatasheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDeleteDatasheet.Location = new System.Drawing.Point(87, 155);
            this.buttonDeleteDatasheet.Name = "buttonDeleteDatasheet";
            this.buttonDeleteDatasheet.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteDatasheet.TabIndex = 2;
            this.buttonDeleteDatasheet.Text = "&Delete";
            this.buttonDeleteDatasheet.UseVisualStyleBackColor = true;
            this.buttonDeleteDatasheet.Click += new System.EventHandler(this.buttonDeleteDatasheet_Click);
            // 
            // checkBoxDefault
            // 
            this.checkBoxDefault.AutoSize = true;
            this.checkBoxDefault.Location = new System.Drawing.Point(4, 124);
            this.checkBoxDefault.Name = "checkBoxDefault";
            this.checkBoxDefault.Size = new System.Drawing.Size(89, 17);
            this.checkBoxDefault.TabIndex = 8;
            this.checkBoxDefault.Text = "D&efault name";
            this.checkBoxDefault.UseVisualStyleBackColor = true;
            this.checkBoxDefault.CheckedChanged += new System.EventHandler(this.checkBoxDefault_CheckedChanged);
            // 
            // PartEntryForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(710, 380);
            this.Controls.Add(this.splitContainerParts);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(718, 414);
            this.Name = "PartEntryForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add...";
            this.Shown += new System.EventHandler(this.PartEntryForm_Shown);
            this.splitContainerParts.Panel1.ResumeLayout(false);
            this.splitContainerParts.Panel1.PerformLayout();
            this.splitContainerParts.Panel2.ResumeLayout(false);
            this.splitContainerParts.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerParts)).EndInit();
            this.splitContainerParts.ResumeLayout(false);
            this.groupBoxDatasheets.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label labelName;
        private Common.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelCategory;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.Label labelPackage;
        private Common.Forms.TextBox textBoxPartPinouts;
        private System.Windows.Forms.Label labelPinouts;
        private Common.Forms.ComboBox comboBoxCategory;
        private Common.Forms.ComboBox comboBoxPackage;
        private Common.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private SplitContainer splitContainerParts;
        private GroupBox groupBoxDatasheets;
        private Common.Forms.ListView listViewDatasheets;
        private ColumnHeader columnHeaderFile;
        private Button buttonViewDatasheet;
        private Button buttonAddDatasheet;
        private Button buttonDeleteDatasheet;
        private CheckBox checkBoxDefault;
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
