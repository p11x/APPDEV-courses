/*
 * ============================================================
 * TOPIC     : Fundamentals - Strings
 * SUBTOPIC  : StringBuilder - Advanced Features
 * FILE      : StringBuilderClass_Part2.cs
 * PURPOSE   : Advanced StringBuilder features including spans, 
 *            performance optimization, and thread safety
 * ============================================================
 */

using System; // Core System namespace
using System.Text; // Required for StringBuilder

namespace CSharp_MasterGuide._01_Fundamentals._07_Strings
{
    class StringBuilderClass_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: StringBuilder and Span Operations
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: CopyTo - Export to char array ──────────────
            var sb = new StringBuilder("Hello World");
            char[] buffer = new char[20];
            
            // Copy entire content to buffer starting at index 0
            sb.CopyTo(0, buffer, 0, sb.Length);
            
            Console.WriteLine($"Copied: '{new string(buffer, 0, sb.Length)}'");
            // Output: Copied: 'Hello World'

            // Copy specific range
            var sb2 = new StringBuilder("0123456789");
            char[] smallBuffer = new char[5];
            sb2.CopyTo(2, smallBuffer, 0, 3); // Copy "234" to buffer
            Console.WriteLine($"Partial copy: '{new string(smallBuffer)}'");
            // Output: Partial copy: '234  '

            // ── EXAMPLE 2: AsSpan - Get Span without allocation ───────
            // Get ReadOnlySpan<char> without copying (C# 7.2+)
            var builder = new StringBuilder("Test String");
            ReadOnlySpan<char> span = builder.AsSpan();
            
            Console.WriteLine($"Span length: {span.Length}"); // Output: 11
            Console.WriteLine($"Span slice: '{span.Slice(5).ToString()}'"); // Output: String

            // Useful for fast operations without ToString() allocation
            bool startsWith = span.StartsWith("Test".AsSpan());
            Console.WriteLine($"Starts with 'Test': {startsWith}"); // True

            // ── REAL-WORLD EXAMPLE: Parse StringBuilder content ───────
            var data = new StringBuilder("ERROR|2024-01-15|ConnectionTimeout|192.168.1.1");
            ReadOnlySpan<char> dataSpan = data.AsSpan();
            
            // Find segments without allocating new strings
            int pipe1 = dataSpan.IndexOf('|');
            int pipe2 = dataSpan.Slice(pipe1 + 1).IndexOf('|') + pipe1 + 1;
            int pipe3 = dataSpan.Slice(pipe2 + 1).IndexOf('|') + pipe2 + 1;
            
            ReadOnlySpan<char> level = dataSpan.Slice(0, pipe1);
            ReadOnlySpan<char> timestamp = dataSpan.Slice(pipe1 + 1, pipe2 - pipe1 - 1);
            ReadOnlySpan<char> message = dataSpan.Slice(pipe2 + 1, pipe3 - pipe2 - 1);
            ReadOnlySpan<char> ip = dataSpan.Slice(pipe3 + 1);
            
            Console.WriteLine($"Level: {level}, Time: {timestamp}");
            Console.WriteLine($"Message: {message}, IP: {ip}");
            // Output: Level: ERROR, Time: 2024-01-15
            // Output: Message: ConnectionTimeout, IP: 192.168.1.1

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Working with StringBuilder in Methods
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Passing StringBuilder to methods ──────────
            var messageBuilder = new StringBuilder();
            
            // Build header
            BuildHeader(messageBuilder, "User Report");
            // Add content
            messageBuilder.AppendLine("Name: John Doe");
            messageBuilder.AppendLine("Email: john@example.com");
            // Build footer
            BuildFooter(messageBuilder);
            
            Console.WriteLine(messageBuilder.ToString());
            
            // ── EXAMPLE 2: StringBuilder as return type ───────────────
            // Methods can return StringBuilder for chaining
            var result = CreateGreeting("Alice", "Morning")
                .AppendLine("Welcome to our service!")
                .AppendLine("Have a great day!");
            
            Console.WriteLine(result.ToString());

            // ── REAL-WORLD EXAMPLE: Builder pattern implementation ────
            var response = new HttpResponseBuilder()
                .SetStatus(200)
                .SetHeader("Content-Type", "application/json")
                .SetBody(@"{""message"":""Success""}")
                .Build();
            
            Console.WriteLine(response);

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: StringBuilder Performance Optimization
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Capacity prediction ───────────────────────
            // When you know approximate final size, pre-allocate
            
            // Bad: starts small, reallocates many times
            var inefficient = new StringBuilder();
            for (int i = 0; i < 1000; i++)
            {
                inefficient.Append(i);
            }
            
            // Good: pre-allocate based on known size
            var efficient = new StringBuilder(4000); // ~4 chars per number avg
            for (int i = 0; i < 1000; i++)
            {
                efficient.Append(i);
            }
            
            Console.WriteLine($"Inefficient capacity: {inefficient.Capacity}");
            Console.WriteLine($"Efficient capacity: {efficient.Capacity}");

            // ── EXAMPLE 2: Clear and reuse ─────────────────────────────
            // For repeated builds, clear and reuse instead of new StringBuilder
            var reusableBuilder = new StringBuilder(1000);
            
