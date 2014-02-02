using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel;

namespace TxEstudioApplication.Dialogs
{
    public partial class BrightContrastDialog :TxAppForm
    {
        private TxImage image;
        private Bitmap bmp;
        private TxBrightContrast bc;

        public BrightContrastDialog()
        {
            InitializeComponent();
        }
       
        public TxImage Image 
        {
            get { return image; }
            set { 
                     image = value;
                     bc = new TxBrightContrast(image); 
                     bmp = bc.CurrentBitmap ;
                     pictureControl.ViewBitmap = bmp;
                     Bright.Value = 0;
                     Contrast.Value = 0;
                     BrightVal.Value = 0;
                     BrightVal.Value = 0;
                }
        }
        public override void SetEnvironment(Environment env)
        {
            base.SetEnvironment(env);
            pictureControl.LoadEnvironment(env);
        }
        private void ImageChanged(object sender, EventArgs e) 
        {
            this.Image = pictureControl.SelectedImage;
        } 
        private void Bright_Scroll(object sender, EventArgs e)
        {
           BrightVal.Value = Bright.Value;
           bc.AdjustBright( Bright.Value );
           pictureControl.ViewBitmap = bc.CurrentBitmap; 
        }

        private void Contrast_Scroll(object sender, EventArgs e)
        {   
            ContrastVal.Value = Contrast.Value;
            bc.AdjustContrast( Contrast.Value);
            pictureControl.ViewBitmap = bc.CurrentBitmap; 
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            ImageHolderForm newForm = new ImageHolderForm();
            newForm.Image = new TxImage( bc.CurrentBitmap );
            newForm.ImageName = pictureControl.SelectedExposer.ImageName + "+" + "BC" + "(" + bc.Bright+"," + bc.Contrast + ")" ;
            env.OpenWindow(newForm);
            pictureControl.AddImageForm(newForm);
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            bc.Reset();
            
            Bright.Value = 0;
            Contrast.Value = 0;
            BrightVal.Value = 0;
            ContrastVal.Value = 0;

            pictureControl.ViewBitmap = bc.CurrentBitmap; 
        }

        private void Close_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void BrightVal_ValueChanged(object sender, EventArgs e)
        {
            if (BrightVal.Focused) 
            { 
                int val = (int)BrightVal.Value;

                Bright.Value = val;
                bc.AdjustBright(val);
               
                pictureControl.ViewBitmap = bc.CurrentBitmap; 
            }
        }

        private void ContrastVal_ValueChanged(object sender, EventArgs e)
        {
            if (ContrastVal.Focused) 
            {
                int val = (int)ContrastVal.Value;

                Contrast.Value = val;
                bc.AdjustContrast(val);

                pictureControl.ViewBitmap = bc.CurrentBitmap; 
            }
        } 
    }
}