using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TxEstudioApplication.Dialogs
{
    public partial class Selection_Result : Form
    {
        string path;
        string nombre;

        public string Path
        {
            get { return path; }
            //set { path = value; }
        }

        public Selection_Result()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "Archivo de texto(*.txt)|*.txt";
            path = null;
        }

        public void AddDescriptor(string name)
        {
            tb_result.Text += name+'\r'+'\n';
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog1.FileName;
 
            }
        }
    }
}