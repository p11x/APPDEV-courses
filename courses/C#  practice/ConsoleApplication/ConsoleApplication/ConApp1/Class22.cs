using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConApp1
{
    //ASCII -- American Standard Codes for Information Interchange
    /*
     * Capital Alphabet -- 65...90
     * Small Alphabet   -- 97..122
     * Digits           -- 48...57
     * Space            -- 32
     * Tab Space        -- 9
     * \n New Line      -- 13
     */
    internal class Class22
    {
        static void Main(string[] args)
        {
            //for (int i = 0; i < 256; i++)
            //{
            //    Console.Write((char)i + "\t");
            //}

            int a = 65;
            Console.WriteLine((char)a);

            char b = 'a';
            Console.WriteLine((int)b);
        }
    }
}
