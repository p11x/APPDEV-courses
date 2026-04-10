/*
 * ============================================================
 * TOPIC     : Data Structures & Algorithms
 * SUBTOPIC  : Algorithms - Dynamic Programming
 * FILE      : 01_DynamicProgramming.cs
 * PURPOSE   : Dynamic programming basics
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._23_DataStructures_Algorithms._04_Algorithms
{
    /// <summary>
    /// Dynamic programming examples
    /// </summary>
    public class DynamicProgramming
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Dynamic Programming ===\n");

            // Fibonacci with memoization
            var fib = Fibonacci(10);
            Console.WriteLine($"   Fibonacci(10): {fib}");
            
            // Knapsack problem
            var maxValue = Knapsack(new[] { 10, 20, 30 }, new[] { 60, 100, 120 }, 50);
            Console.WriteLine($"   Max value: {maxValue}");

            Console.WriteLine("\n=== DP Complete ===");
        }

        /// <summary>
        /// Fibonacci with memoization - O(n)
        /// </summary>
        static int Fibonacci(int n)
        {
            if (n <= 1) return n;
            var memo = new Dictionary<int, int>();
            return FibonacciDP(n, memo);
        }

        static int FibonacciDP(int n, Dictionary<int, int> memo)
        {
            if (memo.ContainsKey(n)) return memo[n];
            if (n <= 1) return n;
            memo[n] = FibonacciDP(n - 1, memo) + FibonacciDP(n - 2, memo);
            return memo[n];
        }

        /// <summary>
        /// 0/1 Knapsack problem
        /// </summary>
        static int Knapsack(int[] weights, int[] values, int capacity)
        {
            int n = values.Length;
            int[,] dp = new int[n + 1, capacity + 1];
            
            for (int i = 1; i <= n; i++)
            {
                for (int w = 0; w <= capacity; w++)
                {
                    if (weights[i - 1] <= w)
                        dp[i, w] = Math.Max(values[i - 1] + dp[i - 1, w - weights[i - 1]], dp[i - 1, w]);
                    else
                        dp[i, w] = dp[i - 1, w];
                }
            }
            
            return dp[n, capacity];
        }
    }
}