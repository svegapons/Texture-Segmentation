using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.CustomAttributes
{
    public class RunLenthDescriptorAttribute : FeatureDescriptorAtribute
    {
        public override string Name
        {
            get { return "Run Lenth Statistics"; }
        }

        public override string Description
        {
            get { return "Por hacer"; }
        }
    }
}
