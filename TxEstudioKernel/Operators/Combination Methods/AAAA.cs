using System;
using System.Collections.Generic;
using System.Text;
using TxEstudioKernel.OpenCV;
using TxEstudioKernel.CustomAttributes;
using ActiveContour;
using TxEstudioKernel.Operators.ConnectedComponents;
using System.Drawing;

namespace TxEstudioKernel.Operators
{
    //[Algorithm("Combinated AC-Border", "This method integrates both approaches, based on regions and borders")]
    //[Abbreviation("comb_ac_border", "Miu", "CCSize", "MaxIteration")]
    public class AC_BiClass_BorderRegion : TxSegmentationAlgorithm
    {
        public AC_BiClass_BorderRegion()
        {
            lengthValues.Add("000", 0f);
            lengthValues.Add("001", 1f);
            lengthValues.Add("010", 1f);
            lengthValues.Add("011", 1.4142135623f);
            lengthValues.Add("100", 1.4142135623f);
            lengthValues.Add("101", 1f);
            lengthValues.Add("110", 1f);
            lengthValues.Add("111", 0f);
        }

        #region Vars

        float miu = 1;
        int ccSize = 300;
        BiClassAC_OneBand[] bands;
        int[,] phi;
        float maxIter = 50;
        int width, height;
        NewFilterCC filter = new NewFilterCC();
        Dictionary<string, float> lengthValues = new Dictionary<string, float>();


        #endregion

        #region Properties

        [Parameter("Miu", "peso para calcular el length(C) ")]
        [RealInRange(0.0f, 1.0f)]
        public float Miu
        {
            get
            {
                return this.miu;
            }
            set
            {
                miu = value;
            }
        }

        [Parameter("CCSize", "Tamaño de la menor componente conexa")]
        public int CCSize
        {
            get
            {
                return this.ccSize;
            }
            set
            {
                ccSize = value;
            }
        }

        [Parameter("maxIterations", "Numero maximo de iteraciones del algoritmo")]
        public float MaxIteration
        {
            get { return maxIter; }
            set { maxIter = value; }
        }

        public BorderMatrix BorderMatrix
        {
            get { return border; }
            set { border = value; }
        }


        #endregion
        //metodo heredado de Iclasificable.
        public override double ProbError()
        {
            throw new Exception("Este segmentador no tiene implementado una funcion para la estimación no supervisada del error de clasificacion");
        }

        public override TxSegmentationResult Segment(TxMatrix[] descriptors)
        {
            width = descriptors[0].Width;
            height = descriptors[0].Height;
            TxSegmentationResult res = new TxSegmentationResult(2, width, height);
            phi = new int[height, width];
            GetInitialPartition();
            filter.MinSize = this.ccSize;

            bands = new BiClassAC_OneBand[descriptors.Length];
            for (int i = 0; i < bands.Length; i++)
            {
                bands[i] = new BiClassAC_OneBand();
                bands[i].Init(descriptors[i], phi, miu);
            }

            int iter = 0;

            int change = Sweep(false);
            while (change > 10 && iter < maxIter)
            {
                iter++;
                GetC1C2();
                change = Sweep(false);
            }

            iter = 0;

            change = Sweep(true);
            while (change > 10 && iter < maxIter)
            {
                iter++;
                if (iter % 5 == 1 && ccSize != 0)
                    phi = filter.Filter(phi);
                GetC1C2();
                change = Sweep(true);
            }
            phi = filter.Filter(phi);

            for (int i = 0; i < res.Height; i++)
                for (int j = 0; j < res.Width; j++)
                {
                    if (phi[i, j] < 0)
                        res[i, j] = 0;
                    else
                        res[i, j] = 1;
                }
            return res;

        }

        private int Sweep(bool compLength)
        {
            int change = 0;
            float energy = 0.0f;

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    energy = 0.0f;

                    if (compLength)
                    {
                       // energy += GetDeltaLength(i, j) * miu;


                        //De monica
                        energy += (float)ComputeDistance(new Point(i,j));
                    }


                    for (int k = 0; k < bands.Length; k++)
                        energy += bands[k].GetDeltaEnergy(i, j);

                    if (energy < 0)
                    {
                        phi[i, j] *= -1;
                        change++;
                        if(compLength)
                        Update(new Point(i, j));
                    }

                }
            return change;
        }

        protected void GetC1C2()
        {
            for (int i = 0; i < bands.Length; i++)
            {
                bands[i].GetC1C2(phi);
            }
        }

