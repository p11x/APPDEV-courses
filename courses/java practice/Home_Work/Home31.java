import java.util.Scanner;
public class Home31 {
    public static void main(String[] args) {
        
        //11. Count frequency of each element in an array

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter size of array: ");
        int size = scanner.nextInt();
        
        int[] arr = new int[size];
        
        System.out.println("Enter " + size + " elements:");
        for (int i = 0; i < size; i++) {
            arr[i] = scanner.nextInt();
        }
        
        int[] frequency = new int[size];
        int[] visited = new int[size];
        
        for (int i = 0; i < size; i++) {
            frequency[i] = -1;
            visited[i] = -1;
        }
        
        for (int i = 0; i < size; i++) {
            int count = 1;
            for (int j = i + 1; j < size; j++) {
                if (arr[i] == arr[j]) {
                    count++;
                    frequency[j] = 0;
                }
            }
            if (frequency[i] != 0) {
                frequency[i] = count;
            }
        }
        
        System.out.println("Frequency of each element:");
        for (int i = 0; i < size; i++) {
            if (frequency[i] != 0) {
                System.out.println(arr[i] + " -> " + frequency[i]);
            }
        }
        
        scanner.close();
    }
}
