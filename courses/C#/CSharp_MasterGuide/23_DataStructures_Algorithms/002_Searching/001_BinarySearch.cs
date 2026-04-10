/*
 * ============================================================
 * TOPIC     : Data Structures & Algorithms
 * SUBTOPIC  : Searching - Binary Search
 * FILE      : 01_BinarySearch.cs
 * PURPOSE   : Binary search algorithm
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._23_DataStructures_Algorithms._02_Searching
{
    /// <summary>
    /// Binary search implementation
    /// </summary>
    public class BinarySearch
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Binary Search ===\n");

            var arr = new int[] { 2, 3, 4, 10, 40 };
            var target = 10;
            
            var result = BinarySearchAlgorithm(arr, target);
            Console.WriteLine($"   Found at index: {result}");

            Console.WriteLine("\n=== Binary Search Complete ===");
        }

        /// <summary>
        /// Binary search - O(log n)
        /// </summary>
        static int BinarySearchAlgorithm(int[] arr, int target)
        {
            int left = 0, right = arr.Length - 1;
            
            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                
                if (arr[mid] == target) return mid;
                if (arr[mid] < target) left = mid + 1;
                else right = mid - 1;
            }
            
            return -1;
        }
    }
}