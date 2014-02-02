using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel.Operators;

namespace TxEstudioApplication.Combination_Methods
{
    public partial class BorderFromImage : TxEstudioApplication.TxAppForm
    {
        public BorderFromImage()
        {
            InitializeComponent();
        }
        public override void SetEnvironment(Environment env)
        {
            base.SetEnvironment(env);
            imageSelectionBox.LoadEnvironment(env);
        }

        private BorderMatrix border;
        public BorderMatrix Border
        {
            get { return border; }
        }
	 

        private void bt_OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            border = BorderMatrix.FromImage(imageSelectionBox.SelectedImage);
            this.Hide();
        }
    }
}

