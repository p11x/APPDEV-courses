/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Command Pattern
 * FILE      : 01_CommandPattern.cs
 * PURPOSE   : Demonstrates Command design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._03_Command
{
    /// <summary>
    /// Demonstrates Command pattern
    /// </summary>
    public class CommandPattern
    {
        /// <summary>
        /// Entry point for Command pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Command Pattern ===
            Console.WriteLine("=== Command Pattern ===\n");

            // ── CONCEPT: What is Command? ─────────────────────────────────────
            // Encapsulates request as an object

            // Example 1: Basic Command
            // Output: 1. Basic Command:
            Console.WriteLine("1. Basic Command:");
            
            // Receiver performs actual work
            var light = new Light();
            
            // Commands encapsulate requests
            var turnOn = new TurnOnCommand(light);
            var turnOff = new TurnOffCommand(light);
            
            // Invoker triggers commands
            var remote = new RemoteControl();
            remote.ExecuteCommand(turnOn);
            // Output: Light turned on
            
            remote.ExecuteCommand(turnOff);
            // Output: Light turned off

            // ── CONCEPT: Command with Undo ─────────────────────────────────────
            // Supports undo operations

            // Example 2: Command with Undo
            // Output: 2. Command with Undo:
            Console.WriteLine("\n2. Command with Undo:");
            
            var editor = new TextEditor();
            var history = new CommandHistory();
            
            // Insert command
            var insert = new InsertTextCommand(editor, "Hello", 0);
            insert.Execute();
            // Output: Inserted: Hello
            
            history.Push(insert);
            
            // Undo
            var last = history.Pop();
            last.Undo();
            // Output: Undid: Hello removed

            // ── CONCEPT: Macro Commands ────────────────────────────────────────
            // Combines multiple commands

            // Example 3: Macro Commands
            // Output: 3. Macro Commands:
            Console.WriteLine("\n3. Macro Commands:");
            
            // Composite command
            var batch = new BatchCommand();
            batch.AddCommand(new TaskCommand("Task 1"));
            batch.AddCommand(new TaskCommand("Task 2"));
            batch.AddCommand(new TaskCommand("Task 3"));
            
            // Execute all at once
            batch.Execute();
            // Output: Executed: Task 1
            // Output: Executed: Task 2
            // Output: Executed: Task 3

            // ── REAL-WORLD EXAMPLE: Task Queue ────────────────────────────────
            // Output: --- Real-World: Task Queue ---
            Console.WriteLine("\n--- Real-World: Task Queue ---");
            
            // Task queue processes commands asynchronously
            var queue = new TaskQueue();
            
            // Add tasks
            queue.Enqueue(new DownloadCommand("file1.zip"));
            queue.Enqueue(new ProcessCommand("data.csv"));
            queue.Enqueue(new UploadCommand("result.json"));
            
            // Process all
            queue.ProcessAll();
            // Output: Downloaded: file1.zip
            // Output: Processed: data.csv
            // Output: Uploaded: result.json

            Console.WriteLine("\n=== Command Pattern Complete ===");
        }
    }

    /// <summary>
    /// Command interface
    /// </summary>
    public interface ICommand
    {
        void Execute(); // method: executes command
        void Undo(); // method: undoes command
    }

    /// <summary>
    /// Light - receiver
    /// </summary>
    public class Light
    {
        public void TurnOn() => Console.WriteLine("   Light turned on");
        public void TurnOff() => Console.WriteLine("   Light turned off");
    }

    /// <summary>
    /// Turn on command
    /// </summary>
    public class TurnOnCommand : ICommand
    {
        private Light _light;
        
        public TurnOnCommand(Light light)
        {
            _light = light;
        }
        
        public void Execute() => _light.TurnOn();
        public void Undo() => _light.TurnOff();
    }

    /// <summary>
    /// Turn off command
    /// </summary>
    public class TurnOffCommand : ICommand
    {
        private Light _light;
        
        public TurnOffCommand(Light light)
        {
            _light = light;
        }
        
        public void Execute() => _light.TurnOff();
        public void Undo() => _light.TurnOn();
    }

    /// <summary>
    /// Remote control - invoker
    /// </summary>
    public class RemoteControl
    {
        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
        }
    }

    /// <summary>
    /// Text editor - receiver
    /// </summary>
    public class TextEditor
    {
        public string Text { get; set; } = "";
    }

    /// <summary>
    /// Insert text command
    /// </summary>
    public class InsertTextCommand : ICommand
    {
        private TextEditor _editor;
        private string _text;
        private int _position;
        
        public InsertTextCommand(TextEditor editor, string text, int position)
        {
            _editor = editor;
            _text = text;
            _position = position;
        }
        
        public void Execute()
        {
            Console.WriteLine($"   Inserted: {_text}");
            _editor.Text = _text + _editor.Text;
        }
        
        public void Undo()
        {
            Console.WriteLine($"   Undid: {_text} removed");
            _editor.Text = _editor.Text.Replace(_text, "");
        }
    }

    /// <summary>
    /// Command history
    /// </summary>
    public class CommandHistory
    {
        private Stack<ICommand> _history = new Stack<ICommand>();
        
        public void Push(ICommand command) => _history.Push(command);
        public ICommand Pop() => _history.Pop();
    }

    /// <summary>
    /// Task command
    /// </summary>
    public class TaskCommand : ICommand
    {
        private string _taskName;
        
        public TaskCommand(string taskName)
        {
            _taskName = taskName;
        }
        
        public void Execute()
        {
            Console.WriteLine($"   Executed: {_taskName}");
        }
        
        public void Undo()
        {
            Console.WriteLine($"   Undid: {_taskName}");
        }
    }

    /// <summary>
    /// Batch command - composite
    /// </summary>
    public class BatchCommand : ICommand
    {
        private List<ICommand> _commands = new List<ICommand>();
        
        public void AddCommand(ICommand command)
        {
            _commands.Add(command);
        }
        
        public void Execute()
        {
            foreach (var cmd in _commands)
            {
                cmd.Execute();
            }
        }
        
        public void Undo()
        {
            foreach (var cmd in _commands)
            {
                cmd.Undo();
            }
        }
    }

    /// <summary>
    /// Download command
    /// </summary>
    public class DownloadCommand : ICommand
    {
        private string _fileName;
        
        public DownloadCommand(string fileName)
        {
            _fileName = fileName;
        }
        
        public void Execute()
        {
            Console.WriteLine($"   Downloaded: {_fileName}");
        }
        
        public void Undo() { }
    }

    /// <summary>
    /// Process command
    /// </summary>
    public class ProcessCommand : ICommand
    {
        private string _fileName;
        
        public ProcessCommand(string fileName)
        {
            _fileName = fileName;
        }
        
        public void Execute()
        {
            Console.WriteLine($"   Processed: {_fileName}");
        }
        
        public void Undo() { }
    }

    /// <summary>
    /// Upload command
    /// </summary>
    public class UploadCommand : ICommand
    {
        private string _fileName;
        
        public UploadCommand(string fileName)
        {
            _fileName = fileName;
        }
        
        public void Execute()
        {
            Console.WriteLine($"   Uploaded: {_fileName}");
        }
        
        public void Undo() { }
    }

    /// <summary>
    /// Task queue
    /// </summary>
    public class TaskQueue
    {
        private Queue<ICommand> _queue = new Queue<ICommand>();
        
        public void Enqueue(ICommand command)
        {
            _queue.Enqueue(command);
        }
        
        public void ProcessAll()
        {
            while (_queue.Count > 0)
            {
                var cmd = _queue.Dequeue();
                cmd.Execute();
            }
        }
    }
}