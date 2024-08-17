/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade.Forms
{
    public partial class PartDetailsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PartDetailsForm));
            this.labelCategory = new System.Windows.Forms.Label();
            this.textBoxCategory = new Common.Forms.TextBox();
            this.labelType = new System.Windows.Forms.Label();
            this.textBoxType = new Common.Forms.TextBox();
            this.labelPackage = new System.Windows.Forms.Label();
            this.textBoxPackage = new Common.Forms.TextBox();
            this.labelPinouts = new System.Windows.Forms.Label();
            this.textBoxPinouts = new Common.Forms.TextBox();
            this.labelParts = new System.Windows.Forms.Label();
            this.buttonAdd = new Arcade.Forms.Button();
            this.buttonClose = new Arcade.Forms.Button();
            this.listViewKeywords = new Common.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDataSheets = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderInventory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonEdit = new Arcade.Forms.Button();
            this.buttonDelete = new Arcade.Forms.Button();
            this.buttonDatasheets = new Arcade.Forms.Button();
            this.splitContainerPart = new System.Windows.Forms.SplitContainer();
            this.buttonInventory = new Arcade.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerPart)).BeginInit();
            this.splitContainerPart.Panel1.SuspendLayout();
            this.splitContainerPart.Panel2.SuspendLayout();
            this.splitContainerPart.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelCategory
            // 
            this.labelCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCategory.Location = new System.Drawing.Point(0, 158);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(56, 16);
            this.labelCategory.TabIndex = 2;
            this.labelCategory.Text = "&Category:";
            // 
            // textBoxCategory
            // 
            this.textBoxCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCategory.Location = new System.Drawing.Point(59, 155);
            this.textBoxCategory.Name = "textBoxCategory";
            this.textBoxCategory.ReadOnly = true;
            this.textBoxCategory.Size = new System.Drawing.Size(212, 20);
            this.textBoxCategory.TabIndex = 3;
            // 
            // labelType
            // 
            this.labelType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelType.Location = new System.Drawing.Point(0, 184);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(32, 16);
            this.labelType.TabIndex = 4;
            this.labelType.Text = "&Type:";
            // 
            // textBoxType
            // 
            this.textBoxType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxType.Location = new System.Drawing.Point(59, 181);
            this.textBoxType.Name = "textBoxType";
            this.textBoxType.ReadOnly = true;
            this.textBoxType.Size = new System.Drawing.Size(212, 20);
            this.textBoxType.TabIndex = 5;
            // 
            // labelPackage
            // 
            this.labelPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelPackage.Location = new System.Drawing.Point(0, 210);
            this.labelPackage.Name = "labelPackage";
            this.labelPackage.Size = new System.Drawing.Size(56, 16);
            this.labelPackage.TabIndex = 6;
            this.labelPackage.Text = "&Package:";
            // 
            // textBoxPackage
            // 
            this.textBoxPackage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPackage.Location = new System.Drawing.Point(59, 207);
            this.textBoxPackage.Name = "textBoxPackage";
            this.textBoxPackage.ReadOnly = true;
            this.textBoxPackage.Size = new System.Drawing.Size(212, 20);
            this.textBoxPackage.TabIndex = 7;
            // 
            // labelPinouts
            // 
            this.labelPinouts.Location = new System.Drawing.Point(0, 0);
            this.labelPinouts.Name = "labelPinouts";
            this.labelPinouts.Size = new System.Drawing.Size(48, 16);
            this.labelPinouts.TabIndex = 0;
            this.labelPinouts.Text = "P&inouts:";
            // 
            // textBoxPinouts
            // 
            this.textBoxPinouts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPinouts.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPinouts.HideSelection = false;
            this.textBoxPinouts.Location = new System.Drawing.Point(3, 19);
            this.textBoxPinouts.MaxLength = 1000000;
            this.textBoxPinouts.Multiline = true;
            this.textBoxPinouts.Name = "textBoxPinouts";
            this.textBoxPinouts.ReadOnly = true;
            this.textBoxPinouts.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxPinouts.Size = new System.Drawing.Size(363, 208);
            this.textBoxPinouts.TabIndex = 1;
            this.textBoxPinouts.WordWrap = false;
            // 
            // labelParts
            // 
            this.labelParts.Location = new System.Drawing.Point(0, 0);
            this.labelParts.Name = "labelParts";
            this.labelParts.Size = new System.Drawing.Size(40, 16);
            this.labelParts.TabIndex = 0;
            this.labelParts.Text = "Pa&rts:";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAdd.Location = new System.Drawing.Point(8, 248);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "&Add...";
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(584, 248);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 6;
            this.buttonClose.Text = "Close";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // listViewKeywords
            // 
            this.listViewKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewKeywords.AutoArrange = false;
            this.listViewKeywords.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderDataSheets,
            this.columnHeaderInventory});
            this.listViewKeywords.ComboBoxItems = ((System.Collections.Specialized.StringCollection)(resources.GetObject("listViewKeywords.ComboBoxItems")));
            this.listViewKeywords.FullRowSelect = true;
            this.listViewKeywords.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewKeywords.HideSelection = false;
            this.listViewKeywords.LabelEditor = Common.Forms.ListView.ELabelEditor.None;
            this.listViewKeywords.LabelWrap = false;
            this.listViewKeywords.Location = new System.Drawing.Point(3, 19);
            this.listViewKeywords.MultiSelect = false;
            this.listViewKeywords.Name = "listViewKeywords";
            this.listViewKeywords.Size = new System.Drawing.Size(268, 130);
            this.listViewKeywords.SortArrow = Common.Forms.ListView.ESortArrow.None;
            this.listViewKeywords.Sorting = Common.Forms.ListView.ESortOrder.GroupSequential;
            this.listViewKeywords.TabIndex = 1;
            this.listViewKeywords.UseCompatibleStateImageBehavior = false;
            this.listViewKeywords.View = System.Windows.Forms.View.Details;
            this.listViewKeywords.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewKeywords_ItemSelectionChanged);
            this.listViewKeywords.DoubleClick += new System.EventHandler(this.listViewKeywords_DoubleClick);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 200;
            // 
            // columnHeaderDataSheets
            // 
            this.columnHeaderDataSheets.Text = "Datasheets";
            // 
            // columnHeaderInventory
            // 
            this.columnHeaderInventory.Text = "Inventory";
            // 
            // buttonEdit
            // 
            this.buttonEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonEdit.Location = new System.Drawing.Point(89, 248);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(75, 23);
            this.buttonEdit.TabIndex = 2;
            this.buttonEdit.Text = "&Edit...";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDelete.Location = new System.Drawing.Point(170, 248);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 3;
            this.buttonDelete.Text = "&Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonDatasheets
            // 
            this.buttonDatasheets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDatasheets.Location = new System.Drawing.Point(252, 248);
            this.buttonDatasheets.Name = "buttonDatasheets";
            this.buttonDatasheets.Size = new System.Drawing.Size(80, 23);
            this.buttonDatasheets.TabIndex = 4;
            this.buttonDatasheets.Text = "Data&sheets...";
            this.buttonDatasheets.UseVisualStyleBackColor = true;
            this.buttonDatasheets.Click += new System.EventHandler(this.buttonDatasheets_Click);
            // 
            // splitContainerPart
            // 
            this.splitContainerPart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerPart.Location = new System.Drawing.Point(8, 12);
            this.splitContainerPart.Name = "splitContainerPart";
            // 
            // splitContainerPart.Panel1
            // 
            this.splitContainerPart.Panel1.Controls.Add(this.labelParts);
            this.splitContainerPart.Panel1.Controls.Add(this.listViewKeywords);
            this.splitContainerPart.Panel1.Controls.Add(this.textBoxType);
            this.splitContainerPart.Panel1.Controls.Add(this.labelCategory);
            this.splitContainerPart.Panel1.Controls.Add(this.labelType);
            this.splitContainerPart.Panel1.Controls.Add(this.labelPackage);
            this.splitContainerPart.Panel1.Controls.Add(this.textBoxPackage);
            this.splitContainerPart.Panel1.Controls.Add(this.textBoxCategory);
            // 
            // splitContainerPart.Panel2
            // 
            this.splitContainerPart.Panel2.Controls.Add(this.labelPinouts);
            this.splitContainerPart.Panel2.Controls.Add(this.textBoxPinouts);
            this.splitContainerPart.Size = new System.Drawing.Size(648, 230);
            this.splitContainerPart.SplitterDistance = 274;
            this.splitContainerPart.TabIndex = 0;
            // 
            // buttonInventory
            // 
            this.buttonInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonInventory.Location = new System.Drawing.Point(338, 248);
            this.buttonInventory.Name = "buttonInventory";
            this.buttonInventory.Size = new System.Drawing.Size(75, 23);
            this.buttonInventory.TabIndex = 5;
            this.buttonInventory.Text = "I&nventory...";
            this.buttonInventory.UseVisualStyleBackColor = true;
            this.buttonInventory.Click += new System.EventHandler(this.buttonInventory_Click);
            // 
            // PartDetailsForm
            // 
            this.AcceptButton = this.buttonClose;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(664, 278);
            this.Controls.Add(this.buttonInventory);
            this.Controls.Add(this.splitContainerPart);
            this.Controls.Add(this.buttonDatasheets);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonAdd);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(672, 312);
            this.Name = "PartDetailsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Details...";
            this.Shown += new System.EventHandler(this.PartDetailsForm_Shown);
            this.splitContainerPart.Panel1.ResumeLayout(false);
            this.splitContainerPart.Panel1.PerformLayout();
            this.splitContainerPart.Panel2.ResumeLayout(false);
            this.splitContainerPart.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerPart)).EndInit();
            this.splitContainerPart.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label labelCategory;
        private Common.Forms.TextBox textBoxCategory;
        private System.Windows.Forms.Label labelType;
        private Common.Forms.TextBox textBoxType;
        private System.Windows.Forms.Label labelPackage;
        private Common.Forms.TextBox textBoxPackage;
        private Common.Forms.ListView listViewKeywords;
        private System.Windows.Forms.ColumnHeader columnHeaderDataSheets;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.Label labelPinouts;
        private System.Windows.Forms.Label labelParts;
        private Common.Forms.TextBox textBoxPinouts;
        private Arcade.Forms.Button buttonAdd;
        private Arcade.Forms.Button buttonEdit;
        private Arcade.Forms.Button buttonDelete;
        private Arcade.Forms.Button buttonDatasheets;
        private Arcade.Forms.Button buttonClose;
        private System.Windows.Forms.SplitContainer splitContainerPart;
        private System.Windows.Forms.ColumnHeader columnHeaderInventory;
        private Arcade.Forms.Button buttonInventory;
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
