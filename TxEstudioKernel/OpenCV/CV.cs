using System;
using System.Runtime.InteropServices;
using System.Drawing;


namespace TxEstudioKernel.OpenCV
{
    public static class CV
    {
        [DllImport(OpenCVLibraries.CVPath, EntryPoint = "cvSobel", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvSobel(IntPtr src, IntPtr result, int dx, int dy, int apertureSize);

        [DllImport(OpenCVLibraries.CVPath, EntryPoint = "cvCanny", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvCanny(IntPtr im, IntPtr edges, double threshold1, double threshold2, int apertureSize);

        [DllImport(OpenCVLibraries.CVPath, EntryPoint = "cvFilter2D", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvFilter2D(IntPtr source, IntPtr dest, IntPtr kernel, Point anchor);

        [DllImport(OpenCVLibraries.CVPath, EntryPoint = "cvCvtColor", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvCvtColor(IntPtr source, IntPtr dest, int code);

        [DllImport(OpenCVLibraries.CVPath, EntryPoint = "cvSmooth", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvSmooth(IntPtr source, IntPtr dest, int smoothtypeint, int param1, int param2, double param3);

        [DllImport(OpenCVLibraries.CVPath, EntryPoint = "cvCreateStructuringElementEx", CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr cvCreateStructuringElementEx(int cols, int rows, int anchor_x, int anchor_y, int shape, IntPtr values);

        [DllImport(OpenCVLibraries.CVPath, EntryPoint = "cvReleaseStructuringElement", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvReleaseStructuringElement(IntPtr element);

        [DllImport(OpenCVLibraries.CVPath, EntryPoint = "cvErode", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvErode(IntPtr src, IntPtr dst, IntPtr element, int iterations);

        [DllImport(OpenCVLibraries.CVPath, EntryPoint = "cvDilate", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvDilate(IntPtr src, IntPtr dst, IntPtr element, int iteration);

        [DllImport(OpenCVLibraries.CVPath, EntryPoint = "cvThreshold", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvThreshold(IntPtr src, IntPtr dst, double threshold, double max_value, int threshold_type);

        [DllImport(OpenCVLibraries.CVPath, EntryPoint = "cvLaplace", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvLaplace( IntPtr src, IntPtr dst, int aperture_size);

        [DllImport(OpenCVLibraries.CVPath, EntryPoint = "cvSampleLine", CallingConvention = CallingConvention.Winapi)]
        public static extern int cvSampleLine(IntPtr img, Point p1, Point p2, IntPtr buffer);

        [DllImport(OpenCVLibraries.CVPath, EntryPoint = "cvMorphologyEx", CallingConvention = CallingConvention.Winapi)]
        public static extern void cvMorphologyEx(IntPtr src, IntPtr dst, IntPtr temp, IntPtr element, int operation, int iterations);
    }
}
