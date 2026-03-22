import java.util.Scanner;
public class Home36 {
    public static void main(String[] args) {
        
        //16. Find reverse of an array

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        System.out.println("Original array:");
        for (int i = 0; i < size; i++) {
            System.out.println(arr[i]);
        }
        
        System.out.println("Reverse array:");
        for (int i = size - 1; i >= 0; i--) {
            System.out.println(arr[i]);
        }
        
        scanner.close();
    }
}
