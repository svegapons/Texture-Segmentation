using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.CustomAttributes
{
    public class FirstOrderStatisticDescriptorAttribute : FeatureDescriptorAtribute
    {
        public override string Name
        {
            get { return "First Order Statistics"; }
        }

        public override string Description
        {
            get { return "Por hacer"; }
        }
    }
}
