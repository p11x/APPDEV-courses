# Java Inheritance and Polymorphism

## Table of Contents
1. [Introduction to Inheritance](#introduction-to-inheritance)
2. [Types of Inheritance](#types-of-inheritance)
3. [The `extends` Keyword](#the-extends-keyword)
4. [The `super` Keyword](#the-super-keyword)
5. [Method Overriding](#method-overriding)
6. [Polymorphism](#polymorphism)
7. [Code Examples](#code-examples)
8. [Exercises](#exercises)
9. [Solutions](#solutions)

---

## 1. Introduction to Inheritance

### What is Inheritance?

**Inheritance** is one of the fundamental concepts of Object-Oriented Programming (OOP) that allows a class to inherit properties and behaviors from another class. It promotes code reusability and establishes a natural hierarchy between classes.

### Key Terminology

| Term | Description |
|------|-------------|
| **Parent Class (Superclass)** | The class whose properties are inherited |
| **Child Class (Subclass)** | The class that inherits from another class |
| **IS-A Relationship** | Inheritance relationship (e.g., Dog IS-A Animal) |
| **Reusability** | Ability to use existing code without modification |

### Why Inheritance Matters for Angular Developers?

When building full-stack applications:
- Backend entities often share common properties (ID, timestamps)
- REST APIs use inheritance for DTOs and entities
- Spring Data JPA uses inheritance for entities
- Understanding inheritance helps map Angular models to Java backend

---

## 2. Types of Inheritance

### Types of Inheritance in Java

```
┌─────────────────────────────────────────────────────────────┐
│                    TYPES OF INHERITANCE                      │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   Single Inheritance          Multilevel Inheritance         │
│   ─────────────────          ────────────────────           │
│        A                          A                          │
│        │                          │                          │
│        B                          B                          │
│                               │                             │
│                               C                             │
│                                                              │
│   Hierarchical Inheritance      Multiple Inheritance*        │
│   ────────────────────        ─────────────────────         │
│        A                          B    C                    │
│       /│\                         │   │                     │
│      B C D                        └── A                     │
│                                    *Not supported in Java    │
│                                                             │
│   Hybrid Inheritance*                                      │
│   ─────────────────                                        │
│         A                                                  │
│        /│\                                                 │
│       B C D          (Combination of above types)          │
│        ││                                                  │
│        E                                                    │
└─────────────────────────────────────────────────────────────┘
```

### Java Inheritance Restrictions

1. **No Multiple Inheritance** - A class cannot extend multiple classes directly
2. **No Cyclic Inheritance** - A class cannot extend itself
3. **Private Members** - Private fields are not inherited directly

---

## 3. The `extends` Keyword

### Basic Syntax

```java
// Parent class (Superclass)
public class Animal {
    // Fields and methods
}

// Child class (Subclass)
public class Dog extends Animal {
    // Inherits all non-private members from Animal
    // Can add its own fields and methods
}
```

### What Gets Inherited?

| Inherited | Not Inherited |
|-----------|---------------|
| Public fields | Private fields |
| Public methods | Private methods |
| Protected fields | Constructors |
| Protected methods | Static members |
| Package-private members | |

---

## 4. The `super` Keyword

### Uses of `super`

The `super` keyword is used to:
1. **Call parent class constructors**
2. **Access parent class methods**
3. **Access parent class fields**

### Constructor Chaining with `super()`

```java
/**
 * Demonstrates super() for constructor chaining
 */
class Parent {
    String name;
    
    Parent() {
        System.out.println("Parent constructor called");
        this.name = "Parent";
    }
    
    Parent(String name) {
        System.out.println("Parent parameterized constructor called");
        this.name = name;
    }
}

class Child extends Parent {
    int age;
    
    // Automatically calls Parent() first
    Child() {
        super();  // Explicit call to parent constructor
        System.out.println("Child constructor called");
        this.age = 0;
    }
    
    // Calls Parent(String name) with "ChildName"
    Child(String name, int age) {
        super(name);  // Call parent constructor with parameter
        System.out.println("Child parameterized constructor called");
        this.age = age;
    }
}
```

---

## 5. Method Overriding

### What is Method Overriding?

**Method Overriding** occurs when a subclass provides a specific implementation of a method already defined in its parent class.

### Rules for Method Overriding

1. Method must have the **same name** as in parent class
2. Method must have the **same parameters** as in parent class
3. Method must have the **same return type** (or covariant return type)
4. Cannot override `static` methods
5. Cannot override `final` methods
6. Override method cannot be less accessible

### Syntax

```java
// Parent class
class Animal {
    public void makeSound() {
        System.out.println("Animal makes a sound");
    }
}

// Child class
class Dog extends Animal {
    @Override  // Annotation - good practice to include
    public void makeSound() {
        System.out.println("Dog barks: Woof! Woof!");
    }
}
```

---

## 6. Polymorphism

### What is Polymorphism?

**Polymorphism** allows objects of different classes to be treated as objects of a common type. It enables one interface to be used for a general class of actions.

### Types of Polymorphism

```
┌─────────────────────────────────────────────────────────────┐
│                      POLYMORPHISM                           │
├─────────────────────────────┬───────────────────────────────┤
│    COMPILE-TIME             │      RUNTIME                  │
│    (Static)                 │      (Dynamic)                 │
│                             │                                │
│  ┌─────────────┐            │   ┌─────────────┐             │
│  │ Method      │            │   │ Method      │             │
│  │ Overloading │            │   │ Overriding  │             │
│  └─────────────┘            │   └─────────────┘             │
│                             │                                │
│  - Same method name         │   - Same method name          │
│  - Different parameters     │   - Same parameters           │
│  - Same class              │   - Different classes          │
└─────────────────────────────┴───────────────────────────────┘
```

### Method Overloading (Compile-Time Polymorphism)

```java
/**
 * Method Overloading - Same name, different parameters
 */
class Calculator {
    // Overloaded add() methods
    public int add(int a, int b) {
        return a + b;
    }
    
    public double add(double a, double b) {
        return a + b;
    }
    
    public int add(int a, int b, int c) {
        return a + b + c;
    }
    
    public String add(String a, String b) {
        return a + b;
    }
}
```

### Runtime Polymorphism with Method Overriding

```java
/**
 * Runtime Polymorphism through method overriding
 */
class Animal {
    public void sound() {
        System.out.println("Animal makes sound");
    }
}

class Dog extends Animal {
    @Override
    public void sound() {
        System.out.println("Dog barks");
    }
}

class Cat extends Animal {
    @Override
    public void sound() {
        System.out.println("Cat meows");
    }
}

// Main
public class PolymorphismDemo {
    public static void main(String[] args) {
        // Reference of type Animal, object of Dog
        Animal myAnimal = new Dog();
        myAnimal.sound();  // Outputs: Dog barks
        
        myAnimal = new Cat();
        myAnimal.sound();  // Outputs: Cat meows
    }
}
```

---

## 7. Code Examples

### Example 1: Employee Hierarchy (Angular Backend Context)

```java
/**
 * Employee - Base class for all employee types
 * This demonstrates how backend systems model hierarchical data
 */
public class Employee {
    // ==================== FIELDS ====================
    protected int id;                    // Protected: accessible by subclasses
    protected String name;
    protected String department;
    protected double salary;
    protected String email;
    
    // ==================== CONSTRUCTORS ====================
    
    public Employee() {
    }
    
    public Employee(int id, String name, String department, double salary) {
        this.id = id;
        this.name = name;
        this.department = department;
        this.salary = salary;
        this.email = name.toLowerCase().replace(" ", ".") + "@company.com";
    }
    
    // ==================== GETTERS AND SETTERS ====================
    
    public int getId() { return id; }
    public void setId(int id) { this.id = id; }
    
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    
    public String getDepartment() { return department; }
    public void setDepartment(String department) { this.department = department; }
    
    public double getSalary() { return salary; }
    public void setSalary(double salary) { this.salary = salary; }
    
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    
    // ==================== METHODS ====================
    
    /**
     * Calculate annual salary
     */
    public double calculateAnnualSalary() {
        return salary * 12;
    }
    
    /**
     * Display employee information
     */
    public void displayInfo() {
        System.out.println("┌────────────────────────────────────────┐");
        System.out.println("│           EMPLOYEE INFO                │");
        System.out.println("├────────────────────────────────────────┤");
        System.out.println("│ ID:         " + id);
        System.out.println("│ Name:       " + name);
        System.out.println("│ Department: " + department);
        System.out.println("│ Salary:     $" + salary);
        System.out.println("│ Email:      " + email);
        System.out.println("└────────────────────────────────────────┘");
    }
    
    /**
     * Work method - overridden by subclasses
     */
    public void work() {
        System.out.println(name + " is working in " + department);
    }
}

/**
 * Manager - Extends Employee
 * Has additional responsibilities and bonus
 */
class Manager extends Employee {
    private int teamSize;
    private double bonus;
    
    public Manager() {
        super();  // Call parent constructor
    }
    
    public Manager(int id, String name, String department, double salary, int teamSize) {
        super(id, name, department, salary);  // Call parent constructor
        this.teamSize = teamSize;
        this.bonus = 0.0;
    }
    
    // Getter and Setter for new field
    public int getTeamSize() { return teamSize; }
    public void setTeamSize(int teamSize) { this.teamSize = teamSize; }
    
    public double getBonus() { return bonus; }
    public void setBonus(double bonus) { this.bonus = bonus; }
    
    /**
     * Override: Calculate annual salary including bonus
     */
    @Override
    public double calculateAnnualSalary() {
        return (salary * 12) + bonus;
    }
    
    /**
     * Override: Manager-specific work
     */
    @Override
    public void work() {
        System.out.println(name + " is managing a team of " + teamSize + " people");
    }
    
    /**
     * Additional method specific to Manager
     */
    public void conductMeeting() {
        System.out.println(name + " is conducting a team meeting");
    }
    
    @Override
    public void displayInfo() {
        super.displayInfo();  // Call parent displayInfo
        System.out.println("Team Size: " + teamSize);
        System.out.println("Bonus:     $" + bonus);
    }
}

/**
 * Developer - Extends Employee
 * Has programming language specialization
 */
class Developer extends Employee {
    private String programmingLanguage;
    private boolean isRemote;
    
    public Developer() {
        super();
    }
    
    public Developer(int id, String name, String department, double salary, 
                     String programmingLanguage, boolean isRemote) {
        super(id, name, department, salary);
        this.programmingLanguage = programmingLanguage;
        this.isRemote = isRemote;
    }
    
    // Getters and Setters
    public String getProgrammingLanguage() { return programmingLanguage; }
    public void setProgrammingLanguage(String programmingLanguage) { 
        this.programmingLanguage = programmingLanguage; 
    }
    
    public boolean isRemote() { return isRemote; }
    public void setRemote(boolean remote) { isRemote = remote; }
    
    /**
     * Override: Developer-specific work
     */
    @Override
    public void work() {
        System.out.println(name + " is coding in " + programmingLanguage + 
                          (isRemote ? " (Remote)" : " (On-site)"));
    }
    
    @Override
    public void displayInfo() {
        super.displayInfo();
        System.out.println("Language:  " + programmingLanguage);
        System.out.println("Work Mode: " + (isRemote ? "Remote" : "On-site"));
    }
}

/**
 * InheritanceDemo - Main class to demonstrate inheritance
 */
public class InheritanceDemo {
    public static void main(String[] args) {
        System.out.println("=== INHERITANCE DEMO: EMPLOYEE HIERARCHY ===\n");
        
        // Create Employee
        System.out.println("--- Creating Employee ---");
        Employee emp1 = new Employee(101, "John Doe", "HR", 5000);
        emp1.displayInfo();
        emp1.work();
        
        // Create Manager
        System.out.println("\n--- Creating Manager ---");
        Manager mgr1 = new Manager(201, "Jane Smith", "Engineering", 8000, 10);
        mgr1.setBonus(5000);
        mgr1.displayInfo();
        mgr1.work();
        mgr1.conductMeeting();
        
        // Create Developer
        System.out.println("\n--- Creating Developer ---");
        Developer dev1 = new Developer(301, "Mike Johnson", "IT", 7000, "Java", true);
        dev1.displayInfo();
        dev1.work();
        
        // Polymorphism Demo
        System.out.println("\n=== POLYMORPHISM DEMO ===");
        Employee[] employees = new Employee[3];
        employees[0] = new Employee(101, "Emp1", "General", 5000);
        employees[1] = new Manager(201, "Mgr1", "Sales", 7000, 5);
        employees[2] = new Developer(301, "Dev1", "Tech", 6500, "Python", false);
        
        System.out.println("\nCalling work() on each (Runtime Polymorphism):");
        for (Employee emp : employees) {
            emp.work();  // Different behavior based on actual object type
        }
        
        // Calculate total annual salary
        System.out.println("\n--- Annual Salary Calculation ---");
        System.out.println("Employee Annual: $" + emp1.calculateAnnualSalary());
        System.out.println("Manager Annual: $" + mgr1.calculateAnnualSalary());
        System.out.println("Developer Annual: $" + dev1.calculateAnnualSalary());
    }
}
```

---

### Example 2: Vehicle Inheritance Hierarchy

```java
/**
 * Vehicle - Base class for all vehicles
 */
class Vehicle {
    protected String brand;
    protected String model;
    protected int year;
    protected double price;
    
    // Constructor
    public Vehicle(String brand, String model, int year, double price) {
        this.brand = brand;
        this.model = model;
        this.year = year;
        this.price = price;
    }
    
    // Getter methods
    public String getBrand() { return brand; }
    public String getModel() { return model; }
    public int getYear() { return year; }
    public double getPrice() { return price; }
    
    // Common method
    public void displayDetails() {
        System.out.println("Brand: " + brand + ", Model: " + model + 
                          ", Year: " + year + ", Price: $" + price);
    }
    
    // Method to be overridden
    public void start() {
        System.out.println(brand + " " + model + " is starting...");
    }
    
    public void stop() {
        System.out.println(brand + " " + model + " is stopping...");
    }
}

/**
 * Car - Extends Vehicle
 */
class Car extends Vehicle {
    private int numberOfDoors;
    private String fuelType;
    
    public Car(String brand, String model, int year, double price, 
               int numberOfDoors, String fuelType) {
        super(brand, model, year, price);  // Call parent constructor
        this.numberOfDoors = numberOfDoors;
        this.fuelType = fuelType;
    }
    
    // Getter methods
    public int getNumberOfDoors() { return numberOfDoors; }
    public String getFuelType() { return fuelType; }
    
    // Override start method
    @Override
    public void start() {
        System.out.println(brand + " " + model + " car is starting with " + 
                          fuelType + " engine...");
    }
    
    // Car-specific method
    public void honk() {
        System.out.println("Car horn: Beep! Beep!");
    }
    
    @Override
    public void displayDetails() {
        super.displayDetails();
        System.out.println("Doors: " + numberOfDoors + ", Fuel: " + fuelType);
    }
}

/**
 * Motorcycle - Extends Vehicle
 */
class Motorcycle extends Vehicle {
    private int engineCC;
    private boolean hasSidecar;
    
    public Motorcycle(String brand, String model, int year, double price, 
                      int engineCC, boolean hasSidecar) {
        super(brand, model, year, price);
        this.engineCC = engineCC;
        this.hasSidecar = hasSidecar;
    }
    
    // Getter methods
    public int getEngineCC() { return engineCC; }
    public boolean hasSidecar() { return hasSidecar; }
    
    // Override start method
    @Override
    public void start() {
        System.out.println(brand + " " + model + " motorcycle engine is starting...");
    }
    
    // Motorcycle-specific method
    public void wheelie() {
        System.out.println("Doing a wheelie!");
    }
    
    @Override
    public void displayDetails() {
        super.displayDetails();
        System.out.println("Engine: " + engineCC + "cc, Sidecar: " + (hasSidecar ? "Yes" : "No"));
    }
}

/**
 * Truck - Extends Vehicle (for Angular backend demo)
 */
class Truck extends Vehicle {
    private double payloadCapacity;
    private int numberOfWheels;
    
    public Truck(String brand, String model, int year, double price,
                 double payloadCapacity, int numberOfWheels) {
        super(brand, model, year, price);
        this.payloadCapacity = payloadCapacity;
        this.numberOfWheels = numberOfWheels;
    }
    
    // Getter methods
    public double getPayloadCapacity() { return payloadCapacity; }
    public int getNumberOfWheels() { return numberOfWheels; }
    
    // Override start method
    @Override
    public void start() {
        System.out.println(brand + " " + model + " truck is starting its engine...");
    }
    
    // Truck-specific method
    public void loadCargo(double weight) {
        if (weight <= payloadCapacity) {
            System.out.println("Loading " + weight + " tons of cargo...");
        } else {
            System.out.println("Warning: Weight exceeds payload capacity!");
        }
    }
    
    @Override
    public void displayDetails() {
        super.displayDetails();
        System.out.println("Payload: " + payloadCapacity + " tons, Wheels: " + numberOfWheels);
    }
}

/**
 * VehicleDemo - Main demonstration class
 */
public class VehicleDemo {
    public static void main(String[] args) {
        System.out.println("=== VEHICLE INHERITANCE HIERARCHY ===\n");
        
        // Create different vehicles
        Car car = new Car("Toyota", "Camry", 2023, 28000, 4, "Gasoline");
        Motorcycle bike = new Motorcycle("Harley-Davidson", "Sportster", 2022, 12000, 883, false);
        Truck truck = new Ford("F-150", 2023, 35000, 2.5, 4);
        
        // Display details
        System.out.println("--- CAR ---");
        car.displayDetails();
        car.start();
        car.honk();
        
        System.out.println("\n--- MOTORCYCLE ---");
        bike.displayDetails();
        bike.start();
        bike.wheelie();
        
        System.out.println("\n--- TRUCK ---");
        truck.displayDetails();
        truck.start();
        truck.loadCargo(2.0);
        
        // Polymorphism with array
        System.out.println("\n=== POLYMORPHISM DEMO ===");
        Vehicle[] vehicles = {car, bike, truck};
        
        for (Vehicle v : vehicles) {
            v.start();  // Calls overridden method based on actual type
            System.out.println("---");
        }
    }
}

// Note: Ford class for truck example - simple subclass
class Ford extends Truck {
    public Ford(String model, int year, double price, double payload, int wheels) {
        super("Ford", model, year, price, payload, wheels);
    }
}
```

---

### Example 3: Shape Hierarchy (with Method Overloading)

```java
/**
 * Shape - Abstract base class (we'll cover abstraction later)
 * Demonstrates polymorphism with different shapes
 */
class Shape {
    protected String color;
    
    public Shape() {
        this.color = "white";
    }
    
    public Shape(String color) {
        this.color = color;
    }
    
    // Methods to be overridden by subclasses
    public double getArea() {
        return 0.0;
    }
    
    public double getPerimeter() {
        return 0.0;
    }
    
    public void display() {
        System.out.println("Color: " + color);
    }
}

/**
 * Circle - Extends Shape
 */
class Circle extends Shape {
    private double radius;
    
    // Multiple constructors (overloading)
    public Circle() {
        super();
        this.radius = 1.0;
    }
    
    public Circle(double radius) {
        super();
        this.radius = radius;
    }
    
    public Circle(double radius, String color) {
        super(color);
        this.radius = radius;
    }
    
    // Getter and Setter
    public double getRadius() { return radius; }
    public void setRadius(double radius) { this.radius = radius; }
    
    // Override methods
    @Override
    public double getArea() {
        return Math.PI * radius * radius;
    }
    
    @Override
    public double getPerimeter() {
        return 2 * Math.PI * radius;
    }
    
    @Override
    public void display() {
        super.display();
        System.out.println("Shape: Circle");
        System.out.println("Radius: " + radius);
        System.out.println("Area: " + getArea());
        System.out.println("Perimeter: " + getPerimeter());
    }
}

/**
 * Rectangle - Extends Shape
 */
class Rectangle extends Shape {
    private double width;
    private double height;
    
    // Multiple constructors (overloading)
    public Rectangle() {
        super();
        this.width = 1.0;
        this.height = 1.0;
    }
    
    public Rectangle(double width, double height) {
        super();
        this.width = width;
        this.height = height;
    }
    
    public Rectangle(double width, double height, String color) {
        super(color);
        this.width = width;
        this.height = height;
    }
    
    // Getters and Setters
    public double getWidth() { return width; }
    public void setWidth(double width) { this.width = width; }
    public double getHeight() { return height; }
    public void setHeight(double height) { this.height = height; }
    
    // Override methods
    @Override
    public double getArea() {
        return width * height;
    }
    
    @Override
    public double getPerimeter() {
        return 2 * (width + height);
    }
    
    @Override
    public void display() {
        super.display();
        System.out.println("Shape: Rectangle");
        System.out.println("Width: " + width + ", Height: " + height);
        System.out.println("Area: " + getArea());
        System.out.println("Perimeter: " + getPerimeter());
    }
}

/**
 * Triangle - Extends Shape
 */
class Triangle extends Shape {
    private double side1;
    private double side2;
    private double side3;
    
    public Triangle() {
        super();
        this.side1 = 1.0;
        this.side2 = 1.0;
        this.side3 = 1.0;
    }
    
    public Triangle(double side1, double side2, double side3) {
        super();
        this.side1 = side1;
        this.side2 = side2;
        this.side3 = side3;
    }
    
    public Triangle(double side1, double side2, double side3, String color) {
        super(color);
        this.side1 = side1;
        this.side2 = side2;
        this.side3 = side3;
    }
    
    // Override methods
    @Override
    public double getArea() {
        // Heron's formula
        double s = (side1 + side2 + side3) / 2;
        return Math.sqrt(s * (s - side1) * (s - side2) * (s - side3));
    }
    
    @Override
    public double getPerimeter() {
        return side1 + side2 + side3;
    }
    
    @Override
    public void display() {
        super.display();
        System.out.println("Shape: Triangle");
        System.out.println("Sides: " + side1 + ", " + side2 + ", " + side3);
        System.out.println("Area: " + getArea());
        System.out.println("Perimeter: " + getPerimeter());
    }
}

/**
 * ShapeDemo - Main demonstration class
 */
public class ShapeDemo {
    public static void main(String[] args) {
        System.out.println("=== SHAPE HIERARCHY DEMO ===\n");
        
        // Create different shapes
        Circle circle = new Circle(5.0, "Red");
        Rectangle rectangle = new Rectangle(4.0, 6.0, "Blue");
        Triangle triangle = new Triangle(3.0, 4.0, 5.0, "Green");
        
        // Display each shape
        System.out.println("--- CIRCLE ---");
        circle.display();
        
        System.out.println("\n--- RECTANGLE ---");
        rectangle.display();
        
        System.out.println("\n--- TRIANGLE ---");
        triangle.display();
        
        // Polymorphism: Treat all shapes uniformly
        System.out.println("\n=== POLYMORPHISM: CALCULATE AREAS ===");
        Shape[] shapes = {circle, rectangle, triangle};
        
        double totalArea = 0;
        for (Shape shape : shapes) {
            System.out.println("Area of " + shape.getClass().getSimpleName() + 
                              ": " + shape.getArea());
            totalArea += shape.getArea();
        }
        System.out.println("Total Area: " + totalArea);
    }
}
```

---

## 8. Exercises

### Exercise 1: Banking System Inheritance

**Objective:** Create a banking system with different account types.

**Requirements:**
1. Create a `BankAccount` class with fields: accountNumber, holderName, balance
2. Create `SavingsAccount` that extends `BankAccount` with field: interestRate
3. Create `CheckingAccount` that extends `BankAccount` with field: overdraftLimit
4. Each account type should have its own `calculateInterest()` or `calculateFee()` method
5. Demonstrate polymorphism by creating an array of different account types

**Expected Output:**
```
Account: 1001, Holder: John, Balance: $5000.0
Savings Interest: $250.0

Account: 1002, Holder: Jane, Balance: $3000.0
Checking Fee: $15.0
```

---

### Exercise 2: University Person Hierarchy

**Objective:** Model a university system with different person types.

**Requirements:**
1. Create `Person` class with: name, age, email
2. Create `Student` that extends Person with: studentId, major, gpa
3. Create `Teacher` that extends Person with: teacherId, department, salary
4. Create `Staff` that extends Person with: staffId, position, department
5. Each subclass should override `displayInfo()` to show specific details

---

### Exercise 3: E-commerce Product Hierarchy

**Objective:** Model products for an e-commerce backend.

**Context:** This would be sent to an Angular frontend as JSON.

**Requirements:**
1. Create `Product` base class with: id, name, price, category
2. Create `Electronics` with: brand, warrantyMonths
3. Create `Clothing` with: size, color, material
4. Create `Book` with: author, isbn, pages
5. Each should have appropriate getters/setters and toString() method

---

## 9. Solutions

### Solution 1: Banking System

```java
/**
 * BankAccount - Base class for bank accounts
 */
class BankAccount {
    protected String accountNumber;
    protected String holderName;
    protected double balance;
    
    public BankAccount() {
    }
    
    public BankAccount(String accountNumber, String holderName, double balance) {
        this.accountNumber = accountNumber;
        this.holderName = holderName;
        this.balance = balance;
    }
    
    // Getters and Setters
    public String getAccountNumber() { return accountNumber; }
    public void setAccountNumber(String accountNumber) { this.accountNumber = accountNumber; }
    
    public String getHolderName() { return holderName; }
    public void setHolderName(String holderName) { this.holderName = holderName; }
    
    public double getBalance() { return balance; }
    public void setBalance(double balance) { this.balance = balance; }
    
    public void displayAccount() {
        System.out.println("Account: " + accountNumber + ", Holder: " + 
                          holderName + ", Balance: $" + balance);
    }
    
    // Method to be overridden
    public double calculateInterest() {
        return 0.0;
    }
}

/**
 * SavingsAccount - Extends BankAccount with interest
 */
class SavingsAccount extends BankAccount {
    private double interestRate;  // Annual interest rate (e.g., 0.05 = 5%)
    
    public SavingsAccount() {
        super();
    }
    
    public SavingsAccount(String accountNumber, String holderName, 
                         double balance, double interestRate) {
        super(accountNumber, holderName, balance);
        this.interestRate = interestRate;
    }
    
    public double getInterestRate() { return interestRate; }
    public void setInterestRate(double interestRate) { this.interestRate = interestRate; }
    
    @Override
    public double calculateInterest() {
        return balance * interestRate;
    }
    
    @Override
    public void displayAccount() {
        super.displayAccount();
        System.out.println("Savings Interest: $" + calculateInterest());
    }
}

/**
 * CheckingAccount - Extends BankAccount with overdraft
 */
class CheckingAccount extends BankAccount {
    private double overdraftLimit;
    private double monthlyFee;
    
    public CheckingAccount() {
        super();
        this.monthlyFee = 15.0;
    }
    
    public CheckingAccount(String accountNumber, String holderName, 
                          double balance, double overdraftLimit) {
        super(accountNumber, holderName, balance);
        this.overdraftLimit = overdraftLimit;
        this.monthlyFee = 15.0;
    }
    
    public double getOverdraftLimit() { return overdraftLimit; }
    public void setOverdraftLimit(double overdraftLimit) { this.overdraftLimit = overdraftLimit; }
    
    public double getMonthlyFee() { return monthlyFee; }
    
    public double calculateFee() {
        return monthlyFee;
    }
    
    @Override
    public void displayAccount() {
        super.displayAccount();
        System.out.println("Checking Fee: $" + calculateFee());
    }
}

/**
 * BankingDemo - Main demonstration
 */
public class BankingDemo {
    public static void main(String[] args) {
        System.out.println("=== BANKING SYSTEM DEMO ===\n");
        
        // Create accounts
        SavingsAccount savings = new SavingsAccount("1001", "John", 5000.0, 0.05);
        CheckingAccount checking = new CheckingAccount("1002", "Jane", 3000.0, 500.0);
        
        // Display account info
        savings.displayAccount();
        System.out.println();
        checking.displayAccount();
        
        // Polymorphism with array
        System.out.println("\n=== POLYMORPHISM DEMO ===");
        BankAccount[] accounts = {savings, checking};
        
        for (BankAccount account : accounts) {
            System.out.println("Account: " + account.getAccountNumber() + 
                              ", Balance: $" + account.getBalance());
        }
    }
}
```

---

### Solution 2: University Person Hierarchy

```java
/**
 * Person - Base class
 */
class Person {
    protected String name;
    protected int age;
    protected String email;
    
    public Person() {
    }
    
    public Person(String name, int age, String email) {
        this.name = name;
        this.age = age;
        this.email = email;
    }
    
    // Getters and Setters
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    
    public int getAge() { return age; }
    public void setAge(int age) { this.age = age; }
    
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    
    public void displayInfo() {
        System.out.println("Name: " + name);
        System.out.println("Age: " + age);
        System.out.println("Email: " + email);
    }
}

/**
 * Student - Extends Person
 */
class Student extends Person {
    private String studentId;
    private String major;
    private double gpa;
    
    public Student() {
        super();
    }
    
    public Student(String name, int age, String email, 
                  String studentId, String major, double gpa) {
        super(name, age, email);
        this.studentId = studentId;
        this.major = major;
        this.gpa = gpa;
    }
    
    // Getters and Setters
    public String getStudentId() { return studentId; }
    public void setStudentId(String studentId) { this.studentId = studentId; }
    
    public String getMajor() { return major; }
    public void setMajor(String major) { this.major = major; }
    
    public double getGpa() { return gpa; }
    public void setGpa(double gpa) { this.gpa = gpa; }
    
    @Override
    public void displayInfo() {
        System.out.println("=== STUDENT INFO ===");
        super.displayInfo();
        System.out.println("Student ID: " + studentId);
        System.out.println("Major: " + major);
        System.out.println("GPA: " + gpa);
    }
}

/**
 * Teacher - Extends Person
 */
class Teacher extends Person {
    private String teacherId;
    private String department;
    private double salary;
    
    public Teacher() {
        super();
    }
    
    public Teacher(String name, int age, String email,
                  String teacherId, String department, double salary) {
        super(name, age, email);
        this.teacherId = teacherId;
        this.department = department;
        this.salary = salary;
    }
    
    // Getters and Setters
    public String getTeacherId() { return teacherId; }
    public void setTeacherId(String teacherId) { this.teacherId = teacherId; }
    
    public String getDepartment() { return department; }
    public void setDepartment(String department) { this.department = department; }
    
    public double getSalary() { return salary; }
    public void setSalary(double salary) { this.salary = salary; }
    
    @Override
    public void displayInfo() {
        System.out.println("=== TEACHER INFO ===");
        super.displayInfo();
        System.out.println("Teacher ID: " + teacherId);
        System.out.println("Department: " + department);
        System.out.println("Salary: $" + salary);
    }
}

/**
 * Staff - Extends Person
 */
class Staff extends Person {
    private String staffId;
    private String position;
    private String department;
    
    public Staff() {
        super();
    }
    
    public Staff(String name, int age, String email,
                String staffId, String position, String department) {
        super(name, age, email);
        this.staffId = staffId;
        this.position = position;
        this.department = department;
    }
    
    // Getters and Setters
    public String getStaffId() { return staffId; }
    public void setStaffId(String staffId) { this.staffId = staffId; }
    
    public String getPosition() { return position; }
    public void setPosition(String position) { this.position = position; }
    
    public String getDepartment() { return department; }
    public void setDepartment(String department) { this.department = department; }
    
    @Override
    public void displayInfo() {
        System.out.println("=== STAFF INFO ===");
        super.displayInfo();
        System.out.println("Staff ID: " + staffId);
        System.out.println("Position: " + position);
        System.out.println("Department: " + department);
    }
}

/**
 * UniversityDemo - Main demonstration
 */
public class UniversityDemo {
    public static void main(String[] args) {
        System.out.println("=== UNIVERSITY SYSTEM DEMO ===\n");
        
        Student student = new Student("Alice", 20, "alice@uni.edu", 
                                      "S001", "Computer Science", 3.8);
        Teacher teacher = new Teacher("Dr. Smith", 45, "smith@uni.edu",
                                     "T001", "Computer Science", 80000);
        Staff staff = new Staff("Bob", 35, "bob@uni.edu",
                               "ST001", "Administrative", "Registrar");
        
        student.displayInfo();
        System.out.println();
        teacher.displayInfo();
        System.out.println();
        staff.displayInfo();
    }
}
```

---

### Solution 3: E-commerce Product Hierarchy

```java
/**
 * Product - Base class for all products
 * This maps directly to what Angular would receive via REST API
 */
class Product {
    protected int id;
    protected String name;
    protected double price;
    protected String category;
    
    public Product() {
    }
    
    public Product(int id, String name, double price, String category) {
        this.id = id;
        this.name = name;
        this.price = price;
        this.category = category;
    }
    
    // Getters and Setters
    public int getId() { return id; }
    public void setId(int id) { this.id = id; }
    
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    
    public double getPrice() { return price; }
    public void setPrice(double price) { this.price = price; }
    
    public String getCategory() { return category; }
    public void setCategory(String category) { this.category = category; }
    
    // Simulates JSON output for Angular
    @Override
    public String toString() {
        return "{" +
                "\"id\":" + id +
                ", \"name\":\"" + name + "\"" +
                ", \"price\":" + price +
                ", \"category\":\"" + category + "\"" +
                "}";
    }
    
    public void displayInfo() {
        System.out.println("ID: " + id + ", Name: " + name + 
                          ", Price: $" + price + ", Category: " + category);
    }
}

/**
 * Electronics - Extends Product
 */
class Electronics extends Product {
    private String brand;
    private int warrantyMonths;
    
    public Electronics() {
        super();
    }
    
    public Electronics(int id, String name, double price, String category,
                     String brand, int warrantyMonths) {
        super(id, name, price, category);
        this.brand = brand;
        this.warrantyMonths = warrantyMonths;
    }
    
    public String getBrand() { return brand; }
    public void setBrand(String brand) { this.brand = brand; }
    
    public int getWarrantyMonths() { return warrantyMonths; }
    public void setWarrantyMonths(int warrantyMonths) { this.warrantyMonths = warrantyMonths; }
    
    @Override
    public String toString() {
        return "{" +
                super.toString().substring(0, super.toString().length() - 1) +
                ", \"brand\":\"" + brand + "\"" +
                ", \"warrantyMonths\":" + warrantyMonths +
                "}";
    }
    
    @Override
    public void displayInfo() {
        super.displayInfo();
        System.out.println("Brand: " + brand + ", Warranty: " + warrantyMonths + " months");
    }
}

/**
 * Clothing - Extends Product
 */
class Clothing extends Product {
    private String size;
    private String color;
    private String material;
    
    public Clothing() {
        super();
    }
    
    public Clothing(int id, String name, double price, String category,
                   String size, String color, String material) {
        super(id, name, price, category);
        this.size = size;
        this.color = color;
        this.material = material;
    }
    
    public String getSize() { return size; }
    public void setSize(String size) { this.size = size; }
    
    public String getColor() { return color; }
    public void setColor(String color) { this.color = color; }
    
    public String getMaterial() { return material; }
    public void setMaterial(String material) { this.material = material; }
    
    @Override
    public String toString() {
        return "{" +
                super.toString().substring(0, super.toString().length() - 1) +
                ", \"size\":\"" + size + "\"" +
                ", \"color\":\"" + color + "\"" +
                ", \"material\":\"" + material + "\"" +
                "}";
    }
    
    @Override
    public void displayInfo() {
        super.displayInfo();
        System.out.println("Size: " + size + ", Color: " + color + ", Material: " + material);
    }
}

/**
 * Book - Extends Product
 */
class Book extends Product {
    private String author;
    private String isbn;
    private int pages;
    
    public Book() {
        super();
    }
    
    public Book(int id, String name, double price, String category,
               String author, String isbn, int pages) {
        super(id, name, price, category);
        this.author = author;
        this.isbn = isbn;
        this.pages = pages;
    }
    
    public String getAuthor() { return author; }
    public void setAuthor(String author) { this.author = author; }
    
    public String getIsbn() { return isbn; }
    public void setIsbn(String isbn) { this.isbn = isbn; }
    
    public int getPages() { return pages; }
    public void setPages(int pages) { this.pages = pages; }
    
    @Override
    public String toString() {
        return "{" +
                super.toString().substring(0, super.toString().length() - 1) +
                ", \"author\":\"" + author + "\"" +
                ", \"isbn\":\"" + isbn + "\"" +
                ", \"pages\":" + pages +
                "}";
    }
    
    @Override
    public void displayInfo() {
        super.displayInfo();
        System.out.println("Author: " + author + ", ISBN: " + isbn + ", Pages: " + pages);
    }
}

/**
 * EcommerceDemo - Main demonstration
 */
public class EcommerceDemo {
    public static void main(String[] args) {
        System.out.println("=== E-COMMERCE PRODUCT HIERARCHY ===\n");
        
        // Create products
        Electronics laptop = new Electronics(1, "MacBook Pro", 2499.99, 
                                              "Electronics", "Apple", 24);
        Clothing tshirt = new Clothing(2, "T-Shirt", 29.99, "Clothing",
                                       "M", "Blue", "Cotton");
        Book book = new Book(3, "Java Programming", 49.99, "Books",
                            "John Doe", "978-0134685991", 1200);
        
        // Display info
        System.out.println("--- ELECTRONICS ---");
        laptop.displayInfo();
        System.out.println("JSON: " + laptop);
        
        System.out.println("\n--- CLOTHING ---");
        tshirt.displayInfo();
        System.out.println("JSON: " + tshirt);
        
        System.out.println("\n--- BOOK ---");
        book.displayInfo();
        System.out.println("JSON: " + book);
        
        // Polymorphism: Process all products uniformly
        System.out.println("\n=== POLYMORPHISM: ALL PRODUCTS ===");
        Product[] products = {laptop, tshirt, book};
        
        double totalValue = 0;
        for (Product p : products) {
            System.out.println(p.getName() + ": $" + p.getPrice());
            totalValue += p.getPrice();
        }
        System.out.println("Total Inventory Value: $" + totalValue);
    }
}
```

---

## Summary

### Key Takeaways

1. **Inheritance promotes reusability** - Child classes inherit parent class members
2. **`extends` keyword** - Used to create inheritance relationship
3. **`super` keyword** - Used to access parent class members
4. **Method Overriding** - Child class provides specific implementation
5. **Polymorphism** - Objects of different types can be treated uniformly
6. **Method Overloading** - Same method name, different parameters

### Angular Backend Integration Tips

- Java classes map to JSON via serialization (Jackson library)
- Inheritance hierarchies become nested JSON objects
- Use `@JsonIgnoreProperties` to control JSON output
- REST APIs often return polymorphic responses

### Next Steps

Continue learning about:
- **Abstraction** - Abstract classes and interfaces
- **Encapsulation** - Data hiding best practices
- **Generics** - Type-safe collections

---

*Happy Coding! 🚀*
