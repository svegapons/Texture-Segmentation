using System;
using TxEstudioKernel.VisualElements;
using System.Windows.Forms;


namespace TxEstudioKernel.CustomAttributes
{
    /// <summary>
    /// La aplicación de este atributo indica el uso de un editor personalizado.
    /// </summary>
    public class CustomParameterEditorAttribute : TxConstraintAttribute 
    {
        Type typeOfEditor;
        public CustomParameterEditorAttribute(Type typeOfEditor)
        {
            if (typeOfEditor.IsSubclassOf(typeof(Control)) && typeOfEditor.GetInterface("TxEstudioKernel.VisualElements.IParameterEditor")!=null && typeOfEditor.GetConstructor(new Type[] { }) != null)
                this.typeOfEditor = typeOfEditor;
        }
        public override IParameterEditor GetEditorFor(System.Reflection.PropertyInfo parameterProperty)
        {
            GeneralParameterEditor generalEditor = new GeneralParameterEditor();
            generalEditor.Editor = (IParameterEditor)Activator.CreateInstance(typeOfEditor);
            return generalEditor;
        }

        public override bool IsCompliantWith(Type parameterType)
        {
            //No se realiza ningun chequeo, para poner uno custom, hay que estar seguro de lo que se hace
            return true;
        }
    }
}
