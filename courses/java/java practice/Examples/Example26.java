/*OOPS Concept
        1. Class and Object
        2. Inheritance
        3. Polymorphism
        4. Abstraction
        5. Encapsulation
          */


        
// Example26: Class and Object - Beginner Tutorial
// This explains what are classes and objects in Java

/*
 * WHAT IS A CLASS?
 * -----------------
 * A class is a blueprint or template for creating objects.
 * It defines what data (attributes) and behavior (methods) an object will have.
 * 
 * Think of it like:
 * - Class = Blueprint/Template (like a cookie cutter)
 * - Object = The actual thing made from the blueprint (like a cookie)
 * 
 * A class contains:
 * - Fields (variables) - store data
 * - Methods (functions) - define behavior
 * - Constructors - initialize objects
 */

// ===== STEP 1: CREATE A CLASS (The Blueprint) =====

// This is a class definition - a blueprint for creating Student objects
class Student {
    // These are FIELDS (attributes) - store data about each student
    String name;      // Student's name
    int age;          // Student's age
    String grade;     // Student's grade/class
    double gpa;       // Student's GPA
    
    // This is a CONSTRUCTOR - special method to initialize the object
    // Constructor has same name as class and no return type
    
    // Default constructor (no parameters)
    public Student() {
        name = "Unknown";
        age = 0;
        grade = "N/A";
        gpa = 0.0;
    }
    
    // Parameterized constructor (with parameters)
    public Student(String n, int a, String g, double gp) {
        name = n;
        age = a;
        grade = g;
        gpa = gp;
    }
    
    // Another constructor with just name and age
    public Student(String n, int a) {
        name = n;
        age = a;
        grade = "N/A";
        gpa = 0.0;
    }
    
    // These are METHODS - define behavior/actions
    
    // Method to display student information
    public void displayInfo() {
        System.out.println("Name: " + name);
        System.out.println("Age: " + age);
        System.out.println("Grade: " + grade);
        System.out.println("GPA: " + gpa);
    }
    
    // Method to update GPA
    public void updateGPA(double newGPA) {
        if (newGPA >= 0.0 && newGPA <= 4.0) {
            gpa = newGPA;
            System.out.println(name + "'s GPA updated to " + gpa);
        } else {
            System.out.println("Invalid GPA! Must be between 0.0 and 4.0");
        }
    }
    
    // Method to celebrate birthday
    public void haveBirthday() {
        age++;
        System.out.println("Happy Birthday " + name + "! Now " + age + " years old!");
    }
    
    // Method to check if student is on honor roll
    public boolean isHonorRoll() {
        return gpa >= 3.5;
    }
}

// Another example class: BankAccount
class BankAccount {
    // Fields
    String accountHolder;
    String accountNumber;
    double balance;
    
    // Constructor
    public BankAccount(String holder, String number, double initialBalance) {
        accountHolder = holder;
        accountNumber = number;
        balance = initialBalance;
    }
    
    // Method to deposit money
    public void deposit(double amount) {
        if (amount > 0) {
            balance += amount;
            System.out.println("Deposited $" + amount + ". New balance: $" + balance);
        } else {
            System.out.println("Invalid deposit amount!");
        }
    }
    
    // Method to withdraw money
    public void withdraw(double amount) {
        if (amount > 0 && amount <= balance) {
            balance -= amount;
            System.out.println("Withdrew $" + amount + ". New balance: $" + balance);
        } else {
            System.out.println("Invalid withdrawal or insufficient funds!");
        }
    }
    
    // Method to display account info
    public void displayAccount() {
        System.out.println("Account Holder: " + accountHolder);
        System.out.println("Account Number: " + accountNumber);
        System.out.println("Balance: $" + balance);
    }
}