        protected void GetInitialPartition()
        {
            for (int i = 0; i < phi.GetLength(0); i++)
                for (int j = 0; j < phi.GetLength(1); j++)
                {
                    if (j < phi.GetLength(1) / 3)
                        phi[i, j] = 1;
                    else
                        phi[i, j] = -1;
                }

            phiBorder = FillBorders(phi);
            Distance(border, phiBorder);

        }

        protected float GetDeltaLength(int row, int col)
        {
            float b = ComputeLength(row, col);
            if (row > 0)
                b += ComputeLength(row - 1, col);
            if (col > 0)
                b += ComputeLength(row, col - 1);

            phi[row, col] *= -1;

            float b1 = ComputeLength(row, col);
            if (row > 0)
                b1 += ComputeLength(row - 1, col);
            if (col > 0)
                b1 += ComputeLength(row, col - 1);

            phi[row, col] *= -1;

            return b1 - b;

        }

        protected float ComputeLength(int row, int col)
        {
            if (row == height - 1 || col == width - 1)
                return Math.Max(0f, phi[row, col]);
            string cod = "";
            if (phi[row, col] >= 0)
                cod += "1";
            else
                cod += "0";
            if (phi[row + 1, col] >= 0)
                cod += "1";
            else
                cod += "0";
            if (phi[row, col + 1] >= 0)
                cod += "1";
            else
                cod += "0";

            return lengthValues[cod];
        }


        #region Border

        int actualDistance;
        BorderMatrix border;
        BorderMatrix phiBorder;
        int[,] pointNeighbours;

        /// <summary>
        /// Calcula la distancia inicialmente....
        /// </summary>
        /// <param name="border"></param>Borde
        /// <param name="phiBorder"></param>Borde actual, borde que se esta calculando
        /// SI devuelve int.MaxValue es porque uno de los bordes es null que es un error
        public void Distance(BorderMatrix border, BorderMatrix phiBorder)
        {
            actualDistance = 0;
            int distance = int.MaxValue;
            if (border != null && phiBorder != null)
            {
                distance = 0;
                for (int i = 0; i < phiBorder.MatrixB.GetLength(1); i++)
                    for (int j = 0; j < phiBorder.MatrixB.GetLength(0); j++)
                        distance += Math.Abs(border[j, i] - phiBorder[j, i]);
            }
            actualDistance = distance;
        }

