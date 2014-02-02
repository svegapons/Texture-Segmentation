using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.OpenCV;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    
    [FirstOrderStatisticDescriptor]
    [Algorithm("Maximum", "Extract the grey level of Maximum value in a Window of size Nx x My.")]
    [Abbreviation("fo_max", "nRows", "nCols")]
    public class MaximumDescriptor :  TextureDescriptor
    {

        //int neigborhood = 3;
        //[Parameter("Neighborhood", "")]
        //[IntegerInSequence(1, 101, 2)]
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
            CV.cvDilate(image.InnerImage, result.InnerImage, kernel.InnerKernel, 1);
            //DilateFilter dilate = new DilateFilter();
            //dilate.Window = neigborhood;
            //return TxMatrix.FromImage(dilate.Process(image));
            return TxMatrix.FromImage(result);
        }

        #endregion
    }
}
