using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioApplication.Interfaces;
using TxEstudioKernel;
using TxEstudioKernel.Operators;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace TxEstudioApplication
{
   
    public partial class ContourPaintForm : TxAppForm
    {
        Bitmap backImage;
        Graphics gr;
        Pen pen = new Pen(Color.Blue);
        Brush bObj = new SolidBrush(Color.Blue);
        bool pushed = false;
        int Xbefore, Ybefore;
        List<List<Point>> points;
        BorderMatrix border;
        int sinceX, sinceY = 0;


        public ContourPaintForm( int width, int heigth)
        {
            InitializeComponent();
            gr = pb.CreateGraphics();
            this.Width = width;
            this.Height = heigth;

            this.pb.Width = width;
            this.pb.Height = heigth;  
      
            points = new List<List<Point>>();
        }

        public Bitmap BackImage
        {
            get { return backImage; }
            set
            {
                backImage = value;
                pb.Image = value;
            }
        }


        public BorderMatrix Border
        {
            get { return BorderTrans(border); }
            set { border = value; }
        }

        private BorderMatrix BorderTrans(BorderMatrix border)
        {
            BorderMatrix transpuesta = new BorderMatrix(border.MatrixB.GetLength(1), border.MatrixB.GetLength(0));
            for(int i=0; i<border.MatrixB.GetLength(0);i++)
                for (int j = 0; j < border.MatrixB.GetLength(1); j++)
                {
                    transpuesta[j, i] = border[i, j];
                }
            return transpuesta;
        }


        private void pb_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.X >= 0 && e.Y >= 0 && e.X < backImage.Width && e.Y < backImage.Height && (e.Button == MouseButtons.Left))
            {
                points.Add(new List<Point>());
                points[points.Count - 1].Add(new Point(e.X, e.Y));

                if ((e.Button == MouseButtons.Left) && !pushed)
                {
                    gr.FillRectangle(bObj, e.X, e.Y, 1, 1);

                }
                Xbefore = e.X;
                Ybefore = e.Y;
                pushed = true;
            }

        }

        private void pb_MouseMove(object sender, MouseEventArgs e)
        {
            toolTip1.SetToolTip(pb, "" + e.X + "," + e.Y);

            if (pushed && e.X >= 0 && e.Y >= 0 && e.X < backImage.Width && e.Y < backImage.Height && e.Button == MouseButtons.Left)
            {
                points[points.Count - 1].Add(new Point(e.X, e.Y));

                gr.FillRectangle(bObj, e.X, e.Y, 1, 1);
                gr.DrawLine(pen, Xbefore, Ybefore, e.X, e.Y);
                Xbefore = e.X;
                Ybefore = e.Y;
            }
        }

        private void pb_MouseUp(object sender, MouseEventArgs e)
        {
            pushed = false;
        }

        private void pb_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Cross;
        }

        private void pb_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void pb_Resize(object sender, EventArgs e)
        {
        }

        private void pictureControl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Point aux = new Point();
            bool before = false;

            for (int i = 0; i < points.Count; i++)
            {
                before = false;
                foreach (Point p in points[i])
                {
                    g.FillRectangle(bObj, p.X, p.Y, 1, 1);
                    if (before)
                    {
                        g.DrawLine(pen, aux, p);
                    }
                    aux.X = p.X;
                    aux.Y = p.Y;
                    before = true;

                }
            }
        }
        private void Aceptar_borde_Click(object sender, EventArgs e)
        {
            border = new BorderMatrix(backImage.Width, backImage.Height);
            foreach (List<Point> p in points)
            {
                foreach (Point point in p)
                    border[point.X, point.Y] = 1;
            }
            FillMatrix(points);
            this.Hide();
            this.DialogResult = DialogResult.OK;
        }

        #region Border manually generated

        
        public void FillMatrix(List<List<Point>> points)
        {
            if (points != null)
            {
                foreach (List<Point> p in points)
                {
                    for (int i = 0; i < p.Count - 2; i++)
                        JointTwoPoints(p[i], p[i + 1]);
                }
            }
        }
        public void JointTwoPoints(Point first, Point second)
        {
            sinceX = first.X;
            sinceY = first.Y;

            int Xvalue = Math.Abs(second.X - sinceX);
            int Yvalue = Math.Abs(second.Y - sinceY);

            if (Xvalue != 0 || Yvalue != 0)
            {
                double tan = Xvalue > Yvalue ? Math.Round((double)(double)((Yvalue + 0.0) / (Xvalue + 0.0)), 3) : Math.Round((double)((Xvalue + 0.0) / (Yvalue + 0.0)), 2);

                if (Yvalue < Xvalue)
                {
                    /// moverse horizontalmente

                    if (first.X < second.X && first.Y < second.Y)
                    {
                        MoveFirstRightThenDown(first, second, tan);
                    }
                    else if (first.X > second.X && first.Y < second.Y)
                    {
                        MoveFirstLeftThenDown(first, second, tan);
                    }
                    else if (first.X < second.X && first.Y > second.Y)
                    {
                        MoveFirstRightThenUp(first, second, tan);
                    }
                    else if (first.X > second.X && first.Y > second.Y)
                    {
                        MoveFirstLeftThenUp(first, second, tan);
                    }
                    else if (first.Y == second.Y)
                    {
                        if (first.X < second.X)
                        {
                            MoveAllRight(first, second, tan);

                        }
                        else
                        {
                            MoveAllLeft(first, second, tan);
                        }
                    }
                    else
                        throw new Exception("The case is odd");

                }
                else
                {
                    /// moverse verticalmente

                    if (first.X < second.X && first.Y < second.Y)
                    {
                        MoveFirstDownThenRight(first, second, tan);
                    }
                    else if (first.X > second.X && first.Y < second.Y)
                    {
                        MoveFirstDownThenLeft(first, second, tan);
                    }
                    else if (first.X < second.X && first.Y > second.Y)
                    {
                        MoveFirstUpThenRight(first, second, tan);
                    }
                    else if (first.X > second.X && first.Y > second.Y)
                    {
                        MoveFirstUpThenLeft(first, second, tan);
                    }
                    else if (first.X == second.X)
                    {
                        if (first.Y < second.Y)
                        {
                            MoveAllDown(first, second, tan);
                        }
                        else
                        {
                            MoveAllUp(first, second, tan);
                        }
                    }
                    else
                        throw new Exception("The case is odd");

                }

            }
        }
        private void MoveFirstRightThenDown(Point first, Point second, double tan)
        {
            double carry = 0;
            sinceX = first.X;
            sinceY = first.Y;

            while ((sinceX < border.MatrixB.GetLength(0) && sinceY < border.MatrixB.GetLength(1)) && (sinceX < second.X || sinceY < second.Y))// ((sinceX != second.X - 1 && sinceY != second.Y - 1) && (sinceX != second.X - 1 && sinceY == second.Y) && (sinceX != second.X - 1 && sinceY != second.Y + 1) && (sinceX != second.X && sinceY != second.Y - 1) && (sinceX != second.X && sinceY != second.Y + 1) && (sinceX != second.X + 1 && sinceY != second.Y - 1) && (sinceX != second.X + 1 && sinceY != second.Y) && (sinceX != second.X + 1 && sinceY != second.Y + 1)))
            {
                carry += tan;
                if (carry < 1)
                {
                    MoveToRight(sinceX, sinceY);
                    sinceX += 1;
                }
                else
                {
                    carry = carry - 1;
                    MoveToRightDown(sinceX, sinceY);
                    sinceX += 1;
                    sinceY += 1;
                }
            }
        }
        private void MoveFirstLeftThenDown(Point first, Point second, double tan)
        {
            double carry = 0;
            sinceX = first.X;
            sinceY = first.Y;

            while ((sinceX > 0) && (sinceY < border.MatrixB.GetLength(1)) && (sinceY <= second.Y) && (sinceX >= second.X)) //((sinceX != second.X - 1 && sinceY != second.Y - 1) && (sinceX != second.X - 1 && sinceY == second.Y) && (sinceX != second.X - 1 && sinceY != second.Y + 1) && (sinceX != second.X && sinceY != second.Y - 1) && (sinceX != second.X && sinceY != second.Y + 1) && (sinceX != second.X + 1 && sinceY != second.Y - 1) && (sinceX != second.X + 1 && sinceY != second.Y) && (sinceX != second.X + 1 && sinceY != second.Y + 1)))
            {
                carry += tan;
                if (carry < 1)
                {
                    MoveToLeft(sinceX, sinceY);
                    sinceX -= 1;
                }
                else
                {
                    carry = carry - 1;
                    MoveToLeftDown(sinceX, sinceY);
                    sinceX -= 1;
                    sinceY += 1;
                }
            }
        }
        private void MoveFirstRightThenUp(Point first, Point second, double tan)
        {
            double carry = 0;
            sinceX = first.X;
            sinceY = first.Y;

            while ((sinceX >= 0 && sinceY >= 0) && (sinceX < border.MatrixB.GetLength(0)) && (sinceX <= second.X && sinceY >= second.Y)) //((sinceX != second.X - 1 && sinceY != second.Y - 1) && (sinceX != second.X - 1 && sinceY == second.Y) && (sinceX != second.X - 1 && sinceY != second.Y + 1) && (sinceX != second.X && sinceY != second.Y - 1) && (sinceX != second.X && sinceY != second.Y + 1) && (sinceX != second.X + 1 && sinceY != second.Y - 1) && (sinceX != second.X + 1 && sinceY != second.Y) && (sinceX != second.X + 1 && sinceY != second.Y + 1)))
            {
                carry += tan;
                if (carry < 1)
                {
                    MoveToRight(sinceX, sinceY);
                    sinceX += 1;
                }
                else
                {
                    carry = carry - 1;
                    MoveToRightUp(sinceX, sinceY);
                    sinceX += 1;
                    sinceY -= 1;
                }
            }
        }
        private void MoveFirstLeftThenUp(Point first, Point second, double tan)
        {
            double carry = 0;
            sinceX = first.X;
            sinceY = first.Y;

            while ((sinceX >= 0 && sinceY >= 0) && (sinceX >= second.X || sinceY >= second.Y)) //((sinceX != second.X - 1 && sinceY != second.Y - 1) && (sinceX != second.X - 1 && sinceY == second.Y) && (sinceX != second.X - 1 && sinceY != second.Y + 1) && (sinceX != second.X && sinceY != second.Y - 1) && (sinceX != second.X && sinceY != second.Y + 1) && (sinceX != second.X + 1 && sinceY != second.Y - 1) && (sinceX != second.X + 1 && sinceY != second.Y) && (sinceX != second.X + 1 && sinceY != second.Y + 1)))
            {
                carry += tan;
                if (carry < 1)
                {
                    MoveToLeft(sinceX, sinceY);
                    sinceX -= 1;
                }
                else
                {
                    carry = carry - 1;
                    MoveToLeftUp(sinceX, sinceY);
                    sinceX -= 1;
                    sinceY -= 1;
                }
            }
        }
        private void MoveAllRight(Point first, Point second, double tan)
        {
            sinceX = first.X;
            sinceY = first.Y;

            while ((sinceX < border.MatrixB.GetLength(0) && sinceY < border.MatrixB.GetLength(1)) && (sinceX < second.X)) //( (sinceX != second.X - 1 && sinceY != second.Y - 1) && (sinceX != second.X - 1 && sinceY == second.Y) && (sinceX != second.X - 1 && sinceY != second.Y + 1) && (sinceX != second.X && sinceY != second.Y - 1) && (sinceX != second.X && sinceY != second.Y + 1) && (sinceX != second.X + 1 && sinceY != second.Y - 1) && (sinceX != second.X + 1 && sinceY != second.Y) && (sinceX != second.X + 1 && sinceY != second.Y + 1)))
            {
                MoveToRight(sinceX, sinceY);
                sinceX += 1;
            }
        }
        private void MoveAllLeft(Point first, Point second, double tan)
        {
            sinceX = first.X;
            sinceY = first.Y;

            while ((sinceX > 0 && sinceY >= 0) && (sinceX > second.X))//((sinceX != second.X - 1 && sinceY != second.Y - 1) && (sinceX != second.X - 1 && sinceY == second.Y) && (sinceX != second.X - 1 && sinceY != second.Y + 1) && (sinceX != second.X && sinceY != second.Y - 1) && (sinceX != second.X && sinceY != second.Y + 1) && (sinceX != second.X + 1 && sinceY != second.Y - 1) && (sinceX != second.X + 1 && sinceY != second.Y) && (sinceX != second.X + 1 && sinceY != second.Y + 1)))
            {
                MoveToLeft(sinceX, sinceY);
                sinceX -= 1;
            }
        }
        private void MoveFirstDownThenRight(Point first, Point second, double tan)
        {
            double carry = 0;
            sinceX = first.X;
            sinceY = first.Y;

            while ((sinceX < border.MatrixB.GetLength(0) && sinceY < border.MatrixB.GetLength(1)) && (sinceX <= second.X || sinceY <= second.Y)) //&& ((sinceX != second.X - 1 && sinceY != second.Y - 1) && (sinceX != second.X - 1 && sinceY == second.Y) && (sinceX != second.X - 1 && sinceY != second.Y + 1) && (sinceX != second.X && sinceY != second.Y - 1) && (sinceX != second.X && sinceY != second.Y + 1) && (sinceX != second.X + 1 && sinceY != second.Y - 1) && (sinceX != second.X + 1 && sinceY != second.Y) && (sinceX != second.X + 1 && sinceY != second.Y + 1)))
            {
                carry += tan;
                if (carry < 1)
                {
                    MoveDown(sinceX, sinceY);
                    sinceY += 1;
                }
                else
                {
                    carry = carry - 1;
                    MoveToRightDown(sinceX, sinceY);
                    sinceX += 1;
                    sinceY += 1;
                }
            }
        }
        private void MoveFirstDownThenLeft(Point first, Point second, double tan)
        {
            double carry = 0;
            sinceX = first.X;
            sinceY = first.Y;

            while ((sinceX >= 0 && sinceY < border.MatrixB.GetLength(1)) && (sinceX >= second.X || sinceY <= second.Y)) //((sinceX != second.X - 1 && sinceY != second.Y - 1) && (sinceX != second.X - 1 && sinceY == second.Y) && (sinceX != second.X - 1 && sinceY != second.Y + 1) && (sinceX != second.X && sinceY != second.Y - 1) && (sinceX != second.X && sinceY != second.Y + 1) && (sinceX != second.X + 1 && sinceY != second.Y - 1) && (sinceX != second.X + 1 && sinceY != second.Y) && (sinceX != second.X + 1 && sinceY != second.Y + 1)))
            {
                carry += tan;
                if (carry < 1)
                {
                    MoveDown(sinceX, sinceY);
                    sinceY += 1;
                }
                else
                {
                    carry = carry - 1;
                    MoveToLeftDown(sinceX, sinceY);
                    sinceX -= 1;
                    sinceY += 1;
                }
            }
        }
        private void MoveFirstUpThenRight(Point first, Point second, double tan)
        {
            double carry = 0;
            sinceX = first.X;
            sinceY = first.Y;

            while ((sinceX >= 0 && sinceY >= 0) && (sinceX < border.MatrixB.GetLength(0)) && (sinceX <= second.X || sinceY >= second.Y))//((sinceX != second.X - 1 && sinceY != second.Y - 1) && (sinceX != second.X - 1 && sinceY == second.Y) && (sinceX != second.X - 1 && sinceY != second.Y + 1) && (sinceX != second.X && sinceY != second.Y - 1) && (sinceX != second.X && sinceY != second.Y + 1) && (sinceX != second.X + 1 && sinceY != second.Y - 1) && (sinceX != second.X + 1 && sinceY != second.Y) && (sinceX != second.X + 1 && sinceY != second.Y + 1)))
            {
                carry += tan;
                if (carry < 1)
                {
                    MoveUp(sinceX, sinceY);
                    sinceY -= 1;
                }
                else
                {
                    carry = carry - 1;
                    MoveToRightUp(sinceX, sinceY);
                    sinceX += 1;
                    sinceY -= 1;
                }
            }
        }
        private void MoveFirstUpThenLeft(Point first, Point second, double tan)
        {
            double carry = 0;
            sinceX = first.X;
            sinceY = first.Y;

            while ((sinceX >= 0 && sinceY >= 0) && (sinceX >= second.X || sinceY >= second.Y))//((sinceX != second.X - 1 && sinceY != second.Y - 1) && (sinceX != second.X - 1 && sinceY == second.Y) && (sinceX != second.X - 1 && sinceY != second.Y + 1) && (sinceX != second.X && sinceY != second.Y - 1) && (sinceX != second.X && sinceY != second.Y + 1) && (sinceX != second.X + 1 && sinceY != second.Y - 1) && (sinceX != second.X + 1 && sinceY != second.Y) && (sinceX != second.X + 1 && sinceY != second.Y + 1)))
            {
                carry += tan;
                if (carry < 1)
                {
                    MoveUp(sinceX, sinceY);
                    sinceY -= 1;
                }
                else
                {
                    carry = carry - 1;
                    MoveToLeftUp(sinceX, sinceY);
                    sinceX -= 1;
                    sinceY -= 1;
                }
            }
        }
        private void MoveAllDown(Point first, Point second, double tan)
        {
            sinceX = first.X;
            sinceY = first.Y;

            while ((sinceY < border.MatrixB.GetLength(1)) && (sinceY < second.Y))  //((sinceX != second.X - 1 && sinceY != second.Y - 1) && (sinceX != second.X - 1 && sinceY == second.Y) && (sinceX != second.X - 1 && sinceY != second.Y + 1) && (sinceX != second.X && sinceY != second.Y - 1) && (sinceX != second.X && sinceY != second.Y + 1) && (sinceX != second.X + 1 && sinceY != second.Y - 1) && (sinceX != second.X + 1 && sinceY != second.Y) && (sinceX != second.X + 1 && sinceY != second.Y + 1)))
            {
                MoveDown(sinceX, sinceY);
                sinceY += 1;
            }
        }
        private void MoveAllUp(Point first, Point second, double tan)
        {
            sinceX = first.X;
            sinceY = first.Y;

            while ((sinceX >= 0 && sinceY > 0) && (sinceY > second.Y)) //((sinceX != second.X - 1 && sinceY != second.Y - 1) && (sinceX != second.X - 1 && sinceY == second.Y) && (sinceX != second.X - 1 && sinceY != second.Y + 1) && (sinceX != second.X && sinceY != second.Y - 1) && (sinceX != second.X && sinceY != second.Y + 1) && (sinceX != second.X + 1 && sinceY != second.Y - 1) && (sinceX != second.X + 1 && sinceY != second.Y) && (sinceX != second.X + 1 && sinceY != second.Y + 1)))
            {
                MoveUp(sinceX, sinceY);
                sinceY -= 1;
            }
        }

        /// <summary>
        /// Auxiliare Methods
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>

        private void MoveToRight(int x, int y)
        {
            if ((x + 1) <= border.MatrixB.GetLength(0) - 1)
            {
                border[x + 1, y] = 1;

            }

        }
        private void MoveToLeft(int x, int y)
        {
            if ((x - 1) >= 0)
            {
                border[x - 1, y] = 1;

            }

        }
        private void MoveUp(int x, int y)
        {
            if ((y - 1) >= 0)
            {
                border[x, y - 1] = 1;

            }

        }
        private void MoveDown(int x, int y)
        {
            if ((y + 1) <= (border.MatrixB.GetLength(1) - 1))
            {
                border[x, y + 1] = 1;

            }

        }
        private void MoveToRightDown(int x, int y)
        {
            if ((x + 1) <= border.MatrixB.GetLength(0) - 1 && (y + 1) <= (border.MatrixB.GetLength(1) - 1))
            {
                border[x + 1, y + 1] = 1;

            }

        }
        private void MoveToLeftDown(int x, int y)
        {
            if ((x - 1) >= 0 && (y + 1) <= (border.MatrixB.GetLength(1) - 1))
            {
                border[x - 1, y + 1] = 1;

            }
        }
        private void MoveToRightUp(int x, int y)
        {
            if ((x + 1) <= border.MatrixB.GetLength(0) - 1 && (y - 1) >= 0)
            {
                border[x + 1, y - 1] = 1;

            }
        }
        private void MoveToLeftUp(int x, int y)
        {
            if ((x - 1) >= 0 && (y - 1) >= 0)
            {
                border[x - 1, y - 1] = 1;

            }
        }

        #endregion

        private void bt_paintBorde_Click(object sender, EventArgs e)
        {
            
            border = new BorderMatrix(backImage.Width, backImage.Height);
            foreach (List<Point> p in points)
            {
                foreach (Point point in p)
                    border[point.X, point.Y] = 1;
            }
            FillMatrix(points);
            BorderTextForm borderTest = new BorderTextForm(border);
            borderTest.Show();

        }

    }

}