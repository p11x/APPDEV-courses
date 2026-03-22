import java.util.Scanner;
public class Home79 {
    public static void main(String[] args) {
        
        //20. Print a number in words

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
        
        for (int i = power; i >= 0; i--) {
            int divisor = (int)Math.pow(10, i);
            int digit = num / divisor;
            num = num % divisor;
            
            switch (digit) {
                case 0:
                    System.out.print("Zero ");
                    break;
                case 1:
                    System.out.print("One ");
                    break;
                case 2:
                    System.out.print("Two ");
                    break;
                case 3:
                    System.out.print("Three ");
                    break;
                case 4:
                    System.out.print("Four ");
                    break;
                case 5:
                    System.out.print("Five ");
                    break;
                case 6:
                    System.out.print("Six ");
                    break;
                case 7:
                    System.out.print("Seven ");
                    break;
                case 8:
                    System.out.print("Eight ");
                    break;
                case 9:
                    System.out.print("Nine ");
                    break;
            }
        }
        
        scanner.close();
    }
}
