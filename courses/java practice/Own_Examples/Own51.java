import java.util.Scanner;

// All Number Properties Checker
public class Own51 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== All Number Properties Checker ===");
        System.out.println();
        
        // Input a number
        System.out.print("Enter a number: ");
        int number = scanner.nextInt();
        
        System.out.println();
        System.out.println("=== Checking Number: " + number + " ===");
        System.out.println();
        
        // Check Prime
        if (isPrime(number)) {
            System.out.println("[✓] Prime: YES");
        } else {
            System.out.println("[✗] Prime: NO");
        }
        
        // Check Armstrong
        if (isArmstrong(number)) {
            System.out.println("[✓] Armstrong: YES");
        } else {
            System.out.println("[✗] Armstrong: NO");
        }
        
        // Check Perfect
        if (isPerfect(number)) {
            System.out.println("[✓] Perfect: YES");
        } else {
            System.out.println("[✗] Perfect: NO");
        }
        
        // Check Palindrome
        if (isPalindrome(number)) {
            System.out.println("[✓] Palindrome: YES");
        } else {
            System.out.println("[✗] Palindrome: NO");
        }
        
        // Check Strong
        if (isStrong(number)) {
            System.out.println("[✓] Strong: YES");
        } else {
            System.out.println("[✗] Strong: NO");
        }
        
        scanner.close();
    }
    
    // Method to check Prime
    public static boolean isPrime(int num) {
        if (num <= 1) return false;
        for (int i = 2; i * i <= num; i++) {
            if (num % i == 0) return false;
        }
        return true;
    }
    
    // Method to check Armstrong
    public static boolean isArmstrong(int num) {
        int original = num;
        int digits = String.valueOf(num).length();
        int sum = 0;
        
        while (num > 0) {
            int digit = num % 10;
            sum += Math.pow(digit, digits);
            num /= 10;
        }
        
        return sum == original;
    }
    
    // Method to check Perfect
    public static boolean isPerfect(int num) {
        int sum = 0;
        for (int i = 1; i < num; i++) {
            if (num % i == 0) sum += i;
        }
        return sum == num;
    }
    
    // Method to check Palindrome
    public static boolean isPalindrome(int num) {
        int original = num;
        int reversed = 0;
        
        while (num > 0) {
            reversed = reversed * 10 + num % 10;
            num /= 10;
        }
        
        return original == reversed;
    }
    
    // Method to check Strong
    public static boolean isStrong(int num) {
        int original = num;
        int sum = 0;
        
        while (num > 0) {
            int digit = num % 10;
            sum += factorial(digit);
            num /= 10;
        }
        
        return sum == original;
    }
    
    // Method to calculate factorial
    public static int factorial(int n) {
        int fact = 1;
        for (int i = 1; i <= n; i++) {
            fact *= i;
        }
        return fact;
    }
}
