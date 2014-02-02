using System;
using System.Runtime.InteropServices;

namespace TxEstudioKernel.OpenCV
{
    /// <summary>
    /// Esta es la estructura que usa OpenCV para almacenar la informacion referente a la imagen
    /// Los comentarios de los campos fueron extraidos de la documentacion de la OpenCV
    /// y la implementacion presente se hizo a partir de la expuesta por CVWrapper
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct _IplImage
    {
        public  int nSize;              // sizeof(_IplImage)
        public  int ID;                 // Version (=0)
        public  int nChannels;          // Most of OpenCV functions support 1,2,3 or 4 channels 

        private int alphaChannel;       // Ignored by OpenCV

        public  int depth;              // Pixel depth in bits

        private int colorModel;         // Ignored by OpenCV
        private int channelSeq;         // ditto ??

        public  int dataOrder;          // 0 - interleaved color channels, 1 - separate color channels. 
                                        // cvCreateImage can only create interleaved images 

        public  int origin;             // 0 - top-left origin,
                                        //  1 - bottom-left origin (Windows bitmaps style) 
        public  int align;              // Alignment of image rows (4 or 8).
                                        // OpenCV ignores it and uses widthStep instead
        public  int width;              // Image width in pixels
        public  int height;             // Image height in pixels
        public  IntPtr roi;             // Image ROI. when it is not NULL, 
                                        // this specifies image region to process

        private IntPtr maskROI;         // Must be NULL in OpenCV */

        private IntPtr imageID;         //ditto ??
        private IntPtr tileInfo;        //ditto ??

        public  int imageSize;          // Image data size in bytes
                                        // (=image->height*image->widthStep
                                        // in case of interleaved data)

        public  IntPtr imageData;       // Pointer to aligned image data 

        public  int widthStep;          // Size of aligned image row in bytes 

        //BorderMode border completion mode, ignored by OpenCV  
        private int pad0;
        private int pad1;
        private int pad2;
        private int pad3;

        //BorderConst ditto ??
        private int pad4;
        private int pad5;
        private int pad6;
        private int pad7;

        public  IntPtr imageDataOrigin; // Pointer to a very origin of image data
                                        // (not necessarily aligned) -
                                        // it is needed for correct image deallocation

    }

    public enum BitDepths : uint
    {
        IPL_DEPTH_8U = 8,
        IPL_DEPTH_8S = 2147483656,
        IPL_DEPTH_16S = 2147483664,
        IPL_DEPTH_32S = 2147483680,
        IPL_DEPTH_32F = 32,
        IPL_DEPTH_64F = 64,
    }
}

