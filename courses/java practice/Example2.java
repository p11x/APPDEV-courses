
import java.util.Scanner;

public class Example2 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        System.out.println("Enter the number: ");
        int number = scanner.nextInt();
        if(number > 0){
           if(number % 2 == 0){
               System.out.println("The number is positive and even");
           }else{
               System.out.println("The number is positive and odd");
           } 
        }else if(number <0){
                if(number % 2 == 0){ 
                    System.out.println("The number is negative and even"); 
                }else{
                    System.out.println("The number is negative and odd"); 
                }
        }else{
            System.out.println("The number is zero");
        }
    }

        
        
}
