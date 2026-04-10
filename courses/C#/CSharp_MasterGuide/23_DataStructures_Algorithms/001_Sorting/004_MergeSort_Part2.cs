/*
 * ============================================================
 * TOPIC     : Data Structures & Algorithms
 * SUBTOPIC  : Sorting - Merge Sort Part 2
 * FILE      : 05_MergeSort_Part2.cs
 * PURPOSE   : Advanced Merge Sort variations
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._23_DataStructures_Algorithms._01_Sorting
{
    /// <summary>
    /// Advanced Merge Sort
    /// </summary>
    public class MergeSortPart2
    {
        public static void Main(string[] args)
        {
            var arr = new int[] { 38, 27, 43, 3, 9, 82, 10 };
            Console.WriteLine($"   Original: {string.Join(", ", arr)}");
            
            MergeSort(arr, 0, arr.Length - 1);
            Console.WriteLine($"   Sorted: {string.Join(", ", arr)}");
        }

        static void MergeSort(int[] arr, int left, int right)
        {
            if (left < right)
            {
                int mid = left + (right - left) / 2;
                MergeSort(arr, left, mid);
                MergeSort(arr, mid + 1, right);
                Merge(arr, left, mid, right);
            }
        }

        static void Merge(int[] arr, int left, int mid, int right)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;
            int[] L = new int[n1];
            int[] R = new int[n2];
            
            for (int x = 0; x < n1; x++) L[x] = arr[left + x];
            for (int x = 0; x < n2; x++) R[x] = arr[mid + 1 + x];
            
            int i = 0, j = 0, k = left;
            while (i < n1 && j < n2)
            {
                if (L[i] <= R[j]) arr[k++] = L[i++];
                else arr[k++] = R[j++];
            }
            while (i < n1) arr[k++] = L[i++];
            while (j < n2) arr[k++] = R[j++];
        }
    }
}