/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : Garbage Collection - Real-World Examples
 * FILE      : 05_GC_RealWorld.cs
 * PURPOSE   : Demonstrates practical GC patterns for production
 *            applications, memory monitoring, and optimization
 * ============================================================
 */

using System; // System namespace for Console, basic types
using System.Collections.Generic; // Collections namespace

namespace CSharp_MasterGuide._08_MemoryManagement._01_GarbageCollection
{
    /// <summary>
    /// Real-world GC patterns and memory management in production applications.
    /// </summary>
    class GC_RealWorld
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // REAL-WORLD: Memory Monitoring ─────────────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== GC Real-World Examples ===\n");

            // ── SCENARIO 1: Web Server Memory Management ─────────────────
            // In web servers, memory management is critical for performance.

            var webServer = new WebServer(); // Create web server simulation
            
            webServer.Start(); // Start server
            
            // Simulate handling requests
            for (int i = 0; i < 100; i++) // int = loop counter
            {
                webServer.HandleRequest($"Request_{i}"); // Handle each request
            }
            
            webServer.Stop(); // Stop server
            
            Console.WriteLine($"Peak memory: {webServer.PeakMemoryUsage:N0} bytes");
            // Output: Peak memory: [varies based on requests]

            // ── SCENARIO 2: Image Processing Pipeline ────────────────────
            // Image processing often involves large objects that need careful handling.

            Console.WriteLine("\n--- Image Processing ---");
            
            var processor = new ImageProcessor(); // Create processor
            
            // Process images
            var images = new[] { "photo1.jpg", "photo2.jpg", "photo3.jpg" }; // string array
            
            foreach (var image in images) // Iterate each image
            {
                processor.ProcessImage(image); // Process single image
            }
            
            Console.WriteLine($"Processed {images.Length} images");
            // Output: Processed 3 images

            // ── SCENARIO 3: Database Connection Pool ───────────────────
            // Connection pooling manages database connections efficiently.

            Console.WriteLine("\n--- Connection Pool ---");
            
            var pool = new ConnectionPool(); // Create connection pool
            
            // Get connections
            var conn1 = pool.Acquire(); // Acquire first connection
            var conn2 = pool.Acquire(); // Acquire second connection
            
            Console.WriteLine($"Active connections: {pool.ActiveCount}"); // Output: Active connections: 2
            
            pool.Release(conn1); // Release first connection
            pool.Release(conn2); // Release second connection
            
            Console.WriteLine($"After release: {pool.ActiveCount}"); // Output: After release: 0

            // ── SCENARIO 4: Cache with Memory Limits ─────────────────────
            // Caches need to balance memory usage with hit rates.

            Console.WriteLine("\n--- Memory-Bounded Cache ---");
            
            var cache = new MemoryBoundedCache(); // Create cache with limits
            
            // Add items to cache
            for (int i = 0; i < 1000; i++) // Add 1000 items
            {
                cache.Set($"key:{i}", $"value:{i}"); // Set each key-value pair
            }
            
            Console.WriteLine($"Cache items: {cache.Count}"); // Output: Cache items: [less than 1000]
            
            // Try to access some items
            var hit = cache.Get("key:500"); // Try to get item
            Console.WriteLine($"Cache hit: {(hit != null ? "Yes" : "No")}"); // Output: Cache hit: Yes or No

            // ── SCENARIO 5: Batch Processing with Memory ─────────────────
            // Batch processors must manage memory to avoid OutOfMemory errors.

            Console.WriteLine("\n--- Batch Processing ---");
            
            var batchProcessor = new BatchProcessor(); // Create processor
            
            // Process 10,000 items in batches of 1000
            batchProcessor.ProcessLargeDataset(10000, batchSize: 1000);
            // Output:
            // Processing batch 0...
            // Processing batch 1...
            // ...
            // Processing batch 9...

            // ── SCENARIO 6: Monitoring GC Events ─────────────────────────
            // Production apps often need to monitor GC behavior.

            Console.WriteLine("\n--- GC Monitoring ---");
            
            var monitor = new GCMonitor(); // Create monitor
            monitor.StartMonitoring(); // Start monitoring
            monitor.StopMonitoring(); // Stop monitoring

