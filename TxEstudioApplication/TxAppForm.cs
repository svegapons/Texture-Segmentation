using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TxEstudioApplication
{
    /// <summary>
    /// Todas las ventanas que seran hijas de la ventana principal de esta aplicacion deben heredar de esta clase.
    /// </summary>
    public partial class TxAppForm : Form
    {
        public TxAppForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Instancia del environment activo.
        /// </summary>
        protected Environment env;

        /// <summary>
        /// Este metodo asigan la instancia del environment activo.
        /// </summary>
        /// <param name="env">La instancia a asignar.</param>
        public virtual void SetEnvironment(Environment env)
        {
            this.env = env;
        }
    }
}