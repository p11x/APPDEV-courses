import java.util.Scanner;
public class Home10 {
    public static void main(String[] args) {
        


        //11. input week number and print day of the week

        Scanner scanner = new Scanner(System.in);    
        System.out.println("Enter the week number: ");    
        int weekNumber = scanner.nextInt();    
        String dayOfWeek = "";    
        switch (weekNumber) {    
            case 1:    
                dayOfWeek = "Sunday";    
                break;    
            case 2:    
                dayOfWeek = "Monday";    
                break;    
            case 3:    
                dayOfWeek = "Tuesday";    
                break;    
            case 4:    
                dayOfWeek = "Wednesday";    
                break;    
            case 5:    
                dayOfWeek = "Thursday";    
                break;    
            case 6:    
                dayOfWeek = "Friday";    
                break;    
            case 7:    
                dayOfWeek = "Saturday";    
                break;    
            default:    
                dayOfWeek = "Invalid week number";    
        }    
        System.out.println("Day of the week: " + dayOfWeek);    
        scanner.close();    
    }
}
