using System;
using System.Collections;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    /// <summary>
    /// Summary description for TxGaborGratingCellFilter.
    /// </summary>
    [GaborDescriptor]
    [Algorithm("2DGabor Simple", "Descriptor de Textura de Gabor Simple 2D")]
    [Abbreviation("gab_simple", "CurrentFrequency", "CurrentAngle")]
    public class GaborSimpleTextureDescriptor2D : TextureDescriptorSequence
    {
        int Sx = 21, Sy = 21;
        float[] frecuencias = new float[] { 1f / 4, 1f / 8, 1f / 16 };
        int currentFreq;
        float fase, radio;
        int orientaciones = 8, currentAngle;

        public GaborSimpleTextureDescriptor2D(int nOrientaciones, float[] frecuencias, float radio, float fase)
        {
            if (nOrientaciones < 0)
                throw new ArgumentException("Las orientaciones en el GaborEnergyTextureDescriptor no pueden ser negativas");
            if (frecuencias == null)
                throw new ArgumentNullException("El parametro frecuencias no puede ser null");
            this.radio = radio;
            this.frecuencias = frecuencias;
            this.orientaciones = nOrientaciones;
            this.fase = fase;
            currentFreq = -1;
        }

        /// <summary>
        /// Constructor de la clase TxGaborEnergyFilter
        /// </summary>
        /// <param name="nOrientaciones">cantidad de orientaciones que se realizaran entre 0 y pi</param>
        /// <param name="frecuencias">Valores frecuenciales del filtrado</param>
        public GaborSimpleTextureDescriptor2D(int nOrientaciones, float[] frecuencias)
        {
            if (nOrientaciones < 0)
                throw new ArgumentException("Las orientaciones en el GaborEnergyTextureDescriptor no pueden ser negativas");
            if (frecuencias == null)
                throw new ArgumentNullException("El parametro frecuencias no puede ser null");
            this.radio = 0.5f;
            this.frecuencias = frecuencias;
            this.orientaciones = nOrientaciones;
            this.fase = 0;
            currentFreq = -1;
        }

        /// <summary>
        /// Constructor de la clase TxGaborEnergyFilter
        /// </summary>
        public GaborSimpleTextureDescriptor2D()
        {
            this.radio = 0.5f;
            this.fase = 0;
            currentFreq = -2;
        }

        #region ITextureDescriptor Members

        public override TxMatrix GetDescription(TxImage image)
        {
            if (frecuencias == null)
                throw new InvalidOperationException("No estan definidas las frecuencias por las cuales se va a aplicar Gabor");
            double xPrima, yPrima, argSinusoide;
            double theta = Math.PI * currentAngle / orientaciones;
            double S = .56 / frecuencias[currentFreq];
            TxMatrix gaborReal = new TxMatrix(Sx, Sy);

            for (int x = 0; x < Sx; x++)
                for (int y = 0; y < Sy; y++)
                {
                    xPrima = (x - Sx / 2 - 1) * Math.Cos(theta) + (y - Sy / 2 - 1) * Math.Sin(theta);
                    yPrima = (y - Sy / 2 - 1) * Math.Cos(theta) - (x - Sx / 2 - 1) * Math.Sin(theta);
                    argSinusoide = 2 * Math.PI * xPrima * frecuencias[currentFreq] + fase;

                    gaborReal[x, y] = (float)(Math.Exp(-.5 * (((xPrima * xPrima + radio * radio * yPrima * yPrima) / (S * S)))) * Math.Cos(argSinusoide));
                }
            TxMatrix simpleCell = TxMatrix.FromImage(image).Convolve(gaborReal);

            return simpleCell;
        }

        #endregion

        #region IEnumerator<TextureDescriptor> Members

        public override TextureDescriptor Current
        {
            get
            {
                if (currentFreq < 0)
                    throw new InvalidOperationException();
                return this;
            }
        }

        #endregion       

        #region IEnumerator Members

       

        public override bool MoveNext()
        {
            if (currentFreq == -2)
                throw new InvalidOperationException();
            if (currentFreq == -1)
            {
                currentAngle = currentFreq = 0;
                return true;
            }
            if (currentFreq != frecuencias.Length)
            {
                if (currentAngle == orientaciones - 1)
                {
                    currentAngle = 0;
                    return ++currentFreq != frecuencias.Length;
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
            currentAngle = currentFreq = -1;
        }

        #endregion

        [Parameter("Gabor Filter Angle", "Angulo del filtro GratingCell")]
        [IntegerInSequence(0, 180)]
        public int CurrentAngle
        {
            get
            {
                return 180 * currentAngle / orientaciones;
            }
        }

        [Parameter("Gabor Filter Frequency", "Frecuencia del filtro GratinCell")]
        [RealInRange(1.0f, float.MaxValue)]
        public float CurrentFrequency
        {
            get
            {
                return frecuencias[currentFreq];
            }
        }

        [Parameter("Frequencies", "Frecuencias del filtro de Gabor Simple")]
        public float[] Frequencies
        {
            get
            {
                return frecuencias;
            }
            set
            {
                frecuencias = value;
            }
        }

        [Parameter("Number of Azimuts", "Cantidad de orientaciones del filtro de Gabor Simple")]
        [IntegerInSequence(1, 180)]
        public int Orientations
        {
            get
            {
                return orientaciones;
            }
            set
            {
                orientaciones = value;
            }
        }
    }
}
