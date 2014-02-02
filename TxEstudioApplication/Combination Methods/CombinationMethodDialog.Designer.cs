namespace TxEstudioApplication.Combination_Methods
{
    partial class CombinationMethodDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.algorithmViewer = new TxEstudioKernel.VisualElements.AlgorithmViewer();
            this.progress_Label = new System.Windows.Forms.Label();
            this.textures_Check = new System.Windows.Forms.CheckBox();
            this.tones_Check = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.textures_Box = new System.Windows.Forms.GroupBox();
            this.descriptor_Viewer = new TxEstudioKernel.VisualElements.AlgorithmViewer();
            this.descriptors_Tree = new System.Windows.Forms.TreeView();
            this.tones_Group = new System.Windows.Forms.GroupBox();
            this.tones_SelectionBox = new TxEstudioApplication.Custom_Controls.ImageMultiSelectionBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.imageSelectionBox = new TxEstudioApplication.Custom_Controls.ImageSelectionBox();
            this.close_Button = new System.Windows.Forms.Button();
            this.apply_Button = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bt_selectImage = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.bt_Select = new System.Windows.Forms.Button();
            this.rb_useOther = new System.Windows.Forms.RadioButton();
            this.bt_borderMap = new System.Windows.Forms.Button();
            this.rb_borderMap = new System.Windows.Forms.RadioButton();
            this.textures_Box.SuspendLayout();
            this.tones_Group.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // algorithmViewer
            // 
            this.algorithmViewer.AlgorithmInstance = null;
            this.algorithmViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.algorithmViewer.Location = new System.Drawing.Point(568, 24);
            this.algorithmViewer.Margin = new System.Windows.Forms.Padding(3, 3, 3, 35);
            this.algorithmViewer.Name = "algorithmViewer";
            this.algorithmViewer.ShowDescription = true;
            this.algorithmViewer.Size = new System.Drawing.Size(201, 346);
            this.algorithmViewer.TabIndex = 23;
            this.algorithmViewer.ViewParameterDescription = true;
            // 
            // progress_Label
            // 
            this.progress_Label.AutoSize = true;
            this.progress_Label.Location = new System.Drawing.Point(15, 546);
            this.progress_Label.Name = "progress_Label";
            this.progress_Label.Size = new System.Drawing.Size(0, 13);
            this.progress_Label.TabIndex = 70;
            // 
            // textures_Check
            // 
            this.textures_Check.AutoSize = true;
            this.textures_Check.Location = new System.Drawing.Point(245, 344);
            this.textures_Check.Name = "textures_Check";
            this.textures_Check.Size = new System.Drawing.Size(67, 17);
            this.textures_Check.TabIndex = 66;
            this.textures_Check.Text = "Textures";
            this.textures_Check.UseVisualStyleBackColor = true;
            this.textures_Check.CheckedChanged += new System.EventHandler(this.textures_Check_CheckedChanged);
            // 
            // tones_Check
            // 
            this.tones_Check.AutoSize = true;
            this.tones_Check.Location = new System.Drawing.Point(244, 149);
            this.tones_Check.Name = "tones_Check";
            this.tones_Check.Size = new System.Drawing.Size(56, 17);
            this.tones_Check.TabIndex = 65;
            this.tones_Check.Text = "Tones";
            this.tones_Check.UseVisualStyleBackColor = true;
            this.tones_Check.CheckedChanged += new System.EventHandler(this.tones_Check_CheckedChanged);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar.Location = new System.Drawing.Point(12, 562);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(257, 10);
            this.progressBar.TabIndex = 69;
            // 
            // textures_Box
            // 
            this.textures_Box.AutoSize = true;
            this.textures_Box.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.textures_Box.Controls.Add(this.descriptor_Viewer);
            this.textures_Box.Controls.Add(this.descriptors_Tree);
            this.textures_Box.Enabled = false;
            this.textures_Box.Location = new System.Drawing.Point(238, 347);
            this.textures_Box.Margin = new System.Windows.Forms.Padding(3, 3, 3, 35);
            this.textures_Box.Name = "textures_Box";
            this.textures_Box.Size = new System.Drawing.Size(294, 196);
            this.textures_Box.TabIndex = 68;
            this.textures_Box.TabStop = false;
            this.textures_Box.Text = "groupBox3";
            // 
            // descriptor_Viewer
            // 
            this.descriptor_Viewer.AlgorithmInstance = null;
            this.descriptor_Viewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.descriptor_Viewer.Location = new System.Drawing.Point(151, 19);
            this.descriptor_Viewer.Name = "descriptor_Viewer";
            this.descriptor_Viewer.ShowDescription = false;
            this.descriptor_Viewer.Size = new System.Drawing.Size(137, 158);
            this.descriptor_Viewer.TabIndex = 28;
            this.descriptor_Viewer.ViewParameterDescription = true;
            // 
            // descriptors_Tree
            // 
            this.descriptors_Tree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.descriptors_Tree.CheckBoxes = true;
            this.descriptors_Tree.Location = new System.Drawing.Point(6, 19);
            this.descriptors_Tree.Name = "descriptors_Tree";
            this.descriptors_Tree.ShowLines = false;
            this.descriptors_Tree.Size = new System.Drawing.Size(139, 170);
            this.descriptors_Tree.TabIndex = 27;
            this.descriptors_Tree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.descriptors_Tree_AfterCheck);
            this.descriptors_Tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.descriptors_Tree_AfterSelect);
            // 
            // tones_Group
            // 
            this.tones_Group.Controls.Add(this.tones_SelectionBox);
            this.tones_Group.Enabled = false;
            this.tones_Group.Location = new System.Drawing.Point(237, 153);
            this.tones_Group.Name = "tones_Group";
            this.tones_Group.Size = new System.Drawing.Size(295, 184);
            this.tones_Group.TabIndex = 67;
            this.tones_Group.TabStop = false;
            this.tones_Group.Text = "groupBox2";
            // 
            // tones_SelectionBox
            // 
            this.tones_SelectionBox.Location = new System.Drawing.Point(6, 19);
            this.tones_SelectionBox.Name = "tones_SelectionBox";
            this.tones_SelectionBox.Size = new System.Drawing.Size(288, 159);
            this.tones_SelectionBox.TabIndex = 25;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.imageSelectionBox);
            this.groupBox1.Location = new System.Drawing.Point(18, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(166, 168);
            this.groupBox1.TabIndex = 64;
            this.groupBox1.TabStop = false;
            // 
            // imageSelectionBox
            // 
            this.imageSelectionBox.Location = new System.Drawing.Point(6, 12);
            this.imageSelectionBox.Name = "imageSelectionBox";
            this.imageSelectionBox.Size = new System.Drawing.Size(156, 150);
            this.imageSelectionBox.TabIndex = 13;
            this.imageSelectionBox.ItemChanged += new System.EventHandler(this.imageSelectionBox_ItemChanged);
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.close_Button.Location = new System.Drawing.Point(687, 549);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(75, 23);
            this.close_Button.TabIndex = 63;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            // 
            // apply_Button
            // 
            this.apply_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply_Button.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.apply_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apply_Button.Location = new System.Drawing.Point(606, 549);
            this.apply_Button.Name = "apply_Button";
            this.apply_Button.Size = new System.Drawing.Size(75, 23);
            this.apply_Button.TabIndex = 62;
            this.apply_Button.Text = "Apply";
            this.apply_Button.UseVisualStyleBackColor = true;
            this.apply_Button.Click += new System.EventHandler(this.apply_Button_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.bt_selectImage);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Controls.Add(this.bt_Select);
            this.groupBox2.Controls.Add(this.rb_useOther);
            this.groupBox2.Controls.Add(this.bt_borderMap);
            this.groupBox2.Controls.Add(this.rb_borderMap);
            this.groupBox2.Location = new System.Drawing.Point(238, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(293, 131);
            this.groupBox2.TabIndex = 71;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Border map";
            // 
            // bt_selectImage
            // 
            this.bt_selectImage.Enabled = false;
            this.bt_selectImage.Location = new System.Drawing.Point(188, 88);
            this.bt_selectImage.Name = "bt_selectImage";
            this.bt_selectImage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bt_selectImage.Size = new System.Drawing.Size(75, 23);
            this.bt_selectImage.TabIndex = 7;
            this.bt_selectImage.Text = "Select...";
            this.bt_selectImage.UseVisualStyleBackColor = true;
            this.bt_selectImage.Click += new System.EventHandler(this.bt_selectImage_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(13, 91);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(133, 17);
            this.radioButton1.TabIndex = 6;
            this.radioButton1.Text = "Select image as border";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // bt_Select
            // 
            this.bt_Select.Enabled = false;
            this.bt_Select.Location = new System.Drawing.Point(188, 55);
            this.bt_Select.Name = "bt_Select";
            this.bt_Select.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bt_Select.Size = new System.Drawing.Size(75, 23);
            this.bt_Select.TabIndex = 5;
            this.bt_Select.Text = "Select...";
            this.bt_Select.UseVisualStyleBackColor = true;
            this.bt_Select.Click += new System.EventHandler(this.bt_Select_Click);
            // 
            // rb_useOther
            // 
            this.rb_useOther.AutoSize = true;
            this.rb_useOther.Location = new System.Drawing.Point(13, 58);
            this.rb_useOther.Name = "rb_useOther";
            this.rb_useOther.Size = new System.Drawing.Size(113, 17);
            this.rb_useOther.TabIndex = 4;
            this.rb_useOther.Text = "Use edge detector";
            this.rb_useOther.UseVisualStyleBackColor = true;
            this.rb_useOther.CheckedChanged += new System.EventHandler(this.rb_useOther_CheckedChanged);
            // 
            // bt_borderMap
            // 
            this.bt_borderMap.Enabled = false;
            this.bt_borderMap.Location = new System.Drawing.Point(188, 23);
            this.bt_borderMap.Name = "bt_borderMap";
            this.bt_borderMap.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bt_borderMap.Size = new System.Drawing.Size(75, 23);
            this.bt_borderMap.TabIndex = 3;
            this.bt_borderMap.Text = "Draw...";
            this.bt_borderMap.UseVisualStyleBackColor = true;
            this.bt_borderMap.Click += new System.EventHandler(this.bt_borderMap_Click);
            // 
            // rb_borderMap
            // 
            this.rb_borderMap.AutoSize = true;
            this.rb_borderMap.Location = new System.Drawing.Point(13, 26);
            this.rb_borderMap.Name = "rb_borderMap";
            this.rb_borderMap.Size = new System.Drawing.Size(106, 17);
            this.rb_borderMap.TabIndex = 1;
            this.rb_borderMap.Text = "Draw border map";
            this.rb_borderMap.UseVisualStyleBackColor = true;
            this.rb_borderMap.CheckedChanged += new System.EventHandler(this.rb_borderMap_CheckedChanged);
            // 
            // CombinationMethodDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(787, 584);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.progress_Label);
            this.Controls.Add(this.textures_Check);
            this.Controls.Add(this.tones_Check);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.textures_Box);
            this.Controls.Add(this.tones_Group);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.close_Button);
            this.Controls.Add(this.apply_Button);
            this.Controls.Add(this.algorithmViewer);
            this.Name = "CombinationMethodDialog";
            this.textures_Box.ResumeLayout(false);
            this.tones_Group.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TxEstudioKernel.VisualElements.AlgorithmViewer algorithmViewer;
        protected System.Windows.Forms.Label progress_Label;
        protected System.Windows.Forms.CheckBox textures_Check;
        protected System.Windows.Forms.CheckBox tones_Check;
        protected System.Windows.Forms.ProgressBar progressBar;
        protected System.Windows.Forms.GroupBox textures_Box;
        protected TxEstudioKernel.VisualElements.AlgorithmViewer descriptor_Viewer;
        protected System.Windows.Forms.TreeView descriptors_Tree;
        protected System.Windows.Forms.GroupBox tones_Group;
        protected TxEstudioApplication.Custom_Controls.ImageMultiSelectionBox tones_SelectionBox;
        protected System.Windows.Forms.GroupBox groupBox1;
        protected TxEstudioApplication.Custom_Controls.ImageSelectionBox imageSelectionBox;
        protected System.Windows.Forms.Button close_Button;
        protected System.Windows.Forms.Button apply_Button;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button bt_borderMap;
        private System.Windows.Forms.RadioButton rb_borderMap;
        private System.Windows.Forms.Button bt_Select;
        private System.Windows.Forms.RadioButton rb_useOther;
        private System.Windows.Forms.Button bt_selectImage;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}
