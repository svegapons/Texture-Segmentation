using System;
using System.Collections;
using System.Collections.Generic;
namespace TxEstudioKernel
{
	/// <summary>
	/// Summary description for TxSparseMatrix.
	/// </summary>
	public class TxSparseMatrix
   {
        
            Node [] rows ;    
            Node [] columns ;
		 internal int cant = 0;
       static double eps = 0.000000000000001;
        static double eps2 = 0.1;
        static double[] col;
        static double[] col2;
		public TxSparseMatrix(int rows,int columns)
		{
			this.rows = new Node[rows];          	     
		 
			this.columns = new  Node[columns];
			for( int i = 0 ; i < this.rows.Length;i++)
			{
				this.rows [i] = new Node(0,-1,-1); 
				this.rows [i].ColumnLink = this.rows[i];       
			}
			
			for(int i = 0 ; i < this.columns.Length;i++)
			{
				this.columns[i]=new Node(0,-1,-1);
				this.columns[i].RowLink=this.columns[i];      
			}
			
		}
		/// <summary>
		/// Esta propiedad devuelve la cantidad de elementos distintos de cero
		/// en la TxSparseMatrix
		/// </summary>
        public  static void  Init(int n)
        {
            col = new double[n];
            col2 = new double[n];
        
        }
		public int Cant{get{return cant;}}
		private Node[]Rows  { get { return this.rows ; } }
		private Node[]Columns{ get { return this.columns ; } }
		/// <summary>
		/// Esta propiedad devuelve un entero que sera la cantidad de rows de la matriz 
		/// </summary>
		public int RowsCount{get { return this.rows.Length;}}
		/// <summary>
		/// Esta propiedad devuelve un entero que sera la cantidad de columns de la matriz
		/// </summary>
		
		public int ColumnsCount{get{return this.columns.Length;}}
		
        private  Node GetNode(int row,int column)
        {  
            
                        if( row >= 0 && row < rows.Length && 
						    column >= 0 && column < columns.Length) 
				     
				           {            
					           Node cursor = rows[ row];
				               while ( cursor.RowLink != null && cursor.Column < column )         
				                           cursor = cursor.RowLink;   
						
							  if(  cursor.Column == column )     
							  { 
								  return cursor ;
							  
							  }
						         else return null;
                           
                            }
                            return null;
                  
        }
        /// <summary>
        ///  La parte get de esta propiedad  retorna el elemento correspondiente a una posicion 
        /// en la matriz y la parte set cambia o inserta un elemento en una posicion   
        /// </summary>
        /// 
		public double this [int row,int column]
		{ 
			
			
			get {
                    
				           if( row >= 0 && row < rows.Length && 
						 column >= 0 && column < columns.Length) 
				     
				  {            
					
								  Node cursor = rows[ row];
				               while ( cursor.RowLink != null && cursor.Column < column )         
				                           cursor = cursor.RowLink;   
						
							  if(  cursor.Column == column )     
							  { 
								  return cursor.Value;
							  
							  }
						     else return 0;
						  
			    }			
				
		          
		    	  throw new  Exception("El indice  rows o columns esta fuera de rango" ); 
			 

				
			     }    
		  
			set 
			{
                
                if (row >= 0 && row < rows.Length && column >= 0 &&
                    column < columns.Length)
                {
                    Node val = new Node(value, row, column);
                    Node cursor = null;
                    Node aux = null;
                    cursor = rows[row];

                    if (value > 0)
                    {
                        //El enlace column del Node ficticio al inicio va a referencia al ultimo elemento de la row
                        //optimizar Caso especial si se  inserta al final de la row
                        if (cursor.ColumnLink.Column < column)
                        {
                            Node temp = cursor.ColumnLink;
                            temp.RowLink = val;
                            cursor.ColumnLink = val;
                        }
                        else
                        {
                            while ( cursor.RowLink.Column< column)
                            {
                                cursor = cursor.RowLink;
                            }
                            if (cursor.RowLink.Column!= column)
                            {
                                aux = cursor.RowLink;

                                cursor.RowLink = val;

                                val.RowLink = aux;

                            }
                            else
                            {
                                cursor.RowLink.Value = val.Value;
                            }

                        }

                        cursor = columns[column];
                        aux = null;

                        //El enlace row del Node ficticio al inicio de la column va a referencia al ultimo elemento de la column
                        //optimizar Caso especial si se  inserta al final de la column
                        if (cursor.RowLink.Row < row)
                        {
                            Node temp = cursor.RowLink;
                            temp.ColumnLink = val;
                            cursor.RowLink = val;
                        }
                        else
                        {
                            while (cursor.ColumnLink.Row < row)
                            {
                                cursor = cursor.ColumnLink;
                            }
                            if (cursor.ColumnLink.Row != row)
                            {
                                aux = cursor.ColumnLink;

                                cursor.ColumnLink = val;

                                val.ColumnLink = aux;

                            }
                            else
                            {
                                cursor.ColumnLink.Value = val.Value;
                            }

                        }

                    }
                    else  //value es 0
                    {
                        

                    }
                }
                
                        	
			}
		}
/// <summary>
///   Devuelve una nueva matriz resultante de sumar dos matrices  
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <returns></returns>

