using System;
using System.Collections.Generic;
using System.Text;
using AForge.Neuro;
using AForge.Neuro.Learning;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Kohonen Self-Organizing Map", "No Supervised image segmentation using the Kohonen Self-Organizing Map.")]
    [Abbreviation("som", "NetworkSize", "Iterations")]  
    public class KohonenSegm:TxSegmentationAlgorithm
    {
        private int networkSize = 2;

        /// <summary>
        /// Tabla LUT con los primeros números primos menores que 100.
        /// </summary>
        private static byte[] LUT_primos ={ 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };

        /// <summary>
        /// Cantidad de clases de la red
        /// </summary>
        [Parameter("Classes","The number of classes for classification")]
        [IntegerInSequence(1,15)]
        public virtual int NetworkSize
        {
            get { return networkSize; }
            set { networkSize = value; }
        }
        private int iteration = 10;

        /// <summary>
        /// Iteraciones
        /// </summary>
        [Parameter("Iterations","The amount of times that the network would process the data")]
        [IntegerInSequence(1,int.MaxValue)]
        public virtual int Iterations
        {
            get { return iteration; }
            set { iteration = value; }
        }

        //private float error = 0.01f;
        //[Parameter("Error","")]
        //public float Error
        //{
        //    get { return error; }
        //    set { error = value; }
        //}

        private double initialLearningRate = 1, finalLearningRate=1e-5;
        private double learningRate=0.3;
        private int learningRadius = 3;

        //metodo heredado de Iclasificable.
        public override double ProbError()
        {
            throw new Exception("Este segmentador no tiene implementado una funcion para la estimacion no supervisada del error de clasificacion");
        }

        public override TxSegmentationResult Segment(params TxMatrix[] descriptors)
        {
            int a = 0, b = 0;
            GetNetworkRowsCols(out a, out b);
            DistanceNetwork network = new DistanceNetwork(descriptors.Length, a*b);
            SOMLearning trainer = new SOMLearning(network,b,a);
            double fixedLearningRate = learningRate / 10;
            double driftingLearningRate = fixedLearningRate * 9;
            TxSegmentationResult result = new TxSegmentationResult(a * b, descriptors[0].Width, descriptors[0].Height);

            trainer.LearningRadius = learningRadius;
            int iter=0;
            double err=float.PositiveInfinity;
            double err0 = 0;
            int rows = descriptors[0].Height;
            int cols = descriptors[0].Width;
            do
            {
                //trainer.LearningRate = driftingLearningRate * (iteration - iter) / iteration + fixedLearningRate;
                trainer.LearningRate = initialLearningRate * Math.Pow(finalLearningRate / initialLearningRate, (float)iter / (float)iteration);
                trainer.LearningRadius = (double)learningRadius * (iteration - iter) / iteration;
                double[] input = new double[descriptors.Length];
                err0 = err;
                err = 0;
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                    {
                        for (int k = 0; k < descriptors.Length; k++)
                            input[k] = descriptors[k][i, j];
                        err += trainer.Run(input);
                        result[i, j] = network.GetWinner();
                    }
                err /= rows * cols;
            }
            while (iter++ < iteration /*&& Math.Abs(err-err0) > error && err<err0*/);
            return result;
        }
        /// <summary>
        /// Devuelve un par de valores fila y columna que se utilizarán en la construcción de la topología del SOM 
        /// </summary>
        /// <param name="rows">Filas que tendrá el SOM</param>
        /// <param name="cols">Columnas que tendrá el SOM</param>
        /// <remarks>El criterio de selección de filas y columnas se basa en la descomposición en factores primos de un número.
        /// Se descompondrá el número de clases especificado en factores primos. Para la selección de la cantidad de filas se 
        /// tomará el producto de la mitad de los elementos presentes en el conjunto de factores primos, el producto del resto
        /// de los elementos será la cantidad de columnas del SOM.
        /// Por ejemplo, si la cantidad de clases es 4, entonces la salida de este método será rows=2 y cols =2.
        /// Si la cantidad de clases es 6, entonces rows=2 y cols=3.
        /// Si la cantidad de clases es 7, entonces rows=1 y cols=7.
        /// Si se desea una nueva forma de seleccionar las filas y columnas para la contrucción del SOM, entonces se debe heredar 
        /// de esta clase y redefinir este método.
        /// </remarks>
        protected virtual void GetNetworkRowsCols(out int rows, out int cols)
        {
            LinkedList<int> descomp = DescomposicionFactorial(NetworkSize);
            GetNetworkRowsCols(descomp, out rows, out cols);
        }
        private LinkedList<int> DescomposicionFactorial(int n)
        {
            LinkedList<int> descomposicion = new LinkedList<int>();
            for (int i = 0; i < LUT_primos.Length; i++)
            {
                while (n % LUT_primos[i] == 0)
                {
                    descomposicion.AddLast(LUT_primos[i]);
                    n /= LUT_primos[i];
                }
            }
            return descomposicion;
        }
        private void GetNetworkRowsCols(LinkedList<int> descomposicionFactorial, out int a, out int b)
        {
            a = 1;
            b = 1;
            int count=0;
            int half=descomposicionFactorial.Count/2;
            foreach (int primo in descomposicionFactorial)
            {
                if (count++ < half)
                    a *= primo;
                else b *= primo;
            }
        }
    }
}
