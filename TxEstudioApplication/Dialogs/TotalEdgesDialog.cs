using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.Operators;
using TxEstudioKernel.OpenCV;

namespace TxEstudioApplication.Dialogs
{
    public partial class TotalEdgesDialog : TonesTexturesBase
    {
        public TotalEdgesDialog()
        {
            InitializeComponent();
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            if (imageSelectionBox.SelectedImage != null && algorithmViewer.AlgorithmInstance != null && AnyChoiceMarked())
            {
                apply_Button.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                List<TxImage> list = new List<TxImage>();
                //Recoger los tonos
                PickUpTones(list);
                //Recoger las texturas
                PickUpTextures(list);
                //Ejecutar el algoritmo

                progress_Label.Text = "Detecting edges";
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Minimum = 0;
                progressBar.Maximum = list.Count;
                progressBar.Step = 1;
                this.Refresh();

                TxOneBand edgeDetector = algorithmViewer.AlgorithmInstance as TxOneBand;
                TxImage[] edgeImages = new TxImage[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    edgeImages[i] = edgeDetector.Process(list[i]);
                    progressBar.PerformStep();
                }
                if (summation_Check.Checked)
                {
                    TxSummation summation = new TxSummation();
                    ImageHolderForm newForm = new ImageHolderForm();
                    TxImage sum = summation.Process(edgeImages);
                    newForm.Image = sum;
                    //TxImage thres = new TxImage(sum.Width, sum.Height, TxImageFormat.GrayScale);
                    //CV.cvThreshold(sum.InnerImage, thres.InnerImage, 240, 255, 0);
                    //newForm.Image = thres;
                    newForm.ImageName = imageSelectionBox.SelectedExposer.ImageName + "+tedge_sum()";

                    env.OpenWindow(newForm);
                }
                if (average_Check.Checked)
                {
                    TxAverage average = new TxAverage();
                    ImageHolderForm newForm = new ImageHolderForm();
                    TxImage ave = average.Process(edgeImages);
                    newForm.Image = ave;
                    //TxImage thres = new TxImage(sum.Width, sum.Height, TxImageFormat.GrayScale);
                    //CV.cvThreshold(sum.InnerImage, thres.InnerImage, 240, 255, 0);
                    //newForm.Image = thres;
                    newForm.ImageName = imageSelectionBox.SelectedExposer.ImageName + "+tedge_ave()";

                    env.OpenWindow(newForm);
                }
               
                if (maximum_Check.Checked)
                {
                    TxMaximum maximum = new TxMaximum();
                    ImageHolderForm newForm = new ImageHolderForm();
                    TxImage max = maximum.Process(edgeImages);
                    newForm.Image = max;
                    newForm.ImageName = imageSelectionBox.SelectedExposer.ImageName + "+tedge_max()";
                    env.OpenWindow(newForm);
                }
                //if (max_min_Check.Checked)
                //{
                //}

                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 0;
                progress_Label.Text = "";

                this.Cursor = Cursors.Default;
                apply_Button.Enabled = true;
            }
        }

        private bool AnyChoiceMarked()
        {
            return summation_Check.Checked  ||
                    maximum_Check.Checked   ||
                    average_Check.Checked;   
        }

        private bool IsEdgeDetector(AlgorithmHolder holder)
        {
            EdgeDetectorAttribute category = new EdgeDetectorAttribute();
            return  holder.AlgorithmType.IsSubclassOf(typeof(TxOneBand)) && 
                    holder.AlgorithmType.GetCustomAttributes(typeof(EdgeDetectorAttribute), false).Length > 0;
        }

        public override void SetEnvironment(Environment env)
        {
            base.SetEnvironment(env);
            TxAlgorithmCondition condition = new TxAlgorithmCondition(IsEdgeDetector);
            foreach (AlgorithmHolder holder in env.GetOperators(condition))
                edgeDetectors_Combo.Items.Add(holder);
            if (edgeDetectors_Combo.Items.Count > 0)
                edgeDetectors_Combo.SelectedIndex = 0;
        }


        #region Harvest
        private void PickUpTones(List<TxImage> list)
        {
            if (tones_Check.Checked)
            {
                progress_Label.Text = "Getting tones";
                this.Refresh();
                list.AddRange(tones_SelectionBox.SelectedImages);
            }
        }
        private void PickUpTextures(List<TxImage> list)
        {
            if (textures_Check.Checked)
            {
                progress_Label.Text = "Applying texture descriptors";
                this.Refresh();
                int total = 0;
                GeneralDescriptorSequence sequence = new GeneralDescriptorSequence();
                foreach (TreeNode node in descriptors_Tree.Nodes)
                    if (node.Checked)//El padre esta marcado, alguno de sus hijos tambien
                        foreach (TreeNode child in node.Nodes)
                        {
                            if (child.Checked)
                            {
                                sequence.Add((TextureDescriptor)((AlgorithmHolder)child.Tag).Algorithm);
                                total++;
                            }
                        }

                progressBar.Maximum = total;
                progressBar.Step = 1;
                progressBar.Value = 0;

                DescriptorsHandler handler = new DescriptorsHandler(sequence, env, new MatrixProccessing(FeaturePostProcessing));
                while (handler.MoveNext())
                {
                    list.Add(handler.GetCurrentDescription(imageSelectionBox.SelectedImage).ToImage());
                    progressBar.PerformStep();
                }
            }
        }
        #endregion

        private void edgeDetectors_Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            algorithmViewer.AlgorithmInstance = (edgeDetectors_Combo.SelectedItem as AlgorithmHolder).Algorithm;
        }



    }
}