// ===== MAIN CLASS =====
public class Example26 {
    public static void main(String[] args) {
        
        // ===== STEP 2: CREATE OBJECTS (Instances of the class) =====
        
        System.out.println("=== Creating Objects from Class ===\n");
        
        // Create first Student object using default constructor
        System.out.println("--- Student 1 (Default Constructor) ---");
        Student student1 = new Student();
        student1.displayInfo();
        
        // Create second Student object using parameterized constructor
        System.out.println("\n--- Student 2 (Parameterized Constructor) ---");
        Student student2 = new Student("Alice", 20, "A", 3.8);
        student2.displayInfo();
        
        // Create third Student object
        System.out.println("\n--- Student 3 ---");
        Student student3 = new Student("Bob", 19, "B", 3.2);
        student3.displayInfo();
        
        // Create fourth Student
        System.out.println("\n--- Student 4 ---");
        Student student4 = new Student("Charlie", 21);
        student4.displayInfo();
        
        // ===== USING OBJECT METHODS =====
        System.out.println("\n=== Using Object Methods ===\n");
        
        // Update GPA
        System.out.println("--- Updating GPA ---");
        student3.updateGPA(3.5);
        student3.displayInfo();
        
        // Have birthday
        System.out.println("\n--- Birthday ---");
        student2.haveBirthday();
        
        // Check honor roll
        System.out.println("\n--- Honor Roll Check ---");
        System.out.println(student2.name + " on honor roll: " + student2.isHonorRoll());
        System.out.println(student3.name + " on honor roll: " + student3.isHonorRoll());
        
        // ===== CREATE BANK ACCOUNT OBJECTS =====
        System.out.println("\n=== Bank Account Objects ===\n");
        
        BankAccount account1 = new BankAccount("John Smith", "123456789", 1000.0);
        account1.displayAccount();
        
        System.out.println();
        account1.deposit(500.0);
        
        System.out.println();
        account1.withdraw(200.0);
        
        System.out.println();
        account1.withdraw(2000.0);  // Should fail - insufficient funds
        
        // ===== MULTIPLE OBJECTS OF SAME CLASS =====
        System.out.println("\n=== Array of Objects ===\n");
        
        // Create array of Student objects
        Student[] students = new Student[3];
        
        // Initialize each object
        students[0] = new Student("Emma", 20, "A", 3.9);
        students[1] = new Student("Liam", 19, "B", 3.2);
        students[2] = new Student("Olivia", 21, "A", 4.0);
        
        // Display all students
        System.out.println("All Students:");
        for (int i = 0; i < students.length; i++) {
            System.out.println("\nStudent " + (i + 1) + ":");
            students[i].displayInfo();
        }
        
        // ===== COMPARING OBJECTS =====
        System.out.println("\n=== Object References ===\n");
        
        Student s1 = new Student("Test", 20, "A", 3.5);
        Student s2 = new Student("Test", 20, "A", 3.5);
        Student s3 = s1;  // s3 refers to same object as s1
        
        System.out.println("s1 == s2 (different objects): " + (s1 == s2));
        System.out.println("s1 == s3 (same reference): " + (s1 == s3));
        
        // Compare using equals() - need to override in class
        System.out.println("s1.equals(s2): " + s1.equals(s2));
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. CLASS vs OBJECT:
 *    - Class = Blueprint/Template
 *    - Object = Instance created from blueprint
 *    - One class can create many objects
 * 
 * 2. ANATOMY OF A CLASS:
 *    - Fields (attributes): Store data
 *    - Methods (behaviors): Perform actions
 *    - Constructors: Initialize new objects
 * 
 * 3. CONSTRUCTOR:
 *    - Same name as class
 *    - No return type (not even void)
 *    - Called when creating object with 'new'
 *    - Can have multiple constructors (overloading)
 * 
 * 4. CREATING OBJECTS:
 *    ClassName objectName = new ClassName();
 *    ClassName objectName = new ClassName(params);
 * 
 * 5. ACCESSING MEMBERS:
 *    - object.field (access field)
 *    - object.method() (call method)
 * 
 * 6. DEFAULT VALUES:
 *    - int, short, byte, long: 0
 *    - float, double: 0.0
 *    - boolean: false
 *    - char: '\0' (null character)
 *    - Objects: null
 * 
 * 7. THIS KEYWORD:
 *    - Refers to current object
 *    - Used to distinguish instance variables from parameters
 * 
 * 8. OBJECTS IN ARRAYS:
 *    - Can store objects just like primitives
 *    - Need to initialize each element
 */
