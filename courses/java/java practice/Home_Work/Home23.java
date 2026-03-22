import java.util.Scanner;
public class Home23 {
    public static void main(String[] args) {
        
        //3. Find sum of all array elements

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        int sum = 0;
        for (int i = 0; i < size; i++) {
            sum = sum + arr[i];
        }
        
        System.out.println("Sum of all elements = " + sum);
        
        scanner.close();
    }
}
