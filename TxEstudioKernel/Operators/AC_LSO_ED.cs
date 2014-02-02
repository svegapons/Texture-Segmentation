using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.OpenCV;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [EdgeDetector]
    [Algorithm("acTED Method", "No Supervised edge detector algorithm using the Active Contour Without Edge Model and Connected Components Analysis")]
    [Abbreviation("acED", "Miu","Classes")]
    public class AC_LSO_ED : TxTextureEdgeDetector
    {
        AC_MultiClass ac = new AC_MultiClass();
        Canny canny = new Canny();
        float miu = 1.0f;
        int ccSize = 300;
        int classes = 4;

        [Parameter("Miu", "peso para calcular el length(C) ")]
        [RealInRange(0.0f, 1.0f)]
        public float Miu
        {
            get
            {
                return this.miu;
            }
            set
            {
                miu = value;
            }
        }

        [Parameter("Classes", "numero de clases")]
        public int Classes
        {
            get
            {
                return this.classes;
            }
            set
            {
                classes = value;
            }
        }

        [Parameter("CCSize", "Tamaño de la menor componente conexa")]
        public int CCSize
        {
            get
            {
                return this.ccSize;
            }
            set
            {
                ccSize = value;
            }
        }

        //public override TxImage Process(TxImage input)
        //{
        //    ac.Miu = this.Miu;
        //    ac.Classes = this.classes;
        //    ac.CCSize = this.CCSize;
        //    canny.Threshold1 = 80.0f;
        //    canny.Threshold2 = 180.0f;
        //    //TxImage result = new TxImage(input.Width, input.Height, TxImageFormat.GrayScale);
        //    //CV.cvCanny(ac.Segment(TxMatrix.FromImage(input)).ToImage().InnerImage, result.InnerImage, 80, 180, 3);
        //    return canny.Process(ac.Segment(TxMatrix.FromImage(input)).ToImage());           
        //}

        public override TxSegmentationResult Segment(params TxMatrix[] descriptors)
        {
            ac.Miu = this.Miu;
            ac.Classes = this.classes;
            ac.CCSize = this.CCSize;
            canny.Threshold1 = 0.0f;
            canny.Threshold2 = 10.0f;
            //TxImage result = new TxImage(input.Width, input.Height, TxImageFormat.GrayScale);
            //CV.cvCanny(ac.Segment(TxMatrix.FromImage(input)).ToImage().InnerImage, result.InnerImage, 80, 180, 3);
            TxSegmentationResult segmentacion = ac.Segment(descriptors);
            TxSegmentationResult segResult = new TxSegmentationResult(2, segmentacion.Width, segmentacion.Height);
            int ant = 0;
            for (int i = 0; i < segmentacion.Height; i++)
            {
                ant = segmentacion[i, 0];
                segResult[i, 0] = 0;
                for (int j = 1; j < segmentacion.Width; j++)
                {
                    if (segmentacion[i, j] != ant)
                        segResult[i, j] = 1;
                    else
                        segResult[i, j] = 0;

                    ant = segmentacion[i, j];
                }
            }

            for (int i = 0; i < segmentacion.Width; i++)
            {
                ant = segmentacion[0, i];
                //segResult[i, 0] = 0;
                for (int j = 1; j < segmentacion.Height; j++)
                {
                    if (segmentacion[j, i] != ant)
                        segResult[j, i] = 1;

                    ant = segmentacion[j, i];
                }
            }






            //TxImage result= canny.Process(ac.Segment(descriptors).ToImage());     
            //segResult = new TxSegmentationResult(2, descriptors[0].Width, descriptors[0].Height);

            //for (int i = 0; i < descriptors[0].Height; i++)
            //    for (int j = 0; j < descriptors[0].Width; j++)
            //    {
            //        if (result[j, i] >= 10)
            //            segResult[i, j] = 1;
            //        else segResult[i, j] = 0;
            //    }
            return segResult;
        }

        public override double ProbError()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
