// Binary Search with Step Counter
public class Own57 {
    public static void main(String[] args) {
        System.out.println("=== Binary Search with Step Counter ===");
        System.out.println();
        
        // Pre-sorted array
        int[] arr = {2, 5, 8, 12, 16, 23, 38, 56, 72, 91};
        
        System.out.println("Sorted Array:");
        for (int i = 0; i < arr.length; i++) {
            System.out.print(arr[i] + " ");
        }
        System.out.println();
        
        // Take input for search
        java.util.Scanner scanner = new java.util.Scanner(System.in);
        System.out.println();
        System.out.print("Enter number to search: ");
        int target = scanner.nextInt();
        
        // Linear Search
        System.out.println();
        System.out.println("=== Linear Search ===");
        int linearSteps = 0;
        int linearIndex = -1;
        for (int i = 0; i < arr.length; i++) {
            linearSteps++;
            System.out.println("Step " + linearSteps + ": Checking index " + i + ", value " + arr[i]);
            if (arr[i] == target) {
                linearIndex = i;
                break;
            }
        }
        
        if (linearIndex != -1) {
            System.out.println("Found at index " + linearIndex + " in " + linearSteps + " steps");
        } else {
            System.out.println("Not found in " + linearSteps + " steps");
        }
        
        // Binary Search
        System.out.println();
        System.out.println("=== Binary Search ===");
        int binarySteps = 0;
        int left = 0;
        int right = arr.length - 1;
        int binaryIndex = -1;
        
        while (left <= right) {
            binarySteps++;
            int mid = (left + right) / 2;
            System.out.println("Step " + binarySteps + ": left=" + left + ", right=" + right + ", mid=" + mid + ", value=" + arr[mid]);
            
            if (arr[mid] == target) {
                binaryIndex = mid;
                break;
            } else if (arr[mid] < target) {
                System.out.println("  -> " + arr[mid] + " < " + target + ", searching right half");
                left = mid + 1;
            } else {
                System.out.println("  -> " + arr[mid] + " > " + target + ", searching left half");
                right = mid - 1;
            }
        }
        
        if (binaryIndex != -1) {
            System.out.println("Found at index " + binaryIndex + " in " + binarySteps + " steps");
        } else {
            System.out.println("Not found in " + binarySteps + " steps");
        }
        
        // Comparison
        System.out.println();
        System.out.println("=== Comparison ===");
        System.out.println("Linear Search: " + linearSteps + " steps");
        System.out.println("Binary Search: " + binarySteps + " steps");
        System.out.println("Binary Search is " + (linearSteps - binarySteps) + " steps faster!");
        
        scanner.close();
    }
}
