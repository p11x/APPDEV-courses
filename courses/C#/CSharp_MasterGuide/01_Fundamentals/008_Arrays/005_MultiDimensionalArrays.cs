/*
 * ============================================================
 * TOPIC     : Fundamentals - Arrays
 * SUBTOPIC  : Multi-Dimensional Arrays
 * FILE      : MultiDimensionalArrays.cs
 * PURPOSE   : Teaches multi-dimensional arrays in C# including
 *            2D, 3D arrays, and rectangular array operations
 * ============================================================
 */

using System; // Core System namespace for Console

namespace CSharp_MasterGuide._01_Fundamentals._08_Arrays
{
    class MultiDimensionalArrays
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Two-Dimensional Arrays Basics
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Declaring 2D arrays ────────────────────────
            // Rectangle array - same number of columns in each row
            int[,] matrix = new int[3, 4]; // 3 rows, 4 columns = 12 elements
            
            Console.WriteLine($"Matrix dimensions: {matrix.GetLength(0)} rows, {matrix.GetLength(1)} columns");
            Console.WriteLine($"Total elements: {matrix.Length}"); // Output: 12

            // Initialize with values at specific positions
            matrix[0, 0] = 1;
            matrix[0, 1] = 2;
            matrix[0, 2] = 3;
            matrix[0, 3] = 4;
            
            matrix[1, 0] = 5;
            matrix[1, 1] = 6;
            matrix[1, 2] = 7;
            matrix[1, 3] = 8;
            
            matrix[2, 0] = 9;
            matrix[2, 1] = 10;
            matrix[2, 2] = 11;
            matrix[2, 3] = 12;

            // ── EXAMPLE 2: Initialize 2D array inline ────────────────
            // All elements specified at declaration
            int[,] smallMatrix = { 
                { 1, 2 }, 
                { 3, 4 }, 
                { 5, 6 } 
            };
            
