import java.util.Scanner;

// Array Rotation Visualizer
public class Own52 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Array Rotation Visualizer ===");
        System.out.println();
        
        // Input number of elements
        System.out.print("Enter number of elements: ");
        int n = scanner.nextInt();
        
        while (n <= 0) {
            System.out.print("Invalid! Enter positive number: ");
            n = scanner.nextInt();
        }
        
        // Input array elements
        int[] arr = new int[n];
        System.out.println("Enter " + n + " elements:");
        for (int i = 0; i < n; i++) {
            System.out.print("Element " + (i + 1) + ": ");
            arr[i] = scanner.nextInt();
        }
        
        // Display original array
        System.out.println();
        System.out.println("Original Array:");
        printArray(arr);
        
        // Input rotation positions
        System.out.print("Enter number of positions to rotate: ");
        int positions = scanner.nextInt();
        
        System.out.print("Enter direction (L for Left, R for Right): ");
        char direction = scanner.next().toUpperCase().charAt(0);
        
        // Normalize positions
        positions = positions % n;
        
        if (direction == 'L') {
            // Left rotation
            System.out.println();
            System.out.println("=== Left Rotation Steps ===");
            for (int step = 0; step < positions; step++) {
                int first = arr[0];
                for (int i = 0; i < n - 1; i++) {
                    arr[i] = arr[i + 1];
                }
                arr[n - 1] = first;
                
                System.out.print("Step " + (step + 1) + ": ");
                printArray(arr);
            }
        } else if (direction == 'R') {
            // Right rotation
            System.out.println();
            System.out.println("=== Right Rotation Steps ===");
            for (int step = 0; step < positions; step++) {
                int last = arr[n - 1];
                for (int i = n - 1; i > 0; i--) {
                    arr[i] = arr[i - 1];
                }
                arr[0] = last;
                
                System.out.print("Step " + (step + 1) + ": ");
                printArray(arr);
            }
        } else {
            System.out.println("Invalid direction!");
        }
        
        // Display final array
        System.out.println();
        System.out.println("Final Rotated Array:");
        printArray(arr);
        
        scanner.close();
    }
    
    public static void printArray(int[] arr) {
        for (int i = 0; i < arr.length; i++) {
            System.out.print(arr[i] + " ");
        }
        System.out.println();
    }
}
