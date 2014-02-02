
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;

using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel
{
    /// <summary>
    /// Represents an image on the RGB space or in gray scales.
    /// Encapsulates IplImage from OpenCV.
    /// </summary>
    [Serializable]
    public unsafe class TxImage : TxObject, ICloneable, IDisposable 
    {

        _IplImage* innerImage;

        public TxImage(_IplImage* image) { innerImage = image; }

        /// <summary>
        /// Creates an instance of an empty image with the geiven dimensions.
        /// </summary>
        /// <param name="width">Width of the new image.</param>
        /// <param name="height">Height of the new image.</param>
        public TxImage(int width, int height, TxImageFormat format)
        {
             innerImage = (_IplImage*)CXCore.cvCreateImage(new Size(width, height), 8, (int)format);
        }
        /// <summary>
        /// Creates an instance of an image based on the given System.Drawing.Image instance. 
        /// </summary>
        /// <remarks>Only 8bpp, 24bpp and 32bpp are supported. On 32bpp alpha channel is ignored. </remarks>
        /// <param name="image">Original image.</param>
        public TxImage(System.Drawing.Image image):this(new System.Drawing.Bitmap(image))
        {
            
        }
                
        /// <summary>
        /// Creates an instance of an image based on the given System.Drawing.Bitmap instance. 
        /// </summary>
        public TxImage(System.Drawing.Bitmap bitmap)
        {
            //TODO: Un metodo para obtener el bitmap de los 32 bpp pues el clipboard lo guarda con ese formato
            if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
                Load24bppRGB(bitmap);
            else if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
                Load8bpp(bitmap);
            else if (bitmap.PixelFormat == PixelFormat.Format32bppArgb)
                Load32bppArgb(bitmap);
            else if (bitmap.PixelFormat == PixelFormat.Format32bppRgb)
                Load32bppRgb(bitmap);
            else
                throw new ArgumentException("Bitmap format not supported ");
            
        }
        #region Bitmaps loading methods by format
        private void Load24bppRGB(Bitmap bitmap)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int srcOffset = bmpData.Stride - bitmap.Width * 3;
            
            innerImage = (_IplImage*)CXCore.cvCreateImage(bitmap.Size, 8, 3);
            int destOffset = innerImage->widthStep - innerImage->width * 3;

            byte* dest = (byte*)innerImage->imageData;
            byte* src = (byte*)(void*)bmpData.Scan0;

            int height = bitmap.Height;
            int width  = 3*bitmap.Width;

            for (int i = 0; i < height; i++, dest += destOffset, src += srcOffset)
                for (int j = 0; j < width; j++, dest++, src++)
                    *dest = *src;

            bitmap.UnlockBits(bmpData);
        }
        private void Load8bpp(Bitmap bitmap) 
        {
#warning Los colores indexados se estan tratando como escalas de gris 
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            int srcOffset = bmpData.Stride - bitmap.Width;
            
            innerImage = (_IplImage*)CXCore.cvCreateImage(bitmap.Size, 8, 3);
            int destOffset = innerImage->widthStep - innerImage->width;

            byte* dest = (byte*)innerImage->imageData;
            byte* src = (byte*)(void*)bmpData.Scan0;

            for (int i = 0; i < bitmap.Height; i++, dest += destOffset, src += srcOffset)
                for (int j = 0; j <  bitmap.Width; j++, dest++, src++)
                    *dest = *src;

            bitmap.UnlockBits(bmpData);
        }

        private void Load32bppArgb(Bitmap bitmap)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int srcOffset = bmpData.Stride - bitmap.Width * 4;

            innerImage = (_IplImage*)CXCore.cvCreateImage(bitmap.Size, 8, 3);
            int destOffset = innerImage->widthStep - innerImage->width * 3;

            byte* dest = (byte*)innerImage->imageData;
            byte* src = (byte*)(void*)bmpData.Scan0;

            int height = bitmap.Height;
            int width  = bitmap.Width;

            byte a = 0, b = 0, c=0;

            for (int i = 0; i < height; i++, dest += destOffset, src += srcOffset)
                for (int j = 0; j < width; j++, dest+=3, src+=4)
                {
                    dest[0] = a = src[0];
                    dest[1] = b = src[1];
                    dest[2] = c = src[2];
                }

            bitmap.UnlockBits(bmpData);
        }

        private void Load32bppRgb(Bitmap bitmap)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
            int srcOffset = bmpData.Stride - bitmap.Width * 4;

            innerImage = (_IplImage*)CXCore.cvCreateImage(bitmap.Size, 8, 3);
            int destOffset = innerImage->widthStep - innerImage->width * 3;

            byte* dest = (byte*)innerImage->imageData;
            byte* src = (byte*)(void*)bmpData.Scan0;

            int height = bitmap.Height;
            int width = bitmap.Width;

            byte a = 0, b = 0, c = 0;

            for (int i = 0; i < height; i++, dest += destOffset, src += srcOffset)
                for (int j = 0; j < width; j++, dest += 3, src += 4)
                {
                    dest[0] = a = src[0];
                    dest[1] = b = src[1];
                    dest[2] = c = src[2];
                }

            bitmap.UnlockBits(bmpData);
        }
