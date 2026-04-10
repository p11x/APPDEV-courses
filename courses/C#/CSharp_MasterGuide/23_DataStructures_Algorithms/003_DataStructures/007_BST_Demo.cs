/*
 * ============================================================
 * TOPIC     : Data Structures & Algorithms
 * SUBTOPIC  : Binary Search Tree
 * FILE      : BST_Demo.cs
 * PURPOSE   : Binary search tree implementation
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._23_DataStructures_Algorithms._03_DataStructures
{
    /// <summary>
    /// BST demonstration
    /// </summary>
    public class BSTDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Binary Search Tree ===\n");

            // Output: --- Insert ---
            Console.WriteLine("--- Insert ---");

            var bst = new BinarySearchTree();
            bst.Insert(50);
            bst.Insert(30);
            bst.Insert(70);
            Console.WriteLine("   Nodes inserted");
            // Output: Nodes inserted

            // Output: --- Search ---
            Console.WriteLine("\n--- Search ---");

            var found = bst.Search(30);
            Console.WriteLine($"   Found: {found}");
            // Output: Found: True

            // Output: --- InOrder Traversal ---
            Console.WriteLine("\n--- InOrder ---");

            bst.InOrder(n => Console.WriteLine($"   {n.Value}"));
            // Output: 30
            // Output: 50
            // Output: 70

            Console.WriteLine("\n=== BST Complete ===");
        }
    }

    /// <summary>
    /// Tree node
    /// </summary>
    public class TreeNode
    {
        public int Value { get; set; } // property: value
        public TreeNode Left { get; set; } // property: left
        public TreeNode Right { get; set; } // property: right
    }

    /// <summary>
    /// Binary search tree
    /// </summary>
    public class BinarySearchTree
    {
        private TreeNode _root; // field: root

        public void Insert(int value)
        {
            var node = new TreeNode { Value = value };
            if (_root == null) { _root = node; return; }

            var current = _root;
            while (true)
            {
                if (value < current.Value)
                {
                    if (current.Left == null) { current.Left = node; break; }
                    current = current.Left;
                }
                else
                {
                    if (current.Right == null) { current.Right = node; break; }
                    current = current.Right;
                }
            }
        }

        public bool Search(int value)
        {
            var current = _root;
            while (current != null)
            {
                if (value == current.Value) return true;
                current = value < current.Value ? current.Left : current.Right;
            }
            return false;
        }

        public void InOrder(Action<TreeNode> action)
        {
            InOrderInternal(_root, action);
        }

        private void InOrderInternal(TreeNode node, Action<TreeNode> action)
        {
            if (node == null) return;
            InOrderInternal(node.Left, action);
            action(node);
            InOrderInternal(node.Right, action);
        }
    }
}