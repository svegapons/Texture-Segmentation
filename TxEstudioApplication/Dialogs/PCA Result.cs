using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TxEstudioKernel;

namespace TxEstudioApplication.Dialogs
{
    public partial class PCAResult : TxAppForm
    {
        
        TxPCA pca;
        public PCAResult(TxPCA pca)
        {
            this.pca = pca;
            InitializeComponent();
            FillEigenVals(pca.EigenVals);
            FillEigenVects(pca.EigenVectors);
        }

        private void FillEigenVals(TxVector vals)
        {
            double total = 0;
            for (int i = 0; i < vals.Length; i++)
            {
                total += vals[i];
            }
            double cumul = 0;
            double percent = 0;
            double cumulpercent = 0;
            for (int i = 0; i < vals.Length; i++)
            {
                cumul += vals[i];
                percent = (vals[i] / total) * 100;
                cumulpercent = (cumul / total) * 100;  
                EigenVals.Rows.Add(new object[]{Math.Round(vals[i],2),Math.Round(percent,2),Math.Round(cumulpercent,2)});
          
            }

              Size nsize  =new Size(EigenVals.Size.Width,EigenVals.ColumnHeadersHeight + 
               EigenVals.Rows.Count* (EigenVals.RowTemplate.DividerHeight +EigenVals.RowTemplate.Height)+ 30);


              if (nsize.Height > 250)
              {
                  nsize.Height = 250;
              }
            EigenVals.Size = nsize;


        }
        private void FillEigenVects(TxMatrix eigvects) 
        {
            DataGridViewColumn [] ColumnsHeaders = new DataGridViewColumn[eigvects.Width];

            for (int i = 0; i < ColumnsHeaders.Length; i++)
            {

                ColumnsHeaders[i] = new DataGridViewTextBoxColumn();
                ColumnsHeaders[i].HeaderText = "X" + i.ToString();
                ColumnsHeaders[i].Name = "X" + i.ToString();
                ColumnsHeaders[i].ReadOnly = true;
                ColumnsHeaders[i].Width = 50;
            }
            EigenVectors.Columns.AddRange(ColumnsHeaders);


            for (int i = 0; i < eigvects.Height; i++)
            {
                object[] row = new object[eigvects.Width];
                for (int j = 0; j < eigvects.Width; j++)
                {
                  row[j] =Math.Round(eigvects[i, j],2); 
                }

                EigenVectors.Rows.Add(row);
                EigenVectors.Rows[i].HeaderCell.Value = "CP" +"_" + ((int)(i + 1)).ToString();
            }
          
            Size  nsize = new Size(EigenVectors.RowHeadersWidth +

                                       eigvects.Width * (EigenVectors.RowTemplate.DividerHeight + 50),

                                     eigvects.Height*(EigenVectors.RowTemplate.DividerHeight + EigenVectors.RowTemplate.Height)+ EigenVectors.ColumnHeadersHeight);

            if (nsize.Height > 250)
            {
                nsize.Height = 250;
            }
            if (nsize.Width > 250)
            {
                nsize.Width = 250;
            }
            
            EigenVectors.Size = nsize;
        
        
        
        }


        //private void PlaceCheckBoxes(int count) 
        //{
        //    list = new CheckBox[count];
        //    int x = dataGridView1.Location.X + dataGridView1.Size.Width + 10;
        //    int y = dataGridView1.Location.Y + dataGridView1.ColumnHeadersHeight;

        //    for (int i = 0; i < count; i++)
        //    {
        //        list[i] = new CheckBox();
        //        list[i].AutoSize = true;
        //        list[i].Location = new System.Drawing.Point(x,y);
        //        list[i].Name ="CheckBox" + i.ToString();
        //        list[i].Size = new System.Drawing.Size(20,17);
        //        list[i].TabIndex = i;
        //        list[i].Text = "";
        //        list[i].UseVisualStyleBackColor = true;
        //        this.Controls.Add(list[i]);

        //        y += dataGridView1.RowTemplate.Height;
        //    } 

        
        //}

       
       
    }
}