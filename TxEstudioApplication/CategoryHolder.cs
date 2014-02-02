using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioApplication
{
    
    public class CategoryHolder:IEnumerable<AlgorithmHolder>
    {

        public CategoryHolder(string name, string description, List<AlgorithmHolder> operators)
        {
            //Pude tener utilidad para los que no tienen categoria
            categoryName = name;
            categoryDescription = description;
            this.operators = operators;
        }


        /// <summary>
        /// Toma de una lista de operadores, aquellos que sean de una categoria dada.
        /// </summary>
        /// <param name="category">Categoria a buscar.</param>
        /// <param name="operators">LIsta de operadores de donde se sacaran los pertecnecientes a la categoria dada.</param>
        public CategoryHolder(TxCategoryAttribute category, List<AlgorithmHolder> operators)
        {
            categoryName = category.Name;
            categoryDescription = category.Description;
            this.operators = new List<AlgorithmHolder>(operators.Count);
            foreach (AlgorithmHolder holder in operators)
            {
              object[] atts =  holder.AlgorithmType.GetCustomAttributes(typeof(TxCategoryAttribute), false);
                for (int i = 0; i < atts.Length; i++)
                {
                    if(((TxCategoryAttribute)atts[i]).Name == categoryName)
                        this.operators.Add(holder);
                }
            }
        }


        public CategoryHolder(List<AlgorithmHolder> operators)
        {
            //TODO: Aqui se debe buscar la  categoria de cada operador y asignar el nombre y la descripcion
            //ademas de verificar que todos pertenezcan a una misma categoria.
        }

        string categoryName = "";
        public string CategoryName
        {
            get
            {
                return categoryName;
            }
        }

        string categoryDescription = "";
        public string CategoryDescription
        {
            get
            {
                return categoryDescription;
            }
        }


        List<AlgorithmHolder> operators;
        public TxAlgorithm this[string operatorName]
        {
            get
            {
                foreach (AlgorithmHolder op in operators)
                {
                    if (op.AlgorithmName == operatorName)
                        return op.Algorithm;
                }
                return null;
            }
        }


        #region IEnumerable<AlgorithmHolder> Members

        public IEnumerator<AlgorithmHolder> GetEnumerator()
        {
            return operators.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return operators.GetEnumerator();
        }

        #endregion
    }
}
