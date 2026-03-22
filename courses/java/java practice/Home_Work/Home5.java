import java.util.Scanner;
public class Home5 {
    public static void main(String[] args) {


        //6.check wether year is leap year or not

        Scanner scanner = new Scanner(System.in);
        System.out.println("Enter the year: ");
        int year = scanner.nextInt();


        boolean isLeapYear = (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
        System.out.println("The year is " + (isLeapYear ? "leap year" : "not a leap year"));
        
    }
}
