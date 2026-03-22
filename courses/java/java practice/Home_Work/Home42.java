import java.util.Scanner;
public class Home42 {
    public static void main(String[] args) {
        
        //22. Right rotate an array

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        System.out.print("Enter number of positions to right rotate: ");
        int positions = scanner.nextInt();
        
        int[] rotated = new int[size];
        
        for (int i = 0; i < size; i++) {
            rotated[(i + positions) % size] = arr[i];
        }
        
        System.out.println("Array after right rotation:");
        for (int i = 0; i < size; i++) {
            System.out.println(rotated[i]);
        }
        
        scanner.close();
    }
}
