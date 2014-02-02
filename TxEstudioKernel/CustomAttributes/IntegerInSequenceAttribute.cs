using System;
using TxEstudioKernel.VisualElements;
using TxEstudioKernel.Exceptions;

namespace TxEstudioKernel.CustomAttributes
{
    public class IntegerInSequenceAttribute : TxConstraintAttribute
    {
        
        public IntegerInSequenceAttribute(int max):this(0,max,1)
        { }
        public IntegerInSequenceAttribute(int min, int max): this(min, max, 1)
        { }
        public IntegerInSequenceAttribute(int min, int max, int step)
        {
            this.min = min;
            this.max = max;
            this.step = step;
        }
        int min, max, step;

        public int Step
        {
            get { return step; }
            set { step = value; }
        }

        public int Maximum
        {
            get { return max; }
            set { max = value; }
        }

        public int Minimum
        {
            get { return min; }
            set { min = value; }
        }

        public override IParameterEditor GetEditorFor(System.Reflection.PropertyInfo parameterProperty)
        {
            if (IsCompliantWith(parameterProperty.PropertyType))
            {
                IntegerParameterEditor result = new IntegerParameterEditor();
                int typeMinimum = int.MinValue;
                int typeMaximum = int.MaxValue;

                if (parameterProperty.PropertyType == typeof(byte))
                {
                    typeMaximum = 255;
                    typeMinimum = 0;
                }

                if (parameterProperty.PropertyType == typeof(short))
                {
                    typeMaximum = short.MaxValue;
                    typeMinimum = short.MinValue;
                }

                result.Minimum = Math.Max(min, typeMinimum);
                result.Maximum = Math.Min(max, typeMaximum);
                result.Increment = step;
                return result;
            }
            else
                throw new TxTypeNotSupportedException(parameterProperty.PropertyType, this.GetType());
        }

        public override bool IsCompliantWith(Type parameterType)
        {
            return parameterType == typeof(int) || parameterType == typeof(byte) || parameterType == typeof(short);
        }
    }
}
