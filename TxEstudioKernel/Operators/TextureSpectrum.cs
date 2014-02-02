using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;
using System.Collections;

namespace TxEstudioKernel.Operators
{
    public abstract class TextureSpectrum:TextureDescriptor
    {
        #region Vars
        /// <summary>
        /// Tabla LUT para las potencias de 3
        /// </summary>
        private static int[] LUT_Pot3 ={ 1, 3, 9, 27, 81, 243, 729, 2187 };
        private int deltha=5;
        private int DIM=15;
        private TSHistogram ts;
        protected byte[] e = new byte[8];
        #endregion


        public TextureSpectrum()
        {
            ts = new TSHistogram();
        }

        internal static byte[] recons(long ntu)
        {
            byte[] e = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                if (ntu < 3)
                {
                    e[i] = Convert.ToByte(ntu);
                    break;
                }
                else
                {
                    e[i] = Convert.ToByte(ntu % 3);
                    ntu /= 3;
                }
            }
            return e;
        }

        #region Parameters
        [Parameter("Delta", "Small positive integer value")]
        public int Deltha
        {
            get { return deltha; }
            set
            {
                deltha = value;
                if (deltha < 0) throw new InvalidOperationException("Deltha debe ser un número mayor o igual que 0");
            }
        }

        [Parameter("Neighborhood", "Size of neighborhood in pixels")]
        [IntegerInSequence(3, int.MaxValue, 2)]
        public int WinSize
        {
            get { return DIM; }
            set
            {
                DIM = value;
                if (DIM < 3) throw new InvalidOperationException("El tamaño de la ventana debe ser mayor que 3");
            }
        }
        #endregion

        #region ITextureDescriptor Members
        public override TxMatrix GetDescription(TxImage input)
        {
            TxImage image = input;
            if (input.ImageFormat == TxImageFormat.RGB)
                image = input.ToGrayScale();

            int W = image.Width;
            int H = image.Height;
            int D = (DIM - 1) / 2;
            float spectrum = 0;

            TxMatrix output = new TxMatrix(image.Height, image.Width);

            WindowOp<int> ut = new WindowOp<int>(DIM - 2);
            
            for (int Y = D; Y < (H - D); Y++)
                for (int X = D; X < (W - D); X++)
                {
                    if (X == D) Create(Y - D, image, ut);
                    else Update(Y - D, X + D, image, ut);
                    spectrum = Espectrum();
                    output[Y, X] = spectrum;
                }
            FillMatrix(output);
            return output;
        }

        protected abstract float Espectrum();

        private void Create(int row, TxImage image, WindowOp<int> ut)
        {
            int tu = 0;
            //for (int j = 0; j < 6561; j++) TS[j] = 0;
            TS.Clear();

            for (int x = 1; x < DIM - 1; x++)
                for (int y = row + 1, k = 0; k < DIM - 2; y++, k++)
                {
                    tu = TextureUnit(x, y, image);
                    ut[x - 1, k] = tu;
                    TS[tu]++;
                }
        }

        private void Update(int row, int col, TxImage image, WindowOp<int> ut)
        {
            for (int j = 0; j < DIM - 2; j++)
                TS[ut[0, j]]--;


            int tu = 0;
            for (int j = row + 1, k = 0; k < DIM - 2; j++, k++)
            {
                //tu = TextureUnit(DIM - 2, j, ng);
                tu = TextureUnit(col - 1, j, image);
                //ut[0, j - 1] = tu;
                ut[0, k] = tu;
                TS[tu]++;
            }
            ut.Shift();
        }

