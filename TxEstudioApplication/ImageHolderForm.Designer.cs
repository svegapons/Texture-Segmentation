namespace TxEstudioApplication
{
    partial class ImageHolderForm
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
                //ojo
                //image.Dispose();
                //bitmap.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageHolderForm));
            this.pictureControl = new TxEstudioApplication.Custom_Controls.PictureControl();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // pictureControl
            // 
            this.pictureControl.Bitmap = null;
            this.pictureControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureControl.Location = new System.Drawing.Point(0, 0);
            this.pictureControl.Name = "pictureControl";
            this.pictureControl.Size = new System.Drawing.Size(292, 266);
            this.pictureControl.TabIndex = 0;
            this.pictureControl.Zoom = 1F;
            this.pictureControl.MouseOverImage += new TxEstudioApplication.MoveOverImageEventHandler(this.pictureControl_MouseOverImage);
            // 
            // ImageHolderForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.pictureControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImageHolderForm";
            this.Text = "ImageHolderForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageHolderForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private TxEstudioApplication.Custom_Controls.PictureControl pictureControl;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;




    }
}