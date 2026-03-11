using System;

namespace CSharpFundamentals.Code
{
    /// <summary>
    /// Demonstrates operators in C#
    /// </summary>
    public class OperatorsDemo
    {
        public static void Main()
        {
            // Arithmetic operators
            int a = 10, b = 3;
            Console.WriteLine($"a = {a}, b = {b}");
            Console.WriteLine($"a + b = {a + b}");
            Console.WriteLine($"a - b = {a - b}");
            Console.WriteLine($"a * b = {a * b}");
            Console.WriteLine($"a / b = {a / b}");
            Console.WriteLine($"a % b = {a % b}");
            
            // Increment/Decrement
            int x = 5;
            Console.WriteLine($"\nx = {x}");
            Console.WriteLine($"x++ = {x++}");  // 5, then becomes 6
            Console.WriteLine($"After x++: {x}");  // 6
            Console.WriteLine($"++x = {++x}");  // 7
            
            // Relational operators
            Console.WriteLine($"\nRelational:");
            Console.WriteLine($"a == b: {a == b}");
            Console.WriteLine($"a != b: {a != b}");
            Console.WriteLine($"a > b: {a > b}");
            Console.WriteLine($"a < b: {a < b}");
            
            // Logical operators
            bool p = true, q = false;
            Console.WriteLine($"\nLogical:");
            Console.WriteLine($"p && q: {p && q}");
            Console.WriteLine($"p || q: {p || q}");
            Console.WriteLine($"!p: {!p}");
            
            // Ternary operator
            int score = 75;
            string result = score >= 60 ? "Pass" : "Fail";
            Console.WriteLine($"\nTernary: Score {score} = {result}");
            
            // Null coalescing
            string? name = null;
            string display = name ?? "Guest";
            Console.WriteLine($"\nNull coalescing: {display}");
            
            // Compound assignment
            int num = 10;
            num += 5;
            Console.WriteLine($"\nnum += 5: {num}");
            num -= 3;
            Console.WriteLine($"num -= 3: {num}");
        }
    }
}
