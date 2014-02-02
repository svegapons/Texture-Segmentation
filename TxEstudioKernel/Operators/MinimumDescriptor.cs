using System;
using System.Collections.Generic;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel.Operators
{
    [FirstOrderStatisticDescriptor]
    [Algorithm("Minimum", "Extract the grey level of Minimum value in a Window of size Nx x My.")]
    [Abbreviation("fo_min", "nRows", "nCols")]
    public class MinimumDescriptor:TextureDescriptor
    {

        //int neigborhood = 3;
        //[Parameter("Neighborhood","")]
        //[IntegerInSequence(1,101,2)]
        //public int Neighborhood
        //{
        //    get
        //    {
        //        return neigborhood;
        //    }
        //    set
        //    {
        //        neigborhood = value;
        //    }
        //}
        int nrows = 3;//Valor por defecto para nrows

        [Parameter("Windows size x", "The x window size of operator")]
        [IntegerInSequence(1, int.MaxValue, 2)]
        public int nRows
        {
            get { return nrows; }
            set { nrows = value; }
        }

        int mcols = 3;//Valor por defecto para mcols
        [Parameter("Windows size y", "The y window size of operator")]
        [IntegerInSequence(1, int.MaxValue, 2)]
        public int nCols
        {
            get { return mcols; }
            set { mcols = value; }
        }
        #region ITextureDescriptor Members

        public override TxMatrix GetDescription(TxImage image)
        {
            TxImage result = (TxImage)image.Clone();
            TxMorphKernel kernel = new TxMorphKernel(nCols, nRows);
            CV.cvErode(image.InnerImage, result.InnerImage, kernel.InnerKernel, 1);
            //ErodeFilter erode = new ErodeFilter();
            //erode.Window = neigborhood;
            //return TxMatrix.FromImage(erode.Process(image));
            return TxMatrix.FromImage(result);
        }

        #endregion
    }
}
