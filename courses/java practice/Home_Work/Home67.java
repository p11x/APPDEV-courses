import java.util.Scanner;
public class Home67 {
    public static void main(String[] args) {
        
        //8. Swap first and last digit of a number

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a number: ");
        int num = scanner.nextInt();
        
        int temp = num;
        int count = 0;
        
        while (temp > 0) {
            count++;
            temp = temp / 10;
        }
        
        int power = count - 1;
        int firstDigit = num / (int)Math.pow(10, power);
        int lastDigit = num % 10;
        
        int middle = num % (int)Math.pow(10, power);
        middle = middle / 10;
        
        int newNum = lastDigit * (int)Math.pow(10, power) + middle * 10 + firstDigit;
        
        System.out.println("Number after swapping first and last digit: " + newNum);
        
        scanner.close();
    }
}
