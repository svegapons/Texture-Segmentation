using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    
    [CoocurrenceMatrixDescriptor]
    [Algorithm("Second Angular Moment", "Calculates the Second Angular Moment feature of a grey levels image using the Co-ocurrence Statistic Model. ")]
    [Abbreviation("coo_sam", "Dimension", "DistanciaX", "DistanciaY", "Direccion", "Niveles")]
    public class SecondAngularMoment : TxCoocurrenceDescriptor
    {

        protected override float Estadistica(MatrizCoocurrencia matriz)
        {
            float result = 0;
            float[,] matrizNorm = matriz.Normaliza();

            for (int j = 0; j < matrizNorm.GetLength(1); j++)
                for (int i = 0; i < matrizNorm.GetLength(0); i++)
                    result += matrizNorm[i, j] * matrizNorm[i, j];
            return result;
        }
    }
}
