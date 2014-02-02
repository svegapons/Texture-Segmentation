using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;
using TxEstudioKernel.OpenCV;
using System.IO;

namespace TxEstudioKernel.Operators
{
    [Algorithm("SWA Method", "Multiescale image segmentation using segmentation by weigthed agregation")]
    [Abbreviation("SWA")]
    public  class SWA : TxSegmentationAlgorithm  
    {
        float alfa = 0.3f;
        double seed_tresh = 0.15;
        double vol_tresh = 0.9;
        double sharp_tresh = 0.5;
        float salient_tresh = 0.0002f;
        int level_sal = 10;



        //Descriptors Vars
        static int descrptrs_count = 2;
        int stats_level = 1;
        int var_level = 7;

        double avrg_coef = 0.02;
        double var_coef = 0.02;
        static int[] pixel;
        static int[] sqrpixel;
        Level Bottom;
        Level Top;

        [Parameter("alfa", "")]
        [RealInRange(0, float.MaxValue)]
        public float Alfa
        {
            get { return (float)alfa; }
            set { alfa = (float)value; }
        }
        [Parameter("Saliency Level", "")]
        [IntegerInSequence(1, int.MaxValue)]
        public int LevelSal
        {
            get { return level_sal; }
            set { level_sal = value; }
        }
        [Parameter("Saliency Treshold", "")]
        [RealInRange(0, float.MaxValue)]
        public float SaliencyTresh
        {
            get { return (float)salient_tresh; }
            set { salient_tresh = (float)value; }
        }
        [Parameter("TopDown Tresh", "")]
        [RealInRange(0, float.MaxValue)]
        public float TodDownTresh
        {
            get { return (float)sharp_tresh; }
            set { sharp_tresh = (float)value; }
        }
        [Parameter("Avrg Level", "")]
        [IntegerInSequence(1, int.MaxValue)]
        public int StatsLevel
        {
            get { return stats_level; }
            set { stats_level = value; }
        }

        [Parameter("Var Level", "")]
        [IntegerInSequence(1, int.MaxValue)]
        public int VarLevel
        {
            get { return var_level; }
            set { var_level = value; }
        }

        [Parameter("Avrg Coef", "")]
        [RealInRange(0, float.MaxValue)]
        public float AverageCoef
        {
            get { return (float)avrg_coef; }
            set { avrg_coef = (float)value; }
        }

