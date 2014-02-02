using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.OpenCV;
using TxEstudioKernel.CustomAttributes;
using ActiveContour;
using TxEstudioKernel.Operators.ConnectedComponents;
using System.Drawing;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Bi-Class Active Contours Algorithm", "No Supervised image segmentation algorithm using the Active Contour Without Edge Model and Connected Components Analysis")]
    [Abbreviation("acB_cc", "Miu")]
    public unsafe class AC_BiClass : TxSegmentationAlgorithm
    {
        public AC_BiClass()
        {
            lengthValues.Add("000", 0f);
            lengthValues.Add("001", 1f);
            lengthValues.Add("010", 1f);
            lengthValues.Add("011", 1.4142135623f);
            lengthValues.Add("100", 1.4142135623f);            lengthValues.Add("101", 1f);
            lengthValues.Add("110", 1f);
            lengthValues.Add("111", 0f);
        }


        #region Vars

        float miu = 1;
        int ccSize = 300;
        BiClassAC_OneBand[] bands;
        int[,] phi;
        float maxIter = 50;
        int width, height;
        NewFilterCC filter = new NewFilterCC();
        Dictionary<string, float> lengthValues = new Dictionary<string, float>();
       

        #endregion

        #region Properties

        [Parameter("Miu", "peso para calcular el length(C) ")]
        [RealInRange(0.0f, 1.0f)]
        public float Miu
        {
            get
            {
                return this.miu;
            }
            set
            {
                miu = value;
            }
        }

        [Parameter("CCSize", "Tamaño de la menor componente conexa")]
        public int CCSize
        {
            get
            {
                return this.ccSize;
            }
            set
            {
                ccSize = value;
            }
        }
        
        [Parameter("maxIterations", "Numero maximo de iteraciones del algoritmo")]
        public float MaxIteration
        {
            get { return maxIter; }
            set { maxIter = value; }
        }

        #endregion



        //metodo heredado de Iclasificable.
        public override double ProbError()
        {
            throw new Exception("Este segmentador no tiene implementado una funcion para la estimación no supervisada del error de clasificacion");
        }

        public override TxSegmentationResult Segment(params TxMatrix[] descriptors)
        {
            width = descriptors[0].Width;
            height = descriptors[0].Height;
            TxSegmentationResult res = new TxSegmentationResult(2, width, height);
            phi = new int[height, width];
            GetInitialPartition();
            filter.MinSize = this.ccSize;
            
            bands = new BiClassAC_OneBand[descriptors.Length];
            for (int i = 0; i < bands.Length; i++)
            {
                bands[i] = new BiClassAC_OneBand();
                bands[i].Init(descriptors[i], phi, miu);
            }

            int iter = 0;

            int change = Sweep(false);
            while (change>10 && iter < maxIter)
            {
                iter++;
                GetC1C2();
                change = Sweep(false);
            }

            iter = 0;

            change = Sweep(true);
            while (change>10 && iter < maxIter)
            {
                iter++;
                if (iter % 5 == 1 && ccSize != 0)
                    phi = filter.Filter(phi);
                GetC1C2();
                change = Sweep(true);
            }
            phi = filter.Filter(phi);

            for (int i = 0; i < res.Height; i++)
                for (int j = 0; j < res.Width; j++)
                {
                    if (phi[i, j] < 0)
                        res[i, j] = 0;
                    else
                        res[i, j] = 1;
                }
            return res;

        }

        private int Sweep(bool compLength)
        {
            int change = 0;
            float energy = 0.0f;

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    energy = 0.0f;

                    if (compLength)
                        energy += GetDeltaLength(i, j) * miu;

                    for (int k = 0; k < bands.Length; k++)
                        energy += bands[k].GetDeltaEnergy(i, j);

                    if (energy < 0)
                    {
                        phi[i, j] *= -1;
                        change++;
                    }

                }
            return change;
        }

        protected void GetC1C2()
        {
            for (int i = 0; i < bands.Length; i++)
            {
                bands[i].GetC1C2(phi);
            }
        }

        protected void GetInitialPartition()
        {
            for(int i=0;i<phi.GetLength(0);i++)
                for (int j = 0; j < phi.GetLength(1); j++)
                {
                    if (j < phi.GetLength(1) / 3)
                        phi[i, j] = 1;
                    else
                        phi[i, j] = -1;
                }
        }

        protected float GetDeltaLength(int row, int col)
        {
            float b = ComputeLength(row, col);
            if (row > 0)
                b += ComputeLength(row - 1, col);
            if (col > 0)
                b += ComputeLength(row, col - 1);

            phi[row, col] *= -1;

            float b1 = ComputeLength(row, col);
            if (row > 0)
                b1 += ComputeLength(row - 1, col);
            if (col > 0)
                b1 += ComputeLength(row, col - 1);

            phi[row, col] *= -1;

            return b1 - b;

        }

        protected float ComputeLength(int row, int col)
        {
            if (row == height - 1 || col == width - 1)
                return Math.Max(0f, phi[row, col]);
            string cod = "";
            if (phi[row, col] >= 0)
                cod += "1";
            else
                cod += "0";
            if (phi[row + 1, col] >= 0)
                cod += "1";
            else
                cod += "0";
            if (phi[row, col + 1] >= 0)
                cod += "1";
            else
                cod += "0";

            return lengthValues[cod];
        }
    }

    public unsafe class BiClassAC_OneBand
    {
        public BiClassAC_OneBand()
        {
           
        }

        #region Vars

        float[,] matrix;
        int[,] phi;
        float miu = 1, c1, c2, inner = 0, outer = 0;

        #endregion

        public void Init(TxMatrix matrix, int[,]phi, float miu)
        {
            this.matrix = new float[matrix.Height, matrix.Width];
            this.phi = phi;
            this.miu = miu;

            inner = 0;
            outer = 0;
            c1 = 0.0f;
            c2 = 0.0f;

            float*matrixData = (float*)((_cvMat*)matrix.InnerMatrix)->data;

            float min = float.MaxValue, max = 0;
            for (int i = 0; i < matrix.Height; i++)
                for (int j = 0; j < matrix.Width; j++, matrixData++)
                {

                    if (min > (*matrixData))
                        min = *matrixData;
                    if (max < *matrixData)
                        max = *matrixData;
                }

            matrixData = (float*)((_cvMat*)matrix.InnerMatrix)->data;

            for (int i = 0; i < matrix.Height; i++)
                for (int j = 0; j < matrix.Width; j++, matrixData++)
                {

                    this.matrix[i, j] =  ((*matrixData) - min) / (max - min);

                    if (phi[i,j]<0)
                    {
                        c2 += this.matrix[i, j];
                        outer++;
                    }
                    else
                    {
                        c1 += this.matrix[i, j];
                        inner++;
                    }
                }
            c1 = c1 / inner;
            c2 = c2 / outer;

        }

        public float GetDeltaEnergy(int i, int j)
        {
            float energy = 0.0f;
            //int result = 1;
            float dl = 0.0f;
          
            if (phi[i, j] > 0)
                energy =  (matrix[i, j] - c2) * (matrix[i, j] - c2) * (outer / (outer + 1)) - (matrix[i, j] - c1) * (matrix[i, j] - c1) * (inner / (inner - 1));
            else
                energy =  (matrix[i, j] - c1) * (matrix[i, j] - c1) * (inner / (inner + 1)) - (matrix[i, j] - c2) * (matrix[i, j] - c2) * (outer / (outer - 1));

            return energy;
        }

       

        public void GetC1C2(int[,]phi)
        {
            this.phi = phi;
            inner = 0;
            outer = 0;
            c1 = 0.0f;
            c2 = 0.0f;

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                {

                    if (phi[i, j] > 0)
                    {
                        c1 += matrix[i, j];
                        inner++;
                    }
                    else if (phi[i, j] < 0)
                    {
                        c2 += matrix[i, j];
                        outer++;
                    }
                }

            c1 /= inner;
            c2 /= outer;
        }

    }
}
