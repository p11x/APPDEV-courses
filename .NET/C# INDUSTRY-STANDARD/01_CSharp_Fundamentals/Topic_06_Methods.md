# 📖 Topic 06: Methods in C#

## 1. Concept Explanation

A **method** is a code block that contains a series of statements. Methods are used to perform operations, encapsulate behavior, and promote code reusability.

### Method Structure

```csharp
// Method signature
accessModifier returnType MethodName(parameters)
{
    // Method body
    // Statements
    return value;  // If return type is not void
}
```

### Components

| Component | Description |
|-----------|-------------|
| Access Modifier | public, private, protected, internal |
| Return Type | Data type returned (void for no return) |
| Method Name | Identifier for the method |
| Parameters | Input values |
| Method Body | Code to execute |

---

## 2. Method Types

### Void Method (No Return)

```csharp
public void Greet()
{
    Console.WriteLine("Hello!");
}

// With parameters
public void Greet(string name)
{
    Console.WriteLine($"Hello, {name}!");
}
```

### Value-Returning Method

```csharp
public int Add(int a, int b)
{
    return a + b;
}

public string GetFullName(string first, string last)
{
    return $"{first} {last}";
}
```

---

## 3. Parameters

### Value Parameters

```csharp
public void Increment(int number)
{
    number++;  // Only affects local copy
}

int x = 5;
Increment(x);
// x is still 5
```

### Reference Parameters (ref)

```csharp
public void Increment(ref int number)
{
    number++;  // Affects original variable
}

int x = 5;
Increment(ref int x);
// x is now 6
```

### Out Parameters

```csharp
public void Divide(int a, int b, out int quotient, out int remainder)
{
    quotient = a / b;
    remainder = a % b;
}

Divide(10, 3, out int q, out int r);
// q = 3, r = 1
```

### Optional Parameters

```csharp
public void Greet(string name = "Guest")
{
    Console.WriteLine($"Hello, {name}!");
}

Greet();           // Hello, Guest!
Greet("John");     // Hello, John!
```

### Named Arguments

```csharp
public void CreatePerson(string firstName, string lastName, int age)
{
    Console.WriteLine($"{firstName} {lastName}, {age}");
}

CreatePerson(lastName: "Doe", age: 30, firstName: "John");
```

### Params (Variable Arguments)

```csharp
public int Sum(params int[] numbers)
{
    int total = 0;
    foreach (int n in numbers)
        total += n;
    return total;
}

int result = Sum(1, 2, 3, 4, 5);  // 15
```

---

## 4. Method Overloading

Same method name, different parameters:

```csharp
public int Add(int a, int b) => a + b;
public double Add(double a, double b) => a + b;
public int Add(int a, int b, int c) => a + b + c;

Add(1, 2);           // Calls int version
Add(1.5, 2.5);       // Calls double version
Add(1, 2, 3);        // Calls three-parameter version
```

---

## 5. Recursion

A method that calls itself:

```csharp
public int Factorial(int n)
{
    if (n <= 1)
        return 1;
    return n * Factorial(n - 1);
}

// Factorial(5) = 5 * 4 * 3 * 2 * 1 = 120
```

---

## 6. Local Functions (C# 7+)

```csharp
public int Calculate()
{
    int Helper(int x) => x * 2;
    
    return Helper(5) + Helper(3);  // 16
}
```

---

## 7. Interview Questions

### Q1: What is the difference between ref and out parameters?

**Answer:**
- `ref`: Caller must initialize variable before passing; method can read/write
- `out`: Caller doesn't need to initialize; method must assign before returning

### Q2: What is method overloading?

**Answer:**
Defining multiple methods with same name but different parameter types or count. The compiler chooses which to call based on arguments.

### Q3: What is recursion?

**Answer:**
A method that calls itself to solve a problem by breaking it into smaller subproblems.

### Q4: What are expression-bodied members?

**Answer:**
C# 6+ syntax for single-statement methods:
```csharp
public int Square(int x) => x * x;
```

---

## 📚 Additional Resources

- [Methods in C#](https://docs.microsoft.com/dotnet/csharp/methods)

---

## ✅ Next Topic

Continue to: [Topic 07 - Arrays](./Topic_07_Arrays.md)
