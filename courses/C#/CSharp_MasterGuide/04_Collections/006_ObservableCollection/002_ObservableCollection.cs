/*
 * TOPIC: ObservableCollection<T> Basics
 * SUBTOPIC: INotifyCollectionChanged, Change Notifications
 * FILE: ObservableCollection.cs
 * PURPOSE: Demonstrate fundamental ObservableCollection features including
 *          change notifications, adding, removing, and replacing items
 */
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CSharp_MasterGuide._04_Collections._06_ObservableCollection
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public override string ToString() => $"{Name} (Age: {Age})";
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public override string ToString() => $"{Name}: ${Price:F2}";
    }

    public class ObservableCollectionBasics
    {
        public static void Main()
        {
            Console.WriteLine("=== ObservableCollection<T> Basics ===\n");

            BasicIntroduction();
            CollectionChangedEvent();
            AddingItems();
            RemovingItems();
            ReplacingItems();
            MovingItems();
            RealWorldInventoryExample();
        }

        static void BasicIntroduction()
        {
            Console.WriteLine("--- 1. Basic Introduction ---");
            
            // ObservableCollection<T> automatically notifies when items are added, removed, or replaced
            var names = new System.Collections.ObjectModel.ObservableCollection<string>
            {
                "Alice", "Bob", "Charlie"
            };

            // Subscribe to CollectionChanged event
            names.CollectionChanged += (s, e) =>
            {
                Console.WriteLine($"  Collection changed: Action={e.Action}");
            };

            names.Add("David");
            // Output: Collection changed: Action=Add

            Console.WriteLine($"  Current names: {string.Join(", ", names)}");
            // Output: Current names: Alice, Bob, Charlie, David

            names.Remove("Bob");
            // Output: Collection changed: Action=Remove

            Console.WriteLine($"  After removal: {string.Join(", ", names)}");
            // Output: After removal: Alice, Charlie, David
            Console.WriteLine();
        }

        static void CollectionChangedEvent()
        {
            Console.WriteLine("--- 2. CollectionChanged Event Details ---");

            var numbers = new System.Collections.ObjectModel.ObservableCollection<int> { 10, 20, 30 };

            // Handle all CollectionChanged events with full details
            numbers.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        Console.WriteLine($"  Added: {e.NewItems[0]} at index {e.NewStartingIndex}");
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        Console.WriteLine($"  Removed: {e.OldItems[0]} from index {e.OldStartingIndex}");
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        Console.WriteLine($"  Replaced {e.OldItems[0]} with {e.NewItems[0]} at index {e.NewStartingIndex}");
                        break;
                    case NotifyCollectionChangedAction.Move:
                        Console.WriteLine($"  Moved from {e.OldStartingIndex} to {e.NewStartingIndex}");
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        Console.WriteLine("  Collection was cleared");
                        break;
                }
            };

            numbers.Add(40);
            // Output: Added: 40 at index 3

            numbers.Remove(10);
            // Output: Removed: 10 from index 0

            numbers[1] = 25;
            // Output: Replaced 20 with 25 at index 1

            numbers.Clear();
            // Output: Collection was cleared
            Console.WriteLine();
        }

        static void AddingItems()
        {
            Console.WriteLine("--- 3. Adding Items ---");

            var fruits = new System.Collections.ObjectModel.ObservableCollection<string>
            {
                "Apple", "Banana"
            };

            fruits.Add("Orange");
            fruits.Insert(1, "Mango");

            Console.WriteLine($"  Fruits: {string.Join(", ", fruits)}");
            // Output: Fruits: Apple, Mango, Banana, Orange

            // Add range using a loop
            var moreFruits = new[] { "Grape", "Kiwi" };
            foreach (var fruit in moreFruits)
            {
                fruits.Add(fruit);
            }

            Console.WriteLine($"  After adding more: {string.Join(", ", fruits)}");
            // Output: After adding more: Apple, Mango, Banana, Orange, Grape, Kiwi
            Console.WriteLine();
        }

        static void RemovingItems()
        {
            Console.WriteLine("--- 4. Removing Items ---");

            var colors = new System.Collections.ObjectModel.ObservableCollection<string>
            {
                "Red", "Green", "Blue", "Yellow"
            };

            Console.WriteLine($"  Initial: {string.Join(", ", colors)}");
            // Output: Initial: Red, Green, Blue, Yellow

            // Remove by value
            bool removed = colors.Remove("Green");
            Console.WriteLine($"  Removed 'Green': {removed}");
            // Output: Removed 'Green': True

            // Remove by index
            string removedItem = colors[1];
            colors.RemoveAt(1);
            Console.WriteLine($"  Removed at index 1: {removedItem}");
            // Output: Removed at index 1: Blue

            Console.WriteLine($"  After removals: {string.Join(", ", colors)}");
            // Output: After removals: Red, Yellow
            Console.WriteLine();
        }

        static void ReplacingItems()
        {
            Console.WriteLine("--- 5. Replacing Items ---");

            var scores = new System.Collections.ObjectModel.ObservableCollection<int>
            {
                85, 90, 78, 92
            };

            Console.WriteLine($"  Before: {string.Join(", ", scores)}");
            // Output: Before: 85, 90, 78, 92

            // Replace via indexer - triggers Replace action
            scores[2] = 88;

            Console.WriteLine($"  After replacing index 2: {string.Join(", ", scores)}");
            // Output: After replacing index 2: 85, 90, 88, 92

            // Replace via method that returns new collection
            var newScores = new System.Collections.ObjectModel.ObservableCollection<int> { 100, 100, 100 };
            scores = newScores;
            Console.WriteLine($"  After replacement: {string.Join(", ", scores)}");
            // Output: After replacement: 100, 100, 100
            Console.WriteLine();
        }

        static void MovingItems()
        {
            Console.WriteLine("--- 6. Moving Items ---");

            var tasks = new System.Collections.ObjectModel.ObservableCollection<string>
            {
                "Task 1", "Task 2", "Task 3", "Task 4"
            };

            Console.WriteLine($"  Initial: {string.Join(", ", tasks)}");
            // Output: Initial: Task 1, Task 2, Task 3, Task 4

            // Move item from index 3 to index 1
            tasks.Move(3, 1);
            Console.WriteLine($"  After Move(3, 1): {string.Join(", ", tasks)}");
            // Output: After Move(3, 1): Task 1, Task 4, Task 2, Task 3

            // Move also triggers CollectionChanged
            tasks.Move(0, 2);
            Console.WriteLine($"  After Move(0, 2): {string.Join(", ", tasks)}");
            // Output: After Move(0, 2): Task 4, Task 1, Task 2, Task 3
            Console.WriteLine();
        }

        static void RealWorldInventoryExample()
        {
            Console.WriteLine("--- Real-World: Inventory Management ---");
            Console.WriteLine();

            // Inventory system that tracks product changes
            var inventory = new System.Collections.ObjectModel.ObservableCollection<Product>
            {
                new Product { Name = "Laptop", Price = 999.99m },
                new Product { Name = "Mouse", Price = 29.99m },
                new Product { Name = "Keyboard", Price = 79.99m }
            };

            // Subscribe to changes
            inventory.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    var newProduct = e.NewItems[0] as Product;
                    Console.WriteLine($"  [LOG] New product added: {newProduct}");
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    var removedProduct = e.OldItems[0] as Product;
                    Console.WriteLine($"  [LOG] Product removed: {removedProduct}");
                }
            };

            Console.WriteLine("  Initial inventory:");
            foreach (var p in inventory)
            {
                Console.WriteLine($"    - {p}");
            }
            // Output: Initial inventory:
            //   - Laptop: $999.99
            //   - Mouse: $29.99
            //   - Keyboard: $79.99

            Console.WriteLine();
            Console.WriteLine("  Adding new products...");

            inventory.Add(new Product { Name = "Monitor", Price = 299.99m });
            // Output: [LOG] New product added: Monitor: $299.99

            inventory.Add(new Product { Name = "Headphones", Price = 149.99m });
            // Output: [LOG] New product added: Headphones: $149.99

            Console.WriteLine();
            Console.WriteLine("  Removing old product...");

            inventory.Remove(inventory[1]); // Remove "Mouse"
            // Output: [LOG] Product removed: Mouse: $29.99

            Console.WriteLine();
            Console.WriteLine("  Final inventory:");
            foreach (var p in inventory)
            {
                Console.WriteLine($"    - {p}");
            }
            // Output: Final inventory:
            //   - Laptop: $999.99
            //   - Keyboard: $79.99
            //   - Monitor: $299.99
            //   - Headphones: $149.99
            Console.WriteLine();
        }
    }
}