using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TxEstudioKernel.Operators
{
    public class TablaClases
    {
        //Estructura que utilizare para almacenar los objetos que se encuentran en cada clase.
        //La idea es tener un diccionario donde las llaves son Clasificadores(representan a la clase) y los valores son un ArrayList de IClasificables( que son los objetos pertenecientes a esa clase).

        /// <summary>
        /// Cantidad de clases
        /// </summary>
        int cantClases;

        public int CantidadClases
        {
            get { return cantClases; }
            set { cantClases = value; }
        }
        /// <summary>
        /// Hastable donde se van a almacenar las clases con su patron
        /// </summary>
        internal Hashtable tabla;

        /// <summary>
        /// Crea una tabla de clases
        /// </summary>
        /// <remarks>Se crea una tabla vacia con cantidad de clases 0</remarks>
        public TablaClases()
        {
            tabla = new Hashtable();
            cantClases = 0;
        }
        //Dado un Clasificador devuelve la coleccion de objetos que pertenecen a esa clase
        public ArrayList this[Clasificador clasificador]
        {
            get
            {
                return (ArrayList)tabla[clasificador];
            }
        }

        /// <summary>
        /// Inserta el objeto Clasificable en la tabla de clases
        /// </summary>
        /// <remarks>Si la tabla no contiene al Clasificador, inserta al clasificador y le pone el objeto clasificable; si ya estaba lo que hace es adicional  a la coleccion el objeto</remarks>
        /// <param name="clasificador">Clasificador que representa a una clase</param>
        /// <param name="objeto">Objeto que pertenece a la clase</param>
        public void Inserta(Clasificador clasificador, IClasificable objeto)
        {
            if (tabla[clasificador] == null)
            {
                tabla[clasificador] = new ArrayList();
                cantClases++;
            }
                ((ArrayList)tabla[clasificador]).Add(objeto);
        }

        /// <summary>
        /// Limpia la tabla
        /// </summary>
        public void Clean()
        {
            tabla = new Hashtable();
            cantClases = 0;
        }
    }
}
