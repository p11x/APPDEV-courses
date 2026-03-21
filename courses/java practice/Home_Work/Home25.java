import java.util.Scanner;
public class Home25 {
    public static void main(String[] args) {
        
        //5. Find second largest element in an array

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        int largest = arr[0];
        int secondLargest = arr[0];
        
        for (int i = 1; i < size; i++) {
            if (arr[i] > largest) {
                secondLargest = largest;
                largest = arr[i];
            } else if (arr[i] > secondLargest) {
                secondLargest = arr[i];
            }
        }
        
        System.out.println("Second largest element = " + secondLargest);
        
        scanner.close();
    }
}
