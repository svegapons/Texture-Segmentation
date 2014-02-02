using System;
using System.Drawing.Printing;


namespace TxEstudioApplication.Interfaces
{
    /// <summary>
    /// Interface that provides printing functionalities.
    /// The methods on this interface should provide the implementation for PrintDocument events handling.
    /// </summary>
    public interface IPrintManager
    {
        void BeginPrint(object sender, PrintEventArgs e);
        void EndPrint(object sender, PrintEventArgs e);
        void QueryPageSettings(object sender, QueryPageSettingsEventArgs e);
        void PrintPage(object sender, PrintPageEventArgs e);
    }
}
