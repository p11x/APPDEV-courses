using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    /*
     * static method can access only other static variables and static methods, if you want instance 
     * variables and instance methods then through an instance we can access.
     * 
     * Instance method can access instance variable, methods and static variables, methods directly with inthe class
     */
    internal class Class6
    {
        int EmpId = 101;            //Instance Variable
        static int StudId = 201;    //static variable

        public void Show()          //Instance Method
        {
            Console.WriteLine("Employee Number  :" + EmpId);
            Console.WriteLine("Student Number   :" + StudId);
            Display();
            GetData();
        }
        public void Display()       //Instance Method
        {

        }
        public static void GetData()
        {

        }
        static void Main(string[] args)     //Static Method
        {
            Console.WriteLine("Student Number  :" + StudId);
            GetData();

            Class6 class6 = new Class6();
            Console.WriteLine("Employee Number  :"+class6.EmpId);
            class6.Show();
        }
    }
}
