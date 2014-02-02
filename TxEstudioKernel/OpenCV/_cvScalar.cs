using System;
using System.Runtime.InteropServices;

namespace TxEstudioKernel.OpenCV
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct _cvScalar
    {
       public double val0;
       public double val1;
       public double val2;
       public double val3;

    }
}
