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
    public partial class ComposeDialogBox : TxAppForm
    {
        public ComposeDialogBox()
        {
            InitializeComponent();
        }

        public override void SetEnvironment(Environment env)
        {
            base.SetEnvironment(env);
            red.ImageFilter = new TxEstudioApplication.Custom_Controls.FilterImageDelegate(RedFilter);
            green.ImageFilter = new TxEstudioApplication.Custom_Controls.FilterImageDelegate(Green_Blue_Filter);
            blue.ImageFilter = new TxEstudioApplication.Custom_Controls.FilterImageDelegate(Green_Blue_Filter);
            red.LoadEnvironment(env);
            
        }

        private void red_ItemChanged(object sender, EventArgs e)
        {
            green.LoadEnvironment(env);
            blue.LoadEnvironment(env);
        }

        private bool RedFilter(TxImage image)
        {
            return image.ImageFormat == TxImageFormat.GrayScale;
        }

        private bool Green_Blue_Filter(TxImage image)
        {
            return image.ImageFormat == TxImageFormat.GrayScale && image.Size == red.SelectedImage.Size;
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            ImageHolderForm newForm = new ImageHolderForm();
            newForm.Image = new TxColorComposition().Process(red.SelectedImage, green.SelectedImage, blue.SelectedImage);
            newForm.ImageName =string.Format("CC({0},{1},{2})", red.SelectedExposer.ImageName, green.SelectedExposer.ImageName , blue.SelectedExposer.ImageName); 
            env.OpenWindow(newForm);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}