using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alg2.Interfaces;

namespace Alg2.Domains
{
    public class RandomIntGenerator : IFlowAmountGenerator
    {
        private Random gen;
        private int length;
        private int left;
        private int right;
        
        public RandomIntGenerator(int bLeft, int bRight)
        {
            gen = new Random();
            length = Math.Abs(bRight - bLeft) + 1;
            left = bLeft;
            right = bRight;
        }
        public int Next()
        {
            return left + gen.Next() % length;           
        }
    }
}
