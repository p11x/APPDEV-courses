# 📖 Topic 07: Arrays in C#

## 1. Concept Explanation

An **array** is a data structure that stores a collection of elements of the same type. Arrays provide fast access to elements using an index.

### Array Types

```
┌─────────────────────────────────────────────────────────────┐
│                      ARRAY TYPES                            │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  Single-Dimensional                                          │
│  ┌───┬───┬───┬───┬───┐                                      │
│  │ 0 │ 1 │ 2 │ 3 │ 4 │  ← Index                             │
│  └───┴───┴───┴───┴───┘                                      │
│                                                              │
│  Multidimensional                                            │
│  ┌───┬───┬───┐                                              │
│  │0,0│0,1│0,2│                                              │
│  ├───┼───┼───┤                                              │
│  │1,0│1,1│1,2│                                              │
│  └───┴───┴───┘                                              │
│                                                              │
│  Jagged (Array of Arrays)                                   │
│  ┌───┬───────────┐                                           │
│  │0,0│ [0,1,2]   │ ← Each row can have different length    │
│  ├───┼───────────┤                                           │
│  │1,0│ [0,1]     │                                           │
│  └───┴───────────┘                                           │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. Single-Dimensional Arrays

### Declaration and Initialization

```csharp
// Declaration
int[] numbers;

// Initialization
numbers = new int[5];        // Array of 5 ints, all 0

// Declaration + Initialization
int[] numbers = new int[5] { 1, 2, 3, 4, 5 };

// Shortcut
int[] numbers = { 1, 2, 3, 4, 5 };

// Using var
var numbers = new[] { 1, 2, 3, 4, 5 };  // Compiler infers int[]
```

### Accessing Elements

```csharp
int[] numbers = { 10, 20, 30, 40, 50 };

Console.WriteLine(numbers[0]);  // 10 (first element)
Console.WriteLine(numbers[4]);  // 50 (last element)

numbers[2] = 35;  // Modify element
```

---

## 3. Multidimensional Arrays

### 2D Array

```csharp
// Declaration
int[,] matrix = new int[3, 3];

// Initialization with values
int[,] matrix = {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

// Accessing
Console.WriteLine(matrix[0, 0]);  // 1
matrix[1, 2] = 10;
```

### 3D Array

```cube[2, 1, 0]csharp
int[,,] cube = new int[2, 3, 4];  // 2x3x4 array
```

---

## 4. Jagged Arrays

```csharp
// Declaration
int[][] jagged = new int[3][];

// Initialize each row
jagged[0] = new int[] { 1, 2, 3 };
jagged[1] = new int[] { 4, 5 };
jagged[2] = new int[] { 6, 7, 8, 9 };

// Access
Console.WriteLine(jagged[0][1]);  // 2
```

---

## 5. Array Methods

```csharp
int[] numbers = { 3, 1, 4, 1, 5, 9, 2, 6 };

Array.Sort(numbers);        // Sort ascending
Array.Reverse(numbers);     // Reverse
Array.Clear(numbers, 0, 3); // Clear first 3 elements
Array.IndexOf(numbers, 5); // Find index
Array.Exists(numbers, n => n > 5); // Check condition
Array.Find(numbers, n => n > 5);  // Find first match
```

---

## 6. Array Properties

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

Console.WriteLine(numbers.Length);        // 5
Console.WriteLine(numbers.Rank);         // 1 (dimensions)
Console.WriteLine(numbers.GetLength(0)); // 5 (length of dimension 0)
```

---

## 7. Interview Questions

### Q1: What is the difference between jagged and multidimensional arrays?

**Answer:**
- **Multidimensional**: Fixed rows and columns, rectangular
- **Jagged**: Array of arrays, each row can have different lengths

### Q2: What is the default value for array elements?

**Answer:**
- Numeric types: 0
- bool: false
- Reference types: null

### Q3: Can you resize an array?

**Answer:**
No, arrays have fixed size. Use `Array.Resize()` to create a new array or use `List<T>` for dynamic sizing.

---

## 📚 Additional Resources

- [Arrays in C#](https://docs.microsoft.com/dotnet/csharp/arrays)

---

## ✅ Next Topic

Continue to: [Topic 08 - Strings](./Topic_08_Strings.md)
