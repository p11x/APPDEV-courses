/*
 * ============================================================
 * TOPIC     : Fundamentals - Arrays
 * SUBTOPIC  : Jagged Arrays
 * FILE      : JaggedArrays.cs
 * PURPOSE   : Teaches jagged arrays (arrays of arrays) in C#,
 *            their declaration, initialization, and use cases
 * ============================================================
 */

using System; // Core System namespace for Console

namespace CSharp_MasterGuide._01_Fundamentals._08_Arrays
{
    class JaggedArrays
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Jagged Array Declaration and Initialization
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Declare empty jagged array ──────────────────
            // Array of 3 arrays (rows), each row is null initially
            int[][] jaggedInt = new int[3][];
            
            // Can't access elements yet - must initialize each row first
            Console.WriteLine($"Outer array length: {jaggedInt.Length}"); // Output: 3
            Console.WriteLine($"Row 0 is null: {jaggedInt[0] == null}"); // Output: True

            // Initialize each row with different sizes
            jaggedInt[0] = new int[2];  // 2 elements, all 0
            jaggedInt[1] = new int[5];  // 5 elements
            jaggedInt[2] = new int[3];  // 3 elements
            
            Console.WriteLine($"Row 0 length: {jaggedInt[0].Length}"); // 2
            Console.WriteLine($"Row 1 length: {jaggedInt[1].Length}"); // 5
            Console.WriteLine($"Row 2 length: {jaggedInt[2].Length}"); // 3

            // ── EXAMPLE 2: Initialize with values ─────────────────────
            // Create and fill in one statement
            int[][] numbers = new int[][] 
            {
                new int[] { 1, 2, 3 },      // Row 0: 3 elements
                new int[] { 4, 5 },        // Row 1: 2 elements
                new int[] { 6, 7, 8, 9 }   // Row 2: 4 elements
            };
            
            Console.WriteLine("Jagged numbers array:");
            for (int i = 0; i < numbers.Length; i++)
            {
                Console.WriteLine($"  Row {i}: {string.Join(", ", numbers[i])}");
            }
            // Output: Row 0: 1, 2, 3
            //         Row 1: 4, 5
            //         Row 2: 6, 7, 8, 9

            // ── EXAMPLE 3: Shorthand initialization ───────────────────
            // Can omit "new int[][]" in C#
            string[][] fruits = {
                new[] { "Apple", "Apricot" },       // Pitted fruits
                new[] { "Banana", "Orange" },      // Citrus
                new[] { "Cherry", "Mango" }        // Stone fruits
            };
            
            Console.WriteLine("\nFruit categories:");
            for (int i = 0; i < fruits.Length; i++)
            {
                Console.WriteLine($"  Category {i}: {string.Join(", ", fruits[i])}");
            }

            // ── REAL-WORLD EXAMPLE: Sales data by region ───────────────
            // Different regions have different number of products
            string[] regions = { "North", "South", "East", "West" };
            decimal[][] salesByRegion = new decimal[4][];
            
            // North: 5 products
            salesByRegion[0] = new decimal[] { 1200m, 850m, 2100m, 500m, 1500m };
            // South: 3 products  
            salesByRegion[1] = new decimal[] { 900m, 1100m, 750m };
            // East: 6 products
            salesByRegion[2] = new decimal[] { 800m, 950m, 1300m, 700m, 1600m, 400m };
            // West: 4 products
            salesByRegion[3] = new decimal[] { 1500m, 1200m, 800m, 950m };
            
