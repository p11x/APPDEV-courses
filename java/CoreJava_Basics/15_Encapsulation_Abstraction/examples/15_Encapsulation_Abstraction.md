# Java Encapsulation and Abstraction

## Table of Contents
1. [Encapsulation](#encapsulation)
2. [Access Modifiers](#access-modifiers)
3. [Getters and Setters](#getters-and-setters)
4. [Data Validation](#data-validation)
5. [Immutability](#immutability)
6. [Abstraction](#abstraction)
7. [Abstract Classes](#abstract-classes)
8. [Interfaces](#interfaces)
9. [Code Examples](#code-examples)
10. [Exercises](#exercises)
11. [Solutions](#solutions)

---

## 1. Encapsulation

### What is Encapsulation?

**Encapsulation** is the bundling of data (fields) and methods that operate on that data into a single unit (class). It restricts direct access to some of an object's components to prevent accidental interference and misuse.

### Benefits of Encapsulation

```
┌─────────────────────────────────────────────────────────────┐
│                    ENCAPSULATION                              │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   ┌─────────────────────────────────────────────┐            │
│   │              Class (Capsule)                │            │
│   │  ┌──────────────┐  ┌──────────────────┐   │            │
│   │  │    DATA      │  │     METHODS      │   │            │
│   │  │  (Fields)    │  │  (Operations)    │   │            │
│   │  │              │  │                  │   │            │
│   │  │  - name      │  │  - getName()     │   │            │
│   │  │  - age       │  │  - setName()     │   │            │
│   │  │  - balance   │  │  - calculate()   │   │            │
│   │  └──────────────┘  └──────────────────┘   │            │
│   │                                              │            │
│   │  PRIVATE ──────► PUBLIC API                  │            │
│   │  (Hidden)      (Controlled Access)          │            │
│   └─────────────────────────────────────────────┘            │
│                                                              │
│   BENEFITS:                                                   │
│   ✓ Data Protection    ✓ Flexibility      ✓ Reusability    │
│   ✓ Maintainability    ✓ Information Hiding                 │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. Access Modifiers

### Types of Access Modifiers in Java

| Modifier | Same Class | Same Package | Subclass | Other Packages |
|----------|-----------|--------------|----------|-----------------|
| `public` | ✓ | ✓ | ✓ | ✓ |
| `protected` | ✓ | ✓ | ✓ | ✗ |
| `default` (no modifier) | ✓ | ✓ | ✗ | ✗ |
| `private` | ✓ | ✗ | ✗ | ✗ |

---

## 3. Getters and Setters

### What are Getters and Setters?

Getters and Setters provide controlled access to private fields.

```java
/**
 * Example: Without encapsulation (BAD)
 */
class BankAccountBad {
    public String accountNumber;  // Public - BAD!
    public double balance;        // Public - BAD!
}

// Anyone can do this:
BankAccountBad account = new BankAccountBad();
account.balance = -1000000;  // Invalid! But allowed

/**
 * Example: With encapsulation (GOOD)
 */
class BankAccountGood {
    private double balance;  // Private - protected
    
    // Getter - read-only access
    public double getBalance() {
        return balance;
    }
    
    // Setter - with validation
    public void setBalance(double balance) {
        if (balance >= 0) {
            this.balance = balance;
        } else {
            System.out.println("Invalid balance!");
        }
    }
}
```

---

## 4. Data Validation

### Validation in Setters

```java
/**
 * Validation in setters prevents invalid data
 */
class Employee {
    private String name;
    private int age;
    private double salary;
    
    public void setName(String name) {
        if (name != null && !name.trim().isEmpty()) {
            this.name = name;
        } else {
            throw new IllegalArgumentException("Name cannot be empty");
        }
    }
    
    public void setAge(int age) {
        if (age >= 0 && age <= 150) {
            this.age = age;
        } else {
            throw new IllegalArgumentException("Invalid age");
        }
    }
    
    public void setSalary(double salary) {
        if (salary >= 0) {
            this.salary = salary;
        } else {
            throw new IllegalArgumentException("Salary cannot be negative");
        }
    }
}
```

---

## 5. Immutability

### Creating Immutable Classes

```java
/**
 * Immutable class - cannot be modified after creation
 */
public final class Person {
    private final String name;
    private final int age;
    private final String email;
    
    public Person(String name, int age, String email) {
        this.name = name;
        this.age = age;
        this.email = email;
    }
    
    // Only getters - no setters!
    public String getName() { return name; }
    public int getAge() { return age; }
    public String getEmail() { return email; }
    
    // To "modify", create a new object
    public Person withName(String newName) {
        return new Person(newName, this.age, this.email);
    }
}
```

---

## 6. Abstraction

### What is Abstraction?

**Abstraction** hides implementation details and shows only functionality to users.

```
┌─────────────────────────┬─────────────────────────────────────┐
│     ENCAPSULATION       │          ABSTRACTION               │
├─────────────────────────┼─────────────────────────────────────┤
│ Data + Methods bundling │  Hiding complexity                 │
│ "How" - Implementation │  "What" - Interface                 │
│ Private fields          │  Abstract methods                  │
│ Controlled access       │  Show essentials, hide details    │
└─────────────────────────┴─────────────────────────────────────┘
```

---

## 7. Abstract Classes

### Abstract Class Syntax

```java
/**
 * Abstract class - cannot be instantiated
 */
abstract class Animal {
    protected String name;
    
    // Regular method
    public void eat() {
        System.out.println(name + " is eating");
    }
    
    // Abstract method - no body, must be implemented by subclass
    public abstract void makeSound();
}

// Concrete class extends abstract class
class Dog extends Animal {
    public Dog(String name) {
        this.name = name;
    }
    
    @Override
    public void makeSound() {
        System.out.println(name + " says: Woof!");
    }
}
```

---

## 8. Interfaces

### What is an Interface?

An **interface** is a contract that defines what a class must do but not how.

```java
/**
 * Interface definition
 */
interface Drawable {
    void draw();  // Abstract method - no implementation
    
    // Java 8+ allows default methods
    default void display() {
        System.out.println("Displaying...");
    }
    
    // Static method
    static void printInfo() {
        System.out.println("Drawable interface");
    }
}

/**
 * Class implements interface
 */
class Circle implements Drawable {
    private double radius;
    
    public Circle(double radius) {
        this.radius = radius;
    }
    
    @Override
    public void draw() {
        System.out.println("Drawing circle with radius " + radius);
    }
}
```

### Interface vs Abstract Class

| Feature | Abstract Class | Interface |
|---------|---------------|-----------|
| Methods | Abstract + concrete | Abstract + default (Java 8+) |
| Fields | Can have any type | Public static final only |
| Constructor | Can have | Cannot have |
| Inheritance | extends (one) | implements (multiple) |
| Access modifiers | Any | Public only |

---

## 9. Code Examples

### Example 1: Bank Account with Full Encapsulation

```java
/**
 * BankAccount - Fully encapsulated class
 * Demonstrates data protection and validation
 */
public class BankAccount {
    // Private fields - hidden from outside
    private String accountNumber;
    private String holderName;
    private double balance;
    private String accountType;
    private boolean isActive;
    
    // ==================== CONSTRUCTORS ====================
    
    public BankAccount() {
        this.accountNumber = "000000";
        this.holderName = "Unknown";
        this.balance = 0.0;
        this.accountType = "Basic";
        this.isActive = true;
    }
    
    public BankAccount(String accountNumber, String holderName, 
                      double balance, String accountType) {
        this.accountNumber = accountNumber;
        this.holderName = holderName;
        this.balance = balance;
        this.accountType = accountType;
        this.isActive = true;
    }
    
    // ==================== GETTERS ====================
    
    public String getAccountNumber() {
        return accountNumber;
    }
    
    public String getHolderName() {
        return holderName;
    }
    
    public double getBalance() {
        return balance;
    }
    
    public String getAccountType() {
        return accountType;
    }
    
    public boolean isActive() {
        return isActive;
    }
    
    // ==================== SETTERS WITH VALIDATION ====================
    
    public void setHolderName(String holderName) {
        if (holderName != null && !holderName.trim().isEmpty()) {
            this.holderName = holderName;
        } else {
            System.out.println("Error: Invalid holder name!");
        }
    }
    
    public void setAccountType(String accountType) {
        if (accountType != null && 
           (accountType.equals("Basic") || accountType.equals("Premium"))) {
            this.accountType = accountType;
        } else {
            System.out.println("Error: Invalid account type!");
        }
    }
    
    // ==================== BUSINESS METHODS ====================
    
    public void deposit(double amount) {
        if (amount > 0 && isActive) {
            balance += amount;
            System.out.println("Deposited: $" + amount);
        } else {
            System.out.println("Error: Cannot deposit!");
        }
    }
    
    public boolean withdraw(double amount) {
        if (amount > 0 && amount <= balance && isActive) {
            balance -= amount;
            System.out.println("Withdrawn: $" + amount);
            return true;
        } else {
            System.out.println("Error: Cannot withdraw!");
            return false;
        }
    }
    
    public void displayInfo() {
        System.out.println("╔════════════════════════════════════╗");
        System.out.println("║      BANK ACCOUNT INFO             ║");
        System.out.println("╠════════════════════════════════════╣");
        System.out.println("║ Account #: " + accountNumber);
        System.out.println("║ Holder:   " + holderName);
        System.out.println("║ Balance:  $" + balance);
        System.out.println("║ Type:     " + accountType);
        System.out.println("║ Status:   " + (isActive ? "Active" : "Inactive"));
        System.out.println("╚════════════════════════════════════╝");
    }
}

/**
 * BankAccountDemo - Test the encapsulated class
 */
public class BankAccountDemo {
    public static void main(String[] args) {
        System.out.println("=== ENCAPSULATION DEMO: BANK ACCOUNT ===\n");
        
        // Create account
        BankAccount account = new BankAccount("ACC123456", "John Doe", 5000, "Premium");
        
        // Display info using getter
        System.out.println("Initial Balance: $" + account.getBalance());
        
        // Deposit using business method
        account.deposit(1000);
        
        // Try invalid operations
        account.deposit(-500);  // Should fail validation
        
        // Withdraw
        account.withdraw(2000);
        
        // Display full info
        account.displayInfo();
        
        // Try to set invalid name
        System.out.println("\n--- Testing Validation ---");
        account.setHolderName("");  // Should fail
        account.setHolderName("Jane Smith");  // Should work
        System.out.println("New Holder: " + account.getHolderName());
    }
}
```

---

### Example 2: Abstract Class - Shape Hierarchy

```java
/**
 * Abstract Shape class
 * Demonstrates abstraction with abstract methods
 */
abstract class Shape {
    protected String color;
    
    // Constructor
    public Shape(String color) {
        this.color = color;
    }
    
    // Concrete method
    public String getColor() {
        return color;
    }
    
    public void setColor(String color) {
        this.color = color;
    }
    
    // Abstract methods - no implementation
    public abstract double getArea();
    public abstract double getPerimeter();
    
    // Concrete method using abstract methods
    public void displayDetails() {
        System.out.println("Color: " + color);
        System.out.println("Area: " + getArea());
        System.out.println("Perimeter: " + getPerimeter());
    }
}

/**
 * Circle - Concrete implementation
 */
class Circle extends Shape {
    private double radius;
    
    public Circle(double radius, String color) {
        super(color);
        this.radius = radius;
    }
    
    public double getRadius() { return radius; }
    public void setRadius(double radius) { this.radius = radius; }
    
    @Override
    public double getArea() {
        return Math.PI * radius * radius;
    }
    
    @Override
    public double getPerimeter() {
        return 2 * Math.PI * radius;
    }
}

/**
 * Rectangle - Concrete implementation
 */
class Rectangle extends Shape {
    private double width;
    private double height;
    
    public Rectangle(double width, double height, String color) {
        super(color);
        this.width = width;
        this.height = height;
    }
    
    public double getWidth() { return width; }
    public void setWidth(double width) { this.width = width; }
    public double getHeight() { return height; }
    public void setHeight(double height) { this.height = height; }
    
    @Override
    public double getArea() {
        return width * height;
    }
    
    @Override
    public double getPerimeter() {
        return 2 * (width + height);
    }
}

/**
 * Triangle - Concrete implementation
 */
class Triangle extends Shape {
    private double side1, side2, side3;
    
    public Triangle(double side1, double side2, double side3, String color) {
        super(color);
        this.side1 = side1;
        this.side2 = side2;
        this.side3 = side3;
    }
    
    @Override
    public double getArea() {
        double s = (side1 + side2 + side3) / 2;
        return Math.sqrt(s * (s - side1) * (s - side2) * (s - side3));
    }
    
    @Override
    public double getPerimeter() {
        return side1 + side2 + side3;
    }
}

/**
 * ShapeDemo - Test abstraction
 */
public class ShapeDemo {
    public static void main(String[] args) {
        System.out.println("=== ABSTRACTION DEMO: SHAPES ===\n");
        
        // Cannot instantiate abstract class
        // Shape shape = new Shape("Red");  // ERROR!
        
        // Create concrete implementations
        Circle circle = new Circle(5, "Red");
        Rectangle rectangle = new Rectangle(4, 6, "Blue");
        Triangle triangle = new Triangle(3, 4, 5, "Green");
        
        // Polymorphism: treat all as Shape
        Shape[] shapes = {circle, rectangle, triangle};
        
        double totalArea = 0;
        for (Shape shape : shapes) {
            System.out.println("--- " + shape.getClass().getSimpleName() + " ---");
            shape.displayDetails();
            totalArea += shape.getArea();
            System.out.println();
        }
        
        System.out.println("Total Area: " + totalArea);
    }
}
```

---

### Example 3: Interfaces for Payment System

```java
/**
 * PaymentProcessor - Interface defining payment operations
 * This is common in Angular + Java backends
 */
interface PaymentProcessor {
    void processPayment(double amount);
    void refundPayment(double amount);
    boolean validateCard(String cardNumber);
}

/**
 * CreditCardPayment - Implements PaymentProcessor
 */
class CreditCardPayment implements PaymentProcessor {
    private String cardNumber;
    private String cardHolder;
    private double balance;
    
    public CreditCardPayment(String cardNumber, String cardHolder, double balance) {
        this.cardNumber = cardNumber;
        this.cardHolder = cardHolder;
        this.balance = balance;
    }
    
    @Override
    public void processPayment(double amount) {
        if (validateCard(cardNumber) && amount <= balance) {
            balance -= amount;
            System.out.println("Credit card payment of $" + amount + " processed");
        } else {
            System.out.println("Payment failed!");
        }
    }
    
    @Override
    public void refundPayment(double amount) {
        balance += amount;
        System.out.println("Refunded $" + amount + " to card");
    }
    
    @Override
    public boolean validateCard(String cardNumber) {
        return cardNumber != null && cardNumber.length() == 16;
    }
    
    public double getBalance() { return balance; }
}

/**
 * PayPalPayment - Another implementation
 */
class PayPalPayment implements PaymentProcessor {
    private String email;
    private double walletBalance;
    
    public PayPalPayment(String email, double walletBalance) {
        this.email = email;
        this.walletBalance = walletBalance;
    }
    
    @Override
    public void processPayment(double amount) {
        if (amount <= walletBalance) {
            walletBalance -= amount;
            System.out.println("PayPal payment of $" + amount + " processed");
        } else {
            System.out.println("Insufficient PayPal balance!");
        }
    }
    
    @Override
    public void refundPayment(double amount) {
        walletBalance += amount;
        System.out.println("Refunded $" + amount + " to PayPal");
    }
    
    @Override
    public boolean validateCard(String cardNumber) {
        // PayPal doesn't use card numbers directly
        return email != null && email.contains("@");
    }
}

/**
 * CryptoPayment - Cryptocurrency implementation
 */
class CryptoPayment implements PaymentProcessor {
    private String walletAddress;
    private double cryptoBalance;
    
    public CryptoPayment(String walletAddress, double cryptoBalance) {
        this.walletAddress = walletAddress;
        this.cryptoBalance = cryptoBalance;
    }
    
    @Override
    public void processPayment(double amount) {
        if (amount <= cryptoBalance) {
            cryptoBalance -= amount;
            System.out.println("Crypto payment of $" + amount + " processed");
        } else {
            System.out.println("Insufficient crypto balance!");
        }
    }
    
    @Override
    public void refundPayment(double amount) {
        cryptoBalance += amount;
        System.out.println("Refunded $" + amount + " to crypto wallet");
    }
    
    @Override
    public boolean validateCard(String cardNumber) {
        // Crypto wallets validated by address
        return walletAddress != null && walletAddress.length() > 10;
    }
}

/**
 * PaymentDemo - Test interface implementations
 */
public class PaymentDemo {
    public static void main(String[] args) {
        System.out.println("=== INTERFACE DEMO: PAYMENT PROCESSORS ===\n");
        
        // Different payment methods implementing same interface
        PaymentProcessor[] processors = {
            new CreditCardPayment("1234567890123456", "John Doe", 5000),
            new PayPalPayment("john@example.com", 1000),
            new CryptoPayment("0x1234567890abcdef", 10)
        };
        
        double paymentAmount = 100;
        
        // Polymorphism: same interface, different implementations
        for (PaymentProcessor processor : processors) {
            System.out.println("Processing payment with: " + 
                             processor.getClass().getSimpleName());
            processor.processPayment(paymentAmount);
            System.out.println();
        }
    }
}
```

---

## 10. Exercises

### Exercise 1: Student Class with Encapsulation

**Requirements:**
1. Create a `Student` class with private fields: id, name, age, gpa, email
2. Implement getters and setters with validation:
   - Name: not empty
   - Age: 0-150
   - GPA: 0.0-4.0
   - Email: must contain @
3. Create displayInfo() method

---

### Exercise 2: Abstract Shape Calculator

**Requirements:**
1. Create abstract class `Shape3D` with abstract methods: getVolume(), getSurfaceArea()
2. Create `Sphere` class extending Shape3D
3. Create `Cube` class extending Shape3D
4. Create `Cylinder` class extending Shape3D
5. Test all with a main method

---

### Exercise 3: Interface for Data Repository

**Requirements:**
1. Create `Repository<T>` interface with methods: save(T), findById(id), findAll(), delete(id)
2. Implement `UserRepository` implementing Repository for User objects
3. Create simple User class and test CRUD operations

---

## 11. Solutions

### Solution 1: Encapsulated Student Class

```java
/**
 * Student - Fully encapsulated class
 */
class Student {
    private int id;
    private String name;
    private int age;
    private double gpa;
    private String email;
    
    // Constructors
    public Student() {}
    
    public Student(int id, String name, int age, double gpa, String email) {
        this.id = id;
        this.setName(name);
        this.setAge(age);
        this.setGpa(gpa);
        this.setEmail(email);
    }
    
    // Getters
    public int getId() { return id; }
    public String getName() { return name; }
    public int getAge() { return age; }
    public double getGpa() { return gpa; }
    public String getEmail() { return email; }
    
    // Setters with validation
    public void setName(String name) {
        if (name != null && !name.trim().isEmpty()) {
            this.name = name;
        } else {
            throw new IllegalArgumentException("Name cannot be empty");
        }
    }
    
    public void setAge(int age) {
        if (age >= 0 && age <= 150) {
            this.age = age;
        } else {
            throw new IllegalArgumentException("Age must be between 0 and 150");
        }
    }
    
    public void setGpa(double gpa) {
        if (gpa >= 0.0 && gpa <= 4.0) {
            this.gpa = gpa;
        } else {
            throw new IllegalArgumentException("GPA must be between 0.0 and 4.0");
        }
    }
    
    public void setEmail(String email) {
        if (email != null && email.contains("@")) {
            this.email = email;
        } else {
            throw new IllegalArgumentException("Invalid email format");
        }
    }
    
    public void displayInfo() {
        System.out.println("ID: " + id + ", Name: " + name + 
                          ", Age: " + age + ", GPA: " + gpa + ", Email: " + email);
    }
}

public class StudentEncapsulationDemo {
    public static void main(String[] args) {
        Student s = new Student(1, "Alice", 20, 3.8, "alice@email.com");
        s.displayInfo();
        
        // Test validation
        try {
            s.setGpa(5.0);  // Invalid
        } catch (IllegalArgumentException e) {
            System.out.println("Error: " + e.getMessage());
        }
    }
}
```

---

### Solution 2: Abstract Shape Classes

```java
abstract class Shape3D {
    protected String name;
    
    public Shape3D(String name) {
        this.name = name;
    }
    
    public abstract double getVolume();
    public abstract double getSurfaceArea();
    
    public void display() {
        System.out.println(name + " - Volume: " + getVolume() + 
                          ", Surface Area: " + getSurfaceArea());
    }
}

class Sphere extends Shape3D {
    private double radius;
    
    public Sphere(double radius) {
        super("Sphere");
        this.radius = radius;
    }
    
    @Override
    public double getVolume() {
        return (4.0/3) * Math.PI * Math.pow(radius, 3);
    }
    
    @Override
    public double getSurfaceArea() {
        return 4 * Math.PI * radius * radius;
    }
}

class Cube extends Shape3D {
    private double side;
    
    public Cube(double side) {
        super("Cube");
        this.side = side;
    }
    
    @Override
    public double getVolume() {
        return Math.pow(side, 3);
    }
    
    @Override
    public double getSurfaceArea() {
        return 6 * side * side;
    }
}

class Cylinder extends Shape3D {
    private double radius;
    private double height;
    
    public Cylinder(double radius, double height) {
        super("Cylinder");
        this.radius = radius;
        this.height = height;
    }
    
    @Override
    public double getVolume() {
        return Math.PI * radius * radius * height;
    }
    
    @Override
    public double getSurfaceArea() {
        return 2 * Math.PI * radius * (radius + height);
    }
}

public class Shape3DDemo {
    public static void main(String[] args) {
        Shape3D[] shapes = {
            new Sphere(5),
            new Cube(4),
            new Cylinder(3, 7)
        };
        
        for (Shape3D shape : shapes) {
            shape.display();
        }
    }
}
```

---

### Solution 3: Repository Interface

```java
/**
 * Generic Repository interface
 */
interface Repository<T> {
    void save(T entity);
    T findById(int id);
    java.util.List<T> findAll();
    void delete(int id);
}

/**
 * User class
 */
class User {
    private int id;
    private String name;
    private String email;
    
    public User(int id, String name, String email) {
        this.id = id;
        this.name = name;
        this.email = email;
    }
    
    public int getId() { return id; }
    public String getName() { return name; }
    public String getEmail() { return email; }
    
    @Override
    public String toString() {
        return "User{id=" + id + ", name='" + name + "', email='" + email + "'}";
    }
}

/**
 * UserRepository implementation
 */
class UserRepository implements Repository<User> {
    private java.util.Map<Integer, User> database = new java.util.HashMap<>();
    
    @Override
    public void save(User user) {
        database.put(user.getId(), user);
        System.out.println("Saved: " + user);
    }
    
    @Override
    public User findById(int id) {
        return database.get(id);
    }
    
    @Override
    public java.util.List<User> findAll() {
        return new java.util.ArrayList<>(database.values());
    }
    
    @Override
    public void delete(int id) {
        database.remove(id);
        System.out.println("Deleted user with id: " + id);
    }
}

public class RepositoryDemo {
    public static void main(String[] args) {
        UserRepository repo = new UserRepository();
        
        // CRUD Operations
        repo.save(new User(1, "Alice", "alice@email.com"));
        repo.save(new User(2, "Bob", "bob@email.com"));
        
        System.out.println("All users: " + repo.findAll());
        System.out.println("Find by id 1: " + repo.findById(1));
        
        repo.delete(1);
        System.out.println("After delete: " + repo.findAll());
    }
}
```

---

## Summary

### Key Takeaways

1. **Encapsulation** protects data using private fields + public methods
2. **Access modifiers** control visibility (private, protected, public)
3. **Getters/Setters** provide controlled access to fields
4. **Validation** in setters prevents invalid data
5. **Immutable classes** cannot be modified after creation
6. **Abstraction** hides complexity using abstract classes/interfaces
7. **Interfaces** define contracts (multiple inheritance possible)

### Angular Backend Connection

- Java entities use encapsulation for data protection
- Abstract classes and interfaces model service contracts
- REST APIs often return abstract types as JSON
- Spring Data uses repository interfaces

### Next Topics

- Control Flow Statements
- Strings and String Handling
- Arrays

---

*Happy Coding! 🚀*
