import java.util.Scanner;
public class Home15 {
    public static void main(String[] args) {
        
        

        //16. Check whether triangle is equilateral, isosceles or scalene

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter first side of triangle: ");
        int side1 = scanner.nextInt();
        System.out.print("Enter second side of triangle: ");
        int side2 = scanner.nextInt();
        System.out.print("Enter third side of triangle: ");
        int side3 = scanner.nextInt();
        
        if (side1 + side2 > side3 && side2 + side3 > side1 && side1 + side3 > side2) {
            if (side1 == side2 && side2 == side3) {
                System.out.println("Equilateral Triangle");
            } else if (side1 == side2 || side2 == side3 || side1 == side3) {
                System.out.println("Isosceles Triangle");
            } else {
                System.out.println("Scalene Triangle");
            }
        } else {
            System.out.println("Triangle is not valid");
        }
        
        scanner.close();
    }
}
