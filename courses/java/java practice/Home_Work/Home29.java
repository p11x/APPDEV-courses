import java.util.Scanner;
public class Home29 {
    public static void main(String[] args) {
        
        //9. Insert an element in an array at a specified position

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size + 1];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        System.out.print("Enter position to insert (1 to " + (size + 1) + "): ");
        int position = scanner.nextInt();
        
        System.out.print("Enter element to insert: ");
        int element = scanner.nextInt();
        
        for (int i = size; i >= position; i--) {
            arr[i] = arr[i - 1];
        }
        arr[position - 1] = element;
        
        System.out.println("Array after insertion:");
        for (int i = 0; i <= size; i++) {
            System.out.println(arr[i]);
        }
        
        scanner.close();
    }
}
