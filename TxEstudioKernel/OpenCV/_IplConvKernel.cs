using System;
using System.Collections.Generic;
using System.Text;

namespace TxEstudioKernel.OpenCV
{
    public unsafe struct _IplConvKernel
    {

        int nCols;
        int nRows;
        int anchorX;
        int anchorY;
        int* values;
        int nShiftR;

    }
}
