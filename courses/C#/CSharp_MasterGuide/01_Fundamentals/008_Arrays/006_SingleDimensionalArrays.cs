/*
 * ============================================================
 * TOPIC     : Fundamentals - Arrays
 * SUBTOPIC  : Single-Dimensional Arrays
 * FILE      : SingleDimensionalArrays.cs
 * PURPOSE   : Teaches basics of single-dimensional arrays in C#,
 *            including declaration, initialization, access, and iteration
 * ============================================================
 */

using System; // Core System namespace for Console

namespace CSharp_MasterGuide._01_Fundamentals._08_Arrays
{
    class SingleDimensionalArrays
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Array Declaration and Initialization
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Declaration with size ──────────────────────
            // Declare array of 5 integers - elements initialized to 0 by default
            int[] numbers = new int[5];
            
            // Access and set values
            numbers[0] = 10;
            numbers[1] = 20;
            numbers[2] = 30;
            numbers[3] = 40;
            numbers[4] = 50;
            
            Console.WriteLine($"Array size: {numbers.Length}"); // Output: 5
            Console.WriteLine($"First element: {numbers[0]}");   // Output: 10
            Console.WriteLine($"Last element: {numbers[4]}");    // Output: 50

            // ── EXAMPLE 2: Declaration with initialization ─────────────
            // Initialize with values - size determined automatically
            string[] fruits = new string[] { "Apple", "Banana", "Cherry", "Date" };
            
            Console.WriteLine($"Fruit count: {fruits.Length}"); // Output: 4
            foreach (string fruit in fruits)
            {
                Console.WriteLine(fruit); // Apple, Banana, Cherry, Date
            }

            // ── EXAMPLE 3: Short array initialization ──────────────────
            // Most concise syntax - C# compiler infers type and size
            int[] scores = { 95, 87, 92, 78, 88 };
            
            Console.WriteLine($"Third score: {scores[2]}"); // Output: 92

            // Initialize with default values of specific type
            bool[] flags = new bool[3]; // All false by default
            Console.WriteLine($"Default bool: {flags[0]}"); // Output: False

            double[] prices = new double[4]; // All 0.0 by default
            Console.WriteLine($"Default double: {prices[0]}"); // Output: 0

            // ── REAL-WORLD EXAMPLE: Temperature readings ───────────────
            // Store daily temperatures for a week
            double[] temperatures = new double[7];
            temperatures[0] = 22.5; // Monday
            temperatures[1] = 24.1;
            temperatures[2] = 21.8;
            temperatures[3] = 23.3;
            temperatures[4] = 25.0;
            temperatures[5] = 26.2;
            temperatures[6] = 24.8; // Sunday

            Console.WriteLine("Week Temperatures:");
            string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine($"  {days[i]}: {temperatures[i]}°C");
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Array Index and Bounds
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Index access ───────────────────────────────
            int[] data = { 10, 20, 30, 40, 50 };
            
            // Index is 0-based - first element is at index 0
            Console.WriteLine($"Index 0: {data[0]}"); // 10
            Console.WriteLine($"Index 1: {data[1]}"); // 20
            Console.WriteLine($"Index 2: {data[2]}"); // 30

            // Negative index causes compile error - no negative indices
            // data[-1] would not compile

            // ── EXAMPLE 2: Using Length property ───────────────────────
            // Length gives total number of elements
            int[] numbers2 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Access last element safely using Length - 1
            int lastIndex = numbers2.Length - 1;
            Console.WriteLine($"Last element: {numbers2[lastIndex]}"); // Output: 10

            // Loop through array using Length
            Console.WriteLine("All elements:");
            for (int i = 0; i < numbers2.Length; i++)
            {
                Console.WriteLine($"  [{i}]: {numbers2[i]}");
            }

            // ── EXAMPLE 3: Out of bounds handling ──────────────────────
            int[] small = { 1, 2, 3 };
            
            // Uncomment to see IndexOutOfRangeException:
            // Console.WriteLine(small[10]); // Throws exception
            
            // Safe access pattern
            int indexToAccess = 2;
            if (indexToAccess >= 0 && indexToAccess < small.Length)
            {
                Console.WriteLine($"Safe access: {small[indexToAccess]}"); // Output: 3
            }
            else
            {
                Console.WriteLine("Index out of bounds");
            }

            // ── REAL-WORLD EXAMPLE: Safe array access with validation ──
            string[] usernames = { "admin", "user1", "guest", "moderator" };
            
