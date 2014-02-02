namespace TxEstudioApplication.Dialogs
{
    partial class DescriptorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DescriptorDialog));
            this.apply_Button = new System.Windows.Forms.Button();
            this.close_Button = new System.Windows.Forms.Button();
            this.algorithmViewer = new TxEstudioKernel.VisualElements.AlgorithmViewer();
            this.descriptors_Check = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.imageSelectionBox = new TxEstudioApplication.Custom_Controls.ImageSelectionBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.category_Label = new System.Windows.Forms.Label();
            this.postProcCheck = new System.Windows.Forms.CheckBox();
            this.postProcGroup = new System.Windows.Forms.GroupBox();
            this.alphaEditor = new TxEstudioKernel.VisualElements.RealParameterEditor();
            this.nonlinearCheck = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.energyCheck = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gaussianCheck = new System.Windows.Forms.CheckBox();
            this.windowSize = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.postProcGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.windowSize)).BeginInit();
            this.SuspendLayout();
            // 
            // apply_Button
            // 
            this.apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply_Button.Location = new System.Drawing.Point(471, 329);
            this.apply_Button.Name = "apply_Button";
            this.apply_Button.Size = new System.Drawing.Size(75, 23);
            this.apply_Button.TabIndex = 0;
            this.apply_Button.Text = "Apply";
            this.apply_Button.UseVisualStyleBackColor = true;
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.Location = new System.Drawing.Point(552, 329);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(75, 23);
            this.close_Button.TabIndex = 1;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            // 
            // algorithmViewer
            // 
            this.algorithmViewer.AlgorithmInstance = null;
            this.algorithmViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.algorithmViewer.Location = new System.Drawing.Point(412, 13);
            this.algorithmViewer.Name = "algorithmViewer";
            this.algorithmViewer.ShowDescription = true;
            this.algorithmViewer.Size = new System.Drawing.Size(208, 301);
            this.algorithmViewer.TabIndex = 2;
            this.algorithmViewer.ViewParameterDescription = true;
            // 
            // descriptors_Check
            // 
            this.descriptors_Check.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptors_Check.FormattingEnabled = true;
            this.descriptors_Check.Location = new System.Drawing.Point(6, 18);
            this.descriptors_Check.Name = "descriptors_Check";
            this.descriptors_Check.Size = new System.Drawing.Size(168, 214);
            this.descriptors_Check.Sorted = true;
            this.descriptors_Check.TabIndex = 3;
            this.descriptors_Check.SelectedIndexChanged += new System.EventHandler(this.descriptors_Check_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.imageSelectionBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(166, 168);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            // 
            // imageSelectionBox
            // 
            this.imageSelectionBox.Location = new System.Drawing.Point(6, 12);
            this.imageSelectionBox.Name = "imageSelectionBox";
            this.imageSelectionBox.Size = new System.Drawing.Size(156, 150);
            this.imageSelectionBox.TabIndex = 13;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.descriptors_Check);
            this.groupBox2.Location = new System.Drawing.Point(200, 24);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(180, 237);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            // 
            // category_Label
            // 
            this.category_Label.AutoSize = true;
            this.category_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.category_Label.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.category_Label.Location = new System.Drawing.Point(194, 13);
            this.category_Label.Name = "category_Label";
            this.category_Label.Size = new System.Drawing.Size(0, 13);
            this.category_Label.TabIndex = 20;
            // 
            // postProcCheck
            // 
            this.postProcCheck.AutoSize = true;
            this.postProcCheck.Checked = true;
            this.postProcCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.postProcCheck.Location = new System.Drawing.Point(12, 190);
            this.postProcCheck.Name = "postProcCheck";
            this.postProcCheck.Size = new System.Drawing.Size(144, 17);
            this.postProcCheck.TabIndex = 62;
            this.postProcCheck.Text = "Features post-processing";
            this.postProcCheck.UseVisualStyleBackColor = true;
            this.postProcCheck.CheckedChanged += new System.EventHandler(this.postProcCheck_CheckedChanged);
            // 
            // postProcGroup
            // 
            this.postProcGroup.Controls.Add(this.alphaEditor);
            this.postProcGroup.Controls.Add(this.nonlinearCheck);
            this.postProcGroup.Controls.Add(this.label2);
            this.postProcGroup.Controls.Add(this.energyCheck);
            this.postProcGroup.Controls.Add(this.label1);
            this.postProcGroup.Controls.Add(this.gaussianCheck);
            this.postProcGroup.Controls.Add(this.windowSize);
            this.postProcGroup.Location = new System.Drawing.Point(12, 196);
            this.postProcGroup.Name = "postProcGroup";
            this.postProcGroup.Size = new System.Drawing.Size(166, 149);
            this.postProcGroup.TabIndex = 63;
            this.postProcGroup.TabStop = false;
            // 
            // alphaEditor
            // 
            this.alphaEditor.Location = new System.Drawing.Point(103, 45);
            this.alphaEditor.Maximum = 1F;
            this.alphaEditor.Minimum = 0F;
            this.alphaEditor.Name = "alphaEditor";
            this.alphaEditor.ParameterValue = 0F;
            this.alphaEditor.Size = new System.Drawing.Size(46, 20);
            this.alphaEditor.TabIndex = 61;
            this.alphaEditor.Text = "0.01";
            // 
            // nonlinearCheck
            // 
            this.nonlinearCheck.AutoSize = true;
            this.nonlinearCheck.Checked = true;
            this.nonlinearCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.nonlinearCheck.Location = new System.Drawing.Point(28, 28);
            this.nonlinearCheck.Name = "nonlinearCheck";
            this.nonlinearCheck.Size = new System.Drawing.Size(84, 17);
            this.nonlinearCheck.TabIndex = 55;
            this.nonlinearCheck.Text = "Non-linearity";
            this.nonlinearCheck.UseVisualStyleBackColor = true;
            this.nonlinearCheck.CheckedChanged += new System.EventHandler(this.nonlinearCheck_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 60;
            this.label2.Text = "Alpha";
            // 
            // energyCheck
            // 
            this.energyCheck.AutoSize = true;
            this.energyCheck.Checked = true;
            this.energyCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.energyCheck.Location = new System.Drawing.Point(28, 75);
            this.energyCheck.Name = "energyCheck";
            this.energyCheck.Size = new System.Drawing.Size(59, 17);
            this.energyCheck.TabIndex = 56;
            this.energyCheck.Text = "Energy";
            this.energyCheck.UseVisualStyleBackColor = true;
            this.energyCheck.CheckedChanged += new System.EventHandler(this.energyCheck_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 59;
            this.label1.Text = "Window";
            // 
            // gaussianCheck
            // 
            this.gaussianCheck.AutoSize = true;
            this.gaussianCheck.Location = new System.Drawing.Point(36, 124);
            this.gaussianCheck.Name = "gaussianCheck";
            this.gaussianCheck.Size = new System.Drawing.Size(129, 17);
            this.gaussianCheck.TabIndex = 57;
            this.gaussianCheck.Text = "Use gaussian weights";
            this.gaussianCheck.UseVisualStyleBackColor = true;
            // 
            // windowSize
            // 
            this.windowSize.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.windowSize.Location = new System.Drawing.Point(103, 98);
            this.windowSize.Maximum = new decimal(new int[] {
            101,
            0,
            0,
            0});
            this.windowSize.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.windowSize.Name = "windowSize";
            this.windowSize.Size = new System.Drawing.Size(46, 20);
            this.windowSize.TabIndex = 58;
            this.windowSize.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // DescriptorDialog
            // 
            this.AcceptButton = this.apply_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.close_Button;
            this.ClientSize = new System.Drawing.Size(639, 355);
            this.Controls.Add(this.postProcCheck);
            this.Controls.Add(this.postProcGroup);
            this.Controls.Add(this.category_Label);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.algorithmViewer);
            this.Controls.Add(this.close_Button);
            this.Controls.Add(this.apply_Button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DescriptorDialog";
            this.ShowInTaskbar = false;
            this.Text = "Feature descriptors";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.postProcGroup.ResumeLayout(false);
            this.postProcGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.windowSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button apply_Button;
        private System.Windows.Forms.Button close_Button;
        private TxEstudioKernel.VisualElements.AlgorithmViewer algorithmViewer;
        private System.Windows.Forms.CheckedListBox descriptors_Check;
        private System.Windows.Forms.GroupBox groupBox1;
        private TxEstudioApplication.Custom_Controls.ImageSelectionBox imageSelectionBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label category_Label;
        private System.Windows.Forms.CheckBox postProcCheck;
        private System.Windows.Forms.GroupBox postProcGroup;
        private TxEstudioKernel.VisualElements.RealParameterEditor alphaEditor;
        private System.Windows.Forms.CheckBox nonlinearCheck;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox energyCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox gaussianCheck;
        private System.Windows.Forms.NumericUpDown windowSize;
    }
}
