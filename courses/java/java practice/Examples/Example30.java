// Example30: Encapsulation - Beginner Tutorial
// This explains encapsulation in Java

/*
 * WHAT IS ENCAPSULATION?
 * --------------------
 * Encapsulation is the bundling of data (variables) and methods (functions) 
 * into a single unit (class), and restricting access to some components.
 * 
 * In simpler terms: "Data Hiding + Methods = Encapsulation"
 * 
 * Benefits:
 * - Data Protection: Prevents unauthorized access
 * - Flexibility: Can change internal implementation without affecting external code
 * - Reusability: Easy to reuse components
 * - Maintainability: Code is easier to maintain
 * 
 * How to achieve:
 * - Make variables private
 * - Provide public getter and setter methods
 * - Validate data in setters
 */

// ===== WITHOUT ENCAPSULATION (BAD PRACTICE) =====

/*
 * This is BAD - variables are public, anyone can modify them directly
 */
class BadStudent {
    public String name;    // Public - anyone can access!
    public int age;        // Public - can be set to any value!
    public double gpa;     // Public - can be negative!
}

// ===== WITH ENCAPSULATION (GOOD PRACTICE) =====

/*
 * This is GOOD - variables are private, access through methods
 */
class Student {
    // Private variables - cannot be accessed directly from outside
    private String name;
    private int age;
    private double gpa;
    private String email;
    
    // DEFAULT CONSTRUCTOR
    public Student() {
        name = "Unknown";
        age = 0;
        gpa = 0.0;
        email = "none@email.com";
    }
    
    // PARAMETERIZED CONSTRUCTOR
    public Student(String name, int age, double gpa, String email) {
        this.name = name;
        this.email = email;
        
        // Validate age in constructor
        if (age >= 0 && age <= 150) {
            this.age = age;
        } else {
            System.out.println("Invalid age! Setting to 0");
            this.age = 0;
        }
        
        // Validate GPA in constructor
        if (gpa >= 0.0 && gpa <= 4.0) {
            this.gpa = gpa;
        } else {
            System.out.println("Invalid GPA! Setting to 0.0");
            this.gpa = 0.0;
        }
    }
    
    // GETTER METHODS - to read private variables
    public String getName() {
        return name;
    }
    
    public int getAge() {
        return age;
    }
    
    public double getGpa() {
        return gpa;
    }
    
    public String getEmail() {
        return email;
    }
    
    // SETTER METHODS - to modify private variables with validation
    public void setName(String name) {
        if (name != null && !name.isEmpty()) {
            this.name = name;
        } else {
            System.out.println("Invalid name!");
        }
    }
    
    public void setAge(int age) {
        if (age >= 0 && age <= 150) {
            this.age = age;
        } else {
            System.out.println("Invalid age! Must be between 0 and 150");
        }
    }
    
    public void setGpa(double gpa) {
        if (gpa >= 0.0 && gpa <= 4.0) {
            this.gpa = gpa;
        } else {
            System.out.println("Invalid GPA! Must be between 0.0 and 4.0");
        }
    }
    
    public void setEmail(String email) {
        if (email != null && email.contains("@")) {
            this.email = email;
        } else {
            System.out.println("Invalid email!");
        }
    }
    
    // Other methods
    public void displayInfo() {
        System.out.println("Student Name: " + name);
        System.out.println("Student Age: " + age);
        System.out.println("Student GPA: " + gpa);
        System.out.println("Student Email: " + email);
    }
    
    // Check honor roll
    public boolean isHonorRoll() {
        return gpa >= 3.5;
    }
}

// ===== ANOTHER EXAMPLE: BANK ACCOUNT =====

class BankAccount {
    // Private data - hidden from outside
    private String accountNumber;
    private String accountHolder;
    private double balance;
    private String pin;
    
    // Constructor
    public BankAccount(String accNum, String holder, String pin, double initialBalance) {
        this.accountNumber = accNum;
        this.accountHolder = holder;
        this.pin = pin;
        
        if (initialBalance >= 0) {
            this.balance = initialBalance;
        } else {
            this.balance = 0;
        }
    }
    
