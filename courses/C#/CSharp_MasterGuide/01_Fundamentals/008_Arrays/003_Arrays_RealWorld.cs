/*
 * ============================================================
 * TOPIC     : Fundamentals - Arrays
 * SUBTOPIC  : Real-World Array Applications
 * FILE      : Arrays_RealWorld.cs
 * PURPOSE   : Demonstrates practical array applications in real-world
 *            scenarios including data processing, calculations, and patterns
 * ============================================================
 */

using System; // Core System namespace
using System.Linq; // For LINQ methods
using System.Text; // For StringBuilder

namespace CSharp_MasterGuide._01_Fundamentals._08_Arrays
{
    class Arrays_RealWorld
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // APPLICATION 1: Student Grade Book
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== Student Grade Book ===\n");

            // Data: Name, grades for 5 subjects
            string[] studentNames = { "Alice", "Bob", "Charlie", "Diana", "Eve" };
            string[] subjects = { "Math", "Science", "English", "History", "Art" };
            int[,] grades = {
                { 85, 92, 78, 88, 95 },
                { 90, 88, 82, 91, 87 },
                { 75, 80, 85, 78, 82 },
                { 95, 98, 92, 96, 90 },
                { 88, 85, 90, 82, 89 }
            };

            // Calculate average per student
            Console.WriteLine("Student Averages:");
            for (int s = 0; s < studentNames.Length; s++)
            {
                double sum = 0;
                for (int sub = 0; sub < subjects.Length; sub++)
                {
                    sum += grades[s, sub];
                }
                double avg = sum / subjects.Length;
                Console.WriteLine($"  {studentNames[s]}: {avg:F1}");
            }

            // Calculate average per subject
            Console.WriteLine("\nSubject Averages:");
            for (int sub = 0; sub < subjects.Length; sub++)
            {
                double sum = 0;
                for (int s = 0; s < studentNames.Length; s++)
                {
                    sum += grades[s, sub];
                }
                double avg = sum / studentNames.Length;
                Console.WriteLine($"  {subjects[sub]}: {avg:F1}");
            }

