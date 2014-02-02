using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.Operators
{
    public interface IClasificable
    {
        /// <returns>Devuelve el vector que caracteriza a un objeto del tipo IClasificable.</returns>
        Vector Valor();

        /// <summary>
        /// Distancia del objeto a un vector
        /// </summary>
        /// <remarks>Distancia del vector caracteristico de un objeto IClasificable a otro vector</remarks>
        /// <param name="patron">Vector con respecto al cual se quiere calcular la distancia</param>
        /// <returns>Distancia del objeto a un vector</returns>
        double Distancia(Vector patron);
    }
}
