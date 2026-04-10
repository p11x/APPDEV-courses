/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Memento Pattern
 * FILE      : 01_Memento.cs
 * PURPOSE   : Demonstrates Memento design pattern in C#
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._07_Memento
{
    /// <summary>
    /// Demonstrates Memento pattern - capture and restore state
    /// </summary>
    public class MementoPattern
    {
        /// <summary>
        /// Entry point for Memento examples
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Memento Pattern Demo ===\n");

            // Example: Text Editor Undo
            Console.WriteLine("1. Text Editor Undo:");
            var editor = new TextEditor();
            editor.Type("Hello");
            editor.Type(" World");
            
            // Save state
            var saved = editor.Save();
            
            editor.Type("!!!");
            // Output: Current: Hello World!!!
            
            // Restore
            editor.Restore(saved);
            // Output: Restored: Hello World

            Console.WriteLine("\n=== Memento Complete ===");
        }
    }

    /// <summary>
    /// Memento - stores state
    /// </summary>
    public class EditorMemento
    {
        public string Content { get; }
        
        public EditorMemento(string content) => Content = content;
    }

    /// <summary>
    /// Originator - creates and restores memento
    /// </summary>
    public class TextEditor
    {
        private string _content = "";
        
        /// <summary>
        /// Types text
        /// </summary>
        public void Type(string text) => _content += text;
        
        /// <summary>
        /// Saves current state
        /// </summary>
        public EditorMemento Save() => new EditorMemento(_content);
        
        /// <summary>
        /// Restores state from memento
        /// </summary>
        public void Restore(EditorMemento memento)
        {
            _content = memento.Content;
            Console.WriteLine($"   Restored: {_content}");
        }
        
        /// <summary>
        /// Gets current content
        /// </summary>
        public string GetContent() => _content;
    }
}