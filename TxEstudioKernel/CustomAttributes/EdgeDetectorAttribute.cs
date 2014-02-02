using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.CustomAttributes
{
    public class EdgeDetectorAttribute : TxEstudioKernel.CustomAttributes.TxCategoryAttribute
    {
        public override string Name
        {
            get { return "Edges Detectors"; }
        }

        public override string Description
        {
            get { return "These operators detec edges on images"; }
        }
    }
}