		public static TxSparseMatrix operator + ( TxSparseMatrix a , TxSparseMatrix b )
		{
		    
			
			if( a.rows.Length == b.rows.Length && a.columns.Length == b.columns.Length )      
			{   
				TxSparseMatrix c = new TxSparseMatrix( a.rows.Length,a.columns.Length );
				 
				for( int i = 0 ; i < a.rows.Length ; i++ )
				{       
					Node acursor = a.rows[i].RowLink;  
					Node bcursor = b.rows[i].RowLink;
					Node val = null;
					while( acursor!=null || bcursor != null )     
					{        
						if(acursor!= null && bcursor != null  )
						{
							if(  acursor.Column < bcursor.Column )
							{
								val = new Node (acursor.Value,i,acursor.Column);     
								acursor = acursor.RowLink;
							}
							else 
								if( acursor.Column > bcursor.Column  )
							{          
								val = new Node (bcursor.Value,i,bcursor.Column);
								bcursor = bcursor.RowLink;
							}	
							else 
							{
								val = new Node ( bcursor.Value + acursor.Value,i,bcursor.Column);
								acursor = acursor.RowLink;
								bcursor = bcursor.RowLink;
							
							}
						
						}
						else if(acursor!=null)
						{
							val = new Node( acursor.Value, i ,acursor.Column);                                                 
							acursor = acursor.RowLink;
					
						}
						else  
						{
							val = new Node( bcursor.Value, i ,bcursor.Column);                                                 
							bcursor = bcursor.RowLink;
					    }
						
					            if       ( val.Value != 0 )
						{    
								           c.cant++ ;
								c.rows[i].ColumnLink.RowLink = val;    
								c.rows[i].ColumnLink = val; 
								c.columns[val.Column].RowLink.ColumnLink = val;
								c.columns[val.Column].RowLink = val;
						}
					
					}
				
				}
				return c ;
			
			}
		
			else throw new Exception("Las matrices deben tener igual numero de rows  y columns");
		
		}
	
