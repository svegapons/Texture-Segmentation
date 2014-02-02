using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel;
using TxEstudioKernel.Operators.ConnectedComponents.DataStructures;
using TxEstudioKernel.Operators.ConnectedComponents;

namespace TxEstudioKernel.Operators.ConnectedComponents
{

    public class NewFilterCC : TxAlgorithm
    {
        int minSize = 100;
        /// <summary>
        /// Gets and sets the minimum size of the flat regions on the resulting segmentation.
        /// </summary>
        public int MinSize
        {
            get { return minSize; }
            set { minSize = value; }
        }

        public int[,] Filter(int[,] matrix)
        {
            DisjointSets<int> dSets;
            SetNode<int>[,] nodes = ConnectedComponentsToolkit.GetConnectedComponents(matrix, out dSets);
            ConnectedComponentsToolkit.Simplify(minSize, nodes, dSets);
            //Se asigna una etiqueta correspondiendo a la componente mas grande que dio lugar a la resultante
            //Se garantiza que componentes resultantes que pertenecian a clases iguales sigan perteneciendo a la misma clase
            //Se pasa mas trabajo por la posible desaparicion de clases
            int[,] values = new int[matrix.GetLength(0), matrix.GetLength(1)];
           // int[] classes = new int[segmentation.Classes];
            int lastClass = 0;
            int currentClass = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    //currentClass = dSets.SetOf(nodes[i, j]).Tag;
                    //if (classes[currentClass] == 0)
                    //{
                    //    lastClass++;
                    //    classes[currentClass] = lastClass;
                    //}
                    //values[i, j] = classes[currentClass] - 1;
                    values[i, j] = dSets.SetOf(nodes[i, j]).Tag;
                }
            return values;
        }
    }

}
