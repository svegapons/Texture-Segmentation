namespace TxEstudioApplication.Dialogs
{
    partial class TonesTexturesBase
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
            this.progress_Label = new System.Windows.Forms.Label();
            this.textures_Check = new System.Windows.Forms.CheckBox();
            this.tones_Check = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.textures_Box = new System.Windows.Forms.GroupBox();
            this.descriptor_Viewer = new TxEstudioKernel.VisualElements.AlgorithmViewer();
            this.descriptors_Tree = new System.Windows.Forms.TreeView();
            this.tones_Group = new System.Windows.Forms.GroupBox();
            this.tones_SelectionBox = new TxEstudioApplication.Custom_Controls.ImageMultiSelectionBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.imageSelectionBox = new TxEstudioApplication.Custom_Controls.ImageSelectionBox();
            this.close_Button = new System.Windows.Forms.Button();
            this.apply_Button = new System.Windows.Forms.Button();
            this.postProcCheck = new System.Windows.Forms.CheckBox();
            this.nonlinearCheck = new System.Windows.Forms.CheckBox();
            this.energyCheck = new System.Windows.Forms.CheckBox();
            this.gaussianCheck = new System.Windows.Forms.CheckBox();
            this.windowSize = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.postProcGroup = new System.Windows.Forms.GroupBox();
            this.alphaEditor = new TxEstudioKernel.VisualElements.RealParameterEditor();
            this.textures_Box.SuspendLayout();
            this.tones_Group.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.windowSize)).BeginInit();
            this.postProcGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // progress_Label
            // 
            this.progress_Label.AutoSize = true;
            this.progress_Label.Location = new System.Drawing.Point(14, 411);
            this.progress_Label.Name = "progress_Label";
            this.progress_Label.Size = new System.Drawing.Size(0, 13);
            this.progress_Label.TabIndex = 53;
            // 
            // textures_Check
            // 
            this.textures_Check.AutoSize = true;
            this.textures_Check.Checked = true;
            this.textures_Check.CheckState = System.Windows.Forms.CheckState.Checked;
            this.textures_Check.Location = new System.Drawing.Point(202, 199);
            this.textures_Check.Name = "textures_Check";
            this.textures_Check.Size = new System.Drawing.Size(67, 17);
            this.textures_Check.TabIndex = 49;
            this.textures_Check.Text = "Textures";
            this.textures_Check.UseVisualStyleBackColor = true;
            this.textures_Check.CheckedChanged += new System.EventHandler(this.textures_Check_CheckedChanged);
            // 
            // tones_Check
            // 
            this.tones_Check.AutoSize = true;
            this.tones_Check.Checked = true;
            this.tones_Check.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tones_Check.Location = new System.Drawing.Point(201, 4);
            this.tones_Check.Name = "tones_Check";
            this.tones_Check.Size = new System.Drawing.Size(56, 17);
            this.tones_Check.TabIndex = 48;
            this.tones_Check.Text = "Tones";
            this.tones_Check.UseVisualStyleBackColor = true;
            this.tones_Check.CheckedChanged += new System.EventHandler(this.tones_Check_CheckedChanged);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar.Location = new System.Drawing.Point(11, 427);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(257, 10);
            this.progressBar.TabIndex = 52;
            // 
            // textures_Box
            // 
            this.textures_Box.AutoSize = true;
            this.textures_Box.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.textures_Box.Controls.Add(this.descriptor_Viewer);
            this.textures_Box.Controls.Add(this.descriptors_Tree);
            this.textures_Box.Location = new System.Drawing.Point(195, 202);
            this.textures_Box.Margin = new System.Windows.Forms.Padding(3, 3, 3, 35);
            this.textures_Box.Name = "textures_Box";
            this.textures_Box.Size = new System.Drawing.Size(294, 196);
            this.textures_Box.TabIndex = 51;
            this.textures_Box.TabStop = false;
            this.textures_Box.Text = "groupBox3";
            // 
            // descriptor_Viewer
            // 
            this.descriptor_Viewer.AlgorithmInstance = null;
            this.descriptor_Viewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.descriptor_Viewer.Location = new System.Drawing.Point(151, 19);
            this.descriptor_Viewer.Name = "descriptor_Viewer";
            this.descriptor_Viewer.ShowDescription = false;
            this.descriptor_Viewer.Size = new System.Drawing.Size(137, 158);
            this.descriptor_Viewer.TabIndex = 28;
            this.descriptor_Viewer.ViewParameterDescription = true;
            // 
            // descriptors_Tree
            // 
            this.descriptors_Tree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.descriptors_Tree.CheckBoxes = true;
            this.descriptors_Tree.Location = new System.Drawing.Point(6, 19);
            this.descriptors_Tree.Name = "descriptors_Tree";
            this.descriptors_Tree.ShowLines = false;
            this.descriptors_Tree.Size = new System.Drawing.Size(139, 170);
            this.descriptors_Tree.TabIndex = 27;
            this.descriptors_Tree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.descriptors_Tree_AfterCheck);
            this.descriptors_Tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.descriptors_Tree_AfterSelect);
            // 
            // tones_Group
            // 
            this.tones_Group.Controls.Add(this.tones_SelectionBox);
            this.tones_Group.Location = new System.Drawing.Point(194, 8);
            this.tones_Group.Name = "tones_Group";
            this.tones_Group.Size = new System.Drawing.Size(295, 184);
            this.tones_Group.TabIndex = 50;
            this.tones_Group.TabStop = false;
            this.tones_Group.Text = "groupBox2";
            // 
            // tones_SelectionBox
            // 
            this.tones_SelectionBox.Location = new System.Drawing.Point(6, 19);
            this.tones_SelectionBox.Name = "tones_SelectionBox";
            this.tones_SelectionBox.Size = new System.Drawing.Size(288, 159);
            this.tones_SelectionBox.TabIndex = 25;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.imageSelectionBox);
            this.groupBox1.Location = new System.Drawing.Point(11, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(166, 168);
            this.groupBox1.TabIndex = 47;
            this.groupBox1.TabStop = false;
            // 
            // imageSelectionBox
            // 
            this.imageSelectionBox.Location = new System.Drawing.Point(6, 12);
            this.imageSelectionBox.Name = "imageSelectionBox";
            this.imageSelectionBox.Size = new System.Drawing.Size(156, 150);
            this.imageSelectionBox.TabIndex = 13;
            this.imageSelectionBox.ItemChanged += new System.EventHandler(this.imageSelectionBox_ItemChanged);
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.close_Button.Location = new System.Drawing.Point(632, 414);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(75, 23);
            this.close_Button.TabIndex = 46;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            // 
            // apply_Button
            // 
            this.apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply_Button.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.apply_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apply_Button.Location = new System.Drawing.Point(551, 414);
            this.apply_Button.Name = "apply_Button";
            this.apply_Button.Size = new System.Drawing.Size(75, 23);
            this.apply_Button.TabIndex = 45;
            this.apply_Button.Text = "Apply";
            this.apply_Button.UseVisualStyleBackColor = true;
            // 
            // postProcCheck
            // 
            this.postProcCheck.AutoSize = true;
            this.postProcCheck.Checked = true;
            this.postProcCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.postProcCheck.Location = new System.Drawing.Point(12, 199);
            this.postProcCheck.Name = "postProcCheck";
            this.postProcCheck.Size = new System.Drawing.Size(144, 17);
            this.postProcCheck.TabIndex = 54;
            this.postProcCheck.Text = "Features post-processing";
            this.postProcCheck.UseVisualStyleBackColor = true;
            this.postProcCheck.CheckedChanged += new System.EventHandler(this.postProcCheck_CheckedChanged);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 59;
            this.label1.Text = "Window";
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
            // postProcGroup
            // 
            this.postProcGroup.Controls.Add(this.alphaEditor);
            this.postProcGroup.Controls.Add(this.nonlinearCheck);
            this.postProcGroup.Controls.Add(this.label2);
            this.postProcGroup.Controls.Add(this.energyCheck);
            this.postProcGroup.Controls.Add(this.label1);
            this.postProcGroup.Controls.Add(this.gaussianCheck);
            this.postProcGroup.Controls.Add(this.windowSize);
            this.postProcGroup.Location = new System.Drawing.Point(12, 205);
            this.postProcGroup.Name = "postProcGroup";
            this.postProcGroup.Size = new System.Drawing.Size(166, 193);
            this.postProcGroup.TabIndex = 61;
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
            // TonesTexturesBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 440);
            this.Controls.Add(this.postProcCheck);
            this.Controls.Add(this.postProcGroup);
            this.Controls.Add(this.progress_Label);
            this.Controls.Add(this.textures_Check);
            this.Controls.Add(this.tones_Check);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.textures_Box);
            this.Controls.Add(this.tones_Group);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.close_Button);
            this.Controls.Add(this.apply_Button);
            this.Name = "TonesTexturesBase";
            this.Text = "TonesTexturesBase";
            this.textures_Box.ResumeLayout(false);
            this.tones_Group.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.windowSize)).EndInit();
            this.postProcGroup.ResumeLayout(false);
            this.postProcGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label progress_Label;
        protected System.Windows.Forms.CheckBox textures_Check;
        protected System.Windows.Forms.CheckBox tones_Check;
        protected System.Windows.Forms.ProgressBar progressBar;
        protected System.Windows.Forms.GroupBox textures_Box;
        protected TxEstudioKernel.VisualElements.AlgorithmViewer descriptor_Viewer;
        protected System.Windows.Forms.TreeView descriptors_Tree;
        protected System.Windows.Forms.GroupBox tones_Group;
        protected TxEstudioApplication.Custom_Controls.ImageMultiSelectionBox tones_SelectionBox;
        protected System.Windows.Forms.GroupBox groupBox1;
        protected TxEstudioApplication.Custom_Controls.ImageSelectionBox imageSelectionBox;
        protected System.Windows.Forms.Button close_Button;
        protected System.Windows.Forms.Button apply_Button;
        private System.Windows.Forms.CheckBox postProcCheck;
        private System.Windows.Forms.CheckBox nonlinearCheck;
        private System.Windows.Forms.CheckBox energyCheck;
        private System.Windows.Forms.CheckBox gaussianCheck;
        private System.Windows.Forms.NumericUpDown windowSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox postProcGroup;
        private TxEstudioKernel.VisualElements.RealParameterEditor alphaEditor;



    }
}