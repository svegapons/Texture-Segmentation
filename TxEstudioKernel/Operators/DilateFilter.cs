using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.OpenCV;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [DigitalFilter]
    [Algorithm("Dilate Morphological Filter", "The filter dilates the source image using the specified structuring element (default is 3x3) that determines the shape of a pixel neighborhood over which the minimum is taken.")]
    public class DilateFilter : TxOneBand
    {

        int window_Size = 3;

        [Parameter("Window Size", "Bla bla bla")]
        [IntegerInSequence(1, 101, 2)]
        public int Window
        {
            get
            {
                return window_Size;
            }
            set
            {
                window_Size = value;
            }
        }

        int iterations = 1;


        [Parameter("Iterations", "Number of times operator is applied")]
        [IntegerInSequence(1, int.MaxValue)]
        public int Iterations
        {
            get
            {
                return iterations;
            }
            set
            {
                iterations = value;
            }
        }

        public override TxImage Process(TxImage input)
        {
            TxImage result = (TxImage)input.Clone();

            TxMorphKernel kernel = new TxMorphKernel(window_Size, window_Size);
            CV.cvDilate(input.InnerImage, result.InnerImage, kernel.InnerKernel, iterations);

            return result;
        }
    }
}
