using System;
using TxEstudioKernel.CustomAttributes;


namespace TxEstudioKernel.Operators
{
    [Algorithm("Directional Operator", "Calculates the image gradient searching in a given direction using a mask of size 3x3 coefficients.")]
    [DirAbb]
    [EdgeDetector]
    public class DirectionalOperator: TxOneBand
    {

        Directions direction ;
        [Parameter("Azimut", "Direction in wich the operator will find the edges")]
        [CustomParameterEditor(typeof(TxEstudioKernel.VisualElements.DirectionEditor))]
        public Directions Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        int threshold = 0;
        [Parameter("Threshold", "Threshold to apply after the convolution")]
        [IntegerInSequence(255)]
        public int Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }



        public override TxImage Process(TxImage input)
        {
            TxImage gray = input;
            if (input.ImageFormat == TxImageFormat.RGB)
                gray = gray.ToGrayScale();
            TxImage result = gray.Convolve(GetMask(direction));
            TxEstudioKernel.OpenCV.CV.cvThreshold(result.InnerImage, result.InnerImage, (double)threshold, 255, 0);
            return result;
        }

        private TxMatrix GetMask(Directions direction)
        {
            TxMatrix result = new TxMatrix(new float[,]{{1f,1f,1f},{1f,-2f,1f},{1f,1f,1f}});
            switch (direction)
            {
                case Directions.North:
                    return new TxMatrix(new float[,] { { 1f, 1f, 1f }, { 1f, -2f, 1f }, { -1f, -1f, -1f } });
                    
                case Directions.NEast:
                    return new TxMatrix(new float[,] { { 1f, 1f, 1f }, { -1f, -2f, 1f }, { -1f, -1f, 1f } });
                    
                case Directions.East:
                    return new TxMatrix(new float[,] { { -1f, 1f, 1f }, { -1f, -2f, 1f }, { -1f, 1f, 1f } });
                    
                case Directions.SEast:
                    return new TxMatrix(new float[,] { { -1f, -1f, 1f }, { -1f, -2f, 1f }, { 1f, 1f, 1f } });
                    
                case Directions.South:
                    return new TxMatrix(new float[,] { { -1f, -1f, -1f }, { 1f, -2f, 1f }, { 1f, 1f, -1f } });
                    
                case Directions.SWest:
                    return new TxMatrix(new float[,] { { 1f, -1f, -1f }, { 1f, -2f, -1f }, { 1f, 1f, 1f } });
                    
                case Directions.West:
                    return new TxMatrix(new float[,] { { 1f, 1f, -1f }, {1f, -2f, -1f }, { 1f, 1f, -1f } });
                    
                case Directions.NWest:
                    return new TxMatrix(new float[,] { { 1f, 1f, 1f }, { 1f, -2f, -1f }, { 1f, -1f, -1f } });
                    
                default:
                    throw new ArgumentException("Unbounded direction");
                    
            }

        }
    }

    public class DirAbbAttribute : AbbreviationAttribute
    {
        public DirAbbAttribute():base("g") { }
        public override string GetAlgorithmAbbreviation(TxAlgorithm algorithm)
        {
            DirectionalOperator dirOp = algorithm as DirectionalOperator;
            return string.Format("g{0}({1})", GetDirName(dirOp.Direction), dirOp.Threshold.ToString());
        }

        private string GetDirName(Directions direction)
        {
            switch (direction)
            {
                case Directions.North:
                    return "N";
                    
                case Directions.NEast:
                    return "NE";
                    
                case Directions.East:
                    return "E";
                    
                case Directions.SEast:
                    return "SE";
                    
                case Directions.South:
                    return "S";
                    
                case Directions.SWest:
                    return "SW";
                    
                case Directions.West:
                    return "W";
                    
                case Directions.NWest:
                    return "NW";
                    
                default:
                    throw new ArgumentException("Unbounded direction");
                    
            }
        }
    }

    public enum Directions : int { North = 0, NEast = 1, East = 2, SEast = 3, South = 4, SWest = 5, West = 6, NWest = 7 }
}
