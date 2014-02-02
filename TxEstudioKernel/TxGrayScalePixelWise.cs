using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel
{
    /// <summary>
    /// Clase base de los operadores puntuales con pixeles en escala de gris.
    /// </summary>
    public abstract class TxGrayScalePixelWise : TxPixelWiseOperator
    {
        /// <summary>
        /// Los herederos de esta clase deben implementar el procesamiento del pixel en este metodo.
        /// </summary>
        public abstract float ProcessGrayPixel(float grayPixel);
    }
}
