namespace TxEstudioApplication
{
    partial class SegmentationResultForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SegmentationResultForm));
            this.pictureControl = new TxEstudioApplication.Custom_Controls.PictureControl();
            this.SuspendLayout();
            // 
            // pictureControl
            // 
            this.pictureControl.Bitmap = null;
            this.pictureControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureControl.Location = new System.Drawing.Point(0, 0);
            this.pictureControl.Name = "pictureControl";
            this.pictureControl.Size = new System.Drawing.Size(534, 334);
            this.pictureControl.TabIndex = 0;
            this.pictureControl.Zoom = 1F;
            this.pictureControl.MouseOverImage += new TxEstudioApplication.MoveOverImageEventHandler(this.pictureControl_MouseOverImage);
            // 
            // SegmentationResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 334);
            this.Controls.Add(this.pictureControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SegmentationResultForm";
            this.Text = "SegmentationResultForm";
            this.ResumeLayout(false);

        }

        #endregion

        
        private TxEstudioApplication.Custom_Controls.PictureControl pictureControl;
    }
}