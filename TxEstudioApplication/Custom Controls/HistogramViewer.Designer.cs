namespace TxEstudioApplication.Custom_Controls
{
    partial class HistogramViewer
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
            this.label1 = new System.Windows.Forms.Label();
            this.min_Label = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.stdv_Label = new System.Windows.Forms.Label();
            this.max_Label = new System.Windows.Forms.Label();
            this.mean_Label = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.buttons_Strip = new System.Windows.Forms.ToolStrip();
            this.blue_Button = new System.Windows.Forms.ToolStripButton();
            this.green_Button = new System.Windows.Forms.ToolStripButton();
            this.red_Button = new System.Windows.Forms.ToolStripButton();
            this.grayScale_Button = new System.Windows.Forms.ToolStripButton();
            this.label2 = new System.Windows.Forms.Label();
            this.minf_Label = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.maxf_Label = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.freq_Label = new System.Windows.Forms.Label();
            this.value_UpDown = new System.Windows.Forms.NumericUpDown();
            this.histogarmCanvas1 = new TxEstudioApplication.Custom_Controls.HistogramCanvas();
            this.label6 = new System.Windows.Forms.Label();
            this.buttons_Strip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.value_UpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 198);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Minimum:";
            // 
            // min_Label
            // 
            this.min_Label.AutoSize = true;
            this.min_Label.Location = new System.Drawing.Point(75, 198);
            this.min_Label.Name = "min_Label";
            this.min_Label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.min_Label.Size = new System.Drawing.Size(0, 13);
            this.min_Label.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(125, 225);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Stdv:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(125, 198);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Maximum:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 225);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Mean:";
            // 
            // stdv_Label
            // 
            this.stdv_Label.AutoSize = true;
            this.stdv_Label.Location = new System.Drawing.Point(203, 225);
            this.stdv_Label.Name = "stdv_Label";
            this.stdv_Label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.stdv_Label.Size = new System.Drawing.Size(0, 13);
            this.stdv_Label.TabIndex = 10;
            // 
            // max_Label
            // 
            this.max_Label.AutoSize = true;
            this.max_Label.Location = new System.Drawing.Point(203, 198);
            this.max_Label.Name = "max_Label";
            this.max_Label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.max_Label.Size = new System.Drawing.Size(0, 13);
            this.max_Label.TabIndex = 11;
            // 
            // mean_Label
            // 
            this.mean_Label.AutoSize = true;
            this.mean_Label.Location = new System.Drawing.Point(75, 225);
            this.mean_Label.Name = "mean_Label";
            this.mean_Label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.mean_Label.Size = new System.Drawing.Size(0, 13);
            this.mean_Label.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(236, 144);
            this.label9.Name = "label9";
            this.label9.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label9.Size = new System.Drawing.Size(28, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "255";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(4, 144);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "0";
            // 
            // buttons_Strip
            // 
            this.buttons_Strip.AutoSize = false;
            this.buttons_Strip.CanOverflow = false;
            this.buttons_Strip.GripMargin = new System.Windows.Forms.Padding(0);
            this.buttons_Strip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.buttons_Strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blue_Button,
            this.green_Button,
            this.red_Button,
            this.grayScale_Button});
            this.buttons_Strip.Location = new System.Drawing.Point(0, 0);
            this.buttons_Strip.Name = "buttons_Strip";
            this.buttons_Strip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.buttons_Strip.Size = new System.Drawing.Size(263, 25);
            this.buttons_Strip.TabIndex = 15;
            this.buttons_Strip.Text = "Buttons";
            this.buttons_Strip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.buttons_Strip_ItemClicked);
            // 
            // blue_Button
            // 
            this.blue_Button.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.blue_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.blue_Button.Image = global::TxEstudioApplication.Properties.Resources.blueBall;
            this.blue_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.blue_Button.Name = "blue_Button";
            this.blue_Button.Size = new System.Drawing.Size(23, 22);
            this.blue_Button.Text = "Blue";
            // 
            // green_Button
            // 
            this.green_Button.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.green_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.green_Button.Image = global::TxEstudioApplication.Properties.Resources.greenBall;
            this.green_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.green_Button.Name = "green_Button";
            this.green_Button.Size = new System.Drawing.Size(23, 22);
            this.green_Button.Text = "Green";
            // 
            // red_Button
            // 
            this.red_Button.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.red_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.red_Button.Image = global::TxEstudioApplication.Properties.Resources.redBall;
            this.red_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.red_Button.Name = "red_Button";
            this.red_Button.Size = new System.Drawing.Size(23, 22);
            this.red_Button.Text = "Red";
            // 
            // grayScale_Button
            // 
            this.grayScale_Button.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.grayScale_Button.Checked = true;
            this.grayScale_Button.CheckState = System.Windows.Forms.CheckState.Checked;
            this.grayScale_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.grayScale_Button.Image = global::TxEstudioApplication.Properties.Resources.grayBall;
            this.grayScale_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.grayScale_Button.Name = "grayScale_Button";
            this.grayScale_Button.Size = new System.Drawing.Size(23, 22);
            this.grayScale_Button.Text = "Gray scale";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(4, 247);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Min Freq:";
            // 
            // minf_Label
            // 
            this.minf_Label.AutoSize = true;
            this.minf_Label.Location = new System.Drawing.Point(74, 247);
            this.minf_Label.Name = "minf_Label";
            this.minf_Label.Size = new System.Drawing.Size(0, 13);
            this.minf_Label.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(125, 247);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Max Freq:";
            // 
            // maxf_Label
            // 
            this.maxf_Label.AutoSize = true;
            this.maxf_Label.Location = new System.Drawing.Point(200, 247);
            this.maxf_Label.Name = "maxf_Label";
            this.maxf_Label.Size = new System.Drawing.Size(0, 13);
            this.maxf_Label.TabIndex = 19;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(137, 158);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Freq:";
            // 
            // freq_Label
            // 
            this.freq_Label.AutoSize = true;
            this.freq_Label.Location = new System.Drawing.Point(194, 158);
            this.freq_Label.Name = "freq_Label";
            this.freq_Label.Size = new System.Drawing.Size(0, 13);
            this.freq_Label.TabIndex = 24;
            // 
            // value_UpDown
            // 
            this.value_UpDown.Location = new System.Drawing.Point(77, 156);
            this.value_UpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.value_UpDown.Name = "value_UpDown";
            this.value_UpDown.Size = new System.Drawing.Size(45, 20);
            this.value_UpDown.TabIndex = 25;
            this.value_UpDown.ValueChanged += new System.EventHandler(this.value_UpDown_ValueChanged);
            // 
            // histogarmCanvas1
            // 
            this.histogarmCanvas1.ChannelHistogram = null;
            this.histogarmCanvas1.Location = new System.Drawing.Point(4, 38);
            this.histogarmCanvas1.Name = "histogarmCanvas1";
            this.histogarmCanvas1.Pen = null;
            this.histogarmCanvas1.Size = new System.Drawing.Size(257, 100);
            this.histogarmCanvas1.TabIndex = 20;
            this.histogarmCanvas1.Text = "histogarmCanvas1";
            this.histogarmCanvas1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.histogarmCanvas1_MouseMove_1);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(28, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Value:";
            // 
            // HistogramViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.value_UpDown);
            this.Controls.Add(this.freq_Label);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.histogarmCanvas1);
            this.Controls.Add(this.maxf_Label);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.minf_Label);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttons_Strip);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.mean_Label);
            this.Controls.Add(this.max_Label);
            this.Controls.Add(this.stdv_Label);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.min_Label);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.MaximumSize = new System.Drawing.Size(263, 277);
            this.MinimumSize = new System.Drawing.Size(263, 277);
            this.Name = "HistogramViewer";
            this.Size = new System.Drawing.Size(263, 277);
            this.buttons_Strip.ResumeLayout(false);
            this.buttons_Strip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.value_UpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label min_Label;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label stdv_Label;
        private System.Windows.Forms.Label max_Label;
        private System.Windows.Forms.Label mean_Label;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ToolStrip buttons_Strip;
        private System.Windows.Forms.ToolStripButton blue_Button;
        private System.Windows.Forms.ToolStripButton green_Button;
        private System.Windows.Forms.ToolStripButton red_Button;
        private System.Windows.Forms.ToolStripButton grayScale_Button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label minf_Label;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label maxf_Label;
        private HistogramCanvas histogarmCanvas1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label freq_Label;
        private System.Windows.Forms.NumericUpDown value_UpDown;
        private System.Windows.Forms.Label label6;
    }
}
