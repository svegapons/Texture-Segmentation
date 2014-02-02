namespace TxEstudioKernel.VisualElements
{
    partial class GeneralParameterEditor
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
            this.components = new System.ComponentModel.Container();
            this.ellipsis_Button = new System.Windows.Forms.Button();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // ellipsis_Button
            // 
            this.ellipsis_Button.AutoEllipsis = true;
            this.ellipsis_Button.Dock = System.Windows.Forms.DockStyle.Right;
            this.ellipsis_Button.Location = new System.Drawing.Point(100, 0);
            this.ellipsis_Button.Name = "ellipsis_Button";
            this.ellipsis_Button.Size = new System.Drawing.Size(19, 19);
            this.ellipsis_Button.TabIndex = 0;
            this.ellipsis_Button.Text = "...";
            this.ellipsis_Button.UseVisualStyleBackColor = true;
            this.ellipsis_Button.Click += new System.EventHandler(this.ellipsis_Button_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.ShowImageMargin = false;
            this.contextMenuStrip.ShowItemToolTips = false;
            this.contextMenuStrip.Size = new System.Drawing.Size(36, 4);
            this.contextMenuStrip.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.contextMenuStrip_Closed);
            // 
            // GeneralParameterEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ellipsis_Button);
            this.MaximumSize = new System.Drawing.Size(300, 19);
            this.MinimumSize = new System.Drawing.Size(19, 19);
            this.Name = "GeneralParameterEditor";
            this.Size = new System.Drawing.Size(119, 19);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ellipsis_Button;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
    }
}
