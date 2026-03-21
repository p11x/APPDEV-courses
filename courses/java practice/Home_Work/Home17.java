import java.util.Scanner;
public class Home17 {
    public static void main(String[] args) {
        
        

        //18. Calculate profit or loss

        Scanner scanner = new Scanner(System.in);
        System.out.print("Enter cost price: ");
        double costPrice = scanner.nextDouble();
        System.out.print("Enter selling price: ");
        double sellingPrice = scanner.nextDouble();
        
        if (sellingPrice > costPrice) {
            double profit = sellingPrice - costPrice;
            System.out.println("Profit = " + profit);
        } else if (costPrice > sellingPrice) {
            double loss = costPrice - sellingPrice;
            System.out.println("Loss = " + loss);
        } else {
            System.out.println("No profit and no loss");
        }
        
        scanner.close();
    }
}
