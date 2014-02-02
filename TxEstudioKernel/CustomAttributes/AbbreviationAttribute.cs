using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;

namespace TxEstudioKernel.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited= false)]
    public class AbbreviationAttribute : TxEstudioKernel.CustomAttributes.TxCustomAttribute
    {
       // TODO:Y si el atributo no corresponde con el tipo, esto da tremento paletazo
        //Hace falta mecanismo para manejar esto
        string abbreviation;
        string[] properties;
        public AbbreviationAttribute(string abbreviation, params string[] properties)
        {
            this.abbreviation = abbreviation;
            this.properties = properties;
        }

        public string Abbreviation
        {
            get
            {
                return abbreviation;
            }
        }

        public object[] GetAlgorithmParameters(TxAlgorithm algorithm)
        {
            object[] result = new object[properties.Length];
            Type type = algorithm.GetType();
            PropertyInfo pInfo = null;
            if (properties != null && properties.Length > 0)
            {
                for (int i = 0; i < properties.Length; i++)
                {

                    pInfo = type.GetProperty(properties[i]);
                    if (pInfo != null)
                        result[i] = pInfo.GetValue(algorithm, new object[] { });
                    else 
                        result = null;
                }
            }
            return result;
        }

        public virtual string GetAlgorithmAbbreviation(TxAlgorithm algorithm)
        {
            string result = abbreviation + "(";
            Type type = algorithm.GetType();
            PropertyInfo pInfo = null;
            if (properties != null && properties.Length > 0)
            {
                for (int i = 0; i < properties.Length - 1; i++)
                {
                    
                    pInfo = type.GetProperty(properties[i]);
                    if (pInfo != null)
                        result += pInfo.GetValue(algorithm, new object[] { }).ToString() + ",";
                    else //Por si alguien como el Voro se cree que yo soy mago
                        result += "error,";
                }
                pInfo = type.GetProperty(properties[properties.Length - 1]);
                if (pInfo != null)
                    result += pInfo.GetValue(algorithm, new object[] { }).ToString();
                else
                    result += "error";
            }
            return result + ")";
        }

        public static string GetFullAbbreviation(TxAlgorithm algorithm)
        {
            
            object[] att = algorithm.GetType().GetCustomAttributes(typeof(AbbreviationAttribute), false);
            if (att.Length > 0)
                return (att[0] as AbbreviationAttribute).GetAlgorithmAbbreviation(algorithm);
            return "";
        }

        public static string GetAbbreviation(TxAlgorithm algorithm)
        {
            object[] att = algorithm.GetType().GetCustomAttributes(typeof(AbbreviationAttribute), false);
            if (att.Length > 0)
                return (att[0] as AbbreviationAttribute).abbreviation;
            return "";
        }

        public static object[] GetParameters(TxAlgorithm algorithm)
        {
            object[] att = algorithm.GetType().GetCustomAttributes(typeof(AbbreviationAttribute), false);
            if (att.Length > 0)
                return (att[0] as AbbreviationAttribute).GetAlgorithmParameters(algorithm);
            return null;
        }
    }
}
