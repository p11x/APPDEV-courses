/*
 * ============================================================
 * TOPIC     : Data Structures & Algorithms
 * SUBTOPIC  : Data Structures - Binary Tree
 * FILE      : 01_BinaryTree.cs
 * PURPOSE   : Binary tree implementation
 * ============================================================
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._23_DataStructures_Algorithms._03_DataStructures
{
    /// <summary>
    /// Binary tree implementation
    /// </summary>
    public class BinaryTreeDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Binary Tree ===\n");

            var root = new TreeNode(1);
            root.Left = new TreeNode(2);
            root.Right = new TreeNode(3);
            
            Console.WriteLine("   Tree created with root: 1");
            
            // Inorder traversal
            var result = new List<int>();
            Inorder(root, result);
            Console.WriteLine($"   Inorder: {string.Join(", ", result)}");

            Console.WriteLine("\n=== Binary Tree Complete ===");
        }

        static void Inorder(TreeNode node, List<int> result)
        {
            if (node == null) return;
            Inorder(node.Left, result);
            result.Add(node.Value);
            Inorder(node.Right, result);
        }
    }

    /// <summary>
    /// Tree node class
    /// </summary>
    public class TreeNode
    {
        public int Value { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }
        
        public TreeNode(int value) => Value = value;
    }
}