using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.Operators
{
    public class FuzzyART
    {
        #region Vars
        private readonly int inputsCount;
        private double vigilanceParamenter;        
        protected double[,] w;
        private int classes;
        private int winner;
        private double learningRate = 1;
        private double choiceParameter = 1;
        #endregion
        /// <summary>
        /// Crea una instancia de una red FuzzyART
        /// </summary>
        /// <param name="inputsCount">Cantidad de elementos esperados en cada patr�n.</param>
        /// <param name="vigilanceParam">Par�metro de vigilancia. Es un valor en el intervalo [0,1]. Mientras mayor sea el par�metro de vigilancia mayor ser� la discriminaci�n de clases que realizar� la red.</param>
        /// <param name="choiseParam">Par�metro de preferencia del usuario. Debe ser un n�mero mayor que 0.</param>
        /// <exception cref="ArgumentException">La cantidad de elementos esperados en cada patr�n es un n�mero menor o igual 0.</exception>
        /// <exception cref="ArgumentException">El par�metro de vigilancia tiene un valor fuera del intervalo [0,1].</exception>
        /// <exception cref="ArgumentException">El par�metro de preferencia es menor que 0.</exception>
        public FuzzyART(int inputsCount, double vigilanceParam, double choiseParam)
        {
            if (inputsCount <= 0)
                throw new ArgumentException("The input vector must have a positive size", "inputsCount");
            if (vigilanceParam < 0 || vigilanceParam > 1)
                throw new ArgumentException("The vigilance parameter must have a value between the interval [0,1].", "vigilanceParam");
            if (choiceParameter < 0)
                throw new ArgumentException("The choice parameter must be a positive number.", "choiceParam");
            this.inputsCount = 2*inputsCount;
            vigilanceParamenter = vigilanceParam;
            choiceParameter = choiseParam;
            w = new double[this.inputsCount, 1];

            classes = 1;
            for (int i = 0; i < this.inputsCount; i++)
                w[i, 0] = 1;
        }
        #region Run
        /// <summary>
        /// Realiza una corrida de la red
        /// </summary>
        /// <param name="input">Vector de entrada</param>
        /// <returns>Devuelve el error cuadr�tico entre el patr�n de entrada y el centroide de la clase.</returns>
        /// <exception cref="InvalidOperationException">El tama�o del vector de entrada no coincide con el la cantidad de elemetentos esperados en el patr�n de entrada.</exception>
        public virtual double Run(double[] input)
        {
            if (2*input.Length != inputsCount)
                throw new InvalidOperationException("The size of the input patter is different from expected input size.");
            double[] I = PreprocessInput(input);
            bool[] isReset = new bool[classes];
            int counter = classes;
            double choiseValue = 0;
            while (counter > 0)
            {
                ChoiseCategory(I, isReset, out choiseValue, out winner);
                //if (choiseValue < vigilanceParamenter)
                if(MatchFunction(winner,I)<vigilanceParamenter)
                {
                    isReset[winner] = true;
                    counter--;
                }
                else
                {
                    UpdateWeights(winner, I);
                    return Distance(GetColVector(w, winner),I);
                }
            }
            double[,] w2 = new double[w.GetLength(0), w.GetLength(1)+1];
            CopyMatrix(w, w2);
            for (int i = 0; i < inputsCount; i++)
                w2[i, classes] = I[i];
            w = w2;
            winner = classes++;
            return 0;
        }
        /// <summary>
        /// Corre una �poca de patrones.
        /// </summary>
        /// <param name="input">Conjunto de patrones de entrada.</param>
        /// <returns>Devuelve la suma del error cuadr�tico entre cada patr�n con su centroide.</returns>
        public virtual double RunEpoch(double[][] input)
        {
            double error = 0;
            foreach(double[]pattern in input)
                error+=Run(pattern);
            return error;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Devuelve o establece la velocidad de aprendizaje de la red.
        /// </summary>
        public virtual double LearningRate
        {
            get { return learningRate; }
            set { learningRate = value; }
        }
        /// <summary>
        /// Devuelve la cantidad de clases encontradas por la red.
        /// </summary>
        public virtual int Classes
        {
            get { return classes; }
        }
        /// <summary>
        /// Devuelve el tama�o del vector de entrada esperado.
        /// </summary>
        public virtual int InputsCount
        {
            get { return inputsCount; }
        }
        /// <summary>
        /// Devuelve el �ndice de la �ltima neurona ganadora
        /// </summary>
        public virtual int Winner
        {
            get { return winner; }
        }
        /// <summary>
        /// Devuelve el valor del par�metro de preferencia.
        /// </summary>
        public virtual double ChoiceParameter
        {
            get { return choiceParameter; }
        }
        /// <summary>
        /// Devuelve el valor del par�metro de vigilancia.
        /// </summary>
        public virtual double VigilanceParameter
        {
            get { return vigilanceParamenter; }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Preprocesa el vector de entrada original.
        /// </summary>
        /// <param name="input">Vector de entrada original.</param>
        /// <returns>
        /// Devuelve un vector de tama�o 2M, donde M es el tama�o del vector de entrada original.
        /// Este nuevo vector tiene las siguientes caracter�sticas:
        /// - Los primeros M elementos es igual al vector imput;
        /// - Cada uno de los restantes M elementos es igual 1 - input[i]. Donde i=0...M-1.
        /// </returns>
        protected virtual double[] PreprocessInput(double[] input)
        {
            double[] result = new double[2 * input.Length];
            Array.Copy(input, result, input.Length);
            for (int i = input.Length; i < result.Length; i++)
                result[i] = 1 - input[i - input.Length];
            return result;
        }
        /// <summary>
        /// Env�a el patr�n de entrada a la capa F2 y realiza la competici�n. Devuelve en winner el �ndice de la neurona ganadora y en choiseValue el valor que tuvo la neurona ganadora en la competici�n.
        /// </summary>
        /// <param name="input">Vector de entrada.</param>
        /// <param name="isReset">Vector que contiene el estado de Reset de la neurona j de la capa F2. Si el �ndice j evaluado en este vector es false, entonces esa neurona no participar� en la competici�n.</param>
        /// <param name="choiseValue">Valor que result� de la competici�n de la neurona ganadora.</param>
        /// <param name="winner">�ndice de la neurona ganadora.</param>
        protected virtual void ChoiseCategory(double[] input, bool[]isReset, out double choiseValue, out int winner)
        {
            winner=-1;
            choiseValue=0;
            double niu=-1;
            for (int j = 0; j < classes; j++)
            {
                if (!isReset[j])
                {
                    niu = ChoiseFunction(j, input);
                    if (niu > choiseValue)
                    {
                        choiseValue = niu;
                        winner = j;
                    }
                }
            }
        }
        /// <summary>
        /// Funci�n de elecci�n.
        /// </summary>
        /// <param name="classIndex">�ndice de la clase.</param>
        /// <param name="input">Vector de entrada extendido.</param>
        /// <returns>Devuelve el valor de la competici�n de la neurona con �ndice classIndex.</returns>
        protected virtual double ChoiseFunction(int classIndex, double[] input)
        {
            double[] wj = GetColVector(w, classIndex);
            return Norm(FuzzyAnd(input, wj)) / (ChoiceParameter + Norm(wj));
        }
        /// <summary>
        /// Funci�n Match. El valor de esta funci�n es utilizado para ser comparado con el par�metro de vigilancia para determinar la pertenencia del patr�n de entrada a la clase ganadora.
        /// </summary>
        /// <param name="winner">�ndice de la neurona ganadora.</param>
        /// <param name="input">Vector de entrada extendido.</param>
        /// <returns>Devuelve un valor es utilizado para ser comparado con el par�metro de vigilancia para determinar la pertenencia del patr�n de entrada a la clase ganadora.</returns>
        protected virtual double MatchFunction(int winner, double[] input)
        {
            return Norm(FuzzyAnd(GetColVector(w, winner), input)) / Norm(input);
        }

        /// <summary>
        /// Actualiza los pesos de la red.
        /// </summary>
        /// <param name="winner">�ndice de la neurona ganadora.</param>
        /// <param name="input">Vector de entrada extendido.</param>
        protected virtual void UpdateWeights(int winner, double[] input)
        {
            for (int i = 0; i < inputsCount; i++)
                w[i, winner] = learningRate * (FuzzyAnd(input[i], w[i, winner])) + (1 - learningRate) * w[i, winner];
        }
        /// <summary>
        /// Realiza la fuzzy intersecci�n de dos n�meros
        /// </summary>
        /// <param name="a">N�mero 1</param>
        /// <param name="b">N�mero 2</param>
        /// <returns>Devuelve el menor valor entre a y b</returns>
        protected double FuzzyAnd(double a, double b)
        {
            return Math.Min(a, b);
        }
        /// <summary>
        /// Realiza la fuzzy intersecci�n de dos vectores.
        /// </summary>
        /// <param name="p">Vector 1</param>
        /// <param name="q">Vector 2</param>
        /// <returns>Retorna un vector donde la componente i es el m�nimo entre las componentes  i del vector p y del vector q.</returns>
        /// <exception cref="InvalidOperationException">Los vectores p y q deben de ser del mismo tama�o.</exception>
        protected double[] FuzzyAnd(double[] p, double[] q)
        {
            if (p.Length != q.Length)
                throw new InvalidOperationException("Is not possible calculate fuzzy and of two vector with different length");
            double[] result = new double[p.Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = FuzzyAnd(p[i], q[i]);
            return result;
        }
        /// <summary>
        /// Retorna la norma de un vector.
        /// </summary>
        /// <param name="v">Vector</param>
        /// <returns>La norma devuelta es la suma de las componentes del vector.</returns>
        protected virtual double Norm(double[] v)
        {
            double sum = 0;
            for (int i = 0; i < v.Length; i++)
            {
                sum += v[i];
            }
            return sum;
        }
        /// <summary>
        /// Calcula la distancia entre dos vectores
        /// </summary>
        /// <param name="v">Vector v</param>
        /// <param name="w">Vector w</param>
        /// <returns>Retorna la distancia euclideana entre dos vectores</returns>
        protected virtual double Distance(double[] v, double[] w)
        {
            double result = 0;
            for (int j = 0; j < v.Length; j++)
                result += Math.Pow(v[j] - w[j], 2);
            return Math.Sqrt(result);
        }
        #endregion
        private static double[] GetColVector(double[,] matrix, int col)
        {
            double[] result = new double[matrix.GetLength(0)];
            for (int j = 0; j < matrix.GetLength(0); j++)
                result[j] = matrix[j, col];
            return result;
        }
        private void CopyMatrix(double[,] src, double[,] dest)
        {
            for (int i = 0; i < src.GetLength(0); i++)
                for (int j = 0; j < src.GetLength(1); j++)
                    dest[i, j] = src[i, j];
        }
    }


    /// <summary>
    /// Nueva implementaci�n de Fuzzy ART. 
    /// Tomado del paper "A New ART Neural Networks for Remote Sensing Image Classification" por AnFei Liu, BiCheng Li, Gang Chen, and Xianfei Zhang; Information Engineering University, China.
    /// </summary>
    public class FuzzyART2C:FuzzyART
    {
        /// <summary>
        /// Crea una instancia de una red FuzzyART
        /// </summary>
        /// <param name="inputsCount">Cantidad de elementos esperados en cada patr�n.</param>
        /// <param name="vigilanceParam">Par�metro de vigilancia. Es un valor en el intervalo [0,1]. Mientras mayor sea el par�metro de vigilancia mayor ser� la discriminaci�n de clases que realizar� la red.</param>
        public FuzzyART2C(int inputsCount, double vigilanceParam):base(inputsCount,vigilanceParam,0)
        {
        }
        /// <summary>
        /// Funci�n de elecci�n.
        /// </summary>
        /// <param name="classIndex">�ndice de la clase.</param>
        /// <param name="input">Vector de entrada extendido.</param>
        /// <returns>Devuelve el valor de la competici�n de la neurona con �ndice classIndex.</returns>
        protected override double ChoiseFunction(int classIndex, double[] input)
        {
            double[] wj = GetColVector(w, classIndex);
            return Norm(FuzzyAnd(input, wj)) / (Norm(input) * Norm(wj));
        }
        /// <summary>
        /// Funci�n Match. El valor de esta funci�n es utilizado para ser comparado con el par�metro de vigilancia para determinar la pertenencia del patr�n de entrada a la clase ganadora.
        /// </summary>
        /// <param name="winner">�ndice de la neurona ganadora.</param>
        /// <param name="input">Vector de entrada extendido.</param>
        /// <returns>Devuelve un valor es utilizado para ser comparado con el par�metro de vigilancia para determinar la pertenencia del patr�n de entrada a la clase ganadora.</returns>
        protected override double MatchFunction(int winner, double[] input)
        {
            return ChoiseFunction(winner, input);
        }
        private static double[] GetColVector(double[,] matrix, int col)
        {
            double[] result = new double[matrix.GetLength(0)];
            for (int j = 0; j < matrix.GetLength(0); j++)
                result[j] = matrix[j, col];
            return result;
        }
    }
}
