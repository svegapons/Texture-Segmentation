using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.OpenCV;
using System.Drawing;

namespace TxEstudioKernel.Operators
{
    /// <summary>
    /// Clase base para los operadores de filtrado en el dominio de frecuencias
    /// </summary>
    /// <remarks>
    /// Las bases del filtrado de imagenes en el domino de frecuencias son:
    /// 1- Calcular F(u,v), la DFT de una una imagen.
    /// 2- Multiplicar F(u,v) por una función de filtrado H(u,v).
    /// 3- Calcular la inversa DFT del resultado de 2.
    /// Esta clase realiza estas tres operaciones, pero no da implementación a la función de filtrado H(u,v).
    /// La función de filtrado la deben definir las clases herederas. Para ello se debe redefinir el método protected FilterFunction(int u, int v, int hc, int vc).
    /// </remarks>
    public abstract class DFTFilt : TxOneBand
    {
        #region Process Methods
        /// <summary>
        /// Realiza el proceso de filtrado de la imagen en el dominio de frecuencias.
        /// </summary>
        /// <param name="input">Imagen original</param>
        /// <returns>Imagen filtrada</returns>
        /// <remarks>
        /// Este método llama al método protected Process(TxMatrix matrix)
        /// </remarks>
        public override TxImage Process(TxImage input)
        {
            TxMatrix matrix = (input.ImageFormat == TxImageFormat.GrayScale) ? TxMatrix.FromImage(input) : TxMatrix.FromImage(input.ToGrayScale());
            return Process(matrix).ToImage();
        }
        /// <summary>
        /// Realiza el proceso de filtrado de la imagen
        /// </summary>
        /// <param name="matrix">Matriz de la imagen</param>
        /// <returns>Matriz de la imagen filtrada</returns>
        /// <remarks>
        /// Este método realiza el proces de filtrado de la matriz de la imagen en el dominio de frecuencias.
        /// 1- Calcular F(u,v), la DFT de una una imagen.
        /// 2- Multiplicar F(u,v) por una función de filtrado H(u,v).
        /// 3- Calcular la inversa DFT del resultado de 2.
        /// </remarks>
        public unsafe virtual TxMatrix Process(TxMatrix matrix)
        {
            int length = matrix.Height * matrix.Width;
            TxMatrix result = new TxMatrix(matrix.Height, matrix.Width);

            //Aplicando FFT
            int dft_M = CXCore.cvGetOptimalDFTSize(matrix.Height);
            int dft_N = CXCore.cvGetOptimalDFTSize(matrix.Width);
            TxMatrix dft_A = new TxMatrix(dft_M, dft_N);

            //TxMatrix dft_B = new TxMatrix(dft_M, dft_N);

            _cvMat tmp = new _cvMat();
            CXCore.cvGetSubRect(dft_A.InnerMatrix, (IntPtr)(&tmp), new Rectangle(0, 0, matrix.Width, matrix.Height));
            CXCore.cvCopy(matrix.InnerMatrix, (IntPtr)(&tmp), IntPtr.Zero);
            CXCore.cvGetSubRect(dft_A.InnerMatrix, (IntPtr)(&tmp), new Rectangle(matrix.Width, 0, dft_A.Width - matrix.Width, matrix.Height));
            CXCore.cvSetZero((IntPtr)(&tmp));
            CXCore.cvDFT(dft_A.InnerMatrix, dft_A.InnerMatrix, 0, matrix.Height);

            //Aplicando filtro
            _cvMat* innerMatrix = (_cvMat*)dft_A.InnerMatrix;
            float* matrixData = (float*)innerMatrix->data;
            int hc = dft_A.Width / 2;
            int vc = dft_A.Height / 2;

            for (int i = 0; i < dft_A.Height; i++)
            {
                for (int j = 0; j < dft_A.Width; j++)
                {
                    *matrixData = FilterFunction(j, i, hc, vc) * (*matrixData);
                    //*matrixData = FilterFunction(j, i, hc, vc);
                    matrixData++;
                }
            }
            //CXCore.cvMulSpectrums(dft_A.InnerMatrix, dft_B.InnerMatrix, dft_A.InnerMatrix, 4);

            //Aplicado IFFT
            CXCore.cvDFT(dft_A.InnerMatrix, dft_A.InnerMatrix, 1 | 2, result.Height);
            CXCore.cvGetSubRect(dft_A.InnerMatrix, (IntPtr)(&tmp), new Rectangle(0, 0, result.Width, result.Height));
            CXCore.cvCopy((IntPtr)(&tmp), result.InnerMatrix, IntPtr.Zero);

            return result;
        }
        #endregion
        #region Protected Methods
        /// <summary>
        /// Distancia al cuadrado de un punto (u,v) al centro de la transformada de Fourier de la imagen.
        /// </summary>
        /// <param name="u">Abscisa del punto.</param>
        /// <param name="v">Ordenada del punto.</param>
        /// <param name="hc">Centro vertical de la imagen. Es la abscisa del punto central de la transformada de Fourier de la imagen.</param>
        /// <param name="vc">Centro horizontal de la imagen. Es la ordenada del punto central de la transformada de Fourier de la imagen.</param>
        /// <returns>Retorna la distancia euclideana al cuadrado del punto (u,v) al punto central de la transformada de Fourier de la imagen (hc,vc).</returns>
        protected virtual float D2(int u, int v, int hc, int vc)
        {
            int dx = u - hc;
            int dy = v - vc;
            return dx * dx + dy * dy;
        }
        #endregion
        #region Filter Function
        /// <summary>
        /// Función de filtrado en el dominio de frecuencias.
        /// </summary>
        /// <param name="u">Abscisa del punto.</param>
        /// <param name="v">Ordenada del punto.</param>
        /// <param name="hc">Centro vertical de la imagen. Es la abscisa del punto central de la transformada de Fourier de la imagen.</param>
        /// <param name="vc">Centro horizontal de la imagen. Es la ordenada del punto central de la transformada de Fourier de la imagen.</param>
        /// <returns>Devuelve el valor de la función de filtrado en las coordenadas (u,v) de la transformada de Fourier.</returns>
        /// <remarks>
        /// Las clases bases deben implementar este método para dar una definición a la función de filtrado.
        /// </remarks>
        protected abstract float FilterFunction(int u, int v, int hc, int vc);
        #endregion
    }
}
