# ConApp2 - Object-Oriented Programming Concepts

This document covers the OOP concepts taught in ConApp2 classes.

## Table of Contents
1. [Inheritance](#inheritance)
2. [Polymorphism](#polymorphism)
3. [Abstraction](#abstraction)
4. [Encapsulation](#encapsulation)
5. [Constructors](#constructors)
6. [Interfaces](#interfaces)

---

## Inheritance

Inheritance allows a class to acquire properties and methods of another class.

### Types of Inheritance

#### Single Inheritance
One base class → One derived class

```csharp
class Sample        // Base class
{
    protected int a, b;
    public void GetData() { /* ... */ }
    public void Show() { /* ... */ }
}

class Sample1 : Sample    // Derived class
{
    int c;
    public void Calculation() { c = a + b; }
}
```

**Source:** [Class11.cs](../../ConsoleApplication/ConApp2/Class11.cs)

#### Multi-Level Inheritance
Class A → Class B → Class C

```csharp
class Sample2 { int a; }
class Sample3 : Sample2 { int b; }
class Sample4 : Sample3 { int c; }
```

**Source:** [Class12.cs](../../ConsoleApplication/ConApp2/Class12.cs)

#### Hierarchical Inheritance
One base class → Multiple derived classes

```csharp
class Company { /* ... */ }
class Bike1 : Company { /* ... */ }
class Bike2 : Company { /* ... */ }
```

**Source:** [Class13.cs](../../ConsoleApplication/ConApp2/Class13.cs)

---

## Polymorphism

### Method Overloading (Compile-time)
Same method name, different parameters

```csharp
class Calculator
{
    public int Sum(int x, int y) { return x + y; }
    public int Sum(int x, int y, int z) { return x + y + z; }
    public float Sum(float x, float y) { return x + y; }
}
```

**Source:** [Class9.cs](../../ConsoleApplication/ConApp2/Class9.cs)

### Method Overriding (Runtime)
Virtual methods can be overridden in derived classes

```csharp
abstract class Test3
{
    public virtual void Show() { }  // Virtual
    public abstract void Display();   // Abstract
}

class Test4 : Test3
{
    public override void Show() { }      // Override virtual
    public override void Display() { }    // Implement abstract
}
```

**Source:** [Class38.cs](../../ConsoleApplication/ConApp2/Class38.cs)

---

## Abstraction

### Abstract Classes
Cannot be instantiated, can have abstract and concrete methods

```csharp
abstract class EmployeeBase
{
    // Concrete method
    public int Bonus(int basic)
    {
        if (basic <= 4000) return 400;
        else if (basic <= 10000) return 800;
        else return 1200;
    }
    
    // Abstract method - must be overridden
    public abstract int CalBonus();
}

class Designer : EmployeeBase
{
    public override int CalBonus() { return Bonus(6500); }
}
```

**Source:** [Class14.cs](../../ConsoleApplication/ConApp2/Class14.cs)

### Interfaces
Contracts that specify method signatures only

```csharp
interface IEmployee
{
    void GetData();
    void ShowData();
}

class Employee : IEmployee
{
    public void GetData() { /* ... */ }
    public void ShowData() { /* ... */ }
}
```

**Source:** [Class15.cs](../../ConsoleApplication/ConApp2/Class15.cs)

---

## Encapsulation

### Properties
Control access to fields

```csharp
class Car
{
    private string _color = "Red";
    
    public string Color
    {
        get { return _color; }
        set { _color = value; }
    }
}

// Usage
Car car = new Car();
car.Color = "White";    // set
Console.WriteLine(car.Color);  // get
```

**Source:** [Class20.cs](../../ConsoleApplication/ConApp2/Class20.cs)

### Auto-Implemented Properties
Shorthand syntax

```csharp
class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// Usage
var student = new Student { Id = 1, Name = "Sathesh" };
```

**Source:** [Class21.cs](../../ConsoleApplication/ConApp2/Class21.cs)

---

## Constructors

Special methods called when an object is created.

```csharp
class MyClass
{
    int a, b;
    
    // Default constructor
    public MyClass()
    {
        a = 10; b = 20;
    }
    
    // Parameterized constructor
    public MyClass(int x, int y)
    {
        a = x; b = y;
    }
    
    // Copy constructor
    public MyClass(MyClass obj)
    {
        a = obj.a; b = obj.b;
    }
}
```

**Source:** [Class10.cs](../../ConsoleApplication/ConApp2/Class10.cs)

---

## Advanced OOP Features

### Partial Classes
Split class definition across files

```csharp
// File 1
partial class Test2
{
    public void Show() { }
    public partial void Display();
}

// File 2
partial class Test2
{
    public partial void Display() { }
}
```

**Source:** [Class26.cs](../../ConsoleApplication/ConApp2/Class26.cs)

### Sealed Classes
Prevent inheritance

```csharp
sealed class FinalClass
{
    // Cannot be inherited
}
```

**Source:** [Class44.cs](../../ConsoleApplication/ConApp2/Class44.cs)

### Delegates
Type-safe function pointers

```csharp
delegate int Operation(int x, int y);

class Calculator
{
    public int Add(int a, int b) { return a + b; }
}

var calc = new Calculator();
Operation op = new Operation(calc.Add);
int result = op.Invoke(5, 3);
```

**Source:** [Class18.cs](../../ConsoleApplication/ConApp2/Class18.cs)

---

## Key Takeaways

| Concept | Purpose | Keyword |
|---------|---------|---------|
| Inheritance | Code reuse | `: BaseClass` |
| Polymorphism | Multiple forms | `override`, `virtual` |
| Abstraction | Hide complexity | `abstract`, `interface` |
| Encapsulation | Data protection | `private`, `properties` |

---

## Related Classes

| Class | Topic |
|-------|-------|
| Class9 | Method Overloading |
| Class10 | Constructors |
| Class11 | Single Inheritance |
| Class12 | Multi-Level Inheritance |
| Class13 | Hierarchical Inheritance |
| Class14 | Abstract Classes |
| Class15 | Interfaces |
| Class18 | Delegates |
| Class20 | Properties |
| Class26 | Partial Classes |
| Class38 | Virtual vs Abstract |

---

*Last Updated: 2026-03-11*