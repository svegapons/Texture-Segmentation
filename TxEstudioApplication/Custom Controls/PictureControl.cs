using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace TxEstudioApplication.Custom_Controls
{
    public partial class PictureControl : UserControl
    {
        public PictureControl()
        {
            InitializeComponent();
            this.SetStyle(
                            ControlStyles.ResizeRedraw |
                            ControlStyles.AllPaintingInWmPaint |
                            ControlStyles.OptimizedDoubleBuffer

                          , true
                          );
        }

        int width;
        int height;
        Bitmap bitmap;
        public Bitmap Bitmap
        {
            get { return bitmap; }
            set
            {

                bitmap = value;
                if (bitmap != null)
                {
                    width = bitmap.Width;
                    height = bitmap.Height;
                }
                AdjustScrolls();
            }
        }
        float zoom = 1;

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; AdjustScrolls(); }
        }


        private void AdjustScrolls()
        {
            if (bitmap != null)
                this.AutoScrollMinSize = new Size((int)(bitmap.Width * zoom), (int)(bitmap.Height * zoom));
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //tomado de iplab
            if (bitmap != null)
            {
                Graphics g = e.Graphics;
                Rectangle rc = ClientRectangle;

                int width = (int)(this.width * zoom);
                int height = (int)(this.height * zoom);
                int x = (rc.Width < width) ? this.AutoScrollPosition.X : (rc.Width - width) / 2;
                int y = (rc.Height < height) ? this.AutoScrollPosition.Y : (rc.Height - height) / 2;

                // set nearest neighbor interpolation to avoid image smoothing
                g.InterpolationMode = InterpolationMode.NearestNeighbor;

                // draw image
                g.DrawImage(bitmap, x, y, width, height);

            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustScrolls();
        }
        public event MoveOverImageEventHandler MouseOverImage;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (MouseOverImage != null && bitmap!= null)
            {
                Rectangle rc = this.ClientRectangle;
                int width = (int)(this.width * zoom);
                int height = (int)(this.height * zoom);
                int x = (rc.Width < width) ? this.AutoScrollPosition.X : (rc.Width - width) / 2;
                int y = (rc.Height < height) ? this.AutoScrollPosition.Y : (rc.Height - height) / 2;

                if ((e.X >= x) && (e.Y >= y) &&
                    (e.X < x + width) && (e.Y < y + height))//mouse over the image
                {
                    x = (int)((e.X - x) / zoom);
                    y = (int)((e.Y - y) / zoom);
                    MouseOverImage(this, new MouseOverImageEventArgs(x,y, bitmap.GetPixel(x,y)));
                }
            }
        }
    }
}
