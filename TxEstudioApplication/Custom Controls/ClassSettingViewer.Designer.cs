namespace TxEstudioApplication.Custom_Controls
{
    partial class ClassSettingViewer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.visible_Check = new System.Windows.Forms.CheckBox();
            this.showOrig_Check = new System.Windows.Forms.CheckBox();
            this.color_Button = new Janus.Windows.EditControls.UIColorButton();
            this.SuspendLayout();
            // 
            // visible_Check
            // 
            this.visible_Check.AutoSize = true;
            this.visible_Check.Location = new System.Drawing.Point(19, 7);
            this.visible_Check.Name = "visible_Check";
            this.visible_Check.Size = new System.Drawing.Size(15, 14);
            this.visible_Check.TabIndex = 0;
            this.visible_Check.UseVisualStyleBackColor = true;
            this.visible_Check.CheckedChanged += new System.EventHandler(this.visible_Check_CheckedChanged);
            // 
            // showOrig_Check
            // 
            this.showOrig_Check.AutoSize = true;
            this.showOrig_Check.Location = new System.Drawing.Point(123, 7);
            this.showOrig_Check.Name = "showOrig_Check";
            this.showOrig_Check.Size = new System.Drawing.Size(15, 14);
            this.showOrig_Check.TabIndex = 1;
            this.showOrig_Check.UseVisualStyleBackColor = true;
            this.showOrig_Check.CheckedChanged += new System.EventHandler(this.showOrig_Check_CheckedChanged);
            // 
            // color_Button
            // 
            // 
            // 
            // 
            this.color_Button.ColorPicker.Location = new System.Drawing.Point(0, 0);
            this.color_Button.ColorPicker.Name = "";
            this.color_Button.ColorPicker.Size = new System.Drawing.Size(100, 100);
            this.color_Button.ColorPicker.TabIndex = 0;
            this.color_Button.ImageReplaceableColor = System.Drawing.Color.Empty;
            this.color_Button.Location = new System.Drawing.Point(56, 3);
            this.color_Button.Name = "color_Button";
            this.color_Button.Size = new System.Drawing.Size(45, 23);
            this.color_Button.TabIndex = 2;
            this.color_Button.UseColorName = false;
            this.color_Button.SelectedColorChanged += new System.EventHandler(this.color_Button_SelectedColorChanged);
            // 
            // ClassSettingViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.color_Button);
            this.Controls.Add(this.showOrig_Check);
            this.Controls.Add(this.visible_Check);
            this.MaximumSize = new System.Drawing.Size(157, 28);
            this.MinimumSize = new System.Drawing.Size(157, 28);
            this.Name = "ClassSettingViewer";
            this.Size = new System.Drawing.Size(157, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox visible_Check;
        private System.Windows.Forms.CheckBox showOrig_Check;
        private Janus.Windows.EditControls.UIColorButton color_Button;
    }
}
