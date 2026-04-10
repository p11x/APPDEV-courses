/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : Property Patterns
 * FILE      : 01_PropertyPattern.cs
 * PURPOSE   : Demonstrates property patterns in C# for matching object property values
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._03_PropertyPatterns
{
    /// <summary>
    /// Demonstrates property pattern matching in C#
    /// </summary>
    public class PropertyPattern
    {
        /// <summary>
        /// Entry point for property pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Property Pattern Matching Demo ===
            Console.WriteLine("=== Property Pattern Matching Demo ===\n");

            // ── CONCEPT: Basic Property Pattern ──────────────────────────────
            // Property pattern matches on object property values

            // Example 1: Basic property matching
            // Output: 1. Basic Property Patterns:
            Console.WriteLine("1. Basic Property Patterns:");
            
            // Person object with Name and Age properties
            var alice = new Person("Alice", 30, "Engineering");
            var bob = new Person("Bob", 25, "Sales");
            var charlie = new Person("Charlie", 45, "Engineering");
            
            // CheckPersonDepartment returns department info
            Console.WriteLine($"   Alice: {CheckPersonDepartment(alice)}");
            Console.WriteLine($"   Bob: {CheckPersonDepartment(bob)}");
            Console.WriteLine($"   Charlie: {CheckPersonDepartment(charlie)}");

            // ── CONCEPT: Nested Property Patterns ───────────────────────────
            // Can match on nested object properties

            // Example 2: Nested property matching
            // Output: 2. Nested Property Patterns:
            Console.WriteLine("\n2. Nested Property Patterns:");
            
            // Order with nested Customer
            var order1 = new Order("ORD-001", 150.00m, new Customer("Alice", "VIP"));
            var order2 = new Order("ORD-002", 50.00m, new Customer("Bob", "Regular"));
            var order3 = new Order("ORD-003", 500.00m, new Customer("Charlie", "VIP"));
            
            // GetOrderPriority returns priority based on properties
            Console.WriteLine($"   {order1.Id}: {GetOrderPriority(order1)}");
            Console.WriteLine($"   {order2.Id}: {GetOrderPriority(order2)}");
            Console.WriteLine($"   {order3.Id}: {GetOrderPriority(order3)}");

            // ── CONCEPT: Property Pattern with Type ──────────────────────────
            // Combine property pattern with type check

            // Example 3: Property + type pattern
            // Output: 3. Property + Type Patterns:
            Console.WriteLine("\n3. Property + Type Patterns:");
            
            // Process different vehicle types
            // GetVehicleDescription returns description
            var car = new Car { Brand = "Toyota", Doors = 4, IsElectric = false };
            var electricCar = new Car { Brand = "Tesla", Doors = 4, IsElectric = true };
            var motorcycle = new Motorcycle { Brand = "Harley", CC = 1200 };
            var truck = new Truck { Brand = "Ford", BedLength = 6.5 };
            
            Console.WriteLine($"   Car: {GetVehicleDescription(car)}");
            Console.WriteLine($"   Electric Car: {GetVehicleDescription(electricCar)}");
            Console.WriteLine($"   Motorcycle: {GetVehicleDescription(motorcycle)}");
            Console.WriteLine($"   Truck: {GetVehicleDescription(truck)}");

            // ── CONCEPT: Property Pattern with Relational ───────────────────
            // Can use relational patterns on properties

            // Example 4: Property with relational patterns
            // Output: 4. Property with Relational Patterns:
            Console.WriteLine("\n4. Property with Relational Patterns:");
            
            // Test different person ages
            var young = new Person("Young", 17, "Student");
            var adult = new Person("Adult", 35, "Professional");
            var senior = new Person("Senior", 70, "Retired");
            
            // GetAgeCategory returns age classification
            Console.WriteLine($"   {young.Name} (age {young.Age}): {GetAgeCategory(young)}");
            Console.WriteLine($"   {adult.Name} (age {adult.Age}): {GetAgeCategory(adult)}");
            Console.WriteLine($"   {senior.Name} (age {senior.Age}): {GetAgeCategory(senior)}");

            // ── REAL-WORLD EXAMPLE: Insurance Premium Calculator ───────────
            // Output: --- Real-World: Insurance Premium Calculator ---
            Console.WriteLine("\n--- Real-World: Insurance Premium Calculator ---");
            
            // Calculate insurance premium based on multiple properties
            // CalculatePremium returns premium amount
            var lowRisk = new Driver { Age = 25, YearsExperience = 5, AccidentCount = 0, VehicleType = "sedan" };
            var mediumRisk = new Driver { Age = 35, YearsExperience = 10, AccidentCount = 1, VehicleType = "suv" };
            var highRisk = new Driver { Age = 20, YearsExperience = 1, AccidentCount = 2, VehicleType = "sports" };
            
            Console.WriteLine($"   Low risk: ${CalculatePremium(lowRisk):F2}");
            Console.WriteLine($"   Medium risk: ${CalculatePremium(mediumRisk):F2}");
            Console.WriteLine($"   High risk: ${CalculatePremium(highRisk):F2}");

            Console.WriteLine("\n=== Property Pattern Complete ===");
        }

        /// <summary>
        /// Checks person's department using property pattern
        /// </summary>
        public static string CheckPersonDepartment(Person person)
        {
            // Property pattern: match on Department property value
            return person.Department switch
            {
                // Engineering = tech team
                "Engineering" => "Tech Team",
                
                // Sales = sales team
                "Sales" => "Sales Team",
                
                // Marketing = marketing team
                "Marketing" => "Marketing Team",
                
                // HR = people team
                "HR" => "People Team",
                
                // Default = general team
                _ => "General Team"
            };
        }

        /// <summary>
        /// Gets order priority based on properties
        /// </summary>
        public static string GetOrderPriority(Order order)
        {
            // Nested property pattern: order.Customer.Tier
            // and relational pattern on order.Amount
            return (order.Customer.Tier, order.Amount) switch
            {
                // VIP customer with high amount = urgent
                ("VIP", > 100) => "Urgent",
                
                // VIP customer = high priority
                ("VIP", _) => "High",
                
                // Regular customer with very high amount = medium
                ("Regular", > 200) => "Medium",
                
                // Regular customer = normal
                ("Regular", _) => "Normal",
                
                // Default = low
                _ => "Low"
            };
        }

        /// <summary>
        /// Gets vehicle description using property patterns
        /// </summary>
        public static string GetVehicleDescription(object vehicle)
        {
            // Combined type and property pattern
            return vehicle switch
            {
                // Car with electric property
                Car c when c.IsElectric => $"Electric Car: {c.Brand} with {c.Doors} doors",
                
                // Car (non-electric)
                Car c => $"Gas Car: {c.Brand} with {c.Doors} doors",
                
                // Motorcycle
                Motorcycle m => $"Motorcycle: {m.Brand}, {m.CC}cc",
                
                // Truck
                Truck t => $"Truck: {t.Brand}, {t.BedLength{ft} bed",
                
                // Default
                _ => "Unknown vehicle type"
            };
        }

        /// <summary>
        /// Gets age category using property pattern with relational
        /// </summary>
        public static string GetAgeCategory(Person person)
        {
            // Property pattern with relational: person.Age
            return person.Age switch
            {
                // Under 18 = minor
                < 18 => "Minor",
                
                // 18-64 = adult
                >= 18 and < 65 => "Adult",
                
                // 65+ = senior
                >= 65 => "Senior",
                
                // Default
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Real-world: Calculates insurance premium
        /// </summary>
        public static double CalculatePremium(Driver driver)
        {
            // Multiple property patterns combined
            return (driver.Age, driver.YearsExperience, driver.AccidentCount, driver.VehicleType) switch
            {
                // Young driver, new license, accidents = very high
                (< 25, < 3, > 0, _) => 2500.00,
                
                // Young driver, sports car = high
                (< 25, _, 0, "sports") => 2000.00,
                
                // Experienced driver, no accidents, safe vehicle = low
                (>= 30, >= 5, 0, "sedan") => 800.00,
                
                // Experienced driver, no accidents, SUV = medium
                (>= 30, >= 5, 0, "suv") => 1000.00,
                
                // No accidents, safe driver = medium-low
                (_, _, 0, _) => 1200.00,
                
                // 1 accident = high
                (_, _, 1, _) => 1800.00,
                
                // 2+ accidents = very high
                (_, _, >= 2, _) => 2500.00,
                
                // Default = standard rate
                _ => 1500.00
            };
        }
    }

    // ── EXAMPLE CLASSES ───────────────────────────────────────────────────
    /// <summary>
    /// Person class with basic properties
    /// </summary>
    public class Person
    {
        public string Name { get; }
        public int Age { get; }
        public string Department { get; }
        
        public Person(string name, int age, string department)
        {
            Name = name;
            Age = age;
            Department = department;
        }
    }

    /// <summary>
    /// Customer with tier property
    /// </summary>
    public class Customer
    {
        public string Name { get; }
        public string Tier { get; }
        
        public Customer(string name, string tier)
        {
            Name = name;
            Tier = tier;
        }
    }

    /// <summary>
    /// Order with nested customer
    /// </summary>
    public class Order
    {
        public string Id { get; }
        public decimal Amount { get; }
        public Customer Customer { get; }
        
        public Order(string id, decimal amount, Customer customer)
        {
            Id = id;
            Amount = amount;
            Customer = customer;
        }
    }

    /// <summary>
    /// Car vehicle type
    /// </summary>
    public class Car
    {
        public string Brand { get; set; }
        public int Doors { get; set; }
        public bool IsElectric { get; set; }
    }

    /// <summary>
    /// Motorcycle vehicle type
    /// </summary>
    public class Motorcycle
    {
        public string Brand { get; set; }
        public int CC { get; set; }
    }

    /// <summary>
    /// Truck vehicle type
    /// </summary>
    public class Truck
    {
        public string Brand { get; set; }
        public double BedLength { get; set; }
    }

    /// <summary>
    /// Driver for insurance calculation
    /// </summary>
    public class Driver
    {
        public int Age { get; set; }
        public int YearsExperience { get; set; }
        public int AccidentCount { get; set; }
        public string VehicleType { get; set; }
    }
}
