using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel.Operators
{
    [DigitalFilter]
    [Algorithm("Closing Morphological Filter", "Perform advanced morphological transformations using erosion and dilation as basic operations")]
    [Abbreviation("closing", "Window", "Iterations")]
    public class ClosingFilter : TxOneBand
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
            CV.cvMorphologyEx(input.InnerImage, result.InnerImage, IntPtr.Zero, kernel.InnerKernel, 3, iterations);

            return result;
        }
    }
}
