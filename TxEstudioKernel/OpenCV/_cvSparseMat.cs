using System;
using System.Runtime.InteropServices;

namespace TxEstudioKernel.OpenCV
{
     [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
     public struct _cvSparseMat
    {
        int type; /* CvSparseMat signature (CV_SPARSE_MAT_MAGIC_VAL), element type and flags */
        int dims; /* number of dimensions */
        IntPtr refcount; /* reference counter - not used */
        IntPtr heap; /* a pool of hashtable nodes */
        unsafe void** hashtable; /* hashtable: each entry has a list of nodes
                             having the same "hashvalue modulo hashsize" */
        int hashsize; /* size of hashtable */
        int total; /* total number of sparse array nodes */
        int valoffset; /* value offset in bytes for the array nodes */
        int idxoffset; /* index offset in bytes for the array nodes */
        int dim1; /* arr of dimension sizes */
        int dim2;
     }
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
     public struct Dims
     {
         int d1;
         int d2;
         public Dims(int d1,int d2) 
         {
             this.d1 = d1;
             this.d2 = d2; 
         }
      }

}
