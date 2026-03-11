# 📖 Topic 05: Inheritance in C#

## 1. Concept Explanation

**Inheritance** allows a class to inherit properties, methods, and fields from another class.

---

## 2. Inheritance Example

```csharp
// Base class
public class Animal
{
    public string Name { get; set; }
    
    public void Eat()
    {
        Console.WriteLine($"{Name} is eating");
    }
}

// Derived class
public class Dog : Animal
{
    public void Bark()
    {
        Console.WriteLine($"{Name} is barking");
    }
}

// Usage
Dog dog = new Dog { Name = "Buddy" };
dog.Eat();  // Inherited from Animal
dog.Bark(); // Defined in Dog
```

---

## 3. base Keyword

```csharp
public class Dog : Animal
{
    public string Breed { get; set; }
    
    public Dog(string name, string breed) : base(name)
    {
        Breed = breed;
    }
}
```

---

## 4. Interview Questions

### Q1: What is inheritance?

**Answer:**
A mechanism where a class derives properties and behavior from another class, promoting code reuse.

### Q2: Can C# support multiple inheritance?

**Answer:**
No, C# doesn't support multiple class inheritance. Use interfaces instead.

---

## ✅ Next Topic

Continue to: [Topic 06 - Polymorphism](./Topic_06_Polymorphism.md)
