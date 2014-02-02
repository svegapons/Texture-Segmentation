using System;


namespace TxEstudioKernel.CustomAttributes
{
    /// <summary>
    /// Clase base de los atributos para la descripción de elementos.
    /// </summary>
    public abstract class TxDescriptorAttribute : TxCustomAttribute
    {
        private string name;

        public virtual string Name
        {
            get { return name; }
            
        }
        private string description;

        public virtual string Description
        {
            get { return description; }
           
        }
    
        public TxDescriptorAttribute(string name, string description)
        {
            this.name = name;
            this.description= description;
        }
    }
}
