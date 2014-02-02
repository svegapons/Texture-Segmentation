using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel.Operators
{
    [EdgeDetector]
    [Algorithm("Laplacian operator", "Calculates the Laplacian of the given image")]
    [Abbreviation("lap", "Aperture_Size")]
    public class Laplacian:TxOneBand
    {

        public override TxImage Process(TxImage input)
        {
            TxImage gray = input.ToGrayScale();
            TxImage result = new TxImage(input.Width, input.Height, TxImageFormat.GrayScale);
            IntPtr image16 = CXCore.cvCreateImage(input.Size, 0x80000000 | 16, 1);
            CV.cvLaplace(gray.InnerImage, image16, aperture_size);
            CXCore.cvConvertScaleAbs(image16, result.InnerImage, 1, 0);
            return result;
        }

        int aperture_size = 3;
        [Parameter("Aperture", "Size of the extended Sobel kernel, must be 1, 3, 5 or 7.")]
        [IntegerInSequence(1, 7, 2)]
        public int Aperture_Size
        {
            get
            {
                return aperture_size;
            }
            set
            {
                aperture_size = value;
            }
        }

    }
}
