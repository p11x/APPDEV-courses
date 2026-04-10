/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Generics - Generic Collections Part 2
 * FILE      : GenericCollections_Part2.cs
 * PURPOSE   : Teaches advanced collection operations, sorting,
 *            searching, and practical examples
 * ============================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._01_Generics
{
    class GenericCollections_Part2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Generic Collections Part 2 ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: List<T> Sorting and Searching
            // ═══════════════════════════════════════════════════════════

            // Sorting list of integers
            List<int> numbers = new List<int> { 5, 2, 8, 1, 9, 3 };
            numbers.Sort();
            Console.WriteLine($"Sorted numbers: {string.Join(", ", numbers)}");
            // Output: Sorted numbers: 1, 2, 3, 5, 8, 9

            // Reverse list
            numbers.Reverse();
            Console.WriteLine($"Reversed: {string.Join(", ", numbers)}");
            // Output: Reversed: 9, 8, 5, 3, 2, 1

            // Sorting complex objects with IComparer
            List<Product6> products = new List<Product6>
            {
                new Product6 { Name = "Laptop", Price = 999.99m },
                new Product6 { Name = "Phone", Price = 699.99m },
                new Product6 { Name = "Tablet", Price = 499.99m }
            };
            products.Sort(new ProductPriceComparer());
            Console.WriteLine("Products sorted by price:");
            foreach (var p in products)
            {
                Console.WriteLine($"  {p.Name}: ${p.Price}");
            }
            // Output:
            //   Tablet: $499.99
            //   Phone: $699.99
            //   Laptop: $999.99

            // Find methods
            Product6 found = products.Find(p => p.Price < 600);
            Console.WriteLine($"First under $600: {found.Name}");
            // Output: First under $600: Tablet

            List<Product6> cheapProducts = products.FindAll(p => p.Price < 800);
            Console.WriteLine($"Products under $800: {string.Join(", ", cheapProducts.Select(p => p.Name))}");
            // Output: Products under $800: Tablet, Phone

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: List<T> Functional Operations
            // ═══════════════════════════════════════════════════════════

            // ConvertAll - transform each element
            List<string> names = new List<string> { "Alice", "Bob", "Charlie" };
            List<int> nameLengths = names.ConvertAll(n => n.Length);
            Console.WriteLine($"Name lengths: {string.Join(", ", nameLengths)}");
            // Output: Name lengths: 5, 3, 7

            // TrueForAll - check if all match condition
            bool allShort = names.TrueForAll(n => n.Length < 10);
            Console.WriteLine($"All names < 10 chars: {allShort}");
            // Output: All names < 10 chars: True

            // ForEach - iterate with action
            Console.Write("Names: ");
            names.ForEach(n => Console.Write(n + " "));
            Console.WriteLine();
            // Output: Names: Alice Bob Charlie

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Dictionary<TKey, TValue> Advanced
            // ═══════════════════════════════════════════════════════════

            // Dictionary with complex key
            Dictionary<string, List<string>> categoryProducts = new Dictionary<string, List<string>>();
            
            categoryProducts["Electronics"] = new List<string> { "Laptop", "Phone", "Tablet" };
            categoryProducts["Furniture"] = new List<string> { "Chair", "Table", "Sofa" };
            
            Console.WriteLine($"Electronics: {string.Join(", ", categoryProducts["Electronics"])}");
            // Output: Electronics: Laptop, Phone, Tablet

            // Loop with KeyValuePair
            Console.WriteLine("\nAll categories:");
            foreach (var kvp in categoryProducts)
            {
                Console.WriteLine($"  {kvp.Key}: {string.Join(", ", kvp.Value)}");
            }
            // Output:
            //   Electronics: Laptop, Phone, Tablet
            //   Furniture: Chair, Table, Sofa

            // Dictionary of dictionaries
            Dictionary<string, Dictionary<string, int>> multiLevel = new Dictionary<string, Dictionary<string, int>>();
            multiLevel["2024"] = new Dictionary<string, int>
            {
                ["January"] = 100,
                ["February"] = 150
            };
            Console.WriteLine($"2024 January: {multiLevel["2024"]["January"]}");
            // Output: 2024 January: 100

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Real-World - Inventory Management
            // ═══════════════════════════════════════════════════════════

            var inventory = new InventorySystem();
            
            // Add items
            inventory.AddItem("SKU001", "Laptop", 10, 999.99m);
            inventory.AddItem("SKU002", "Mouse", 50, 29.99m);
            inventory.AddItem("SKU003", "Keyboard", 30, 79.99m);

            // Get item
            var laptop = inventory.GetItem("SKU001");
            Console.WriteLine($"Item: {laptop.Name}, Qty: {laptop.Quantity}, Price: ${laptop.Price}");
            // Output: Item: Laptop, Qty: 10, Price: $999.99

            // Update quantity
            inventory.UpdateQuantity("SKU001", 8);
            Console.WriteLine($"Updated qty: {inventory.GetItem("SKU001").Quantity}");
            // Output: Updated qty: 8

            // Get total value
            decimal total = inventory.GetTotalValue();
            Console.WriteLine($"Total inventory value: ${total}");
            // Output: Total inventory value: $16297.70

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Collection Initialization and Manipulation
            // ═══════════════════════════════════════════════════════════

            // List initialization with expressions
            List<string> dynamicList = new List<string> 
            { 
                "one", "two", "three", "four", "five" 
            };

            // Get range - extract portion of list
            List<string> subList = dynamicList.GetRange(1, 3);
            Console.WriteLine($"Sub-list: {string.Join(", ", subList)}");
            // Output: Sub-list: two, three, four

            // Insert range
            dynamicList.InsertRange(2, new[] { "insert1", "insert2" });
            Console.WriteLine($"After insert: {string.Join(", ", dynamicList)}");
            // Output: After insert: one, two, insert1, insert2, three, four, five

            // Remove range
            dynamicList.RemoveRange(2, 2);
            Console.WriteLine($"After remove: {string.Join(", ", dynamicList)}");
            // Output: After remove: one, two, three, four, five

            Console.WriteLine("\n=== Generic Collections Part 2 Complete ===");
        }
    }

    // Product class for sorting demonstration
    class Product6
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    // Custom comparer for Product sorting
    class ProductPriceComparer : IComparer<Product6>
    {
        public int Compare(Product6 x, Product6 y)
        {
            return x.Price.CompareTo(y.Price);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Inventory System
    // ═══════════════════════════════════════════════════════════

    class InventoryItem
    {
        public string SKU { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    class InventorySystem
    {
        private Dictionary<string, InventoryItem> _items = new Dictionary<string, InventoryItem>();

        public void AddItem(string sku, string name, int quantity, decimal price)
        {
            _items[sku] = new InventoryItem
            {
                SKU = sku,
                Name = name,
                Quantity = quantity,
                Price = price
            };
        }

        public InventoryItem GetItem(string sku)
        {
            return _items.ContainsKey(sku) ? _items[sku] : null;
        }

        public void UpdateQuantity(string sku, int newQuantity)
        {
            if (_items.ContainsKey(sku))
            {
                _items[sku].Quantity = newQuantity;
            }
        }

        public void RemoveItem(string sku)
        {
            _items.Remove(sku);
        }

        public decimal GetTotalValue()
        {
            decimal total = 0;
            foreach (var item in _items.Values)
            {
                total += item.Quantity * item.Price;
            }
            return total;
        }

        public List<InventoryItem> GetAllItems()
        {
            return new List<InventoryItem>(_items.Values);
        }

        public int Count => _items.Count;
    }
}