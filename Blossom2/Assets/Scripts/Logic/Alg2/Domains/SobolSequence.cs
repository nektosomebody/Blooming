using Alg2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alg2.Domains
{
    public class SobolSequence: IPointGenerator
    {
        private const int MAX_BITS = 30;
        private readonly int dimension;
        private readonly uint[,] direction;
        private readonly uint[] x;
        private int count;

        private readonly uint[] scramble;
        private readonly Random random = new Random();

        public SobolSequence(int dimension)
        {
            if (dimension < 1 || dimension > 16)
                throw new ArgumentException("Dimension must be between 1 and 16");

            this.dimension = dimension;
            direction = new uint[dimension, MAX_BITS];
            x = new uint[dimension];
            count = 0;

            scramble = new uint[dimension];
            for (int i = 0; i < dimension; i++)
            {
                scramble[i] = (uint)random.Next();
            }

            InitDirectionNumbers();
        }

        private void InitDirectionNumbers()
        {
            for (int i = 0; i < MAX_BITS; i++)
            {
                direction[0, i] = 1u << (31 - i);
            }
            int[] s = { 0, 1, 2, 3, 3, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5 };
            int[] a = { 0, 0, 1, 1, 2, 1, 4, 2, 4, 7, 11, 13, 14, 1, 13, 16 };

            uint[][] m = new uint[][]
            {
                new uint[] {1},
                new uint[] {1,3},
                new uint[] {1,3,5},
                new uint[] {1,1,5},
                new uint[] {1,3,3,9},
                new uint[] {1,1,5,13},
                new uint[] {1,3,5,13},
                new uint[] {1,1,5,5,17},
                new uint[] {1,3,1,15,5},
                new uint[] {1,1,5,5,5},
                new uint[] {1,3,5,5,31},
                new uint[] {1,1,7,11,19},
                new uint[] {1,3,7,13,3},
                new uint[] {1,1,5,11,7},
                new uint[] {1,3,3,9,15}
            };

            for (int dim = 1; dim < dimension; dim++)
            {
                int si = s[dim];
                int ai = a[dim];
                for (int i = 0; i < si; i++)
                {
                    direction[dim, i] = m[dim - 1][i] << 31 - i;
                }
                for (int i = si; i < MAX_BITS; i++)
                {
                    uint val = direction[dim, i - si] ^ direction[dim, i - si] >> si;

                    for (int k = 1; k < si; k++)
                    {
                        if ((ai >> si - 1 - k & 1) == 1)
                        {
                            val ^= direction[dim, i - k];
                        }
                    }

                    direction[dim, i] = val;
                }
            }
        }

        public float[] Next()
        {
            int c = 0;
            int value = count++;
            while ((value & 1) == 1)
            {
                value >>= 1;
                c++;
            }
            float[] result = new float[dimension];
            for (int i = 0; i < dimension; i++)
            {
                x[i] ^= direction[i, c];
                uint scrambled = x[i] ^ scramble[i];
                result[i] = scrambled / (float)uint.MaxValue;
            }
            return result;
        }
    }
}
