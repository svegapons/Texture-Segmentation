using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    
    [CoocurrenceMatrixDescriptor]
    [Algorithm("Entropy", "Calculates the Entropy feature of a grey levels image using the Co-ocurrence Statistic Model.")]
    [Abbreviation("coo_entro", "Dimension", "DistanciaX", "DistanciaY", "Direccion", "Niveles")]
    public class Entropy : TxCoocurrenceDescriptor
    {

        

        protected override float Estadistica(MatrizCoocurrencia matriz)
        {
            float[,] matrizNorm = matriz.Normaliza();
            float result = 0;
            for (int j = 0; j < matrizNorm.GetLength(1); j++)
                for (int i = 0; i < matrizNorm.GetLength(1); i++)
                    if (matrizNorm[i, j] != 0.0)
                        result += (-1) * matrizNorm[i, j] * (float)System.Math.Log(matrizNorm[i, j]);
            return result;
        }
    }
}
