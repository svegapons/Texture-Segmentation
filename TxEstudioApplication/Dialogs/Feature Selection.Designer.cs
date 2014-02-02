namespace TxEstudioApplication.Dialogs
{
    partial class Feature_Selection
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
            this.algorithmViewer = new TxEstudioKernel.VisualElements.AlgorithmViewer();
            this.label3 = new System.Windows.Forms.Label();
            this.algorithmSelector = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groundTruthSelectionBox = new TxEstudioApplication.Custom_Controls.ImageSelectionBox();
            this.label4 = new System.Windows.Forms.Label();
            this.measureSelector = new System.Windows.Forms.ComboBox();
            this.dimensionSelector = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.lb_error = new System.Windows.Forms.Label();
            this.tbx_error = new System.Windows.Forms.TextBox();
            this.cb_super = new System.Windows.Forms.CheckBox();
            this.textures_Box.SuspendLayout();
            this.tones_Group.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dimensionSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // descriptor_Viewer
            // 
            this.descriptor_Viewer.ViewParameterDescription = true;
            // 
            // descriptors_Tree
            // 
            this.descriptors_Tree.LineColor = System.Drawing.Color.Black;
            // 
            // close_Button
            // 
            this.close_Button.Location = new System.Drawing.Point(718, 414);
            // 
            // apply_Button
            // 
            this.apply_Button.Location = new System.Drawing.Point(637, 414);
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // algorithmViewer
            // 
            this.algorithmViewer.AlgorithmInstance = null;
            this.algorithmViewer.Location = new System.Drawing.Point(495, 69);
            this.algorithmViewer.Name = "algorithmViewer";
            this.algorithmViewer.ShowDescription = true;
            this.algorithmViewer.Size = new System.Drawing.Size(194, 329);
            this.algorithmViewer.TabIndex = 62;
            this.algorithmViewer.ViewParameterDescription = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(505, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 13);
            this.label3.TabIndex = 63;
            this.label3.Text = "Segmentation Algorithm:";
            // 
            // algorithmSelector
            // 
            this.algorithmSelector.FormattingEnabled = true;
            this.algorithmSelector.Location = new System.Drawing.Point(495, 43);
            this.algorithmSelector.Name = "algorithmSelector";
            this.algorithmSelector.Size = new System.Drawing.Size(194, 21);
            this.algorithmSelector.TabIndex = 64;
            this.algorithmSelector.SelectedValueChanged += new System.EventHandler(this.algorithmSelector_SelectedValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(695, 36);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(166, 168);
            this.groupBox2.TabIndex = 65;
            this.groupBox2.TabStop = false;
            // 
            // groundTruthSelectionBox
            // 
            this.groundTruthSelectionBox.Location = new System.Drawing.Point(702, 43);
            this.groundTruthSelectionBox.Name = "groundTruthSelectionBox";
            this.groundTruthSelectionBox.Size = new System.Drawing.Size(156, 150);
            this.groundTruthSelectionBox.TabIndex = 13;
            this.groundTruthSelectionBox.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(714, 250);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 66;
            this.label4.Text = "Similarity Measure";
            this.label4.Visible = false;
            // 
            // measureSelector
            // 
            this.measureSelector.FormattingEnabled = true;
            this.measureSelector.Location = new System.Drawing.Point(702, 268);
            this.measureSelector.Name = "measureSelector";
            this.measureSelector.Size = new System.Drawing.Size(162, 21);
            this.measureSelector.TabIndex = 67;
            this.measureSelector.Visible = false;
            // 
            // dimensionSelector
            // 
            this.dimensionSelector.Location = new System.Drawing.Point(736, 319);
            this.dimensionSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dimensionSelector.Name = "dimensionSelector";
            this.dimensionSelector.Size = new System.Drawing.Size(51, 20);
            this.dimensionSelector.TabIndex = 69;
            this.dimensionSelector.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(699, 303);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 70;
            this.label6.Text = "Dimension:";
            // 
            // lb_error
            // 
            this.lb_error.AutoSize = true;
            this.lb_error.Location = new System.Drawing.Point(702, 343);
            this.lb_error.Name = "lb_error";
            this.lb_error.Size = new System.Drawing.Size(32, 13);
            this.lb_error.TabIndex = 71;
            this.lb_error.Text = "Error:";
            // 
            // tbx_error
            // 
            this.tbx_error.Location = new System.Drawing.Point(736, 358);
            this.tbx_error.Name = "tbx_error";
            this.tbx_error.Size = new System.Drawing.Size(51, 20);
            this.tbx_error.TabIndex = 72;
            this.tbx_error.Text = "0.03";
            // 
            // cb_super
            // 
            this.cb_super.AutoSize = true;
            this.cb_super.Location = new System.Drawing.Point(705, 16);
            this.cb_super.Name = "cb_super";
            this.cb_super.Size = new System.Drawing.Size(92, 17);
            this.cb_super.TabIndex = 73;
            this.cb_super.Text = " Ground Truth";
            this.cb_super.UseVisualStyleBackColor = true;
            this.cb_super.CheckedChanged += new System.EventHandler(this.cb_super_CheckedChanged);
            // 
            // Feature_Selection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(804, 440);
            this.Controls.Add(this.groundTruthSelectionBox);
            this.Controls.Add(this.cb_super);
            this.Controls.Add(this.tbx_error);
            this.Controls.Add(this.lb_error);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dimensionSelector);
            this.Controls.Add(this.measureSelector);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.algorithmSelector);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.algorithmViewer);
            this.Name = "Feature_Selection";
            this.Text = "Feature Selection";
            this.Controls.SetChildIndex(this.algorithmViewer, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.algorithmSelector, 0);
            this.Controls.SetChildIndex(this.apply_Button, 0);
            this.Controls.SetChildIndex(this.close_Button, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.tones_Group, 0);
            this.Controls.SetChildIndex(this.textures_Box, 0);
            this.Controls.SetChildIndex(this.progressBar, 0);
            this.Controls.SetChildIndex(this.tones_Check, 0);
            this.Controls.SetChildIndex(this.textures_Check, 0);
            this.Controls.SetChildIndex(this.progress_Label, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.measureSelector, 0);
            this.Controls.SetChildIndex(this.dimensionSelector, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.lb_error, 0);
            this.Controls.SetChildIndex(this.tbx_error, 0);
            this.Controls.SetChildIndex(this.cb_super, 0);
            this.Controls.SetChildIndex(this.groundTruthSelectionBox, 0);
            this.textures_Box.ResumeLayout(false);
            this.tones_Group.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dimensionSelector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TxEstudioKernel.VisualElements.AlgorithmViewer algorithmViewer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox algorithmSelector;
        protected System.Windows.Forms.GroupBox groupBox2;
        protected TxEstudioApplication.Custom_Controls.ImageSelectionBox groundTruthSelectionBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox measureSelector;
        private System.Windows.Forms.NumericUpDown dimensionSelector;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lb_error;
        private System.Windows.Forms.TextBox tbx_error;
        private System.Windows.Forms.CheckBox cb_super;
    }
}
