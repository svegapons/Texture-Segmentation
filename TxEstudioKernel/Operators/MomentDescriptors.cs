using System;
using TxEstudioKernel;
using TxEstudioKernel.CustomAttributes;

namespace TxEstudioKernel
{
    [MomentsDescriptor]
    [Algorithm("Simple (p+q)th moments descriptor.", "Calculates the (p+q)th order spacial moments of the given image.")]
    [Abbreviation("pq_mom", "Size", "P", "Q")]
    public class PQMomentDescriptor: TextureDescriptor
    {
        public PQMomentDescriptor() { }

        public PQMomentDescriptor(int p, int q, int size)
        {
            this.p = p;
            this.q = q;
            this.size = size;
        }

        public int size = 3, q = 1, p = 2;

        [Parameter("P", "P paremeter")]
        public int P
        {
            get { return p; }
            set { p = value; }
        }

        [Parameter("Q", "Q parameter")]
        public int Q
        {
            get { return q; }
            set { q = value; }
        }

        [Parameter("Size", "Size of convolution kernel to apply.")]
        [IntegerInSequence(1, int.MaxValue, 2)]
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        public override TxMatrix GetDescription(TxImage image)
        {
            Console.WriteLine("{0}\t{1}",p,q);
            float[,] kernel = new float[size, size];
            for (int m = -size / 2; m <= size / 2; m++)
            {
                for (int n = -size / 2; n <= size / 2; n++)
                {
                    kernel[ m+ size / 2,n + size / 2 ] = getXmp(m) * getYnq(n);
                    Console.Write("\t" + kernel[m + size / 2,n + size / 2 ].ToString());
                }
                Console.WriteLine();
            }
            //for (int n = -size / 2; n <= size / 2; n++)
            //{
            //    for (int m = -size / 2; m <= size / 2; m++)
            //    {
            //        kernel[n + size / 2, m + size / 2] = getXmp(m) * getYnq(n);
            //        Console.Write("\t" + kernel[n + size / 2, m + size / 2].ToString());
            //    }
            //    Console.WriteLine();
            //}
            Console.ReadLine();
            TxMatrix imageMatrix = TxMatrix.FromImage(image);
            return imageMatrix.Convolve(new TxMatrix(kernel));
        }

        private float getXmp(int m)
        {
            return (float)Math.Pow(m / (size / 2), p);
        }

        private float getYnq(int n)
        {
            return (float)Math.Pow(n / (size / 2), q);
        }

    }


    [MomentsDescriptor]
    [Algorithm("Bank (p+q)th of moments descriptors.", "Calculates a set of spacial moments up to the given value on the image with a window with the given size.")]
    [Abbreviation("pq_mom", "Order", "Size")]
    public class PQMomentDescriptorSequence:TextureDescriptorSequence
    {

        private int size = 3;

        [Parameter("Size", "Size of convolution kernel to apply.")]
        [IntegerInSequence(1, int.MaxValue, 2)]
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        int momentOrder;

        [Parameter("Order", "Major order momen.t")]
        public int Order
        {
            get { return momentOrder; }
            set { momentOrder = value; }
        }

        int current = 0;
        int currentp = -1;

        public override TxMatrix GetDescription(TxImage image)
        {
            return Current.GetDescription(image);
        }

        public override TextureDescriptor Current
        {
            get
            {
                return new PQMomentDescriptor(currentp, current - currentp, size);
            }
        }

        

        public override bool MoveNext()
        {
            currentp++;
            if (currentp > current)
            {
                current++;
                if (current > momentOrder)
                {
                    current --;
                    return false;
                }
                else
                    currentp = 0;
                
            }
            return true;
        }

        public override void Reset()
        {
            currentp = -1;
            current = 0;
        }
    }
}
