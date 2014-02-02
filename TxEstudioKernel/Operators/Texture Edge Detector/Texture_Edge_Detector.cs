using System;
using System.Collections.Generic;
using System.Text;
using AForge.Neuro;
using AForge.Neuro.Learning;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators.Texture_Edge_Detector
{
    [EdgeDetector]
    [Algorithm("Texture Edge Detector", "Calculates the edges using multiresolution features and SOM.")]
    [Abbreviation("tx_edge")]
    public class Texture_Edge_Detector: TxOneBand
    {
        public override TxImage Process(TxImage input)
        {
            TxImage[] filteredImage = Filtering(input);
            TxImage SOMfeatures = CalculateFeaturesMap(filteredImage);
            GaussianFilter g = new GaussianFilter();
            g.Stdv = 5;
            g.nCols = 21;
            g.nRows = 21;
            TxImage filterdSom= g.Process(SOMfeatures);
            filterdSom.Save("som.bmp");

            PrewittEdgeDetector prewitt = new PrewittEdgeDetector();
            TxImage resultingImage = prewitt.Process(filterdSom);
            for (int i = 0; i < resultingImage.Width; i++)
            {
                for (int j = 0; j < resultingImage.Height; j++)

                    if (resultingImage[i, j] > 128)
                    {
                        resultingImage[i, j] = 255;
                    }
                    else resultingImage[i, j] = 0;
            }
            return resultingImage;

        }
        public TxImage[] Filtering(TxImage image)
        {
            TxImage[] filters_result = new TxImage[4];
            // Gabor unidimensional por primera vez por columnas
            UnidimensionalGaborByColumns gbC = new UnidimensionalGaborByColumns();
            filters_result[0] = gbC.GetDescription(image).ToImage();
            filters_result[0].Save("0.bmp");
            // Gabor unidimensional por primera vez por filas
            UnidimensionalGaborByRows gbR = new UnidimensionalGaborByRows();
            filters_result[1] = gbR.GetDescription(image).ToImage();
            filters_result[1].Save("1.bmp");


            // Gabor unidimensional por segunda vez por columnas
            filters_result[2] = gbC.GetDescription(image).ToImage();
            filters_result[2].Save("2.bmp");
           
            //// por filas despues de haberlo hecho por columnas
            //filters_result[3] = gbR.GetDescription(filters_result[0]).ToImage();
            //filters_result[3].Save("3.bmp");
            
            // Gabor unidimensional por segunda vez por filas
            filters_result[3] = gbR.GetDescription(image).ToImage();
            filters_result[3].Save("3.bmp");


            //PrewittEdgeDetector pre=new PrewittEdgeDetector();
            //filters_result[4] = pre.Process(image);
            //filters_result[5] = pre.Process(image);
            //filters_result[5].Save("4.bmp");
            //filters_result[5].Save("5.bmp");
            
            //// Gabor unidimensional por columnas despues de haberlo hecho por filas
            //filters_result[5] = gbC.GetDescription(filters_result[1]).ToImage();
            //filters_result[5].Save("5.bmp");

            // Filtro Gaussiano a las primeras componentes unidimensionales de Gabor unidimensional
            //GaussianFilter gaussian = new GaussianFilter();
            //gaussian.Stdv = 5f;
            //gaussian.nRows = 7;
            //gaussian.nCols = 7;
            
            //filters_result[4] = TxMatrix.FromImage(gaussian.Process(filters_result[0])).ToImage();
            //filters_result[5] = TxMatrix.FromImage(gaussian.Process(filters_result[1])).ToImage();
            //filters_result[4].Save("4.bmp");
            //filters_result[5].Save("5.bmp");

            //filters_result[6] = TxMatrix.FromImage(gaussian.Process(filters_result[2])).ToImage();
            //filters_result[6].Save("6.bmp");
            //filters_result[7] = TxMatrix.FromImage(gaussian.Process(filters_result[3])).ToImage();
            //filters_result[7].Save("7.bmp");

            /// esto es para lograr mandar solo 16 clases al SOM
          //TxMatrix aux;
          //  for (int i = 0; i < filters_result.Length; i++)
          //  {
          //      aux = TxMatrix.FromImage(filters_result[i]);
          //      for (int j = 0; j < aux.Width; j++)
          //      {
          //          for (int k = 0; k < aux.Height; k++)
          //          {
                       
          //             aux[j,k]= ((aux[j,k]) / 16) * 16; 
          //          }
                    
          //      }
          //      filters_result[i]=aux.ToImage();
          //  }

            #region Gabor bidimensional
            //float[] orientaciones = new float[1];
            //orientaciones[0] = 0.25f;
            //GaborEnergyTextureDescriptor2D gaborBi = new GaborEnergyTextureDescriptor2D(8, orientaciones);
            //int position = 8;
            //while (gaborBi.MoveNext())
            //{
            //    filters_result[position] = gaborBi.Current.GetDescription(image).ToImage();
            //    filters_result[position].Save(position.ToString() + ".bmp");
            //    position++;
            //}
            #endregion

            return filters_result;
        }

        #region Kohonen


        //private int networkSize = 2;
        private int iteration =5;
        private double initialLearningRate = 1, finalLearningRate = 1e-5;
        private double learningRate = 0.3;
        private int learningRadius = 3;

            public TxImage CalculateFeaturesMap(params TxImage[] descriptors)
            {
                DistanceNetwork network = new DistanceNetwork(descriptors.Length, 256);
                SOMLearning trainer = new SOMLearning(network, 16, 16);
                double fixedLearningRate = learningRate / 10;
                double driftingLearningRate = fixedLearningRate * 9;
                TxImage result = new TxImage(descriptors[0].Width,descriptors[0].Height,TxImageFormat.GrayScale);
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

    }
}
