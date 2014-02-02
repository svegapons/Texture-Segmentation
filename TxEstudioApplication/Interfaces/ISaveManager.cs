using System;

namespace TxEstudioApplication.Interfaces
{
    /// <summary>
    /// Las ventanas que ejecuten acciones de salvado debe n implementar esta interfaz.
    /// </summary>
    public interface ISaveManager
    {
        void Save();
        void SaveAs();
    }
}
