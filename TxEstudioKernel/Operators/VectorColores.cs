using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel;

namespace TxEstudioKernel.Operators
{
    public class VectorColores : IClasificable
    {
        /// <summary>
        /// Arreglo de valores del vector
        /// </summary>
        float[] valores;
        /// <summary>
        /// Contador que se utilizara para llevar hasta donde esta lleno el arreglo de valores
        /// </summary>
        int contador;

        /// <summary>
        /// Crea un objeto VectorColores
        /// </summary>
        /// <param name="dimension">Dimension del objeto</param>
        public VectorColores(int dimension)
        {
            valores = new float[dimension];
            contador = 0;
        }
        /// <summary>
        /// Adiciona un nuevo valor al arreglo de valores
        /// </summary>
        public void Add(float numero)
        {
            if (contador < valores.Length)
            {
                valores[contador] = numero;
                contador++;
            }
        }

        #region IClasificable Members

        public Vector Valor()
        {
            double[] vector = new double[valores.Length];
            for (int i = 0; i < valores.Length; i++)
                vector[i] = (double)valores[i];
            return new Vector(vector);
        }

        public double Distancia(Vector patron)
        {
            double result = 0;

            for (int i = 0; i < valores.Length; i++)
                result += (valores[i] - patron[i]) * (valores[i] - patron[i]);

            return Math.Sqrt(result);
        }

        #endregion
    }
}
