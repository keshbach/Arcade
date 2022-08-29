/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade.Forms
{
    partial class GameBoardEntryForm
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
            this.labelBoardType = new System.Windows.Forms.Label();
            this.comboBoxBoardType = new System.Windows.Forms.ComboBox();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new Common.Forms.TextBox();
            this.labelSize = new System.Windows.Forms.Label();
            this.textBoxSize = new Common.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDescription = new Common.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelBoardType
            // 
            this.labelBoardType.AutoSize = true;
            this.labelBoardType.Location = new System.Drawing.Point(4, 12);
            this.labelBoardType.Name = "labelBoardType";
            this.labelBoardType.Size = new System.Drawing.Size(65, 13);
            this.labelBoardType.TabIndex = 0;
            this.labelBoardType.Text = "&Board Type:";
            // 
            // comboBoxBoardType
            // 
            this.comboBoxBoardType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxBoardType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxBoardType.FormattingEnabled = true;
            this.comboBoxBoardType.Location = new System.Drawing.Point(72, 8);
            this.comboBoxBoardType.Name = "comboBoxBoardType";
            this.comboBoxBoardType.Size = new System.Drawing.Size(231, 21);
            this.comboBoxBoardType.TabIndex = 1;
            this.comboBoxBoardType.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxBoardType_Validating);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(4, 39);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "&Name:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(72, 35);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(231, 20);
            this.textBoxName.TabIndex = 3;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Location = new System.Drawing.Point(4, 65);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(30, 13);
            this.labelSize.TabIndex = 4;
            this.labelSize.Text = "&Size:";
            // 
            // textBoxSize
            // 
            this.textBoxSize.Location = new System.Drawing.Point(72, 61);
            this.textBoxSize.Name = "textBoxSize";
            this.textBoxSize.Size = new System.Drawing.Size(193, 20);
            this.textBoxSize.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "&Description:";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.AcceptsReturn = true;
            this.textBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDescription.Location = new System.Drawing.Point(7, 109);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxDescription.Size = new System.Drawing.Size(340, 103);
            this.textBoxDescription.TabIndex = 7;
            this.textBoxDescription.WordWrap = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(274, 219);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(193, 219);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // GameBoardEntryForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(356, 248);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxSize);
            this.Controls.Add(this.labelSize);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.comboBoxBoardType);
            this.Controls.Add(this.labelBoardType);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameBoardEntryForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GameBoardEntryForm";
            this.Shown += new System.EventHandler(this.GameBoardEntryForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelBoardType;
        private System.Windows.Forms.ComboBox comboBoxBoardType;
        private System.Windows.Forms.Label labelName;
        private Common.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelSize;
        private Common.Forms.TextBox textBoxSize;
        private System.Windows.Forms.Label label3;
        private Common.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