    // GETTERS
    public String getAccountNumber() {
        return accountNumber;
    }
    
    public String getAccountHolder() {
        return accountHolder;
    }
    
    public double getBalance() {
        return balance;
    }
    
    // SETTER with validation
    public void setAccountHolder(String holder) {
        if (holder != null && !holder.isEmpty()) {
            this.accountHolder = holder;
        }
    }
    
    // Deposit method
    public void deposit(double amount) {
        if (amount > 0) {
            balance += amount;
            System.out.println("Deposited $" + amount);
            System.out.println("New balance: $" + balance);
        } else {
            System.out.println("Invalid deposit amount!");
        }
    }
    
    // Withdraw method with validation
    public void withdraw(double amount, String enteredPin) {
        // Verify PIN first
        if (!pin.equals(enteredPin)) {
            System.out.println("Incorrect PIN!");
            return;
        }
        
        if (amount > 0 && amount <= balance) {
            balance -= amount;
            System.out.println("Withdrew $" + amount);
            System.out.println("New balance: $" + balance);
        } else {
            System.out.println("Invalid withdrawal amount or insufficient funds!");
        }
    }
    
    // Transfer money
    public void transfer(BankAccount recipient, double amount, String enteredPin) {
        if (!pin.equals(enteredPin)) {
            System.out.println("Incorrect PIN!");
            return;
        }
        
        if (amount > 0 && amount <= balance) {
            this.balance -= amount;
            recipient.balance += amount;
            System.out.println("Transferred $" + amount);
        } else {
            System.out.println("Transfer failed!");
        }
    }
    
    // Display account info (partial for security)
    public void displayAccountInfo() {
        System.out.println("Account Number: " + accountNumber);
        System.out.println("Account Holder: " + accountHolder);
        System.out.println("Balance: $" + balance);
    }
}

// ===== EMPLOYEE CLASS WITH ENCAPSULATION =====

class Employee {
    private String id;
    private String name;
    private String department;
    private double salary;
    private String phone;
    
    public Employee(String id, String name, String department, double salary, String phone) {
        this.id = id;
        this.name = name;
        this.department = department;
        
        if (salary >= 0) {
            this.salary = salary;
        } else {
            this.salary = 0;
        }
        
        this.phone = phone;
    }
    
    // Getters
    public String getId() { return id; }
    public String getName() { return name; }
    public String getDepartment() { return department; }
    public double getSalary() { return salary; }
    public String getPhone() { return phone; }
    
    // Setters
    public void setName(String name) {
        if (name != null && !name.isEmpty()) {
            this.name = name;
        }
    }
    
    public void setDepartment(String department) {
        if (department != null && !department.isEmpty()) {
            this.department = department;
        }
    }
    
    public void setSalary(double salary) {
        if (salary >= 0) {
            this.salary = salary;
        }
    }
    
    public void setPhone(String phone) {
        if (phone != null && phone.length() >= 10) {
            this.phone = phone;
        }
    }
    
    // Calculate annual salary
    public double getAnnualSalary() {
        return salary * 12;
    }
    
    // Give raise
    public void giveRaise(double percentage) {
        if (percentage > 0) {
            salary = salary + (salary * percentage / 100);
            System.out.println("Salary increased by " + percentage + "%");
        }
    }
    
    public void displayInfo() {
        System.out.println("Employee ID: " + id);
        System.out.println("Name: " + name);
        System.out.println("Department: " + department);
        System.out.println("Salary: $" + salary);
        System.out.println("Phone: " + phone);
        System.out.println("Annual Salary: $" + getAnnualSalary());
    }
}

