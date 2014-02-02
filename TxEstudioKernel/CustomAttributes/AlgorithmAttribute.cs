using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.CustomAttributes
{
    public class AlgorithmAttribute : TxDescriptorAttribute
    {
        public AlgorithmAttribute(string name, string description) : base(name, description) { }

    }
}
