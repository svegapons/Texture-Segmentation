using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TxEstudioKernel.Operators
{
    public abstract class KMean
    {
        /// <summary>
        /// Arreglo de clases
        /// </summary>
        protected Clasificador[] clases;

        
        /// <summary>
        /// Tabla de clases que contiene como llave a la clase y como valor los objetos que pertenecen a esa clase
        /// </summary>
        protected TablaClases tabla;

        /// <summary>
        /// Verifica si dos arreglos de clases son iguales
        /// </summary>
        /// <remarks>Se utiliza para verificar si han cambiado los patrones de una iteracion del Kmean a otra</remarks>
        /// <returns>True si son iguales, Falso en otro caso</returns>
        protected bool Igual(Clasificador[] antigua, Clasificador[] nuevo)
        {
            if (antigua.Length != nuevo.Length) return false;
            for (int i = 0; i < antigua.Length; i++)
                for (int j = 0; j < nuevo.Length; j++)
                    if ((antigua[i]).Id == nuevo[j].Id)
                        if ((!antigua[i].Patron.Equals(nuevo[j].Patron))) return false;
                        else break;
            return true;

        }

        /// <summary>
        /// Recalcula los patrones de las clases
        /// </summary>
        /// <remarks>Dada la tabla de clases y los objetos pertenecientes a cada clase, recalcula los patrones de las clases para actualizarlos.</remarks>
        protected virtual Clasificador[] RecalcularPatrones()
        {
            int cont = 0;
            Vector patron;
            ArrayList lista = new ArrayList();//Variable temporal para almacenar la lista de objetos pertenecientes a una clase
            Clasificador[] resultado = new Clasificador[tabla.CantidadClases];//REsultado
            
            int total = 0;//Variable para conocer la cantidad de objetos pertenecientes a una clase

            foreach (Clasificador clasificador in tabla.tabla.Keys)
            {
                total = 0;
                lista = (ArrayList)tabla[clasificador];

                if (lista == null)
                {
                    resultado[cont] = clasificador;
                    cont++;
                    continue;
                }

                patron = new Vector(((IClasificable)lista[0]).Valor().Longitud);
                foreach (IClasificable obj in lista)
                {
                    patron = patron.Suma(obj.Valor());
                    total++;
                }
                patron = patron.Division(total);
                resultado[cont] = new Clasificador(patron, clasificador.Id);
                cont++;
            }
            return resultado;

        }

        public virtual double Error()
        {
            ////cambios ultimos
            //double[] resultadosClases = new double[clases.Length];
            //int contador = 0;
            double distMax=0;

            double dist=0;

            double result=0;
            double resultpar;
            int total=0;
            ArrayList lista = new ArrayList();//Variable temporal para almacenar la lista de objetos pertenecientes a una clase

            foreach (Clasificador clasificador in tabla.tabla.Keys)
            {
                lista = (ArrayList)tabla[clasificador];

                foreach (IClasificable obj in lista)
                {
                    dist = System.Math.Pow(obj.Distancia(clasificador.Patron.Valor()), 2);
                    if (distMax < dist) distMax = dist;
                    
                }
                

            }

            foreach (Clasificador clasificador in tabla.tabla.Keys)
            {
                
                resultpar = 0;
                total=0;
                lista = (ArrayList)tabla[clasificador];
                foreach (IClasificable obj in lista)
                {
                    dist=System.Math.Pow(obj.Distancia(clasificador.Patron.Valor()),2);
                    resultpar += dist/distMax;
                    //if (distMax < dist) distMax = dist;
                    total++;
                }
                result += resultpar /total;
                
            }
            return result;

        }

        /// <summary>
        /// Metodo que dado que las clases estan definidas clasifica un objeto
        /// </summary>
        public abstract string Clasifica(IClasificable objeto);
        

        /// <summary>
        /// Metodo que halla los patrones de las clases
        /// </summary>
        abstract public void HallaPatrones();
    }
}
