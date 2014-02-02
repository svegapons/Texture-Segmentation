namespace TxEstudioApplication.Combination_Methods
{
    partial class EdgeDetectorSelector
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
            this.lb_edgesDetectors = new System.Windows.Forms.ListBox();
            this.algorithmViewer = new TxEstudioKernel.VisualElements.AlgorithmViewer();
            this.bt_OK = new System.Windows.Forms.Button();
            this.bt_Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lb_edgesDetectors
            // 
            this.lb_edgesDetectors.FormattingEnabled = true;
            this.lb_edgesDetectors.Location = new System.Drawing.Point(29, 26);
            this.lb_edgesDetectors.Name = "lb_edgesDetectors";
            this.lb_edgesDetectors.Size = new System.Drawing.Size(183, 251);
            this.lb_edgesDetectors.TabIndex = 1;
            this.lb_edgesDetectors.SelectedIndexChanged += new System.EventHandler(this.lb_edgesDetectors_SelectedIndexChanged);
            // 
            // algorithmViewer
            // 
            this.algorithmViewer.AlgorithmInstance = null;
            this.algorithmViewer.Location = new System.Drawing.Point(237, 26);
            this.algorithmViewer.Name = "algorithmViewer";
            this.algorithmViewer.ShowDescription = true;
            this.algorithmViewer.Size = new System.Drawing.Size(194, 253);
            this.algorithmViewer.TabIndex = 2;
            this.algorithmViewer.ViewParameterDescription = true;
            // 
            // bt_OK
            // 
            this.bt_OK.Location = new System.Drawing.Point(311, 304);
            this.bt_OK.Name = "bt_OK";
            this.bt_OK.Size = new System.Drawing.Size(75, 23);
            this.bt_OK.TabIndex = 3;
            this.bt_OK.Text = "OK";
            this.bt_OK.UseVisualStyleBackColor = true;
            this.bt_OK.Click += new System.EventHandler(this.bt_OK_Click);
            // 
            // bt_Cancel
            // 
            this.bt_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_Cancel.Location = new System.Drawing.Point(402, 304);
            this.bt_Cancel.Name = "bt_Cancel";
            this.bt_Cancel.Size = new System.Drawing.Size(75, 23);
            this.bt_Cancel.TabIndex = 4;
            this.bt_Cancel.Text = "Cancel";
            this.bt_Cancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Edge detectors";
            // 
            // EdgeDetectorSelector
            // 
            this.AcceptButton = this.bt_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.bt_Cancel;
            this.ClientSize = new System.Drawing.Size(495, 339);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bt_Cancel);
            this.Controls.Add(this.bt_OK);
            this.Controls.Add(this.algorithmViewer);
            this.Controls.Add(this.lb_edgesDetectors);
            this.Name = "EdgeDetectorSelector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lb_edgesDetectors;
        private TxEstudioKernel.VisualElements.AlgorithmViewer algorithmViewer;
        private System.Windows.Forms.Button bt_OK;
        private System.Windows.Forms.Button bt_Cancel;
        private System.Windows.Forms.Label label1;
    }
}