            Console.WriteLine("\nSales by Region:");
            for (int r = 0; r < regions.Length; r++)
            {
                decimal total = 0;
                Console.WriteLine($"  {regions[r]} ({salesByRegion[r].Length} products):");
                for (int p = 0; p < salesByRegion[r].Length; p++)
                {
                    Console.WriteLine($"    Product {p + 1}: ${salesByRegion[r][p]:N0}");
                    total += salesByRegion[r][p];
                }
                Console.WriteLine($"    Total: ${total:N0}");
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Accessing Jagged Array Elements
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Basic access ────────────────────────────────
            int[][] scores = {
                new[] { 90, 85, 88 },
                new[] { 92, 87 },
                new[] { 78, 95, 82, 89 }
            };
            
            // Access first element of first row
            Console.WriteLine($"scores[0][0]: {scores[0][0]}"); // Output: 90
            
            // Access last element of third row
            Console.WriteLine($"scores[2][3]: {scores[2][3]}"); // Output: 89

            // ── EXAMPLE 2: Dynamic row length access ─────────────────
            int[][] varyingSizes = {
                new[] { 1, 2 },
                new[] { 3, 4, 5, 6 },
                new[] { 7, 8, 9 }
            };
            
            Console.WriteLine("\nProcess each row:");
            foreach (int[] row in varyingSizes)
            {
                Console.WriteLine($"  Row has {row.Length} elements, sum = {row.Sum()}");
            }

            // Use row-specific length in loop
            for (int i = 0; i < varyingSizes.Length; i++)
            {
                Console.WriteLine($"\nRow {i} elements:");
                for (int j = 0; j < varyingSizes[i].Length; j++)
                {
                    Console.WriteLine($"  [{i}][{j}] = {varyingSizes[i][j]}");
                }
            }

            // ── EXAMPLE 3: IndexOf with Array.BinarySearch (not directly applicable)
            // Finding elements in jagged requires nested search
            int[][] searchArray = {
                new[] { 10, 20, 30 },
                new[] { 40, 50 },
                new[] { 60, 70, 80, 90 }
            };
            
            // Search for value 50
            int searchValue = 50;
            bool found = false;
            
            for (int i = 0; i < searchArray.Length && !found; i++)
            {
                for (int j = 0; j < searchArray[i].Length; j++)
                {
                    if (searchArray[i][j] == searchValue)
                    {
                        Console.WriteLine($"\nFound {searchValue} at [{i}][{j}]");
                        found = true;
                        break;
                    }
                }
            }
            
            if (!found)
            {
                Console.WriteLine($"\n{searchValue} not found");
            }

            // ── REAL-WORLD EXAMPLE: Employee tasks ───────────────────
            // Each employee has different number of tasks
            string[] employees = { "Alice", "Bob", "Charlie", "Diana" };
            string[][] employeeTasks = new string[4][];
            
            employeeTasks[0] = new[] { "Design UI", "Code review", "Write docs" };
            employeeTasks[1] = new[] { "Fix bugs", "Deploy", "Test features", "Write tests" };
            employeeTasks[2] = new[] { "Meeting", "Planning" };
            employeeTasks[3] = new[] { "Data entry", "Reports", "Analytics", "Dashboard", "Export" };
            
            Console.WriteLine("\nEmployee Tasks:");
            for (int i = 0; i < employees.Length; i++)
            {
                Console.WriteLine($"  {employees[i]} ({employeeTasks[i].Length} tasks):");
                foreach (string task in employeeTasks[i])
                {
                    Console.WriteLine($"    - {task}");
                }
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Jagged Array Methods and Operations
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: GetLength vs Length ───────────────────────
            int[][] lengths = {
                new[] { 1, 2, 3 },
                new[] { 4, 5 },
                new[] { 6, 7, 8, 9 }
            };
            
            // GetLength(0) = number of rows (outer array length)
            Console.WriteLine($"Number of rows: {lengths.GetLength(0)}"); // 3
            
            // Length = total elements in ALL rows
            int totalElements = 0;
            for (int i = 0; i < lengths.Length; i++)
            {
                totalElements += lengths[i].Length;
            }
            Console.WriteLine($"Total elements: {totalElements}"); // 9

            // ── EXAMPLE 2: Array.Sort with custom comparison ──────────
            // Sort jagged array by row length
            int[][] unsorted = {
                new[] { 1, 2, 3, 4 },
                new[] { 5, 6 },
                new[] { 7, 8, 9 }
            };
            
            Console.WriteLine("\nBefore sort by row length:");
            foreach (var row in unsorted)
            {
                Console.Write($"[{string.Join(",", row)}] ");
            }
            
            // Sort by row length (shortest first)
            Array.Sort(unsorted, (a, b) => a.Length.CompareTo(b.Length));
            
            Console.WriteLine("\nAfter sort by row length:");
            foreach (var row in unsorted)
            {
                Console.Write($"[{string.Join(",", row)}] ");
            }

            // ── EXAMPLE 3: Array.Reverse on individual rows ───────────
            int[][] toReverse = {
                new[] { 1, 2, 3 },
                new[] { 4, 5 },
                new[] { 6, 7, 8, 9 }
            };
            
            Console.WriteLine("\nBefore reverse each row:");
            Console.WriteLine(string.Join(", ", toReverse[0]));
            
            Array.Reverse(toReverse[0]); // Reverse just the first row
            Console.WriteLine("After reverse first row:");
            Console.WriteLine(string.Join(", ", toReverse[0])); // Output: 3, 2, 1

            // Reverse entire outer array
            Array.Reverse(toReverse);
            // Now order is: row 2, row 1, row 0

            // ── REAL-WORLD EXAMPLE: Student grades by subject ─────────
            string[] subjects = { "Math", "Science", "History", "English" };
            int[][] studentScores = new int[5][]; // 5 students
            
            // Each student has different number of assignments per subject
            Random rand = new Random();
            for (int s = 0; s < 5; s++)
            {
                int[] scoresPerSubject = new int[subjects.Length];
                for (int sub = 0; sub < subjects.Length; sub++)
                {
                    // Random number of assignments 3-6
                    int numAssignments = rand.Next(3, 7);
                    int[] assignments = new int[numAssignments];
                    for (int a = 0; a < numAssignments; a++)
                    {
                        assignments[a] = rand.Next(60, 100); // Random score 60-100
                    }
                    scoresPerSubject[sub] = assignments.Sum(); // Simplified - store sum
                }
                studentScores[s] = scoresPerSubject;
            }
            
            Console.WriteLine("\nStudent Scores:");
            for (int stu = 0; stu < 5; stu++)
            {
                Console.WriteLine($"  Student {stu + 1}:");
                for (int sub = 0; sub < subjects.Length; sub++)
                {
                    Console.WriteLine($"    {subjects[sub]}: {studentScores[stu][sub]}");
                }
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Mixing Jagged and Multi-Dimensional
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Array of 2D arrays ────────────────────────
            // Each element is a 2D matrix
            int[][,] matrixArray = new int[2][,]; // Array of 2 2D arrays
            
            matrixArray[0] = new int[2, 2] { { 1, 2 }, { 3, 4 } };
            matrixArray[1] = new int[3, 3] { 
                { 1, 2, 3 }, 
                { 4, 5, 6 }, 
                { 7, 8, 9 } 
            };
            
            Console.WriteLine("Array of 2D matrices:");
            for (int i = 0; i < matrixArray.Length; i++)
            {
                Console.WriteLine($"Matrix {i}: {matrixArray[i].GetLength(0)}x{matrixArray[i].GetLength(1)}");
            }

            // ── EXAMPLE 2: 2D array of arrays ────────────────────────
            // Each cell contains an array (variable-length data)
            string[][,] dataByRegion = new string[2][,]; // 2 regions
            
            // Region 0: 2 departments with variable employees
            dataByRegion[0] = new string[2, 3]; // 2 rows (depts), 3 columns (positions)
            dataByRegion[0][0, 0] = "Manager";
            dataByRegion[0][0, 1] = "Director";
            dataByRegion[0][0, 2] = "VP";
            dataByRegion[0][1, 0] = "Lead";
            dataByRegion[0][1, 1] = "Senior";
            dataByRegion[0][1, 2] = "Junior";
            
            // Region 1: different structure
            dataByRegion[1] = new string[3, 2];
            dataByRegion[1][0, 0] = "CEO";
            dataByRegion[1][0, 1] = "COO";
            
            Console.WriteLine("\n2D array of strings (positions):");
            for (int r = 0; r < 2; r++)
            {
                Console.WriteLine($"Region {r}:");
                for (int row = 0; row < dataByRegion[r].GetLength(0); row++)
                {
                    Console.Write("  Row " + row + ": ");
                    for (int col = 0; col < dataByRegion[r].GetLength(1); col++)
                    {
                        string val = dataByRegion[r][row, col] ?? "-";
                        Console.Write($"{val} ");
                    }
                    Console.WriteLine();
                }
            }

            // ── REAL-WORLD EXAMPLE: Store inventory with variants ─────
            // Product categories, each with variable color variants
            string[] categories = { "Shirts", "Pants", "Shoes" };
            string[][][] inventory = new string[3][][]; // 3 categories
            
            // Shirts: 3 styles, each with different colors
            inventory[0] = new string[3][];
            inventory[0][0] = new[] { "Red", "Blue", "White", "Black" };
            inventory[0][1] = new[] { "Green", "Yellow" };
            inventory[0][2] = new[] { "Black", "White", "Navy", "Gray", "Red" };
            
            // Pants: 2 styles
            inventory[1] = new string[2][];
            inventory[1][0] = new[] { "Blue", "Black", "Khaki" };
            inventory[1][1] = new[] { "Brown", "Gray", "Beige", "Navy" };
            
            // Shoes: 4 styles
            inventory[2] = new string[4][];
            inventory[2][0] = new[] { "Black", "Brown" };
            inventory[2][1] = new[] { "White", "Black", "Red" };
            inventory[2][2] = new[] { "Gray" };
            inventory[2][3] = new[] { "Black", "Brown", "Tan" };
            
            Console.WriteLine("\nInventory by Category:");
            for (int cat = 0; cat < categories.Length; cat++)
            {
                Console.WriteLine($"  {categories[cat]}:");
                for (int style = 0; style < inventory[cat].Length; style++)
                {
                    Console.WriteLine($"    Style {style + 1}: {string.Join(", ", inventory[cat][style])}");
                }
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Jagged Array Performance Benefits
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Memory efficiency ─────────────────────────
            // Jagged allows different sized rows - no wasted space
            // vs rectangular array which reserves fixed size for all rows
            
            // Jagged: exactly what's needed
            int[][] efficient = {
                new[] { 1 },                           // 1 int
                new[] { 1, 2 },                       // 2 ints
                new[] { 1, 2, 3 },                    // 3 ints
                new[] { 1, 2, 3, 4 },                 // 4 ints
                new[] { 1, 2, 3, 4, 5 }              // 5 ints
            };
            // Total memory: 1+2+3+4+5 = 15 ints + 5 array headers
            
            // Same as rectangular would be 5x5 = 25 ints
            // plus array headers

            // ── EXAMPLE 2: Flexible data structures ──────────────────
            // Perfect for variable-length data like:
            // - Different length time series per category
            // - Sparse matrices
            // - Nested hierarchical data
            
            // Time series: stock prices per company (different trading days)
            string[] companies = { "Apple", "Microsoft", "Google" };
            double[][] stockPrices = {
                new double[] { 150.25, 151.50, 152.75, 153.00, 154.25 }, // Apple - 5 days
                new double[] { 280.00, 282.50 }, // Microsoft - 2 days
                new double[] { 2800.00, 2810.00, 2795.00, 2820.00, 2830.00, 2840.00 } // Google - 6 days
            };
            
            Console.WriteLine("\nStock Prices (variable history):");
            for (int i = 0; i < companies.Length; i++)
            {
                Console.WriteLine($"  {companies[i]}: {string.Join(", ", stockPrices[i].Select(p => $"${p:F2}"))}");
                Console.WriteLine($"    Days tracked: {stockPrices[i].Length}, Avg: ${stockPrices[i].Average():F2}");
            }

            // ── REAL-WORLD EXAMPLE: Graph adjacency list ──────────────
            // Perfect representation for sparse graphs
            int numNodes = 6;
            int[][] adjacencyList = new int[numNodes][];
            
            // Node 0 connects to 1, 2
            adjacencyList[0] = new[] { 1, 2 };
            // Node 1 connects to 0, 3
            adjacencyList[1] = new[] { 0, 3 };
            // Node 2 connects to 0, 4
            adjacencyList[2] = new[] { 0, 4 };
            // Node 3 connects to 1, 5
            adjacencyList[3] = new[] { 1, 5 };
            // Node 4 connects to 2, 5
            adjacencyList[4] = new[] { 2, 5 };
            // Node 5 connects to 3, 4
            adjacencyList[5] = new[] { 3, 4 };
            
            Console.WriteLine("\nGraph Adjacency List:");
            for (int node = 0; node < numNodes; node++)
            {
                string connections = adjacencyList[node].Length > 0 
                    ? string.Join(", ", adjacencyList[node]) 
                    : "(isolated)";
                Console.WriteLine($"  Node {node} -> {connections}");
            }

            Console.WriteLine("\n=== Jagged Arrays Complete ===");
        }
    }
}