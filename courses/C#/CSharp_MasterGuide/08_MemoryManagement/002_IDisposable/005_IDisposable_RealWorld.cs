/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : IDisposable Real-World Examples
 * FILE      : 05_IDisposable_RealWorld.cs
 * PURPOSE   : Practical real-world examples of IDisposable
 *            in database, file handling, graphics, and more
 * ============================================================
 */

using System; // System namespace for Console, basic types
using System.IO; // For stream types

namespace CSharp_MasterGuide._08_MemoryManagement._02_IDisposable
{
    /// <summary>
    /// Real-world examples of IDisposable implementation
    /// in common application scenarios.
    /// </summary>
    class IDisposable_RealWorld
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // REAL-WORLD SCENARIOS ──────────────────────────────────────
            // ═══════════════════════════════════════════════════════════
            // IDisposable is essential in real applications:
            // - Database connections (limited pool)
            // - File handles (OS resources)
            // - Network sockets (limited ports)
            // - Graphics handles (device contexts)
            // - Large memory buffers (pool management)
            //
            // These resources are expensive/limited and must be
            // explicitly released, not relying on GC.

            Console.WriteLine("=== IDisposable Real-World Examples ===\n");

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 1: Database Connection Pool ─────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== Scenario 1: Database Connection Pool ===\n");

            // Acquire connections from pool
            Console.WriteLine("1.1. Creating database manager:");

            using (var dbManager = new DatabaseManager("Server=localhost;Database=Shop")) // using = auto-dispose
            {
                dbManager.Open(); // Open connection
                Console.WriteLine("   Connection opened from pool"); // Output: Connection opened from pool

                // Execute queries
                var orderCount = dbManager.ExecuteScalar("SELECT COUNT(*) FROM Orders"); // Execute query
                Console.WriteLine($"   Order count: {orderCount}"); // Output: Order count: [n]

                var productCount = dbManager.ExecuteScalar("SELECT COUNT(*) FROM Products"); // Execute query
                Console.WriteLine($"   Product count: {productCount}"); // Output: Product count: [n]
            } // Connection returned to pool

            Console.WriteLine("   Connection returned to pool"); // Output: Connection returned to pool

            // ════════════════════════════════════════���══════════════════
            // SCENARIO 2: File Processing with Multiple Files ───────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 2: File Processing ===\n");

            Console.WriteLine("2.1. Processing large file:");

            using (var processor = new FileProcessor()) // using = auto-dispose
            {
                processor.OpenFiles("input.txt", "output.txt"); // Open files
                processor.TransformData(); // Transform data
                Console.WriteLine("   Data transformed"); // Output: Data transformed
            } // All files closed properly

            Console.WriteLine("   All file handles released"); // Output: All file handles released

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 3: Network Socket ────────────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 3: Network Socket ===\n");

            Console.WriteLine("3.1. TCP client connection:");

            using (var client = new TcpClient("localhost", 8080)) // using = auto-dispose
            {
                client.Connect(); // Connect to server
                Console.WriteLine($"   Connected to {client.RemoteEndPoint}"); // Output: Connected to localhost:8080

                client.Send("Hello, Server!"); // Send data
                Console.WriteLine("   Data sent"); // Output: Data sent

                string response = client.Receive(); // Receive response
                Console.WriteLine($"   Received: {response}"); // Output: Received: [response]
            } // Socket closed properly

            Console.WriteLine("   Socket closed"); // Output: Socket closed

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 4: Graphics/Device Context ─────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 4: Graphics Context ===\n");

            Console.WriteLine("4.1. Drawing on device context:");

            using (var dc = new DeviceContext()) // using = auto-dispose
            {
                dc.BeginDrawing(); // Begin drawing
                dc.DrawRectangle(0, 0, 100, 50); // Draw rectangle
                dc.DrawText("Hello Graphics!", 10, 10); // Draw text
                dc.EndDrawing(); // End drawing
                Console.WriteLine("   Graphics drawn"); // Output: Graphics drawn
            } // Device context released

            Console.WriteLine("   Device context released"); // Output: Device context released

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 5: Large Memory Buffer (Pooling) ──────────────
            // ══════════════════════���═��══════════════════════════════════

            Console.WriteLine("\n=== Scenario 5: Memory Buffer Pool ===\n");

            Console.WriteLine("5.1. Buffer pool usage:");

            // Acquire buffer from pool
            using (var buffer = BufferPool.Rent(1024)) // using = auto-return
            {
                // Fill buffer with data
                for (int i = 0; i < 256; i++) // int = loop counter
                {
                    buffer.Data[i * 4] = (byte)(i & 0xFF); // byte = fill buffer
                }

                Console.WriteLine($"   Buffer filled: {buffer.Length} bytes"); // Output: Buffer filled: 1024 bytes

                // Process buffer
                buffer.Process(); // Process data
                Console.WriteLine("   Buffer processed"); // Output: Buffer processed
            } // Returned to pool

