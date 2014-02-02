using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators.ImageQualityAssessment
{
    [Algorithm("UIQ", "Calculates the UIQ index for image quality assesment.")]
    [Abbreviation("uiq", "XSize", "YSize")]
    public class UIQ:ImageQuality
    {

        public override double Error(TxImage imagen1, TxImage imagen2)
        {
            
            double resultado = 0;            

            MeanDescriptor med = new MeanDescriptor();
            med.nCols = xSize;
            med.nRows = ySize;

            

            TxMatrix med1 = med.GetDescription(imagen1);//media de imagen1
            TxMatrix med2 = med.GetDescription(imagen2);//media de imagen2
            TxMatrix var1 = Var(imagen1, med1);//varianza de imagen1
            TxMatrix var2 = Var(imagen2, med2);//varianza de imagen2
            TxMatrix stdvxy = StdvXY(imagen1, imagen2, med1, med2);

            mapa = new TxMatrix(imagen1.Height, imagen1.Width);

            float[,] mapa1 = new float[imagen1.Height, imagen1.Width];

            for(int y=0;y<imagen1.Height;y++)
                for (int x = 0; x < imagen1.Width; x++)
                {
                    if (((var1[y, x] + var2[y, x]) * (med1[y, x] * med1[y, x] + med2[y, x] * med2[y, x])) == 0)mapa[y,x]=1-(2*System.Math.Abs(med1[y,x]-med2[y,x])/255);
                        

                    else mapa[y, x] = (4 * stdvxy[y, x] * med1[y, x] * med2[y, x]) / ((var1[y, x] + var2[y, x]) * (med1[y, x] * med1[y, x] + med2[y, x] * med2[y, x]));

                    mapa1[y, x] = mapa[y, x];
                    resultado += mapa[y, x];
                    
                    
                }
            return resultado / ((imagen1.Width)*(imagen1.Height));


            

        }
    }
}
