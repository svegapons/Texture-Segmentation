using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Left Diagonal Micro - Structure", "Calculates the Left Diagonal Micro - Structure feature of a grey levels image using the Texture Spectrum Model.")]
    [TextureSpectrumDescriptor]
    [Abbreviation("ts_msld", "Deltha", "WinSize")]
    public class MicroDiagonalStructure2:TextureSpectrum
    {
        private static byte[] Phab, Pdef;
        public MicroDiagonalStructure2()
        {
            if (Phab == null)
            {
                Phab = new byte[6561];
                Pdef = new byte[6561];
                byte[] e = new byte[8];
                for (int i = 0; i < 6561; i++)
                {
                    e = recons(i);
                    if (e[7] == e[0] && e[0] == e[1])
                        Phab[i] = 3;
                    else if (e[7] == e[0] || e[7] == e[1] || e[0] == e[1])
                        Phab[i] = 2;
                    else Phab[i] = 1;

                    if (e[3] == e[4] && e[4] == e[5])
                        Pdef[i] = 3;
                    else if (e[3] == e[4] || e[3] == e[5] || e[4] == e[5])
                        Pdef[i] = 2;
                    else Pdef[i] = 1;
                }
            }
        }
        public virtual byte P_hab(long i)
        {
            return Phab[i];
        }
        public virtual byte P_def(long i)
        {
            return Pdef[i];
        }
        public virtual byte DM2(long i)
        {
            return (byte)(P_hab(i) * P_def(i));
        }
        protected override float Espectrum()
        {
            long dms2 = 0, s2 = 0;
            //for (int i = 0; i < 6561; i++)
            //{
            //    dms2 += TS[i] * DM2(i);
            //    s2 += TS[i];
            //}
            foreach (KeyValuePair<long, int> entry in TS)
            {
                dms2 += entry.Value * DM2(entry.Key);
                s2 += entry.Value;
            }
            return (dms2 / (s2 * 9.0f)) * 255.0f;
        }
    }
}
