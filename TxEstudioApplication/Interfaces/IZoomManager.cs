using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioApplication.Interfaces
{
    /// <summary>
    /// Las ventanas que realizan manejo de zoom deben implementar esta interfaz.
    /// </summary>
    public interface IZoomManager
    {
        int ZoomLevel { get; set; }
    }
}
