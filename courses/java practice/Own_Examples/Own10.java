import java.util.Scanner;

// Simple ATM Simulator
public class Own10 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Fixed balance
        double balance = 10000.0;
        int choice;
        
        System.out.println("=== Simple ATM Simulator ===");
        System.out.println("Welcome! Your initial balance is Rs. 10,000");
        System.out.println();
        
        // Menu loop
        do {
            // Display menu
            System.out.println("===== ATM Menu =====");
            System.out.println("1. Check Balance");
            System.out.println("2. Deposit");
            System.out.println("3. Withdraw");
            System.out.println("4. Exit");
            System.out.print("Enter your choice: ");
            
            choice = scanner.nextInt();
            System.out.println();
            
            switch (choice) {
                case 1:
                    // Check Balance
                    System.out.println("Your current balance is: Rs. " + balance);
                    break;
                    
                case 2:
                    // Deposit
                    System.out.print("Enter amount to deposit: Rs. ");
                    double depositAmount = scanner.nextDouble();
                    
                    if (depositAmount > 0) {
                        balance = balance + depositAmount;
                        System.out.println("Deposit successful!");
                        System.out.println("New balance: Rs. " + balance);
                    } else {
                        System.out.println("Invalid amount! Deposit amount must be positive.");
                    }
                    break;
                    
                case 3:
                    // Withdraw
                    System.out.print("Enter amount to withdraw: Rs. ");
                    double withdrawAmount = scanner.nextDouble();
                    
                    if (withdrawAmount > 0) {
                        if (withdrawAmount <= balance) {
                            balance = balance - withdrawAmount;
                            System.out.println("Withdrawal successful!");
                            System.out.println("New balance: Rs. " + balance);
                        } else {
                            System.out.println("Insufficient funds!");
                            System.out.println("Your balance is: Rs. " + balance);
                        }
                    } else {
                        System.out.println("Invalid amount! Withdrawal amount must be positive.");
                    }
                    break;
                    
                case 4:
                    // Exit
                    System.out.println("Thank you for using ATM!");
                    System.out.println("Your final balance: Rs. " + balance);
                    break;
                    
                default:
                    System.out.println("Invalid choice! Please enter 1-4.");
            }
            
            System.out.println();
            
        } while (choice != 4);
        
        scanner.close();
    }
}
