/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : List<T> Advanced Operations
 * FILE      : ListGeneric_Part2.cs
 * PURPOSE   : Demonstrates advanced List<T> operations including
 *            ConvertAll, FindAll, ForEach, TrueForAll, 
 *            Conversion methods, and set operations
 * ============================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._04_Collections._01_List_ArrayList
{
    class ListGeneric_Part2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== List<T> Advanced Operations ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: ConvertAll - Transform Each Element
            // ═══════════════════════════════════════════════════════════

            // ConvertAll applies a transformation to each element
            var numbers = new List<int> { 1, 2, 3, 4, 5 };

            // Convert integers to their squares
            var squares = numbers.ConvertAll(n => n * n);
            Console.WriteLine($"Original: {string.Join(", ", numbers)}");
            Console.WriteLine($"Squares: {string.Join(", ", squares)}");
            // Output: Original: 1, 2, 3, 4, 5
            // Output: Squares: 1, 4, 9, 16, 25

            // Convert numbers to strings
            var asStrings = numbers.ConvertAll(n => $"Num:{n}");
            Console.WriteLine($"As strings: {string.Join(", ", asStrings)}");
            // Output: As strings: Num:1, Num:2, Num:3, Num:4, Num:5

            // Convert to different object type
            var people = new List<Person>
            {
                new Person { FirstName = "John", LastName = "Doe" },
                new Person { FirstName = "Jane", LastName = "Smith" },
                new Person { FirstName = "Bob", LastName = "Johnson" }
            };

            var fullNames = people.ConvertAll(p => $"{p.FirstName} {p.LastName}");
            Console.WriteLine($"Full names: {string.Join(", ", fullNames)}");
            // Output: Full names: John Doe, Jane Smith, Bob Johnson

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: ForEach - Iterate with Action
            // ═══════════════════════════════════════════════════════════

            var fruits = new List<string> { "Apple", "Banana", "Cherry", "Date" };

            // ForEach with lambda action
            Console.WriteLine("\n--- ForEach Demo ---");
            fruits.ForEach(f => Console.WriteLine($"  Fruit: {f}"));
            // Output:
            //   Fruit: Apple
            //   Fruit: Banana
            //   Fruit: Cherry
            //   Fruit: Date

            // ForEach with method reference
            fruits.ForEach(PrintWithPrefix);
            
            // ForEach can modify elements directly
            var numbersToDouble = new List<int> { 1, 2, 3, 4, 5 };
            numbersToDouble.ForEach(n => n *= 2); // This doesn't work as expected!
            // Note: ForEach lambda receives a copy for value types

            // For reference types, modifications work:
            var persons = new List<Person>
            {
                new Person { FirstName = "Alice", Age = 25 },
                new Person { FirstName = "Bob", Age = 30 }
            };
            persons.ForEach(p => p.Age += 10);
            Console.WriteLine($"After ForEach modification: {persons[0].Age}, {persons[1].Age}");
            // Output: After ForEach modification: 35, 40

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: TrueForAll and Exists
            // ═══════════════════════════════════════════════════════════

            var scores = new List<int> { 85, 90, 78, 92, 88 };

            // TrueForAll - checks if ALL elements match predicate
            bool allAbove70 = scores.TrueForAll(s => s > 70);
            bool allAbove90 = scores.TrueForAll(s => s > 90);
            Console.WriteLine($"\nAll above 70: {allAbove70}");
            Console.WriteLine($"All above 90: {allAbove90}");
            // Output: All above 70: True
            // Output: All above 90: False

            // Exists - checks if ANY element matches
            bool hasPerfectScore = scores.Exists(s => s == 100);
            bool hasAbove90 = scores.Exists(s => s > 90);
            Console.WriteLine($"Has perfect score: {hasPerfectScore}");
            Console.WriteLine($"Has score above 90: {hasAbove90}");
            // Output: Has perfect score: False
            // Output: Has score above 90: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: GetRange and Utility Methods
            // ═══════════════════════════════════════════════════════════

            var days = new List<string> { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

            // GetRange returns a portion of the list
            var weekdays = days.GetRange(0, 5);
            Console.WriteLine($"\nWeekdays: {string.Join(", ", weekdays)}");
            // Output: Weekdays: Mon, Tue, Wed, Thu, Fri

            var weekend = days.GetRange(5, 2);
            Console.WriteLine($"Weekend: {string.Join(", ", weekend)}");
            // Output: Weekend: Sat, Sun

            // AddRange to add a range
            var additional = new List<string> { "Holiday1", "Holiday2" };
            var combined = new List<string>(days);
            combined.AddRange(additional);
            Console.WriteLine($"Combined: {string.Join(", ", combined)}");
            // Output: Combined: Mon, Tue, Wed, Thu, Fri, Sat, Sun, Holiday1, Holiday2

            // GetHashCode and Equals
            var list1 = new List<int> { 1, 2, 3 };
            var list2 = new List<int> { 1, 2, 3 };
            Console.WriteLine($"Same content equals: {list1.Equals(list2)}");
            // Output: Same content equals: False (different references)
            // Use SequenceEqual for content comparison:
            Console.WriteLine($"SequenceEqual: {list1.SequenceEqual(list2)}");
            // Output: SequenceEqual: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Binary Search Operations
            // ═══════════════════════════════════════════════════════════

            // BinarySearch requires sorted list for correct results
            var sortedNumbers = new List<int> { 10, 20, 30, 40, 50, 60, 70 };
            
            int index = sortedNumbers.BinarySearch(40);
            Console.WriteLine($"\nBinarySearch for 40: index {index}");
            // Output: BinarySearch for 40: index 3

            int notFoundIndex = sortedNumbers.BinarySearch(45);
            // When not found, returns negative bitwise complement of insertion point
            Console.WriteLine($"BinarySearch for 45 (not found): index {notFoundIndex}");
            // Output: BinarySearch for 45 (not found): index -5 (bitwise complement of 4)

            // BinarySearch with custom comparer
            var names = new List<string> { "Adam", "Charlie", "Bob", "David" };
            names.Sort(); // Must sort first
            int bobIndex = names.BinarySearch("Bob", StringComparer.OrdinalIgnoreCase);
            Console.WriteLine($"BinarySearch 'Bob': index {bobIndex}");
            // Output: BinarySearch 'Bob': index 1

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Set Operations
            // ═══════════════════════════════════════════════════════════

            // Note: List<T> doesn't have built-in set operations like Union/Intersect
            // But we can use LINQ or convert to HashSet

            var listA = new List<int> { 1, 2, 3, 4, 5 };
            var listB = new List<int> { 4, 5, 6, 7, 8 };

            // Union - combine and remove duplicates
            var union = listA.Union(listB).ToList();
            Console.WriteLine($"\nUnion: {string.Join(", ", union)}");
            // Output: Union: 1, 2, 3, 4, 5, 6, 7, 8

            // Intersect - common elements
            var intersect = listA.Intersect(listB).ToList();
            Console.WriteLine($"Intersect: {string.Join(", ", intersect)}");
            // Output: Intersect: 4, 5

            // Except - elements in A but not in B
            var except = listA.Except(listB).ToList();
            Console.WriteLine($"Except (A - B): {string.Join(", ", except)}");
            // Output: Except (A - B): 1, 2, 3

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Capacity Management
            // ═══════════════════════════════════════════════════════════

            var dynamicList = new List<string>(2);
            Console.WriteLine($"\n--- Capacity Demo ---");
            Console.WriteLine($"Initial capacity: {dynamicList.Capacity}");
            // Output: Initial capacity: 2

            dynamicList.Add("One");
            dynamicList.Add("Two");
            Console.WriteLine($"After 2 items, capacity: {dynamicList.Capacity}");
            // Output: After 2 items, capacity: 2

            dynamicList.Add("Three"); // Triggers resize
            Console.WriteLine($"After 3 items, capacity: {dynamicList.Capacity}");
            // Output: After 3 items, capacity: 4 (doubles)

            // TrimExcess removes extra capacity
            dynamicList.TrimExcess();
            Console.WriteLine($"After TrimExcess, capacity: {dynamicList.Capacity}");
            // Output: After TrimExcess, capacity: 3

            // Pre-set capacity to avoid resizing
            var preSized = new List<int>(1000);
            Console.WriteLine($"Pre-sized list: {preSized.Capacity}");
            // Output: Pre-sized list: 1000

            // ═══════════════════════════════════════════════════════════
            // SECTION 8: Real-World Example - Email System
            // ═══════════════════════════════════════════════════════════

            var emails = new List<EmailMessage>
            {
                new EmailMessage { Subject = "Meeting", IsRead = false, Priority = Priority.High },
                new EmailMessage { Subject = "Newsletter", IsRead = true, Priority = Priority.Low },
                new EmailMessage { Subject = "Urgent", IsRead = false, Priority = Priority.Critical },
                new EmailMessage { Subject = "Hello", IsRead = true, Priority = Priority.Normal }
            };

            // Get all unread messages
            var unread = emails.FindAll(e => !e.IsRead);
            Console.WriteLine($"\n=== Email System Demo ===");
            Console.WriteLine($"Unread messages: {unread.Count}");
            // Output: Unread messages: 2

            // Get high priority messages
            var highPriority = emails.FindAll(e => e.Priority == Priority.High || e.Priority == Priority.Critical);
            Console.WriteLine($"High priority: {highPriority.Count}");
            // Output: High priority: 2

            // Check if all are read
            bool allRead = emails.TrueForAll(e => e.IsRead);
            Console.WriteLine($"All read: {allRead}");
            // Output: All read: False

            // Mark all as read
            emails.ForEach(e => e.IsRead = true);
            Console.WriteLine($"After marking all read: {emails[0].IsRead}");
            // Output: After marking all read: True

            // Convert to different view model
            var emailSummaries = emails.ConvertAll(e => $"{e.Subject} [{e.Priority}]");
            Console.WriteLine($"Summaries: {string.Join(", ", emailSummaries)}");
            // Output: Summaries: Meeting [High], Newsletter [Low], Urgent [Critical], Hello [Normal]

            Console.WriteLine("\n=== List<T> Advanced Operations Complete ===");
        }

        // Helper method for ForEach demo
        static void PrintWithPrefix(string s)
        {
            Console.WriteLine($"  >> {s}");
        }
    }

    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }

    enum Priority { Low, Normal, High, Critical }

    class EmailMessage
    {
        public string Subject { get; set; }
        public bool IsRead { get; set; }
        public Priority Priority { get; set; }
    }
}