using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel;
using System.Collections;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    
    public abstract class TxCoocurrenceDescriptor : TextureDescriptor
    {
        protected int dimension=3;

        ///// <summary>
        ///// Dimension de la ventana
        ///// </summary>
        [Parameter("Windows size xy", "The  window size of operator")]
        [IntegerInSequence(1, int.MaxValue, 2)]
        public int Dimension
        {
            get { return dimension; }
            set { dimension = value; }
        }
        protected int distanciaX=1;

        ///// <summary>
        ///// Distancia de busqueda
        ///// </summary>
        [Parameter("X_Distance", "The x-distance  of operator")]
        [IntegerInSequence(1, int.MaxValue, 1)]
        public int DistanciaX
        {
            get { return distanciaX; }
            set { if(value<dimension)distanciaX = value; }
        }

        protected int distanciaY = 1;

        ///// <summary>
        ///// Distancia de busqueda
        ///// </summary>
        [Parameter("Y_Distance", "The y-distance  of operator")]
        [IntegerInSequence(1, int.MaxValue, 1)]
        public int DistanciaY
        {
            get { return distanciaY; }
            set { if (value < dimension)distanciaY = value; }
        }
       protected int direccion=0;

       ///// <summary>
       ///// Angulo de direccion.
       ///// </summary>
       [Parameter("Azimut", "The direction of  operator")]
       [IntegerInSequence(0, 135, 45)]
       public int Direccion
       {
           get { return direccion; }
           set { direccion = value; }
       }

       private int niveles = 16;

        [Parameter("Gray Level", "Niveles de grises de la imagen")]
        [IntegerInSequence(16, 32, 16)]
       public int Niveles
       {
           get { return niveles; }
           set { niveles = value; }
       }


        

        /// <summary>
        /// Halla la matriz de coocurrencia dado las coordenadas de un pixel central y las coordenadas de  la direccion.
        /// </summary>
        protected virtual MatrizCoocurrencia HallaMatriz(TxImage imagen, int xcentro, int ycentro, int movx, int movy,int niveles)
        {
            
            MatrizCoocurrencia matriz = new MatrizCoocurrencia(niveles);

            for (int j = ycentro - dimension / 2; j <= ycentro + dimension / 2; j++)
                for (int i = xcentro - dimension / 2; i <= xcentro + dimension / 2; i++)
                {
                    if (EstaEnVentana(xcentro, ycentro, i + movx, j + movy))
                    {
                        matriz.Incrementa(imagen[i, j, ColorChannel.Red], imagen[i + movx, j + movy, ColorChannel.Red]);
                    }
                    if (EstaEnVentana(xcentro, ycentro, i - movx, j - movy))
                    {
                        matriz.Incrementa(imagen[i, j, ColorChannel.Red],imagen[i - movx, j - movy, ColorChannel.Red] );
                    }
                }
            return matriz;
        }
        /// <summary>
        /// Halla el descriptor correspondiente.
        /// </summary>
        public override TxMatrix GetDescription(TxImage imagen)
        {
            MatrizCoocurrencia matriz;
            
            int movx=0;
            int movy=0;

            TxMatrix resultado = new TxMatrix(imagen.Height, imagen.Width);
            TxImage gris = imagen.ToGrayScale();

            #region Direcciones
            if (direccion == 0)
            {
                movx = distanciaX;
                movy = 0;
            }
            if (direccion == 45)
            {
                movx = distanciaX;
                movy = -distanciaY;
            }
            if (direccion == 90)
            {
                movx = 0;
                movy = distanciaY;
            }
            if (direccion == 135)
            {
                movx = distanciaX;
                movy = distanciaY;
            }
            #endregion
            float result = 0;

            for (int y=dimension/2;y<imagen.Height-dimension/2;y++)
                for (int x = dimension / 2; x < imagen.Width - dimension / 2; x++)
                {
                    matriz = HallaMatriz(gris,x, y, movx, movy,niveles);
                    
                    result=Estadistica(matriz)*255;
                    resultado[y, x] = result;

                }
            resultado = CopyRectangle(resultado, new System.Drawing.Point(dimension / 2, dimension / 2), new System.Drawing.Point(0, dimension/2), resultado.Width-dimension/2, dimension / 2);
            resultado = CopyRectangle(resultado, new System.Drawing.Point(0, dimension / 2), new System.Drawing.Point(0, 0), dimension / 2, resultado.Height );
            resultado = CopyRectangle(resultado, new System.Drawing.Point(0, resultado.Width  - dimension), new System.Drawing.Point(0, resultado.Width  - dimension / 2), dimension / 2, resultado.Height);
            resultado=CopyRectangle(resultado,new System.Drawing.Point (resultado.Height-dimension,0),new System.Drawing.Point (resultado.Height-dimension/2,0),resultado.Width,dimension/2);
            
            
            return resultado;

        }


        /// <summary>
        /// Verifica si las coordenadas de un punto se encuentran dentro de las dimensiones de la ventana
        /// </summary>
        protected bool EstaEnVentana(int xcentro, int ycentro, int x, int y)
        {
            if ((x < xcentro - dimension / 2) || (x > xcentro + dimension / 2)) return false;
            if ((y < ycentro - dimension / 2) || (y > ycentro + dimension / 2)) return false;
            return true;
        }

       

        protected int CantidadNiveles(TxImage imagen, int xcentro, int ycentro)
        {
            Hashtable niveles = new Hashtable(dimension * dimension);
            for(int j=ycentro-dimension/2;j<=ycentro+dimension/2;j++)
                for (int i = xcentro - dimension / 2; i <= xcentro + dimension / 2; i++)
                {
                    if (!niveles.ContainsKey(imagen[i, j, ColorChannel.Red])) niveles[imagen[i, j, ColorChannel.Red]] = 1;
                }
            return niveles.Count;
            
 
        }

        protected abstract float Estadistica(MatrizCoocurrencia matriz);


        
    }
}
