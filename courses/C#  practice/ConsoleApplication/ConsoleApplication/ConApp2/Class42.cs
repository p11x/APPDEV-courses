using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp2
{
    //Frequency of each character
    //Deleting of duplicate charactres in a given string (unique, repeated, non repeated characters)
    //Find two strings are anagrams or not
    //Title Case String
    internal class Class42
    {
        static void Main(string[] args)
        {
            string st = "sathesh kumar";            //string data type

            char[] chars = st.ToCharArray();        //Conversion of string to char array

            List<char> chars1 = st.ToList<char>();  //Conversion of string to List<char>

            List<char> chars2 = chars.ToList();     //Convrsion of char array into List<char>

            char[] chars3 = chars1.ToArray();       //Conversion of List<char> into char array

            string st1 = chars1.ToString();         //Conversion of List<char> into string
        }
    }
}
