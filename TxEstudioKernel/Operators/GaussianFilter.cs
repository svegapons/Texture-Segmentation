using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel.Operators
{
    
    [DigitalFilter]
    [Algorithm("Gaussian Smooth", "Smooths the image applying a simple blur. Summation over a pixel nRows × nCols neighborhood with subsequent scaling by 1/(nRows•mCols).")]
    [Abbreviation("gaussian_smooth", "nRows", "nCols","Stdv")]
    public class GaussianFilter : TxOneBand
    {

        int nrows = 3;//Valor por defecto para nrows

        [Parameter("x-size", "The x window size of operator")]
        [IntegerInSequence(1, int.MaxValue, 2)]
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


        float stdv = 1;
        [Parameter("standard_deviation", "The standard_deviation  of operator")]
        [RealInRange(0, float.MaxValue)]
        public float Stdv
        {
            get { return stdv; }
            set { stdv = value; }
        }

        public override TxImage Process(TxImage input)
        {
            //TxImage gray = input.ToGrayScale();
            TxImage result = new TxImage(input.Width, input.Height, input.ImageFormat);  //TxImageFormat.GrayScale);

            CV.cvSmooth(/*gray.*/input.InnerImage, result.InnerImage, 2, nrows, mcols, stdv);
            return result;
        }
    }
}
