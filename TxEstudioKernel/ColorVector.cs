using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel
{
    public struct ColorVector
    {
        private float red;
        private float green;
        private float blue;
        /// <summary>
        /// Construye una instancia de ColorVector a partir de una instancia de <ref>System.Drawing.Color</ref>
        /// </summary>
        public ColorVector(System.Drawing.Color color)
        {
            red   = color.R;
            green = color.G;
            blue  = color.B;
        }

        /// <summary>
        /// Construye una instancia de ColorVector a partir de los valores para sus canales.
        /// </summary>
        /// <param name="red">Valor del canal rojo.</param>
        /// <param name="green">Valor del canal verde.</param>
        /// <param name="blue">Valor del canal azul.</param>
        public ColorVector(float red, float green, float blue)
        {
            this.red   = red;
            this.green = green;
            this.blue  = blue;
        }
        /// <summary>
        /// Permite obtener o establecer el canal rojo del color.
        /// </summary>
        public float Red
        {
            get
            {
                return red;
            }
            set
            {
                red = value;
            }
        }

        /// <summary>
        /// Permite obtener o establecer el canal verde del color.
        /// </summary>
        public float Green
        {
            get
            {
                return green;
            }
            set
            {
                green = value;
            }
        }

        /// <summary>
        /// Permite obtener o establecer el canal azul del color.
        /// </summary>
        public float Blue
        {
            get
            {
                return blue;
            }
            set
            {
                blue = value;
            }
        }
        public void SetValues(float red, float green, float blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }
    }
}
