using System;
using System.Collections.Generic;

using TxEstudioKernel;
using TxEstudioKernel.OpenCV;
using TxEstudioKernel.CustomAttributes;

using TxEstudioKernel.Operators.ConnectedComponents.DataStructures;


namespace TxEstudioKernel.Operators.ConnectedComponents
{
    /// <summary>
    /// Simplifies the image by using the connected components regions.
    /// </summary>
    [DigitalFilter]
    [Algorithm("Connected components image simplification","Simplifies the image in gray scale using a region growing method baseed on connected components." )]
    [Abbreviation("cc_sim", "Threshold")]
    public class GrayScaleImageSimplification:TxOneBand
    {

        int threshold = 0;
        /// <summary>
        /// Gets or sets the threshold for connected componnets extraction
        /// </summary>
        [Parameter("Threshold", "Threshold for connected componnets extraction")]
        [IntegerInSequence(255)]
        public int Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }

        int minSize;
        /// <summary>
        /// Gets or sets the minimum size of flat regions on the resulting image.
        /// </summary>
        [Parameter("Minimum Size", "Minimum size of flat regions on the resulting image")]
        [IntegerInSequence(0, int.MaxValue)]
        public int MinSize
        {
            get{return minSize;}
            set{minSize = value;}
        }


        public override TxImage Process(TxImage input)
        {
            DisjointSets<int> dSets;
            SetNode<int>[,] nodes = ConnectedComponentsToolkit.GetGrayScaleConnectedComponents(input, threshold, out dSets);
            ConnectedComponentsToolkit.Simplify(minSize, nodes, dSets);
            Dictionary<SetNode<int>, int> indexes = ConnectedComponentsToolkit.SetIndexes(nodes, dSets);
            //Buscar el color que mas se repite por componente conexa
            int[,] colors = new int[indexes.Count, 256];
            for (int i = 0; i < nodes.GetLength(0); i++)
                for (int j = 0; j < nodes.GetLength(1); j++)
                    colors[indexes[dSets.SetOf(nodes[i, j])], nodes[i, j].Tag]++;
            byte[] colorByComponent = new byte[indexes.Count];
            for (int i = 0; i < colors.GetLength(0); i++)
            {
                int maxCount = int.MinValue;
                int maxIndex = 0;
                for (int j = 0; j < colors.GetLength(1); j++)
                    if (colors[i, j] > maxCount)
                    {
                        maxIndex = j;
                        maxCount = colors[i, j];
                    }
                colorByComponent[i] = (byte)maxIndex;
            }
            return BuildImage(nodes, indexes, colorByComponent, dSets);
        }

        private TxImage BuildImage(SetNode<int>[,] nodes, Dictionary<SetNode<int>, int> indexes, byte[] colorByComponent, DisjointSets<int> dSets)
        {
            TxImage result = new TxImage(nodes.GetLength(1), nodes.GetLength(0), TxImageFormat.GrayScale);
            unsafe
            {
                _IplImage* innerImage = (_IplImage*)result.InnerImage;
                byte* current = (byte*)innerImage->imageData;
                int offset = innerImage->widthStep - innerImage->width;

                for (int i = 0; i < result.Height; i++, current += offset)
                    for (int j = 0; j < result.Width; j++, current++)
                        *current = colorByComponent[indexes[dSets.SetOf(nodes[i, j])]];
            }

            return result;
        }
    }
}
