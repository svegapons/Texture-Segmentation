using System;
using TxEstudioKernel.SegmentationSimilarity.DataStructures;

namespace TxEstudioKernel.SegmentationSimilarity.Matchers
{
    public class StableMarriageAlgorithm
    {
        public int[] GetMatch(int[,] preferences)
        {
            //TODO: Verificar que las dimensiones del arreglo son iguales
            int dimension = preferences.GetLength(0);
            //Building data structures
            int[] next = new int[dimension];
            int[] fiancee = new int[dimension];
            for (int i = 0; i < dimension; i++)
                fiancee[i] = -1;

            Heap heap = new Heap(dimension);
            int[,] prefer = new int[dimension, dimension];
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                    heap.Insert(new HeapNode(j, preferences[i, j]));

                for (int j = 0; j < dimension; j++)
                    prefer[i, j] = heap.ExtractMin().Index;
            }

            int[,] rank = new int[dimension, dimension];
            for (int j = 0; j < dimension; j++)
            {
                for (int i = 0; i < dimension; i++)
                    heap.Insert(new HeapNode(i, preferences[i, j]));

                for (int i = 0; i < dimension; i++)
                    rank[j, heap.ExtractMin().Index] = i;
            }

            for (int m = 0; m < dimension; m++)
            {
                int s = m;
                do
                {
                    int w = prefer[s, next[s]];
                    next[s]++;
                    if (fiancee[w] == -1)
                    {
                        fiancee[w] = s;
                        break;
                    }
                    if (rank[w, s] < rank[w, fiancee[w]])
                    {
                        int t = fiancee[w];
                        fiancee[w] = s;
                        s = t;
                    }
                } while (s > 0);
            }
            return fiancee;
        }
    }
}
