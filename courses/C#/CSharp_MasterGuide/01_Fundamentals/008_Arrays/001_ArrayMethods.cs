/*
 * ============================================================
 * TOPIC     : Fundamentals - Arrays
 * SUBTOPIC  : Array Methods and Operations
 * FILE      : ArrayMethods.cs
 * PURPOSE   : Teaches essential array methods and operations
 *            including sorting, searching, copying, and transformations
 * ============================================================
 */

using System; // Core System namespace for Console
using System.Linq; // For LINQ extension methods

namespace CSharp_MasterGuide._01_Fundamentals._08_Arrays
{
    class ArrayMethods
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Sorting Arrays
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Array.Sort ──────────────────────────────────
            int[] numbers = { 5, 2, 8, 1, 9, 3 };
            
            Console.WriteLine($"Before sort: {string.Join(", ", numbers)}");
            Array.Sort(numbers);
            Console.WriteLine($"After sort: {string.Join(", ", numbers)}");
            // Output: 1, 2, 3, 5, 8, 9

            // Sort strings
            string[] colors = { "Red", "green", "Blue", "yellow", "Orange" };
            Array.Sort(colors);
            Console.WriteLine($"Sorted colors: {string.Join(", ", colors)}");
            // Output: Blue, Orange, Red, green, yellow (case-sensitive)

            // ── EXAMPLE 2: Sort with custom comparison ───────────────
            // Sort in descending order using comparison delegate
            int[] descending = { 5, 2, 8, 1, 9 };
            Array.Sort(descending, (a, b) => b.CompareTo(a)); // Reverse order
            Console.WriteLine($"Descending: {string.Join(", ", descending)}");
            // Output: 9, 8, 5, 2, 1

            // Sort by string length
            string[] words = { "cat", "elephant", "a", "hi", "world" };
            Array.Sort(words, (a, b) => a.Length.CompareTo(b.Length));
            Console.WriteLine($"By length: {string.Join(", ", words)}");
            // Output: a, hi, cat, world, elephant

            // ── EXAMPLE 3: Reverse ────────────────────────────────────
            int[] toReverse = { 1, 2, 3, 4, 5 };
            
            Console.WriteLine($"\nBefore reverse: {string.Join(", ", toReverse)}");
            Array.Reverse(toReverse);
            Console.WriteLine($"After reverse: {string.Join(", ", toReverse)}");
            // Output: 5, 4, 3, 2, 1

            // Reverse a portion of array
            int[] partial = { 1, 2, 3, 4, 5, 6, 7, 8 };
            Array.Reverse(partial, 2, 4); // Reverse elements 2-5 (indices 2-5)
            Console.WriteLine($"Partial reverse: {string.Join(", ", partial)}");
            // Output: 1, 2, 7, 6, 5, 4, 3, 8

            // ── REAL-WORLD EXAMPLE: Leaderboard ───────────────────────
            var players = new[] {
                (Name: "Alice", Score: 1500),
                (Name: "Bob", Score: 2300),
                (Name: "Charlie", Score: 1800),
                (Name: "Diana", Score: 2100)
            };
            
            // Sort by score descending
            Array.Sort(players, (a, b) => b.Score.CompareTo(a.Score));
            
            Console.WriteLine("\nLeaderboard:");
            for (int i = 0; i < players.Length; i++)
            {
                Console.WriteLine($"  #{i + 1}: {players[i].Name} - {players[i].Score} points");
            }
            // Output shows Bob, Diana, Charlie, Alice

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Searching Arrays
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Array.IndexOf ──────────────────────────────
            string[] fruits = { "Apple", "Banana", "Cherry", "Date", "Apple" };
            
            int firstApple = Array.IndexOf(fruits, "Apple");
            Console.WriteLine($"First 'Apple' at index: {firstApple}"); // Output: 0
            
            int secondApple = Array.IndexOf(fruits, "Apple", firstApple + 1);
            Console.WriteLine($"Second 'Apple' at index: {secondApple}"); // Output: 4
            
            int notFound = Array.IndexOf(fruits, "Grape");
            Console.WriteLine($"'Grape' not found, returns: {notFound}"); // Output: -1

            // ── EXAMPLE 2: Array.BinarySearch ─────────────────────────
            // Requires sorted array! Binary search is much faster O(log n) vs O(n)
            int[] sorted = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            
            int foundIndex = Array.BinarySearch(sorted, 50);
            Console.WriteLine($"\nBinary search for 50: {foundIndex}"); // Output: 4
            
            int notFoundBinary = Array.BinarySearch(sorted, 55);
            Console.WriteLine($"Binary search for 55: {notFoundBinary}"); // Output: -6 (negative = insertion point)
            
            // Binary search works with custom comparators
            string[] sortedNames = { "Alice", "Bob", "Charlie", "Diana" };
            int nameIndex = Array.BinarySearch(sortedNames, "Charlie", StringComparer.OrdinalIgnoreCase);
            Console.WriteLine($"Search 'Charlie': {nameIndex}"); // Output: 2

