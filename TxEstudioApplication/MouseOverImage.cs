using System;
using System.Drawing;

namespace TxEstudioApplication
{
    public class MouseOverImageEventArgs : EventArgs
    {
        public MouseOverImageEventArgs(int x, int y, Color color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }
        int x, y;
        public int Y
        {
            get { return y; }
        }

        public int X
        {
            get { return x; }
        }
        Color color;
        public Color Color
        {
            get { return color; }
        }
    }

    public delegate void MoveOverImageEventHandler(object sender, MouseOverImageEventArgs e);
}
