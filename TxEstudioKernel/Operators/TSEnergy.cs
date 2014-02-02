using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("TS Energy", "Calculates the Energy feature of a grey levels image using the Texture Spectrum Model.")]
    [TextureSpectrumDescriptor]
    [Abbreviation("ts_energ", "Deltha", "WinSize")]
    public class TSEnergy:TextureSpectrum
    {
        protected override float Espectrum()
        {
            long energy = 0, s2 = 0;
            //for (int i = 0; i < 6561; i++)
            //{
            //    energy += TS[i] * TS[i];
            //    s2 += TS[i];
            //}
            foreach (KeyValuePair<long, int> entry in TS)
            {
                energy += entry.Value * entry.Value;
                s2 += entry.Value;
            }
            return 255.0f * energy / (s2 * s2);
        }
    }
}
