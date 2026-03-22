import java.util.Scanner;

// Array Statistics Calculator
public class Own22 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Array Statistics Calculator ===");
        System.out.println();
        
        // Input number of elements
        System.out.print("Enter number of elements: ");
        int n = scanner.nextInt();
        
        // Validate input
        while (n <= 0) {
            System.out.print("Invalid! Enter positive number: ");
            n = scanner.nextInt();
        }
        
        // Create array and input elements
        int[] arr = new int[n];
        
        System.out.println("Enter " + n + " elements:");
        for (int i = 0; i < n; i++) {
            System.out.print("Element " + (i + 1) + ": ");
            arr[i] = scanner.nextInt();
        }
        
        // Calculate statistics
        int sum = 0;
        int max = arr[0];
        int min = arr[0];
        int evenCount = 0;
        int oddCount = 0;
        int negativeCount = 0;
        
        for (int i = 0; i < n; i++) {
            // Sum
            sum = sum + arr[i];
            
            // Maximum
            if (arr[i] > max) {
                max = arr[i];
            }
            
            // Minimum
            if (arr[i] < min) {
                min = arr[i];
            }
            
            // Even/Odd count
            if (arr[i] % 2 == 0) {
                evenCount++;
            } else {
                oddCount++;
            }
            
            // Negative count
            if (arr[i] < 0) {
                negativeCount++;
            }
        }
        
        // Calculate average
        double average = (double) sum / n;
        
        // Display results
        System.out.println();
        System.out.println("=== Array Statistics ===");
        System.out.println("Sum: " + sum);
        System.out.println("Average: " + average);
        System.out.println("Maximum: " + max);
        System.out.println("Minimum: " + min);
        System.out.println("Count of even numbers: " + evenCount);
        System.out.println("Count of odd numbers: " + oddCount);
        System.out.println("Count of negative numbers: " + negativeCount);
        
        scanner.close();
    }
}
