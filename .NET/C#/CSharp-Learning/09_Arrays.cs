/*
================================================================================
TOPIC 09: ARRAYS
================================================================================

Arrays are data structures that store multiple values of the same type.
They are fundamental to working with collections of data.

TABLE OF CONTENTS:
1. What are Arrays?
2. Declaring and Initializing Arrays
3. Accessing Array Elements
4. Array Properties and Methods
5. Multi-Dimensional Arrays
6. Jagged Arrays
7. Array Methods (Search, Sort, Copy)
8. Common Mistakes
================================================================================
*/

// ================================================================================
// SECTION 1: WHAT ARE ARRAYS?
// ================================================================================

/*
ARRAYS DEFINED:
---------------
An array is a fixed-size collection of elements of the same type.
Elements are stored in contiguous memory locations.

REAL-WORLD ANALOGY:
-------------------
Think of a parking lot:
- Each space has a number (index)
- Each space holds one car (value)
- All spaces are the same size
- Fixed number of spaces

VARIABLE vs ARRAY:
-----------------
Variable: One box, one value
         int age = 25;

Array: Many boxes in a row, same type
         int[] ages = new int[5];  // 5 boxes

WHY USE ARRAYS?
---------------
1. Store multiple related values
2. Efficient random access by index
3. Organize data logically
4. Process collections with loops
5. Foundation for collections
*/


// ================================================================================
// SECTION 2: DECLARING AND INITIALIZING ARRAYS
// ================================================================================

namespace ArrayDeclaration
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // DECLARING ARRAYS
            // ====================================================================
            
            // Method 1: Declare, then initialize
            int[] numbers = new int[5];  // 5 elements, all 0 by default
            
            // Method 2: Declare with values
            string[] names = new string[] { "Alice", "Bob", "Charlie" };
            
            // Method 3: Shortcut (most common)
            int[] scores = { 85, 90, 78, 92, 88 };
            
            // Method 4: Array of specific size with initializer
            double[] prices = new double[3] { 10.99, 20.50, 15.00 };
            
            // ====================================================================
            // DEFAULT VALUES
            // ====================================================================
            
            Console.WriteLine("=== Default Values ===");
            
            int[] intArray = new int[3];
            Console.WriteLine($"int array: {intArray[0]}, {intArray[1]}, {intArray[2]}");
            
            double[] doubleArray = new double[3];
            Console.WriteLine($"double array: {doubleArray[0]}, {doubleArray[1]}, {doubleArray[2]}");
            
            bool[] boolArray = new bool[3];
            Console.WriteLine($"bool array: {boolArray[0]}, {boolArray[1]}, {boolArray[2]}");
            
            string[] stringArray = new string[3];
            Console.WriteLine($"string array: {stringArray[0]}, {stringArray[1]}, {stringArray[2]}");
            
            // ====================================================================
            // INITIALIZATION SHORTCUTS
            // ====================================================================
            
            Console.WriteLine("\n=== Initialization ===");
            
            // Empty array with size
            char[] letters = new char[5];
            
            // With values (size inferred)
            int[] data = { 1, 2, 3, 4, 5 };
            Console.WriteLine($"Length: {data.Length}");
            
            // Array of strings
            string[] fruits = { "Apple", "Banana", "Cherry" };
            
            // Array of objects (mixed)
            object[] mixed = { 1, "hello", 3.14, true };
        }
    }
}

/*
ARRAY DECLARATION SYNTAX:
-------------------------
type[] arrayName = new type[size];
type[] arrayName = { value1, value2, value3 };
type[] arrayName = new type[] { value1, value2, value3 };

DEFAULT VALUES:
---------------
int, float, double, decimal: 0
bool: false
char: '\0' (null character)
string, objects: null
*/


// ================================================================================
// SECTION 3: ACCESSING ARRAY ELEMENTS
// ================================================================================

