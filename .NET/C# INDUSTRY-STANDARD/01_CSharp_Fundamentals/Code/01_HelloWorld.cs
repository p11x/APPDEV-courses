using System;

namespace CSharpFundamentals.Code
{
    /// <summary>
    /// Demonstrates basic Hello World program in C#
    /// </summary>
    public class HelloWorld
    {
        /// <summary>
        /// Entry point of the application
        /// </summary>
        public static void Main(string[] args)
        {
            // Basic output
            Console.WriteLine("Hello, World!");
            
            // Output with string formatting
            string name = "Developer";
            Console.WriteLine("Welcome, {0}!", name);
            
            // String interpolation (C# 6+)
            Console.WriteLine($"Hello, {name}!");
            
            // Get user input
            Console.Write("Enter your name: ");
            string? input = Console.ReadLine();
            
            // Process and output
            if (!string.IsNullOrEmpty(input))
            {
                Console.WriteLine($"Nice to meet you, {input}!");
            }
            
            // Multiple arguments with interpolation
            int age = 30;
            Console.WriteLine($"Name: {name}, Age: {age}");
        }
    }
}
