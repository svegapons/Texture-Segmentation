using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    /// <summary>
    /// Clase base para los rasgos de textura Run Length
    /// </summary>
    public abstract class RunLength:TextureDescriptor
    {
        #region Vars
        /// <summary>
        /// Matrix de dirección
        /// </summary>
        private static int[,] IHOKO ={ { 0, 1, -1, -1 }, { 1, -1, 0, -1 } };

        private int winSize=3;
        private int direction;

        protected int[] LUT;
        private int ng=16;

        #endregion


        #region Constructor
        public RunLength()
        {
            //int ng = 256;
            //float tmp = 16.0f / ng;
            //float tmp = (float)NG / 256.0f;
            LUT = new int[256];

            //for (int i = 0; i < 256; i++)
            //    LUT[i] = i * tmp;
        }
        #endregion

        /// <summary>
        /// Calcula la matriz de run length
        /// </summary>
        /// <param name="Win">Arreglo de la imagen</param>
        /// <param name="nNgImage">Niveles de gris de la imagen</param>
        /// <param name="nMaxRL">Máximo de run length</param>
        /// <param name="direction">Dirección del run length</param>
        /// <returns></returns>
        protected virtual int[,] rlength(int[,] Win, int nNgImage, int nMaxRL, int direction)
        {
            //int[,] IHOKO = new int[2, 4];
            //IHOKO[0, 0] = 1; IHOKO[1, 0] = 0;    //0 grados
            //IHOKO[0, 1] = 1; IHOKO[1, 1] = -1;   //45 grados
            //IHOKO[0, 2] = 0; IHOKO[1, 2] = -1;    //90 grados
            //IHOKO[0, 3] = -1; IHOKO[1, 3] = -1;   //135 grados

            int[,] MRL = new int[nNgImage, nMaxRL];

            int isy = Win.GetLength(1);
            int isx = Win.GetLength(0);
            int ipd = 0;
            int jx = 0, jy = 0;
            int irunl = 0;

            //Especificando dirección de búsqueda
            int ixd = IHOKO[0, direction];
            int iyd = IHOKO[1, direction];

            //Calculo del run length
            for (int iy = 0; iy < isy; iy++)
                for (int ix = 0; ix < isx; ix++)
                {
                    ipd = Win[ix, iy];
                    //Chequeo del punto de comienzo
                    jx = ix - ixd;
                    jy = iy - iyd;

                    if (jx >= 0 && jx < isx && jy >= 0 && jy < isy && Win[jx, jy] == ipd)
                        continue;

                    //sigue el run

                    for (irunl = -1, jx = ix, jy = iy; jx >= 0 && jx < isx && jy >= 0 && jy < isy && Win[jx, jy] == ipd; irunl++, jx += ixd, jy += iyd) ;

                    //Almacena al run length
                    ipd = Math.Max(0, Math.Min(nNgImage - 1, ipd));
                    irunl = Math.Min(nMaxRL - 1, irunl);
                    MRL[ipd, irunl]++;
                }
            
            return MRL;
        }

        /// <summary>
        /// Crea una matriz de run length en una vecindad con centro en x,y y de tamaño nMaxRL x nMaxRL
        /// </summary>
        /// <param name="image">Imagen</param>
        /// <param name="x">Abscisa del centro</param>
        /// <param name="y">Ordenada del centro</param>
        /// <param name="nNgImage">Niveles de gris de la imagen</param>
        /// <param name="nMaxRL">Máxima longitud del run</param>
        /// <param name="direction">Ángulo de dirección. Debe ser uno de los siguientes valores 0, 45, 90, 135 grados</param>
        /// <returns>Matriz de run length asociada a una vecindad de tamaño nMaxRL x nMaxRL con centro en x,y</returns>
        protected virtual int[,]/*RunLengthMatrix*/ rlength(TxImage image, int x, int y,int nNgImage, int nMaxRL, int direction)
        {
            int[,] MRL = new int[nNgImage, nMaxRL];
            //RunLengthMatrix MRL=new RunLengthMatrix(NG, winSize);
            int ipd = 0;
            int jx = 0, jy = 0;
            int irunl = 0;

            int ixd = IHOKO[0, direction];
            int iyd = IHOKO[1, direction];

            int D = (winSize - 1) / 2;
            int X = x-D;
            int Y = y-D;

            for(int iy=0;iy<nMaxRL;iy++)
                for (int ix = 0; ix < nMaxRL; ix++)
                {
                    ipd = LUT[image[X + ix, Y + iy]];

                    jx = X+(ix - ixd);
                    jy = Y+(iy - iyd);

                    if (jx >= X && jx < Math.Min(X + nMaxRL, image.Width) && jy >= Y && jy < Math.Min(Y + nMaxRL, image.Height) && LUT[image[jx, jy]] == ipd)
                        continue;

                    for (irunl = -1, jx = X+ix, jy = Y+iy; jx >= X && jx < Math.Min(X + nMaxRL, image.Width) && jy >= Y && jy < Math.Min(Y + nMaxRL, image.Height) && LUT[image[jx, jy]] == ipd; irunl++, jx += ixd, jy += iyd) ;
                    ipd = Math.Max(0, Math.Min(nNgImage - 1, ipd));
                    irunl = Math.Min(nMaxRL - 1, irunl);
                    MRL[ipd, irunl]++;
                }

            return MRL;
        }
        /// <summary>
        /// Cuando es implementada por una clase heredera brinda una forma de calcular un descriptor de textura Run Length  
        /// </summary>
        /// <param name="MRL">Matrix de run length</param>
        /// <param name="nNgImage">Niveles de gris de la imagen</param>
        /// <param name="nMaxRL">Máximo run length</param>
        /// <returns></returns>
        protected abstract float Run(int[,] MRL, int nNgImage, int nMaxRL);

        protected virtual void Wnd(int col, int row, int[,] win, TxImage image)
        {
            if (win.GetLength(0) != win.GetLength(1))
                throw new InvalidOperationException("La cantidad de filas y de columnas de la ventana deben coincidir.");
            int d = (win.GetLength(0) - 1) / 2;

            for (int y = 0, n = -d; y < win.GetLength(1); y++, n++)
                for (int x = 0, m = -d; x < win.GetLength(0); x++, m++)
                {
                    //win[x, y] = (int)LUT[(int)image[i+m, j+n].Red];
                    win[x, y] = image[col + m, row + n];
                    win[x, y] = LUT[win[x, y]];
                }
        }


        #region Public Properties
        [Parameter("Windows Size", "The size in pixels of the window operation")]
        [IntegerInSequence(3, 7, 2)]
        public int WinSize
        {
            get { return winSize; }
            set { winSize = value; }
        }

        [Parameter("Azimut", "Run azimut")]
        [IntegerInSequence(0, 135, 45)]
        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        [Parameter("Gray Levels Scale", "Numbre of gray levels")]
        [IntegerInSequence(16,32,16)]
        public virtual int NG
        {
            get { return ng; }
            set { ng = value; }
        }

        #endregion
        #region ITextureDescriptor Members

        public override TxMatrix GetDescription(TxImage input)
        {
            int W = input.Width;
            int H = input.Height;
            TxImage image = input;
            if (input.ImageFormat == TxImageFormat.RGB)
                image = input.ToGrayScale();

            float tmp = (float)NG / 256.0f;
            for (int i = 0; i < 256; i++)
                LUT[i] = (int)(i * tmp);

            int D = (WinSize - 1) / 2;
            int[,] Win = new int[winSize, winSize];
            //int ng = 32;
            int[,] MRL = null;
            //RunLengthMatrix MRL=null;
            TxMatrix output = new TxMatrix(input.Height, input.Width);

            for (int Y = D; Y < H - D; Y++)
                for (int X = D; X < W - D; X++)
                {
                    //Wnd(X, Y, Win, image);
                    //MRL = rlength(Win, NG, winSize, direction / 45);
                    MRL = rlength(image, X, Y, NG, this.WinSize, direction / 45);
                    float tempo = Run(MRL, NG, winSize);
                    tempo = tempo * (255);
                    output[Y, X] = tempo;
                }
            FillMatrix(output);
            return output;
        }

        #endregion
        /// <summary>
        /// Rellena los pixeles de las esquinas replicando los valores de los pixeles hubicados en los extremos
        /// </summary>
        /// <param name="matrix">Matriz de float que contiene información obtenida de haber aplicado un descriptor de espectro de textura</param>
        private void FillMatrix(TxMatrix matrix)
        {
            //Rellenado esquina izquierda
            int i = 0, j = 0;
            int D = (winSize - 1) / 2;
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

    public class RunLengthMatrix
    {
        SortedList<int,int>[] grayLevels;
        public RunLengthMatrix(int grayLevels, int runLength)
        {
            this.grayLevels = new SortedList<int,int>[runLength];
            for (int i = 0; i < runLength; i++)
                this.grayLevels[i] = new SortedList<int,int>();
        }
        public virtual int this[int grayLevel, int run]
        {
            get
            {
                if (grayLevels[run].ContainsKey(grayLevel))
                    return grayLevels[run][grayLevel];
                return 0;
            }
            set
            {
                bool contains = grayLevels[run].ContainsKey(grayLevel);
                if (contains && value != 0)
                    grayLevels[run][grayLevel] = value;
                else if (!contains && value != 0)
                    grayLevels[run].Add(grayLevel, value);
                else if (contains && value == 0)
                    grayLevels[run].Remove(grayLevel);
            }
        }
        public virtual IEnumerator<KeyValuePair<int, int>> GetGrayLevelEnumerator(int run)
        {
            return grayLevels[run].GetEnumerator();
        }
    }
}
