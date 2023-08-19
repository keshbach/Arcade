/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade.Forms
{
    partial class DisplayEntryForm
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
            this.labelType = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.labelResolution = new System.Windows.Forms.Label();
            this.comboBoxResolution = new System.Windows.Forms.ComboBox();
            this.labelColors = new System.Windows.Forms.Label();
            this.comboBoxColors = new System.Windows.Forms.ComboBox();
            this.labelOrientation = new System.Windows.Forms.Label();
            this.comboBoxOrientation = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new Arcade.Forms.Button();
            this.buttonOK = new Arcade.Forms.Button();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new Common.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(4, 36);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(34, 13);
            this.labelType.TabIndex = 2;
            this.labelType.Text = "&Type:";
            // 
            // comboBoxType
            // 
            this.comboBoxType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(71, 33);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(121, 21);
            this.comboBoxType.TabIndex = 3;
            this.comboBoxType.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxType_Validating);
            // 
            // labelResolution
            // 
            this.labelResolution.AutoSize = true;
            this.labelResolution.Location = new System.Drawing.Point(4, 63);
            this.labelResolution.Name = "labelResolution";
            this.labelResolution.Size = new System.Drawing.Size(60, 13);
            this.labelResolution.TabIndex = 4;
            this.labelResolution.Text = "&Resolution:";
            // 
            // comboBoxResolution
            // 
            this.comboBoxResolution.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxResolution.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxResolution.FormattingEnabled = true;
            this.comboBoxResolution.Location = new System.Drawing.Point(71, 60);
            this.comboBoxResolution.Name = "comboBoxResolution";
            this.comboBoxResolution.Size = new System.Drawing.Size(121, 21);
            this.comboBoxResolution.TabIndex = 5;
            this.comboBoxResolution.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxResolution_Validating);
            // 
            // labelColors
            // 
            this.labelColors.AutoSize = true;
            this.labelColors.Location = new System.Drawing.Point(4, 90);
            this.labelColors.Name = "labelColors";
            this.labelColors.Size = new System.Drawing.Size(39, 13);
            this.labelColors.TabIndex = 6;
            this.labelColors.Text = "&Colors:";
            // 
            // comboBoxColors
            // 
            this.comboBoxColors.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxColors.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxColors.FormattingEnabled = true;
            this.comboBoxColors.Location = new System.Drawing.Point(71, 87);
            this.comboBoxColors.Name = "comboBoxColors";
            this.comboBoxColors.Size = new System.Drawing.Size(121, 21);
            this.comboBoxColors.TabIndex = 7;
            this.comboBoxColors.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxColors_Validating);
            // 
            // labelOrientation
            // 
            this.labelOrientation.AutoSize = true;
            this.labelOrientation.Location = new System.Drawing.Point(4, 118);
            this.labelOrientation.Name = "labelOrientation";
            this.labelOrientation.Size = new System.Drawing.Size(61, 13);
            this.labelOrientation.TabIndex = 8;
            this.labelOrientation.Text = "&Orientation:";
            // 
            // comboBoxOrientation
            // 
            this.comboBoxOrientation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxOrientation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxOrientation.FormattingEnabled = true;
            this.comboBoxOrientation.Location = new System.Drawing.Point(71, 115);
            this.comboBoxOrientation.Name = "comboBoxOrientation";
            this.comboBoxOrientation.Size = new System.Drawing.Size(121, 21);
            this.comboBoxOrientation.TabIndex = 9;
            this.comboBoxOrientation.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxOrientation_Validating);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(207, 144);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 11;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(126, 144);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 10;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(4, 9);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "&Name:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(71, 6);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(212, 20);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // DisplayEntryForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(290, 174);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.comboBoxOrientation);
            this.Controls.Add(this.labelOrientation);
            this.Controls.Add(this.comboBoxColors);
            this.Controls.Add(this.labelColors);
            this.Controls.Add(this.comboBoxResolution);
            this.Controls.Add(this.labelResolution);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.labelType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DisplayEntryForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DisplayEntryForm";
            this.Shown += new System.EventHandler(this.DisplayEntryForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label labelResolution;
        private System.Windows.Forms.ComboBox comboBoxResolution;
        private System.Windows.Forms.Label labelColors;
        private System.Windows.Forms.ComboBox comboBoxColors;
        private System.Windows.Forms.Label labelOrientation;
        private System.Windows.Forms.ComboBox comboBoxOrientation;
        private Arcade.Forms.Button buttonCancel;
        private Arcade.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelName;
        private Common.Forms.TextBox textBoxName;
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2009-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
