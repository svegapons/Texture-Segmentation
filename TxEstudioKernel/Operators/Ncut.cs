using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;

namespace TxEstudioKernel.Operators
{

    [Algorithm("Ncut Method", "Image segmentation using the Ncut Algorithm")]
    [Abbreviation("nc")]
    public class Ncut: TxSegmentationAlgorithm          
    {

        double alfa=0.3;
        double maxNcut = 0.001;
        double stab_fact=0.06;
        TxMatrix w;
        TxMatrix d;
        TxVector[] pvects;
        bool[] ispartA;
        bool[] ispartB;
        bool[] isV;
        int width;
        int height;
        int maxclass = 8; 
        public override TxSegmentationResult Segment(params TxMatrix[] descriptors)
        {


            TxMatrix image = descriptors[0];
            width = image.Width;
            height = image.Height;
            int length = width * height;
            
            ispartA = new bool[length];
            ispartB = new bool[length];
                isV = new bool[length];


            w = Weight(image);
            d = Diagonal( w );

            //d^(-1/2)
             TxMatrix invsqr = TxMatrix.DiagInvSqr(d);
             
            //d^(-1/2) *(d - w)* d^(-1/2)
            
            TxMatrix symetric = invsqr*(d - w)*invsqr;

           
          //eigen system d^(-1/2) *(d - w)* d^(-1/2)*  Z = &*Z

            //Init
            TxMatrix vectors = new TxMatrix(length,length);
            TxVector vals = new TxVector(length);
            
            
            double eps = 0.0005;
            //Result eigenvalues are in descending order 
             symetric.ComputeEigenVV(vectors,vals,eps);

            
             pvects = this.FirstEigenVectors(vectors,invsqr,maxclass);

             List<LinkedList<int>> segments = new List<LinkedList<int>>();
             LinkedList<int> src = new LinkedList<int>();
           
            for (int i = 0; i < length; i++)
             {
                 src.AddLast(i);
             }

             Recursive(1, src,segments);


            //Converting  to plataform  result form
             TxSegmentationResult res = new TxSegmentationResult(segments.Count ,width,height);

            int classes = 0;
            foreach ( LinkedList<int> list  in segments)
            {

                foreach (int pos in list)
                {
                    int i = pos / width;
                    int j = pos % width;
                    res[i,j] = classes; 
                } 
                    classes++;
            }

          
           return res;
        }
        public double Recursive( int vector,LinkedList<int> src,List<LinkedList<int>>  res) 
        {

            if (vector < pvects.Length)
            {
                int bestv = vector;
                double v_stab;
                double bestv_stab = 1000000;
                for (int i = vector; i < pvects.Length; i++)
                {
                    v_stab = pvects[i].Stability;

                    if (stab_fact > v_stab)
                    {
                        break;
                    }
                    else
                    {
                        if (v_stab < bestv_stab)
                        {
                            bestv_stab = v_stab;
                            bestv = i;
                        }


                        vector++;
                    }
                }

                if (!(vector < pvects.Length))
                    vector = bestv;

                LinkedList<int> A = new LinkedList<int>();
                LinkedList<int> B = new LinkedList<int>();

                Partition(pvects[vector], src, A, B);


                double VNcut = NcutVal(A, B);

                CleanArrays();

                if (VNcut < maxNcut)
                {
                    double Ares = this.Recursive(vector + 1, A, res);
                    double Bres = this.Recursive(vector + 1, B, res);


                    if (Ares == -1) 
                    {
                        res.Add(A);            
                    
                    }
                    if (Bres == -1)
                    {
                        res.Add(B);
                    }

                    return VNcut;
                }
                else
                {

                    return -1;
                }


            }
            else return -1;
        }   
        public void Partition(TxVector  vector,LinkedList<int> src,LinkedList<int> partA,LinkedList<int> partB)
        {
            double spltpoint = SplittingPoint(vector);
            
            foreach (int i in src )
            {
                isV[i] = true; 
                if (vector[i] >= spltpoint)
                {
                    ispartA[i] = true;
                    partA.AddLast(i);
                }
                else
                {
                    ispartB[i] = true;
                    partB.AddLast(i);
                }
            }

        }
        public double NcutVal( LinkedList<int> partA , LinkedList<int> partB) 
        {
            //Recordar ABSum == BASum
             double ABsum = 0;
             double BASum = 0;
             double AVSum = 0;
             double BVSum = 0;


             foreach (int i in partA) 
             {

                 ABsum += NeighborSum(i, ispartB); 
             }

             foreach (int i in partB) 
             {
                 BASum += NeighborSum(i, ispartA);
             }
             foreach (int i in partA)
             {

                 AVSum += NeighborSum(i, isV);
             }
             foreach (int i in partB)
             {
                 BVSum += NeighborSum(i,isV);
             }


             double res = ABsum / AVSum + BASum / BVSum; 
            
            return res;
        }
        private double NeighborSum(int node,bool[] neighborpart) 
        {
            double res = 0;     
            
            int i =  node / width ; 
            int j =  node % width ;
            int neighbor = 0;  
            

            //tiene vecino a la izquierda o no esta en la primera columna 
             neighbor = node-1;

             if (j != 0 && neighborpart[neighbor])
            {
                 res +=  w[node,neighbor];  
            }
            //tiene vecino arriba o no esta en la primera fila  

            neighbor = node - width;

            if (i != 0 && neighborpart[neighbor])
            {   
                 res += w[ node,neighbor ];               
            }  
            // tiene vecino a la derecha o no esta en la ultima columna  
            neighbor = node + 1;

            if (j != width - 1 && neighborpart[neighbor])
            {
                res += w[node, neighbor];   
            }
            //tiene vecino debajo o no esta en la ultima fila
            neighbor = node + width;

            if (i != height - 1 && neighborpart[neighbor])
            {
                res += w[node, neighbor];
            }
            
            return res;
            
        }
        public double SplittingPoint (TxVector v)
        {
            return v.Median;
        }
        public override double ProbError()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        private void CleanArrays()
        {
        
              for ( int i = 0; i < ispartA.Length ; i++)
			{
                ispartA[i] = false;
                ispartB[i] = false;
                isV[i] = false;
			}
        }
        private TxMatrix Weight( TxMatrix image)
        {
            int rows = image.Height * image.Width;
            int cols = rows;

            int width = image.Width;
            int height = image.Height;

            TxMatrix res = new TxMatrix(rows, cols); 

            int resi;
            int resj;
            for (int i = 0; i < height -1 ; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {

                    resi = i * width  +  j;

                    res[resi, resi + 1] =(float) Weight(image[i, j], image[i, j + 1]);

                    resi = resi;
                    

                    res[resi,resi + width] =(float) Weight(image[i, j], image[ i+1 , j ]);
                
                }
            }

            //Ultima fila 

            resi = width * (height - 1);
            for (int i = 0; i < width - 1; i++)
            {             
                res[resi, resi + 1]=(float) Weight(image[height -1,i], image[height - 1 ,i+1]);

                resi += 1;
            }

           // Ultima columna 
            resi = width - 1;
          
            for (int i = 0; i < height - 1; i++)
            {

                res[resi,resi + width] =(float) Weight(image[i, width - 1], image[i + 1, width - 1]);
            
                  resi += width;
                 
            }
            res[rows - 1, cols - 1] = 0;

            res.ExpandSymetric();

            
            return res;

        }
        private double Weight(float val1,float val2) 
        {
                 return Math.Exp(- Math.Abs(val1 - val2)* alfa );  
        } 
        private TxMatrix Diagonal( TxMatrix  weight ) 
        {
            
            TxMatrix dg = new TxMatrix(weight.Height,weight.Width);

            double temp = 0;

            for (int i = 0; i < weight.Height; i++)
            {
                for (int j = 0; j < weight.Width; j++)
                {
                    temp += weight[i, j];
                }

                dg[i, i] = (float)temp;
                temp = 0;
            }

            return dg; 
        }
        private TxVector[] FirstEigenVectors(TxMatrix vectors,TxMatrix invsqr,int n)
        {
            TxVector[] y = new TxVector[n];
            TxVector[] z = new TxVector[n];
            int length = width*height;

            for (int i = 0;i < n ;i++)
            {
               y[i] = new TxVector(length);
               z[i] = new TxVector(length);    
              
                GetRow(vectors,z[i],length - i - 1);

                CXCore.cvGEMM((IntPtr)z[i].Data, (IntPtr)invsqr.InnerMatrix, 1, (IntPtr)0, 1, (IntPtr)y[i].Data, 0);
                y[i].Print();    
            }
            return y;
       }
        private void GetRow(TxMatrix src,TxVector dst, int row) 
        {

            for ( int j = 0; j < dst.Length ; j++) 
            {
                dst[j] = src[row, j]; 
            } 
        }
    }
}
