
import java.util.Scanner;

public class Example9 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        System.out.println("Enter the number: ");
        int number = scanner.nextInt();
        
        MathOperatione mathOps = new MathOperatione();


        //palindrome
        if (mathOps.palindrome(number) == number) {
            System.out.println("The number is a palindrome.");
        } else {
            System.out.println("The number is not a palindrome.");
        }


        //armstrong
        if (mathOps.armstrong(number) == number) {
            System.out.println("The number is an Armstrong number.");
        } else {
            System.out.println("The number is not an Armstrong number.");
        }


        //strong
        if (mathOps.strong(number) == number) {
            System.out.println("The number is a strong number.");
        } else {
            System.out.println("The number is not a strong number.");
        }


        //prime
        if (mathOps.primeNumber(number) == number) {
            System.out.println("The number is a prime number.");
        } else {
            System.out.println("The number is not a prime number.");
        }


        //perfect
        if (mathOps.perfect(number) == number) {
            System.out.println("The number is a perfect number.");
        } else {
            System.out.println("The number is not a perfect number.");
        }


        //sumoffactors
        mathOps.sumoffactors(number);
        System.out.println("The sum of factors of the number is: " + mathOps.sumoffactors(number));
         



        //sumofdigits 
        mathOps.sumofdigits(number);
        System.out.println("The sum of digits of the number is: " + mathOps.sumofdigits(number)); 


        


        //sumofdigitcubes
        mathOps.sumofdigitalcubes(number);
        System.out.println("The sum of cubes of digits of the number is: " + mathOps.sumofdigitalcubes(number));





        //reverse
        mathOps.reverse(number);
        System.out.println("The reverse of the number is: " + mathOps.reverse(number));


        scanner.close();



        

    }
        

}
