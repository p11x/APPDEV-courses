/*
 * ============================================================
 * TOPIC     : Data Structures & Algorithms
 * SUBTOPIC  : Sorting - Quick Sort Part 2
 * FILE      : 07_QuickSort_Part2.cs
 * PURPOSE   : Advanced Quick Sort variations
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._23_DataStructures_Algorithms._01_Sorting
{
    /// <summary>
    /// Advanced Quick Sort
    /// </summary>
    public class QuickSortPart2
    {
        public static void Main(string[] args)
        {
            var arr = new int[] { 10, 7, 8, 9, 1, 5 };
            Console.WriteLine($"   Original: {string.Join(", ", arr)}");
            
            QuickSort(arr, 0, arr.Length - 1);
            Console.WriteLine($"   Sorted: {string.Join(", ", arr)}");
        }

        static void QuickSort(int[] arr, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(arr, low, high);
                QuickSort(arr, low, pi - 1);
                QuickSort(arr, pi + 1, high);
            }
        }

        static int Partition(int[] arr, int low, int high)
        {
            int pivot = arr[high];
            int i = low - 1;
            for (int j = low; j < high; j++)
            {
                if (arr[j] < pivot)
                {
                    i++;
                    (arr[i], arr[j]) = (arr[j], arr[i]);
                }
            }
            (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
            return i + 1;
        }
    }
}