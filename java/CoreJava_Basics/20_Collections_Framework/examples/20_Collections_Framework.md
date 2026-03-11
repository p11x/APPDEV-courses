# Java Collections Framework

## Table of Contents
1. [Introduction](#introduction)
2. [Collection Hierarchy](#collection-hierarchy)
3. [List Implementations](#list-implementations)
4. [Set Implementations](#set-implementations)
5. [Map Implementations](#map-implementations)
6. [Queue Implementations](#queue-implementations)
7. [Collections Utility Class](#collections-utility-class)
8. [Code Examples](#code-examples)
9. [Exercises](#exercises)
10. [Solutions](#solutions)

---

## 1. Introduction

### What is the Collections Framework?

The **Java Collections Framework** provides a set of interfaces and classes for storing and manipulating groups of objects.

```
┌─────────────────────────────────────────────────────────────┐
│              COLLECTIONS FRAMEWORK OVERVIEW                   │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   ┌─────────────────────────────────────────────────────┐   │
│   │                  Iterable                             │   │
│   │                    └─► Collection                   │   │
│   │                    ├─► List (ordered, indexed)       │   │
│   │                    ├─► Set (unique, no duplicates)  │   │
│   │                    └─► Queue (FIFO)                 │   │
│   └─────────────────────────────────────────────────────┘   │
│                                                              │
│   ┌─────────────────────────────────────────────────────┐   │
│   │                    Map (key-value)                  │   │
│   │              (Does NOT extend Collection)           │   │
│   └─────────────────────────────────────────────────────┘   │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. Collection Hierarchy

### Core Interfaces

| Interface | Description | Key Methods |
|-----------|-------------|-------------|
| `Collection` | Root interface | add, remove, size, iterator |
| `List` | Ordered, indexed | get(i), set(i), add(i), remove(i) |
| `Set` | No duplicates | add, remove, contains |
| `Queue` | FIFO operations | offer, poll, peek |
| `Map` | Key-value pairs | put, get, keySet, values |

---

## 3. List Implementations

### ArrayList

```java
// ArrayList - Dynamic array, most common
List<String> arrayList = new ArrayList<>();
arrayList.add("Apple");
arrayList.add("Banana");
arrayList.get(0);  // "Apple"
```

### LinkedList

```java
// LinkedList - Doubly linked list
List<String> linkedList = new LinkedList<>();
linkedList.add("Apple");
linkedList.addFirst("Banana");  // Add at beginning
linkedList.addLast("Cherry");   // Add at end
```

### Vector

```java
// Vector - Synchronized (thread-safe) ArrayList
List<String> vector = new Vector<>();
```

### Comparison

| Feature | ArrayList | LinkedList | Vector |
|---------|-----------|------------|--------|
| Access | O(1) | O(n) | O(1) |
| Insert/Delete | O(n) | O(1) | O(n) |
| Thread-safe | No | No | Yes |
| Growth | 50% | Double | 100% |

---

## 4. Set Implementations

### HashSet

```java
// HashSet - Unordered, no duplicates
Set<String> hashSet = new HashSet<>();
hashSet.add("Apple");
hashSet.add("Apple");  // Won't be added (duplicate)
```

### LinkedHashSet

```java
// LinkedHashSet - Maintains insertion order
Set<String> linkedHashSet = new LinkedHashSet<>();
```

### TreeSet

```java
// TreeSet - Sorted order, implements NavigableSet
Set<String> treeSet = new TreeSet<>();
```

---

## 5. Map Implementations

### HashMap

```java
// HashMap - Key-value, no order
Map<String, Integer> hashMap = new HashMap<>();
hashMap.put("Apple", 10);
hashMap.put("Banana", 20);
hashMap.get("Apple");  // 10
```

### LinkedHashMap

```java
// LinkedHashMap - Maintains insertion order
Map<String, Integer> linkedHashMap = new LinkedHashMap<>();
```

### TreeMap

```java
// TreeMap - Sorted by keys
Map<String, Integer> treeMap = new TreeMap<>();
```

### Hashtable

```java
// Hashtable - Synchronized, legacy
Map<String, Integer> hashtable = new Hashtable<>();
```

---

## 6. Queue Implementations

### PriorityQueue

```java
// PriorityQueue - Elements ordered by priority
Queue<Integer> priorityQueue = new PriorityQueue<>();
priorityQueue.offer(5);
priorityQueue.offer(1);
priorityQueue.poll();  // Returns 1 (highest priority)
```

### ArrayDeque

```java
// ArrayDeque - Double-ended queue
Deque<String> deque = new ArrayDeque<>();
deque.addFirst("A");
deque.addLast("B");
deque.removeFirst();  // "A"
```

---

## 7. Collections Utility Class

### Common Methods

```java
import java.util.Collections;
import java.util.List;
import java.util.ArrayList;

// Sorting
Collections.sort(list);

// Reverse
Collections.reverse(list);

// Shuffle
Collections.shuffle(list);

// Synchronized collection
List<String> syncList = Collections.synchronizedList(new ArrayList<>());

// Unmodifiable collection
List<String> unmodifiable = Collections.unmodifiableList(list);

// Binary search (list must be sorted!)
int index = Collections.binarySearch(sortedList, target);
```

---

## 8. Code Examples

### Example 1: List Operations

```java
import java.util.*;

/**
 * ListOperationsDemo - Comprehensive List demonstrations
 */
public class ListOperationsDemo {
    
    public static void main(String[] args) {
        System.out.println("=== LIST OPERATIONS DEMO ===\n");
        
        // ArrayList
        List<String> fruits = new ArrayList<>();
        
        // Add elements
        fruits.add("Apple");
        fruits.add("Banana");
        fruits.add("Cherry");
        fruits.add(1, "Avocado");  // Insert at index
        
        System.out.println("Fruits: " + fruits);
        
        // Access elements
        System.out.println("First: " + fruits.get(0));
        System.out.println("Size: " + fruits.size());
        
        // Modify
        fruits.set(0, "Apricot");
        System.out.println("After set: " + fruits);
        
        // Remove
        fruits.remove("Banana");
        fruits.remove(0);  // By index
        System.out.println("After remove: " + fruits);
        
        // Search
        System.out.println("Contains Cherry? " + fruits.contains("Cherry"));
        System.out.println("Index of Cherry: " + fruits.indexOf("Cherry"));
        
        // Iterate
        System.out.println("\n--- Iteration ---");
        for (String fruit : fruits) {
            System.out.println("  " + fruit);
        }
        
        // Sublist
        List<String> sublist = fruits.subList(0, 2);
        System.out.println("Sublist: " + sublist);
        
        // Array to List and vice versa
        String[] arr = {"X", "Y", "Z"};
        List<String> listFromArray = Arrays.asList(arr);
        System.out.println("List from array: " + listFromArray);
    }
}
```

---

### Example 2: Set Operations

```java
import java.util.*;

/**
 * SetOperationsDemo - Comprehensive Set demonstrations
 */
public class SetOperationsDemo {
    
    public static void main(String[] args) {
        System.out.println("=== SET OPERATIONS DEMO ===\n");
        
        // HashSet - Unordered
        Set<String> hashSet = new HashSet<>();
        hashSet.add("Apple");
        hashSet.add("Banana");
        hashSet.add("Apple");  // Duplicate - ignored
        hashSet.add(null);     // Allows one null
        
        System.out.println("HashSet: " + hashSet);
        
        // LinkedHashSet - Insertion order
        Set<String> linkedSet = new LinkedHashSet<>();
        linkedSet.add("First");
        linkedSet.add("Second");
        linkedSet.add("Third");
        System.out.println("LinkedHashSet: " + linkedSet);
        
        // TreeSet - Sorted order
        Set<String> treeSet = new TreeSet<>();
        treeSet.add("Zebra");
        treeSet.add("Apple");
        treeSet.add("Monkey");
        System.out.println("TreeSet: " + treeSet);
        
        // Set operations
        Set<Integer> setA = new HashSet<>(Arrays.asList(1, 2, 3, 4, 5));
        Set<Integer> setB = new HashSet<>(Arrays.asList(4, 5, 6, 7, 8));
        
        System.out.println("\n--- Set Operations ---");
        System.out.println("Set A: " + setA);
        System.out.println("Set B: " + setB);
        
        // Union
        Set<Integer> union = new HashSet<>(setA);
        union.addAll(setB);
        System.out.println("Union: " + union);
        
        // Intersection
        Set<Integer> intersection = new HashSet<>(setA);
        intersection.retainAll(setB);
        System.out.println("Intersection: " + intersection);
        
        // Difference
        Set<Integer> difference = new HashSet<>(setA);
        difference.removeAll(setB);
        System.out.println("A - B: " + difference);
    }
}
```

---

### Example 3: Map Operations

```java
import java.util.*;

/**
 * MapOperationsDemo - Comprehensive Map demonstrations
 */
public class MapOperationsDemo {
    
    public static void main(String[] args) {
        System.out.println("=== MAP OPERATIONS DEMO ===\n");
        
        // Create map
        Map<String, Integer> products = new HashMap<>();
        
        // Put entries
        products.put("Laptop", 999);
        products.put("Phone", 699);
        products.put("Tablet", 449);
        products.put("Laptop", 1099);  // Updates existing key
        
        System.out.println("Products: " + products);
        
        // Get value
        System.out.println("Laptop price: $" + products.get("Laptop"));
        System.out.println("Non-existent: " + products.getOrDefault("TV", 0));
        
        // Check existence
        System.out.println("Contains 'Phone'? " + products.containsKey("Phone"));
        System.out.println("Contains value 699? " + products.containsValue(699));
        
        // Remove
        products.remove("Tablet");
        System.out.println("After remove: " + products);
        
        // Iterate
        System.out.println("\n--- Iteration ---");
        for (Map.Entry<String, Integer> entry : products.entrySet()) {
            System.out.println(entry.getKey() + " = $" + entry.getValue());
        }
        
        // Keys only
        System.out.println("\nKeys: " + products.keySet());
        
        // Values only
        System.out.println("Values: " + products.values());
        
        // TreeMap - Sorted by keys
        System.out.println("\n--- TreeMap (Sorted) ---");
        Map<String, Integer> sortedMap = new TreeMap<>(products);
        System.out.println(sortedMap);
        
        // Get methods
        System.out.println("\nFirst key: " + sortedMap.firstKey());
        System.out.println("Last key: " + sortedMap.lastKey());
    }
}
```

---

### Example 4: Queue and Stack

```java
import java.util.*;

/**
 * QueueStackDemo - Queue and Stack demonstrations
 */
public class QueueStackDemo {
    
    public static void main(String[] args) {
        System.out.println("=== QUEUE AND STACK DEMO ===\n");
        
        // PriorityQueue - Smallest element first (by default)
        System.out.println("--- PriorityQueue ---");
        PriorityQueue<Integer> pq = new PriorityQueue<>();
        pq.offer(30);
        pq.offer(10);
        pq.offer(50);
        pq.offer(20);
        
        System.out.println("Added: 30, 10, 50, 20");
        System.out.println("Poll (removes smallest): " + pq.poll());
        System.out.println("Poll: " + pq.poll());
        System.out.println("Poll: " + pq.poll());
        
        // ArrayDeque - Can be used as Queue or Stack
        System.out.println("\n--- ArrayDeque ---");
        Deque<String> deque = new ArrayDeque<>();
        
        // Queue operations (FIFO)
        deque.offer("First");
        deque.offer("Second");
        deque.offer("Third");
        
        System.out.println("Queue poll: " + deque.poll());
        System.out.println("Queue poll: " + deque.poll());
        
        // Stack operations (LIFO)
        deque.push("A");
        deque.push("B");
        deque.push("C");
        
        System.out.println("\nStack pop: " + deque.pop());
        System.out.println("Stack pop: " + deque.pop());
        System.out.println("Stack pop: " + deque.pop());
    }
}
```

---

### Example 5: Collections Utility

```java
import java.util.*;

/**
 * CollectionsUtilityDemo - Collections utility methods
 */
public class CollectionsUtilityDemo {
    
    public static void main(String[] args) {
        System.out.println("=== COLLECTIONS UTILITY DEMO ===\n");
        
        List<Integer> numbers = new ArrayList<>(Arrays.asList(5, 2, 8, 1, 9, 3, 7, 4, 6));
        
        System.out.println("Original: " + numbers);
        
        // Sort
        Collections.sort(numbers);
        System.out.println("Sorted: " + numbers);
        
        // Reverse
        Collections.reverse(numbers);
        System.out.println("Reversed: " + numbers);
        
        // Shuffle
        Collections.shuffle(numbers);
        System.out.println("Shuffled: " + numbers);
        
        // Binary search (list must be sorted!)
        Collections.sort(numbers);
        int index = Collections.binarySearch(numbers, 5);
        System.out.println("Index of 5: " + index);
        
        // Min/Max
        System.out.println("Min: " + Collections.min(numbers));
        System.out.println("Max: " + Collections.max(numbers));
        
        // Fill
        List<String> list = new ArrayList<>(Arrays.asList("a", "b", "c"));
        Collections.fill(list, "X");
        System.out.println("After fill: " + list);
        
        // Frequency
        List<String> words = Arrays.asList("apple", "banana", "apple", "cherry", "apple");
        System.out.println("'apple' count: " + Collections.frequency(words, "apple"));
        
        // Synchronized collection
        List<String> syncList = Collections.synchronizedList(new ArrayList<>());
        System.out.println("Synchronized list created");
        
        // Unmodifiable collection
        List<String> unmodifiable = Collections.unmodifiableList(Arrays.asList("a", "b"));
        System.out.println("Unmodifiable list: " + unmodifiable);
    }
}
```

---

## 9. Exercises

### Exercise 1: Student Management System

**Requirements:**
1. Use ArrayList to store Student objects
2. Add, remove, search students by ID
3. Sort students by name and GPA

---

### Exercise 2: Word Frequency Counter

**Requirements:**
1. Count occurrences of each word in text
2. Use HashMap for counting
3. Display words sorted alphabetically

---

## 10. Solutions

### Solution 1: Student Management System

```java
import java.util.*;

class Student {
    private int id;
    private String name;
    private double gpa;
    
    public Student(int id, String name, double gpa) {
        this.id = id;
        this.name = name;
        this.gpa = gpa;
    }
    
    public int getId() { return id; }
    public String getName() { return name; }
    public double getGpa() { return gpa; }
    
    @Override
    public String toString() {
        return "Student{id=" + id + ", name='" + name + "', gpa=" + gpa + "}";
    }
}

public class StudentManagement {
    public static void main(String[] args) {
        List<Student> students = new ArrayList<>();
        
        students.add(new Student(1, "Alice", 3.8));
        students.add(new Student(2, "Bob", 3.5));
        students.add(new Student(3, "Charlie", 3.9));
        
        // Sort by name
        students.sort(Comparator.comparing(Student::getName));
        System.out.println("Sorted by name: " + students);
        
        // Sort by GPA descending
        students.sort(Comparator.comparing(Student::getGpa).reversed());
        System.out.println("Sorted by GPA: " + students);
        
        // Search by ID
        int searchId = 2;
        for (Student s : students) {
            if (s.getId() == searchId) {
                System.out.println("Found: " + s);
            }
        }
    }
}
```

---

### Solution 2: Word Frequency Counter

```java
import java.util.*;

public class WordFrequency {
    public static void main(String[] args) {
        String text = "the quick brown fox jumps over the lazy dog the dog was not amused";
        String[] words = text.toLowerCase().split("\\s+");
        
        Map<String, Integer> frequency = new HashMap<>();
        
        for (String word : words) {
            frequency.put(word, frequency.getOrDefault(word, 0) + 1);
        }
        
        // Sort by alphabetical order
        TreeMap<String, Integer> sorted = new TreeMap<>(frequency);
        
        System.out.println("Word frequencies:");
        for (Map.Entry<String, Integer> entry : sorted.entrySet()) {
            System.out.println(entry.getKey() + ": " + entry.getValue());
        }
    }
}
```

---

## Summary

### Key Takeaways

1. **List** - Ordered, indexed, allows duplicates (ArrayList, LinkedList)
2. **Set** - No duplicates (HashSet, LinkedHashSet, TreeSet)
3. **Map** - Key-value pairs (HashMap, TreeMap, LinkedHashMap)
4. **Queue** - FIFO operations (PriorityQueue, ArrayDeque)
5. **Collections utility** - Provides sorting, searching, synchronization

### Angular Backend Connection

- Collections map to JSON arrays/objects
- Maps become JSON objects with key-value pairs
- Spring Data JPA uses collections for database operations

---

*Happy Coding! 🚀*
