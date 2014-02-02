using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    
    [CoocurrenceMatrixDescriptor]
    [Algorithm("Inverse Diferent Moment", "Calculates the Inverse Difference Moment feature of a grey levels image using the Co-ocurrence Statistic Model. ")]
    [Abbreviation("coo_idm", "Dimension", "DistanciaX", "DistanciaY", "Direccion", "Niveles")]
    public class InverseDiferentMoment : TxCoocurrenceDescriptor
    {

       


        protected override float Estadistica(MatrizCoocurrencia matriz)
        {
            float result = 0;
            float[,] matrizNorm = matriz.Normaliza();//matriz normalizada de la s probabilidades
            
            for (int j = 0; j < matriz.GrayLevels;j++ )
                for (int i = 0; i < matriz.GrayLevels; i++)
                {
                    result += matrizNorm[i, j] / (1 + (i - j) * (i - j));
                }
           
            return result;
        }
    }
}
