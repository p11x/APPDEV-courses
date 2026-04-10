/*
 * ============================================================
 * TOPIC     : Data Structures & Algorithms
 * SUBTOPIC  : Data Structures - Heap
 * FILE      : 08_Heap.cs
 * PURPOSE   : Binary heap implementation
 * ============================================================
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._23_DataStructures_Algorithms._03_DataStructures
{
    /// <summary>
    /// Binary Heap
    /// </summary>
    public class BinaryHeap
    {
        private readonly List<int> _heap = new();
        
        public static void Main(string[] args)
        {
            var heap = new BinaryHeap();
            heap.Insert(10);
            heap.Insert(20);
            heap.Insert(5);
            
            Console.WriteLine($"   Min: {heap.Peek()}");
        }
        
        public void Insert(int value)
        {
            _heap.Add(value);
            HeapifyUp(_heap.Count - 1);
        }
        
        public int Peek() => _heap.Count > 0 ? _heap[0] : -1;
        
        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (_heap[index] < _heap[parent])
                    (_heap[index], _heap[parent]) = (_heap[parent], _heap[index]);
                else break;
                index = parent;
            }
        }
    }
}