using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Right Diagonal Micro - Structure", "Calculates the Right Diagonal Micro - Structure feature of a grey levels image using the Texture Spectrum Model.")]
    [TextureSpectrumDescriptor]
    [Abbreviation("ts_msrd", "Deltha", "WinSize")]
    public class MicroDiagonalStructure1:TextureSpectrum
    {
        private static byte[] Pbcd, Pfgh;
        public MicroDiagonalStructure1()
        {
            if (Pbcd == null)
            {
                Pbcd = new byte[6561];
                Pfgh = new byte[6561];
                byte[] e = null;
                for (int i = 0; i < 6561; i++)
                {
                    e = recons(i);
                    if (e[1] == e[2] && e[2] == e[3])
                        Pbcd[i] = 3;
                    else if (e[1] == e[2] || e[1] == e[3] || e[2] == e[3])
                        Pbcd[i] = 2;
                    else Pbcd[i] = 1;

                    if (e[5] == e[6] && e[6] == e[7])
                        Pfgh[i] = 3;
                    else if (e[5] == e[6] || e[5] == e[7] || e[6] == e[7])
                        Pfgh[i] = 2;
                    else Pfgh[i] = 1;
                }
            }
        }
        public virtual byte P_bcd(long i)
        {
            return Pbcd[i];
        }
        public virtual byte P_fgh(long i)
        {
            return Pfgh[i];
        }
        public virtual byte DM1(long i)
        {
            return (byte)(P_bcd(i) * P_fgh(i));
        }
        protected override float Espectrum()
        {
            long mds1 = 0, s2 = 0;
            //for (int i = 0; i < 6561; i++)
            //{
            //    mds1 += TS[i] * DM1(i);
            //    s2 += TS[i];
            //}
            foreach (KeyValuePair<long, int> entry in TS)
            {
                mds1 += entry.Value * DM1(entry.Key);
                s2 += entry.Value;
            }
            return (mds1 / (s2 * 9.0f)) * 255.0f;
        }
    }
}
