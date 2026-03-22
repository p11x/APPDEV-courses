import java.util.Scanner;
public class Home39 {
    public static void main(String[] args) {
        
        //19. Sort array elements in ascending or descending order

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        System.out.println("Press 1 for Ascending or 2 for Descending: ");
        int choice = scanner.nextInt();
        
        if (choice == 1) {
            for (int i = 0; i < size - 1; i++) {
                for (int j = 0; j < size - i - 1; j++) {
                    if (arr[j] > arr[j + 1]) {
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
            System.out.println("Array in Ascending order:");
        } else {
            for (int i = 0; i < size - 1; i++) {
                for (int j = 0; j < size - i - 1; j++) {
                    if (arr[j] < arr[j + 1]) {
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
            System.out.println("Array in Descending order:");
        }
        
        for (int i = 0; i < size; i++) {
            System.out.println(arr[i]);
        }
        
        scanner.close();
    }
}
