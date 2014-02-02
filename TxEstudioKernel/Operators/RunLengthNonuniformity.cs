using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Run Length Distribution", "Calculates the Run Length Distribution feature of a grey levels image using the Run Length Statistic Model.")]
    [RunLenthDescriptor]
    [Abbreviation("rl_rld", "WinSize", "Direction", "NG")]
    public class RunLengthNonuniformity:RunLength
    {
        protected override float Run(int[,] MRL, int nNgImage, int nMaxRL)
        {
            float msum = 0, msum2 = 0, rf = 0, mxij;
            for (int j = 0; j < nMaxRL; j++)
            {
                msum2 = 0;

                for (int i = 0; i < nNgImage; i++)
                {
                    mxij = MRL[i, j];
                    msum += mxij;
                    msum2 += mxij;
                }
                rf += msum2 * msum2;
            }
            return rf / msum;
        }
    }
}
