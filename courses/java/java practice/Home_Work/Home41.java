import java.util.Scanner;
public class Home41 {
    public static void main(String[] args) {
        
        //21. Left rotate an array

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        System.out.print("Enter number of positions to left rotate: ");
        int positions = scanner.nextInt();
        
        int[] rotated = new int[size];
        
        for (int i = 0; i < size; i++) {
            rotated[i] = arr[(i + positions) % size];
        }
        
        System.out.println("Array after left rotation:");
        for (int i = 0; i < size; i++) {
            System.out.println(rotated[i]);
        }
        
        scanner.close();
    }
}
