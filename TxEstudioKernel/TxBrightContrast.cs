using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel
{
     public class TxBrightContrast
    {
         int mean;
        double scale;
        double maxscale = 5;
        int newmean; 
        static int interval= 100;

        TxImage image_src;
        TxImage image_dst;
        Bitmap  bitmap_dst;
        public TxBrightContrast(TxImage image) 
        {
            Init(image);
               
        }
        public int Mean
        {
            get { return mean; }
        }
        public double Contrast
        {
            get { return scale; }
        }
        public int Bright
        {
            get { return newmean; }
        }
        public TxImage SrcImage 
        {
            get { return this.image_src; }
        }
        public TxImage CurrentImage 
        {
            get { return this.image_dst; }
        }
        public Bitmap CurrentBitmap
        {
          get { return this.bitmap_dst; }
        }
        public void AdjustBright(int val)
        {
            if (val <= interval && val >= -interval )
            {
                if (val > 0)
                {
                    this.newmean = mean + (int)( val /(double)interval *( 255 - mean));

                }
                else
                {
                    this.newmean = mean + (int)( val /(double)interval * mean);
                }
                if (image_src.ImageFormat == TxImageFormat.RGB && image_dst.ImageFormat == TxImageFormat.RGB)
                {

                    ShiftScaleMeanRGB();
                }
                else if (image_src.ImageFormat == TxImageFormat.GrayScale && image_src.ImageFormat == TxImageFormat.GrayScale)
                {
                    ShiftScaleMeanGrayScale();
                }
                else throw new Exception("Incorrect image format");
            }
            else throw new Exception("Bright value should be between " + interval + " and " + -interval ); 
        }
         
         public void AdjustContrast(int val)
         {
             if (val <= interval && val >= -interval )
             {

                 if( val > 0 )
                 {
                     this.scale = Math.Pow( 2 , val/(double)interval*maxscale);
                 }
                 else if (val < 0)
                 {

                    this.scale = (interval  + val) /(double)interval; 
                 }
                 else
                     this.scale = 1;
             

                 if (image_src.ImageFormat == TxImageFormat.RGB && image_dst.ImageFormat == TxImageFormat.RGB)
                 {
                     ShiftScaleMeanRGB();
                 }
                 else if (image_src.ImageFormat == TxImageFormat.GrayScale && image_src.ImageFormat == TxImageFormat.GrayScale)
                 {
                     ShiftScaleMeanGrayScale();
                 }
             }
             else throw new Exception("Contrast value should be between " + interval + " and " + -interval ); 
         }
         private void ShiftScaleMeanGrayScale() 
        {
            if ( newmean >= 0 && newmean  < 256 && scale >= 0 && scale < 128)
            {
                
                    unsafe
                    {
                        _IplImage* innerImage_src = (_IplImage*)image_src.InnerImage;
                        byte* current_src = (byte*)innerImage_src->imageData;

                        _IplImage* innerImage_dst = (_IplImage*)image_dst.InnerImage;
                        byte* current_dst = (byte*)innerImage_dst->imageData;

                        BitmapData bmpData = bitmap_dst.LockBits(new Rectangle(0, 0, bitmap_dst.Width, bitmap_dst.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed );
                        byte* bmp_dst = (byte*)bmpData.Scan0.ToPointer();
                        int bmp_offset = bmpData.Stride - bitmap_dst.Width;
                        
                        int offset = innerImage_src->widthStep - innerImage_src->width;

                        for (int i = 0; i < image_src.Height; i++, current_src += offset, current_dst += offset , bmp_dst +=  bmp_offset)
                            for (int j = 0; j < image_src.Width; j++, current_src ++, current_dst ++,bmp_dst ++)
                            {
                               
                               int gray = current_src[0];

                                gray =(int) ((gray - mean)*scale) + newmean;

                                if (gray < 0)
                                    gray = 0; 
                                else if (gray > 255)
                                {
                                   gray = 255;
                                }
                              
                                current_dst[0] = (byte)gray;
                                bmp_dst[0] = (byte)gray;
                            }
                           bitmap_dst.UnlockBits(bmpData);
                    }
               
            }
            else  throw new Exception("Val should be between 0 and 255"); 
        }

         private void ShiftScaleMeanRGB()
         {
             if (newmean >= 0 && newmean < 256 && scale >= 0 && scale < 128)
             {

                 unsafe
                 {
                     _IplImage* innerImage_src = (_IplImage*)image_src.InnerImage;
                     byte* current_src = (byte*)innerImage_src->imageData;

                     _IplImage* innerImage_dst = (_IplImage*)image_dst.InnerImage;
                     byte* current_dst = (byte*)innerImage_dst->imageData;

                     BitmapData bmpData = bitmap_dst.LockBits(new Rectangle(0, 0, bitmap_dst.Width, bitmap_dst.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                     byte* bmp_dst = (byte*)bmpData.Scan0.ToPointer();
                     int bmp_offset = bmpData.Stride - 3 * bitmap_dst.Width;

                     int offset = innerImage_src->widthStep - 3 * innerImage_src->width;

                     for (int i = 0; i < image_src.Height; i++, current_src += offset, current_dst += offset, bmp_dst += bmp_offset)
                         for (int j = 0; j < image_src.Width; j++, current_src += 3, current_dst += 3, bmp_dst += 3)
                         {
                             //red on current[2]
                             //blue on current[1]
                             //green on current[0]
                             // gray[(byte)(0.299f * current[2] + 0.587f * current[1] + 0.114f * current[0])]++;
                             int red = current_src[2];
                             int blue = current_src[1];
                             int green = current_src[0];

                             red = (int)((red - mean) * scale) + newmean;
                             blue = (int)((blue - mean) * scale) + newmean;
                             green = (int)((green - mean) * scale) + newmean;

                             if (red < 0)
                             { red = 0; }
                             else if (red > 255)
                             {
                                 red = 255;
                             }
                             if (blue < 0)
                             { blue = 0; }
                             else if (blue > 255)
                             {
                                 blue = 255;
                             }
                             if (green < 0)
                             {
                                 green = 0;
                             }
                             else if (green > 255)
                             {
                                 green = 255;
                             }
                             current_dst[2] = (byte)red;
                             current_dst[1] = (byte)blue;
                             current_dst[0] = (byte)green;

                             bmp_dst[2] = (byte)red;
                             bmp_dst[1] = (byte)blue;
                             bmp_dst[0] = (byte)green;
                         }
                     bitmap_dst.UnlockBits(bmpData);
                 }

             }
             else throw new Exception("Val should be between 0 and 255");
         }

        private void Init(TxImage image)
        {
            
            image_src = image; 
            image_dst = image.CloneImage();

            unsafe
            {
            

                _cvScalar means ;

                means = CXCore.cvAvg((IntPtr)image_src.InnerImage, (IntPtr)0);





                if (image.ImageFormat == TxImageFormat.RGB)
                {
                    mean = (int)(means.val0 + means.val1 + means.val2) / 3;
      
                    bitmap_dst = image.ToBitamp();
                   
                }
                else                //RGB 
                {
                    mean = (int)means.val0;
                  
                    bitmap_dst = image.ToBitamp();
                }

              
                scale = 1;
                newmean = mean;
            }
        }

         public void Reset() 
         {
             scale = 1;
             newmean = mean;
             if (SrcImage.ImageFormat == TxImageFormat.RGB)
             {
                 ShiftScaleMeanRGB();
             }
             else
                 ShiftScaleMeanGrayScale();
         }

       
    }
}
