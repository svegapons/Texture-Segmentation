using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Degree of Direction", "Calculates the Degree of Direction feature of a grey levels image using the Texture Spectrum Model.")]
    [TextureSpectrumDescriptor]
    [Abbreviation("ts_dd", "Deltha", "WinSize")]
    public class DegreeOfDirection:TextureSpectrum
    {
        public DegreeOfDirection()
        {
            TS = new ExtendedTSHistogram();
        }
        protected override float Espectrum()
        {
            int s1 = 0, s2 = 0;
            float s3 = 0, s = 0;

            IEnumerator<KeyValuePair<long, int>>[] enumerators = new IEnumerator<KeyValuePair<long, int>>[4];

            for (byte m = 0; m < 3; m++)
            {
                s3 = 0;
                for (byte n = (byte)(m + 1); n < 4; n++)
                {
                    s1 = 0; s2 = 0;
                    //for (int i = 0; i < 6561; i++)
                    //{
                    //    s1 += Math.Abs(TS[i, m] - TS[i, n]);
                    //    s2 += TS[i, m];
                    //}
                    //s3 += s1 / (2.0f * s2);

                    if (enumerators[m] == null)
                        enumerators[m] = ((ExtendedTSHistogram)TS).GetFrecuencyEnumerator(m);
                    if (enumerators[n] == null)
                        enumerators[n] = ((ExtendedTSHistogram)TS).GetFrecuencyEnumerator(n);

                    IEnumerator<KeyValuePair<long, int>> enum1 = enumerators[m];
                    IEnumerator<KeyValuePair<long, int>> enum2 = enumerators[n];
                    enum1.Reset(); enum2.Reset();
                    bool b1 = enum1.MoveNext();
                    bool b2 = enum2.MoveNext();
                    if (b1 && b2)
                        while (true)
                        {
                            if (enum1.Current.Key < enum2.Current.Key)
                            {
                                s1 += enum1.Current.Value;
                                b1 = enum1.MoveNext();
                                if (!b1) break;
                            }
                            if (enum1.Current.Key > enum2.Current.Key)
                            {
                                s1 += enum2.Current.Value;
                                b2 = enum2.MoveNext();
                                if (!b2) break;
                            }
                            if (enum1.Current.Key == enum2.Current.Key)
                            {
                                s1 += Math.Abs(enum1.Current.Value - enum2.Current.Value);
                                b1 = enum1.MoveNext(); b2 = enum2.MoveNext();
                                if (!b1 || !b2) break;
                            }
                        }
                    if (!b1)
                        do
                        {
                            s1 += enum2.Current.Value;
                        } while (enum2.MoveNext());
                    if (!b2)
                        do
                        {
                            s1 += enum1.Current.Value;
                        } while (enum1.MoveNext());

                    s2 = TS.TotalFrecuency;
                    s3 += s1 / (2.0f * s2);
                }
                s += s3;
            }
            return (1.0f - (1.0f / 6.0f) * s) * 255f;
        }
    }
}
