import java.util.Scanner;

// Bubble Sort Visualizer
public class Own28 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Bubble Sort Visualizer ===");
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
        
        // Display original array
        System.out.println();
        System.out.println("Original Array:");
        printArray(arr);
        
        // Perform Bubble Sort with visualization
        System.out.println();
        System.out.println("=== Bubble Sort Steps ===");
        
        int pass = 1;
        boolean swapped;
        
        for (int i = 0; i < n - 1; i++) {
            swapped = false;
            
            System.out.println();
            System.out.println("--- Pass " + pass + " ---");
            
            for (int j = 0; j < n - i - 1; j++) {
                if (arr[j] > arr[j + 1]) {
                    // Swap elements
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                    swapped = true;
                    
                    System.out.println("Swapped " + arr[j + 1] + " and " + arr[j]);
                    printArray(arr);
                }
            }
            
            // If no swapping occurred, array is already sorted
            if (!swapped) {
                System.out.println("No swapping in this pass - Array is sorted!");
                break;
            }
            
            pass++;
        }
        
        // Display sorted array
        System.out.println();
        System.out.println("=== Final Sorted Array ===");
        printArray(arr);
        
        scanner.close();
    }
    
    // Method to print array
    public static void printArray(int[] arr) {
        for (int i = 0; i < arr.length; i++) {
            System.out.print(arr[i] + " ");
        }
        System.out.println();
    }
}
