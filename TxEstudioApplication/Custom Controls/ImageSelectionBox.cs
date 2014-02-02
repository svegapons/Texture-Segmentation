using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TxEstudioApplication.Interfaces;
using TxEstudioKernel;

namespace TxEstudioApplication.Custom_Controls
{
    public delegate bool FilterImageDelegate(TxImage image);
    public delegate bool FilterExposerDelegate(IImageExposer exposer);
    public partial class ImageSelectionBox : UserControl
    {
        public ImageSelectionBox()
        {
            InitializeComponent();
            ImageFilter = new FilterImageDelegate(NoFilter);
            ExposerFilter = new FilterExposerDelegate(NoExposerFilter);
        }

        public FilterImageDelegate ImageFilter;
        public FilterExposerDelegate ExposerFilter;

        public virtual void LoadEnvironment(Environment env)
        {
            comboBox.Items.Clear();
            foreach (Form child in env.OpenedForms)
                if (child is IImageExposer)
                    if (ImageFilter((child as IImageExposer).Image) && ExposerFilter(child as IImageExposer))
                        comboBox.Items.Add(child);
            if (comboBox.Items.Count > 0)
                comboBox.SelectedIndex = 0;
        }

        private bool NoFilter(TxImage image)
        {
            return true;
        }

        private bool NoExposerFilter(IImageExposer exposer)
        {
            return true;
        }


        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox.Image = (comboBox.SelectedItem as IImageExposer).Bitmap;
            OnItemChanged(this, EventArgs.Empty);
        }

        public TxImage SelectedImage
        {
            get
            {
                if (comboBox.SelectedItem != null)
                    return (comboBox.SelectedItem as IImageExposer).Image;
                return null;
            }
        }

        public IImageExposer SelectedExposer
        {
            get
            {
                return comboBox.SelectedItem as IImageExposer;
            }
        }

        public event EventHandler ItemChanged;

        public Bitmap ViewBitmap
        {
            set { this.pictureBox.Image = value; }
        }

        protected void OnItemChanged(object sender, EventArgs e)
        {
            if (ItemChanged != null)
                ItemChanged(sender, e);
        }

        public void AddImageForm(Form child)
        {
            if (child is IImageExposer)
                comboBox.Items.Add(child);
        }


    }
}
