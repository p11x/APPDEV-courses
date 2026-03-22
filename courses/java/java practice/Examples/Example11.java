import java.util.Scanner;
public class Example11 {
    public static void main(String[] args) {
        Scanner Scanner = new Scanner(System.in);
        System.out.println("Enter how many +ve  number you want to add : "); 
        int n = Scanner.nextInt();
        int sum = 0;
        int i  = 1;
        

        while (i<=n) {
            System.out.println("enter ant +ve number");
            int num = Scanner.nextInt();
            if(num<0){
                System.out.println("negitive number not allowed");
                continue; 
            }else{
                sum = sum + num;
            }
        }
        System.out.println("sum of all +ve number is : " + sum);
        
        
        Scanner.close();


        
    
    }
}
