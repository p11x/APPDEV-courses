/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : Object Pooling - Real-World Examples
 * FILE      : 04_ObjectPooling_RealWorld.cs
 * PURPOSE   : Practical real-world examples using pooling
 *            in network, XML, and processing scenarios
 * ============================================================
 */

using System; // System namespace for Console, basic types
using System.Buffers; // For ObjectPool<T>, ArrayPool<T>
using System.Text; // For Encoding

namespace CSharp_MasterGuide._08_MemoryManagement._04_ObjectPooling
{
    /// <summary>
    /// Real-world examples of object pooling
    /// in application scenarios.
    /// </summary>
    class ObjectPooling_RealWorld
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // REAL-WORLD SCENARIOS ───────────────────────────────────────
            // ═══════════════════════════════════════════════════════════
            // Pooling is essential in high-throughput:
            // - Web servers (request buffers)
            // - Database connections
            // - File processing
            // - Cache systems
            // - Network I/O
            //
            // Using pooling correctly reduces:
            // - GC pressure
            // - Memory allocations
            // - Latency spikes

            Console.WriteLine("=== Object Pooling Real-World ===\n");

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 1: HTTP Server Request Buffer ──────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== Scenario 1: HTTP Server ===\n");

            Console.WriteLine("1.1. Request buffer management:");

            var requestPool = ArrayPool<byte>.Shared; // ArrayPool = shared
            var responsePool = ArrayPool<byte>.Shared; // ArrayPool for response

            // Process multiple requests
            string[] requests = {
                "GET /api/users HTTP/1.1",
                "POST /api/data HTTP/1.1", 
                "GET /health HTTP/1.1"
            };

            foreach (var request in requests) // string = loop
            {
                // Rent buffers
                byte[] requestBuffer = requestPool.Rent(4096); // byte[] = request
                byte[] responseBuffer = responsePool.Rent(4096); // byte[] = response

                // Copy request
                byte[] requestBytes = Encoding.UTF8.GetBytes(request); // byte[] = bytes
                Buffer.BlockCopy(requestBytes, 0, requestBuffer, 0, requestBytes.Length); // Copy

                // Process request
                Console.WriteLine($"   Processing: {request}"); // Output: Processing: [request]

                // Generate response
                string response = "HTTP/1.1 200 OK"; // string = response
                byte[] responseBytes = Encoding.UTF8.GetBytes(response); // byte[] = bytes
                Buffer.BlockCopy(responseBytes, 0, responseBuffer, 0, responseBytes.Length); // Copy

                // Return buffers
                requestPool.Return(requestBuffer, true); // Return request
                responsePool.Return(responseBuffer, true); // Return response
            }

            Console.WriteLine("   Requests processed"); // Output: Requests processed

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 2: XML/JSON Processing ────────────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 2: XML/JSON Processing ===\n");

            Console.WriteLine("2.1. JSON serialization:");

            // Create JSON pools
            var jsonCharPool = ArrayPool<char>.Shared; // ArrayPool<char> for JSON
            var jsonBytePool = ArrayPool<byte>.Shared; // ArrayPool<byte> for bytes

            // Process objects
            var people = new[] {
                new Person("Alice", 30),
                new Person("Bob", 25),
                new Person("Charlie", 35)
            };

            foreach (var person in people) // Person = loop
            {
                // Serialize to pooled buffer
                string json = $"{{\"name\":\"{person.Name}\",\"age\":{person.Age}}}"; // string = JSON

                // Encode to pooled byte buffer
                byte[] encodeBuffer = jsonBytePool.Rent(256); // byte[] = buffer
                byte[] jsonBytes = Encoding.UTF8.GetBytes(json); // byte[] = bytes
                Buffer.BlockCopy(jsonBytes, 0, encodeBuffer, 0, jsonBytes.Length); // Copy

                // Process
                Console.WriteLine($"   Serialized: {json}"); // Output: Serialized: [json]

                // Return buffer
                jsonBytePool.Return(encodeBuffer, true); // Return
            }

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 3: File Copy Operations ──────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 3: File Copy ===\n");

