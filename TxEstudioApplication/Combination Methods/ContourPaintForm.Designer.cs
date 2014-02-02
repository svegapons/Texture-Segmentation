namespace TxEstudioApplication
{
    partial class ContourPaintForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContourPaintForm));
            this.pb = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.bt_paintBorde = new System.Windows.Forms.Button();
            this.Aceptar_borde = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pb)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(0, 0);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(341, 359);
            this.pb.TabIndex = 0;
            this.pb.TabStop = false;
            this.pb.MouseLeave += new System.EventHandler(this.pb_MouseLeave);
            this.pb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_MouseDown);
            this.pb.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pb_MouseMove);
            this.pb.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureControl_Paint);
            this.pb.Resize += new System.EventHandler(this.pb_Resize);
            this.pb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pb_MouseUp);
            this.pb.MouseEnter += new System.EventHandler(this.pb_MouseEnter);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.bt_paintBorde);
            this.splitContainer1.Panel1.Controls.Add(this.Aceptar_borde);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pb);
            this.splitContainer1.Size = new System.Drawing.Size(424, 362);
            this.splitContainer1.SplitterDistance = 76;
            this.splitContainer1.TabIndex = 1;
            // 
            // bt_paintBorde
            // 
            this.bt_paintBorde.Location = new System.Drawing.Point(4, 12);
            this.bt_paintBorde.Name = "bt_paintBorde";
            this.bt_paintBorde.Size = new System.Drawing.Size(58, 23);
            this.bt_paintBorde.TabIndex = 1;
            this.bt_paintBorde.Text = "Preview";
            this.bt_paintBorde.UseVisualStyleBackColor = true;
            this.bt_paintBorde.Click += new System.EventHandler(this.bt_paintBorde_Click);
            // 
            // Aceptar_borde
            // 
            this.Aceptar_borde.Location = new System.Drawing.Point(4, 41);
            this.Aceptar_borde.Name = "Aceptar_borde";
            this.Aceptar_borde.Size = new System.Drawing.Size(59, 23);
            this.Aceptar_borde.TabIndex = 1;
            this.Aceptar_borde.Text = "Ok";
            this.Aceptar_borde.UseVisualStyleBackColor = true;
            this.Aceptar_borde.Click += new System.EventHandler(this.Aceptar_borde_Click);
            // 
            // ContourPaintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 362);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ContourPaintForm";
            this.Text = "ContourPaintForm";
            ((System.ComponentModel.ISupportInitialize)(this.pb)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pb;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button Aceptar_borde;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button bt_paintBorde;
    }
}