using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TxEstudioKernel.VisualElements;

namespace TxEstudioApplication.Dialogs
{
    public partial class ExternalFiltersDialog : TxAppForm
    {
        public ExternalFiltersDialog()
        {
            InitializeComponent();
        }

        public override void SetEnvironment(Environment env)
        {
            base.SetEnvironment(env);
            imageSelectionBox.LoadEnvironment(env);
        }
        private Size getKernelSize()
        {
            int rows = 1, cols = 1;
            float current = 0f;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    current = (float)(kernelPanel.GetControlFromPosition(j, i) as RealParameterEditor).ParameterValue;
                    if (current != 0f)
                    {
                        rows = Math.Max(i, rows);
                        cols = Math.Max(j, cols);
                    }
                }

            return new Size((cols % 2 == 1) ? cols : cols + 1, (rows % 2 == 1) ? rows : rows + 1);
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader reader = new StreamReader(openFileDialog.OpenFile()))
                    {
                        string currentLine = "";
                        //Primera linea el nombre
                        nameBox.Text = reader.ReadLine();
                        //Segunda linea la abreviatura
                        abbreviationBox.Text = reader.ReadLine();
                        //Cantidad de filas
                        currentLine = reader.ReadLine();
                        int rows = int.Parse(currentLine.Substring(0, currentLine.IndexOf(':')));
                        //Cantidad de columnas
                        currentLine = reader.ReadLine();
                        int cols = int.Parse(currentLine.Substring(0, currentLine.IndexOf(':')));
                        if (cols > 9 || rows > 9)
                        {
                            env.ShowError("Filter size must not exceed 9x9.");
                            return;
                        }
                        //Factor
                        currentLine = reader.ReadLine();
                        float factor = float.Parse(currentLine.Substring(0, currentLine.IndexOf(':')), System.Globalization.NumberStyles.Float);
                        for (int i = 0; i < rows; i++)
                        {
                            int j = 0;
                            currentLine = reader.ReadLine();
                            foreach (string number in currentLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                (kernelPanel.GetControlFromPosition(j, i) as RealParameterEditor).ParameterValue = float.Parse(number) * factor;
                                j++;
                            }
                        }

                    }

                }
            }
            catch (Exception exc)
            {
                env.ShowError("Some error ocurred while opening the filter file.\n Message: " + exc.Message);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile()))
                    {
                        writer.WriteLine(nameBox.Text);
                        writer.WriteLine(abbreviationBox.Text);
                        Size kernelSize = getKernelSize();
                        writer.WriteLine(string.Format("{0}:Number of rows", kernelSize.Height));
                        writer.WriteLine(string.Format("{0}:Number of columns", kernelSize.Width));
                        writer.WriteLine(string.Format("{0}:Amplitude factor", factorBox.ParameterValue));
                        for (int i = 0; i < kernelSize.Height; i++)
                        {
                            for (int j = 0; j < kernelSize.Width; j++)
                            {
                                writer.Write(((float)(kernelPanel.GetControlFromPosition(j, i) as RealParameterEditor).ParameterValue).ToString());
                                writer.Write(" ");
                            }
                            writer.WriteLine();
                        }

                    }
                }
            }
            catch (Exception exc)
            {
                env.ShowError("Some error ocurred while saving the filter file.\nMessage: " + exc.Message);
            }
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            if (imageSelectionBox.SelectedExposer != null)
            {
                float[,] kernel;
                float amplitude = (float)factorBox.ParameterValue;
                Size kernelSize = getKernelSize();
                kernel = new float[kernelSize.Height, kernelSize.Width];
                for (int i = 0; i < kernelSize.Height; i++)
                    for (int j = 0; j < kernelSize.Width; j++)
                        kernel[i, j] = amplitude * (float)(kernelPanel.GetControlFromPosition(j, i) as RealParameterEditor).ParameterValue;
                ImageHolderForm newForm = new ImageHolderForm();
                newForm.Image = imageSelectionBox.SelectedImage.Convolve(new TxEstudioKernel.TxMatrix(kernel));
                newForm.ImageName = imageSelectionBox.SelectedExposer.ImageName + "+ef_" + abbreviationBox.Text;
                env.OpenWindow(newForm);
            }
        }

        private void ExternalFiltersDialog_Load(object sender, EventArgs e)
        {
            foreach (RealParameterEditor editor in kernelPanel.Controls)
                editor.ParameterValue = 0f;
            (kernelPanel.GetControlFromPosition(0, 0) as RealParameterEditor).ParameterValue = 1f;
            factorBox.ParameterValue = 1f;
            
        }
    }
}