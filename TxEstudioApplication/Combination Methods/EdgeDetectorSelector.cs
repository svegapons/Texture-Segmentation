using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioApplication.Combination_Methods
{
    public partial class EdgeDetectorSelector : TxEstudioApplication.TxAppForm
    {
        public EdgeDetectorSelector()
        {
            InitializeComponent();
        }

        public override void SetEnvironment(Environment env)
        {
            base.SetEnvironment(env);
            lb_edgesDetectors.Items.AddRange(env.GetOperators(IsEdgeDetector).ToArray());
        }


        private bool IsEdgeDetector(AlgorithmHolder algorithmHolder)
        {
            return algorithmHolder.AlgorithmType.GetCustomAttributes(typeof(EdgeDetectorAttribute), true).Length > 0;
        }

        private void lb_edgesDetectors_SelectedIndexChanged(object sender, EventArgs e)
        {
            algorithmViewer.AlgorithmInstance = (lb_edgesDetectors.SelectedItem as AlgorithmHolder).Algorithm;
        }

        TxEstudioKernel.TxOneBand algorithm;
        public TxEstudioKernel.TxOneBand Operator
        {
            get
            {
                return algorithm;
            }
            set 
            {
                algorithm = value;
            }
            
        }

        private void bt_OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            algorithm = algorithmViewer.AlgorithmInstance as TxEstudioKernel.TxOneBand;
            Close();
        }




    }
}

