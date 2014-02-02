using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.CustomAttributes
{
    public class CoocurrenceMatrixDescriptorAttribute : FeatureDescriptorAtribute
    {
        public override string Name
        {
            get { return "Co-Ocurrence Matrix Statistics"; }
        }

        public override string Description
        {
            get { return "Por hacer"; }
        }
    }
}
