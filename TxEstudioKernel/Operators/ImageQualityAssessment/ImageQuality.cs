using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators.ImageQualityAssessment
{
    public abstract class ImageQuality:TxAlgorithm
    {
        //matriz para el mapeo
        protected TxMatrix mapa;

        //x-size del kernel para aplicar med,var,stdv
        protected int xSize = 3;
        [Parameter("Windows Size x", "The x size of the window")]
        [IntegerInSequence(3,int.MaxValue,2)]
        public int XSize
        {
            get { return xSize; }
            set { xSize = value; }
        }

        //y-size del kernel para aplicar med,var,stdv
        protected int ySize = 3;
        [Parameter("Windows Size y", "The y size of the window")]
        [IntegerInSequence(3, int.MaxValue, 2)]
        public int YSize
        {
            get { return ySize; }
            set { ySize = value; }
        }

        public TxMatrix Mapa
        { get { return mapa; } }

        public abstract double Error(TxImage imagen1, TxImage imagen2);

        protected TxMatrix StdvXY(TxImage imagen1, TxImage Imagen2,TxMatrix med1,TxMatrix med2)
        {
            if (imagen1.Width != Imagen2.Width && imagen1.Height != Imagen2.Height) throw new ArgumentException("Las imagenes deben tener las mismas dimensiones");

            TxImage gris1 = imagen1.ToGrayScale();
            TxImage gris2= Imagen2.ToGrayScale();
            double resultadoParcial = 0;
            double color;
            
            TxMatrix resultado = new TxMatrix(med1.Height, med1.Width);

            for (int j = ySize / 2; j < imagen1.Height - (ySize / 2); j++)
            {
                for (int i = xSize / 2; i < imagen1.Width - (xSize / 2); i++)
                {
                    //Para calcular el deviation
                    for (int y = -ySize / 2; y < ySize / 2 + 1; y++)
                        for (int x = -xSize / 2; x < xSize / 2 + 1; x++)
                        {
                            color = (gris1[i + x, j + y] - med1[j, i]) * (gris2[i + x, j + y] - med2[j, i]);
                            resultadoParcial += color;

                        }

                    resultadoParcial = resultadoParcial / (xSize * ySize - 1);
                    resultado[j, i] = (float)resultadoParcial;
                    resultadoParcial = 0;
                }
            }
            resultado = CopyRectangle(resultado, new System.Drawing.Point(xSize / 2, ySize / 2), new System.Drawing.Point(0, ySize / 2), resultado.Width - ySize / 2, xSize / 2);
            resultado = CopyRectangle(resultado, new System.Drawing.Point(0, ySize / 2), new System.Drawing.Point(0, 0), ySize / 2, resultado.Height);
            resultado = CopyRectangle(resultado, new System.Drawing.Point(0, resultado.Width - ySize), new System.Drawing.Point(0, resultado.Width - ySize / 2), ySize / 2, resultado.Height);
            resultado = CopyRectangle(resultado, new System.Drawing.Point(resultado.Height - xSize, 0), new System.Drawing.Point(resultado.Height - xSize / 2, 0), resultado.Width, xSize / 2);
            return resultado;

        }

        protected TxMatrix Var(TxImage imagen, TxMatrix med)
        {
            TxImage gris = imagen.ToGrayScale();
            double resultadoParcial = 0;
            double color;

                       
            TxMatrix resultado = new TxMatrix(med.Height, med.Width);

            for (int j = ySize / 2; j < imagen.Height - (ySize / 2); j++)
            {
                for (int i = xSize / 2; i < imagen.Width - (xSize / 2); i++)
                {
                    //Para calcular el deviation
                    for (int y = -ySize / 2; y < ySize / 2 + 1; y++)
                        for (int x = -xSize / 2; x < xSize / 2 + 1; x++)
                        {
                            color = (gris[i + x, j + y] - med[j, i]) * (gris[i + x, j + y] - med[j, i]);
                            resultadoParcial += color;

                        }

                    resultadoParcial = resultadoParcial / (xSize * ySize - 1);
                    resultado[j, i] = (float)resultadoParcial;
                    resultadoParcial = 0;
                }
            }
            resultado = CopyRectangle(resultado, new System.Drawing.Point(xSize / 2, ySize / 2), new System.Drawing.Point(0, ySize / 2), resultado.Width - ySize / 2, xSize / 2);
            resultado = CopyRectangle(resultado, new System.Drawing.Point(0, ySize / 2), new System.Drawing.Point(0, 0), ySize / 2, resultado.Height);
            resultado = CopyRectangle(resultado, new System.Drawing.Point(0, resultado.Width - ySize), new System.Drawing.Point(0, resultado.Width - ySize / 2), ySize / 2, resultado.Height);
            resultado = CopyRectangle(resultado, new System.Drawing.Point(resultado.Height - xSize, 0), new System.Drawing.Point(resultado.Height - xSize / 2, 0), resultado.Width, xSize / 2);
            return resultado;

        }
        protected TxMatrix Stdv(TxMatrix var)
        {
            TxMatrix resultado = new TxMatrix(var.Height, var.Width);

            for (int j = 0; j < var.Height; j++)
                for (int i = 0; i < var.Width; i++)
                    resultado[j, i] = (float)Math.Sqrt(var[j, i]);
            return resultado;
        }
        protected TxMatrix Stdv(TxImage imagen, TxMatrix med)
        {
            TxImage gris = imagen.ToGrayScale();
            double resultadoParcial = 0;
            double color;

            TxMatrix resultado = new TxMatrix(med.Height, med.Width);

            for (int j = ySize / 2; j < imagen.Height - (ySize / 2); j++)
            {
                for (int i = xSize / 2; i < imagen.Width - (xSize / 2); i++)
                {
                    //Para calcular el deviation
                    for (int y = -ySize / 2; y < ySize / 2 + 1; y++)
                        for (int x = -xSize / 2; x < xSize / 2 + 1; x++)
                        {
                            color = (gris[i + x, j + y] - med[j, i]) * (gris[i + x, j + y] - med[j, i]);
                            resultadoParcial += color;

                        }

                    resultadoParcial = Math.Sqrt(resultadoParcial) / (xSize * ySize - 1);
                    resultado[j, i] = (float)resultadoParcial ;
                    resultadoParcial = 0;
                }
            }
            resultado = CopyRectangle(resultado, new System.Drawing.Point(xSize / 2, ySize / 2), new System.Drawing.Point(0, ySize / 2), resultado.Width - ySize / 2, xSize / 2);
            resultado = CopyRectangle(resultado, new System.Drawing.Point(0, ySize / 2), new System.Drawing.Point(0, 0), ySize / 2, resultado.Height);
            resultado = CopyRectangle(resultado, new System.Drawing.Point(0, resultado.Width - ySize), new System.Drawing.Point(0, resultado.Width - ySize / 2), ySize / 2, resultado.Height);
            resultado = CopyRectangle(resultado, new System.Drawing.Point(resultado.Height - xSize, 0), new System.Drawing.Point(resultado.Height - xSize / 2, 0), resultado.Width, xSize / 2);
            return resultado;

        }
        protected virtual TxMatrix CopyRectangle(TxMatrix matriz, System.Drawing.Point verticeSup, System.Drawing.Point dest, int alto, int ancho)
        {
            TxMatrix result = matriz;

            for (int j = 0; j < alto; j++)
                for (int i = 0; i < ancho; i++)
                {
                    result[dest.X + i, dest.Y + j] = matriz[verticeSup.X + i, verticeSup.Y + j];
                }
            return result;

        }
        }

        
        
    }

