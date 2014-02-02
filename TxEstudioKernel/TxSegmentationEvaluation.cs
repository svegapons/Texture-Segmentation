using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.SegmentationSimilarity.Matchers;

namespace TxEstudioKernel
{
    public abstract class TxSegmentationEvaluation : TxAlgorithm
    {
        public abstract double Evaluate(TxSegmentationResult segmentation, TxSegmentationAlgorithm segmenter);
    }

    public abstract class TxSupervisedSegmentationEvaluation : TxSegmentationEvaluation
    {
        protected TxSegmentationResult groundTruth;

        public TxSegmentationResult GroundTruth
        {
            get { return groundTruth; }
            set { groundTruth = value; }
        }
    }

    public class TxUnsupervisedSegmentationEvaluation : TxSegmentationEvaluation
    {


        public override double Evaluate(TxSegmentationResult segmentation, TxSegmentationAlgorithm segmenter)
        {
            try
            {
                return  1-(((TxEstudioKernel.Operators.IEvaluable)segmenter).ProbError());
            }
            catch (InvalidCastException e)
            {
                throw new Exception("Para la seleccion de caracteristicas no supervisada debe seleccionarse un segmentador que tenga la definicion de su Error ");
            }

        }
    }

    public class RegionBasedSegmentationSimilarity : TxSupervisedSegmentationEvaluation
    {
        public override double Evaluate(TxSegmentationResult segmentation, TxSegmentationAlgorithm segmenter)
        {
            return new TxEstudioKernel.SegmentationSimilarity.HungarianBasedSimilarity().GetSimilarity(segmentation, groundTruth);
        }
    }

    public class HammingDistance : TxSupervisedSegmentationEvaluation
    {
        public override double Evaluate(TxSegmentationResult segmentation, TxSegmentationAlgorithm segmenter)
        {
            int m = segmentation.Classes;
            int n = groundTruth.Classes;
            int[,] tabla = new int[m, n];
            for (int i = 0; i < segmentation.Height; i++)
                for (int j = 0; j < segmentation.Width; j++)
                    tabla[segmentation[i, j], groundTruth[i, j]]++;
            int total = 0;
            //S1=>S2
            int[] par = new int[n];
            int current = 0;
            for (int j = 0; j < n; j++)
            {
                current = tabla[0, j];
                for (int i = 1; i < m; i++)
                    if (current < tabla[i, j])
                    {
                        par[j] = i; current = tabla[i, j];
                    }
            }
            for (int j = 0; j < n; j++)
                for (int i = 0; i < m; i++)
                    if (i != par[j])
                        total += tabla[i, j];
            //S2=>S1
            par = new int[n];
            current = 0;
            for (int i = 0; i < m; i++)
            {
                current = tabla[i, 0];
                for (int j = 1; j < n; j++)
                    if (current < tabla[i, j])
                    {
                        par[i] = j; current = tabla[i, j];
                    }
            }
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    if (j != par[i])
                        total += tabla[i, j];
            return (double)total / (2 * segmentation.Height * segmentation.Width);
        }
    }

    public class ClusteringSimilarity : TxSupervisedSegmentationEvaluation
    {
        public override double Evaluate(TxSegmentationResult segmentation, TxSegmentationAlgorithm segmenter)
        {
            double[,] tabla = new double[Math.Max(segmentation.Classes, groundTruth.Classes)
                                        , Math.Max(segmentation.Classes, groundTruth.Classes)];
            for (int i = 0; i < segmentation.Height; i++)
                for (int j = 0; j < segmentation.Width; j++)
                    tabla[segmentation[i, j], groundTruth[i, j]]++;
            HungarianAlgorithm hungarian = new HungarianAlgorithm();
            int[] match = hungarian.GetMatch(tabla);
            double result = 0;
            for (int i = 0; i < match.GetLength(0); i++)
                result += tabla[i, match[i]];
            return 1 - result / (2 * segmentation.Height * segmentation.Width);
        }
    }
}
