/*
 * ============================================================
 * TOPIC     : Data Structures & Algorithms
 * SUBTOPIC  : Sorting - Insertion Sort
 * FILE      : 03_InsertionSort.cs
 * PURPOSE   : Insertion sort algorithm
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._23_DataStructures_Algorithms._01_Sorting
{
    /// <summary>
    /// Insertion sort
    /// </summary>
    public class InsertionSort
    {
        public static void Main(string[] args)
        {
            var arr = new int[] { 12, 11, 13, 5, 6 };
            Console.WriteLine($"   Original: {string.Join(", ", arr)}");
            
            InsertionSortAlgorithm(arr);
            Console.WriteLine($"   Sorted: {string.Join(", ", arr)}");
        }

        static void InsertionSortAlgorithm(int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                int key = arr[i];
                int j = i - 1;
                while (j >= 0 && arr[j] > key)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = key;
            }
        }
    }
}