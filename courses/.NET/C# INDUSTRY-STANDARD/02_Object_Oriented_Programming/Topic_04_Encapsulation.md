# 📖 Topic 04: Encapsulation

## 1. Concept Explanation

**Encapsulation** bundles data and methods that operate on that data within a single unit (class). It also restricts direct access to some components.

---

## 2. Why Encapsulation?

- **Data Protection**: Prevents unauthorized access
- **Flexibility**: Change internal implementation without breaking users
- **Simplicity**: Hide complex logic behind simple interfaces

---

## 3. Implementation with Properties

```csharp
public class BankAccount
{
    private decimal _balance;
    
    // Property with validation
    public decimal Balance
    {
        get { return _balance; }
        private set 
        {
            if (value < 0)
                throw new ArgumentException("Balance cannot be negative");
            _balance = value;
        }
    }
    
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");
        Balance += amount;
    }
    
    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds");
        Balance -= amount;
    }
}
```

---

## 4. Auto-Implemented Properties

```csharp
public class Person
{
    // Simple auto-property
    public string Name { get; set; }
    
    // Read-only (no setter)
    public int Age { get; private set; }
    
    public Person(string name, int age)
    {
        Name = name;
        Age = age;  // Can set in constructor
    }
}
```

---

## ✅ Next Topic

Continue to: [Topic 05 - Inheritance](./Topic_05_Inheritance.md)
