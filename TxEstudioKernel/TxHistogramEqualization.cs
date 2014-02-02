using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel
{
    [Algorithm("Histogram Equalization ", "Perform histogram equalization")]
 
    [Abbreviation("Hist_Equalization")]
        
        public class TxHistogramEqualization : TxOneBand
        {

            public override TxImage Process(TxImage input)
            {
                if (input.ImageFormat == TxImageFormat.RGB)
                {
                    return EqualizeRGB(input);
                }
                else 
                {
                    return EqualizeGrayScale(input);
                  
                }
            }

            private TxImage EqualizeRGB(TxImage input) 
            {

                unsafe
                {

                    _IplImage* innerImage = (_IplImage*)input.InnerImage;
                    byte* current = (byte*)innerImage->imageData;
                    int offset = innerImage->widthStep - 3 * innerImage->width;


                    int[] r_histogram = new int[256];
                    int[] g_histogram = new int[256];
                    int[] b_histogram = new int[256];
                    
                    for (int i = 0; i < r_histogram.Length; i++)
                    {
                        r_histogram[i] = 0;
                        g_histogram[i] = 0;
                        b_histogram[i] = 0;
                    }
                    for (int i = 0; i < input.Height; i++, current += offset)
                        for (int j = 0; j < input.Width; j++, current += 3)
                        {  
                           r_histogram[current[0]]++;
                           g_histogram[current[1]]++;
                           b_histogram[current[2]]++;
                        }

                    float[] r_LUT = Cumulative(r_histogram, input.Width * input.Height);
                    float[] g_LUT = Cumulative(g_histogram, input.Width * input.Height);
                    float[] b_LUT = Cumulative(b_histogram, input.Width * input.Height);


                    //Resultant image
                    TxImage res = input.CloneImage();


                    innerImage = (_IplImage*)res.InnerImage;
                    current = (byte*)innerImage->imageData;
                    offset = innerImage->widthStep - 3 * innerImage->width;
                    
                     int r_index = 0;
                     int g_index = 0;
                     int b_index = 0;

                     byte r_newval = 0;
                     byte g_newval = 0;
                     byte b_newval = 0;

                    for (int i = 0; i < input.Height; i++, current += offset)
                        for (int j = 0; j < input.Width; j++, current += 3)
                        {
                            r_index = current[0];
                            g_index = current[1];
                            b_index = current[2];
                            

                              r_newval =(byte)r_LUT[r_index];
                              g_newval =(byte)g_LUT[g_index];
                              b_newval =(byte)b_LUT[b_index];
                            
                            if (r_LUT[r_index] > 255)
                            {
                               r_newval = 255;
                            }
                            if (g_LUT[g_index] > 255)
                            {
                                g_newval = 255;
                            }
                            if (b_LUT[ b_index] > 255)
                            {
                                b_newval = 255;
                            }
                            current[0] = r_newval;
                            current[1] = g_newval;
                            current[2] = b_newval;

                        }

                    return res;
                }
            }
            
            
            private TxImage EqualizeGrayScale(TxImage input) 
            {
                unsafe
                {

                    _IplImage* innerImage = (_IplImage*)input.InnerImage;
                    byte* current = (byte*)innerImage->imageData;
                    int offset = innerImage->widthStep - innerImage->width;


                    int[] histogram = new int[256];
                    for (int i = 0; i < histogram.Length; i++)
                        histogram[i] = 0;

                    for (int i = 0; i < input.Height; i++, current += offset)
                        for (int j = 0; j < input.Width; j++, current += 1)
                        {
                            int index = current[0];
       
                            histogram[index]++;
                        }

                    float[] LUT = Cumulative(histogram, input.Width * input.Height);


                    //Resultant image
                    TxImage res = input.CloneImage();


                    innerImage = (_IplImage*)res.InnerImage;
                    current = (byte*)innerImage->imageData;
                    offset = innerImage->widthStep - innerImage->width;


                    for (int i = 0; i < input.Height; i++, current += offset)
                        for (int j = 0; j < input.Width; j++, current += 1)
                        {
                            int index = current[0];

                            byte newval = (byte)LUT[index];

                            if (LUT[index] > 255)
                            {
                                newval = 255;
                            }
                            current[0] = newval;

                        }

                    return res;
                }
            
            }

            public float[] Cumulative(int[] hist, long PixelCount)
            {
                float[] newhist = new float[256];

                newhist[0] = hist[0] * hist.Length/PixelCount;
                long prev = hist[0];
                for (int i = 1; i < hist.Length; i++)
                {
                    prev += hist[i];
                    newhist[i] = prev * hist.Length / PixelCount;
                }

                return newhist;

            }

        }


       
}
