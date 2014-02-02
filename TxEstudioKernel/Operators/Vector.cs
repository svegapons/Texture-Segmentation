using System;
using System.Collections.Generic;
using System.Text;


namespace TxEstudioKernel.Operators
{
    public class Vector:IClasificable
    {
        /// <summary>
        /// Arreglo de valores del Vector
        /// </summary>
        double[] valores;

        /// <summary>
        /// Se crea un vector con los valores especificados
        /// </summary>
        /// <param name="valores">Valores del vector</param>
        public Vector(double[] valores)
        {
            this.valores = valores;
        }
        /// <summary>
        /// Se crea un vector de la longitud especificada con los elementos en 0.
        /// </summary>
        /// <param name="longitud">Longitud del vector</param>
        public Vector(int longitud)
        {
            valores = new double[longitud];
        }

        /// <summary>
        /// Longitud del vector
        /// </summary>
        public int Longitud
        {
            get
            {
                return valores.Length;
            }
        }

        /// <summary>
        /// La suma del vector con otro
        /// </summary>
        public Vector Suma(Vector otro)
        {
            if (valores.Length != otro.Longitud) throw new ArgumentException("Los vectores deben tener la misma dimension");
            Vector resultado = new Vector(otro.Longitud);

            for (int i = 0; i < resultado.Longitud; i++)
                resultado[i] = valores[i] + otro[i];
            return resultado;
        }

        /// <summary>
        /// Division de un vector por un numero
        /// </summary>
        public Vector Division(double numero)
        {
            if (numero == 0) throw new ArgumentException("Division por cero no esta definida");
            Vector resultado = new Vector(valores.Length);

            for (int i = 0; i < valores.Length; i++)
                resultado[i] = valores[i] / numero;
            return resultado;

        }
        public Vector Producto(double numero)
        {
            Vector result = new Vector(valores.Length);

            for (int i = 0; i < valores.Length; i++)
                result[i] = valores[i] * numero;
            return result;
        }

        /// <summary>
        /// Norma euclidiana de un vector
        /// </summary>
        public double Norma()
        {
            double resultado = 0;
            for (int i = 0; i < valores.Length; i++)
                resultado = resultado + valores[i] * valores[i];
            resultado = System.Math.Sqrt(resultado);
            return resultado;
        }

        

        public double this[int i]
        {
            get
            {
                if (i >= valores.Length) throw new ArgumentException("El valor debe estar entre los limites del arreglo");
                return valores[i];
            }
            set
            {
                if (i >= valores.Length) throw new ArgumentException("El valor debe estar entre los limites del arreglo");
                valores[i] = value;
            }

        }

        /// <summary>
        /// Redefinicion del Equals para que compare por valor y no por referencia
        /// </summary>
        public override bool Equals(object obj)
        {
            Vector entrada = (Vector)obj;

            if (Longitud != entrada.Longitud) throw new ArgumentException("Los vectores deben tener la misma longitud");
            for (int i = 0; i < entrada.Longitud; i++)
            {
                if (valores[i] != entrada[i]) return false;
            }
            return true;
        }

        #region IClasificable Members

        public Vector Valor()
        {
            return this;
        }

        public  double Distancia(Vector otro)
        {
            double resultado = 0;
            for (int i = 0; i < valores.Length; i++)
                resultado = resultado + (valores[i] - otro[i]) * (valores[i] - otro[i]);
            //resultado = System.Math.Sqrt(resultado);
            return resultado;

        }

        #endregion

    }
}
