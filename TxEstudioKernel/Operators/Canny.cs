using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel.Operators
{

    
    [EdgeDetector]
    [Algorithm("Canny", "Calculates the Canny edge feature using two thresholds.")]
    [Abbreviation("canny", "Threshold1", "Threshold2")]
    public class Canny:TxOneBand
    {
        float threshold1 = 80.0f;
        [Parameter("Threshold1", "This is the first Canny Threshold")]
        [RealInRange(0.0f, 255.0f)]
        public float Threshold1
        {
            get { return threshold1; }
            set { threshold1 = value;}
        }

        float threshold2 = 180.0f;
        [Parameter("Threshold2", "This is the second Canny Threshold")]
        [RealInRange(0.0f, 255.0f)]
        public float Threshold2
        {
            get { return threshold2; }
            set { threshold2 = value; }
        }
        public override TxImage Process(TxImage input)
        {
            TxImage gray = input.ToGrayScale();
            TxImage result = new TxImage(input.Width, input.Height, TxImageFormat.GrayScale);
            CV.cvCanny(gray.InnerImage, result.InnerImage, threshold1, threshold2, 3);
            return result;
        }
    }
}
