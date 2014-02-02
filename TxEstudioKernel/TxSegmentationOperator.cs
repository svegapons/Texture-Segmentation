using System;

namespace TxEstudioKernel
{
    /// <summary>
    /// Base class of operators that take as input one segmentation result.
    /// </summary>
    public abstract class TxSegmentationOperator:TxAlgorithm
    {
        public abstract TxSegmentationResult Process(TxSegmentationResult segmentation);
    }
}
