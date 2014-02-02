using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseManager
{
    public class DescInfo
    {
        int hash;

        public int Hash
        {
            get { return hash; }
            set { hash = value; }
        }
        object[] parameters;

        public object[] Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
        float[,] matrix;

        public float[,] Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }

        public DescInfo(int hash, object[] parameters, float[,] matrix)
        {
            this.hash = hash;
            this.parameters = parameters;
            this.matrix = matrix;
        }
    }
}
