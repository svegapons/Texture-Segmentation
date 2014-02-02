using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace TxEstudioKernel.VisualElements
{
    public partial class ArrayEditor : UserControl, IParameterEditor
    {
        public ArrayEditor()
        {
            InitializeComponent();
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            remove_Button.Enabled = (listBox.SelectedIndex >= 0);
        }

        private void add_Button_Click(object sender, EventArgs e)
        {
            listBox.Items.Add(newValue_Editor.ParameterValue);
        }

        private void remove_Button_Click(object sender, EventArgs e)
        {
            listBox.Items.Remove(listBox.SelectedItem);
        }

        #region IParameterEditor Members

        public object ParameterValue
        {
            get
            {
                float[] result = new float[listBox.Items.Count];
                for (int i = 0; i < result.Length; i++)
                    result[i] = (float)listBox.Items[i];
                return result;
                
            }
            set
            {
                listBox.Items.Clear();
                if (value is float[])
                {
                    float[] items = (float[])value;
                    for (int i = 0; i < items.Length; i++)
                    {
                        listBox.Items.Add(items[i]);
                    }
                }
            }
        }

        public string GetStringParameterRepresentation()
        {
            return "";
        }

        public event EventHandler ParameterValueChanged;

        #endregion

        
    }
}
