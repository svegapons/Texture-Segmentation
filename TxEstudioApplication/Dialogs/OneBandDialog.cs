using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel;

namespace TxEstudioApplication.Dialogs
{
    public partial class OneBandDialog : TxAppForm
    {
        public OneBandDialog()
        {
            InitializeComponent();
        }

        
        /// <summary>
        /// Se mostrará el dialogo a partir de un operador a cargar
        /// </summary>
        /// <param name="algorithm">El operador en cuestión</param>
        /// <returns></returns>
        public DialogResult ShowDialogWith(IWin32Window owner ,TxDIPAlgorithm algorithm)
        {
            if(algorithm == null)
                throw new ArgumentNullException();
            if (!TxDIPAlgorithm.IsOneBandInput(algorithm))
                throw new ArgumentException("Operator is not one band input");
            algorithmViewer.AlgorithmInstance = algorithm;
            return ShowDialog(owner);
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            //TODO: Hacer lo del hilo para la no mareadera y la posibilidad de cancelar.
            //TODO: Recargar el ambiente cuando se termine
            if (imageSelectionBox.SelectedImage != null)
            {
                apply_Button.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                if (algorithmViewer.AlgorithmInstance is TxOneBand)
                    PerformAction((TxOneBand)algorithmViewer.AlgorithmInstance);
                else
                    PerformAction((TxMultiBandOutput)algorithmViewer.AlgorithmInstance);

                this.Cursor = Cursors.Arrow;
                apply_Button.Enabled = true;
            }
        }

        private void PerformAction(TxOneBand oneBand)
        {
            ImageHolderForm newForm = new ImageHolderForm();
            newForm.Image = oneBand.Process(imageSelectionBox.SelectedImage);
            string abbr = AbbreviationAttribute.GetFullAbbreviation(oneBand);
            newForm.ImageName = String.Format("{0}+{1}", imageSelectionBox.SelectedExposer.ImageName, abbr);
            env.OpenWindow(newForm);
        }

        private void PerformAction(TxMultiBandOutput multiOut)
        {
            string abbr = AbbreviationAttribute.GetFullAbbreviation(multiOut);
            List<TxImage> result = multiOut.Process(imageSelectionBox.SelectedImage);
            for( int i=0; i<result.Count; i++)
            {
                ImageHolderForm newForm = new ImageHolderForm();
                newForm.Image = result[i];
                newForm.ImageName = string.Format("{0}+{1}_{2}", imageSelectionBox.SelectedExposer.ImageName, abbr, i);
                env.OpenWindow(newForm);
            }
        }
        public override void SetEnvironment(Environment env)
        {
            base.SetEnvironment(env);
            imageSelectionBox.LoadEnvironment(env);
        }

        
    }
}