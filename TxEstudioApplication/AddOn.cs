using System;
using System.Windows.Forms;

namespace TxEstudioApplication
{

    /// <summary>
    /// Base class for TxEstudioApplication add-ons
    /// </summary>
    public abstract class AddOn
    {
        protected Environment env;

        public AddOn(Environment env)
        {
            this.env = env;
        }
        public abstract ToolStripItem GetAddOnMenu();
    }
}
