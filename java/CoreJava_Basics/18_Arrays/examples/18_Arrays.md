# Java Arrays

## Table of Contents
1. [Introduction to Arrays](#introduction-to-arrays)
2. [Array Declaration and Initialization](#array-declaration-and-initialization)
3. [Array Types](#array-types)
4. [Array Operations](#array-operations)
5. [Arrays Utility Class](#arrays-utility-class)
6. [Code Examples](#code-examples)
7. [Exercises](#exercises)
8. [Solutions](#solutions)

---

## 1. Introduction to Arrays

### What is an Array?

An **array** is a container object that holds a fixed number of values of a single type.

```
┌─────────────────────────────────────────────────────────────┐
│                     ARRAYS CONCEPT                           │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   int[] numbers = {10, 20, 30, 40, 50};                    │
│                                                              │
│   Index:     [0]  [1]  [2]  [3]  [4]                       │
│   Value:      10   20   30   40   50                       │
│                                                              │
│   Length: 5                                                 │
│   First element: numbers[0] = 10                           │
│   Last element: numbers[4] = 50                            │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. Array Declaration and Initialization

### Declaration

```java
// Method 1: Declaration with size
int[] numbers = new int[5];

// Method 2: Declaration with initialization
int[] numbers = {1, 2, 3, 4, 5};

// Method 3: Declare then initialize
int[] numbers;
numbers = new int[]{1, 2, 3, 4, 5};

// Method 4: Array of objects
String[] names = new String[3];
names[0] = "John";
names[1] = "Jane";
names[2] = "Bob";
```

---

## 3. Array Types

### Single-Dimensional Arrays

```java
int[] intArray = {1, 2, 3, 4, 5};
double[] doubleArray = {1.1, 2.2, 3.3};
char[] charArray = {'a', 'b', 'c'};
boolean[] boolArray = {true, false, true};
String[] stringArray = {"Apple", "Banana", "Cherry"};
```

### Multi-Dimensional Arrays

```java
// 2D Array
int[][] matrix = {
    {1, 2, 3},
    {4, 5, 6},
    {7, 8, 9}
};

// 3D Array
int[][][] cube = new int[2][3][4];
```

---

## 4. Array Operations

### Common Operations

| Operation | Description |
|-----------|-------------|
| `array.length` | Get array length |
| `array[index]` | Access element |
| `Arrays.sort()` | Sort array |
| `Arrays.fill()` | Fill array |
| `Arrays.toString()` | Convert to string |
| `Arrays.deepToString()` | 2D array to string |

---

## 5. Arrays Utility Class

### java.util.Arrays Methods

```java
import java.util.Arrays;

// Sort
Arrays.sort(array);

// Binary Search (array must be sorted)
int index = Arrays.binarySearch(array, value);

// Fill
Arrays.fill(array, value);

// CopyOf
int[] copy = Arrays.copyOf(original, newLength);

// CopyOfRange
int[] copy = Arrays.copyOfRange(original, from, to);

// Equals
boolean equal = Arrays.equals(array1, array2);

// toString
String str = Arrays.toString(array);
```

---

## 6. Code Examples

### Example 1: Basic Array Operations

```java
import java.util.Arrays;

/**
 * BasicArrayOperations - Fundamental array operations
 */
public class BasicArrayOperations {
    
    public static void main(String[] args) {
        System.out.println("=== BASIC ARRAY OPERATIONS ===\n");
        
        // Create array
        int[] numbers = {5, 2, 8, 1, 9, 3};
        
        // Print original
        System.out.println("Original array: " + Arrays.toString(numbers));
        
        // Length
        System.out.println("Length: " + numbers.length);
        
        // Access elements
        System.out.println("First element: " + numbers[0]);
        System.out.println("Last element: " + numbers[numbers.length - 1]);
        
        // Modify element
        numbers[0] = 10;
        System.out.println("After modifying first: " + Arrays.toString(numbers));
        
        // Sort
        Arrays.sort(numbers);
        System.out.println("After sorting: " + Arrays.toString(numbers));
        
        // Search (binary search - array must be sorted!)
        int index = Arrays.binarySearch(numbers, 5);
        System.out.println("Index of 5: " + index);
        
        // Fill
        int[] filled = new int[5];
        Arrays.fill(filled, 7);
        System.out.println("Filled array: " + Arrays.toString(filled));
        
        // Copy
        int[] copy = Arrays.copyOf(numbers, numbers.length + 2);
        System.out.println("Copied with extra space: " + Arrays.toString(copy));
        
        // CopyOfRange
        int[] range = Arrays.copyOfRange(numbers, 1, 4);
        System.out.println("Copy of range [1-4): " + Arrays.toString(range));
    }
}
```

---

### Example 2: 2D Array - Matrix Operations

```java
import java.util.Arrays;

/**
 * MatrixOperations - 2D array operations
 */
public class MatrixOperations {
    
    public static void main(String[] args) {
        System.out.println("=== MATRIX OPERATIONS ===\n");
        
        // Create 3x3 matrix
        int[][] matrix = {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };
        
        // Print matrix
        System.out.println("Matrix:");
        printMatrix(matrix);
        
        // Sum of all elements
        int sum = sumMatrix(matrix);
        System.out.println("\nSum of all elements: " + sum);
        
        // Sum of rows
        System.out.println("\nRow sums:");
        for (int i = 0; i < matrix.length; i++) {
            int rowSum = 0;
            for (int j = 0; j < matrix[0].length; j++) {
                rowSum += matrix[i][j];
            }
            System.out.println("Row " + i + ": " + rowSum);
        }
        
        // Sum of columns
        System.out.println("\nColumn sums:");
        for (int j = 0; j < matrix[0].length; j++) {
            int colSum = 0;
            for (int i = 0; i < matrix.length; i++) {
                colSum += matrix[i][j];
            }
            System.out.println("Column " + j + ": " + colSum);
        }
        
        // Find largest element
        int max = findMax(matrix);
        System.out.println("\nLargest element: " + max);
        
        // Transpose matrix
        System.out.println("\nTranspose:");
        int[][] transpose = transpose(matrix);
        printMatrix(transpose);
    }
    
    public static void printMatrix(int[][] matrix) {
        for (int[] row : matrix) {
            System.out.println(Arrays.toString(row));
        }
    }
    
    public static int sumMatrix(int[][] matrix) {
        int sum = 0;
        for (int[] row : matrix) {
            for (int num : row) {
                sum += num;
            }
        }
        return sum;
    }
    
    public static int findMax(int[][] matrix) {
        int max = matrix[0][0];
        for (int[] row : matrix) {
            for (int num : row) {
                if (num > max) max = num;
            }
        }
        return max;
    }
    
    public static int[][] transpose(int[][] matrix) {
        int rows = matrix.length;
        int cols = matrix[0].length;
        int[][] result = new int[cols][rows];
        
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                result[j][i] = matrix[i][j];
            }
        }
        return result;
    }
}
```

---

### Example 3: Array of Objects

```java
/**
 * Student class for array of objects example
 */
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

/**
 * ArrayOfObjects - Working with arrays of objects
 */
public class ArrayOfObjects {
    
    public static void main(String[] args) {
        System.out.println("=== ARRAY OF OBJECTS ===\n");
        
        // Create array of students
        Student[] students = new Student[5];
        
        // Initialize students
        students[0] = new Student(1, "Alice", 3.8);
        students[1] = new Student(2, "Bob", 3.5);
        students[2] = new Student(3, "Charlie", 3.9);
        students[3] = new Student(4, "Diana", 3.7);
        students[4] = new Student(5, "Eve", 3.6);
        
        // Print all students
        System.out.println("All students:");
        for (Student s : students) {
            System.out.println("  " + s);
        }
        
        // Find top student
        Student topStudent = students[0];
        for (Student s : students) {
            if (s.getGpa() > topStudent.getGpa()) {
                topStudent = s;
            }
        }
        System.out.println("\nTop student: " + topStudent.getName());
        
        // Calculate average GPA
        double totalGpa = 0;
        for (Student s : students) {
            totalGpa += s.getGpa();
        }
        double avgGpa = totalGpa / students.length;
        System.out.println("Average GPA: " + avgGpa);
    }
}
```

---

### Example 4: Searching and Sorting

```java
import java.util.Arrays;

/**
 * SearchAndSort - Various search and sort algorithms
 */
public class SearchAndSort {
    
    public static void main(String[] args) {
        int[] arr = {64, 34, 25, 12, 22, 11, 90};
        
        System.out.println("=== SEARCH AND SORT DEMO ===\n");
        System.out.println("Original: " + Arrays.toString(arr));
        
        // Bubble Sort
        int[] bubble = arr.clone();
        bubbleSort(bubble);
        System.out.println("Bubble Sort: " + Arrays.toString(bubble));
        
        // Selection Sort
        int[] selection = arr.clone();
        selectionSort(selection);
        System.out.println("Selection Sort: " + Arrays.toString(selection));
        
        // Insertion Sort
        int[] insertion = arr.clone();
        insertionSort(insertion);
        System.out.println("Insertion Sort: " + Arrays.toString(insertion));
        
        // Linear Search
        System.out.println("\nLinear Search for 25: " + linearSearch(arr, 25));
        
        // Binary Search (array must be sorted!)
        Arrays.sort(arr);
        System.out.println("Sorted array: " + Arrays.toString(arr));
        System.out.println("Binary Search for 25: " + binarySearch(arr, 25));
    }
    
    public static void bubbleSort(int[] arr) {
        int n = arr.length;
        for (int i = 0; i < n - 1; i++) {
            for (int j = 0; j < n - i - 1; j++) {
                if (arr[j] > arr[j + 1]) {
                    // Swap
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
    }
    
    public static void selectionSort(int[] arr) {
        int n = arr.length;
        for (int i = 0; i < n - 1; i++) {
            int minIdx = i;
            for (int j = i + 1; j < n; j++) {
                if (arr[j] < arr[minIdx]) {
                    minIdx = j;
                }
            }
            // Swap
            int temp = arr[minIdx];
            arr[minIdx] = arr[i];
            arr[i] = temp;
        }
    }
    
    public static void insertionSort(int[] arr) {
        int n = arr.length;
        for (int i = 1; i < n; i++) {
            int key = arr[i];
            int j = i - 1;
            while (j >= 0 && arr[j] > key) {
                arr[j + 1] = arr[j];
                j--;
            }
            arr[j + 1] = key;
        }
    }
    
    public static int linearSearch(int[] arr, int target) {
        for (int i = 0; i < arr.length; i++) {
            if (arr[i] == target) {
                return i;
            }
        }
        return -1;
    }
    
    public static int binarySearch(int[] arr, int target) {
        int left = 0;
        int right = arr.length - 1;
        
        while (left <= right) {
            int mid = left + (right - left) / 2;
            
            if (arr[mid] == target) {
                return mid;
            } else if (arr[mid] < target) {
                left = mid + 1;
            } else {
                right = mid - 1;
            }
        }
        return -1;
    }
}
```

---

## 7. Exercises

### Exercise 1: Array Statistics

**Requirements:**
1. Find minimum, maximum, and average of array
2. Count even and odd numbers
3. Return results as formatted output

### Exercise 2: Remove Duplicates

**Requirements:**
1. Remove duplicate elements from sorted array
2. Return new length
3. Do it in-place (without extra collection)

---

## 8. Solutions

### Solution 1: Array Statistics

```java
import java.util.Arrays;

public class ArrayStatistics {
    
    public static void main(String[] args) {
        int[] numbers = {5, 10, 15, 20, 25, 30, 35, 40};
        
        // Min
        int min = numbers[0];
        for (int n : numbers) {
            if (n < min) min = n;
        }
        
        // Max
        int max = numbers[0];
        for (int n : numbers) {
            if (n > max) max = n;
        }
        
        // Average
        double sum = 0;
        for (int n : numbers) {
            sum += n;
        }
        double avg = sum / numbers.length;
        
        // Count even/odd
        int even = 0, odd = 0;
        for (int n : numbers) {
            if (n % 2 == 0) even++;
            else odd++;
        }
        
        System.out.println("Array: " + Arrays.toString(numbers));
        System.out.println("Min: " + min);
        System.out.println("Max: " + max);
        System.out.println("Average: " + avg);
        System.out.println("Even count: " + even);
        System.out.println("Odd count: " + odd);
    }
}
```

---

### Solution 2: Remove Duplicates

```java
import java.util.Arrays;

public class RemoveDuplicates {
    
    public static int removeDuplicates(int[] sorted) {
        if (sorted.length == 0) return 0;
        
        int unique = 1;
        for (int i = 1; i < sorted.length; i++) {
            if (sorted[i] != sorted[unique - 1]) {
                sorted[unique] = sorted[i];
                unique++;
            }
        }
        return unique;
    }
    
    public static void main(String[] args) {
        int[] arr = {1, 1, 2, 2, 2, 3, 4, 4, 5};
        System.out.println("Original: " + Arrays.toString(arr));
        
        int newLength = removeDuplicates(arr);
        System.out.println("New length: " + newLength);
        System.out.println("Array after: " + Arrays.toString(arr));
    }
}
```

---

## Summary

### Key Takeaways

1. Arrays store fixed-size sequential elements
2. Index starts at 0
3. Use Arrays utility class for operations
4. For dynamic sizing, use ArrayList
5. Multi-dimensional arrays are arrays of arrays

---

*Happy Coding! 🚀*
