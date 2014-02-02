using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel.Operators.ImageQualityAssessment;
using TxEstudioKernel;

namespace TxEstudioApplication.Dialogs
{
    public partial class QualityDialog : TxAppForm
    {
        public QualityDialog()
        {
            InitializeComponent();
            imageBox2.ImageFilter += new TxEstudioApplication.Custom_Controls.FilterImageDelegate(Filter);
        }


        

        public ImageQuality QualityAssessment
        {
            get { return (ImageQuality)qualityViewer.AlgorithmInstance; }
            set {  qualityViewer.AlgorithmInstance = value; }
        }


        public override void SetEnvironment(Environment env)
        {
            base.SetEnvironment(env);
            imageBox1.LoadEnvironment(env);
            imageBox2.LoadEnvironment(env);
        }

        private bool Filter(TxImage image)
        {
            return imageBox1.SelectedImage.Size == image.Size;
        }

        private void imageBox1_ItemChanged(object sender, EventArgs e)
        {
            imageBox2.LoadEnvironment(env);
            
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            ImageQuality qualityAssessment  = (ImageQuality)qualityViewer.AlgorithmInstance;
            valueLabel.Text = qualityAssessment.Error(imageBox1.SelectedImage, imageBox2.SelectedImage).ToString();
            mapBox.Image = qualityAssessment.Mapa.ToImage().ToBitamp();
            
        }

       
    }
}