		/// <summary>
		/// Devuelve una nueva matriz que es el resultado de transponer la matriz atual     
		/// </summary>
		/// <returns></returns>
       
		
		public TxSparseMatrix  Transpose()  
		{
		    TxSparseMatrix m = new TxSparseMatrix(this.columns.Length,this.rows.Length);     
		      m.cant = this.cant;
				   for( int i = 0 ; i <  this.rows.Length ; i++ ) 
				   {
				       Node cursor = this.rows[i] ;
				       Node val;
					   while( cursor.RowLink != null )
					   {
						   cursor = cursor.RowLink;
					       
						   val = new Node(cursor.Value,cursor.Column,cursor.Row  );
			
							   m.columns[i].RowLink.ColumnLink = val ;  
						       m.columns[i].RowLink = val; 
						       m.rows[val.Row].ColumnLink.RowLink = val;
							   m.rows[val.Row].ColumnLink = val;
					   }
				   
				   }
		   
		                    return m ;
			   }	
		/// <summary>
		/// Devuelve una nueva matriz resultado de multiplicar dos matrices      
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns> 
		public static  TxSparseMatrix operator *(TxSparseMatrix a,TxSparseMatrix b)
		{
			if(a.columns.Length == b.rows.Length)
			{
			    TxSparseMatrix m = new TxSparseMatrix(a.rows.Length,b.columns.Length);   
				Node acursor ;
				Node bcursor ;
				for( int i = 0 ; i < a.rows.Length ; i++ )
					for( int j = 0 ; j < b.columns.Length ; j++ ) 
					{ 
						 acursor = a.rows[i];
					     bcursor = b.columns[j];
					     double val = 0 ;
						while( acursor.RowLink != null && bcursor.ColumnLink != null) 
						{
							if( acursor.RowLink.Column < bcursor .ColumnLink.Row  )
								acursor = acursor.RowLink;
							else if( bcursor.ColumnLink.Row < acursor.RowLink.Column)
								bcursor = bcursor.ColumnLink;
							else
							{    
    						    acursor = acursor.RowLink;
								bcursor = bcursor.ColumnLink;
								val += acursor.Value*bcursor.Value;
							}
						}
						if(val != 0)
						{        m.cant++;    
								Node Value = new Node(val,i,j);
							m.rows[i].ColumnLink.RowLink = Value;     
							m.rows[i].ColumnLink = Value;	
						    m.columns[ Value.Column].RowLink.ColumnLink=Value; 
						    m.columns[Value.Column].RowLink = Value;
						}
						acursor = null; 
			              bcursor = null;
					}
			     return m; 
			}
		else throw new Exception("El numero de columns de la primera matriz debe ser igual al numero de rows de la segunda");
		
		}
        public static TxSparseMatrix Identity(int size) 
        {
            TxSparseMatrix res = new TxSparseMatrix(size,size);

            for (int i = 0; i < size; i++)
            {
                res[i, i] = 1;
            }
            return res;
        }
        public static TxSparseMatrix  MultiplyWbyPTransPose(TxSparseMatrix w,TxSparseMatrix  p)
        {
           //W kl == P ik W ij Pjl
            int pcolscount = p.ColumnsCount;
            TxSparseMatrix  wres = new TxSparseMatrix( pcolscount,pcolscount);
            
            Node[]pcols = p.columns;
            Node[]prows = p.rows;  

            Node[]wcols = w.columns;
            Node[]wrows = w.rows;

            Node[] wresrows = wres.rows;
            Node[] wrescols = wres.columns;

            Node wrescursor=null;
            Node pcursor1=null;
            Node pcursor2=null;
            Node wcursor=null;
            
            Node res = null; 
            double total = 0;
            double totalp = 0;
            double val = 0 ;
            int cant = 0 ; 
            for (int k = 0; k < pcolscount ; k++)
            {

                wrescursor = wresrows[k];
                
                for (int l = k; l < pcolscount; l++)
                {
                                    
                    pcursor1 = pcols[k ];
                    pcursor2 = pcols[l];
                    while ( pcursor1.ColumnLink != null) 
                    {
                        pcursor1 = pcursor1.ColumnLink;

                        val = pcursor1.Value;

                        wcursor = wrows[pcursor1.Row];

                        while (wcursor.RowLink != null && pcursor2.ColumnLink != null)
                        {
                            if (wcursor.RowLink.Column < pcursor2.ColumnLink.Row)
                                wcursor = wcursor.RowLink;
                            else if (pcursor2.ColumnLink.Row < wcursor.RowLink.Column)
                                pcursor2 = pcursor2.ColumnLink;
                            else
                            {
                                wcursor = wcursor.RowLink;
                                pcursor2 = pcursor2.ColumnLink;
                               // if ( wcursor.Column != pcursor1.Row)
                                totalp += wcursor.Value * pcursor2.Value;
                            }
                        }
                        total += val * totalp; 
                        pcursor2 = pcols[l];
						wcursor = null;
                        totalp = 0;
                    }
                    if ( total != 0 )
                    {
                        res = new Node(total,k,l);
                        wrescursor.RowLink = res;
                        wrescursor = res;  
                        wrescols[l].RowLink.ColumnLink = res;
                        wrescols[l].RowLink = res;
                        cant++;
                        total = 0;
                    }
                }

                if (res != null) 
                {
                    wresrows[k].ColumnLink = res; 
                }
   
            }

            ExpandSymetric(wres);
            return wres;
        }
   
