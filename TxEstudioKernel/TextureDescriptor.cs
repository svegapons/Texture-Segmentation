using System;
using System.Collections.Generic;


namespace TxEstudioKernel
{
    /// <summary>
    /// Clase base de todos los descriptores de textura implementados en TxEstudio
    /// </summary>
    public abstract class TextureDescriptor: TxAlgorithm
    {
        /// <summary>
        /// Implementacion del calculo de la caracteristica de textura
        /// </summary>
        /// <param name="image">Imagen a calcularle la caracteristia o descripcion de textura.</param>
        /// <returns>La matriz que represena la descripcion</returns>
        public abstract TxMatrix GetDescription(TxImage image);
        protected virtual TxMatrix CopyRectangle(TxMatrix matriz, System.Drawing.Point verticeSup, System.Drawing.Point dest, int alto, int ancho)
        {
            TxMatrix result = matriz;

            for (int j = 0; j < alto; j++)
                for (int i = 0; i < ancho; i++)
                {
                    result[dest.X + i, dest.Y + j] = matriz[verticeSup.X + i, verticeSup.Y + j];
                }
            return result;

        }
    }

    /// <summary>
    /// Clase base de los descriptortes de textura que a su vez agrupan varios descriptores de una familia
    /// y pueden funcionar como un enumerador de descriptores
    /// </summary>
    public abstract class TextureDescriptorSequence : TextureDescriptor, IEnumerator<TextureDescriptor>
    {
        public override TxMatrix GetDescription(TxImage image)
        {
            return Current.GetDescription(image);
        }

        #region IEnumerator<TextureDescriptor> Members

        public abstract TextureDescriptor Current {get;}

        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            //pass
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public abstract bool MoveNext();
        
        public abstract  void Reset();
        
        #endregion
    }

    /// <summary>
    /// Funciona como una cola de descriptores.
    /// </summary>
    public class GeneralDescriptorSequence : TextureDescriptorSequence
    {
        LinkedList<TextureDescriptor> queue = new LinkedList<TextureDescriptor>();
        LinkedListNode<TextureDescriptor> current = null;


        public void Add(TextureDescriptor descriptor)
        {
            queue.AddLast(descriptor);
        }


        public override TextureDescriptor Current
        {
            get
            {
                if (current.Value is TextureDescriptorSequence)
                    return (current.Value as TextureDescriptorSequence).Current;
                return current.Value;
            }
        }

        public override bool MoveNext()
        {

            if (current == null)
                current = queue.First;
            else if (current.Value is TextureDescriptorSequence)
            {
                if ((current.Value as TextureDescriptorSequence).MoveNext())
                    return true;
                else
                    current = current.Next;
            }
            else
                current = current.Next;
            while (current != null)
            {
                if (current.Value is TextureDescriptorSequence)
                {
                    (current.Value as TextureDescriptorSequence).Reset();
                    if ((current.Value as TextureDescriptorSequence).MoveNext())
                        return true;
                    else
                        current = current.Next;
                }
                else
                    return true;
            }
            return false;
        }

        public override void Reset()
        {
            current = null;//queue.First;
        }

    }

}
