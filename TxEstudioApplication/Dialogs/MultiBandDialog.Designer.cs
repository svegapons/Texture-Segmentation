namespace TxEstudioApplication.Dialogs
{
    partial class MultiBandDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiBandDialog));
            this.imageMultiSelectionBox1 = new TxEstudioApplication.Custom_Controls.ImageMultiSelectionBox();
            this.algorithmViewer = new TxEstudioKernel.VisualElements.AlgorithmViewer();
            this.apply_Button = new System.Windows.Forms.Button();
            this.close_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // imageMultiSelectionBox1
            // 
            this.imageMultiSelectionBox1.Location = new System.Drawing.Point(2, 4);
            this.imageMultiSelectionBox1.Name = "imageMultiSelectionBox1";
            this.imageMultiSelectionBox1.Size = new System.Drawing.Size(288, 159);
            this.imageMultiSelectionBox1.TabIndex = 0;
            // 
            // algorithmViewer
            // 
            this.algorithmViewer.AlgorithmInstance = null;
            this.algorithmViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.algorithmViewer.Location = new System.Drawing.Point(296, 12);
            this.algorithmViewer.Margin = new System.Windows.Forms.Padding(3, 3, 3, 35);
            this.algorithmViewer.Name = "algorithmViewer";
            this.algorithmViewer.ShowDescription = true;
            this.algorithmViewer.Size = new System.Drawing.Size(179, 263);
            this.algorithmViewer.TabIndex = 1;
            this.algorithmViewer.ViewParameterDescription = true;
            // 
            // apply_Button
            // 
            this.apply_Button.Location = new System.Drawing.Point(309, 293);
            this.apply_Button.Name = "apply_Button";
            this.apply_Button.Size = new System.Drawing.Size(75, 23);
            this.apply_Button.TabIndex = 2;
            this.apply_Button.Text = "Apply";
            this.apply_Button.UseVisualStyleBackColor = true;
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // close_Button
            // 
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.Location = new System.Drawing.Point(390, 293);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(75, 23);
            this.close_Button.TabIndex = 3;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            // 
            // MultiBandDialog
            // 
            this.AcceptButton = this.apply_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close_Button;
            this.ClientSize = new System.Drawing.Size(488, 328);
            this.Controls.Add(this.close_Button);
            this.Controls.Add(this.apply_Button);
            this.Controls.Add(this.algorithmViewer);
            this.Controls.Add(this.imageMultiSelectionBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultiBandDialog";
            this.ShowInTaskbar = false;
            this.Text = "Multi Band Operator";
            this.ResumeLayout(false);

        }

        #endregion

        private TxEstudioApplication.Custom_Controls.ImageMultiSelectionBox imageMultiSelectionBox1;
        private TxEstudioKernel.VisualElements.AlgorithmViewer algorithmViewer;
        private System.Windows.Forms.Button apply_Button;
        private System.Windows.Forms.Button close_Button;
    }
}