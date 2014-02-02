using System;
using System.Runtime.InteropServices;
using System.Drawing;


namespace TxEstudioKernel.OpenCV
{
    public static class CXCore
    {
        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvCreateImage", CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr cvCreateImage(Size size, uint depth, int channels);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvCloneImage", CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr cvCloneImage(IntPtr image);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvPtr2D", CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr cvPtr2D( IntPtr arr, int idx0, int idx1, IntPtr type);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvReleaseImage", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvReleaseImage(IntPtr imageDoublePtr);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvCopy", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvCopy( IntPtr src, IntPtr dst, IntPtr mask );

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvCreateMat", CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr cvCreateMat(int rows, int cols, int type);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvmGet", CallingConvention = CallingConvention.Winapi)]
        public static extern double cvmGet( IntPtr mat, int row, int col );

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvmSet", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvmSet(IntPtr mat, int row, int col, double value);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvReleaseMat", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvReleaseMat(IntPtr matDoublePtr);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvCartToPolar", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvCartToPolar(IntPtr X, IntPtr Y, IntPtr M, IntPtr A, int angle_in_degrees);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvMinMaxLoc", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvMinMaxLoc(IntPtr src, IntPtr minVal, IntPtr maxVal, IntPtr minLoc, IntPtr maxLoc, IntPtr mask);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvConvertScale", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvConvertScale(IntPtr src, IntPtr dst, double scale, double shift);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvConvertScaleAbs", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvConvertScaleAbs(IntPtr src, IntPtr dst, double scale, double shift);

        [DllImport("cxcore097.dll", EntryPoint = "cvDFT", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvDFT(IntPtr src, IntPtr dst, int flags, int nonzerorows);

        [DllImport("cxcore097.dll", EntryPoint = "cvGetOptimalDFTSize", CallingConvention = CallingConvention.Winapi)]
        public static extern int cvGetOptimalDFTSize(int size0);

        [DllImport("cxcore097.dll", EntryPoint = "cvMulSpectrums", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvMulSpectrums(IntPtr src1, IntPtr src2, IntPtr dst, int flags);

        [DllImport("cxcore097.dll", EntryPoint = "cvGetSubRect", CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr cvGetSubRect(IntPtr arr, IntPtr submat, Rectangle rect);

        [DllImport("cxcore097.dll", EntryPoint = "cvSetZero", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvSetZero(IntPtr arr);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvAvg", CallingConvention = CallingConvention.Winapi)]
        public static extern _cvScalar cvAvg(IntPtr CvArr, IntPtr mask);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvAvgSdv", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvAvgSdv(IntPtr CvArr, IntPtr mean, IntPtr std_dev, IntPtr mask);


        //  Matrix Operations

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvAdd", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvAdd(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvSub", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvSub(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvMul", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvMul(IntPtr src1, IntPtr src2, IntPtr dst);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvGEMM", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvGEMM(IntPtr A, IntPtr B, double alpha, IntPtr C, double beta, IntPtr D, int tABC);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvTranspose", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvTranspose(IntPtr src, IntPtr dst);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvInvert", CallingConvention = CallingConvention.Winapi)]
        public static extern double cvInvert(IntPtr src, IntPtr dst, int method);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvGetRows", CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr cvGetRows(IntPtr arr, IntPtr submat, int start_row, int end_row, int delta_row);


        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvEigenVV", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvEigenVV(IntPtr A, IntPtr evects, IntPtr evals, double eps);
       
        //añadido sandro

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvAnd", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvAnd(IntPtr source1, IntPtr source2, IntPtr destination, IntPtr mask);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvOr", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvOr(IntPtr source1, IntPtr source2, IntPtr destination, IntPtr mask);

        [DllImport(OpenCVLibraries.CXCorePath, EntryPoint = "cvXor", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvXor(IntPtr source1, IntPtr source2, IntPtr destination, IntPtr mask);

    }
}
