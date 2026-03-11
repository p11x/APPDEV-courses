/*
================================================================================
TOPIC 11: COLLECTIONS
================================================================================

Collections are dynamic data structures that can grow and shrink at runtime.
Unlike arrays, collections provide more flexibility and built-in functionality.

TABLE OF CONTENTS:
1. Introduction to Collections
2. List<T>
3. Dictionary<TKey, TValue>
4. HashSet<T>
5. Queue<T> and Stack<T>
6. LinkedList<T>
7. When to Use Which Collection
================================================================================
*/

// ================================================================================
// SECTION 1: INTRODUCTION TO COLLECTIONS
// ================================================================================

/*
WHAT ARE COLLECTIONS?
---------------------
Collections are dynamic data structures that store multiple items.
Unlike arrays, they can resize automatically.

ARRAYS vs COLLECTIONS:
----------------------
Feature          | Array         | Collection
-----------------|---------------|-----------------
Size             | Fixed         | Dynamic
Resize           | Manual        | Automatic
Methods          | Limited       | Rich API
Type Safety      | Generic       | Generic available

COLLECTION TYPES:
-----------------
1. Generic Collections (strongly typed) - Preferred in modern C#
2. Non-generic Collections (older) - Avoid for new code

We'll focus on Generic Collections!
*/


// ================================================================================
// SECTION 2: LIST<T>
// ================================================================================

namespace ListExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // LIST<T> - Dynamic array
            // ====================================================================
            
            // Create a list
            List<int> numbers = new List<int>();
            
            // Add items
            numbers.Add(10);
            numbers.Add(20);
            numbers.Add(30);
            
            Console.WriteLine("=== List Operations ===");
            
            // Count
            Console.WriteLine($"Count: {numbers.Count}");
            
            // Access by index
            Console.WriteLine($"First: {numbers[0]}");
            
            // AddRange
            numbers.AddRange(new int[] { 40, 50 });
            Console.WriteLine($"After AddRange: {numbers.Count}");
            
            // Insert at position
            numbers.Insert(0, 5);  // Insert 5 at index 0
            Console.WriteLine($"After Insert: {numbers[0]}");
            
            // Remove
            numbers.Remove(20);  // Remove first occurrence
            Console.WriteLine($"After Remove(20): {string.Join(", ", numbers)}");
            
            // RemoveAt
            numbers.RemoveAt(0);  // Remove at index 0
            
            // Contains
            Console.WriteLine($"Contains 30: {numbers.Contains(30)}");
            
            // Find
            List<string> names = new List<string> { "Alice", "Bob", "Charlie" };
            string found = names.Find(n => n.StartsWith("C"));
            Console.WriteLine($"Find 'starts with C': {found}");
            
            // FindAll
            List<int> nums = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<int> evens = nums.FindAll(n => n % 2 == 0);
            Console.WriteLine($"Evens: {string.Join(", ", evens)}");
            
            // Sort
            nums.Sort();
            Console.WriteLine($"Sorted: {string.Join(", ", nums)}");
            
            // Reverse
            nums.Reverse();
            Console.WriteLine($"Reversed: {string.Join(", ", nums)}");
            
            // Clear
            nums.Clear();
            Console.WriteLine($"After Clear: {nums.Count}");
        }
    }
}

/*
LIST<T> METHODS:
----------------
Add()           - Add item to end
AddRange()      - Add multiple items
Insert()        - Insert at position
Remove()        - Remove first occurrence
RemoveAt()      - Remove at index
Contains()      - Check if exists
Find()          - Find first match
FindAll()       - Find all matches
Sort()          - Sort ascending
Reverse()       - Reverse order
Clear()         - Remove all
*/


// ================================================================================
// SECTION 3: DICTIONARY<TKEY, TVALUE>
// ================================================================================

namespace DictionaryExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // DICTIONARY<TKEY, TVALUE> - Key-value pairs
            // ====================================================================
            
            // Create dictionary
            Dictionary<string, int> ages = new Dictionary<string, int>();
            
            // Add items
            ages["Alice"] = 25;
            ages["Bob"] = 30;
            ages["Charlie"] = 35;
            
            Console.WriteLine("=== Dictionary Operations ===");
            
            // Access by key
            Console.WriteLine($"Alice's age: {ages["Alice"]}");
            
            // TryGetValue (safe access)
            if (ages.TryGetValue("Bob", out int bobAge))
            {
                Console.WriteLine($"Bob's age (safe): {bobAge}");
            }
            
            // ContainsKey
            Console.WriteLine($"Contains 'Alice': {ages.ContainsKey("Alice")}");
            
            // ContainsValue
            Console.WriteLine($"Contains age 25: {ages.ContainsValue(25)}");
            
            // Count
            Console.WriteLine($"Count: {ages.Count}");
            
            // Remove
            ages.Remove("Bob");
            Console.WriteLine($"After Remove: {ages.Count}");
            
            // Loop through
            Console.WriteLine("\n=== Loop ===");
            foreach (KeyValuePair<string, int> person in ages)
            {
                Console.WriteLine($"{person.Key}: {person.Value}");
            }
            
            // Keys and Values
            Console.WriteLine($"\nNames: {string.Join(", ", ages.Keys)}");
            Console.WriteLine($"Ages: {string.Join(", ", ages.Values)}");
            
            // Initialize inline
            var fruits = new Dictionary<string, string>
            {
                { "apple", "red" },
                { "banana", "yellow" },
                { "orange", "orange" }
            };
            
            Console.WriteLine($"\nApple color: {fruits["apple"]}");
        }
    }
}

