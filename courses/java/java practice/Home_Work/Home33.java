import java.util.Scanner;
public class Home33 {
    public static void main(String[] args) {
        
        //13. Count total number of duplicate elements in an array

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        int duplicateCount = 0;
        
        for (int i = 0; i < size; i++) {
            for (int j = i + 1; j < size; j++) {
                if (arr[i] == arr[j]) {
                    duplicateCount++;
                    break;
                }
            }
        }
        
        System.out.println("Total number of duplicate elements = " + duplicateCount);
        
        scanner.close();
    }
}
