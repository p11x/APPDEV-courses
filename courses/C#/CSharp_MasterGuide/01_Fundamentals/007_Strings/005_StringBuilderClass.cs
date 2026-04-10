/*
 * ============================================================
 * TOPIC     : Fundamentals - Strings
 * SUBTOPIC  : StringBuilder Class
 * FILE      : StringBuilderClass.cs
 * PURPOSE   : Teaches StringBuilder for efficient string concatenation
 *            and building strings in loops or complex scenarios
 * ============================================================
 */

using System; // Core System namespace
using System.Text; // Required for StringBuilder

namespace CSharp_MasterGuide._01_Fundamentals._07_Strings
{
    class StringBuilderClass
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: StringBuilder Basics
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Creating StringBuilder ─────────────────────
            // Default capacity - starts with 16 characters, grows as needed
            var sb1 = new StringBuilder();
            
            // With initial capacity - better for known size
            var sb2 = new StringBuilder(100); // Pre-allocates 100 chars
            
            // With initial string value
            var sb3 = new StringBuilder("Hello");
            
            // With capacity and max capacity
            var sb4 = new StringBuilder(50, 100); // Start at 50, max 100

            Console.WriteLine($"Default: '{sb1}', With text: '{sb3}'");
            // Output: Default: '', With text: 'Hello'

            // ── EXAMPLE 2: Append - Adding to end of string ───────────
            // Append is the primary method - very efficient
            var builder = new StringBuilder();
            
            builder.Append("Hello");
            builder.Append(" ");
            builder.Append("World");
            builder.Append("!");
            
            Console.WriteLine(builder.ToString()); // Output: Hello World!

            // AppendLine - adds text + newline (like Append + "\n")
            var sb = new StringBuilder();
            sb.AppendLine("Line 1");
            sb.AppendLine("Line 2");
            sb.AppendLine("Line 3");
            
            Console.WriteLine(sb.ToString());
            // Output: Line 1
            //         Line 2
            //         Line 3

            // AppendFormat - combine Append and string.Format
            string formatResult = new StringBuilder()
                .AppendFormat("Name: {0}, Age: {1}", "Alice", 30)
                .AppendFormat(", Score: {0:P0}", 0.85)
                .ToString();
            Console.WriteLine(formatResult);
            // Output: Name: Alice, Age: 30, Score: 85%

