import java.util.Scanner;

// Prime Numbers in a Range
public class Own21 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Prime Numbers in a Range ===");
        System.out.println();
        
        // Input two numbers (low and high)
        System.out.print("Enter the lower bound: ");
        int low = scanner.nextInt();
        
        System.out.print("Enter the upper bound: ");
        int high = scanner.nextInt();
        
        // Validate input
        while (low > high || low < 1) {
            System.out.println("Invalid range! Lower bound must be less than upper bound and >= 1.");
            System.out.print("Enter the lower bound: ");
            low = scanner.nextInt();
            System.out.print("Enter the upper bound: ");
            high = scanner.nextInt();
        }
        
        // Find and display prime numbers
        System.out.println();
        System.out.println("Prime numbers between " + low + " and " + high + ":");
        
        int primeCount = 0;
        
        for (int num = low; num <= high; num++) {
            if (isPrime(num)) {
                System.out.print(num + " ");
                primeCount++;
            }
        }
        
        // Display total count of primes
        System.out.println();
        System.out.println();
        System.out.println("Total count of prime numbers: " + primeCount);
        
        scanner.close();
    }
    
    // Method to check if a number is prime
    public static boolean isPrime(int num) {
        if (num <= 1) {
            return false;
        }
        
        // Check divisibility from 2 to sqrt(num)
        for (int i = 2; i * i <= num; i++) {
            if (num % i == 0) {
                return false;
            }
        }
        
        return true;
    }
}
