using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //String Reverse with out using a function
    internal class Class41
    {
        static void Main(string[] args)
        {
            string st = "My Name is Sathesh";
            //output="yM emaN si hsehtaS";
           
            string[] starray= st.Split(" ");

            for (int i = 0; i < starray.Length; i++)
            {
                starray[i]=StrRev(starray[i]);
            }

            string res=String.Join(" ", starray);

            Console.WriteLine(res);
            
          
        }
        public static string StrRev(string st)
        {
            string rest = String.Empty;
            for (int i = st.Length - 1; i >= 0; i--)
            {
                rest += st[i];
            }
            return rest;
        }
    }
}