        //expand symetric
        private  static void ExpandSymetric(TxSparseMatrix w) 
        {
          int rows = w.rows.Length;
           
            Node[]wcols = w.columns;
            Node[]wrows = w.rows;
            Node symcursor;
            Node[] diag = new Node[rows];
            Node[] last = new Node[rows];
             Node cursor=null;
             Node val=null;
            for (int i = 0; i < rows; i++)
            {
                last[i] = w.rows[i];
                diag[i] = wrows[i].RowLink;                
            }
            
            for (int i = 0; i < rows; i++)
            {
                cursor = diag[i];
                symcursor = cursor;
                
                
                while (cursor.RowLink != null)
                {
                    cursor = cursor.RowLink; 
                    val = new Node(cursor.Value,cursor.Column, cursor.Row);

                    symcursor.ColumnLink = val;
                    symcursor = val;
                    last[val.Row].RowLink = val;
                    last[val.Row] = val;
                    val.RowLink = diag[val.Row];
                }

                wcols[i].RowLink = val;
            }
		   
        
        }

        //Returns the Laplacian matrix from a 
        public static TxSparseMatrix Laplacian(TxSparseMatrix w)
        {
            int length = w.RowsCount;
            TxSparseMatrix l = new TxSparseMatrix(length,length);
             Node wcursor;
             Node pcursor;
             Node pvalue=null;
             Node  diag=null;
            double total=0;
            double val;
            int row;
            int col;
            bool half=false; 
            
            for (int i = 0; i < length-1; i++)
            {
                wcursor = w.rows[i];
                pcursor = l.rows[i];

                while (wcursor.RowLink != null) 
                {
                    wcursor = wcursor.RowLink;
                    val = wcursor.Value;  
                    row = wcursor.Row;
                    col = wcursor.Column;
                    total += val;

                    if ( !half && col > i) 
                    {
                        half = true;

                        diag = new Node(0, i, i);
                        //insert the diagonal value
                        pcursor.RowLink = diag;

                        pcursor = diag;

                        l.columns[i].RowLink.ColumnLink = diag;

                        l.columns[i].RowLink = diag;
                    }
                    
                    //Insert new value in the row 
                    pvalue = new Node( -val , row , col);

                    pcursor.RowLink = pvalue;

                    pcursor = pvalue;

                     l.columns[col].RowLink.ColumnLink = pvalue;

                     l.columns[col].RowLink = pvalue;

                }
                
                l.rows[i].ColumnLink = pvalue;

                diag.Value = total; 
                half = false;
                total = 0;
            }

            //ultima fila columna
            int j= length - 1;
            total = 0;
                wcursor = w.rows[j];
                pcursor = l.rows[j];

                while (wcursor.RowLink != null)
                {
                    wcursor = wcursor.RowLink;
                    val = wcursor.Value;
                    row = wcursor.Row;
                    col = wcursor.Column;
                    total += val;
                    //Insert new value in the row 
                    pvalue = new Node(-val, row, col);

                    pcursor.RowLink = pvalue;

                    pcursor = pvalue;

                    l.columns[col].RowLink.ColumnLink = pvalue;

                    l.columns[col].RowLink = pvalue;
                }

                diag = new Node(total , j, j);
                pcursor.RowLink = diag;
                
                 l.columns[j].RowLink.ColumnLink = diag;

                l.columns[j].RowLink = diag;

                l.rows[j].ColumnLink = diag;
                
          


            return l;
        }
        /// <summary>
		/// Select  max value of the row, with column position flag labeled true in flags array and make sum equal to the total sum of the row.   
		/// </summary>
		/// <returns></returns>
      
