using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
   
    [FirstOrderStatisticDescriptor]
    [Algorithm("Average", "Calculates the Average feature of grey levels in a Window of size Nx x My.")]
    [Abbreviation("fo_ave", "nRows", "nCols")]
    public class MeanDescriptor : TextureDescriptor
    {

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
            SmoothSimpleBlurFilter mean = new SmoothSimpleBlurFilter();
            mean.nRows = nrows;
            mean.nCols = nCols;
            return TxMatrix.FromImage(mean.Process(image));
        }

        #endregion
    }
}
