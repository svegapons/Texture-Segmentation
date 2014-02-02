using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel;
using TxEstudioKernel.OpenCV;
using TxEstudioKernel.SegmentationSimilarity.Matchers;

namespace TxEstudioKernel.SegmentationSimilarity
{

    /// <summary>
    /// Base class for segmentation similarity measure
    /// </summary>
    public abstract class TxSegmentationSimilarityMeasure : TxAlgorithm
    {
        /// <summary>
        /// When override in a derived class claculates the similarity between towo segmentation results.
        /// </summary>
        /// <param name="one">One segmentation result.</param>
        /// <param name="other">Other segmentation result.</param>
        /// <returns>A value between 0 and 1 that corresponds to similarty. 
        /// The greather the value the similar the two segmentations are.</returns>
        public abstract double GetSimilarity(TxSegmentationResult one, TxSegmentationResult other);

        #region Getting data

        protected double[,] GetRatio(int dimension, int[,] coincidences)
        {
            int[] oneSizes = new int[dimension];
            int[] otherSizes = new int[dimension];
            for (int i = 0; i < dimension; i++)
                for (int j = 0; j < dimension; j++)
                {
                    oneSizes[i] += coincidences[i, j];
                    otherSizes[j] += coincidences[i, j];
                }
            double[,] ratio = new double[dimension, dimension];
            for (int i = 0; i < dimension; i++)
                for (int j = 0; j < dimension; j++)
                    ratio[i, j] = ((double)coincidences[i, j]) / ((double)(oneSizes[i] + otherSizes[j] - coincidences[i, j]));
            return ratio;
        }

        protected int[,] GetCoincidences(TxSegmentationResult one, TxSegmentationResult other, int dimension)
        {
            int[,] coincidences = new int[dimension, dimension];
            for (int i = 0; i < one.Height; i++)
                for (int j = 0; j < one.Width; j++)
                    coincidences[one[i, j], other[i, j]]++;
            return coincidences;
        }

        #endregion

        #region Calculating Stadistics

        protected void GetStadistics(TxSegmentationResult one, TxSegmentationResult other, int dimension, int[,] coincidences, double[,] ratio)
        {
            //Obteniendo estadisticas
            int total = other.Height * other.Width;
            coincidenceMeasure = 0;
            coincidenceByClass = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                coincidenceByClass[i] =  ratio[i, match[i]] ;
                coincidenceMeasure += coincidences[i, match[i]] * coincidenceByClass[i]/ total;
            }
            int coincidencePixels = BuildCoincidenceMap(one, other);
            totalCoincidencePercent = 100 * (((double)coincidencePixels) / total);
        }

