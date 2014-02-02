using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Geometric Symetry", "Calculates the Geometric Symmetry feature of a grey levels image using the Texture Spectrum Model.")]
    [TextureSpectrumDescriptor]
    [Abbreviation("ts_gs", "Deltha", "WinSize")]
    public class GeometricSimetry : TextureSpectrum
    {
        public GeometricSimetry()
        {
            TS = new ExtendedTSHistogram();
        }
        protected override float Espectrum()
        {
            int suma1 = 0;
            //int suma2 = 0;
            float suma = 0;
            //int si = 0;
            //for (byte j = 0; j < 4; j++)
            //{
            //    suma1 = 0;
            //    suma2 = 0;
            //    for (int i = 0; i < 6561; i++)
            //    {
            //        si = TS[i, j];
            //        suma1 += Math.Abs(si - TS[i, (byte)(j + 4)]);
            //        suma2 += si;
            //    }
            //    suma += suma1 / (2.0f * suma2);
            //}

            for (byte j = 0; j < 4; j++)
            {
                IEnumerator<KeyValuePair<long, int>> enum1 = ((ExtendedTSHistogram)TS).GetFrecuencyEnumerator(j);
                IEnumerator<KeyValuePair<long, int>> enum2 = ((ExtendedTSHistogram)TS).GetFrecuencyEnumerator((byte)(j + 4));
                enum1.Reset(); enum2.Reset();
                bool b1 = enum1.MoveNext();
                bool b2 = enum2.MoveNext();
                if (b1 && b2)
                    while (true)
                    {
                        if (enum1.Current.Key < enum2.Current.Key)
                        {
                            suma1 += enum1.Current.Value;
                            b1 = enum1.MoveNext();
                            if (!b1) break;
                        }
                        if (enum1.Current.Key > enum2.Current.Key)
                        {
                            suma1 += enum2.Current.Value;
                            b2 = enum2.MoveNext();
                            if (!b2) break;
                        }
                        if (enum1.Current.Key == enum2.Current.Key)
                        {
                            suma1 += Math.Abs(enum1.Current.Value - enum2.Current.Value);
                            b1 = enum1.MoveNext(); b2 = enum2.MoveNext();
                            if (!b1 || !b2) break;
                        }
                    }
                if (!b1)
                    do
                    {
                        suma1 += enum2.Current.Value;
                    } while (enum2.MoveNext());
                if (!b2)
                    do
                    {
                        suma1 += enum1.Current.Value;
                    } while (enum1.MoveNext());

            }
            suma += suma1 / (2.0f * TS.TotalFrecuency);
            return (1.0f - 0.25f * suma) * 255.0f;
        }
    }

}
