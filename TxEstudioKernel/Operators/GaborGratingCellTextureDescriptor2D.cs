using System;
using System.Collections;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;
using System.Drawing;
using System.Reflection;

namespace TxEstudioKernel.Operators
{
	/// <summary>
	/// Summary description for TxGaborGratingCellFilter.
    /// </summary>
    [GaborDescriptor]
    [Algorithm("2DGabor GratingCell", "Descriptor de Textura GratingCell 2D")]
    [Abbreviation("gab_gcell", "CurrentFrequency", "CurrentAngle")]
    public class GaborGratingCellTextureDescriptor2D : TextureDescriptorSequence
    {
		int Sx = 21, Sy = 21;
        float[] frecuencias = new float[] { 1f / 4, 1f / 8, 1f / 16, 1f / 32 };
        int currentFreq;
		float fase, radio;
        int orientaciones = 8, currentAngle;
        float threshold, beta;
		
		public GaborGratingCellTextureDescriptor2D(int nOrientaciones, float[] frecuencias, float radio, float fase)
		{
            if (nOrientaciones < 0)
                throw new ArgumentException("Las orientaciones en el GaborEnergyTextureDescriptor no pueden ser negativas");
            if (frecuencias == null)
                throw new ArgumentNullException("El parametro frecuencias no puede ser null");
			this.radio = radio;
			this.frecuencias = frecuencias;
			this.orientaciones = nOrientaciones;
            this.fase = fase; 
            this.threshold = .9f;
            beta = 5;
            currentFreq = -1;
		}

		/// <summary>
		/// Constructor de la clase TxGaborEnergyFilter
		/// </summary>
		/// <param name="nOrientaciones">cantidad de orientaciones que se realizaran entre 0 y pi</param>
		/// <param name="frecuencias">Valores frecuenciales del filtrado</param>
		public GaborGratingCellTextureDescriptor2D(int nOrientaciones, float[] frecuencias)
        {
            if (nOrientaciones < 0)
                throw new ArgumentException("Las orientaciones en el GaborEnergyTextureDescriptor no pueden ser negativas");
            if (frecuencias == null)
                throw new ArgumentNullException("El parametro frecuencias no puede ser null");
			this.radio = 0.5f;
            this.threshold = .9f;
            beta = 5;
			this.frecuencias = frecuencias;
			this.orientaciones = nOrientaciones;
            this.fase = 0;
            currentFreq = -1;
		}

		/// <summary>
		/// Constructor de la clase TxGaborEnergyFilter
		/// </summary>
        public GaborGratingCellTextureDescriptor2D()
        {
			this.radio = 0.5f;
            this.threshold = .9f;
            beta = 5;
			//this.frecuencias = new float[]{2, 5, 10, 15};//new int[]{2, 4, 8, 16, 32};
			//this.orientaciones = 8;
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
            TxMatrix simpleCell = TxMatrix.FromImage(image).Convolve(gaborReal).Scale(0, 1);

            //Calcular matriz de pesos gaussianos de 7x7
            TxMatrix weights = new TxMatrix(7, 7);
            S = .56 / frecuencias[currentFreq];
            for (int x = 0; x < 7; x++)
                for (int y = 0; y < 7; y++)
                    weights[x, y] = (float)(Math.Exp(-.5 * (Math.Pow(x - 7 / 2 - 1, 2) + Math.Pow(y - 7 / 2 - 1, 2)) / Math.Pow(beta * S, 2)));

            return Activation_Stage(simpleCell).Convolve(weights);
        }

        #endregion

