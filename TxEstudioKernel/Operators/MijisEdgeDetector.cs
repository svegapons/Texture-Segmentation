using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;


namespace TxEstudioKernel.Operators
{
    [EdgeDetector]
    [Algorithm("MijisEdgeDetector", "Detector de Bordes basado en la idea del polinomio para ventana de [5 x 5].")]
    [Abbreviation("mijisED")]
    public class MijisEdgeDetector : TxOneBand
    {
        public override TxImage Process(TxImage input)
        {
            TxMatrix imageMatrix = TxMatrix.FromImage(input);
            TxMatrix filterMijisVer = new TxMatrix(new float[,] { { -1, -1, -1, -1, -1 }, { 8, 8, 8, 8, 8 }, { 0, 0, 0, 0, 0 }, { -8, -8, -8, -8, -8 }, { 1, 1, 1, 1, 1 } });
            TxMatrix filterMijisHor = new TxMatrix(new float[,] { { -1, 8, 0, -8, 1 }, { -1, 8, 0, -8, 1 }, { -1, 8, 0, -8, 1 }, { -1, 8, 0, -8, 1 }, { -1, 8, 0, -8, 1 } });
            TxMatrix mijisFiltered = TxMatrix.VectorialAdd(imageMatrix.Convolve(filterMijisHor), imageMatrix.Convolve(filterMijisVer));
            return mijisFiltered.ToImage();
        }
    }
}
