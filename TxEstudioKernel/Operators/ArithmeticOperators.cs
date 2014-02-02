using System;
using TxEstudioKernel.OpenCV;
using TxEstudioKernel.CustomAttributes;


namespace TxEstudioKernel.Operators
{

    [Algorithm("Summation", "Calculates the summation from the given images")]
    [Abbreviation("summation")]
    public class TxSummation:TxMultiBand
    {
        protected unsafe void Add(int[,] accumulator, TxImage image)
        {
            _IplImage* innerImage = (_IplImage*)image.InnerImage;
            byte* current = (byte*)innerImage->imageData;
            int offset = innerImage->widthStep - innerImage->width;

            for (int i = 0; i < image.Height; i++, current += offset)
                for (int j = 0; j < image.Width; j++, current++)
                    accumulator[i, j] += *current;
        }

        public override TxImage Process(params TxImage[] images)
        {
            TxImage[] gray = new TxImage[images.Length];
            for (int i = 0; i < gray.Length; i++)
            {
                if (images[i].ImageFormat != TxImageFormat.GrayScale)
                    gray[i] = images[i].ToGrayScale();
                else
                    gray[i] = images[i];
            }

            int[,] accumulator = new int[images[0].Height, images[0].Width];
            for (int i = 0; i < images.Length; i++)
                Add(accumulator, gray[i]);

            int max=int.MinValue, min = int.MaxValue;
            for (int i = 0; i < accumulator.GetLength(0); i++)
            {
                for (int j = 0; j < accumulator.GetLength(1); j++)
                {
                    max = Math.Max(max, accumulator[i, j]);
                    min = Math.Min(min, accumulator[i,j]);
                }
            }
            float ratio = 255f / (max - min);

            unsafe
            {
                TxImage result = new TxImage(accumulator.GetLength(1), accumulator.GetLength(0) , TxImageFormat.GrayScale);
                _IplImage* innerImage = (_IplImage*)result.InnerImage;
                byte* current = (byte*)innerImage->imageData;
                int offset = innerImage->widthStep - innerImage->width;

                for (int i = 0; i < result.Height; i++, current += offset)
                    for (int j = 0; j < result.Width; j++, current++)
                       *current = (byte)((accumulator[i, j] - min) * ratio);
               return result;
            }
        }
    }


    [Algorithm("Average", "Calculates the average from the given images")]
    [Abbreviation("average")]
    public class TxAverage : TxSummation
    {
        public override TxImage Process(params TxImage[] images)
        {
            TxImage[] gray = new TxImage[images.Length];
            for (int i = 0; i < gray.Length; i++)
            {
                if (images[i].ImageFormat != TxImageFormat.GrayScale)
                    gray[i] = images[i].ToGrayScale();
                else
                    gray[i] = images[i];
            }

            int[,] accumulator = new int[images[0].Height, images[0].Width];
            for (int i = 0; i < images.Length; i++)
                Add(accumulator, gray[i]);


            float ratio = 1f / images.Length;

            unsafe
            {
                TxImage result = new TxImage(accumulator.GetLength(1), accumulator.GetLength(0), TxImageFormat.GrayScale);
                _IplImage* innerImage = (_IplImage*)result.InnerImage;
                byte* current = (byte*)innerImage->imageData;
                int offset = innerImage->widthStep - innerImage->width;

                for (int i = 0; i < result.Height; i++, current += offset)
                    for (int j = 0; j < result.Width; j++, current++)
                        *current = (byte)(accumulator[i, j]* ratio);
                return result;
            }
        }
    }

    [Algorithm("Minimum", "Calculates the minimum from the given images")]
    [Abbreviation("min")]
    public class TxMinimum: TxMultiBand
    {
        private static void AccumulMin(TxImage image, TxImage result)
        {
            unsafe
            {
                _IplImage* innerImage = (_IplImage*)image.InnerImage;
                byte* current = (byte*)innerImage->imageData;
                _IplImage* resultInner = (_IplImage*)result.InnerImage;
                byte* resultCurrent = (byte*)resultInner->imageData;

                int offset = innerImage->widthStep - innerImage->width;

                for (int i = 0; i < image.Height; i++, current += offset, resultCurrent+=offset)
                    for (int j = 0; j < image.Width; j++, current++, resultCurrent++)
                    {
                        *resultCurrent = Math.Min(*current, *resultCurrent);
                    }
            }
        }

        public override TxImage Process(params TxImage[] images)
        {
            TxImage[] gray = new TxImage[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i].ImageFormat == TxImageFormat.RGB)
                    gray[i] = images[i].ToGrayScale();
                else
                    gray[i] = images[i];
            }
            TxImage result = (TxImage)gray[0].Clone();
            for (int i = 1; i < gray.Length; i++)
            {
                AccumulMin(gray[i], result);
            }
            return result;
        }
    }

    [Algorithm("Maximum", "Calculates the maximum from the given images")]
    [Abbreviation("max")]
    public class TxMaximum : TxMultiBand
    {
        private static void AccumulMax(TxImage image, TxImage result)
        {
            unsafe
            {
                _IplImage* innerImage = (_IplImage*)image.InnerImage;
                byte* current = (byte*)innerImage->imageData;
                _IplImage* resultInner = (_IplImage*)result.InnerImage;
                byte* resultCurrent = (byte*)resultInner->imageData;

                int offset = innerImage->widthStep - innerImage->width;

                for (int i = 0; i < image.Height; i++, current += offset, resultCurrent += offset)
                    for (int j = 0; j < image.Width; j++, current++, resultCurrent++)
                    {
                        *resultCurrent = Math.Max(*current, *resultCurrent);
                    }
            }
        }

        public override TxImage Process(params TxImage[] images)
        {
            TxImage[] gray = new TxImage[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i].ImageFormat == TxImageFormat.RGB)
                    gray[i] = images[i].ToGrayScale();
                else
                    gray[i] = images[i];
            }
            TxImage result = (TxImage)gray[0].Clone();
            for (int i = 1; i < gray.Length; i++)
            {
                AccumulMax(gray[i], result);
            }
            return result;
        }
    }
}