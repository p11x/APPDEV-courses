/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Composite Pattern
 * FILE      : 04_Composite.cs
 * PURPOSE   : Demonstrates Composite design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural
{
    /// <summary>
    /// Demonstrates Composite pattern
    /// </summary>
    public class CompositePattern
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Composite Pattern ===\n");

            Console.WriteLine("1. Composite - Tree Structure:");
            var root = new Folder("root");
            var docs = new Folder("documents");
            var pics = new Folder("pictures");
            
            docs.AddFile(new File("resume.pdf", 1024));
            docs.AddFile(new File("notes.txt", 256));
            pics.AddFile(new File("photo.jpg", 4096));
            
            root.AddFolder(docs);
            root.AddFolder(pics);
            
            root.Display(0);
            // Output: root/
            // Output:   documents/
            // Output:     resume.pdf (1 KB)
            // Output:     notes.txt (0.3 KB)
            // Output:   pictures/
            // Output:     photo.jpg (4 KB)

            Console.WriteLine("\n=== Composite Complete ===");
        }
    }

    public interface IFileSystemItem
    {
        string Name { get; }
        long GetSize();
        void Display(int indent);
    }

    public class File : IFileSystemItem
    {
        public string Name { get; }
        private long _size;
        
        public File(string name, long size)
        {
            Name = name;
            _size = size;
        }
        
        public long GetSize() => _size;
        
        public void Display(int indent)
        {
            Console.WriteLine($"{new string(' ', indent * 2)}{Name} ({_size / 1024.0:F1} KB)");
        }
    }

    public class Folder : IFileSystemItem
    {
        public string Name { get; }
        private List<IFileSystemItem> _items = new();
        
        public Folder(string name) => Name = name;
        
        public void AddFile(IFileSystemItem item) => _items.Add(item);
        public void AddFolder(Folder folder) => _items.Add(folder);
        
        public long GetSize()
        {
            long total = 0;
            foreach (var item in _items) total += item.GetSize();
            return total;
        }
        
        public void Display(int indent)
        {
            Console.WriteLine($"{new string(' ', indent * 2)}{Name}/");
            foreach (var item in _items) item.Display(indent + 1);
        }
    }
}