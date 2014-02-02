using System;

namespace TxEstudioKernel.Exceptions
{
    public  abstract class TxException:Exception
    {

    }

    public class TxIncorrectValueForParameterException : TxException
    {
        string parameterName = "";
        public TxIncorrectValueForParameterException(string parameterName)
        {
            this.parameterName = parameterName;
        }

        public override string Message
        {
            get
            {
                return "Incorrect value for parameter " + parameterName;
            }
        }
    }

    public class TxTypeNotSupportedException : TxException
    {
        Type type, constraint;
        public TxTypeNotSupportedException(Type type, Type constraint)
        {
            this.type = type;
            this.constraint = constraint;
        }

        public override string Message
        {
            get
            {
                return string.Format("Type {0} is not supported by constraint of type {1}.", type.FullName, constraint.FullName);
            }
        }
    }
}
