using System;
using System.Reflection;
using TxEstudioKernel.VisualElements;

namespace TxEstudioKernel.CustomAttributes
{
    /// <summary>
    /// Clase base de los atributos que especifican las restricciones respecto a los parámetros y proveen el editor
    /// adecuado para establecer su valor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited = true)]
    public abstract class TxConstraintAttribute : TxCustomAttribute
    {
        //Pasarle la propiedad, a la restriccion inicializar correctamente el editor
        public abstract IParameterEditor GetEditorFor(PropertyInfo parameterProperty);
        
        public abstract bool IsCompliantWith(Type parameterType);

    }
}
