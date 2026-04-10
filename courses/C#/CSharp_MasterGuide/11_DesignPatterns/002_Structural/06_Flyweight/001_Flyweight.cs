/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Flyweight Pattern
 * FILE      : 01_Flyweight.cs
 * PURPOSE   : Demonstrates Flyweight design pattern in C#
 * ============================================================
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._06_Flyweight
{
    /// <summary>
    /// Demonstrates Flyweight pattern - shared objects for efficiency
    /// </summary>
    public class FlyweightPattern
    {
        /// <summary>
        /// Entry point for Flyweight examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Flyweight Pattern Demo ===
            Console.WriteLine("=== Flyweight Pattern Demo ===\n");

            // ── CONCEPT: Flyweight Pattern ───────────────────────────────────────
            // Share common data between many similar objects

            // Example 1: Tree Forest
            // Output: 1. Tree Forest:
            Console.WriteLine("1. Tree Forest:");
            
            var forest = new TreeFactory();
            
            // Many trees share same tree type
            var tree1 = forest.GetTree("Oak", "Green", "Medium");
            var tree2 = forest.GetTree("Oak", "Green", "Medium");
            var tree3 = forest.GetTree("Pine", "DarkGreen", "Tall");
            
            // Output: Oak instances: 1 (shared), Pine instances: 1
            Console.WriteLine($"   Oak instances: {tree1.Type == tree2.Type}, Pine: {tree3.Type}");

            // Example 2: Character Rendering
            // Output: 2. Character Rendering:
            Console.WriteLine("\n2. Character Rendering:");
            
            var characterFactory = new CharacterFactory();
            
            // Same character 'A' shared across document
            var char1 = characterFactory.GetCharacter('A', "Arial", 12);
            var char2 = characterFactory.GetCharacter('A', "Arial", 12);
            var char3 = characterFactory.GetCharacter('B', "Arial", 12);
            
            // Output: A: Same object, B: Different
            Console.WriteLine($"   A: {char1 == char2}, B: {char3 != char1}");

            // ── REAL-WORLD EXAMPLE: Game Particle System ───────────────────────
            // Output: --- Real-World: Particle System ---
            Console.WriteLine("\n--- Real-World: Particle System ---");
            
            var particleFactory = new ParticleFactory();
            
            // Create many particles - sharing types
            var p1 = particleFactory.GetParticle("Fire", "Red", "Circle");
            var p2 = particleFactory.GetParticle("Fire", "Red", "Circle");
            var p3 = particleFactory.GetParticle("Smoke", "Gray", "Cloud");
            
            // Output: Fire particles: 1 shared type, Smoke: 1
            Console.WriteLine($"   Fire particles: {p1.Type}, Smoke: {p3.Type}");

            Console.WriteLine("\n=== Flyweight Pattern Complete ===");
        }
    }

    /// <summary>
    /// Tree type - intrinsic state (shared)
    /// </summary>
    public class TreeType
    {
        public string Type { get; }
        public string Color { get; }
        public string Size { get; }
        
        public TreeType(string type, string color, string size)
        {
            Type = type;
            Color = color;
            Size = size;
        }
    }

    /// <summary>
    /// Tree factory - manages flyweight objects
    /// </summary>
    public class TreeFactory
    {
        private readonly Dictionary<string, TreeType> _treeTypes = new();
        
        /// <summary>
        /// Gets or creates tree type
        /// </summary>
        public TreeType GetTree(string type, string color, string size)
        {
            var key = $"{type}_{color}_{size}";
            
            if (!_treeTypes.ContainsKey(key))
            {
                _treeTypes[key] = new TreeType(type, color, size);
            }
            
            return _treeTypes[key];
        }
    }

    /// <summary>
    /// Character - flyweight for text rendering
    /// </summary>
    public class Character
    {
        public char Symbol { get; }
        public string Font { get; }
        public int Size { get; }
        
        public Character(char symbol, string font, int size)
        {
            Symbol = symbol;
            Font = font;
            Size = size;
        }
    }

    /// <summary>
    /// Character factory
    /// </summary>
    public class CharacterFactory
    {
        private readonly Dictionary<string, Character> _characters = new();
        
        /// <summary>
        /// Gets or creates character
        /// </summary>
        public Character GetCharacter(char symbol, string font, int size)
        {
            var key = $"{symbol}_{font}_{size}";
            
            if (!_characters.ContainsKey(key))
            {
                _characters[key] = new Character(symbol, font, size);
            }
            
            return _characters[key];
        }
    }

    /// <summary>
    /// Particle type
    /// </summary>
    public class ParticleType
    {
        public string Type { get; }
        public string Color { get; }
        public string Shape { get; }
        
        public ParticleType(string type, string color, string shape)
        {
            Type = type;
            Color = color;
            Shape = shape;
        }
    }

    /// <summary>
    /// Particle factory
    /// </summary>
    public class ParticleFactory
    {
        private readonly Dictionary<string, ParticleType> _types = new();
        
        /// <summary>
        /// Gets or creates particle type
        /// </summary>
        public ParticleType GetParticle(string type, string color, string shape)
        {
            var key = $"{type}_{color}_{shape}";
            
            if (!_types.ContainsKey(key))
            {
                _types[key] = new ParticleType(type, color, shape);
            }
            
            return _types[key];
        }
    }
}