using System;
using System.Collections.Generic;
using System.Text; 
using TxEstudioKernel.OpenCV;
 namespace TxEstudioKernel
{
     public unsafe  class TxVector
    {
         IntPtr vect;
         double est =-1;
         double med = double.MaxValue ;
         int length= 0;
         
        public TxVector(int length)
        {
            this.length= length;
            vect = CXCore.cvCreateMat(1,length, 5);
        
        }
        public TxVector(_cvMat* vect) 
        {
           

        }

         public float this[int index]
         {
             get { return ((float*)((_cvMat*)vect)->data)[index]; }

             set { ((float*)((_cvMat*)vect)->data)[index] = value; }

         }
         public int Length
         {
             get { return length; } 
         
         }
      

         public double Stability
         {
             get
              {
                   if(est < 0 )
                   {
                       Dictionary <double,int>  hist = new Dictionary<double,int>();  

                       for (int i = 0; i < length; i++)
                       {
                           if (hist.ContainsKey(this[i]))
                           {
                               hist[this[i]] += 1;
                           }
                           else 
                           {
                               hist[this[i]] = 1; 
                           }
                          
                       }
                         double max = double.MinValue; 
                         double min = double.MaxValue;
                       
                       foreach( int val in hist.Values  ) 
                       {
                          if( max < val )
                          {
                            max = val;
                          }
                           if( val < min )
                           {
                             min = val;
                           }

                       }

                       est = min/max ;

                   }

                 return est;
             }
         }
         public double Median 
         {

             get 
             {
                 if ( med == double.MaxValue)
                 {
                     _cvScalar temp = CXCore.cvAvg(vect,(IntPtr) 0);

                     med = temp.val0;
                 }
                 return med;
             }
         }
       
         public IntPtr Data
         {
          get{ return this.vect;}
         }
         public void Print() 
         {
             float temp =0;
             for (int i = 0; i <length ; i++)
             {
                 temp=this[i];
                 
             }
         
         }
    }
}
