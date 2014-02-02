namespace TxEstudioApplication.Dialogs
{
    partial class OneBandDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OneBandDialog));
            this.apply_Button = new System.Windows.Forms.Button();
            this.close_Button = new System.Windows.Forms.Button();
            this.imageSelectionBox = new TxEstudioApplication.Custom_Controls.ImageSelectionBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.algorithmViewer = new TxEstudioKernel.VisualElements.AlgorithmViewer();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // apply_Button
            // 
            this.apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply_Button.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.apply_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apply_Button.Location = new System.Drawing.Point(245, 266);
            this.apply_Button.Name = "apply_Button";
            this.apply_Button.Size = new System.Drawing.Size(75, 23);
            this.apply_Button.TabIndex = 3;
            this.apply_Button.Text = "Apply";
            this.apply_Button.UseVisualStyleBackColor = true;
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.close_Button.Location = new System.Drawing.Point(329, 266);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(75, 23);
            this.close_Button.TabIndex = 4;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            // 
            // imageSelectionBox
            // 
            this.imageSelectionBox.Location = new System.Drawing.Point(6, 12);
            this.imageSelectionBox.Name = "imageSelectionBox";
            this.imageSelectionBox.Size = new System.Drawing.Size(156, 150);
            this.imageSelectionBox.TabIndex = 13;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.imageSelectionBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(166, 168);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // algorithmViewer
            // 
            this.algorithmViewer.AlgorithmInstance = null;
            this.algorithmViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.algorithmViewer.Location = new System.Drawing.Point(217, 12);
            this.algorithmViewer.Margin = new System.Windows.Forms.Padding(3, 3, 3, 35);
            this.algorithmViewer.Name = "algorithmViewer";
            this.algorithmViewer.ShowDescription = true;
            this.algorithmViewer.Size = new System.Drawing.Size(176, 235);
            this.algorithmViewer.TabIndex = 18;
            this.algorithmViewer.ViewParameterDescription = true;
            // 
            // OneBandDialog
            // 
            this.AcceptButton = this.apply_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.close_Button;
            this.ClientSize = new System.Drawing.Size(416, 291);
            this.Controls.Add(this.algorithmViewer);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.close_Button);
            this.Controls.Add(this.apply_Button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OneBandDialog";
            this.ShowInTaskbar = false;
            this.Text = "One band operator";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button apply_Button;
        private System.Windows.Forms.Button close_Button;
        private TxEstudioApplication.Custom_Controls.ImageSelectionBox imageSelectionBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private TxEstudioKernel.VisualElements.AlgorithmViewer algorithmViewer;
    }
}