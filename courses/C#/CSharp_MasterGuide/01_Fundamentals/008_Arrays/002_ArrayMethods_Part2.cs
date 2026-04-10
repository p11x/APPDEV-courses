/*
 * ============================================================
 * TOPIC     : Fundamentals - Arrays
 * SUBTOPIC  : Array Methods - Advanced Operations
 * FILE      : ArrayMethods_Part2.cs
 * PURPOSE   : Advanced array operations including set operations,
 *            segment handling, and type-specific methods
 * ============================================================
 */

using System; // Core System namespace for Console
using System.Linq; // For LINQ extension methods
using System.Collections.Generic; // For generic collections

namespace CSharp_MasterGuide._01_Fundamentals._08_Arrays
{
    class ArrayMethods_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Set Operations on Arrays
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Find common elements (intersection) ────────
            int[] setA = { 1, 2, 3, 4, 5 };
            int[] setB = { 4, 5, 6, 7, 8 };
            
            // Manual intersection
            var intersection = new List<int>();
            foreach (int a in setA)
            {
                if (Array.IndexOf(setB, a) >= 0)
                {
                    intersection.Add(a);
                }
            }
            Console.WriteLine($"Intersection: {string.Join(", ", intersection)}"); // 4, 5
            
            // Using LINQ Intersect (for reference)
            var intersectLinq = setA.Intersect(setB).ToArray();
            Console.WriteLine($"Intersection (LINQ): {string.Join(", ", intersectLinq)}");

            // ── EXAMPLE 2: Union - combine unique elements ───────────
            var union = new List<int>(setA);
            foreach (int b in setB)
            {
                if (!union.Contains(b))
                {
                    union.Add(b);
                }
            }
            Console.WriteLine($"\nUnion: {string.Join(", ", union)}"); // 1,2,3,4,5,6,7,8

            // ── EXAMPLE 3: Difference - elements in A but not B ──────
            var difference = new List<int>();
            foreach (int a in setA)
            {
                if (Array.IndexOf(setB, a) < 0)
                {
                    difference.Add(a);
                }
            }
            Console.WriteLine($"A - B: {string.Join(", ", difference)}"); // 1,2,3

            // ── EXAMPLE 4: Symmetric difference - not in both ──────────
            var symmetricDiff = new List<int>();
            
            foreach (int a in setA)
            {
                if (Array.IndexOf(setB, a) < 0)
                    symmetricDiff.Add(a);
            }
            foreach (int b in setB)
            {
                if (Array.IndexOf(setA, b) < 0)
                    symmetricDiff.Add(b);
            }
            Console.WriteLine($"Symmetric diff: {string.Join(", ", symmetricDiff)}"); // 1,2,3,6,7,8

            // ── REAL-WORLD EXAMPLE: User permissions ───────────────────
            string[] allPermissions = { "read", "write", "delete", "execute", "admin" };
            string[] userRole = { "read", "write" };
            string[] adminRole = { "read", "write", "delete", "admin" };
            string[] userPermissions = { "read", "write", "execute" };
            
            // Find role permissions
            var rolePerms = allPermissions.Intersect(userRole).ToArray();
            Console.WriteLine("Role permissions: " + string.Join(", ", rolePerms));
            
            // Find extra permissions user has beyond role
            var userExtras = userPermissions.Except(userRole).ToArray();
            Console.WriteLine("User extra perms: " + string.Join(", ", userExtras));
            
