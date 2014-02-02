using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [DigitalFilter]
    [Algorithm("Anisotropic Filter", "Anisotropic Filter to obtain the image invariant illumination.")]  
    [Abbreviation("norm_aniso","Smooth")]
    public class Anisotropic:TxOneBand
    {
        protected float smooth = 1;

        [Parameter("Smooth","Smooth Constrain")]
        public virtual float Smooth
        {
            get { return smooth; }
            set { smooth = value; }
        }

        public override TxImage Process(TxImage image)
        {
            TxMatrix src = new TxMatrix(image.Height, image.Width);

            bool first = true;
            byte N, S, E, W, A;
            short Lw, Le, Ls, Ln = 0;
            float pw, pe, ps, pn, eps = 0.1f, min = 0, max = 0;
            float tmp = 0;

            int width=image.Width, height=image.Height;
            TxImage grayImage=image;
            if (image.ImageFormat == TxImageFormat.RGB)
                grayImage = image.ToGrayScale();
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    tmp = grayImage[x,y];
                    //Applying the process to all pixels of image except to the borders
                    if ((x > 0) && (x < width - 1) && (y > 0) && (y < height - 1))
                    {
                        //Adjacent neighbouring pixels
                        A = grayImage[x,y];         //current
                        E = grayImage[x, y + 1];    //east
                        S = grayImage[x + 1, y];    //south
                        N = grayImage[x - 1, y];    //north
                        W = grayImage[x, y - 1];    //west

                        //Ld refers to the derivative with respect to 
                        //each of the four adjacent neighbouring pixels
                        Lw = (short)(A - W);
                        Le = (short)(A - E);
                        Ln = (short)(A - N);
                        Ls = (short)(A - S);

                        //Weber’s contrast inverse
                        pw = Math.Min(A, W) / (Math.Abs(A - W) + eps);
                        pe = Math.Min(A, E) / (Math.Abs(A - E) + eps);
                        pn = Math.Min(A, N) / (Math.Abs(A - N) + eps);
                        ps = Math.Min(A, S) / (Math.Abs(A - S) + eps);

                        //Anisotropic smoothing
                        tmp = A + smooth * (Ln * pn + Ls * ps + Le * pe + Lw * pw);
                    }

                    src[y, x] = tmp;

                    //Computing the min and max values from all pixels of image
                    if (first) { min = max = tmp; first = false; }
                    else
                    {
                        if (tmp < min) min = tmp;
                        else
                            if (tmp > max) max = tmp;
                    }
                }
            }
            //return Normalize(src, min, max);
            return src.ToImage();
        }

        protected virtual TxImage Normalize(TxMatrix matrix, float min, float max)
        {
            int nfil = matrix.Height;
            int ncol = matrix.Width;

            float k = 255f / (max - min);
            TxImage result = new TxImage(ncol, nfil, TxImageFormat.GrayScale);

            for (int y = 0; y < ncol; y++)
            {
                for (int x = 0; x < nfil; x++)
                {
                    float gray = k * (matrix[x, y] - min);
                    result[y, x] = Convert.ToByte(gray);
                }
            }
            return result;
        }
    }
}