            // ── EXAMPLE 3: Find with custom predicate ─────────────────
            // Use Array.Find, FindAll, FindIndex (requires System.Array methods)
            int[] values = { 3, 7, 2, 9, 5, 12, 8, 4 };
            
            // Find first element > 5
            int firstGreater = Array.Find(values, x => x > 5);
            Console.WriteLine($"\nFirst > 5: {firstGreater}"); // Output: 7
            
            // Find all elements > 5
            int[] allGreater = Array.FindAll(values, x => x > 5);
            Console.WriteLine($"All > 5: {string.Join(", ", allGreater)}"); // Output: 7, 9, 12, 8
            
            // Find index of first even number
            int evenIndex = Array.FindIndex(values, x => x % 2 == 0);
            Console.WriteLine($"First even at index: {evenIndex}"); // Output: 2 (value 2)

            // Find last element < 10
            int lastLess = Array.FindLast(values, x => x < 10);
            Console.WriteLine($"Last < 10: {lastLess}"); // Output: 4

            // ── REAL-WORLD EXAMPLE: Product search ────────────────────
            var products = new[] {
                (Id: 1, Name: "Laptop", Price: 999.99m, InStock: true),
                (Id: 2, Name: "Mouse", Price: 29.99m, InStock: true),
                (Id: 3, Name: "Keyboard", Price: 79.99m, InStock: false),
                (Id: 4, Name: "Monitor", Price: 299.99m, InStock: true),
                (Id: 5, Name: "Webcam", Price: 69.99m, InStock: false)
            };
            
            // Find first in-stock product
            var inStock = Array.Find(products, p => p.InStock);
            Console.WriteLine($"\nFirst in-stock: {inStock.Name} - ${inStock.Price}");
            
