using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [EdgeDetector]
    [Algorithm("Kirsh Edge Detector", "Detector de Bordes Kirsh")]
    [Abbreviation("kirsh")]
    public class KirshEdgeDetector : TxOneBand
    {
        public override TxImage Process(TxImage input)
        {
            TxMatrix imageMatrix = TxMatrix.FromImage(input);
            TxMatrix filterPrewittVer = new TxMatrix(new float[,] { { -3, -3, -3 }, { -3, 0, -3 }, { 5, 5, 5 } });
            TxMatrix filterPrewittHor = new TxMatrix(new float[,] { { 5, -3, -3 }, { 5, 0, -3 }, { 5, -3, -3 } });
            TxMatrix prewittFiltered = TxMatrix.VectorialAdd(imageMatrix.Convolve(filterPrewittHor), imageMatrix.Convolve(filterPrewittVer));
            return prewittFiltered.ToImage();
        }
    }
}
