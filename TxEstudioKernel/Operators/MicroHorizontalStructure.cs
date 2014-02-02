using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Micro Horizontal Structure", "Calculates the  Micro Horizontal Structure feature of a grey levels image using the Texture Spectrum Model.")]
    [TextureSpectrumDescriptor]
    [Abbreviation("ts_mhs", "Deltha", "WinSize")]
    public class MicroHorizontalStructure:TextureSpectrum
    {
        private static byte[] Pabc = null, Pfgh = null;
        public MicroHorizontalStructure()
        {
            if (Pabc == null)
            {
                byte[] e = null;
                Pabc = new byte[6561];
                Pfgh = new byte[6561];
                for (int i = 0; i < 6561; i++)
                {
                    e = TextureSpectrum.recons(i);
                    if (e[0] == e[1] && e[1] == e[2])
                        Pabc[i] = 3;
                    else if (e[0] == e[1] || e[0] == e[2] || e[1] == e[2])
                        Pabc[i] = 2;
                    else Pabc[i] = 1;

                    if (e[4] == e[5] && e[5] == e[6])
                        Pfgh[i] = 3;
                    else if (e[4] == e[5] || e[5] == e[6] || e[4] == e[6])
                        Pfgh[i] = 2;
                    else Pfgh[i] = 1;
                }
            }
        }
        protected override float Espectrum()
        {
            long mhs = 0 /*,hm=0*/, s2 = 0;
            //for (int i = 0; i < 6561; i++)
            //{
            //    hm = Pabc[i] * Pfgh[i];
            //    mhs += TS[i] * hm;
            //    s2 += TS[i];
            //}
            foreach (KeyValuePair<long, int> entry in TS)
            {
                mhs += entry.Value * HM(entry.Key);
                s2 += entry.Value;
            }
            //Para normalizar
            return (mhs / (s2 * 9.0f)) * 255;
        }
        public virtual byte P_abc(long i)
        {
            return Pabc[i];
        }
        public virtual byte P_fgh(long i)
        {
            return Pfgh[i];
        }
        public virtual byte HM(long i)
        {
            return (byte)(P_abc(i) * P_fgh(i));
        }
    }
}
