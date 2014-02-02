using System;

namespace TxEstudioKernel
{
    class Utilities
    {
        public static void writeInteger(int value, int pos, byte[] data)
        {
            int mask = 255;
            for (int i = 0; i < 4; i++)
            {
                data[pos + i] = (byte)(value & mask);
                value >>= 8;
            }
        }

        public static int readInteger(int pos, byte[] data)
        {
            int result = 0;
            for (int i = 3; i > 0; i--)
            {
                result |= data[i + pos];
                result <<= 8;
            }
            return result | data[pos];
        }
    }
}
