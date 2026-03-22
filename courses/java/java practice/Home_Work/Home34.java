import java.util.Scanner;
public class Home34 {
    public static void main(String[] args) {
        
        //14. Delete all duplicate elements from an array

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        int[] temp = new int[size];
        int newSize = 0;
        
        for (int i = 0; i < size; i++) {
            boolean isDuplicate = false;
            for (int j = 0; j < newSize; j++) {
                if (arr[i] == temp[j]) {
                    isDuplicate = true;
                    break;
                }
            }
            if (!isDuplicate) {
                temp[newSize] = arr[i];
                newSize++;
            }
        }
        
        System.out.println("Array after removing duplicates:");
        for (int i = 0; i < newSize; i++) {
            System.out.println(temp[i]);
        }
        
        scanner.close();
    }
}
