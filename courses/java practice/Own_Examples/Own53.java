// Pattern Collection
public class Own53 {
    public static void main(String[] args) {
        System.out.println("=== Pattern Collection ===");
        System.out.println();
        
        // Pattern 1: Hollow Square
        System.out.println("Pattern 1: Hollow Square");
        int size1 = 5;
        for (int i = 1; i <= size1; i++) {
            for (int j = 1; j <= size1; j++) {
                if (i == 1 || i == size1 || j == 1 || j == size1) {
                    System.out.print("* ");
                } else {
                    System.out.print("  ");
                }
            }
            System.out.println();
        }
        
        System.out.println();
        
        // Pattern 2: Hollow Triangle
        System.out.println("Pattern 2: Hollow Triangle");
        int size2 = 5;
        for (int i = 1; i <= size2; i++) {
            for (int j = 1; j <= i; j++) {
                if (j == 1 || j == i || i == size2) {
                    System.out.print("* ");
                } else {
                    System.out.print("  ");
                }
            }
            System.out.println();
        }
        
        System.out.println();
        
        // Pattern 3: Butterfly pattern
        System.out.println("Pattern 3: Butterfly Pattern");
        int size3 = 4;
        for (int i = 1; i <= size3; i++) {
            // Upper half
            for (int j = 1; j <= i; j++) {
                System.out.print("* ");
            }
            for (int j = 1; j <= 2 * (size3 - i); j++) {
                System.out.print("  ");
            }
            for (int j = 1; j <= i; j++) {
                System.out.print("* ");
            }
            System.out.println();
        }
        for (int i = size3; i >= 1; i--) {
            // Lower half
            for (int j = 1; j <= i; j++) {
                System.out.print("* ");
            }
            for (int j = 1; j <= 2 * (size3 - i); j++) {
                System.out.print("  ");
            }
            for (int j = 1; j <= i; j++) {
                System.out.print("* ");
            }
            System.out.println();
        }
        
        System.out.println();
        
        // Pattern 4: Zigzag pattern
        System.out.println("Pattern 4: Zigzag Pattern");
        int size4 = 9;
        for (int i = 1; i <= 3; i++) {
            for (int j = 1; j <= size4; j++) {
                if ((i + j) % 4 == 0 || (i == 2 && j % 4 == 0)) {
                    System.out.print("* ");
                } else {
                    System.out.print("  ");
                }
            }
            System.out.println();
        }
        
        System.out.println();
        
        // Pattern 5: Number Diamond
        System.out.println("Pattern 5: Number Diamond");
        int size5 = 4;
        // Upper half
        for (int i = 1; i <= size5; i++) {
            for (int j = 1; j <= size5 - i; j++) {
                System.out.print(" ");
            }
            for (int j = 1; j <= i; j++) {
                System.out.print(j + " ");
            }
            for (int j = i - 1; j >= 1; j--) {
                System.out.print(j + " ");
            }
            System.out.println();
        }
        // Lower half
        for (int i = size5 - 1; i >= 1; i--) {
            for (int j = 1; j <= size5 - i; j++) {
                System.out.print(" ");
            }
            for (int j = 1; j <= i; j++) {
                System.out.print(j + " ");
            }
            for (int j = i - 1; j >= 1; j--) {
                System.out.print(j + " ");
            }
            System.out.println();
        }
    }
}
