using System;
using System.Runtime.InteropServices;


namespace TxEstudioKernel.OpenCV
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct _cvMat
    {
        public int type;          /* CvMat signature (CV_MAT_MAGIC_VAL), element type and flags */
        public int step;          /* full row length in bytes */
        public IntPtr refcount;   /* underlying data reference counter */
        unsafe public void* data; /* data pointers */
        public int rows;          /*number of rows*/
        public int cols;          /*numbre of columns*/
    }
}