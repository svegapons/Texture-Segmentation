using System;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;


namespace TxEstudioKernel.Operators
{
    [DigitalFilter]
    [Algorithm("Smooth bilateral filter", "")]
    [Abbreviation("bil_smooth", "ColorSigma", "SpaceSigma")]
    public class SmoothBilateral:TxOneBand
    {

        int colorSigma = 50;
        [Parameter("Color sigma", "")]
        [IntegerInSequence(255)]
        public int ColorSigma
        {
            get { return colorSigma; }
            set { colorSigma = value; }
        }

        int spaceSigma;
        [Parameter("Space sigma", "")]
        public int SpaceSigma
        {
            get { return spaceSigma; }
            set { spaceSigma = value; }
        }

        public override TxImage Process(TxImage input)
        {
            TxImage result = new TxImage(input.Width, input.Height, input.ImageFormat);
            CV.cvSmooth(input.InnerImage, result.InnerImage, 4, colorSigma, spaceSigma, 0);
            return result;
        }
    }
}
