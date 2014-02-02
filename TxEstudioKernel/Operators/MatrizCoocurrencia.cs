using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TxEstudioKernel.Operators
{
    public class MatrizCoocurrencia
    {
        
        int[,] matriz;
        int[] posiciones;
        

        

        /// <param name="graylevels">Cantidad de niveles de gris que aparecen en la ventana</param>
        public MatrizCoocurrencia(int graylevels)
        {
            
            matriz = new int[graylevels, graylevels];
            posiciones = new int[256];
            

            for (int i = 0; i < posiciones.Length; i++)
                posiciones[i] = graylevels * i / 256;

            
            
        }


        #region Metodos publicos

        /// <summary>
        /// Incrementa la el valor de la coocurrencia de dos niveles de gris
        /// </summary>
        /// <param name="tonoGris1">Tono de gris</param>
        /// <param name="tonoGris2">Tono de gris</param>
        public void Incrementa(float tonoGris1, float tonoGris2)
        {
            int posx;
            int posy;

            

            posx = posiciones[(int)tonoGris1];
            posy = posiciones[(int)tonoGris2];

            //Incrementa el valor de la coocurrencia en la matriz
            matriz[posx, posy]++;
        }

        //Dado dos niveles de grises da las coocurrencias de estos que esta guardada en la matriz
        public float this[float tonoGris1, float tonoGris2]
        {
            get 
            {
               
                int posx = posiciones[(int)tonoGris1];
                int posy = posiciones[(int)tonoGris2];
                return matriz[posx, posy]; 
            }
            set 
            {
               
                int posx = posiciones[(int)tonoGris1];
                int posy = posiciones[(int)tonoGris2];
                matriz[posx, posy]=(int)value; 
            }
        }

       
        public int GrayLevels
        {
            get { return matriz.GetLength(0); }
        }

        //dado un nivel de gris devuelve la posicion que ocupa en la matriz de coocurrencia.
        public int PosGrayLevel(float gray)
        {
            return posiciones[(int)gray];
        }
       
       

        public int Width
        {
            get { return matriz.GetLength(0); }
        }
        public int Heigth
        {
            get { return matriz.GetLength(1); }
        }

        /// <summary>
        /// Normaliza la matriz de coocurrencia
        /// </summary>
        /// <remarks>Despues de llamar este metodo el campo matriz ya esta normalizado.</remarks>
        public float[,] Normaliza()
        {
            float[,] result = new float[matriz.GetLength(0), matriz.GetLength(1)];
            
            int totalEntradas = 0;

            for (int j = 0; j < matriz.GetLength(1); j++)
                for (int i = 0; i < matriz.GetLength(0); i++)
                    totalEntradas += matriz[i, j];

            for (int j = 0; j < matriz.GetLength(1); j++)
                for (int i = 0; i < matriz.GetLength(0); i++)
                    result[i, j] = (float)matriz[i, j] / (float)totalEntradas;

            return result;
        }

        #endregion

        
        
        
    }
}
