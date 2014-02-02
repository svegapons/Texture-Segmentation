using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Haralick", "Calculates the Haralick edge feature using a square Window of size 3x3 and a contrast specified.")]
    [EdgeDetector]
    [Abbreviation("haral", "Rho")]
    public class HaralickEdgeDetector:TxOneBand
    {
        private static int winsize=5;


        public HaralickEdgeDetector()
        {
        }
        public override TxImage Process(TxImage input)
        {
            TxImage image=input;
            if (input.ImageFormat == TxImageFormat.RGB)
                image = input.ToGrayScale();
            TxMatrix floatImage = TxMatrix.FromImage(image);

            TxMatrix[] Wi = new TxMatrix[10];
                //Wi = new float[10][,];
                int a = (winsize - 1) / 2;
                int b = (winsize - 1) / 2;
            //int s1=0,s2=0;
                for (byte i = 0; i < 10; i++)
                {
                    Wi[i] = new TxMatrix(winsize, winsize);
                }

            //float [] s=new float[9];
            float []s1=new float[10];

            for (int i = 0; i < winsize; i++)
                for (int j = 0; j < winsize; j++)
                {
                    Wi[0][i, j] = g0(i - a, j - b);
                    s1[0] += Wi[0][i, j] * Wi[0][i, j];

                    Wi[1][i, j] = g1(i - a, j - b);
                    s1[1] += Wi[1][i, j] * Wi[1][i, j];

                    Wi[2][i, j] = g2(i - a, j - b);
                    s1[2] += Wi[2][i, j] * Wi[2][i, j];

                    Wi[3][i, j] = g3(i - a, j - b);
                    s1[3] += Wi[3][i, j] * Wi[3][i, j];

                    Wi[4][i, j] = g4(i - a, j - b);
                    s1[4] += Wi[4][i, j] * Wi[4][i, j];

                    Wi[5][i, j] = g5(i - a, j - b);
                    s1[5] += Wi[5][i, j] * Wi[5][i, j];

                    Wi[6][i, j] = g6(i - a, j - b);
                    s1[6] += Wi[6][i, j] * Wi[6][i, j];

                    Wi[7][i, j] = g7(i - a, j - b);
                    s1[7] += Wi[7][i, j] * Wi[7][i, j];

                    Wi[8][i, j] = g8(i - a, j - b);
                    s1[8] += Wi[8][i, j] * Wi[8][i, j];

                    Wi[9][i, j] = g9(i - a, j - b);
                    s1[9] += Wi[9][i, j] * Wi[9][i, j];
                }
            for (int i = 0; i < winsize; i++)
                for (int j = 0; j < winsize; j++)
                {
                    for (byte k = 0; k < 10; k++)
                        Wi[k][i, j] /= s1[k];
                }

            TxMatrix[] Ki = new TxMatrix[10];
            for (byte i = 0; i < 10; i++)
                //Ki[i] = Convolve(image, Wi[i]);
                Ki[i] = floatImage.Convolve(Wi[i]);

            //float[][,] ki=new float[10][,];
            TxMatrix[] ki = new TxMatrix[10];
            for (byte k = 0; k < 10; k++)
                ki[k] = new TxMatrix(image.Height, image.Width);

            for(int y=0;y<image.Height;y++)
                for(int x=0;x<image.Width;x++)
                {
                    ki[0][y, x] = Ki[0][y, x] - 2.0f * Ki[3][y, x] - 2.0f * Ki[5][y, x];
                    ki[1][y, x] = Ki[1][y, x] - (17.0f / 5.0f) * Ki[6][y, x] - 2.0f * Ki[8][y, x];
                    ki[2][y, x] = Ki[2][y, x] - (17.0f / 5.0f) * Ki[9][y, x] - 2.0f * Ki[7][y, x];

                    for (byte i = 3; i < 10; i++)
                        ki[i][y, x] = Ki[i][y, x];
                }

            //float L2=2*2;
            TxImage output = new TxImage(input.Width, input.Height,TxImageFormat.GrayScale);
            for(int y=0;y<image.Height;y++)
                for(int x=0;x<image.Width;x++)
                {

                    //float A = L2 * ki[6][y, x] + (1.0f / 3.0f) * L2 * ki[8][y, x] + ki[1][y, x];
                    //float B = L2 * ki[9][y, x] + (1.0f / 3.0f) * L2 * ki[7][y, x] + ki[2][y, x];
                    //float theta = Convert.ToSingle(Math.Atan2(B, A));
                    //float rho = Convert.ToSingle(Math.Sqrt(A * A + B * B));
                    float theta = Convert.ToSingle(Math.Atan2(ki[1][y, x], ki[2][y, x]));
                    float rho = Convert.ToSingle(Math.Sqrt(ki[1][y, x] * ki[1][y, x] + ki[2][y, x] * ki[2][y, x]));


                    float sin = Convert.ToSingle(Math.Sin(theta));
                    float cos = Convert.ToSingle(Math.Cos(theta));
                    //float sin = ki[1][y,x] / rho;
                    //float cos = ki[2][y,x] / rho;

                    float C3 = ki[6][y, x] * sin * sin * sin + ki[7][y, x] * sin * sin * cos + ki[8][y, x] * sin * cos * cos + ki[9][y, x] * cos * cos * cos;
                    float C2 = ki[3][y, x] * sin * sin + ki[4][y, x] * sin * cos + ki[5][y, x] * cos * cos;

                    if (C3 < 0 && Math.Abs(C2 / (3.0f * C3)) <= Rho)
                        output[x, y] = 255;
                    else output[x, y] = 0;
                }
            return output;

        }

        private float rho=0.5f;
        [Parameter("Contrast","")]
        [RealInRange(0,0.5f)]
        public virtual float Rho
        {
            get { return rho; }
            set { rho = value; }
        }

        private float g0(int r, int c)
        {
            return 1;
        }
        private float g1(int r, int c)
        {
            return r;
        }
        private float g2(int r, int c)
        {
            return c;
        }
        private float g3(int r, int c)
        {
            return r * r - 2;
        }
        private float g4(int r, int c)
        {
            return r * c;
        }
        private float g5(int r, int c)
        {
            return c * c - 2;
        }
        private float g6(int r, int c)
        {
            return r * r * r - r * 17.0f / 5.0f;
        }
        private float g7(int r, int c)
        {
            return (r * r - 2) * c;
        }
        private float g8(int r, int c)
        {
            return r * (c * c - 2);
        }
        private float g9(int r, int c)
        {
            return c * c * c - c * 17.0f / 5.0f;
        }

    }
}
