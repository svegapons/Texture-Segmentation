using System;

namespace TxEstudioKernel.CustomAttributes
{
    public class ParameterAttribute : TxDescriptorAttribute
    {
        public ParameterAttribute(string name, string description) : base(name, description) { }

    }
}
