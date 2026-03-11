# Variables and Data Types in Java

## Short Definition

Variables are containers for storing data values. Java is a strongly-typed language with primitive data types and reference types. Understanding data types is essential for writing Java programs.

---

## Key Bullet Points

- **Variables**: Named storage locations in memory
- **Data Types**: Define the type of data a variable can hold
- **Primitive Types**: 8 basic data types built into Java
- **Reference Types**: Objects, arrays, strings
- **Type Casting**: Converting between data types
- **Constants**: Variables that cannot be changed (final keyword)

---

## Primitive Data Types

### Integer Types

| Type | Size | Range |
|------|------|-------|
| `byte` | 8-bit | -128 to 127 |
| `short` | 16-bit | -32,768 to 32,767 |
| `int` | 32-bit | -2,147,483,648 to 2,147,483,647 |
| `long` | 64-bit | -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807 |

### Floating Point Types

| Type | Size | Range |
|------|------|-------|
| `float` | 32-bit | Approximately ±3.4×10^38 |
| `double` | 64-bit | Approximately ±1.8×10^308 |

### Other Types

| Type | Size | Values |
|------|------|--------|
| `char` | 16-bit | Unicode characters (0 to 65,535) |
| `boolean` | 1-bit | true or false |

---

## Variable Declaration

```java
// Declaration
int age;
String name;
double salary;

// Declaration with initialization
int count = 10;
String message = "Hello";
boolean isActive = true;
```

---

## Variable Types

### 1. Local Variables
```java
public void method() {
    int localVar = 10;  // Local variable
    System.out.println(localVar);
}
```

### 2. Instance Variables
```java
class Person {
    String name;  // Instance variable
    int age;
}
```

### 3. Static Variables
```java
class Counter {
    static int count = 0;  // Static variable
}
```

---

## Type Casting

### Implicit Casting (Widening)
```java
int num = 100;
long largeNum = num;    // int to long
double decimal = num;   // int to double
```

### Explicit Casting (Narrowing)
```java
double pi = 3.14159;
int intPi = (int) pi;  // double to int (truncates)
```

---

## Constants

Use the `final` keyword to create constants:

```java
final double PI = 3.14159;
final int MAX_SIZE = 100;

PI = 3.14;  // Error! Cannot reassign
```

---

## Reference Types

### Strings
```java
String name = "John";
String greeting = new String("Hello");
```

### Arrays
```java
int[] numbers = {1, 2, 3, 4, 5};
String[] names = new String[10];
```

### Objects
```java
Person p = new Person("Alice", 25);
```

---

## Naming Conventions

| Type | Convention | Example |
|------|-----------|---------|
| Variables | camelCase | `studentCount` |
| Constants | UPPER_SNAKE_CASE | `MAX_SIZE` |
| Classes | PascalCase | `StudentRecord` |
| Methods | camelCase | `calculateTotal()` |

---

## Why This Matters for Angular Developers?

- Java backend uses these data types for API responses
- JSON maps to Java objects with these types
- TypeScript interfaces mirror Java classes
- Understanding types helps with data transformation

---

## Exercises

### Exercise 1: Declare Variables
Create variables of each primitive type and print their values.

### Exercise 2: Type Casting
Convert a double to int and observe the result.

### Exercise 3: Constants
Create a class with constants for application settings.

---

## Summary

- Java has 8 primitive data types and reference types
- Variables must be declared before use
- Type casting allows conversion between types
- Constants are declared with the `final` keyword
- Proper naming conventions improve code readability
