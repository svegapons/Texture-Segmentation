using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{

    [Algorithm("KMeans Method", "No Supervised image segmentation using the KMean Algorithm")]
    [Abbreviation("km", "NumeroClases", "MaxIteraciones", "NumeroMuestras", "NumeroPruebas")]
    public class KmeanNoSupervImagenes : TxSegmentationAlgorithm
    {
        double error;

        int numeroPruebas = 1;
        [Parameter("Test #", "")]
        [IntegerInSequenceAttribute(1, 100)]
        public int NumeroPruebas
        {
            get { return numeroPruebas; }
            set { numeroPruebas = value; }
        }

        int numeroMuestras = 6;//Porciento del total de pixeles de la imagen que van a ser tomados de muestra
        [Parameter("Sample %", "")]
        [IntegerInSequenceAttribute(1, 100)]
        public int NumeroMuestras
        {
            get
            {
                return numeroMuestras;
            }
            set
            {
                numeroMuestras = value;
            }
        }


        int numeroClases = 5;
        [Parameter("Class number", "")]
        public int NumeroClases
        {
            get
            {
                return numeroClases;
            }
            set
            {
                numeroClases = value;
            }
        }
        int maxIteraciones = 10;
        [Parameter("MaxIterations", "")]
        public int MaxIteraciones
        {
            get
            {
                return maxIteraciones;
            }
            set
            {
                maxIteraciones = value;
            }
        }
        public override TxSegmentationResult Segment(params TxMatrix[] descriptors)
        {

            IClasificable[] muestra = ConstruyeMuestra(descriptors);
            KMeanNoSupervisado kmean = new KMeanNoSupervisado(muestra, numeroClases, maxIteraciones);
            kmean.HallaPatrones();

            //Buscar el mejor resultado de entre un numero de pruebas realizadas.
            KMeanNoSupervisado kmean1;
            error = kmean.Error();
            double error1;
            for (int i = 0; i < numeroPruebas; i++)
            {
                kmean1 = new KMeanNoSupervisado(muestra, NumeroClases, maxIteraciones);
                kmean1.HallaPatrones();
                error1 = kmean1.Error();
                if (error1 < error) { kmean = kmean1; error = error1; }
            }



            TxSegmentationResult result = new TxSegmentationResult(numeroClases, descriptors[0].Width, descriptors[0].Height);
            string clase;
            VectorColores vector;


            for (int i = 0; i < descriptors[0].Height; i++)
                for (int j = 0; j < descriptors[0].Width; j++)
                {
                    vector = new VectorColores(descriptors.Length);

                    for (int x = 0; x < descriptors.Length; x++)
                        vector.Add(descriptors[x][i, j]);

                    clase = kmean.Clasifica(vector);
                    result[i, j] = int.Parse(kmean.Clases[int.Parse(clase)].Id);


                }

            return result;


        }

        public override double ProbError()
        {
            return error;
        }


        private IClasificable[] ConstruyeMuestra(TxMatrix[] imagen)
        {
            IClasificable[] muestra = new IClasificable[imagen[0].Height * imagen[0].Width * numeroMuestras / 100];
            bool[,] marcas = new bool[imagen[0].Width, imagen[0].Height];
            Random random = new Random();
            int x = 0;
            int y = 0;
            //Si el numero de muestras es menor que el 75% hago una seleccion aleatoria
            if (numeroMuestras < 75)
            {
                //Seleccionando numeros aleatorios
                for (int i = 0; i < muestra.Length; i++)
                {
                    x = random.Next();
                    if (x > imagen[0].Width - 5) x = x % imagen[0].Width;


                    y = random.Next();
                    if (y > imagen[0].Height - 5) y = y % imagen[0].Height;

                    //Viendo que ese pixel no haya sido seleccionado
                    if (!marcas[x, y])
                    {
                        marcas[x, y] = true;
                        //Aqui la muestra va a ser una coleccion de Vectores de color, que cada vector de color va a tener en la posicion iesima 
                        //el valor del pixel x,y de la imagen iesima dentro del arreglo de imagenes
                        muestra[i] = new VectorColores(imagen.Length);
                        for (int j = 0; j < imagen.Length; j++)
                            ((VectorColores)muestra[i]).Add(imagen[j][y, x]);
                    }
                    else
                    {

                        i = i - 1;
                        continue;
                    }

                }
            }
            //Si el numero de muestras es mayor que el 75% selecciono los pixeles centrales de la imagen.
            else
            {
                //Seleccionando una buena posicion a partir de la cual voy a tomar los pixeles
                int elemASeleccionar = imagen[0].Width * imagen[0].Height * numeroMuestras / 100;
                int elemSobran = imagen[0].Width * imagen[0].Height - elemASeleccionar;

                int posy = 0;
                while (posy * imagen[0].Width < elemSobran / 2)
                {
                    posy++;
                }
                if (posy != 0) posy--;
                int posx = elemSobran / 2 - posy * imagen[0].Width;

                int cont = 0;
                for (int j = posy; j < imagen[0].Height; j++)
                {
                    if (cont == elemASeleccionar) break;

                    for (int i = 0; i < imagen[0].Width; i++)
                    {
                        if (cont == elemASeleccionar) break;

                        muestra[cont] = new VectorColores(imagen.Length);
                        for (int k = 0; k < imagen.Length; k++)
                            ((VectorColores)muestra[cont]).Add(imagen[k][j, i]);
                        cont++;
                    }
                }

            }
            return muestra;

        }
    }
}
