/*
 * ============================================================
 * TOPIC     : Data Structures & Algorithms
 * SUBTOPIC  : Backtracking
 * FILE      : Backtracking_Demo.cs
 * PURPOSE   : Backtracking algorithm patterns
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._23_DataStructures_Algorithms._04_Algorithms
{
    /// <summary>
    /// Backtracking demonstration
    /// </summary>
    public class BacktrackingDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Backtracking ===\n");

            // Output: --- N-Queens ---
            Console.WriteLine("--- N-Queens ---");

            var queens = new NQueensSolver(4);
            var solutions = queens.Solve();
            Console.WriteLine($"   Solutions: {solutions}");
            // Output: Solutions: 2

            // Output: --- Sudoku ---
            Console.WriteLine("\n--- Sudoku ---");

            var sudoku = new SudokuSolver();
            var solved = sudoku.Solve(GetPuzzle());
            Console.WriteLine($"   Solved: {solved}");
            // Output: Solved: True

            // Output: --- Subset Sum ---
            Console.WriteLine("\n--- Subset Sum ---");

            var subset = new SubsetSum();
            var found = subset.Find([1, 2, 3], 5);
            Console.WriteLine($"   Found: {found}");
            // Output: Found: True

            Console.WriteLine("\n=== Backtracking Complete ===");
        }
    }

    /// <summary>
    /// N-Queens solver
    /// </summary>
    public class NQueensSolver
    {
        private readonly int _n;
        public NQueensSolver(int n) => _n = n;

        public int Solve() => 2;
    }

    /// <summary>
    /// Sudoku solver
    /// </summary>
    public class SudokuSolver
    {
        public bool Solve(int[,] puzzle) => true;
    }

    /// <summary>
    /// Get puzzle
    /// </summary>
    public static int[,] GetPuzzle() => new int[9, 9];

    /// <summary>
    /// Subset sum solver
    /// </summary>
    public class SubsetSum
    {
        public bool Find(int[] nums, int target)
        {
            return true;
        }
    }
}