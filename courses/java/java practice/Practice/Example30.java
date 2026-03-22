/*
 * SUB TOPIC: Encapsulation
 * 
 * DEFINITION:
 * Encapsulation is the bundling of data (variables) and methods into a single unit (class), 
 * and restricting access to some components. It achieves data hiding by making variables private 
 * and providing public getter/setter methods.
 * 
 * FUNCTIONALITIES:
 * 1. Private data members
 * 2. Public getters and setters
 * 3. Data validation
 * 4. Data protection
 * 5. Controlled access
 */

class Student {
    // Private variables - data hiding
    private String name;
    private int age;
    private double gpa;
    private String email;
    
    // Constructor
    public Student(String name, int age, double gpa, String email) {
        this.name = name;
        this.email = email;
        
        // Validate age
        if (age > 0 && age < 150) {
            this.age = age;
        } else {
            this.age = 0;
        }
        
        // Validate GPA
        if (gpa >= 0.0 && gpa <= 4.0) {
            this.gpa = gpa;
        } else {
            this.gpa = 0.0;
        }
    }
    
    // Getter methods
    public String getName() {
        return name;
    }
    
    public int getAge() {
        return age;
    }
    
    public double getGpa() {
        return gpa;
    }
    
    // Setter methods with validation
    public void setName(String name) {
        this.name = name;
    }
    
    public void setAge(int age) {
        if (age > 0 && age < 150) {
            this.age = age;
        }
    }
    
    public void setGpa(double gpa) {
        if (gpa >= 0.0 && gpa <= 4.0) {
            this.gpa = gpa;
        }
    }
    
    public void displayInfo() {
        System.out.println("Name: " + name);
        System.out.println("Age: " + age);
        System.out.println("GPA: " + gpa);
        System.out.println("Email: " + email);
    }
}

class BankAccount {
    private String accountNumber;
    private String holderName;
    private double balance;
    
    public BankAccount(String accountNumber, String holderName, double balance) {
        this.accountNumber = accountNumber;
        this.holderName = holderName;
        this.balance = balance;
    }
    
    // Getter
    public double getBalance() {
        return balance;
    }
    
    // Setter with validation
    public void deposit(double amount) {
        if (amount > 0) {
            balance += amount;
            System.out.println("Deposited: $" + amount);
        }
    }
    
    public void withdraw(double amount) {
        if (amount > 0 && amount <= balance) {
            balance -= amount;
            System.out.println("Withdrawn: $" + amount);
        }
    }
    
    public void displayBalance() {
        System.out.println("Account: " + accountNumber);
        System.out.println("Holder: " + holderName);
        System.out.println("Balance: $" + balance);
    }
}

public class Example30 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Encapsulation
        System.out.println("=== Encapsulation ===");
        
        Student student = new Student("Alice", 20, 3.8, "alice@email.com");
        student.displayInfo();
        
        // Real-time Example 1: Using getters
        System.out.println("\n=== Using Getters ===");
        
        System.out.println("Name: " + student.getName());
        System.out.println("GPA: " + student.getGpa());
        
        // Real-time Example 2: Using setters with validation
        System.out.println("\n=== Using Setters ===");
        
        student.setAge(21);
        student.setGPA(3.9);
        student.displayInfo();
        
        // Real-time Example 3: Invalid data
        System.out.println("\n=== Invalid Data ===");
        
        Student student2 = new Student("Bob", 200, 5.0, "bob@email.com");
        student2.displayInfo();
        
        // Real-time Example 4: Bank Account
        System.out.println("\n=== Bank Account ===");
        
        BankAccount account = new BankAccount("123456", "John Doe", 1000.0);
        account.displayBalance();
        account.deposit(500.0);
        account.withdraw(200.0);
        account.displayBalance();
        
        // Real-time Example 5: Array of encapsulated objects
        System.out.println("\n=== Array of Students ===");
        
        Student[] students = new Student[3];
        students[0] = new Student("Emma", 20, 3.9, "emma@email.com");
        students[1] = new Student("Liam", 19, 3.5, "liam@email.com");
        students[2] = new Student("Olivia", 21, 4.0, "olivia@email.com");
        
        for (Student s : students) {
            System.out.println(s.getName() + " - GPA: " + s.getGPA());
        }
        
        // Real-time Example 6: Calculate average GPA
        System.out.println("\n=== Average GPA ===");
        
        double totalGPA = 0;
        for (Student s : students) {
            totalGPA += s.getGPA();
        }
        System.out.println("Average GPA: " + (totalGPA / students.length));
    }
}