// ===== MAIN CLASS =====
public class Example30 {
    public static void main(String[] args) {
        
        System.out.println("=== WITHOUT ENCAPSULATION (BAD) ===\n");
        
        // This is BAD - can set any value, even invalid ones!
        BadStudent bad = new BadStudent();
        bad.name = "John";
        bad.age = -50;        // Invalid! No protection
        bad.gpa = 10.0;      // Invalid! GPA should be 0-4
        
        System.out.println("Name: " + bad.name);
        System.out.println("Age: " + bad.age);
        System.out.println("GPA: " + bad.gpa);
        
        System.out.println("\n=== WITH ENCAPSULATION (GOOD) ===\n");
        
        // Create student with valid data
        Student student1 = new Student("Alice", 20, 3.8, "alice@email.com");
        student1.displayInfo();
        
        System.out.println("\n--- Trying to set invalid GPA ---");
        student1.setGpa(5.0);  // Invalid! Should fail
        
        System.out.println("\n--- Trying to set negative age ---");
        student1.setAge(-10);  // Invalid! Should fail
        
        System.out.println("\n--- Updating with valid values ---");
        student1.setGpa(3.9);
        student1.setAge(21);
        student1.displayInfo();
        
        System.out.println("\n--- Check honor roll ---");
        System.out.println("On honor roll: " + student1.isHonorRoll());
        
        // ===== BANK ACCOUNT EXAMPLE =====
        System.out.println("\n=== Bank Account Encapsulation ===\n");
        
        BankAccount account1 = new BankAccount("123456789", "John", "1234", 1000.0);
        BankAccount account2 = new BankAccount("987654321", "Jane", "5678", 500.0);
        
        account1.displayAccountInfo();
        
        System.out.println("\n--- Deposit ---");
        account1.deposit(500);
        
        System.out.println("\n--- Withdraw with correct PIN ---");
        account1.withdraw(200, "1234");
        
        System.out.println("\n--- Withdraw with wrong PIN ---");
        account1.withdraw(100, "0000");
        
        System.out.println("\n--- Transfer ---");
        account1.transfer(account2, 300, "1234");
        
        System.out.println("\n--- Account 1 after transfer ---");
        account1.displayAccountInfo();
        
        System.out.println("\n--- Account 2 after transfer ---");
        account2.displayAccountInfo();
        
        // ===== EMPLOYEE EXAMPLE =====
        System.out.println("\n=== Employee Encapsulation ===\n");
        
        Employee emp = new Employee("E001", "Bob", "IT", 5000, "1234567890");
        emp.displayInfo();
        
        System.out.println("\n--- Give raise ---");
        emp.giveRaise(10);  // 10% raise
        emp.displayInfo();
        
        // ===== KEY BENEFITS =====
        System.out.println("\n=== Key Benefits of Encapsulation ===\n");
        
        System.out.println("1. DATA PROTECTION:");
        System.out.println("   - Private variables cannot be accessed directly");
        System.out.println("   - Prevents invalid data");
        
        System.out.println("\n2. FLEXIBILITY:");
        System.out.println("   - Can change internal implementation");
        System.out.println("   - Doesn't affect external code");
        
        System.out.println("\n3. VALIDATION:");
        System.out.println("   - Setters can validate data before setting");
        System.out.println("   - Ensures data integrity");
        
        System.out.println("\n4. REUSABILITY:");
        System.out.println("   - Encapsulated classes are easy to reuse");
        System.out.println("   - Works as independent components");
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. WHAT IS ENCAPSULATION?
 *    - Bundling data and methods together
 *    - Restricting access to data
 *    - "Data Hiding"
 * 
 * 2. HOW TO ACHIEVE:
 *    - Make variables private
 *    - Provide public getters and setters
 *    - Validate in setters
 * 
 * 3. ACCESS MODIFIERS:
 *    - private: Only within the class
 *    - public: Anywhere
 *    - protected: Same package + subclasses
 *    - default: Same package only
 * 
 * 4. GETTER AND SETTER:
 *    - Getter: Returns value
 *    - Setter: Sets value with validation
 * 
 * 5. BENEFITS:
 *    - Data protection
 *    - Flexibility
 *    - Validation
 *    - Reusability
 *    - Easy to maintain
 * 
 * 6. EXAMPLE:
 *    private int age;
 *    public void setAge(int age) {
 *        if (age >= 0) {
 *            this.age = age;
 *        }
 *    }
 *    public int getAge() {
 *        return age;
 *    }
 */
