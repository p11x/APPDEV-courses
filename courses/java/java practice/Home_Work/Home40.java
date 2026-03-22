import java.util.Scanner;
public class Home40 {
    public static void main(String[] args) {
        
        //20. Sort even and odd elements of array separately

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
        
        for (int i = 0; i < evenCount - 1; i++) {
            for (int j = 0; j < evenCount - i - 1; j++) {
                if (evenArr[j] > evenArr[j + 1]) {
                    int temp = evenArr[j];
                    evenArr[j] = evenArr[j + 1];
                    evenArr[j + 1] = temp;
                }
            }
        }
        
        for (int i = 0; i < oddCount - 1; i++) {
            for (int j = 0; j < oddCount - i - 1; j++) {
                if (oddArr[j] > oddArr[j + 1]) {
                    int temp = oddArr[j];
                    oddArr[j] = oddArr[j + 1];
                    oddArr[j + 1] = temp;
                }
            }
        }
        
        System.out.println("Sorted even elements:");
        for (int i = 0; i < evenCount; i++) {
            System.out.println(evenArr[i]);
        }
        
        System.out.println("Sorted odd elements:");
        for (int i = 0; i < oddCount; i++) {
            System.out.println(oddArr[i]);
        }
        
        scanner.close();
    }
}
