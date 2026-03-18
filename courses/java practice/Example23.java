// Example23: Array of Longs - Beginner Tutorial
// This shows different ways to work with long arrays

public class Example23 {
    public static void main(String[] args) {
        
        // ===== METHOD 1: Create long array with size =====
        System.out.println("=== Method 1: Create with size ===");
        
        long[] numbers = new long[5];
        
        // Default value is 0
        numbers[0] = 100000L;
        numbers[1] = 200000L;
        numbers[2] = 300000L;
        numbers[3] = 400000L;
        numbers[4] = 500000L;
        
        System.out.println("Long values:");
        for (int i = 0; i < numbers.length; i++) {
            System.out.println("  Index " + i + ": " + numbers[i]);
        }
        
        // ===== METHOD 2: Direct initialization =====
        System.out.println("\n=== Method 2: Direct initialization ===");
        
        // Note: Long values need 'L' suffix for clarity
        long[] population = {5000000L, 8000000L, 12000000L, 3000000L, 6000000L};
        
        System.out.print("Population: ");
        for (long p : population) {
            System.out.print(p + " ");
        }
        System.out.println();
        
        // ===== LONG RANGE =====
        System.out.println("\n=== Long Range ===");
        
        // Long ranges from -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
        long minValue = Long.MIN_VALUE;
        long maxValue = Long.MAX_VALUE;
        
        System.out.println("Minimum long value: " + minValue);
        System.out.println("Maximum long value: " + maxValue);
        
        // ===== PRACTICAL: Large Numbers - Population =====
        System.out.println("\n=== Practical: City Populations ===");
        
        long[] cityPopulations = {
            8539000L,   // New York
            3979000L,  // Los Angeles
            2716000L,   // Chicago
            2324000L,  // Houston
            1702000L   // Phoenix
        };
        
        String[] cities = {"New York", "Los Angeles", "Chicago", "Houston", "Phoenix"};
        
        System.out.println("City Populations:");
        for (int i = 0; i < cities.length; i++) {
            System.out.println("  " + cities[i] + ": " + cityPopulations[i]);
        }
        
        // Calculate total population
        long totalPop = 0;
        for (long pop : cityPopulations) {
            totalPop += pop;
        }
        
        System.out.println("Total: " + totalPop);
        
        // Find largest city
        long largest = cityPopulations[0];
        String largestCity = cities[0];
        
        for (int i = 1; i < cityPopulations.length; i++) {
            if (cityPopulations[i] > largest) {
                largest = cityPopulations[i];
                largestCity = cities[i];
            }
        }
        
        System.out.println("Largest city: " + largestCity + " with " + largest);
        
        // ===== PRACTICAL: Financial Data - Bank Balances =====
        System.out.println("\n=== Practical: Bank Balances ===");
        
        long[] accounts = {150000L, 250000L, 50000L, 750000L, 100000L};
        
        System.out.println("Account balances:");
        for (int i = 0; i < accounts.length; i++) {
            System.out.println("  Account " + (i + 1) + ": $" + accounts[i]);
        }
        
        // Sum all accounts
        long totalBalance = 0;
        for (long balance : accounts) {
            totalBalance += balance;
        }
        
        System.out.println("Total in all accounts: $" + totalBalance);
        
        // Average balance
        double averageBalance = (double) totalBalance / accounts.length;
        System.out.println("Average balance: $" + averageBalance);
        
        // ===== PRACTICAL: Time in Milliseconds =====
        System.out.println("\n=== Practical: Time Calculations ===");
        
        // Current timestamps (in milliseconds since 1970)
        long[] timestamps = {
            1609459200000L,  // Jan 1, 2021
            1612137600000L,  // Feb 1, 2021
            1614556800000L,  // Mar 1, 2021
            1617235200000L  // Apr 1, 2021
        };
        
        // Convert to dates
        java.util.Date date = new java.util.Date();
        
        System.out.println("Timestamps and dates:");
        for (long ts : timestamps) {
            date.setTime(ts);
            System.out.println("  " + ts + " -> " + date);
        }
        
        // ===== PRACTICAL: Distance in Meters =====
        System.out.println("\n=== Practical: Distances (Meters) ===");
        
        // Good for very large distances
        long[] distances = {
            5000L,      // 5 km in meters
            10000L,     // 10 km
            2500L,     // 2.5 km
            7500L,     // 7.5 km
            15000L     // 15 km
        };
        
        System.out.print("Distances in meters: ");
        for (long d : distances) {
            System.out.print(d + "m ");
        }
        System.out.println();
        
        // Convert to kilometers
        System.out.print("Same in km: ");
        for (long d : distances) {
            System.out.print((d / 1000.0) + "km ");
        }
        System.out.println();
        
        // ===== PRACTICAL: Factorial Numbers =====
        System.out.println("\n=== Practical: Factorial Numbers ===");
        
        long[] factorials = new long[10];
        
        factorials[0] = 1;  // 0! = 1
        
        for (int i = 1; i < factorials.length; i++) {
            factorials[i] = factorials[i - 1] * i;
        }
        
        System.out.println("Factorials (0! to 9!):");
        for (int i = 0; i < factorials.length; i++) {
            System.out.println("  " + i + "! = " + factorials[i]);
        }
        
        // ===== PRACTICAL: Product IDs =====
        System.out.println("\n=== Practical: Product IDs ===");
        
        // Product IDs can be very long numbers
        long[] productIds = {
            1000000000001L,
            1000000000002L,
            1000000000003L,
            2000000000001L,
            2000000000002L
        };
        
        System.out.println("Product IDs:");
        for (long id : productIds) {
            System.out.println("  " + id);
        }
        
        // ===== CONVERT LONG TO OTHER TYPES =====
        System.out.println("\n=== Convert Long to Other Types ===");
        
        long bigNumber = 1234567890L;
        
        // Long to String
        String stringValue = Long.toString(bigNumber);
        System.out.println("Long to String: " + bigNumber + " -> \"" + stringValue + "\"");
        
        // Long to int (narrowing - may lose data)
        long smallLong = 1000L;
        int intValue = (int) smallLong;
        System.out.println("Long to int: " + smallLong + " -> " + intValue);
        
        // Long to double
        double doubleValue = bigNumber;
        System.out.println("Long to double: " + bigNumber + " -> " + doubleValue);
        
        // ===== CONVERT TO LONG =====
        System.out.println("\n=== Convert to Long ===");
        
        // String to long
        String strNum = "9999999999";
        long longFromString = Long.parseLong(strNum);
        System.out.println("String to long: \"" + strNum + "\" -> " + longFromString);
        
        // Int to long (widening)
        int intNum = 500;
        long longFromInt = intNum;
        System.out.println("Int to long: " + intNum + " -> " + longFromInt);
        
        // ===== WHY USE LONG? =====
        System.out.println("\n=== Why Use Long? ===");
        
        // Perfect for very large numbers
        System.out.println("Long is used for:");
        System.out.println("  - Population numbers");
        System.out.println("  - Financial amounts");
        System.out.println("  - Timestamps (milliseconds)");
        System.out.println("  - Very large counts");
        System.out.println("  - Unique IDs");
        
        // Memory comparison
        long[] longArray = new long[1000];
        int[] intArray = new int[1000];
        
        System.out.println("\nMemory comparison for 1000 elements:");
        System.out.println("  long array: " + longArray.length * 8 + " bytes");
        System.out.println("  int array: " + intArray.length * 4 + " bytes");
        System.out.println("  Long uses: " + (longArray.length * 8 / (double)(intArray.length * 4)) + "x more memory");
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. DECLARING LONG ARRAYS:
 *    long[] array = new long[size];
 *    long[] array = {1000L, 2000L, 3000L};
 * 
 * 2. LONG RANGE:
 *    - Very large: -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
 *    - Takes 8 bytes of memory (64 bits)
 *    - Use 'L' suffix for long literals
 * 
 * 3. WHY USE LONG?
 *    - For very large numbers that int can't hold
 *    - Perfect for:
 *      * Population counts
 *      * Financial calculations
 *      * Timestamps (milliseconds since 1970)
 *      * Large IDs
 *      * Distances in small units
 *      * Scientific calculations
 * 
 * 4. IMPORTANT NOTES:
 *    - Default value is 0L
 *    - Use 'L' suffix to avoid ambiguity
 *    - Can store any int value safely
 *    - Use when int overflows
 * 
 * 5. CONVERSIONS:
 *    - long to int: requires casting (may lose data)
 *    - long to String: Long.toString(long)
 *    - String to long: Long.parseLong(string)
 *    - int to long: automatic widening
 * 
 * 6. TIPS:
 *    - Don't use long if int suffices (saves memory)
 *    - Always use 'L' suffix for long literals
 *    - Be careful when converting long to int
 *    - Common in database IDs and timestamps
 */
