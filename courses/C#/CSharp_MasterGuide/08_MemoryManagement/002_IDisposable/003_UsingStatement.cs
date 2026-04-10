/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : Using Statement
 * FILE      : 03_UsingStatement.cs
 * PURPOSE   : Teaches using statement and using declaration
 *            for automatic resource cleanup
 * ============================================================
 */

using System; // System namespace for Console, basic types
using System.IO; // For stream types

namespace CSharp_MasterGuide._08_MemoryManagement._02_IDisposable
{
    /// <summary>
    /// Demonstrates using statement and using declaration
    /// for automatic resource management in C#.
    /// </summary>
    class UsingStatement
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // CONCEPT: Using Statement ────────────────────────────────────
            // ═══════════════════════════════════════════════════════════
            // The 'using' statement provides automatic cleanup:
            // - Calls Dispose() when block exits (normal or exception)
            // - Works with any type implementing IDisposable
            // - Transforms to try-finally block at compile time
            //
            // Two Forms:
            // 1. using (declaration) { } - traditional
            // 2. using var x = value; - C# 8+ declaration

            Console.WriteLine("=== Using Statement Demo ===\n");

            // ── EXAMPLE 1: Traditional Using Statement ───────────────
            // Syntax: using (type variable = expression) { }

            Console.WriteLine("1. Traditional using statement:");

            // using block with explicit type declaration
            using (FileStream fs = new FileStream("sample.txt", FileMode.Create)) // using = auto-cleanup
            {
                // Write data to file
                byte[] data = System.Text.Encoding.UTF8.GetBytes("Hello via using!"); // byte[] = text as bytes
                fs.Write(data, 0, data.Length); // Write all bytes
                Console.WriteLine("   Data written to file"); // Output: Data written to file
            } // Dispose() called automatically here

            Console.WriteLine("   File properly closed"); // Output: File properly closed

            // ── EXAMPLE 2: Using with Var (C# 9+) ─────────────────────
            // Can use 'var' with type inference.

            Console.WriteLine("\n2. Using with var:");

            // using with var type inference
            using (var stream = new MemoryStream()) // using var = infer type
            {
                stream.WriteByte(100); // Write byte
                Console.WriteLine($"   Stream position: {stream.Position}"); // Output: Stream position: 1
            } // Dispose() called automatically

            Console.WriteLine("   Stream disposed"); // Output: Stream disposed

            // ── EXAMPLE 3: Using Declaration (C# 8+) ───────────────────
            // Declarative syntax: using var name = expression;
            // Disposes at end of containing scope, not just block.

            Console.WriteLine("\n3. Using declaration (C# 8+):");

            // Using declaration - disposed at end of method/scope
            using var configStream = new FileStream("config.txt", FileMode.Create); // using var = declaration
            byte[] configData = System.Text.Encoding.UTF8.GetBytes("Timeout=30"); // byte[] = config bytes
            configStream.Write(configData, 0, configData.Length); // Write config
            Console.WriteLine("   Config written"); // Output: Config written

            // More code here - stream still open
            Console.WriteLine("   Stream still open"); // Output: Stream still open

            // Stream disposed when method ends (after this point)
            // Note: In this example, ends when Main ends

            // ── EXAMPLE 4: Multiple Resources ─────────────────────────
            // Can nest using statements or chain them.

            Console.WriteLine("\n4. Multiple resources:");

            // Nested using statements
            using (FileStream input = new FileStream("input.txt", FileMode.Create)) // using = first resource
            using (FileStream output = new FileStream("output.txt", FileMode.Create)) // using = second
            {
                // Write to input
                byte[] inputData = System.Text.Encoding.UTF8.GetBytes("Input Data"); // byte[] = input bytes
                input.Write(inputData, 0, inputData.Length); // Write input

                // Write to output
                byte[] outputData = System.Text.Encoding.UTF8.GetBytes("Output Data"); // byte[] = output bytes
                output.Write(outputData, 0, outputData.Length); // Write output

                Console.WriteLine("   Both files written"); // Output: Both files written
            } // Both disposed - order: output, then input

            Console.WriteLine("   Both files closed"); // Output: Both files closed

            // ── EXAMPLE 5: Dispose Order (LIFO) ───────────────────────
            // Resources are disposed in reverse order of creation.

            Console.WriteLine("\n5. Dispose order demonstration:");

            // Create resources in order
            using (var first = new ResourceTracker("First")) // using = first resource
            using (var second = new ResourceTracker("Second")) // using = second resource
            using (var third = new ResourceTracker("Third")) // using = third resource
            {
                // Use resources
                Console.WriteLine("   Using all resources"); // Output: Using all resources
            } // Dispose order: Third, Second, First (LIFO)

            // ── EXAMPLE 6: Using with Exception Handling ───────────────
            // Using ensures cleanup even when exceptions occur.

            Console.WriteLine("\n6. Using with exceptions:");

            try // Try block for exception handling
            {
                using (var risky = new RiskyResource()) // using = auto-cleanup
                {
                    risky.DoSomething(); // Do something that might fail
                    Console.WriteLine("   Operation succeeded"); // Output: Operation succeeded
                    throw new InvalidOperationException("Simulated error!"); // throw = simulate error
                } // Dispose called even after exception
            }
            catch (InvalidOperationException ex) // Catch exception
            {
                Console.WriteLine($"   Caught: {ex.Message}"); // Output: Caught: Simulated error!
            } // Resource cleaned up properly

