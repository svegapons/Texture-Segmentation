using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Marr/Hildreth", "Calculates the Marr/Hildreth edge feature using a square Window of size Nxy and a Gaussian Standard Deviation specified.")]
    [EdgeDetector]
    [Abbreviation("m_h", "Std_dev","TemplateSize")]
    public class MarrHildreth_EdgeDetector:TxOneBand
    {
        private float std_dev = 1.5f;

        [Parameter("Gaussian Standard Deviation", "Gaussian Standard Deviation for the Gaussian operator")]
        [RealInRange(0.0f,float.MaxValue)]
        public float Std_dev
        {
            get { return std_dev; }
            set { std_dev = value; }
        }

        int templateSize = 11;
        [Parameter("Window size", "Windows size for the convolution")]
        [IntegerInSequence(3,59,2)]
        public int TemplateSize
        {
            get 
            {
                return templateSize;
            }
            set 
            {
                templateSize = value;
            }
        }

        //public virtual TxImage Convolve(TxImage image, float[,] mask)
        //{
        //    //TxImage newImage = new TxImage(image.Width, image.Height);
        //    //TxImage newImage = new TxImage(image.Width, image.Height, TxImageFormat.GrayScale);
        //    TxImage newImage=image;
        //    if (image.ImageFormat == TxImageFormat.RGB)
        //        newImage = image.ToGrayScale();

        //    int M = image.Width;
        //    int N = image.Height;
        //    int m = mask.GetLength(0);
        //    int n = mask.GetLength(1);
        //    int a = (m - 1) / 2, b = (n - 1) / 2;

        //    for (int x = a; x < M - a; x++)
        //        for (int y = b; y < N - a; y++)
        //        {
        //            float sum = 0;
        //            for (int s = 0; s < m; s++)
        //                for (int t = 0; t < n; t++)
        //                    //sum += image[x + s - a, y + t - b].Red * mask[s, t];
        //                    sum += image[x + s - a, y + t - b] * mask[s, t];
        //            newImage[x, y] = sum;
        //        }
        //    return newImage;
        //}

        /// <summary>
        /// Crea una máscara para la convolución de tamaño mrows*ncols utilizando el Lapplaciano del Gaussiano (LoG)
        /// </summary>
        /// <param name="nrows">Cantidad de filas de la máscara</param>
        /// <param name="ncols">Cantidad de columans de la máscara</param>
        /// <param name="std_dev">Desviación estándar</param>
        /// <returns></returns>
        protected virtual TxMatrix LoG(int nrows, int ncols, float std_dev)
        {
            //float sigma2 = std_dev * std_dev;
            //float inv_sigma2 = 1f / sigma2;
            //float two_sigma2 = 2 * sigma2;
            TxMatrix template = new TxMatrix(nrows, ncols);
            int cx = (ncols - 1) / 2;
            int cy = (nrows - 1) / 2;
            int nx = 0, ny = 0;
            float sigma2 = std_dev * std_dev;
            float inv_two_pi_sigma4 = -1.0f / 2.0f*Convert.ToSingle( Math.PI * sigma2 * sigma2);
            float expr = 0;

            for (int x = 0; x < ncols; x++)
                for (int y = 0; y < nrows; y++)
                {
                    nx = x - cx;
                    ny = y - cy;
                    //nx2_plus_ny_2 = nx * nx + ny * ny;
                    expr = (nx * nx + ny * ny) / sigma2;
                    //template[y, x] = Convert.ToSingle(inv_sigma2 * (nx2_plus_ny_2 / sigma2 - 2) * Math.Exp(-(nx2_plus_ny_2 / two_sigma2)));
                    template[y, x] = Convert.ToSingle(inv_two_pi_sigma4 * (2.0 - expr) * Math.Exp(-(0.5 * expr)));
                }
            return template;

        }
        protected virtual TxImage ZeroCrossing(TxMatrix image)
        {
            TxImage newImage = new TxImage(image.Width, image.Height, TxImageFormat.GrayScale);
            for (int i = 1; i < newImage.Height - 1; i++)
                for (int j = 1; j < newImage.Width - 1; j++)
                {
                    float[] array = new float[4];
                    //Revisar
                    array[0] = (image[i - 1, j - 1] + image[i - 1, j] + image[i, j - 1] + image[i, j]);
                    array[1] = (image[i - 1, j] + image[i - 1, j + 1] + image[i, j] + image[i, j + 1]);
                    array[2] = (image[i, j - 1] + image[i, j] + image[i + 1, j - 1] + image[i + 1, j]);
                    array[3] = (image[i, j] + image[i, j + 1] + image[i + 1, j] + image[i + 1, j + 1]);
                    float maxval = Max(array);
                    float minval = Min(array);
                    if (maxval * minval < 0)
                        newImage[j, i] = 255;
                    else newImage[j, i] = 0; 
                }
            return newImage;
        }
        private float Min(float[] array)
        {
            float min = array[0];
            for (int i = 1; i < array.Length; i++)
                if (array[i] < min)
                    min = array[i];
            return min;
        }
        private float Max(float[] array)
        {
            float max = array[0];
            for (int i = 1; i < array.Length; i++)
                if (array[i] > max)
                    max = array[i];
            return max;
        }
        public override TxImage Process(TxImage input)
        {
            TxMatrix template = LoG(templateSize, templateSize, std_dev);
            if (input.Width < template.Width || input.Height < template.Height)
                return null;
            TxMatrix floatImage = TxMatrix.FromImage(input);
            TxMatrix newImage = floatImage.Convolve(template);       
            return ZeroCrossing(newImage);
        }
    }
}
