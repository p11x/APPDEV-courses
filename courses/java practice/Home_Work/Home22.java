import java.util.Scanner;
public class Home22 {
    public static void main(String[] args) {
        
        //2. Print all negative elements in an array

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        System.out.println("Negative elements in the array:");
        for (int i = 0; i < size; i++) {
            if (arr[i] < 0) {
                System.out.println(arr[i]);
            }
        }
        
        scanner.close();
    }
}