            // ── EXAMPLE 7: Using vs Manual Try-Finally ───────────────
            // Shows how using simplifies try-finally pattern.

            Console.WriteLine("\n7. Manual try-finally vs using:");

            // Manual approach (what compiler generates)
            FileStream manualStream = null; // FileStream = declare first
            try // Try-finally block
            {
                manualStream = new FileStream("manual.txt", FileMode.Create); // Create file
                byte[] manualData = System.Text.Encoding.UTF8.GetBytes("Manual"); // byte[] = data bytes
                manualStream.Write(manualData, 0, manualData.Length); // Write data
                Console.WriteLine("   Manual write done"); // Output: Manual write done
            }
            finally // Finally block ensures cleanup
            {
                if (manualStream != null) // Check if created
                {
                    manualStream.Dispose(); // Manually dispose
                    Console.WriteLine("   Manual cleanup done"); // Output: Manual cleanup done
                }
            }

            // Using approach (equivalent, cleaner)
            using (var cleanStream = new FileStream("clean.txt", FileMode.Create)) // using = auto-cleanup
            {
                byte[] cleanData = System.Text.Encoding.UTF8.GetBytes("Clean"); // byte[] = data bytes
                cleanStream.Write(cleanData, 0, cleanData.Length); // Write data
                Console.WriteLine("   Using write done"); // Output: Using write done
            } // Dispose called automatically

            // ── EXAMPLE 8: StringReader Example ───────────────────────
            // Common use case: reading text from strings.

            Console.WriteLine("\n8. StringReader with using:");

            // StringReader wraps string as TextReader
            using (StringReader reader = new StringReader("Line 1\nLine 2\nLine 3")) // using = auto-cleanup
            {
                string line = null; // string = line buffer
                while ((line = reader.ReadLine()) != null) // Read while lines exist
                {
                    Console.WriteLine($"   Read: {line}"); // Output: Read: Line 1, etc.
                }
            } // Reader disposed

            // ── REAL-WORLD EXAMPLE: Database Operations ───────────────
            Console.WriteLine("\n9. Real-world: Database operations:");

            // Using with database connection (simulated)
            using (var dbConnection = new DbConnection("Server=localhost;Database=Test")) // using = auto-cleanup
            {
                dbConnection.Open(); // Open connection
                Console.WriteLine($"   DB connected: {dbConnection.IsOpen}"); // Output: DB connected: True

                var result = dbConnection.ExecuteQuery("SELECT * FROM Users"); // Execute query
                Console.WriteLine($"   Query returned {result} rows"); // Output: Query returned [n] rows
            } // Connection properly closed

            Console.WriteLine("\n=== Using Statement Demo Complete ===");
        }
    }

    /// <summary>
    /// Helper class to track resource lifecycle.
    /// </summary>
    class ResourceTracker : IDisposable
    {
        private readonly string _name; // string = resource name

        public ResourceTracker(string name) // Constructor
        {
            _name = name; // Set name
            Console.WriteLine($"   {_name}: Created"); // Output: [name]: Created
        }

        public void DoSomething() // Use resource
        {
            Console.WriteLine($"   {_name}: Doing work"); // Output: [name]: Doing work
        }

        public void Dispose() // IDisposable implementation
        {
            Console.WriteLine($"   {_name}: Disposed"); // Output: [name]: Disposed
        }
    }

    /// <summary>
    /// Resource that might throw during use.
    /// Demonstrates cleanup on exception.
    /// </summary>
    class RiskyResource : IDisposable
    {
        public RiskyResource() // Constructor
        {
            Console.WriteLine("   RiskyResource: Acquired"); // Output: RiskyResource: Acquired
        }

        public void DoSomething() // Use resource
        {
            Console.WriteLine("   RiskyResource: Doing work"); // Output: RiskyResource: Doing work
        }

        public void Dispose() // IDisposable implementation
        {
            Console.WriteLine("   RiskyResource: Disposed"); // Output: RiskyResource: Disposed
        }
    }

    /// <summary>
    /// Simulated database connection for example.
    /// </summary>
    class DbConnection : IDisposable
    {
        private readonly string _connectionString; // string = DB connection
        private bool _isOpen = false; // bool = open flag

        public bool IsOpen => _isOpen; // Property getter

        public DbConnection(string connectionString) // Constructor
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public void Open() // Open connection
        {
            _isOpen = true; // Set open
            Console.WriteLine("   DbConnection: Opened"); // Output: DbConnection: Opened
        }

        public int ExecuteQuery(string query) // Execute query
        {
            if (!_isOpen) // Check if open
                throw new InvalidOperationException("Connection not open"); // Throw if not open

            // Simulate query execution (would be actual DB call)
            Console.WriteLine($"   DbConnection: Executing '{query}'"); // Output: DbConnection: Executing '[query]'
            return 5; // Return row count
        }

        public void Dispose() // IDisposable implementation
        {
            if (_isOpen) // Check if open
            {
                _isOpen = false; // Close connection
                Console.WriteLine("   DbConnection: Closed"); // Output: DbConnection: Closed
            }

            GC.SuppressFinalize(this); // Prevent finalization
        }

        ~DbConnection() // Finalizer
        {
            Dispose(); // Cleanup
        }
    }
}