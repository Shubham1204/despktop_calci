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
            this.Result = Math.Sqrt(this.FNum);
        }
        public void Sqr()
        {
            this.Result = System.Math.Pow(this.FNum, 2);
        }
        public void Power()
        {
            this.Result = System.Math.Pow(this.FNum, this.SNum);
        }
        public void Abs()
        {
            this.Result = System.Math.Abs(FNum);
        }
        public void Pi()
        {
            this.Result = System.Math.PI;
        }
        public void Fact()
        {
            double fact = 1;
            for (int i = 1; i <= this.FNum; i++)
                fact *= i;
            this.Result = fact;
        }
        public void Log()
        {
            this.Result = System.Math.Log10(FNum);
        }
        public void Ln()
        {
            this.Result = System.Math.Log(FNum);
        }
        public void Sine()
        {
            this.Result = System.Math.Sinh(FNum);
        }
        public void Cosine()
        {
            this.Result = System.Math.Cosh(FNum);
        }
        public void Tan()
        {
            this.Result = System.Math.Tanh(FNum);
        }
    }
}
