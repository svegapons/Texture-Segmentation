namespace TxEstudioApplication
{
    partial class DialogoYadi
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
            this.ch_ani = new System.Windows.Forms.CheckBox();
            this.ch_iso = new System.Windows.Forms.CheckBox();
            this.ch_homo = new System.Windows.Forms.CheckBox();
            this.ch_mer = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ch_mer);
            this.groupBox1.Controls.Add(this.ch_homo);
            this.groupBox1.Controls.Add(this.ch_iso);
            this.groupBox1.Controls.Add(this.ch_ani);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(213, 126);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtro";
            // 
            // ch_ani
            // 
            this.ch_ani.AutoSize = true;
            this.ch_ani.Location = new System.Drawing.Point(14, 25);
            this.ch_ani.Name = "ch_ani";
            this.ch_ani.Size = new System.Drawing.Size(78, 17);
            this.ch_ani.TabIndex = 1;
            this.ch_ani.Text = "Anisotropic";
            this.ch_ani.UseVisualStyleBackColor = true;
            // 
            // ch_iso
            // 
            this.ch_iso.AutoSize = true;
            this.ch_iso.Location = new System.Drawing.Point(14, 48);
            this.ch_iso.Name = "ch_iso";
            this.ch_iso.Size = new System.Drawing.Size(66, 17);
            this.ch_iso.TabIndex = 2;
            this.ch_iso.Text = "Isotropic";
            this.ch_iso.UseVisualStyleBackColor = true;
            // 
            // ch_homo
            // 
            this.ch_homo.AutoSize = true;
            this.ch_homo.Location = new System.Drawing.Point(14, 71);
            this.ch_homo.Name = "ch_homo";
            this.ch_homo.Size = new System.Drawing.Size(82, 17);
            this.ch_homo.TabIndex = 3;
            this.ch_homo.Text = "Homomorfic";
            this.ch_homo.UseVisualStyleBackColor = true;
            // 
            // ch_mer
            // 
            this.ch_mer.AutoSize = true;
            this.ch_mer.Location = new System.Drawing.Point(14, 94);
            this.ch_mer.Name = "ch_mer";
            this.ch_mer.Size = new System.Drawing.Size(128, 17);
            this.ch_mer.TabIndex = 4;
            this.ch_mer.Text = "Multi_Escala_Retinex";
            this.ch_mer.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(129, 158);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Da Click Aqui";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // b_ok
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 193);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Name = "b_ok";
            this.Text = "Dialogo para Yadi";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ch_mer;
        private System.Windows.Forms.CheckBox ch_homo;
        private System.Windows.Forms.CheckBox ch_iso;
        private System.Windows.Forms.CheckBox ch_ani;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
    }
}