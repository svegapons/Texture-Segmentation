using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [TextureSpectrumDescriptor]
    [Algorithm("Black - White Simmetry", "Calculates the Black - White Symmetry feature of a grey levels image using the Texture Spectrum Model.")]
    [Abbreviation("ts_bws", "Deltha", "WinSize")]
    public class Black_WhiteSymmetry: TextureSpectrum
    {
        protected override float Espectrum()
        {
            long s1 = 0;
            long s2 = 0;
            //for (int i = 0; i < 6561; i++)
            //{
            //    if(i<3280)
            //        s1 += Math.Abs(TS[i] - TS[3281 + i]);
            //    s2 += TS[i];
            //}
            //return (1.0f - Convert.ToSingle(s1) / Convert.ToSingle(s2))*255.0f;


            //LinkedList<KeyValuePair<long, int>> list1 = new LinkedList<KeyValuePair<long, int>>();
            //LinkedList<KeyValuePair<long, int>> list2 = new LinkedList<KeyValuePair<long, int>>();
            SortedList<long, int> list1 = new SortedList<long, int>();
            SortedList<long, int> list2 = new SortedList<long, int>();
            foreach (KeyValuePair<long, int> entry in TS)
            {
                //if (entry.Key < 3280)
                //    list1.AddLast(entry);
                //else if (entry.Key >= 3281)
                //    list2.AddLast(entry);
                if (entry.Key < 3280)
                    list1.Add(entry.Key, entry.Value);
                else if (entry.Key >= 3281)
                    list2.Add(entry.Key, entry.Value);

                s2 += entry.Value;
            }
            IEnumerator<KeyValuePair<long, int>> enum1 = list1.GetEnumerator();
            IEnumerator<KeyValuePair<long, int>> enum2 = list2.GetEnumerator();
            bool b1 = enum1.MoveNext();
            bool b2 = enum2.MoveNext();
            if (b1 && b2)
                while (true)
                {
                    if (enum1.Current.Key + 3281 < enum2.Current.Key)
                    {
                        s1 += enum1.Current.Value;
                        b1 = enum1.MoveNext();
                        if (!b1) break;
                    }
                    if (enum1.Current.Key + 3281 > enum2.Current.Key)
                    {
                        s1 += enum2.Current.Value;
                        b2 = enum2.MoveNext();
                        if (!b2) break;
                    }
                    if (enum1.Current.Key + 3281 == enum2.Current.Key)
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

            return (1.0f - Convert.ToSingle(s1) / Convert.ToSingle(s2)) * 255.0f;

        }
    }
}
