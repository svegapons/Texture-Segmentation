namespace TxEstudioApplication.Dialogs
{
    partial class PCAResult
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.EigenVals = new System.Windows.Forms.DataGridView();
            this.EigenValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Percent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccumulatedPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EigenVectors = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.EigenVals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EigenVectors)).BeginInit();
            this.SuspendLayout();
            // 
            // EigenVals
            // 
            this.EigenVals.AllowUserToAddRows = false;
            this.EigenVals.AllowUserToDeleteRows = false;
            this.EigenVals.AllowUserToResizeColumns = false;
            this.EigenVals.AllowUserToResizeRows = false;
            this.EigenVals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EigenVals.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EigenValue,
            this.Percent,
            this.AccumulatedPercent});
            this.EigenVals.Location = new System.Drawing.Point(12, 22);
            this.EigenVals.Name = "EigenVals";
            this.EigenVals.ReadOnly = true;
            this.EigenVals.RowHeadersVisible = false;
            this.EigenVals.RowHeadersWidth = 40;
            this.EigenVals.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.EigenVals.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.EigenVals.Size = new System.Drawing.Size(234, 121);
            this.EigenVals.TabIndex = 0;
            // 
            // EigenValue
            // 
            this.EigenValue.HeaderText = "Eigen Value  ";
            this.EigenValue.Name = "EigenValue";
            this.EigenValue.ReadOnly = true;
            this.EigenValue.Width = 80;
            // 
            // Percent
            // 
            this.Percent.HeaderText = "Percent";
            this.Percent.Name = "Percent";
            this.Percent.ReadOnly = true;
            this.Percent.Width = 50;
            // 
            // AccumulatedPercent
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.AccumulatedPercent.DefaultCellStyle = dataGridViewCellStyle1;
            this.AccumulatedPercent.HeaderText = "Accumulated Percent";
            this.AccumulatedPercent.Name = "AccumulatedPercent";
            this.AccumulatedPercent.ReadOnly = true;
            // 
            // EigenVectors
            // 
            this.EigenVectors.AllowUserToAddRows = false;
            this.EigenVectors.AllowUserToDeleteRows = false;
            this.EigenVectors.AllowUserToResizeColumns = false;
            this.EigenVectors.AllowUserToResizeRows = false;
            this.EigenVectors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EigenVectors.Location = new System.Drawing.Point(277, 22);
            this.EigenVectors.Name = "EigenVectors";
            this.EigenVectors.RowHeadersWidth = 60;
            this.EigenVectors.Size = new System.Drawing.Size(240, 55);
            this.EigenVectors.TabIndex = 1;
            // 
            // PCAResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 276);
            this.Controls.Add(this.EigenVectors);
            this.Controls.Add(this.EigenVals);
            this.Name = "PCAResult";
            this.Text = "PCA Result";
            ((System.ComponentModel.ISupportInitialize)(this.EigenVals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EigenVectors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView EigenVals;
        private System.Windows.Forms.DataGridViewTextBoxColumn EigenValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Percent;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccumulatedPercent;
        private System.Windows.Forms.DataGridView EigenVectors;
    }
}