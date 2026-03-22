// Number Pyramid Patterns (Advanced)
public class Own58 {
    public static void main(String[] args) {
        System.out.println("=== Number Pyramid Patterns (Advanced) ===");
        System.out.println();
        
        java.util.Scanner scanner = new java.util.Scanner(System.in);
        System.out.println("Choose a pattern:");
        System.out.println("1. Palindrome Number Pyramid");
        System.out.println("2. Floyd's Triangle");
        System.out.println("3. Prime Number Triangle");
        System.out.println("4. Diamond Pattern with Numbers");
        System.out.println("5. Pascal's Triangle");
        System.out.print("Enter choice (1-5): ");
        int choice = scanner.nextInt();
        
        System.out.println();
        
        switch (choice) {
            case 1:
                System.out.println("=== Palindrome Number Pyramid ===");
                System.out.print("Enter number of rows: ");
                int rows1 = scanner.nextInt();
                System.out.println();
                for (int i = 1; i <= rows1; i++) {
                    for (int space = 1; space <= rows1 - i; space++) {
                        System.out.print(" ");
                    }
                    for (int j = i; j >= 1; j--) {
                        System.out.print(j);
                    }
                    for (int j = 2; j <= i; j++) {
                        System.out.print(j);
                    }
                    System.out.println();
                }
                break;
                
            case 2:
                System.out.println("=== Floyd's Triangle ===");
                System.out.print("Enter number of rows: ");
                int rows2 = scanner.nextInt();
                System.out.println();
                int num = 1;
                for (int i = 1; i <= rows2; i++) {
                    for (int j = 1; j <= i; j++) {
                        System.out.print(num + "\t");
                        num++;
                    }
                    System.out.println();
                }
                break;
                
            case 3:
                System.out.println("=== Prime Number Triangle ===");
                System.out.print("Enter number of rows: ");
                int rows3 = scanner.nextInt();
                System.out.println();
                int count = 2;
                for (int i = 1; i <= rows3; i++) {
                    for (int j = 1; j <= i; j++) {
                        while (!isPrime(count)) {
                            count++;
                        }
                        System.out.print(count + "\t");
                        count++;
                    }
                    System.out.println();
                }
                break;
                
            case 4:
                System.out.println("=== Diamond Pattern with Numbers ===");
                System.out.print("Enter number of rows (odd): ");
                int rows4 = scanner.nextInt();
                System.out.println();
                
                // Upper half
                for (int i = 1; i <= rows4 / 2 + 1; i++) {
                    for (int space = 1; space <= rows4 / 2 + 1 - i; space++) {
                        System.out.print(" ");
                    }
                    for (int j = 1; j <= 2 * i - 1; j++) {
                        System.out.print(j);
                    }
                    System.out.println();
                }
                
                // Lower half
                for (int i = rows4 / 2; i >= 1; i--) {
                    for (int space = 1; space <= rows4 / 2 + 1 - i; space++) {
                        System.out.print(" ");
                    }
                    for (int j = 1; j <= 2 * i - 1; j++) {
                        System.out.print(j);
                    }
                    System.out.println();
                }
                break;
                
            case 5:
                System.out.println("=== Pascal's Triangle ===");
                System.out.print("Enter number of rows: ");
                int rows5 = scanner.nextInt();
                System.out.println();
                
                for (int i = 0; i < rows5; i++) {
                    // Spaces
                    for (int space = 1; space <= rows5 - i; space++) {
                        System.out.print(" ");
                    }
                    
                    // Calculate and print values
                    int value = 1;
                    for (int j = 0; j <= i; j++) {
                        System.out.print(value + " ");
                        value = value * (i - j) / (j + 1);
                    }
                    System.out.println();
                }
                break;
                
            default:
                System.out.println("Invalid choice!");
        }
        
        scanner.close();
    }
    
    static boolean isPrime(int n) {
        if (n <= 1) return false;
        if (n <= 3) return true;
        if (n % 2 == 0 || n % 3 == 0) return false;
        for (int i = 5; i * i <= n; i += 6) {
            if (n % i == 0 || n % (i + 2) == 0) return false;
        }
        return true;
    }
}
