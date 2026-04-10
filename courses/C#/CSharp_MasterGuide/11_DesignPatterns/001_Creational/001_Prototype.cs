/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Prototype Pattern
 * FILE      : 08_Prototype.cs
 * PURPOSE   : Demonstrates Prototype design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational
{
    /// <summary>
    /// Demonstrates Prototype pattern
    /// </summary>
    public class PrototypeDemo
    {
        /// <summary>
        /// Entry point for Prototype examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Prototype Pattern ===
            Console.WriteLine("=== Prototype Pattern ===\n");

            // ── CONCEPT: What is Prototype? ───────────────────────────────────
            // Creates new objects by cloning existing ones

            // Example 1: Basic Prototype
            // Output: 1. Basic Prototype:
            Console.WriteLine("1. Basic Prototype:");
            
            var original = new DocumentPrototype("Original", "Content here");
            var clone = original.Clone();
            // Output: Original: Original (Content here)
            // Output: Clone: Original (Content here)
            Console.WriteLine($"   Original: {original.Title} ({original.Content})");
            Console.WriteLine($"   Clone: {clone.Title} ({clone.Content})");

            // Example 2: Modifying Clone
            // Output: 2. Modifying Clone:
            Console.WriteLine("\n2. Modifying Clone:");
            
            clone.Title = "Modified Clone";
            // Output: Original: Original (unchanged)
            // Output: Clone: Modified Clone (independent)
            Console.WriteLine($"   Original: {original.Title} (unchanged)");
            Console.WriteLine($"   Clone: {clone.Title} (independent)");

            // Example 3: Prototype Registry
            // Output: 3. Prototype Registry:
            Console.WriteLine("\n3. Prototype Registry:");
            
            var registry = new DocumentRegistry();
            var report = registry.GetPrototype("Report");
            var invoice = registry.GetPrototype("Invoice");
            // Output: Report cloned: Report Template
            // Output: Invoice cloned: Invoice Template

            Console.WriteLine("\n=== Prototype Complete ===");
        }
    }

    /// <summary>
    /// Prototype interface
    /// </summary>
    public interface IPrototype<T>
    {
        T Clone(); // method: clones the object
    }

    /// <summary>
    /// Document - concrete prototype
    /// </summary>
    public class DocumentPrototype : IPrototype<DocumentPrototype>
    {
        public string Title { get; set; } // property: document title
        public string Content { get; set; } // property: document content
        
        public DocumentPrototype(string title, string content)
        {
            Title = title;
            Content = content;
        }
        
        /// <summary>
        /// Creates a deep copy of the document
        /// </summary>
        public DocumentPrototype Clone()
        {
            return new DocumentPrototype(Title, Content);
        }
    }

    /// <summary>
    /// Prototype registry
    /// </summary>
    public class DocumentRegistry
    {
        private Dictionary<string, DocumentPrototype> _prototypes = 
            new Dictionary<string, DocumentPrototype>();
        
        public DocumentRegistry()
        {
            _prototypes["Report"] = new DocumentPrototype("Report Template", "Report content");
            _prototypes["Invoice"] = new DocumentPrototype("Invoice Template", "Invoice content");
        }
        
        /// <summary>
        /// Gets prototype by type
        /// </summary>
        public DocumentPrototype GetPrototype(string type)
        {
            var prototype = _prototypes[type];
            Console.WriteLine($"   {type} cloned: {prototype.Title}");
            return prototype.Clone();
        }
    }
}