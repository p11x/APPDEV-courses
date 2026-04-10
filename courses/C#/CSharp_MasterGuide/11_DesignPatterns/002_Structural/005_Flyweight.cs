/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Flyweight Pattern
 * FILE      : 09_Flyweight.cs
 * PURPOSE   : Demonstrates Flyweight design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for Dictionary

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural
{
    /// <summary>
    /// Demonstrates Flyweight pattern
    /// </summary>
    public class FlyweightPattern
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Flyweight Pattern ===\n");

            Console.WriteLine("1. Flyweight - Share Objects:");
            var factory = new TreeTypeFactory();
            
            var tree1 = factory.GetTreeType("Oak", "green", "oak texture");
            var tree2 = factory.GetTreeType("Oak", "green", "oak texture");
            var tree3 = factory.GetTreeType("Pine", "dark green", "pine texture");
            
            Console.WriteLine($"   Same oak: {ReferenceEquals(tree1, tree2)}");
            Console.WriteLine($"   Different type: {!ReferenceEquals(tree1, tree3)}");

            Console.WriteLine("\n=== Flyweight Complete ===");
        }
    }

    public class TreeType
    {
        public string Name { get; }
        public string Color { get; }
        public string Texture { get; }
        
        public TreeType(string name, string color, string texture)
        {
            Name = name;
            Color = color;
            Texture = texture;
        }
        
        public void Draw(int x, int y) => Console.WriteLine($"   Drawing {Name} at {x},{y}");
    }

    public class TreeTypeFactory
    {
        private Dictionary<string, TreeType> _types = new();
        
        public TreeType GetTreeType(string name, string color, string texture)
        {
            var key = $"{name}_{color}";
            if (!_types.ContainsKey(key))
            {
                _types[key] = new TreeType(name, color, texture);
                Console.WriteLine($"   Created new TreeType: {name}");
            }
            return _types[key];
        }
    }
}