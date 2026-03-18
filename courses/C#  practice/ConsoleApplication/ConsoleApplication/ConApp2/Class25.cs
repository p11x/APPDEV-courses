using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
        constant & readonly
        const :- It can not be changed and should be initialised while defining
        readonly :- It can not be changed but it can be initialised only in the constructor
    */
    internal class Class25
    {
        const int a = 100;
        readonly int b;

        public Class25()
        {
            b = 300;
        }
        public void Show()
        {
            //a = 100;
            //b = 200;
        }
        static void Main(string[] args)
        {
            
        }
    }
}
