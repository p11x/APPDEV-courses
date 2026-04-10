/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Command Variations
 * FILE      : 02_CommandVariations.cs
 * PURPOSE   : Demonstrates different Command pattern approaches
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>, Queue<T>

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._03_Command
{
    /// <summary>
    /// Demonstrates Command variations
    /// </summary>
    public class CommandVariations
    {
        /// <summary>
        /// Entry point for Command variations
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Command Variations ===
            Console.WriteLine("=== Command Variations ===\n");

            // ── CONCEPT: Functional Command ───────────────────────────────────
            // Using delegates as commands

            // Example 1: Functional Command
            // Output: 1. Functional Command:
            Console.WriteLine("1. Functional Command:");
            
            // Use Action delegate as command
            var commandManager = new FunctionalCommandManager();
            
            commandManager.Execute(() => Console.WriteLine("   Action 1 executed"));
            commandManager.Execute(() => Console.WriteLine("   Action 2 executed"));
            
            // Output: Action 1 executed
            // Output: Action 2 executed

            // ── CONCEPT: Async Commands ───────────────────────────────────────
            // Commands that execute asynchronously

            // Example 2: Async Commands
            // Output: 2. Async Commands:
            Console.WriteLine("\n2. Async Commands:");
            
            // Async command execution
            var asyncManager = new AsyncCommandManager();
            
            asyncManager.ExecuteAsync(new AsyncDownloadCommand("file.zip"));
            // Output: Downloading: file.zip (async)
            
            asyncManager.ExecuteAsync(new AsyncProcessCommand("data.csv"));
            // Output: Processing: data.csv (async)
            
            // Output: Async commands queued

            // ── CONCEPT: Command with Parameters ─────────────────────────────
            // Commands carrying data

            // Example 3: Command with Parameters
            // Output: 3. Command with Parameters:
            Console.WriteLine("\n3. Command with Parameters:");
            
            // Parameterized command
            var paramCommand = new ParameterizedCommand<int>(x => x * 2);
            var result = paramCommand.Execute(5);
            // Output: Parameterized: 5 * 2 = 10

            // ── REAL-WORLD EXAMPLE: Transaction Management ───────────────────
            // Output: --- Real-World: Transaction Management ---
            Console.WriteLine("\n--- Real-World: Transaction Management ---");
            
            // Database transactions with command pattern
            var transaction = new TransactionManager();
            
            // Add operations
            transaction.AddOperation(new TransferCommand(100, "account1", "account2"));
            transaction.AddOperation(new UpdateCommand("UPDATE users SET active=1"));
            transaction.AddOperation(new InsertCommand("INSERT INTO logs VALUES ('txn')"));
            
            // Commit all
            transaction.Commit();
            // Output: Transaction: Transferred $100 from account1 to account2
            // Output: Transaction: Updated records
            // Output: Transaction: Inserted log entry
            // Output: Transaction committed: Success

            Console.WriteLine("\n=== Command Variations Complete ===");
        }
    }

    /// <summary>
    /// Functional command manager
    /// </summary>
    public class FunctionalCommandManager
    {
        private List<Action> _commands = new List<Action>();
        
        public void Execute(Action command)
        {
            command();
        }
        
        public void ExecuteAndStore(Action command)
        {
            _commands.Add(command);
            command();
        }
    }

    /// <summary>
    /// Async command interface
    /// </summary>
    public interface IAsyncCommand
    {
        Task ExecuteAsync(); // method: executes asynchronously
    }

    /// <summary>
    /// Async download command
    /// </summary>
    public class AsyncDownloadCommand : IAsyncCommand
    {
        private string _fileName;
        
        public AsyncDownloadCommand(string fileName)
        {
            _fileName = fileName;
        }
        
        public Task ExecuteAsync()
        {
            Console.WriteLine($"   Downloading: {_fileName} (async)");
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Async process command
    /// </summary>
    public class AsyncProcessCommand : IAsyncCommand
    {
        private string _fileName;
        
        public AsyncProcessCommand(string fileName)
        {
            _fileName = fileName;
        }
        
        public Task ExecuteAsync()
        {
            Console.WriteLine($"   Processing: {_fileName} (async)");
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Async command manager
    /// </summary>
    public class AsyncCommandManager
    {
        private Queue<IAsyncCommand> _queue = new Queue<IAsyncCommand>();
        
        public void ExecuteAsync(IAsyncCommand command)
        {
            _queue.Enqueue(command);
            command.ExecuteAsync();
            Console.WriteLine("   Async commands queued");
        }
    }

    /// <summary>
    /// Generic parameterized command
    /// </summary>
    public class ParameterizedCommand<T>
    {
        private Func<T, T> _operation;
        
        public ParameterizedCommand(Func<T, T> operation)
        {
            _operation = operation;
        }
        
        public T Execute(T input)
        {
            var result = _operation(input);
            Console.WriteLine($"   Parameterized: {input} * 2 = {result}");
            return result;
        }
    }

    /// <summary>
    /// Transaction command interface
    /// </summary>
    public interface ITransactionCommand
    {
        void Execute(); // method: executes transaction
    }

    /// <summary>
    /// Transfer command
    /// </summary>
    public class TransferCommand : ITransactionCommand
    {
        private decimal _amount;
        private string _from, _to;
        
        public TransferCommand(decimal amount, string from, string to)
        {
            _amount = amount;
            _from = from;
            _to = to;
        }
        
        public void Execute()
        {
            Console.WriteLine($"   Transaction: Transferred ${_amount} from {_from} to {_to}");
        }
    }

    /// <summary>
    /// Update command
    /// </summary>
    public class UpdateCommand : ITransactionCommand
    {
        private string _sql;
        
        public UpdateCommand(string sql)
        {
            _sql = sql;
        }
        
        public void Execute()
        {
            Console.WriteLine("   Transaction: Updated records");
        }
    }

    /// <summary>
    /// Insert command
    /// </summary>
    public class InsertCommand : ITransactionCommand
    {
        private string _sql;
        
        public InsertCommand(string sql)
        {
            _sql = sql;
        }
        
        public void Execute()
        {
            Console.WriteLine("   Transaction: Inserted log entry");
        }
    }

    /// <summary>
    /// Transaction manager
    /// </summary>
    public class TransactionManager
    {
        private List<ITransactionCommand> _operations = new List<ITransactionCommand>();
        
        public void AddOperation(ITransactionCommand command)
        {
            _operations.Add(command);
        }
        
        public void Commit()
        {
            foreach (var op in _operations)
            {
                op.Execute();
            }
            Console.WriteLine("   Transaction committed: Success");
        }
        
        public void Rollback()
        {
            Console.WriteLine("   Transaction rolled back");
        }
    }
}