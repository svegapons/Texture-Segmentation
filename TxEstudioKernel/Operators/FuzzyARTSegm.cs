using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;
 

namespace TxEstudioKernel.Operators
{
    [Algorithm("Fuzzy ART2 Neural Network", "No Supervised image segmentation using the  Fuzzy ART Neural Network.")]
    [Abbreviation("fart2", "Rho", "Alpha", "Iteration", "Error")]    
    public class FuzzyARTSegm:TxSegmentationAlgorithm
    {
        private float rho = 0.5f;

        private int iter = 10;

        private float error = 0.1f;

        double initialLearningRate = 1, finalLearningRate = 10E-5;

        private float alpha=0.35f;

        [Parameter("Threshold","A value between 0 and 1. A great value will result in finer discrimination between classes than will a small value.")]
        [RealInRange(0f,1f)]
        public float Rho
        {
            get { return rho; }
            set { rho = value; }
        }
        [Parameter("Choise Parameter","A value greater than 0.")]
        [RealInRange(0,float.PositiveInfinity)]
        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }
        [Parameter("Error","An average error between the input set and the centroids")]
        [RealInRange(0,float.MaxValue)]
        public float Error
        {
            get { return error; }
            set { error = value; }
        }
        [Parameter("Iterations", "The amount of times that the network would process the data")]
        [IntegerInSequence(1, short.MaxValue)]
        public int Iteration
        {
            get { return iter; }
            set { iter = value; }
        }

        //metodo heredado de Iclasificable.
        public override double ProbError()
        {
            throw new Exception("Este segmentador no tiene implementado una funcion para la estimacion no supervisada del error de clasificacion");
        }

        public override TxSegmentationResult Segment(params TxMatrix[] descriptors)
        {
            double[] I = new double[descriptors.Length];
            FuzzyART network = new FuzzyART(descriptors.Length, Rho, Alpha);

            int[,] classes = new int[descriptors[0].Height, descriptors[0].Width];
            for (int k = 0; k < descriptors.Length; k++)
                Normalize(descriptors[k]);

            double err = 0;
            int iteration = 0;
            do
            {
                err = 0;
                network.LearningRate = initialLearningRate * Math.Pow(finalLearningRate / initialLearningRate, (float)iter / (float)iteration);
                for (int i = 0; i < descriptors[0].Height; i++)
                    for (int j = 0; j < descriptors[0].Width; j++)
                    {
                        for (int k = 0; k < descriptors.Length; k++)
                            I[k] = descriptors[k][i, j];
                        err += network.Run(I);
                        classes[i, j] = network.Winner;
                    }
            } while (err / (descriptors[0].Height * descriptors[0].Width) > error && iteration++ < Iteration);

            bool[] bclasses = new bool[network.Classes];
            int[] LUT_classes = new int[network.Classes];
            for (int i = 0; i < classes.GetLength(0); i++)
                for (int j = 0; j < classes.GetLength(1); j++)
                    bclasses[classes[i, j]] = true;
            int nclasses = 0;
            for (int j = 0; j < bclasses.Length; j++)
                LUT_classes[j] = bclasses[j] ? nclasses++ : -1;
            TxSegmentationResult result = new TxSegmentationResult(nclasses, classes.GetLength(1), classes.GetLength(0));
            for (int i = 0; i < classes.GetLength(0); i++)
                for (int j = 0; j < classes.GetLength(1); j++)
                    result[i, j] = LUT_classes[classes[i, j]];
            return result;
        }
        private void Normalize(TxMatrix matrix)
        {
            for (int i = 0; i < matrix.Height; i++)
                for (int j = 0; j < matrix.Width; j++)
                {
                    if (float.IsNaN(matrix[i, j])) matrix[i, j] = 255;
                    matrix[i, j] = Math.Max(0.0f, matrix[i, j]);
                    matrix[i, j] = Math.Min(255.0f, matrix[i, j]);
                    matrix[i, j] /= 255.0f;
                }
        }


    }
}