            Console.WriteLine("   Buffer returned to pool"); // Output: Buffer returned to pool

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 6: Transaction Scope ────────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 6: Transaction Scope ===\n");

            Console.WriteLine("6.1. Database transaction:");

            using (var scope = new TransactionScope()) // using = auto-dispose
            {
                // Execute operation 1
                scope.Execute("INSERT INTO Accounts (Name) VALUES ('Alice')"); // Insert
                Console.WriteLine("   Inserted into Accounts"); // Output: Inserted into Accounts

                // Execute operation 2
                scope.Execute("INSERT INTO AuditLog (Action) VALUES ('AccountCreated')"); // Insert
                Console.WriteLine("   Inserted into AuditLog"); // Output: Inserted into AuditLog

                // Commit transaction
                scope.Commit(); // Commit all operations
                Console.WriteLine("   Transaction committed"); // Output: Transaction committed
            } // Automatically commits or rollbacks

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 7: HTTP Client ────────────────────────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 7: HTTP Client ===\n");

            Console.WriteLine("7.1. HTTP request:");

            using (var httpClient = new HttpClient()) // using = auto-dispose
            {
                httpClient.BaseAddress = "https://api.example.com"; // Set base URL

                var response = httpClient.Get("/users"); // GET request
                Console.WriteLine($"   Status: {response.StatusCode}"); // Output: Status: 200

                var content = response.Content; // Get content
                Console.WriteLine($"   Content length: {content.Length}"); // Output: Content length: [n]
            } // Connection closed

            Console.WriteLine("   HTTP connection closed"); // Output: HTTP connection closed

