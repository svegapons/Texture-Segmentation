using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.CustomAttributes
{
    public class MomentsDescriptorAttribute:FeatureDescriptorAtribute
    {
        public override string Name
        {
            get { return "Moments descriptors"; }
        }

        public override string Description
        {
            get { return ""; }
        }
    }
}
