using System;
using TxEstudioKernel;
using System.Drawing;

namespace TxEstudioApplication.Interfaces
{
    /// <summary>
    /// Las ventanas que sean contenedoras de imagenes deben implementar esta interfaz.
    /// </summary>
    public interface IImageExposer
    {
        TxImage Image {get; set;}
        Bitmap Bitmap { get; set;}
        string ImageName { get; set;}
        event MoveOverImageEventHandler MouseOverImage;
    }
}
