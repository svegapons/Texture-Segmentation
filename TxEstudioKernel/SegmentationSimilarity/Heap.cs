using System;

namespace TxEstudioKernel.SegmentationSimilarity.DataStructures
{
    class HeapNode
    {
        public readonly int Index;
        public readonly int Rank;
        public HeapNode(int index, int rank)
        {
            Index = index;
            Rank = rank;
        }
    }
    class Heap
    {

        HeapNode[] elements;
        int count = 0;
        public Heap(int capacity)
        {
            elements = new HeapNode[capacity];
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        public HeapNode Top
        {
            get
            {
                if (count == 0)
                    throw new InvalidOperationException("No nodes on heap");
                return elements[0];
            }
        }

        public HeapNode ExtractMin()
        {
            HeapNode min = elements[0];
            count--;
            elements[0] = elements[count];
            elements[count] = null;
            HeapifyDown();
            return min;
        }

        public void Insert(HeapNode node)
        {
            if (count == elements.Length)
                throw new Exception("Heap overflow");
            elements[count] = node;
            count++;
            HeapifyUp();
            
        }
        private int Parent(int index)
        {
            return (index % 2 == 0) ? index/2  -1 :index / 2 ;
        }
        private int RightChild(int index)
        {
            return 2 * (index + 1);
        }
        private int LeftChild(int index)
        {
            return 2 * index + 1;
        }
        private void HeapifyDown()
        {
            int currentIndex = 0;
            int left  = LeftChild(currentIndex);
            int right = RightChild(currentIndex);
            int targetIndex = left ;
            while (left < count)
            {
                if (right < count && elements[right].Rank > elements[left].Rank)
                    targetIndex = right;
                if (elements[currentIndex].Rank < elements[targetIndex].Rank)
                {
                    Change(currentIndex, targetIndex);
                    currentIndex = targetIndex;
                    targetIndex = left = LeftChild(currentIndex);
                    right = RightChild(currentIndex);
                }
                else
                    break;
            }
        }

        private void HeapifyUp()
        {
            int currentIndex = count-1;
            int parentIndex = Parent(currentIndex);
            while (parentIndex >= 0)
            {
                if (elements[parentIndex].Rank < elements[currentIndex].Rank)
                //No se cumple la condicion de heap
                {
                    Change(currentIndex, parentIndex);
                    currentIndex = parentIndex;
                    parentIndex = Parent(currentIndex);
                }
                else
                    break;
            }
        }

        private void Change(int one, int other)
        {
            HeapNode aux = elements[one];
            elements[one] = elements[other];
            elements[other] = aux;
        }
    }
   
}
