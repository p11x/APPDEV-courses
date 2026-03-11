using System;

namespace CSharpFundamentals.Code
{
    /// <summary>
    /// Demonstrates methods in C#
    /// </summary>
    public class MethodsDemo
    {
        // Method with no return
        public static void Greet()
        {
            Console.WriteLine("Hello!");
        }
        
        // Method with parameter
        public static void Greet(string name)
        {
            Console.WriteLine($"Hello, {name}!");
        }
        
        // Method with return value
        public static int Add(int a, int b)
        {
            return a + b;
        }
        
        // Expression-bodied method (C# 6+)
        public static int Square(int x) => x * x;
        
        // Method with out parameter
        public static void Divide(int a, int b, out int quotient, out int remainder)
        {
            quotient = a / b;
            remainder = a % b;
        }
        
        // Method with ref parameter
        public static void Increment(ref int number)
        {
            number++;
        }
        
        // Optional parameter
        public static void CreateUser(string name, int age = 18)
        {
            Console.WriteLine($"User: {name}, Age: {age}");
        }
        
        // Params array
        public static int Sum(params int[] numbers)
        {
            int total = 0;
            foreach (int n in numbers)
                total += n;
            return total;
        }
        
        public static void Main()
        {
            Greet();
            Greet("John");
            
            int result = Add(5, 3);
            Console.WriteLine($"5 + 3 = {result}");
            
            Console.WriteLine($"Square of 5 = {Square(5)}");
            
            Divide(10, 3, out int q, out int r);
            Console.WriteLine($"10 / 3 = {q} remainder {r}");
            
            int num = 5;
            Increment(ref num);
            Console.WriteLine($"After increment: {num}");
            
            CreateUser("John");
            CreateUser("Jane", 25);
            
            Console.WriteLine($"Sum: {Sum(1, 2, 3, 4, 5)}");
        }
    }
}
