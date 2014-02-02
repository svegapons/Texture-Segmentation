using System;
using System.Collections.Generic;
using System.Text;


namespace TxEstudioKernel.Operators
{
    public class Elemento
    {
        int indice;

        public int Indice
        {
            get { return indice; }
            set { indice = value; }
        }
        double evaluacion;

        public double Evaluacion
        {
            get { return evaluacion; }
            set { evaluacion = value; }
        }

        public Elemento(int indice, double evaluacion)
        {
            if (indice >= 0) this.indice = indice;
            else this.indice = 0;

            this.evaluacion = evaluacion;

        }

    }


    public class SFFS
    {
        double[] funcionesCriterio;
        bool[] seleccionados;//Aqui se van amarcar las caracteristicas seleccionadas
        int k;//cantidad de caracteristicas seleccionadas.

        public bool[] FeatureSelection(TxMatrix[] descritores, TxSegmentationAlgorithm segmentador, TxSegmentationEvaluation evaluador, int dimension, float error)
        {

            funcionesCriterio = new double[dimension];
            seleccionados = new bool[descritores.Length];

            //Si la dimension del subconjunto resultante es 0, entonces no se devuelve ningun elemento.
            if (dimension == 0) return seleccionados;

            Elemento masSig;
            Elemento menosSig;

            bool[] conjPrima;

            Inicializacion(descritores, segmentador, evaluador, dimension);

            while (k + 1 < dimension)
            {
                //Paso1
                masSig = Selecciona_Caracteristica_Mas_Significativa_Del_Conjunto(descritores, segmentador, evaluador);

                //Paso2
                menosSig = SeleccionaCaracteristicaMenosSignificativaEnConjunto(descritores, segmentador, evaluador, seleccionados);

                if (menosSig.Indice == masSig.Indice)
                {
                    k++;
                    funcionesCriterio[k] = masSig.Evaluacion;
                    continue;
                }
                else
                {
                    //formando un conjunto xkprima que es igual al conjunto xk-caracMenosSignificativa.
                    conjPrima = seleccionados;
                    conjPrima[menosSig.Indice] = false;

                    if (k + 1 == 2)
                    {
                        seleccionados = conjPrima;
                        funcionesCriterio[k] = menosSig.Evaluacion;
                        continue;
                    }
                    else
                    {
                        //Paso3
                        Elemento menosSigPrima;
                        while (k + 1 > 2)
                        {
                            menosSigPrima = SeleccionaCaracteristicaMenosSignificativaEnConjunto(descritores, segmentador, evaluador, conjPrima);

                            if (funcionesCriterio[k - 1] - (menosSigPrima.Evaluacion + error) <= 0)
                            {
                                seleccionados = conjPrima;
                                funcionesCriterio[k] = menosSigPrima.Evaluacion;
                                break;
                            }
                            else
                            {
                                conjPrima[menosSigPrima.Indice] = false;
                                k = k - 1;
                                if (k == 2)
                                {
                                    seleccionados = conjPrima;
                                    funcionesCriterio[k] = menosSigPrima.Evaluacion;
                                    break;
                                }
                            }

                        }
                    }
                }
            }
            return seleccionados;
        }

        protected void Inicializacion(TxMatrix[] descriptores, TxSegmentationAlgorithm segmentador, TxSegmentationEvaluation evaluador, int dimension)
        {
            funcionesCriterio[0] = Selecciona_Caracteristica_Mas_Significativa_Del_Conjunto(descriptores, segmentador, evaluador).Evaluacion;
            k = 0;
            if (dimension == 1) return;
            funcionesCriterio[1] = Selecciona_Caracteristica_Mas_Significativa_Del_Conjunto(descriptores, segmentador, evaluador).Evaluacion;
            k = 1;

        }
        protected Elemento Selecciona_Caracteristica_Mas_Significativa_Del_Conjunto(TxMatrix[] descriptores, TxSegmentationAlgorithm segmentador, TxSegmentationEvaluation evaluador)
        {
            Elemento resultado = new Elemento(0, 0);
            double evaluacion = 0;
            TxSegmentationResult resultSeg;
            TxMatrix[] descSelecc = DescriptoresSeleccionados(descriptores, seleccionados);

            //Hallando la caracteristica de maxima significacion
            for (int i = 0; i < seleccionados.Length; i++)
            {
                if (!seleccionados[i])
                {
                    //Evaluando la significacion de una caracteristica
                    descSelecc[descSelecc.Length - 1] = descriptores[i];
                    resultSeg = segmentador.Segment(descSelecc);
                    evaluacion = evaluador.Evaluate(resultSeg, segmentador);

                    //Verificando si es maximo parcial
                    if (evaluacion > resultado.Evaluacion)
                    {
                        resultado.Indice = i;
                        resultado.Evaluacion = evaluacion;
                    }
                }
            }

            //Marcando como seleccionada la caracteristica de mas significacion
            seleccionados[resultado.Indice] = true;
            return resultado;

        }

        protected Elemento SeleccionaCaracteristicaMenosSignificativaEnConjunto(TxMatrix[] descriptores, TxSegmentationAlgorithm segmentador, TxSegmentationEvaluation evaluador, bool[] conjunto)
        {
            Elemento elem = new Elemento(0, 0);
            bool[] conjAux = conjunto;
            TxSegmentationResult resultSeg;
            double evaluacion;

            for (int i = 0; i < conjAux.Length; i++)
            {
                if (conjAux[i])
                {
                    //Evaluando las caracteristicas
                    conjAux[i] = false;
                    resultSeg = segmentador.Segment(ColeccionDescriptores(conjAux, descriptores));
                    evaluacion = evaluador.Evaluate(resultSeg, segmentador);

                    if (elem.Evaluacion < evaluacion)
                    {
                        elem.Evaluacion = evaluacion;
                        elem.Indice = i;
                    }
                    conjAux[i] = true;
                }
            }
            return elem;
        }

        protected TxMatrix[] ColeccionDescriptores(bool[] conjAux, TxMatrix[] descriptores)
        {
            int cont = 0;

            for (int i = 0; i < conjAux.Length; i++)
                if (conjAux[i]) cont++;

            TxMatrix[] result = new TxMatrix[cont];
            int indice = 0;

            for (int i = 0; i < conjAux.Length; i++)
            {
                if (conjAux[i])
                {
                    result[indice] = descriptores[i];
                    indice++;
                }
            }
            return result;
        }

        protected TxMatrix[] DescriptoresSeleccionados(TxMatrix[] descriptores, bool[] conjunto)
        {
            int cont = 0;

            for (int i = 0; i < conjunto.Length; i++)
                if (conjunto[i]) cont++;

            TxMatrix[] result = new TxMatrix[cont + 1];
            int indice = 0;

            for (int i = 0; i < seleccionados.Length; i++)
            {
                if (seleccionados[i])
                {
                    result[indice] = descriptores[i];
                    indice++;
                }
            }
            return result;
        }


    }
}