        protected void GetStadistics_TQM(TxSegmentationResult one, TxSegmentationResult other, int dimension, int[,] coincidences, double[,] ratio)
        {
            //Obteniendo estadisticas
            int total = other.Height * other.Width;
            coincidenceMeasure = 0;
            coincidenceByClass = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                coincidenceByClass[i] = ratio[i, match[i]];
                coincidenceMeasure += other.ClassPixelCount(i) * coincidenceByClass[i] / total;
            }
            int coincidencePixels = BuildCoincidenceMap(one, other);
            totalCoincidencePercent = 100 * (((double)coincidencePixels) / total);
        }

        protected int BuildCoincidenceMap(TxSegmentationResult one, TxSegmentationResult other)
        {
            map = new TxImage(one.Width, one.Height, TxImageFormat.GrayScale);
            int coincidencePixels = 0;
            unsafe
            {
                _IplImage* innerImage = (_IplImage*)map.InnerImage;
                byte* current = (byte*)innerImage->imageData;
                int offset = innerImage->widthStep - innerImage->width;

                for (int i = 0; i < map.Height; i++, current += offset)
                    for (int j = 0; j < map.Width; j++, current++)
                        if (other[i, j] == match[one[i, j]])
                        {
                            *current = 255;
                            coincidencePixels++;
                        }
            }
            return coincidencePixels;
        }

        #endregion

        #region Stadistics

        protected int[] match;

        public int[] LastMatch
        {
            get { return match; }
        }

        protected double[] coincidenceByClass;

        public double GetLastCoincidenceOnClass(int index)
        {
            return coincidenceByClass[index];
        }

        protected double coincidenceMeasure;

        public double LastCoincidenceMeasure
        {
            get
            {
                return coincidenceMeasure;
            }
        }
        protected double totalCoincidencePercent;

        public double LastTotalCoincidencePercent
        {
            get
            {
                return totalCoincidencePercent;
            }
        }

        protected TxImage map;

        public TxImage LastCoincidenceMap
        {
            get
            {
                return map;
            }
        }
        #endregion

    }

    /// <summary>
    /// Segmentation similarity that uses the hungarian algorithm for class matching.
    /// </summary>
    public class HungarianBasedSimilarity:TxSegmentationSimilarityMeasure
    {
        public override double GetSimilarity(TxSegmentationResult one, TxSegmentationResult other)
        {
            if (one.Classes > other.Classes)
            //Que el de menor cantidad de clases quede en one
            {
                TxSegmentationResult aux = one;
                one = other;
                other = aux;
            }

            int dimension = other.Classes;
            int[,] coincidences = GetCoincidences(one, other, dimension);
            double[,] ratio = GetRatio(dimension, coincidences);

            //Construyendo la matriz para el hungaro
            double maxRatio = double.MinValue;
            for (int i = 0; i < dimension; i++)
                for (int j = 0; j < dimension; j++)
                    maxRatio = Math.Max(maxRatio, ratio[i, j]);

            double[,] matrix = new double[dimension, dimension];
            for (int i = 0; i < dimension; i++)
                for (int j = 0; j < dimension; j++)
                    matrix[i, j] = maxRatio - ratio[i, j];

            match = new HungarianAlgorithm().GetMatch(matrix);

            GetStadistics(one, other, dimension, coincidences, ratio);
            return coincidenceMeasure;
        }
    }

    /// <summary>
    /// Segmentation similarity that uses the hungarian algorithm for class matching.
    /// </summary>
    public class TQM : TxSegmentationSimilarityMeasure
    {
        public override double GetSimilarity(TxSegmentationResult one, TxSegmentationResult other)
        {
            TxSegmentationResult groundTruth = other;
            if (one.Classes > other.Classes)
            //Que el de menor cantidad de clases quede en one
            {
                TxSegmentationResult aux = one;
                one = other;
                other = aux;
            }

            int dimension = other.Classes;
            int[,] coincidences = GetCoincidences(one, other, dimension);
            double[,] ratio = GetRatio(dimension, coincidences);

            //Construyendo la matriz para el hungaro
            double maxRatio = double.MinValue;
            for (int i = 0; i < dimension; i++)
                for (int j = 0; j < dimension; j++)
                    maxRatio = Math.Max(maxRatio, ratio[i, j]);

            double[,] matrix = new double[dimension, dimension];
            for (int i = 0; i < dimension; i++)
                for (int j = 0; j < dimension; j++)
                    matrix[i, j] = maxRatio - ratio[i, j];

            match = new HungarianAlgorithm().GetMatch(matrix);

            GetStadistics_TQM(one, other, dimension, coincidences, ratio);
            return coincidenceMeasure;
        }
    }


    public class StableMarriageBaseSimilarity:TxSegmentationSimilarityMeasure
    {
        public override double GetSimilarity(TxSegmentationResult one, TxSegmentationResult other)
        {
            if (one.Classes > other.Classes)
            //Que el de menor cantidad de clases quede en one
            {
                TxSegmentationResult aux = one;
                one = other;
                other = aux;
            }

            int dimension = other.Classes;
            int[,] coincidences = GetCoincidences(one, other, dimension);
            double[,] ratio = GetRatio(dimension, coincidences);

            match = new StableMarriageAlgorithm().GetMatch(coincidences);

            GetStadistics(one, other, dimension, coincidences, ratio);
            return coincidenceMeasure;

        }
    }
}
