using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace TxEstudioApplication.Custom_Controls
{
    public partial class ClassSettingViewer : UserControl
    {
        public ClassSettingViewer()
        {
            InitializeComponent();
        }

        ClassDisplaySetting setting = new ClassDisplaySetting();
        public ClassDisplaySetting Setting
        {
            get
            {
                return setting;
            }
            set
            {
                setting = value;
                visible_Check.Checked = setting.visible;
                showOrig_Check.Checked = setting.showOriginal;
                color_Button.SelectedColor = setting.color;
                OnSettingChanged(this, EventArgs.Empty);
            }
        }

        private void visible_Check_CheckedChanged(object sender, EventArgs e)
        {
            //if (setting != null)
            {
                setting.visible = visible_Check.Checked;
                color_Button.Enabled = visible_Check.Checked;
                showOrig_Check.Enabled = visible_Check.Checked;
                OnSettingChanged(this, EventArgs.Empty);
            }
        }

        private void showOrig_Check_CheckedChanged(object sender, EventArgs e)
        {
           // if (setting != null)
            {
                setting.showOriginal = showOrig_Check.Checked;
                OnSettingChanged(this, EventArgs.Empty);
            }
        }

        private void color_Button_SelectedColorChanged(object sender, EventArgs e)
        {
           // if (setting != null)
            {
                setting.color = color_Button.SelectedColor;
                OnSettingChanged(this, EventArgs.Empty);
            }
        }
        public event EventHandler SettingChanged;

        protected virtual void OnSettingChanged(object sender, EventArgs e)
        {
            if (SettingChanged != null)
                SettingChanged(sender, e);
        }
    }
}
