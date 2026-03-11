# 📖 Topic 02: Constructors in C#

## 1. Concept Explanation

A **constructor** is a special method that runs when an object is created. It's used to initialize the object.

### Types of Constructors

- **Parameterless** - Default constructor
- **Parameterized** - Constructor with parameters
- **Static** - Initializes static members
- **Chained** - One constructor calling another

---

## 2. Constructor Examples

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Parameterless constructor
    public Person()
    {
        Name = "Unknown";
        Age = 0;
    }
    
    // Parameterized constructor
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
    
    // Static constructor
    static Person()
    {
        Console.WriteLine("Static constructor called");
    }
}
```

---

## 3. Constructor Chaining

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string City { get; set; }
    
    // Chain to another constructor using :this()
    public Person() : this("Unknown", 0)
    {
    }
    
    public Person(string name, int age) : this(name, age, "Unknown")
    {
    }
    
    public Person(string name, int age, string city)
    {
        Name = name;
        Age = age;
        City = city;
    }
}
```

---

## 4. Interview Questions

### Q1: What is a constructor?

**Answer:**
A special method that initializes an object when it's created. Has the same name as the class and no return type.

### Q2: What is the difference between static and instance constructor?

**Answer:**
- **Static**: Called once when class is loaded, initializes static members
- **Instance**: Called when object is created, initializes instance members

---

## ✅ Next Topic

Continue to: [Topic 03 - Access Modifiers](./Topic_03_Access_Modifiers.md)