        /// <summary>
        /// Distancia al cambiar el punto point de la clase en la que esta para la otra.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public double ComputeDistance(Point point)
        {
            pointNeighbours = Neighbourhood(point, phiBorder);
            List<Point> neighbours = Neighbours(phi, point);
            int distWithChange = this.actualDistance;

            //Cambiar de 1->0 
            if (phi[point.X, point.Y] == 1)
            {
                if (phiBorder[point.X, point.Y] == 1)
                //si es borde, los vecinos 0 no se afectan, pero los 1 que no sean bordes se vuelven bordes
                {
                    for (int i = 0; i < neighbours.Count; i++)
                    {
                        if (phi[neighbours[i].X, neighbours[i].Y] == 1 && phiBorder[neighbours[i].X, neighbours[i].Y] == 0)
                        { // los vecinos 1 pero que no son bordes se vuelven bordes

                            int xN = point.X - neighbours[i].X;
                            int yN = point.Y - neighbours[i].Y;

                            if (xN == 1 && yN == 0)
                            {
                                pointNeighbours[0, 1] = 1;
                            }
                            else if (xN == 0 && yN == -1)
                            {
                                pointNeighbours[1, 2] = 1;
                            }
                            else if (xN == -1 && yN == 0)
                            {
                                pointNeighbours[2, 1] = 1;
                            }
                            else if (xN == 0 && yN == 1)
                            {
                                pointNeighbours[1, 0] = 1;
                            }
                            else throw new Exception("Algo esta mal en los vecinos (Update phi)");
                        }
                    }


                    // Para calcular la distancia se suma el nuevo cambio y se resta lo anterior que habia
                    if (point.X - 1 >= 0 && point.Y >= 0)
                    {
                        distWithChange += Math.Abs(pointNeighbours[0, 1] - border[point.X - 1, point.Y]);
                        distWithChange -= Math.Abs(phiBorder[point.X - 1, point.Y] - border[point.X - 1, point.Y]);
                    }

                    if (point.X >= 0 && point.Y - 1 >= 0)
                    {
                        distWithChange += Math.Abs(pointNeighbours[1, 0] - border[point.X, point.Y - 1]);
                        distWithChange -= Math.Abs(phiBorder[point.X, point.Y - 1] - border[point.X, point.Y - 1]);
                    }

                    if (point.X + 1 < border.MatrixB.GetLength(0) && point.Y >= 0)
                    {
                        distWithChange += Math.Abs(pointNeighbours[2, 1] - border[point.X + 1, point.Y]);
                        distWithChange -= Math.Abs(phiBorder[point.X + 1, point.Y] - border[point.X + 1, point.Y]);
                    }

                    if (point.X >= 0 && point.Y + 1 < border.MatrixB.GetLength(1))
                    {
                        distWithChange += Math.Abs(pointNeighbours[1, 2] - border[point.X, point.Y + 1]);
                        distWithChange -= Math.Abs(phiBorder[point.X, point.Y + 1] - border[point.X, point.Y + 1]);
                    }

                    //Esto es porque yo deje de ser borde 
                    //pointNeighbours[1, 1] = 0;

                    distWithChange += Math.Abs(pointNeighbours[1, 1] - border[point.X, point.Y]);
                    distWithChange -= Math.Abs(phiBorder[point.X, point.Y] - border[point.X, point.Y]);
                }
                else
                {
                    pointNeighbours[0, 1] = 1;
                    pointNeighbours[1, 2] = 1;
                    pointNeighbours[2, 1] = 1;
                    pointNeighbours[1, 0] = 1;

                    // Para calcular la distancia se suma el nuevo cambio y se resta lo anterior que habia
                    if (point.X - 1 >= 0 && point.Y >= 0)
                    {
                        distWithChange += Math.Abs(pointNeighbours[0, 1] - border[point.X - 1, point.Y]);
                        distWithChange -= Math.Abs(phiBorder[point.X - 1, point.Y] - border[point.X - 1, point.Y]);
                    }

                    if (point.X >= 0 && point.Y - 1 >= 0)
                    {
                        distWithChange += Math.Abs(pointNeighbours[1, 0] - border[point.X, point.Y - 1]);
                        distWithChange -= Math.Abs(phiBorder[point.X, point.Y - 1] - border[point.X, point.Y - 1]);
                    }

                    if (point.X + 1 < border.MatrixB.GetLength(0) && point.Y >= 0)
                    {
                        distWithChange += Math.Abs(pointNeighbours[2, 1] - border[point.X + 1, point.Y]);
                        distWithChange -= Math.Abs(phiBorder[point.X + 1, point.Y] - border[point.X + 1, point.Y]);
                    }

                    if (point.X >= 0 && point.Y + 1 < border.MatrixB.GetLength(1))
                    {
                        distWithChange += Math.Abs(pointNeighbours[1, 2] - border[point.X, point.Y + 1]);
                        distWithChange -= Math.Abs(phiBorder[point.X, point.Y + 1] - border[point.X, point.Y + 1]);
                    }
                }
                pointNeighbours[1, 1] = 0;
            }
            //Cambiar de 0->1
            else
            {
                List<Point> NoBorders = new List<Point>();
                pointNeighbours[1, 1] = 0;
                for (int i = 0; i < neighbours.Count; i++)
                {
                    // Algun vecino mio es cero por tanto yo soy borde
                    if (phi[neighbours[i].X, neighbours[i].Y] == -1)
                    {
                        pointNeighbours[1, 1] = 1;

                    }
                    else
                    {
                        //Calculo si mis vecinos 1 que tenian que ser borde porque yo era cero 
                        // eran borde solo por mi (dejan de ser bordes) o no (siguen siendo bordes)

                        List<Point> aux = Neighbours(phi, new Point(neighbours[i].X, neighbours[i].Y));

                        bool anotherZero = false;

                        for (int j = 0; j < aux.Count; j++)
                        {

                            // sigue siendo borde , encontre un vecino suyo != de mi que es 0 (ahora -1)
                            if (aux[j] != point && phi[aux[j].X, aux[j].Y] == -1)
                            {
                                anotherZero = true;
                                break;
                            }
                        }
                        if (!anotherZero)
                        {

                            // NoBorders.Add(new Point(neighbours[i].X, neighbours[i].Y));
                            //Mirar que esto funcione.

                            pointNeighbours[neighbours[i].X - point.X + 1, neighbours[i].Y - point.Y + 1] = 0;
                            // aqui yo restaba 0 - border[neighbours[i].X, neighbours[i].Y] pero ahora debe ser -1

                            distWithChange += Math.Abs(-1 - border[neighbours[i].X, neighbours[i].Y]);
                            distWithChange -= Math.Abs(phiBorder[neighbours[i].X, neighbours[i].Y] - border[neighbours[i].X, neighbours[i].Y]);
                        }

                    }

                }

                distWithChange += Math.Abs(pointNeighbours[1, 1] - border[point.X, point.Y]);
                distWithChange -= Math.Abs(phiBorder[point.X, point.Y] - border[point.X, point.Y]);
            }
            return distWithChange - actualDistance;

        }

