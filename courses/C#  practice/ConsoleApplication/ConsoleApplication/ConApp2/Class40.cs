using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //String Methods
    internal class Class40
    {
        static void Main(string[] args)
        {
            string FirstName = "Sathesh";
            string LastName = "Kumar";
            string Address = "KPHB, Hyderabad";

            string st = "My Name is " + FirstName + " " + LastName + ", I am from " + Address;
            Console.WriteLine(st);

            //String Interpolation
            string st1 = $"My Name is {FirstName} {LastName}, I am from {Address}";
            Console.WriteLine(st1);

            //Split, Trim, Substring, Replace...
            string src = "My Name is Sathesh";

            //split
            string[] strArray = src.Split(" ");
            Console.WriteLine("String Array Elements are....");
            foreach (string str in strArray)
            {
                Console.WriteLine(str);
            }

            //Join
            string dst = String.Join(" ", strArray);
            Console.WriteLine(dst);

            //trim
            string str1 = "   Sathesh    ";
            str1 = str1.Trim();
            Console.WriteLine(str1.Length);

            //Substring    
            string str2 = "Sathesh";
            string str3 = str2.Substring(2, 4);
            Console.WriteLine(str3);

            //Concat
            string fName = "Sathesh ";
            string lName = "Kumar";
            string fullName = string.Concat(fName, lName);
            Console.WriteLine(fullName);

            //replace
            string str4 = "SATHESH";
            str4 = str4.Replace('T', 'Z');
            Console.WriteLine(str4);

            //Upper Case
            string str5 = "sathesh";
            Console.WriteLine(str5.ToUpper());

            //Lower Case
            string str6 = "SATHESH";
            Console.WriteLine(str6.ToLower());

            //ToCharArray
            string str7 = "SATHESH";
            char[] charArray = str7.ToCharArray();
            Console.WriteLine("Character Array Elements are....");
            foreach (char c in charArray)
            {
                Console.WriteLine(c);
            }

            //Srring reverse with using function
            charArray = str7.Reverse().ToArray();
            Console.WriteLine("Character Array Elements are....");
            foreach (char c in charArray)
            {
                Console.WriteLine(c);
            }
        }
    }
}