        public double  MaxValue(int row,bool [] flags) 
        {
                Node cursor = rows[row];
                double max = 0;
                 
 
            while( cursor.RowLink != null) 
            {
                cursor = cursor.RowLink;
                if ( flags[ cursor.Column ] &&  cursor.Value > max ) 
                {
                    max = cursor.Value;
                }
            }
            return max;
        }



         //return the Sum of the  elements  in ith row excluding i,i
        public double  RowSum(int row) 
        {
                Node cursor = rows[row];
                double sum = 0;
            
             while( cursor.RowLink != null ) 
            {
                cursor = cursor.RowLink;
                 if( cursor.Column != row)
                    sum += cursor.Value;
             
            }
            return sum;
        }
        public double RowSum(int row,bool []flags)
        {
            Node cursor = rows[row];
            double sum = 0;

            while (cursor.RowLink != null)
            {
                cursor = cursor.RowLink;
                if( flags [cursor.Column] )
                sum += cursor.Value;

            }
            return sum;
        }
        //SPecial method to be used by SWA
        public static void NormalizeRow(int row,double norm,TxSparseMatrix src,TxSparseMatrix dst,bool[]flags,int[] cumul_seeds) 
        {
           Node cursorsrc = src.rows[row];
           Node cursordst = dst.rows[row];
           int i = 0;
           while ( cursorsrc.RowLink != null )
           {
               cursorsrc = cursorsrc.RowLink;

               if (flags[cursorsrc.Column]) 
               {
                   if( cursorsrc.Value != 0)
                   {
                     double val = cursorsrc.Value/(double)norm;
                     i = cumul_seeds[cursorsrc.Column];  
                       dst[row,i] = val;           
                   }

           
               }
           
           }   
        }
        //Returns the count of the column values greater than eps 
        public int ColumnValuesGreaterThan(int column, double eps) 
        {
            Node current = columns[column];
            int count = 0;
            while (current.ColumnLink != null) 
            {
                current = current.ColumnLink;

                if (current.Value > eps) 
                {
                    count++;
                }
            }
            return count;
        }

        //Mark the values of the specified column that are greater than tresh, in the vector array with making vector[i] =CheckMark if column[i] >= tresh
        public void  MarkValues(int col,int Mark,double tresh,int[]vector) 
        {
            Node current = columns[col];
            while (current.ColumnLink != null)
            {
                current = current.ColumnLink;

                if (current.Value > tresh)
                {
                    vector[current.Row] = Mark; 
                }
            }                 
        
        }