            for (int batch = 0; batch < 3; batch++)
            {
                reusableBuilder.Clear();
                reusableBuilder.AppendLine($"Batch {batch + 1} Report");
                
                for (int i = 0; i < 10; i++)
                {
                    reusableBuilder.AppendLine($"  Item {i}: Data");
                }
                
                Console.WriteLine(reusableBuilder.ToString());
            }

            // ── EXAMPLE 3: Using StringBuilder in recursion ────────────
            // Build result through recursive calls
            var treeBuilder = new StringBuilder();
            BuildTree(treeBuilder, 0, 3);
            
            Console.WriteLine(treeBuilder.ToString());
            
            // ── REAL-WORLD EXAMPLE: Large JSON building ────────────────
            // Pre-allocate for expected JSON size
            int itemCount = 100;
            int estimatedSize = itemCount * 100; // Rough estimate
            var json = new StringBuilder(estimatedSize);
            
            json.AppendLine("[");
            
            for (int i = 0; i < itemCount; i++)
            {
                if (i > 0) json.AppendLine(",");
                
                json.Append("  {")
                    .Append($"\"id\":{i},")
                    .Append("\"name\":\"Item " + i + "\",")
                    .Append("\"value\":" + (i * 10.5).ToString("F2") + "}")
                    ;
            }
            
            json.AppendLine("]");
            Console.WriteLine($"JSON length: {json.Length} chars");

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: StringBuilder Edge Cases and Gotchas
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: StringBuilder is mutable ────────────────────
            // Changes to StringBuilder affect all references
            var sb1 = new StringBuilder("Original");
            var sb2 = sb1; // Both reference same object
            
            sb1.Append(" Modified");
            Console.WriteLine($"sb1: {sb1}, sb2: {sb2}");
            // Output: sb1: Original Modified, sb2: Original Modified (SAME object!)

            // To get copy, need to create new StringBuilder
            var sb3 = new StringBuilder(sb1.ToString()); // Clone
            sb3.Append(" Extra");
            Console.WriteLine($"sb1: {sb1}, sb3: {sb3}");
            // Output: sb1: Original Modified, sb3: Original Modified Extra

            // ── EXAMPLE 2: ToString() creates new string ───────────────
            // Each ToString() call creates new string - don't call in loops
            var builder = new StringBuilder();
            builder.Append("Test");
            
            string str1 = builder.ToString();
            string str2 = builder.ToString();
            
            Console.WriteLine($"Same object: {ReferenceEquals(str1, str2)}"); // False
            
            // For repeated access to same string, store the result
            string cached = builder.ToString(); // Use this multiple times
            Console.WriteLine(cached); // No new allocation each time

            // ── EXAMPLE 3: Length vs Capacity ───────────────────────────
            // Length = actual content length
            // Capacity = allocated buffer size
            var sb = new StringBuilder(100);
            
            sb.Append("Hi"); // Length = 2, Capacity = 100
            Console.WriteLine($"Length: {sb.Length}, Capacity: {sb.Capacity}");
            // Output: Length: 2, Capacity: 100
            
            sb.Length = 50; // Can set length shorter - truncates
            Console.WriteLine($"After truncate - Length: {sb.Length}");
            // Output: After truncate - Length: 50 (content is gone)

            sb.Length = 0; // Clear content (faster than Clear() in some cases)
            Console.WriteLine($"After clear - Length: {sb.Length}, Capacity: {sb.Capacity}");
            // Output: After clear - Length: 0, Capacity: 100

            // ── REAL-WORLD EXAMPLE: Truncate and reuse ─────────────────
            var logBuffer = new StringBuilder(1000);
            
            void WriteLog(string level, string message)
            {
                // Always keep buffer at reasonable size
                if (logBuffer.Length > 800) // Too large, truncate
                {
                    logBuffer.Length = 500; // Keep last 500 chars
                }
                
                logBuffer.AppendLine($"[{level}] {message}");
            }
            
            for (int i = 0; i < 200; i++)
            {
                WriteLog("INFO", $"Log entry number {i}");
            }
            
            Console.WriteLine($"Final buffer: {logBuffer.Length} chars");

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: StringBuilder in Async and Parallel Scenarios
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: StringBuilder and async methods ────────────
            // StringBuilder CAN be used in async but handle carefully
            
            async Task<string> BuildAsyncReport()
            {
                var report = new StringBuilder();
                
                // Simulate async data fetching
                var data1 = await Task.FromResult("Data Set 1");
                var data2 = await Task.FromResult("Data Set 2");
                
                report.AppendLine("=== Async Report ===");
                report.AppendLine(data1);
                report.AppendLine(data2);
                
                return report.ToString();
            }
            
            // Run and get result
            var reportTask = BuildAsyncReport();
            Console.WriteLine(reportTask.Result);

            // ── EXAMPLE 2: Thread safety note ──────────────────────────
            // StringBuilder is NOT thread-safe
            // DO NOT share single StringBuilder across threads
            
            // Wrong - shared mutable state:
            // var shared = new StringBuilder();
            // Parallel.For(0, 100, i => shared.Append(i)); // Race conditions!
            
