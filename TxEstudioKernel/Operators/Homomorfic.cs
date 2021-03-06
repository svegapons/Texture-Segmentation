using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;
using System.Drawing;

namespace TxEstudioKernel.Operators
{
    /// <summary>
    /// Clase base para filtrado homomorfico 
    /// </summary>
    /// <remarks>
    /// Esta clase define el método Process, pero no define una función de filtrado H(u,v).
    /// Las clases herederas deben de definir el método FilterFunction con el cual se dará implementación a la función de filtrado.
    /// </remarks>
    public abstract class Homomorfic : DFTFilt
    {

        public unsafe override TxImage Process(TxImage input)
        {
            TxMatrix matrix = (input.ImageFormat == TxImageFormat.GrayScale) ? TxMatrix.FromImage(input) : TxMatrix.FromImage(input.ToGrayScale());
            //TxMatrix result = new TxMatrix(input.Height, input.Width);
            int length = matrix.Height * matrix.Width;

            //Aplicando logaritmos
            _cvMat* innerMatrix = (_cvMat*)matrix.InnerMatrix;
            float* matrixData = (float*)innerMatrix->data;
            for (int i = 0; i < length; i++)
            {
                *matrixData = (float)Math.Log(*matrixData + 1);
                matrixData++;
            }
            //Aplicando DFT, funcion de filtrado e IDFT
            TxMatrix result = Process(matrix);
            //Aplicando exponecial
            innerMatrix = (_cvMat*)result.InnerMatrix;
            matrixData = (float*)innerMatrix->data;
            for (int i = 0; i < length; i++)
            {
                *matrixData = (float)Math.Exp(*matrixData) - 1;
                matrixData++;
            }

            return result.ToImage();
        }
    }


    /// <summary>
    /// Clase para realizar el filtrado homomórfico de una imagen.
    /// </summary>
    /// <remarks>Esta clase usa la definición dada por Rafael Gonzales en su libro "Digital Image Processing, Second Edition, Prentice Hall, p.194"</remarks>
    [DigitalFilter]
    [Algorithm("Homomorfic Filter", "Homomorfic Filter to obtain the image invariant illumination.")]
    [Abbreviation("norm_homo", "GammaL", "GammaH", "C", "D0")]    
    public class GaussianHiPassHomomorficFilter:Homomorfic
    {
        #region Vars
        private float gamma_L = 0.5f, gamma_H = 2.0f;
        private float c = 0.5f;
        private float d0 = 30;
        #endregion

        #region Public Propierties
        [Parameter("Filter function min value", "A value less than 1 used for decrease the contribution made by the low frequencies (ilumination).")]
        [RealInRange(0,1)]
        public float GammaL
        {
            get { return gamma_L; }
            set { gamma_L = value; }
        }
        [Parameter("Filter function max value", "A value greather than 1 used to amplify the contribution made by high frequencies (reflectance)")]
        [RealInRange(1,float.MaxValue)]
        public float GammaH
        {
            get { return gamma_H; }
            set { gamma_H = value; }
        }
        [Parameter("Filter function slope", "Constant to control the sharpness of the slope of the filter function as it transitions between γL and γH")]
        [RealInRange(float.Epsilon,float.MaxValue)]
        public float C
        {
            get { return c; }
            set { c = value; }
        }
        [Parameter("Cutoff frequency", "Cutoff frequency at the distance D0 from the origin.")]
        [RealInRange(float.Epsilon,float.MaxValue)]
        public virtual float D0
        {
            get { return d0; }
            set { d0 = value; }
        }
        #endregion

        /// <summary>
        /// Función de filtrado homomórfico.
        /// </summary>
        /// <param name="u">Abscisa del punto.</param>
        /// <param name="v">Ordenada del punto.</param>
        /// <param name="hc">Centro vertical de la imagen. Es la ordenada del punto central de la transformada de Fourier de la imagen.</param>
        /// <param name="vc">Centro horizontal de la imagen. Es la abscisa del punto central de la transformada de Fourier de la imagen.</param>
        /// <returns>Devuelve el valor de la función de filtrado homomórico en las coordenadas (u,v) de la transformada de Fourier.</returns>
        /// <remarks>Esta función es la propuesta por Rafael González en el libro "Digital Image Processing, Second Edition, Prentice Hall, p.194".
        /// Es una verisón modificada del filtro paso alto Gaussiano.
        /// </remarks>
        protected override float FilterFunction(int u, int v, int hc, int vc)
        {
            return (gamma_H - gamma_L) * (1.0f - (float)Math.Exp(-c * (D2(u, v, hc, vc) / (D0 * D0)))) + gamma_L;
            //return 1.0f - (float)Math.Exp(-D2(u, v, hc, vc) / (2.0f*D0 * D0));
        }
    }
}
