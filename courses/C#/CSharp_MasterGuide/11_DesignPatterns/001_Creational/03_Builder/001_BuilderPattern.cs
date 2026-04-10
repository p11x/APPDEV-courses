/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Builder Pattern
 * FILE      : 01_BuilderPattern.cs
 * PURPOSE   : Demonstrates Builder design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._03_Builder
{
    /// <summary>
    /// Demonstrates Builder pattern
    /// </summary>
    public class BuilderPattern
    {
        /// <summary>
        /// Entry point for Builder pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Builder Pattern ===
            Console.WriteLine("=== Builder Pattern ===\n");

            // ── CONCEPT: What is Builder? ─────────────────────────────────────
            // Separates object construction from representation

            // Example 1: Basic Builder
            // Output: 1. Basic Builder:
            Console.WriteLine("1. Basic Builder:");
            
            // Use builder to construct complex object
            var builder = new HouseBuilder();
            var house = builder
                .SetRooms(4) // set number of rooms
                .SetBathrooms(2) // set number of bathrooms
                .SetGarage(true) // include garage
                .SetPool(false) // no pool
                .Build(); // construct the house
            
            // Output: House: 4 rooms, 2 bathrooms, Garage: True, Pool: False
            Console.WriteLine($"   House: {house.Rooms} rooms, {house.Bathrooms} bathrooms, Garage: {house.HasGarage}, Pool: {house.HasPool}");

            // ── CONCEPT: Fluent Interface ──────────────────────────────────────
            // Method chaining for readable construction

            // Example 2: Fluent Interface
            // Output: 2. Fluent Interface:
            Console.WriteLine("\n2. Fluent Interface:");
            
            // Chain methods for clean syntax
            var car = new CarBuilder()
                .WithEngine("V8") // set engine type
                .WithColor("Red") // set color
                .WithSunroof(true) // add sunroof
                .WithNavigation(true) // add navigation
                .Build(); // build car
            
            // Output: Car: V8 Engine, Red, Sunroof: True, Navigation: True
            Console.WriteLine($"   Car: {car.Engine} Engine, {car.Color}, Sunroof: {car.HasSunroof}, Navigation: {car.HasNavigation}");

            // ── CONCEPT: Director for Construction Steps ─────────────────────
            // Director defines construction order

            // Example 3: Director Pattern
            // Output: 3. Director Pattern:
            Console.WriteLine("\n3. Director Pattern:");
            
            // Director controls building process
            var director = new ConstructionDirector();
            var villa = director.BuildLuxuryVilla(); // predefined sequence
            var cottage = director.BuildCottage(); // different sequence
            
            // Output: Villa: 5 rooms, 3 bathrooms, Pool: True, Garden: True
            Console.WriteLine($"   Villa: {villa.Rooms} rooms, {villa.Bathrooms} bathrooms, Pool: {villa.HasPool}, Garden: {villa.HasGarden}");
            // Output: Cottage: 2 rooms, 1 bathroom, Pool: False, Garden: True
            Console.WriteLine($"   Cottage: {cottage.Rooms} rooms, {cottage.Bathrooms} bathrooms, Pool: {cottage.HasPool}, Garden: {cottage.HasGarden}");

            // ── REAL-WORLD EXAMPLE: Query Builder ────────────────────────────
            // Output: --- Real-World: Query Builder ---
            Console.WriteLine("\n--- Real-World: Query Builder ---");
            
            // Build SQL queries programmatically
            var query = new QueryBuilder()
                .Select("id", "name", "email") // select columns
                .From("users") // from table
                .Where("age > 18") // where condition
                .OrderBy("name") // order by column
                .Limit(100) // limit results
                .Build(); // generate SQL
            
            // Output: SELECT id, name, email FROM users WHERE age > 18 ORDER BY name LIMIT 100
            Console.WriteLine($"   {query}");

            Console.WriteLine("\n=== Builder Pattern Complete ===");
        }
    }

    /// <summary>
    /// House product
    /// </summary>
    public class House
    {
        public int Rooms { get; set; } // property: number of rooms
        public int Bathrooms { get; set; } // property: number of bathrooms
        public bool HasGarage { get; set; } // property: has garage flag
        public bool HasPool { get; set; } // property: has pool flag
        public bool HasGarden { get; set; } // property: has garden flag
    }

    /// <summary>
    /// House builder
    /// </summary>
    public class HouseBuilder
    {
        private House _house = new House(); // new house instance
        
        /// <summary>
        /// Sets number of rooms
        /// </summary>
        public HouseBuilder SetRooms(int rooms)
        {
            _house.Rooms = rooms; // assign rooms
            return this; // return builder for chaining
        }
        
        /// <summary>
        /// Sets number of bathrooms
        /// </summary>
        public HouseBuilder SetBathrooms(int bathrooms)
        {
            _house.Bathrooms = bathrooms; // assign bathrooms
            return this;
        }
        
        /// <summary>
        /// Sets garage flag
        /// </summary>
        public HouseBuilder SetGarage(bool hasGarage)
        {
            _house.HasGarage = hasGarage; // assign garage flag
            return this;
        }
        
        /// <summary>
        /// Sets pool flag
        /// </summary>
        public HouseBuilder SetPool(bool hasPool)
        {
            _house.HasPool = hasPool; // assign pool flag
            return this;
        }
        
        /// <summary>
        /// Sets garden flag
        /// </summary>
        public HouseBuilder SetGarden(bool hasGarden)
        {
            _house.HasGarden = hasGarden; // assign garden flag
            return this;
        }
        
        /// <summary>
        /// Builds the house
        /// </summary>
        public House Build()
        {
            return _house; // return constructed house
        }
    }

    /// <summary>
    /// Car product
    /// </summary>
    public class Car
    {
        public string Engine { get; set; } // property: engine type
        public string Color { get; set; } // property: car color
        public bool HasSunroof { get; set; } // property: sunroof flag
        public bool HasNavigation { get; set; } // property: navigation flag
    }

    /// <summary>
    /// Car builder with fluent interface
    /// </summary>
    public class CarBuilder
    {
        private Car _car = new Car(); // new car instance
        
        /// <summary>
        /// Sets engine type
        /// </summary>
        public CarBuilder WithEngine(string engine)
        {
            _car.Engine = engine;
            return this;
        }
        
        /// <summary>
        /// Sets color
        /// </summary>
        public CarBuilder WithColor(string color)
        {
            _car.Color = color;
            return this;
        }
        
        /// <summary>
        /// Sets sunroof
        /// </summary>
        public CarBuilder WithSunroof(bool hasSunroof)
        {
            _car.HasSunroof = hasSunroof;
            return this;
        }
        
        /// <summary>
        /// Sets navigation
        /// </summary>
        public CarBuilder WithNavigation(bool hasNavigation)
        {
            _car.HasNavigation = hasNavigation;
            return this;
        }
        
        /// <summary>
        /// Builds the car
        /// </summary>
        public Car Build()
        {
            return _car;
        }
    }

    /// <summary>
    /// Director for construction
    /// </summary>
    public class ConstructionDirector
    {
        /// <summary>
        /// Builds luxury villa
        /// </summary>
        public House BuildLuxuryVilla()
        {
            return new HouseBuilder()
                .SetRooms(5)
                .SetBathrooms(3)
                .SetGarage(true)
                .SetPool(true)
                .SetGarden(true)
                .Build();
        }
        
        /// <summary>
        /// Builds simple cottage
        /// </summary>
        public House BuildCottage()
        {
            return new HouseBuilder()
                .SetRooms(2)
                .SetBathrooms(1)
                .SetGarage(false)
                .SetPool(false)
                .SetGarden(true)
                .Build();
        }
    }

    /// <summary>
    /// Query builder for SQL
    /// </summary>
    public class QueryBuilder
    {
        private string _select = ""; // select clause
        private string _from = ""; // from clause
        private string _where = ""; // where clause
        private string _orderBy = ""; // order by clause
        private int _limit = 0; // limit value
        
        /// <summary>
        /// Adds select columns
        /// </summary>
        public QueryBuilder Select(params string[] columns)
        {
            _select = string.Join(", ", columns); // join columns with comma
            return this;
        }
        
        /// <summary>
        /// Sets from table
        /// </summary>
        public QueryBuilder From(string table)
        {
            _from = table;
            return this;
        }
        
        /// <summary>
        /// Adds where condition
        /// </summary>
        public QueryBuilder Where(string condition)
        {
            _where = condition;
            return this;
        }
        
        /// <summary>
        /// Adds order by clause
        /// </summary>
        public QueryBuilder OrderBy(string column)
        {
            _orderBy = column;
            return this;
        }
        
        /// <summary>
        /// Sets limit
        /// </summary>
        public QueryBuilder Limit(int limit)
        {
            _limit = limit;
            return this;
        }
        
        /// <summary>
        /// Builds SQL query
        /// </summary>
        public string Build()
        {
            var query = $"SELECT {_select} FROM {_from}"; // start with select and from
            if (!string.IsNullOrEmpty(_where)) query += $" WHERE {_where}"; // add where if present
            if (!string.IsNullOrEmpty(_orderBy)) query += $" ORDER BY {_orderBy}"; // add order by if present
            if (_limit > 0) query += $" LIMIT {_limit}"; // add limit if present
            return query;
        }
    }
}