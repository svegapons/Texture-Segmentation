using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace TxEstudioKernel.VisualElements
{
    public partial class MultiScaleParameterEditor : UserControl, IParameterEditor
    {
        public float maxSum ;
        public MultiScaleParameterEditor()
        {
            InitializeComponent();
            maxSum = 1;
        }

        

        private void addButton_Click(object sender, EventArgs e)
        {
            float sum = 0;
            for (int i = 0; i < weightsList.Items.Count; i++)
                sum += float.Parse(weightsList.Items[i].ToString());

            float newWeight = (float)newWeightEditor.ParameterValue;
            if ((sum + newWeight <= 1.0001)&&(float.Parse(newSigmaEditor.ParameterValue.ToString())>0))
            {
                sum += newWeight;
                weightsList.Items.Add(newWeight);
                sigmasList.Items.Add(newSigmaEditor.ParameterValue);
                newWeightEditor.Minimum = 0f;
                newWeightEditor.Maximum =(float)Math.Round(1-sum,2);
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (weightsList.SelectedIndex >= 0)
            {
                float toRemove = float.Parse(weightsList.Text);
                newWeightEditor.Maximum = newWeightEditor.Maximum+(float)toRemove;
                int index=weightsList.SelectedIndex;
                weightsList.Items.Remove(weightsList.SelectedItem);
                sigmasList.Items.RemoveAt(index);
            }
        }

        private void weightsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            sigmasList.SelectedIndex = weightsList.SelectedIndex;
        }

        private void sigmasList_SelectedIndexChanged(object sender, EventArgs e)
        {
            weightsList.SelectedIndex = sigmasList.SelectedIndex;
        }


        #region IParameterEditor Members

        public object ParameterValue
        {
            get
            {
                return new TxEstudioKernel.Operators.MultiScaleRetinexParameter(getList(weightsList), getList(sigmasList));
            }
            set
            {
                maxSum = 0;
                setList(((TxEstudioKernel.Operators.MultiScaleRetinexParameter)value).weights, weightsList);
                setList(((TxEstudioKernel.Operators.MultiScaleRetinexParameter)value).sigmas, sigmasList);
            }
        }

        private void setList(float[] p, ListBox list)
        {
            for (int i = 0; i < p.Length; i++)
                list.Items.Add(p[i]);
        }

        private float[] getList(ListBox list)
        {
            float[] result = new float[list.Items.Count];
            for (int i = 0; i < result.Length; i++)
                result[i] = (float)list.Items[i];
            return result;
        }

        public string GetStringParameterRepresentation()
        {
            return "";
        }

        public event EventHandler ParameterValueChanged;

        #endregion
    }

    
}
