using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel
{
    public class TxColorDecomposition : TxMultiBandOutput
    {
        public override List<TxImage> Process(TxImage input)
        {
            if (input.ImageFormat == TxImageFormat.RGB)
            {
                List<TxImage> result = new List<TxImage>(3);
                for (int i = 0; i < 3; i++)
                    result.Add(new TxImage(input.Width,input.Height, TxImageFormat.GrayScale));

                unsafe
                {
                    byte* red   = (byte*)((_IplImage*)result[0].InnerImage)->imageData;
                    byte* green = (byte*)((_IplImage*)result[1].InnerImage)->imageData;
                    byte* blue  = (byte*)((_IplImage*)result[2].InnerImage)->imageData;
                    byte* src   = (byte*)((_IplImage*)input.InnerImage)->imageData;

                    int colorOffset = ((_IplImage*)input.InnerImage)->widthStep - 3*input.Width;
                    int grayOffset = ((_IplImage*)result[0].InnerImage)->widthStep - input.Width;

                    for (int i = 0; i < input.Height; i++, red += grayOffset, green += grayOffset, blue += grayOffset, src+=colorOffset)
                        for (int j = 0; j < input.Width; j++, red++, green++, blue++, src+=3)
                        {
                            *red   = src[2];
                            *green = src[1];
                            *blue  = src[0];
                        }
                }
                return result;
            }
            throw new ArgumentException("Image must be in RGB format.");

        }
    }
}
