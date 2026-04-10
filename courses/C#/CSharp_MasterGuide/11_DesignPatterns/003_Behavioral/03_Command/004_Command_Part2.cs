/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Command Part 2
 * FILE      : 04_Command_Part2.cs
 * PURPOSE   : Demonstrates advanced Command patterns in C#
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for Stack

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._03_Command
{
    /// <summary>
    /// Advanced Command patterns with undo/redo
    /// </summary>
    public class CommandPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Command Part 2 ===\n");

            Console.WriteLine("1. Command with Undo/Redo:");
            var editor = new TextEditorWithUndo();
            editor.ExecuteCommand(new TypeCommand("Hello "));
            editor.ExecuteCommand(new TypeCommand("World"));
            editor.Undo();
            editor.Redo();
            // Output: Hello World
            // Output: Undo removed World
            // Output: Redo restored World

            Console.WriteLine("\n=== Command Part 2 Complete ===");
        }
    }

    public interface ICommand { void Execute(); void Undo(); }

    public class TypeCommand : ICommand
    {
        private TextEditorWithUndo _editor;
        private string _text;
        
        public TypeCommand(string text)
        {
            _text = text;
        }
        
        public void Execute() => Console.WriteLine($"   Typed: {_text}");
        public void Undo() => Console.WriteLine($"   Undo removed {_text}");
    }

    public class TextEditorWithUndo
    {
        private Stack<ICommand> _history = new();
        private Stack<ICommand> _redo = new();
        
        public void ExecuteCommand(ICommand cmd)
        {
            cmd.Execute();
            _history.Push(cmd);
            _redo.Clear();
        }
        
        public void Undo()
        {
            if (_history.Count > 0)
            {
                var cmd = _history.Pop();
                cmd.Undo();
                _redo.Push(cmd);
            }
        }
        
        public void Redo()
        {
            if (_redo.Count > 0)
            {
                var cmd = _redo.Pop();
                cmd.Execute();
                _history.Push(cmd);
            }
        }
    }
}