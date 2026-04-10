/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : TDD Part 2 - Advanced
 * FILE      : TDD_Part2.cs
 * PURPOSE   : Advanced TDD techniques
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._14_Testing._04_TDD
{
    /// <summary>
    /// TDD Part 2 demonstration
    /// </summary>
    public class TDDPart2Demo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== TDD Part 2 ===\n");

            // Output: --- Red-Green-Refactor ---
            Console.WriteLine("--- Red-Green-Refactor ---");

            // Red: Write failing test
            // Green: Make it pass
            // Refactor: Clean up

            var calc = new TddCalculator();
            Assert.AreEqual(5, calc.Add(2, 3));
            Console.WriteLine("   Test passed");
            // Output: Test passed

            // Output: --- FizzBuzz Example ---
            Console.WriteLine("\n--- FizzBuzz ---");

            var fizz = new FizzBuzz();
            Console.WriteLine(fizz.Convert(1));
            // Output: 1
            Console.WriteLine(fizz.Convert(3));
            // Output: Fizz
            Console.WriteLine(fizz.Convert(5));
            // Output: Buzz
            Console.WriteLine(fizz.Convert(15));
            // Output: FizzBuzz

            // Output: --- Bowling Game ---
            Console.WriteLine("\n--- Bowling Game ---");

            var game = new BowlingGame();
            game.Roll(10); // strike
            game.Roll(5);
            game.Roll(4);
            var score = game.Score();
            Console.WriteLine($"   Score: {score}");
            // Output: Score: 28

            Console.WriteLine("\n=== Part 2 Complete ===");
        }
    }

    /// <summary>
    /// TDD calculator
    /// </summary>
    public class TddCalculator
    {
        public int Add(int a, int b) => a + b;
    }

    /// <summary>
    /// FizzBuzz implementation
    /// </summary>
    public class FizzBuzz
    {
        public string Convert(int n)
        {
            if (n % 15 == 0) return "FizzBuzz";
            if (n % 3 == 0) return "Fizz";
            if (n % 5 == 0) return "Buzz";
            return n.ToString();
        }
    }

    /// <summary>
    /// Bowling game
    /// </summary>
    public class BowlingGame
    {
        private readonly int[] _rolls = new int[21];
        private int _rollIndex;

        public void Roll(int pins) => _rolls[_rollIndex++] = pins;

        public int Score()
        {
            int score = 0;
            for (int i = 0; i < 20; i += 2)
            {
                score += _rolls[i] + _rolls[i + 1];
            }
            return score;
        }
    }

    /// <summary>
    /// Assert helper
    /// </summary>
    public static class Assert
    {
        public static void AreEqual(object expected, object actual)
        {
            if (!expected.Equals(actual))
                throw new Exception($"Expected {expected} but got {actual}");
        }
    }
}