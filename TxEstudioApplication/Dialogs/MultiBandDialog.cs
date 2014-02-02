using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioApplication.Dialogs
{
    public partial class MultiBandDialog : TxAppForm
    {
        public MultiBandDialog()
        {
            InitializeComponent();
        }

        public override void SetEnvironment(Environment env)
        {
            base.SetEnvironment(env);
            imageMultiSelectionBox1.LoadEnvironment(env);
        }

        public DialogResult ShowDialogWith(IWin32Window owner, TxAlgorithm algorithmInstance)
        {
            if (algorithmInstance == null)
                throw new ArgumentNullException();
            if(TxDIPAlgorithm.IsOneBandInput((TxDIPAlgorithm)algorithmInstance))
                throw new ArgumentException("Operator must be multi band input");
            algorithmViewer.AlgorithmInstance = algorithmInstance;
            return ShowDialog(owner);
        }

        private void PerformAction(TxMultiBand multiBand)
        {
            ImageHolderForm newForm = new ImageHolderForm();
            newForm.Image = multiBand.Process(imageMultiSelectionBox1.SelectedImages);
            string abbr = AbbreviationAttribute.GetFullAbbreviation(multiBand);
            newForm.ImageName = "new" + abbr;
            env.OpenWindow(newForm);
        }

        private void PerformAction(TxGeneral multiOut)
        {
            string abbr = AbbreviationAttribute.GetFullAbbreviation(multiOut);
            List<TxImage> result = multiOut.Process(imageMultiSelectionBox1.SelectedImages);
            for (int i = 0; i < result.Count; i++)
            {
                ImageHolderForm newForm = new ImageHolderForm();
                newForm.Image = result[i];
                newForm.ImageName = string.Format("{0}+{1}_{2}", "new", abbr, i);
                env.OpenWindow(newForm);
            }
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            try
            {
                TxImage[] selectedImages = imageMultiSelectionBox1.SelectedImages;
                if (selectedImages.Length > 0)
                {
                    apply_Button.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;

                    if (algorithmViewer.AlgorithmInstance is TxMultiBand)
                        PerformAction((TxMultiBand)algorithmViewer.AlgorithmInstance);
                    else
                        PerformAction((TxGeneral)algorithmViewer.AlgorithmInstance);

                    apply_Button.Enabled = true;
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);              
            }
        }
    }
}