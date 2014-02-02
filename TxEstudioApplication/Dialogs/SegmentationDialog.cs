using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;
using System.Threading;

namespace TxEstudioApplication.Dialogs
{
    public partial class SegmentationDialog : TonesTexturesBase
    {
        private TxEstudioKernel.VisualElements.AlgorithmViewer algorithmViewer;
    
        public SegmentationDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SegmentationDialog));
            this.algorithmViewer = new TxEstudioKernel.VisualElements.AlgorithmViewer();
            this.textures_Box.SuspendLayout();
            this.tones_Group.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(11, 453);
            // 
            // textures_Box
            // 
            this.textures_Box.Location = new System.Drawing.Point(195, 199);
            this.textures_Box.Size = new System.Drawing.Size(294, 238);
            // 
            // descriptor_Viewer
            // 
            this.descriptor_Viewer.Size = new System.Drawing.Size(137, 200);
            this.descriptor_Viewer.ViewParameterDescription = true;
            // 
            // descriptors_Tree
            // 
            this.descriptors_Tree.LineColor = System.Drawing.Color.Black;
            this.descriptors_Tree.Size = new System.Drawing.Size(139, 200);
            // 
            // tones_Group
            // 
            this.tones_Group.Size = new System.Drawing.Size(295, 185);
            this.tones_Group.Text = "";
            // 
            // tones_SelectionBox
            // 
            this.tones_SelectionBox.Location = new System.Drawing.Point(6, 12);
            this.tones_SelectionBox.Size = new System.Drawing.Size(283, 167);
            // 
            // close_Button
            // 
            this.close_Button.Location = new System.Drawing.Point(632, 433);
            // 
            // apply_Button
            // 
            this.apply_Button.Location = new System.Drawing.Point(551, 433);
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // algorithmViewer
            // 
            this.algorithmViewer.AlgorithmInstance = null;
            this.algorithmViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.algorithmViewer.Location = new System.Drawing.Point(506, 25);
            this.algorithmViewer.Margin = new System.Windows.Forms.Padding(3, 3, 3, 35);
            this.algorithmViewer.Name = "algorithmViewer";
            this.algorithmViewer.ShowDescription = true;
            this.algorithmViewer.Size = new System.Drawing.Size(201, 346);
            this.algorithmViewer.TabIndex = 22;
            this.algorithmViewer.ViewParameterDescription = true;
            // 
            // SegmentationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(720, 468);
            this.Controls.Add(this.algorithmViewer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SegmentationDialog";
            this.ShowInTaskbar = false;
            this.Text = "Segment image";
            this.Controls.SetChildIndex(this.tones_Group, 0);
            this.Controls.SetChildIndex(this.close_Button, 0);
            this.Controls.SetChildIndex(this.apply_Button, 0);
            this.Controls.SetChildIndex(this.algorithmViewer, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.textures_Box, 0);
            this.Controls.SetChildIndex(this.progressBar, 0);
            this.Controls.SetChildIndex(this.tones_Check, 0);
            this.Controls.SetChildIndex(this.textures_Check, 0);
            this.Controls.SetChildIndex(this.progress_Label, 0);
            this.textures_Box.ResumeLayout(false);
            this.tones_Group.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            if (imageSelectionBox.SelectedImage != null)
            {
                apply_Button.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                List<TxMatrix> list = new List<TxMatrix>();
                //Recoger los tonos
                PickUpTones(list);
                //Recoger las texturas
                PickUpTextures(list);
                //Ejecutar el algoritmo

                progress_Label.Text = "Segmenting image";
                progressBar.Style = ProgressBarStyle.Marquee;
                this.Refresh();

                TxSegmentationResult result = null;
                if (list.Count == 0)//Se hace solamente con la imagen
                    result = (algorithmViewer.AlgorithmInstance as TxSegmentationAlgorithm).Segment(TxMatrix.FromImage(imageSelectionBox.SelectedImage));
                else
                    result = (algorithmViewer.AlgorithmInstance as TxSegmentationAlgorithm).Segment(list.ToArray());
                //Mostrar el resultado

                progress_Label.Text = "Displaying result";
                this.Refresh();

                SegmentationResultForm newForm = new SegmentationResultForm();
                newForm.ImageName = imageSelectionBox.SelectedExposer.ImageName + "+" + AbbreviationAttribute.GetFullAbbreviation(algorithmViewer.AlgorithmInstance);
                newForm.SetData((Bitmap)imageSelectionBox.SelectedExposer.Bitmap.Clone(), result);
                env.OpenWindow(newForm);


                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 0;
                progress_Label.Text = "";

                this.Cursor = Cursors.Default;
                apply_Button.Enabled = true;
            }
        }

        public DialogResult ShowDialogWith(IWin32Window owner, TxEstudioKernel.TxSegmentationAlgorithm txSegmentationAlgorithm)
        {
            if (txSegmentationAlgorithm == null)
                throw new ArgumentNullException();
            algorithmViewer.AlgorithmInstance = txSegmentationAlgorithm;
            return ShowDialog(owner);
        }

        public DialogResult ShowDialogWith(IWin32Window owner, TxEstudioKernel.TxTextureEdgeDetector txSegmentationAlgorithm)
        {
            if (txSegmentationAlgorithm == null)
                throw new ArgumentNullException();
            algorithmViewer.AlgorithmInstance = txSegmentationAlgorithm;
            return ShowDialog(owner);
        }

        #region Harvest
        private void PickUpTones(List<TxMatrix> list)
        {
            if (tones_Check.Checked)
            {
                progress_Label.Text = "Getting tones";
                this.Refresh();
                TxImage[] images = tones_SelectionBox.SelectedImages;
                progressBar.Maximum = images.Length;
                progressBar.Value = 0;
                progressBar.Step = 1;

                for (int i = 0; i < images.Length; i++)
                {
                    list.Add(TxMatrix.FromImage(images[i]));
                    progressBar.PerformStep();
                }
            }
        }
        private void PickUpTextures(List<TxMatrix> list)
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
                    progressBar.PerformStep();
                }
            }
        }

        #endregion
    }
}