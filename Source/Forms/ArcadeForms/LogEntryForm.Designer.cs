/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade.Forms
{
    partial class LogEntryForm
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
            this.buttonOK = new Arcade.Forms.Button();
            this.buttonCancel = new Arcade.Forms.Button();
            this.labelType = new System.Windows.Forms.Label();
            this.comboBoxType = new Common.Forms.ComboBox();
            this.labelDate = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxDescription = new Common.Forms.TextBox();
            this.dateTimePickerLog = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(316, 230);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(397, 230);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(12, 45);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(34, 13);
            this.labelType.TabIndex = 2;
            this.labelType.Text = "&Type:";
            // 
            // comboBoxType
            // 
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(52, 42);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(204, 21);
            this.comboBoxType.TabIndex = 3;
            this.comboBoxType.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxType_Validating);
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Location = new System.Drawing.Point(12, 14);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(33, 13);
            this.labelDate.TabIndex = 0;
            this.labelDate.Text = "&Date:";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(12, 74);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(63, 13);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "D&escription:";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDescription.HideSelection = false;
            this.textBoxDescription.Location = new System.Drawing.Point(15, 92);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxDescription.Size = new System.Drawing.Size(456, 130);
            this.textBoxDescription.TabIndex = 5;
            this.textBoxDescription.WordWrap = false;
            this.textBoxDescription.TextChanged += new System.EventHandler(this.textBoxDescription_TextChanged);
            // 
            // dateTimePickerLog
            // 
            this.dateTimePickerLog.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerLog.Location = new System.Drawing.Point(51, 14);
            this.dateTimePickerLog.Name = "dateTimePickerLog";
            this.dateTimePickerLog.Size = new System.Drawing.Size(96, 20);
            this.dateTimePickerLog.TabIndex = 1;
            // 
            // LogEntryForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(484, 262);
            this.Controls.Add(this.dateTimePickerLog);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelDate);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "LogEntryForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LogEntryForm";
            this.Shown += new System.EventHandler(this.LogEntryForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Arcade.Forms.Button buttonOK;
        private Arcade.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelType;
        private Common.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.Label labelDescription;
        private Common.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.DateTimePicker dateTimePickerLog;
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2023 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
