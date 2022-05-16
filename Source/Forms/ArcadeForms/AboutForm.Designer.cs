/***************************************************************************/
/*  Copyright (C) 2006-2022 Kevin Eshbach                                  */
/***************************************************************************/

namespace Arcade.Forms
{
    partial class AboutForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
        this.labelProductName = new System.Windows.Forms.Label();
        this.labelProductVersion = new System.Windows.Forms.Label();
        this.labelProductCopyright = new System.Windows.Forms.Label();
        this.label4 = new System.Windows.Forms.Label();
        this.buttonClose = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // labelProductName
        // 
        this.labelProductName.AutoSize = true;
        this.labelProductName.Location = new System.Drawing.Point(13, 9);
        this.labelProductName.MinimumSize = new System.Drawing.Size(338, 0);
        this.labelProductName.Name = "labelProductName";
        this.labelProductName.Size = new System.Drawing.Size(338, 13);
        this.labelProductName.TabIndex = 0;
        this.labelProductName.Text = "Product Name";
        this.labelProductName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // labelProductVersion
        // 
        this.labelProductVersion.AutoSize = true;
        this.labelProductVersion.Location = new System.Drawing.Point(13, 26);
        this.labelProductVersion.MinimumSize = new System.Drawing.Size(338, 0);
        this.labelProductVersion.Name = "labelProductVersion";
        this.labelProductVersion.Size = new System.Drawing.Size(338, 13);
        this.labelProductVersion.TabIndex = 1;
        this.labelProductVersion.Text = "Product Version";
        this.labelProductVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // labelProductCopyright
        // 
        this.labelProductCopyright.AutoSize = true;
        this.labelProductCopyright.Location = new System.Drawing.Point(13, 43);
        this.labelProductCopyright.MinimumSize = new System.Drawing.Size(338, 0);
        this.labelProductCopyright.Name = "labelProductCopyright";
        this.labelProductCopyright.Size = new System.Drawing.Size(338, 13);
        this.labelProductCopyright.TabIndex = 2;
        this.labelProductCopyright.Text = "Product Copyright";
        this.labelProductCopyright.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // label4
        // 
        this.label4.AutoSize = true;
        this.label4.Location = new System.Drawing.Point(13, 60);
        this.label4.MinimumSize = new System.Drawing.Size(338, 0);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(338, 13);
        this.label4.TabIndex = 3;
        this.label4.Text = "All Rights Reserved";
        this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // buttonClose
        // 
        this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.buttonClose.Location = new System.Drawing.Point(279, 94);
        this.buttonClose.Name = "buttonClose";
        this.buttonClose.Size = new System.Drawing.Size(75, 23);
        this.buttonClose.TabIndex = 8;
        this.buttonClose.Text = "Close";
        this.buttonClose.UseVisualStyleBackColor = true;
        // 
        // AboutForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.buttonClose;
        this.ClientSize = new System.Drawing.Size(366, 127);
        this.Controls.Add(this.buttonClose);
        this.Controls.Add(this.label4);
        this.Controls.Add(this.labelProductCopyright);
        this.Controls.Add(this.labelProductVersion);
        this.Controls.Add(this.labelProductName);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "AboutForm";
        this.Padding = new System.Windows.Forms.Padding(9);
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "AboutForm";
        this.ResumeLayout(false);
        this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.Label labelProductVersion;
        private System.Windows.Forms.Label labelProductCopyright;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonClose;
    }
}

/***************************************************************************/
/*  Copyright (C) 2006-2022 Kevin Eshbach                                  */
/***************************************************************************/

