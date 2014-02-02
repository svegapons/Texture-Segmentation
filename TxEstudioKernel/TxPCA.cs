using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms; 
using TxEstudioKernel.OpenCV;
using TxEstudioKernel.CustomAttributes;
namespace TxEstudioKernel
{
    [Algorithm("Principal Components Analysis", "Perform principal components analysis based on the covariance matrix")]

    [Abbreviation("PCA","Percent")]
    public class TxPCA:TxGeneral
    {
        int stdv_percent = 85;       
        [Parameter("%_Variance", "")]
        [IntegerInSequence(0, 100, 2)]
        public int Percent
        {
            get
            {
                return stdv_percent;
            }
            set
            {
                stdv_percent = value;
            }
        }
        
        TxVector medians;
        TxVector eigvals; 
        TxMatrix eigvects;
        private ShowRes Show;
        public override List<TxImage> Process(params TxImage[] parameters)
        {
            medians = new TxVector(parameters.Length);
            
            TxMatrix data = RowDataAdjust(parameters);

            TxMatrix cov = CovarianceMatrix(data);

                     eigvects = new TxMatrix(cov.Height,cov.Width);
                     eigvals  = new TxVector(cov.Width);

            cov.ComputeEigenVV(eigvects,eigvals,0.00005);

            int count = Overflow(((double)stdv_percent / 100));

            TxMatrix sel_vects = eigvects.GetRows(0,count);

            TxMatrix finaldata = sel_vects * data;

            //TxMatrix orig_data = TxMatrix.Transpose(sel_vects) * finaldata;

            //TxMatrix final_data = AddMeans(orig_data, medians);

            Normalice (finaldata);

            Show(this);
            return Reconstruct(finaldata,count,parameters[0].Width,parameters[0].Height)  ;
        }

           
        //Fill a Matrix with  each row vector formed from an image wiht the median substracted 
        private  TxMatrix  RowDataAdjust( TxImage[] parameters )
      {
            int image_lenght = parameters[0].Height* parameters[0].Width;
            int height = parameters [0].Height;
            int width  = parameters[0].Width;
         
          TxMatrix data = new TxMatrix( parameters.Length , image_lenght );


          for (int i = 0; i < parameters.Length; i++)
          {
              TxImage current = parameters[i];
               
              _cvScalar temp = CXCore.cvAvg( current.InnerImage,(IntPtr)0);
              
              double med = temp.val0;
              medians[i] = (float) med;
             
              for (int j = 0; j < height ; j++)
              {
                  for (int k = 0; k < width; k++)
                  {
                      data[i, j * width + k] =( float )( current[k, j] - med);

                  }

              }                 

          }

          return data;
                  
      }
        private  TxMatrix CovarianceMatrix(TxMatrix data) 
        {
            int n = data.Height;  
            TxMatrix covariance =  new TxMatrix ( n , n );
            for (int i = 0; i < n ; i++)
            {
                for (int j = i; j < n; j++)
                {

                    double cov = Covariance(data,i, j);

                    covariance[i, j] =(float) cov;
                    covariance[j, i] =(float) cov;
                }
            }
            return covariance;
        
        }
        private double Covariance( TxMatrix data ,int i,int j) 
        {
            double cov = 0 ;
            int n = data.Width ;
            for (int k = 0; k < n; k++)
            {
                cov += data[i, k] * data[j, k];   
            }
        
            cov = cov/(n-1);

            return cov; 

        }
        private int Overflow(double val) 
        {
            double total = 0;
            double cumul = 0;

            for (int i = 0; i < eigvals.Length; i++)
            {
                total += eigvals[i];
            }
            
            for (int i = 0; i < eigvals.Length; i++)
            {
                cumul += eigvals[i]/total;
                
                if (cumul >= val)
                {
                    return i + 1;
                }
            }

            return eigvals.Length;
        }


        private TxMatrix AddMeans(TxMatrix data,TxMatrix eigvects,TxVector means) 
        {
            for (int i = 0; i < data.Height;i++)
            {
                float mean = 0;
                for (int k = 0; k < eigvects.Width ; k++)
                {
                    mean += eigvects[i, k] * means[k];
                }
                    
                for (int j = 0; j < data.Width;j ++)
                {
                    data[i, j] += mean;

                    if (data[i, j] > 255)
                    {
                        data[i, j] = 255;
                    }
                    else if( data[i,j]< 0  )
                    {
                        data[i, j] = 0;
                    }
                }
            }
            return data;
        
        }
        private void Normalice(TxMatrix data)
        {
            float min = float.MaxValue;
            float max = float.MinValue;
            float val = 0;
            float dif = 0;
            for (int i = 0; i < data.Height; i++)
            {

                for (int j = 0; j < data.Width; j++)
                {
                    if (data[i, j] > max )
                    {
                        max = data[i, j];
                    }
                    else if (data[i, j] < min)
                    {
                       min = data[i, j];
                    }
                }
                
               
            }

            dif = max - min;

            for (int i = 0; i < data.Height; i++)
            {
                   for (int j = 0; j < data.Width; j++)
                {
                    data[i, j] = ((data[i, j] - min) / dif) * 255;
                }
            
            }
          
        }


        private List<TxImage> Reconstruct(TxMatrix data,int count,int width ,int height) 
        {
            List<TxImage> res = new List<TxImage>(count);
            
            unsafe{

                  for (int k = 0; k < count ; k++)
                 {
                     res.Add( new TxImage(width,height,TxImageFormat.GrayScale));
                
                      _IplImage* innerImage = (_IplImage*)res[k].InnerImage;

                      TxImage actual = res[k];
                       int offset = innerImage->widthStep - innerImage->width;

                       byte* current =(byte*) innerImage->imageData;

                       int length = width * height;

                       int x;
                       int y;
                       for (int i = 0; i < length; i++)
                       {
                           x = i % width;
                           y = i / width;

                           actual[x,y] =(byte) data[k,i];
                       }

                  }
            
            
                    }

                    return res;
          }

        public TxMatrix EigenVectors 
        {
            get { return this.eigvects; }
        }
        public TxVector EigenVals 
        {
            get { return this.eigvals; }
        }
        public ShowRes ResultViewer 
        {
            get { return this.Show; }
            set { this.Show = value; }
        }  
    }
    public delegate void ShowRes(TxPCA res); 
}
