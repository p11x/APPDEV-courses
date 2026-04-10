/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : NUnit Part 2 - Advanced
 * FILE      : NUnit_Part2.cs
 * PURPOSE   : Advanced NUnit testing techniques
 * ============================================================
 */
using System; // Core System namespace
using NUnit.Framework; // NUnit framework

namespace CSharp_MasterGuide._14_Testing._01_UnitTesting
{
    /// <summary>
    /// Advanced NUnit testing
    /// </summary>
    public class NUnitPart2Demo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== NUnit Part 2 ===\n");

            // Output: --- Parameterized Tests ---
            Console.WriteLine("--- Parameterized Tests ---");

            var calc = new AdvancedCalculator();
            Assert.That(calc.Add(2, 3), Is.EqualTo(5));
            Assert.That(calc.Add(-1, 1), Is.EqualTo(0));
            Console.WriteLine("   Parameterized tests passed");
            // Output: Parameterized tests passed

            // Output: --- Range Tests ---
            Console.WriteLine("\n--- Range Tests ---");

            var list = new[] { 1, 2, 3, 4, 5 };
            Assert.That(list, Has.All.GreaterThan(0));
            Assert.That(list, Has.None.EqualTo(0));
            Console.WriteLine("   Range tests passed");
            // Output: Range tests passed

            // Output: --- Async Tests ---
            Console.WriteLine("\n--- Async Tests ---");

            var async = new AsyncService();
            var result = async.GetDataAsync().Result;
            Console.WriteLine($"   Async: {result}");
            // Output: Async: data

            // Output: --- Setup/Teardown ---
            Console.WriteLine("\n--- Setup/Teardown ---");

            var service = new TestService();
            service.Setup();
            service.Test();
            service.Teardown();
            Console.WriteLine("   Lifecycle tests passed");
            // Output: Lifecycle tests passed

            Console.WriteLine("\n=== NUnit Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Advanced calculator
    /// </summary>
    public class AdvancedCalculator
    {
        public int Add(int a, int b) => a + b;
    }

    /// <summary>
    /// Async service
    /// </summary>
    public class AsyncService
    {
        public async System.Threading.Tasks.Task<string> GetDataAsync()
        {
            await System.Threading.Tasks.Task.Delay(1);
            return "data";
        }
    }

    /// <summary>
    /// Test service
    /// </summary>
    public class TestService
    {
        [SetUp]
        public void Setup() => Console.WriteLine("   Setup");

        [TearDown]
        public void Teardown() => Console.WriteLine("   Teardown");

        [Test]
        public void Test() => Console.WriteLine("   Test");
    }

    /// <summary>
    /// Parameterized test case
    /// </summary>
    public class ParameterizedTests
    {
        [TestCase(2, 3, 5)]
        [TestCase(-1, 1, 0)]
        [TestCase(0, 0, 0)]
        public void Add_TwoNumbers_ReturnsSum(int a, int b, int expected)
        {
            var calc = new AdvancedCalculator();
            Assert.That(calc.Add(a, b), Is.EqualTo(expected));
        }
    }
}