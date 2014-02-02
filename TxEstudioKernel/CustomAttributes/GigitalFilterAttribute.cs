using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.CustomAttributes
{
    public class DigitalFilterAttribute : TxEstudioKernel.CustomAttributes.TxCategoryAttribute
    {
        public override string Name
        {
            get { return "Digital Filters"; }
        }

        public override string Description
        {
            get { return "These are filters"; }
        }
    }
}
