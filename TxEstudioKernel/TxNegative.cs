using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel
{
    /// <summary>
    /// Calculates the negative image from a given one.
    /// </summary>
    public class TxNegative : TxOneBand
    {
        public override TxImage Process(TxImage input)
        {
            TxImage result = new TxImage(input.Width, input.Height, input.ImageFormat);
            int innerLoop = input.Width;
            if(input.ImageFormat == TxImageFormat.RGB)
                innerLoop*=3;
            unsafe
            {
                GetNegative((byte*)((_IplImage*)input.InnerImage)->imageData, (byte*)((_IplImage*)result.InnerImage)->imageData, innerLoop, input.Height, ((_IplImage*)input.InnerImage)->widthStep - innerLoop);
            }
            return result;
        }

        private unsafe void GetNegative(byte* src, byte* dest, int innerLoop, int outerLoop, int offset)
        {
            for (int i = 0; i < outerLoop; i++, src+=offset, dest+=offset)
                for (int j = 0; j < innerLoop; j++, src++, dest++)
                    *dest = (byte)(255 - *src);
        }
    }
}
