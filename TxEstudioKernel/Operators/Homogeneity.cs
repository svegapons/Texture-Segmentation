using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Homogeneity", "Calculates the Homogeneity feature of a grey levels image using the Texture Spectrum Model.")]
    [TextureSpectrumDescriptor]
    [Abbreviation("ts_homog", "Deltha", "WinSize")]
    public class Homogeneity:TextureSpectrum
    {
        private static float[] raizD;
        public Homogeneity()
        {
            if (raizD == null)
            {
                raizD = new float[6561];
                byte[] e = null;
                sbyte res = 0;
                for (int i = 0; i < 6561; i++)
                {
                    e = recons(i);
                    for (byte j = 0; j < 7; j++)
                        for (byte k = (byte)(j + 1); k < 8; k++)
                        {
                            res = (sbyte)(e[j] - e[k]);
                            raizD[i] += (sbyte)(res * res);
                        }
                    raizD[i] = Convert.ToSingle(Math.Sqrt(raizD[i]));
                }
            }
        }
        protected override float Espectrum()
        {
            float homogeneity = 0, s2 = 0;
            //for (int i = 0; i < 6561; i++)
            //{
            //    homogeneity += TS[i] / (1.0f + raizD[i]);
            //    s2 += TS[i];
            //}
            foreach (KeyValuePair<long, int> entry in TS)
            {
                homogeneity += entry.Value / (1.0f + raizD[entry.Key]);
                s2 += entry.Value;
            }
            return (homogeneity / s2) * 255.0f;
        }
    }
}
