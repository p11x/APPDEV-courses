/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : NUnit - Part 1
 * FILE      : NUnit.cs
 * PURPOSE   : Introduction to NUnit testing framework
 * ============================================================
 */
using System; // Core System namespace
using NUnit.Framework; // NUnit framework

namespace CSharp_MasterGuide._14_Testing._01_UnitTesting
{
    /// <summary>
    /// NUnit testing examples
    /// </summary>
    public class NUnitDemo
    {
        /// <summary>
        /// Entry point for NUnit demo
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== NUnit Testing ===\n");

            // Output: --- Basic Tests ---
            Console.WriteLine("--- Basic Tests ---");

            var math = new MathUtils();
            Assert.That(math.Add(2, 3), Is.EqualTo(5));
            Console.WriteLine("   Add test passed");
            // Output: Add test passed

            Console.WriteLine("   Multiply test passed");
            Assert.That(math.Multiply(3, 4), Is.EqualTo(12));
            // Output: Multiply test passed

            // Output: --- String Tests ---
            Console.WriteLine("\n--- String Tests ---");

            var result = math.Reverse("hello");
            Assert.That(result, Is.EqualTo("olleh").IgnoreCase);
            Console.WriteLine($"   Reverse: {result}");
            // Output: Reverse: olleh

            // Output: --- Collection Tests ---
            Console.WriteLine("\n--- Collection Tests ---");

            var list = new System.Collections.Generic.List<int> { 1, 2, 3 };
            Assert.That(list, Is.Not.Empty);
            Assert.That(list, Has.Count.EqualTo(3));
            Console.WriteLine($"   Collection tests passed");
            // Output: Collection tests passed

            // Output: --- Exception Tests ---
            Console.WriteLine("\n--- Exception Tests ---");

            Assert.Throws<DivideByZeroException>(() => math.Divide(1, 0));
            Console.WriteLine("   Exception test passed");
            // Output: Exception test passed

            Console.WriteLine("\n=== NUnit Complete ===");
        }
    }

    /// <summary>
    /// Math utilities for testing
    /// </summary>
    public class MathUtils
    {
        public int Add(int a, int b) => a + b;
        public int Multiply(int a, int b) => a * b;
        public string Reverse(string s) => new string(s.ToCharArray().Reverse().ToArray());
        public int Divide(int a, int b) => a / b;
    }

    /// <summary>
    /// NUnit test fixture
    /// </summary>
    [TestFixture]
    public class MathUtilsTests
    {
        private MathUtils _math;

        [SetUp]
        public void Setup()
        {
            _math = new MathUtils();
        }

        [Test]
        public void Add_TwoNumbers_ReturnsSum()
        {
            Assert.That(_math.Add(2, 3), Is.EqualTo(5));
        }

        [Test]
        public void Multiply_TwoNumbers_ReturnsProduct()
        {
            Assert.That(_math.Multiply(3, 4), Is.EqualTo(12));
        }
    }
}