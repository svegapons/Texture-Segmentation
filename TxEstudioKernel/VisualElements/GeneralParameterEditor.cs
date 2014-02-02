using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace TxEstudioKernel.VisualElements
{
    public partial class GeneralParameterEditor : UserControl, IParameterEditor
    {
        public GeneralParameterEditor()
        {
            InitializeComponent();
        }

        public GeneralParameterEditor(IParameterEditor editor)
            : this()
        {
            Editor = editor;
        }


        string valueRep;
        protected override void OnPaint(PaintEventArgs e)
        {
            StringFormat format = new StringFormat();
            format.Trimming = StringTrimming.EllipsisCharacter;
            valueRep = editor.GetStringParameterRepresentation();
            e.Graphics.DrawString(valueRep, this.Font, new SolidBrush(this.ForeColor), new RectangleF(0, 0, this.Width, this.Height), format);
            
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            VisualStyleRenderer renderer = new VisualStyleRenderer(VisualStyleElement.TextBox.TextEdit.Normal);
            renderer.DrawBackground(e.Graphics, this.ClientRectangle);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            ellipsis_Button.Visible = true;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            ellipsis_Button.Visible = false;
        }

        IParameterEditor editor;

        public IParameterEditor Editor
        {
            get { return editor; }
            set 
            {
                if (value == null)
                    throw new ArgumentNullException();
                editor = value;
                editor.ParameterValueChanged += new EventHandler(editor_ParameterValueChanged);
                //Using context menu strip
                contextMenuStrip.Items.Clear();
                ToolStripControlHost host = new ToolStripControlHost((Control)editor);
                host.Padding = new Padding(2);
                contextMenuStrip.Items.Add(host);
            }
        }

        void editor_ParameterValueChanged(object sender, EventArgs e)
        {
            OnParameterValueChanged(this, e);
        }

        private void ellipsis_Button_Click(object sender, EventArgs e)
        {
            contextMenuStrip.Show(ellipsis_Button, 0, ellipsis_Button.Height);
        }

        private void contextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            valueRep = editor.GetStringParameterRepresentation();
            OnParameterValueChanged(this, EventArgs.Empty);
            this.Refresh();
        }


        #region IParameterEditor Members

        public object ParameterValue
        {
            get
            {
                return editor.ParameterValue;
            }
            set
            {
                editor.ParameterValue = value;
            }
        }

        public string GetStringParameterRepresentation()
        {
            return editor.GetStringParameterRepresentation();
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
