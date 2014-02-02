using System;
using System.Collections.Generic;
using System.Drawing;
using DataStructures;

namespace ActiveContour
{
    public class SimpleLinkedListNode<T>
    {
        public SimpleLinkedListNode<T> Previous;
        public SimpleLinkedListNode<T> Next;
        public T Tag;
    }
    public class SimpleLinkedList<T>
    {
        public SimpleLinkedListNode<T> First;
        public SimpleLinkedListNode<T> Last;

        public void Add(T tag)
        {
            if (First == null)
            {
                Last = First = new SimpleLinkedListNode<T>();
                First.Tag = tag;
            }
            else
            {
                Last.Next = new SimpleLinkedListNode<T>();
                Last.Next.Previous = Last;
                Last = Last.Next;
                Last.Tag = tag;
            }
        }
        public void Add(SimpleLinkedList<T> list)
        {
            if (First != null)
            {
                Last.Next = list.First;
                list.First.Previous = Last;
                Last = list.Last;
            }
            else
            {
                First = list.First;
                Last = list.Last;
            }
        }
    }
    public class Blob
    {
        public readonly int start, end, row, value;
        public readonly SetNode<int> node;

        public Blob(int row, int start, int end, int value, DisjointSets<int> dSets)
        {
            this.row = row;
            this.start = start;
            this.end = end;
            this.value = value;
            node = dSets.CreateSet(end - start  + 1);
        }

        public bool Overlaps(Blob blob)
        {
            if (start >= blob.start)
                return blob.end >= start;
            else
                return end >= blob.start; 
        }
    }

    public class CCSizeFilter
    {
        
        public void Filter(int[,] matrix, int size)
        {
            DisjointSets<int> dSets = new DisjointSets<int>();
            int aux = 0;
            SimpleLinkedList<Blob> allBlobs = GetConectedComponents(matrix, dSets);
            SimpleLinkedListNode<Blob> current = allBlobs.First;
            Blob blob;
            while (current != null)
            {
                blob = current.Tag;
                if (dSets.SetOf(blob.node).Tag <= size)
                    for (int i = blob.start; i <= blob.end; i++)
                    {
                        aux = -blob.value;//faster
                        matrix[blob.row, i] = aux;
                    }
                current = current.Next;
            }
        }

        protected SimpleLinkedList<Blob> GetConectedComponents(int[,] matrix, DisjointSets<int> dSets)
        {
            SimpleLinkedList<Blob> allBlobs = new SimpleLinkedList<Blob>();
            SimpleLinkedList<Blob> priorRow = new SimpleLinkedList<Blob>();
            SimpleLinkedList<Blob> currentRow = null;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                currentRow = ReadRow(i, matrix, dSets);
                MergeRows(priorRow, currentRow, dSets);
                if (priorRow.First != null)
                    allBlobs.Add(priorRow);
                priorRow = currentRow;
            }
            return allBlobs;
        }

        protected SimpleLinkedList<Blob> ReadRow(int row,int[,] matrix, DisjointSets<int> dSets)
        {
            SimpleLinkedList<Blob> result = new SimpleLinkedList<Blob>();
            int start = 0, value = matrix[row,0];
            for (int i = 1; i < matrix.GetLength(1); i++)
            {
                if (matrix[row, i] != value)
                {
                    result.Add(new Blob(row, start, i - 1, value, dSets));
                    value = matrix[row, i];
                    start = i;
                }
            }
            result.Add(new Blob(row, start, matrix.GetLength(1)-1, value, dSets));
            return result;
        }

        protected void MergeRows(SimpleLinkedList<Blob> first, SimpleLinkedList<Blob> second, DisjointSets<int> dSets)
        {
            SimpleLinkedListNode<Blob> firstCurrent = first.First;
            SimpleLinkedListNode<Blob> secondCurrent = second.First;
            while (firstCurrent != null)
            {
                secondCurrent = second.First;
                while (secondCurrent != null)
                {
                    if (firstCurrent.Tag.value == secondCurrent.Tag.value && firstCurrent.Tag.Overlaps(secondCurrent.Tag))
                        dSets.Merge(firstCurrent.Tag.node, secondCurrent.Tag.node).Tag += secondCurrent.Tag.node.Tag;
                    secondCurrent = secondCurrent.Next;
                }
                firstCurrent = firstCurrent.Next;
            }
        }
    }
}