        /// <summary>
        /// Actualiza todo lo que hay q actualizar, 
        ///  Esto no se haceeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee   poniendo a phi[point]=value
        /// </summary>
        /// <param name="point"></param>
        /// <param name="value"></param>
        
        public void Update(Point point)
        {
            for (int i = 0; i < pointNeighbours.GetLength(0); i++)
            {
                for (int j = 0; j < pointNeighbours.GetLength(1); j++)
                {
                    if (point.X - 1 + i >= 0 && point.Y - 1 + j >= 0 && point.X - 1 + i < pointNeighbours.GetLength(0) && point.Y - 1 + j < pointNeighbours.GetLength(1))
                        phiBorder[point.X - 1 + i, point.Y - 1 + j] = pointNeighbours[i, j];
                }
            }
        }

        /// <summary>
        /// Devulelve la vecindad de un punto en el phiBorder en un arreglo de 3*3 cdo esta en un limite del arreglo el punto 
        /// se pone -1 en esa posicion 
        /// es como un arreglo de configuracion del punto y su vecindad despues del cambio en la matriz de bordes
        /// </summary>
        /// <param name="point"></param>
        /// <param name="phiBorder"></param>
        /// <returns></returns>
        
        public int[,] Neighbourhood(Point point, BorderMatrix phiBorder)
        {
            int[,] result = new int[3, 3];

            if (point.X - 1 >= 0 && point.Y >= 0)
            {
                result[0, 1] = phiBorder[point.X, point.Y];
            }
            else
                result[0, 1] = -1;

            if (point.X >= 0 && point.Y - 1 >= 0)
            {
                result[1, 0] = phiBorder[point.X, point.Y - 1];
            }
            else
                result[1, 0] = -1;

            if (point.X + 1 < phiBorder.MatrixB.GetLength(0) && point.Y >= 0)
            {
                result[2, 1] = phiBorder[point.X + 1, point.Y];
            }
            else
                result[2, 1] = -1;

            if (point.X >= 0 && point.Y + 1 < phiBorder.MatrixB.GetLength(1))
            {
                result[1, 2] = phiBorder[point.X, point.Y + 1];
            }
            else
                result[1, 2] = -1;

            return result;
        }

        /// <summary>
        /// Dado el arreglo phi inicial se le calcula el borde
        /// </summary>
        /// <param name="phi"></param>
        /// <returns></returns> Si retorna null es porque phi es null lo que es un error
        

        //TODO: phi como es?????
        public BorderMatrix FillBorders(int[,] phi)
        {
            if (phi != null)
            {
                BorderMatrix result = new BorderMatrix(phi.GetLength(0), phi.GetLength(1));

                for (int i = 0; i < phi.GetLength(1); i++)
                    for (int j = 0; j < phi.GetLength(0) - 1; j++)
                        if (phi[j, i] == 1)
                            if (phi[j + 1, i] == -1)
                                result[j, i] = 1;
                            else continue;
                        else  //si es 0
                            if (phi[j + 1, i] == 1)
                                result[j + 1, i] = 1;
                for (int i = 0; i < phi.GetLength(0); i++)
                    for (int j = 0; j < phi.GetLength(1) - 1; j++)
                        if (phi[i, j] == 1)
                            if (phi[i, j + 1] == -1)
                                result[i, j] = 1;
                            else continue;
                        else //si es 0
                            if (phi[i, j + 1] == 1)
                                result[i, j + 1] = 1;
                return result;               
            }
            return null;
        }