            // ── REAL-WORLD EXAMPLE: Building HTML content ────────────
            var html = new StringBuilder();
            
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("  <title>Generated Page</title>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.AppendLine("  <h1>Welcome</h1>");
            html.AppendLine("  <p>Content goes here.</p>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            
            Console.WriteLine(html.ToString());

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Insert, Remove, and Replace
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Insert - Add at specific position ─────────
            var sb1 = new StringBuilder("Hello World");
            
            sb1.Insert(6, "C# "); // Insert at index 6
            Console.WriteLine(sb1.ToString()); // Output: Hello C# World

            // Insert various types automatically
            var sb2 = new StringBuilder("Count: ");
            sb2.Insert(7, 42); // Insert integer
            sb2.Insert(10, " items"); // Insert string
            Console.WriteLine(sb2.ToString()); // Output: Count: 42 items

            // ── EXAMPLE 2: Remove - Delete portion of string ─────────
            var sb3 = new StringBuilder("Hello, Beautiful World!");
            
            sb3.Remove(5, 12); // Remove ", Beautiful" (12 chars from index 5)
            Console.WriteLine(sb3.ToString()); // Output: Hello World!

            // Remove from position to end
            var sb4 = new StringBuilder("Remove after this point");
            int removeIndex = sb4.ToString().IndexOf(" after");
            sb4.Remove(removeIndex, sb4.Length - removeIndex);
            Console.WriteLine(sb4.ToString()); // Output: Remove

            // Clear - removes all characters (set Length to 0)
            var sb5 = new StringBuilder("Temporary");
            sb5.Clear(); // Equivalent to Length = 0
            Console.WriteLine($"Length after clear: {sb5.Length}"); // Output: 0

            // ── EXAMPLE 3: Replace - Substitute text ───────────────────
            var sb6 = new StringBuilder("I like cats");
            
            sb6.Replace("cats", "dogs"); // Replace all occurrences
            Console.WriteLine(sb6.ToString()); // Output: I like dogs

            // Replace with StringComparison for case-insensitive
            var sb7 = new StringBuilder("Hello HELLO hello");
            sb7.Replace("hello", "Hi", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine(sb7.ToString()); // Output: Hi Hi Hi

            // Replace single character
            var sb8 = new StringBuilder("a,b;c:d");
            sb8.Replace(',', ';').Replace(':', ';'); // Multiple replacements
            Console.WriteLine(sb8.ToString()); // Output: a;b;c;d

            // ── REAL-WORLD EXAMPLE: SQL query building ───────────────
            var query = new StringBuilder("SELECT ");
            
            string[] columns = { "Id", "Name", "Email", "CreatedAt" };
            query.Append(string.Join(", ", columns));
            query.Append(" FROM Users");
            query.Append(" WHERE Active = 1");
            query.Append(" ORDER BY Name");
            
            Console.WriteLine(query.ToString());
            // Output: SELECT Id, Name, Email, CreatedAt FROM Users WHERE Active = 1 ORDER BY Name

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Capacity and Memory Management
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Capacity property ───────────────────────────
            // StringBuilder grows automatically, but setting capacity helps
            var sbSmall = new StringBuilder(10); // Initial capacity 10
            sbSmall.Append("1234567890"); // Exactly fills capacity
            Console.WriteLine($"Capacity: {sbSmall.Capacity}, Length: {sbSmall.Length}");
            // Output: Capacity: 10, Length: 10

            sbSmall.Append("1"); // Exceeds capacity - doubles to 20
            Console.WriteLine($"After overflow - Capacity: {sbSmall.Capacity}, Length: {sbSmall.Length}");
            // Output: After overflow - Capacity: 20, Length: 11

            // ── EXAMPLE 2: EnsureCapacity - Pre-allocate ──────────────
            var sb = new StringBuilder();
            
            sb.EnsureCapacity(1000); // Pre-allocate for expected size
            // Now can append 1000 chars without reallocation
            
            for (int i = 0; i < 500; i++)
            {
                sb.Append(i).Append(",");
            }
            
            Console.WriteLine($"Final capacity: {sb.Capacity}, Length: {sb.Length}");

            // ── EXAMPLE 3: MaxCapacity - Limit growth ──────────────────
            // Default max capacity is int.MaxValue (~2 billion)
            var sbMax = new StringBuilder(10, 100); // Max 100 chars
            
            try
            {
                sbMax.Append(new string('x', 150)); // Try to exceed max
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            // ── REAL-WORLD EXAMPLE: Large document generation ─────────
            // Pre-allocate for performance when size is known
            int estimatedSize = 50000; // Estimate
            var document = new StringBuilder(estimatedSize);
            
            // Add XML declaration
            document.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            document.AppendLine("<root>");
            
            // Simulate adding many elements
            for (int i = 0; i < 1000; i++)
            {
                document.AppendLine($"  <item id=\"{i}\"><name>Item {i}</name></item>");
            }
            
            document.AppendLine("</root>");
            
            Console.WriteLine($"Document size: {document.Length} chars");
            // Output: Document size: ~48000 chars

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: StringBuilder Methods Chaining
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Fluent interface - method chaining ─────────
            // All modification methods return StringBuilder for chaining
            string result = new StringBuilder()
                .Append("Start ")
                .AppendLine("Line")
                .Append("Value: ")
                .AppendFormat("{0:C}", 99.99)
                .AppendLine()
                .Replace("Start", "Begin")
                .Insert(0, "# ")
                .ToString();
            
            Console.WriteLine(result);
            // Output: # Begin Line
            //         Value: $99.99

            // ── EXAMPLE 2: Complex string building patterns ───────────
            // Build a table of contents
            var toc = new StringBuilder();
            
            string[] chapters = { "Introduction", "Getting Started", "Advanced Topics", "Reference" };
            int chapterNum = 1;
            
            foreach (string chapter in chapters)
            {
                toc.AppendLine($"Chapter {chapterNum}: {chapter}")
                   .AppendLine(new string('.', 60 - $"Chapter {chapterNum}: {chapter}".Length) + " ")
                   .AppendLine($"  Page {chapterNum * 10}");
                chapterNum++;
            }
            
            Console.WriteLine(toc.ToString());

            // ── REAL-WORLD EXAMPLE: CSV builder with headers ───────────
            var csv = new StringBuilder();
            
            // Header
            csv.AppendLine("Name,Age,City,Email");
            
            // Data rows
            var people = new[] {
                ("John Doe", 30, "NYC", "john@example.com"),
                ("Jane Smith", 25, "LA", "jane@example.com"),
                ("Bob Wilson", 35, "Chicago", "bob@example.com")
            };
            
            foreach (var (name, age, city, email) in people)
            {
                csv.AppendLine($"{name},{age},{city},{email}");
            }
            
            Console.WriteLine(csv.ToString());

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: StringBuilder vs String Performance
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Loop concatenation comparison ───────────────
            // String concatenation in loop - creates new string each iteration
            string bad = "";
            for (int i = 0; i < 100; i++)
            {
                bad += i + ","; // O(n²) - allocates new string each time
            }
            Console.WriteLine($"String concat length: {bad.Length}");

            // StringBuilder - O(n) - single buffer
            var good = new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                good.Append(i).Append(",");
            }
            Console.WriteLine($"StringBuilder length: {good.Length}");

            // ── EXAMPLE 2: When to use String vs StringBuilder ───────
            // Use simple string + for few concatenations (1-3)
            string simple = "Hello " + "World " + "!"; // Fine
            Console.WriteLine($"Simple concat: {simple}"); // Output: Hello World !

            // Use StringBuilder for 4+ concatenations or loops
            int iterations = 10;
            var builder = new StringBuilder();
            for (int i = 0; i < iterations; i++)
            {
                builder.Append(i);
            }
            string result2 = builder.ToString();
            Console.WriteLine($"Built string: {result2}"); // Output: 0123456789

            // ── REAL-WORLD EXAMPLE: Log message builder ───────────────
            // Build formatted log messages efficiently
            DateTime timestamp = DateTime.Now;
            string level = "ERROR";
            string message = "Connection failed";
            string source = "DatabaseService";
            
            // Using StringBuilder for complex log format
            var logMessage = new StringBuilder()
                .Append('[').Append(timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff")).Append("] ")
                .Append('[').Append(level).Append("] ")
                .Append('[').Append(source).Append("] ")
                .Append(message)
                .ToString();
            
            Console.WriteLine(logMessage);
            // Output: [2024-01-15 10:30:45.123] [ERROR] [DatabaseService] Connection failed

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: StringBuilder with Custom Types
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Appending objects ──────────────────────────
            // ToString() is called automatically
            var person = new Person("Alice", 30);
            
            var sb = new StringBuilder();
            sb.Append("Person: ").Append(person).AppendLine();
            Console.WriteLine(sb.ToString()); // Output: Person: Name: Alice, Age: 30

            // ── EXAMPLE 2: Custom object formatting ───────────────────
            // Define ToString() in your class for desired output
            var order = new Order(12345, "Pending", 250.00m);
            
            var orderBuilder = new StringBuilder();
            orderBuilder.AppendLine("Order Details:")
                        .AppendLine($"  ID: {order.OrderId}")
                        .AppendLine($"  Status: {order.Status}")
                        .AppendLine($"  Total: {order.Total:C}");
            
            Console.WriteLine(orderBuilder.ToString());

            Console.WriteLine("\n=== StringBuilder Class Complete ===");
        }
    }

    // Helper classes for examples
    class Person
    {
        public string Name { get; }
        public int Age { get; }
        
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
        
        public override string ToString() => $"Name: {Name}, Age: {Age}";
    }

    class Order
    {
        public int OrderId { get; }
        public string Status { get; }
        public decimal Total { get; }
        
        public Order(int id, string status, decimal total)
        {
            OrderId = id;
            Status = status;
            Total = total;
        }
    }
}