            int[] indicesToCheck = { 0, 1, 5, -1, 3 };
            
            foreach (int idx in indicesToCheck)
            {
                if (idx >= 0 && idx < usernames.Length)
                {
                    Console.WriteLine($"Index {idx}: {usernames[idx]}");
                }
                else
                {
                    Console.WriteLine($"Index {idx}: Invalid (valid range: 0-{usernames.Length - 1})");
                }
            }
            // Output shows valid accesses and error messages for invalid ones

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Iterating Through Arrays
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: For loop iteration ──────────────────────────
            int[] forLoopArray = { 10, 20, 30, 40, 50 };
            
            Console.WriteLine("For loop iteration:");
            for (int i = 0; i < forLoopArray.Length; i++)
            {
                Console.WriteLine($"  Element {i}: {forLoopArray[i]}");
            }

            // ── EXAMPLE 2: Foreach loop iteration ─────────────────────
            string[] colors = { "Red", "Green", "Blue", "Yellow" };
            
            Console.WriteLine("\nForeach iteration:");
            foreach (string color in colors)
            {
                Console.WriteLine($"  Color: {color}");
            }

            // Foreach is read-only - cannot modify elements
            // foreach (string color in colors) { color = "Purple"; } // Won't compile

            // ── EXAMPLE 3: While loop iteration ──────────────────────
            char[] letters = { 'A', 'B', 'C', 'D' };
            
            Console.WriteLine("\nWhile loop iteration:");
            int whileIndex = 0;
            while (whileIndex < letters.Length)
            {
                Console.WriteLine($"  Letter: {letters[whileIndex]}");
                whileIndex++;
            }

            // ── REAL-WORLD EXAMPLE: Calculate statistics ──────────────
            int[] examScores = { 85, 92, 78, 88, 95, 67, 82, 91, 74, 89 };
            
            // Calculate sum and average
            int sum = 0;
            foreach (int score in examScores)
            {
                sum += score;
            }
            double average = (double)sum / examScores.Length;
            
            Console.WriteLine($"\nExam Statistics:");
            Console.WriteLine($"  Number of students: {examScores.Length}");
            Console.WriteLine($"  Total points: {sum}");
            Console.WriteLine($"  Average score: {average:F2}"); // Output: 84.10

            // Find highest and lowest
            int highest = examScores[0];
            int lowest = examScores[0];
            
            foreach (int score in examScores)
            {
                if (score > highest) highest = score;
                if (score < lowest) lowest = score;
            }
            
