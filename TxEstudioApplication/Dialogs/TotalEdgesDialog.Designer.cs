namespace TxEstudioApplication.Dialogs
{
    partial class TotalEdgesDialog
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
            this.edgeDetectors_Combo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.average_Check = new System.Windows.Forms.CheckBox();
            this.maximum_Check = new System.Windows.Forms.CheckBox();
            this.summation_Check = new System.Windows.Forms.CheckBox();
            this.textures_Box.SuspendLayout();
            this.tones_Group.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            // apply_Button
            // 
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // algorithmViewer
            // 
            this.algorithmViewer.AlgorithmInstance = null;
            this.algorithmViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.algorithmViewer.Location = new System.Drawing.Point(510, 54);
            this.algorithmViewer.Margin = new System.Windows.Forms.Padding(3, 3, 3, 35);
            this.algorithmViewer.Name = "algorithmViewer";
            this.algorithmViewer.ShowDescription = true;
            this.algorithmViewer.Size = new System.Drawing.Size(184, 243);
            this.algorithmViewer.TabIndex = 37;
            this.algorithmViewer.ViewParameterDescription = true;
            // 
            // edgeDetectors_Combo
            // 
            this.edgeDetectors_Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.edgeDetectors_Combo.FormattingEnabled = true;
            this.edgeDetectors_Combo.Location = new System.Drawing.Point(510, 27);
            this.edgeDetectors_Combo.Name = "edgeDetectors_Combo";
            this.edgeDetectors_Combo.Size = new System.Drawing.Size(171, 21);
            this.edgeDetectors_Combo.TabIndex = 45;
            this.edgeDetectors_Combo.SelectedIndexChanged += new System.EventHandler(this.edgeDetectors_Combo_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(496, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 46;
            this.label1.Text = "Edge detector";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.average_Check);
            this.groupBox2.Controls.Add(this.maximum_Check);
            this.groupBox2.Controls.Add(this.summation_Check);
            this.groupBox2.Location = new System.Drawing.Point(510, 307);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(184, 91);
            this.groupBox2.TabIndex = 47;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Criteria";
            // 
            // average_Check
            // 
            this.average_Check.AutoSize = true;
            this.average_Check.Location = new System.Drawing.Point(7, 66);
            this.average_Check.Name = "average_Check";
            this.average_Check.Size = new System.Drawing.Size(66, 17);
            this.average_Check.TabIndex = 4;
            this.average_Check.Text = "Average";
            this.average_Check.UseVisualStyleBackColor = true;
            // 
            // maximum_Check
            // 
            this.maximum_Check.AutoSize = true;
            this.maximum_Check.Location = new System.Drawing.Point(6, 43);
            this.maximum_Check.Name = "maximum_Check";
            this.maximum_Check.Size = new System.Drawing.Size(70, 17);
            this.maximum_Check.TabIndex = 2;
            this.maximum_Check.Text = "Maximum";
            this.maximum_Check.UseVisualStyleBackColor = true;
            // 
            // summation_Check
            // 
            this.summation_Check.AutoSize = true;
            this.summation_Check.Location = new System.Drawing.Point(7, 20);
            this.summation_Check.Name = "summation_Check";
            this.summation_Check.Size = new System.Drawing.Size(78, 17);
            this.summation_Check.TabIndex = 0;
            this.summation_Check.Text = "Summation";
            this.summation_Check.UseVisualStyleBackColor = true;
            // 
            // TotalEdgesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 440);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edgeDetectors_Combo);
            this.Controls.Add(this.algorithmViewer);
            this.Controls.Add(this.groupBox2);
            this.Name = "TotalEdgesDialog";
            this.Text = "TotalEdgesDialog";
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.algorithmViewer, 0);
            this.Controls.SetChildIndex(this.edgeDetectors_Combo, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.apply_Button, 0);
            this.Controls.SetChildIndex(this.close_Button, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.tones_Group, 0);
            this.Controls.SetChildIndex(this.textures_Box, 0);
            this.Controls.SetChildIndex(this.progressBar, 0);
            this.Controls.SetChildIndex(this.tones_Check, 0);
            this.Controls.SetChildIndex(this.textures_Check, 0);
            this.Controls.SetChildIndex(this.progress_Label, 0);
            this.textures_Box.ResumeLayout(false);
            this.tones_Group.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TxEstudioKernel.VisualElements.AlgorithmViewer algorithmViewer;
        private System.Windows.Forms.ComboBox edgeDetectors_Combo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox average_Check;
        private System.Windows.Forms.CheckBox maximum_Check;
        private System.Windows.Forms.CheckBox summation_Check;

    }
}