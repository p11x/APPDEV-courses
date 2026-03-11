# Java Classes and Objects - OOP Fundamentals

## Table of Contents
1. [Introduction to OOP](#introduction-to-oop)
2. [Classes in Java](#classes-in-java)
3. [Objects in Java](#objects-in-java)
4. [Constructors](#constructors)
5. [The `this` Keyword](#the-this-keyword)
6. [Code Examples](#code-examples)
7. [Exercises](#exercises)
8. [Solutions](#solutions)

---

## 1. Introduction to OOP

### What is Object-Oriented Programming?

**Object-Oriented Programming (OOP)** is a programming paradigm that uses "objects" to represent data and methods to manipulate that data. Java is a fully object-oriented language, meaning everything in Java is an object (except for primitive types).

### Key OOP Concepts

| Concept | Description |
|---------|-------------|
| **Encapsulation** | Bundling data and methods that operate on that data into a single unit (class) |
| **Inheritance** | Mechanism where a class acquires the properties and behaviors of another class |
| **Polymorphism** | Ability of objects to take on many forms; method overloading and overriding |
| **Abstraction** | Hiding complex implementation details and showing only essential features |

### Why OOP Matters for Angular Developers?

When building full-stack applications with Angular and Java backend:
- Your Java backend will use classes to model data (entities, DTOs)
- REST APIs return objects that Angular consumes as JSON
- Understanding OOP helps you design better data models
- Spring Boot (popular Java framework) relies heavily on OOP principles

---

## 2. Classes in Java

### What is a Class?

A **class** is a blueprint or template for creating objects. It defines:
- **Fields/Attributes** - Variables that hold data
- **Methods** - Functions that perform operations
- **Constructors** - Special methods for initializing objects

### Class Syntax

```java
// Class declaration
public class ClassName {
    // Fields (also called instance variables)
    fieldType fieldName;
    
    // Constructors
    ClassName() {
        // constructor body
    }
    
    // Methods
    returnType methodName(parameters) {
        // method body
        return value;
    }
}
```

### Class Components

```
┌─────────────────────────────────────────┐
│            Class Name                   │
├─────────────────────────────────────────┤
│  FIELDS (Instance Variables)            │
│  - data members                         │
│  - state of the object                  │
├─────────────────────────────────────────┤
│  CONSTRUCTORS                           │
│  - initialize new objects               │
│  - same name as class                   │
├─────────────────────────────────────────┤
│  METHODS (Member Functions)             │
│  - behavior of the object               │
│  - actions that object can perform      │
└─────────────────────────────────────────┘
```

---

## 3. Objects in Java

### What is an Object?

An **object** is an instance of a class. It represents a real-world entity with:
- **State** - Values stored in fields
- **Behavior** - Actions defined by methods
- **Identity** - Unique reference to the object

### Creating Objects

Objects are created using the `new` keyword:

```java
// Syntax: ClassName objectName = new Constructor();
MyClass obj = new MyClass();
```

### Object Lifecycle

```
┌──────────────┐     ┌──────────────┐     ┌──────────────┐
│   CREATION   │────▶│    USAGE    │────▶│  DESTRUCTION │
│              │     │             │     │              │
│ new keyword  │     │ Call methods│     │ Garbage      │
│ allocation   │     │ Access      │     │ Collection   │
└──────────────┘     └──────────────┘     └──────────────┘
```

---

## 4. Constructors

### Types of Constructors

| Type | Description | When to Use |
|------|-------------|-------------|
| **Default Constructor** | No parameters, provided by Java if no constructor defined | When default initialization is sufficient |
| **Parameterized Constructor** | Accepts parameters | When you need to initialize with specific values |
| **Copy Constructor** | Creates object as copy of another | When you need to clone objects |

### Constructor Examples

```java
// Default Constructor
public class Student {
    String name;
    int age;
    
    // Default constructor (no arguments)
    public Student() {
        this.name = "Unknown";
        this.age = 0;
    }
}

// Parameterized Constructor
public class Student {
    String name;
    int age;
    
    // Parameterized constructor
    public Student(String name, int age) {
        this.name = name;
        this.age = age;
    }
}
```

### Constructor Overloading

You can have multiple constructors with different parameters:

```java
public class Student {
    String name;
    int age;
    String course;
    
    // Constructor 1: No parameters
    public Student() {
        this.name = "Unknown";
        this.age = 0;
        this.course = "None";
    }
    
    // Constructor 2: Two parameters
    public Student(String name, int age) {
        this.name = name;
        this.age = age;
        this.course = "None";
    }
    
    // Constructor 3: Three parameters
    public Student(String name, int age, String course) {
        this.name = name;
        this.age = age;
        this.course = course;
    }
}
```

---

## 5. The `this` Keyword

### Uses of `this`

The `this` keyword refers to the current object. It has several uses:

1. **Distinguish instance variables from local variables**
2. **Call one constructor from another (constructor chaining)**
3. **Pass current object as parameter**
4. **Return current object from method**

### Example: `this` to Avoid Variable Shadowing

```java
public class Employee {
    String name;      // instance variable
    int salary;       // instance variable
    
    // Without 'this' - parameter names shadow instance variables
    public void setData(String name, int salary) {
        name = name;      // WRONG: assigns parameter to itself
        salary = salary;  // WRONG: assigns parameter to itself
    }
    
    // With 'this' - correctly assigns to instance variables
    public void setData(String name, int salary) {
        this.name = name;      // CORRECT: instance = parameter
        this.salary = salary;  // CORRECT: instance = parameter
    }
}
```

### Example: Constructor Chaining with `this()`

```java
public class User {
    String username;
    String email;
    String password;
    boolean active;
    
    // Constructor 1: Minimal info
    public User(String username) {
        this(username, "default@email.com");  // call constructor 2
    }
    
    // Constructor 2: Username and email
    public User(String username, String email) {
        this(username, email, "password123");  // call constructor 3
    }
    
    // Constructor 3: All parameters
    public User(String username, String email, String password) {
        this.username = username;
        this.email = email;
        this.password = password;
        this.active = true;
    }
}
```

---

## 6. Code Examples

### Example 1: Basic Class and Object

```java
/**
 * BankAccount - A simple class representing a bank account
 * This demonstrates fundamental class structure and object creation
 * 
 * @author Java Learning
 * @version 1.0
 */
public class BankAccount {
    // ==================== FIELDS ====================
    // These represent the state/data of each BankAccount object
    private String accountNumber;    // Unique account identifier
    private String accountHolderName; // Name of the account owner
    private double balance;          // Current account balance
    
    // ==================== CONSTRUCTORS ====================
    
    /**
     * Default constructor - creates account with default values
     */
    public BankAccount() {
        this.accountNumber = "000000";
        this.accountHolderName = "Unknown";
        this.balance = 0.0;
    }
    
    /**
     * Parameterized constructor - creates account with specified values
     * @param accountNumber The unique account number
     * @param accountHolderName The name of the account holder
     * @param balance The initial balance
     */
    public BankAccount(String accountNumber, String accountHolderName, double balance) {
        this.accountNumber = accountNumber;
        this.accountHolderName = accountHolderName;
        this.balance = balance;
    }
    
    // ==================== GETTERS ====================
    // Public methods to access private fields (Encapsulation)
    
    public String getAccountNumber() {
        return this.accountNumber;
    }
    
    public String getAccountHolderName() {
        return this.accountHolderName;
    }
    
    public double getBalance() {
        return this.balance;
    }
    
    // ==================== SETTERS ====================
    // Public methods to modify private fields
    
    public void setAccountHolderName(String name) {
        this.accountHolderName = name;
    }
    
    public void setBalance(double balance) {
        if (balance >= 0) {
            this.balance = balance;
        } else {
            System.out.println("Error: Balance cannot be negative!");
        }
    }
    
    // ==================== METHODS ====================
    // These represent the behavior of BankAccount
    
    /**
     * Deposit money into the account
     * @param amount The amount to deposit
     * @return true if deposit was successful, false otherwise
     */
    public boolean deposit(double amount) {
        if (amount > 0) {
            this.balance += amount;
            System.out.println("Deposited: $" + amount);
            return true;
        } else {
            System.out.println("Error: Deposit amount must be positive!");
            return false;
        }
    }
    
    /**
     * Withdraw money from the account
     * @param amount The amount to withdraw
     * @return true if withdrawal was successful, false otherwise
     */
    public boolean withdraw(double amount) {
        if (amount > 0 && amount <= this.balance) {
            this.balance -= amount;
            System.out.println("Withdrawn: $" + amount);
            return true;
        } else {
            System.out.println("Error: Invalid withdrawal amount!");
            return false;
        }
    }
    
    /**
     * Display account information
     */
    public void displayAccountInfo() {
        System.out.println("╔════════════════════════════════════╗");
        System.out.println("║        ACCOUNT INFORMATION          ║");
        System.out.println("╠════════════════════════════════════╣");
        System.out.println("║ Account Number: " + this.accountNumber);
        System.out.println("║ Holder Name:    " + this.accountHolderName);
        System.out.println("║ Balance:        $" + this.balance);
        System.out.println("╚════════════════════════════════════╝");
    }
    
    /**
     * Main method - demonstrates class usage
     */
    public static void main(String[] args) {
        System.out.println("=== BANK ACCOUNT DEMO ===\n");
        
        // Create first account using default constructor
        BankAccount account1 = new BankAccount();
        System.out.println("--- Account 1 (Default Constructor) ---");
        account1.displayAccountInfo();
        
        // Create second account using parameterized constructor
        BankAccount account2 = new BankAccount("ACC123456", "John Smith", 5000.00);
        System.out.println("\n--- Account 2 (Parameterized Constructor) ---");
        account2.displayAccountInfo();
        
        // Perform operations on account2
        System.out.println("\n--- Operations on Account 2 ---");
        account2.deposit(1000.00);    // Deposit $1000
        account2.withdraw(500.00);    // Withdraw $500
        account2.displayAccountInfo();
        
        // Test validation - try to set negative balance
        System.out.println("\n--- Testing Validation ---");
        account2.setBalance(-1000);   // This should fail
    }
}
```

**Output:**
```
=== BANK ACCOUNT DEMO ===

--- Account 1 (Default Constructor) ---
╔════════════════════════════════════╗
║        ACCOUNT INFORMATION          ║
╠════════════════════════════════════╣
║ Account Number: 000000
║ Holder Name:    Unknown
║ Balance:        $0.0
╚════════════════════════════════════╝

--- Account 2 (Parameterized Constructor) ---
╔════════════════════════════════════╗
║        ACCOUNT INFORMATION          ║
╠════════════════════════════════════╣
║ Account Number: ACC123456
║ Holder Name:    John Smith
║ Balance:        $5000.0
╚════════════════════════════════════╝

--- Operations on Account 2 ---
Deposited: $1000.0
Withdrawn: $500.0
╔════════════════════════════════════╗
║        ACCOUNT INFORMATION          ║
╠════════════════════════════════════╣
║ Account Number: ACC123456
║ Holder Name:    John Smith
║ Balance:        $5500.0
╚════════════════════════════════════╝

--- Testing Validation ---
Error: Balance cannot be negative!
```

---

### Example 2: Product Class for E-commerce (Angular Backend Integration)

This example shows how Java classes map to JSON that Angular can consume.

```java
/**
 * Product - Represents a product in an e-commerce system
 * This is the kind of class that gets converted to JSON for Angular
 * 
 * In a real application, you'd use Jackson annotations for JSON mapping
 */
public class Product {
    // Fields - these become JSON properties
    private int id;
    private String name;
    private String description;
    private double price;
    private int quantityInStock;
    private String category;
    private boolean active;
    
    // ==================== CONSTRUCTORS ====================
    
    // Default constructor - required for JSON deserialization
    public Product() {
    }
    
    // Parameterized constructor
    public Product(int id, String name, String description, double price, 
                   int quantityInStock, String category) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.price = price;
        this.quantityInStock = quantityInStock;
        this.category = category;
        this.active = true;
    }
    
    // ==================== GETTERS AND SETTERS ====================
    
    public int getId() {
        return id;
    }
    
    public void setId(int id) {
        this.id = id;
    }
    
    public String getName() {
        return name;
    }
    
    public void setName(String name) {
        this.name = name;
    }
    
    public String getDescription() {
        return description;
    }
    
    public void setDescription(String description) {
        this.description = description;
    }
    
    public double getPrice() {
        return price;
    }
    
    public void setPrice(double price) {
        this.price = price;
    }
    
    public int getQuantityInStock() {
        return quantityInStock;
    }
    
    public void setQuantityInStock(int quantityInStock) {
        this.quantityInStock = quantityInStock;
    }
    
    public String getCategory() {
        return category;
    }
    
    public void setCategory(String category) {
        this.category = category;
    }
    
    public boolean isActive() {
        return active;
    }
    
    public void setActive(boolean active) {
        this.active = active;
    }
    
    // ==================== BUSINESS METHODS ====================
    
    /**
     * Check if product is available for purchase
     */
    public boolean isAvailable() {
        return active && quantityInStock > 0;
    }
    
    /**
     * Calculate total value of this product in inventory
     */
    public double getInventoryValue() {
        return price * quantityInStock;
    }
    
    /**
     * Reduce stock after purchase
     * @param quantity Amount to reduce
     * @return true if successful, false if insufficient stock
     */
    public boolean reduceStock(int quantity) {
        if (quantity > 0 && quantity <= quantityInStock) {
            quantityInStock -= quantity;
            return true;
        }
        return false;
    }
    
    /**
     * Convert to JSON-like string (simplified representation)
     * In real apps, use Jackson ObjectMapper
     */
    @Override
    public String toString() {
        return "{" +
                "\"id\":" + id +
                ", \"name\":\"" + name + "\"" +
                ", \"description\":\"" + description + "\"" +
                ", \"price\":" + price +
                ", \"quantityInStock\":" + quantityInStock +
                ", \"category\":\"" + category + "\"" +
                ", \"active\":" + active +
                "}";
    }
    
    // ==================== MAIN METHOD FOR TESTING ====================
    
    public static void main(String[] args) {
        System.out.println("=== PRODUCT CLASS DEMO ===\n");
        
        // Create a product - like you'd do in a backend service
        Product laptop = new Product(
            1, 
            "MacBook Pro 16\"", 
            "Apple MacBook Pro with M3 chip, 16GB RAM", 
            2499.99, 
            50, 
            "Electronics"
        );
        
        // Display product details
        System.out.println("Product Details:");
        System.out.println("  ID: " + laptop.getId());
        System.out.println("  Name: " + laptop.getName());
        System.out.println("  Price: $" + laptop.getPrice());
        System.out.println("  Stock: " + laptop.getQuantityInStock());
        System.out.println("  Category: " + laptop.getCategory());
        System.out.println("  Available: " + laptop.isAvailable());
        System.out.println("  Inventory Value: $" + laptop.getInventoryValue());
        
        // Show JSON representation (what Angular receives)
        System.out.println("\nJSON Representation (for Angular):");
        System.out.println(laptop.toString());
        
        // Simulate purchase
        System.out.println("\n--- Simulating Purchase ---");
        boolean success = laptop.reduceStock(5);
        System.out.println("Purchase of 5 laptops: " + (success ? "Successful" : "Failed"));
        System.out.println("Remaining stock: " + laptop.getQuantityInStock());
    }
}
```

---

### Example 3: Student Management System

```java
import java.util.ArrayList;
import java.util.List;

/**
 * Student - Represents a student in a college管理系统
 * Demonstrates more complex class design with multiple data types
 */
class Student {
    // ==================== FIELDS ====================
    private int studentId;
    private String firstName;
    private String lastName;
    private String email;
    private int age;
    private String major;
    private double gpa;
    private List<String> enrolledCourses;
    
    // ==================== CONSTRUCTORS ====================
    
    public Student() {
        this.enrolledCourses = new ArrayList<>();
    }
    
    public Student(int studentId, String firstName, String lastName, String email, int age) {
        this.studentId = studentId;
        this.firstName = firstName;
        this.lastName = lastName;
        this.email = email;
        this.age = age;
        this.major = "Undeclared";
        this.gpa = 0.0;
        this.enrolledCourses = new ArrayList<>();
    }
    
    // ==================== GETTERS AND SETTERS ====================
    
    public int getStudentId() { return studentId; }
    public void setStudentId(int studentId) { this.studentId = studentId; }
    
    public String getFirstName() { return firstName; }
    public void setFirstName(String firstName) { this.firstName = firstName; }
    
    public String getLastName() { return lastName; }
    public void setLastName(String lastName) { this.lastName = lastName; }
    
    public String getFullName() {
        return firstName + " " + lastName;
    }
    
    public String getEmail() { return email; }
    public void setEmail(String email) { 
        if (email.contains("@")) {
            this.email = email;
        } else {
            System.out.println("Error: Invalid email format!");
        }
    }
    
    public int getAge() { return age; }
    public void setAge(int age) {
        if (age >= 0 && age <= 150) {
            this.age = age;
        }
    }
    
    public String getMajor() { return major; }
    public void setMajor(String major) { this.major = major; }
    
    public double getGpa() { return gpa; }
    public void setGpa(double gpa) {
        if (gpa >= 0.0 && gpa <= 4.0) {
            this.gpa = gpa;
        }
    }
    
    public List<String> getEnrolledCourses() { return enrolledCourses; }
    
    // ==================== METHODS ====================
    
    public void enrollCourse(String course) {
        if (!enrolledCourses.contains(course)) {
            enrolledCourses.add(course);
            System.out.println(getFullName() + " enrolled in: " + course);
        }
    }
    
    public void dropCourse(String course) {
        if (enrolledCourses.remove(course)) {
            System.out.println(getFullName() + " dropped: " + course);
        } else {
            System.out.println(getFullName() + " is not enrolled in: " + course);
        }
    }
    
    public void displayInfo() {
        System.out.println("┌────────────────────────────────────────┐");
        System.out.println("│           STUDENT INFORMATION          │");
        System.out.println("├────────────────────────────────────────┤");
        System.out.println("│ ID:        " + studentId);
        System.out.println("│ Name:      " + getFullName());
        System.out.println("│ Email:     " + email);
        System.out.println("│ Age:       " + age);
        System.out.println("│ Major:     " + major);
        System.out.println("│ GPA:       " + gpa);
        System.out.println("│ Courses:   " + enrolledCourses);
        System.out.println("└────────────────────────────────────────┘");
    }
}

/**
 * StudentDemo - Demonstrates using the Student class
 */
public class StudentDemo {
    public static void main(String[] args) {
        System.out.println("=== STUDENT MANAGEMENT SYSTEM ===\n");
        
        // Create students
        Student student1 = new Student(1001, "Alice", "Johnson", "alice@college.edu", 20);
        student1.setMajor("Computer Science");
        student1.setGpa(3.85);
        
        Student student2 = new Student(1002, "Bob", "Smith", "bob@college.edu", 22);
        student2.setMajor("Mathematics");
        student2.setGpa(3.5);
        
        // Display student info
        student1.displayInfo();
        System.out.println();
        student2.displayInfo();
        
        // Enroll students in courses
        System.out.println("\n=== ENROLLMENT ===");
        student1.enrollCourse("Data Structures");
        student1.enrollCourse("Algorithms");
        student1.enrollCourse("Web Development");  // This is relevant for Angular!
        
        student2.enrollCourse("Linear Algebra");
        student2.enrollCourse("Calculus III");
        
        // Display updated info
        System.out.println("\n=== UPDATED INFORMATION ===");
        student1.displayInfo();
        
        // Test email validation
        System.out.println("\n=== EMAIL VALIDATION TEST ===");
        student1.setEmail("newemail@college.edu");
        System.out.println("New email: " + student1.getEmail());
        
        student1.setEmail("invalid-email");  // Should show error
    }
}
```

---

## 7. Exercises

### Exercise 1: Create a Rectangle Class

**Objective:** Create a `Rectangle` class that represents a rectangle with width and height.

**Requirements:**
1. Create fields for `width` and `height` (both private, double type)
2. Create a default constructor that sets width and height to 1.0
3. Create a parameterized constructor that accepts width and height
4. Create getter and setter methods for both fields
5. Create methods:
   - `getArea()` - returns the area (width × height)
   - `getPerimeter()` - returns the perimeter (2 × (width + height))
   - `display()` - prints all rectangle information

**Expected Output:**
```
Rectangle 1 (Default):
Width: 1.0, Height: 1.0
Area: 1.0, Perimeter: 4.0

Rectangle 2 (Custom):
Width: 5.0, Height: 3.0
Area: 15.0, Perimeter: 16.0
```

---

### Exercise 2: Create a Book Class for Library System

**Objective:** Create a `Book` class that models a book in a library system.

**Requirements:**
1. Fields: `isbn` (String), `title` (String), `author` (String), `year` (int), `isBorrowed` (boolean)
2. Default constructor and parameterized constructor
3. Getters and setters for all fields
4. Methods:
   - `borrowBook()` - marks book as borrowed if not already borrowed
   - `returnBook()` - marks book as returned if currently borrowed
   - `displayBookInfo()` - prints all book details in formatted way

**Expected Output:**
```
Book: The Great Gatsby
Author: F. Scott Fitzgerald
Year: 1925
ISBN: 978-0743273565
Status: Available

--- Borrowing book ---
Successfully borrowed: The Great Gatsby

--- Attempting to borrow again ---
Book already borrowed!
```

---

### Exercise 3: Create a Calculator Class

**Objective:** Create a `Calculator` class that performs basic arithmetic operations.

**Requirements:**
1. Create methods for:
   - `add(double a, double b)` - returns sum
   - `subtract(double a, double b)` - returns difference
   - `multiply(double a, double b)` - returns product
   - `divide(double a, double b)` - returns quotient (handle division by zero)
   - `power(double base, double exponent)` - returns base^exponent
   - `sqrt(double number)` - returns square root
2. Create a main method that demonstrates all operations

---

### Exercise 4: Design a Car Class (Angular Integration Context)

**Objective:** Create a `Car` class that could be part of a car rental backend API.

**Context:** This class would eventually be converted to JSON and sent to an Angular frontend.

**Requirements:**
1. Fields: `id`, `make`, `model`, `year`, `color`, `pricePerDay`, `isAvailable`
2. Implement constructor and all getters/setters
3. Methods:
   - `calculateRentalCost(int days)` - calculates total rental cost
   - `getCarDetails()` - returns formatted string (simulating JSON output)
   - `rentCar()` - marks car as rented
   - `returnCar()` - marks car as available

---

## 8. Solutions

### Solution 1: Rectangle Class

```java
/**
 * Rectangle - Solution to Exercise 1
 * Represents a rectangle with width and height
 */
public class Rectangle {
    // Private fields - encapsulation
    private double width;
    private double height;
    
    // Default constructor
    public Rectangle() {
        this.width = 1.0;
        this.height = 1.0;
    }
    
    // Parameterized constructor
    public Rectangle(double width, double height) {
        this.width = width;
        this.height = height;
    }
    
    // Getters and Setters
    public double getWidth() {
        return width;
    }
    
    public void setWidth(double width) {
        this.width = width;
    }
    
    public double getHeight() {
        return height;
    }
    
    public void setHeight(double height) {
        this.height = height;
    }
    
    // Calculate area
    public double getArea() {
        return width * height;
    }
    
    // Calculate perimeter
    public double getPerimeter() {
        return 2 * (width + height);
    }
    
    // Display rectangle information
    public void display() {
        System.out.println("Width: " + width + ", Height: " + height);
        System.out.println("Area: " + getArea() + ", Perimeter: " + getPerimeter());
    }
    
    // Main method for testing
    public static void main(String[] args) {
        // Rectangle 1 - Default constructor
        System.out.println("Rectangle 1 (Default):");
        Rectangle rect1 = new Rectangle();
        rect1.display();
        
        System.out.println();
        
        // Rectangle 2 - Parameterized constructor
        System.out.println("Rectangle 2 (Custom):");
        Rectangle rect2 = new Rectangle(5.0, 3.0);
        rect2.display();
    }
}
```

---

### Solution 2: Book Class

```java
/**
 * Book - Solution to Exercise 2
 * Represents a book in a library system
 */
public class Book {
    // Fields
    private String isbn;
    private String title;
    private String author;
    private int year;
    private boolean isBorrowed;
    
    // Default constructor
    public Book() {
        this.isBorrowed = false;
    }
    
    // Parameterized constructor
    public Book(String isbn, String title, String author, int year) {
        this.isbn = isbn;
        this.title = title;
        this.author = author;
        this.year = year;
        this.isBorrowed = false;
    }
    
    // Getters and Setters
    public String getIsbn() { return isbn; }
    public void setIsbn(String isbn) { this.isbn = isbn; }
    
    public String getTitle() { return title; }
    public void setTitle(String title) { this.title = title; }
    
    public String getAuthor() { return author; }
    public void setAuthor(String author) { this.author = author; }
    
    public int getYear() { return year; }
    public void setYear(int year) { this.year = year; }
    
    public boolean isBorrowed() { return isBorrowed; }
    public void setBorrowed(boolean borrowed) { isBorrowed = borrowed; }
    
    // Borrow book method
    public boolean borrowBook() {
        if (!isBorrowed) {
            isBorrowed = true;
            return true;
        }
        return false;
    }
    
    // Return book method
    public boolean returnBook() {
        if (isBorrowed) {
            isBorrowed = false;
            return true;
        }
        return false;
    }
    
    // Display book information
    public void displayBookInfo() {
        System.out.println("Book: " + title);
        System.out.println("Author: " + author);
        System.out.println("Year: " + year);
        System.out.println("ISBN: " + isbn);
        System.out.println("Status: " + (isBorrowed ? "Borrowed" : "Available"));
    }
    
    // Main method for testing
    public static void main(String[] args) {
        // Create a book
        Book book = new Book("978-0743273565", "The Great Gatsby", 
                            "F. Scott Fitzgerald", 1925);
        
        // Display book info
        book.displayBookInfo();
        
        // Test borrowing
        System.out.println("\n--- Borrowing book ---");
        if (book.borrowBook()) {
            System.out.println("Successfully borrowed: " + book.getTitle());
        }
        
        // Try to borrow again
        System.out.println("\n--- Attempting to borrow again ---");
        if (!book.borrowBook()) {
            System.out.println("Book already borrowed!");
        }
        
        // Display updated status
        System.out.println("\n--- Updated Status ---");
        book.displayBookInfo();
    }
}
```

---

### Solution 3: Calculator Class

```java
/**
 * Calculator - Solution to Exercise 3
 * Performs basic arithmetic operations
 */
public class Calculator {
    
    // Addition
    public double add(double a, double b) {
        return a + b;
    }
    
    // Subtraction
    public double subtract(double a, double b) {
        return a - b;
    }
    
    // Multiplication
    public double multiply(double a, double b) {
        return a * b;
    }
    
    // Division - handles division by zero
    public double divide(double a, double b) {
        if (b == 0) {
            System.out.println("Error: Division by zero!");
            return Double.NaN;  // Not a Number
        }
        return a / b;
    }
    
    // Power (a^b)
    public double power(double base, double exponent) {
        return Math.pow(base, exponent);
    }
    
    // Square root
    public double sqrt(double number) {
        if (number < 0) {
            System.out.println("Error: Cannot calculate square root of negative number!");
            return Double.NaN;
        }
        return Math.sqrt(number);
    }
    
    // Main method - demonstrates all operations
    public static void main(String[] args) {
        Calculator calc = new Calculator();
        
        double a = 10;
        double b = 5;
        
        System.out.println("=== CALCULATOR OPERATIONS ===");
        System.out.println("a = " + a + ", b = " + b);
        System.out.println();
        
        System.out.println("Addition:        " + calc.add(a, b));
        System.out.println("Subtraction:    " + calc.subtract(a, b));
        System.out.println("Multiplication: " + calc.multiply(a, b));
        System.out.println("Division:       " + calc.divide(a, b));
        
        // Test power
        System.out.println("\nPower operations:");
        System.out.println("2^8 = " + calc.power(2, 8));
        System.out.println("3^4 = " + calc.power(3, 4));
        
        // Test square root
        System.out.println("\nSquare root operations:");
        System.out.println("sqrt(144) = " + calc.sqrt(144));
        System.out.println("sqrt(81) = " + calc.sqrt(81));
        
        // Test division by zero
        System.out.println("\nDivision by zero test:");
        double result = calc.divide(a, 0);
        System.out.println("Result: " + result);
    }
}
```

---

### Solution 4: Car Class (Angular Integration)

```java
/**
 * Car - Solution to Exercise 4
 * Represents a car in a rental system
 * This class demonstrates how Java objects map to JSON for Angular
 */
public class Car {
    // Fields
    private int id;
    private String make;
    private String model;
    private int year;
    private String color;
    private double pricePerDay;
    private boolean isAvailable;
    
    // Constructors
    public Car() {
        this.isAvailable = true;
    }
    
    public Car(int id, String make, String model, int year, 
               String color, double pricePerDay) {
        this.id = id;
        this.make = make;
        this.model = model;
        this.year = year;
        this.color = color;
        this.pricePerDay = pricePerDay;
        this.isAvailable = true;
    }
    
    // Getters and Setters
    public int getId() { return id; }
    public void setId(int id) { this.id = id; }
    
    public String getMake() { return make; }
    public void setMake(String make) { this.make = make; }
    
    public String getModel() { return model; }
    public void setModel(String model) { this.model = model; }
    
    public int getYear() { return year; }
    public void setYear(int year) { this.year = year; }
    
    public String getColor() { return color; }
    public void setColor(String color) { this.color = color; }
    
    public double getPricePerDay() { return pricePerDay; }
    public void setPricePerDay(double pricePerDay) { this.pricePerDay = pricePerDay; }
    
    public boolean isAvailable() { return isAvailable; }
    public void setAvailable(boolean available) { isAvailable = available; }
    
    // Calculate rental cost
    public double calculateRentalCost(int days) {
        if (days <= 0) {
            System.out.println("Error: Days must be positive!");
            return 0;
        }
        return pricePerDay * days;
    }
    
    // Get car details (simulates JSON output for Angular)
    public String getCarDetails() {
        return "{" +
                "\"id\":" + id +
                ", \"make\":\"" + make + "\"" +
                ", \"model\":\"" + model + "\"" +
                ", \"year\":" + year +
                ", \"color\":\"" + color + "\"" +
                ", \"pricePerDay\":" + pricePerDay +
                ", \"isAvailable\":" + isAvailable +
                "}";
    }
    
    // Rent car
    public boolean rentCar() {
        if (isAvailable) {
            isAvailable = false;
            System.out.println("Car rented successfully!");
            return true;
        }
        System.out.println("Car is not available for rent.");
        return false;
    }
    
    // Return car
    public boolean returnCar() {
        if (!isAvailable) {
            isAvailable = true;
            System.out.println("Car returned successfully!");
            return true;
        }
        System.out.println("Car was not rented.");
        return false;
    }
    
    // Display car info
    public void displayInfo() {
        System.out.println("╔════════════════════════════════════╗");
        System.out.println("║           CAR DETAILS               ║");
        System.out.println("╠════════════════════════════════════╣");
        System.out.println("║ ID:         " + id);
        System.out.println("║ Make:       " + make);
        System.out.println("║ Model:      " + model);
        System.out.println("║ Year:       " + year);
        System.out.println("║ Color:      " + color);
        System.out.println("║ Price/Day:  $" + pricePerDay);
        System.out.println("║ Available:  " + (isAvailable ? "Yes" : "No"));
        System.out.println("╚════════════════════════════════════╝");
    }
    
    // Main method for testing
    public static void main(String[] args) {
        // Create a car
        Car car = new Car(1, "Toyota", "Camry", 2023, "Silver", 49.99);
        
        // Display car information
        car.displayInfo();
        
        // Calculate rental cost
        System.out.println("\nRental Cost for 7 days: $" + car.calculateRentalCost(7));
        
        // Display JSON representation (for Angular)
        System.out.println("\nJSON Output (for Angular Frontend):");
        System.out.println(car.getCarDetails());
        
        // Test rent/return
        System.out.println("\n--- Testing Rent/Return ---");
        car.rentCar();
        System.out.println("Available: " + car.isAvailable());
        
        car.returnCar();
        System.out.println("Available: " + car.isAvailable());
    }
}
```

---

## Summary

### Key Takeaways

1. **Classes are blueprints** - they define the structure and behavior of objects
2. **Objects are instances** - created from classes using the `new` keyword
3. **Constructors initialize** - objects and can be overloaded for flexibility
4. **`this` keyword** - refers to current object and helps avoid variable shadowing
5. **Encapsulation** - use private fields with public getters/setters
6. **For Angular Integration** - Java classes become JSON via serialization libraries

### Next Steps

After mastering classes and objects, continue with:
- **Inheritance** - Creating class hierarchies
- **Polymorphism** - Method overloading and overriding
- **Abstraction** - Abstract classes and interfaces

These concepts will help you build more sophisticated backend systems that integrate seamlessly with Angular frontends!

---

*Happy Coding! 🚀*
