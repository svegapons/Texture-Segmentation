using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel.Operators
{
    [DigitalFilter]
    [Algorithm("Smooth simple blur Filter", "Smooths the image applying a simple blur. Summation over a pixel nRows × nCols neighborhood with subsequent scaling by 1/(nRows•mCols).")]
    [Abbreviation("simple_blur", "nRows", "nCols")]
    public class SmoothSimpleBlurFilter:TxOneBand
    {

        int nrows = 3;//Valor por defecto para nrows

        [Parameter("x-size", "The x window size of operator")]
        [IntegerInSequence(1,int.MaxValue, 2)]
        public int nRows
        {
            get { return nrows; }
            set { nrows = value; }
        }

        int mcols = 3;//Valor por defecto para mcols
        [Parameter("y-size", "The y window size of operator")]
        [IntegerInSequence(1, int.MaxValue, 2)]
        public int nCols
        {
            get { return mcols; }
            set { mcols = value; }
        }

        public override TxImage Process(TxImage input)
        {
            //TxImage gray = input.ToGrayScale();
            TxImage result = new TxImage(input.Width, input.Height, input.ImageFormat);  //TxImageFormat.GrayScale);

            CV.cvSmooth(/*gray.*/input.InnerImage, result.InnerImage, 1 ,nrows,mcols,0);
            return result;
        }
    }
}
