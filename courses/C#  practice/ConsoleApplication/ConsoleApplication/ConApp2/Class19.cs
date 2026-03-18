using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConApp2
{
    /*
       * Exception Handling :-
       * try :-
       * It is the suspecious code or where an error can occur.
       * catch :-
       * It is an exception detector and catch block contains the code to be executed whenever
       * that particular exception occurs. We can have the multiple catch blocks also.
       * finally :-
       * It executes always if the execution leaves any part of the try block.
       * throw :-
       * It is used to throw an exception object explicitly.
    */

    class MyException : Exception
    {
        string msg;
        public MyException()
        {
            msg = "Both are equal";
        }
        public MyException(string m)
        {
            msg = m;
        }
        public override string Message
        {
            get { return msg; }
        }
    }

    class Test
    {
        int a, b, c;
        public void GetData()
        {
            Console.Write("Enter a value :");
            a = int.Parse(Console.ReadLine());
            Console.Write("Enter b value :");
            b = int.Parse(Console.ReadLine());
        }
        public void Calculation()
        {
            if (a == b)
                throw new MyException();
            else if (b > a)
                throw new MyException("Denominator is Big");
            else
                c = a / b;
        }
        public void Show()
        {
            Console.WriteLine("a value is :" + a);
            Console.WriteLine("b value is :" + b);
            Console.WriteLine("c value is :" + c);
        }
    }
    internal class Class19
    {
        static void Main(string[] args)
        {
            Test test = new Test();
            try
            {

                try
                {
                    int[] x = new int[5];
                    x[8] = 100;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                test.GetData();
                test.Calculation();
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (OverflowException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (MyException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                test.Show();
            }
            Console.WriteLine("End of Program reached");
        }
    }
}
