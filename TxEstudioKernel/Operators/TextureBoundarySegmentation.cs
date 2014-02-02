using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{

    [Algorithm("kmTED Method", "No Supervised image texture segmentation using the KMean Algorithm, Conected Component Analisys and Canny Edge Extractor")]
    [Abbreviation("tbs", "NumeroClases", "MaxIteraciones", "NumeroMuestras", "NumeroPruebas", "MinimumThreshold", "MaximumThreshold", "Size")]
    public class Texture_Boundary__Segmentation : TxTextureEdgeDetector
    {
        KmeanNoSupervImagenes segmenter = new KmeanNoSupervImagenes();
        ConnectedComponents.SegmentationSimplificationByCount cc = new TxEstudioKernel.Operators.ConnectedComponents.SegmentationSimplificationByCount();
        ConnectedComponents.SegmentationSimplificationBySize ccs = new TxEstudioKernel.Operators.ConnectedComponents.SegmentationSimplificationBySize();
        //Canny borderExtractor = new Canny();
        Laplacian borderExtractor = new Laplacian();
        

        #region Propiedades del metodo Kmeans
        int numeroPruebas = 1;
        [Parameter("nTest", "")]
        [IntegerInSequenceAttribute(1, 100)]
        public int NumeroPruebas
        {
            get { return numeroPruebas; }
            set
            {
                numeroPruebas = value;
                segmenter.NumeroPruebas = value;
            }
        }

        int numeroMuestras = 6;//Porciento del total de pixeles de la imagen que van a ser tomados de muestra
        [Parameter("%muestreo", "")]
        [IntegerInSequenceAttribute(1, 100)]
        public int NumeroMuestras
        {
            get
            {
                return numeroMuestras;
            }
            set
            {
                numeroMuestras = value;
                segmenter.NumeroMuestras = value;                
            }
        }

        int numeroClases = 5;
        [Parameter("nClases", "")]
        public int NumeroClases
        {
            get
            {
                return numeroClases;
            }
            set
            {
                numeroClases = value;
                segmenter.NumeroClases = value;
                cc.MaxCount = value;
            }
        }

        int maxIteraciones = 10;
        [Parameter("MaxIter", "")]
        public int MaxIteraciones
        {
            get
            {
                return maxIteraciones;
            }
            set
            {
                maxIteraciones = value;
                segmenter.MaxIteraciones = value;
            }
        }

        int size = 1;//Porciento del total de pixeles de la imagen que van a ser tomados de muestra
        [Parameter("ccFilterSize", "")]
        [IntegerInSequenceAttribute(1, 10000)]
        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                ccs.MinSize = value;
            }
        }


        int aperture_size = 3;
        [Parameter("Apertura", "Size of the extended Sobel kernel, must be 1, 3, 5 or 7.")]
        [IntegerInSequence(1, 7, 2)]
        public int Aperture_Size
        {
            get
            {
                return aperture_size;
            }
            set
            {
                aperture_size = value;
                borderExtractor.Aperture_Size = aperture_size;
            }
        }
        #region Parametros de Canny
        //int thresholdmMin = 80;
        //[Parameter("MinimumThreshold", "")]
        //public int MinimumThreshold
        //{
        //    get
        //    {
        //        return thresholdmMin;
        //    }
        //    set
        //    {
        //        thresholdmMin = value;
        //        borderExtractor.Threshold1 = value;
        //    }
        //}
        //int thresholdmMax = 80;
        //[Parameter("MaximumThreshold", "")]
        //public int MaximumThreshold
        //{
        //    get
        //    {
        //        return thresholdmMax;
        //    }
        //    set
        //    {
        //        thresholdmMax = value;
        //        borderExtractor.Threshold2 = value;
        //    }
        //}
        #endregion

        #endregion

        #region Metodos Heredados
        public override TxSegmentationResult Segment(params TxMatrix[] descriptors)
        {

            TxSegmentationResult segResult = segmenter.Segment(descriptors);
            if (ccs.MinSize > 1)
                segResult = ccs.Process(segResult);
            else
                segResult = cc.Process(segResult);
            TxImage result = borderExtractor.Process(segResult.ToImage());
            segResult = new TxSegmentationResult(2, descriptors[0].Width, descriptors[0].Height);

            for (int i = 0; i < descriptors[0].Height; i++)
                for (int j = 0; j < descriptors[0].Width; j++)
                {
                    if (result[j, i] >= 10)
                        segResult[i, j] = 1;
                    else segResult[i, j] = 0;
                }
            return segResult;
        }

        public override double ProbError()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}