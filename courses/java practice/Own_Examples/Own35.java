import java.util.Scanner;

// Simple Maze Navigator (Grid-based)
public class Own35 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Simple Maze Navigator ===");
        System.out.println();
        
        // Define 5x5 grid (0 = path, 1 = blocked)
        int[][] maze = {
            {0, 0, 1, 0, 0},
            {0, 1, 1, 0, 1},
            {0, 0, 0, 0, 0},
            {0, 1, 0, 1, 0},
            {0, 0, 0, 0, 0}
        };
        
        // Starting position
        int playerRow = 0;
        int playerCol = 0;
        
        // Goal position
        int goalRow = 4;
        int goalCol = 4;
        
        System.out.println("Maze Legend: 0 = Path, 1 = Blocked");
        System.out.println("Start: (0,0), Goal: (4,4)");
        System.out.println();
        
        // Display initial maze
        System.out.println("=== Initial Maze ===");
        displayMaze(maze, playerRow, playerCol);
        
        // Game loop
        boolean reachedGoal = false;
        int moves = 0;
        
        while (!reachedGoal) {
            System.out.println();
            System.out.print("Enter move (U=up, D=down, L=left, R=right, Q=quit): ");
            char move = scanner.next().toUpperCase().charAt(0);
            
            int newRow = playerRow;
            int newCol = playerCol;
            
            if (move == 'Q') {
                System.out.println("You quit the game!");
                break;
            }
            
            // Calculate new position
            if (move == 'U') {
                newRow--;
            } else if (move == 'D') {
                newRow++;
            } else if (move == 'L') {
                newCol--;
            } else if (move == 'R') {
                newCol++;
            } else {
                System.out.println("Invalid move! Use U, D, L, R, or Q.");
                continue;
            }
            
            // Check boundaries
            if (newRow < 0 || newRow > 4 || newCol < 0 || newCol > 4) {
                System.out.println("Cannot move! Out of boundaries.");
                continue;
            }
            
            // Check for blocked cell
            if (maze[newRow][newCol] == 1) {
                System.out.println("Cannot move! Path is blocked.");
                continue;
            }
            
            // Move player
            playerRow = newRow;
            playerCol = newCol;
            moves++;
            
            // Display updated maze
            System.out.println();
            displayMaze(maze, playerRow, playerCol);
            
            // Check if goal reached
            if (playerRow == goalRow && playerCol == goalCol) {
                reachedGoal = true;
                System.out.println();
                System.out.println("You reached the goal!");
                System.out.println("Total moves: " + moves);
            }
        }
        
        scanner.close();
    }
    
    // Method to display maze with player position
    public static void displayMaze(int[][] maze, int playerRow, int playerCol) {
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 5; j++) {
                if (i == playerRow && j == playerCol) {
                    System.out.print("P ");
                } else if (maze[i][j] == 1) {
                    System.out.print("# ");
                } else {
                    System.out.print(". ");
                }
            }
            System.out.println();
        }
    }
}
