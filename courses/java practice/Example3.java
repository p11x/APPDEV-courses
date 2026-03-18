import java.util.Scanner;            
public class Example3 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        System.out.println("Enter the number: ");
        int number = scanner.nextInt();
        String result = (number % 2 == 0) ? "number is Even" : "number is Odd";
        System.out.println("The number is " + result);   
        }
        

}
    

