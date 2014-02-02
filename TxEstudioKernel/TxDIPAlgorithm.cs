using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel
{
    public abstract class TxDIPAlgorithm : TxAlgorithm
    {
        public static bool IsOneBandInput(TxDIPAlgorithm algorithmInstance)
        {
            return algorithmInstance is TxOneBand || algorithmInstance is TxMultiBandOutput;
        }

        public static bool IsOneBandOutPut(TxDIPAlgorithm algorithmInstance)
        {
            return algorithmInstance is TxOneBand || algorithmInstance is TxMultiBand;
        }
    }
}
