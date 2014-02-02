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

namespace TxEstudioApplication.Combination_Methods
{
    public partial class CombinationMethodDialog : TxEstudioApplication.TxAppForm
    {
        public CombinationMethodDialog()
        {
            InitializeComponent();

            algorithmViewer.AlgorithmInstance = new AC_BiClass_BorderRegion();

        }

        #region Events Actions

        protected virtual void textures_Check_CheckedChanged(object sender, EventArgs e)
        {
            textures_Box.Enabled = textures_Check.Checked;
        }

        protected virtual void tones_Check_CheckedChanged(object sender, EventArgs e)
        {
            tones_Group.Enabled = tones_Check.Checked;
        }

        protected virtual void descriptors_Tree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Parent == null)
                {
                    bool mark = e.Node.Checked;
                    foreach (TreeNode child in e.Node.Nodes)
                        child.Checked = mark;
                }
                else
                {
                    if (e.Node.Checked)
                        e.Node.Parent.Checked = true;
                    else
                    {
                        foreach (TreeNode sibling in e.Node.Parent.Nodes)
                            if (sibling.Checked)
                                return;
                        e.Node.Parent.Checked = false;
                    }
                }
            }
        }

        protected virtual void imageSelectionBox_ItemChanged(object sender, EventArgs e)
        {
            tones_SelectionBox.SetInitialTarget(imageSelectionBox.SelectedExposer);
        }

        protected virtual void descriptors_Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                descriptor_Viewer.AlgorithmInstance = ((AlgorithmHolder)e.Node.Tag).Algorithm;
            }
        }

        private void rb_borderMap_CheckedChanged(object sender, EventArgs e)
        {
            bt_borderMap.Enabled = rb_borderMap.Checked;
        }

        private void rb_useOther_CheckedChanged(object sender, EventArgs e)
        {
            bt_Select.Enabled = rb_useOther.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            bt_selectImage.Enabled = radioButton1.Checked;
        }
       
        private void bt_borderMap_Click(object sender, EventArgs e)
        {
            ContourPaintForm paint = new ContourPaintForm(imageSelectionBox.SelectedImage.Width + 120, imageSelectionBox.SelectedImage.Height + 120);
            paint.BackImage = imageSelectionBox.SelectedImage.ToBitamp();
            if (paint.ShowDialog(this) == DialogResult.OK)
                (algorithmViewer.AlgorithmInstance as AC_BiClass_BorderRegion).BorderMatrix = paint.Border;

        }

        private void bt_Select_Click(object sender, EventArgs e)
        {
            EdgeDetectorSelector selector = new EdgeDetectorSelector();
            selector.SetEnvironment(env);
            if (selector.ShowDialog(this) == DialogResult.OK)
                (algorithmViewer.AlgorithmInstance as AC_BiClass_BorderRegion).BorderMatrix = BorderMatrix.FromImage(selector.Operator.Process(imageSelectionBox.SelectedImage));
        }

        private void bt_selectImage_Click(object sender, EventArgs e)
        {
            BorderFromImage dialog = new BorderFromImage();
            dialog.SetEnvironment(env);
            if(dialog.ShowDialog() == DialogResult.OK)
                (algorithmViewer.AlgorithmInstance as AC_BiClass_BorderRegion).BorderMatrix = dialog.Border;
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
                {
                    //TODO: Poner los descriptores por defecto 
                    result = (algorithmViewer.AlgorithmInstance as TxSegmentationAlgorithm).Segment(TxMatrix.FromImage(imageSelectionBox.SelectedImage));
                }
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

        #endregion

        public override void SetEnvironment(Environment env)
        {
            base.SetEnvironment(env);
            imageSelectionBox.LoadEnvironment(env);
            tones_SelectionBox.LoadEnvironment(env);
            tones_SelectionBox.SetInitialTarget(imageSelectionBox.SelectedExposer);

            //Cargando todos los descriptores de textura

            foreach (CategoryHolder catHolder in env.GetAllDescriptorsCategories())
            {
                TreeNode catNode = new TreeNode(catHolder.CategoryName);
                descriptors_Tree.Nodes.Add(catNode);
                foreach (AlgorithmHolder holder in catHolder)
                {
                    TreeNode node = new TreeNode(holder.AlgorithmName);
                    node.Tag = holder;
                    catNode.Nodes.Add(node);
                }
            }

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

                DescriptorsHandler handler = new DescriptorsHandler(sequence, env);
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

