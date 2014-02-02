using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel
{
    /// <summary>
    /// Clase base de los operadores puntuales con tratamiento de los tres  canales de colores.
    /// </summary>
    public abstract class TxColorPixelWise : TxPixelWiseOperator
    {
        /// <summary>
        /// Los herederos de esta clase, deben implementar el procesamiento del pixel en este metodo.
        /// </summary>
        public abstract ColorVector ProcessColorPixel(ColorVector colorPixel);

        public override TxImage Process(TxImage input)
        {
            throw new NotImplementedException();
            //TxImage result = new TxImage(input.Width, input.Height, TxImageFormat.RGB);
            //ColorVector a, b;
            //for (int i = 0; i < result.Width; i++)
            //{
            //    for (int j = 0; j < result.Height; j++)
            //    {
            //        a = input[i, j];
            //        b = ProcessColorPixel(a);
            //        result[i, j] = b;
            //        //result[i, j] = ProcessColorPixel(input[i, j]);
            //    }
            //}
            //return result;
        }
    }
}