            // Find highest in each subject
            Console.WriteLine("\nHighest Scores:");
            for (int sub = 0; sub < subjects.Length; sub++)
            {
                int highest = grades[0, sub];
                int bestStudent = 0;
                for (int s = 1; s < studentNames.Length; s++)
                {
                    if (grades[s, sub] > highest)
                    {
                        highest = grades[s, sub];
                        bestStudent = s;
                    }
                }
                Console.WriteLine($"  {subjects[sub]}: {studentNames[bestStudent]} ({highest})");
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 2: Monthly Sales Analysis
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Monthly Sales Analysis ===\n");

            string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                               "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            
            // 3 products, 12 months
            string[] products = { "Product A", "Product B", "Product C" };
            decimal[,] sales = {
                { 12000, 11500, 13000, 14000, 13500, 15000, 14500, 16000, 15500, 17000, 18000, 19000 },
                { 8000, 8500, 8200, 8800, 9000, 9500, 9200, 9800, 10200, 10500, 11000, 11500 },
                { 5000, 5200, 4800, 5500, 5800, 6000, 5800, 6200, 6500, 6800, 7200, 7500 }
            };

            // Total sales per product
            Console.WriteLine("Total Sales by Product:");
            for (int p = 0; p < products.Length; p++)
            {
                decimal total = 0;
                for (int m = 0; m < 12; m++)
                {
                    total += sales[p, m];
                }
                Console.WriteLine($"  {products[p]}: ${total:N0}");
            }

            // Best month for each product
            Console.WriteLine("\nBest Month per Product:");
            for (int p = 0; p < products.Length; p++)
            {
                decimal best = sales[p, 0];
                int bestMonth = 0;
                for (int m = 1; m < 12; m++)
                {
                    if (sales[p, m] > best)
                    {
                        best = sales[p, m];
                        bestMonth = m;
                    }
                }
                Console.WriteLine($"  {products[p]}: {months[bestMonth]} (${best:N0})");
            }

            // Month-over-month growth
            Console.WriteLine("\nMonth-over-Month Growth (Product A):");
            for (int m = 1; m < 12; m++)
            {
                decimal prev = sales[0, m - 1];
                decimal curr = sales[0, m];
                decimal growth = ((curr - prev) / prev) * 100;
                Console.WriteLine($"  {months[m]}: {growth:+0.0;-0.0}%");
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 3: Inventory Management
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Inventory Management ===\n");

            // Warehouse inventory by category and item
            string[] categories = { "Electronics", "Furniture", "Clothing" };
            string[][] itemNames = {
                new[] { "Laptop", "Phone", "Tablet", "Monitor" },
                new[] { "Chair", "Table", "Desk", "Shelf", "Sofa" },
                new[] { "Shirt", "Pants", "Jacket", "Shoes" }
            };
            int[,] stock = {
                { 50, 100, 30, 25 },
                { 20, 15, 10, 30, 8 },
                { 200, 150, 75, 100 }
            };
            decimal[,] prices = {
                { 999.99m, 699.99m, 449.99m, 299.99m },
                { 149.99m, 199.99m, 249.99m, 99.99m, 599.99m },
                { 29.99m, 49.99m, 89.99m, 79.99m }
            };

            // Total inventory value per category
            Console.WriteLine("Inventory Value by Category:");
            for (int cat = 0; cat < categories.Length; cat++)
            {
                decimal total = 0;
                for (int item = 0; item < itemNames[cat].Length; item++)
                {
                    total += stock[cat, item] * prices[cat, item];
                }
                Console.WriteLine($"  {categories[cat]}: ${total:N2}");
            }

            // Low stock alerts
            Console.WriteLine("\nLow Stock Alerts (threshold: 20):");
            for (int cat = 0; cat < categories.Length; cat++)
            {
                for (int item = 0; item < itemNames[cat].Length; item++)
                {
                    if (stock[cat, item] < 20)
                    {
                        Console.WriteLine($"  {categories[cat]} - {itemNames[cat][item]}: {stock[cat, item]} units");
                    }
                }
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 4: Game Score Board
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Game Scoreboard ===\n");

            // 4 players, 5 rounds
            string[] players = { "Player 1", "Player 2", "Player 3", "Player 4" };
            int[,] scores = new int[4, 5];
            
            // Simulate random scores
            Random rand = new Random(42);
            for (int p = 0; p < 4; p++)
            {
                int cumulative = 0;
                for (int r = 0; r < 5; r++)
                {
                    int roundScore = rand.Next(10, 50);
                    scores[p, r] = roundScore;
                    cumulative += roundScore;
                }
            }

            // Display scores by round
            Console.WriteLine("Scores by Round:");
            Console.Write("Round    ");
            foreach (string player in players)
            {
                Console.Write($"{player.PadRight(10)}");
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', 45));

            for (int r = 0; r < 5; r++)
            {
                Console.Write($"Round {r + 1}: ");
                for (int p = 0; p < 4; p++)
                {
                    Console.Write($"{scores[p, r].ToString().PadRight(10)}");
                }
                Console.WriteLine();
            }

            // Total scores
            Console.WriteLine("\nTotal Scores:");
            int[] totals = new int[4];
            for (int p = 0; p < 4; p++)
            {
                for (int r = 0; r < 5; r++)
                {
                    totals[p] += scores[p, r];
                }
            }

            // Sort by score
            var playerScores = new (string Name, int Score)[4];
            for (int i = 0; i < 4; i++)
            {
                playerScores[i] = (players[i], totals[i]);
            }
            Array.Sort(playerScores, (a, b) => b.Score.CompareTo(a.Score));

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"  #{i + 1}: {playerScores[i].Name} - {playerScores[i].Score} pts");
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 5: Calendar/Schedule System
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Weekly Schedule ===\n");

            string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri" };
            string[] timeSlots = { "9AM", "10AM", "11AM", "12PM", "1PM", "2PM", "3PM", "4PM" };
            
            // 5 days x 8 time slots
            string[,] schedule = new string[5, 8];
            
            // Fill with meetings
            schedule[0, 0] = "Team Standup";
            schedule[0, 2] = "Client Call";
            schedule[1, 1] = "Code Review";
            schedule[1, 4] = "Lunch Break";
            schedule[2, 3] = "Sprint Planning";
            schedule[3, 0] = "Tech Talk";
            schedule[3, 5] = "1:1 with Manager";
            schedule[4, 6] = "Retro";

            // Display schedule
            Console.Write("Time    ");
            foreach (string day in days)
            {
                Console.Write($"{day.PadRight(12)}");
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', 65));

            for (int t = 0; t < timeSlots.Length; t++)
            {
                Console.Write($"{timeSlots[t].PadRight(8)}");
                for (int d = 0; d < days.Length; d++)
                {
                    string meeting = schedule[d, t] ?? "-";
                    Console.Write($"{meeting.PadRight(12)}");
                }
                Console.WriteLine();
            }

            // Count meetings per day
            Console.WriteLine("\nMeetings per Day:");
            for (int d = 0; d < days.Length; d++)
            {
                int count = 0;
                for (int t = 0; t < timeSlots.Length; t++)
                {
                    if (schedule[d, t] != null) count++;
                }
                Console.WriteLine($"  {days[d]}: {count} meetings");
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 6: Temperature Monitoring
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Temperature Monitoring ===\n");

            // 7 days, 24 hours
            int[,] temps = new int[7, 24];
            
            // Simulate temperature data (reasonable ranges for demo)
            for (int day = 0; day < 7; day++)
            {
                for (int hour = 0; hour < 24; hour++)
                {
                    // Cooler at night (0-6), warmer midday (10-16)
                    double baseTemp = 20;
                    double hourFactor = Math.Sin((hour - 6) * Math.PI / 12) * 8;
                    double dayVariation = Math.Sin(day * Math.PI / 7) * 3;
                    temps[day, hour] = (int)(baseTemp + hourFactor + dayVariation + rand.Next(-3, 4));
                }
            }

            // Daily averages
            Console.WriteLine("Daily Averages:");
            string[] dayNames = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            for (int d = 0; d < 7; d++)
            {
                int sum = 0;
                for (int h = 0; h < 24; h++)
                {
                    sum += temps[d, h];
                }
                double avg = sum / 24.0;
                Console.WriteLine($"  {dayNames[d]}: {avg:F1}°C");
            }

            // Hottest time of day
            int[] hourlyAvg = new int[24];
            for (int h = 0; h < 24; h++)
            {
                int sum = 0;
                for (int d = 0; d < 7; d++)
                {
                    sum += temps[d, h];
                }
                hourlyAvg[h] = sum / 7;
            }
            
            int hottestHour = 0;
            for (int h = 1; h < 24; h++)
            {
                if (hourlyAvg[h] > hourlyAvg[hottestHour])
                    hottestHour = h;
            }
            
            string hottestTime = hottestHour switch
            {
                < 12 => $"{hottestHour}AM",
                12 => "Noon",
                _ => $"{hottestHour - 12}PM"
            };
            Console.WriteLine($"\nHottest time: {hottestTime} ({hourlyAvg[hottestHour]}°C avg)");

            // Coldest time
            int coldestHour = 0;
            for (int h = 1; h < 24; h++)
            {
                if (hourlyAvg[h] < hourlyAvg[coldestHour])
                    coldestHour = h;
            }
            
            string coldestTime = coldestHour switch
            {
                < 12 => $"{coldestHour}AM",
                12 => "Noon",
                _ => $"{coldestHour - 12}PM"
            };
            Console.WriteLine($"Coldest time: {coldestTime} ({hourlyAvg[coldestHour]}°C avg)");

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 7: Matrix Math Utilities
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Matrix Operations ===\n");

            // 2D identity matrix
            int size = 4;
            int[,] identity = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                identity[i, i] = 1;
            }

            Console.WriteLine("Identity Matrix (4x4):");
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    Console.Write($"{identity[r, c]} ");
                }
                Console.WriteLine();
            }

            // Add two matrices
            int[,] matrixA = { { 1, 2 }, { 3, 4 } };
            int[,] matrixB = { { 5, 6 }, { 7, 8 } };
            int[,] sumMatrix = new int[2, 2];
            
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < 2; c++)
                {
                    sumMatrix[r, c] = matrixA[r, c] + matrixB[r, c];
                }
            }