        private int TextureUnit(int x, int y, /*WindowOp<byte>*/TxImage image)
        {
            byte e;
            int suma = 0;
            int[] V = new int[8];
            int V0 = Convert.ToInt32(image[x, y]);

            V[0] = Convert.ToByte(image[x - 1, y - 1]);
            V[1] = Convert.ToByte(image[x, y - 1]);
            V[2] = Convert.ToByte(image[x + 1, y - 1]);
            V[3] = Convert.ToByte(image[x + 1, y]);
            V[4] = Convert.ToByte(image[x + 1, y + 1]);
            V[5] = Convert.ToByte(image[x, y + 1]);
            V[6] = Convert.ToByte(image[x - 1, y + 1]);
            V[7] = Convert.ToByte(image[x - 1, y]);

            int menos_deltha = V0 - deltha;
            int mas_deltha = V0 + deltha;

            for (int i = 0; i < 8; i++)
            {
                if (V[i] < menos_deltha) e = 0;
                else if (V[i] > mas_deltha) e = 2;
                else e = 1;
                suma += (e * LUT_Pot3[i]);
            }
            return suma;
        }
        #endregion
        protected virtual TSHistogram TS
        {
            get
            {
                return ts;
            }
            set
            {
                ts = value;
            }
        }
        /// <summary>
        /// Rellena los pixeles de las esquinas replicando los valores de los pixeles hubicados en los extremos
        /// </summary>
        /// <param name="matrix">Matriz de float que contiene información obtenida de haber aplicado un descriptor de espectro de textura</param>
        private void FillMatrix(TxMatrix matrix)
        {
            //Rellenado esquina izquierda
            int i = 0, j = 0;
            int D = (DIM - 1) / 2;
            for (i = D - 1; i < matrix.Height; i++)
                for (j = D; j >= 0; j--)
                    matrix[i, j] = matrix[i, j + 1];
            //Rellenando esquina derecha
            for (i = D - 1; i < matrix.Height; i++)
                for (j = matrix.Width - (D + 1); j < matrix.Width; j++)
                    matrix[i, j] = matrix[i, j - 1];
            //Rellenando esquina superior
            for (i = D - 1; i >= 0; i--)
                for (j = 0; j < matrix.Width; j++)
                    matrix[i, j] = matrix[i + 1, j];
            //Rellenando esquina inferior
            for (i = matrix.Height - (D + 1); i < matrix.Height; i++)
                for (j = 0; j < matrix.Width; j++)
                    matrix[i, j] = matrix[i - 1, j];
        }
    }
    class WindowOp<T>
    {
        long first;
        T[,] matrix;
        internal WindowOp(int winsize)
        {
            matrix = new T[winsize, winsize];
            first = 0;
        }
        //internal WindowOp(T[,] matrix)
        //{
        //    this.matrix = (T)matrix.Clone();
        //}
        internal T this[int x, int y]
        {
            get
            {
                return matrix[y, (first + x) % matrix.GetLength(1)];
            }
            set
            {
                matrix[y, (first + x) % matrix.GetLength(1)] = value;
            }
        }

        internal void Shift()
        {
            first++;
        }
    }
    /// <summary>
    /// Histograma del espectro de textuta. Por cada número de textura se almacena la frecuencia de ocurrencia de este en una vecindad
    /// </summary>
    /// <remarks>Esta clase admite 6561 números de texturas</remarks>
    public class TSHistogram : IEnumerable<KeyValuePair<long, int>>
    {
        //static int[,] Sj;
        int[] S;

        //static int[,] invSj;
        SortedList<long, int> list;

        public TSHistogram()
        {
            S = new int[6561];
            list = new SortedList<long, int>();
            //if (Sj == null)
            //{
            //    Sj = new int[7, 6561];

            //    invSj = new int[7, 6561];

            //    byte[] e = null;
            //    int suma = 0;
            //    for (int i = 0; i < 6561; i++)
            //    {
            //        e = recons(i);
            //        for (byte j = 1; j < 8; j++)
            //        {
            //            suma = e[(0 + j) % 8] + e[(1 + j) % 8] * 3 + e[(2 + j) % 8] * 9 + e[(3 + j) % 8] * 27 + e[(4 + j) % 8] * 81 + e[(5 + j) % 8] * 243 + e[(6 + j) % 8] * 729 + e[(7 + j) % 8] * 2187;
            //            Sj[j - 1, i] = suma;

            //            invSj[j - 1, suma] = i;
            //        }
            //    }
            //}
        }
        //internal TSHistogram(long totalNTU)
        //{
        //    S = new int[totalNTU];
        //    Sj=new int[7,totalNTU];
        //}
        //public virtual int this[long i, byte j]
        //{
        //    get
        //    {
        //        //if (j == 0) return S[i];
        //        //return S[Sj[j-1, i]];
        //        if (j == 0) return this[i];
        //        return this[Sj[j - 1, i]];

        //    }
        //}
        /// <summary>
        /// Devuelve o establece el valor de frecuencia de el número de textura i
        /// </summary>
        /// <param name="i">Nuémro de textura. Debe de estar entre 0 y 6560. </param>
        /// <returns></returns>
        public virtual int this[long i]
        {
            get
            {
                return S[i];
                //if (!list.ContainsKey(i))
                //    return 0;
                //return list[i];
            }
            set
            {
                bool contains = list.ContainsKey(i);
                if (!contains && value != 0)
                {
                    list.Add(i, value);
                }
                else if (contains && value == 0)
                    list.Remove(i);
                else if (contains)
                    list[i] = value;

                S[i] = value;

            }
        }
        /// <summary>
        /// Cantidad de números de texturas.
        /// </summary>
        /// <remarks>Para esta implementación la cantidad de números de textura es 6561.
        /// Este método puede ser útil para clases si en clases herederas se desea generalizar la cantidad de números de texturas según lo propuesto por He.
        /// </remarks>
        public virtual long LongLength
        {
            get
            {
                return S.LongLength;
            }
        }
        /// <summary>
        /// Cantidad de números de texturas.
        /// </summary>
        /// <remarks>Para esta implementación la cantidad de números de textura es 6561.
        /// Este método puede ser útil para clases si en clases herederas se desea generalizar la cantidad de números de texturas según lo propuesto por He.
        /// </remarks>
        public virtual int Lenght
        {
            get
            {
                return S.Length;
            }
        }

        //public virtual IEnumerator<KeyValuePair<long,int>> GetFrecuencyEnumerator(byte j)
        //{
        //    if (j == 0) return list.GetEnumerator();

        //    SortedList<long,int> newList=new SortedList<long,int>();
        //    foreach(KeyValuePair<long,int> entry in list)
        //        newList.Add(invSj[j-1,entry.Key],entry.Value);
        //    return newList.GetEnumerator();
        //}

        public virtual int TotalFrecuency
        {
            get
            {
                int result = 0;
                foreach (KeyValuePair<long, int> entry in list)
                    result += entry.Value;
                return result;
            }
        }

        public virtual void Clear()
        {
            list.Clear();
            S = new int[6561];
        }




        #region IEnumerable<KeyValuePair<long,int>> Members
        /// <summary>
        /// Enumera los ntu distintos de 0 calculados por la forma de ordenamiento a,b,...,h usual.
        /// </summary>
        /// <returns></returns>

        public IEnumerator<KeyValuePair<long, int>> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// Espectro de textura generalizada para poder acceder al número de textura obtenido por alguna de las ocho formas de etiquetado.
    /// </summary>
    public class ExtendedTSHistogram : TSHistogram
    {
        static int[,] Sj;
        static int[,] invSj;

        public ExtendedTSHistogram()
        {
            if (Sj == null)
            {
                Sj = new int[7, 6561];

                invSj = new int[7, 6561];

                byte[] e = null;
                int suma = 0;
                for (int i = 0; i < 6561; i++)
                {
                    e = recons(i);
                    for (byte j = 1; j < 8; j++)
                    {
                        suma = e[(0 + j) % 8] + e[(1 + j) % 8] * 3 + e[(2 + j) % 8] * 9 + e[(3 + j) % 8] * 27 + e[(4 + j) % 8] * 81 + e[(5 + j) % 8] * 243 + e[(6 + j) % 8] * 729 + e[(7 + j) % 8] * 2187;
                        Sj[j - 1, i] = suma;

                        invSj[j - 1, suma] = i;
                    }
                }
            }
        }
        /// <summary>
        /// Obtiene la frecuencia del número de textura i en la forma de ordenamiento j.
        /// </summary>
        /// <param name="i">Número de textura. El valor de i debe de estar en el rango 0 a 6561.</param>
        /// <param name="j">Forma de ordenamiento j. La forma de ordenamiento cambia en sentido de las manecillas del reloj. j debe de estar entre 0 y 7</param>
        /// <returns></returns>
        public virtual int this[long i, byte j]
        {
            get
            {
                //if (j == 0) return S[i];
                //return S[Sj[j-1, i]];
                if (j == 0) return this[i];
                return this[Sj[j - 1, i]];

            }
        }
        /// <summary>
        /// Enumerador de números de texturas obtenidos por la forma de ordenamiento j.
        /// </summary>
        /// <param name="j">Forma de ordenamiento j. Las formas de ordenamiento son obtenidas en sentido de las manecillas del reloj. j está entre 0 y 7</param>
        /// <returns></returns>
        public virtual IEnumerator<KeyValuePair<long, int>> GetFrecuencyEnumerator(byte j)
        {
            if (j == 0) return GetEnumerator();

            SortedList<long, int> newList = new SortedList<long, int>();
            foreach (KeyValuePair<long, int> entry in this)
                newList.Add(invSj[j - 1, entry.Key], entry.Value);
            return newList.GetEnumerator();
        }
        /// <summary>
        /// Reconstruye el conjunto Unidad de Textura definido como TU={E1,E2,...,E8}
        /// </summary>
        /// <param name="ntu">Número de textura</param>
        private static byte[] recons(long ntu)
        {
            byte[] e = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                if (ntu < 3)
                {
                    e[i] = Convert.ToByte(ntu);
                    break;
                }
                else
                {
                    e[i] = Convert.ToByte(ntu % 3);
                    ntu /= 3;
                }
            }
            return e;
        }
    }

}
