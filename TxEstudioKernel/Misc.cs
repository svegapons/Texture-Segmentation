using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel
{
    public class Misc : TxEstudioKernel.CustomAttributes.TxCategoryAttribute
    {
        public override string Name
        {
            get { return "Misc"; }
        }

        public override string Description
        {
            get { return ""; }
        }
    }
}
