import java.util.Scanner;
public class Home37 {
    public static void main(String[] args) {
        
        //17. Put even and odd elements of array in two separate arrays

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        int evenCount = 0;
        int oddCount = 0;
        
        for (int i = 0; i < size; i++) {
            if (arr[i] % 2 == 0) {
                evenCount++;
            } else {
                oddCount++;
            }
        }
        
        int[] evenArr = new int[evenCount];
        int[] oddArr = new int[oddCount];
        
        int evenIndex = 0;
        int oddIndex = 0;
        
        for (int i = 0; i < size; i++) {
            if (arr[i] % 2 == 0) {
                evenArr[evenIndex] = arr[i];
                evenIndex++;
            } else {
                oddArr[oddIndex] = arr[i];
                oddIndex++;
            }
        }
        
        System.out.println("Even elements:");
        for (int i = 0; i < evenCount; i++) {
            System.out.println(evenArr[i]);
        }
        
        System.out.println("Odd elements:");
        for (int i = 0; i < oddCount; i++) {
            System.out.println(oddArr[i]);
        }
        
        scanner.close();
    }
}