            // Correct - each thread has its own:
            var results = new System.Collections.Concurrent.ConcurrentBag<string>();
            
            System.Threading.Tasks.Parallel.For(0, 10, i =>
            {
                var local = new StringBuilder();
                local.AppendLine($"Thread {i} result");
                for (int j = 0; j < 5; j++)
                {
                    local.AppendLine($"  Item {j}");
                }
                results.Add(local.ToString());
            });
            
            foreach (var r in results)
            {
                Console.WriteLine(r);
            }

            // ── REAL-WORLD EXAMPLE: Parallel log aggregation ───────────
            var logEntries = new System.Collections.Concurrent.ConcurrentBag<string>();
            
            void ProcessBatch(string batchId, int count)
            {
                var batchLog = new StringBuilder();
                batchLog.AppendLine($"Batch: {batchId}");
                
                for (int i = 0; i < count; i++)
                {
                    batchLog.AppendLine($"  Processing item {i}");
                }
                
                logEntries.Add(batchLog.ToString());
            }
            
            // Simulate parallel processing
            System.Threading.Tasks.Parallel.Invoke(
                () => ProcessBatch("A", 10),
                () => ProcessBatch("B", 15),
                () => ProcessBatch("C", 8)
            );
            
            // Combine all logs
            var finalLog = new StringBuilder();
            foreach (var entry in logEntries)
            {
                finalLog.Append(entry);
            }
            
            Console.WriteLine($"Total log size: {finalLog.Length} chars");

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Advanced StringBuilder Patterns
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Indentation helpers ─────────────────────────
            // Build indented text efficiently
            var indented = new StringBuilder();
            
            indented.AppendLine("Root");
            indented.AppendLine("{"); // Indent open
            
            // Helper for nested indentation
            void AddIndented(string text, int indentLevel)
            {
                indented.AppendLine(new string(' ', indentLevel * 2) + text);
            }
            
            AddIndented("Child 1", 1);
            AddIndented("{", 2);
            AddIndented("Grandchild", 3);
            AddIndented("}", 2);
            AddIndented("Child 2", 1);
            indented.AppendLine("}");
            
            Console.WriteLine(indented.ToString());

            // ── EXAMPLE 2: Conditional content building ───────────────
            var options = new { ShowHeader = true, ShowFooter = true, MaxItems = 5 };
            var items = new[] { "A", "B", "C", "D", "E", "F" };
            
            var output = new StringBuilder();
            
            if (options.ShowHeader)
                output.AppendLine("=== Items ===");
            
            int count = 0;
            foreach (var item in items)
            {
                if (count >= options.MaxItems) break;
                output.AppendLine($"- {item}");
                count++;
            }
            
            if (items.Length > options.MaxItems)
                output.AppendLine($"... and {items.Length - options.MaxItems} more");
            
            if (options.ShowFooter)
                output.AppendLine($"=== Total: {Math.Min(items.Length, options.MaxItems)} ===");
            
            Console.WriteLine(output.ToString());

            // ── EXAMPLE 3: StringBuilder with StringReader ─────────────
            // Can use StringReader to parse StringBuilder content
            var sb = new StringBuilder();
            sb.AppendLine("Line 1");
            sb.AppendLine("Line 2");
            sb.AppendLine("Line 3");
            
            using var reader = new System.IO.StringReader(sb.ToString());
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine($"Read: {line}");
            }

            Console.WriteLine("\n=== StringBuilder Part 2 Complete ===");
        }

        // Helper methods for examples
        static void BuildHeader(StringBuilder sb, string title)
        {
            sb.AppendLine(new string('=', 50));
            sb.AppendLine($"  {title}");
            sb.AppendLine(new string('=', 50));
            sb.AppendLine();
        }

        static void BuildFooter(StringBuilder sb)
        {
            sb.AppendLine();
            sb.AppendLine(new string('-', 50));
            sb.AppendLine("Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
        }

        static StringBuilder CreateGreeting(string name, string timeOfDay)
        {
            return new StringBuilder()
                .AppendLine($"Good {timeOfDay}, {name}!");
        }

        static void BuildTree(StringBuilder sb, int level, int maxLevel)
        {
            if (level >= maxLevel) return;
            
            sb.AppendLine(new string(' ', level * 2) + $"Level {level}");
            
            for (int i = 0; i < 2; i++)
            {
                BuildTree(sb, level + 1, maxLevel);
            }
        }
    }

    // Builder pattern example classes
    class HttpResponseBuilder
    {
        private readonly StringBuilder _sb = new StringBuilder();
        
        public HttpResponseBuilder SetStatus(int code)
        {
            _sb.AppendLine($"HTTP/1.1 {code} {(code == 200 ? "OK" : "Error")}");
            return this;
        }
        
        public HttpResponseBuilder SetHeader(string name, string value)
        {
            _sb.AppendLine($"{name}: {value}");
            return this;
        }
        
        public HttpResponseBuilder SetBody(string body)
        {
            _sb.AppendLine();
            _sb.Append(body);
            return this;
        }
        
        public string Build() => _sb.ToString();
    }
}