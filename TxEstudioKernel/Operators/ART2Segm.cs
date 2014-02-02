using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("ART2 Neural Network", "No Supervised image segmentation using the ART2 Neural Network.")]
    [Abbreviation("art2","Rho","Iteration","Error")]    
    public class ART2Segm:TxSegmentationAlgorithm
    {
        private float rho = 0.5f;

        private int iter = 10;


        private float error = 0.01f;


        [Parameter("Threshold","A value between 0 and 1. A small value will result in finer discrimination between classes than will a greater value. This value can be considered as the radius of the centroid.")]
        [RealInRange(0f,1f)]
        public float Rho
        {
            get { return rho; }
            set { rho = value; }
        }
        [Parameter("Error","An average error between the input set and the centroids")]
        [RealInRange(0,float.MaxValue)]
        public float Error
        {
            get { return error; }
            set { error = value; }
        }
        [Parameter("Iterations","The amount of times that the network would process the data")]
        [IntegerInSequence(1,short.MaxValue)]
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

        private TxMatrix PromediaDescriptores(TxMatrix[] descriptors)
        {
            int rows=descriptors[0].Height;
            int cols=descriptors[0].Width;
            TxMatrix result = new TxMatrix(rows, cols);
            int nDescriptors=descriptors.Length;
            float sum=0;
            for(int i=0;i<rows;i++)
                for (int j = 0; j < cols; j++)
                {
                    sum = 0;
                    for (int k = 0; k < nDescriptors; k++)
                        sum += descriptors[k][i, j];
                    result[i, j] = sum / nDescriptors;
                }
            return result;
        }
        private float[] BuildInput(int i, int j, TxMatrix image)
        {
            float[] textureUnit=new float[9];
            for (int m = -1, k = 0; m <= 1; m++)
                for (int n = -1; n <= 1; n++, k++)
                    textureUnit[k] = image[i + m, j + n];
            return textureUnit;
        }


        public override TxSegmentationResult Segment(params TxMatrix[] descriptors)
        {
            float[] I = new float[descriptors.Length];
            ART2 network = new ART2(descriptors.Length, 15, Rho);
            int[,] classes = new int[descriptors[0].Height, descriptors[0].Width];
            for (int k = 0; k < descriptors.Length; k++)
                Normalize(descriptors[k]);

            float[,] wji=null, wji0=null, tij0=null, tij = null;
            float err = 0;
            int iteration=0;
            do
            {
                err = 0;
                for (int i = 0; i < descriptors[0].Height; i++)
                    for (int j = 0; j < descriptors[0].Width; j++)
                    {
                        for (int k = 0; k < descriptors.Length; k++)
                            I[k] = descriptors[k][i, j];
                        err+=network.Run(I);
                        classes[i, j] = network.Winner;
                    }
            } while (err/(descriptors[0].Height*descriptors[0].Width) > error && iteration++ < Iteration);

            bool[] bclasses = new bool[network.Classes];
            int[] LUT_classes = new int[network.Classes];
            for (int i = 0; i < classes.GetLength(0); i++)
                for (int j = 0; j < classes.GetLength(1); j++)
                    bclasses[classes[i,j]]=true;
            int nclasses = 0;
            for (int j = 0; j < bclasses.Length; j++)
                LUT_classes[j] = bclasses[j] ? nclasses++ : -1;
            TxSegmentationResult result = new TxSegmentationResult(nclasses,classes.GetLength(1), classes.GetLength(0));
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

        protected float[,] RandomSamples(TxMatrix[] descriptors,int percent)
        {
            float[,] samples = new float[descriptors[0].Height * descriptors[0].Width * percent / 100, descriptors.Length];
            int i = 0, j = 0;
            Random rand1 = new Random(unchecked((int)DateTime.Now.Ticks));
            Random rand2 = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int k = 0; k < samples.GetLength(0); k++)
            {
                i = rand1.Next(0, descriptors[0].Height - 1);
                j = rand2.Next(0, descriptors[0].Width - 1);
                for (int m = 0; m < descriptors.Length; m++)
                    samples[k, m] = descriptors[m][i, j];
            }
            return samples;
        }
        private float DiffAbsMedia(float[,] matrix1, float[,] matrix2)
        {
            if (matrix1.GetLength(0) != matrix2.GetLength(0) && matrix1.GetLength(1) != matrix2.GetLength(1))
                throw new InvalidOperationException("Las dimensiones de las matrices no son iguales");
            float error = 0;
            int rows = matrix1.GetLength(0);
            int cols = matrix1.GetLength(1);
            int count=matrix1.Length;
            for(int i=0;i<rows;i++)
                for (int j = 0; j < cols; j++)
                {
                    error += Math.Abs(matrix1[i, j] - matrix2[i, j]);
                    
                }
            return error / count;
        }
    }
}
