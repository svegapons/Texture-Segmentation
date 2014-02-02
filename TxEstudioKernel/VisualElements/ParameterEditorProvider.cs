using System;
using System.Collections.Generic;
using System.Reflection;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.VisualElements
{


    public class ParameterEditorProvider
    {
        delegate IParameterEditor ParameterEditorDelegate();

        static Dictionary<Type, ParameterEditorDelegate> predefinedParameters = new Dictionary<Type, ParameterEditorDelegate>();
        static ParameterEditorProvider()
        {
            predefinedParameters.Add(typeof(int), new ParameterEditorDelegate(delegate() { return new IntegerParameterEditor(); }));
            predefinedParameters.Add(typeof(byte), new ParameterEditorDelegate(
                                                                                delegate() 
                                                                                    { 
                                                                                        IntegerParameterEditor result = new IntegerParameterEditor();
                                                                                        result.Minimum = byte.MinValue; 
                                                                                        result.Maximum = byte.MaxValue; 
                                                                                        result.Increment = 1; 
                                                                                        result.Value = 0;
                                                                                        return result;
                                                                                    }));
            predefinedParameters.Add(typeof(short), new ParameterEditorDelegate(
                                                                                delegate() 
                                                                                    { 
                                                                                        IntegerParameterEditor result = new IntegerParameterEditor(); 
                                                                                        result.Minimum = short.MinValue; 
                                                                                        result.Maximum = short.MaxValue; 
                                                                                        result.Increment = 1; 
                                                                                        result.Value = 0;
                                                                                        return result;
                                                                                    }));
            predefinedParameters.Add(typeof(float), new ParameterEditorDelegate(delegate() { return new RealParameterEditor(); }));
            predefinedParameters.Add(typeof(float[]), new ParameterEditorDelegate(delegate() { return new GeneralParameterEditor(new ArrayEditor());}));

        }
        /// <summary>
        /// Devuelve de los editores predeterminados el correpspondiente al tipo de la propiedad
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static IParameterEditor GetParameterEditor(PropertyInfo property)
        {
            object[] atts = property.GetCustomAttributes(typeof(TxConstraintAttribute), false);
            if (atts.Length > 0)
            {
                TxConstraintAttribute constraint = (TxConstraintAttribute)atts[0];
                if (!constraint.IsCompliantWith(property.PropertyType))
                    return new ErrorParameterEditor();
                return constraint.GetEditorFor(property);
            }
            else
            {
                if (predefinedParameters.ContainsKey(property.PropertyType))
                    return predefinedParameters[property.PropertyType]();
            }
            return new ErrorParameterEditor();
        }

        private static bool IsPredefined(Type type)
        {
            return type == typeof(RealParameterEditor) || type == typeof(IntegerParameterEditor) || type == typeof(GeneralParameterEditor); 
        }

    }
}
