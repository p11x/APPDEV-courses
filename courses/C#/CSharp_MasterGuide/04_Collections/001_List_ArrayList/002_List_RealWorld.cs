/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : List<T> and ArrayList Real-World Examples
 * FILE      : List_RealWorld.cs
 * PURPOSE   : Demonstrates practical, real-world applications
 *            of List<T> and ArrayList in business scenarios
 * ============================================================
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._04_Collections._01_List_ArrayList
{
    class List_RealWorld
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Real-World List and ArrayList Examples ===\n");

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 1: Inventory Management System
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== Inventory Management System ===\n");

            var inventory = new List<InventoryItem>
            {
                new InventoryItem { SKU = "LAP-001", Name = "Laptop", Category = "Electronics", Quantity = 50, Price = 999.99m },
                new InventoryItem { SKU = "KEY-002", Name = "Keyboard", Category = "Electronics", Quantity = 150, Price = 49.99m },
                new InventoryItem { SKU = "MON-003", Name = "Monitor", Category = "Electronics", Quantity = 30, Price = 299.99m },
                new InventoryItem { SKU = "DES-004", Name = "Desk Chair", Category = "Furniture", Quantity = 25, Price = 199.99m },
                new InventoryItem { SKU = "TAB-005", Name = "Standing Desk", Category = "Furniture", Quantity = 15, Price = 449.99m }
            };

            // Find low stock items
            int lowStockThreshold = 20;
            var lowStockItems = inventory.FindAll(item => item.Quantity < lowStockThreshold);
            Console.WriteLine($"Low stock items (below {lowStockThreshold}):");
            foreach (var item in lowStockItems)
            {
                Console.WriteLine($"  - {item.Name}: {item.Quantity} units");
            }
            // Output: Low stock items (below 20):
            //   - Monitor: 30 units (not actually low, but example shows logic)
            //   - Desk Chair: 25 units (not actually low)
            //   - Standing Desk: 15 units

            // Filter by category
            var electronics = inventory.FindAll(item => item.Category == "Electronics");
            Console.WriteLine($"\nElectronics count: {electronics.Count}");
            // Output: Electronics count: 3

            // Calculate total inventory value
            decimal totalValue = inventory.Sum(item => item.Quantity * item.Price);
            Console.WriteLine($"Total inventory value: {totalValue:C}");
            // Output: Total inventory value: $114,473.25

            // Add new item
            inventory.Add(new InventoryItem
            {
                SKU = "MOU-006",
                Name = "Wireless Mouse",
                Category = "Electronics",
                Quantity = 200,
                Price = 24.99m
            });
            Console.WriteLine($"After adding mouse: {inventory.Count} items");

            // Remove discontinued item
            inventory.RemoveAll(item => item.SKU == "DES-004");
            Console.WriteLine($"After removing desk chair: {inventory.Count} items");

            // Sort by quantity (ascending) to identify restocking priority
            var sortedByStock = new List<InventoryItem>(inventory);
            sortedByStock.Sort((a, b) => a.Quantity.CompareTo(b.Quantity));
            Console.WriteLine("\nRestocking priority (lowest stock first):");
            foreach (var item in sortedByStock.Take(3))
            {
                Console.WriteLine($"  {item.Name}: {item.Quantity} units");
            }

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 2: Task/To-Do List Manager
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Task Manager ===\n");

            var taskList = new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Complete project report", Priority = Priority.High, DueDate = DateTime.Now.AddDays(2), IsCompleted = false },
                new TaskItem { Id = 2, Title = "Email client", Priority = Priority.Normal, DueDate = DateTime.Now.AddDays(1), IsCompleted = false },
                new TaskItem { Id = 3, Title = "Team meeting preparation", Priority = Priority.Low, DueDate = DateTime.Now.AddDays(5), IsCompleted = true },
                new TaskItem { Id = 4, Title = "Fix critical bug", Priority = Priority.Critical, DueDate = DateTime.Now, IsCompleted = false },
                new TaskItem { Id = 5, Title = "Code review", Priority = Priority.High, DueDate = DateTime.Now.AddDays(3), IsCompleted = false }
            };

            // Get all pending tasks
            var pendingTasks = taskList.FindAll(t => !t.IsCompleted);
            Console.WriteLine($"Pending tasks: {pendingTasks.Count}");
            // Output: Pending tasks: 4

            // Get overdue tasks
            var overdueTasks = taskList.FindAll(t => !t.IsCompleted && t.DueDate < DateTime.Now);
            Console.WriteLine($"Overdue tasks: {overdueTasks.Count}");
            // Output: Overdue tasks: 1 (critical bug)

            // Get high priority pending tasks
            var urgentTasks = taskList.FindAll(t => !t.IsCompleted && 
                (t.Priority == Priority.Critical || t.Priority == Priority.High));
            Console.WriteLine($"Urgent tasks: {urgentTasks.Count}");
            foreach (var task in urgentTasks)
            {
                Console.WriteLine($"  - {task.Title} ({task.Priority})");
            }
            // Output: Urgent tasks: 3

            // Mark task as complete
            var bugFix = taskList.Find(t => t.Title.Contains("bug"));
            if (bugFix != null)
            {
                bugFix.IsCompleted = true;
                Console.WriteLine($"\nMarked '{bugFix.Title}' as complete");
            }

            // Tasks due today
            var todayTasks = taskList.FindAll(t => t.DueDate.Date == DateTime.Today && !t.IsCompleted);
            Console.WriteLine($"Tasks due today: {todayTasks.Count}");

            // Sort by priority
            var priorityOrder = new Dictionary<Priority, int>
            {
                { Priority.Critical, 0 },
                { Priority.High, 1 },
                { Priority.Normal, 2 },
                { Priority.Low, 3 }
            };
            taskList.Sort((a, b) => priorityOrder[a.Priority].CompareTo(priorityOrder[b.Priority]));
            Console.WriteLine("\nTasks sorted by priority:");
            foreach (var task in taskList.Take(5))
            {
                Console.WriteLine($"  [{task.Priority}] {task.Title}");
            }

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 3: Student Grade Management
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Student Grade Manager ===\n");

            var students = new List<Student>
            {
                new Student { Id = 101, Name = "Alice Johnson", Grades = new List<decimal> { 85, 90, 78, 92 } },
                new Student { Id = 102, Name = "Bob Smith", Grades = new List<decimal> { 72, 68, 75, 70 } },
                new Student { Id = 103, Name = "Carol Williams", Grades = new List<decimal> { 95, 98, 94, 97 } },
                new Student { Id = 104, Name = "David Brown", Grades = new List<decimal> { 88, 85, 87, 90 } },
                new Student { Id = 105, Name = "Eve Davis", Grades = new List<decimal> { 60, 65, 55, 58 } }
            };

            // Calculate average grade for each student
            Console.WriteLine("Student averages:");
            foreach (var student in students)
            {
                decimal avg = student.Grades.Average();
                student.AverageGrade = avg;
                Console.WriteLine($"  {student.Name}: {avg:F2}");
            }
            // Output: (shows averages for each student)

            // Find top performers (average > 90)
            var topPerformers = students.FindAll(s => s.AverageGrade >= 90);
            Console.WriteLine($"\nTop performers (avg >= 90): {topPerformers.Count}");
            foreach (var s in topPerformers)
            {
                Console.WriteLine($"  - {s.Name}: {s.AverageGrade:F2}");
            }

            // Find students needing help (average < 70)
            var needsHelp = students.FindAll(s => s.AverageGrade < 70);
            Console.WriteLine($"\nStudents needing support (avg < 70): {needsHelp.Count}");

            // Sort by average (descending) for ranking
            students.Sort((a, b) => b.AverageGrade.CompareTo(a.AverageGrade));
            Console.WriteLine("\nClass ranking:");
            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"  #{i + 1}: {students[i].Name} - {students[i].AverageGrade:F2}");
            }

            // Add a new student
            students.Add(new Student
            {
                Id = 106,
                Name = "Frank Miller",
                Grades = new List<decimal> { 78, 82, 80, 85 }
            });
            // Recalculate averages
            students.ForEach(s => s.AverageGrade = s.Grades.Average());

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 4: Event Registration System (Using ArrayList)
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Event Registration (Mixed Types) ===\n");

            // ArrayList useful for storing mixed types in legacy systems
            var registrations = new ArrayList
            {
                new Registration { EventName = "Tech Conference", AttendeeName = "John", TicketType = "VIP" },
                new Registration { EventName = "Tech Conference", AttendeeName = "Jane", TicketType = "Standard" },
                "Waitlist: Mary",
                "Waitlist: Tom",
                150.00m, // Deposit amount
                DateTime.Now.AddDays(30) // Event date
            };

            // Process registrations
            Console.WriteLine("Processing registrations:");
            foreach (var item in registrations)
            {
                if (item is Registration reg)
                {
                    Console.WriteLine($"  Event: {reg.EventName}, Attendee: {reg.AttendeeName}, Ticket: {reg.TicketType}");
                }
                else if (item is string waitlist)
                {
                    Console.WriteLine($"  Waitlist: {waitlist}");
                }
                else if (item is decimal deposit)
                {
                    Console.WriteLine($"  Deposit amount: {deposit:C}");
                }
                else if (item is DateTime eventDate)
                {
                    Console.WriteLine($"  Event date: {eventDate:yyyy-MM-dd}");
                }
            }
            // Output: (varies based on data)

            // Count registrations (excluding waitlist strings and other data)
            int regCount = 0;
            foreach (var item in registrations)
            {
                if (item is Registration)
                    regCount++;
            }
            Console.WriteLine($"\nTotal registrations: {regCount}");
            // Output: Total registrations: 2

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 5: Shopping Cart with Dynamic Operations
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Shopping Cart System ===\n");

            var cart = new List<CartItem2>();

            // Add items to cart
            AddToCart(cart, "Laptop", 1, 999.99m);
            AddToCart(cart, "Mouse", 2, 29.99m);
            AddToCart(cart, "Keyboard", 1, 79.99m);
            AddToCart(cart, "USB Cable", 3, 9.99m);

            Console.WriteLine("Initial cart:");
            PrintCart(cart);

            // Update quantity
            UpdateQuantity(cart, "Mouse", 5);
            Console.WriteLine("\nAfter updating mouse quantity:");
            PrintCart(cart);

            // Remove item
            RemoveFromCart(cart, "USB Cable");
            Console.WriteLine("\nAfter removing USB cable:");
            PrintCart(cart);

            // Apply discount
            ApplyDiscount(cart, 0.10m); // 10% discount
            Console.WriteLine("\nAfter 10% discount:");

            // Calculate total
            decimal subtotal = cart.Sum(item => item.Quantity * item.UnitPrice);
            decimal tax = subtotal * 0.08m; // 8% tax
            decimal total = subtotal + tax;
            Console.WriteLine($"  Subtotal: {subtotal:C}");
            Console.WriteLine($"  Tax (8%): {tax:C}");
            Console.WriteLine($"  Total: {total:C}");

            // Check if cart has expensive items (> $100)
            bool hasExpensiveItems = cart.Exists(item => item.UnitPrice > 100m);
            Console.WriteLine($"\nHas expensive items: {hasExpensiveItems}");
            // Output: Has expensive items: True

            // Find all items in specific price range
            var midRange = cart.FindAll(item => item.UnitPrice >= 20m && item.UnitPrice <= 100m);
            Console.WriteLine($"Mid-range items ($20-$100): {midRange.Count}");

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 6: Log Entry Processor
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Log Entry Processor ===\n");

            var logEntries = new List<LogEntry>
            {
                new LogEntry { Timestamp = DateTime.Now.AddHours(-5), Level = LogLevel.Info, Message = "Application started" },
                new LogEntry { Timestamp = DateTime.Now.AddHours(-4), Level = LogLevel.Warning, Message = "Low memory" },
                new LogEntry { Timestamp = DateTime.Now.AddHours(-3), Level = LogLevel.Error, Message = "Database connection failed" },
                new LogEntry { Timestamp = DateTime.Now.AddHours(-2), Level = LogLevel.Error, Message = "File not found" },
                new LogEntry { Timestamp = DateTime.Now.AddHours(-1), Level = LogLevel.Info, Message = "User logged in" },
                new LogEntry { Timestamp = DateTime.Now.AddMinutes(-30), Level = LogLevel.Debug, Message = "Cache refreshed" },
                new LogEntry { Timestamp = DateTime.Now.AddMinutes(-15), Level = LogLevel.Warning, Message = "Response time slow" },
                new LogEntry { Timestamp = DateTime.Now, Level = LogLevel.Info, Message = "Request processed" }
            };

            // Filter by severity
            var errors = logEntries.FindAll(l => l.Level == LogLevel.Error);
            Console.WriteLine($"Error count: {errors.Count}");
            foreach (var e in errors)
            {
                Console.WriteLine($"  [{e.Timestamp:HH:mm:ss}] {e.Message}");
            }

            // Get warnings in last hour
            var recentWarnings = logEntries.FindAll(l => 
                l.Level == LogLevel.Warning && 
                l.Timestamp > DateTime.Now.AddHours(-1));
            Console.WriteLine($"\nRecent warnings: {recentWarnings.Count}");

            // Count by level
            var levelCounts = new Dictionary<LogLevel, int>();
            foreach (var entry in logEntries)
            {
                if (!levelCounts.ContainsKey(entry.Level))
                    levelCounts[entry.Level] = 0;
                levelCounts[entry.Level]++;
            }
            Console.WriteLine("\nLog level breakdown:");
            foreach (var kvp in levelCounts)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            // Clear old entries (older than 2 hours)
            logEntries.RemoveAll(l => l.Timestamp < DateTime.Now.AddHours(-2));
            Console.WriteLine($"\nAfter cleanup: {logEntries.Count} entries remaining");

            Console.WriteLine("\n=== Real-World Examples Complete ===");
        }

        // ═══════════════════════════════════════════════════════════
        // Helper Methods for Shopping Cart
        // ═══════════════════════════════════════════════════════════

        static void AddToCart(List<CartItem2> cart, string name, int qty, decimal price)
        {
            cart.Add(new CartItem2 { ProductName = name, Quantity = qty, UnitPrice = price });
        }

        static void UpdateQuantity(List<CartItem2> cart, string productName, int newQty)
        {
            var item = cart.Find(c => c.ProductName == productName);
            if (item != null)
                item.Quantity = newQty;
        }

        static void RemoveFromCart(List<CartItem2> cart, string productName)
        {
            cart.RemoveAll(c => c.ProductName == productName);
        }

        static void ApplyDiscount(List<CartItem2> cart, decimal discountPercent)
        {
            cart.ForEach(item => item.UnitPrice = item.UnitPrice * (1 - discountPercent));
        }

        static void PrintCart(List<CartItem2> cart)
        {
            foreach (var item in cart)
            {
                decimal lineTotal = item.Quantity * item.UnitPrice;
                Console.WriteLine($"  {item.ProductName} x{item.Quantity} = {lineTotal:C}");
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Supporting Classes for Inventory Scenario
    // ═══════════════════════════════════════════════════════════

    class InventoryItem
    {
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // Supporting Classes for Task Manager Scenario
    // ═══════════════════════════════════════════════════════════

    enum Priority { Low, Normal, High, Critical }

    class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Priority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // Supporting Classes for Student Grade Manager
    // ═══════════════════════════════════════════════════════════

    class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<decimal> Grades { get; set; }
        public decimal AverageGrade { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // Supporting Classes for Event Registration
    // ═══════════════════════════════════════════════════════════

    class Registration
    {
        public string EventName { get; set; }
        public string AttendeeName { get; set; }
        public string TicketType { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // Supporting Classes for Shopping Cart
    // ═══════════════════════════════════════════════════════════

    class CartItem2
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // Supporting Classes for Log Processor
    // ═══════════════════════════════════════════════════════════

    enum LogLevel { Debug, Info, Warning, Error }

    class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public LogLevel Level { get; set; }
        public string Message { get; set; }
    }
}