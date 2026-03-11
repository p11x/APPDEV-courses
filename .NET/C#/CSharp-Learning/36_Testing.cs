/*
================================================================================
TOPIC 36: UNIT TESTING IN C#
================================================================================

Unit testing is essential for maintaining code quality.

TABLE OF CONTENTS:
1. What is Unit Testing?
2. xUnit Framework
3. NUnit Framework
4. Mocking
5. Test-Driven Development
================================================================================
*/

using System;

namespace TestingExamples
{
    // ====================================================================
    // CODE TO TEST
    // ====================================================================
    
    public class Calculator
    {
        public int Add(int a, int b) => a + b;
        public int Subtract(int a, int b) => a - b;
        public int Multiply(int a, int b) => a * b;
        
        public int Divide(int a, int b)
        {
            if (b == 0)
                throw new DivideByZeroException("Cannot divide by zero");
            return a / b;
        }
    }
    
    // ====================================================================
    // XUNIT TEST EXAMPLE
    // ====================================================================
    
    // Install: xunit, xunit.runner.visualstudio
    /*
    public class CalculatorTests
    {
        [Fact]
        public void Add_ShouldReturnSum()
        {
            // Arrange
            var calc = new Calculator();
            
            // Act
            int result = calc.Add(2, 3);
            
            // Assert
            Assert.Equal(5, result);
        }
        
        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(10, 20, 30)]
        [InlineData(-1, 1, 0)]
        public void Add_ShouldReturnCorrectSum(int a, int b, int expected)
        {
            var calc = new Calculator();
            Assert.Equal(expected, calc.Add(a, b));
        }
        
        [Fact]
        public void Divide_ByZero_ShouldThrowException()
        {
            var calc = new Calculator();
            Assert.Throws<DivideByZeroException>(() => calc.Divide(10, 0));
        }
    }
    */
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Unit Testing ===");
            
            Console.WriteLine("\nTesting Frameworks:");
            Console.WriteLine("xUnit - Modern, popular");
            NUnit - Mature feature-rich");
            Console.WriteLine("MSTest - Microsoft built-in");
            
            Console.WriteLine("\nTest Attributes:");
            Console.WriteLine("[Fact] - Single test case");
            Console.WriteLine("[Theory] - Multiple test cases");
            Console.WriteLine("[InlineData] - Test data");
            
            Console.WriteLine("\nNuGet Packages:");
            Console.WriteLine("xunit + xunit.runner.visualstudio");
            Console.WriteLine("Moq - Mocking framework");
            Console.WriteLine("FluentAssertions - Better assertions");
        }
    }
}

/*
TESTING PRINCIPLES:
-------------------
AAA Pattern:
1. Arrange - Set up test data
2. Act - Execute the method
3. Assert - Verify the result

TYPES OF TESTS:
---------------
- Unit Tests - Test single methods/classes
- Integration Tests - Test multiple components
- End-to-End Tests - Test entire application
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 37 covers SignalR.
*/
