// Sudoku Row/Column Validator
public class Own56 {
    public static void main(String[] args) {
        System.out.println("=== Sudoku Row/Column Validator ===");
        System.out.println();
        
        // Hardcoded sample Sudoku grid (valid for testing)
        int[][] sudoku = {
            {5, 3, 4, 6, 7, 8, 9, 1, 2},
            {6, 7, 2, 1, 9, 5, 3, 4, 8},
            {1, 9, 8, 3, 4, 2, 5, 6, 7},
            {8, 5, 9, 7, 6, 1, 4, 2, 3},
            {4, 2, 6, 8, 5, 3, 7, 9, 1},
            {7, 1, 3, 9, 2, 4, 8, 5, 6},
            {9, 6, 1, 5, 3, 7, 2, 8, 4},
            {2, 8, 7, 4, 1, 9, 6, 3, 5},
            {3, 4, 5, 2, 8, 6, 1, 7, 9}
        };
        
        System.out.println("Sample Sudoku Grid:");
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                System.out.print(sudoku[i][j] + " ");
            }
            System.out.println();
        }
        
        boolean isValid = true;
        
        // Validate rows
        System.out.println();
        System.out.println("=== Validating Rows ===");
        for (int i = 0; i < 9; i++) {
            boolean[] seen = new boolean[10];
            for (int j = 0; j < 9; j++) {
                int num = sudoku[i][j];
                if (seen[num]) {
                    System.out.println("Row " + (i + 1) + " has duplicate: " + num);
                    isValid = false;
                }
                seen[num] = true;
            }
            System.out.println("Row " + (i + 1) + ": OK");
        }
        
        // Validate columns
        System.out.println();
        System.out.println("=== Validating Columns ===");
        for (int j = 0; j < 9; j++) {
            boolean[] seen = new boolean[10];
            for (int i = 0; i < 9; i++) {
                int num = sudoku[i][j];
                if (seen[num]) {
                    System.out.println("Column " + (j + 1) + " has duplicate: " + num);
                    isValid = false;
                }
                seen[num] = true;
            }
            System.out.println("Column " + (j + 1) + ": OK");
        }
        
        // Validate 3x3 boxes
        System.out.println();
        System.out.println("=== Validating 3x3 Boxes ===");
        for (int boxRow = 0; boxRow < 3; boxRow++) {
            for (int boxCol = 0; boxCol < 3; boxCol++) {
                boolean[] seen = new boolean[10];
                for (int i = 0; i < 3; i++) {
                    for (int j = 0; j < 3; j++) {
                        int num = sudoku[boxRow * 3 + i][boxCol * 3 + j];
                        if (seen[num]) {
                            System.out.println("Box (" + boxRow + "," + boxCol + ") has duplicate: " + num);
                            isValid = false;
                        }
                        seen[num] = true;
                    }
                }
                System.out.println("Box (" + (boxRow + 1) + "," + (boxCol + 1) + "): OK");
            }
        }
        
        System.out.println();
        if (isValid) {
            System.out.println("Result: Valid Sudoku!");
        } else {
            System.out.println("Result: Invalid Sudoku!");
        }
    }
}
