import java.util.Scanner;
public class Home28 {
    public static void main(String[] args) {
        
        //8. Copy all elements from an array to another array

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        int[] copy = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        for (int i = 0; i < size; i++) {
            copy[i] = arr[i];
        }
        
        System.out.println("Elements in original array:");
        for (int i = 0; i < size; i++) {
            System.out.println(arr[i]);
        }
        
        System.out.println("Elements in copied array:");
        for (int i = 0; i < size; i++) {
            System.out.println(copy[i]);
        }
        
        scanner.close();
    }
}
