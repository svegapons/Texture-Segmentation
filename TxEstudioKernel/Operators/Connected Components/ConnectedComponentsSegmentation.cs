using System;
using System.Collections.Generic;
using TxEstudioKernel.Operators.ConnectedComponents.DataStructures;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators.ConnectedComponents
{
    /// <summary>
    /// Segments the image by its connecteed components. If more than one descriptor is used returns the connected components on the resulting average image.
    /// </summary>
    [Algorithm("Connected Components Segmentation", 
        "Segments the image by its connecteed components. If more than one descriptor is used returns the connected components on the resulting average image.")]
    [Abbreviation("cc_seg", "Threshold")]
    public class ConnectedComponentsSegmentation:TxSegmentationAlgorithm
    {

        int threshold = 0;

        /// <summary>
        /// Maximal differrence between two adyacent pixels to be considered on the same connected component
        /// </summary>
        [Parameter("Threshold", "Maximal differrence between two adyacent pixels to be considered on the same connected component")]
        [IntegerInSequence(255)]
        public int Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }
        /// <summary>
        /// Performs a connected components segmentation over the given image.
        /// </summary>
        /// <param name="image">Target image</param>
        /// <returns>A TxSegmentationResult object containing the connected components regions</returns>
        public  TxSegmentationResult Segment(TxImage image)
        {
            TxImage grayImage = (image.ImageFormat == TxImageFormat.GrayScale)?image:image.ToGrayScale();
            DisjointSets<int> dSets;
            SetNode<int>[,] nodes = ConnectedComponentsToolkit.GetGrayScaleConnectedComponents(grayImage, threshold, out dSets);
            Dictionary<SetNode<int>, int> indexes = ConnectedComponentsToolkit.SetIndexes(nodes, dSets);
            TxSegmentationResult result = new TxSegmentationResult(indexes.Count, image.Width, image.Height);
            for (int i = 0; i < result.Height; i++)
                for (int j = 0; j < result.Width; j++)
                    result[i, j] = indexes[dSets.SetOf(nodes[i, j])];
            return result;
        }

        /// <summary>
        /// Performs a connected component segmenation over the resulting average image of the given image features.
        /// </summary>
        /// <param name="descriptors">An array of feature images</param>
        /// <returns>A TxSegmentationResult object containing the connected components regions on the resulting average image</returns>
        public override TxSegmentationResult Segment(params TxMatrix[] descriptors)
        {
            TxImage[] images = new TxImage[descriptors.Length];
            for (int i = 0; i < descriptors.Length; i++)
                images[i] = descriptors[i].ToImage();
            return Segment(new TxEstudioKernel.Operators.TxAverage().Process(images));
        }

        //metodo heredado de Iclasificable.
        public override double ProbError()
        {
            throw new Exception("Este segmentador no tiene implementado una funcion para la estimacion no supervisada del error de clasificacion");
        }
    }
}
