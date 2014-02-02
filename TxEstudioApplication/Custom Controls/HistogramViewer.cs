using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel;

namespace TxEstudioApplication.Custom_Controls
{
    public partial class HistogramViewer : UserControl
    {
        public HistogramViewer()
        {
            InitializeComponent();
           // SetStyle(ControlStyles.FixedHeight | ControlStyles.FixedWidth, true);

            //initializing buttons

            grayScale_Button.Tag = 0;
            red_Button.Tag       = 1;
            green_Button.Tag     = 2;
            blue_Button.Tag      = 3;

            pressed = grayScale_Button;
        }

        int currentIndex = 0; 
        Rectangle targetRectangle = new Rectangle(4, 38, 257, 100);
        Pen[] myPens = new Pen[] { Pens.Black, Pens.Red, Pens.Green, Pens.Blue};

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
        TxHistogram histogram;

        public TxHistogram Histogram
        {
            get { return histogram; }
            set 
            { 
                histogram = value;
                UpdateValues();
            }
        }

        private void UpdateValues()
        {
            if (histogram  != null)
            {
                histogarmCanvas1.Pen = myPens[currentIndex];
                histogarmCanvas1.ChannelHistogram = histogram[currentIndex];
                min_Label.Text = histogram[currentIndex].Minimum.ToString();
                max_Label.Text = histogram[currentIndex].Maximum.ToString();
                minf_Label.Text = histogram[currentIndex].MinFreq.ToString();
                maxf_Label.Text = histogram[currentIndex].MaxFreq.ToString();
                stdv_Label.Text = histogram[currentIndex].Stdv.ToString("F2");
                mean_Label.Text = histogram[currentIndex].Mean.ToString("F2");
            }
            else
            {
                min_Label.Text = "";
                max_Label.Text = "";
                stdv_Label.Text = "";
                mean_Label.Text = "";
                minf_Label.Text = "";
                maxf_Label.Text = "";
                freq_Label.Text = "";
                
            }
            Refresh();
        }

        ToolStripButton pressed;

        private void buttons_Strip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            pressed.Checked = false;
            pressed = (ToolStripButton)e.ClickedItem;
            pressed.Checked = true;
            currentIndex = (int)pressed.Tag;
            UpdateValues();
        }


        private void histogarmCanvas1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (histogram != null && e.X > 0 && e.X < 257)
            {
                value_UpDown.Value = e.X - 1;
                freq_Label.Text = histogram[currentIndex][e.X - 1].ToString();
            }
        }

        private void value_UpDown_ValueChanged(object sender, EventArgs e)
        {
            freq_Label.Text = histogram[currentIndex][(int)value_UpDown.Value].ToString();
        }
    }
}
