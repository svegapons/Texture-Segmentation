using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.Operators
{
    public class KMeanSupervisado : Kmean_Batch_Update
    {
        /// <param name="tabla">Tabla de clases que contiene a las clases con los objetos que pertenecen a ella</param>
        public KMeanSupervisado(TablaClases tabla)
            : base()
        {
            this.tabla = tabla;
            clases = new Clasificador[tabla.CantidadClases];

        }
        /// <summary>
        /// Halla los patrones de las clases
        /// </summary>
        /// <remarks>Utiliza la clasificacion de los objetos expresadas en la tabla de clases</remarks>
        public override void HallaPatrones()
        {
            clases = RecalcularPatrones();
        }
    }
}
