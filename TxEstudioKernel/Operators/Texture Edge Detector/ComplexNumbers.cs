using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.Operators.Texture_Edge_Detector
{
    public class ComplexNumber
    {
        float realPart;
        float imaginaryPart;

        public ComplexNumber(float real, float imaginary)
        {
            this.imaginaryPart = imaginary;
            this.realPart = real;
        }
        public ComplexNumber(float real)
        {
            this.imaginaryPart = 0;
            this.realPart = real;
        }

        public float RealPart
        {
            get
            {
                return realPart;
            }
            set
            {
                realPart = value;
            }
        }
        public float ImaginaryPart
        {
            get
            {
                return imaginaryPart;
            }
            set
            {
                imaginaryPart = value;
            }
        }
        public ComplexNumber Plus(ComplexNumber other)
        {
            return new ComplexNumber(this.realPart + other.RealPart, this.imaginaryPart + other.ImaginaryPart);
        }
        public ComplexNumber Minus(ComplexNumber other)
        {
            return new ComplexNumber(this.realPart - other.RealPart, this.imaginaryPart - other.ImaginaryPart);
        }
        public ComplexNumber Mult(float other)
        {
            return new ComplexNumber(other * this.realPart, other * this.imaginaryPart);
        }
        public ComplexNumber Mult(ComplexNumber other)
        {
            if (this.imaginaryPart == 0)
                return other.Mult(this.realPart);
            else if (other.ImaginaryPart == 0)
                return this.Mult(other.RealPart);
            else return new ComplexNumber(this.realPart * other.RealPart + ((-1) * this.imaginaryPart * other.ImaginaryPart), this.realPart * other.ImaginaryPart + this.imaginaryPart * other.RealPart);
        }
        public ComplexNumber Divide(ComplexNumber other)
        {
            if (other.ImaginaryPart != 0 || other.RealPart != 0)
                return new ComplexNumber((this.realPart * other.RealPart + this.imaginaryPart * other.ImaginaryPart) / (float)(Math.Pow(other.RealPart, 2) + (float)Math.Pow(other.ImaginaryPart, 2)), (float)(this.imaginaryPart * other.RealPart - this.realPart * other.ImaginaryPart) / ( (float)Math.Pow(other.RealPart, 2) + (float)Math.Pow(other.ImaginaryPart, 2)));
            else throw new ArgumentException("A Complex number is 0 in the division ");
        }

        public float Modulus
        {
            get
            {
                return (float)Math.Sqrt( realPart * realPart + imaginaryPart * imaginaryPart);
            }
        }

        public static ComplexNumber One
        {
            get
            {
                return new ComplexNumber(1);
            }
        }

    }
}