            Console.WriteLine("\n=== GC Real-World Complete ===");
        }
    }

    /// <summary>
    /// Web server simulation demonstrating memory management.
    /// </summary>
    class WebServer
    {
        // List<Request> = stores active requests in memory
        private readonly List<Request> _activeRequests = new List<Request>();
        
        private long _peakMemory = 0; // long = 64-bit for large values
        private bool _isRunning = false; // bool = true/false flag

        public long PeakMemoryUsage => _peakMemory; // Property to get peak memory

        public void Start() // Start server
        {
            _isRunning = true; // Set running flag
            Console.WriteLine("Web server started"); // Output: Web server started
        }

        public void HandleRequest(string requestId) // Handle single request
        {
            if (!_isRunning) // Check if server is running
                return; // Exit if not running

            // Create request object
            var request = new Request { Id = requestId }; // Create new request
            _activeRequests.Add(request); // Add to active list
            
            // Simulate processing
            var response = ProcessRequest(request); // Process the request
            
            // Track peak memory
            long currentMemory = GC.GetTotalMemory(false); // Get approximate memory
            if (currentMemory > _peakMemory) // Check if new peak
            {
                _peakMemory = currentMemory; // Update peak
            }
            
            // Remove processed request
            _activeRequests.Remove(request); // Remove from active list
        }

        private Response ProcessRequest(Request request) // Process the request
        {
            // Simulate work - in real app, this would do actual processing
            var data = new byte[1000]; // Create 1KB of data
            return new Response { RequestId = request.Id, Status = "OK" }; // Return response
        }

        public void Stop() // Stop server
        {
            _activeRequests.Clear(); // Clear all requests
            _isRunning = false; // Set running flag
            Console.WriteLine("Web server stopped"); // Output: Web server stopped
        }
    }

    /// <summary>
    /// Request class for web server.
    /// </summary>
    class Request
    {
        public string Id { get; set; } // string = request identifier
    }

    /// <summary>
    /// Response class for web server.
    /// </summary>
    class Response
    {
        public string RequestId { get; set; } // string = request ID
        public string Status { get; set; } // string = status code
    }

    /// <summary>
    /// Image processor demonstrating large object handling.
    /// </summary>
    class ImageProcessor
    {
        public void ProcessImage(string filename) // Process single image
        {
            // In real implementation, would load image from disk
            // For demo, simulate processing
            var imageData = new byte[1000000]; // 1MB - large object
            
            // Process image data
            ProcessPixels(imageData); // Process the data
            
            // imageData is now eligible for GC
            Console.WriteLine($"  Processed: {filename}"); // Output: Processed: [filename]
        }

        private void ProcessPixels(byte[] data) // Process image pixels
        {
            // Simulate processing - iterate through data
            for (int i = 0; i < data.Length; i++) // Loop through bytes
            {
                data[i] = (byte)(data[i] ^ 0xFF); // Invert bytes (example)
            }
        }
    }

    /// <summary>
    /// Connection pool demonstrating resource pooling.
    /// </summary>
    class ConnectionPool
    {
        // Queue<Connection> = available connections waiting to be used
        private readonly Queue<Connection> _available = new Queue<Connection>();
        
        private int _maxSize = 10; // int = maximum pool size
        private int _activeCount = 0; // int = currently active connections

        public int ActiveCount => _activeCount; // Property for active count

        public ConnectionPool() // Constructor
        {
            // Pre-create some connections
            for (int i = 0; i < 5; i++) // Create 5 initial connections
            {
                _available.Enqueue(new Connection()); // Add to queue
            }
        }

        public Connection Acquire() // Get connection from pool
        {
            if (_available.Count > 0) // Check if connections available
            {
                _activeCount++; // Increment active count
                return _available.Dequeue(); // Return existing connection
            }
            
            if (_activeCount < _maxSize) // Check if under max limit
            {
                _activeCount++; // Increment active count
                return new Connection(); // Create new connection
            }
            
            // Pool exhausted - wait or throw
            throw new InvalidOperationException("Connection pool exhausted"); // Throw if no connection
        }

        public void Release(Connection conn) // Return connection to pool
        {
            if (_available.Count < _maxSize) // Check if pool has space
            {
                _available.Enqueue(conn); // Return to queue
            }
            
            _activeCount--; // Decrement active count
        }
    }

    /// <summary>
    /// Simulated database connection.
    /// </summary>
    class Connection
    {
        public string Id { get; } = Guid.NewGuid().ToString(); // Guid = unique identifier
    }

    /// <summary>
    /// Cache with memory limits to prevent excessive memory usage.
    /// </summary>
    class MemoryBoundedCache
    {
        // Dictionary<string, byte[]> = key-value storage
        private readonly Dictionary<string, byte[]> _storage = new Dictionary<string, byte[]>();
        
        private const long MaxMemoryBytes = 100000; // 100KB limit
        private long _currentMemory = 0; // Track current usage

        public int Count => _storage.Count; // Property for count

        public void Set(string key, string value) // Add item to cache
        {
            var data = System.Text.Encoding.UTF8.GetBytes(value); // Convert string to bytes
            long itemSize = data.Length + key.Length; // Calculate approximate size
            
            // Check if adding this would exceed limit
            while (_currentMemory + itemSize > MaxMemoryBytes && _storage.Count > 0) // While over limit
            {
                // Remove oldest item
                var oldestKey = _storage.Keys.FirstOrDefault(); // Get first key
                if (oldestKey != null) // Check if key exists
                {
                    _currentMemory -= _storage[oldestKey].Length + oldestKey.Length; // Subtract size
                    _storage.Remove(oldestKey); // Remove from storage
                }
            }
            
            // Add new item
            _storage[key] = data; // Store data
            _currentMemory += itemSize; // Add to memory total
        }

        public string Get(string key) // Retrieve item from cache
        {
            if (_storage.TryGetValue(key, out var data)) // Try to get data
            {
                return System.Text.Encoding.UTF8.GetString(data); // Convert bytes to string
            }
            
            return null; // Return null if not found
        }
    }

    /// <summary>
    /// Batch processor for large datasets.
    /// </summary>
    class BatchProcessor
    {
        public void ProcessLargeDataset(int totalItems, int batchSize) // Process large dataset
        {
            int batchCount = (totalItems + batchSize - 1) / batchSize; // Calculate batches
            
            for (int batch = 0; batch < batchCount; batch++) // Process each batch
            {
                // Create batch data
                var batchData = new List<string>(batchSize); // List for batch data
                
                int start = batch * batchSize; // Calculate start index
                int count = Math.Min(batchSize, totalItems - start); // Items in this batch
                
                for (int i = 0; i < count; i++) // Populate batch
                {
                    batchData.Add($"Item_{start + i}"); // Add item
                }
                
                // Process batch
                ProcessBatch(batchData, batch); // Process this batch
                
                // batchData goes out of scope and is collected
            }
        }

        private void ProcessBatch(List<string> batchData, int batchNumber) // Process single batch
        {
            Console.WriteLine($"Processing batch {batchNumber}..."); // Output: Processing batch [n]...
        }
    }

    /// <summary>
    /// GC monitoring utility for production systems.
    /// </summary>
    class GCMonitor
    {
        private bool _isMonitoring = false; // bool = monitoring flag

        public void StartMonitoring() // Start monitoring GC
        {
            _isMonitoring = true; // Set flag
            Console.WriteLine("GC monitoring started"); // Output: GC monitoring started
            
            // In production, this would set up timers to periodically log GC stats
            var initialMem = GC.GetTotalMemory(false); // Get initial memory
            var gen0Count = GC.CollectionCount(0); // Get Gen 0 count
            
            Console.WriteLine($"Initial memory: {initialMem:N0}, Gen0 collections: {gen0Count}");
            // Output: [varies]
        }

        public void StopMonitoring() // Stop monitoring
        {
            _isMonitoring = false; // Clear flag
            Console.WriteLine("GC monitoring stopped"); // Output: GC monitoring stopped
            
            // Log final stats
            var finalMem = GC.GetTotalMemory(false); // Get final memory
            var gen0Count = GC.CollectionCount(0); // Get Gen 0 count
            var gen1Count = GC.CollectionCount(1); // Get Gen 1 count
            var gen2Count = GC.CollectionCount(2); // Get Gen 2 count
            
            Console.WriteLine($"Final memory: {finalMem:N0}");
            Console.WriteLine($"Gen0: {gen0Count}, Gen1: {gen1Count}, Gen2: {gen2Count}");
            // Output: [varies]
        }
    }
}