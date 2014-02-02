using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Long Run", "Calculates the Long Run feature of a grey levels image using the Run Length Statistic Model.")]
    [RunLenthDescriptor]
    [Abbreviation("rl_lr", "WinSize", "Direction", "NG")]
    public class LongRun:RunLength
    {
        protected override float Run(int[,] MRL, int nNgImage, int nMaxRL)
        {
            float msum = 0, rf = 0, mxij, fj = 0;
            for (int j = 1; j <= nMaxRL; j++)
            {
                fj = j * j;
                for (int i = 1; i <= nNgImage; i++)
                {
                    mxij = MRL[i - 1, j - 1];
                    rf += fj * mxij;
                    msum += mxij;
                }
            }
            return rf / msum;
        }
    }
}
