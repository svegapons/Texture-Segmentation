using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;


namespace TxEstudioKernel.Operators
{
    [EdgeDetector]
    [Algorithm("Prewitt Edge Detector", "Detector de Bordes Prewitt")]
    [Abbreviation("prew")]
    public class PrewittEdgeDetector : TxOneBand
    {
        public override TxImage Process(TxImage input)
        {
            TxMatrix imageMatrix = TxMatrix.FromImage(input);
            TxMatrix filterPrewittVer = new TxMatrix(new float[,] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } });
            TxMatrix filterPrewittHor = new TxMatrix(new float[,] { { 1, 0, -1 }, { 1, 0, -1 }, { 1, 0, -1 } });
            TxMatrix prewittFiltered = TxMatrix.VectorialAdd(imageMatrix.Convolve(filterPrewittHor), imageMatrix.Convolve(filterPrewittVer));
            return prewittFiltered.ToImage();
        }
    }
}
