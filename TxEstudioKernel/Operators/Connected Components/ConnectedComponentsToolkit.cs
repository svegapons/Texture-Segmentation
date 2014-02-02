using System;
using System.Collections.Generic;

using TxEstudioKernel.OpenCV;

using TxEstudioKernel.Operators.ConnectedComponents.DataStructures;

using System.Drawing;

namespace TxEstudioKernel.Operators.ConnectedComponents
{
    /// <summary>
    /// Basic operations for connected components operators
    /// </summary>
    static class ConnectedComponentsToolkit
    {
        #region Getting Connected Components

        public static SetNode<int>[,] GetGrayScaleConnectedComponents(TxImage image, int threshold, out DisjointSets<int> dSets)
        {
            TxImage gray = image;
            if(gray.ImageFormat != TxImageFormat.GrayScale)
                gray = gray.ToGrayScale();

            SetNode<int>[,] nodes = new SetNode<int>[gray.Height, gray.Width];

            dSets = new DisjointSets<int>();

            unsafe
            {
                _IplImage* innerImage = (_IplImage*)gray.InnerImage;
                byte* current = (byte*)innerImage->imageData;
                int offset = innerImage->widthStep - innerImage->width;

                //First row
                nodes[0, 0] = dSets.CreateSet(*current);
                byte* prior = current;
                current++;
                for (int i = 1; i < gray.Width; i++, current++, prior++)
                {
                    nodes[0, i] = dSets.CreateSet(*current);
                    if (Math.Abs(*prior - *current) <= threshold)
                        dSets.Merge(nodes[0, i - 1], nodes[0, i]);
                }

                //The rest
                current += offset;
                prior = (byte*)innerImage->imageData;
                byte* priorSameLine = current;
                for (int i = 1; i < gray.Height; i++, current += offset, prior += offset)
                {
                    nodes[i, 0] = dSets.CreateSet(*current);
                    if (Math.Abs(*prior - *current) <= threshold)
                        dSets.Merge(nodes[i - 1, 0], nodes[i, 0]);
                    priorSameLine = current;
                    current++;
                    prior++;
                    for (int j = 1; j < gray.Width; j++, current++, prior++, priorSameLine++)
                    {
                        nodes[i, j] = dSets.CreateSet(*current);
                        if (Math.Abs(*prior - *current) <= threshold)
                            dSets.Merge(nodes[i - 1, j], nodes[i, j]);
                        if (Math.Abs(*priorSameLine - *current) <= threshold)
                            dSets.Merge(nodes[i, j-1], nodes[i, j]);
                    }
                }
            }
            return nodes;
        }
        public static SetNode<int>[,] GetConnectedComponents(TxImage image, double threshold, out DisjointSets<int> dSets)
        {
            SetNode<int>[,] nodes = new SetNode<int>[image.Height, image.Width];

            dSets = new DisjointSets<int>();
            
            nodes[0, 0] = dSets.CreateSet(image.GetColor(0, 0).ToArgb());

            System.Drawing.Color currentColor;
            for (int i = 1; i < image.Width; i++)
            {
                currentColor = image.GetColor(i, 0);
                nodes[0, i] = dSets.CreateSet(currentColor.ToArgb());
                
                if (ColorDistance(image.GetColor(i - 1, 0), currentColor) <= threshold)
                    dSets.Merge(nodes[0, i], nodes[0, i - 1]);
            }

            for (int i = 1; i < image.Height; i++)
            {
                currentColor = image.GetColor(0, i);
                nodes[i, 0] = dSets.CreateSet(currentColor.ToArgb());
                if (ColorDistance(image.GetColor(0, i - 1), currentColor) <= threshold)
                    dSets.Merge(nodes[i, 0], nodes[i - 1, 0]);
            }

            for (int i = 1; i < image.Height; i++)
            {
                for (int j = 1; j < image.Width; j++)
                {
                    currentColor = image.GetColor(j, i);
                    nodes[i, j] = dSets.CreateSet(currentColor.ToArgb());
                    if (ColorDistance(currentColor, image.GetColor(j, i - 1)) <= threshold)
                        dSets.Merge(nodes[i, j], nodes[i - 1, j]);
                    if (ColorDistance(currentColor, image.GetColor(j - 1, i)) <= threshold)
                        dSets.Merge(nodes[i, j], nodes[i, j - 1]);
                }
            }
            return nodes;
        }
        public static SetNode<int>[,] GetConnectedComponents(TxSegmentationResult segmentation, out DisjointSets<int> dSets)
        {
            SetNode<int>[,] nodes = new SetNode<int>[segmentation.Height, segmentation.Width];

            dSets = new DisjointSets<int>();

            nodes[0, 0] = dSets.CreateSet(segmentation[0, 0]);

            for (int i = 1; i < segmentation.Width; i++)
            {
                nodes[0, i] = dSets.CreateSet(segmentation[0, i]);
                if (segmentation[0, i - 1] == segmentation[0, i])
                    dSets.Merge(nodes[0, i], nodes[0, i - 1]);
            }

            for (int i = 1; i < segmentation.Height; i++)
            {
                nodes[i, 0] = dSets.CreateSet(segmentation[i, 0]);
                if (segmentation[i - 1, 0] == segmentation[i, 0])
                    dSets.Merge(nodes[i, 0], nodes[i - 1, 0]);
            }

            for (int i = 1; i < segmentation.Height; i++)
            {
                for (int j = 1; j < segmentation.Width; j++)
                {
                    nodes[i, j] = dSets.CreateSet(segmentation[i, j]);
                    if (segmentation[i, j] == segmentation[i - 1, j])
                        dSets.Merge(nodes[i, j], nodes[i - 1, j]);
                    if (segmentation[i, j] == segmentation[i, j - 1])
                        dSets.Merge(nodes[i, j], nodes[i, j - 1]);
                }
            }
            return nodes;
        }
        #region Para Sandro
        public static SetNode<int>[,] GetConnectedComponents(int[,] matrix, out DisjointSets<int> dSets)
        {
            SetNode<int>[,] nodes = new SetNode<int>[matrix.GetLength(0), matrix.GetLength(1)];

            dSets = new DisjointSets<int>();

            nodes[0, 0] = dSets.CreateSet(matrix[0, 0]);

            for (int i = 1; i < matrix.GetLength(1); i++)
            {
                nodes[0, i] = dSets.CreateSet(matrix[0, i]);
                if (matrix[0, i - 1] == matrix[0, i])
                    dSets.Merge(nodes[0, i], nodes[0, i - 1]);
            }

            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                nodes[i, 0] = dSets.CreateSet(matrix[i, 0]);
                if (matrix[i - 1, 0] == matrix[i, 0])
                    dSets.Merge(nodes[i, 0], nodes[i - 1, 0]);
            }

            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                for (int j = 1; j < matrix.GetLength(1); j++)
                {
                    nodes[i, j] = dSets.CreateSet(matrix[i, j]);
                    if (matrix[i, j] == matrix[i - 1, j])
                        dSets.Merge(nodes[i, j], nodes[i - 1, j]);
                    if (matrix[i, j] == matrix[i, j - 1])
                        dSets.Merge(nodes[i, j], nodes[i, j - 1]);
                }
            }
            return nodes;
        }

        internal static void SymplifyArray(int[,] matrix, int minSize)
        {
            DisjointSets<int> dSets = null;
            SetNode<int>[,] nodes = GetConnectedComponents(matrix, out dSets);
            Simplify(minSize, nodes, dSets);
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    matrix[i, j] = dSets.SetOf(nodes[i, j]).Tag;
        }

        #endregion
        private static double ColorDistance(Color one, Color other)
        {
            double result = Math.Pow(one.R - other.R, 2);
            result += Math.Pow(one.G - other.G, 2);
            result += Math.Pow(one.B - other.B, 2);
            return Math.Sqrt(result);
        }

        #endregion

        #region Assigning indexes

        public static Dictionary<SetNode<int>, int> SetIndexes(SetNode<int>[,] nodes, DisjointSets<int> dSets)
        {
            Dictionary<SetNode<int>, int> dictionary = new Dictionary<SetNode<int>, int>(dSets.SetsCount);
            int index = 0;
            SetNode<int> current;
            for (int i = 0; i < nodes.GetLength(0); i++)
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    current = dSets.SetOf(nodes[i, j]);
                    if (!dictionary.ContainsKey(current))
                    {
                        dictionary.Add(current, index);
                        index++;
                    }
                }
            return dictionary;
        }
        public static void SetIndexes(SetNode<int>[,] nodes, DisjointSets<int> dSets, out Dictionary<SetNode<int>, int> indexes, out SetNode<int>[] sets)
        {
            indexes = new Dictionary<SetNode<int>, int>(dSets.SetsCount);
            sets = new SetNode<int>[dSets.SetsCount];
            int index = 0;
            SetNode<int> current;
            for (int i = 0; i < nodes.GetLength(0); i++)
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    current = dSets.SetOf(nodes[i, j]);
                    if (!indexes.ContainsKey(current))
                    {
                        indexes.Add(current, index);
                        sets[index] = current;
                        index++;
                    }
                }
        }

        #endregion

        #region Simplifiying 


        #region By Size

        private static void SimplifyOnArray(int minRank, SetNode<int>[,] nodes, DisjointSets<int> dSets)
        {
            SetNode<int>[] sets;
            Dictionary<SetNode<int>, int> indexes;
            SetIndexes(nodes, dSets, out indexes, out sets);
            int[,] adyacentPairs = GetAdyacentPairs(nodes, indexes, dSets);
            //Obteniendo los menores que el tamanno especificado
            LinkedList<SetNode<int>> smallSets = new LinkedList<SetNode<int>>();
            for (int i = 0; i < sets.Length; i++)
                if (sets[i].Rank < minRank)
                    smallSets.AddLast(sets[i]);
            while (smallSets.Count > 0)
            {
                LinkedListNode<SetNode<int>> smallest = smallSets.First;
                LinkedListNode<SetNode<int>> current = smallest;
                while (current != null)
                {
                    if (current.Value.Rank < smallest.Value.Rank)
                        smallest = current;
                    current = current.Next;
                }
                if (smallest.Value.Rank >= minRank)
                    break;
                MergeWithCandidate(adyacentPairs, smallest.Value, indexes, sets, dSets);
                smallSets.Remove(smallest);
            }
        }
        private static void SimplifyOnMatrix(int minRank, SetNode<int>[,] nodes, DisjointSets<int> dSets)
        {
            SetNode<int>[] sets;
            Dictionary<SetNode<int>, int> indexes;
            SetIndexes(nodes, dSets, out indexes, out sets);
            SpecialSparsedMatrix adyacentPairs = GetAdyacentPairsMatrix(nodes, indexes, dSets);
            //Obteniendo los menores que el tamanno especificado
            LinkedList<SetNode<int>> smallSets = new LinkedList<SetNode<int>>();
            for (int i = 0; i < sets.Length; i++)
                if (sets[i].Rank < minRank)
                    smallSets.AddLast(sets[i]);
            while (smallSets.Count > 0)
            {

                LinkedListNode<SetNode<int>> smallest = smallSets.First;
                LinkedListNode<SetNode<int>> current = smallest;
                while (current != null)
                {
                    if (current.Value.Rank < smallest.Value.Rank)
                        smallest = current;
                    current = current.Next;
                }
                if (smallest.Value.Rank >= minRank)
                    break;
                MergeWithCandidate(adyacentPairs, smallest.Value, indexes, sets, dSets);
                smallSets.Remove(smallest);
            }
        }
        public static void Simplify(int minSize, SetNode<int>[,] nodes, DisjointSets<int> dSets)
        {
            if (dSets.SetsCount > 50)
                SimplifyOnMatrix(minSize, nodes, dSets);
            else
                SimplifyOnArray(minSize, nodes, dSets);
        }

        #endregion

        #region By Count

        public static void SimplifyTo(int maxCCCount, SetNode<int>[,] nodes, DisjointSets<int> dSets)
        {
            if (dSets.SetsCount > 50)
                SimplifyToOnMatrix(maxCCCount, nodes, dSets);
            else
                SimplifyToOnArray(maxCCCount, nodes, dSets);
        }
        private static void SimplifyToOnArray(int maxCCCount, SetNode<int>[,] nodes, DisjointSets<int> dSets)
        {
            SetNode<int>[] sets;
            Dictionary<SetNode<int>, int> indexes;
            SetIndexes(nodes, dSets, out indexes, out sets);
            int[,] adyacentPairs = GetAdyacentPairs(nodes, indexes, dSets);
            LinkedList<SetNode<int>> allSets = new LinkedList<SetNode<int>>();
            for (int i = 0; i < sets.Length; i++)
                allSets.AddLast(sets[i]);
            while (allSets.Count > 0 && dSets.SetsCount > maxCCCount)
            {
                LinkedListNode<SetNode<int>> smallest = allSets.First;
                LinkedListNode<SetNode<int>> current = smallest;
                while (current != null)
                {
                    if (current.Value.Rank < smallest.Value.Rank)
                        smallest = current;
                    current = current.Next;
                }
                MergeWithCandidate(adyacentPairs, smallest.Value, indexes, sets, dSets);
                allSets.Remove(smallest);
            }
        }
        private static void SimplifyToOnMatrix(int maxCCCount, SetNode<int>[,] nodes, DisjointSets<int> dSets)
        {
            SetNode<int>[] sets;
            Dictionary<SetNode<int>, int> indexes;
            SetIndexes(nodes, dSets, out indexes, out sets);
            SpecialSparsedMatrix adyacentPairs = GetAdyacentPairsMatrix(nodes, indexes, dSets);
            LinkedList<SetNode<int>> allSets = new LinkedList<SetNode<int>>();
            for (int i = 0; i < sets.Length; i++)
                allSets.AddLast(sets[i]);
            while (allSets.Count > 0 && dSets.SetsCount > maxCCCount)
            {
                LinkedListNode<SetNode<int>> smallest = allSets.First;
                LinkedListNode<SetNode<int>> current = smallest;
                while (current != null)
                {
                    if (current.Value.Rank < smallest.Value.Rank)
                        smallest = current;
                    current = current.Next;
                }
                MergeWithCandidate(adyacentPairs, smallest.Value, indexes, sets, dSets);
                allSets.Remove(smallest);
            }
        }

        #endregion

        
        private static SetNode<int> MergeWithCandidate(int[,] adyacentPairs, SetNode<int> current, Dictionary<SetNode<int>, int> setsIndex, SetNode<int>[] sets, DisjointSets<int> dSets)
        {
            int currentIndex = setsIndex[current];
            int candidateIndex = -1;
            float maxRatio = float.MinValue;
            float currentRatio = 0.0f;

            for (int i = 0; i < adyacentPairs.GetLength(1); i++)
            {
                if (dSets.SetOf(current) != dSets.SetOf(sets[i]))
                {
                    currentRatio = adyacentPairs[currentIndex, i];
                    currentRatio /= dSets.SetOf(sets[i]).Rank;
                    if (currentRatio > maxRatio)
                    {
                        candidateIndex = i;
                        maxRatio = currentRatio;
                    }
                }
            }

            if (candidateIndex >= 0)
            {
                //Updating matrix
                SetNode<int> result = dSets.Merge(sets[candidateIndex], current);
                int resultIndex = setsIndex[result];//Este indice no tiene porque ser el mismo del candidato
                adyacentPairs[currentIndex, candidateIndex] = adyacentPairs[candidateIndex, currentIndex] = 0;
                adyacentPairs[currentIndex, resultIndex] = adyacentPairs[resultIndex, currentIndex] = 0;
                for (int i = 0; i < adyacentPairs.GetLength(1); i++)
                {
                    if (adyacentPairs[resultIndex, i] != 0 && adyacentPairs[currentIndex, i] != 0)
                    {
                        adyacentPairs[resultIndex, i] += adyacentPairs[currentIndex, i];
                        adyacentPairs[i, resultIndex] = adyacentPairs[resultIndex, i];
                    }
                    else
                        adyacentPairs[resultIndex, i] += adyacentPairs[currentIndex, i];
                }
                return result;
            }
            return null;

        }
        private static int[,] GetAdyacentPairs(SetNode<int>[,] nodes, Dictionary<SetNode<int>, int> indexes, DisjointSets<int> dSets)
        {
            int[,] adyacentPairs = new int[indexes.Count, indexes.Count];
            SetNode<int> current, adyacent;

            for (int i = 1; i < nodes.GetLength(1); i++)
            {
                current = dSets.SetOf(nodes[0, i - 1]);
                adyacent = dSets.SetOf(nodes[0, i]);
                if (current != adyacent)
                {
                    adyacentPairs[indexes[current], indexes[adyacent]]++;
                    adyacentPairs[indexes[adyacent], indexes[current]]++;
                }
            }
            for (int i = 1; i < nodes.GetLength(0); i++)
            {
                current = dSets.SetOf(nodes[i, 0]);
                adyacent = dSets.SetOf(nodes[i - 1, 0]);
                if (current != adyacent)
                {
                    adyacentPairs[indexes[current], indexes[adyacent]]++;
                    adyacentPairs[indexes[adyacent], indexes[current]]++;
                }
            }
            for (int i = 1; i < nodes.GetLength(0); i++)
            {
                for (int j = 1; j < nodes.GetLength(1); j++)
                {
                    current = dSets.SetOf(nodes[i, j]);
                    adyacent = dSets.SetOf(nodes[i - 1, j]);
                    if (current != adyacent)
                    {
                        adyacentPairs[indexes[current], indexes[adyacent]]++;
                        adyacentPairs[indexes[adyacent], indexes[current]]++;
                    }
                    adyacent = dSets.SetOf(nodes[i, j - 1]);
                    if (current != adyacent)
                    {
                        adyacentPairs[indexes[current], indexes[adyacent]]++;
                        adyacentPairs[indexes[adyacent], indexes[current]]++;
                    }
                }
            }
            return adyacentPairs;
        }
        private static SpecialSparsedMatrix GetAdyacentPairsMatrix(SetNode<int>[,] nodes, Dictionary<SetNode<int>, int> indexes, DisjointSets<int> dSets)
        {
            SpecialSparsedMatrix matrix = new SpecialSparsedMatrix(dSets.SetsCount, dSets.SetsCount);
            SetNode<int> current, adyacent;

            for (int i = 1; i < nodes.GetLength(1); i++)
            {
                current = dSets.SetOf(nodes[0, i - 1]);
                adyacent = dSets.SetOf(nodes[0, i]);
                if (current != adyacent)
                {
                    matrix.IncrementOn(indexes[current], indexes[adyacent]);
                    matrix.IncrementOn(indexes[adyacent], indexes[current]);
                }
            }
            for (int i = 1; i < nodes.GetLength(0); i++)
            {
                current = dSets.SetOf(nodes[i, 0]);
                adyacent = dSets.SetOf(nodes[i - 1, 0]);
                if (current != adyacent)
                {
                    matrix.IncrementOn(indexes[current], indexes[adyacent]);
                    matrix.IncrementOn(indexes[adyacent], indexes[current]);
                }
            }
            for (int i = 1; i < nodes.GetLength(0); i++)
            {
                for (int j = 1; j < nodes.GetLength(1); j++)
                {
                    current = dSets.SetOf(nodes[i, j]);
                    adyacent = dSets.SetOf(nodes[i - 1, j]);
                    if (current != adyacent)
                    {
                        matrix.IncrementOn(indexes[current], indexes[adyacent]);
                        matrix.IncrementOn(indexes[adyacent], indexes[current]);
                    }
                    adyacent = dSets.SetOf(nodes[i, j - 1]);
                    if (current != adyacent)
                    {
                        matrix.IncrementOn(indexes[current], indexes[adyacent]);
                        matrix.IncrementOn(indexes[adyacent], indexes[current]);
                    }
                }
            }
            return matrix;
        }
        private static SetNode<int> MergeWithCandidate(SpecialSparsedMatrix adyacentPairs, SetNode<int> current, Dictionary<SetNode<int>, int> setsIndex, SetNode<int>[] sets, DisjointSets<int> dSets)
        {
            int currentIndex = setsIndex[current];
            int candidateIndex = -1;
            float maxRatio = float.MinValue;
            float currentRatio = 0.0f;

            SparsedNode node = adyacentPairs.rows[currentIndex].Next;

            while (node != null)
            {
                if (dSets.SetOf(current) != dSets.SetOf(sets[node.column]))
                {
                    currentRatio = ((float)node.value) / dSets.SetOf(sets[node.column]).Rank;
                    if (currentRatio > maxRatio)
                    {
                        candidateIndex = node.column;
                        maxRatio = currentRatio;
                    }
                }
                node = node.Next;
            }
            
            if (candidateIndex >= 0)
            {
                //Updating matrix
                SetNode<int> result = dSets.Merge(sets[candidateIndex], current);
                int resultIndex = setsIndex[result];//Este indice no tiene porque ser el mismo del candidato
                //Console.WriteLine("merge {0} with {1}", resultIndex, currentIndex);
                LinkedList<int> toUpdate =  adyacentPairs.SpecialMerge(resultIndex ,currentIndex );
                LinkedListNode<int> currentNode = toUpdate.First;
                while (currentNode != null)
                {
                    UpdateRow(currentNode.Value, adyacentPairs, result, sets, dSets);
                    currentNode = currentNode.Next;
                }
                return result;
            }
            return null;

        }
        private static void UpdateRow(int row, SpecialSparsedMatrix matrix, SetNode<int> parentNode, SetNode<int>[] sets, DisjointSets<int> dSets)
        {
            SparsedNode node = matrix.rows[row].Next;
            int value = 0;
            while (node != null)
            {
                if (dSets.SetOf(sets[node.column]) == parentNode)
                    value += node.value;
                node = node.Next;
            }
            node = matrix.rows[row].Next;
            while (node != null)
            {
                if (dSets.SetOf(sets[node.column]) == parentNode)
                    node.value = value;
                node = node.Next;
            }
        }


        

        #endregion
    }
}
