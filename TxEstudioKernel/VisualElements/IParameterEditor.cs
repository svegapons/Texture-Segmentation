using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace TxEstudioKernel.VisualElements
{
    public interface IParameterEditor
    {
        object ParameterValue { get; set;}
        string GetStringParameterRepresentation();
        event EventHandler ParameterValueChanged;
    }

    public class IntegerParameterEditor: NumericUpDown, IParameterEditor
    {
        public IntegerParameterEditor():base()
        {
            this.Maximum = int.MaxValue;
            this.Minimum = int.MinValue;
            this.Value = 0;
            this.Increment = 1;
        }
        //TODO: Buscar forma de mantener cero en los lugares decimales
        #region IParameterEditor Members

        public object ParameterValue
        {
            get
            {
                return (int)Value;
            }
            set
            {
                int intValue = (int)Convert.ChangeType(value, typeof(int));
                Value = (decimal)intValue;
            }
        }

        public string GetStringParameterRepresentation()
        {
            return ((int)Value).ToString();
        }
        public event EventHandler ParameterValueChanged;

        protected virtual void OnParameterValueChanged(object sender, EventArgs e)
        {
            if (ParameterValueChanged != null)
                ParameterValueChanged(sender, e);
        }

        protected override void OnValueChanged(EventArgs e)
        {
            base.OnValueChanged(e);
            OnParameterValueChanged(this, e);
        }

        #endregion
    }

    public class RealParameterEditor : TextBox, IParameterEditor
    {

        public RealParameterEditor():base()
        {
            this.Text = parameterValue.ToString(); 
            this.Validating += new CancelEventHandler(RealParameterEditor_Validating);            
        }

        void RealParameterEditor_Validating(object sender, CancelEventArgs e)
        {
            float newValue = 0f;
            if (float.TryParse(Text, out newValue))
            {
                if (parameterValue <= maximum && parameterValue >= minimum)
                {
                    parameterValue = newValue;
                    this.Text = parameterValue.ToString();
                    OnParameterValueChanged(this, EventArgs.Empty);
                }
                else
                {
                    e.Cancel = true;
                    ReportError(string.Format("Parameter must be a real value between {0} and {1}", minimum, maximum));
                }
            }
            else
            {
                e.Cancel = true;
                ReportError("Incorrect value");
            }
        }

        float minimum = float.MinValue;
        public float Minimum
        {
            get
            {
                return minimum;
            }
            set
            {
                minimum = value;
                ParameterValue = Math.Max(minimum, parameterValue);
            }
        }

        float maximum = float.MaxValue;
        public float Maximum
        {
            get
            {
                return maximum;
            }
            set
            {
                maximum = value;
                ParameterValue = Math.Min(maximum, parameterValue);
            }
        }

        #region IParameterEditor Members
        float parameterValue = 0.0f;
        public object ParameterValue
        {
            get
            {
                return parameterValue;
            }
            set
            {
                //Esto da invalid cast exception si no es float
                parameterValue = (float)value;
                this.Text = parameterValue.ToString();
            }
        }

        

        public string GetStringParameterRepresentation()
        {
            return Text;
        }

        public event EventHandler ParameterValueChanged;

        protected virtual void OnParameterValueChanged(object sender, EventArgs e)
        {
            if (ParameterValueChanged != null)
                ParameterValueChanged(sender, e);
        }

        #endregion

        #region Show error

        [DllImport("user32.dll")]
        private static extern bool SendMessage(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam);

        [DllImport("Kernel32.dll")]
        private static extern int GetLastError();


        
        private struct EditBalloonTip
        {
            public int cbStruct;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszTitle;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszText;
            public int ttiIcon;
        }

        private void ReportError(string message)
        {
            //ToolTip tip = new ToolTip();
            //tip.IsBalloon = true;
            //tip.ToolTipIcon = ToolTipIcon.Error;
            //tip.Show(message, this, 1000);

            EditBalloonTip tip = new EditBalloonTip();
            tip.cbStruct = Marshal.SizeOf(typeof(EditBalloonTip));
            tip.pszText = message;
            tip.pszTitle = "Error";
            tip.ttiIcon = 3;//TTI_ERROR
            IntPtr ptrTip = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(EditBalloonTip)));
            Marshal.StructureToPtr(tip, ptrTip, true);
            bool b = SendMessage(this.Handle, (0x1503) /*EM_SHOWBALLOONTIP*/, IntPtr.Zero, ptrTip);
            if (!b)
            {
                MessageBox.Show("Error: " + GetLastError().ToString());
            }
            
        }
        #endregion
    }
}
