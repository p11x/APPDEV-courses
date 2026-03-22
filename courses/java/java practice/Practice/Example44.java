/*
 * SUB TOPIC: Java Keywords and Access Modifiers
 * 
 * DEFINITION:
 * Java keywords are reserved words that have special meaning in the language. Access modifiers 
 * (public, private, protected, default) control the visibility of classes, methods, and variables. 
 * Other important keywords include static, final, abstract, this, super, etc.
 * 
 * FUNCTIONALITIES:
 * 1. Access modifiers - public, private, protected, default
 * 2. Static keyword - class-level variables and methods
 * 3. Final keyword - constants and preventing inheritance
 * 4. This keyword - reference to current object
 * 5. Super keyword - reference to parent class
 * 6. Abstract keyword - incomplete classes and methods
 */

public class Example44 {
    
    // Access modifier examples - Public class
    public static class Person {
        // Private - only accessible within class
        private String name;
        private int age;
        
        // Protected - accessible within package and subclasses
        protected String address;
        
        // Package-private (default) - accessible within package
        String country;
        
        // Public - accessible everywhere
        public String email;
        
        // Constructor
        public Person(String name, int age) {
            this.name = name; // this keyword
            this.age = age;
        }
        
        // Public getter
        public String getName() {
            return this.name;
        }
        
        // Public setter
        public void setName(String name) {
            this.name = name;
        }
        
        // Final method - cannot be overridden
        public final void display() {
            System.out.println("Name: " + name + ", Age: " + age);
        }
    }
    
    // Static example
    static class Counter {
        // Static variable - shared across all instances
        private static int count = 0;
        
        // Instance variable
        private int instanceCount = 0;
        
        public Counter() {
            count++; // Increment static counter
            instanceCount++; // Increment instance counter
        }
        
        // Static method - can be called without object
        public static int getCount() {
            return count;
        }
        
        public int getInstanceCount() {
            return instanceCount;
        }
    }
    
    // Final example
    static class Constants {
        // Final variable - constant (cannot change)
        public static final double PI = 3.14159;
        public static final String APP_NAME = "MyApp";
        
        // Final class - cannot be extended
    }
    
    // Final class example
    public static final class ImmutableClass {
        private final int value;
        
        public ImmutableClass(int value) {
            this.value = value; // Can only be set in constructor
        }
        
        // No setter - value cannot be changed after object creation
        public int getValue() {
            return value;
        }
    }
    
    // Abstract example
    abstract static class Animal {
        // Abstract method - no implementation
        public abstract void makeSound();
        
        // Regular method
        public void sleep() {
            System.out.println("Animal is sleeping");
        }
    }
    
    static class Dog extends Animal {
        @Override
        public void makeSound() {
            System.out.println("Dog barks: Woof!");
        }
    }
    
