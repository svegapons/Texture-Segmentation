using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel
{
    /// <summary>
    /// Base class of multi-channel segmentation algorithm implementations.
    /// </summary>
    public abstract class TxSegmentationAlgorithm : TxAlgorithm, TxEstudioKernel.Operators.IEvaluable
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="descriptors">An array of image features</param>
        /// <returns>THe segmentation result</returns>
        public abstract TxSegmentationResult Segment(params TxMatrix[] descriptors);
        public abstract double ProbError();

    }
}
