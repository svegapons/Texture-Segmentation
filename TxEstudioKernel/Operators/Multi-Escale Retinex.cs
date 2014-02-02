using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{

    [DigitalFilter]
    [Algorithm("Multi Scale Retinex", "Isotropic Filter to obtain the image invariant illumination.")]
    [Abbreviation("ms_ret","NCol","NRow")]
    public class Multi_Escale_Retinex :  TxOneBand
    {
        float[] sigmas = new float[] { };
        float[] pesos = new float[] { };

        public Multi_Escale_Retinex() { }
       
        int nrow = 3;
        [Parameter("Windows Size y", "")]
        [IntegerInSequence(1, int.MaxValue, 2)]
        public int Nrow
        {
            get { return nrow; }
            set { nrow = value; }
        }
        int ncol = 3;
        [Parameter("Windows Size x", "")]
        [IntegerInSequence(1, int.MaxValue, 2)]
        public int Ncol
        {
            get { return ncol; }
            set { ncol = value; }
        }
        
        public Multi_Escale_Retinex( float[] sigmas,  float[] pesos):base()
        {
            float sumpesos = 0;
            for (int i = 0; i < pesos.Length; i++)
                sumpesos += pesos[i];
            if (sumpesos != 1.0) throw new ArgumentException("Los pesos tienen que sumar 1");
            if (sigmas.Length != pesos.Length) throw new ArgumentException("Deben haber la misma cantidad de sigmas que de pesos");

            this.sigmas = sigmas;
            this.pesos = pesos;
        }

        public  TxImage Filtrado(TxImage imagen)
        {
            
            TxImage result;

            TxImage suma=new TxImage(imagen.Width,imagen.Height, TxImageFormat.GrayScale);
            for (int i = 0; i < suma.Width; i++)
                for (int j = 0; j < suma.Height; j++)
                    suma[i, j] = 0;
            
            GaussianFilter filter = new GaussianFilter();
            filter.nCols = ncol;
            filter.nRows = nrow;
            for (int i = 0; i < sigmas.Length; i++)
            {
                
                filter.Stdv = sigmas[i];
                result = filter.Process((TxImage)imagen.Clone());
                suma = SumaImagenesPeso(result, suma, pesos[i]);
                
            }


            TxMatrix resultado = (DivideImagenes(imagen, suma));
            resultado = NormalizaImagen(resultado);
            return resultado.ToImage();         
                
        }

       
        private TxImage SumaImagenesPeso(TxImage imagen,TxImage dest,float peso)
        {
            TxImage result = new TxImage(imagen.Width, imagen.Height, TxImageFormat.GrayScale);
            
            for (int j = 0; j < imagen.Height; j++)
            {
                for (int i = 0; i < imagen.Width; i++)
                {
                    
                    result[i, j] = (byte)(peso * imagen[i, j] + dest[i, j]);
                }
            }
            return result;
        }

       
        private TxMatrix DivideImagenes(TxImage imagen, TxImage div)
        {
            TxMatrix result = new TxMatrix(imagen.Height,imagen.Width );
            float color;
            for (int j = 0; j < imagen.Height; j++)
            {
                for (int i = 0; i < imagen.Width; i++)
                {
                    
                        color=(float)imagen[i, j]/(float)(div[i, j] + 1);
                        result[j,i ] = color;                  
                                                               
                }
            }
            return result;

        }


        protected TxMatrix NormalizaImagen(TxMatrix imagen)
        {
            float min = 10000000000;
            float max = -1000000000;
            float k;
            float color;

           
            for (int j = 5; j < imagen.Height - 5; j++)
                for (int i = 5; i < imagen.Width - 5; i++)
                {
                    color = imagen[j,i ];
                    if (color < min) { min = color; }
                    if (color > max) { max = color;  }
                }

            TxMatrix result = new TxMatrix(imagen.Height,imagen.Width );

            // Normalizando
            k = (255) / (max - min);

            for (int j = 0; j < imagen.Height; j++)
            {
                for (int i = 0; i < imagen.Width; i++)
                {

                    result[j,i ] = (k * (imagen[j,i ] - min));
                    

                }
            }
            return result;

        }
        

        

        [Parameter("Weights and Sigmas", "")]
        [CustomParameterEditor(typeof(TxEstudioKernel.VisualElements.MultiScaleParameterEditor))]
        public MultiScaleRetinexParameter Parameter
        {
            get { return new MultiScaleRetinexParameter(pesos, sigmas); }
            set { pesos = value.weights; sigmas = value.sigmas; }
        }




        public override TxImage Process(TxImage input)
        {
            return Filtrado(input.ToGrayScale()); 
        }
    }

    public class MultiScaleRetinexParameter
    {
        public float[] weights;
        public float[] sigmas;

        public MultiScaleRetinexParameter(float[] weights, float[] sigmas)
        {
            this.weights = weights;
            this.sigmas = sigmas;
        }
    }
}
