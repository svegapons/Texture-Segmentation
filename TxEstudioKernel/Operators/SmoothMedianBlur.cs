using System;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;


namespace TxEstudioKernel.Operators
{
    [DigitalFilter]
    [Algorithm("Smooth median blur filter", "Smooths the image finding the median in a NxN neighborhood.")]
    [Abbreviation("median_blur", "WinSize")]
    public class SmoothMedianBlur: TxOneBand
    {
        int nrows = 3;//Valor por defecto para nrows

        [Parameter("N", "The neighborhood size")]
        [IntegerInSequence(1, int.MaxValue, 2)]
        public int Neighborhood
        {
            get { return nrows; }
            set { nrows = value; }
        }

        public override TxImage Process(TxImage input)
        {
            TxImage result = new TxImage(input.Width, input.Height, input.ImageFormat);
            CV.cvSmooth(input.InnerImage, result.InnerImage, 3, nrows, 0, 0);
            return result;
        }
    }
}


