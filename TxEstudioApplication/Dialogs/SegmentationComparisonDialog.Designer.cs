namespace TxEstudioApplication.Dialogs
{
    partial class SegmentationComparisonDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.calculateButton = new System.Windows.Forms.Button();
            this.segmentationBox1 = new TxEstudioApplication.Custom_Controls.ImageSelectionBox();
            this.segmentationBox2 = new TxEstudioApplication.Custom_Controls.ImageSelectionBox();
            this.label1 = new System.Windows.Forms.Label();
            this.totalCoincidenceLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.coincidenceMasureLabel = new System.Windows.Forms.Label();
            this.totalCoincidenceMapBox = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.coincidenceByClassList = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.totalCoincidenceMapBox)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.calculateButton);
            this.groupBox1.Controls.Add(this.segmentationBox1);
            this.groupBox1.Controls.Add(this.segmentationBox2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(389, 214);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Segmentation";
            // 
            // calculateButton
            // 
            this.calculateButton.Location = new System.Drawing.Point(308, 185);
            this.calculateButton.Name = "calculateButton";
            this.calculateButton.Size = new System.Drawing.Size(75, 23);
            this.calculateButton.TabIndex = 2;
            this.calculateButton.Text = "Calculate";
            this.calculateButton.UseVisualStyleBackColor = true;
            this.calculateButton.Click += new System.EventHandler(this.calculateButton_Click);
            // 
            // segmentationBox1
            // 
            this.segmentationBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.segmentationBox1.Location = new System.Drawing.Point(6, 19);
            this.segmentationBox1.Name = "segmentationBox1";
            this.segmentationBox1.Size = new System.Drawing.Size(150, 150);
            this.segmentationBox1.TabIndex = 0;
            // 
            // segmentationBox2
            // 
            this.segmentationBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.segmentationBox2.Location = new System.Drawing.Point(204, 19);
            this.segmentationBox2.Name = "segmentationBox2";
            this.segmentationBox2.Size = new System.Drawing.Size(150, 150);
            this.segmentationBox2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Total coincidence percent:";
            // 
            // totalCoincidenceLabel
            // 
            this.totalCoincidenceLabel.AutoSize = true;
            this.totalCoincidenceLabel.Location = new System.Drawing.Point(146, 19);
            this.totalCoincidenceLabel.Name = "totalCoincidenceLabel";
            this.totalCoincidenceLabel.Size = new System.Drawing.Size(34, 13);
            this.totalCoincidenceLabel.TabIndex = 4;
            this.totalCoincidenceLabel.Text = "00.00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Location = new System.Drawing.Point(6, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Coincidence measure:";
            // 
            // coincidenceMasureLabel
            // 
            this.coincidenceMasureLabel.AutoSize = true;
            this.coincidenceMasureLabel.Location = new System.Drawing.Point(146, 36);
            this.coincidenceMasureLabel.Name = "coincidenceMasureLabel";
            this.coincidenceMasureLabel.Size = new System.Drawing.Size(34, 13);
            this.coincidenceMasureLabel.TabIndex = 6;
            this.coincidenceMasureLabel.Text = "00.00";
            // 
            // totalCoincidenceMapBox
            // 
            this.totalCoincidenceMapBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.totalCoincidenceMapBox.Location = new System.Drawing.Point(134, 61);
            this.totalCoincidenceMapBox.Name = "totalCoincidenceMapBox";
            this.totalCoincidenceMapBox.Size = new System.Drawing.Size(135, 108);
            this.totalCoincidenceMapBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.totalCoincidenceMapBox.TabIndex = 7;
            this.totalCoincidenceMapBox.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label5.Location = new System.Drawing.Point(6, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Total coincidence map:";
            // 
            // coincidenceByClassList
            // 
            this.coincidenceByClassList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.coincidenceByClassList.FormattingEnabled = true;
            this.coincidenceByClassList.Location = new System.Drawing.Point(9, 185);
            this.coincidenceByClassList.Name = "coincidenceByClassList";
            this.coincidenceByClassList.Size = new System.Drawing.Size(260, 108);
            this.coincidenceByClassList.TabIndex = 9;
            this.coincidenceByClassList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.coincidenceByClassList_DrawItem);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label6.Location = new System.Drawing.Point(6, 167);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Coincidence by class:";
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(623, 335);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 11;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.totalCoincidenceLabel);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.coincidenceByClassList);
            this.groupBox2.Controls.Add(this.coincidenceMasureLabel);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.totalCoincidenceMapBox);
            this.groupBox2.Location = new System.Drawing.Point(407, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(291, 306);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Result";
            // 
            // SegmentationComparisonDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 367);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SegmentationComparisonDialog";
            this.ShowInTaskbar = false;
            this.Text = "Compare segmentation results";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.totalCoincidenceMapBox)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TxEstudioApplication.Custom_Controls.ImageSelectionBox segmentationBox1;
        private TxEstudioApplication.Custom_Controls.ImageSelectionBox segmentationBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button calculateButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label totalCoincidenceLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label coincidenceMasureLabel;
        private System.Windows.Forms.PictureBox totalCoincidenceMapBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox coincidenceByClassList;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}