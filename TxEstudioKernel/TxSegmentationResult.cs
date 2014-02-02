using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel
{
    /// <summary>
    /// Class that represents the result of a segmentation process over an image.
    /// </summary>
    public class TxSegmentationResult : TxObject
    {
        
        private int[,] values;
        private int classes;
        private List<int> class_count;
        private static Color[] defaultMapping = new Color[]{Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Cyan, Color.Gold, Color.Orange, Color.Pink, Color.Magenta, Color.GreenYellow, Color.BlanchedAlmond, Color.Tomato, Color.FloralWhite, Color.Ivory, Color.Khaki, Color.LemonChiffon, Color.MintCream, Color.Wheat, Color.PaleGreen, Color.SandyBrown, Color.LightBlue, Color.Turquoise, Color.RoyalBlue, Color.LavenderBlush, Color.Gainsboro, Color.Fuchsia, Color.IndianRed, Color.Silver, Color.Olive, Color.OldLace};
        public static Color[] GetDefaultMapping(int classes)
        {
            Color[] result = new Color[classes];
            if (classes <= defaultMapping.Length)
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = defaultMapping[i];
                }
            else
            {
                int step = 16777215 / classes;
                for (int i = 0; i < classes; i++)
                    result[i] = Color.FromArgb(i * step);
            }
            return result;
        }
    
        public TxSegmentationResult(int classes, int width, int height)
        {
            this.classes = classes;
            values = new int[height, width];
            class_count = new List<int>(classes);
        }
        //Se asume el arreglo con enteros positivos entre 0 y classes
        public TxSegmentationResult(int[,] values)
        {
            this.values = values;
            classes = 0;
            class_count = new List<int>();
            for (int i = 0; i < values.GetLength(0); i++)
                for (int j = 0; j < values.GetLength(1); j++)
                {  
                    int val = values[i, j];
                    
                    CheckNewClass(val);
                    
                    classes = Math.Max(classes,val);//Se asume el arreglo con enteros positivos
                }
               
                classes++;
        }
        public TxSegmentationResult(int[,] values,int classes)
        {
            this.values = values;
            this.classes = classes;
            class_count = new List<int>();
            for (int i = 0; i < values.GetLength(0); i++)
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    int val = values[i, j];

                    CheckNewClass(val);
                }
        }

        public int ClassPixelCount(int i) 
        {
           if( i <  class_count.Count )
            return class_count[i];
               return 0;
        }
        public int this[int row, int col]
        {
            get { return values[row, col]; }
            set {   
                    values[row, col] = value;
                    CheckNewClass(value);
                }
        }

        public int Width
        {
            get
            {
                return values.GetLength(1);
            }
        }

        public int Height
        {
            get
            {
                return values.GetLength(0);
            }
        }

        public int Classes
        {
            get
            {
                return classes;
            }
            set
            {
                classes = value;
            }
        }

        public TxImage ToImage()
        {
            if (defaultMapping.Length > classes)
                return ToImage(defaultMapping);
            Color[] mapping = GetDefaultMapping(classes);
            return ToImage(mapping);
        }

        public TxImage ToImage(Color[] colorMapping)
        {
            TxImage result = new TxImage(values.GetLength(1), values.GetLength(0), TxImageFormat.RGB);
            unsafe
            {
                _IplImage* innerImage = (_IplImage*)result.InnerImage;
                byte* current = (byte*)innerImage->imageData;
                int offset = innerImage->widthStep - 3 * innerImage->width;

                for (int i = 0; i < result.Height; i++, current += offset)
                    for (int j = 0; j < result.Width; j++, current += 3)
                    {
                        current[2] = colorMapping[values[i, j]].R;
                        current[1] = colorMapping[values[i, j]].G;
                        current[0] = colorMapping[values[i, j]].B;
                    }
                
            }
            return result;
        }
        private void CheckNewClass(int val)
        {
            if (val >= class_count.Count)
            {
                for (int i = class_count.Count - 1; i <= val; i++)
                {
                    class_count.Add(0);
                }
                class_count[val]++;
            }
            else
                class_count[val]++;
        
        }
        //public TxMatrix ToMatrix()
        //{
        //    throw new System.NotImplementedException();
        //}
        ////Qué hago en este caso?
        //public static TxImage FromImage()
        //{
        //    throw new System.NotImplementedException();
        //}

        //public static TxMatrix FromMatrix()
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
