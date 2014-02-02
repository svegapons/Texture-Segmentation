using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [EdgeDetector]
    [Algorithm("Sobel2", "Calculates the Sobel edge feature using a Sobel mask of 3x3 coefficients.")]
    [Abbreviation("sob2", "Umbral")]
    public class Sobel2 : Sobel
    {
        //[Algorithm("Sobel2", "Detector de bordes de Sobel, variante2")]
        public Sobel2() : base() { }
        /// <summary>
        /// Halla el color de un pixel
        /// </summary>
        /// <remarks>Lo halla como la suma de los valores absolutos de las derivadas con respecto a los ejes X y Y en el pixel</remarks>
        /// <returns>Color del pixel</returns>
        public override float Color(TxEstudioKernel.TxImage gx, TxEstudioKernel.TxImage gy, int x, int y)
        {
            double result = Math.Abs((double)gx[x, y, ColorChannel.Red]) + Math.Abs((double)gy[x, y, ColorChannel.Red]);
            return (float)result;
        }
    }
}
