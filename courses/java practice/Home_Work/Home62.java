import java.util.Scanner;
public class Home62 {
    public static void main(String[] args) {
        
        //3. Print multiplication table of a given number

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a number: ");
        int num = scanner.nextInt();
        
        for (int i = 1; i <= 10; i++) {
            System.out.println(num + " * " + i + " = " + (num * i));
        }
        
        scanner.close();
    }
}
