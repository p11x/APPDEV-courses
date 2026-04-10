/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : xUnit Part 2 - Advanced Testing
 * FILE      : xUnit_Part2.cs
 * PURPOSE   : Advanced xUnit testing techniques
 * ============================================================
 */
using System; // Core System namespace
using Xunit; // xUnit framework

namespace CSharp_MasterGuide._14_Testing._01_UnitTesting
{
    /// <summary>
    /// Advanced xUnit testing examples
    /// </summary>
    public class xUnitPart2Demo
    {
        /// <summary>
        /// Entry point for xUnit Part 2
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== xUnit Part 2 ===\n");

            // Output: --- Theory Tests ---
            Console.WriteLine("--- Theory Tests ---");

            // Data-driven tests
            var calculator = new Calculator2();
            var result = calculator.Add(5, 3);
            Console.WriteLine($"   5 + 3 = {result}");
            // Output: 5 + 3 = 8

            // Output: --- Collection Tests ---
            Console.WriteLine("\n--- Collection Tests ---");

            var coll = new TestCollection();
            coll.Add(1);
            coll.Add(2);
            Console.WriteLine($"   Count: {coll.Count}");
            // Output: Count: 2

            // Output: --- Mock Tests ---
            Console.WriteLine("\n--- Mock Tests ---");

            var mockRepo = new MockUserRepository();
            var service = new UserService2(mockRepo);
            service.Create("test");
            // Output: User created: test

            Console.WriteLine("\n=== xUnit Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Calculator for testing
    /// </summary>
    public class Calculator2
    {
        public int Add(int a, int b) => a + b;
        public int Subtract(int a, int b) => a - b;
    }

    /// <summary>
    /// Test collection
    /// </summary>
    public class TestCollection
    {
        private readonly System.Collections.Generic.List<int> _items = new();

        public void Add(int item) => _items.Add(item);
        public int Count => _items.Count;
    }

    /// <summary>
    /// User repository interface
    /// </summary>
    public interface IUserRepository2
    {
        void Add(string user); // method: add user
    }

    /// <summary>
    /// Mock repository for testing
    /// </summary>
    public class MockUserRepository : IUserRepository2
    {
        public void Add(string user) => Console.WriteLine($"   User created: {user}");
    }

    /// <summary>
    /// User service for testing
    /// </summary>
    public class UserService2
    {
        private readonly IUserRepository2 _repository;

        public UserService2(IUserRepository2 repository)
        {
            _repository = repository;
        }

        public void Create(string username)
        {
            _repository.Add(username);
        }
    }

    /// <summary>
    /// xUnit theory test example
    /// </summary>
    public class CalculatorTheoryTests
    {
        [Theory]
        [InlineData(2, 2, 4)]
        [InlineData(3, 3, 6)]
        [InlineData(5, 5, 10)]
        public void Add_ShouldReturnCorrectSum(int a, int b, int expected)
        {
            var calc = new Calculator2();
            var result = calc.Add(a, b);
            Assert.Equal(expected, result);
        }
    }
}