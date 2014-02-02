using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel;
using System.Windows.Forms.VisualStyles;

namespace TxEstudioApplication.Custom_Controls
{
    public partial class HistogramCanvas : Control
    {
        public HistogramCanvas()
        {
            InitializeComponent();
        }

        ChannelHistogram histogram;
        public ChannelHistogram ChannelHistogram
        {
            get { return histogram; }
            set { histogram = value; }
        }

        Pen pen;

        public Pen Pen
        {
            get { return pen; }
            set { pen = value; }
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            int currentBaseY = ClientRectangle.Bottom;
            int currentX = ClientRectangle.Left + 1;
            int currentY = 0;
            
            if (histogram != null)
            {
                for (int i = 0; i < 256; i++, currentX++)
                {
                    currentY = ClientRectangle.Bottom - ClientRectangle.Height * histogram[i] / histogram.MaxFreq;
                    pe.Graphics.DrawLine(pen, currentX, currentBaseY, currentX, currentY);
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
           // base.OnPaintBackground(pevent);
            VisualStyleRenderer renderer = new VisualStyleRenderer(VisualStyleElement.TextBox.TextEdit.Normal);
            renderer.DrawBackground(pevent.Graphics, this.ClientRectangle);
        }
    }
}
