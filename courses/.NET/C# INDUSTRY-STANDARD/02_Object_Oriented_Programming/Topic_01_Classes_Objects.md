# 📖 Topic 01: Classes and Objects

## 1. Concept Explanation

### What is a Class?

A **class** is a blueprint for creating objects. It defines:
- **Fields** (data)
- **Properties** (accessors)
- **Methods** (behavior)
- **Events** (notifications)

### What is an Object?

An **object** is an instance of a class. It occupies memory and can be manipulated.

---

## 2. Class Definition

```csharp
public class Person
{
    // Fields (private data)
    private string _name;
    private int _age;
    
    // Properties (public access)
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    
    public int Age
    {
        get { return _age; }
        set { _age = value; }
    }
    
    // Constructor
    public Person(string name, int age)
    {
        _name = name;
        _age = age;
    }
    
    // Method
    public void Greet()
    {
        Console.WriteLine($"Hello, I'm {_name}");
    }
}
```

---

## 3. Creating Objects

```csharp
// Create object using constructor
Person person1 = new Person("John", 30);

// Access properties
person1.Name = "Jane";
person1.Greet();

// Object initializer (C# 3+)
Person person2 = new Person
{
    Name = "Bob",
    Age = 25
};
```

---

## 4. Key Concepts

| Term | Description |
|------|-------------|
| Class | Blueprint/template |
| Object | Instance of a class |
| Field | Variable in a class |
| Property | Smart field with getter/setter |
| Method | Function in a class |

---

## 5. Interview Questions

### Q1: What is the difference between a class and an object?

**Answer:**
- **Class**: Blueprint/template that defines structure and behavior
- **Object**: Instance of a class that occupies memory

### Q2: What are the components of a class?

**Answer:**
Fields, properties, methods, constructors, events, and nested types.

---

## 📚 Additional Resources

- [Classes (C# Programming Guide)](https://docs.microsoft.com/dotnet/csharp/fundamentals/types/classes)

---

## ✅ Next Topic

Continue to: [Topic 02 - Constructors](./Topic_02_Constructors.md)