            Console.WriteLine("\n=== All Real-World Examples Complete ===");
        }
    }

    /// <summary>
    /// Real-world: Database connection manager.
    /// Manages connection pool and ensures cleanup.
    /// </summary>
    class DatabaseManager : IDisposable
    {
        private readonly string _connectionString; // string = connection string
        private SqlConnection _connection = null; // SqlConnection = actual connection
        private bool _disposed = false; // bool = disposal flag

        public DatabaseManager(string connectionString) // Constructor
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public void Open() // Open connection
        {
            if (_disposed) // Check if disposed
                throw new ObjectDisposedException(nameof(DatabaseManager)); // Throw if disposed

            // Acquire from connection pool (simulated)
            _connection = new SqlConnection(_connectionString); // Create connection
            _connection.Open(); // Open connection
        }

        public int ExecuteScalar(string sql) // Execute query
        {
            if (_disposed || _connection == null) // Check if open
                throw new InvalidOperationException("Connection not open"); // Throw if not

            // Execute query (simulated)
            return new Random().Next(100); // Return random count
        }

        /// <summary>
        /// Dispose releases connection back to pool.
        /// </summary>
        public void Dispose() // IDisposable implementation
        {
            if (!_disposed) // Check if disposed
            {
                if (_connection != null) // Check if connection exists
                {
                    _connection.Close(); // Close connection
                    _connection = null; // Clear reference
                }

                _disposed = true; // Mark disposed
            }

            GC.SuppressFinalize(this); // Prevent finalization
        }

        ~DatabaseManager() // Finalizer (safety net)
        {
            Dispose(); // Cleanup
        }
    }

    /// <summary>
    /// Real-world: File processor managing multiple files.
    /// </summary>
    class FileProcessor : IDisposable
    {
        private FileStream _inputFile = null; // FileStream = input file
        private FileStream _outputFile = null; // FileStream = output file
        private bool _disposed = false; // bool = disposal flag

        public void OpenFiles(string inputPath, string outputPath) // Open files
        {
            _inputFile = new FileStream(inputPath, FileMode.Create); // Open input
            _outputFile = new FileStream(outputPath, FileMode.Create); // Open output

            // Write initial data
            byte[] data = System.Text.Encoding.UTF8.GetBytes("Initial data"); // byte[] = initial data
            _inputFile.Write(data, 0, data.Length); // Write input
        }

        public void TransformData() // Transform data
        {
            if (_disposed) // Check if disposed
                throw new ObjectDisposedException(nameof(FileProcessor)); // Throw if disposed

            // Read input, transform, write output
            _inputFile.Position = 0; // Reset position
            byte[] buffer = new byte[1024]; // byte[] = read buffer
            int bytesRead = _inputFile.Read(buffer, 0, buffer.Length); // Read data

            // Transform (uppercase for demo)
            for (int i = 0; i < bytesRead; i++) // int = loop
            {
                if (buffer[i] >= 97 && buffer[i] <= 122) // Check lowercase
                    buffer[i] = (byte)(buffer[i] - 32); // byte = to uppercase
            }

            _outputFile.Write(buffer, 0, bytesRead); // Write output
        }

        public void Dispose() // IDisposable implementation
        {
            if (!_disposed) // Check if disposed
            {
                // Close all files
                _inputFile?.Dispose(); // Dispose input
                _inputFile = null; // Clear reference

                _outputFile?.Dispose(); // Dispose output
                _outputFile = null; // Clear reference

                _disposed = true; // Mark disposed
            }

            GC.SuppressFinalize(this); // Prevent finalization
        }

        ~FileProcessor() // Finalizer
        {
            Dispose(); // Cleanup
        }
    }

    /// <summary>
    /// Real-world: TCP client for network communication.
    /// </summary>
    class TcpClient : IDisposable
    {
        private Socket _socket = null; // Socket = network socket
        private readonly string _host; // string = remote host
        private readonly int _port; // int = port number
        private bool _disposed = false; // bool = disposal flag

        public string RemoteEndPoint => $"{_host}:{_port}"; // Property getter

        public TcpClient(string host, int port) // Constructor
        {
            _host = host; // Set host
            _port = port; // Set port
        }

        public void Connect() // Connect to server
        {
            if (_disposed) // Check if disposed
                throw new ObjectDisposedException(nameof(TcpClient)); // Throw if disposed

            // Create and connect socket
            _socket = new Socket(); // Create socket
            _socket.Connect(_host, _port); // Connect
        }

        public void Send(string data) // Send data
        {
            if (_socket == null) // Check if connected
                throw new InvalidOperationException("Not connected"); // Throw if not

            // Send data
            Console.WriteLine($"   Sending: {data}"); // Output: Sending: [data]
        }

        public string Receive() // Receive data
        {
            if (_socket == null) // Check if connected
                throw new InvalidOperationException("Not connected"); // Throw if not

            // Return mock response
            return "Hello, Client!"; // string = mock response
        }

        public void Dispose() // IDisposable implementation
        {
            if (!_disposed) // Check if disposed
            {
                if (_socket != null) // Check if socket exists
                {
                    _socket.Close(); // Close socket
                    _socket = null; // Clear reference
                }

                _disposed = true; // Mark disposed
            }

            GC.SuppressFinalize(this); // Prevent finalization
        }

        ~TcpClient() // Finalizer
        {
            Dispose(); // Cleanup
        }
    }

    /// <summary>
    /// Real-world: Device context for graphics.
    /// </summary>
    class DeviceContext : IDisposable
    {
        private bool _disposed = false; // bool = disposal flag
        private bool _drawing = false; // bool = drawing state

        public void BeginDrawing() // Begin drawing
        {
            _drawing = true; // Set drawing
            Console.WriteLine("   Began drawing"); // Output: Began drawing
        }

        public void DrawRectangle(int x, int y, int width, int height) // Draw rectangle
        {
            if (!_drawing) // Check if drawing
                throw new InvalidOperationException("Not in drawing mode"); // Throw if not

            Console.WriteLine($"   Drew rectangle at ({x},{y}) size {width}x{height}"); // Output: Drew rectangle...
        }

        public void DrawText(string text, int x, int y) // Draw text
        {
            if (!_drawing) // Check if drawing
                throw new InvalidOperationException("Not in drawing mode"); // Throw if not

            Console.WriteLine($"   Drew text '{text}' at ({x},{y})"); // Output: Drew text...
        }

        public void EndDrawing() // End drawing
        {
            _drawing = false; // Clear drawing
            Console.WriteLine("   Ended drawing"); // Output: Ended drawing
        }

        public void Dispose() // IDisposable implementation
        {
            if (_drawing) // Check if still drawing
            {
                EndDrawing(); // End drawing first
            }

            _disposed = true; // Mark disposed
            GC.SuppressFinalize(this); // Prevent finalization
        }

        ~DeviceContext() // Finalizer
        {
            Dispose(); // Cleanup
        }
    }

    /// <summary>
    /// Real-world: Buffer pool for large buffers.
    /// </summary>
    class BufferPool
    {
        public static PooledBuffer Rent(int size) // Rent buffer
        {
            return new PooledBuffer(size); // Return pooled buffer
        }
    }

    /// <summary>
    /// Pooled buffer that returns to pool on dispose.
    /// </summary>
    class PooledBuffer : IDisposable
    {
        private readonly byte[] _data; // byte[] = buffer data
        private readonly int _length; // int = buffer length

        public byte[] Data => _data; // Property getter
        public int Length => _length; // Property getter

        public PooledBuffer(int size) // Constructor
        {
            _data = new byte[size]; // Allocate buffer
            _length = size; // Set length
        }

        public void Process() // Process buffer
        {
            // Simulate processing
            long sum = 0; // long = checksum
            for (int i = 0; i < _length; i++) // int = loop
            {
                sum += _data[i]; // byte = add to sum
            }

            Console.WriteLine($"   Processed checksum: {sum}"); // Output: Processed checksum: [n]
        }

        public void Dispose() // IDisposable implementation
        {
            // Return to pool (simulated)
            Console.WriteLine("   Buffer returned to pool"); // Output: Buffer returned to pool
            GC.SuppressFinalize(this); // Prevent finalization
        }

        ~PooledBuffer() // Finalizer
        {
            // Pool would reclaim here in real implementation
            Dispose(); // Cleanup
        }
    }

    /// <summary>
    /// Real-world: Transaction scope for atomic operations.
    /// </summary>
    class TransactionScope : IDisposable
    {
        private readonly List<string> _operations = new List<string>(); // List<string> = queued ops
        private bool _committed = false; // bool = commit flag

        public void Execute(string sql) // Queue operation
        {
            _operations.Add(sql); // Add to queue
            Console.WriteLine($"   Queued: {sql}"); // Output: Queued: [sql]
        }

        public void Commit() // Commit transaction
        {
            // Execute all operations
            _committed = true; // Set committed
            Console.WriteLine("   All operations executed"); // Output: All operations executed
        }

        public void Dispose() // IDisposable implementation
        {
            if (!_committed) // Check if committed
            {
                // Rollback on dispose if not committed
                _operations.Clear(); // Clear operations
                Console.WriteLine("   Transaction rolled back"); // Output: Transaction rolled back
            }

            GC.SuppressFinalize(this); // Prevent finalization
        }

        ~TransactionScope() // Finalizer
        {
            Dispose(); // Cleanup
        }
    }

    /// <summary>
    /// Real-world: HTTP client for web requests.
    /// </summary>
    class HttpClient : IDisposable
    {
        private HttpConnection _connection = null; // HttpConnection = HTTP connection
        private string _baseAddress = string.Empty; // string = base URL
        private bool _disposed = false; // bool = disposal flag

        public string BaseAddress // Property setter
        {
            set { _baseAddress = value; } // Set base address
        }

        public HttpResponse Get(string path) // GET request
        {
            _connection = new HttpConnection(_baseAddress); // Create connection
            _connection.Connect(); // Connect

            return new HttpResponse(200, "{\"users\":[]}"); // Return mock response
        }

        public void Dispose() // IDisposable implementation
        {
            if (!_disposed) // Check if disposed
            {
                if (_connection != null) // Check if connected
                {
                    _connection.Close(); // Close connection
                    _connection = null; // Clear reference
                }

                _disposed = true; // Mark disposed
            }

            GC.SuppressFinalize(this); // Prevent finalization
        }

        ~HttpClient() // Finalizer
        {
            Dispose(); // Cleanup
        }
    }

    // Supporting types
    class SqlConnection
    {
        private readonly string _connString; // string = connection string

        public SqlConnection(string connString) // Constructor
        {
            _connString = connString; // Set connection string
        }

        public void Open() // Open connection
        {
            // Simulate open
        }

        public void Close() // Close connection
        {
            // Simulate close
        }
    }

    class Socket
    {
        public void Connect(string host, int port) // Connect
        {
            // Simulate connect
        }

        public void Close() // Close
        {
            // Simulate close
        }
    }

    class HttpConnection
    {
        private readonly string _baseUrl; // string = base URL

        public HttpConnection(string baseUrl) // Constructor
        {
            _baseUrl = baseUrl; // Set base URL
        }

        public void Connect() // Connect
        {
            // Simulate connect
        }

        public void Close() // Close
        {
            // Simulate close
        }
    }

    class HttpResponse
    {
        private readonly int _statusCode; // int = HTTP status
        private readonly string _content; // string = response content

        public int StatusCode => _statusCode; // Property getter
        public string Content => _content; // Property getter

        public HttpResponse(int statusCode, string content) // Constructor
        {
            _statusCode = statusCode; // Set status code
            _content = content; // Set content
        }
    }

    // Simple List<T> simulation
    class List<T>
    {
        private T[] _items = new T[0]; // T[] = internal array

        public void Add(T item) // Add item
        {
            // Simplified
        }

        public void Clear() // Clear all
        {
            _items = new T[0]; // Reset
        }
    }
}