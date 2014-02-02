namespace TxEstudioApplication.Custom_Controls
{
    partial class CustomGroupBox
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
            this.text_Label = new System.Windows.Forms.Label();
            this.checkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // text_Label
            // 
            this.text_Label.AutoEllipsis = true;
            this.text_Label.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.text_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.text_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_Label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.text_Label.Location = new System.Drawing.Point(0, 0);
            this.text_Label.Name = "text_Label";
            this.text_Label.Size = new System.Drawing.Size(290, 19);
            this.text_Label.TabIndex = 0;
            this.text_Label.Text = "label1";
            this.text_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBox
            // 
            this.checkBox.AutoEllipsis = true;
            this.checkBox.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.checkBox.Checked = true;
            this.checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.checkBox.Location = new System.Drawing.Point(0, 19);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(290, 19);
            this.checkBox.TabIndex = 1;
            this.checkBox.Text = "checkBox1";
            this.checkBox.UseVisualStyleBackColor = false;
            this.checkBox.Visible = false;
            // 
            // CustomGroupBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox);
            this.Controls.Add(this.text_Label);
            this.Name = "CustomGroupBox";
            this.Size = new System.Drawing.Size(290, 227);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label text_Label;
        private System.Windows.Forms.CheckBox checkBox;
    }
}
