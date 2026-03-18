using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
     * Arrays :-
     * An array is collection of elements with same data type and same name but with different index values (subscript values)
     * Syntax :-
     *          <data_type> [] <array_name> = new <data_type>[size];
     * Ex :-
     *          int [] a = new int[5];
     *          a[0], a[1], a[2], a[3], a[4]
     * 
     * Array :- It is a predefined class in the System namespace, It provides methods for creating, manipulating, searchig & sorting
     * of array elements.
     *
     * Properties :-
     * Length :- It gives total number of elements in all dimensions of an array.
     * 
     * Methods :-
     * Sort :- It sorts all given single dimension array elements in ascending order.
     * Reverse :- It reverses all given single dimension array elements.
     * IndexOf :- It searches for an element and returns position of that element if it is available or it returns -1
     */
    internal class Class1
    {
        static void Main(string[] args)
        {
            int[] a = new int[5] { 10, 20, 30, 40, 50 };

            Console.WriteLine("The Array Elements are.....");
            for (int i = 0; i < a.Length; i++)
            {
                //Console.WriteLine("a[" + i + "]=" + a[i]);
                Console.WriteLine($"a[{i}]={a[i]}");
            }

        }
    }
}