        [Parameter("Var Coef", "")]
        [RealInRange(0, float.MaxValue)]
        public float VarCoef
        {
            get { return (float)var_coef; }
            set { var_coef = (float)value; }
        }
        [Parameter("Aggregate Tresh", "")]
        [RealInRange(0, float.MaxValue)]
        public float AgregateTresh
        {
            get { return (float)seed_tresh; }
            set { seed_tresh = (float)value; }
        }
        public override TxSegmentationResult Segment(params TxMatrix[] descriptors)
        {

          
            int width = descriptors[0].Width;
            int height = descriptors[0].Height;

            TxSparseMatrix.Init(width * height);
   
            TxSparseMatrix w = Weight(descriptors[0]);
            TxSparseMatrix L = Laplacian(w);
           
          
           GetPixelsIntensity(descriptors[0]);

            Bottom = new Level();
            Bottom.LevelN = 0;
            Bottom.W = w;
            Bottom.Laplacian = L;


            DescriptorPackage dp = InitDescriptors(); 
            
            int count = w.RowsCount;
            
            LinkedList<int> nodes = new LinkedList<int>();

            for (int n = 0; n < count ; n++)
            {
                nodes.AddLast(n);
            }
            Dictionary<double, LinkedList<int>> vol_nodes = new Dictionary<double, LinkedList<int>>();
            
            vol_nodes.Add(0,nodes);
           

            BottomUpProcess(Bottom,vol_nodes,dp);

            int[] pixels = new int[width * height];

            TopDownProcess(pixels, Top, 1);

            int[,] pixels_classes = new int[height,width];
            
            int i;
            int j;
            
            int[] map = new int[width*height/2]; 
            int temp;
            int  nclass =0;
            for (int pos = 0; pos < count; pos++)
            {
                i = pos / width;
                j = pos % width;
                temp = pixels[pos]; 
               if( map[temp] == 0)
               {
                   nclass++;
                 map[temp ] = nclass;
                
               }
               pixels_classes[i, j] = map[temp] - 1;
            }



            //Free Memory
            Bottom = null;
            Top = null;
            pixel = null;
            sqrpixel = null;

            GC.Collect();

            TxSegmentationResult res = new TxSegmentationResult(pixels_classes,nclass);

            
            return res;
        
        }
        public override double ProbError()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private Level BottomUpProcess( Level Prev,Dictionary<double,LinkedList <int>> vol_nodes,DescriptorPackage dp) 
        {
            

            TxSparseMatrix W = Prev.W;
            TxSparseMatrix L = Prev.Laplacian;

            //Principal structure to perform top down process 
            Level current = new Level(Prev);
             
            bool[] is_seed = new bool[W.RowsCount];
           //Maps between row pos and  column pos of the representative nodes 
            int[] cumul_seeds = new int[W.RowsCount];
            
            if (W.RowsCount <= 3) 
            {
               
                vol_nodes = WeightSort(vol_nodes, W);
            }
           
            //Select representative nodes as seeds
            int agregates_count = SelectRepresentativeNodes(vol_nodes,W,is_seed,cumul_seeds);
            //Free memory
            vol_nodes = null;

    

            if (agregates_count == W.RowsCount) 
            {
                Top = Prev;
                return null;
            }

            //Agregate other pixels around seeds,based on their couplings
            TxSparseMatrix p = BuildInterpolationMatrix(W,is_seed,cumul_seeds,agregates_count);
            
            //Free Memory 
            is_seed = null;
            cumul_seeds = null;
            
            
            // TxSparseMatrix pT = p.Transpose();
             
                     //this function helps to maintain linear time sorting; 
                     Dictionary<double, LinkedList<int>> vol = BuildAgregatesByVolume(p);
            //Calculat Segment agregate properties
            
            //Derive coarse level couplins from fine level couplings;
             // TxSparseMatrix new_W = pT * W * p;
                       Node[,] wdiagonal = new Node[2,p.ColumnsCount];
                       Node[,] ldiagonal = new Node[2,p.ColumnsCount];
             
                     TxSparseMatrix new_W = TxSparseMatrix.MultiplybyColmumns(W, p,wdiagonal);// = TxSparseMatrix.MultiplyWbyPTransPose(W, p);
                     TxSparseMatrix new_L = TxSparseMatrix.MultiplybyColmumns(L, p,ldiagonal);// = TxSparseMatrix.MultiplyWbyPTransPose(L, p); 
                     
              // TxSparseMatrix new_L = pT * L * p;  
            //Modify couplings by similarity in agregates  properties

 
             LinkedList<int> SalientSegments=null;
            if( Prev.LevelN + 1   >=  level_sal)
            {

                //Evaluate segments saliency to find representative agregates
              SalientSegments = FindSalientSegments(new_W,new_L);            
            
            }
            //Fill Level principal structure
            current.LevelN = Prev.LevelN + 1;
            current.P = p;
            current.W = new_W;
            current.Laplacian =new_L;
            current.SalientSegments = SalientSegments;
            
            //Calculate agregates properties 
            dp.CalculateDescriptors(p);


           if( current.LevelN >=  stats_level)
            ModifyWeights(wdiagonal,dp,current.LevelN);
            
            //Free Memory
            Prev.FreeMemory();
            W = null;
            L = null;
            wdiagonal = null;
            ldiagonal = null;


            
            if ( agregates_count > 2)
            {

                current.Next = BottomUpProcess(current, vol,dp);   

            }
            else 
            {
                Top = current;
            }


            return current;
        }
       #region Segmentation Region

