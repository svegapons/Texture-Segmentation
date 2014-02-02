using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [GaborDescriptor]
    [Algorithm("2DGabor Wavelets", "Descriptor de Textura de Wavelets de Gabor 2D")]
    [Abbreviation("gab_wav", "CurrentScale", "CurrentAngle")]
    public class GaborWaveletTextureDescriptor2D :TextureDescriptorSequence
    {
		int scale = 4, orientations = 8, side = 21, currentScale, currentAngle;
		double uL, uH;
		bool normalizationFlag;

        public GaborWaveletTextureDescriptor2D(int scale, int orientations, double uL, double uH, bool normalizationFlag)
        {
            if (orientations < 0)
                throw new ArgumentException("Las orientaciones en el GaborEnergyTextureDescriptor no pueden ser negativas");
            if (scale <= 0)
                throw new ArgumentNullException("El parametro scale tiene que ser positivo");
            this.scale = scale;
			this.orientations = orientations;
			this.uL = uL;
			this.uH = uH;
            this.normalizationFlag = normalizationFlag;
            currentScale = -1;
		}


        public GaborWaveletTextureDescriptor2D()
        {
            this.uL = 0.05;
            this.uH = 0.3;
            this.normalizationFlag = true;
            currentScale = -1;
        }

        #region ITextureDescriptor Members

        public override TxMatrix GetDescription(TxImage image)
        {
            if (scale == 0)
                throw new InvalidOperationException("No estan definidas las frecuencias por las cuales se va a aplicar Gabor");
            TxMatrix gaborReal = new TxMatrix(2 * side + 1, 2 * side + 1);
            TxMatrix gaborImag = new TxMatrix(2 * side + 1, 2 * side + 1);

            double a;
            if (scale >= 2)
            {
                double baseD = uH / uL;
                a = Math.Pow(baseD, 1.0 / (scale - 1));
            }
            else
            {
                a = 1.0;
            }

            double u0 = uH / Math.Pow(a, scale - currentScale + 1);
            double var = Math.Pow(0.6 / uH * Math.Pow(a, scale - currentScale + 1), 2.0);

            double t1 = Math.Cos(Math.PI / orientations * currentAngle);
            double t2 = Math.Sin(Math.PI / orientations * currentAngle);
            double X, Y, G;

            for (int x = 0; x < 2 * side + 1; x++)
            {
                for (int y = 0; y < 2 * side + 1; y++)
                {
                    X = (y - side) * t1 + (x - side) * t2;
                    Y = -(y - side) * t2 + (x - side) * t1;
                    G = 1.0 / (2.0 * Math.PI * var) * Math.Pow(a, scale - currentScale + 1) * Math.Exp(-0.5 * (X * X + Y * Y) / var);
                    gaborReal[x, y] = (float)(G * Math.Cos(2.0 * Math.PI * u0 * X));
                    gaborImag[x, y] = (float)(G * Math.Sin(2.0 * Math.PI * u0 * X));
                }
            }

            // if normalizationFlag, then remove the DC from the real part of Gabor

            if (normalizationFlag)
            {
                double m = 0.0;
                for (int x = 0; x < 2 * side + 1; x++)
                {
                    for (int y = 0; y < 2 * side + 1; y++)
                    {
                        m += gaborReal[x, y];
                    }
                }
                m /= Math.Pow(2.0 * side + 1, 2.0);

                for (int x = 0; x < 2 * side + 1; x++)
                {
                    for (int y = 0; y < 2 * side + 1; y++)
                    {
                        gaborReal[x, y] -= (float)m;
                    }
                }
            }
            TxMatrix imageMatrix = TxMatrix.FromImage(image);
            return TxMatrix.VectorialAdd(imageMatrix.Convolve(gaborReal), imageMatrix.Convolve(gaborImag));
        }

        #endregion

        #region IEnumerator<TextureDescriptor> Members

        public override TextureDescriptor Current
        {
            get
            {
                if (currentScale < 0)
                    throw new InvalidOperationException();
                return this;
            }
        }

        #endregion

       

        #region IEnumerator Members

        

        public override bool MoveNext()
        {
            if (currentScale == -2)
                throw new InvalidOperationException();
            if (currentScale == -1)
            {
                currentAngle = currentScale = 0;
                return true;
            }
            if (currentScale != scale)
            {
                if (currentAngle == orientations - 1)
                {
                    currentAngle = 0;
                    return ++currentScale != scale;
                }
                else
                {
                    currentAngle++;
                    return true;
                }
            }
            return false;
        }

        public override void Reset()
        {
            currentScale = -1;
        }

        #endregion

        [Parameter("Current Gabor Filter Angle", "Angulo del filtro de wavelets de Gabor")]
        [IntegerInSequence(0, 180)]
        public int CurrentAngle
        {
            get
            {
                return 180 * currentAngle / orientations;
            }
        }

        [Parameter("Current Gabor Filter Frequency", "Frecuencia del filtro de wavelets de Gabor")]
        [IntegerInSequence(1, int.MaxValue)]
        public int CurrentScale
        {
            get
            {
                return currentScale;
            }
        }

        [Parameter("Scale", "Cantidad de niveles del filtro de wavelets de Gabor")]
        [IntegerInSequence(1, int.MaxValue)]
        public int Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        [Parameter("Number of Azimuts", "Cantidad de orientaciones del filtro de wavelets de Gabor")]
        [IntegerInSequence(1, 180)]
        public int Orientations
        {
            get
            {
                return orientations;
            }
            set
            {
                orientations = value;
            }
        }
    }
}
