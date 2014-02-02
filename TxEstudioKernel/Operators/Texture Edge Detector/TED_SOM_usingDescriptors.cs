using System;
using System.Collections.Generic;
using System.Text;
using AForge.Neuro;
using AForge.Neuro.Learning;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators.Texture_Edge_Detector
{
    [EdgeDetector]
    [Algorithm("somTED method", "Calculates the edges using multiresolution features and SOM.")]
    [Abbreviation("tx_edge_descriptors_SOM")]

    public class TED_SOM_usingDescriptors : TxTextureEdgeDetector
    {
        public override TxSegmentationResult Segment(params TxMatrix[] descriptors)
        {
            TxImage[] images = new TxImage[descriptors.Length];
            for (int i = 0; i < descriptors.Length; i++)
            {
                images[i] = descriptors[i].ToImage();
            }
            TxImage SOMfeatures = CalculateFeaturesMap(images);
            GaussianFilter g = new GaussianFilter();
            g.Stdv = 5;
            g.nCols = 7;
            g.nRows = 7;
            TxImage filterdSom = g.Process(SOMfeatures);
            filterdSom.Save("somdescriptors.bmp");

            AC_LSO_ED detector = new AC_LSO_ED();

            return detector.Segment(TxMatrix.FromImage(filterdSom));
        }
        #region Kohonen

        private int iteration = 5;
        private double initialLearningRate = 1, finalLearningRate = 1e-5;
        private double learningRate = 0.3;
        private int learningRadius = 3;

        public TxImage CalculateFeaturesMap(params TxImage[] descriptors)
        {
            DistanceNetwork network = new DistanceNetwork(descriptors.Length, 256);
            SOMLearning trainer = new SOMLearning(network, 16, 16);
            double fixedLearningRate = learningRate / 10;
            double driftingLearningRate = fixedLearningRate * 9;
            TxImage result = new TxImage(descriptors[0].Width, descriptors[0].Height, TxImageFormat.GrayScale);
            trainer.LearningRadius = learningRadius;
            int iter = 0;
            double err = float.PositiveInfinity;
            double err0 = 0;
            int rows = descriptors[0].Height;
            int cols = descriptors[0].Width;
            do
            {
                trainer.LearningRate = initialLearningRate * Math.Pow(finalLearningRate / initialLearningRate, (float)iter / (float)iteration);
                trainer.LearningRadius = (double)learningRadius * (iteration - iter) / iteration;
                double[] input = new double[descriptors.Length];
                err0 = err;
                err = 0;
                for (int y = 0; y < rows; y++)
                    for (int x = 0; x < cols; x++)
                    {
                        for (int k = 0; k < descriptors.Length; k++)
                            input[k] = descriptors[k][x, y];
                        err += trainer.Run(input);
                        result[x, y] = (byte)network.GetWinner();
                    }
                err /= rows * cols;
            }
            while (iter++ < iteration);

            return result;
        }

        #endregion

        public override double ProbError()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
