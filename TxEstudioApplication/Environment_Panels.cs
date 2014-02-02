using System;
using System.Windows.Forms;
using System.Drawing;
using Janus.Windows.UI.Dock;

namespace TxEstudioApplication
{
    public partial class Environment
    {
        public void PublishPanel(string name, Control control)
        {
            UIPanel panel = new UIPanel();
            panel.Name = panel.Text = name;
            panel.InnerContainer.Controls.Add(control);
            mainForm.AddPanel(panel);

        }
        public void PublishPanel(string name, Control control, Image image)
        {
            UIPanel panel = new UIPanel();
            panel.Name = panel.Text = name;
            panel.InnerContainer.Controls.Add(control);
            panel.Image = image;
            mainForm.AddPanel(panel);
        }

        public void ShowOperatorsPanel()
        {

        }

        public void ShowSegmentationPanel()
        {

        }

        public void ShowFeaturesPanel()
        {

        }
    }
}