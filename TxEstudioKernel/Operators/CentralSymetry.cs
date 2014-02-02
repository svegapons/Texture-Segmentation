using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Central Symmetry", "Calculates the Central Symmetry feature of a grey levels image using the Texture Spectrum Model.")]
    [TextureSpectrumDescriptor]
    [Abbreviation("ts_cs", "Deltha", "WinSize")]
    public class CentralSymmetry:TextureSpectrum
    {
        private static byte[] LUT_Pot2 ={ 0, 1, 4, 9, 16 };
        private static byte[] PotCs = null;
        public CentralSymmetry()
        {
            if (PotCs == null)
            {
                byte[] e = null;
                byte km = 0;
                PotCs = new byte[6561];
                for (int i = 0; i < 6561; i++)
                {
                    km = 0;
                    e = TextureSpectrum.recons(i);
                    if (e[0] == e[4]) km++;
                    if (e[1] == e[5]) km++;
                    if (e[2] == e[6]) km++;
                    if (e[3] == e[7]) km++;
                    PotCs[i] = LUT_Pot2[km];
                }
            }

        }
        protected override float Espectrum()
        {
            int cs = 0, s2 = 0;
            //for (int i = 0; i < 6561; i++)
            //{
            //    cs += TS[i] * PotCs[i];

            //    s2 += TS[i];
            //}
            foreach (KeyValuePair<long, int> entry in TS)
            {
                cs += entry.Value * PotCs[entry.Key];
                s2 += entry.Value;
            }
            return (cs / (s2 * 16.0f)) * 255.0f;
        }
    }
}
