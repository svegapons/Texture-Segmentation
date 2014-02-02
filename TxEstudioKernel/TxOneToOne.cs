using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel
{
    /// <summary>
    /// Base class of operators that take as input one image and also one image for output.
    /// </summary>
    public abstract class TxOneBand : TxDIPAlgorithm
    {
        public abstract TxImage Process(TxImage input);
    }
}