        private void TopDownProcess(int[]pixels, Level current,int lastlabel) 
        {
            int level = current.LevelN;

            if (level >= level_sal) 
            {
                LinkedList<int> salient_segments = current.SalientSegments;

                if (salient_segments.Count > 0) 
                {

                    foreach (int segment in salient_segments )
                    {
                        ExpandToFinestLevel(segment, lastlabel, current, pixels);
                        lastlabel++;
 
                    }
                }  
      
                TopDownProcess( pixels , current.Previous , lastlabel);
            
            }

        }
        private void ExpandToFinestLevel(int agregate,int label,Level x,int []pixels) 
        { 
            int level = x.LevelN;
            TxSparseMatrix p =  x.P;
            TxSparseMatrix u = new TxSparseMatrix(p.ColumnsCount, 1);

            u[ agregate , 0 ] = 1;
            while (level > 0) 
            {
                u =TxSparseMatrix.SMatrixSVectorMultiply(p,u,sharp_tresh,1-sharp_tresh) ;
                x = x.Previous;
                p = x.P;
                level = x.LevelN ;
            }

            u.MarkValues(0, label, sharp_tresh, pixels);
        
        }
       #endregion


        #region Init
        private double Weight(float val1, float val2)
        {
            return Math.Exp(-Math.Abs(val1 - val2) * alfa);
        }
        private TxSparseMatrix Weight(TxMatrix image)
        {
            int rows = image.Height * image.Width;
            int cols = rows;

            int width = image.Width;
            int height = image.Height;

            TxSparseMatrix res = new TxSparseMatrix(rows, cols);

            int resi;
            double temp;
            for (int i = 0; i < height - 1; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {

                     resi = i * width + j;

                    temp  = (float)Weight(image[i, j], image[i, j + 1]);
                   
                    if( temp != 0 )
                    {
                        res[ resi , resi + 1 ] = temp;
                        res[ resi + 1 , resi ] = temp;  
                    }
                    

                    temp = (float)Weight(image[i, j], image[i + 1, j]);

                    if (temp != 0)
                    {
                        res[resi, resi + width] = temp;

                        res[resi + width, resi] = temp;
                    }
                }
            }

            //Ultima fila 

            resi = width * (height - 1);
            for (int i = 0; i < width - 1; i++)
            {

                temp = (float)Weight(image[height - 1, i], image[height - 1, i + 1]);
                 if( temp != 0 )
                 {
                     res[resi , resi + 1]= temp ;
                     res[resi + 1 , resi ] = temp;
                 }
                resi += 1;
            }

            // Ultima columna 
            resi = width - 1;

            for (int i = 0; i < height - 1; i++)
            {

                 temp = (float)Weight(image[i, width - 1], image[i + 1, width - 1]);
                 
                if(temp!=0)
                {
                    res[resi, resi + width] = temp;
                    res[resi + width, resi] = temp;
                }
                resi += width;

            }
           

            return res;
        }
        private TxSparseMatrix Laplacian(TxSparseMatrix w) 
        {

            TxSparseMatrix L = TxSparseMatrix.Laplacian(w);
           
            return L;
        }
        private void GetPixelsIntensity(TxMatrix image) 
        {
            int height = image.Height;
            int width = image.Width;
            int pos;
            int val;
            pixel = new int[width * height];
            sqrpixel = new int[width * height];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    val = (int)image[i, j];
                    pos = i * width + j;
                   pixel[pos] = val;
                   sqrpixel[pos] = val * val;
                }
            }
           
        }

        private DescriptorPackage InitDescriptors() 
        {
            DescriptorPackage res = new DescriptorPackage(descrptrs_count);
            res[0] = new AverageDescriptor(pixel,sqrpixel,avrg_coef);
            res[1] = new VarianceDescriptor((AverageDescriptor)res[0], var_level, var_coef);
            return res;
        }
       
         #endregion
       
        //Returns the  count of representative nodes and check this nodes with true in the seeds array  
        private int SelectRepresentativeNodes( Dictionary<double,LinkedList <int>> vol_nodes,TxSparseMatrix w, bool[] seeds,int []cumul_seeds ) 
        {
           
            int res = 0;
                             
            //Sort by bucket

            SortedList<double, LinkedList<int>> sorted = BucketSort(vol_nodes);
                 
               //End of sort
               //The array is sorted in ascending order with  max volume at the end; 
               IList<double> keylist = sorted.Keys;
               for (int i = keylist.Count - 1; i >= 0; i--)
               {
                   LinkedList<int> current = sorted[keylist[i]];

                   if (current != null)
                   {
                       foreach (int node in current)
                       {

                           if (IsRepresentative(node, w, seeds))
                           {
                               seeds[node] = true;

                               res++;
                           }

                       }

                   }
               }
                 
                int cumul = 0;
                for (int i = 0; i < seeds.Length; i++)
                {
                    if (seeds[i]) 
                    {
                        
                        cumul_seeds[i] = cumul;
                        cumul++;
                    }      
                } 

                return res ;                               
        }



        private bool IsRepresentative(int node,TxSparseMatrix w, bool[] seeds) 
        { 
             double numerator = w.MaxValue(node,seeds);
             double denominator = w.RowSum(node);

             double rate = numerator / denominator;
             if (numerator == 0)
             {
                 return true; 
             }

             return rate < seed_tresh;
              
        }
        private Dictionary<double, LinkedList<int>> WeightSort(Dictionary<double, LinkedList<int>>vol_nodes,TxSparseMatrix w ) 
        {
          Dictionary<double, LinkedList<int>>  newvol_nodes=new Dictionary<double,LinkedList<int>>();

          double sum;
            foreach (double var in vol_nodes.Keys)
            {
                LinkedList<int> list = vol_nodes[var];

                foreach (int node in list)
                {
                    sum = w.RowSum(node);

                    if (!newvol_nodes.ContainsKey(-sum))
                    {
                         newvol_nodes.Add(-sum, new LinkedList<int>());
                    }
                    newvol_nodes[-sum].AddLast(node);
                   
                }

            }

            return newvol_nodes;
        }
        private SortedList<double, LinkedList<int>> BucketSort(Dictionary<double, LinkedList<int>> nodes) 
        {
        
           SortedList<double, LinkedList<int>> res = new SortedList<double, LinkedList<int>>(nodes.Count);

   
            foreach (double key in nodes.Keys)
            {
                res.Add(key, nodes[key]);
            }
            return res;
        }
        private TxSparseMatrix BuildInterpolationMatrix(TxSparseMatrix w,bool[]seeds,int []cumul_seeds,int count) 
        { 
              int rows = w.RowsCount;
              int cols = count;
              TxSparseMatrix p = new TxSparseMatrix(rows,cols);
              int j = 0;
              for (int i = 0; i < rows; i++)
              {
                  if ( seeds[i])
                  {

                      p[i,j] = 1;
                      j++;

                  }
                  else 
                  {
                      double total = w.RowSum( i , seeds );
                      TxSparseMatrix.NormalizeRow(i, total, w, p, seeds,cumul_seeds);
                  }

              }
              return p;
        
        }

        //Returns the selected agregates agrupated by their volumes
        private Dictionary<double, LinkedList<int>>  BuildAgregatesByVolume(TxSparseMatrix p)
       {
           int count = p.ColumnsCount;
           Dictionary<double, LinkedList<int>> res = new Dictionary<double, LinkedList<int>>();
            double volume;
           
           for ( int i = 0; i < count; i++)
           {
               volume = p.ColumnValuesGreaterThan( i , vol_tresh);

               if (!res.ContainsKey(volume))
               {
                   res.Add(volume,new LinkedList<int>());
               }

               res[volume].AddLast(i);
           }
           return res;
       }
        private void FillSeeds(int []seeds,bool[]is_seed) 
        {
            int j=0;
            for (int i = 0; i < is_seed.Length; i++)
            {

                if (is_seed[i]) 
                {
                    seeds[j] = i;
                    j++;  
                }
            }     
        
        }
        private LinkedList<int> FindSalientSegments(TxSparseMatrix new_W,TxSparseMatrix new_L ) 
        {
            int length = new_L.RowsCount;
            LinkedList<int> res = new LinkedList<int>();
            double  saliency;
            double external_weights;
            double internal_weights;

            for (int i = 0; i < length; i++)
            {
                external_weights = new_L[i,i];
                internal_weights = new_W[i,i];
                saliency = 2 * (external_weights / internal_weights);
                if ( saliency < salient_tresh ) 
                {
                    res.AddLast(i);
                }
            }
            return res;
        }
       

        private void ModifyWeights(Node[,]wdiagonal,DescriptorPackage dp,int level) 
        {

            Node cursor1;
            Node cursor2;
            int length = wdiagonal.GetLength(1);
            length = length - 1;
            double val;
            int j; 
            
            for ( int i = 0; i < length;i++)
            {
                cursor1 = wdiagonal[1,i];
                cursor2 = wdiagonal[0,i];
                if (cursor1 != null)
                {
                    while (cursor1.RowLink != null)
                    {
                        cursor1 = cursor1.RowLink;
                        cursor2 = cursor2.ColumnLink;
                        j = cursor1.Column;
                       
                        val = cursor1.Value * dp[0][i, j];
                         if(level >= var_level)
                         {
                             val*= dp[1][i, j];
                         }
                        cursor1.Value = val;
                        cursor2.Value = val;

                    }
                }
            }
       
            
        }
      
    }

    class Level
    {
        int level;
        Level  previous;
        Level  next;
        //Interpolation Matrix level ,level -1
        TxSparseMatrix p;
        TxSparseMatrix w;
        TxSparseMatrix l;
       
        int[,] pixelpos;
       
        LinkedList<int> salients;
        
        
        public Level()
        {
        }
        public Level(Level Prev) 
        {
            this.previous = Prev;
        }
        public int[,] PixelPos
        {
            get { return pixelpos; }
            set { pixelpos = value; }
        }
        
        public Level Next
        {
            get { return next; }
            set { next = value; }
        }
        public Level Previous 
        {
            get { return previous; }
            set { previous = value; }
        
        }
      
        public int LevelN 
        { 
            get { return level; }
            set { level = value; }
        }
        public TxSparseMatrix P 
        {
            get { return p; }
            set { p = value; }
        }
        public TxSparseMatrix W
        {
            get { return w; }
            set { w = value; }
        }
        public TxSparseMatrix Laplacian
        {
            get { return l; }
            set { l = value; }
        }
        public LinkedList<int> SalientSegments
        {

            get { return salients; }
            set { salients = value; }
           
        }
        public void FreeMemory()
        {
            this.w = null;
            this.l = null;
        }
    
    }
    public class  DescriptorPackage 
    {
        Descriptor[] descriptors;

        public DescriptorPackage(int count) 
        {
            descriptors = new Descriptor[count];
        }

        public void Init() 
        { 
        
        
        }
        public void CalculateDescriptors(TxSparseMatrix p) 
        {
            for (int i = 0; i < descriptors.Length; i++)
            {
                descriptors[i].Update(p);
            }
              
        }
        public Descriptor this[int i] 
        {
            get { return descriptors[i]; }
            set { descriptors[i] = value; }
        }
           
    }
    public abstract  class Descriptor 
    {

        protected double coef;
        
        public Descriptor( double coef ) 
        {
            this.coef = coef;
        }
        public abstract void Update(TxSparseMatrix p);

        public abstract  double this[int i, int j]
        {
            get;
        }
    
    }
    public class AverageDescriptor:Descriptor   
    {
        private TxSparseMatrix rel;
        private int[] avrgs;
        private int[] sqr_avrgs;
        private static int[] pixel_vals;
        private static int[] sqr_pixel_vals;
        
        public AverageDescriptor(int [] pixel_vals,int [] sqr_pixel_vals ,double coef):base (coef) 
        {
           AverageDescriptor.pixel_vals = pixel_vals;
           AverageDescriptor.sqr_pixel_vals = sqr_pixel_vals;
           rel = TxSparseMatrix.Identity( pixel_vals.Length ); 
        }
      
        public  override double this[int i, int j]
        {
            get 
            {   double val= Math.Exp(- Math.Abs(avrgs[i] - avrgs[j]) * coef);
                return val;
            }
        }
        public override void Update(TxSparseMatrix p)
        {
            rel = TxSparseMatrix.MultiplybyColmumns(rel, p); 
            int length = p.ColumnsCount;
            avrgs = new int[length];
            sqr_avrgs = new int[length];
            double colsum = 0;
            double[] product = new double[2];

            for (int i = 0; i < length; i++)
            {
                rel.MultiplyColumnByDenseVector( i , pixel_vals , sqr_pixel_vals , product,ref colsum);
                avrgs[i] = (int)(product[0]/colsum);
                sqr_avrgs[i] = (int)(product[1]/colsum);
            }
        }
      
        public int[] Averages 
        {
            get { return this.avrgs; }
            set { this.avrgs = value; }
        }
        public int[] SqrAverages
        {
            get { return this.sqr_avrgs; }
            set { this.sqr_avrgs = value; }
        }
    }
    public class VarianceDescriptor : Descriptor 
    {
        AverageDescriptor avrg;
        static List<TxSparseMatrix> rel;
        static List<int[]>variances ;
        int[,] vectors;
        static TxMatrix val=new TxMatrix(1,1) ;
        double[] varvar;
        int startlevel;
        int level=0;

        public VarianceDescriptor(AverageDescriptor avrg,int startlevel ,double coef)
            : base(coef)
        {
            this.avrg = avrg;
            this.startlevel = startlevel;
            rel = new List<TxSparseMatrix>(18);
            variances = new List<int[]>(18);
        }
        
        public override double this[int i, int j]
        {
            get { double val = Math.Exp(- MahalanobisDistance(i, j)*coef);
            return val;
        }
        }
        public override void Update(TxSparseMatrix p)
        {
            level++;


            int[] avrgs = avrg.Averages;
            int[] sqr_avrgs = avrg.SqrAverages;

            int length = p.ColumnsCount;
            
            int[] level_var = new int[length];

            for (int i = 0; i < length; i++)
            {
                level_var[i] = sqr_avrgs[i] - avrgs[i] * avrgs[i];
            }

            variances.Add(level_var);
            
            if (level > 1)
            {
               
                MultiplyMatrices(p);
                
                if (level >= startlevel)
                {
                    varvar = new double[level];
                    vectors = CalculateVectors(level_var);
                    CalculateVarVar(varvar);
                }
            }
        }

        private void CalculateVarVar(double[] varvar) 
        {
            float media; 
            float total=0;
            int n = vectors.GetLength(1);
            for (int i = 0; i < level; i++)
            {
                for (int j = 0; j < n ; j++)
                {
                    total += vectors[i, j]; 
                }
                media = total / n;
                for (int j = 0; j < n; j++)
			   {
                   varvar[i] += Math.Pow((vectors[i, j] - media), 2);             
			   }
               varvar[i] = varvar[i] / n;
               total = 0;
            }


        }
        private int[,] CalculateVectors( int []levelvar) 
        { 
            int length = levelvar.Length;
             int [,] res = new int[level,length];

             double val = 0; 
             double colsum = 0;
             int length2 = variances.Count - 1;

             for (int i = 0; i < length; i++)
             {
                 for (int j = 0;  j < length2;  j++)
                 {
                     val = rel[j].MultiplyColumnByDenseVector(i,variances[j],ref colsum);
                     res[j, i] = (int)(val / colsum);                      
                 }
                 res[length2, i] = levelvar[i];   
             }
             return res;
        }
        private void MultiplyMatrices( TxSparseMatrix p)
        {

            for (int i = 0; i < rel.Count;i++)
            {
                rel[i] = TxSparseMatrix.MultiplybyColmumns(rel[i],p);
            }
            rel.Add(p);
        }
        private double MahalanobisDistance(int vector1,int vector2) 
        {
            double res=0;

            for (int i = 0; i < level; i++)
            {
                res +=Math.Pow( vectors[i, vector1] - vectors[i, vector2] , 2) / varvar[i];
            }
            
            
            res = Math.Sqrt(res);
            
            return res;
        }
        private TxMatrix CovarianceMatrix(int vector1,int vector2) 
        {
          
            return null;

        }
    }
}
