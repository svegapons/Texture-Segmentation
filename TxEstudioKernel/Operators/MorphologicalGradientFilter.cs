using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel.Operators
{
    [DigitalFilter]
    [Algorithm("Morphological Gradient Filter", "Perform advanced morphological transformations using erosion and dilation as basic operations")]
    [Abbreviation("gradient", "Window", "Iterations")]
    public class MorphologicalGradientFilter : TxOneBand
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
            TxImage temp = new TxImage(input.Width, input.Height, input.ImageFormat);

            TxMorphKernel kernel = new TxMorphKernel(window_Size, window_Size);
            CV.cvMorphologyEx(input.InnerImage, result.InnerImage, temp.InnerImage, kernel.InnerKernel, 4, iterations);

            return result;
        }
    }
}