            Console.WriteLine("3.1. Buffered file copy:");

            var copyPool = ArrayPool<byte>.Shared; // ArrayPool = buffer pool

            // Simulate copy
            byte[] sourceData = Encoding.UTF8.GetBytes("Simulated file content with lots of data..."); // byte[] = source
            byte[] destData = new byte[sourceData.Length]; // byte[] = destination

            // Copy in chunks
            int chunkSize = 16; // int = chunk size
            int totalCopied = 0; // int = total

            while (totalCopied < sourceData.Length) // While more data
            {
                int toCopy = Math.Min(chunkSize, sourceData.Length - totalCopied); // int = amount
                
                Buffer.BlockCopy(sourceData, totalCopied, destData, totalCopied, toCopy); // Copy
                totalCopied += toCopy; // Increment
            }

            string result = Encoding.UTF8.GetString(destData); // string = result
            Console.WriteLine($"   Copied: {totalCopied} bytes"); // Output: Copied: [n] bytes
            Console.WriteLine($"   Content: {result}"); // Output: Content: [content]

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 4: StringBuilder Pool ──────────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 4: String Building ===\n");

            Console.WriteLine("4.1. Dynamic string building:");

            var sbPool = ObjectPool<StringBuilderExt>.Create(
                () => new StringBuilderExt(), // Factory
                10 // MaxRetained
            );

            // Build multiple strings
            string[] parts = { "Part1", "Part2", "Part3" }; // string[] = parts

            foreach (var part in parts) // string = loop
            {
                using (var sb = sbPool.Get()) // using = get pooled
                {
                    // Append parts
                    sb.Append("Header: "); // Append
                    sb.Append(part); // Append
                    sb.AppendLine(" - Footer"); // Append line

                    Console.WriteLine($"   Built: {sb.ToString().Trim()}"); // Output: Built: [string]
                } // Return to pool
            }

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 5: Cache Pool ───────────────────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 5: Cache Pool ===\n");

            Console.WriteLine("5.1. Data caching:");

            var cachePool = ObjectPool<CacheEntry>.Create(
                () => new CacheEntry(), // Factory
                100 // Max entries
            );

            // Simulate cache operations
            var cache1 = cachePool.Rent(); // Rent
            cache1.Set("key1", "value1"); // Set cache
            Console.WriteLine($"   Cached: {cache1.Key} = {cache1.Value}"); // Output: Cached: key1 = value1
            cachePool.Return(cache1); // Return

            var cache2 = cachePool.Rent(); // Rent
            cache2.Set("key2", "value2"); // Set
            Console.WriteLine($"   Cached: {cache2.Key} = {cache2.Value}"); // Output: Cached: key2 = value2
            cachePool.Return(cache2); // Return

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 6: Stream Buffer Pool ───────────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 6: Stream Processing ===\n");

            Console.WriteLine("6.1. Stream buffer:");

            var streamPool = ArrayPool<byte>.Shared; // ArrayPool

            // Simulate stream read
            string testData = "Streamed data here"; // string = test data

            byte[] streamBuffer = streamPool.Rent(1024); // byte[] = buffer

            // Write to buffer
            byte[] dataBytes = Encoding.UTF8.GetBytes(testData); // byte[] = bytes
            Buffer.BlockCopy(dataBytes, 0, streamBuffer, 0, dataBytes.Length); // Copy

            // Read from buffer
            string readData = Encoding.UTF8.GetString(streamBuffer, 0, dataBytes.Length); // string = read
            Console.WriteLine($"   Read: {readData}"); // Output: Read: Streamed data here

            // Return buffer
            streamPool.Return(streamBuffer, true); // Return

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 7: Protocol Buffer ──────────────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 7: Protocol Buffers ===\n");

            Console.WriteLine("7.1. Binary protocol:");

            var protocolPool = ArrayPool<byte>.Shared; // ArrayPool

            // Simulate protocol message
            byte[] protoBuffer = protocolPool.Rent(512); // byte[] = buffer
            int offset = 0; // int = position