#endregion


        /// <summary>
        /// Gets the width of the image.
        /// </summary>
        public int Width
        {
            get
            {
                return innerImage->width;
            }
            
        }

        /// <summary>
        /// Gets the Height of the image.
        /// </summary>
        public int Height
        {
            get
            {
               return  innerImage->height;
            }
            
        }

        /// <summary>
        /// Gets the corresponding size of the image.
        /// </summary>
        public System.Drawing.Size Size
        {
            get
            {
                return new System.Drawing.Size(innerImage->width, innerImage->height);
            }
            
        }

        /// <summary>
        /// Gets or sets gray scale pixel.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <remarks>If the image is on the RGB space sets the value on the three channels.</remarks>
        /// <returns></returns>
        public byte this[int x, int y]
        {
            get
            {
                if (innerImage->nChannels == 1)
                    return ((byte*)innerImage->imageData + innerImage->widthStep * y)[x];
                else
                {
                    byte* current = (byte*)innerImage->imageData + innerImage->widthStep * y;
                    return (byte)(0.2125f * current[x+2] + 0.7154f * current[x+1] + 0.0712f * current[x]);
                }
            }
            set
            {
                if (innerImage->nChannels == 1)
                    ((byte*)innerImage->imageData + innerImage->widthStep * y)[x] = value;
                else
                {
                    byte* current = (byte*)innerImage->imageData + innerImage->widthStep * y;
                    current[x + 2] = current[x + 1] = current[x] = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the value of the pixel in the specified channel.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <param name="channel">Color channel to get or set.</param>
        /// <remarks>If the image is in gray scale, the channel is ignored.</remarks>
        /// <returns></returns>
        public byte this[int x, int y, ColorChannel channel]
        {
            get
            {
                int offset = (int)channel;
                if (innerImage->nChannels == 1)
                    offset = 0;
                return ((byte*)innerImage->imageData + innerImage->widthStep * y)[x + offset];
            }
            set 
            {
                int offset = (int)channel;
                if (innerImage->nChannels == 1)
                    offset = 0;
                ((byte*)innerImage->imageData + innerImage->widthStep * y)[x + offset] = value;
            }
        }

        /// <summary>
        /// Sets the value of a pixel in the RGB space using the System.Drawing.Color instance.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <param name="color">The instance of System.Drawing.Color that contains the color values.</param>
        /// <remarks>If the image is in gray scale throws a System.InavlidOperationException.
        /// Alpha channel information is ignored.</remarks>
        public void SetColor(int x, int y, Color color)
        {
            if (innerImage->nChannels == 3)
            {
                byte* pixel = (byte*)innerImage->imageData + innerImage->widthStep * y;
                pixel[0] = color.B;
                pixel[1] = color.G;
                pixel[2] = color.R;

            }
            else
                throw new InvalidOperationException("Can't assign a color to a gray-scale image");
        }

        /// <summary>
        /// Gets an instance of System.Drawing.Color with the color values of pixel in x,y.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <remarks>If the image is in gray scale it returns a System.Drawing.Color with the same value information on the three channels.</remarks>
        public Color GetColor(int x, int y) 
        {
            byte* pixel = (byte*)innerImage->imageData + innerImage->widthStep * y;
            if (innerImage->nChannels == 1)
                return Color.FromArgb(pixel[0], pixel[0], pixel[0]);
            return Color.FromArgb(pixel[2], pixel[1], pixel[0]);
        }


        /// <summary>
        /// Gets ann instance of System.Drawing.Color that represents the image.
        /// </summary>
        /// <returns>A 8bpp bitmap if the image is in gray scale or a 24bpp if image is on RGB space.</returns>
        public Bitmap ToBitamp()
        {
            if (innerImage->nChannels == 1)
            {
                return to8bpp(); 
            }
            else
            {
                return to24bpp();
            }
        }
        #region Bitmap conversion methods by formats
        private Bitmap to8bpp()
        {
            Bitmap result = new Bitmap(innerImage->width, innerImage->height, PixelFormat.Format8bppIndexed);
            ColorPalette palette = result.Palette;
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            result.Palette = palette;
            BitmapData resultData = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            byte* src = (byte*)innerImage->imageData;
            byte* dest = (byte*)resultData.Scan0.ToPointer();
            int destOffset = resultData.Stride - result.Width;
            int srcOffset = innerImage->widthStep - innerImage->width;

            for (int i = 0; i < Height; i++, dest += destOffset, src+=srcOffset)
                for (int j = 0; j < Width; j++,  src++, dest++)
                    *dest = *src;

            result.UnlockBits(resultData);
            return result;
        }

        private Bitmap to24bpp()
        {
            Bitmap result = new Bitmap(innerImage->width, innerImage->height, PixelFormat.Format24bppRgb);
            BitmapData resultData = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            byte* src = (byte*)innerImage->imageData;
            byte* dest = (byte*)resultData.Scan0.ToPointer();
            int destOffset = resultData.Stride - 3 * result.Width;
            int srcOffset = innerImage->widthStep - innerImage->width * 3;
            int byteWidth = 3 * result.Width;

            for (int i = 0; i < result.Height; i++, dest += destOffset, src += srcOffset)
                for (int j = 0; j < byteWidth; j++, src++, dest++)//, src += 3, dest+=3)
                    *dest = *src;

            result.UnlockBits(resultData);
            return result;
        }
        #endregion

        /// <summary>
        /// Gets a pointer of the encapsulated IplImage
        /// </summary>
        public IntPtr InnerImage
        {
            get
            {
                return (IntPtr)innerImage;
            }
        }

        /// <summary>
        /// Gets the instance of the image stored  in the given file.
        /// </summary>
        /// <param name="path">The path of the file that contains the image.</param>
        /// <remarks>Supported formats:
        ///     OpenCV:
        ///             Windows bitmaps - BMP, DIB; 
        ///             JPEG files - JPEG, JPG, JPE; 
        ///             Portable Network Graphics - PNG; 
        ///             Portable image format - PBM, PGM, PPM; 
        ///             Sun rasters - SR, RAS; 
        ///             TIFF files - TIFF, TIF. 
        ///     Others:
        ///             DICOM - DCM 
        /// </remarks>
        public static TxImage LoadImageFrom(string path)
        {
            //TODO: El resto de los formatos no soportados aun
            _IplImage* image = (_IplImage*)HighGUI.cvLoadImage(path, -1);
            if (image != null)
                return new TxImage(image);
            return null;
        }

        /// <summary>
        /// Saves the image on the specified file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <remarks>The format will be determinated by the extension of the file in the given path.</remarks>
        public void Save(string path)
        {
            //TODO: El resto de los formatos no soportados
            HighGUI.cvSaveImage(path, (IntPtr)this.innerImage);
        }


        /// <summary>
        /// Gets the format of the image.
        /// </summary>
        public TxImageFormat ImageFormat
        {
            get 
            {
                if (innerImage->nChannels == 1)
                    return TxImageFormat.GrayScale;
                return TxImageFormat.RGB;
            }
        }

        /// <summary>
        /// Applies a convolution on the image using the given kernel.
        /// </summary>
        /// <param name="matrix">Convolution kernel.</param>
        /// <returns>A new image resulting from the convolution application.</returns>
        public TxImage Convolve(TxMatrix matrix)
        {
            TxImage result = new TxImage(innerImage->width, innerImage->height, this.ImageFormat);
            CV.cvFilter2D((IntPtr)innerImage, result.InnerImage, matrix.InnerMatrix, new Point(-1, -1));
            return result;
        }

        /// <summary>
        /// Gets a copy of the image.
        /// </summary>
        /// <returns></returns>
        public TxImage CloneImage()
        {
            return new TxImage((_IplImage*)CXCore.cvCloneImage((IntPtr)innerImage));
        }

        #region ICloneable Members


        /// <summary>
        /// Gets a copy of the image.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return CloneImage();
        }

        #endregion

        /// <summary>
        /// Converts the image to a gray scale image.
        /// </summary>
        /// <returns>An image on gray scale.</returns>
        /// <remarks>If the image already has gray scale values, returns a clone.</remarks>
        public TxImage ToGrayScale()
        {
            if (innerImage->nChannels == 1)
                return CloneImage();
            TxImage result = new TxImage(innerImage->width, innerImage->height, TxImageFormat.GrayScale);
            CV.cvCvtColor((IntPtr)innerImage, (IntPtr)result.innerImage, 6);
            return result;
        }

        /// <summary>
        /// Gets an image on RGB space.
        /// </summary>
        /// <returns>A new image on RGB space.</returns>
        /// <remarks>If the image was already  on RGB space returns a clone.</remarks>
        public TxImage ToColor()
        {
            if (innerImage->nChannels == 3)
                return CloneImage();
            TxImage result = new TxImage(innerImage->width, innerImage->height, TxImageFormat.RGB);
            CV.cvCvtColor((IntPtr)innerImage, (IntPtr)result.innerImage, 8);
            return result;
        }

        #region IDisposable Members

        /// <summary>
        /// Performs a cvReleaseImage method call. 
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                fixed (_IplImage** ptr = &innerImage)
                {
                    CXCore.cvReleaseImage((IntPtr)ptr);
                }
                disposed = true;
            }
        }

        bool disposed = false;

        #endregion

        /// <summary>
        /// Returns a hash string
        /// </summary>
        /// <returns></returns>
        public string GetHash()
        {
            long result = 0;
            int byteWidth = (ImageFormat == TxImageFormat.GrayScale) ? Width : 3 * Width;
            //En escala de gris
            byte* src = (byte*)innerImage->imageData;
            int srcOffset = innerImage->widthStep - byteWidth;
            for (int i = 0; i < Height; i++, src += srcOffset)
                for (int j = 0; j < byteWidth; j++, src++)
                    result = (i * byteWidth + j) * (*src);
            return result.ToString();
        }

        ~TxImage()
        {
            Dispose();

            GC.SuppressFinalize(this);
        }
    }
    /// <summary>
    /// Indicates a channel on the RGB space.
    /// </summary>
    public enum ColorChannel:int {Red = 2,Green = 1, Blue = 0 }
    /// <summary>
    /// Represets TxImage formats. 
    /// </summary>
    public enum TxImageFormat : int 
    {
        /// <summary>
        /// IMage on gray scale.
        /// </summary>
        GrayScale = 1, 

        /// <summary>
        /// Image on RGB space.
        /// </summary>
        RGB = 3 
    }
}

