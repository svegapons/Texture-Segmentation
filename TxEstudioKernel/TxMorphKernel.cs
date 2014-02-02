using System;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel
{
    /// <summary>
    /// Represents a kernel for morphological operations.
    /// Encapsulates the _IplConvKernel from OpenCV
    /// </summary>
    public unsafe class TxMorphKernel:IDisposable   
    {

        _IplConvKernel* kernel;

        public TxMorphKernel(int nRows, int nCols)
        {
            kernel = (_IplConvKernel*)CV.cvCreateStructuringElementEx(nCols, nRows, nCols/2,nRows/2 , 0, IntPtr.Zero);
        }

        ~TxMorphKernel()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

        public IntPtr InnerKernel
        {
            get
            {
                return (IntPtr)kernel;
            }
        }

        #region IDisposable Members


        bool disposed = false;
        public void Dispose()
        {
            if (!disposed)
            {
                fixed (_IplConvKernel** ptr = &kernel)
                {
                    CV.cvReleaseStructuringElement((IntPtr)ptr);
                }
                disposed = true;
            }
        }

        #endregion
    }
}
