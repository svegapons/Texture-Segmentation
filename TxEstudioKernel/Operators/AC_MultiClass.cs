using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.OpenCV;
using TxEstudioKernel.CustomAttributes;
using ActiveContour;
using TxEstudioKernel.Operators.ConnectedComponents;

namespace TxEstudioKernel.Operators
{

    [Algorithm("Multi-Class Active Contours Algorithm", "No Supervised image segmentation using the Active Contour Without Edge multi-phase model and Connected Components Analysis")]
    [Abbreviation("acM_cc", "Miu", "Classes")]
    public unsafe class AC_MultiClass : TxSegmentationAlgorithm
    {
        public AC_MultiClass()
        {
            lengthValues.Add("000", 0f);
            lengthValues.Add("001", 1f);
            lengthValues.Add("010", 1f);
            lengthValues.Add("011", 1.4142135623f);
            lengthValues.Add("100", 1.4142135623f);
            lengthValues.Add("101", 1f);
            lengthValues.Add("110", 1f);
            lengthValues.Add("111", 0f);
            arrCls = new string[sizeIni];
            float nm = (float)Math.Log(sizeIni, 2);
            if (nm - (float)Math.Floor(nm) > 0)
                nm++;
            m = (int)nm;
            GetClasses(sizeIni);
            InitLength(sizeIni);
        }

        #region Properties

        [Parameter("Miu", "peso para calcular el length(C) ")]
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

        [Parameter("Classes", "numero de clases")]
        public int Classes
        {
            get
            {
                return this.classes;
            }
            set
            {
                classes = value;
            }
        }

        [Parameter("CCSize", "Tamaño del filtro de componentes conexas")]
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

        [Parameter("MaxIter", "Cantidad maxima de iteraciones")]
        public float MaxIterations
        {
            get { return maxIter; }
            set { maxIter = value; }
        }

        #endregion

        #region Vars

        int ccSize = 100;
        int classes = 4;
        int width, height;
        //TxMatrix[] matrixes;
        float maxIter = 30;    
        float miu = 1f;
        int sizeIni = 11;
        int[,] labels;
        string[] arrCls;
        int m = 0;
        Dictionary<string, int> cls = new Dictionary<string, int>();
        NewFilterCC ccf = new NewFilterCC();
        Dictionary<string, float> lengthValues = new Dictionary<string, float>();
        MultiClassAC_OneBand[]bands;

        #endregion



        //metodo heredado de Iclasificable.
        public override double ProbError()
        {
            throw new Exception("Este segmentador no tiene implementado una funcion para la estimacion no supervisada del error de clasificacion");
        }

        public override TxSegmentationResult Segment(params TxMatrix[] descriptors)
        {

            width = descriptors[0].Width;
            height = descriptors[0].Height;
            TxSegmentationResult res = new TxSegmentationResult(classes, descriptors[0].Width, descriptors[0].Height);
            bands = new MultiClassAC_OneBand[descriptors.Length];
            labels = new int[height, width];
            GetInitialPartition();
            arrCls = new string[sizeIni];
            float nm = (float)Math.Log(classes, 2);
            if (nm - (float)Math.Floor(nm) > 0)
                nm++;
            m = (int)nm;
            GetClasses(classes);

            if (classes > sizeIni)
            {
                InitLength(classes);
                sizeIni = classes;
            }
            for (int i = 0; i < bands.Length; i++)
            {
                bands[i] = new MultiClassAC_OneBand();
                bands[i].Init(descriptors[i], labels, miu, classes);
            }

          
            int iter = 0;
            ccf.MinSize = ccSize;

            int change = Sweep(false);
            while (change>10 && iter < maxIter)
            {
                iter++;
                GetC();
                change = Sweep(false);
            }

            iter = 0;

            change = Sweep(true);
            while (change>10 && iter < maxIter)
            {
                iter++;
                if (iter % 5 == 1 && ccSize != 0)
                    labels = ccf.Filter(labels);
                GetC();
                change = Sweep(true);
            }
            labels = ccf.Filter(labels);

            int deltaColor = 255 / (classes - 1);

            for (int i = 0; i < res.Height; i++)
                for (int j = 0; j < res.Width; j++)
                    res[i, j] = labels[i, j];

            return res;

        }

        protected void GetC()
        {
            for (int i = 0; i < bands.Length; i++)
            {
                bands[i].GetC(labels);
            }
        }

        private void GetInitialPartition()
        {
            string current = "";
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    current = arrCls[j * classes / width];
                    for (int t = 0; t < current.Length; t++)
                        labels[i, j] = cls[current];

                }
        }

        protected int Sweep(bool compLength)
        {
            float[] energies = new float[classes];
            string comb = "";
            int change = 0;
            float aux = 0.0f;

           
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < classes; k++)
                        energies[k] = 0.0f;
                   
                    for (int k = 0; k < classes; k++)
                    {
                        comb = arrCls[k];
                        if (compLength)
                            energies[k] += ComputeLength(comb, i, j) * miu;
                        for (int t = 0; t < bands.Length; t++)
                            energies[k] += bands[t].Energy(i, j, comb);
                    }

