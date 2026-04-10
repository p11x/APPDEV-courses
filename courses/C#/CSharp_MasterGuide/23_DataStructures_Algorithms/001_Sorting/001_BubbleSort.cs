/*
 * ============================================================
 * TOPIC     : Data Structures & Algorithms
 * SUBTOPIC  : Sorting - Bubble Sort
 * FILE      : 01_BubbleSort.cs
 * PURPOSE   : Bubble sort algorithm
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._23_DataStructures_Algorithms._01_Sorting
{
    /// <summary>
    /// Bubble sort implementation
    /// </summary>
    public class BubbleSort
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Bubble Sort ===\n");

            var arr = new int[] { 64, 34, 25, 12, 22, 11, 90 };
            Console.WriteLine($"   Original: {string.Join(", ", arr)}");
            
            BubbleSortAlgorithm(arr);
            Console.WriteLine($"   Sorted: {string.Join(", ", arr)}");

            Console.WriteLine("\n=== Bubble Sort Complete ===");
        }

        /// <summary>
        /// Bubble sort algorithm - O(n^2)
        /// </summary>
        static void BubbleSortAlgorithm(int[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                    }
                }
            }
        }
    }
}