using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.CustomAttributes
{
    public class GaborDescriptorAttribute : FeatureDescriptorAtribute
    {
        public override string Name
        {
            get { return "Gabor Descriptors"; }
        }

        public override string Description
        {
            get { return "Por hacer"; }
        }
    }
}
