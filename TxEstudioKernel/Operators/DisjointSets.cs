using System;
using System.Collections.Generic;


namespace DataStructures
{    
    public class SetNode<T>
    {
        public SetNode(T tag)
        {
            this.Tag = tag;
        }
        public SetNode(T tag, SetNode<T> parent)
            : this(tag)
        {
            this.Parent = parent;
        }
        public T Tag;
        public SetNode<T> Parent;
        public int Rank = 1;
    }
    public class DisjointSets<T>:IEnumerable<SetNode<T>>
    {
        protected LinkedList<SetNode<T>> setNodes = new LinkedList<SetNode<T>>();

        private int setsCount = 0;
        public int SetsCount
        {
            get { return setsCount; }
        }
        public SetNode<T> CreateSet(T element)
        {
            SetNode<T> result = new SetNode<T>(element);
            setNodes.AddLast(result);
            setsCount++;
            return result;
        }
        public SetNode<T> SetOf(SetNode<T> child)
        {
            if (child.Parent == null)
                return child;
            SetNode<T> result = child.Parent;
            while (result.Parent != null)
                result = result.Parent;
            child.Parent = result;//Path compression
            return result;
        }
        public SetNode<T> Merge(SetNode<T> one, SetNode<T> other)
        {
            if (one == other) throw new Exception("Iguales");
            SetNode<T> parentSet   = SetOf(one);
            SetNode<T> childSet = SetOf(other);

            if (parentSet != childSet)//Si ya estan mezclados no hacerlo
            {
                if (parentSet.Rank < childSet.Rank)
                {
                    SetNode<T> aux = parentSet;
                    parentSet = childSet;
                    childSet = aux;
                }

                childSet.Parent = parentSet;
                parentSet.Rank += childSet.Rank;
                setsCount--;
            }

            return parentSet;
        }

        #region IEnumerable<SetNode<T>> Members

        IEnumerator<SetNode<T>> IEnumerable<SetNode<T>>.GetEnumerator()
        {
            return setNodes.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return setNodes.GetEnumerator();
        }

        #endregion
    }
}
