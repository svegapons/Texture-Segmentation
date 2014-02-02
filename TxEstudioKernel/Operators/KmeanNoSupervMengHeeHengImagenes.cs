using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{

    [Algorithm("KMeans Meng Hee Heng Method ", "No Supervised image segmentation using the KMean Algorithm in the Meng-Hee Heng Variant known as Clustering Heng Method.")]
    [Abbreviation("km_mhh")]
    public class KmeanNoSupervMengHeeHengImagenes : TxSegmentationAlgorithm
    {

        //metodo heredado de Iclasificable.
        public override double ProbError()
        {
            throw new Exception("Este segmentador no tiene implementado una funcion para la estimacion no supervisada del error de clasificacion");
        }
        
        public override TxSegmentationResult Segment(params TxMatrix[] descriptors)
        {
            //TxSegmentationResult result = new TxSegmentationResult(1, descriptors[0].Width, descriptors[0].Height);
            //return result;
            IClasificable[] muestra = ConstruyeMuestra(descriptors);
            KMeanNoSupervMengHeeHeng kmean = new KMeanNoSupervMengHeeHeng(muestra);
            kmean.HallaPatrones();
            TxSegmentationResult result = new TxSegmentationResult(kmean.CantidadClases, descriptors[0].Width, descriptors[0].Height);
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
                    //info.PonerClasificacion(i, j, clases[int.Parse(clase)]);

                }

            return result;


        }
        private IClasificable[] ConstruyeMuestra(TxMatrix[] imagen)
        {
            IClasificable[] muestra = new IClasificable[imagen[0].Height * imagen[0].Width * 6 / 100];
            Random random = new Random();
            int x = 0;
            int y = 0;

            //Seleccionando numeros aleatorios
            for (int i = 0; i < muestra.Length; i++)
            {
                x = random.Next();
                if (x > imagen[0].Width - 5) x = x % imagen[0].Width;
                //while (x > imagen.Width-5) x = random.Next();

                y = random.Next();
                if (y > imagen[0].Height - 5) y = y % imagen[0].Height;
                //while(y>imagen.Height-5)y=random.Next();

                //Aqui la muestra va a ser una coloeccion de Vectores de color, que cada vector de color va a tener en la posicion iesima 
                //el valor del pixel x,y de la imagen iesima dentro del arreglo de imagenes
                muestra[i] = new VectorColores(imagen.Length);
                for (int j = 0; j < imagen.Length; j++)
                    ((VectorColores)muestra[i]).Add(imagen[j][y,x]);

            }
            return muestra;

        }
    }
}
