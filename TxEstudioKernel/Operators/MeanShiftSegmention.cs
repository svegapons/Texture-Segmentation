using System;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel.Operators
{
    [Algorithm("Mean Shift", "Segmentación mediante el procedimiento mean shift")]
    [Abbreviation("mshift", "SpatialBandWidth", "ResolutionBandWidth", "MinRegion")]
    public class MeanShiftSegmentation : TxSegmentationAlgorithm
    {
        int sigmaS = 5;
        float sigmaR = 45.5f;
        int minRegion = 10000;

        //Input Data Parameters
        int width, height, N;
        TxMatrix[] data;

        //Output Data Storage
        int regionCount;
        int[,] labels;
        float[, ,] msRawData; 
        int[] modePointCounts;
        float[,] modes;

        //Basin of Attraction
        int[,] modeTable;
        int pointCount;
        int[] pointList;

        //Image Clasification
        int[,] neigh;
        int[] indexTable;
        float LUV_treshold = 1.0f;

        //Region Adjacency Matrix
        RAList[] raList;
        RAList freeRAList;	

        //Computation of Edge Strenghts
        float epsilon = 1.0f;

        //Constantes Definidas
        const double EPSILON = 0.01;			// define threshold (approx. Value of Mh at a peak or plateau)
        const int LIMIT = 100;		// define max. # of iterations to find mode
        const double TC_DIST_FACTOR = 0.5;		// cluster search windows near convergence that are a distance
                                                // h[i]*TC_DIST_FACTOR of one another (transitive closure)
        const int NODE_MULTIPLE	= 10;

        /// <summary>
        /// Crea una instancia de MeanShiftSegmentation
        /// </summary>
        public MeanShiftSegmentation()
        {
            //define eight connected neighbors
            neigh = new int[8, 2];

            neigh[0, 0] = 0;
            neigh[0, 1] = 1;

            neigh[1, 0] = -1;
            neigh[1, 1] = 1;

            neigh[2, 0] = -1;
            neigh[2, 0] = 0;

            neigh[3, 0] = -1;
            neigh[3, 1] = -1;

            neigh[4, 0] = 0;
            neigh[4, 1] = -1;

            neigh[5, 0] = 1;
            neigh[5, 1] = -1;

            neigh[6, 0] = 1;
            neigh[6, 1] = 0;

            neigh[7, 0] = 1;
            neigh[7, 1] = 1;
        }

        //metodo heredado de Iclasificable.
        public override double ProbError()
        {
            throw new Exception("Este segmentador no tiene implementado una funcion para la estimacion no supervisada del error de clasificacion");
        }
        
        public override TxSegmentationResult Segment(params TxMatrix[] descriptors)
        {
            width =  descriptors[0].Width;
            height = descriptors[0].Height;
            N = descriptors.Length;
            data = descriptors;

            msRawData = new float[N, height, width];

	        //Apply mean shift to data set using sigmaS and sigmaR...
	        Filter(sigmaS, sigmaR);

	        //Apply transitive closure iteratively to the regions classified
	        //by the RAM updating labels and modes until the color of each neighboring
	        //region is within sqrt(rR2) of one another.
            float rR2 = sigmaR * sigmaR * 0.25f;
            TransitiveClosure(rR2);
            int oldRC = regionCount;
            int deltaRC, counter = 0;
            do
            {
                TransitiveClosure(rR2);
                deltaRC = oldRC - regionCount;
                oldRC = regionCount;
                counter++;
            } while ((deltaRC <= 0) && (counter < 10));

	        //Prune spurious regions (regions whose area is under minRegion) using RAM
	        Prune(minRegion);

            TxSegmentationResult segmentedImage = new TxSegmentationResult(regionCount, width, height);
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    segmentedImage[i, j] = labels[i, j];
            return segmentedImage;
        }

        /// <summary>
        /// Prunes regions from the image whose pixel density is less than a specified threshold.
        /// </summary>
        /// <param name="minRegion">The minimum allowable pixel density a region
        /// may have without being pruned</param>
        private void Prune(int minRegion)
        {
            //allocate memory for mode and point count temporary buffers...
            float[,] modes_buffer = new float[N, regionCount];
            int[] MPC_buffer = new int[regionCount];

            //allocate memory for label buffer
            int[] label_buffer = new int[regionCount];

            //Declare variables
            int candidate, iCanEl, neighCanEl, iMPC, label, oldRegionCount, minRegionCount;
            double minSqDistance, neighborDistance;
            RAList neighbor;

            //Apply pruning algorithm to classification structure, removing all regions whose area
            //is under the threshold area minRegion (pixels)
            do
            {
                //Assume that no region has area under threshold area  of 
                minRegionCount = 0;

                //Step (1):

                // Build RAM using classifiction structure originally
                // generated by the method GridTable::Connect()
                BuildRAM();

                // Step (2):

                // Traverse the RAM joining regions whose area is less than minRegion (pixels)
                // with its respective candidate region.

                // A candidate region is a region that displays the following properties:

                //	- it is adjacent to the region being pruned

                //  - the distance of its mode is a minimum to that of the region being pruned
                //    such that or it is the only adjacent region having an area greater than
                //    minRegion

                for (int i = 0; i < regionCount; i++)
                {
                    //if the area of the ith region is less than minRegion
                    //join it with its candidate region...

                    //*******************************************************************************

                    //Note: Adjust this if statement if a more sophisticated pruning criterion
                    //      is desired. Basically in this step a region whose area is less than
                    //      minRegion is pruned by joining it with its "closest" neighbor (in color).
                    //      Therefore, by placing a different criterion for fusing a region the
                    //      pruning method may be altered to implement a more sophisticated algorithm.

                    //*******************************************************************************

                    if (modePointCounts[i] < minRegion)
                    {
                        //update minRegionCount to indicate that a region
                        //having area less than minRegion was found
                        minRegionCount++;

                        //obtain a pointer to the first region in the
                        //region adjacency list of the ith region...
                        neighbor = raList[i].next;

                        //calculate the distance between the mode of the ith
                        //region and that of the neighboring region...
                        candidate = neighbor.label;
                        minSqDistance = SqDistance(i, candidate);

                        //traverse region adjacency list of region i and select
                        //a candidate region
                        neighbor = neighbor.next;
                        while (neighbor != null)
                        {

                            //calculate the square distance between region i
                            //and current neighbor...
                            neighborDistance = SqDistance(i, neighbor.label);

                            //if this neighbors square distance to region i is less
                            //than minSqDistance, then select this neighbor as the
                            //candidate region for region i
                            if (neighborDistance < minSqDistance)
                            {
                                minSqDistance = neighborDistance;
                                candidate = neighbor.label;
                            }

                            //traverse region list of region i
                            neighbor = neighbor.next;

                        }

                        //join region i with its candidate region:

                        // (1) find the canonical element of region i
                        iCanEl = i;
                        while (raList[iCanEl].label != iCanEl)
                            iCanEl = raList[iCanEl].label;

                        // (2) find the canonical element of neighboring region
                        neighCanEl = candidate;
                        while (raList[neighCanEl].label != neighCanEl)
                            neighCanEl = raList[neighCanEl].label;

                        // if the canonical elements of are not the same then assign
                        // the canonical element having the smaller label to be the parent
                        // of the other region...
                        if (iCanEl < neighCanEl)
                            raList[neighCanEl].label = iCanEl;
                        else
                        {
                            //must replace the canonical element of previous
                            //parent as well
                            raList[raList[iCanEl].label].label = neighCanEl;

                            //re-assign canonical element
                            raList[iCanEl].label = neighCanEl;
                        }
                    }
                }

                // Step (3):

                // Level binary trees formed by canonical elements
                for (int i = 0; i < regionCount; i++)
                {
                    iCanEl = i;
                    while (raList[iCanEl].label != iCanEl)
                        iCanEl = raList[iCanEl].label;
                    raList[i].label = iCanEl;
                }

                // Step (4):

                //Traverse joint sets, relabeling image.

                // Accumulate modes and re-compute point counts using canonical
                // elements generated by step 2.

                //traverse raList accumulating modes and point counts
                //using canoncial element information...
                for (int i = 0; i < regionCount; i++)
                {

                    //obtain canonical element of region i
                    iCanEl = raList[i].label;

                    //obtain mode point count of region i
                    iMPC = modePointCounts[i];

                    //accumulate modes_buffer[iCanEl]
                    for (int k = 0; k < N; k++)
                        modes_buffer[k, iCanEl] += iMPC * modes[k, i];

                    //accumulate MPC_buffer[iCanEl]
                    MPC_buffer[iCanEl] += iMPC;

                }

                // (b)

                // Re-label new regions of the image using the canonical
                // element information generated by step (2)

                // Also use this information to compute the modes of the newly
                // defined regions, and to assign new region point counts in
                // a consecute manner to the modePointCounts array

                //initialize label buffer to -1
                for (int i = 0; i < regionCount; i++)
                    label_buffer[i] = -1;

                //traverse raList re-labeling the regions
                label = -1;
                for (int i = 0; i < regionCount; i++)
                {
                    //obtain canonical element of region i
                    iCanEl = raList[i].label;
                    if (label_buffer[iCanEl] < 0)
                    {
                        //assign a label to the new region indicated by canonical
                        //element of i
                        label_buffer[iCanEl] = ++label;

                        //recompute mode storing the result in modes[label]...
                        iMPC = MPC_buffer[iCanEl];
                        for (int k = 0; k < N; k++)
                            modes[k, label] = (modes_buffer[k, iCanEl]) / (iMPC);

                        //assign a corresponding mode point count for this region into
                        //the mode point counts array using the MPC buffer...
                        modePointCounts[label] = MPC_buffer[iCanEl];
                    }
                }

                //re-assign region count using label counter
                oldRegionCount = regionCount;
                regionCount = label + 1;

                // (c)

                // Use the label buffer to reconstruct the label map, which specified
                // the new image given its new regions calculated above

                for (int i = 0; i < height; i++)
                    for(int j = 0; j < width; j++)
                        labels[i, j] = label_buffer[raList[labels[i, j]].label];


            } while (minRegionCount > 0);
        }

        /// <summary>
        /// Computs the normalized square distance between two modes.
        /// </summary>
        /// <param name="mode1">Index into the modes array specifying a mode of the image.</param>
        /// <param name="mode2">Index into the modes array specifying a mode of the image.</param>
        /// <returns></returns>
        private float SqDistance(int mode1, int mode2)
        {
            float dist = 0, el;

            //Calculate distance squared of sub-space s	
            for (int p = 0; p < N; p++)
            {
                el = (modes[p, mode1] - modes[p, mode2]) / sigmaR;
                dist += el * el;
            }

            //return normalized square distance between modes 1 and 2
            return dist;
        }

        /// <summary>
        /// Apply mean shift filter to the defined image.      
        /// </summary>
        /// <param name="sigmaS">The spatial radius of the mean shift window.</param>
        /// <param name="sigmaR">The range radius of the mean shift window.</param>
        /// <param name="speedUpLevel">Determines if a speed up optimization should be
        /// used to perform image filtering.</param>
        private void Filter(int sigmaS, float sigmaR/*, SpeedUpLevel speedUpLevel*/)
        {
            //Allocate memory for msRawData (filtered image output)
            if ((msRawData = new float[N, height, width]) == null)
                throw new Exception("Not enough memory.");

            //Allocate memory used to store image modes and their corresponding regions...
            if (((modes = new float[N + 2, height* width]) == null) || ((labels = new int[height, width]) == null) || ((modePointCounts = new int[height* width]) == null) || ((indexTable = new int[height * width]) == null))
                throw new Exception("Not enough memory.");

	        //Allocate memory for basin of attraction mode structure...
	        if(((modeTable = new int[height, width]) == null)||((pointList = new int[height * width]) == null))
                throw new Exception("Not enough memory.");

            OptimizedFilter(sigmaS, sigmaR);

	        //Perform connecting (label image regions) using LUV_data
	        Connect();
        }

        /// <summary>
        /// Classifies mean shift filtered image regions using
        /// private classification structure of this class
        /// </summary>
        private void Connect()
        {
            //initialize labels and "modePointCounts"
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    labels[i, j] = -1;

            //Traverse the image labeling each new region encountered
            int label = -1; 
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    //if this region has not yet been labeled - label it
                    if (labels[i, j] < 0)
                    {
                        //assign new label to this region
                        labels[i, j] = ++label;

                        //copy region color into modes
                        for (int k = 0; k < N; k++)
                            modes[k, label] = msRawData[k, i, j];

                        //populate labels with label for this specified region
                        //calculating modePointCounts[label]...
                        Fill(i, j, label);
                    }
                }

            //calculate region count using label
            regionCount = label + 1;
        }

        /// <summary>
        /// Used by Connect to perform label each region in the
        /// mean shift filtered image using an eight-connected fill
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="label"></param>
        private void Fill(int regionLocX, int regionLocY, int label)
        {
            //declare variables
            int neighLocX, neighLocY, neighborsFound;//, imageSize = width * height;

            //Fill region starting at region location using labels...
            int regionLoc = regionLocX * width + regionLocY;

            //initialzie indexTable
            int index = 0;
            indexTable[0] = regionLoc;

            //increment mode point counts for this region to
            //indicate that one pixel belongs to this region
            modePointCounts[label]++;

            while (true)
            {
                //assume no neighbors will be found
                neighborsFound = 0;

                //check the eight connected neighbors at regionLoc -
                //if a pixel has similar color to that located at 
                //regionLoc then declare it as part of this region
                for (int i = 0; i < 8; i++)
                {
                    // no need
                    /*
                       //if at boundary do not check certain neighbors because
                       //they do not exist...
                       if((regionLoc%width == 0)&&((i == 3)||(i == 4)||(i == 5)))
                           continue;
                       if((regionLoc%(width-1) == 0)&&((i == 0)||(i == 1)||(i == 7)))
                           continue;
                    */

                    //check bounds and if neighbor has been already labeled
                    neighLocX = regionLocX + neigh[i, 0];
                    neighLocY = regionLocY + neigh[i, 1];
                    if ((neighLocX >= 0) && (neighLocX < height) && (neighLocY >= 0) && (neighLocY < width) && (labels[neighLocX, neighLocY] < 0))
                    {   
                        int k;
                        for ( k= 0; k < N; k++)
                            if (Math.Abs(msRawData[k, regionLocX, regionLocY] - msRawData[k, neighLocX, neighLocY]) >= LUV_treshold)
                                break;

                        //neighbor i belongs to this region so label it and
                        //place it onto the index table buffer for further processing
                        if (k == N)
                        {
                            //assign label to neighbor i
                            labels[neighLocX, neighLocY] = label;

                            //increment region point count
                            modePointCounts[label]++;

                            //place index of neighbor i onto the index tabel buffer
                            indexTable[++index] = neighLocX * width + neighLocY;

                            //indicate that a neighboring region pixel was identified
                            neighborsFound = 1;
                        }
                    }
                }

                //check the indexTable to see if there are any more
                //entries to be explored - if so explore them, otherwise exit the loop - we are finished
                if (neighborsFound != 0)
                    regionLoc = indexTable[index];
                else if (index > 1)
                    regionLoc = indexTable[--index];
                else
                    break; //fill complete
            }
        }

        /// <summary>
        /// Filters the image using previous mode information
        /// to avoid re-applying mean shift to some data points
        /// Advantage   : maintains high level of accuracy,
        ///               large speed up compared to non-optimized version
        /// Disadvantage  : POSSIBLY not as accurate as non-optimized version
        /// </summary>
        /// <param name="sigmaS">The spatial radius of the mean shift window.</param>
        /// <param name="sigmaR">The range radius of the mean shift window.</param>
        private void OptimizedFilter(float sigmaS, float sigmaR)
        {
	        // Declare Variables
	        int		iterationCount, modeCandidateX, modeCandidateY;//, modeCandidate_i;
	        double	mvAbs, diff, el;

            //define input data dimension with lattice
	        int lN	= N + 2;
        	
	        // Traverse each data point applying mean shift
	        // to each data point
        	
	        // Allcocate memory for yk
            double[] yk	= new double[lN];
        	
	        // Allocate memory for Mh
            double[] Mh	= new double[lN];

           // let's use some temporary data
           float[,,] sdata = new float[lN, height, width];

          // copy the scaled data
          for(int i = 0; i < height; i++)
            for(int j = 0; j < width; j++)
            {
                sdata[0, i, j] = j / sigmaS;
                sdata[1, i, j] = i / sigmaS;
            }
            for(int k = 2; k < lN; k++)
              for(int i = 0; i < height; i++)
                for(int j = 0; j < width; j++)
                {
                    sdata[k, i, j] = data[k - 2][i, j] / sigmaR;
                    sdata[k, i, j] = data[k - 2][i, j] / sigmaR;
                 }
            
            // index the data in the 3d buckets (x, y, L)

            int[,,] buckets;
           int[,] slist = new int[height, width];
           int[,,] bucNeigh = new int[3, 3, 3];

           float sMins;
           float[] sMaxs = new float[3];
           sMaxs[0] = width / sigmaS;
           sMaxs[1] = height / sigmaS;
           sMins = sMaxs[2] = sdata[2, 0, 0];
           float cval;
           for (int i = 0; i < height; i++)
               for (int j = 0; j < width; j++)
               {
                   cval = sdata[2, i, j];
                   if (cval < sMins)
                       sMins = cval;
                   else if (cval > sMaxs[2])
                       sMaxs[2] = cval;
               }

           int nBuck1, nBuck2, nBuck3;
           int cBuck1, cBuck2, cBuck3, cBuck;
           nBuck1 = (int) (sMaxs[0] + 3);
           nBuck2 = (int) (sMaxs[1] + 3);
           nBuck3 = (int) (sMaxs[2] - sMins + 3);
           buckets = new int[nBuck1, nBuck2, nBuck3];
           for (int i = 0; i < nBuck1; i++)
               for (int j = 0; j < nBuck2; j++)
                   for (int k = 0; k < nBuck3; k++)
                       buckets[i, j, k] = -1;
           for (int i = 0; i < height; i++)
               for (int j = 0; j < width; j++)
               {
                  // find bucket for current data and add it to the list
                   cBuck1 = (int)sdata[0, i, j] + 1;
                   cBuck2 = (int)sdata[1, i, j] + 1;
                   cBuck3 = (int)(sdata[2, i, j] - sMins) + 1;

                   slist[i, j] = buckets[cBuck1, cBuck2, cBuck3];
                   buckets[cBuck1, cBuck2, cBuck3] = j + width * i;
               }

           for (cBuck1 = -1; cBuck1 <= 1; cBuck1++)
               for (cBuck2 = -1; cBuck2 <= 1; cBuck2++)
                   for (cBuck3 = -1; cBuck3 <= 1; cBuck3++)
                       bucNeigh[cBuck1 + 1, cBuck2 + 1, cBuck3 + 1] = cBuck1 + nBuck1 * (cBuck2 + nBuck2 * cBuck3);
          double wsuml, weight;
           double hiLTr = 80.0/sigmaR;

           // done indexing/hashing
           // Initialize mode table used for basin of attraction
            modeTable = new int[height, width];

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
	            {
		            // if a mode was already assigned to this data point
		            // then skip this point, otherwise proceed to
		            // find its mode by applying mean shift...
		            if (modeTable[i, j] == 1)
			            continue;

		            // initialize point list...
		            pointCount = 0;

		            // Assign window center (window centers are
		            // initialized by createLattice to be the point
		            // data[i])
                    for (int k = 0; k < lN; k++)
                        yk[k] = sdata[k, i, j];
            		
		            // Calculate the mean shift vector using the lattice
		            // LatticeMSVector(Mh, yk); // modify to new
                    /*****************************************************/

                   // Initialize mean shift vector
                    Mh = new double[lN];

                    wsuml = 0;
                    // uniformLSearch(Mh, yk_ptr);  modify to new find bucket of yk
                      cBuck1 = (int) yk[0] + 1;
                      cBuck2 = (int) yk[1] + 1;
                      cBuck3 = (int) (yk[2] - sMins) + 1;
                      cBuck = cBuck1 + nBuck1 * (cBuck2 + nBuck2 * cBuck3);
                      for (int k1 = 0; k1 < 3; k1++)
                          for (int k2 = 0; k2 < 3; k2++)
                              for (int k3 = 0; k3 < 3; k3++)
                              {
                                  cBuck1 = (cBuck + bucNeigh[k1, k2, k3]) % nBuck1;
                                  cBuck2 = ((cBuck + bucNeigh[k1, k2, k3]) / nBuck1) % nBuck2;
                                  cBuck3 = ((cBuck + bucNeigh[k1, k2, k3]) / nBuck1) / nBuck2;
                                  int idxd = buckets[cBuck1, cBuck2, cBuck3];
                                 // list parse, crt point is cHeadList
                                 while (idxd >= 0)
                                 {
                                     // determine if inside search window
                                     el = sdata[0, idxd / width, idxd %width] - yk[0];
                                     diff = el * el;
                                     el = sdata[1, idxd / width, idxd % width] - yk[1];
                                     diff += el * el;

                                    if (diff < 1.0)
                                    {
                                        el = sdata[2, idxd / width, idxd % width] - yk[2];
                                        if (yk[2] > hiLTr)
                                            diff = 4 * el * el;
                                        else
                                            diff = el * el;

                                       if (N > 2)
                                       {
                                           el = sdata[3, idxd / width, idxd % width] - yk[3];
                                          diff += el*el;
                                          el = sdata[4, idxd / width, idxd % width] - yk[4];
                                          diff += el * el;
                                       }

                                       if (diff < 1.0)
                                       {
                                           weight = 1 /*- weightMap[idxd / width, idxd % width]*/;
                                          for (int k = 0; k < lN; k++)
                                              Mh[k] += weight * sdata[k, idxd / width, idxd % width];
                                          wsuml += weight;
                                       }
                                    }
                                    idxd = slist[idxd / width, idxd % width];
                                 }
                              }
   	                if (wsuml > 0)
   	                {
		                   for(int k = 0; k < lN; k++)
   			                Mh[k] = Mh[k]/wsuml - yk[k];
   	                }
   	                else
   	                {
		                   for(int k = 0; k < lN; k++)
   			                Mh[k] = 0;
   	                }
                      /*****************************************************/
   	                // Calculate its magnitude squared
		                //mvAbs = 0;
		                //for(j = 0; j < lN; j++)
		                //	mvAbs += Mh[j]*Mh[j];
                    mvAbs = (Mh[0] * Mh[0] + Mh[1] * Mh[1]) * sigmaS * sigmaS;
                    if (N == 3)
                        mvAbs += (Mh[2] * Mh[2] + Mh[3] * Mh[3] + Mh[4] * Mh[4]) * sigmaR * sigmaR;
                    else
                        mvAbs += Mh[2] * Mh[2] * sigmaR * sigmaR;

            		
		            // Keep shifting window center until the magnitude squared of the
		            // mean shift vector calculated at the window center location is
		            // under a specified threshold (Epsilon)
            		
		            // NOTE: iteration count is for speed up purposes only - it
		            //       does not have any theoretical importance
		            iterationCount = 1;
		            while((mvAbs >= EPSILON)&&(iterationCount < LIMIT))
		            {
            			
			            // Shift window location
			            for(int k = 0; k < lN; k++)
				            yk[k] += Mh[k];
            			
			            // check to see if the current mode location is in the
                        // basin of attraction...

                        modeCandidateY = (int)(sigmaS * yk[0] + 0.5);
                        modeCandidateX = (int)(sigmaS * yk[1] + 0.5);

                        // if mvAbs != 0 (yk did indeed move) then check
			            // location basin_i in the mode table to see if
			            // this data point either:
            			
			            // (1) has not been associated with a mode yet
			            //     (modeTable[basin_i] = 0), so associate
			            //     it with this one
			            //
			            // (2) it has been associated with a mode other
			            //     than the one that this data point is converging
			            //     to (modeTable[basin_i] = 1), so assign to
			            //     this data point the same mode as that of basin_i

                        if ((modeTable[modeCandidateX, modeCandidateY] != 2) && ((modeCandidateX != i) || (modeCandidateY != j)))
			            {
				            // obtain the data point at basin_i to
				            // see if it is within h*TC_DIST_FACTOR of yk
                            diff = 0;
                            for (int k = 2; k < lN; k++)
                            {
                                el = sdata[k, modeCandidateX, modeCandidateY] - yk[k];
                                diff += el * el;
                            }

				                // if the data point at basin_i is within
				                // a distance of h*TC_DIST_FACTOR of yk
				                // then depending on modeTable[basin_i] perform
				                // either (1) or (2)
				                if (diff < TC_DIST_FACTOR)
				                {
					                // if the data point at basin_i has not
					                // been associated to a mode then associate
					                // it with the mode that this one will converge
					                // to
                                    if (modeTable[modeCandidateX, modeCandidateY] == 0)
					                {
						                // no mode associated yet so associate it with this one...
                                        pointList[pointCount++] = modeCandidateX * width + modeCandidateY;
                                        modeTable[modeCandidateX, modeCandidateY] = 2;

					                } 
                                    else
					                {

						                // the mode has already been associated with
						                // another mode, thererfore associate this one
						                // mode and the modes in the point list with
						                // the mode associated with data[basin_i]...

						                // store the mode info into yk using msRawData...
                                        for (int k = 0; k < N; k++)
                                            yk[k + 2] = msRawData[k, modeCandidateX, modeCandidateY] / sigmaR;

						                // update mode table for this data point
						                // indicating that a mode has been associated with it
                                        modeTable[i, j] = 1;

						                // indicate that a mode has been associated
						                // to this data point (data[i])
						                mvAbs = -1;

						                // stop mean shift calculation...
						                break;
					                }
				                }
			            }
            			
                         // Calculate the mean shift vector at the new
                         // window location using lattice
                         // Calculate the mean shift vector using the lattice
                         // LatticeMSVector(Mh, yk); // modify to new
                        /*****************************************************/

                        // Initialize mean shift vector
                        Mh = new double[lN];

                        wsuml = 0;
                         // uniformLSearch(Mh, yk_ptr); modify to new find bucket of yk
                         cBuck1 = (int) yk[0] + 1;
                         cBuck2 = (int) yk[1] + 1;
                         cBuck3 = (int) (yk[2] - sMins) + 1;
                         cBuck = cBuck1 + nBuck1*(cBuck2 + nBuck2*cBuck3);
                         for (int k1 = 0; k1 < 3; k1++)
                             for (int k2 = 0; k2 < 3; k2++)
                                 for (int k3 = 0; k3 < 3; k3++)
                                 {
                                     cBuck1 = (cBuck + bucNeigh[k1, k2, k3]) % nBuck1;
                                     cBuck2 = ((cBuck + bucNeigh[k1, k2, k3]) / nBuck1) % nBuck2;
                                     cBuck3 = ((cBuck + bucNeigh[k1, k2, k3]) / nBuck1) / nBuck2;
                                     int idxd = buckets[cBuck1, cBuck2, cBuck3];
                                     // list parse, crt point is cHeadList
                                     while (idxd >= 0)
                                     {
                                         el = sdata[0, idxd / width, idxd % width] - yk[0];
                                         diff = el * el;
                                         el = sdata[1, idxd / width, idxd % width] - yk[1];
                                         diff += el * el;

                                         if (diff < 1.0)
                                         {
                                             el = sdata[2, idxd / width, idxd % width] - yk[2];
                                             if (yk[2] > hiLTr)
                                                 diff = 4 * el * el;
                                             else
                                                 diff = el * el;

                                             if (N > 2)
                                             {
                                                 el = sdata[3, idxd / width, idxd % width] - yk[3];
                                                 diff += el * el;
                                                 el = sdata[4, idxd / width, idxd % width] - yk[4];
                                                 diff += el * el;
                                             }

                                             if (diff < 1.0)
                                             {
                                                 weight = 1 /*- weightMap[idxd / width, idxd % width]*/;
                                                 for (int k = 0; k < lN; k++)
                                                     Mh[k] += weight * sdata[k, idxd / width, idxd % width];
                                                 wsuml += weight;
                                             }
                                         }
                                         idxd = slist[idxd / width, idxd % width];
                                     }
                                 }
                         if (wsuml > 0)
                         {
                            for(int k = 0; k < lN; k++)
                               Mh[k] = Mh[k]/wsuml - yk[k];
                         }
                         else
                         {
                            for(int k = 0; k < lN; k++)
                               Mh[k] = 0;
                         }
                         /*****************************************************/
                			
			                // Calculate its magnitude squared
			                //mvAbs = 0;
			                //for(j = 0; j < lN; j++)
			                //	mvAbs += Mh[j]*Mh[j];
                         mvAbs = (Mh[0] * Mh[0] + Mh[1] * Mh[1]) * sigmaS * sigmaS;
                         if (N == 3)
                             mvAbs += (Mh[2] * Mh[2] + Mh[3] * Mh[3] + Mh[4] * Mh[4]) * sigmaR * sigmaR;
                         else
                             mvAbs += Mh[2] * Mh[2] * sigmaR * sigmaR;

			                // Increment iteration count
			                iterationCount++;
            			
		            }

		            // if a mode was not associated with this data point
		            // yet associate it with yk...
		            if (mvAbs >= 0)
		            {
			            // Shift window location
			            for(int k = 0; k < lN; k++)
				            yk[k] += Mh[k];
            			
			            // update mode table for this data point
			            // indicating that a mode has been associated
			            // with it
			            modeTable[i, j] = 1;

		            }

                    for (int k = 0; k < N; k++)
                        yk[k + 2] *= sigmaR;

		            // associate the data point indexed by
		            // the point list with the mode stored
		            // by yk
		            for (int p = 0; p < pointCount; p++)
		            {
			            // obtain the point location from the
			            // point list
			            //modeCandidate_i = pointList[j];

			            // update the mode table for this point
			            //modeTable[modeCandidate_i] = 1;
                        modeTable[pointList[p] / width, pointList[p] % width] = 1;

			            //store result into msRawData...
                        for (int k = 0; k < N; k++)
                            msRawData[k, pointList[p] / width, pointList[p] % width] = (float)(yk[k + 2]);
		            }

		            //store result into msRawData...
                    for (int k = 0; k < N; k++)
                        msRawData[k, i, j] = (float)(yk[k + 2]);
                }
        }    

        /// <summary>
        /// Use the RAM to apply transitive closure to the image modes
        /// </summary>
        /// <param name="rR2">Defines square range radius used when clustering pixels together, thus defining image regions</param>
        private void TransitiveClosure(float rR2)
        {
            //Step (1):

            // Build RAM using classifiction structure originally
            // generated by the method GridTable::Connect()
            BuildRAM();

            //Step (1a):
            //Compute weights of weight graph using confidence map
            //(if defined)
            //----if (weightMapDefined) ComputeEdgeStrengths();

            //Step (2):

            //Treat each region Ri as a disjoint set:

            // - attempt to join Ri and Rj for all i != j that are neighbors and
            //   whose associated modes are a normalized distance of < 0.5 from one
            //   another

            // - the label of each region in the raList is treated as a pointer to the
            //   canonical element of that region (e.g. raList[i], initially has raList[i].label = i,
            //   namely each region is initialized to have itself as its canonical element).

            //Traverse RAM attempting to join raList[i] with its neighbors...
            int iCanEl, neighCanEl;
            float threshold;
            RAList neighbor;
            for (int i = 0; i < regionCount; i++)
            {
                //aquire first neighbor in region adjacency list pointed to
                //by raList[i]
                neighbor = raList[i].next;

                //compute edge strenght threshold using global and local
                //epsilon
                if (epsilon > raList[i].edgeStrength)
                    threshold = epsilon;
                else
                    threshold = raList[i].edgeStrength;

                //traverse region adjacency list of region i, attempting to join
                //it with regions whose mode is a normalized distance < 0.5 from
                //that of region i...
                while (neighbor != null)
                {
                    //attempt to join region and neighbor...
                    if ((InWindow(i, neighbor.label)) && (neighbor.edgeStrength < epsilon))
                    {
                        //region i and neighbor belong together so join them
                        //by:

                        // (1) find the canonical element of region i
                        iCanEl = i;
                        while (raList[iCanEl].label != iCanEl)
                            iCanEl = raList[iCanEl].label;

                        // (2) find the canonical element of neighboring region
                        neighCanEl = neighbor.label;
                        while (raList[neighCanEl].label != neighCanEl)
                            neighCanEl = raList[neighCanEl].label;

                        // if the canonical elements of are not the same then assign
                        // the canonical element having the smaller label to be the parent
                        // of the other region...
                        if (iCanEl < neighCanEl)
                            raList[neighCanEl].label = iCanEl;
                        else
                        {
                            //must replace the canonical element of previous
                            //parent as well
                            raList[raList[iCanEl].label].label = neighCanEl;

                            //re-assign canonical element
                            raList[iCanEl].label = neighCanEl;
                        }
                    }

                    //check the next neighbor...
                    neighbor = neighbor.next;

                }
            }

            // Step (3):

            // Level binary trees formed by canonical elements
            for (int i = 0; i < regionCount; i++)
            {
                iCanEl = i;
                while (raList[iCanEl].label != iCanEl)
                    iCanEl = raList[iCanEl].label;
                raList[i].label = iCanEl;
            }

            // Step (4):

            //Traverse joint sets, relabeling image.

            // (a)

            // Accumulate modes and re-compute point counts using canonical
            // elements generated by step 2.

            //allocate memory for mode and point count temporary buffers...
            float[,] modes_buffer = new float[N, regionCount];
            int[] MPC_buffer = new int[regionCount];

            //traverse raList accumulating modes and point counts
            //using canoncial element information...
            int iMPC;
            for (int i = 0; i < regionCount; i++)
            {

                //obtain canonical element of region i
                iCanEl = raList[i].label;

                //obtain mode point count of region i
                iMPC = modePointCounts[i];

                //accumulate modes_buffer[iCanEl]
                for (int k = 0; k < N; k++)
                    modes_buffer[k, iCanEl] += iMPC * modes[k, i];

                //accumulate MPC_buffer[iCanEl]
                MPC_buffer[iCanEl] += iMPC;

            }

            // (b)

            // Re-label new regions of the image using the canonical
            // element information generated by step (2)

            // Also use this information to compute the modes of the newly
            // defined regions, and to assign new region point counts in
            // a consecute manner to the modePointCounts array

            //allocate memory for label buffer
            int[] label_buffer = new int[regionCount];

            //initialize label buffer to -1
            for (int i = 0; i < regionCount; i++)
                label_buffer[i] = -1;

            //traverse raList re-labeling the regions
            int label = -1;
            for (int i = 0; i < regionCount; i++)
            {
                //obtain canonical element of region i
                iCanEl = raList[i].label;
                if (label_buffer[iCanEl] < 0)
                {
                    //assign a label to the new region indicated by canonical
                    //element of i
                    label_buffer[iCanEl] = ++label;

                    //recompute mode storing the result in modes[label]...
                    iMPC = MPC_buffer[iCanEl];
                    for (int k = 0; k < N; k++)
                        modes[k, label] = (modes_buffer[k, iCanEl]) / (iMPC);

                    //assign a corresponding mode point count for this region into
                    //the mode point counts array using the MPC buffer...
                    modePointCounts[label] = MPC_buffer[iCanEl];
                }
            }

            //re-assign region count using label counter
            int oldRegionCount = regionCount;
            regionCount = label + 1;

            // (c)

            // Use the label buffer to reconstruct the label map, which specified
            // the new image given its new regions calculated above

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    labels[i, j] = label_buffer[raList[labels[i, j]].label];
        }

        /// <summary>
        /// Build a region adjacency matrix using the region list object.
        /// </summary>
        private void BuildRAM()
        {            
            int i;
            //Allocate memory for region adjacency matrix if it hasn't already been allocated
	        if((raList = new RAList [regionCount]) == null)
                throw new Exception("Not enough memory.");

	        //initialize the region adjacency list
	        for(i = 0; i < regionCount; i++)
	        {
                raList[i] = new RAList();
		        raList[i].edgeStrength		= 0;
		        raList[i].edgePixelCount	= 0;
		        raList[i].label				= i;
            }

            //initialize RAM free list
            RAList current = new RAList();
            freeRAList = current;
            for (i = 0; i < NODE_MULTIPLE * regionCount - 1; i++)
            {
                current.edgeStrength = 0;
                current.edgePixelCount = 0;
                current.next = new RAList();
                current = current.next;
            }

	        //traverse the labeled image building the RAM by looking to the right of
	        //and below the current pixel location thus determining if a given region is adjacent to another
	        int	curLabel, rightLabel, bottomLabel;
            bool exists;
	        RAList	raNode1, raNode2, oldRAFreeList;
	        for(i = 0; i < height - 1; i++)
	        {
		        //check the right and below neighbors
		        //for pixel locations whose x < width - 1
                int j;
		        for(j= 0; j < width - 1; j++)
		        {
			        //calculate pixel labels
			        curLabel	= labels[i, j];	//current pixel
			        rightLabel	= labels[i, j+1];	//right   pixel
			        bottomLabel	= labels[i+1, j];	//bottom  pixel

			        //check to the right, if the label of
			        //the right pixel is not the same as that
			        //of the current one then region[j] and region[j+1]
			        //are adjacent to one another - update the RAM
			        if(curLabel != rightLabel)
			        {
				        //obtain RAList object from region adjacency free list
				        raNode1			= freeRAList;
				        raNode2			= freeRAList.next;

				        //keep a pointer to the old region adj. free
				        //list just in case nodes already exist in respective region lists
				        oldRAFreeList	= freeRAList;

				        //update region adjacency free list
				        freeRAList		= freeRAList.next.next;

				        //populate RAList nodes
				        raNode1.label	= curLabel;
				        raNode2.label	= rightLabel;

				        //insert nodes into the RAM
				        exists	= false;
				        raList[curLabel].Insert(raNode2);
				        exists	= raList[rightLabel].Insert(raNode1);

				        //if the node already exists then place nodes back onto the region adjacency free list
				        if(exists)
					        freeRAList = oldRAFreeList;

			        }

			        //check below, if the label of the bottom pixel is not the same as that
			        //of the current one then region[j] and region[j+width]
			        //are adjacent to one another - update the RAM
			        if(curLabel != bottomLabel)
			        {
				        //obtain RAList object from region adjacency free list
				        raNode1			= freeRAList;
				        raNode2			= freeRAList.next;

				        //keep a pointer to the old region adj. free list just in case nodes 
                        //already exist in respective region lists
				        oldRAFreeList	= freeRAList;

				        //update region adjacency free list
				        freeRAList		= freeRAList.next.next;

				        //populate RAList nodes
				        raNode1.label	= curLabel;
				        raNode2.label	= bottomLabel;

				        //insert nodes into the RAM
				        exists			= false;
				        raList[curLabel  ].Insert(raNode2);
				        exists			= raList[bottomLabel].Insert(raNode1);

				        //if the node already exists then place
				        //nodes back onto the region adjacency
				        //free list
				        if(exists)
					        freeRAList = oldRAFreeList;

			        }

		        }

		        //check only to the bottom neighbors of the right boundary
		        //pixels...

		        //calculate pixel locations (j = width-1)
		        curLabel	= labels[i, j];	//current pixel
		        bottomLabel = labels[i+1, j];	//bottom  pixel

		        //check below, if the label of
		        //the bottom pixel is not the same as that
		        //of the current one then region[j] and region[j+width]
		        //are adjacent to one another - update the RAM
		        if(curLabel != bottomLabel)
		        {
			        //obtain RAList object from region adjacency free list
			        raNode1			= freeRAList;
			        raNode2			= freeRAList.next;
        			
			        //keep a pointer to the old region adj. free
			        //list just in case nodes already exist in respective region lists
			        oldRAFreeList	= freeRAList;
        			
			        //update region adjacency free list
			        freeRAList		= freeRAList.next.next;
        			
			        //populate RAList nodes
			        raNode1.label	= curLabel;
			        raNode2.label	= bottomLabel;
        			
			        //insert nodes into the RAM
			        exists			= false;
			        raList[curLabel  ].Insert(raNode2);
			        exists			= raList[bottomLabel].Insert(raNode1);
        			
			        //if the node already exists then place
			        //nodes back onto the region adjacency
			        //free list
			        if(exists)
				        freeRAList = oldRAFreeList;

		        }
	        }

	        //check only to the right neighbors of the bottom boundary
	        //pixels...

	        //check the right for pixel locations whose x < width - 1
            for (int j = 0; j < width - 1; j++)
	        {
		        //calculate pixel labels (i = height-1)
		        curLabel = labels[i, j];	//current pixel
		        rightLabel	= labels[i, j+1];	//right   pixel
        		
		        //check to the right, if the label of the right pixel is not the same as that
		        //of the current one then region[j] and region[j+1] are adjacent to one another - update the RAM
		        if(curLabel != rightLabel)
		        {
			        //obtain RAList object from region adjacency free list
			        raNode1	= freeRAList;
			        raNode2	= freeRAList.next;

			        //keep a pointer to the old region adj. free
			        //list just in case nodes already exist in respective region lists
			        oldRAFreeList	= freeRAList;
        			
			        //update region adjacency free list
			        freeRAList		= freeRAList.next.next;
        			
			        //populate RAList nodes
			        raNode1.label	= curLabel;
			        raNode2.label	= rightLabel;
        			
			        //insert nodes into the RAM
			        exists = false;
			        raList[curLabel].Insert(raNode2);
			        exists = raList[rightLabel].Insert(raNode1);
        			
			        //if the node already exists then place nodes back onto the region adjacency free list
			        if(exists)
				        freeRAList = oldRAFreeList;
		        }
            }
        }

        /// <summary>
        /// Returns true if the two specified data points are within rR of each other.
        /// </summary>
        /// <param name="mode1">Index into msRawData specifying
        /// the modes of the pixels having these indexes.</param>
        /// <param name="mode2">Index into msRawData specifying
        /// the modes of the pixels having these indexes.</param>
        /// <returns></returns>
        private bool InWindow(int mode1, int mode2)
        {
	        double	diff	= 0, el;

            //Calculate distance squared of sub-space s	
            diff = 0;
            for (int p = 0; p < N; p++)
            {
                el = (modes[p, mode1] - modes[p, mode2]) / sigmaR; //(h[k] * offset[k]);
                if ((p == 0)/* && (k == 1)*/ && (modes[0, mode1] > 80))
                    diff += 4 * el * el;
                else
                    diff += el * el;
            }
	        return diff < 0.25;
        }

        [Parameter("Min Region", "Minimo tamaño de una región")]
        [IntegerInSequence(0, int.MaxValue)]
        public int MinRegion
        {
            get
            {
                return minRegion;
            }
            set
            {
                minRegion = value;
            }
        }

        [Parameter("Spatial Range", "Ancho de banda del espacio")]
        [IntegerInSequence(0, int.MaxValue)]
        public int SpatialBandWidth
        {
            get
            {
                return sigmaS;
            }
            set
            {
                sigmaS = value;
            }
        }

        [Parameter("Resolution Range", "Ancho de banda de resolución")]
        [RealInRange(1.0f, float.MaxValue)]
        public float ResolutionBandWidth
        {
            get
            {
                return sigmaR;
            }
            set
            {
                sigmaR = value;
            }
        }

    }

    public class RAList
    {
        public int label;

        ////////////RAM Weight/////////
        public float edgeStrength;
        public int edgePixelCount;

        ////////////RAM Link///////////
        public RAList next;

        ///////current and previous pointer/////
	    RAList	cur;

	    ////////flag///////////
	    bool exists;

        public RAList()
        {
            label = -1;

            //initialize edge strenght weight and count
            edgeStrength = 0;
            edgePixelCount = 0;
        }

        public bool Insert(RAList entry)
        {
            //if the list contains only one element
            //then insert this element into next
            if (next == null)
            {
                //insert entry
                next = entry;
                entry.next = null;

                //done
                return false;
            }

            //traverse the list until either:

            //(a) entry's label already exists - do nothing
            //(b) the list ends or the current label is
            //    greater than entry's label, thus insert the entry
            //    at this location

            //check first entry
            if (next.label > entry.label)
            {
                //insert entry into the list at this location
                entry.next = next;
                next = entry;

                //done
                return false;
            }

            //check the rest of the list...
            exists = false;
            cur = next;
            while (cur != null)
            {
                if (entry.label == cur.label)
                {
                    //node already exists
                    exists = true;
                    break;
                }
                else if ((cur.next == null) || (cur.next.label > entry.label))
                {
                    //insert entry into the list at this location
                    entry.next = cur.next;
                    cur.next = entry;
                    break;
                }

                //traverse the region adjacency list
                cur = cur.next;
            }

            //done. Return exists indicating whether or not a new node was
            //      actually inserted into the region adjacency list.
            return exists;
        }
    }
}
