using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TxEstudioApplication.Dialogs
{
    public partial class TonesTexturesBase : TxAppForm
    {
        public TonesTexturesBase()
        {
            InitializeComponent();
            alphaEditor.ParameterValue = 0.01f;
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