            // Find all out of stock
            var outOfStock = Array.FindAll(products, p => !p.InStock);
            Console.WriteLine("Out of stock:");
            foreach (var p in outOfStock)
            {
                Console.WriteLine($"  - {p.Name}");
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Copying and Cloning Arrays
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Array.Clone ────────────────────────────────
            int[] original = { 1, 2, 3, 4, 5 };
            int[] cloned = (int[])original.Clone(); // Creates shallow copy
            
            cloned[0] = 100; // Modify clone
            Console.WriteLine($"Original[0]: {original[0]}"); // Output: 1 (unchanged)
            Console.WriteLine($"Cloned[0]: {cloned[0]}"); // Output: 100

            // ── EXAMPLE 2: Array.Copy ──────────────────────────────────
            int[] source = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int[] destination = new int[10];
            
            // Copy all elements
            Array.Copy(source, destination, source.Length);
            Console.WriteLine($"Copied: {string.Join(", ", destination)}");
            
            // Copy subset - from index 2, copy 5 elements to destination starting at index 0
            int[] partial = new int[5];
            Array.Copy(source, 2, partial, 0, 5);
            Console.WriteLine($"Partial copy: {string.Join(", ", partial)}"); // 3, 4, 5, 6, 7

            // ── EXAMPLE 3: Array.Resize ───────────────────────────────
            int[] dynamic = { 1, 2, 3 };
            
            Console.WriteLine($"\nBefore resize: {dynamic.Length} elements");
            Array.Resize(ref dynamic, 10); // Increase size
            
            // New elements are default (0 for int)
            Console.WriteLine($"After resize: {dynamic.Length} elements");
            Console.WriteLine($"New elements are 0: {dynamic[5]}, {dynamic[9]}");
            
            // Can also shrink
            Array.Resize(ref dynamic, 2);
            Console.WriteLine($"After shrink: {dynamic.Length} elements, first 2: {string.Join(", ", dynamic)}");

            // ── REAL-WORLD EXAMPLE: Buffer management ─────────────────
            // Simulate reading data in chunks
            byte[] dataBuffer = new byte[100];
            byte[] finalBuffer = new byte[0];
            
            // Simulate chunks of data
            byte[][] chunks = {
                new byte[] { 1, 2, 3, 4, 5 },
                new byte[] { 6, 7, 8 },
                new byte[] { 9, 10 }
            };
            
            foreach (byte[] chunk in chunks)
            {
                int oldLength = finalBuffer.Length;
                Array.Resize(ref finalBuffer, oldLength + chunk.Length);
                Array.Copy(chunk, 0, finalBuffer, oldLength, chunk.Length);
            }
            
            Console.WriteLine($"\nMerged data: {string.Join(", ", finalBuffer)}");
            // Output: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Array Transformations
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: ConvertAll ─────────────────────────────────
            // Convert each element to different type
            int[] nums = { 1, 2, 3, 4, 5 };
            
            // Convert to strings
            string[] asStrings = Array.ConvertAll(nums, n => n.ToString());
            Console.WriteLine($"As strings: {string.Join(", ", asStrings)}");
            
            // Convert to double
            double[] asDoubles = Array.ConvertAll(nums, n => (double)n);
            Console.WriteLine($"As doubles: {string.Join(", ", asDoubles)}");
            
            // Convert to squared values
            int[] squared = Array.ConvertAll(nums, n => n * n);
            Console.WriteLine($"Squared: {string.Join(", ", squared)}");

            // ── EXAMPLE 2: ForEach ────────────────────────────────────
            string[] messages = { "Hello", "World", "From", "C#" };
            
            Console.WriteLine("\nForEach iteration:");
            Array.ForEach(messages, msg => Console.WriteLine($"  Message: {msg}"));
            
            // Modify each element in place (not recommended but possible)
            int[] toModify = { 1, 2, 3 };
            Array.ForEach(toModify, x => x *= 2); // Note: this doesn't modify original!
            Console.WriteLine($"After ForEach: {string.Join(", ", toModify)}"); // Still 1,2,3
            
            // To actually modify, use Array.ForEach with index
            for (int i = 0; i < toModify.Length; i++)
            {
                toModify[i] *= 2;
            }
            Console.WriteLine($"After real modification: {string.Join(", ", toModify)}"); // 2,4,6

            // ── EXAMPLE 3: TrueForAll and Exists ─────────────────────
            int[] checkValues = { 10, 20, 30, 40, 50 };
            
            // Check if all elements meet condition
            bool allPositive = Array.TrueForAll(checkValues, x => x > 0);
            Console.WriteLine($"\nAll > 0: {allPositive}"); // True
            
            bool allEven = Array.TrueForAll(checkValues, x => x % 2 == 0);
            Console.WriteLine($"All even: {allEven}"); // True
            
            // Check if any element exists
            bool has50 = Array.Exists(checkValues, x => x == 50);
            Console.WriteLine($"Has 50: {has50}"); // True
            
            bool hasOdd = Array.Exists(checkValues, x => x % 2 != 0);
            Console.WriteLine($"Has odd: {hasOdd}"); // False

            // ── REAL-WORLD EXAMPLE: Data validation ───────────────────
            var formSubmissions = new[] {
                (Name: "John", Email: "john@email.com", Age: 25),
                (Name: "Jane", Email: "jane.email.com", Age: 30), // Invalid email
                (Name: "Bob", Email: "bob@email.com", Age: -5),  // Invalid age
                (Name: "Alice", Email: "alice@email.com", Age: 22)
            };
            
            Console.WriteLine("\nForm Validation:");
            
            // Validate all fields
            bool allValid = Array.TrueForAll(formSubmissions, f => 
                f.Email.Contains("@") && f.Age > 0 && f.Age < 150);
            Console.WriteLine($"All submissions valid: {allValid}"); // False
            
            // Find invalid submissions
            var invalid = Array.FindAll(formSubmissions, f => 
                !f.Email.Contains("@") || f.Age <= 0 || f.Age >= 150);
            
            Console.WriteLine("Invalid submissions:");
            foreach (var inv in invalid)
            {
                string reason = !inv.Email.Contains("@") ? "bad email" : 
                               inv.Age <= 0 ? "negative age" : "too old";
                Console.WriteLine($"  - {inv.Name}: {reason}");
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Multi-Dimensional Array Methods
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: GetLength and Rank ────────────────────────
            int[,] matrix = new int[3, 4];
            
            Console.WriteLine($"Rank: {matrix.Rank}"); // 2
            Console.WriteLine($"Rows: {matrix.GetLength(0)}"); // 3
            Console.WriteLine($"Columns: {matrix.GetLength(1)}"); // 4

            // ── EXAMPLE 2: 2D array initialization ───────────────────
            int[,] initialized = { 
                { 1, 2, 3 },
                { 4, 5, 6 }
            };
            
            Console.WriteLine("\n2D Array:");
            for (int row = 0; row < initialized.GetLength(0); row++)
            {
                for (int col = 0; col < initialized.GetLength(1); col++)
                {
                    Console.Write($"{initialized[row, col]} ");
                }
                Console.WriteLine();
            }

            // ── EXAMPLE 3: Iterate all elements ───────────────────────
            int[,,] threeD = {
                {
                    {1, 2}, {3, 4}
                },
                {
                    {5, 6}, {7, 8}
                }
            };
            
            Console.WriteLine("\n3D Array iteration:");
            for (int d0 = 0; d0 < threeD.GetLength(0); d0++)
            {
                for (int d1 = 0; d1 < threeD.GetLength(1); d1++)
                {
                    for (int d2 = 0; d2 < threeD.GetLength(2); d2++)
                    {
                        Console.WriteLine($"  [{d0},{d1},{d2}] = {threeD[d0, d1, d2]}");
                    }
                }
            }

            // ── REAL-WORLD EXAMPLE: Matrix operations ──────────────────
            // Multiply two matrices
            int[,] matrixA = { { 1, 2 }, { 3, 4 } };
            int[,] matrixB = { { 5, 6 }, { 7, 8 } };
            int[,] result = new int[2, 2];
            
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < 2; k++)
                    {
                        result[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }
            
            Console.WriteLine("\nMatrix multiplication result:");
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Console.Write($"{result[i, j]} ");
                }
                Console.WriteLine();
            }
            // Output: 19 22
            //         43 50

            Console.WriteLine("\n=== Array Methods Complete ===");
        }
    }
}