namespace TxEstudioKernel.VisualElements
{
    partial class AlgorithmViewer
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
            this.name_Label = new System.Windows.Forms.Label();
            this.description_Label = new System.Windows.Forms.Label();
            this.description_TextBox = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.parameters_Label = new System.Windows.Forms.Label();
            this.paramDsc_TextBox = new System.Windows.Forms.TextBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.parameters_Table = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // name_Label
            // 
            this.name_Label.AutoEllipsis = true;
            this.name_Label.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.name_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.name_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name_Label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.name_Label.Location = new System.Drawing.Point(0, 0);
            this.name_Label.Name = "name_Label";
            this.name_Label.Size = new System.Drawing.Size(194, 13);
            this.name_Label.TabIndex = 0;
            this.name_Label.Text = "Operator name:";
            // 
            // description_Label
            // 
            this.description_Label.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.description_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.description_Label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.description_Label.Location = new System.Drawing.Point(0, 13);
            this.description_Label.Name = "description_Label";
            this.description_Label.Size = new System.Drawing.Size(194, 16);
            this.description_Label.TabIndex = 1;
            this.description_Label.Text = "Description:";
            // 
            // description_TextBox
            // 
            this.description_TextBox.CausesValidation = false;
            this.description_TextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.description_TextBox.Location = new System.Drawing.Point(0, 29);
            this.description_TextBox.Multiline = true;
            this.description_TextBox.Name = "description_TextBox";
            this.description_TextBox.ReadOnly = true;
            this.description_TextBox.Size = new System.Drawing.Size(194, 58);
            this.description_TextBox.TabIndex = 2;
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 87);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(194, 3);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // parameters_Label
            // 
            this.parameters_Label.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.parameters_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.parameters_Label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.parameters_Label.Location = new System.Drawing.Point(0, 90);
            this.parameters_Label.Name = "parameters_Label";
            this.parameters_Label.Size = new System.Drawing.Size(194, 16);
            this.parameters_Label.TabIndex = 4;
            this.parameters_Label.Text = "Parameters:";
            // 
            // paramDsc_TextBox
            // 
            this.paramDsc_TextBox.CausesValidation = false;
            this.paramDsc_TextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.paramDsc_TextBox.Location = new System.Drawing.Point(0, 296);
            this.paramDsc_TextBox.Multiline = true;
            this.paramDsc_TextBox.Name = "paramDsc_TextBox";
            this.paramDsc_TextBox.ReadOnly = true;
            this.paramDsc_TextBox.Size = new System.Drawing.Size(194, 37);
            this.paramDsc_TextBox.TabIndex = 6;
            // 
            // splitter2
            // 
            this.splitter2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 293);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(194, 3);
            this.splitter2.TabIndex = 7;
            this.splitter2.TabStop = false;
            // 
            // parameters_Table
            // 
            this.parameters_Table.AutoScroll = true;
            this.parameters_Table.ColumnCount = 2;
            this.parameters_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.parameters_Table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.parameters_Table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parameters_Table.Location = new System.Drawing.Point(0, 106);
            this.parameters_Table.Name = "parameters_Table";
            this.parameters_Table.RowCount = 1;
            this.parameters_Table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.parameters_Table.Size = new System.Drawing.Size(194, 187);
            this.parameters_Table.TabIndex = 8;
            // 
            // AlgorithmViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.parameters_Table);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.paramDsc_TextBox);
            this.Controls.Add(this.parameters_Label);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.description_TextBox);
            this.Controls.Add(this.description_Label);
            this.Controls.Add(this.name_Label);
            this.Name = "AlgorithmViewer";
            this.Size = new System.Drawing.Size(194, 333);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label name_Label;
        private System.Windows.Forms.Label description_Label;
        private System.Windows.Forms.TextBox description_TextBox;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Label parameters_Label;
        private System.Windows.Forms.TextBox paramDsc_TextBox;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.TableLayoutPanel parameters_Table;
    }
}
