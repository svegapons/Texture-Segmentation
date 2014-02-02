using System;
using System.Collections.Generic;
using System.Text;


namespace TxEstudioKernel.Operators
{
    public class Clasificador
    {

        public Clasificador(Vector patron, string id)
        {
            this.patron = patron;
            idClase = id;
        }
        public Clasificador(double[] patron, string id) : this(new Vector(patron), id) { }
        
        /// <summary>
        /// El centroide de una clase
        /// </summary>
        Vector patron;

        /// <summary>
        /// El centroide de una clase
        /// </summary>
        public Vector Patron
        {
            get { return patron; }
            set { patron = value; }
        }
        /// <summary>
        /// El identificador de una clase
        /// </summary>
        string idClase;

        /// <summary>
        /// El identificador de una clase
        /// </summary>
        public string Id
        {
            get { return idClase; }
            set { idClase = value; }
        }
    }
}