/*
DICTIONARY USE CASES:
---------------------
- Lookups by ID
- Caching
- Counters
- Mapping relationships
- Configuration storage
*/


// ================================================================================
// SECTION 4: HASHSET<T>
// ================================================================================

namespace HashSetExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // HASHSET<T> - Unique elements, no duplicates
            // ====================================================================
            
            HashSet<int> numbers = new HashSet<int>();
            
            // Add
            numbers.Add(1);
            numbers.Add(2);
            numbers.Add(3);
            numbers.Add(1);  // Duplicate - ignored!
            
            Console.WriteLine($"Count: {numbers.Count}");  // 3, not 4!
            
            // Contains
            Console.WriteLine($"Contains 2: {numbers.Contains(2)}");
            
            // Remove
            numbers.Remove(2);
            
            // Set operations
            HashSet<int> set1 = new HashSet<int> { 1, 2, 3, 4, 5 };
            HashSet<int> set2 = new HashSet<int> { 4, 5, 6, 7, 8 };
            
            Console.WriteLine("\n=== Set Operations ===");
            
            // Union (all unique elements)
            HashSet<int> union = new HashSet<int>(set1);
            union.UnionWith(set2);
            Console.WriteLine($"Union: {string.Join(", ", union)}");  // 1,2,3,4,5,6,7,8
            
            // Intersect (common elements)
            HashSet<int> intersect = new HashSet<int>(set1);
            intersect.IntersectWith(set2);
            Console.WriteLine($"Intersect: {string.Join(", ", intersect)}");  // 4,5
            
            // Except (elements in set1 but not in set2)
            HashSet<int> except = new HashSet<int>(set1);
            except.ExceptWith(set2);
            Console.WriteLine($"Except: {string.Join(", ", except)}");  // 1,2,3
            
            // Symmetric Except (elements in either but not both)
            HashSet<int> symExcept = new HashSet<int>(set1);
            symExcept.SymmetricExceptWith(set2);
            Console.WriteLine($"Symmetric Except: {string.Join(", ", symExcept)}");  // 1,2,3,6,7,8
        }
    }
}

/*
HASHSET BENEFITS:
-----------------
- Automatic duplicate prevention
- Fast lookups (O(1) average)
- Set operations built-in

USE WHEN:
- You need unique elements
- Set math (union, intersection)
- Fast membership testing
*/


// ================================================================================
// SECTION 5: QUEUE<T> AND STACK<T>
// ================================================================================

namespace QueueStackExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // QUEUE<T> - First In, First Out (FIFO)
            // ====================================================================
            
            Queue<string> queue = new Queue<string>();
            
            // Enqueue (add to back)
            queue.Enqueue("First");
            queue.Enqueue("Second");
            queue.Enqueue("Third");
            
            Console.WriteLine("=== Queue (FIFO) ===");
            
            // Peek (view front without removing)
            Console.WriteLine($"Peek: {queue.Peek()}");
            
            // Dequeue (remove from front)
            Console.WriteLine($"Dequeue: {queue.Dequeue()}");  // First
            Console.WriteLine($"Dequeue: {queue.Dequeue()}");  // Second
            
            Console.WriteLine($"Remaining: {queue.Count}");
            
            // ====================================================================
            // STACK<T> - Last In, First Out (LIFO)
            // ====================================================================
            
            Stack<int> stack = new Stack<int>();
            
            // Push (add to top)
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            
            Console.WriteLine("\n=== Stack (LIFO) ===");
            
            // Peek (view top without removing)
            Console.WriteLine($"Peek: {stack.Peek()}");
            
            // Pop (remove from top)
            Console.WriteLine($"Pop: {stack.Pop()}");  // 3
            Console.WriteLine($"Pop: {stack.Pop()}");  // 2
            
            Console.WriteLine($"Remaining: {stack.Count}");
            
            // ====================================================================
            // PRACTICAL USES
            // ====================================================================
            
            // Queue: Print jobs, message queues, BFS traversal
            // Stack: Undo functionality, expression evaluation, DFS traversal
        }
    }
}

