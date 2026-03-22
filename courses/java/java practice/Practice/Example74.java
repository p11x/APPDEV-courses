/*
 * SUB TOPIC: Encapsulation - Data Hiding
 * 
 * DEFINITION:
 * Encapsulation wraps data and methods into a single unit. Uses private fields with public getters/setters.
 */

class BankAccount {
    private double balance = 0;
    
    public void deposit(double amount) {
        if (amount > 0) balance += amount;
    }
    
    public double getBalance() { return balance; }
}

public class Example74 {
    public static void main(String[] args) {
        BankAccount acc = new BankAccount();
        acc.deposit(1000);
        System.out.println("Balance: " + acc.getBalance());
    }
}
