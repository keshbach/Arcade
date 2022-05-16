/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade.Forms
{
    partial class ManualEntryForm
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
            this.labelName = new System.Windows.Forms.Label();
            this.comboBoxStorageBox = new System.Windows.Forms.ComboBox();
            this.labelStorageBox = new System.Windows.Forms.Label();
            this.labelPartNumber = new System.Windows.Forms.Label();
            this.textBoxPartNumber = new Common.Forms.TextBox();
            this.labelYearPrinted = new System.Windows.Forms.Label();
            this.comboBoxPrintEdition = new System.Windows.Forms.ComboBox();
            this.comboBoxCondition = new System.Windows.Forms.ComboBox();
            this.comboBoxManufacturer = new System.Windows.Forms.ComboBox();
            this.labelPrintEdition = new System.Windows.Forms.Label();
            this.labelCondition = new System.Windows.Forms.Label();
            this.labelManufacturer = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxDescription = new Common.Forms.TextBox();
            this.textBoxName = new Common.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkBoxComplete = new System.Windows.Forms.CheckBox();
            this.checkBoxOriginal = new System.Windows.Forms.CheckBox();
            this.maskedTextBoxYearPrinted = new Common.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(6, 17);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "&Name:";
            // 
            // comboBoxStorageBox
            // 
            this.comboBoxStorageBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxStorageBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxStorageBox.FormattingEnabled = true;
            this.comboBoxStorageBox.Location = new System.Drawing.Point(80, 42);
            this.comboBoxStorageBox.Name = "comboBoxStorageBox";
            this.comboBoxStorageBox.Size = new System.Drawing.Size(155, 21);
            this.comboBoxStorageBox.TabIndex = 3;
            this.comboBoxStorageBox.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxStorageBox_Validating);
            // 
            // labelStorageBox
            // 
            this.labelStorageBox.AutoSize = true;
            this.labelStorageBox.Location = new System.Drawing.Point(6, 45);
            this.labelStorageBox.Name = "labelStorageBox";
            this.labelStorageBox.Size = new System.Drawing.Size(68, 13);
            this.labelStorageBox.TabIndex = 2;
            this.labelStorageBox.Text = "&Storage Box:";
            // 
            // labelPartNumber
            // 
            this.labelPartNumber.AutoSize = true;
            this.labelPartNumber.Location = new System.Drawing.Point(6, 71);
            this.labelPartNumber.Name = "labelPartNumber";
            this.labelPartNumber.Size = new System.Drawing.Size(69, 13);
            this.labelPartNumber.TabIndex = 4;
            this.labelPartNumber.Text = "&Part Number:";
            // 
            // textBoxPartNumber
            // 
            this.textBoxPartNumber.HideSelection = false;
            this.textBoxPartNumber.Location = new System.Drawing.Point(80, 69);
            this.textBoxPartNumber.MaxLength = 30;
            this.textBoxPartNumber.Name = "textBoxPartNumber";
            this.textBoxPartNumber.Size = new System.Drawing.Size(100, 20);
            this.textBoxPartNumber.TabIndex = 5;
            // 
            // labelYearPrinted
            // 
            this.labelYearPrinted.AutoSize = true;
            this.labelYearPrinted.Location = new System.Drawing.Point(6, 99);
            this.labelYearPrinted.Name = "labelYearPrinted";
            this.labelYearPrinted.Size = new System.Drawing.Size(68, 13);
            this.labelYearPrinted.TabIndex = 6;
            this.labelYearPrinted.Text = "&Year Printed:";
            // 
            // comboBoxPrintEdition
            // 
            this.comboBoxPrintEdition.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxPrintEdition.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxPrintEdition.FormattingEnabled = true;
            this.comboBoxPrintEdition.Location = new System.Drawing.Point(80, 124);
            this.comboBoxPrintEdition.Name = "comboBoxPrintEdition";
            this.comboBoxPrintEdition.Size = new System.Drawing.Size(121, 21);
            this.comboBoxPrintEdition.TabIndex = 9;
            this.comboBoxPrintEdition.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxPrintEdition_Validating);
            // 
            // comboBoxCondition
            // 
            this.comboBoxCondition.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxCondition.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxCondition.FormattingEnabled = true;
            this.comboBoxCondition.Location = new System.Drawing.Point(80, 152);
            this.comboBoxCondition.Name = "comboBoxCondition";
            this.comboBoxCondition.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCondition.TabIndex = 11;
            this.comboBoxCondition.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxCondition_Validating);
            // 
            // comboBoxManufacturer
            // 
            this.comboBoxManufacturer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxManufacturer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxManufacturer.FormattingEnabled = true;
            this.comboBoxManufacturer.Location = new System.Drawing.Point(80, 180);
            this.comboBoxManufacturer.Name = "comboBoxManufacturer";
            this.comboBoxManufacturer.Size = new System.Drawing.Size(160, 21);
            this.comboBoxManufacturer.TabIndex = 13;
            this.comboBoxManufacturer.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxManufacturer_Validating);
            // 
            // labelPrintEdition
            // 
            this.labelPrintEdition.AutoSize = true;
            this.labelPrintEdition.Location = new System.Drawing.Point(6, 127);
            this.labelPrintEdition.Name = "labelPrintEdition";
            this.labelPrintEdition.Size = new System.Drawing.Size(66, 13);
            this.labelPrintEdition.TabIndex = 8;
            this.labelPrintEdition.Text = "Print &Edition:";
            // 
            // labelCondition
            // 
            this.labelCondition.AutoSize = true;
            this.labelCondition.Location = new System.Drawing.Point(6, 155);
            this.labelCondition.Name = "labelCondition";
            this.labelCondition.Size = new System.Drawing.Size(54, 13);
            this.labelCondition.TabIndex = 10;
            this.labelCondition.Text = "&Condition:";
            // 
            // labelManufacturer
            // 
            this.labelManufacturer.AutoSize = true;
            this.labelManufacturer.Location = new System.Drawing.Point(6, 183);
            this.labelManufacturer.Name = "labelManufacturer";
            this.labelManufacturer.Size = new System.Drawing.Size(73, 13);
            this.labelManufacturer.TabIndex = 12;
            this.labelManufacturer.Text = "&Manufacturer:";
            // 
            // labelDescription
            // 
            this.labelDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(6, 238);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(63, 13);
            this.labelDescription.TabIndex = 16;
            this.labelDescription.Text = "&Description:";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.AcceptsReturn = true;
            this.textBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDescription.HideSelection = false;
            this.textBoxDescription.Location = new System.Drawing.Point(9, 255);
            this.textBoxDescription.MaxLength = 2000000;
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(371, 92);
            this.textBoxDescription.TabIndex = 17;
            this.textBoxDescription.WordWrap = false;
            // 
            // textBoxName
            // 
            this.textBoxName.HideSelection = false;
            this.textBoxName.Location = new System.Drawing.Point(80, 14);
            this.textBoxName.MaxLength = 10;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(300, 20);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(306, 353);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 19;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(225, 353);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 18;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkBoxComplete
            // 
            this.checkBoxComplete.AutoSize = true;
            this.checkBoxComplete.Location = new System.Drawing.Point(9, 212);
            this.checkBoxComplete.Name = "checkBoxComplete";
            this.checkBoxComplete.Size = new System.Drawing.Size(70, 17);
            this.checkBoxComplete.TabIndex = 14;
            this.checkBoxComplete.Text = "C&omplete";
            this.checkBoxComplete.UseVisualStyleBackColor = true;
            // 
            // checkBoxOriginal
            // 
            this.checkBoxOriginal.AutoSize = true;
            this.checkBoxOriginal.Location = new System.Drawing.Point(85, 212);
            this.checkBoxOriginal.Name = "checkBoxOriginal";
            this.checkBoxOriginal.Size = new System.Drawing.Size(61, 17);
            this.checkBoxOriginal.TabIndex = 15;
            this.checkBoxOriginal.Text = "O&riginal";
            this.checkBoxOriginal.UseVisualStyleBackColor = true;
            // 
            // maskedTextBoxYearPrinted
            // 
            this.maskedTextBoxYearPrinted.HideSelection = false;
            this.maskedTextBoxYearPrinted.Location = new System.Drawing.Point(80, 96);
            this.maskedTextBoxYearPrinted.Mask = "9999";
            this.maskedTextBoxYearPrinted.Name = "maskedTextBoxYearPrinted";
            this.maskedTextBoxYearPrinted.Size = new System.Drawing.Size(34, 20);
            this.maskedTextBoxYearPrinted.TabIndex = 7;
            // 
            // ManualEntryForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(390, 384);
            this.Controls.Add(this.maskedTextBoxYearPrinted);
            this.Controls.Add(this.checkBoxOriginal);
            this.Controls.Add(this.checkBoxComplete);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelManufacturer);
            this.Controls.Add(this.labelCondition);
            this.Controls.Add(this.labelPrintEdition);
            this.Controls.Add(this.comboBoxManufacturer);
            this.Controls.Add(this.comboBoxCondition);
            this.Controls.Add(this.comboBoxPrintEdition);
            this.Controls.Add(this.labelYearPrinted);
            this.Controls.Add(this.textBoxPartNumber);
            this.Controls.Add(this.labelPartNumber);
            this.Controls.Add(this.labelStorageBox);
            this.Controls.Add(this.comboBoxStorageBox);
            this.Controls.Add(this.labelName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManualEntryForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add...";
            this.Load += new System.EventHandler(this.ManualEntryForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ComboBox comboBoxStorageBox;
        private System.Windows.Forms.Label labelStorageBox;
        private System.Windows.Forms.Label labelPartNumber;
        private Common.Forms.TextBox textBoxPartNumber;
        private System.Windows.Forms.Label labelYearPrinted;
        private System.Windows.Forms.ComboBox comboBoxPrintEdition;
        private System.Windows.Forms.ComboBox comboBoxCondition;
        private System.Windows.Forms.ComboBox comboBoxManufacturer;
        private System.Windows.Forms.Label labelPrintEdition;
        private System.Windows.Forms.Label labelCondition;
        private System.Windows.Forms.Label labelManufacturer;
        private System.Windows.Forms.Label labelDescription;
        private Common.Forms.TextBox textBoxDescription;
        private Common.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkBoxComplete;
        private System.Windows.Forms.CheckBox checkBoxOriginal;
        private Common.Forms.MaskedTextBox maskedTextBoxYearPrinted;
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
