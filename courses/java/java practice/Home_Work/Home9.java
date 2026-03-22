import java.util.Scanner;
public class Home9 {
    public static void main(String[] args) {
        


        //10.check if character is uppercase or lowercase alphabet


        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter a character: ");
        char character = scanner.next().charAt(0);
        if (character >= 'a' && character <= 'z') {
            System.out.println("The character is a lowercase alphabet");
        } else if (character >= 'A' && character <= 'Z') {
            System.out.println("The character is an uppercase alphabet");
        } else {
            System.out.println("The character is not an alphabet");
        }
    }
}
