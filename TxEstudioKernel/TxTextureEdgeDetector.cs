using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel
{
    /// <summary>
    /// Base class of texture edge detectors
    /// </summary>
    public abstract class TxTextureEdgeDetector : TxSegmentationAlgorithm, TxEstudioKernel.Operators.IEvaluable
    {
        //public abstract TxSegmentationResult Segment(params TxMatrix[] descriptors);
       // public abstract double ProbError();
    }
}
