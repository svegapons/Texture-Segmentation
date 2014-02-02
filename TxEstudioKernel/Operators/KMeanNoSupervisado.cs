using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TxEstudioKernel.Operators
{
    public class KMeanNoSupervisado : Kmean_Batch_Update
    {
        /// <summary>
        /// Muestra que se va utilizar para calcular los patrones de las clases
        /// </summary>
        protected IClasificable[] muestra;
        /// <summary>
        /// Numero de clases
        /// </summary>
        protected int numeroClases;
        /// <summary>
        /// Cantidad de iteraciones prefijadas.Cirterio de parada
        /// </summary>
        protected int cantIteraciones;

        public Clasificador[] Clases
        {
            get { return clases; }
            set { clases = value; }
        }


        public int CantidadIteraciones
        {
            get { return cantIteraciones; }
            set { cantIteraciones = value; }
        }

        /// <param name="muestra">Muestra</param>
        /// <param name="numeroClases">Numero de clases</param>
        /// <param name="cantIteraciones">Cantidad maxima de iteraciones</param>
        public KMeanNoSupervisado(IClasificable[] muestra, int numeroClases, int cantIteraciones)
            : base()
        {
            
            this.muestra = muestra;
            this.numeroClases = numeroClases;
            this.cantIteraciones = cantIteraciones;
            Clases = new Clasificador[numeroClases];
            tabla = new TablaClases();
        }

        /// <summary>
        /// Inicializa las clases
        /// </summary>
        /// <remarks>Inicializacion de los centriodes aleatoriamente</remarks>
        protected virtual void InicializaClases()
        {
            //hashtable que utilizo para detectar si existen patrones para mas de una clase.
            Hashtable patrones = new Hashtable();
            Random random = new Random();
            int pos = 0;
            int poselem = 0;
            for (int i = 0; i < muestra.Length; i++)
            {
                poselem = random.Next();
                if (poselem > muestra.Length) poselem = poselem % muestra.Length;

                if (patrones[muestra[poselem].Valor().Norma()] == null)
                {
                    Clases[pos] = new Clasificador(muestra[poselem].Valor(), pos.ToString());
                    pos++;
                    if(pos==Clases.Length)break;
                    patrones[muestra[poselem].Valor().Norma()] = true;

                }
 
            }
            
        }

        /// <summary>
        /// Clasifica las muestras
        /// </summary>
        protected virtual void ClasificaMuestras()
        {
            double mejordistancia;
            int mejorclase;

            //Para que inicialmente esten todas las clases insertadas en la tabla, asi me evito que por alguna razon se me deje de insertar alguna clase en la tabla.
            for (int i = 0; i < Clases.Length; i++)
            {
                tabla.Inserta(Clases[i],Clases[i].Patron);

            }

            for (int i = 0; i < muestra.Length; i++)
            {
                mejordistancia = 1000000000;
                mejorclase = 0;

                for (int j = 0; j < Clases.Length; j++)
                {
                    if (muestra[i].Distancia(Clases[j].Patron) < mejordistancia)
                    {
                        mejordistancia = muestra[i].Distancia(Clases[j].Patron);
                        mejorclase = j;

                    }


                }

                tabla.Inserta(Clases[mejorclase], muestra[i]);

            }

        }
       
        /// <summary>
        /// Halla los patrones de las clases a partir de la muestra
        /// </summary>
        public override void HallaPatrones()
        {
            int iterActual = 0;
            Clasificador[] nuevasClases;

            InicializaClases();

            while (iterActual < cantIteraciones)
            {
                tabla.Clean();
                ClasificaMuestras();

                nuevasClases = RecalcularPatrones();

                if (Igual(nuevasClases, Clases)) break;
                else
                {
                    Clases = nuevasClases;
                    iterActual++;
                    
                }
            }
        }
    }
}
