using System;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;


namespace TxEstudioKernel.Operators
{
    [DigitalFilter]
    //[Algorithm("Smooth Gaussian blur filter", "Smooths the image with NxM Gaussian kernel.")]
    [Abbreviation("gauss_blur", "nRows", "nCols")]
    public class SmoothGaussianBlur: TxOneBand
    {
        int nrows = 3;//Valor por defecto para nrows

        [Parameter("N", "The x window size of operator")]
        [IntegerInSequence(1, int.MaxValue, 2)]
        public int nRows
        {
            get { return nrows; }
            set { nrows = value; }
        }

        int mcols = 3;//Valor por defecto para mcols
        [Parameter("M", "The y window size of operator")]
        [IntegerInSequence(1, int.MaxValue, 2)]
        public int nCols
        {
            get { return mcols; }
            set { mcols = value; }
        }

        public override TxImage Process(TxImage input)
        {
            TxImage result = new TxImage(input.Width, input.Height, input.ImageFormat);
            CV.cvSmooth(input.InnerImage, result.InnerImage, 2, nrows, mcols, 0);
            return result;
        }
    }
}


