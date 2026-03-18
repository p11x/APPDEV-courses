# ConApp2 - Arrays and Collections

This document covers arrays and collections taught in ConApp2.

## Table of Contents
1. [Arrays](#arrays)
2. [Non-Generic Collections](#non-generic-collections)
3. [Generic Collections](#generic-collections)
4. [LINQ Operations](#linq-operations)

---

## Arrays

### Single-Dimensional Arrays

```csharp
// Declaration and initialization
int[] numbers = new int[5];
int[] numbers = { 10, 20, 30, 40, 50 };

// Accessing elements
for (int i = 0; i < numbers.Length; i++)
{
    Console.WriteLine(numbers[i]);
}
```

**Source:** [Class1.cs](../../ConsoleApplication/ConApp2/Class1.cs)

### Array Operations

```csharp
int[] arr = { 5, 2, 8, 1, 9 };

// Sort ascending
Array.Sort(arr);

// Reverse
Array.Reverse(arr);

// Search - returns index or -1
int index = Array.IndexOf(arr, 5);
```

**Source:** [Class2.cs](../../ConsoleApplication/ConApp2/Class2.cs)

### Two-Dimensional Arrays (Matrix)

```csharp
int[,] matrix = new int[3, 3];

// Input
for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 3; j++)
    {
        matrix[i, j] = int.Parse(Console.ReadLine());
    }
}

// Output
for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 3; j++)
    {
        Console.Write(matrix[i, j] + "\t");
    }
}
```

**Source:** [Class3.cs](../../ConsoleApplication/ConApp2/Class3.cs)

### Jagged Arrays (Array of Arrays)

```csharp
int[][] jagged = new int[3][];
jagged[0] = new int[3];  // 3 elements
jagged[1] = new int[2];  // 2 elements
jagged[2] = new int[4];  // 4 elements
```

**Source:** [Class7.cs](../../ConsoleApplication/ConApp2/Class7.cs)

---

## Non-Generic Collections

### ArrayList

```csharp
ArrayList list = new ArrayList();
list.Add(10);
list.Add("Sathesh");
list.Insert(2, 100);
list.Remove(20);
list.RemoveAt(0);
```

**Source:** [Class27.cs](../../ConsoleApplication/ConApp2/Class27.cs)

### Stack (LIFO)

```csharp
Stack stack = new Stack();
stack.Push(10);
stack.Push("Hello");
Console.WriteLine(stack.Pop());  // Returns and removes top
Console.WriteLine(stack.Peek());  // Returns top without removing
```

**Source:** [Class28.cs](../../ConsoleApplication/ConApp2/Class28.cs)

### Queue (FIFO)

```csharp
Queue queue = new Queue();
queue.Enqueue(10);
queue.Enqueue(20);
Console.WriteLine(queue.Dequeue());  // Returns and removes front
```

**Source:** [Class29.cs](../../ConsoleApplication/ConApp2/Class29.cs)

### Hashtable

```csharp
Hashtable ht = new Hashtable();
ht.Add(1, "One");
ht.Add("two", 2);
foreach (DictionaryEntry item in ht)
{
    Console.WriteLine(item.Key + ": " + item.Value);
}
```

**Source:** [Class32.cs](../../ConsoleApplication/ConApp2/Class32.cs)

---

## Generic Collections

### List<T>

```csharp
List<int> numbers = new List<int>();
numbers.Add(5);
numbers.Add(10);
numbers.Insert(0, 1);
numbers.Sort();
numbers.Remove(5);
numbers.RemoveAt(0);

// LINQ operations
int sum = numbers.Sum();
int max = numbers.Max();
int min = numbers.Min();
double avg = numbers.Average();
```

**Source:** [Class30.cs](../../ConsoleApplication/ConApp2/Class30.cs)

### LinkedList<T>

```csharp
LinkedList<int> list = new LinkedList<int>();
list.AddFirst(10);
list.AddLast(20);
list.AddAfter(node, 15);
```

**Source:** [Class31.cs](../../ConsoleApplication/ConApp2/Class31.cs)

### Dictionary<TKey, TValue>

```csharp
Dictionary<int, string> dict = new Dictionary<int, string>();
dict.Add(1, "Sathesh");
dict.Add(2, "Kumar");

foreach (var item in dict)
{
    Console.WriteLine(item.Key + ": " + item.Value);
}
```

**Source:** [Class32.cs](../../ConsoleApplication/ConApp2/Class32.cs)

### HashSet<T> (Unique Elements)

```csharp
HashSet<int> set = new HashSet<int>();
set.Add(1);
set.Add(2);
set.Add(1);  // Duplicate - ignored

// Remove duplicates from array
int[] arr = { 1, 2, 3, 1, 2, 4 };
HashSet<int> unique = new HashSet<int>(arr);
```

**Source:** [Class33.cs](../../ConsoleApplication/ConApp2/Class33.cs)

---

## Collection Comparison

| Collection | Type | Key Feature | Use Case |
|------------|------|-------------|----------|
| ArrayList | Non-generic | Dynamic size | Mixed types |
| List<T> | Generic | Type-safe | Most scenarios |
| LinkedList<T> | Generic | Fast insert/delete | Frequent modifications |
| Dictionary<TKey,TValue> | Generic | Fast lookup | Key-value pairs |
| HashSet<T> | Generic | Unique values | Remove duplicates |
| Stack<T> | Generic | LIFO | Undo operations |
| Queue<T> | Generic | FIFO | Task scheduling |

---

## Key Takeaways

1. **Prefer Generic collections** - Type-safe, better performance
2. **Use List<T>** for most scenarios
3. **Use Dictionary<TKey,TValue>** when you need fast lookup by key
4. **Use HashSet<T>** when you need unique elements only

---

## Related Classes

| Class | Topic |
|-------|-------|
| Class1 | Single Arrays |
| Class2 | Array Operations |
| Class3 | 2D Arrays |
| Class7 | Jagged Arrays |
| Class27 | ArrayList |
| Class28 | Stack |
| Class29 | Queue |
| Class30 | List<T> |
| Class31 | LinkedList<T> |
| Class32 | Dictionary |
| Class33 | HashSet<T> |
| Class34 | Generic Classes |

---

*Last Updated: 2026-03-11*