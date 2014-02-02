namespace TxEstudioApplication.Custom_Controls
{
    partial class ImageMultiSelectionBox
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
            this.pool = new System.Windows.Forms.ListBox();
            this.targets = new System.Windows.Forms.ListBox();
            this.add_Button = new System.Windows.Forms.Button();
            this.remove_Button = new System.Windows.Forms.Button();
            this.addAll_Button = new System.Windows.Forms.Button();
            this.remAll_Button = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // pool
            // 
            this.pool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pool.DisplayMember = "ImageName";
            this.pool.FormattingEnabled = true;
            this.pool.HorizontalScrollbar = true;
            this.pool.Location = new System.Drawing.Point(4, 4);
            this.pool.Name = "pool";
            this.pool.Size = new System.Drawing.Size(120, 147);
            this.pool.TabIndex = 0;
            this.pool.SelectedIndexChanged += new System.EventHandler(this.pool_SelectedIndexChanged);
            // 
            // targets
            // 
            this.targets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.targets.DisplayMember = "ImageName";
            this.targets.FormattingEnabled = true;
            this.targets.HorizontalScrollbar = true;
            this.targets.Location = new System.Drawing.Point(162, 4);
            this.targets.Name = "targets";
            this.targets.Size = new System.Drawing.Size(120, 147);
            this.targets.TabIndex = 1;
            this.targets.SelectedIndexChanged += new System.EventHandler(this.targets_SelectedIndexChanged);
            // 
            // add_Button
            // 
            this.add_Button.Image = global::TxEstudioApplication.Properties.Resources.right_arrow;
            this.add_Button.Location = new System.Drawing.Point(130, 4);
            this.add_Button.Name = "add_Button";
            this.add_Button.Size = new System.Drawing.Size(26, 26);
            this.add_Button.TabIndex = 2;
            this.toolTip.SetToolTip(this.add_Button, "Add");
            this.add_Button.UseVisualStyleBackColor = true;
            this.add_Button.Click += new System.EventHandler(this.add_Button_Click);
            // 
            // remove_Button
            // 
            this.remove_Button.Image = global::TxEstudioApplication.Properties.Resources.left_arrow;
            this.remove_Button.Location = new System.Drawing.Point(130, 36);
            this.remove_Button.Name = "remove_Button";
            this.remove_Button.Size = new System.Drawing.Size(26, 26);
            this.remove_Button.TabIndex = 3;
            this.toolTip.SetToolTip(this.remove_Button, "Remove");
            this.remove_Button.UseVisualStyleBackColor = true;
            this.remove_Button.Click += new System.EventHandler(this.remove_Button_Click);
            // 
            // addAll_Button
            // 
            this.addAll_Button.Image = global::TxEstudioApplication.Properties.Resources.double_arrow2;
            this.addAll_Button.Location = new System.Drawing.Point(131, 93);
            this.addAll_Button.Name = "addAll_Button";
            this.addAll_Button.Size = new System.Drawing.Size(26, 26);
            this.addAll_Button.TabIndex = 4;
            this.toolTip.SetToolTip(this.addAll_Button, "Add all");
            this.addAll_Button.UseVisualStyleBackColor = true;
            this.addAll_Button.Click += new System.EventHandler(this.addAll_Button_Click);
            // 
            // remAll_Button
            // 
            this.remAll_Button.Image = global::TxEstudioApplication.Properties.Resources.double_left_arrow;
            this.remAll_Button.Location = new System.Drawing.Point(130, 125);
            this.remAll_Button.Name = "remAll_Button";
            this.remAll_Button.Size = new System.Drawing.Size(26, 26);
            this.remAll_Button.TabIndex = 5;
            this.toolTip.SetToolTip(this.remAll_Button, "Remove all");
            this.remAll_Button.UseVisualStyleBackColor = true;
            this.remAll_Button.Click += new System.EventHandler(this.remAll_Button_Click);
            // 
            // ImageMultiSelectionBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.remAll_Button);
            this.Controls.Add(this.addAll_Button);
            this.Controls.Add(this.remove_Button);
            this.Controls.Add(this.add_Button);
            this.Controls.Add(this.targets);
            this.Controls.Add(this.pool);
            this.Name = "ImageMultiSelectionBox";
            this.Size = new System.Drawing.Size(288, 159);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox pool;
        private System.Windows.Forms.ListBox targets;
        private System.Windows.Forms.Button add_Button;
        private System.Windows.Forms.Button remove_Button;
        private System.Windows.Forms.Button addAll_Button;
        private System.Windows.Forms.Button remAll_Button;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
