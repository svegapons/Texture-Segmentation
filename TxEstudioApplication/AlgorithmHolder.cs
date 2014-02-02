using System;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioApplication
{
    public class AlgorithmHolder
    {
        public AlgorithmHolder(Type algorithmType)
        {
            type = algorithmType;
            object[] att = algorithmType.GetCustomAttributes(typeof(AlgorithmAttribute), false);
            name = (att[0] as AlgorithmAttribute).Name;
            description = (att[0] as AlgorithmAttribute).Description;
        }

        Type type;

        public Type AlgorithmType
        {
            get { return type; }
        }

        TxAlgorithm algorithm;

        public TxAlgorithm Algorithm
        {
            get 
            {
                if (algorithm == null)
                    algorithm = type.GetConstructor(new Type[] { }).Invoke(new object[] { }) as TxAlgorithm; 
                 return algorithm; 
            }
        }
        string name;

        public string AlgorithmName
        {
            get { return name; }
        }
        string description;

        public string AlgorithmDescription
        {
            get { return description; }
        }

        public override string ToString()
        {
            return name;//Porque no encontre algo parecido al DisplayMember en el checked listbox
        }

    }
}
