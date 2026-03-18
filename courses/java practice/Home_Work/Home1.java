public class Home1 {
    public static void main(String[] args) {

        //1&2. find maximum btw two numbers  & minimum  btw three numbers

        int a = 10;
        int b = 20;

        if (a > b) {
            System.out.println("Maximum number is: " + a);
        } else if (a < b) {
            System.out.println("Maximum number is: " + b);
        } else {
            System.out.println("both are equal");
        }



        int c = 30;

        if (a > b && a > c) {
            System.out.println("Maximum number is: " + a);
        } else if (b > a && b > c) {
            System.out.println("Maximum number is: " + b);
        } else {
            System.out.println("Maximum number is: " + c);
        }

    }
}
