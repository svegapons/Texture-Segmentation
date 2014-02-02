using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Short Run", "Calculates the Short Run feature of a grey levels image using the Run Length Statistic Model.")]
    [RunLenthDescriptor]
    [Abbreviation("rl_sr", "WinSize", "Direction", "NG")]
    public class ShortRun:RunLength
    {
        protected override float Run(int[,] MRL, int nNgImage, int nMaxRL)
        {
            float rf = 0f, mxij = 0, msum = 0, fj = 0;
            for (int j = 1; j <= nMaxRL; j++)
            {
                fj = j * j;
                for (int i = 1; i <= nNgImage; i++)
                {
                    mxij = MRL[i - 1, j - 1];
                    rf += mxij / fj;
                    msum += mxij;
                }
            }
            return rf / msum;
        }
    }
}
