using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Muliti Level Inheritance
    class Sample2
    {
        int a;
        public Sample2(int x)
        {
            a = x;
        }
        public void Show()
        {
            Console.WriteLine("a value is :" + a);
        }
    }
    class Sample3 : Sample2
    {
        int b;
        public Sample3(int y) : base(10)
        {
            b = y;
        }
        public new void Show()
        {
            base.Show();
            Console.WriteLine("b value is :" + b);
        }
    }
    class Sample4 : Sample3
    {
        int c;
        public Sample4(int z) : base(20)
        {
            c = z;
        }
        public new void Show()
        {
            base.Show();
            Console.WriteLine("c value is :" + c);
        }
    }
    internal class Class12
    {
        static void Main(string[] args)
        {
            Sample4 sample4 = new Sample4(30);
            sample4.Show();
        }
    }
}
