using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel.Operators.Texture_Edge_Detector
{
    [Algorithm("1D Gabor by rows", "Calculates the unidimensional gabor filter on an image by its rows")]
    [GaborDescriptor]
    [Abbreviation("ugr_", "Sigma", "Omega")]
    public class UnidimensionalGaborByRows:TextureDescriptor
    {
        float sigma = 2.0f;
        [Parameter("Sigma", "Esta es la sigma")]
        public float Sigma
        {
            get { return sigma; }
            set 
            {
                sigma = value;
            
            }
        }

        float omega = (float)Math.PI/2.0f;
        [Parameter("Omega", "Esta es la omega")]
        public float Omega
        {
            get { return omega; }
            set { omega = value; }
        }

        float scale;
        float q;

        float m0_Value = 1.16680f;
        float m1_Value = 1.10783f;
        float m2_Value = 1.40586f;

        //float b0 = 1;
        float b1;
        float b2;
        float b3;
        float B;

        public UnidimensionalGaborByRows()
        {
            CalculateConstants();
        }

        #region Initialization
        private void CalculateConstants()
        {
            Calculate_q();
            Calculate_scale();
            Calculate_b1();
            Calculate_b2();
            Calculate_b3();
            Calculate_B();
            Calculate_ComplexConstants();
        }

        ComplexNumber cf1;
        ComplexNumber cf2;
        ComplexNumber cf3;
        ComplexNumber cf_123_denominator;

        ComplexNumber cb1;
        ComplexNumber cb2;
        ComplexNumber cb3;
        ComplexNumber cb_123_denominator;

        private void Calculate_ComplexConstants()
        {
            //Forward
             cf1 = new ComplexNumber((float)Math.Cos(this.omega), (float)Math.Sin(omega));
             cf2 = new ComplexNumber((float)Math.Cos(2 * this.omega), (float)Math.Sin(2 * this.omega));
             cf3 = new ComplexNumber((float)Math.Cos(3 * this.omega), (float)Math.Sin(3 * this.omega));
             cf_123_denominator = ComplexNumber.One.Plus(cf1.Mult(b1)).Plus(cf2.Mult(b2)).Plus(cf3.Mult(b3));

            //Backward
              cb1 = new ComplexNumber((float)Math.Cos(this.omega * (-1)), (float)Math.Sin(omega * (-1)));
              cb2 = new ComplexNumber((float)Math.Cos(2 * this.omega * (-1)), (float)Math.Sin(2 * this.omega * (-1)));
              cb3 = new ComplexNumber((float)Math.Cos(3 * this.omega * (-1)), (float)Math.Sin(3 * this.omega * (-1)));
              cb_123_denominator = ComplexNumber.One.Plus(cb1.Mult(b1)).Plus(cb2.Mult(b2)).Plus(cb3.Mult(b3));
        }

        private void Calculate_q()
        {
            if (sigma < 3.556)
                q = -0.2568f + (0.5784f * sigma) + (0.0561f * (float)Math.Pow(sigma, 2));
            else
                q = 2.5091f + 0.9804f * (sigma - 3.556f);
        }
        private void Calculate_scale()
        {
            scale=(m0_Value+q)*((float)Math.Pow(m1_Value,2)+ (float)Math.Pow(m2_Value,2)+2*m1_Value*q+ (float)Math.Pow(q,2));
        }
        private void Calculate_b1()
        {
            b1 = -q * (2 * m0_Value * m1_Value + (float)Math.Pow(m1_Value, 2) + (float)Math.Pow(m2_Value, 2) + (2 * m0_Value + 4 * m1_Value) * q + 3 * (float)Math.Pow(q, 2)) / scale;
        }
        private void Calculate_b2()
        {
            b2 = (float)Math.Pow(q,2)*(m0_Value+2*m1_Value+3*q) / scale;
        }
        private void Calculate_b3()
        {
            b3 = - (float)Math.Pow(q,3) / scale;
        }
        private void Calculate_B()
        {
            B = (float)Math.Pow(m0_Value * ( (float)Math.Pow(m1_Value, 2) + (float)Math.Pow(m2_Value, 2)) / scale, 2);
        }

        #endregion

        protected void Apply(TxMatrix matrix)
        {
            ComplexNumber[,] auxiliar = new ComplexNumber[matrix.Height, matrix.Width];

            unsafe
            {
                float* matrixData = (float*)(((_cvMat*)matrix.InnerMatrix)->data);
                //Forward
                for (int row = 0; row < matrix.Height; row++)
                {
                    auxiliar[row, 0] = auxiliar[row, 1] = auxiliar[row, 2] = new ComplexNumber(*matrixData).Divide(cf_123_denominator);
                    matrixData+=3;
                    for (int column = 3; column < matrix.Width; column++)
                    {
                        //*matrixData es el valor en la fila y columna correspondiente
                            auxiliar[row, column] = (new ComplexNumber(*matrixData)) .Minus 
                                                        ( 
                                                            ((cf1.Mult(b1)).Mult(auxiliar[row, column-1])).Plus
                                                                (
                                                                    ((cf2.Mult(b2)).Mult(auxiliar[row, column-2])).Plus
                                                                        ((cf3.Mult(b3)).Mult(auxiliar[row, column-3]))
                                                                )
                                                        );
                        matrixData++;
                    }
                }

                //Backward

                for (int row = matrix.Height-1 ; row >= 0; row--)
                {
                    auxiliar[row, matrix.Width -1] = auxiliar[row, matrix.Width - 2] = auxiliar[row, matrix.Width -3] = 
                        (auxiliar[row, matrix.Width-1].Mult(B)).Divide( cb_123_denominator);
                    matrixData -= 3;
                    for (int column = matrix.Width-4; column >= 0; column--)
                    {
                        //*matrixData es el valor en la fila y columna correspondiente
                        auxiliar[row, column] = (auxiliar[row, column].Mult(B)).Minus
                                                        (
                                                            ((cb1.Mult(b1)).Mult(auxiliar[row, column + 1])).Plus
                                                                (
                                                                    ((cb2.Mult(b2)).Mult(auxiliar[row, column + 2])).Plus
                                                                        ((cb3.Mult(b3)).Mult(auxiliar[row, column + 3]))
                                                                )
                                                        );
                        

                        matrixData--;
                    }
                }

                for (int row = 0; row < matrix.Height; row++)
                    for (int column = 0; column < matrix.Width; column++)
                    {
                        *matrixData = auxiliar[row, column].Modulus;
                        matrixData++;
                    }
            }    
        }

        #region TextureDescriptor   
    
            public override TxMatrix  GetDescription(TxImage image)
            {
                TxMatrix result=TxMatrix.FromImage(image);
                Apply(result);
                return result;
            }

        #endregion 


    }


    [Algorithm("1D Gabor by columns", "Calculates the unidimensional gabor filter on an image by its columns")]
    [GaborDescriptor]
    [Abbreviation("ugc_", "Sigma", "Omega")]
    public class UnidimensionalGaborByColumns:UnidimensionalGaborByRows
    {
        public override TxMatrix  GetDescription(TxImage image)
        {
            TxMatrix result = TxMatrix.FromImage(image).Transpose();
            Apply(result);
            return result.Transpose();
        }
    }
}
