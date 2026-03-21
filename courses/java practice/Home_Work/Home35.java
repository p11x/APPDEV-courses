import java.util.Scanner;
public class Home35 {
    public static void main(String[] args) {
        
        //15. Merge two arrays into a third array

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of first array: ");
        int size1 = scanner.nextInt();
        
        int[] arr1 = new int[size1];
        
        System.out.println("Enter " + size1 + " elements of first array:");
        for (int i = 0; i < size1; i++) {
            arr1[i] = scanner.nextInt();
        }
        
        System.out.print("Enter size of second array: ");
        int size2 = scanner.nextInt();
        
        int[] arr2 = new int[size2];
        
        System.out.println("Enter " + size2 + " elements of second array:");
        for (int i = 0; i < size2; i++) {
            arr2[i] = scanner.nextInt();
        }
        
        int size3 = size1 + size2;
        int[] arr3 = new int[size3];
        
        for (int i = 0; i < size1; i++) {
            arr3[i] = arr1[i];
        }
        
        for (int i = 0; i < size2; i++) {
            arr3[size1 + i] = arr2[i];
        }
        
        System.out.println("Merged array:");
        for (int i = 0; i < size3; i++) {
            System.out.println(arr3[i]);
        }
        
        scanner.close();
    }
}