        /// <summary>
        /// Dado un elemento devuelve sus vecinos en una lista de puntos
        /// </summary>
        /// <param name="phi"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        
        public List<Point> Neighbours(int[,] phi, Point point)
        {
            List<Point> neighbours = new List<Point>();
            if (point.X == 0 && point.Y == 0)
            {
                if (((point.X) + 1) < phi.GetLength(0))
                    neighbours.Add(new Point((point.X) + 1, point.Y));
                if (((point.Y) + 1) < phi.GetLength(1))
                    neighbours.Add(new Point(point.X, (point.Y) + 1));
            }
            else if (point.X == phi.GetLength(0) - 1 && point.Y == 0)
            {
                if (((point.X) - 1) >= 0)
                    neighbours.Add(new Point((point.X) - 1, point.Y));
                if (((point.Y) + 1) < phi.GetLength(1))
                    neighbours.Add(new Point(point.X, (point.Y) + 1));
            }
            else if (point.X == 0 && point.Y == phi.GetLength(1) - 1)
            {
                if (((point.Y) - 1) >= 0)
                    neighbours.Add(new Point(point.X, (point.Y) - 1));
                if (((point.X) + 1) < phi.GetLength(0))
                    neighbours.Add(new Point((point.X) + 1, point.Y));
            }
            else if (point.X == phi.GetLength(0) - 1 && point.Y == phi.GetLength(1) - 1)
            {
                if (((point.Y) - 1) >= 0)
                    neighbours.Add(new Point(point.X, (point.Y) - 1));
                if (((point.X) - 1) >= 0)
                    neighbours.Add(new Point((point.X) - 1, point.Y));
            }
            else if (point.X == phi.GetLength(0) - 1)
            {
                if (((point.Y) - 1) >= 0)
                    neighbours.Add(new Point(point.X, (point.Y) - 1));
                if (((point.Y) + 1) < phi.GetLength(1))
                    neighbours.Add(new Point(point.X, (point.Y) + 1));
                if (((point.X) - 1) >= 0)
                    neighbours.Add(new Point((point.X) - 1, point.Y));
            }
            else if (point.X == 0)
            {
                if (((point.Y) - 1) >= 0)
                    neighbours.Add(new Point(point.X, (point.Y) - 1));
                if (((point.X) + 1) < phi.GetLength(0))
                    neighbours.Add(new Point((point.X) + 1, point.Y));
                if (((point.Y) + 1) < phi.GetLength(1))
                    neighbours.Add(new Point(point.X, (point.Y) + 1));

            }
            else if (point.Y == phi.GetLength(1) - 1)
            {
                if (((point.Y) - 1) >= 0)
                    neighbours.Add(new Point(point.X, (point.Y) - 1));
                if (((point.X) + 1) < phi.GetLength(0))
                    neighbours.Add(new Point((point.X) + 1, point.Y));
                if (((point.X) - 1) >= 0)
                    neighbours.Add(new Point((point.X) - 1, point.Y));
            }
            else if (point.Y == 0)
            {
                if (((point.X) + 1) < phi.GetLength(0))
                    neighbours.Add(new Point((point.X) + 1, point.Y));
                if (((point.X) - 1) >= 0)
                    neighbours.Add(new Point((point.X) - 1, point.Y));
                if (((point.Y) + 1) < phi.GetLength(1))
                    neighbours.Add(new Point(point.X, (point.Y) + 1));
            }
            else
            {
                if (((point.Y) - 1) >= 0)
                    neighbours.Add(new Point(point.X, (point.Y) - 1));
                if (((point.X) + 1) < phi.GetLength(0))
                    neighbours.Add(new Point((point.X) + 1, point.Y));
                if (((point.X) - 1) >= 0)
                    neighbours.Add(new Point((point.X) - 1, point.Y));
                if (((point.Y) + 1) < phi.GetLength(1))
                    neighbours.Add(new Point(point.X, (point.Y) + 1));
            }
            return neighbours;
        }


        #endregion

    }

    
    public class BorderMatrix
    {
        int[,] tab;

        public BorderMatrix(int weight, int height)
        {
            tab = new int[weight, height];
        }
        public int[,] MatrixB
        {
            get
            {
                return tab;
            }
        }

        public int this[int x, int y]
        {
            get
            {
                return tab[x, y];
            }
            set
            {
                tab[x, y] = value;
            }
        }


        public static  BorderMatrix FromImage(TxImage txImage)
        {
            TxImage borderInput = txImage.ToGrayScale();
            BorderMatrix result = new BorderMatrix(txImage.Height, txImage.Width );
            unsafe
            {
                byte* src = (byte*)((_IplImage*)(txImage.InnerImage))->imageData;
                int srcOffset = ((_IplImage*)(txImage.InnerImage))->widthStep - ((_IplImage*)(txImage.InnerImage))->width;

                for (int i = 0; i < txImage.Height; i++, src += srcOffset)
                    for (int j = 0; j < txImage.Width; j++, src++)
                        //*src es el valor del pixel en i, j
                        result.tab[i, j] = ( *src >= 200)?1:0;
            }
            return result;
        }
    }


}
