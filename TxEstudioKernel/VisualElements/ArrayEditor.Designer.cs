namespace TxEstudioKernel.VisualElements
{
    partial class ArrayEditor
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
            this.newValue_Editor = new TxEstudioKernel.VisualElements.RealParameterEditor();
            this.listBox = new System.Windows.Forms.ListBox();
            this.add_Button = new System.Windows.Forms.Button();
            this.remove_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // newValue_Editor
            // 
            this.newValue_Editor.Location = new System.Drawing.Point(4, 4);
            this.newValue_Editor.Maximum = 3.402823E+38F;
            this.newValue_Editor.Minimum = -3.402823E+38F;
            this.newValue_Editor.Name = "newValue_Editor";
            this.newValue_Editor.ParameterValue = 0F;
            this.newValue_Editor.Size = new System.Drawing.Size(75, 20);
            this.newValue_Editor.TabIndex = 0;
            this.newValue_Editor.Text = "0.0";
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(96, 4);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(76, 69);
            this.listBox.TabIndex = 1;
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
            // 
            // add_Button
            // 
            this.add_Button.Location = new System.Drawing.Point(16, 30);
            this.add_Button.Name = "add_Button";
            this.add_Button.Size = new System.Drawing.Size(25, 23);
            this.add_Button.TabIndex = 2;
            this.add_Button.Text = "+";
            this.add_Button.UseVisualStyleBackColor = true;
            this.add_Button.Click += new System.EventHandler(this.add_Button_Click);
            // 
            // remove_Button
            // 
            this.remove_Button.Enabled = false;
            this.remove_Button.Location = new System.Drawing.Point(47, 30);
            this.remove_Button.Name = "remove_Button";
            this.remove_Button.Size = new System.Drawing.Size(24, 23);
            this.remove_Button.TabIndex = 3;
            this.remove_Button.Text = "-";
            this.remove_Button.UseVisualStyleBackColor = true;
            this.remove_Button.Click += new System.EventHandler(this.remove_Button_Click);
            // 
            // ArrayEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.remove_Button);
            this.Controls.Add(this.add_Button);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.newValue_Editor);
            this.Name = "ArrayEditor";
            this.Size = new System.Drawing.Size(179, 78);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RealParameterEditor newValue_Editor;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Button add_Button;
        private System.Windows.Forms.Button remove_Button;
    }
}
