using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("TS Contrast", "Calculates the Contrast feature of a grey levels image using the Texture Spectrum Model.")]
    [TextureSpectrumDescriptor]
    [Abbreviation("ts_contr", "Deltha", "WinSize")]
    public class TSContrast:TextureSpectrum
    {
        private static int[] d;
        public TSContrast()
        {
            if (d == null)
            {
                d = new int[6561];
                sbyte res = 0;
                byte[] e = null;
                for (int i = 0; i < 6561; i++)
                {
                    e = recons(i);
                    for (byte j = 0; j < 7; j++)
                        for (byte k = (byte)(j + 1); k < 8; k++)
                        {
                            res = (sbyte)(e[j] - e[k]);
                            d[i] += res * res;
                        }
                }
            }

        }
        protected override float Espectrum()
        {
            long contrast = 0, s2 = 0;
            //for (int i = 0; i < 6561; i++)
            //{
            //    contrast += TS[i] * d[i];
            //    s2 += TS[i];
            //}
            foreach (KeyValuePair<long, int> entry in TS)
            {
                contrast += entry.Value * d[entry.Key];
                s2 += entry.Value;
            }
            return (contrast / (128.0f * s2)) * 255.0f;
        }
    }
}
