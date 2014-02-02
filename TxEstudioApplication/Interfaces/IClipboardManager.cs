using System;


namespace TxEstudioApplication.Interfaces
{

    /// <summary>
    /// Las ventanas que hagan manejos con el clipboard deben implementar esta interfaz.
    /// </summary>
    public interface IClipboardManager
    {
        bool CanCopyToClipboard();
        void CopyToClipboard();

        bool CanGetFromClipboard();
        void GetFromClipboard();
    }
}
