import java.util.Scanner;

// Bank Account Manager
public class Own41 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Declare arrays for 3 account holders
        String[] names = {"John", "Alice", "Bob"};
        String[] accountNumbers = {"ACC001", "ACC002", "ACC003"};
        double[] balances = {10000, 15000, 8000};
        
        int choice;
        
        System.out.println("=== Bank Account Manager ===");
        System.out.println();
        
        // Menu loop
        do {
            System.out.println("===== Bank Menu =====");
            System.out.println("1. View all accounts");
            System.out.println("2. Deposit into an account");
            System.out.println("3. Withdraw from an account");
            System.out.println("4. Transfer between two accounts");
            System.out.println("5. Exit");
            System.out.print("Enter your choice: ");
            
            choice = scanner.nextInt();
            System.out.println();
            
            switch (choice) {
                case 1:
                    // View all accounts
                    System.out.println("=== All Accounts ===");
                    for (int i = 0; i < 3; i++) {
                        System.out.println("Account: " + accountNumbers[i]);
                        System.out.println("Name: " + names[i]);
                        System.out.println("Balance: Rs. " + balances[i]);
                        System.out.println();
                    }
                    break;
                    
                case 2:
                    // Deposit
                    System.out.print("Enter account number: ");
                    String depositAcc = scanner.next();
                    
                    int depositIndex = -1;
                    for (int i = 0; i < 3; i++) {
                        if (accountNumbers[i].equals(depositAcc)) {
                            depositIndex = i;
                            break;
                        }
                    }
                    
                    if (depositIndex != -1) {
                        System.out.print("Enter amount to deposit: Rs. ");
                        double depositAmount = scanner.nextDouble();
                        
                        if (depositAmount > 0) {
                            balances[depositIndex] += depositAmount;
                            System.out.println("Deposit successful!");
                            System.out.println("New balance: Rs. " + balances[depositIndex]);
                        } else {
                            System.out.println("Invalid amount!");
                        }
                    } else {
                        System.out.println("Account not found!");
                    }
                    break;
                    
                case 3:
                    // Withdraw
                    System.out.print("Enter account number: ");
                    String withdrawAcc = scanner.next();
                    
                    int withdrawIndex = -1;
                    for (int i = 0; i < 3; i++) {
                        if (accountNumbers[i].equals(withdrawAcc)) {
                            withdrawIndex = i;
                            break;
                        }
                    }
                    
                    if (withdrawIndex != -1) {
                        System.out.print("Enter amount to withdraw: Rs. ");
                        double withdrawAmount = scanner.nextDouble();
                        
                        if (withdrawAmount > 0) {
                            if (withdrawAmount <= balances[withdrawIndex]) {
                                balances[withdrawIndex] -= withdrawAmount;
                                System.out.println("Withdrawal successful!");
                                System.out.println("New balance: Rs. " + balances[withdrawIndex]);
                            } else {
                                System.out.println("Insufficient balance!");
                            }
                        } else {
                            System.out.println("Invalid amount!");
                        }
                    } else {
                        System.out.println("Account not found!");
                    }
                    break;
                    
                case 4:
                    // Transfer
                    System.out.print("Enter sender account number: ");
                    String senderAcc = scanner.next();
                    
                    System.out.print("Enter receiver account number: ");
                    String receiverAcc = scanner.next();
                    
                    int senderIndex = -1, receiverIndex = -1;
                    
                    for (int i = 0; i < 3; i++) {
                        if (accountNumbers[i].equals(senderAcc)) {
                            senderIndex = i;
                        }
                        if (accountNumbers[i].equals(receiverAcc)) {
                            receiverIndex = i;
                        }
                    }
                    
                    if (senderIndex != -1 && receiverIndex != -1) {
                        System.out.print("Enter amount to transfer: Rs. ");
                        double transferAmount = scanner.nextDouble();
                        
                        if (transferAmount > 0) {
                            if (transferAmount <= balances[senderIndex]) {
                                balances[senderIndex] -= transferAmount;
                                balances[receiverIndex] += transferAmount;
                                System.out.println("Transfer successful!");
                                System.out.println("Sender new balance: Rs. " + balances[senderIndex]);
                                System.out.println("Receiver new balance: Rs. " + balances[receiverIndex]);
                            } else {
                                System.out.println("Insufficient balance!");
                            }
                        } else {
                            System.out.println("Invalid amount!");
                        }
                    } else {
                        System.out.println("Account(s) not found!");
                    }
                    break;
                    
                case 5:
                    System.out.println("Thank you for using Bank Manager!");
                    break;
                    
                default:
                    System.out.println("Invalid choice!");
            }
            
            System.out.println();
            
        } while (choice != 5);
        
        scanner.close();
    }
}
