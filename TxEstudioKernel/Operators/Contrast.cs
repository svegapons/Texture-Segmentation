using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [CoocurrenceMatrixDescriptor]
    [Algorithm("Contrast", "Calculates the Contrast feature of a grey levels image using the Co-ocurrence Statistic Model.")]
    [Abbreviation("coo_ctrast", "Dimension", "DistanciaX", "DistanciaY", "Direccion", "Niveles")]
    public class Contrast : TxCoocurrenceDescriptor
    {

        
       

        protected override float Estadistica(MatrizCoocurrencia matriz)
        {
            float result = 0;
            float[,] matrizNorm = matriz.Normaliza();//matriz normalizada de la s probabilidades
           

            for (int j = 0; j < matriz.GrayLevels; j++)
                for (int i = 0; i < matriz.GrayLevels; i++)
                    result += matrizNorm[i, j] * (i - j) * (i - j);
            return result;
        }
    }
}
