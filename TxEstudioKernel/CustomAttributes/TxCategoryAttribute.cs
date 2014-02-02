using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.CustomAttributes
{
    /// <summary>
    /// Clase base de las categorias que agrupan a los distintos operadores dentro de TxEstudio
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public abstract class TxCategoryAttribute : TxEstudioKernel.CustomAttributes.TxCustomAttribute
    {
        
        /// <summary>
        /// Permite obtener una cadena con el nombre de la categoria.
        /// </summary>
        public  abstract string Name
        {
            get;
        }

        /// <summary>
        /// Retorna una cadena con una breve descripcion de los operadores que pertenecen a la categoria.
        /// </summary>
        public   abstract string Description
        {
            get;
        }
        
    }
}
