using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class ScienceMathLib : MathLib
    {
        public void OneDivByX()
        {
            this.Result = 1 / this.FNum;
        }

        public void Sqrt()
        {
            this.Result = Math.Sqrt(FNum);
        }
    }
}
