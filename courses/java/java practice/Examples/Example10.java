public class Example10 {
    public static void main(String[] args) {
        
        //lucky number
        MathOperatione mathOps = new MathOperatione();
        int sum = 0;

        System.out.println("Lucky numbers from 4200 to 4500 are:");
        
        for (int n=4200;n<=4500;n++) {
            sum=mathOps.sumofdigits(n);
        while(sum>9) {
            sum=mathOps.sumofdigits(sum);
        }
        if(sum==9) {
            System.out.println(n+"\t");
        }
        }
    }
}
