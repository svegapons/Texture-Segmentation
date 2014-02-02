using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel
{
    public class TxColorComposition : TxMultiBand
    {
        public override TxImage Process(params TxImage[] images)
        {
            //Validating input
            if (AreImagesOK(images))
            {
                TxImage result = new TxImage(images[0].Width, images[0].Height, TxImageFormat.RGB);
                unsafe
                {
                    byte* red = (byte*)((_IplImage*)images[0].InnerImage)->imageData;
                    byte* green = (byte*)((_IplImage*)images[1].InnerImage)->imageData;
                    byte* blue = (byte*)((_IplImage*)images[2].InnerImage)->imageData;
                    byte* dest = (byte*)((_IplImage*)result.InnerImage)->imageData;

                    int colorOffset = ((_IplImage*)result.InnerImage)->widthStep - 3 * result.Width;
                    int grayOffset = ((_IplImage*)images[0].InnerImage)->widthStep - result.Width;

                    for (int i = 0; i < result.Height; i++, red += grayOffset, green += grayOffset, blue += grayOffset, dest += colorOffset)
                        for (int j = 0; j < result.Width; j++, red++, green++, blue++, dest += 3)
                        {
                            dest[2] = *red;
                            dest[1] = *green;
                            dest[0] = *blue;
                        }

                }
                return result;
            }
            throw new ArgumentException("Images must be gray scale and same size");
            
        }

        private bool AreImagesOK(params TxImage[] images)
        {
            Size size = images[0].Size;
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i].ImageFormat != TxImageFormat.GrayScale)
                    return false;
                if (images[i].Size != size)
                    return false;
                
            }
            return true;
        }
    }
}
