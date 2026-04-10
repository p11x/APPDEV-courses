/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : List<T> Generic Collection - Core Operations
 * FILE      : ListGeneric.cs
 * PURPOSE   : Demonstrates List<T> generic collection fundamentals
 *            including Add, Remove, Find, Sort, and other
 *            essential operations
 * ============================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._04_Collections._01_List_ArrayList
{
    class ListGeneric
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== List<T> Generic Collection Basics ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Creating List<T>
            // ═══════════════════════════════════════════════════════════

            // Empty list with specified type
            var emptyList = new List<int>();
            Console.WriteLine($"Empty list count: {emptyList.Count}");
            // Output: Empty list count: 0

            // Initialize with initial capacity (optimizes memory allocation)
            var listWithCapacity = new List<string>(10);
            Console.WriteLine($"List with capacity, count: {listWithCapacity.Count}");
            // Output: List with capacity, count: 0

            // Initialize with collection initializer syntax
            var colors = new List<string> { "Red", "Green", "Blue", "Yellow" };
            Console.WriteLine($"Colors initialized: {colors.Count}");
            // Output: Colors initialized: 4

            // Create List from an existing array
            int[] numbersArray = { 1, 2, 3, 4, 5 };
            var listFromArray = new List<int>(numbersArray);
            Console.WriteLine($"Created from array: {listFromArray.Count} elements");
            // Output: Created from array: 5 elements

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Adding Elements
            // ═══════════════════════════════════════════════════════════

            var fruits = new List<string>();

            // Add single element to end
            fruits.Add("Apple");
            fruits.Add("Banana");
            Console.WriteLine($"\nAfter Add: {string.Join(", ", fruits)}");
            // Output: After Add: Apple, Banana

            // Add multiple elements using AddRange
            var moreFruits = new List<string> { "Cherry", "Date", "Elderberry" };
            fruits.AddRange(moreFruits);
            Console.WriteLine($"After AddRange: {string.Join(", ", fruits)}");
            // Output: After AddRange: Apple, Banana, Cherry, Date, Elderberry

            // Insert at specific index
            fruits.Insert(2, "Blueberry");
            Console.WriteLine($"After Insert at index 2: {string.Join(", ", fruits)}");
            // Output: After Insert at index 2: Apple, Banana, Blueberry, Cherry, Date, Elderberry

            // Insert range at specific index
            var tropicalFruits = new List<string> { "Mango", "Papaya" };
            fruits.InsertRange(3, tropicalFruits);
            Console.WriteLine($"After InsertRange: {string.Join(", ", fruits)}");
            // Output: After InsertRange: Apple, Banana, Blueberry, Mango, Papaya, Cherry, Date, Elderberry

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Accessing and Searching Elements
            // ═══════════════════════════════════════════════════════════

            // Direct index access (throws IndexOutOfRangeException if invalid)
            Console.WriteLine($"\nElement at index 0: {fruits[0]}");
            // Output: Element at index 0: Apple

            // Using indexer with safe access pattern
            if (fruits.Count > 5)
            {
                Console.WriteLine($"Element at index 5: {fruits[5]}");
                // Output: Element at index 5: Cherry
            }

            // Check if element exists using Contains
            Console.WriteLine($"Contains 'Mango': {fruits.Contains("Mango")}");
            // Output: Contains 'Mango': True
            Console.WriteLine($"Contains 'Grape': {fruits.Contains("Grape")}");
            // Output: Contains 'Grape': False

            // Find index of element
            int mangoIndex = fruits.IndexOf("Mango");
            Console.WriteLine($"IndexOf 'Mango': {mangoIndex}");
            // Output: IndexOf 'Mango': 3

            // Find last index of element (for duplicates)
            var numbersWithDupes = new List<int> { 10, 20, 30, 20, 40, 20 };
            Console.WriteLine($"First index of 20: {numbersWithDupes.IndexOf(20)}");
            // Output: First index of 20: 1
            Console.WriteLine($"Last index of 20: {numbersWithDupes.LastIndexOf(20)}");
            // Output: Last index of 20: 5

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Finding Elements with Lambda
            // ═══════════════════════════════════════════════════════════

            var animals = new List<string> { "Cat", "Dog", "Elephant", "Lion", "Tiger" };

            // Find first element matching predicate
            string foundAnimal = animals.Find(a => a.StartsWith("E"));
            Console.WriteLine($"\nFirst animal starting with 'E': {foundAnimal}");
            // Output: First animal starting with 'E': Elephant

            // Find last element matching predicate
            string lastLongName = animals.FindLast(a => a.Length > 3);
            Console.WriteLine($"Last animal with name > 3 chars: {lastLongName}");
            // Output: Last animal with name > 3 chars: Tiger

            // Find all elements matching predicate (returns new List)
            var longNames = animals.FindAll(a => a.Length > 3);
            Console.WriteLine($"Animals with name > 3 chars: {string.Join(", ", longNames)}");
            // Output: Animals with name > 3 chars: Elephant, Tiger

            // Check if any element matches
            bool hasCat = animals.Exists(a => a == "Cat");
            bool hasMouse = animals.Exists(a => a == "Mouse");
            Console.WriteLine($"Exists 'Cat': {hasCat}, Exists 'Mouse': {hasMouse}");
            // Output: Exists 'Cat': True, Exists 'Mouse': False

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Removing Elements
            // ═══════════════════════════════════════════════════════════

            var colorsList = new List<string> { "Red", "Green", "Blue", "Green", "Yellow" };

            // Remove first occurrence of element (returns bool)
            bool removed = colorsList.Remove("Green");
            Console.WriteLine($"\nAfter Remove('Green'), success: {removed}");
            Console.WriteLine($"List after Remove: {string.Join(", ", colorsList)}");
            // Output: List after Remove: Red, Blue, Green, Yellow

            // Remove element at specific index
            colorsList.RemoveAt(1);
            Console.WriteLine($"After RemoveAt(1): {string.Join(", ", colorsList)}");
            // Output: After RemoveAt(1): Red, Green, Yellow

            // Remove all elements matching predicate
            var numbersToFilter = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            numbersToFilter.RemoveAll(n => n % 2 == 0);
            Console.WriteLine($"After RemoveAll(even): {string.Join(", ", numbersToFilter)}");
            // Output: After RemoveAll(even): 1, 3, 5, 7, 9

            // Clear all elements
            colorsList.Clear();
            Console.WriteLine($"After Clear, count: {colorsList.Count}");
            // Output: After Clear, count: 0

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Sorting and Reversing
            // ═══════════════════════════════════════════════════════════

            var unsortedNumbers = new List<int> { 5, 2, 8, 1, 9, 3 };
            
            // Sort in ascending order (in-place)
            unsortedNumbers.Sort();
            Console.WriteLine($"\nSorted ascending: {string.Join(", ", unsortedNumbers)}");
            // Output: Sorted ascending: 1, 2, 3, 5, 8, 9

            // Sort with custom comparison (descending)
            unsortedNumbers.Sort((a, b) => b.CompareTo(a));
            Console.WriteLine($"Sorted descending: {string.Join(", ", unsortedNumbers)}");
            // Output: Sorted descending: 9, 8, 5, 3, 2, 1

            // Reverse the list
            unsortedNumbers.Reverse();
            Console.WriteLine($"Reversed: {string.Join(", ", unsortedNumbers)}");
            // Output: Reversed: 1, 2, 3, 5, 8, 9

            // Sort strings
            var unsortedStrings = new List<string> { "Zebra", "Apple", "Mango" };
            unsortedStrings.Sort();
            Console.WriteLine($"Strings sorted: {string.Join(", ", unsortedStrings)}");
            // Output: Strings sorted: Apple, Mango, Zebra

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Working with Custom Objects
            // ═══════════════════════════════════════════════════════════

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99m },
                new Product { Id = 2, Name = "Mouse", Price = 29.99m },
                new Product { Id = 3, Name = "Keyboard", Price = 79.99m }
            };

            // Find product by name
            var foundProduct = products.Find(p => p.Name == "Mouse");
            Console.WriteLine($"\nFound product: {foundProduct?.Name}, Price: {foundProduct?.Price}");
            // Output: Found product: Mouse, Price: 29.99

            // Sort by price (using lambda for comparison)
            products.Sort((a, b) => a.Price.CompareTo(b.Price));
            Console.WriteLine("Products sorted by price:");
            foreach (var p in products)
            {
                Console.WriteLine($"  {p.Name}: {p.Price:C}");
            }
            // Output:
            //   Mouse: $29.99
            //   Keyboard: $79.99
            //   Laptop: $999.99

            // ═══════════════════════════════════════════════════════════
            // SECTION 8: Converting List
            // ═══════════════════════════════════════════════════════════

            var sourceList = new List<int> { 10, 20, 30, 40, 50 };

            // Convert List to array
            int[] toArray = sourceList.ToArray();
            Console.WriteLine($"\nTo array, first element: {toArray[0]}");
            // Output: To array, first element: 10

            // Convert to HashSet (removes duplicates)
            var hashSet = new HashSet<int>(sourceList);
            Console.WriteLine($"To HashSet, count: {hashSet.Count}");
            // Output: To HashSet, count: 5

            // Convert to comma-separated string
            string asString = string.Join(" | ", sourceList);
            Console.WriteLine($"As string: {asString}");
            // Output: As string: 10 | 20 | 30 | 40 | 50

            // ═══════════════════════════════════════════════════════════
            // SECTION 9: Real-World Example - Shopping Cart
            // ═══════════════════════════════════════════════════════════

            var shoppingCart = new List<CartItem>
            {
                new CartItem { ProductName = "Book", Quantity = 2, UnitPrice = 15.99m },
                new CartItem { ProductName = "Pen", Quantity = 5, UnitPrice = 2.50m },
                new CartItem { ProductName = "Notebook", Quantity = 3, UnitPrice = 5.99m }
            };

            // Calculate total
            decimal total = shoppingCart.Sum(item => item.Quantity * item.UnitPrice);
            Console.WriteLine($"\n=== Shopping Cart ===");
            Console.WriteLine($"Total items: {shoppingCart.Count}");
            Console.WriteLine($"Total price: {total:C}");
            // Output:
            //   Total items: 3
            //   Total price: $64.43

            // Add more items
            shoppingCart.Add(new CartItem { ProductName = "Stapler", Quantity = 1, UnitPrice = 8.99m });
            Console.WriteLine($"After adding stapler: {shoppingCart.Count} items");

            // Remove out-of-stock item
            shoppingCart.RemoveAll(item => item.ProductName == "Pen");
            Console.WriteLine($"After removing pens: {shoppingCart.Count} items");

            // Find expensive items
            var expensiveItems = shoppingCart.FindAll(item => item.UnitPrice > 10);
            Console.WriteLine($"Expensive items: {string.Join(", ", expensiveItems.Select(i => i.ProductName))}");

            Console.WriteLine("\n=== List<T> Generic Collection Complete ===");
        }
    }

    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    class CartItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public override string ToString() => $"{ProductName} x{Quantity} @ {UnitPrice:C}";
    }
}