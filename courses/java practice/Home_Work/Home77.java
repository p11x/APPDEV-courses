import java.util.Scanner;
public class Home77 {
    public static void main(String[] args) {
        
        //18. Check whether a number is Prime or not using both while loop and for loop

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a number: ");
        int num = scanner.nextInt();
        
        // Using while loop
        System.out.println("Using while loop:");
        int i = 2;
        boolean isPrimeWhile = true;
        
        while (i <= num / 2) {
            if (num % i == 0) {
                isPrimeWhile = false;
                break;
            }
            i++;
        }
        
        if (isPrimeWhile && num > 1) {
            System.out.println(num + " is a prime number");
        } else {
            System.out.println(num + " is not a prime number");
        }
        
        // Using for loop
        System.out.println("Using for loop:");
        boolean isPrimeFor = true;
        
        for (int j = 2; j <= num / 2; j++) {
            if (num % j == 0) {
                isPrimeFor = false;
                break;
            }
        }
        
        if (isPrimeFor && num > 1) {
            System.out.println(num + " is a prime number");
        } else {
            System.out.println(num + " is not a prime number");
        }
        
        scanner.close();
    }
}
