/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Decorator Pattern
 * FILE      : 01_DecoratorPattern.cs
 * PURPOSE   : Demonstrates Decorator design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._02_Decorator
{
    /// <summary>
    /// Demonstrates Decorator pattern
    /// </summary>
    public class DecoratorPattern
    {
        /// <summary>
        /// Entry point for Decorator pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Decorator Pattern ===
            Console.WriteLine("=== Decorator Pattern ===\n");

            // ── CONCEPT: What is Decorator? ───────────────────────────────────
            // Attaches additional responsibilities dynamically

            // Example 1: Basic Decorator
            // Output: 1. Basic Decorator:
            Console.WriteLine("1. Basic Decorator:");
            
            // Base component
            var coffee = new SimpleCoffee();
            // Output: Coffee: Simple Coffee, Cost: $2.00
            Console.WriteLine($"   Coffee: {coffee.GetDescription()}, Cost: ${coffee.GetCost():F2}");
            
            // Add milk decorator
            var coffeeWithMilk = new MilkDecorator(coffee);
            // Output: Coffee: Simple Coffee, Milk, Cost: $2.50
            Console.WriteLine($"   Coffee: {coffeeWithMilk.GetDescription()}, Cost: ${coffeeWithMilk.GetCost():F2}");
            
            // Add sugar decorator on top of milk
            var coffeeWithMilkAndSugar = new SugarDecorator(coffeeWithMilk);
            // Output: Coffee: Simple Coffee, Milk, Sugar, Cost: $2.75
            Console.WriteLine($"   Coffee: {coffeeWithMilkAndSugar.GetDescription()}, Cost: ${coffeeWithMilkAndSugar.GetCost():F2}");

            // ── CONCEPT: Multiple Decorators ──────────────────────────────────
            // Stack decorators in any order

            // Example 2: Multiple Decorators
            // Output: 2. Multiple Decorators:
            Console.WriteLine("\n2. Multiple Decorators:");
            
            // Start fresh
            var espresso = new Espresso();
            
            // Add whipped cream
            var whippedEspresso = new WhippedCreamDecorator(espresso);
            // Add chocolate
            var chocolateEspresso = new ChocolateDecorator(whippedEspresso);
            // Add caramel
            var caramelEspresso = new CaramelDecorator(chocolateEspresso);
            
            // Output: Espresso: Espresso, Whipped Cream, Chocolate, Caramel, Cost: $5.25
            Console.WriteLine($"   Espresso: {caramelEspresso.GetDescription()}, Cost: ${caramelEspresso.GetCost():F2}");

            // ── CONCEPT: Transparent Decoration ───────────────────────────────
            // Decorators can be used interchangeably with component

            // Example 3: Transparent Decoration
            // Output: 3. Transparent Decoration:
            Console.WriteLine("\n3. Transparent Decoration:");
            
            // All are ICoffee - can use in collections
            ICoffee[] coffees = { 
                new SimpleCoffee(), 
                new MilkDecorator(new SimpleCoffee()),
                new ChocolateDecorator(new Espresso())
            };
            
            // Output: Costs: $2.00, $2.50, $3.75
            Console.Write("   Costs: ");
            foreach (var c in coffees)
            {
                Console.Write($"${c.GetCost():F2}, ");
            }
            Console.WriteLine();

            // ── REAL-WORLD EXAMPLE: Stream Decorators ───────────────────────
            // Output: --- Real-World: Stream Decorators ---
            Console.WriteLine("\n--- Real-World: Stream Decorators ---");
            
            // BufferDecorator wraps stream for performance
            // EncryptionDecorator encrypts data
            // CompressionDecorator compresses data
            
            // In real scenario:
            // var secureStream = new EncryptionDecorator(new BufferDecorator(new FileStream(...)));
            Console.WriteLine("   Stream decorators can be chained: Buffer -> Encryption -> Compression");

            Console.WriteLine("\n=== Decorator Pattern Complete ===");
        }
    }

    /// <summary>
    /// Component interface
    /// </summary>
    public interface ICoffee
    {
        string GetDescription(); // method: returns description
        decimal GetCost(); // method: returns cost
    }

    /// <summary>
    /// Simple coffee - base component
    /// </summary>
    public class SimpleCoffee : ICoffee
    {
        public string GetDescription() => "Simple Coffee";
        public decimal GetCost() => 2.00m;
    }

    /// <summary>
    /// Espresso - another component
    /// </summary>
    public class Espresso : ICoffee
    {
        public string GetDescription() => "Espresso";
        public decimal GetCost() => 3.00m;
    }

    /// <summary>
    /// Base decorator
    /// </summary>
    public abstract class CoffeeDecorator : ICoffee
    {
        protected ICoffee _coffee; // wraps decorated coffee
        
        public CoffeeDecorator(ICoffee coffee)
        {
            _coffee = coffee;
        }
        
        public virtual string GetDescription() => _coffee.GetDescription();
        public virtual decimal GetCost() => _coffee.GetCost();
    }

    /// <summary>
    /// Milk decorator
    /// </summary>
    public class MilkDecorator : CoffeeDecorator
    {
        public MilkDecorator(ICoffee coffee) : base(coffee) { }
        
        public override string GetDescription()
        {
            return _coffee.GetDescription() + ", Milk";
        }
        
        public override decimal GetCost()
        {
            return _coffee.GetCost() + 0.50m;
        }
    }

    /// <summary>
    /// Sugar decorator
    /// </summary>
    public class SugarDecorator : CoffeeDecorator
    {
        public SugarDecorator(ICoffee coffee) : base(coffee) { }
        
        public override string GetDescription()
        {
            return _coffee.GetDescription() + ", Sugar";
        }
        
        public override decimal GetCost()
        {
            return _coffee.GetCost() + 0.25m;
        }
    }

    /// <summary>
    /// Whipped cream decorator
    /// </summary>
    public class WhippedCreamDecorator : CoffeeDecorator
    {
        public WhippedCreamDecorator(ICoffee coffee) : base(coffee) { }
        
        public override string GetDescription()
        {
            return _coffee.GetDescription() + ", Whipped Cream";
        }
        
        public override decimal GetCost()
        {
            return _coffee.GetCost() + 0.75m;
        }
    }

    /// <summary>
    /// Chocolate decorator
    /// </summary>
    public class ChocolateDecorator : CoffeeDecorator
    {
        public ChocolateDecorator(ICoffee coffee) : base(coffee) { }
        
        public override string GetDescription()
        {
            return _coffee.GetDescription() + ", Chocolate";
        }
        
        public override decimal GetCost()
        {
            return _coffee.GetCost() + 0.75m;
        }
    }

    /// <summary>
    /// Caramel decorator
    /// </summary>
    public class CaramelDecorator : CoffeeDecorator
    {
        public CaramelDecorator(ICoffee coffee) : base(coffee) { }
        
        public override string GetDescription()
        {
            return _coffee.GetDescription() + ", Caramel";
        }
        
        public override decimal GetCost()
        {
            return _coffee.GetCost() + 0.50m;
        }
    }
}