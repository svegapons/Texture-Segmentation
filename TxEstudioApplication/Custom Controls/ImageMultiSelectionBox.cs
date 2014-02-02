using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TxEstudioApplication.Interfaces;
using TxEstudioKernel;

namespace TxEstudioApplication.Custom_Controls
{
    public partial class ImageMultiSelectionBox : UserControl
    {
        public ImageMultiSelectionBox()
        {
            InitializeComponent();
            pool_SelectedIndexChanged(this, EventArgs.Empty);
            targets_SelectedIndexChanged(this, EventArgs.Empty);
        }

        public void LoadEnvironment(Environment env)
        {
            pool.Items.Clear();
            targets.Items.Clear();
            IImageExposer ie;
            foreach (Form child in env.OpenedForms)
            {
                ie = child as IImageExposer;
                if (child != null)
                    pool.Items.Add(ie);
            }
        }

        private void pool_SelectedIndexChanged(object sender, EventArgs e)
        {
             
            bool enabled =      (pool.SelectedItem != null) && ((targets.Items.Count==0) 
                            ||  ((targets.Items[0] as IImageExposer).Image.Size == (pool.SelectedItem as IImageExposer).Image.Size));
            add_Button.Enabled = enabled;
            addAll_Button.Enabled = enabled;
        }

        private void targets_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enabled = targets.SelectedItem != null;//Asumo que si esta vacia esto es null
            remAll_Button.Enabled = targets.Items.Count > 0;
            remove_Button.Enabled = enabled;
        }

        private void PassSelected(ListBox from, ListBox to)
        {
            //foreach (object selectedItem in from.SelectedItems)
            //{
            //    to.Items.Add(selectedItem);
            //    from.Items.Remove(selectedItem);
            //}
            //Permitir multiseleccion no parece una buena politica, pues cual
            //seria el comportamiento si se seleccionan dos imagenes de tamannos distintos?
            to.Items.Add(from.SelectedItem);
            from.Items.Remove(from.SelectedItem);
        }

        private void PassSelectedAndAll(ListBox from, ListBox to)
        {
            IImageExposer selected = from.SelectedItem as IImageExposer;
            foreach (IImageExposer ie in from.Items)
            {
                if (ie.Image.Size == selected.Image.Size)
                    to.Items.Add(ie);
            }
            foreach (object item in to.Items)
                from.Items.Remove(item);
        }
        #region Buttons actions
        private void add_Button_Click(object sender, EventArgs e)
        {
            PassSelected(pool, targets);
        }

        private void remove_Button_Click(object sender, EventArgs e)
        {
            PassSelected(targets, pool);
        }

        private void addAll_Button_Click(object sender, EventArgs e)
        {
            PassSelectedAndAll(pool, targets);
        }

        private void remAll_Button_Click(object sender, EventArgs e)
        {
            if (targets.Items.Count > 0)
                ClearTargets();
        }

        private void ClearTargets()
        {
            while (targets.Items.Count > 0)
            {
                pool.Items.Add(targets.Items[0]);
                targets.Items.RemoveAt(0);
            }
        }
        #endregion

        public void SetInitialTarget(IImageExposer imageExposer)
        {
            if (imageExposer != null)
            {
                ClearTargets();
                if (pool.Items.Contains(imageExposer))
                    pool.Items.Remove(imageExposer);
                targets.Items.Add(imageExposer);
            }
        }

        public TxImage[] SelectedImages
        {
            get
            {
                TxImage[] result = new TxImage[targets.Items.Count];
                int i = 0;
                foreach (IImageExposer ie in targets.Items)
                {
                    result[i] = ie.Image;
                    i++;
                }
                return result;
            }
        }

        public IImageExposer[] SelectedExposers
        {
            get
            {
                IImageExposer[] result = new IImageExposer[targets.Items.Count];
                int i = 0;
                foreach (IImageExposer ie in targets.Items)
                {
                    result[i] = ie;
                    i++;
                }
                return result;
            }
        }

    }
}
