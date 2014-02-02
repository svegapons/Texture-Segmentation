using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Micro Vertical Structure", "Calculates the Micro Vertical Structure feature of a grey levels image using the Texture Spectrum Model.")]
    [TextureSpectrumDescriptor]
    [Abbreviation("ts_mvs", "Deltha", "WinSize")]
    public class MicroVerticalStructure:TextureSpectrum
    {
        private static byte[] Pahg, Pcde;
        public MicroVerticalStructure()
        {
            if (Pahg == null)
            {
                Pahg = new byte[6561];
                Pcde = new byte[6561];
                byte[] e = null;
                for (int i = 0; i < 6561; i++)
                {
                    e = TextureSpectrum.recons(i);
                    if (e[0] == e[7] && e[7] == e[6])
                        Pahg[i] = 3;
                    else if (e[0] == e[7] || e[0] == e[6] || e[6] == e[7])
                        Pahg[i] = 2;
                    else Pahg[i] = 1;

                    if (e[2] == e[3] && e[3] == e[4])
                        Pcde[i] = 3;
                    else if (e[2] == e[3] || e[2] == e[4] || e[3] == e[4])
                        Pcde[i] = 2;
                    else Pcde[i] = 1;
                }
            }
        }
        protected override float Espectrum()
        {
            long mvs = 0, s2 = 0;
            //for (int i = 0; i < 6561; i++)
            //{
            //    mvs += TS[i] * (Pahg[i] * Pcde[i]);
            //    s2+=TS[i];
            //}
            foreach (KeyValuePair<long, int> entry in TS)
            {
                mvs += entry.Value * VM(entry.Key);
                s2 += entry.Value;
            }
            return (mvs / (s2 * 9.0f)) * 255.0f;
        }
        public virtual byte P_ahg(long i)
        {
            return Pahg[i];
        }
        public virtual byte P_cde(long i)
        {
            return Pcde[i];
        }
        public virtual byte VM(long i)
        {
            return (byte)(P_ahg(i) * P_cde(i));
        }

    }
}
