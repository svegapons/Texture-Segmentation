using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel;

namespace TxEstudioApplication.Dialogs
{
    public partial class DescriptorDialog : TxEstudioApplication.TxAppForm
    {
        public DescriptorDialog()
        {
            InitializeComponent();
            alphaEditor.ParameterValue = 0.01f;
        }

        public override void SetEnvironment(Environment env)
        {
            base.SetEnvironment(env);
            imageSelectionBox.LoadEnvironment(env);
        }

        public DialogResult ShowDialogWith(IWin32Window owner,  string category)
        {
            if (env != null)
            {
                category_Label.Text = category;
                foreach (AlgorithmHolder holder in env.GetDescriptorsCategory(category))
                    descriptors_Check.Items.Add(holder);
            }
            return ShowDialog(owner);
        }

        private void descriptors_Check_SelectedIndexChanged(object sender, EventArgs e)
        {
            algorithmViewer.AlgorithmInstance = ((AlgorithmHolder)descriptors_Check.SelectedItem).Algorithm;
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            if (imageSelectionBox.SelectedImage != null)
            {

                GeneralDescriptorSequence sequence = new GeneralDescriptorSequence();
                foreach (AlgorithmHolder holder in descriptors_Check.CheckedItems)
                    sequence.Add((TextureDescriptor)holder.Algorithm);
                DescriptorsHandler handler = new DescriptorsHandler(sequence, env, new MatrixProccessing(FeaturePostProcessing));
                apply_Button.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                while (handler.MoveNext())
                {
                    ImageHolderForm newForm = new ImageHolderForm();
                    newForm.Image = handler.GetCurrentDescription(imageSelectionBox.SelectedImage).ToImage();
                    newForm.ImageName = imageSelectionBox.SelectedExposer.ImageName + "+" + handler.GetCurrentAbbreviation();
                    env.OpenWindow(newForm);
                }
                this.Cursor = Cursors.Default;
                apply_Button.Enabled = true;
            }
        }

        #region Features post processing
        private void nonlinearCheck_CheckedChanged(object sender, EventArgs e)
        {
            alphaEditor.Enabled = nonlinearCheck.Checked;
        }

        private void energyCheck_CheckedChanged(object sender, EventArgs e)
        {
            windowSize.Enabled = gaussianCheck.Enabled = energyCheck.Checked;
        }

        private void postProcCheck_CheckedChanged(object sender, EventArgs e)
        {
            postProcGroup.Enabled = postProcCheck.Checked;
        }

        protected TxEstudioKernel.TxMatrix FeaturePostProcessing(TxEstudioKernel.TxMatrix matrix)
        {
            if (postProcCheck.Checked)
            {
                if (nonlinearCheck.Checked)
                    matrix = new TxEstudioKernel.Operators.Matrix_Operators.NonLinearOperator((float)alphaEditor.ParameterValue).Process(matrix);
                if (energyCheck.Checked)
                {
                    if (gaussianCheck.Checked)
                        matrix = new TxEstudioKernel.Operators.Matrix_Operators.GaussianEnergyFilter((int)windowSize.Value).Process(matrix);
                    else
                        matrix = new TxEstudioKernel.Operators.Matrix_Operators.SimpleEnergyFilter((int)windowSize.Value).Process(matrix);
                }
            }

            //ImageHolderForm newForm = new ImageHolderForm();
            //newForm.Image = matrix.ToImage();
            //env.OpenWindow(newForm);
            return matrix;
        }

        #endregion
    }
}

