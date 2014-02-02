using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel;
using TxEstudioApplication.Interfaces;
using TxEstudioKernel.Operators;

namespace TxEstudioApplication.Dialogs
{
    public partial class Feature_Selection : TxEstudioApplication.Dialogs.TonesTexturesBase
    {
        
        public Feature_Selection()
        {
            InitializeComponent();
        }

        public override void SetEnvironment(Environment env)
        {
            base.SetEnvironment(env);

            //Cargando los algortimos de segmentación
            foreach (AlgorithmHolder holder in env.GetAllSegmentationOperators())
                algorithmSelector.Items.Add(holder);

            //Cargando las segmentaciones

            groundTruthSelectionBox.ExposerFilter = new TxEstudioApplication.Custom_Controls.FilterExposerDelegate(SegmentationFilter);
            groundTruthSelectionBox.LoadEnvironment(env);
        }

        private bool SegmentationFilter(IImageExposer imageExposer)
        {
            return imageExposer is SegmentationResultForm;
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            if (imageSelectionBox.SelectedImage != null)
            {
                apply_Button.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                descriptorsNames = new List<string>();
                List<TxMatrix> list = new List<TxMatrix>();
                //Recoger los tonos
                PickUpTones(list);
                //Recoger las texturas
                PickUpTextures(list);

                descriptors = list.ToArray();
                segmenter = (TxSegmentationAlgorithm)(algorithmSelector.SelectedItem as AlgorithmHolder).Algorithm;


                progress_Label.Text = "Segmenting image";
                progressBar.Style = ProgressBarStyle.Marquee;
                this.Refresh();

                progress_Label.Text = "Selecting Features";
                this.Refresh();

                if (cb_super.Checked)
                {
                    //TODO: Poner codigo referente a la la utilizacion de otras medidas de evaluacion
                    evaluator = new RegionBasedSegmentationSimilarity();
                    //TODO: En realidad hay que hacer un metodo mejor para obtener el ground truth
                    ((RegionBasedSegmentationSimilarity)evaluator).GroundTruth = ((SegmentationResultForm)groundTruthSelectionBox.SelectedExposer).Segmentation;

                    //System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(SelectFeatures));
                    //thread.Start();

                }
                else
                {
                    evaluator = new TxUnsupervisedSegmentationEvaluation();
                }
               

                SelectFeatures();


                this.Cursor = Cursors.Default;
                apply_Button.Enabled = true;
            }
        }

        #region Feature Selection

        TxSegmentationEvaluation evaluator;
        TxMatrix[] descriptors;
        TxSegmentationAlgorithm segmenter;
        //int dimention;

        private void SelectFeatures()
        {
            bool[] result=null; 
            SFFS sffs = new SFFS();
            lock (evaluator)
            {
                try
                {
                    result = sffs.FeatureSelection(descriptors, segmenter, evaluator, Math.Min(descriptors.Length, (int)dimensionSelector.Value), float.Parse(tbx_error.Text));
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter("selectionResult.txt"))
                    {
                        Selection_Result dialogResult = new Selection_Result();

                        //Poniendo las caracteristicas seleccionadas
                        writer.WriteLine("Features Selected");
                        dialogResult.AddDescriptor("Features Selected");
                        dialogResult.AddDescriptor("");
                        writer.WriteLine("");
                        for (int i = 0; i < result.Length; i++)
                            if (result[i])
                            {
                                writer.WriteLine(descriptorsNames[i]);
                                dialogResult.AddDescriptor(descriptorsNames[i]);

                            }

                        //Formato
                        writer.WriteLine("");
                        writer.WriteLine("");
                        dialogResult.AddDescriptor("");
                        dialogResult.AddDescriptor("");

                        writer.WriteLine("Set of Features");
                        dialogResult.AddDescriptor("Set of Features");
                        dialogResult.AddDescriptor("");
                        writer.WriteLine("");

                        //Poniendo el conjunto inicial
                        for (int i = 0; i < result.Length; i++)
                        {
                            writer.WriteLine(descriptorsNames[i]);
                            dialogResult.AddDescriptor(descriptorsNames[i]);

                        }
                                                        
                        progressBar.Style = ProgressBarStyle.Blocks;
                        progressBar.Value = 0;
                        progress_Label.Text = "";

                        dialogResult.ShowDialog();


                        writer.Close();

                        if (dialogResult.Path != null)
                        {
                            System.IO.File.Copy("selectionResult.txt", dialogResult.Path);
                        }

                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                
            }

            			
        }

        #endregion

        #region Harvest

        protected List<string> descriptorsNames;

        protected void PickUpTones(List<TxMatrix> list)
        {
            if (tones_Check.Checked)
            {
                progress_Label.Text = "Getting tones";
                this.Refresh();
                TxImage[] images = tones_SelectionBox.SelectedImages;
                IImageExposer[] tonesExposers= tones_SelectionBox.SelectedExposers;
                progressBar.Maximum = images.Length;
                progressBar.Value = 0;
                progressBar.Step = 1;

                for (int i = 0; i < images.Length; i++)
                {
                    list.Add(TxMatrix.FromImage(images[i]));
                    descriptorsNames.Add(tones_SelectionBox.SelectedExposers[i].ImageName);
                    progressBar.PerformStep();
                }
            }
        }
        protected void PickUpTextures(List<TxMatrix> list)
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
                    list.Add(handler.GetCurrentDescription(imageSelectionBox.SelectedImage));
                    descriptorsNames.Add(handler.GetCurrentAbbreviation());
                    progressBar.PerformStep();
                }
            }
        }

        #endregion

        private void algorithmSelector_SelectedValueChanged(object sender, EventArgs e)
        {
            algorithmViewer.AlgorithmInstance = ((AlgorithmHolder)algorithmSelector.SelectedItem).Algorithm;
        }

        private void cb_super_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_super.Checked) groundTruthSelectionBox.Visible = true;
            else groundTruthSelectionBox.Visible = false;
        }

        
 
    }
}

