import java.util.Scanner;
public class Home14 {
    public static void main(String[] args) {
        
        

        //15. Check whether triangle is valid (sides)

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter first side of triangle: ");
        int side1 = scanner.nextInt();
        System.out.print("Enter second side of triangle: ");
        int side2 = scanner.nextInt();
        System.out.print("Enter third side of triangle: ");
        int side3 = scanner.nextInt();
        
        if (side1 + side2 > side3 && side2 + side3 > side1 && side1 + side3 > side2) {
            System.out.println("Triangle is valid");
        } else {
            System.out.println("Triangle is not valid");
        }
        
        scanner.close();
    }
}
