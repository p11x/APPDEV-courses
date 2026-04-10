/*
 * ============================================================
 * TOPIC     : Data Structures & Algorithms
 * SUBTOPIC  : Data Structures - Graph
 * FILE      : 06_Graph_AdjacencyList.cs
 * PURPOSE   : Graph using adjacency list
 * ============================================================
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._23_DataStructures_Algorithms._03_DataStructures
{
    /// <summary>
    /// Graph with adjacency list
    /// </summary>
    public class GraphAdjacencyList
    {
        public static void Main(string[] args)
        {
            var graph = new Graph(5);
            graph.AddEdge(0, 1);
            graph.AddEdge(0, 2);
            graph.AddEdge(1, 3);
            graph.AddEdge(2, 4);
            
            Console.WriteLine("   Graph created with 5 vertices");
        }
    }

    public class Graph
    {
        private readonly List<int>[] _adjacency;
        
        public Graph(int vertices)
        {
            _adjacency = new List<int>[vertices];
            for (int i = 0; i < vertices; i++)
                _adjacency[i] = new List<int>();
        }
        
        public void AddEdge(int source, int destination)
        {
            _adjacency[source].Add(destination);
            _adjacency[destination].Add(source);
        }
    }
}