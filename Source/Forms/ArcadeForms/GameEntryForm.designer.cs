/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-20022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

namespace Arcade.Forms
{
    partial class GameEntryForm
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
            this.textBoxName = new Common.Forms.TextBox();
            this.labelManufacturer = new System.Windows.Forms.Label();
            this.comboBoxManufacturer = new System.Windows.Forms.ComboBox();
            this.labelPinouts = new System.Windows.Forms.Label();
            this.textBoxPinouts = new Common.Forms.TextBox();
            this.textBoxDipSwitches = new Common.Forms.TextBox();
            this.labelDipSwitches = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxDescription = new Common.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonBoards = new System.Windows.Forms.Button();
            this.buttonManuals = new System.Windows.Forms.Button();
            this.comboBoxWiringHarness = new System.Windows.Forms.ComboBox();
            this.labelWiringHarness = new System.Windows.Forms.Label();
            this.buttonVideo = new System.Windows.Forms.Button();
            this.buttonAudio = new System.Windows.Forms.Button();
            this.buttonControls = new System.Windows.Forms.Button();
            this.buttonDisplays = new System.Windows.Forms.Button();
            this.labelCocktail = new System.Windows.Forms.Label();
            this.comboBoxCocktail = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxNeedPowerOnReset = new System.Windows.Forms.CheckBox();
            this.checkBoxHaveWiringHarness = new System.Windows.Forms.CheckBox();
            this.checkBoxNotJAMMA = new System.Windows.Forms.CheckBox();
            this.buttonLogs = new System.Windows.Forms.Button();
            this.splitContainerTop = new System.Windows.Forms.SplitContainer();
            this.splitContainerBottom = new System.Windows.Forms.SplitContainer();
            this.splitContainerTopBottom = new System.Windows.Forms.SplitContainer();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).BeginInit();
            this.splitContainerTop.Panel1.SuspendLayout();
            this.splitContainerTop.Panel2.SuspendLayout();
            this.splitContainerTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBottom)).BeginInit();
            this.splitContainerBottom.Panel1.SuspendLayout();
            this.splitContainerBottom.Panel2.SuspendLayout();
            this.splitContainerBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTopBottom)).BeginInit();
            this.splitContainerTopBottom.Panel1.SuspendLayout();
            this.splitContainerTopBottom.Panel2.SuspendLayout();
            this.splitContainerTopBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(3, 6);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "&Name:";
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(105, 3);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(277, 20);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // labelManufacturer
            // 
            this.labelManufacturer.AutoSize = true;
            this.labelManufacturer.Location = new System.Drawing.Point(4, 32);
            this.labelManufacturer.Name = "labelManufacturer";
            this.labelManufacturer.Size = new System.Drawing.Size(73, 13);
            this.labelManufacturer.TabIndex = 2;
            this.labelManufacturer.Text = "&Manufacturer:";
            // 
            // comboBoxManufacturer
            // 
            this.comboBoxManufacturer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxManufacturer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxManufacturer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxManufacturer.FormattingEnabled = true;
            this.comboBoxManufacturer.Location = new System.Drawing.Point(105, 29);
            this.comboBoxManufacturer.Name = "comboBoxManufacturer";
            this.comboBoxManufacturer.Size = new System.Drawing.Size(277, 21);
            this.comboBoxManufacturer.TabIndex = 3;
            this.comboBoxManufacturer.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxManufacturer_Validating);
            // 
            // labelPinouts
            // 
            this.labelPinouts.AutoSize = true;
            this.labelPinouts.Location = new System.Drawing.Point(0, 0);
            this.labelPinouts.Name = "labelPinouts";
            this.labelPinouts.Size = new System.Drawing.Size(45, 13);
            this.labelPinouts.TabIndex = 0;
            this.labelPinouts.Text = "&Pinouts:";
            // 
            // textBoxPinouts
            // 
            this.textBoxPinouts.AcceptsReturn = true;
            this.textBoxPinouts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPinouts.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPinouts.HideSelection = false;
            this.textBoxPinouts.Location = new System.Drawing.Point(3, 16);
            this.textBoxPinouts.MaxLength = 2000000;
            this.textBoxPinouts.Multiline = true;
            this.textBoxPinouts.Name = "textBoxPinouts";
            this.textBoxPinouts.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxPinouts.Size = new System.Drawing.Size(379, 156);
            this.textBoxPinouts.TabIndex = 1;
            this.textBoxPinouts.WordWrap = false;
            // 
            // textBoxDipSwitches
            // 
            this.textBoxDipSwitches.AcceptsReturn = true;
            this.textBoxDipSwitches.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDipSwitches.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDipSwitches.HideSelection = false;
            this.textBoxDipSwitches.Location = new System.Drawing.Point(3, 16);
            this.textBoxDipSwitches.MaxLength = 2000000;
            this.textBoxDipSwitches.Multiline = true;
            this.textBoxDipSwitches.Name = "textBoxDipSwitches";
            this.textBoxDipSwitches.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxDipSwitches.Size = new System.Drawing.Size(379, 156);
            this.textBoxDipSwitches.TabIndex = 1;
            this.textBoxDipSwitches.WordWrap = false;
            // 
            // labelDipSwitches
            // 
            this.labelDipSwitches.AutoSize = true;
            this.labelDipSwitches.Location = new System.Drawing.Point(0, 0);
            this.labelDipSwitches.Name = "labelDipSwitches";
            this.labelDipSwitches.Size = new System.Drawing.Size(72, 13);
            this.labelDipSwitches.TabIndex = 0;
            this.labelDipSwitches.Text = "Dip &Switches:";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(63, 13);
            this.labelDescription.TabIndex = 0;
            this.labelDescription.Text = "&Description:";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.AcceptsReturn = true;
            this.textBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDescription.HideSelection = false;
            this.textBoxDescription.Location = new System.Drawing.Point(3, 15);
            this.textBoxDescription.MaxLength = 2000000;
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxDescription.Size = new System.Drawing.Size(380, 173);
            this.textBoxDescription.TabIndex = 1;
            this.textBoxDescription.WordWrap = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(709, 388);
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
            this.buttonOK.Location = new System.Drawing.Point(628, 388);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonBoards
            // 
            this.buttonBoards.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonBoards.Location = new System.Drawing.Point(8, 388);
            this.buttonBoards.Name = "buttonBoards";
            this.buttonBoards.Size = new System.Drawing.Size(75, 23);
            this.buttonBoards.TabIndex = 1;
            this.buttonBoards.Text = "&Boards...";
            this.buttonBoards.UseVisualStyleBackColor = true;
            this.buttonBoards.Click += new System.EventHandler(this.buttonBoards_Click);
            // 
            // buttonManuals
            // 
            this.buttonManuals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonManuals.Location = new System.Drawing.Point(89, 388);
            this.buttonManuals.Name = "buttonManuals";
            this.buttonManuals.Size = new System.Drawing.Size(75, 23);
            this.buttonManuals.TabIndex = 2;
            this.buttonManuals.Text = "Man&uals...";
            this.buttonManuals.UseVisualStyleBackColor = true;
            this.buttonManuals.Click += new System.EventHandler(this.buttonManuals_Click);
            // 
            // comboBoxWiringHarness
            // 
            this.comboBoxWiringHarness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxWiringHarness.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxWiringHarness.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxWiringHarness.FormattingEnabled = true;
            this.comboBoxWiringHarness.Location = new System.Drawing.Point(99, 19);
            this.comboBoxWiringHarness.Name = "comboBoxWiringHarness";
            this.comboBoxWiringHarness.Size = new System.Drawing.Size(271, 21);
            this.comboBoxWiringHarness.TabIndex = 2;
            this.comboBoxWiringHarness.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxWiringHarness_Validating);
            // 
            // labelWiringHarness
            // 
            this.labelWiringHarness.AutoSize = true;
            this.labelWiringHarness.Location = new System.Drawing.Point(11, 22);
            this.labelWiringHarness.Name = "labelWiringHarness";
            this.labelWiringHarness.Size = new System.Drawing.Size(82, 13);
            this.labelWiringHarness.TabIndex = 1;
            this.labelWiringHarness.Text = "&Wiring Harness:";
            // 
            // buttonVideo
            // 
            this.buttonVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonVideo.Location = new System.Drawing.Point(251, 388);
            this.buttonVideo.Name = "buttonVideo";
            this.buttonVideo.Size = new System.Drawing.Size(75, 23);
            this.buttonVideo.TabIndex = 4;
            this.buttonVideo.Text = "&Video...";
            this.buttonVideo.UseVisualStyleBackColor = true;
            this.buttonVideo.Click += new System.EventHandler(this.buttonVideo_Click);
            // 
            // buttonAudio
            // 
            this.buttonAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAudio.Location = new System.Drawing.Point(332, 388);
            this.buttonAudio.Name = "buttonAudio";
            this.buttonAudio.Size = new System.Drawing.Size(75, 23);
            this.buttonAudio.TabIndex = 5;
            this.buttonAudio.Text = "&Audio...";
            this.buttonAudio.UseVisualStyleBackColor = true;
            this.buttonAudio.Click += new System.EventHandler(this.buttonAudio_Click);
            // 
            // buttonControls
            // 
            this.buttonControls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonControls.Location = new System.Drawing.Point(170, 388);
            this.buttonControls.Name = "buttonControls";
            this.buttonControls.Size = new System.Drawing.Size(75, 23);
            this.buttonControls.TabIndex = 3;
            this.buttonControls.Text = "&Controls...";
            this.buttonControls.UseVisualStyleBackColor = true;
            this.buttonControls.Click += new System.EventHandler(this.buttonControls_Click);
            // 
            // buttonDisplays
            // 
            this.buttonDisplays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDisplays.Location = new System.Drawing.Point(413, 388);
            this.buttonDisplays.Name = "buttonDisplays";
            this.buttonDisplays.Size = new System.Drawing.Size(75, 23);
            this.buttonDisplays.TabIndex = 6;
            this.buttonDisplays.Text = "&Displays...";
            this.buttonDisplays.UseVisualStyleBackColor = true;
            this.buttonDisplays.Click += new System.EventHandler(this.buttonDisplays_Click);
            // 
            // labelCocktail
            // 
            this.labelCocktail.AutoSize = true;
            this.labelCocktail.Location = new System.Drawing.Point(4, 59);
            this.labelCocktail.Name = "labelCocktail";
            this.labelCocktail.Size = new System.Drawing.Size(48, 13);
            this.labelCocktail.TabIndex = 4;
            this.labelCocktail.Text = "C&ocktail:";
            // 
            // comboBoxCocktail
            // 
            this.comboBoxCocktail.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxCocktail.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxCocktail.FormattingEnabled = true;
            this.comboBoxCocktail.Location = new System.Drawing.Point(105, 56);
            this.comboBoxCocktail.Name = "comboBoxCocktail";
            this.comboBoxCocktail.Size = new System.Drawing.Size(200, 21);
            this.comboBoxCocktail.TabIndex = 5;
            this.comboBoxCocktail.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxCocktail_Validating);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxNeedPowerOnReset);
            this.groupBox1.Controls.Add(this.checkBoxHaveWiringHarness);
            this.groupBox1.Controls.Add(this.checkBoxNotJAMMA);
            this.groupBox1.Controls.Add(this.comboBoxWiringHarness);
            this.groupBox1.Controls.Add(this.labelWiringHarness);
            this.groupBox1.Location = new System.Drawing.Point(4, 90);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(376, 98);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // checkBoxNeedPowerOnReset
            // 
            this.checkBoxNeedPowerOnReset.AutoSize = true;
            this.checkBoxNeedPowerOnReset.Location = new System.Drawing.Point(14, 76);
            this.checkBoxNeedPowerOnReset.Name = "checkBoxNeedPowerOnReset";
            this.checkBoxNeedPowerOnReset.Size = new System.Drawing.Size(131, 17);
            this.checkBoxNeedPowerOnReset.TabIndex = 4;
            this.checkBoxNeedPowerOnReset.Text = "Need Power on &Reset";
            this.checkBoxNeedPowerOnReset.UseVisualStyleBackColor = true;
            // 
            // checkBoxHaveWiringHarness
            // 
            this.checkBoxHaveWiringHarness.AutoSize = true;
            this.checkBoxHaveWiringHarness.Location = new System.Drawing.Point(14, 53);
            this.checkBoxHaveWiringHarness.Name = "checkBoxHaveWiringHarness";
            this.checkBoxHaveWiringHarness.Size = new System.Drawing.Size(127, 17);
            this.checkBoxHaveWiringHarness.TabIndex = 3;
            this.checkBoxHaveWiringHarness.Text = "&Have Wiring Harness";
            this.checkBoxHaveWiringHarness.UseVisualStyleBackColor = true;
            // 
            // checkBoxNotJAMMA
            // 
            this.checkBoxNotJAMMA.AutoSize = true;
            this.checkBoxNotJAMMA.Location = new System.Drawing.Point(8, -2);
            this.checkBoxNotJAMMA.Name = "checkBoxNotJAMMA";
            this.checkBoxNotJAMMA.Size = new System.Drawing.Size(83, 17);
            this.checkBoxNotJAMMA.TabIndex = 0;
            this.checkBoxNotJAMMA.Text = "Not &JAMMA";
            this.checkBoxNotJAMMA.UseVisualStyleBackColor = true;
            this.checkBoxNotJAMMA.CheckedChanged += new System.EventHandler(this.checkBoxNotJAMMA_CheckedChanged);
            // 
            // buttonLogs
            // 
            this.buttonLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLogs.Location = new System.Drawing.Point(494, 388);
            this.buttonLogs.Name = "buttonLogs";
            this.buttonLogs.Size = new System.Drawing.Size(75, 23);
            this.buttonLogs.TabIndex = 7;
            this.buttonLogs.Text = "&Logs...";
            this.buttonLogs.UseVisualStyleBackColor = true;
            this.buttonLogs.Click += new System.EventHandler(this.buttonLogs_Click);
            // 
            // splitContainerTop
            // 
            this.splitContainerTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerTop.Location = new System.Drawing.Point(8, 6);
            this.splitContainerTop.Name = "splitContainerTop";
            // 
            // splitContainerTop.Panel1
            // 
            this.splitContainerTop.Panel1.Controls.Add(this.labelName);
            this.splitContainerTop.Panel1.Controls.Add(this.textBoxName);
            this.splitContainerTop.Panel1.Controls.Add(this.groupBox1);
            this.splitContainerTop.Panel1.Controls.Add(this.labelManufacturer);
            this.splitContainerTop.Panel1.Controls.Add(this.comboBoxCocktail);
            this.splitContainerTop.Panel1.Controls.Add(this.comboBoxManufacturer);
            this.splitContainerTop.Panel1.Controls.Add(this.labelCocktail);
            // 
            // splitContainerTop.Panel2
            // 
            this.splitContainerTop.Panel2.Controls.Add(this.labelDescription);
            this.splitContainerTop.Panel2.Controls.Add(this.textBoxDescription);
            this.splitContainerTop.Size = new System.Drawing.Size(776, 191);
            this.splitContainerTop.SplitterDistance = 386;
            this.splitContainerTop.TabIndex = 0;
            // 
            // splitContainerBottom
            // 
            this.splitContainerBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerBottom.Location = new System.Drawing.Point(8, 0);
            this.splitContainerBottom.Name = "splitContainerBottom";
            // 
            // splitContainerBottom.Panel1
            // 
            this.splitContainerBottom.Panel1.Controls.Add(this.labelPinouts);
            this.splitContainerBottom.Panel1.Controls.Add(this.textBoxPinouts);
            // 
            // splitContainerBottom.Panel2
            // 
            this.splitContainerBottom.Panel2.Controls.Add(this.labelDipSwitches);
            this.splitContainerBottom.Panel2.Controls.Add(this.textBoxDipSwitches);
            this.splitContainerBottom.Size = new System.Drawing.Size(776, 175);
            this.splitContainerBottom.SplitterDistance = 386;
            this.splitContainerBottom.TabIndex = 0;
            // 
            // splitContainerTopBottom
            // 
            this.splitContainerTopBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerTopBottom.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTopBottom.Name = "splitContainerTopBottom";
            this.splitContainerTopBottom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTopBottom.Panel1
            // 
            this.splitContainerTopBottom.Panel1.Controls.Add(this.splitContainerTop);
            // 
            // splitContainerTopBottom.Panel2
            // 
            this.splitContainerTopBottom.Panel2.Controls.Add(this.splitContainerBottom);
            this.splitContainerTopBottom.Size = new System.Drawing.Size(790, 381);
            this.splitContainerTopBottom.SplitterDistance = 199;
            this.splitContainerTopBottom.TabIndex = 0;
            // 
            // GameEntryForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(792, 419);
            this.Controls.Add(this.splitContainerTopBottom);
            this.Controls.Add(this.buttonLogs);
            this.Controls.Add(this.buttonDisplays);
            this.Controls.Add(this.buttonControls);
            this.Controls.Add(this.buttonAudio);
            this.Controls.Add(this.buttonVideo);
            this.Controls.Add(this.buttonManuals);
            this.Controls.Add(this.buttonBoards);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameEntryForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GameEntryForm";
            this.Load += new System.EventHandler(this.GameEntryForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainerTop.Panel1.ResumeLayout(false);
            this.splitContainerTop.Panel1.PerformLayout();
            this.splitContainerTop.Panel2.ResumeLayout(false);
            this.splitContainerTop.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).EndInit();
            this.splitContainerTop.ResumeLayout(false);
            this.splitContainerBottom.Panel1.ResumeLayout(false);
            this.splitContainerBottom.Panel1.PerformLayout();
            this.splitContainerBottom.Panel2.ResumeLayout(false);
            this.splitContainerBottom.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerBottom)).EndInit();
            this.splitContainerBottom.ResumeLayout(false);
            this.splitContainerTopBottom.Panel1.ResumeLayout(false);
            this.splitContainerTopBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTopBottom)).EndInit();
            this.splitContainerTopBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private Common.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelManufacturer;
        private System.Windows.Forms.ComboBox comboBoxManufacturer;
        private System.Windows.Forms.Label labelPinouts;
        private Common.Forms.TextBox textBoxPinouts;
        private Common.Forms.TextBox textBoxDipSwitches;
        private System.Windows.Forms.Label labelDipSwitches;
        private System.Windows.Forms.Label labelDescription;
        private Common.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonBoards;
        private System.Windows.Forms.Button buttonManuals;
        private System.Windows.Forms.ComboBox comboBoxWiringHarness;
        private System.Windows.Forms.Label labelWiringHarness;
        private System.Windows.Forms.Button buttonVideo;
        private System.Windows.Forms.Button buttonAudio;
        private System.Windows.Forms.Button buttonControls;
        private System.Windows.Forms.Button buttonDisplays;
        private System.Windows.Forms.Label labelCocktail;
        private System.Windows.Forms.ComboBox comboBoxCocktail;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxNotJAMMA;
        private System.Windows.Forms.CheckBox checkBoxHaveWiringHarness;
        private System.Windows.Forms.CheckBox checkBoxNeedPowerOnReset;
        private System.Windows.Forms.Button buttonLogs;
        private System.Windows.Forms.SplitContainer splitContainerTop;
        private System.Windows.Forms.SplitContainer splitContainerBottom;
        private System.Windows.Forms.SplitContainer splitContainerTopBottom;
    }
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2006-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
