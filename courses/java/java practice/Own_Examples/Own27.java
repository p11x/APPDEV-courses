import java.util.Scanner;

// Perfect Number Checker
public class Own27 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Perfect Number Checker ===");
        System.out.println();
        
        // Input a number
        System.out.print("Enter a number to check if it's Perfect: ");
        int number = scanner.nextInt();
        
        // Check if the number is Perfect
        if (isPerfect(number)) {
            System.out.println(number + " is a Perfect number!");
        } else {
            System.out.println(number + " is NOT a Perfect number.");
        }
        
        System.out.println();
        
        // Find all Perfect numbers between 1 to 1000
        System.out.println("Perfect numbers between 1 and 1000:");
        int count = 0;
        
        for (int i = 1; i <= 1000; i++) {
            if (isPerfect(i)) {
                System.out.print(i + " ");
                count++;
            }
        }
        
        System.out.println();
        System.out.println("Total Perfect numbers found: " + count);
        
        scanner.close();
    }
    
    // Method to check if a number is Perfect
    public static boolean isPerfect(int num) {
        // Sum of proper divisors (excluding the number itself)
        int sum = 0;
        
        for (int i = 1; i < num; i++) {
            if (num % i == 0) {
                sum = sum + i;
            }
        }
        
        return sum == num;
    }
}
