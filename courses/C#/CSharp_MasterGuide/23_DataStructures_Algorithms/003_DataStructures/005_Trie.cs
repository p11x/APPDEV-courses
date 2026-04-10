/*
 * ============================================================
 * TOPIC     : Data Structures & Algorithms
 * SUBTOPIC  : Data Structures - Trie
 * FILE      : 10_Trie.cs
 * PURPOSE   : Trie (prefix tree) implementation
 * ============================================================
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._23_DataStructures_Algorithms._03_DataStructures
{
    /// <summary>
    /// Trie data structure
    /// </summary>
    public class Trie
    {
        private readonly TrieNode _root = new();
        
        public static void Main(string[] args)
        {
            var trie = new Trie();
            trie.Insert("hello");
            Console.WriteLine($"   Contains 'hello': {trie.Search("hello")}");
        }
        
        public void Insert(string word)
        {
            var node = _root;
            foreach (char c in word)
            {
                if (!node.Children.ContainsKey(c))
                    node.Children[c] = new TrieNode();
                node = node.Children[c];
            }
            node.IsEndOfWord = true;
        }
        
        public bool Search(string word)
        {
            var node = _root;
            foreach (char c in word)
            {
                if (!node.Children.ContainsKey(c)) return false;
                node = node.Children[c];
            }
            return node.IsEndOfWord;
        }
    }

    public class TrieNode
    {
        public Dictionary<char, TrieNode> Children = new();
        public bool IsEndOfWord;
    }
}