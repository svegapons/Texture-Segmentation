using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel.Operators;
using TxEstudioKernel;
using System.Drawing.Imaging;
using System.IO;

namespace TxEstudioApplication
{
    public partial class DialogoYadi : Form
    {
        public DialogoYadi()
        {
            InitializeComponent();
        }

        GaussianFilter homo = new GaussianFilter();
        IsotropicFilter iso = new IsotropicFilter();
        Anisotropic ani = new Anisotropic();
        Multi_Escale_Retinex mer = new Multi_Escale_Retinex();
        string homoStr = "";
        string aniStr = "";
        string isoStr = "";
        string merStr = "";

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                TxImage aux = null;
                Bitmap bmp = null;
                 Directory.CreateDirectory(folderBrowser.SelectedPath + "\\Anisotropic\\");
                 aniStr = folderBrowser.SelectedPath + "\\Anisotropic\\";
                 Directory.CreateDirectory(folderBrowser.SelectedPath + "\\Isotropic\\");
                isoStr = folderBrowser.SelectedPath + "\\Isotropic\\";
                Directory.CreateDirectory(folderBrowser.SelectedPath + "\\Homomorfic\\");
                homoStr = folderBrowser.SelectedPath + "\\Homomorfic\\";
                Directory.CreateDirectory(folderBrowser.SelectedPath + "\\Muli_Escala_Retinex\\");
                merStr = folderBrowser.SelectedPath + "\\Muli_Escala_Retinex\\";


                foreach( Form form in ((MainForm)this.Owner).MdiChildren)
                    if (form is ImageHolderForm)
                    {
                        bmp = ((ImageHolderForm)form).Bitmap;
                      
                        aux = ((ImageHolderForm)form).Image;
                        if (ch_ani.Checked)
                            ani.Process(aux).Save(aniStr+((ImageHolderForm)form).ImageName+".jpg");
                        if(ch_homo.Checked)
                            homo.Process(aux).Save(homoStr + ((ImageHolderForm)form).ImageName + ".jpg");
                        if (ch_iso.Checked)
                            iso.Process(aux).Save(isoStr + ((ImageHolderForm)form).ImageName +".jpg");
                        if (ch_mer.Checked)
                            mer.Process(aux).Save(merStr + ((ImageHolderForm)form).ImageName + ".jpg");
                    }
               
            }
        }
    }
}