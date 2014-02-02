using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TxEstudioKernel.Operators
{
    public class KMeanNoSupervMengHeeHeng : KMeanNoSupervisado
    {
        int contadorClases;//cantador para llevar la cantidad de clases que tengo hasta entonces
        public KMeanNoSupervMengHeeHeng(IClasificable[] muestra):base(muestra, 100, 100)
        {
            clases = new Clasificador[2];
            contadorClases = 0; 
        }

        public override void HallaPatrones()
        {
            Clasificador[] nuevasclases;
            InicializaClases();
            

            int cont = 0;

            while (cont != contadorClases)
            {
                ClasificaMuestras();
                clases=RecalcularPatrones();
                cont = contadorClases;
                nuevasclases = NuevaClase();
                //if (contadorClases > numeroClases) { contadorClases--; break; } 
                //else clases = nuevasclases;
                clases = nuevasclases;
  
            }
        }

        public override string Clasifica(IClasificable objeto)
        {
            double distAnterior = 100000000000000;//Como valor inicial poner un numero lo suficientemente grande.
            int pos = 0;

            for (int i = 0; i < contadorClases; i++)
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

        //Este metodo lo que hace es buscar los dos puntos mas distantes y convertirlos en los centroides de las dos primeras clases. El criterio de mas distantes seran los que tengan mayor y menor norma.
        protected override void InicializaClases()
        {
            IClasificable menor;
            IClasificable mayor;
            IClasificable temp;

            //Inicializando el mayor y el menor
            if (muestra[0].Valor().Norma() > muestra[1].Valor().Norma())
            {
                menor = muestra[1];
                mayor = muestra[0];
            }
            else 
            {
                menor = muestra[0];
                mayor = muestra[1];
            }

            //Ciclo para encontrar el mayor valor y el menor valor
            for (int i = 2; i < muestra.Length; i++)
            {
                temp = muestra[i];
                if (temp.Valor().Norma() > mayor.Valor().Norma()) mayor = temp;
                else
                    if (temp.Valor().Norma() < menor.Valor().Norma()) menor = temp;
            }
            clases[0] = new Clasificador(menor.Valor(), "0");
            clases[1] = new Clasificador(mayor.Valor(), "1");
            tabla.Inserta(clases[0], menor);
            tabla.Inserta(clases[1], mayor);
            contadorClases = 2;


        }

        //Clasifica las muestras a partir de las clases existentes hasta entonces.
        protected override void ClasificaMuestras()
        {
            double mejordistancia;
            int mejorclase;
            tabla.Clean();

            for (int i = 0; i < muestra.Length; i++)
            {
               
                mejordistancia = 1000000000;
                mejorclase = 0;

                for (int j = 0; j < contadorClases; j++)
                {
                    if (muestra[i].Distancia(clases[j].Patron) < mejordistancia)
                    {
                        mejordistancia = muestra[i].Distancia(clases[j].Patron);
                        mejorclase = j;

                    }


                }

                tabla.Inserta(clases[mejorclase], muestra[i]);

            }
        }

        //Halla el patron de la nueva clase
        protected virtual Clasificador[] NuevaClase()
        {
            Clasificador[] result=null;
            ArrayList info=PuntoMasAlejado();
            IClasificable punto = (IClasificable)info[0];
            //double d = DistanciaAlCluster(punto);
            double d = (double)info[1];
            double q = MediaDistanciaEntreCluster();

            if (d > q / 2)
            {
                result = new Clasificador[contadorClases+1];
                for (int i = 0; i < contadorClases; i++)
                    result[i] = clases[i];
                result[contadorClases] = new Clasificador(punto.Valor(), contadorClases.ToString());
                contadorClases++;
            }
            else result = clases;
            return result;
        }

        //Devuelve el punto que se encuentra mas alejado del centro de algun cluster
        protected virtual ArrayList PuntoMasAlejado()
        {
            ArrayList resultado=new ArrayList(2);//punto mas alejado
            double mayordistancia=-1000000000000;//valor de la mayor distancia parcial
            ArrayList lista = new ArrayList();

            foreach (Clasificador clasificador in tabla.tabla.Keys)
            {
                lista = (ArrayList)tabla[clasificador];
                //Recorre los elementos de cada clase
                foreach (IClasificable obj in lista)
                {
                    
                    if (obj.Distancia(clasificador.Patron) > mayordistancia)
                    {
                        mayordistancia = obj.Distancia(clasificador.Patron);
                        if (resultado.Count > 0)
                        { 
                            resultado.RemoveAt(0);
                            resultado.RemoveAt(0);
                        }
                        resultado.Insert(0, obj);
                        resultado.Insert(1, mayordistancia);
                    }
                }
                
            }
            return resultado;
                     
        }

       
        //Halla el promedio de las distancia entre todo par de centroides
        protected virtual double MediaDistanciaEntreCluster()
        {
            double distancia = 0;
            int cont = 0;

            for(int i=0;i<contadorClases;i++)
                for(int j=i+1;j<contadorClases;j++)
                {
                    distancia+=clases[i].Patron.Distancia(clases[j].Patron);
                    cont++;
                }
            return distancia / cont;
                    
        }

        public int CantidadClases
        {
            get { return clases.Length; }
        }

    }
}
