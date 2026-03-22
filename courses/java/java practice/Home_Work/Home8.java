import java.util.Scanner;
public class Home8 {
    public static void main(String[] args) {
        


        //9.check if alphabet, digit or special character


        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a character: ");
        char character = scanner.next().charAt(0);  
        if ((character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z')) {
            System.out.println("The character is an alphabet");
        } else if (character >= '0' && character <= '9') {
            System.out.println("The character is a digit");
        } else {
            System.out.println("The character is a special character");
        }
    }
}
