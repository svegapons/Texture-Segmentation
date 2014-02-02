using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel
{
    /// <summary>
    /// Base class of operators that take multiple images as input and resturn only one.
    /// </summary>
    public abstract class TxMultiBand : TxDIPAlgorithm
    {
        public abstract TxImage Process(params TxEstudioKernel.TxImage[] images);
    }
}
