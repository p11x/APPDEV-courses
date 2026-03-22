import java.util.Scanner;
public class Home30 {
    public static void main(String[] args) {
        
        //10. Delete an element from an array at specified position

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        System.out.print("Enter position to delete (1 to " + size + "): ");
        int position = scanner.nextInt();
        
        for (int i = position - 1; i < size - 1; i++) {
            arr[i] = arr[i + 1];
        }
        
        System.out.println("Array after deletion:");
        for (int i = 0; i < size - 1; i++) {
            System.out.println(arr[i]);
        }
        
        scanner.close();
    }
}
