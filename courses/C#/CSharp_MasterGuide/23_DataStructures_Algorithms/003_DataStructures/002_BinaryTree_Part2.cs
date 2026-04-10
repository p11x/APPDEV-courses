/*
 * ============================================================
 * TOPIC     : Data Structures & Algorithms
 * SUBTOPIC  : Data Structures - Binary Tree Part 2
 * FILE      : 02_BinaryTree_Part2.cs
 * PURPOSE   : Advanced binary tree operations
 * ============================================================
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._23_DataStructures_Algorithms._03_DataStructures
{
    /// <summary>
    /// Advanced Binary Tree
    /// </summary>
    public class BinaryTreePart2
    {
        public static void Main(string[] args)
        {
            var root = new TreeNode(1);
            root.Left = new TreeNode(2);
            root.Right = new TreeNode(3);
            
            // Level order
            var result = new List<int>();
            LevelOrder(root, result);
            Console.WriteLine($"   Level Order: {string.Join(", ", result)}");
        }

        static void LevelOrder(TreeNode node, List<int> result)
        {
            if (node == null) return;
            var queue = new Queue<TreeNode>();
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                result.Add(current.Value);
                if (current.Left != null) queue.Enqueue(current.Left);
                if (current.Right != null) queue.Enqueue(current.Right);
            }
        }
    }

    public class TreeNode
    {
        public int Value { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }
        public TreeNode(int v) { Value = v; }
    }
}