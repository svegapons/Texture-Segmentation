using System;
using System.Collections.Generic;

namespace TxEstudioKernel.Operators.ConnectedComponents.DataStructures
{
    class SpecialSparsedMatrix
    {
        internal SparsedNode[] rows;
        int cols;
        public int Width
        {
            get
            {
                return cols;
            }
        }
        public int Height
        {
            get
            {
                return rows.Length;
            }
        }
        public SpecialSparsedMatrix(int rows, int cols)
        {
            this.cols = cols;
            this.rows = new SparsedNode[rows];
            for (int i = 0; i < rows; i++)
                this.rows[i] = new SparsedNode(-1);
            
        }
        public int this[int row, int col]
        {
            get
            {
                if (row >= rows.Length || col >= cols) throw new IndexOutOfRangeException();
                SparsedNode current = rows[row];
                while (current != null && current.column < col)
                    current = current.Next;
                if (current == null || current.column != col)
                    return 0;
                else
                    return current.value;
            }
            set
            {
                if (row < 0 || col < 0|| row >= rows.Length || col >= cols) throw new IndexOutOfRangeException();
                if (value == 0)
                {
                    SparsedNode current = rows[row];
                    while (current.Next != null && current.column < col)
                        current = current.Next;
                    if (current.column == col)
                    {
                        current.Previous.Next = current.Next;
                        if (current.Next != null)
                            current.Previous.Next.Previous = current.Previous;
                    }
                }
                else
                {
                    SparsedNode current = rows[row];
                    while (current.Next != null && current.column < col)
                        current = current.Next;
                    if (current.column == col)
                        current.value = value;
                    else if (current.column < col)
                    {
                        SparsedNode newNode = new SparsedNode(col, value);
                        newNode.Previous = current;
                        newNode.Next = current.Next;
                        current.Next = newNode;
                        if (newNode.Next != null)
                            newNode.Next.Previous = newNode;
                    }
                    else
                    {
                        SparsedNode newNode = new SparsedNode(col, value);
                        newNode.Next = current;
                        newNode.Previous = current.Previous;
                        current.Previous.Next = newNode;
                        newNode.Next.Previous = newNode;
                    }
                }
            }
        }
        public void IncrementOn(int row, int col)
        {
            if (rows[row] == null)
                rows[row] = new SparsedNode(col, 1);
            else
            {
                SparsedNode current = rows[row];
                while (current.Next != null && current.Next.column < col)
                    current = current.Next;
                if (current.Next == null)
                {
                    current.Next = new SparsedNode(col, 1);
                    current.Next.Previous = current;
                }
                else
                {
                    if (current.Next.column == col)
                        current.Next.value++;
                    else
                    {
                        SparsedNode newNode = new SparsedNode(col, 1);
                        newNode.Next = current.Next;
                        newNode.Previous = current;
                        current.Next = newNode;
                        if (newNode.Next != null)
                            newNode.Next.Previous = newNode;
                    }
                }

            }
        }
        public LinkedList<int> SpecialMerge(int row, int with)
        {
            this[row, with] = 0;
            this[with, row] = 0;
            SparsedNode rowCurrent = rows[row].Next;//Sabemos que existe al menos uno en cada fila
            SparsedNode withCurrent = rows[with].Next;
            SparsedNode newCurrent = new SparsedNode(-1);
            LinkedList<int> toUpdate = new LinkedList<int>();
            
            while ( rowCurrent!= null || withCurrent != null)
            {
                if (rowCurrent == null)
                {
                    newCurrent.Next = withCurrent;
                    withCurrent.Previous = newCurrent;
                    break;
                }
                if (withCurrent == null)
                {
                    newCurrent.Next = rowCurrent;
                    rowCurrent.Previous = newCurrent;
                    break;
                }
                if (rowCurrent.column == withCurrent.column)
                {
                    newCurrent.Next = new SparsedNode(rowCurrent.column, rowCurrent.value + withCurrent.value);
                    newCurrent.Next.Previous = newCurrent;
                    toUpdate.AddLast(rowCurrent.column);
                    rowCurrent = rowCurrent.Next;
                    withCurrent = withCurrent.Next;
                    
                }
                else if (rowCurrent.column < withCurrent.column)
                {
                    newCurrent.Next = rowCurrent;
                    rowCurrent.Previous = newCurrent;
                    rowCurrent = rowCurrent.Next;
                }
                else
                {
                    newCurrent.Next = withCurrent;
                    withCurrent.Previous = newCurrent;
                    withCurrent = withCurrent.Next;
                }

                newCurrent = newCurrent.Next;
                //rowCurrent = rowCurrent.Next;
                //withCurrent = withCurrent.Next;
            }
            
            while (newCurrent.Previous != null)
                newCurrent = newCurrent.Previous;
            rows[row] = newCurrent;

            

            return toUpdate;
            //LinkedListNode<int> currentNode = toUpdate.First;
            //while (currentNode != null)
            //{
            //    UpdateRow( currentNode.Value, row, with);
            //    currentNode = currentNode.Next;
            //}
        }
        public void UpdateRow(int update, int colOne, int colTwo)
        {
            int max = Math.Max(colOne, colTwo);
            int min = Math.Min(colOne, colTwo);
            SparsedNode minNode = null;
            SparsedNode current = rows[update];

            while (current.column < min)
                current = current.Next;
            minNode = current;
            while (current.column < max)
                current = current.Next;
            current.value = minNode.value = minNode.value + current.value;
        }
    }

    class SparsedNode
    {
        public readonly int column;
        public int value;
        public SparsedNode(int col)
        {
            column = col;
        }
        public SparsedNode(int col, int value):this(col)
        {
            this.value = value;
        }
        public SparsedNode Next;
        public SparsedNode Previous;

    }
}
