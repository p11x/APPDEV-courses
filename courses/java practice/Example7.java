public class Example7 {
    public static void main(String[] args) {
        int n = 10;
        int i = 1;
        int sum = 0;
        System.out.print("The first " + n + " natural numbers are:");
        do{
            System.out.println(i+"\t");
            sum += i;
            i++;
        }while(i<=n);
        System.out.println("The sum of the first " + n + " natural numbers is: " + sum);
    }

        
}