            Console.WriteLine($"3x2 Matrix:");
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    Console.Write($"{smallMatrix[row, col]} ");
                }
                Console.WriteLine();
            }
            // Output: 1 2
            //         3 4
            //         5 6

            // ── EXAMPLE 3: GetLength method ───────────────────────────
            int[,] data = { 
                { 10, 20, 30 },
                { 40, 50, 60 },
                { 70, 80, 90 },
                { 100, 110, 120 }
            };
            
            // GetLength(0) = rows, GetLength(1) = columns
            Console.WriteLine($"Rows: {data.GetLength(0)}"); // 4
            Console.WriteLine($"Columns: {data.GetLength(1)}"); // 3
            
            // Iterate using GetLength
            Console.WriteLine("\nAll elements:");
            for (int row = 0; row < data.GetLength(0); row++)
            {
                for (int col = 0; col < data.GetLength(1); col++)
                {
                    Console.Write($"{data[row, col]}\t");
                }
                Console.WriteLine();
            }

            // ── REAL-WORLD EXAMPLE: Classroom seating ──────────────────
            // 5x6 classroom with 30 seats
            string[,] seatingChart = new string[5, 6];
            
            // Fill with student names (simplified)
            string[] students = { "Alice", "Bob", "Charlie", "Diana", "Eve", "Frank",
                                 "Grace", "Henry", "Ivy", "Jack", "Kate", "Liam",
                                 "Mia", "Noah", "Olivia", "Peter", "Quinn", "Rose",
                                 "Sam", "Tina", "Uma", "Victor", "Wendy", "Xavier",
                                 "Yara", "Zack", "Amy", "Ben", "Clara", "David" };
            
            int studentIndex = 0;
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    if (studentIndex < students.Length)
                    {
                        seatingChart[row, col] = students[studentIndex];
                        studentIndex++;
                    }
                }
            }
            
            // Display seating chart
            Console.WriteLine("Classroom Seating:");
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    string seat = seatingChart[row, col] ?? "EMPTY";
                    Console.Write($"[{seat.PadRight(8)}] ");
                }
                Console.WriteLine();
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Three-Dimensional and Higher Arrays
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: 3D Array ────────────────────────────────────
            // [floor, row, seat] - like a theater with multiple floors
            int[,,] theater = new int[2, 3, 4]; // 2 floors, 3 rows, 4 seats each
            
            Console.WriteLine($"Theater: {theater.GetLength(0)} floors, {theater.GetLength(1)} rows, {theater.GetLength(2)} seats");
            Console.WriteLine($"Total seats: {theater.Length}"); // 24

            // Fill with seat prices
            // Floor 0 = Balcony ($50), Floor 1 = Main ($100)
            for (int floor = 0; floor < 2; floor++)
            {
                int basePrice = (floor == 0) ? 50 : 100;
                for (int row = 0; row < 3; row++)
                {
                    for (int seat = 0; seat < 4; seat++)
                    {
                        // Premium for front rows
                        int rowPremium = (row == 0) ? 20 : 0;
                        theater[floor, row, seat] = basePrice + rowPremium;
                    }
                }
            }
            
            Console.WriteLine("\nSeat Prices:");
            for (int floor = 0; floor < 2; floor++)
            {
                string floorName = (floor == 0) ? "Balcony" : "Main Floor";
                Console.WriteLine($"  {floorName}:");
                for (int row = 0; row < 3; row++)
                {
                    Console.Write($"    Row {row + 1}: ");
                    for (int seat = 0; seat < 4; seat++)
                    {
                        Console.Write($"${theater[floor, row, seat]} ");
                    }
                    Console.WriteLine();
                }
            }

            // ── EXAMPLE 2: 4D Array (complex example) ───────────────
            // [year, quarter, month, day] - calendar data
            // Simplified: 2 years, 4 quarters, 3 months, 30 days
            int[,,,] calendar = new int[2, 4, 3, 30];
            
            Console.WriteLine($"\nCalendar dimensions: {calendar.GetLength(0)} years x {calendar.GetLength(1)} quarters");
            
            // Just show structure, not fill all
            Console.WriteLine($"Total data points: {calendar.Length}");

            // ── REAL-WORLD EXAMPLE: Temperature by location/time ──────
            // [city, day, hour] - 3 cities, 7 days, 24 hours
            double[,,] cityTemps = new double[3, 7, 24];
            
            string[] cities = { "New York", "Los Angeles", "Chicago" };
            string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            
            // Simulate some temperatures (simplified pattern)
            Random rand = new Random(42);
            for (int city = 0; city < 3; city++)
            {
                double baseTemp = (city == 1) ? 25 : 20; // LA is warmer
                for (int day = 0; day < 7; day++)
                {
                    for (int hour = 0; hour < 24; hour++)
                    {
                        // Cooler at night, warmer midday
                        double hourFactor = Math.Sin((hour - 6) * Math.PI / 12) * 5;
                        cityTemps[city, day, hour] = baseTemp + hourFactor + rand.Next(-3, 4);
                    }
                }
            }
            
            // Display sample
            Console.WriteLine("\nSample Temperatures:");
            for (int city = 0; city < 3; city++)
            {
                Console.WriteLine($"  {cities[city]}:");
                for (int day = 0; day < 3; day++)
                {
                    Console.Write($"    {days[day]}: ");
                    for (int hour = 6; hour <= 18; hour += 4) // Morning, noon, afternoon, evening
                    {
                        Console.Write($"{cityTemps[city, day, hour]:F1}° ");
                    }
                    Console.WriteLine();
                }
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Jagged vs Rectangular Arrays
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Rectangular array (uniform) ───────────────
            // All rows have same length
            int[,] rectangular = new int[3, 4];
            // 12 elements, all accessible
            
            // ── EXAMPLE 2: Jagged array (non-uniform) ────────────────
            // Array of arrays - each row can have different length
            int[][] jagged = new int[3][]; // 3 rows, each row is an array
            
            // Initialize each row with different sizes
            jagged[0] = new int[2] { 1, 2 };       // 2 elements
            jagged[1] = new int[4] { 3, 4, 5, 6 }; // 4 elements
            jagged[2] = new int[3] { 7, 8, 9 };    // 3 elements
            
            Console.WriteLine("Jagged array structure:");
            for (int i = 0; i < jagged.Length; i++)
            {
                Console.Write($"Row {i}: ");
                for (int j = 0; j < jagged[i].Length; j++)
                {
                    Console.Write($"{jagged[i][j]} ");
                }
                Console.WriteLine();
            }
            // Output: Row 0: 1 2
            //         Row 1: 3 4 5 6
            //         Row 2: 7 8 9

            // ── EXAMPLE 3: Initialize jagged inline ─────────────────
            int[][] jaggedInline = new int[][] 
            {
                new int[] { 1, 2, 3 },
                new int[] { 4, 5 },
                new int[] { 6, 7, 8, 9 }
            };
            
            Console.WriteLine("\nInline jagged:");
            foreach (int[] row in jaggedInline)
            {
                Console.WriteLine($"  {string.Join(", ", row)}");

            // ── REAL-WORLD EXAMPLE: Employee schedule ─────────────────
            // Different departments have different shift patterns
            string[][] departmentSchedule = new string[4][];
            
            departmentSchedule[0] = new string[] { "9:00-17:00", "9:00-17:00", "9:00-17:00", "9:00-17:00", "9:00-17:00" }; // Engineering
            departmentSchedule[1] = new string[] { "8:00-16:00", "8:00-16:00", "8:00-16:00", "8:00-16:00", "8:00-16:00" }; // Sales
            departmentSchedule[2] = new string[] { "10:00-18:00", "10:00-18:00", "10:00-18:00", "10:00-18:00", "10:00-18:00" }; // Support
            departmentSchedule[3] = new string[] { "12:00-20:00", "12:00-20:00", "12:00-20:00", "Off", "Off" }; // Security
            
            string[] depts = { "Engineering", "Sales", "Support", "Security" };
            string[] weekdays = { "Mon", "Tue", "Wed", "Thu", "Fri" };
            
            Console.WriteLine("\nDepartment Schedules:");
            for (int d = 0; d < 4; d++)
            {
                Console.WriteLine($"  {depts[d]}:");
                for (int day = 0; day < 5; day++)
                {
                    Console.WriteLine($"    {weekdays[day]}: {departmentSchedule[d][day]}");
                }
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Array Rank and Iteration
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: GetRank ─────────────────────────────────────
            int[,] twoDim = new int[3, 3];
            int[,,] threeDim = new int[2, 3, 4];
            
            Console.WriteLine($"2D array rank: {twoDim.Rank}"); // 2
            Console.WriteLine($"3D array rank: {threeDim.Rank}"); // 3

            // ── EXAMPLE 2: Nested foreach iteration ───────────────────
            int[,] numbers = { 
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            };
            
            Console.WriteLine("\nUsing nested foreach:");
            foreach (int row in numbers)
            {
                Console.WriteLine($"  {row}"); // Iterates all elements in row-major order
            }
            // Output: 1, 2, 3, 4, 5, 6, 7, 8, 9 (flattened)

            // For true row/col iteration, use nested for
            Console.WriteLine("\nUsing nested for:");
            for (int r = 0; r < numbers.GetLength(0); r++)
            {
                for (int c = 0; c < numbers.GetLength(1); c++)
                {
                    Console.Write($"{numbers[r, c]} ");
                }
                Console.WriteLine();
            }

            // ── EXAMPLE 3: Iterate all dimensions dynamically ───────
            int[,,] sample3D = {
                {
                    {1, 2}, {3, 4}
                },
                {
                    {5, 6}, {7, 8}
                }
            };
            
            Console.WriteLine("\n3D array iteration:");
            for (int d0 = 0; d0 < sample3D.GetLength(0); d0++)
            {
                Console.WriteLine($"  Dimension 0 = {d0}:");
                for (int d1 = 0; d1 < sample3D.GetLength(1); d1++)
                {
                    Console.Write("    [");
                    for (int d2 = 0; d2 < sample3D.GetLength(2); d2++)
                    {
                        Console.Write($"{sample3D[d0, d1, d2]} ");
                    }
                    Console.WriteLine("]");
                }
            }

            // ── REAL-WORLD EXAMPLE: Matrix operations ─────────────────
            // Add two matrices
            int[,] matrixA = { { 1, 2 }, { 3, 4 } };
            int[,] matrixB = { { 5, 6 }, { 7, 8 } };
            int[,] matrixSum = new int[2, 2];
            
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < 2; c++)
                {
                    matrixSum[r, c] = matrixA[r, c] + matrixB[r, c];
                }
            }
            
            Console.WriteLine("\nMatrix Addition:");
            Console.WriteLine("A + B = ");
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < 2; c++)
                {
                    Console.Write($"{matrixSum[r, c]} ");
                }
                Console.WriteLine();
            }
            // Output: 6 8
            //         10 12

            Console.WriteLine("\n=== Multi-Dimensional Arrays Complete ===");
        }
    }
}