import java.util.Scanner;
public class Home21 {
    public static void main(String[] args) {
        
        //1. Read and print elements of array

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        System.out.println("Array elements:");
        for (int i = 0; i < size; i++) {
            System.out.println(arr[i]);
        }
        
        scanner.close();
    }
}
