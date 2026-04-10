/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Builder Pattern Extended
 * FILE      : 04_Builder.cs
 * PURPOSE   : Extended Builder pattern with director and fluent interface
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._03_Builder
{
    /// <summary>
    /// Demonstrates Builder pattern with director
    /// </summary>
    public class BuilderExtended
    {
        /// <summary>
        /// Entry point for Builder examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Builder Pattern Extended ===
            Console.WriteLine("=== Builder Pattern Extended ===\n");

            // ── CONCEPT: Fluent Builder Interface ───────────────────────────────
            // Method chaining for readable object construction

            // Example 1: Fluent Interface
            // Output: 1. Fluent Interface:
            Console.WriteLine("1. Fluent Interface:");
            
            // Build using chained methods
            var person = new PersonBuilder()
                .SetName("John Doe")
                .SetAge(30)
                .SetEmail("john@example.com")
                .Build();
            
            // Output: Person: John Doe, Age: 30, Email: john@example.com
            Console.WriteLine($"   Person: {person.Name}, Age: {person.Age}, Email: {person.Email}");

            // Example 2: Director Pattern
            // Output: 2. Director Pattern:
            Console.WriteLine("\n2. Director Pattern:");
            
            // Director constructs with predefined steps
            var director = new ConstructionDirector();
            var houseBuilder = new HouseBuilder();
            
            // Build standard house via director
            var standardHouse = director.BuildStandardHouse(houseBuilder);
            // Output: Standard: 2 Bed, 1 Bath, Garage
            Console.WriteLine($"   Standard: {standardHouse.Bedrooms} Bed, {standardHouse.Bathrooms} Bath, {standardHouse.HasGarage}");

            // Example 3: Complex Object Builder
            // Output: 3. Complex Object Builder:
            Console.WriteLine("\n3. Complex Object Builder:");
            
            // Build complex query with many options
            var query = new QueryBuilder()
                .Select("Id, Name, Price")
                .From("Products")
                .Where("Price > 100")
                .OrderBy("Price DESC")
                .Limit(10)
                .Build();
            
            // Output: Query: SELECT Id, Name, Price FROM Products WHERE Price > 100 ORDER BY Price DESC LIMIT 10
            Console.WriteLine($"   Query: {query}");

            // ── REAL-WORLD EXAMPLE: HTTP Request Builder ───────────────────────
            // Output: --- Real-World: HTTP Request Builder ---
            Console.WriteLine("\n--- Real-World: HTTP Request Builder ---");
            
            // Build HTTP request fluently
            var request = new HttpRequestBuilder()
                .SetMethod("POST")
                .SetUrl("https://api.example.com/users")
                .AddHeader("Content-Type", "application/json")
                .AddHeader("Authorization", "Bearer token123")
                .SetBody("{\"name\": \"John\"}")
                .Build();
            
            // Output: POST https://api.example.com/users
            Console.WriteLine($"   {request.Method} {request.Url}");
            
            // Headers displayed
            // Output: Headers: Content-Type: application/json, Authorization: Bearer token123
            Console.WriteLine($"   Headers: {request.Headers.Count}");

            Console.WriteLine("\n=== Builder Pattern Extended Complete ===");
        }
    }

    /// <summary>
    /// Person class with multiple properties
    /// </summary>
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }

    /// <summary>
    /// Fluent builder for Person
    /// </summary>
    public class PersonBuilder
    {
        private readonly Person _person = new Person();
        
        /// <summary>
        /// Sets person name
        /// </summary>
        /// <param name="name">Person name</param>
        /// <returns>Builder for chaining</returns>
        public PersonBuilder SetName(string name)
        {
            _person.Name = name;
            return this;
        }
        
        /// <summary>
        /// Sets person age
        /// </summary>
        /// <param name="age">Person age</param>
        /// <returns>Builder for chaining</returns>
        public PersonBuilder SetAge(int age)
        {
            _person.Age = age;
            return this;
        }
        
        /// <summary>
        /// Sets person email
        /// </summary>
        /// <param name="email">Email address</param>
        /// <returns>Builder for chaining</returns>
        public PersonBuilder SetEmail(string email)
        {
            _person.Email = email;
            return this;
        }
        
        /// <summary>
        /// Sets person address
        /// </summary>
        /// <param name="address">Address</param>
        /// <returns>Builder for chaining</returns>
        public PersonBuilder SetAddress(string address)
        {
            _person.Address = address;
            return this;
        }
        
        /// <summary>
        /// Sets person phone
        /// </summary>
        /// <param name="phone">Phone number</param>
        /// <returns>Builder for chaining</returns>
        public PersonBuilder SetPhone(string phone)
        {
            _person.Phone = phone;
            return this;
        }
        
        /// <summary>
        /// Builds the person object
        /// </summary>
        /// <returns>Person instance</returns>
        public Person Build() => _person;
    }

    /// <summary>
    /// House class for builder example
    /// </summary>
    public class House
    {
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public bool HasGarage { get; set; }
        public bool HasPool { get; set; }
        public int SquareFeet { get; set; }
    }

    /// <summary>
    /// House builder interface
    /// </summary>
    public interface IHouseBuilder
    {
        IHouseBuilder SetBedrooms(int count);
        IHouseBuilder SetBathrooms(int count);
        IHouseBuilder SetGarage(bool hasGarage);
        IHouseBuilder SetPool(bool hasPool);
        IHouseBuilder SetSquareFeet(int sqft);
        House Build();
    }

    /// <summary>
    /// Concrete house builder
    /// </summary>
    public class HouseBuilder : IHouseBuilder
    {
        private readonly House _house = new House();
        
        /// <summary>
        /// Sets number of bedrooms
        /// </summary>
        public IHouseBuilder SetBedrooms(int count)
        {
            _house.Bedrooms = count;
            return this;
        }
        
        /// <summary>
        /// Sets number of bathrooms
        /// </summary>
        public IHouseBuilder SetBathrooms(int count)
        {
            _house.Bathrooms = count;
            return this;
        }
        
        /// <summary>
        /// Sets garage presence
        /// </summary>
        public IHouseBuilder SetGarage(bool hasGarage)
        {
            _house.HasGarage = hasGarage;
            return this;
        }
        
        /// <summary>
        /// Sets pool presence
        /// </summary>
        public IHouseBuilder SetPool(bool hasPool)
        {
            _house.HasPool = hasPool;
            return this;
        }
        
        /// <summary>
        /// Sets square footage
        /// </summary>
        public IHouseBuilder SetSquareFeet(int sqft)
        {
            _house.SquareFeet = sqft;
            return this;
        }
        
        /// <summary>
        /// Builds the house
        /// </summary>
        /// <returns>House instance</returns>
        public House Build() => _house;
    }

    /// <summary>
    /// Director - defines construction steps
    /// </summary>
    public class ConstructionDirector
    {
        /// <summary>
        /// Builds standard house
        /// </summary>
        /// <param name="builder">House builder</param>
        /// <returns>Standard house</returns>
        public House BuildStandardHouse(IHouseBuilder builder)
        {
            return builder
                .SetBedrooms(2)
                .SetBathrooms(1)
                .SetGarage(true)
                .SetPool(false)
                .SetSquareFeet(1200)
                .Build();
        }
        
        /// <summary>
        /// Builds luxury house
        /// </summary>
        /// <param name="builder">House builder</param>
        /// <returns>Luxury house</returns>
        public House BuildLuxuryHouse(IHouseBuilder builder)
        {
            return builder
                .SetBedrooms(5)
                .SetBathrooms(4)
                .SetGarage(true)
                .SetPool(true)
                .SetSquareFeet(5000)
                .Build();
        }
    }

    /// <summary>
    /// SQL query builder
    /// </summary>
    public class QueryBuilder
    {
        private readonly List<string> _selectColumns = new List<string>();
        private string _fromTable = "";
        private string _whereClause = "";
        private string _orderByColumn = "";
        private string _orderDirection = "";
        private int? _limitCount;
        
        /// <summary>
        /// Adds columns to select
        /// </summary>
        /// <param name="columns">Comma-separated columns</param>
        /// <returns>Builder</returns>
        public QueryBuilder Select(string columns)
        {
            _selectColumns.Add(columns);
            return this;
        }
        
        /// <summary>
        /// Sets from table
        /// </summary>
        /// <param name="table">Table name</param>
        /// <returns>Builder</returns>
        public QueryBuilder From(string table)
        {
            _fromTable = table;
            return this;
        }
        
        /// <summary>
        /// Sets where clause
        /// </summary>
        /// <param name="condition">Where condition</param>
        /// <returns>Builder</returns>
        public QueryBuilder Where(string condition)
        {
            _whereClause = condition;
            return this;
        }
        
        /// <summary>
        /// Sets order by column
        /// </summary>
        /// <param name="column">Column to order by</param>
        /// <returns>Builder</returns>
        public QueryBuilder OrderBy(string column)
        {
            _orderByColumn = column;
            return this;
        }
        
        /// <summary>
        /// Sets result limit
        /// </summary>
        /// <param name="count">Limit count</param>
        /// <returns>Builder</returns>
        public QueryBuilder Limit(int count)
        {
            _limitCount = count;
            return this;
        }
        
        /// <summary>
        /// Builds SQL query string
        /// </summary>
        /// <returns>SQL query</returns>
        public string Build()
        {
            var query = $"SELECT {string.Join(", ", _selectColumns)} FROM {_fromTable}";
            
            if (!string.IsNullOrEmpty(_whereClause))
                query += $" WHERE {_whereClause}";
            
            if (!string.IsNullOrEmpty(_orderByColumn))
                query += $" ORDER BY {_orderByColumn}";
            
            if (_limitCount.HasValue)
                query += $" LIMIT {_limitCount}";
            
            return query;
        }
    }

    /// <summary>
    /// HTTP request class
    /// </summary>
    public class HttpRequest
    {
        public string Method { get; set; }
        public string Url { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new();
        public string Body { get; set; }
    }

    /// <summary>
    /// HTTP request builder
    /// </summary>
    public class HttpRequestBuilder
    {
        private readonly HttpRequest _request = new HttpRequest();
        
        /// <summary>
        /// Sets HTTP method
        /// </summary>
        /// <param name="method">Method (GET, POST, etc)</param>
        /// <returns>Builder</returns>
        public HttpRequestBuilder SetMethod(string method)
        {
            _request.Method = method;
            return this;
        }
        
        /// <summary>
        /// Sets URL
        /// </summary>
        /// <param name="url">Request URL</param>
        /// <returns>Builder</returns>
        public HttpRequestBuilder SetUrl(string url)
        {
            _request.Url = url;
            return this;
        }
        
        /// <summary>
        /// Adds header
        /// </summary>
        /// <param name="key">Header name</param>
        /// <param name="value">Header value</param>
        /// <returns>Builder</returns>
        public HttpRequestBuilder AddHeader(string key, string value)
        {
            _request.Headers[key] = value;
            return this;
        }
        
        /// <summary>
        /// Sets request body
        /// </summary>
        /// <param name="body">Request body</param>
        /// <returns>Builder</returns>
        public HttpRequestBuilder SetBody(string body)
        {
            _request.Body = body;
            return this;
        }
        
        /// <summary>
        /// Builds the request
        /// </summary>
        /// <returns>HttpRequest</returns>
        public HttpRequest Build() => _request;
    }
}