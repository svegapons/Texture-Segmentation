using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    
    [CoocurrenceMatrixDescriptor]
    [Algorithm("Energy", "Calculates the Energy feature of grey levels image using the Co-ocurrence Statistic Model. ")]
    [Abbreviation("coo_energ", "Dimension", "DistanciaX", "DistanciaY", "Direccion", "Niveles")]
    public class Energy : TxCoocurrenceDescriptor
    {

        

        protected override float Estadistica(MatrizCoocurrencia matriz)
        {
            float result = 0;
            float[,] matrizNorm = matriz.Normaliza();

            for (int j = 0; j < matrizNorm.GetLength(1); j++)
                for (int i = 0; i < matrizNorm.GetLength(0); i++)
                    result += matrizNorm[i, j] * matrizNorm[i, j];
            return (float)Math.Sqrt((double)result);
        }
    }
}
