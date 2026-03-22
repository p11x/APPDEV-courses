import java.util.Scanner;
public class Home26 {
    public static void main(String[] args) {
        
        //6. Count total number of even and odd elements in an array

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        int evenCount = 0;
        int oddCount = 0;
        
        for (int i = 0; i < size; i++) {
            if (arr[i] % 2 == 0) {
                evenCount++;
            } else {
                oddCount++;
            }
        }
        
        System.out.println("Total even elements = " + evenCount);
        System.out.println("Total odd elements = " + oddCount);
        
        scanner.close();
    }
}
