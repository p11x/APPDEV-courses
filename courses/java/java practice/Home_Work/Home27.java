import java.util.Scanner;
public class Home27 {
    public static void main(String[] args) {
        
        //7. Count total number of negative elements in an array

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        int negativeCount = 0;
        
        for (int i = 0; i < size; i++) {
            if (arr[i] < 0) {
                negativeCount++;
            }
        }
        
        System.out.println("Total negative elements = " + negativeCount);
        
        scanner.close();
    }
}
