using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel
{
    /// <summary>
    /// Base class of operators that take one image as input and return multiple images.
    /// </summary>
    public abstract class TxMultiBandOutput : TxDIPAlgorithm
    {
        public abstract List<TxImage> Process(TxImage input);
    }
}
