import java.util.Scanner;
public class Home12 {
    public static void main(String[] args) {
        


        //13.count total number of notes in given amount

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter the amount: "); 
        int amount = scanner.nextInt(); 
        int notes1000 = amount / 1000; 
        int notes500 = (amount % 1000) / 500; 
        int notes100 = ((amount % 1000) % 500) / 100; 
        int notes50 = ((amount % 1000) % 500) % 100 / 50; 
        int notes20 = ((amount % 1000) % 500) % 100 % 50 / 20; 
        int notes10 = ((amount % 1000) % 500) % 100 % 50 % 20 / 10; 
        int notes5 = ((amount % 1000) % 500) % 100 % 50 % 20 % 10 / 5; 
        int notes2 = ((amount % 1000) % 500) % 100 % 50 % 20 % 10 % 5 / 2; 
        int notes1 = ((amount % 1000) % 500) % 100 % 50 % 20 % 10 % 5 % 2; 
        System.out.println("Notes of 1000: " + notes1000); 
        System.out.println("Notes of 500: " + notes500); 
        System.out.println("Notes of 100: " + notes100); 
        System.out.println("Notes of 50: " + notes50); 
        System.out.println("Notes of 20: " + notes20); 
        System.out.println("Notes of 10: " + notes10); 
        System.out.println("Notes of 5: " + notes5); 
        System.out.println("Notes of 2: " + notes2); 
        System.out.println("Notes of 1: " + notes1); 
        scanner.close(); 
    }
}
