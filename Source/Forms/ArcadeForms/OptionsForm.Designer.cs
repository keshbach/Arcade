
namespace Arcade.Forms
{
    partial class OptionsForm
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
            this.listViewSettings = new Common.Forms.ListView();
            this.columnHeaderValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelDatabaseSettings = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewSettings
            // 
            this.listViewSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewSettings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderValue,
            this.columnHeaderName});
            this.listViewSettings.ComboBoxItems = null;
            this.listViewSettings.FullRowSelect = true;
            this.listViewSettings.HideSelection = false;
            this.listViewSettings.LabelEdit = true;
            this.listViewSettings.LabelEditor = Common.Forms.ListView.ELabelEditor.Label;
            this.listViewSettings.LabelWrap = false;
            this.listViewSettings.Location = new System.Drawing.Point(9, 25);
            this.listViewSettings.MultiSelect = false;
            this.listViewSettings.Name = "listViewSettings";
            this.listViewSettings.ShowGroups = false;
            this.listViewSettings.Size = new System.Drawing.Size(314, 158);
            this.listViewSettings.SortArrow = Common.Forms.ListView.ESortArrow.None;
            this.listViewSettings.Sorting = Common.Forms.ListView.ESortOrder.None;
            this.listViewSettings.TabIndex = 1;
            this.listViewSettings.UseCompatibleStateImageBehavior = false;
            this.listViewSettings.View = System.Windows.Forms.View.Details;
            this.listViewSettings.KeyPressLabelEdit += new Common.Forms.ListView.KeyPressLabelEditHandler(this.listViewSettings_KeyPressLabelEdit);
            this.listViewSettings.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewSettings_AfterLabelEdit);
            this.listViewSettings.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewSettings_BeforeLabelEdit);
            // 
            // columnHeaderValue
            // 
            this.columnHeaderValue.DisplayIndex = 1;
            this.columnHeaderValue.Text = "Value";
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.DisplayIndex = 0;
            this.columnHeaderName.Text = "Name";
            // 
            // labelDatabaseSettings
            // 
            this.labelDatabaseSettings.AutoSize = true;
            this.labelDatabaseSettings.Location = new System.Drawing.Point(9, 9);
            this.labelDatabaseSettings.Name = "labelDatabaseSettings";
            this.labelDatabaseSettings.Size = new System.Drawing.Size(97, 13);
            this.labelDatabaseSettings.TabIndex = 0;
            this.labelDatabaseSettings.Text = "&Database Settings:";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(166, 190);
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
            this.buttonCancel.Location = new System.Drawing.Point(247, 190);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(334, 221);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelDatabaseSettings);
            this.Controls.Add(this.listViewSettings);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Common.Forms.ListView listViewSettings;
        private System.Windows.Forms.Label labelDatabaseSettings;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderValue;
    }
}
