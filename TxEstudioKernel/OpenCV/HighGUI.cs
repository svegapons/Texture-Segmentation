using System;
using System.Runtime.InteropServices;


namespace TxEstudioKernel.OpenCV
{
    public static class HighGUI
    {
        [DllImport(OpenCVLibraries.HighGUIPath, EntryPoint = "cvLoadImage", CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr cvLoadImage(string fileName, int isColour);

        [DllImport(OpenCVLibraries.HighGUIPath, EntryPoint = "cvSaveImage", CallingConvention = CallingConvention.Winapi)]
        public static extern int cvSaveImage(string fileName, IntPtr img);
    }
}
