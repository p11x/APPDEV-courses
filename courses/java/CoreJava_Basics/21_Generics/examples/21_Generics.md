# Java Advanced Collections

## Table of Contents
1. [Advanced Set Operations](#advanced-set-operations)
2. [Advanced Map Operations](#advanced-map-operations)
3. [WeakHashMap and IdentityHashMap](#weakhashmap-and-identityhashmap)
4. [Collections Utility Methods](#collections-utility-methods)
5. [Code Examples](#code-examples)

---

## 1. Advanced Set Operations

### NavigableSet and SortedSet

```java
// TreeSet with NavigableSet methods
NavigableSet<Integer> treeSet = new TreeSet<>();

treeSet.add(5);
treeSet.add(2);
treeSet.add(8);
treeSet.add(1);

treeSet.lower(5);  // 2 (greatest < 5)
treeSet.higher(5); // 8 (least > 5)
treeSet.floor(5);  // 5 (greatest <= 5)
treeSet.ceiling(5); // 5 (least >= 5)
```

---

## 2. Advanced Map Operations

### Compute Operations

```java
Map<String, Integer> map = new HashMap<>();

// compute - compute value for key
map.compute("A", (k, v) -> (v == null) ? 1 : v + 1);

// computeIfAbsent - if key missing, compute and put
map.computeIfAbsent("B", k -> 100);

// computeIfPresent - if key exists, compute new value
map.computeIfPresent("A", (k, v) -> v + 10);

// merge - merge values
map.merge("C", 50, (v1, v2) -> v1 + v2);
```

---

## 3. WeakHashMap and IdentityHashMap

### WeakHashMap

```java
// Keys can be garbage collected when no references
WeakHashMap<Key, String> weakMap = new WeakHashMap<>();
```

### IdentityHashMap

```java
// Uses == instead of equals() for key comparison
IdentityHashMap<String, String> identityMap = new IdentityHashMap<>();
```

---

## 4. Collections Utility Methods

### Singleton Collections

```java
// Singleton list (one element)
List<String> singleList = Collections.singletonList("Only one");

// Singleton set
Set<String> singleSet = Collections.singleton("Only one");

// Singleton map
Map<String, Integer> singleMap = Collections.singletonMap("Key", 1);
```

### Empty Collections

```java
List<String> emptyList = Collections.emptyList();
Set<String> emptySet = Collections.emptySet();
Map<String, Integer> emptyMap = Collections.emptyMap();
```

---

## 5. Code Examples

### AdvancedCollectionsDemo

```java
import java.util.*;

public class AdvancedCollectionsDemo {
    public static void main(String[] args) {
        System.out.println("=== ADVANCED COLLECTIONS DEMO ===\n");
        
        // NavigableSet
        NavigableSet<Integer> numbers = new TreeSet<>();
        numbers.addAll(Arrays.asList(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
        
        System.out.println("NavigableSet: " + numbers);
        System.out.println("lower(5): " + numbers.lower(5));
        System.out.println("higher(5): " + numbers.higher(5));
        System.out.println("floor(5): " + numbers.floor(5));
        System.out.println("ceiling(5): " + numbers.ceiling(5));
        
        // SubSet
        System.out.println("subSet(3, 7): " + numbers.subSet(3, 7));
        
        // Map compute
        System.out.println("\n--- Map Compute Operations ---");
        Map<String, Integer> scores = new HashMap<>();
        scores.put("A", 10);
        
        scores.compute("A", (k, v) -> v + 5);
        System.out.println("After compute: " + scores);
        
        scores.computeIfAbsent("B", k -> 100);
        System.out.println("After computeIfAbsent: " + scores);
        
        // Merge
        scores.merge("C", 50, (v1, v2) -> v1 + v2);
        System.out.println("After merge: " + scores);
    }
}
```

---

*Advanced Collections Complete!*
