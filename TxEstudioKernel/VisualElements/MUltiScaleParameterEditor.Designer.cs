namespace TxEstudioKernel.VisualElements
{
    partial class MultiScaleParameterEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.weightsList = new System.Windows.Forms.ListBox();
            this.sigmasList = new System.Windows.Forms.ListBox();
            this.newWeightEditor = new TxEstudioKernel.VisualElements.RealParameterEditor();
            this.newSigmaEditor = new TxEstudioKernel.VisualElements.RealParameterEditor();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // weightsList
            // 
            this.weightsList.FormattingEnabled = true;
            this.weightsList.Location = new System.Drawing.Point(130, 24);
            this.weightsList.Name = "weightsList";
            this.weightsList.Size = new System.Drawing.Size(40, 82);
            this.weightsList.TabIndex = 0;
            this.weightsList.SelectedIndexChanged += new System.EventHandler(this.weightsList_SelectedIndexChanged);
            // 
            // sigmasList
            // 
            this.sigmasList.FormattingEnabled = true;
            this.sigmasList.Location = new System.Drawing.Point(183, 24);
            this.sigmasList.Name = "sigmasList";
            this.sigmasList.Size = new System.Drawing.Size(40, 82);
            this.sigmasList.TabIndex = 2;
            this.sigmasList.SelectedIndexChanged += new System.EventHandler(this.sigmasList_SelectedIndexChanged);
            // 
            // newWeightEditor
            // 
            this.newWeightEditor.Location = new System.Drawing.Point(14, 24);
            this.newWeightEditor.Maximum = 3.402823E+38F;
            this.newWeightEditor.Minimum = -3.402823E+38F;
            this.newWeightEditor.Name = "newWeightEditor";
            this.newWeightEditor.ParameterValue = 0F;
            this.newWeightEditor.Size = new System.Drawing.Size(60, 20);
            this.newWeightEditor.TabIndex = 3;
            this.newWeightEditor.Text = "0";
            // 
            // newSigmaEditor
            // 
            this.newSigmaEditor.Location = new System.Drawing.Point(14, 63);
            this.newSigmaEditor.Maximum = 3.402823E+38F;
            this.newSigmaEditor.Minimum = -3.402823E+38F;
            this.newSigmaEditor.Name = "newSigmaEditor";
            this.newSigmaEditor.ParameterValue = 0F;
            this.newSigmaEditor.Size = new System.Drawing.Size(60, 20);
            this.newSigmaEditor.TabIndex = 4;
            this.newSigmaEditor.Text = "0";
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(91, 24);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(23, 23);
            this.addButton.TabIndex = 5;
            this.addButton.Text = "+";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(91, 53);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(23, 23);
            this.removeButton.TabIndex = 6;
            this.removeButton.Text = "-";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "New Weight";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "New Sigma";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(130, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Weights";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(184, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Sigmas";
            // 
            // MultiScaleParameterEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.newSigmaEditor);
            this.Controls.Add(this.newWeightEditor);
            this.Controls.Add(this.sigmasList);
            this.Controls.Add(this.weightsList);
            this.Name = "MultiScaleParameterEditor";
            this.Size = new System.Drawing.Size(228, 118);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox weightsList;
        private System.Windows.Forms.ListBox sigmasList;
        private RealParameterEditor newWeightEditor;
        private RealParameterEditor newSigmaEditor;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