            // Check if user has all role permissions
            bool hasRolePerms = userPermissions.Intersect(userRole).Count() == userRole.Length;
            Console.WriteLine($"Has all role perms: {hasRolePerms}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Array Segment Operations
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: ArraySegment<T> ────────────────────────────
            // View into portion of array without copying
            int[] fullArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Create segment from index 2, length 5 (elements 3,4,5,6,7)
            ArraySegment<int> segment = new ArraySegment<int>(fullArray, 2, 5);
            
            Console.WriteLine($"Segment offset: {segment.Offset}, count: {segment.Count}");
            Console.WriteLine($"Segment array length: {segment.Array.Length}");
            Console.WriteLine($"First element: {segment.Array[segment.Offset]}"); // 3
            Console.WriteLine($"Segment values: {string.Join(", ", segment)}");
            // Output: 3, 4, 5, 6, 7

            // ── EXAMPLE 2: Iterate ArraySegment ─────────────────────
            Console.WriteLine("\nIterate segment:");
            for (int i = segment.Offset; i < segment.Offset + segment.Count; i++)
            {
                Console.WriteLine($"  Index {i}: {segment.Array[i]}");
            }

            // Use segment in methods
            int segmentSum = SumSegment(segment);
            Console.WriteLine($"Segment sum: {segmentSum}"); // 3+4+5+6+7 = 25

            // ── EXAMPLE 3: Buffer slices ─────────────────────────────
            // Useful for parsing data in chunks
            byte[] buffer = new byte[100];
            
            // Fill with sample data
            for (int i = 0; i < 100; i++)
            {
                buffer[i] = (byte)(i + 1);
            }
            
            // Process in chunks
            int chunkSize = 20;
            for (int offset = 0; offset < 100; offset += chunkSize)
            {
                int remaining = Math.Min(chunkSize, 100 - offset);
                ArraySegment<byte> chunk = new ArraySegment<byte>(buffer, offset, remaining);
                
                byte sum = 0;
                foreach (byte b in chunk)
                {
                    sum += b;
                }
                Console.WriteLine($"Chunk {offset/chunkSize}: sum = {sum}");
            }

            // ── REAL-WORLD EXAMPLE: Parse protocol buffer ─────────────
            // Simulate parsing a message with header and body
            byte[] message = new byte[50];
            // Header: [type:1][length:1][checksum:1]
            // Body: variable length
            
            // Fill with simulated data
            message[0] = 1; // Type = 1 (data)
            message[1] = 10; // Length = 10
            message[2] = 0; // Checksum placeholder
            for (int i = 3; i < 13; i++) message[i] = (byte)(i - 2);
            
            // Parse header
            ArraySegment<byte> header = new ArraySegment<byte>(message, 0, 3);
            byte type = header.Array[header.Offset];
            byte length = header.Array[header.Offset + 1];
            byte checksum = header.Array[header.Offset + 2];
            
            Console.WriteLine($"\nParsed header: type={type}, length={length}, checksum={checksum}");
            
            // Parse body
            ArraySegment<byte> body = new ArraySegment<byte>(message, 3, length);
            Console.WriteLine($"Body: {string.Join(", ", body)}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Value Type Array Specifics
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Default values for value types ──────────────
            int[] ints = new int[3]; // All 0
            double[] doubles = new double[3]; // All 0.0
            bool[] bools = new bool[3]; // All false
            DateTime[] dates = new DateTime[3]; // All default (0001-01-01)
            
            Console.WriteLine($"int default: {ints[0]}");
            Console.WriteLine($"double default: {doubles[0]}");
            Console.WriteLine($"bool default: {bools[0]}");
            Console.WriteLine($"DateTime default: {dates[0]}");

            // ── EXAMPLE 2: Struct arrays ──────────────────────────────
            // Struct stored directly in array (not reference)
            Point[] points = new Point[3];
            points[0] = new Point { X = 1, Y = 2 };
            points[1] = new Point { X = 3, Y = 4 };
            points[2] = new Point { X = 5, Y = 6 };
            
            foreach (var p in points)
            {
                Console.WriteLine($"Point: ({p.X}, {p.Y})");
            }

            // ── EXAMPLE 3: Nullable value types in arrays ───────────
            // Can store null in value type arrays using ?
            int?[] nullableInts = new int?[3];
            nullableInts[0] = 10;
            nullableInts[1] = null; // Allowed
            nullableInts[2] = 20;
            
            Console.WriteLine("\nNullable int array:");
            foreach (int? ni in nullableInts)
            {
                Console.WriteLine(ni.HasValue ? ni.Value.ToString() : "(null)");
            }

            // ── REAL-WORLD EXAMPLE: Sensor readings with gaps ─────────
            // Store temperature readings, some times no reading
            DateTime[] readingTimes = new DateTime[7];
            double?[] temperatures = new double?[7];
            
            // Simulate: Monday-Friday have readings, weekend missing
            DateTime baseDate = new DateTime(2024, 1, 15); // Monday
            double[] temps = { 22.5, 23.1, 21.8, 24.0, 22.9 };
            
            for (int i = 0; i < 5; i++)
            {
                readingTimes[i] = baseDate.AddDays(i);
                temperatures[i] = temps[i];
            }
            // Days 5, 6 remain null
            
            Console.WriteLine("\nTemperature readings:");
            for (int i = 0; i < 7; i++)
            {
                string day = readingTimes[i].ToString("ddd");
                if (temperatures[i].HasValue)
                {
                    Console.WriteLine($"  {day}: {temps[i]}°C");
                }
                else
                {
                    Console.WriteLine($"  {day}: (no reading)");
                }
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Enum Arrays and Special Types
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Enum arrays ────────────────────────────────
            DayOfWeek[] weekDays = new DayOfWeek[7];
            for (int i = 0; i < 7; i++)
            {
                weekDays[i] = (DayOfWeek)i;
            }
            
            Console.WriteLine("Week days:");
            foreach (DayOfWeek day in weekDays)
            {
                Console.WriteLine($"  {day}");
            }

            // ── EXAMPLE 2: Array of enums with custom values ─────────
            Status[] statuses = { Status.Pending, Status.Active, Status.Completed, Status.Failed };
            
            int[] statusCodes = new int[4];
            statusCodes[(int)Status.Pending] = 0;
            statusCodes[(int)Status.Active] = 1;
            statusCodes[(int)Status.Completed] = 2;
            statusCodes[(int)Status.Failed] = -1;
            
            Console.WriteLine("\nStatus codes:");
            foreach (Status s in statuses)
            {
                Console.WriteLine($"  {s} = {statusCodes[(int)s]}");
            }

            // ── REAL-WORLD EXAMPLE: Day of week sales ────────────────
            DayOfWeek[] days = Enum.GetValues<DayOfWeek>(); // C# 9+ GetValues
            decimal[] salesByDay = new decimal[7];
            
            // Sample sales data
            salesByDay[(int)DayOfWeek.Monday] = 1200m;
            salesByDay[(int)DayOfWeek.Tuesday] = 1500m;
            salesByDay[(int)DayOfWeek.Wednesday] = 1100m;
            salesByDay[(int)DayOfWeek.Thursday] = 1400m;
            salesByDay[(int)DayOfWeek.Friday] = 2000m;
            salesByDay[(int)DayOfWeek.Saturday] = 2500m;
            salesByDay[(int)DayOfWeek.Sunday] = 1800m;
            
            Console.WriteLine("\nSales by day:");
            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine($"  {days[i]}: ${salesByDay[i]:N0}");
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Spans and Memory Operations
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Span<T> from array ────────────────────────
            int[] data = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Create span - no allocation, just a view
            Span<int> span = data.AsSpan();
            
            // Slice without allocation
            Span<int> slice = span.Slice(2, 5); // Elements 3-7
            Console.WriteLine($"Slice: {string.Join(", ", slice.ToArray())}"); // 3,4,5,6,7
            
            // Copy to stack-allocated array
            Span<int> small = stackalloc int[3]; // C# 7.2+
            slice.Slice(0, 3).CopyTo(small);
            Console.WriteLine($"Stack copy: {string.Join(", ", small.ToArray())}");

            // ── EXAMPLE 2: ReadOnlySpan for strings ───────────────────
            string text = "Hello, World!";
            ReadOnlySpan<char> textSpan = text.AsSpan();
            
            // Find without allocation
            int commaIndex = textSpan.IndexOf(',');
            Console.WriteLine($"Comma at index: {commaIndex}");
            
            ReadOnlySpan<char> before = textSpan.Slice(0, commaIndex);
            ReadOnlySpan<char> after = textSpan.Slice(commaIndex + 1);
            Console.WriteLine($"Before comma: {before.ToString()}"); // Hello
            Console.WriteLine($"After comma: {after.ToString()}"); // World!

            // ── EXAMPLE 3: Memory<T> for async operations ─────────────
            // Similar to Span but can be used in async
            int[] buffer2 = new int[100];
            Memory<int> memory = buffer2;
            
            // Can slice Memory too
            Memory<int> segment2 = memory.Slice(10, 20);
            Console.WriteLine($"Memory segment length: {segment2.Length}");

            // ── REAL-WORLD EXAMPLE: High-performance parsing ─────────
            // Parse CSV-like data using Span for performance
            byte[] csvData = System.Text.Encoding.ASCII.GetBytes("123,456,789,1000");
            ReadOnlySpan<byte> csvSpan = csvData.AsSpan();
            
            // Find all numbers
            var numbers = new List<int>();
            int start = 0;
            
            for (int i = 0; i <= csvSpan.Length; i++)
            {
                if (i == csvSpan.Length || csvSpan[i] == ',')
                {
                    if (i > start)
                    {
                        var numSpan = csvSpan.Slice(start, i - start);
                        int num = int.Parse(numSpan);
                        numbers.Add(num);
                    }
                    start = i + 1;
                }
            }
            
            Console.WriteLine($"\nParsed numbers: {string.Join(", ", numbers)}");
            // Output: 123, 456, 789, 1000

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Complex Array Scenarios
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Jagged array from 2D ──────────────────────
            int[,] matrix = { 
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            };
            
            // Convert 2D to jagged
            int[][] jaggedFrom2D = new int[matrix.GetLength(0)][];
            
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                jaggedFrom2D[row] = new int[matrix.GetLength(1)];
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    jaggedFrom2D[row][col] = matrix[row, col];
                }
            }
            
            Console.WriteLine("\nConverted to jagged:");
            foreach (int[] row in jaggedFrom2D)
            {
                Console.WriteLine($"  {string.Join(", ", row)}");
            }

            // ── EXAMPLE 2: Transpose matrix ───────────────────────────
            int[,] toTranspose = {
                { 1, 2, 3 },
                { 4, 5, 6 }
            }; // 2x3
            
            int[,] transposed = new int[3, 2]; // 3x2
            
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    transposed[c, r] = toTranspose[r, c];
                }
            }
            
            Console.WriteLine("\nTransposed:");
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 2; c++)
                {
                    Console.Write($"{transposed[r, c]} ");
                }
                Console.WriteLine();
            }
            // Output: 1 4
            //         2 5
            //         3 6

            // ── EXAMPLE 3: Flatten nested arrays ──────────────────────
            int[][] nested = {
                new[] { 1, 2 },
                new[] { 3, 4, 5 },
                new[] { 6 }
            };
            
            // Calculate total length
            int totalLength = 0;
            foreach (int[] arr in nested)
            {
                totalLength += arr.Length;
            }
            
            // Flatten
            int[] flattened = new int[totalLength];
            int index = 0;
            foreach (int[] arr in nested)
            {
                foreach (int val in arr)
                {
                    flattened[index++] = val;
                }
            }
            
            Console.WriteLine($"\nFlattened: {string.Join(", ", flattened)}");
            // Output: 1, 2, 3, 4, 5, 6

            Console.WriteLine("\n=== Array Methods Part 2 Complete ===");
        }

        // Helper method for ArraySegment
        static int SumSegment(ArraySegment<int> segment)
        {
            int sum = 0;
            for (int i = segment.Offset; i < segment.Offset + segment.Count; i++)
            {
                sum += segment.Array[i];
            }
            return sum;
        }

        // Helper struct for example
        struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        // Helper enum
        enum Status { Pending, Active, Completed, Failed }
    }
}