namespace ArrayAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // INDEXING - Arrays are ZERO-BASED!
            // ====================================================================
            
            int[] numbers = { 10, 20, 30, 40, 50 };
            
            // First element (index 0)
            Console.WriteLine($"First: {numbers[0]}");   // 10
            
            // Last element (index length-1)
            Console.WriteLine($"Last: {numbers[4]}");    // 50
            
            // Access with variable
            int index = 2;
            Console.WriteLine($"At index 2: {numbers[index]}");  // 30
            
            // ====================================================================
            // MODIFYING ELEMENTS
            // ====================================================================
            
            Console.WriteLine("\n=== Modification ===");
            
            numbers[0] = 100;
            Console.WriteLine($"Changed first: {numbers[0]}");
            
            // ====================================================================
            // LOOPING THROUGH ARRAYS
            // ====================================================================
            
            Console.WriteLine("\n=== For Loop ===");
            for (int i = 0; i < numbers.Length; i++)
            {
                Console.WriteLine($"numbers[{i}] = {numbers[i]}");
            }
            
            Console.WriteLine("\n=== Foreach Loop ===");
            foreach (int num in numbers)
            {
                Console.WriteLine(num);
            }
            
            // ====================================================================
            // BOUNDS CHECKING
            // ====================================================================
            
            // Console.WriteLine(numbers[5]);  // ERROR! IndexOutOfRangeException
            // Valid indices: 0 to Length-1
            
            Console.WriteLine($"\nArray length: {numbers.Length}");
            Console.WriteLine($"Max valid index: {numbers.Length - 1}");
        }
    }
}

/*
IMPORTANT: ZERO-BASED INDEXING!
-------------------------------
C# arrays are ZERO-based:
- First element is at index 0
- Second element is at index 1
- nth element is at index n-1
- Last element is at index Length-1

COMMON ERROR:
Accessing index >= Length causes IndexOutOfRangeException!
*/


// ================================================================================
// SECTION 4: ARRAY PROPERTIES AND METHODS
// ================================================================================

namespace ArrayPropertiesMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // USEFUL PROPERTIES
            // ====================================================================
            
            int[] numbers = { 5, 2, 8, 1, 9 };
            
            Console.WriteLine("=== Properties ===");
            Console.WriteLine($"Length: {numbers.Length}");      // 5
            Console.WriteLine($"Rank (dimensions): {numbers.Rank}");  // 1
            
            // ====================================================================
            // ARRAY CLASS METHODS
            // ====================================================================
            
            Console.WriteLine("\n=== Before Sort ===");
            PrintArray(numbers);
            
            // Sort
            Array.Sort(numbers);
            Console.WriteLine("\n=== After Sort ===");
            PrintArray(numbers);
            
            // Reverse
            Array.Reverse(numbers);
            Console.WriteLine("\n=== After Reverse ===");
            PrintArray(numbers);
            
            // Clear (set to default)
            int[] toClear = { 1, 2, 3, 4, 5 };
            Array.Clear(toClear, 0, toClear.Length);  // All zeros
            Console.WriteLine("\n=== After Clear ===");
            PrintArray(toClear);
            
            // IndexOf - find position
            int[] search = { 10, 20, 30, 40, 50 };
            int pos = Array.IndexOf(search, 30);
            Console.WriteLine($"\nIndex of 30: {pos}");  // 2
            
            // BinarySearch (requires sorted array)
            Array.Sort(search);
            int binaryPos = Array.BinarySearch(search, 30);
            Console.WriteLine($"Binary search for 30: {binaryPos}");
            
            // Resize (creates new array)
            int[] original = { 1, 2, 3 };
            Array.Resize(ref original, 5);  // Now size 5
            Console.WriteLine($"\nResized length: {original.Length}");
        }
        
        static void PrintArray(int[] arr)
        {
            foreach (int n in arr)
                Console.Write(n + " ");
            Console.WriteLine();
        }
    }
}

/*
COMMON ARRAY METHODS:
--------------------
Sort()        - Sort in ascending order
Reverse()     - Reverse the array
Clear()       - Set all elements to default
IndexOf()     - Find first occurrence
BinarySearch()- Find in sorted array (fast!)
Resize()      - Change array size
Copy()        - Copy elements to new array
*/


