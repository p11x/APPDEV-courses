// BankAccount - A simple class representing a bank account
// Demonstrates fundamental class structure, fields, constructors, and methods
// This class is used to demonstrate OOP concepts in Java
// and shows how Java objects map to JSON for Angular frontend

public class BankAccount {
    
    // ==================== FIELDS ====================
    private String accountNumber;
    private String accountHolderName;
    private double balance;
    
    // ==================== CONSTRUCTORS ====================
    
    // Default constructor
    public BankAccount() {
        this.accountNumber = "000000";
        this.accountHolderName = "Unknown";
        this.balance = 0.0;
    }
    
    // Parameterized constructor
    public BankAccount(String accountNumber, String accountHolderName, double balance) {
        this.accountNumber = accountNumber;
        this.accountHolderName = accountHolderName;
        this.balance = balance;
    }
    
    // ==================== GETTERS ====================
    public String getAccountNumber() { return this.accountNumber; }
    public String getAccountHolderName() { return this.accountHolderName; }
    public double getBalance() { return this.balance; }
    
    // ==================== SETTERS ====================
    public void setAccountHolderName(String name) { this.accountHolderName = name; }
    
    public void setBalance(double balance) {
        if (balance >= 0) {
            this.balance = balance;
        } else {
            System.out.println("Error: Balance cannot be negative!");
        }
    }
    
    // ==================== METHODS ====================
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
    
    public void displayAccountInfo() {
        System.out.println("=== ACCOUNT INFORMATION ===");
        System.out.println("Account Number: " + this.accountNumber);
        System.out.println("Holder Name:    " + this.accountHolderName);
        System.out.println("Balance:        $" + this.balance);
    }
    
    // Main method for demonstration
    public static void main(String[] args) {
        System.out.println("=== BANK ACCOUNT DEMO ===\n");
        
        BankAccount account1 = new BankAccount();
        System.out.println("--- Account 1 (Default) ---");
        account1.displayAccountInfo();
        
        BankAccount account2 = new BankAccount("ACC123456", "John Smith", 5000.00);
        System.out.println("\n--- Account 2 ---");
        account2.displayAccountInfo();
        
        System.out.println("\n--- Operations ---");
        account2.deposit(1000.00);
        account2.withdraw(500.00);
        account2.displayAccountInfo();
    }
}