                    int min = 0;
                    for (int k = 0; k < classes; k++)
                        if (energies[k] < energies[min])
                            min = k;
                    if (labels[i, j] != min)
                    {
                        labels[i, j] = min;
                        change++;
                    }
                }

            return change;
        }

        protected void GetClasses(int classes)
        {
            this.cls.Clear();
            string aux = "";
            char[] comb = new char[m];
            for (int i = 0; i < comb.Length; i++)
                comb[i] = '0';

            for (int i = 0; i < classes; i++)
            {
                aux = AString(comb);
                cls.Add(aux, i);
                arrCls[i] = aux;
                comb = NextComb(comb);
            }

        }

        private char[] NextComb(char[] comb)
        {
            char[] result = new char[comb.Length];
            Array.Copy(comb, result, comb.Length);
            int index = result.Length - 1;

            while (true)
            {
                if (index < 0)
                    return new char[comb.Length];
                if (result[index] == '1')
                    result[index] = '0';
                else
                {
                    result[index] = '1';
                    break;
                }
                index--;
            }
            return result;
        }

        private string AString(char[] arr)
        {
            string result = "";
            for (int i = 0; i < arr.Length; i++)
                result += arr[i].ToString();
            return result;
        }

        protected float ComputeLength(string comb, int row, int col)
        {
            float result = 0.0f;
            int oldPhi = labels[row, col];
            labels[row, col] = cls[comb];

            if (row == 0 || col == 0)
                result = ComputeLength(row, col);
            else
                result = ComputeLength(row, col) + ComputeLength(row - 1, col) + ComputeLength(row, col - 1);

            labels[row, col] = oldPhi;

            return result;
        }

        private float ComputeLength(int row, int col)
        {
            if (row == height - 1 || col == width - 1)
                return 0;
            return lengthComp[labels[row, col] * sizeIni * sizeIni + labels[row + 1, col] * sizeIni + labels[row, col + 1]];
        }

        protected float Length(int x1, int x2, int x3)
        {
            string c1 = arrCls[x1];
            string c2 = arrCls[x2];
            string c3 = arrCls[x3];
            float result = 0;
            string aux = "";

            for (int i = 0; i < c1.Length; i++)
            {
                aux = "" + c1[i] + c2[i] + c3[i];
                result += lengthValues[aux];
            }
            return result;
        }

        float[] lengthComp;

        protected void InitLength(int size)
        {
            lengthComp = new float[size * size * size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    for (int k = 0; k < size; k++)
                    {
                        lengthComp[i * size * size + j * size + k] = Length(i, j, k);
                    }
        }

    }



    public unsafe class MultiClassAC_OneBand
    {
        public MultiClassAC_OneBand()
        {

        }

        #region Vars

        float[] c, inner;
        float[,] matrix;
        int[,] labels;        
        string[] arrCls;
        Dictionary<string, int> cls = new Dictionary<string, int>();
        int m = 0;

        #endregion

       
        public float Energy(int row, int col, string comb)
        {
            float x = c[cls[comb]] - matrix[row,col];
            return x * x;
        }

        public void Init(TxMatrix mat,int[,]labels,float miu, int classes)
        {

            c = new float[classes];

            inner = new float[classes];

            float nm = (float)Math.Log(classes, 2);
            if (nm - (float)Math.Floor(nm) > 0)
                nm++;
            m = (int)nm;

            this.labels=labels;

            matrix = new float[mat.Height, mat.Width];

            arrCls = new string[classes];
            GetClasses(classes);

            float* matrixData = (float*)((_cvMat*)mat.InnerMatrix)->data;

            float min=float.MaxValue, max = 0;
            for (int i = 0; i < mat.Height; i++)
                for (int j = 0; j < mat.Width; j++, matrixData++)
                {

                    if (min > (*matrixData))
                        min = *matrixData;
                    if (max < *matrixData)
                        max = *matrixData;
                }
            matrixData = (float*)((_cvMat*)mat.InnerMatrix)->data;
            for (int i = 0; i < mat.Height; i++)
                for (int j = 0; j < mat.Width; j++,matrixData++)
                {
                    //Se puede optimizar calculando C aqui mismo.
                    matrix[i, j] = ((*matrixData)-min)/(max-min);                    
                }

            GetC(this.labels);

        }

        public void GetC(int[,]labels)
        {
            this.labels = labels;
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = 0f;
                inner[i] = 0f;
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                {

                    c[labels[i, j]] += matrix[i, j];
                    inner[labels[i, j]]++;
                }
            for (int i = 0; i < c.Length; i++)
            {
                c[i] /= inner[i];
            }
        }

        protected void GetClasses(int classes)
        {
            this.cls.Clear();
            string aux = "";
            char[] comb = new char[m];
            for (int i = 0; i < comb.Length; i++)
                comb[i] = '0';

            for (int i = 0; i < classes; i++)
            {
                aux = AString(comb);
                cls.Add(aux, i);
                arrCls[i] = aux;
                comb = NextComb(comb);
            }

        }
        private char[] NextComb(char[] comb)
        {
            char[] result = new char[comb.Length];
            Array.Copy(comb, result, comb.Length);
            int index = result.Length - 1;

            while (true)
            {
                if (index < 0)
                    return new char[comb.Length];
                if (result[index] == '1')
                    result[index] = '0';
                else
                {
                    result[index] = '1';
                    break;
                }
                index--;
            }
            return result;
        }



        private string AString(char[] arr)
        {
            string result = "";
            for (int i = 0; i < arr.Length; i++)
                result += arr[i].ToString();
            return result;
        }


    }

}

