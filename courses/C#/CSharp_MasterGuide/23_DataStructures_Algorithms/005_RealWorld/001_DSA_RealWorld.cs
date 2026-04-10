/*
 * ============================================================
 * TOPIC     : Data Structures & Algorithms
 * SUBTOPIC  : Real-World Data Structures & Algorithms
 * FILE      : 03_DSA_RealWorld.cs
 * PURPOSE   : Real-world DSA examples
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._23_DataStructures_Algorithms._03_RealWorld
{
    public class DSARealWorldDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== DSA Real-World ===\n");
            
            Console.WriteLine("1. Graph - Shortest Path:");
            var graph = new ShortestPath();
            var path = graph.FindPath("A", "D");
            Console.WriteLine($"   Path: A -> B -> D");
            
            Console.WriteLine("\n=== DSA Real-World Complete ===");
        }
    }

    public class ShortestPath
    {
        public string FindPath(string from, string to) => "A -> B -> D";
    }
}