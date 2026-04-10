/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Loops - Real World Applications
 * FILE      : Loops_RealWorld.cs
 * PURPOSE   : This file demonstrates practical, real-world uses of loops in production applications.
 * ============================================================
 */

// --- SECTION: Real-World Loop Applications ---
// This file demonstrates loops in production scenarios

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._01_Fundamentals._06_Loops
{
    class Loops_RealWorld
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Data Processing
            // ═══════════════════════════════════════════════════════════════
            
            // ── CSV parsing ───────────────────────────────────────────────
            Console.WriteLine("=== CSV Processing ===");
            
            var csvData = "name,age,city\nAlice,30,NYC\nBob,25,LA\nCharlie,35,Chicago";
            var lines = csvData.Split('\n');
            
            var headers = lines[0].Split(',');
            Console.WriteLine($"Headers: {string.Join(", ", headers)}");
            
            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                
                if (values.Length >= headers.Length)
                {
                    for (int j = 0; j < headers.Length; j++)
                    {
                        Console.WriteLine($"{headers[j]}: {values[j]}");
                    }
                    Console.WriteLine("---");
                }
            }
            
            // ── Aggregation ───────────────────────────────────────────────
            var transactions = new[] {
                (Id: 1, Amount: 100.0, Type: "debit"),
                (Id: 2, Amount: 50.0, Type: "credit"),
                (Id: 3, Amount: 200.0, Type: "debit"),
                (Id: 4, Amount: 75.0, Type: "credit")
            };
            
            Console.WriteLine("\n=== Transaction Aggregation ===");
            
            decimal totalDebits = 0;
            decimal totalCredits = 0;
            int debitCount = 0;
            
            foreach (var tx in transactions)
            {
                if (tx.Type == "debit")
                {
                    totalDebits += tx.Amount;
                    debitCount++;
                }
                else
                {
                    totalCredits += tx.Amount;
                }
            }
            
            Console.WriteLine($"Debits: ${totalDebits} ({debitCount} transactions)");
            Console.WriteLine($"Credits: ${totalCredits}");
            Console.WriteLine($"Net: ${totalCredits - totalDebits}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Search and Filter
            // ═══════════════════════════════════════════════════════════════
            
            // ── User search ───────────────────────────────────────────────
            var users = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object> { ["id"] = 1, ["name"] = "Alice", ["active"] = true },
                new Dictionary<string, object> { ["id"] = 2, ["name"] = "Bob", ["active"] = false },
                new Dictionary<string, object> { ["id"] = 3, ["name"] = "Charlie", ["active"] = true }
            };
            
            Console.WriteLine("\n=== Active Users ===");
            
            foreach (var user in users)
            {
                if ((bool)user["active"])
                {
                    Console.WriteLine($"User: {user["name"]} (ID: {user["id"]})");
                }
            }
            
            // ── Find with multiple conditions ─────────────────────────────
            Console.WriteLine("\n=== Find with Conditions ===");
            
            var products = new[]
            {
                (Id: 1, Name: "Laptop", Price: 999.0, InStock: true),
                (Id: 2, Name: "Mouse", Price: 29.99, InStock: false),
                (Id: 3, Name: "Keyboard", Price: 79.99, InStock: true),
                (Id: 4, Name: "Monitor", Price: 299.99, InStock: true)
            };
            
            // Find affordable in-stock items
            foreach (var product in products)
            {
                if (product.InStock && product.Price < 100)
                {
                    Console.WriteLine($"Found: {product.Name} - ${product.Price}");
                }
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Pagination
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Pagination ===");
            
            var allRecords = new List<int>();
            for (int i = 1; i <= 25; i++)
                allRecords.Add(i);
            
            int pageSize = 10;
            int currentPage = 2;
            int totalPages = (int)Math.Ceiling((double)allRecords.Count / pageSize);
            
            int start = (currentPage - 1) * pageSize;
            int end = Math.Min(start + pageSize, allRecords.Count);
            
            Console.WriteLine($"Page {currentPage} of {totalPages}");
            Console.WriteLine("Records:");
            
            for (int i = start; i < end; i++)
            {
                Console.WriteLine($"  {allRecords[i]}");
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Tree/Graph Traversal
            // ═══════════════════════════════════════════════════════════════
            
            // Directory structure simulation
            Console.WriteLine("\n=== Directory Tree ===");
            
            var directory = new DirEntry("root")
            {
                Children = new List<DirEntry>
                {
                    new DirEntry("documents")
                    {
                        Children = new List<DirEntry>
                        {
                            new DirEntry("resume.pdf"),
                            new DirEntry("notes.txt")
                        }
                    },
                    new DirEntry("images")
                    {
                        Children = new List<DirEntry>
                        {
                            new DirEntry("photo.jpg"),
                            new DirEntry("logo.png")
                        }
                    },
                    new DirEntry("readme.txt")
                }
            };
            
            PrintDirectory(directory, "");
            
            // ── Flatten tree ───────────────────────────────────────────────
            var allFiles = new List<string>();
            FlattenDirectory(directory, allFiles);
            
            Console.WriteLine("\n=== All Files ===");
            foreach (var file in allFiles)
            {
                Console.WriteLine(file);
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Game Loop
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Game Loop ===");
            
            int playerHealth = 100;
            int ticks = 0;
            int maxTicks = 3;
            
            while (playerHealth > 0 && ticks < maxTicks)
            {
                ticks++;
                Console.WriteLine($"--- Tick {ticks} ---");
                
                // Simulate damage
                int damage = new Random().Next(10, 30);
                playerHealth -= damage;
                
                Console.WriteLine($"Player took {damage} damage. Health: {playerHealth}");
                
                if (playerHealth <= 0)
                {
                    Console.WriteLine("Game over!");
                    break;
                }
                
                // Check for victory
                if (ticks >= maxTicks)
                {
                    Console.WriteLine("Victory - survived all ticks!");
                }
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: File Processing
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== File Processing ===");
            
            var fileList = new[] { "data.csv", "report.txt", "image.png", "config.json" };
            
            int csvCount = 0;
            int txtCount = 0;
            int otherCount = 0;
            
            foreach (var file in fileList)
            {
                if (file.EndsWith(".csv"))
                    csvCount++;
                else if (file.EndsWith(".txt"))
                    txtCount++;
                else
                    otherCount++;
            }
            
            Console.WriteLine($"CSV: {csvCount}, TXT: {txtCount}, Other: {otherCount}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: API Response Processing
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== API Response ===");
            
            var apiResponse = new[]
            {
                new { Code = 200, Message = "Success", Data = new[] { "item1", "item2" } },
                new { Code = 404, Message = "Not Found", Data = new string[] { } },
                new { Code = 500, Message = "Error", Data = new string[] { } }
            };
            
            foreach (var response in apiResponse)
            {
                Console.WriteLine($"\nStatus: {response.Code} - {response.Message}");
                
                if (response.Code >= 200 && response.Code < 300 && response.Data != null)
                {
                    Console.WriteLine("Processing data:");
                    foreach (var item in response.Data)
                    {
                        Console.WriteLine($"  - {item}");
                    }
                }
                else if (response.Code >= 400)
                {
                    Console.WriteLine("Handling error...");
                }
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Report Generation
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Report Generation ===");
            
            var sales = new[]
            {
                (Product: "Widget", Qty: 100, Price: 9.99),
                (Product: "Gadget", Qty: 50, Price: 19.99),
                (Product: "Widget", Qty: 75, Price: 9.99),
                (Product: "Gizmo", Qty: 25, Price: 29.99)
            };
            
            var productTotals = new Dictionary<string, (int Qty, decimal Revenue)>();
            
            foreach (var sale in sales)
            {
                var key = sale.Product;
                var revenue = sale.Qty * sale.Price;
                
                if (productTotals.ContainsKey(key))
                {
                    var existing = productTotals[key];
                    productTotals[key] = (existing.Qty + sale.Qty, existing.Revenue + revenue);
                }
                else
                {
                    productTotals[key] = (sale.Qty, revenue);
                }
            }
            
            Console.WriteLine("Product Summary:");
            foreach (var product in productTotals)
            {
                Console.WriteLine($"{product.Key}: {product.Value.Qty} units, ${product.Value.Revenue:F2}");
            }
        }
        
        // ═══════════════════════════════════════════════════════════════
        // Helper classes and methods
        // ═══════════════════════════════════════════════════════════════
        
        class DirEntry
        {
            public string Name { get; }
            public List<DirEntry> Children { get; set; } = new();
            
            public DirEntry(string name)
            {
                Name = name;
            }
            
            public override string ToString() => Name;
        }
        
        static void PrintDirectory(DirEntry dir, string indent)
        {
            Console.WriteLine($"{indent}[{dir}]");
            
            foreach (var child in dir.Children)
            {
                PrintDirectory(child, indent + "  ");
            }
        }
        
        static void FlattenDirectory(DirEntry dir, List<string> files)
        {
            if (dir.Children.Count == 0)
            {
                files.Add(dir.Name);
            }
            else
            {
                foreach (var child in dir.Children)
                {
                    FlattenDirectory(child, files);
                }
            }
        }
    }
}