    public static void main(String[] args) {
        
        // Topic Explanation: Access Modifiers
        
        // Public - accessible everywhere
        System.out.println("=== Access Modifiers ===");
        Person person = new Person("John", 25);
        person.setName("Alice");
        System.out.println("Name: " + person.getName());
        
        // Private - only within class
        // person.name - NOT accessible (private)
        
        // Default (package-private)
        person.country = "USA"; // Accessible within same package
        
        // Protected - accessible within package and subclasses
        
        // Topic Explanation: Static keyword
        System.out.println("\n=== Static Keyword ===");
        Counter c1 = new Counter();
        Counter c2 = new Counter();
        Counter c3 = new Counter();
        
        System.out.println("Static count: " + Counter.getCount()); // 3
        System.out.println("c1 instance count: " + c1.getInstanceCount()); // 1
        System.out.println("c2 instance count: " + c2.getInstanceCount()); // 1
        
        // Topic Explanation: Final keyword
        System.out.println("\n=== Final Keyword ===");
        System.out.println("PI: " + Constants.PI);
        System.out.println("App Name: " + Constants.APP_NAME);
        
        // Final object reference - cannot reassign
        final StringBuilder sb = new StringBuilder("Hello");
        sb.append(" World"); // Can modify contents
        System.out.println("Modified: " + sb.toString());
        
        // Topic Explanation: This keyword
        System.out.println("\n=== This Keyword ===");
        // Already shown in constructor - refers to current object
        
        // Topic Explanation: Super keyword
        System.out.println("\n=== Super Keyword ===");
        Dog dog = new Dog();
        dog.makeSound(); // Calls overridden method
        dog.sleep(); // Calls parent method
        
        // Topic Explanation: Abstract
        System.out.println("\n=== Abstract Keyword ===");
        Animal animal = new Dog();
        animal.makeSound();
        animal.sleep();
        
        // Real-time Example 1: Bank Account
        System.out.println("\n=== Example 1: Bank Account ===");
        
        class BankAccount {
            private String accountNumber;
            private double balance;
            
            public BankAccount(String accountNumber, double balance) {
                this.accountNumber = accountNumber;
                this.balance = balance;
            }
            
            public void deposit(double amount) {
                if (amount > 0) {
                    this.balance += amount;
                    System.out.println("Deposited: $" + amount);
                }
            }
            
            public void withdraw(double amount) {
                if (amount <= this.balance) {
                    this.balance -= amount;
                    System.out.println("Withdrawn: $" + amount);
                } else {
                    System.out.println("Insufficient funds");
                }
            }
            
            public double getBalance() {
                return this.balance;
            }
        }
        
        BankAccount account = new BankAccount("ACC123", 1000);
        account.deposit(500);
        account.withdraw(200);
        System.out.println("Balance: $" + account.getBalance());
        
        // Real-time Example 2: Static Counter for tracking
        System.out.println("\n=== Example 2: User Tracker ===");
        
        class UserTracker {
            private static int totalUsers = 0;
            private String username;
            
            public UserTracker(String username) {
                this.username = username;
                totalUsers++;
            }
            
            public static int getTotalUsers() {
                return totalUsers;
            }
        }
        
        new UserTracker("John");
        new UserTracker("Jane");
        new UserTracker("Mike");
        
        System.out.println("Total users: " + UserTracker.getTotalUsers());
        
        // Real-time Example 3: Configuration class
        System.out.println("\n=== Example 3: Config Constants ===");
        
        class Config {
            public static final int MAX_LOGIN_ATTEMPTS = 3;
            public static final long SESSION_TIMEOUT = 3600000;
            public static final String DATABASE_URL = "jdbc:mysql://localhost:3306/mydb";
        }
        
        System.out.println("Max login attempts: " + Config.MAX_LOGIN_ATTEMPTS);
        System.out.println("Session timeout: " + Config.SESSION_TIMEOUT + "ms");
        
        // Real-time Example 4: Immutable Order
        System.out.println("\n=== Example 4: Immutable Order ===");
        
        class Order {
            private final String orderId;
            private final double amount;
            private final String status;
            
            public Order(String orderId, double amount) {
                this.orderId = orderId;
                this.amount = amount;
                this.status = "PENDING";
            }
            
            public String getOrderId() { return orderId; }
            public double getAmount() { return amount; }
            public String getStatus() { return status; }
        }
        
        Order order = new Order("ORD001", 99.99);
        System.out.println("Order ID: " + order.getOrderId());
        System.out.println("Amount: $" + order.getAmount());
        
        // Real-time Example 5: Shape hierarchy
        System.out.println("\n=== Example 5: Shape Hierarchy ===");
        
        class RectangleDemo {
            private double length, width;
            
            public RectangleDemo(double l, double w) {
                this.length = l;
                this.width = w;
            }
            
            public double area() {
                return length * width;
            }
            
            public double perimeter() {
                return 2 * (length + width);
            }
        }
        
        RectangleDemo shape = new RectangleDemo(10, 5);
        System.out.println("Area: " + shape.area());
        System.out.println("Perimeter: " + shape.perimeter());
        
        // Real-time Example 6: Employee inheritance
        System.out.println("\n=== Example 6: Employee Classes ===");
        
        class Employee {
            protected String name;
            protected double salary;
            
            public Employee(String name, double salary) {
                this.name = name;
                this.salary = salary;
            }
            
            public void display() {
                System.out.println("Employee: " + name + ", Salary: $" + salary);
            }
        }
        
        class Manager extends Employee {
            private String department;
            
            public Manager(String name, double salary, String dept) {
                super(name, salary); // Super keyword
                this.department = dept;
            }
            
            @Override
            public void display() {
                super.display(); // Call parent method
                System.out.println("Department: " + department);
            }
        }
        
        Manager mgr = new Manager("Alice", 80000, "Sales");
        mgr.display();
    }
}
