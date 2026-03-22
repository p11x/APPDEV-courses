import java.util.Scanner;
public class Home11 {
    public static void main(String[] args) {


        //12.input month number and print number of days in that month

        Scanner scanner = new Scanner(System.in);
        System.out.println("Enter the month number: ");
        int monthNumber = scanner.nextInt();
        String monthName = "";
        switch (monthNumber) {
            case 1:
                monthName = "January";
                System.out.println("January");
                System.out.println("Number of days in January: ");
                System.out.println("January has 31 days.");
                break;
            case 2:
                monthName = "February";
                System.out.println("February");
                System.out.println("Number of days in February: ");
                System.out.println("February has 28 days.");
                break;
            case 3:
                monthName = "March";
                System.out.println("March");
                System.out.println("Number of days in March: ");
                System.out.println("March has 31 days.");
                break;
            case 4:
                monthName = "April";
                System.out.println("April");
                System.out.println("Number of days in April: ");
                System.out.println("April has 30 days.");
                break;
            case 5:
                monthName = "May";
                System.out.println("May");
                System.out.println("Number of days in May: ");
                System.out.println("May has 31 days.");
                break;
            case 6:
                monthName = "June";
                System.out.println("June");
                System.out.println("Number of days in June: ");
                System.out.println("June has 30 days.");
                break;
            case 7:
                monthName = "July";
                System.out.println("July");
                System.out.println("Number of days in July: ");
                System.out.println("July has 31 days.");
                break;
            case 8:
                monthName = "August";
                System.out.println("August");
                System.out.println("Number of days in August: ");
                System.out.println("August has 31 days.");
                break;
            case 9:
                monthName = "September";
                System.out.println("September");
                System.out.println("Number of days in September: ");
                System.out.println("September has 30 days.");
                break;
            case 10:
                monthName = "October";
                System.out.println("October");
                System.out.println("Number of days in October: ");
                System.out.println("October has 31 days.");
                break;
            case 11:
                monthName = "November";
                System.out.println("November");
                System.out.println("Number of days in November: ");
                System.out.println("November has 30 days.");
                break;
            case 12:
                monthName = "December";
                System.out.println("December");
                System.out.println("Number of days in December: ");
                System.out.println("December has 31 days.");
                break;
            default:
                System.out.println("Invalid month number.");
        }
        
    }
}
