using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.Operators
{
    public class ART2
    {
        /// <summary>
        /// Matriz de pesos
        /// </summary>
        protected float[,] wji;
        /// <summary>
        /// Parámetro de vigilancia
        /// </summary>
        protected float rho;

        private int nClasses;

        /// <summary>
        /// Número de veces que la neurona i ha sido ganadora
        /// </summary>
        protected uint[] num;

        private int winner = -1;

        public ART2(int nInput, int classes, float rho)
        {
            wji = new float[nInput, classes];
            this.rho = rho;
            for (int i = 0; i < classes; i++)
                for (int j = 0; j < nInput; j++)
                    wji[j,i] = 1.0f / (1.0f + nInput);
            num = new uint[classes];
        }
        /// <summary>
        /// Devuelve la cantidad de elementos esperados en el patrón de entrada.
        /// </summary>
        public virtual int InputCount
        {
            get { return wji.GetLength(0); }
        }
        /// <summary>
        /// Devuelve la cantidad de clases en la red
        /// </summary>
        public virtual int Classes
        {
            get { return nClasses; }
        }
        public virtual int Winner
        {
            get { return winner; }
        }
        /// <summary>
        /// Devuelve el valor del parámetro de vigilancia.
        /// </summary>
        public virtual float Rho
        {
            get { return rho; }
        }

        public virtual float Run(float[] input)
        {
            if (input.Length != InputCount)
                throw new InvalidOperationException("La cantidad de elementos del vector de entrada no coincide con la cantidad de elementos esperados");
            float dist=0;
            GetMinDist(input, out dist, out winner);
            if (dist <= rho)
            {
                UpdateWeigths(input, winner);
            }
            else
            {
                if (nClasses == wji.GetLength(1))
                {
                    float[,] wji0 = new float[wji.GetLength(0), wji.GetLength(1) + 1];
                    uint[] num0 = new uint[num.Length + 1];
                    CopyMatrix(wji, wji0);
                    for (int i = 0; i < wji0.GetLength(0); i++)
                        wji0[i, nClasses] = 1.0f / (1.0f + InputCount);
                    Array.Copy(num, num0, num.Length);
                    wji = wji0;
                    num = num0;
                }
                winner = nClasses++;
                UpdateWeigths(input, winner);
                dist = Distance(input, GetColVector(wji, winner));
            }
            return dist;
        }
        public virtual float RunEpoch(float[][] inputs)
        {
            float error = 0;
            foreach (float[] input in inputs)
                error += Run(input);
            return error;
        }
        /// <summary>
        /// Calcula la distancia entre dos vectores
        /// </summary>
        /// <param name="v">Vector v</param>
        /// <param name="w">Vector w</param>
        /// <returns>Retorna la distancia entre dos vectores</returns>
        /// <remarks>Este método calcula la distancia euclideana entre los vectores v y w.
        /// El criterio de distancia se utilizará para determinar la menor distancia entre el patrón de entrada y el centroide.
        /// Si se desea utilizar otra forma de calcular la distancia basta con heredar de esta clase y redefinir este método.
        /// </remarks>
        protected virtual float Distance(float[] v, float[] w)
        {
            float result = 0;
            for (int j = 0; j < v.Length; j++)
                result += (float)Math.Pow(v[j] - w[j], 2);
            return (float)Math.Sqrt(result);
        }
        /// <summary>
        /// Actualiza los pesos de la red
        /// </summary>
        /// <param name="input">Patrón de entrada</param>
        /// <param name="winner">Índice de la neurona ganadora</param>
        protected virtual void UpdateWeigths(float[] input, int winner)
        {
            for (int j = 0; j < wji.GetLength(0); j++)
                wji[j, winner] = (input[j] + wji[j, winner] * num[winner]) / (num[winner] + 1.0f);
            num[winner]++;
        }
        /// <summary>
        /// Devuelve el índice de la neurona ganadora y el valor de la distancia hacia el centroide.
        /// </summary>
        /// <param name="input">Vector de entrada</param>
        /// <param name="minDist">Menor distancia del vector entrada hacia los centroides</param>
        /// <param name="winner">Índice de la neurona cuya distancia es menor al vector de entrada</param>
        private void GetMinDist(float[] input, out float minDist, out int winner)
        {
            float dist = 0;
            minDist = float.PositiveInfinity;
            winner = -1;
            for (int i = 0; i < Classes; i++)
            {
                dist = Distance(input, GetColVector(wji, i));
                if (dist < minDist)
                {
                    minDist = dist;
                    winner = i;
                }
            }
        }

        private static float[] GetColVector(float[,] matrix, int col)
        {
            float[] result = new float[matrix.GetLength(0)];
            for (int j = 0; j < matrix.GetLength(0); j++)
                result[j] = matrix[j, col];
            return result;
        }
        private void CopyMatrix(float[,] src, float[,] dest)
        {
            for (int i = 0; i < src.GetLength(0); i++)
                for (int j = 0; j < src.GetLength(1); j++)
                    dest[i, j] = src[i, j];
        }

    }
}
