import java.util.Scanner;
public class Home13 {
    public static void main(String[] args) {
        
        

        //14. Check whether triangle is valid (angles)

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter first angle of triangle: ");
        int angle1 = scanner.nextInt();
        System.out.print("Enter second angle of triangle: ");
        int angle2 = scanner.nextInt();
        System.out.print("Enter third angle of triangle: ");
        int angle3 = scanner.nextInt();
        
        int sum = angle1 + angle2 + angle3;
        
        if (sum == 180 && angle1 > 0 && angle2 > 0 && angle3 > 0) {
            System.out.println("Triangle is valid");
        } else {
            System.out.println("Triangle is not valid");
        }
        
        scanner.close();
    }
}
