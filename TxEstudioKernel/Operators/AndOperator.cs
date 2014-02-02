using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.OpenCV;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("AND", "Computes the AND operator between the given images")]
    [Abbreviation("AND")]
    public class AndOperator:TxMultiBand
    {
        public override TxImage Process(params TxImage[] images)
        {
            if (images.Length != 2)
                throw new Exception("Two images are required to perform this action");
            TxImage tx1 = images[0];
            TxImage tx2 = images[1];
            if (images[0].ImageFormat != TxImageFormat.GrayScale)
                tx1 = images[0].ToGrayScale();
            if (images[1].ImageFormat != TxImageFormat.GrayScale)
                tx2 = images[1].ToGrayScale();

            TxImage result = new TxImage(tx1.Width,tx1.Height,tx1.ImageFormat);

            CXCore.cvAnd(tx1.InnerImage, tx2.InnerImage, result.InnerImage, IntPtr.Zero);
            return result;
        }
    }
}
