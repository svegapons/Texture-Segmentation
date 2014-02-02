using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel;
using TxEstudioKernel.Operators.ConnectedComponents.DataStructures;

namespace TxEstudioKernel.Operators.ConnectedComponents
{
    /// <summary>
    /// Simplifies the given segmentation using a region growing method.
    /// All flat regions in the resulting segmentation are greather than the minimum size given.
    /// </summary>
    [TxEstudioKernel.CustomAttributes.Abbreviation("cc_size", "MinSize")]
    public class SegmentationSimplificationBySize: TxSegmentationOperator
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

        public override TxSegmentationResult Process(TxSegmentationResult segmentation)
        {
            DisjointSets<int> dSets;
            SetNode<int>[,] nodes = ConnectedComponentsToolkit.GetConnectedComponents(segmentation, out dSets);
            ConnectedComponentsToolkit.Simplify(minSize, nodes, dSets);
            //Se asigna una etiqueta correspondiendo a la componente mas grande que dio lugar a la resultante
            //Se garantiza que componentes resultantes que pertenecian a clases iguales sigan perteneciendo a la misma clase
            //Se pasa mas trabajo por la posible desaparicion de clases
            int[,] values = new int[segmentation.Height, segmentation.Width];
            int[] classes = new int[segmentation.Classes];
            int lastClass = 0;
            int currentClass = 0;
            for (int i = 0; i < segmentation.Height; i++)
                for (int j = 0; j < segmentation.Width; j++)
                {
                    currentClass = dSets.SetOf(nodes[i, j]).Tag;
                    if (classes[currentClass] == 0)
                    {
                        lastClass++;
                        classes[currentClass] = lastClass;
                    }
                    values[i, j] = classes[currentClass] - 1;
                }
            return new TxSegmentationResult(values);
        }
    }

    /// <summary>
    /// Simplifies the given segmentation using a region growing method.
    /// The resulting segmentation contains at most the specified count.
    /// </summary>
    [TxEstudioKernel.CustomAttributes.Abbreviation("cc_count", "MaxCount")]
    public class SegmentationSimplificationByCount : TxSegmentationOperator
    {
        int maxCount = 5;
        /// <summary>
        /// Gets or sets the maximal count of connected components that the resulting segmentation should have.
        /// </summary>
        public int MaxCount
        {
            get { return maxCount; }
            set { maxCount = value; }
        }

        public override TxSegmentationResult Process(TxSegmentationResult segmentation)
        {
            DisjointSets<int> dSets;
            SetNode<int>[,] nodes = ConnectedComponentsToolkit.GetConnectedComponents(segmentation, out dSets);
            ConnectedComponentsToolkit.SimplifyTo(maxCount, nodes, dSets);
            Dictionary<SetNode<int>, int> indexes = ConnectedComponentsToolkit.SetIndexes(nodes, dSets);
            TxSegmentationResult result = new TxSegmentationResult(indexes.Count, segmentation.Width, segmentation.Height);
            for (int i = 0; i < segmentation.Height; i++)
                for (int j = 0; j < segmentation.Width; j++)
                    result[i, j] = indexes[dSets.SetOf(nodes[i, j])];
            return result;
        }
    }
}
