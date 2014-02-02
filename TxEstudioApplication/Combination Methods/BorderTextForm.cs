using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel.Operators;

namespace TxEstudioApplication
{
    public partial class BorderTextForm : Form
    {
        BorderMatrix border;
        Graphics g;
        Brush br = new SolidBrush(Color.Black);

        public BorderTextForm(BorderMatrix border)
        {
            this.border= border;
            InitializeComponent();
            g = pictureBoxBorder.CreateGraphics();

            this.Width = border.MatrixB.GetLength(0)+5;
            this.Height = border.MatrixB.GetLength(1)+ 30;

            Brush br = new SolidBrush(Color.Black);

            for (int i = 0; i < border.MatrixB.GetLength(0); i++)
                for (int j = 0; j < border.MatrixB.GetLength(1); j++)
                {
                    if (border.MatrixB[i, j] == 1)
                        g.FillRectangle(br, i, j, 1, 1);
                }
            this.Refresh();     
        }

        private void pictureBoxBorder_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            for (int i = 0; i < border.MatrixB.GetLength(0); i++)
                for (int j = 0; j < border.MatrixB.GetLength(1); j++)
                {
                    if (border.MatrixB[i, j] == 1)
                        g.FillRectangle(br, i, j, 1, 1);
                }
                    
        }

    }
}