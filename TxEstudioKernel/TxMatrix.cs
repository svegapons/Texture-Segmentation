using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.OpenCV;
using System.Drawing;
using System.IO;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel
{
    /// <summary>
    /// Represents a real-values matrix.
    /// Encapsulates _cvMat from OpenCV
    /// </summary>
    public unsafe class TxMatrix: TxObject, IDisposable   
    {
        IntPtr innerMatrix;
        int width;
        int height;
        public TxMatrix(float[,] data)
        {
            innerMatrix = CXCore.cvCreateMat(data.GetLength(0), data.GetLength(1), 5);

            float* matrixData = (float*)((_cvMat *)innerMatrix)->data;

            height = data.GetLength(0);
            width = data.GetLength(1);
            
            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    *matrixData = data[i, j];
                    matrixData++;
                }
           
        }
        private TxMatrix(_cvMat* inner) 
        {
            this.width = inner->cols;
            this.height = inner->rows;
            this.innerMatrix = (IntPtr)inner;
        }
        public TxMatrix(int rows, int cols)
        {
            height = rows;
            width = cols;
            innerMatrix = CXCore.cvCreateMat(rows,cols,5);
        }

        protected TxMatrix() 
        { 
        }
        /// <summary>
        /// Permite acceder al elemento en la fila i y la columna j.
        /// </summary>
        /// <param name="i">Fila</param>
        /// <param name="j">Columna</param>
        /// <remarks>Las coordenadas en TxMatrix funcionan al inverso de la TxImage</remarks>
        public virtual float this[int i, int j]
        {
            get
            {
                return ((float*)((_cvMat*)innerMatrix)->data + width * i)[j];
            }
            set
            {
                ((float*)((_cvMat *)innerMatrix)->data + width * i)[j] = value;
            }
        }
        

        public IntPtr InnerMatrix
        {
            get
            {
                return (IntPtr)innerMatrix;
            }
        }

        public int Height
        {
            get { return height; }
        }
        public int Width 
        { 
            get 
            { 
                return width; 
            } 
        }

        public virtual TxImage ToImage()
        {
            //Normalizar primero
            float max = float.MinValue, min = float.MaxValue;
            float* matrixData = (float*)((_cvMat*)innerMatrix)->data;
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++,matrixData++ )
                {
                    max = Math.Max(*matrixData, max);
                    min = Math.Min(*matrixData, min);
                }

            float ratio = 255f / (max - min);

            TxImage result = new TxImage( width , height , TxImageFormat.GrayScale);
            _IplImage* ptrImage = (_IplImage*)result.InnerImage;
            int destOffset = ptrImage->widthStep - ptrImage->width;
            byte* dest = (byte*)ptrImage->imageData;

            matrixData = (float*)((_cvMat*)innerMatrix)->data;
            
            for (int i = 0; i < height; i++, dest += destOffset)
                for (int j = 0; j < width; j++, dest++, matrixData++)
                {
                    *dest = (byte)((*matrixData - min)*ratio);
                }

            return result;
        }

        public TxMatrix Convolve(TxMatrix kernel)
        {
            TxMatrix result = new TxMatrix( height , width );
            CV.cvFilter2D(InnerMatrix, result.InnerMatrix, kernel.InnerMatrix, new System.Drawing.Point(-1, -1));
            return result;
        }

        public static TxMatrix operator +(TxMatrix m1, TxMatrix m2)
        {

            TxMatrix res = new TxMatrix(m1.height, m2.width);

            CXCore.cvAdd((IntPtr)m1.innerMatrix, (IntPtr)m2.innerMatrix, (IntPtr)res.innerMatrix, IntPtr.Zero);

            return res;
        }
        public static TxMatrix operator -(TxMatrix m1, TxMatrix m2)
        {

            TxMatrix res = new TxMatrix(m1.height, m2.width);

            CXCore.cvSub((IntPtr)m1.innerMatrix, (IntPtr)m2.innerMatrix, (IntPtr)res.innerMatrix, IntPtr.Zero);

            return res;
        }
        public static TxMatrix operator *(TxMatrix m1, TxMatrix m2)
        {

            if (m1.width != m2.height) 
                throw new Exception("The number of columns of param m1 must be equal to the number of rows of param m2"   ); 

            TxMatrix res = new TxMatrix(m1.height, m2.width);

            double total = 0;
            for (int i = 0; i < m1.height ; i++)
            {
                for (int j = 0; j < m2.width ; j++)
                {
                    total = 0;
                    for (int k = 0; k < m1.width; k++)
                    {
                        total += m1[i,k] * m2[k,j];
                    }

                    res[i, j] =(float) total;
                }


            }



            //CXCore.cvGEMM((IntPtr)m1.innerMatrix, (IntPtr)m2.innerMatrix,1,(IntPtr)0,0,(IntPtr)res.innerMatrix,0);

            return res;
        }
        public static void Multiply(TxMatrix a, TxMatrix b, TxMatrix res) 
        {
            CXCore.cvGEMM((IntPtr)a.innerMatrix, (IntPtr)b.innerMatrix, 1, (IntPtr)0, 0, (IntPtr)res.innerMatrix, 0);
        }

        //returns a new matrix with diagonal elements equal to one divided by it´s square root    
        public static TxMatrix DiagInvSqr(TxMatrix src)
        {

            TxMatrix res = new TxMatrix(src.height,src.width);

            for (int i = 0; i <  src.width; i++)
            {
                res[i, i] =(float) (1 / Math.Sqrt(src[i, i]));
            }
            return res;
        }
        public static TxMatrix Inv(TxMatrix src)
        {

            TxMatrix res = new TxMatrix(src.height, src.width);

            double det = CXCore.cvInvert((IntPtr)src.innerMatrix, (IntPtr)res.innerMatrix, 0);

            return res;
        }
        public static void Inv(TxMatrix src,TxMatrix dst)
        {

            double det = CXCore.cvInvert((IntPtr)src.innerMatrix, (IntPtr)dst.innerMatrix,2);

        }

        public static TxMatrix FromImage(TxImage image)
        {
            TxMatrix result = new TxMatrix(image.Height,image.Width );
            _cvMat* ptrResult = (_cvMat*)result.InnerMatrix;
            TxImage grayImage = image.ToGrayScale();
            _IplImage* ptrImage = (_IplImage*)grayImage.InnerImage;

            int srcOffset = ptrImage->widthStep - ptrImage->width;
            //int byteWidth = 3 * ptrResult->cols;

            byte* src = (byte*)ptrImage->imageData;
            float* dest = (float*)ptrResult->data;

            for (int i = 0; i < ptrResult->rows; i++, src += srcOffset)
                for (int j = 0; j < ptrResult->cols; j++, src++, dest++)
                    *dest = (float)*src;

            return result;
        }
        public static TxMatrix VectorialAdd(TxMatrix imageX, TxMatrix imageY)
        {
            if (imageX.Height != imageY.Height || imageX.Width != imageY.Width)
                throw new ArgumentException("Las TxImage de entrada no tienen las mismas dimensiones");
            TxMatrix magnitudes = new TxMatrix(imageX.Height, imageX.Width);
            CXCore.cvCartToPolar(imageX.InnerMatrix, imageY.InnerMatrix, magnitudes.InnerMatrix, IntPtr.Zero, 0);
            return magnitudes;
        }

        public TxMatrix Scale(float min, float max)
        {
            double* maxValue = stackalloc double[1];
            double* minValue = stackalloc double[1];
            Point* minLoc = stackalloc Point[1];
            Point* maxLoc = stackalloc Point[1];
            CXCore.cvMinMaxLoc( innerMatrix, (IntPtr)minValue, (IntPtr)maxValue, (IntPtr)minLoc, (IntPtr)maxLoc, IntPtr.Zero);

            TxMatrix result = new TxMatrix( height , width );
            CXCore.cvConvertScale( innerMatrix , result.innerMatrix, (max - min) / (*maxValue - *minValue), -*minValue * (max - min) / (*maxValue - *minValue) + min);
            return result;
        }
        public void ExpandSymetric() 
        {
            if(width == height)
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    this[i, j] = this[j, i];        
                }

            }
        }
        public bool IsSymetric() 
        {

            if (width == height)
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (this[i, j] != this[j, i]) 
                        {
                            return false;
                        }
                    }

                }

            return true;
        }
        public void ComputeEigenVV(TxMatrix vectors,TxVector vals ,double eps ) 
        {

           CXCore.cvEigenVV(innerMatrix, vectors.innerMatrix ,vals.Data ,eps);
        
        }
       // The functions GetRows return a reference, corresponding to a specified submatrix  row/row span of the calling matrix.:
        public TxMatrix GetRows(int start_row,int end_row) 
        {
           TxMatrix  res = new TxMatrix(end_row - start_row,width);

           for (int i = start_row; i < end_row; i++)
           {
               for (int j = 0; j < width; j++)
               {
                   res[i - start_row, j] = this[i, j];
   
               }

           }
              return res;
        }
        
        public static TxMatrix Transpose(TxMatrix src) 
        {
            TxMatrix dst = new TxMatrix(src.Width,src.height);

            CXCore.cvTranspose(src.innerMatrix, dst.innerMatrix);

            return dst;
        }
        ~TxMatrix()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }


        public TxMatrix Transpose()
        {
            TxMatrix result = new TxMatrix(Width, Height);
            CXCore.cvTranspose(InnerMatrix, result.InnerMatrix);
            return result;
        }


        #region IDisposable Members
        bool disposed = false;
        public virtual void Dispose()
        {
            if (!disposed)
            {
                fixed ( IntPtr* ptr = &innerMatrix )
                {
                    CXCore.cvReleaseMat((IntPtr)ptr);
                }
                disposed = true;
            }
        }

        #endregion

        #region Byte Conversions

        public virtual byte[] ToByteArray()
        {
            byte* matrixData = (byte*)((_cvMat*)innerMatrix)->data;
            int byteLength = 4 *height *  + 8;
            byte[] result = new byte[byteLength];
            Utilities.writeInteger(height, 0, result);
            Utilities.writeInteger(width, 4, result);
            for (int i = 8; i < byteLength; i++, matrixData++)
                result[i] = *matrixData;
            return result;
        }

        public static TxMatrix FromByteArray(byte[] data)
        {
            TxMatrix matrix = new TxMatrix(Utilities.readInteger(0, data), Utilities.readInteger(4, data));
            byte* matrixData = (byte*)((_cvMat*)matrix.innerMatrix)->data;
            for (int i = 8; i < data.Length; i++, matrixData++)
                *matrixData = data[i];
            return matrix;
        }

        #endregion


    }
}
