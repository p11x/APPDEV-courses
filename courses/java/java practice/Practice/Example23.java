/*
 * SUB TOPIC: Long Array Operations
 * 
 * DEFINITION:
 * A long array stores large integer values from -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807.
 * It uses 8 bytes of memory per element and is used for very large numbers like population counts,
 * timestamps, and financial data.
 * 
 * FUNCTIONALITIES:
 * 1. Long array declaration and initialization
 * 2. Long range and limitations
 * 3. Type conversions
 * 4. Large number operations
 * 5. Practical applications
 */

public class Example23 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Long Array
        System.out.println("=== Long Array ===");
        
        long[] numbers = new long[5]; // Default value is 0L
        numbers[0] = 100000L;
        numbers[1] = 200000L;
        numbers[2] = 300000L;
        numbers[3] = 400000L;
        numbers[4] = 500000L;
        
        for (int i = 0; i < numbers.length; i++) {
            System.out.println("numbers[" + i + "] = " + numbers[i]);
        }
        
        // Real-time Example 1: Long Range
        System.out.println("\n=== Long Range ===");
        
        long minValue = Long.MIN_VALUE;
        long maxValue = Long.MAX_VALUE;
        
        System.out.println("Minimum: " + minValue);
        System.out.println("Maximum: " + maxValue);
        
        // Real-time Example 2: City Populations
        System.out.println("\n=== City Populations ===");
        
        long[] populations = {8539000L, 3979000L, 2716000L, 2324000L, 1702000L};
        String[] cities = {"New York", "Los Angeles", "Chicago", "Houston", "Phoenix"};
        
        long totalPop = 0;
        for (long pop : populations) {
            totalPop += pop;
        }
        
        for (int i = 0; i < cities.length; i++) {
            System.out.println(cities[i] + ": " + populations[i]);
        }
        
        System.out.println("Total: " + totalPop);
        
        // Real-time Example 3: Bank Balances
        System.out.println("\n=== Bank Balances ===");
        
        long[] accounts = {150000L, 250000L, 50000L, 750000L, 100000L};
        
        long totalBalance = 0;
        for (long balance : accounts) {
            totalBalance += balance;
        }
        
        System.out.println("Total Balance: $" + totalBalance);
        
        // Real-time Example 4: Timestamps
        System.out.println("\n=== Timestamps ===");
        
        long[] timestamps = {
            1609459200000L,
            1612137600000L,
            1614556800000L
        };
        
        java.util.Date date = new java.util.Date();
        
        for (long ts : timestamps) {
            date.setTime(ts);
            System.out.println("Timestamp: " + ts + " -> " + date);
        }
        
        // Real-time Example 5: Factorial Numbers
        System.out.println("\n=== Factorial Numbers ===");
        
        long[] factorials = new long[10];
        factorials[0] = 1;
        
        for (int i = 1; i < factorials.length; i++) {
            factorials[i] = factorials[i - 1] * i;
        }
        
        for (int i = 0; i < factorials.length; i++) {
            System.out.println(i + "! = " + factorials[i]);
        }
        
        // Real-time Example 6: Type Conversions
        System.out.println("\n=== Type Conversions ===");
        
        long bigNumber = 1234567890L;
        
        String stringValue = Long.toString(bigNumber); // long to String
        int intValue = (int) bigNumber; // long to int (casting)
        double doubleValue = bigNumber; // long to double
        
        System.out.println("Long to String: " + stringValue);
        System.out.println("Long to int: " + intValue);
        System.out.println("Long to double: " + doubleValue);
    }
}