// ================================================================================
// SECTION 5: MULTI-DIMENSIONAL ARRAYS
// ================================================================================

namespace MultiDimensionalArrays
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // 2D ARRAYS (Matrix)
            // ====================================================================
            
            // Declaration: type[,] name = new type[rows, cols];
            
            int[,] matrix = new int[3, 4];  // 3 rows, 4 columns
            
            // Initialize with values
            int[,] numbers = {
                { 1, 2, 3, 4 },    // Row 0
                { 5, 6, 7, 8 },    // Row 1
                { 9, 10, 11, 12 }  // Row 2
            };
            
            // Access elements: array[row, col]
            Console.WriteLine("=== 2D Array ===");
            Console.WriteLine($"numbers[0,0] = {numbers[0,0]}");  // 1
            Console.WriteLine($"numbers[1,2] = {numbers[1,2]}");  // 7
            Console.WriteLine($"numbers[2,3] = {numbers[2,3]}");  // 12
            
            // Loop through 2D array
            Console.WriteLine("\n=== Matrix ===");
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    Console.Write($"{numbers[row, col]}\t");
                }
                Console.WriteLine();
            }
            
            // Get dimensions
            Console.WriteLine($"\nRows: {numbers.GetLength(0)}");
            Console.WriteLine($"Columns: {numbers.GetLength(1)}");
            
            // ====================================================================
            // 3D ARRAYS
            // ====================================================================
            
            int[,,] cube = new int[2, 3, 4];  // 2x3x4 cube
            
            int[,,] cubeInit = {
                { {1,2,3,4}, {5,6,7,8}, {9,10,11,12} },
                { {13,14,15,16}, {17,18,19,20}, {21,22,23,24} }
            };
            
            Console.WriteLine($"\n3D array: {cubeInit[1, 2, 3]}");  // 24
        }
    }
}

/*
MULTI-DIMENSIONAL ARRAYS:
-------------------------
2D: type[,] - Like a table (rows and columns)
3D: type[,,] - Like a cube (layers, rows, columns)
4D+: type[,,,] - Higher dimensions

Think of:
- 1D: A line of parking spaces
- 2D: A parking lot (rows and columns)
- 3D: A parking garage (floors, rows, columns)
*/


// ================================================================================
// SECTION 6: JAGGED ARRAYS
// ================================================================================

namespace JaggedArrays
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // JAGGED ARRAYS - Array of arrays
            // ====================================================================
            
            // Declaration: type[][] name = new type[size][];
            
            // Each row can have different lengths!
            int[][] jagged = new int[3][];
            
            jagged[0] = new int[] { 1, 2, 3 };          // 3 elements
            jagged[1] = new int[] { 4, 5 };            // 2 elements
            jagged[2] = new int[] { 6, 7, 8, 9 };      // 4 elements
            
            // Access
            Console.WriteLine("=== Jagged Array ===");
            Console.WriteLine($"jagged[0][0] = {jagged[0][0]}");  // 1
            Console.WriteLine($"jagged[1][1] = {jagged[1][1]}");  // 5
            Console.WriteLine($"jagged[2][2] = {jagged[2][2]}");  // 8
            
            // Loop through
            Console.WriteLine("\n=== Contents ===");
            for (int i = 0; i < jagged.Length; i++)
            {
                Console.Write($"Row {i}: ");
                for (int j = 0; j < jagged[i].Length; j++)
                {
                    Console.Write(jagged[i][j] + " ");
                }
                Console.WriteLine();
            }
            
            // Initialize inline
            string[][] names = {
                new string[] { "John", "Jane" },
                new string[] { "Bob" },
                new string[] { "Alice", "Adam", "Anna" }
            };
            
            // Practical use: Days in each month
            int[][] daysInMonth = {
                new int[] { 31 },           // January
                new int[] { 28 },           // February (non-leap)
                new int[] { 31 },           // March
                new int[] { 30 },           // April
                new int[] { 31 },
                new int[] { 30 },
                new int[] { 31 },
                new int[] { 31 },
                new int[] { 30 },
                new int[] { 31 },
                new int[] { 30 },
                new int[] { 31 }
            };
            
            Console.WriteLine($"\nDays in March: {daysInMonth[2][0]}");
        }
    }
}

