namespace TxEstudioApplication.Dialogs
{
    partial class BrightContrastDialog
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
            this.Contrast = new System.Windows.Forms.TrackBar();
            this.Bright = new System.Windows.Forms.TrackBar();
            this.pictureControl = new TxEstudioApplication.Custom_Controls.ImageSelectionBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Apply = new System.Windows.Forms.Button();
            this.Reset = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.BrightVal = new System.Windows.Forms.NumericUpDown();
            this.ContrastVal = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.Contrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bright)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrightVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContrastVal)).BeginInit();
            this.SuspendLayout();
            // 
            // Contrast
            // 
            this.Contrast.Location = new System.Drawing.Point(6, 21);
            this.Contrast.Maximum = 100;
            this.Contrast.Minimum = -100;
            this.Contrast.Name = "Contrast";
            this.Contrast.Size = new System.Drawing.Size(196, 45);
            this.Contrast.TabIndex = 0;
            this.Contrast.Scroll += new System.EventHandler(this.Contrast_Scroll);
            // 
            // Bright
            // 
            this.Bright.Location = new System.Drawing.Point(6, 19);
            this.Bright.Maximum = 100;
            this.Bright.Minimum = -100;
            this.Bright.Name = "Bright";
            this.Bright.Size = new System.Drawing.Size(196, 45);
            this.Bright.TabIndex = 1;
            this.Bright.Scroll += new System.EventHandler(this.Bright_Scroll);
            // 
            // pictureControl
            // 
            this.pictureControl.Location = new System.Drawing.Point(17, 19);
            this.pictureControl.Name = "pictureControl";
            this.pictureControl.Size = new System.Drawing.Size(150, 150);
            this.pictureControl.TabIndex = 5;
            this.pictureControl.ItemChanged += new System.EventHandler(this.ImageChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureControl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(185, 181);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.BrightVal);
            this.groupBox2.Controls.Add(this.Bright);
            this.groupBox2.Location = new System.Drawing.Point(222, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(271, 76);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Bright";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ContrastVal);
            this.groupBox3.Controls.Add(this.Contrast);
            this.groupBox3.Location = new System.Drawing.Point(222, 115);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(271, 78);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Contrast";
            // 
            // Apply
            // 
            this.Apply.Location = new System.Drawing.Point(258, 222);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(75, 23);
            this.Apply.TabIndex = 9;
            this.Apply.Text = "Apply";
            this.Apply.UseVisualStyleBackColor = true;
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // Reset
            // 
            this.Reset.Location = new System.Drawing.Point(81, 222);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(75, 23);
            this.Reset.TabIndex = 10;
            this.Reset.Text = "Reset";
            this.Reset.UseVisualStyleBackColor = true;
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // Close
            // 
            this.Close.Location = new System.Drawing.Point(365, 222);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 23);
            this.Close.TabIndex = 11;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // BrightVal
            // 
            this.BrightVal.Location = new System.Drawing.Point(208, 19);
            this.BrightVal.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.BrightVal.Name = "BrightVal";
            this.BrightVal.Size = new System.Drawing.Size(50, 20);
            this.BrightVal.TabIndex = 2;
            this.BrightVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BrightVal.ValueChanged += new System.EventHandler(this.BrightVal_ValueChanged);
            // 
            // ContrastVal
            // 
            this.ContrastVal.Location = new System.Drawing.Point(209, 20);
            this.ContrastVal.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.ContrastVal.Name = "ContrastVal";
            this.ContrastVal.Size = new System.Drawing.Size(50, 20);
            this.ContrastVal.TabIndex = 1;
            this.ContrastVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ContrastVal.ValueChanged += new System.EventHandler(this.ContrastVal_ValueChanged);
            // 
            // BrightContrastDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 284);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.Reset);
            this.Controls.Add(this.Apply);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "BrightContrastDialog";
            this.Text = "BrightContrastDialog";
            ((System.ComponentModel.ISupportInitialize)(this.Contrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bright)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BrightVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContrastVal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar Contrast;
        private System.Windows.Forms.TrackBar Bright;
        private TxEstudioApplication.Custom_Controls.ImageSelectionBox pictureControl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button Apply;
        private System.Windows.Forms.Button Reset;
        private System.Windows.Forms.Button Close;
        private System.Windows.Forms.NumericUpDown BrightVal;
        private System.Windows.Forms.NumericUpDown ContrastVal;
    }
}