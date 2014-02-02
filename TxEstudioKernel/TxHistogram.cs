using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel
{
    public class TxHistogram : TxObject
    {
        /// <summary>
        /// Datos del histograma.
        /// </summary>
        protected ChannelHistogram[] data = new ChannelHistogram[4];
    
        /// <summary>
        /// Construye el histograma a partir de la imagen dada.
        /// </summary>
        /// <param name="image">Instancia de la clase TxImage a partir de la cual se construira el histograma.</param>
        public TxHistogram(TxImage image)
        {
            if (image.ImageFormat == TxImageFormat.RGB)
                BuildFromRGB(image);
            else
                BuildFromGrayScale(image);

        }

        private void BuildFromRGB(TxImage image)
        {
            int[] gray  = new int[256];
            int[] red   = new int[256];
            int[] green = new int[256];
            int[] blue  = new int[256];


            unsafe
            {
                _IplImage* innerImage = (_IplImage*)image.InnerImage;
                byte* current = (byte*)innerImage->imageData;
                int offset = innerImage->widthStep - 3 * innerImage->width;

                for (int i = 0; i < image.Height; i++, current += offset)
                    for (int j = 0; j < image.Width; j++, current += 3)
                    {
                        //red on current[2]
                        //blue on current[1]
                        //green on current[0]
                        gray[(byte)(0.299f * current[2]+ 0.587f* current[1]+ 0.114f *current[0])]++;  
                        red[current[2]]++;
                        green[current[1]]++;
                        blue[current[0]]++;
                    }
            }
            data[0] = new ChannelHistogram(gray);
            data[1] = new ChannelHistogram(red);
            data[2] = new ChannelHistogram(green);
            data[3] = new ChannelHistogram(blue);
        }

        private void BuildFromGrayScale(TxImage image)
        {
            int[] grayScaleData = new int[256];
            unsafe
            {
                _IplImage* innerImage = (_IplImage*)image.InnerImage;
                byte* current = (byte*)innerImage->imageData;
                int offset = innerImage->widthStep - innerImage->width;

                for (int i = 0; i < image.Height; i++, current += offset)
                    for (int j = 0; j < image.Width; j++, current++)
                    {
                        //Value on *current
                        grayScaleData[*current]++;
                    }
            }
            data[0] = new ChannelHistogram(grayScaleData);
            data[1] = data[2] = data[3] = data[0];
        }

        /// <summary>
        /// Devuelve los valores del histograma para la escala de gris.
        /// </summary>
        public ChannelHistogram GrayScale
        {
            get
            {
                return data[0];
            }
        }

        /// <summary>
        /// Devuelve los valores del  histograma para el canal rojo.
        /// </summary>
        public ChannelHistogram Red
        {
            get
            {
                return data[1];
            }
        }

        /// <summary>
        /// Devuelve los valores del  histograma para el canal verde.
        /// </summary>
        public ChannelHistogram Green
        {
            get
            {
                return data[2];
            }
        }

        /// <summary>
        /// Devuelve los valores del  histograma para el canal azul.
        /// </summary>
        public ChannelHistogram Blue
        {
            get
            {
                return data[3];
            }
        }

        public ChannelHistogram this[int channel]
        {
            get
            {
                return data[channel];
            }
        }
    }

    public class ChannelHistogram
    {
        /// <summary>
        /// Los datos del histograma para cada frecuencia.
        /// </summary>
        int[] data;
        public ChannelHistogram(int[] data)
        {
            if (data.Length != 256)
                throw new ArgumentException("Histogram data for one color channel must have lenght 256.");
            this.data = data;
            maxFreq = int.MinValue;
            minFreq = int.MaxValue;
            mean    = 0.0f;
            stdv    = 0.0f;
            int current = 0;
            int total   = 0;
            for (int i = 0; i < 256; i++)
            {
                current = data[i];
                //min = (current!=0&&)?:;
                maxFreq = Math.Max(current, maxFreq);
                minFreq = Math.Min(current, minFreq);
                total   += current;
            }

            mean = total / 256; 
            double aux = 0;
            //Stdv calculo tomado de iplab
            for (int i = 0; i < 256; i++)
            {
                aux = i-mean;
                stdv += aux * aux * data[i];
            }
            stdv = Math.Sqrt(stdv / total);

            for (minimum = 0; minimum <= 255; minimum++)
                if (data[minimum] != 0)
                    break;
            for (maximum = 255; maximum < 0; maximum--)
                if (data[maximum] != 0)
                    break;

        }

        public int this[int frequency]
        {
            get
            {
                return data[frequency];
            }
        }

        byte maximum;
        public byte Maximum
        {
            get
            { return maximum; }
        }

        byte minimum;

        public byte Minimum
        {
            get { return minimum; }
        }

        int maxFreq;

        public int MaxFreq
        {
            get { return maxFreq; }
            set { maxFreq = value; }
        }

        int minFreq;

        public int MinFreq
        {
            get { return minFreq; }
            set { minFreq = value; }
        }


        double mean;

        public double Mean
        {
            get { return mean; }
        }

        double stdv;

        public double Stdv
        {
            get { return stdv; }
        }
    }
}
