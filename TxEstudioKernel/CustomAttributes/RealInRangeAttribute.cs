using System;
using System.Reflection;
using System.ComponentModel;
using TxEstudioKernel.Exceptions;
using TxEstudioKernel.VisualElements;

namespace TxEstudioKernel.CustomAttributes
{
    public class RealInRangeAttribute : TxConstraintAttribute
    {
        float min, max;

        public float Maximum
        {
            get { return max; }
            set { max = value; }
        }

        public float Minimum
        {
            get { return min; }
            set { min = value; }
        }
        public RealInRangeAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public override IParameterEditor GetEditorFor(PropertyInfo parameterProperty)
        {
            if (IsCompliantWith(parameterProperty.PropertyType))
            {
                RealParameterEditor result = new RealParameterEditor();
                result.Minimum = min;
                result.Maximum = max;
                return result;
            }
            else
                throw new TxTypeNotSupportedException(parameterProperty.PropertyType, this.GetType());
        }

        public override bool IsCompliantWith(Type parameterType)
        {
            return parameterType == typeof(float);
          //  TypeConverter floatConverter = TypeDescriptor.GetConverter(typeof(float));
          //  return (floatConverter.CanConvertFrom(parameterType) && floatConverter.CanConvertTo(parameterType));
        }
    }
}