/*
QUEUE METHODS:
--------------
Enqueue()   - Add to back
Dequeue()   - Remove from front
Peek()      - View front

STACK METHODS:
-------------
Push()      - Add to top
Pop()       - Remove from top
Peek()      - View top
*/


// ================================================================================
// SECTION 6: LINKEDLIST<T>
// ================================================================================

namespace LinkedListExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // LINKEDLIST<T> - Efficient insertions/deletions
            // ====================================================================
            
            LinkedList<string> list = new LinkedList<string>();
            
            // Add nodes
            list.AddFirst("First");
            list.AddLast("Last");
            
            // Insert in middle
            LinkedListNode<string> middle = list.AddAfter(list.First, "Middle");
            list.AddBefore(middle, "Before Middle");
            
            Console.WriteLine("=== LinkedList ===");
            
            // Iterate
            foreach (string s in list)
            {
                Console.WriteLine(s);
            }
            
            // Find node
            LinkedListNode<string> found = list.Find("Middle");
            Console.WriteLine($"\nFound: {found?.Value}");
            
            // Remove
            list.RemoveFirst();
            list.RemoveLast();
            
            Console.WriteLine($"\nAfter removal: {string.Join(", ", list)}");
        }
    }
}

/*
LINKEDLIST vs LIST:
-------------------
LinkedList:
- O(1) insert/delete anywhere
- No index access (must traverse)
- More memory per element

List:
- O(1) access by index
- O(n) insert/delete (shifts elements)
- More memory efficient

USE LINKEDLIST when:
- Many insertions/deletions in middle
- No random access needed
*/


// ================================================================================
// SECTION 7: WHEN TO USE WHICH COLLECTION
// ================================================================================

/*
COLLECTION SELECTION GUIDE:
---------------------------

NEED...                    | USE...
---------------------------|------------------------
Simple list, random access | List<T>
Key-value lookups          | Dictionary<TKey, TValue>
Unique items only          | HashSet<T>
FIFO (first-in-first-out)  | Queue<T>
LIFO (last-in-first-out)  | Stack<T>
Many insertions in middle  | LinkedList<T>
Sorted collection         | SortedList<T>, SortedDictionary<T>
*/


// ================================================================================
// SECTION 8: COMMON MISTAKES
// ================================================================================

/*
MISTAKE 1: Modifying collection while iterating
------------------------------------------------
foreach (var item in list)
{
    list.Remove(item);  // Exception!


MISTAKE 2: Using non-generic collections
-----------------------------------------
Use ArrayList instead of List<T> - WRONG!
Always prefer generic collections.


MISTAKE 3: Not checking for null
--------------------------------
Dictionary<string, int> dict = null;
int value = dict["key"];  // NullReferenceException!


MISTAKE 4: Using wrong key type
-------------------------------
Use meaningful keys, not indices.


MISTAKE 5: Forgetting to initialize
-----------------------------------
List<int> list;  // null
list.Add(1);    // Exception!
*/


// ================================================================================
// SECTION 9: PRACTICE EXERCISES
// ================================================================================

/*
EXERCISE 1: To-Do List
----------------------
Use List<string> to create a simple to-do list.
Add items, remove items, display all.

EXERCISE 2: Word Counter
-------------------------
Use Dictionary<string, int> to count word frequencies.
Input: "hello world hello"
Output: hello: 2, world: 1

EXERCISE 3: Unique Numbers
-------------------------
Use HashSet to get unique numbers from a list.

EXERCISE 4: Queue Simulation
-----------------------------
Simulate a print queue with Queue<string>.

EXERCISE 5: Stack Undo
-----------------------
Implement a simple undo system using Stack<string>.
*/


// ================================================================================
// SECTION 10: INTERVIEW QUESTIONS
// ================================================================================

/*
Q1: What is the difference between List<T> and array?
A: Arrays have fixed size, List<T> is dynamic and can grow/shrink.
   List provides more methods for manipulation.

Q2: When would you use Dictionary over List?
A: Use Dictionary when you need fast lookups by key. O(1) vs O(n)
   for searching.

Q3: What is HashSet<T> used for?
A: For storing unique elements with fast lookups. Automatically
   prevents duplicates.

Q4: Difference between Queue and Stack?
A: Queue is FIFO (first-in-first-out), Stack is LIFO (last-in-first-out).

Q5: What are generic collections?
A: Collections that are type-safe at compile time. List<T> is generic,
   ArrayList is non-generic.
*/


// ================================================================================
// NEXT STEPS
// =============================================================================

/*
EXCELLENT! You now understand Collections. You've completed the beginner section!

WHAT'S NEXT:
Topic 12 introduces Object-Oriented Programming (OOP) - the paradigm that
C# was designed for. This is crucial for building real-world applications!
*/