        //returns w = pT * W * p
        public static TxSparseMatrix MultiplybyColmumns(TxSparseMatrix w,TxSparseMatrix p,Node [,] diag)
        {
            int cols = p.columns.Length;
            TxSparseMatrix wres = new TxSparseMatrix(cols, cols);
            Node[] wresrows = wres.rows;
            Node[] wrescols = wres.columns;
            Node wrescursor;
            Node temp;
            SortedList<int,int> a ; 
            SortedList<int,int> b ; 
            a = new SortedList<int, int>(20);
            b = new SortedList<int, int>(20);
            Node cursorw;
            Node cursorp;
            Node[ ]pcolumns = p.columns;
            Node[ ] wcolumns = w.columns;
            Node[] prows = p.rows;
            int c=0;
            int r=0;
            double val;
            for (int i = 0; i < cols; i++)
            {   
                
                 cursorp = pcolumns[i];
                 while (cursorp.ColumnLink != null)
                 {
                     cursorp = cursorp.ColumnLink;
                     val =cursorp.Value;

                     r = cursorp.Row;
                     
                     cursorw = wcolumns[r];
                     while (cursorw.ColumnLink != null)
                     {
                          cursorw = cursorw.ColumnLink;
                          r = cursorw.Row;
                          if (col[r] == 0)
                          {
                              col[r] = val * cursorw.Value;

                              if ( col[r] != 0)
                              {
                                  a.Add(r, r);
                              }
                              else 
                              {
                                  col[r] = 0;
                              }
                          }
                          else 
                          {
                              col[r] += val * cursorw.Value;
                              
                              if (col[r] == 0)
                              {   
                                  a.Remove(r);
                                  col[r] = 0;
                              }
                          }
                     }
                   
                 }
                 foreach (int k in a.Keys)
                 {
                     val = col[k];
                     col[k] = 0;

                     cursorp = prows[k];

                     while (cursorp.RowLink != null)
                     {
                         cursorp = cursorp.RowLink;
                         c = cursorp.Column;
                         if (col2[c] == 0)
                         {
                             col2[c] = val * cursorp.Value;
                             if (col2[c] != 0)
                             {
                                 b.Add(c, c);
                             }
                             else 
                             {
                                 col2[c] = 0;
                             }
                         }
                         else
                         {
                             col2[c] += val * cursorp.Value;

                             if (col2[c] == 0) 
                             {
                                 b.Remove(c);
                                 col2[c] = 0;
                             }

                         }
                     }

                 }
                 wrescursor = wrescols[i];
                 foreach (int k in b.Keys)
                 {
                     val = col2[k];
                     col2[k]=0;

                     if ( Math.Abs(val) > eps)
                     {
                         temp = new Node(val, k, i);
                         wrescursor.ColumnLink = temp;
                         wrescursor = temp;
                         wresrows[k].ColumnLink.RowLink = temp;
                         wresrows[k].ColumnLink = temp;

                         if (k < i)
                         {

                             diag[0, i] = temp;
                         }
                         else if (k > i)
                         {
                             diag[1, k] = temp;
                         }
                         else
                         {
                             diag[0, i] = temp;
                             diag[1, i] = temp;
                         }
                     }
                 }
                 wrescols[i].RowLink = wrescursor;
                 
                  b.Clear();
                 a.Clear();
            }
            return wres;
        }
        public static TxSparseMatrix MultiplybyColmumns(TxSparseMatrix a, TxSparseMatrix b)
        {


            int cols =  b.columns.Length;
            int rows = a.rows.Length;
            TxSparseMatrix res = new TxSparseMatrix(rows, cols);
            Node[] resrows = res.rows;
            Node[] rescols = res.columns;
            Node rescursor;
            Node temp;
            SortedList<int, int> list;
           
       
            list = new SortedList<int, int>(20);
            Node cursora;
            Node cursorb;
            Node[] bcolumns = b.columns;
            Node[] acolumns = a.columns;
            int c = 0;
            int r = 0;
            double val;
            for (int i = 0; i < cols; i++)
            {

                cursorb = bcolumns[i];
                while (cursorb.ColumnLink != null)
                {
                    cursorb = cursorb.ColumnLink;
                    val = cursorb.Value;

                    r = cursorb.Row;

                    cursora = acolumns[r];
                    while (cursora.ColumnLink != null)
                    {
                        cursora = cursora.ColumnLink;
                        r = cursora.Row;
                        if (col[r] == 0)
                        {
                            col[r] = val * cursora.Value;

                            if (col[r] != 0)
                            {
                                list.Add(r, r);
                            }
                            
                        }
                        else
                        {
                            col[r] += val * cursora.Value;

                            if (col[r] == 0)
                            {
                                list.Remove(r);
                                
                            }
                        }
                    }

                }
                rescursor = rescols[i];

                foreach (int k in list.Keys)
                {
                    val = col[k];
                    col[k] = 0;

                       if (val > eps2)
                    {
                        temp = new Node(val, k, i);
                        rescursor.ColumnLink = temp;
                        rescursor = temp;
                        resrows[k].ColumnLink.RowLink = temp;
                        resrows[k].ColumnLink = temp;
                    }
                    

                }
                
                rescols[i].RowLink = rescursor;

                list.Clear();
            }
            return res;
        }
        