            // Write fields
            short length = 10; // short = length
            byte type = 1; // byte = type
            int id = 12345; // int = ID

            // Copy field 1
            byte[] lenBytes = BitConverter.GetBytes(length); // byte[] = bytes
            Buffer.BlockCopy(lenBytes, 0, protoBuffer, offset, lenBytes.Length); // Copy
            offset += lenBytes.Length; // Increment

            // Copy field 2
            protoBuffer[offset] = type; // byte = type
            offset++; // Increment

            // Copy field 3
            byte[] idBytes = BitConverter.GetBytes(id); // byte[] = bytes
            Buffer.BlockCopy(idBytes, 0, protoBuffer, offset, idBytes.Length); // Copy
            offset += idBytes.Length; // Increment

            Console.WriteLine($"   Wrote {offset} bytes"); // Output: Wrote [n] bytes

            // Parse buffer
            offset = 0; // Reset
            short parsedLen = BitConverter.ToInt16(protoBuffer, offset); // short = parse
            offset += 2; // Increment

            byte parsedType = protoBuffer[offset]; // byte = parse
            offset++; // Increment

            int parsedId = BitConverter.ToInt32(protoBuffer, offset); // int = parse

            Console.WriteLine($"   Parsed: type={parsedType}, id={parsedId}"); // Output: Parsed: type=1, id=12345

            // Return
            protocolPool.Return(protoBuffer, true); // Return

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 8: Search Results Pool ───────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 8: Search Results ===\n");

            Console.WriteLine("8.1. Result buffer:");

            var resultPool = ObjectPool<ResultBuffer>.Create(
                () => new ResultBuffer(), // Factory
                50 // MaxRetained
            );

            // Process search results
            string[] queries = { "search1", "search2", "search3" }; // string[] = queries

            foreach (var query in queries) // string = loop
            {
                using (var results = resultPool.Get()) // using = get
                {
                    // Add results
                    results.Add($"Result for {query} #1"); // Add
                    results.Add($"Result for {query} #2"); // Add

                    Console.WriteLine($"   Query '{query}': {results.Count} results"); // Output: Query 'query': [n] results
                } // Return to pool
            }

            Console.WriteLine("\n=== All Real-World Examples Complete ===");
        }
    }

    /// <summary>
    /// Person for JSON example.
    /// </summary>
    class Person
    {
        public string Name { get; } // string = name
        public int Age { get; } // int = age

        public Person(string name, int age) // Constructor
        {
            Name = name; // Set name
            Age = age; // Set age
        }
    }

    /// <summary>
    /// Extended StringBuilder for pooling.
    /// </summary>
    class StringBuilderExt
    {
        private readonly System.Text.StringBuilder _sb = new System.Text.StringBuilder(); // StringBuilder

        public void Append(string text) // Append text
        {
            _sb.Append(text); // Append
        }

        public void AppendLine(string text) // Append line
        {
            _sb.AppendLine(text); // Append line
        }

        public override string ToString() // Get string
        {
            return _sb.ToString(); // string = return
        }

        public void Clear() // Clear
        {
            _sb.Clear(); // Clear
        }
    }

    /// <summary>
    /// Cache entry for pooling.
    /// </summary>
    class CacheEntry
    {
        public string Key { get; private set; } = string.Empty; // string = key
        public string Value { get; private set; } = string.Empty; // string = value

        public void Set(string key, string value) // Set entry
        {
            Key = key; // Set key
            Value = value; // Set value
        }

        public void Clear() // Clear entry
        {
            Key = string.Empty; // Clear key
            Value = string.Empty; // Clear value
        }
    }

    /// <summary>
    /// Result buffer for search results.
    /// </summary>
    class ResultBuffer
    {
        private readonly System.Collections.Generic.List<string> _results; // List<string>

        public ResultBuffer() // Constructor
        {
            _results = new System.Collections.Generic.List<string>(); // Create list
        }

        public void Add(string result) // Add result
        {
            _results.Add(result); // Add
        }

        public int Count => _results.Count; // Property

        public void Clear() // Clear results
        {
            _results.Clear(); // Clear
        }
    }
}