/*
JAGGED vs MULTI-DIMENSIONAL:
---------------------------
int[,] matrix = new int[3,4];   // 3 rows, 4 cols each (rectangular)
int[][] jagged = new int[3][]; // 3 rows, each with different lengths

Use jagged when:
- Rows have different lengths
- Memory efficiency matters
- Irregular data structures

Use 2D when:
- Regular grid structure
- Simple and predictable
*/


// ================================================================================
// SECTION 7: COMMON MISTAKES
// ================================================================================

/*
MISTAKE 1: Off-by-one errors
------------------------------
int[] arr = new int[5];
arr[5] = 10;  // ERROR! Valid indices: 0-4


MISTAKE 2: Not initializing array
----------------------------------
int[] arr;
Console.WriteLine(arr[0]);  // ERROR! arr is null


MISTAKE 3: Forgetting arrays are reference types
-------------------------------------------------
int[] a = { 1, 2, 3 };
int[] b = a;  // Both point to SAME array!
b[0] = 99;
Console.WriteLine(a[0]);  // 99!


MISTAKE 4: Confusing Length with last index
-------------------------------------------
arr[arr.Length];  // ERROR! Last index is Length-1


MISTAKE 5: Modifying array while iterating
-------------------------------------------
foreach (var item in arr)
{
    arr[index] = 0;  // Can cause issues!


MISTAKE 6: Not using new keyword
---------------------------------
int[] arr = { 1, 2, 3 };  // OK (implicit new)
int[] arr2 = new int[5] { 1,2,3,4,5 };  // Also OK


MISTAKE 7: Using wrong array type
---------------------------------
// Mixed types need object[], not int[]
*/


// ================================================================================
// SECTION 8: PRACTICE EXERCISES
// ================================================================================

/*
EXERCISE 1: Sum and Average
---------------------------
Create array of 5 numbers.
Calculate and print sum and average.

EXERCISE 2: Find Maximum
-------------------------
Find the largest number in an array.
Don't use built-in methods.

EXERCISE 3: Array Reversal
--------------------------
Reverse an array without using Reverse().
Output in original and reversed form.

EXERCISE 4: 2D Array
---------------------
Create 3x3 matrix.
Calculate sum of all elements.
Calculate sum of each row.

EXERCISE 5: Frequency Counter
-----------------------------
Array: {1, 2, 3, 2, 1, 3, 2, 1}
Count how many times each number appears.
*/


// ================================================================================
// SECTION 9: INTERVIEW QUESTIONS
// ================================================================================

/*
Q1: What is the default index of arrays in C#?
A: Arrays in C# are zero-indexed, meaning the first element is at index 0.

Q2: What is the difference between jagged and multidimensional arrays?
A: Jagged arrays (int[][]) are arrays of arrays, each row can have 
   different lengths. Multidimensional arrays (int[,]) are rectangular,
   all rows have the same number of columns.

Q3: How do you find the length of an array?
A: Use the Length property: array.Length

Q4: What happens if you try to access an index outside the array bounds?
A: IndexOutOfRangeException is thrown.

Q5: Are C# arrays reference types or value types?
A: Arrays are reference types, meaning they are stored on the heap.
*/


// ================================================================================
// NEXT STEPS
// =============================================================================

/*
GREAT PROGRESS! You now understand:
- Array declaration and initialization
- Accessing elements by index
- Looping through arrays
- Multi-dimensional and jagged arrays
- Array methods

WHAT'S NEXT:
In Topic 10, we'll explore Strings - one of the most commonly used
types in C# programming.
*/
