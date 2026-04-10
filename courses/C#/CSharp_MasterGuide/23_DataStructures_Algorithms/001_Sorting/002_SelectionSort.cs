/*
 * ============================================================
 * TOPIC     : Data Structures & Algorithms
 * SUBTOPIC  : Sorting - Selection Sort
 * FILE      : 02_SelectionSort.cs
 * PURPOSE   : Selection sort algorithm
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._23_DataStructures_Algorithms._01_Sorting
{
    /// <summary>
    /// Selection sort
    /// </summary>
    public class SelectionSort
    {
        public static void Main(string[] args)
        {
            var arr = new int[] { 64, 25, 12, 22, 11 };
            Console.WriteLine($"   Original: {string.Join(", ", arr)}");
            
            SelectionSortAlgorithm(arr);
            Console.WriteLine($"   Sorted: {string.Join(", ", arr)}");
        }

        static void SelectionSortAlgorithm(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                int minIdx = i;
                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (arr[j] < arr[minIdx]) minIdx = j;
                }
                (arr[i], arr[minIdx]) = (arr[minIdx], arr[i]);
            }
        }
    }
}