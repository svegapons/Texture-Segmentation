using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TxEstudioKernel.Operators
{
    public abstract class Kmean_Batch_Update : KMean
    {
        /// <summary>
        /// Clasifica un objeto
        /// </summary>
        /// <remarks>Dado que ya estan calculados los patrones de las clases en la tabla de Clases, busca cual es la clase que se encuentra  a menor distancia del objeto</remarks>
        /// <returns>El identificador de la clase a la que pertenece el objeto</returns>
        public override string Clasifica(IClasificable objeto)
        {
            double distAnterior = 100000000000000;//Como valor inicial poner un numero lo suficientemente grande.
            int pos = 0;

            for (int i = 0; i < clases.Length; i++)
            {
                //Buscando la clase mas cercana al punto
                if (objeto.Distancia(clases[i].Patron) < distAnterior)
                {
                    distAnterior = objeto.Distancia(clases[i].Patron);
                    pos = i;
                }
            }
            return clases[pos].Id;
        }
    }

       
}
