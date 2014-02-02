using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators.ImageQualityAssessment
{
    [Algorithm("MSSIM", "Calculates the MSSIM index for image quality assesment. ")]
    [Abbreviation("mssim", "K1", "K2", "Alfa", "Beta", "Gamma", "XSize", "YSize")]
    public class MSSIM:ImageQuality
    {
        float k1 = 0.1f;
        [Parameter("Constante 1", "Value for avoid stabilty promblems")]
        [RealInRange(0, 1)]
        public float K1
        {
            get { return k1; }
            set { k1 = value; }
        }
        float k2 = 0.01f;

        [Parameter("Constante 2", "Value for avoid stabilty promblems ")]
        [RealInRange(0, 1)]
        public float K2
        {
            get { return k2; }
            set { k2 = value; }
        }

        float alfa = 1;

        [Parameter("Alfa", "To control the relevance of the luminosity component ")]
        [RealInRange(0, int.MaxValue)]
        public float Alfa
        {
            get { return alfa; }
            set { alfa = value; }
        }
        float beta = 1;
        [Parameter("Beta", "To control the relevance of the contrast component ")]
        [RealInRange(0, int.MaxValue)]
        public float Beta
        {
            get { return beta; }
            set { beta = value; }
        }
        float gamma = 1;
        [Parameter("Gamma", "To control the relevance of the structure component")]
        [RealInRange(0, int.MaxValue)]
        public float Gamma
        {
            get { return gamma; }
            set { gamma = value; }
        }

        public override double Error(TxImage imagen1, TxImage imagen2)
        {
            double c1= (K1*256)*(K1*256);//256 es el rango de valores del pixel
            double c2=(K2*256)*(K2*256);//256 es el rango de valores del pixel
            double c3=c2/2;

            double luminosidad = 0;
            double contraste = 0;
            double estructura = 0;

            MeanDescriptor med = new MeanDescriptor();
            med.nCols = xSize;
            med.nRows = ySize;


            TxMatrix med1 = med.GetDescription(imagen1);
            TxMatrix med2 = med.GetDescription(imagen2);
            TxMatrix var1 = Var(imagen1, med1);
            TxMatrix var2 = Var(imagen2, med2);
            TxMatrix stdv1 = Stdv(var1);
            TxMatrix stdv2 = Stdv(var2);
            TxMatrix stdvxy = StdvXY(imagen1, imagen2, med1, med2);

            mapa = new TxMatrix(imagen1.Height, imagen1.Width);
            double resultado = 0;
            
            

            for (int y = 0; y < imagen1.Height; y++)
                for (int x = 0; x < imagen1.Width; x++)
                {
                    luminosidad = (2 * med1[y, x] * med2[y, x] + c1) / (med1[y, x] * med1[y, x] + med2[y, x] * med2[y, x] + c1);

                    contraste = (2 * stdv1[y, x] * stdv2[y, x] + c2) / (var1[y,x] + var2[y,x] + c2);

                    estructura = (Math.Abs(stdvxy[y, x])+ c3) / (stdv1[y, x] * stdv2[y, x] + c3);

                    mapa[y, x] = (float)(Math.Pow(luminosidad, alfa) * Math.Pow(contraste, beta) * Math.Pow(estructura, gamma));

                    
                    resultado += mapa[y, x];
                    
                    

                }
            return resultado / (imagen1.Width * imagen1.Height);
        }
    }
}
