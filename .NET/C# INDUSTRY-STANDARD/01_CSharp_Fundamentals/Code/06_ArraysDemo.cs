using System;

namespace CSharpFundamentals.Code
{
    /// <summary>
    /// Demonstrates arrays in C#
    /// </summary>
    public class ArraysDemo
    {
        public static void Main()
        {
            // Single-dimensional array
            int[] numbers = { 1, 2, 3, 4, 5 };
            Console.WriteLine("Single-dimensional array:");
            foreach (int n in numbers)
                Console.Write(n + " ");
            
            // Array initialization
            string[] names = new string[3];
            names[0] = "Alice";
            names[1] = "Bob";
            names[2] = "Charlie";
            
            // Multidimensional array (2D)
            int[,] matrix = {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            };
            
            Console.WriteLine("\n\n2D Array:");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    Console.Write(matrix[i, j] + " ");
                Console.WriteLine();
            }
            
            // Jagged array
            int[][] jagged = new int[3][];
            jagged[0] = new int[] { 1, 2 };
            jagged[1] = new int[] { 3, 4, 5 };
            jagged[2] = new int[] { 6 };
            
            Console.WriteLine("\nJagged Array:");
            for (int i = 0; i < jagged.Length; i++)
            {
                for (int j = 0; j < jagged[i].Length; j++)
                    Console.Write(jagged[i][j] + " ");
                Console.WriteLine();
            }
            
            // Array methods
            int[] arr = { 5, 2, 8, 1, 9 };
            Console.WriteLine($"\nOriginal: {string.Join(", ", arr)}");
            
            Array.Sort(arr);
            Console.WriteLine($"Sorted: {string.Join(", ", arr)}");
            
            Console.WriteLine($"Length: {arr.Length}");
            Console.WriteLine($"Index of 5: {Array.IndexOf(arr, 5)}");
        }
    }
}
