using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    /*
        Strings :-
             Group of characters or Character array is called string.
             Strings are immutable i.e can not be modified
    */
    internal class Class39
    {
        static void Main(string[] args)
        {

            // Declare without initializing.
            string message1;

            // Initialize to null.
            string message2 = null;

            // Initialize as an empty string.
            // Use the Empty constant instead of the literal "".
            string message3 = String.Empty;
            message3 = "";

            // Initialize with a regular string literal.
            string oldPath = "c:\\Program Files\\Microsoft Visual Studio 8.0";

            // Initialize with a verbatim string literal.
            string newPath = @"c:\Program Files\Microsoft Visual Studio 9.0";

            // Use System.String if you prefer.
            String greeting = "Hello World!";

            // In local variables (i.e. within a method body)
            // you can use implicit typing.
            var temp = "I'm still a strongly-typed System.String!";

            // Use a const string to prevent 'message4' from
            // being used to store another string value.
            const string message4 = "You can't get rid of me!";
            //message4 = "Satehsh";     //Not allowed because of const

            // Use the String constructor only when creating
            // a string from a char*, char[], or sbyte*. 
            char[] letters = { 'A', 'B', 'C' };
            string alphabet = new string(letters);

            //converting string to character array
            string st = "Sathesh";
            char[] chars = st.ToCharArray();


            string st1 = "Sathesh";
            //st1.ToList().ForEach(x => Console.Write(x));
            Console.Write(st1[0]);
            st1 = "Kumar";
            Console.Write(st1);
        }
    }
}
