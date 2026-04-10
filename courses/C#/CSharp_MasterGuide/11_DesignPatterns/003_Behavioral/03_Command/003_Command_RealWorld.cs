/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Command Real-World
 * FILE      : 03_Command_RealWorld.cs
 * PURPOSE   : Real-world Command pattern applications
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>, Queue<T>

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._03_Command
{
    /// <summary>
    /// Real-world Command pattern examples
    /// </summary>
    public class CommandRealWorld
    {
        /// <summary>
        /// Entry point for real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Command Real-World ===
            Console.WriteLine("=== Command Real-World ===\n");

            // ── REAL-WORLD 1: Undo/Redo System ────────────────────────────────
            // Text editor with history

            // Example 1: Undo/Redo System
            // Output: 1. Undo/Redo System:
            Console.WriteLine("1. Undo/Redo System:");
            
            var textDoc = new TextDocument();
            var editor = new TextEditorCommandManager(textDoc);
            
            // Type text
            editor.ExecuteCommand(new TypeCommand(textDoc, "Hello "));
            editor.ExecuteCommand(new TypeCommand(textDoc, "World!"));
            // Output: Typed: Hello 
            // Output: Typed: World!
            
            // Undo
            editor.Undo();
            // Output: Undo: Removed "World!"
            
            // Redo
            editor.Redo();
            // Output: Redo: Re-applied "World!"

            // ── REAL-WORLD 2: Macro Recording ────────────────────────────────
            // Record and replay actions

            // Example 2: Macro Recording
            // Output: 2. Macro Recording:
            Console.WriteLine("\n2. Macro Recording:");
            
            var macroRecorder = new MacroRecorder();
            
            // Start recording
            macroRecorder.StartRecording();
            
            // Record actions
            macroRecorder.Record(new OpenFileCommand("doc1.txt"));
            macroRecorder.Record(new FormatCommand("Bold"));
            macroRecorder.Record(new SaveCommand());
            
            // Stop recording
            var macro = macroRecorder.StopRecording();
            
            // Play back
            macro.Play();
            // Output: Macro: Opened doc1.txt
            // Output: Macro: Applied Bold format
            // Output: Macro: Saved doc1.txt

            // ── REAL-WORLD 3: Queue Processing ────────────────────────────────
            // Background job processing

            // Example 3: Queue Processing
            // Output: 3. Queue Processing:
            Console.WriteLine("\n3. Queue Processing:");
            
            var jobQueue = new JobQueue();
            
            // Enqueue jobs
            jobQueue.Enqueue(new EmailJobCommand("user1@example.com", "Welcome!"));
            jobQueue.Enqueue(new EmailJobCommand("user2@example.com", "Reset password"));
            jobQueue.Enqueue(new CleanupJobCommand());
            
            // Process queue
            jobQueue.Process();
            // Output: Job: Sent email to user1@example.com
            // Output: Job: Sent email to user2@example.com
            // Output: Job: Cleanup completed

            // ── REAL-WORLD 4: Menu Actions ───────────────────────────────────
            // GUI menu commands

            // Example 4: Menu Actions
            // Output: 4. Menu Actions:
            Console.WriteLine("\n4. Menu Actions:");
            
            var menu = new MenuManager();
            
            // Register menu commands
            menu.Register("File.New", new NewFileCommand());
            menu.Register("File.Open", new OpenFileMenuCommand(""));
            menu.Register("File.Save", new SaveMenuCommand());
            menu.Register("Edit.Undo", new UndoMenuCommand());
            
            // Execute menu action
            menu.Execute("File.New");
            // Output: Menu: New file created
            
            menu.Execute("File.Save");
            // Output: Menu: File saved
            
            menu.Execute("Edit.Undo");
            // Output: Menu: Undo performed

            // ── REAL-WORLD 5: Batch Operations ────────────────────────────────
            // Multiple operations as single unit

            // Example 5: Batch Operations
            // Output: 5. Batch Operations:
            Console.WriteLine("\n5. Batch Operations:");
            
            var batchOps = new BatchOperationManager();
            
            // Add batch operations
            batchOps.Add(new CreateUserCommand("john", "john@email.com"));
            batchOps.Add(new UpdateUserCommand("john", "active"));
            batchOps.Add(new AssignRoleCommand("john", "admin"));
            
            // Execute as single unit
            batchOps.ExecuteBatch();
            // Output: Batch: Created user john
            // Output: Batch: Updated user john
            // Output: Batch: Assigned admin role to john
            // Output: Batch: All operations completed

            Console.WriteLine("\n=== Command Real-World Complete ===");
        }
    }

    /// <summary>
    /// Text document
    /// </summary>
    public class TextDocument
    {
        public string Content { get; set; } = ""; // property: document content
    }

    /// <summary>
    /// Type command
    /// </summary>
    public class TypeCommand : ICommand
    {
        private TextDocument _document;
        private string _text;
        
        public TypeCommand(TextDocument document, string text)
        {
            _document = document;
            _text = text;
        }
        
        public void Execute()
        {
            _document.Content += _text;
            Console.WriteLine($"   Typed: {_text}");
        }
        
        public void Undo()
        {
            _document.Content = _document.Content.Replace(_text, "");
            Console.WriteLine($"   Undo: Removed \"{_text}\"");
        }
    }

    /// <summary>
    /// Text editor with undo/redo
    /// </summary>
    public class TextEditorCommandManager
    {
        private TextDocument _document;
        private Stack<ICommand> _undo = new Stack<ICommand>();
        private Stack<ICommand> _redo = new Stack<ICommand>();
        
        public TextEditorCommandManager(TextDocument document)
        {
            _document = document;
        }
        
        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _undo.Push(command);
            _redo.Clear();
        }
        
        public void Undo()
        {
            if (_undo.Count > 0)
            {
                var cmd = _undo.Pop();
                cmd.Undo();
                _redo.Push(cmd);
                Console.WriteLine($"   Redo: Re-applied \"{((TypeCommand)cmd).ToString()}\"");
            }
        }
        
        public void Redo()
        {
            if (_redo.Count > 0)
            {
                var cmd = _redo.Pop();
                cmd.Execute();
                _undo.Push(cmd);
            }
        }
    }

    /// <summary>
    /// Open file command
    /// </summary>
    public class OpenFileCommand : ICommand
    {
        private string _fileName;
        
        public OpenFileCommand(string fileName)
        {
            _fileName = fileName;
        }
        
        public void Execute() => Console.WriteLine($"   Macro: Opened {_fileName}");
        public void Undo() { }
    }

    /// <summary>
    /// Format command
    /// </summary>
    public class FormatCommand : ICommand
    {
        private string _format;
        
        public FormatCommand(string format)
        {
            _format = format;
        }
        
        public void Execute() => Console.WriteLine($"   Macro: Applied {_format} format");
        public void Undo() { }
    }

    /// <summary>
    /// Save command
    /// </summary>
    public class SaveCommand : ICommand
    {
        public void Execute() => Console.WriteLine("   Macro: Saved doc1.txt");
        public void Undo() { }
    }

    /// <summary>
    /// Macro recorder
    /// </summary>
    public class MacroRecorder
    {
        private List<ICommand> _recorded = new List<ICommand>();
        private bool _recording;
        
        public void StartRecording()
        {
            _recording = true;
            _recorded.Clear();
        }
        
        public void Record(ICommand command)
        {
            if (_recording)
            {
                _recorded.Add(command);
            }
        }
        
        public Macro StopRecording()
        {
            _recording = false;
            return new Macro(_recorded);
        }
    }

    /// <summary>
    /// Macro - recorded commands
    /// </summary>
    public class Macro
    {
        private List<ICommand> _commands;
        
        public Macro(List<ICommand> commands)
        {
            _commands = commands;
        }
        
        public void Play()
        {
            foreach (var cmd in _commands)
            {
                cmd.Execute();
            }
        }
    }

    /// <summary>
    /// Email job command
    /// </summary>
    public class EmailJobCommand : ICommand
    {
        private string _to, _message;
        
        public EmailJobCommand(string to, string message)
        {
            _to = to;
            _message = message;
        }
        
        public void Execute() => Console.WriteLine($"   Job: Sent email to {_to}");
        public void Undo() { }
    }

    /// <summary>
    /// Cleanup job command
    /// </summary>
    public class CleanupJobCommand : ICommand
    {
        public void Execute() => Console.WriteLine("   Job: Cleanup completed");
        public void Undo() { }
    }

    /// <summary>
    /// Job queue
    /// </summary>
    public class JobQueue
    {
        private Queue<ICommand> _jobs = new Queue<ICommand>();
        
        public void Enqueue(ICommand job) => _jobs.Enqueue(job);
        
        public void Process()
        {
            while (_jobs.Count > 0)
            {
                _jobs.Dequeue().Execute();
            }
        }
    }

    /// <summary>
    /// Menu command interface
    /// </summary>
    public interface IMenuCommand
    {
        void Execute(); // method: executes menu action
    }

    /// <summary>
    /// New file command
    /// </summary>
    public class NewFileCommand : IMenuCommand
    {
        public void Execute() => Console.WriteLine("   Menu: New file created");
    }

    /// <summary>
    /// Open file menu command
    /// </summary>
    public class OpenFileMenuCommand : IMenuCommand
    {
        private string _path;
        
        public OpenFileMenuCommand(string path)
        {
            _path = path;
        }
        
        public void Execute() => Console.WriteLine("   Menu: File opened");
    }

    /// <summary>
    /// Save menu command
    /// </summary>
    public class SaveMenuCommand : IMenuCommand
    {
        public void Execute() => Console.WriteLine("   Menu: File saved");
    }

    /// <summary>
    /// Undo menu command
    /// </summary>
    public class UndoMenuCommand : IMenuCommand
    {
        public void Execute() => Console.WriteLine("   Menu: Undo performed");
    }

    /// <summary>
    /// Menu manager
    /// </summary>
    public class MenuManager
    {
        private Dictionary<string, IMenuCommand> _commands = new Dictionary<string, IMenuCommand>();
        
        public void Register(string menuItem, IMenuCommand command)
        {
            _commands[menuItem] = command;
        }
        
        public void Execute(string menuItem)
        {
            _commands[menuItem].Execute();
        }
    }

    /// <summary>
    /// Create user command
    /// </summary>
    public class CreateUserCommand : ICommand
    {
        private string _username, _email;
        
        public CreateUserCommand(string username, string email)
        {
            _username = username;
            _email = email;
        }
        
        public void Execute() => Console.WriteLine($"   Batch: Created user {_username}");
        public void Undo() { }
    }

    /// <summary>
    /// Update user command
    /// </summary>
    public class UpdateUserCommand : ICommand
    {
        private string _username, _status;
        
        public UpdateUserCommand(string username, string status)
        {
            _username = username;
            _status = status;
        }
        
        public void Execute() => Console.WriteLine($"   Batch: Updated user {_username}");
        public void Undo() { }
    }

    /// <summary>
    /// Assign role command
    /// </summary>
    public class AssignRoleCommand : ICommand
    {
        private string _username, _role;
        
        public AssignRoleCommand(string username, string role)
        {
            _username = username;
            _role = role;
        }
        
        public void Execute() => Console.WriteLine($"   Batch: Assigned {_role} role to {_username}");
        public void Undo() { }
    }

    /// <summary>
    /// Batch operation manager
    /// </summary>
    public class BatchOperationManager
    {
        private List<ICommand> _operations = new List<ICommand>();
        
        public void Add(ICommand operation) => _operations.Add(operation);
        
        public void ExecuteBatch()
        {
            foreach (var op in _operations)
            {
                op.Execute();
            }
            Console.WriteLine("   Batch: All operations completed");
        }
    }
}