            Console.WriteLine("\nMatrix Addition:");
            Console.WriteLine("A + B =");
            for (int r = 0; r < 2; r++)
            {
                for (int c = 0; c < 2; c++)
                {
                    Console.Write($"{sumMatrix[r, c]} ");
                }
                Console.WriteLine();
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 8: Path Finding Grid
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Path Finding Grid ===\n");

            // 5x5 grid: 0 = passable, 1 = blocked
            int[,] grid = {
                { 0, 0, 1, 0, 0 },
                { 0, 1, 1, 0, 1 },
                { 0, 0, 0, 0, 0 },
                { 1, 0, 1, 0, 1 },
                { 0, 0, 0, 0, 0 }
            };

            Console.WriteLine("Grid (0=open, 1=blocked):");
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    Console.Write(grid[r, c] == 0 ? ". " : "# ");
                }
                Console.WriteLine();
            }

            // BFS to find path (simplified - find any path)
            Console.WriteLine("\nSimple path from (0,0) to (4,4):");
            
            // Just show accessible cells (flood fill)
            bool[,] visited = new bool[5, 5];
            var queue = new Queue<(int Row, int Col)>();
            queue.Enqueue((0, 0));
            visited[0, 0] = true;
            
            while (queue.Count > 0)
            {
                var (r, c) = queue.Dequeue();
                
                // Check neighbors
                int[] dr = { -1, 1, 0, 0 };
                int[] dc = { 0, 0, -1, 1 };
                
                for (int i = 0; i < 4; i++)
                {
                    int nr = r + dr[i];
                    int nc = c + dc[i];
                    
                    if (nr >= 0 && nr < 5 && nc >= 0 && nc < 5 && 
                        grid[nr, nc] == 0 && !visited[nr, nc])
                    {
                        visited[nr, nc] = true;
                        queue.Enqueue((nr, nc));
                    }
                }
            }

            Console.WriteLine("Accessible cells:");
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    if (visited[r, c])
                        Console.Write("O ");
                    else if (grid[r, c] == 1)
                        Console.Write("# ");
                    else
                        Console.Write(". ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n=== Arrays Real-World Complete ===");
        }
    }
}