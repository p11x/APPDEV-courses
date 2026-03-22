/*
 * SUB TOPIC: Class and Object
 * 
 * DEFINITION:
 * A class is a blueprint or template for creating objects. It defines what data (attributes) and 
 * behavior (methods) an object will have. An object is an instance of a class - the actual 
 * entity created from the class blueprint.
 * 
 * FUNCTIONALITIES:
 * 1. Creating classes with fields and methods
 * 2. Creating objects from classes
 * 3. Using constructors
 * 4. Accessing object members
 * 5. Creating arrays of objects
 */

// Student class - blueprint for creating Student objects
class Student {
    // Fields - store data
    String name;
    int age;
    String grade;
    double gpa;
    
    // Constructor - initialize object
    public Student(String n, int a, String g, double gp) {
        name = n;
        age = a;
        grade = g;
        gpa = gp;
    }
    
    // Method to display info
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
            System.out.println("Invalid GPA!");
        }
    }
    
    // Method to check honor roll
    public boolean isHonorRoll() {
        return gpa >= 3.5;
    }
}

// BankAccount class
class BankAccount {
    String accountHolder;
    String accountNumber;
    double balance;
    
    public BankAccount(String holder, String number, double initialBalance) {
        accountHolder = holder;
        accountNumber = number;
        balance = initialBalance;
    }
    
    public void deposit(double amount) {
        if (amount > 0) {
            balance += amount;
            System.out.println("Deposited $" + amount + ". New balance: $" + balance);
        }
    }
    
    public void withdraw(double amount) {
        if (amount > 0 && amount <= balance) {
            balance -= amount;
            System.out.println("Withdrew $" + amount + ". New balance: $" + balance);
        }
    }
    
    public void displayAccount() {
        System.out.println("Account Holder: " + accountHolder);
        System.out.println("Account Number: " + accountNumber);
        System.out.println("Balance: $" + balance);
    }
}

public class Example26 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Creating Objects
        System.out.println("=== Class and Object ===");
        
        // Create Student object
        Student student1 = new Student("Alice", 20, "A", 3.8);
        student1.displayInfo();
        
        // Real-time Example 1: Multiple Students
        System.out.println("\n=== Multiple Students ===");
        
        Student student2 = new Student("Bob", 19, "B", 3.2);
        student2.displayInfo();
        
        // Real-time Example 2: Update GPA
        System.out.println("\n=== Update GPA ===");
        
        student2.updateGPA(3.5);
        System.out.println("On Honor Roll: " + student2.isHonorRoll());
        
        // Real-time Example 3: Bank Account
        System.out.println("\n=== Bank Account ===");
        
        BankAccount account = new BankAccount("John Smith", "123456789", 1000.0);
        account.displayAccount();
        account.deposit(500.0);
        account.withdraw(200.0);
        
        // Real-time Example 4: Array of Objects
        System.out.println("\n=== Array of Objects ===");
        
        Student[] students = new Student[3];
        students[0] = new Student("Emma", 20, "A", 3.9);
        students[1] = new Student("Liam", 19, "B", 3.2);
        students[2] = new Student("Olivia", 21, "A", 4.0);
        
        for (Student s : students) {
            s.displayInfo();
            System.out.println("On Honor Roll: " + s.isHonorRoll());
        }
        
        // Real-time Example 5: Multiple Bank Accounts
        System.out.println("\n=== Multiple Accounts ===");
        
        BankAccount[] accounts = new BankAccount[2];
        accounts[0] = new BankAccount("Alice", "ACC001", 5000.0);
        accounts[1] = new BankAccount("Bob", "ACC002", 3000.0);
        
        for (BankAccount acc : accounts) {
            acc.displayAccount();
        }
        
        // Real-time Example 6: Object References
        System.out.println("\n=== Object References ===");
        
        Student s1 = new Student("Test", 20, "A", 3.5);
        Student s2 = s1; // Both refer to same object
        
        System.out.println("s1 == s2: " + (s1 == s2));
        
        s1.name = "Updated";
        System.out.println("s2 name after s1 update: " + s2.name);
    }
}