            Console.WriteLine($"  Highest score: {highest}");
            Console.WriteLine($"  Lowest score: {lowest}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Array Methods and Properties
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Array.IndexOf ──────────────────────────────
            string[] names = { "Alice", "Bob", "Charlie", "Diana", "Eve" };
            
            int foundIndex = Array.IndexOf(names, "Charlie");
            Console.WriteLine($"Index of 'Charlie': {foundIndex}"); // Output: 2
            
            int notFoundIndex = Array.IndexOf(names, "Frank");
            Console.WriteLine($"Index of 'Frank': {notFoundIndex}"); // Output: -1

            // ── EXAMPLE 2: Array.Sort ──────────────────────────────────
            int[] unsorted = { 5, 2, 8, 1, 9, 3 };
            
            Console.WriteLine($"\nBefore sort: {string.Join(", ", unsorted)}");
            Array.Sort(unsorted);
            Console.WriteLine($"After sort: {string.Join(", ", unsorted)}");
            // Output: 1, 2, 3, 5, 8, 9

            // Sort strings alphabetically
            string[] unsortedNames = { "Zebra", "Apple", "Mango", "Banana" };
            Array.Sort(unsortedNames);
            Console.WriteLine($"Sorted names: {string.Join(", ", unsortedNames)}");
            // Output: Apple, Banana, Mango, Zebra

            // ── EXAMPLE 3: Array.Reverse ────────────────────────────────
            int[] toReverse = { 1, 2, 3, 4, 5 };
            
            Console.WriteLine($"\nBefore reverse: {string.Join(", ", toReverse)}");
            Array.Reverse(toReverse);
            Console.WriteLine($"After reverse: {string.Join(", ", toReverse)}");
            // Output: 5, 4, 3, 2, 1

            // ── EXAMPLE 4: Array.Clear ──────────────────────────────────
            int[] toClear = { 10, 20, 30, 40, 50 };
            
            Console.WriteLine($"\nBefore clear: {string.Join(", ", toClear)}");
            Array.Clear(toClear, 0, toClear.Length); // Set all to default (0)
            Console.WriteLine($"After clear: {string.Join(", ", toClear)}");
            // Output: 0, 0, 0, 0, 0

            // Clear specific range
            int[] partialClear = { 1, 2, 3, 4, 5 };
            Array.Clear(partialClear, 1, 3); // Clear elements 1, 2, 3
            Console.WriteLine($"After partial clear: {string.Join(", ", partialClear)}");
            // Output: 1, 0, 0, 0, 5

            // ── REAL-WORLD EXAMPLE: Manage inventory ───────────────────
            string[] products = { "Laptop", "Mouse", "Keyboard", "Monitor", "Webcam" };
            int[] stock = { 10, 25, 15, 5, 8 };
            
            Console.WriteLine("\nInventory:");
            Console.WriteLine("Product".PadRight(15) + "Stock");
            Console.WriteLine("-".PadRight(15, '-') + "-----");
            
            for (int i = 0; i < products.Length; i++)
            {
                Console.WriteLine(products[i].PadRight(15) + stock[i]);
            }
            
            // Sort by stock level (ascending) - keep products aligned
            int[] sortedIndices = (int[])stock.Clone();
            Array.Sort(sortedIndices); // This would misalign! Use parallel sort
            
            // Better approach - use Array.Sort with Array.IndexOf
            Console.WriteLine("\nLow stock items (sorted):");
            var stockCopy = (int[])stock.Clone();
            var productsCopy = (string[])products.Clone();
            Array.Sort(stockCopy, productsCopy); // Sort both arrays together
            
            foreach (var p in productsCopy)
            {
                int idx = Array.IndexOf(products, p);
                Console.WriteLine($"  {p}: {stock[idx]} units");
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Array Initialization Patterns
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Initialize with default values ────────────
            // Using Enumerable.Repeat
            int[] repeated = Enumerable.Repeat(100, 5).ToArray();
            Console.WriteLine($"Repeated values: {string.Join(", ", repeated)}");
            // Output: 100, 100, 100, 100, 100

            // Initialize with range using Enumerable.Range
            int[] range = Enumerable.Range(1, 10).ToArray();
            Console.WriteLine($"Range 1-10: {string.Join(", ", range)}");
            // Output: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10

            // ── EXAMPLE 2: Initialize with default using new ──────────
            // int[] defaultInts = new int[5] { 1, 2, 3, 4, 5 }; // Both work
            int[] explicitSize = new int[3] { 1, 2, 3 }; // Must match size
            Console.WriteLine($"Explicit size: {string.Join(", ", explicitSize)}");
            // Output: 1, 2, 3

            // ── EXAMPLE 3: Array of specific type initialization ──────
            // Empty string array - strings initialized to null
            string[] emptyStrings = new string[3];
            Console.WriteLine($"Empty strings: {string.Join(", ", emptyStrings)}");
            // Output: , ,  (empty/null values)

            // Object array
            object[] mixed = new object[3];
            mixed[0] = 42;
            mixed[1] = "hello";
            mixed[2] = new DateTime(2024, 1, 1);
            
            foreach (object item in mixed)
            {
                Console.WriteLine($"  Type: {item?.GetType().Name ?? "null"}");
            }

            // ── REAL-WORLD EXAMPLE: Initialize monthly sales ───────────
            // Track sales for 12 months - pre-initialize to zero
            decimal[] monthlySales = new decimal[12];
            
            // Simulate some sales data
            monthlySales[0] = 15000m;  // Jan
            monthlySales[1] = 18000m;  // Feb
            monthlySales[2] = 22000m;  // Mar
            // ... others default to 0
            
            string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                               "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            
            Console.WriteLine("\nMonthly Sales:");
            for (int i = 0; i < 12; i++)
            {
                Console.WriteLine($"  {months[i]}: ${monthlySales[i]:N0}");
            }

            // Calculate yearly total
            decimal yearlyTotal = 0;
            foreach (decimal sale in monthlySales)
            {
                yearlyTotal += sale;
            }
            Console.WriteLine($"\nYearly Total: ${yearlyTotal:N0}");

            Console.WriteLine("\n=== Single-Dimensional Arrays Complete ===");
        }
    }
}