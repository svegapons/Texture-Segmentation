using System;
using System.Collections.Generic;

namespace TxEstudioKernel
{
    public abstract class TxGeneral : TxDIPAlgorithm
    {
        public abstract List<TxImage> Process(params TxImage[] parameters);
    }
}
