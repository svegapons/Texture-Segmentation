using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel
{
    /// <summary>
    /// Base class of opertators that take one matrix as input and return also one matrix.
    /// </summary>
    public abstract class TxMatrixOperator : TxAlgorithm
    {
        public abstract TxMatrix Process(TxMatrix matrix);
    }
}
