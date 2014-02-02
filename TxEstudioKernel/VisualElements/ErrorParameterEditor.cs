using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace TxEstudioKernel.VisualElements
{
    public partial class ErrorParameterEditor : UserControl, IParameterEditor
    {
        public ErrorParameterEditor()
        {
            InitializeComponent();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            VisualStyleRenderer renderer = new VisualStyleRenderer(VisualStyleElement.TextBox.TextEdit.Normal);
            renderer.DrawBackground(pevent.Graphics, this.ClientRectangle);
            pevent.Graphics.DrawIcon(SystemIcons.Error, this.ClientRectangle);
        }

         

        #region IParameterEditor Members

        public object ParameterValue
        {
            get
            {
                return null;
            }
            set
            {
                
            }
        }

        public string GetStringParameterRepresentation()
        {
            return "Error";
        }

        public event EventHandler ParameterValueChanged;

        protected virtual void OnParameterValueChanged(object sender, EventArgs e)
        {
            if (ParameterValueChanged != null)
                ParameterValueChanged(sender, e);
        }

        #endregion
    }
}
