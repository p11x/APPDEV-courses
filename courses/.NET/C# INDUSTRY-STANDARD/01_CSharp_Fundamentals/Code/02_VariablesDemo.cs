using System;

namespace CSharpFundamentals.Code
{
    /// <summary>
    /// Demonstrates variables and data types in C#
    /// </summary>
    public class VariablesDemo
    {
        public static void Main()
        {
            // Integer types
            int age = 25;
            long bigNumber = 1234567890L;
            short smallNumber = 100;
            byte byteValue = 255;
            
            // Floating point
            double temperature = 72.5;
            float height = 5.9f;
            decimal price = 19.99m;
            
            // Boolean
            bool isActive = true;
            
            // Character
            char grade = 'A';
            
            // String (reference type)
            string name = "John Doe";
            
            // Output all values
            Console.WriteLine($"Age: {age}");
            Console.WriteLine($"Big Number: {bigNumber}");
            Console.WriteLine($"Small Number: {smallNumber}");
            Console.WriteLine($"Byte Value: {byteValue}");
            Console.WriteLine($"Temperature: {temperature}");
            Console.WriteLine($"Height: {height}");
            Console.WriteLine($"Price: {price:C}");
            Console.WriteLine($"Is Active: {isActive}");
            Console.WriteLine($"Grade: {grade}");
            Console.WriteLine($"Name: {name}");
            
            // Type inference with var
            var message = "Hello";  // inferred as string
            var count = 10;        // inferred as int
            
            Console.WriteLine($"Message: {message}, Count: {count}");
            
            // Constants
            const double Pi = 3.14159;
            const string AppName = "MyApp";
            
            Console.WriteLine($"Pi: {Pi}");
            Console.WriteLine($"App: {AppName}");
        }
    }
}
