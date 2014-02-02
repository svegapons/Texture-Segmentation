using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace TxEstudioApplication.Custom_Controls
{


    //TODO: Ver como ponerle comportamiento del group box
    [DefaultEvent("CheckedChanged")]
    [DefaultProperty("Text")]
    public partial class CustomGroupBox : UserControl
    {
        public CustomGroupBox()
        {
            InitializeComponent();
        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
            VisualStyleRenderer renderer = new VisualStyleRenderer(VisualStyleElement.TextBox.TextEdit.Normal);
            renderer.DrawBackground(e.Graphics, this.ClientRectangle);
        }

        [Browsable(true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                text_Label.Text = value;
                checkBox.Text = value;
            }
        }

        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Returns whether the group box should show a text box.")]
        public bool ShowCheckBox
        {
            get
            {
                return checkBox.Visible;
            }
            set
            {
                checkBox.Visible = value;
                text_Label.Visible = !value;
            }
        }

        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Returns whether the group box is checked or not.")]
        public bool Checked
        {
            get
            {
                return  checkBox.Checked;
            }
            set
            {
                if (checkBox.Checked != value)
                {
                    checkBox.Checked = value;
                    OnCheckedChanged(this, EventArgs.Empty);
                }
            }
        }


        
        public event EventHandler CheckedChanged;

        protected virtual void OnCheckedChanged(object sender, EventArgs e)
        {
            foreach (Control control in Controls)
                control.Enabled = checkBox.Checked;
            if (CheckedChanged != null)
                CheckedChanged(sender, e);
        }
    }
}
