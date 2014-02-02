using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{

    [FirstOrderStatisticDescriptor]
    [Algorithm("Deviation", "Calculates the Deviation feature of a grey levels in a Window of size Nx x My.")]
    [Abbreviation("fo_devi", "nRows", "nCols")]
    public class DeviationDescriptor :  TextureDescriptor
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


        #region TextureDescriptor Members

        public override TxMatrix GetDescription(TxImage image)
        {
            TxImage gris = image.ToGrayScale();
            double resultadoParcial = 0;
            double color;
            TxMatrix media;

            MeanDescriptor med = new MeanDescriptor();
            med.nCols = nCols;
            med.nRows = nrows;
            media = med.GetDescription(gris);
            TxMatrix resultado = new TxMatrix(media.Height, media.Width);

            for (int j = nCols / 2; j < image.Height - (nCols / 2); j++)
            {
                for (int i = nrows / 2; i < image.Width - (nrows / 2); i++)
                {
                    //Para calcular el deviation
                    for (int y = -nCols / 2; y < nCols / 2 + 1; y++)
                        for (int x = -nrows / 2; x < nrows / 2 + 1; x++)
                        {
                            color = (gris[i + x, j + y] - media[j, i]) * (gris[i + x, j + y] - media[j, i]);
                            resultadoParcial += color;

                        }

                    resultadoParcial = resultadoParcial / (nrows * nCols - 1);
                    
                    resultado[j, i] = (float)resultadoParcial;
                    resultadoParcial = 0;
                }
            }

            resultado = CopyRectangle(resultado, new System.Drawing.Point(nCols / 2, nrows / 2), new System.Drawing.Point(0, nrows / 2), resultado.Width - nrows / 2, nCols / 2);
            resultado = CopyRectangle(resultado, new System.Drawing.Point(0, nrows / 2), new System.Drawing.Point(0, 0), nrows / 2, resultado.Height);
            resultado = CopyRectangle(resultado, new System.Drawing.Point(0, resultado.Width - nrows), new System.Drawing.Point(0, resultado.Width - nrows / 2), nrows / 2, resultado.Height);
            resultado = CopyRectangle(resultado, new System.Drawing.Point(resultado.Height - nCols, 0), new System.Drawing.Point(resultado.Height - nCols / 2, 0), resultado.Width, nCols / 2);

            return resultado;

        }

        #endregion
    }
}