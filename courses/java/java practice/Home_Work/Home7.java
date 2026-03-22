
import java.util.Scanner;

public class Home7 {
    public static void main(String[] args) {
        

        //8.check if it is vowels or not


        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a character: ");
        char character = scanner.next().charAt(0);  
        if ((character == 'a') || (character == 'e') || (character == 'i') || (character == 'o') || (character == 'u')) {
            System.out.println("The character is a vowel");
        } else {
            System.out.println("The character is not a vowel");
        }
    }
}