        //returns
        public static TxSparseMatrix SMatrixSVectorMultiply(TxSparseMatrix p,TxSparseMatrix u)
        {
            int cols = p.rows.Length; 
            TxSparseMatrix ures = new TxSparseMatrix(cols,1);
            
            return null;
        }
        public static TxSparseMatrix SMatrixSVectorMultiply(TxSparseMatrix p, TxSparseMatrix u,double toptresh,double  bottresh)
        {
            int cols = p.rows.Length;
            TxSparseMatrix ures = new TxSparseMatrix(cols, 1);
            Node[] uresrows = ures.rows;
            Node cursoru = u.columns[0];
            Node cursorp;
            Node urescursor = ures.columns[0];
            Node temp;
            SortedList<int, int> rowlist = new SortedList<int, int>(20); 

            Node[] pcolumns = p.columns;
            double val;
            int r;
            
            while (cursoru.ColumnLink != null)
            {
                cursoru = cursoru.ColumnLink;
                val = cursoru.Value;

                r = cursoru.Row;

                cursorp = pcolumns[r];
                while (cursorp.ColumnLink != null)
                {
                    cursorp = cursorp.ColumnLink;
                    r = cursorp.Row;
                    if (col[r] == 0)
                    {
                        col[r] = val * cursorp.Value;
                        if (col[r] != 0)
                        {
                            rowlist.Add(r, r);
                        }
                    }
                    else
                    {
                        col[r] += val * cursorp.Value;

                        if (col[r] == 0)
                        {
                            rowlist.Remove(r);
                        }
                    }
                }

                
            }
            foreach (int k in rowlist.Keys)
            {
                val = col[k];
                col[k] = 0;


                if (val >= bottresh)
                {   
                    //Subir a 1 el valor
                    if (val >= toptresh)
                    {
                        val = 1;
                    }
                    temp = new Node(val, k, 0);
                    urescursor.ColumnLink = temp;
                    urescursor = temp;
                    uresrows[k].ColumnLink.RowLink = temp;
                    uresrows[k].ColumnLink = temp;
                }
                //else llevar a cero ,no representar 

            }
            ures.columns[0].RowLink = urescursor;
            rowlist.Clear();
            
            
            return ures ;
        }
          public double MultiplyColumnByDenseVector (int col ,int[ ]vector,ref double colsum)
          {
              Node cursor = columns[col];
              colsum = 0;
              double val= 0;
              double res = 0;
                 while (cursor.ColumnLink != null)
                {
                    cursor = cursor.ColumnLink;
                    val = cursor.Value;

                    colsum += val;
                    res += val * vector[cursor.Row];
                      
                }
                return res;
          }
        public double[] MultiplyColumnByDenseVector(int col, int[] vector1,int[] vector2,double[]res,ref double colsum)
        {
            Node cursor = columns[col];
            colsum = 0;
            double val = 0;
            res[0] = 0;
            res[1] = 0;

            while (cursor.ColumnLink != null)
            {
                cursor = cursor.ColumnLink;
                val = cursor.Value;

                colsum += val;
                res[0] += val * vector1[cursor.Row];
                res[1] += val * vector2[cursor.Row];
            }
            return res;
        } 
    }





	/// <summary>
	/// En esta clase se almacenan los elementos que contiene la matriz  
	/// </summary>
	public class Node
	{     
               int  row;
		       int column ;      
               double val ;
	     
		    Node rowlink = null;
		 Node columnlink = null;
		
		
		public Node( double val , int row , int column )  
		{
		  this.val = val;
		   this.row = row;
		this.column = column;
		}
		/// <summary>
		/// Esta propiedad retorna el proximo elemento en la row
		/// </summary>
		public Node RowLink {
			get { return rowlink ;}   
			set { rowlink = value; }
		                       }
		/// <summary>
		/// Esta propiedad retorna el proximo elemento en la column
		/// </summary>
		public Node ColumnLink{
			get { return columnlink ;}
			set { columnlink = value; } 
		    }
		
	/// <summary>
	/// Esta propiedad retorna el elemento almacenado en este Node 
	/// </summary>

        public double Value { get { return val; } set { val = value; } }                      	
		
		/// <summary>
		/// Esta propiedad retorna la row donde se encuentra el elemento almacenado 
		/// en el Node 
		/// </summary>
		public int Row {get { return row ;}}
   	  /// <summary>
	  /// Esta propiedad retorna la column donde se encuentra el elemento almacenado 
	  /// en el Node 
   	  /// </summary>
		public int Column{get {return column;}}
	
	}







}
