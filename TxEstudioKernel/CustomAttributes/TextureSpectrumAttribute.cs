using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.CustomAttributes
{
    public class TextureSpectrumDescriptorAttribute : FeatureDescriptorAtribute
    {
        public override string Name
        {
            get { return "Statistics of Texture Spectrum"; }
        }

        public override string Description
        {
            get { return "Por hacer"; }
        }
    }
}
