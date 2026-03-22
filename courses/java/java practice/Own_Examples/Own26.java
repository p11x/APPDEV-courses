import java.util.Scanner;

// Strong Number Checker
public class Own26 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Strong Number Checker ===");
        System.out.println();
        
        // Input a number
        System.out.print("Enter a number to check if it's Strong: ");
        int number = scanner.nextInt();
        
        // Check if the number is Strong
        if (isStrong(number)) {
            System.out.println(number + " is a Strong number!");
        } else {
            System.out.println(number + " is NOT a Strong number.");
        }
        
        System.out.println();
        
        // Find all Strong numbers between 1 to n
        System.out.print("Enter a range (n) to find all Strong numbers: ");
        int range = scanner.nextInt();
        
        System.out.println("Strong numbers between 1 and " + range + ":");
        int count = 0;
        
        for (int i = 1; i <= range; i++) {
            if (isStrong(i)) {
                System.out.print(i + " ");
                count++;
            }
        }
        
        System.out.println();
        System.out.println("Total Strong numbers found: " + count);
        
        scanner.close();
    }
    
    // Method to calculate factorial
    public static int factorial(int n) {
        int fact = 1;
        for (int i = 1; i <= n; i++) {
            fact = fact * i;
        }
        return fact;
    }
    
    // Method to check if a number is Strong
    public static boolean isStrong(int num) {
        int originalNum = num;
        int sum = 0;
        
        while (num > 0) {
            int digit = num % 10;
            sum = sum + factorial(digit);
            num = num / 10;
        }
        
        return sum == originalNum;
    }
}
