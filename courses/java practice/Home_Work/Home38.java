import java.util.Scanner;
public class Home38 {
    public static void main(String[] args) {
        
        //18. Search an element in an array (Linear Search)

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        System.out.print("Enter element to search: ");
        int search = scanner.nextInt();
        
        int found = -1;
        
        for (int i = 0; i < size; i++) {
            if (arr[i] == search) {
                found = i;
                break;
            }
        }
        
        if (found != -1) {
            System.out.println("Element found at position " + (found + 1));
        } else {
            System.out.println("Element not found");
        }
        
        scanner.close();
    }
}
