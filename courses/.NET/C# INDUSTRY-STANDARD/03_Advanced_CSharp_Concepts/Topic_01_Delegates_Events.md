# 📖 Topic 01: Delegates and Events

## 1. Concept Explanation

A **delegate** is a type-safe function pointer. It holds a reference to a method.

---

## 2. Delegate Example

```csharp
// Define delegate
public delegate int Calculator(int a, int b);

// Methods with matching signature
public static int Add(int a, int b) => a + b;
public static int Multiply(int a, int b) => a * b;

// Usage
Calculator calc = Add;
int result = calc(5, 3);  // 8

calc = Multiply;
result = calc(5, 3);  // 15
```

---

## 3. Multicast Delegate

```csharp
public delegate void Log(string message);

public static void LogToConsole(string m) => Console.WriteLine($"Console: {m}");
public static void LogToFile(string m) => Console.WriteLine($"File: {m}");

Log log = LogToConsole;
log += LogToFile;  // Now calls both

log("Hello!");  // Calls both methods
```

---

## 4. Events

```csharp
public class Button
{
    // Event declaration
    public event EventHandler Clicked;
    
    public void Click()
    {
        // Raise event
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}

// Usage
Button btn = new Button();
btn.Clicked += (s, e) => Console.WriteLine("Button clicked!");
btn.Click();
```

---

## 5. Interview Questions

### Q1: What is a delegate?

**Answer:**
A type-safe function pointer that holds references to methods with a specific signature.

### Q2: What is the difference between delegate and event?

**Answer:**
A delegate holds method references and can be invoked directly. An event restricts access - external code can only subscribe/unsubscribe, not invoke directly.

---

## 📚 Additional Resources

- [Delegates](https://docs.microsoft.com/dotnet/csharp/delegates-overview)

---

## ✅ Next Topic

Continue to: [Topic 02 - Lambda Expressions](./Topic_02_Lambda_Expressions.md)
