using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TxEstudioKernel.Operators
{
    class FuzzyKMeans : Kmean_Batch_Update
    {
        float[,] probabilidades;//Probabilidades de pertenencia de cada elemento a la clase.

        
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

        private int b;

        protected int B
        {
            get { return b; }
            set 
            {
                if (b < 2) throw new Exception();
                else b = value;
                 
            }
        }

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
        public FuzzyKMeans(IClasificable[] muestra, int numeroClases, int cantIteraciones,int b)
            : base()
        {
            
            this.muestra = muestra;
            this.numeroClases = numeroClases;
            this.cantIteraciones = cantIteraciones;
            this.b = b;
            Clases = new Clasificador[numeroClases];
            tabla = new TablaClases();
            probabilidades=new float[muestra.Length,numeroClases];
        }

        //Inicializa las clases y las probabilidades normalizadas
        protected virtual void InicializaClases()
        {
            //hashtable que utilizo para detectar si existen patrones para mas de una clase.
            Hashtable patrones = new Hashtable();
            Random random = new Random();
            int pos = 0;//Ir llevando la posicion de la clase a la que se le esta poniendo el valor
            int poselem = 0;//Posicion del elemento dentro de la muestra que va a ser convertido en el centro de la clase
            int[] posiciones = new int[clases.Length];//Se almacena las posiciones de los elementos que han sido seleccionados centroides para actualizar sus probabilidades.
            //Seleccionado los centroides aleatorios
            for (int i = 0; i < muestra.Length; i++)
            {
                poselem = random.Next();
                if (poselem > muestra.Length) poselem = poselem % muestra.Length;

                if (patrones[muestra[poselem].Valor().Norma()] == null)
                {
                    Clases[pos] = new Clasificador(muestra[poselem].Valor(), pos.ToString());
                    posiciones[pos] = poselem;
                    pos++;
                    if (pos == Clases.Length) break;
                    patrones[muestra[poselem].Valor().Norma()] = true;
                                        
                }

            }
           

                           
        }
        //Recalcula los centroides.
        protected virtual Clasificador[] RecalculaCentroides()
        {
            Vector numerador;
            float denominador;
            Vector patron;
            Clasificador[] result=new Clasificador [clases.Length];

            //Recorriendo las clases
            for (int j = 0; j < clases.Length; j++)
            {
                numerador=new Vector(muestra[0].Valor().Longitud) ;
                denominador=0;
                //Recorriendo los elementos 
                for (int i = 0; i < muestra.Length; i++)
                {
                    numerador=numerador.Suma(muestra[i].Valor().Producto(System.Math.Pow((double)probabilidades[i,j],(double)b)));
                    denominador +=(float)System.Math.Pow(probabilidades[i, j],b);
                }
                patron=numerador.Division((double)denominador);
                result[j] = new Clasificador(patron.Valor(), j.ToString());

            }
            return result;
 
        }

        //Recalcula las probabilidades
        protected virtual void RecalculaProbabilidades()
        {
            float numerador;
            float denominador;
            float distancia;
            float distanciaTemp;

            for (int i = 0; i < muestra.Length; i++)
            {
                numerador = 0;
                denominador = 0;

                for (int k = 0; k < clases.Length; k++)
                {
                    distanciaTemp = (float)(clases[k].Patron.Distancia(muestra[i].Valor()));
                    if (distanciaTemp == 0) distanciaTemp = 0.001f;
                    denominador += (float)(System.Math.Pow((double)(1.0 / distanciaTemp), (double)(1.0 / (b - 1))));
                }
                for (int j = 0; j < clases.Length; j++)
                {
                    distancia = (float)(clases[j].Patron.Distancia(muestra[i].Valor()));
                    if (distancia == 0) distancia = 0.001f;
                    numerador = (float)(System.Math.Pow((double)(1.0 / distancia), (double)(1.0 / (b - 1))));
                    
                    probabilidades[i, j] = numerador / denominador;
                    
                    
                }
            }
 
        }
        protected void Clusteriza()
        {
            int indice = 0;
            for (int i = 0; i < clases.Length; i++)
            {
                tabla.Inserta(clases[i],clases[i].Patron);
            }
            for (int i = 0; i < muestra.Length; i++)
            {
                indice = IndiceClase(i);
                tabla.Inserta(clases[indice], muestra[i]);
            }

        }
        protected int IndiceClase(int indiceMuestra)
        {
            int posMin=0;
            double distanciaMin=double.MaxValue;

            for(int i=0;i<probabilidades.GetLength(1);i++)
                if (clases[i].Patron.Distancia(muestra[indiceMuestra].Valor()) < distanciaMin)
                {
                    posMin = i;
                    distanciaMin=clases[i].Patron.Distancia(muestra[indiceMuestra].Valor());
                }
            return posMin;
        }
        public override void HallaPatrones()
        {
            int iterActual = 0;
            Clasificador[] nuevasClases;

            InicializaClases();
            RecalculaProbabilidades();

            while (iterActual < cantIteraciones)
            {
                tabla.Clean();
                nuevasClases = RecalculaCentroides();
                
                 
                if (Igual(nuevasClases, Clases)) break;
                else
                {
                    Clases = nuevasClases;
                    RecalculaProbabilidades();
                    iterActual++;

                }
            }


        }
    }
}
