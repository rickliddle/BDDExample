using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDExample.Lib
{
    public class Calculator
    {
        public int Value { get; set; }

        public int Add(int val)
        {
            return Value += val;
        }

        public int Subtract(int val)
        {
            return Value -= val;
        }
    }
}