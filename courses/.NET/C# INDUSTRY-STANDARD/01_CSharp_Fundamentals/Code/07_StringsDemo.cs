using System;
using System.Text;

namespace CSharpFundamentals.Code
{
    /// <summary>
    /// Demonstrates strings in C#
    /// </summary>
    public class StringsDemo
    {
        public static void Main()
        {
            // String creation
            string s1 = "Hello";
            string s2 = new string('A', 5);
            
            // String methods
            string text = "  Hello, World!  ";
            Console.WriteLine($"Original: '{text}'");
            Console.WriteLine($"Length: {text.Length}");
            Console.WriteLine($"Trimmed: '{text.Trim()}'");
            Console.WriteLine($"Upper: '{text.ToUpper()}'");
            Console.WriteLine($"Lower: '{text.ToLower()}'");
            
            // Contains, IndexOf
            string msg = "Hello, World!";
            Console.WriteLine($"\nContains 'World': {msg.Contains("World")}");
            Console.WriteLine($"IndexOf 'World': {msg.IndexOf("World")}");
            
            // Substring
            Console.WriteLine($"Substring(0,5): {msg.Substring(0, 5)}");
            
            // Replace
            Console.WriteLine($"Replace 'World' with 'C#': {msg.Replace("World", "C#")}");
            
            // String interpolation
            string name = "John";
            int age = 30;
            string interpolated = $"Name: {name}, Age: {age}";
            Console.WriteLine($"\nInterpolated: {interpolated}");
            
            // StringBuilder for frequent modifications
            StringBuilder sb = new StringBuilder();
            sb.Append("Hello");
            sb.AppendLine(" World");
            sb.AppendFormat("Count: {0}", 42);
            Console.WriteLine($"\nStringBuilder: {sb.ToString()}");
            
            // Split
            string csv = "apple,banana,cherry";
            string[] fruits = csv.Split(',');
            Console.WriteLine("\nSplit result:");
            foreach (string fruit in fruits)
                Console.WriteLine($"  {fruit}");
        }
    }
}
