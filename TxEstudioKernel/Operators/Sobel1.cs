using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [EdgeDetector]
    [Algorithm("Sobel1", "Calculates the Sobel edge feature using a Sobel mask of 3x3 coefficients.")]
    [Abbreviation("sob1", "Umbral")]
    public class Sobel1 : Sobel
    {
        //[Algorithm("Sobel1", "Detector de bordes de Sobel, variante1")]
        public Sobel1() : base() { }
        /// <summary>
        /// Calcula el color de un pixel
        /// </summary>
        /// <remarks>Halla el color del pixel como la raiz cuadrada de la suma de la derivada con respecto a X al cuadrado en el pixel x,y con la derivada con respecto al eje Y al cuadrado en el pixel x,y</remarks>
        /// <returns>El color del pixel</returns>
        
        public override float Color(TxEstudioKernel.TxImage gx, TxEstudioKernel.TxImage gy, int x, int y)
        {
            double result = Math.Sqrt(Math.Pow(((double)gx[x, y, ColorChannel.Red]), 2) + Math.Pow(((double)gy[x, y, ColorChannel.Red]), 2));
            return (float)result;

        }
    }
}
