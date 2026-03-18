using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Multiple Inheritance with the help of interfaces
    interface IInter1
    {
        void One();
    }
    interface IInter2
    {
        void Two();
        void One();
    }
    abstract class Sample5
    {
        public abstract void Three();
    }
    class Sample6 : Sample5, IInter1, IInter2
    {

        public void Two()
        {
            Console.WriteLine("This is Method Two");
        }
        public override void Three()
        {
            Console.WriteLine("This is Method Three");
        }

        void IInter1.One()          //explicit implementation
        {
            Console.WriteLine("This is Method One from IInter1");
        }
        void IInter2.One()
        {
            Console.WriteLine("This is Method One from Iinter2");
        }
    }
    internal class Class16
    {
        static void Main(string[] args)
        {
            IInter1 inter1;
            inter1=new Sample6();
            inter1.One();

            IInter2 inter2;
            inter2=new Sample6();
            inter2.One();

            Sample6 sample6 = new Sample6();
            sample6.Two();
            sample6.Three();
        }
    }
}
