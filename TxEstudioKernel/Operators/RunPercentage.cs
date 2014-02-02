using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Run Length Percentage", "Calculates the Run Length Percentage feature of a grey levels image using the Run Length Statistic Model.")]
    [RunLenthDescriptor]
    [Abbreviation("rl_rlp", "WinSize", "Direction", "NG")]
    public class RunPercentage:RunLength
    {
        protected override float Run(int[,] MRL, int nNgImage, int nMaxRL)
        {
            float rf = 0;
            for (int j = 0; j < nMaxRL; j++)
            {
                for (int i = 0; i < nNgImage; i++)
                {
                    rf += MRL[i, j];
                }
            }
            return rf / (WinSize * WinSize);
        }
    }
}
