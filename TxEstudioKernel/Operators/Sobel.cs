using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel.Operators
{
    public abstract class Sobel : TxOneBand
    {
        /// <summary>
        /// Mascara utilizada para hallar la derivada con respecto al eje X
        /// </summary>
        protected TxMatrix mascaraX;
        /// <summary>
        /// Mascara utilizada para hallar la derivada con respecto al eje Y
        /// </summary>
        protected TxMatrix mascaraY;
        /// <summary>
        /// Umbral con el cual se van a comparar los valores para determinar si  son bordes o no
        /// </summary>
        protected float t;

        /// <param name="t">Umbral para la clasificacion de bordes</param>
        public Sobel()
            : base()
        {
            mascaraX = new TxMatrix(3,3);
            mascaraX[0, 0] = mascaraX[2, 0] = -1;
            mascaraX[1, 0] = -2;
            mascaraX[0, 1] = mascaraX[1, 1] = mascaraX[2, 1] = 0;
            mascaraX[0, 2] = mascaraX[2, 2] = 1;
            mascaraX[1, 2] = 2;

            mascaraY = new TxMatrix(3, 3);
            mascaraY[0, 0] = mascaraY[0, 2] = -1;
            mascaraY[0, 1] = -2;
            mascaraY[1, 0] = mascaraY[1, 1] = mascaraY[1, 2] = 0;
            mascaraY[2, 0] = mascaraY[2, 2] = 1;
            mascaraY[2, 1] = 2;

            this.t = 0;


        }

        [Parameter("Threshold", "This is the Sobel Threshold")]
        [RealInRange(0.0f, 255.0f)]
        public float Umbral
        { get { return t; } set { t = value; } }

      
        /// <summary>
        /// Halla la derivada con respecto al eje X
        /// </summary>
        /// <remarks>Realiza una convolucion con la mascaraX, que da como resultado la derivada direccional en el eje X</remarks>
        /// <param name="imagen">Imagen a la que se le va a realizar el proceso</param>
        /// <returns>Derivada con respecto al eje X</returns>
        public TxImage DirectionX(TxImage imagen)
        {
            return imagen.Convolve(mascaraX);

        }

        /// <summary>
        /// Halla la derivada con respecto al eje X
        /// </summary>
        /// <remarks>Utiliza la mascaraY para realizar una convolucion sobre la imagen y obtener la derivada con respecto al eje Y</remarks>
        /// <returns>La derivada con respecto al eje Y</returns>
        public TxImage DirectionY(TxImage imagen)
        {
            return imagen.Convolve(mascaraY);
        }

       
        
        /// <summary>
        /// La implementacion que se realiza de la funcion Color es la que diferencia los metodos de Sobel
        /// </summary>
        /// <param name="gx">Derivarda con respecto al eje X</param>
        /// <param name="gy">Derivada con respecto al eje Y</param>
        /// <param name="x">x pos</param>
        /// <param name="y">y pos</param>
        /// <returns>El color del pixel</returns>
        public abstract float Color(TxImage gx, TxImage gy, int x, int y);


        public override TxImage Process(TxImage input)
        {
            TxImage imagen = input.ToGrayScale();
            TxImage resultado = new TxImage(imagen.Width, imagen.Height, TxImageFormat.GrayScale);
            TxImage gx = new TxImage(imagen.Width, imagen.Height, TxImageFormat.GrayScale);
            TxImage gy = new TxImage(imagen.Width, imagen.Height, TxImageFormat.GrayScale);
            double color;


            //hallando las derivadas direccionales.
            gx = DirectionX(imagen);
            gy = DirectionY(imagen);


            for (int j = 0; j < imagen.Height; j++)
                for (int i = 0; i < imagen.Width; i++)
                {
                    //color =Math.Sqrt(Math.Pow(((double)gx[i, j, ColorChannel.Red]), 2)+Math.Pow(((double)gy[i,j,ColorChannel.Red]),2));

                    color = Color(gx, gy, i, j);

                    //Poniendo los colores de la imagen entre 0 y 255;
                    if (color < 0) color = 0;
                    if (color > 255) color = 255;

                    //Si el color  es menor que el humbral el color del pixel devuelto devuelta es el de la imagen original
                    //si es mayor el valor del pixel resultante va a ser el color.
                    if (color < t) color = imagen[i, j, ColorChannel.Red];


                    resultado[i, j] = (byte)color;

                }

           

            return resultado;

        }
    }
}
