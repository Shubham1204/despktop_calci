using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class MathLib
    {
        public double FNum { get; set; }
        public double SNum { get; set; }
        public double Result { get; set; }
        public MathLib()
        {

        }
        public MathLib(double FNum, double SNum)
        {
            this.FNum = FNum;
            this.SNum = SNum;
        }
        public void Add()
        {
            this.Result = this.FNum + this.SNum;
        }
        public void Sub()
        {
            this.Result = this.FNum - this.SNum;
        }
        public void Div()
        {
            this.Result = this.FNum / this.SNum;
        }
        public void Mul()
        {
            this.Result = this.FNum * this.SNum;
        }
        public void Mod()
        {
            this.Result = this.FNum % this.SNum;
        }
        public void Sqrt()
        {
            this.Result = System.Math.Sqrt(this.FNum);
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
    }
}
