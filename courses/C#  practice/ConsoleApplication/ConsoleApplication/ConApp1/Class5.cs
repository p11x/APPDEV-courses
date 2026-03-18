using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    //Local Variables
    internal class Class5
    {
        static void Main(string[] args)
        {
            int EmpNumber = 101;
            string EmpName = "Rajesh";

            Console.WriteLine("Employee Number :" + EmpNumber);
            Console.WriteLine("Employee Name   :" + EmpName);
            //Console.WriteLine("Employee Salary :" + EmpSalary);

            Class5 class5 = new Class5();
            class5.Show();

            if(true)
            {
                int a = 100;
            }
            //Console.WriteLine(a);
        }
        public void Show()
        {
            decimal EmpSalary = 65000;
            Console.WriteLine("Employee Salary :" + EmpSalary);
        }
    }
}
