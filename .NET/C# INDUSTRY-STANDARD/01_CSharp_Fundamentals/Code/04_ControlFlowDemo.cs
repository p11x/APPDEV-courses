using System;

namespace CSharpFundamentals.Code
{
    /// <summary>
    /// Demonstrates control flow statements in C#
    /// </summary>
    public class ControlFlowDemo
    {
        public static void Main()
        {
            // If-else statement
            int score = 85;
            if (score >= 90)
                Console.WriteLine("Grade: A");
            else if (score >= 80)
                Console.WriteLine("Grade: B");
            else if (score >= 70)
                Console.WriteLine("Grade: C");
            else
                Console.WriteLine("Grade: F");
            
            // Switch statement (traditional)
            int day = 3;
            string dayName;
            switch (day)
            {
                case 1: dayName = "Monday"; break;
                case 2: dayName = "Tuesday"; break;
                case 3: dayName = "Wednesday"; break;
                default: dayName = "Unknown"; break;
            }
            Console.WriteLine($"Day: {dayName}");
            
            // Switch expression (C# 8+)
            string dayType = day switch
            {
                1 or 2 or 3 or 4 or 5 => "Weekday",
                6 or 7 => "Weekend",
                _ => "Invalid"
            };
            Console.WriteLine($"Day Type: {dayType}");
            
            // For loop
            Console.WriteLine("\nFor loop:");
            for (int i = 1; i <= 5; i++)
            {
                Console.Write(i + " ");
            }
            
            // Foreach loop
            Console.WriteLine("\n\nForeach loop:");
            string[] fruits = { "Apple", "Banana", "Cherry" };
            foreach (string fruit in fruits)
            {
                Console.WriteLine(fruit);
            }
            
            // While loop
            Console.WriteLine("\nWhile loop:");
            int count = 0;
            while (count < 3)
            {
                Console.WriteLine($"Count: {count}");
                count++;
            }
            
            // Do-while loop
            Console.WriteLine("\nDo-while loop:");
            int n = 0;
            do
            {
                Console.WriteLine(n);
                n++;
            } while (n < 3);
        }
    }
}
