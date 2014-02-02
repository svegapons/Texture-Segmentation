using System;
using System.Collections.Generic;


namespace TxEstudioKernel.SegmentationSimilarity.Matchers
{
    public class Tuple
    {
        public readonly int Row;
        public readonly int Column;
        public Tuple(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
    public class HungarianAlgorithm
    {
        private void SubstractInColumn(double[,] matrix, int column, double value)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
                matrix[i, column] -= value;
        }

        private double FindMinimumInColumn(double[,] matrix, int column)
        {
            double minimum = double.MaxValue;
            for (int i = 0; i < matrix.GetLength(0); i++)
                if(matrix[i, column]< minimum) 
                    minimum = matrix[i,column];
            return minimum;
        }

        private int FindInColumn(bool[,] set, int column, int except)
        {
            for (int i = 0; i < set.GetLength(0); i++)
                if (i!= except && set[i, column])
                    return i;
            return -1;
        }

        private int FindInRow(bool[,] set, int row, int except)
        {
            for (int i = 0; i < set.GetLength(1); i++)
                if (i!= except && set[row, i])
                    return i;
            return -1;
        }

        public int[] GetMatch(double[,] costMatrix)
        {
            double[,] matrix = (double[,])costMatrix.Clone();
            bool[,] starred = new bool[matrix.GetLength(0), matrix.GetLength(1)];
            bool[,] primed = new bool[matrix.GetLength(0), matrix.GetLength(1)];
            bool[] coverCol = new bool[matrix.GetLength(1)];
            bool[] coverRow = new bool[matrix.GetLength(0)];
            int matchDimension = 0;

            for (int i = 0; i < matrix.GetLength(1); i++)
                SubstractInColumn(matrix, i, FindMinimumInColumn(matrix, i));

            bool[] starredRow = new bool[matrix.GetLength(0)];

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 0 && !starredRow[i] && !coverCol[j])
                    {
                        starredRow[i] = true;
                        coverCol[j] = true;
                        starred[i, j] = true;
                        matchDimension++;
                    }
                }

            while (matchDimension != matrix.GetLength(1))//Hasta que el emparejamiento sea maximo
            {
                FindAndPrime(matrix, coverCol, coverRow, starred, primed);
                matchDimension = 0;
                for (int i = 0; i < matrix.GetLength(1); i++)
                    if (FindInColumn(starred, i, -1) >= 0)
                    {
                        coverCol[i] = true;
                        matchDimension++;
                    }
            }

            int[] result = new int[matrix.GetLength(0)];
            for (int i = 0; i < starred.GetLength(0); i++)
                for (int j = 0; j < starred.GetLength(1); j++)
                    if (starred[i, j])
                        result[i] = j;
            return result;
        }

        private void FindAndPrime(double[,] matrix, bool[] coverCol, bool[] coverRow, bool[,] starred, bool[,] primed)
        {
            Tuple uncov = FindUncoveredZeros(matrix, coverRow, coverCol);
            if (uncov == null)
            {
                RescaleUncoverMatrix(matrix, coverRow, coverCol);
                uncov = FindUncoveredZeros(matrix, coverRow, coverCol);
            }
            while (true)
            {
                Tuple lastPrimed = new Tuple(uncov.Row, uncov.Column);
                primed[uncov.Row, uncov.Column] = true;
                int temp = FindInRow(starred, uncov.Row, uncov.Column);
                if (temp != -1)
                {
                    coverRow[uncov.Row] = true;
                    coverCol[uncov.Column] = false;
                    uncov = FindUncoveredZeros(matrix, coverRow, coverCol);
                    if (uncov == null)
                    {
                        RescaleUncoverMatrix(matrix, coverRow, coverCol);
                        uncov = FindUncoveredZeros(matrix, coverRow, coverCol);
                    }
                }
                else
                {
                    ZedSeries(matrix, coverRow, coverCol, starred, primed, lastPrimed);
                    break;
                }
            }
        }

        private void ZedSeries(double[,] matrix, bool[] coverRow, bool[] coverCol, bool[,] starred, bool[,] primed, Tuple lastPrimed)
        {
            List<Tuple> starSerie = new List<Tuple>(matrix.GetLength(0));
            List<Tuple> primeSerie = new List<Tuple>(matrix.GetLength(1));
            primeSerie.Add(lastPrimed);
            int i = FindInColumn(starred, lastPrimed.Column, lastPrimed.Row);
            int j = lastPrimed.Column;
            while (i != -1)
            {
                starSerie.Add(new Tuple(i, j));
                j = FindInRow(primed, i, j);
                primeSerie.Add(new Tuple(i, j));
                i = FindInColumn(starred, j, i);
            }
            foreach (Tuple tuple in starSerie)
                starred[tuple.Row, tuple.Column] = false;
            foreach (Tuple tuple in primeSerie)
                starred[tuple.Row, tuple.Column] = true;

            for (int r = 0; r < primed.GetLength(0); r++)
                for (int c = 0; c < primed.GetLength(1); c++)
                    primed[r, c] = false;

            for (int r = 0; r < coverRow.Length; r++)
                coverRow[r] = false;
            for (int c = 0; c < coverCol.Length; c++)
                coverCol[c] = false;
        }

        private void RescaleUncoverMatrix(double[,] matrix, bool[] coverRow, bool[] coverCol)
        {
            double min = FindUncoveredMin(matrix, coverRow, coverCol);
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (!coverRow[i] && !coverCol[j])
                        matrix[i, j] -= min;
        }

        private double FindUncoveredMin(double[,] matrix, bool[] coverRow, bool[] coverCol)
        {
            double min = double.MaxValue;
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (!coverRow[i] && !coverCol[j] && matrix[i,j]<min)
                        min = matrix[i, j];
            return min;
        }

        private Tuple FindUncoveredZeros(double[,] matrix, bool[] coverRow, bool[] coverCol)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (!coverRow[i] && !coverCol[j] && matrix[i, j] == 0)
                        return new Tuple(i, j);
            return null;
        }
    }
}