        TxMatrix Activation_Stage(TxMatrix input)
        {
            int width, height;
            double theta, cos, sin, max, min, maxTheta, maxThetaPi, minTheta, minThetaPi;
            int finalX, finalY, Lx, Ly, nextLx, nextLy, cLy;
            int[] epsilon = new int[7];
            int[] etha = new int[7];

            theta = Math.PI / orientaciones * currentAngle;
            cos = Math.Cos(theta);
            sin = Math.Sin(theta);
            width = input.Height;
            height = input.Width;

            TxMatrix activations = new TxMatrix(width, height);
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    Lx = (int)(-3 / (2 * frecuencias[currentFreq]) * cos + x);
                    Ly = (int)(-3 / (2 * frecuencias[currentFreq]) * sin + y);
                    maxTheta = maxThetaPi = double.MinValue;
                    minTheta = minThetaPi = double.MaxValue;
                    for (int k = -3; k < 3; k += 2)
                    {
                        nextLx = finalX = (int)((k + 1) / (2 * frecuencias[currentFreq]) * cos + x);
                        nextLy = finalY = (int)((k + 1) / (2 * frecuencias[currentFreq]) * sin + y);
                        if (cos < 0)
                        {
                            finalX = Lx;
                            Lx = nextLx;
                        }
                        if (sin < 0)
                        {
                            finalY = Ly;
                            Ly = nextLy;
                        }
                        max = double.MinValue;
                        min = double.MaxValue;
                        for (; Lx <= finalX; Lx++)
                        {
                            cLy = Ly;
                            for (; cLy <= finalY; cLy++)
                            {
                                if (Lx >= 0 && Lx < width && cLy >= 0 && cLy < height)
                                {
                                    if (input[Lx, Ly] > max)
                                        max = input[Lx, Ly];
                                    if (input[Lx, Ly] < min)
                                        min = input[Lx, Ly];
                                }

                            }
                        }
                        if (max > maxTheta)
                            maxTheta = max;
                        else if (max < minTheta)
                            minTheta = max;
                        if (min > maxThetaPi)
                            maxThetaPi = min;
                        else if (min < minThetaPi)
                            minThetaPi = min;
                        Lx = nextLx;
                        Ly = nextLy;
                        nextLx = finalX = (int)((k + 2) / (2 * frecuencias[currentFreq]) * cos + x);
                        nextLy = finalY = (int)((k + 2) / (2 * frecuencias[currentFreq]) * sin + y);
                        if (cos < 0)
                        {
                            finalX = Lx;
                            Lx = nextLx;
                        }
                        if (sin < 0)
                        {
                            finalY = Ly;
                            Ly = nextLy;
                        }
                        max = double.MinValue;
                        min = double.MaxValue;
                        for (; Lx <= finalX; Lx++)
                        {
                            cLy = Ly;
                            for (; cLy <= finalY; cLy++)
                            {
                                if (Lx >= 0 && Lx < width && cLy >= 0 && cLy < height)
                                {
                                    if (input[Lx, Ly] > max)
                                        max = input[Lx, Ly];
                                    if (input[Lx, Ly] < min)
                                        min = input[Lx, Ly];
                                }
                            }
                        }
                        if (min > maxTheta)
                            maxTheta = min;
                        else if (min < minTheta)
                            minTheta = min;
                        if (max > maxThetaPi)
                            maxThetaPi = max;
                        else if (max < minThetaPi)
                            minThetaPi = max;
                        Lx = nextLx;
                        Ly = nextLy;
                    }
                    activations[x, y] = 0;
                    if (threshold * maxTheta < minTheta)
                        activations[x, y] = 1;
                    if (threshold * maxThetaPi < minThetaPi)
                        activations[x, y] += 1;
                }
            return activations;
        }

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

        [Parameter("Current Gabor Filter Angle", "Angulo del filtro GratingCell")]
        [IntegerInSequence(0, 180)]
        public int CurrentAngle
        {
            get
            {
                return 180 * currentAngle / orientaciones;
            }
        }

        [Parameter("Current Gabor Filter Frequency", "Frecuencia del filtro GratingCell")]
        [RealInRange(1.0f, float.MaxValue)]
        public float CurrentFrequency
        {
            get
            {
                return frecuencias[currentFreq];
            }
        }

        [Parameter("Frequencies", "Frecuencias del filtro de Gabor GratingCell")]
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

        [Parameter("Number of Azimuts", "Cantidad de orientaciones del filtro de Gabor GratingCell")]
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
