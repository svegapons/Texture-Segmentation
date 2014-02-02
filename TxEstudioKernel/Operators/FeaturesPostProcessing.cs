using System;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel.Operators.Matrix_Operators
{

   

    /// <summary>
    /// Aplica a cada elemento de la matriz un operador no lineal sigmoidal para resaltar
    /// las carateristicas de textura
    /// </summary>
    public class NonLinearOperator : TxMatrixOperator
    {

        public NonLinearOperator(float alpha)
        {
            this.alpha = alpha;
        }

        float alpha = 0.01f;
        [RealInRange(0f, 1f)]
        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }
        public override TxMatrix Process(TxMatrix matrix)
        {
            matrix = matrix.Scale(0f, 255f);
            float[] transform = new float[256];
            for (int i = 0; i < 256; i++)
                transform[i] = (float)Math.Tanh(alpha * i);
            unsafe
            {
                float* matrixData = (float*)((_cvMat*)matrix.InnerMatrix)->data;
                for (int i = 0; i < matrix.Height; i++)
                    for (int j = 0; j < matrix.Width; j++, matrixData++)
                        *matrixData = transform[(int)(*matrixData)];

            }
            return matrix;
        }
    }

    /// <summary>
    /// Filtro de kernel cuadrado de dimension M donde cada componente es 1/M^2.
    /// </summary>
    public class SimpleEnergyFilter : TxMatrixOperator
    {

        public SimpleEnergyFilter(int windowSize)
        {
            M = windowSize;
        }
        int M = 3;
        [IntegerInSequence(3, int.MaxValue, 2)]
        public int WindowSize
        {
            get
            {
                return M;
            }
            set
            {
                M = value;
            }
        }

        public override TxMatrix Process(TxMatrix matrix)
        {
            float value = 1.0f / (M * M);
            float[,] kernel = new float[M, M];
            for (int i = 0; i < M; i++)
                for (int j = 0; j < M; j++)
                    kernel[i, j] = value;
            return matrix.Convolve(new TxMatrix(kernel));

        }
    }

    /// <summary>
    /// Realiza un filtrado de la matriz empleando el filtro gaussiano.
    /// La desviacion estandar se calcula a partir del tamanno del kernel.
    /// </summary>
    public class GaussianEnergyFilter : TxMatrixOperator
    {
        public GaussianEnergyFilter(int windowSize)
        {
            winSize = windowSize;
        }

        int winSize = 3;
        [IntegerInSequence(3, int.MaxValue, 2)]
        public int WindowSize
        {
            get
            {
                return winSize;
            }
            set
            {
                winSize = value;
            }
        }

        private TxMatrix getGaussianKernel()
        {
            int bound = winSize / 2;
            float sum = 0f;
            float sigma = 2 * bound;
            float[,] kernel = new float[winSize, winSize];
            for (int i = -bound; i <= bound; i++)
                for (int j = -bound; j <= bound; j++)
                {
                    kernel[i + bound, j + bound] = (float)(Math.Exp(-.5 * (Math.Pow(i / sigma, 2) + Math.Pow(j / sigma, 2))) / (2 * sigma * sigma));
                    sum += kernel[i + bound, j + bound];
                }

            for (int i = 0; i < kernel.GetLength(0); i++)
                for (int j = 0; j < kernel.GetLength(1); j++)
                    kernel[i, j] /= sum;
            return new TxMatrix(kernel);
        }

        public override TxMatrix Process(TxMatrix matrix)
        {
            return matrix.Convolve(getGaussianKernel());
        }
    }
}
