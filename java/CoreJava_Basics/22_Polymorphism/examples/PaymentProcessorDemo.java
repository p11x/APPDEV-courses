// PaymentProcessorDemo - Demonstrates Polymorphism for Payment Processing
// Use case: Multiple payment methods in Angular e-commerce app

public class PaymentProcessorDemo {
    
    // Base payment class
    static abstract class Payment {
        protected double amount;
        
        public Payment(double amount) {
            this.amount = amount;
        }
        
        // Abstract method - must be implemented
        public abstract boolean process();
        
        // Concrete method
        public double getAmount() {
            return amount;
        }
    }
    
    // Credit card payment
    static class CreditCardPayment extends Payment {
        private String cardNumber;
        
        public CreditCardPayment(double amount, String cardNumber) {
            super(amount);
            this.cardNumber = cardNumber;
        }
        
        @Override
        public boolean process() {
            System.out.println("Processing Credit Card: $" + amount);
            System.out.println("Card: ****" + cardNumber.substring(cardNumber.length() - 4));
            return true;
        }
    }
    
    // PayPal payment
    static class PayPalPayment extends Payment {
        private String email;
        
        public PayPalPayment(double amount, String email) {
            super(amount);
            this.email = email;
        }
        
        @Override
        public boolean process() {
            System.out.println("Processing PayPal: $" + amount);
            System.out.println("Email: " + email);
            return true;
        }
    }
    
    // Crypto payment
    static class CryptoPayment extends Payment {
        private String walletAddress;
        
        public CryptoPayment(double amount, String walletAddress) {
            super(amount);
            this.walletAddress = walletAddress;
        }
        
        @Override
        public boolean process() {
            System.out.println("Processing Crypto: $" + amount);
            System.out.println("Wallet: " + walletAddress.substring(0, 6) + "...");
            return true;
        }
    }
    
    // Payment processor - demonstrates polymorphism
    public static boolean processPayment(Payment payment) {
        System.out.println("\n--- Processing Payment ---");
        return payment.process();
    }
    
    // Process multiple payments
    public static void processAll(Payment[] payments) {
        System.out.println("\n=== PROCESSING MULTIPLE PAYMENTS ===");
        for (Payment p : payments) {
            processPayment(p);
            System.out.println("Status: Success\n");
        }
    }
    
    public static void main(String[] args) {
        System.out.println("=== POLYMORPHISM IN PAYMENT PROCESSING ===\n");
        
        // Create different payment types
        Payment[] payments = new Payment[3];
        payments[0] = new CreditCardPayment(99.99, "4111111111111111");
        payments[1] = new PayPalPayment(49.99, "user@email.com");
        payments[2] = new CryptoPayment(0.005, "0x1234567890abcdef");
        
        // Process all with polymorphism
        processAll(payments);
        
        // Direct processing
        System.out.println("=== DIRECT POLYMORPHISM ===");
        Payment card = new CreditCardPayment(150.00, "5555555555554444");
        Payment paypal = new PayPalPayment(75.00, "paypal@user.com");
        
        processPayment(card);
        processPayment(paypal);
        
        System.out.println("\n=== ANGULAR USE CASES ===");
        System.out.println("1. Multiple payment options (credit, PayPal, crypto)");
        System.out.println("2. Different shipping methods");
        System.out.println("3. Various discount types");
        System.out.println("4. Multiple notification channels");
        System.out.println("5. Dynamic component rendering");
    }
}
