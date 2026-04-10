/*
TOPIC: C# File I/O Operations
SUBTOPIC: Serialization
FILE: 01_JsonSerialization.cs
PURPOSE: JSON serialization with System.Text.Json - basics and options
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CSharp_MasterGuide._06_FileIO._03_Serialization
{
    public class NN_01_JsonSerialization
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== JSON Serialization Demo (System.Text.Json) ===");
            Console.WriteLine();

            BasicObjectSerialization();
            Console.WriteLine();

            CollectionSerialization();
            Console.WriteLine();

            DeserializationExamples();
            Console.WriteLine();

            PrettyPrintAndOptions();
            Console.WriteLine();

            RealWorldExample_ConfigFile();
            
            CleanupDemoFiles();
        }

        private static void BasicObjectSerialization()
        {
            Console.WriteLine("--- Basic Object Serialization ---");
            
            var person = new Person
            {
                Name = "John Doe",
                Age = 30,
                Email = "john@example.com",
                IsActive = true
            };
            
            string json = JsonSerializer.Serialize(person);
            Console.WriteLine($"Serialized: {json}");
            
            var deserialized = JsonSerializer.Deserialize<Person>(json);
            Console.WriteLine($"Deserialized: {deserialized?.Name}, {deserialized?.Age}, {deserialized?.Email}");
            Console.WriteLine("// Output: Simple object converted to/from JSON");
        }

        private static void CollectionSerialization()
        {
            Console.WriteLine("--- Collection Serialization ---");
            
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99m },
                new Product { Id = 2, Name = "Mouse", Price = 29.99m },
                new Product { Id = 3, Name = "Keyboard", Price = 79.99m }
            };
            
            string json = JsonSerializer.Serialize(products);
            Console.WriteLine($"Products JSON: {json}");
            
            var restored = JsonSerializer.Deserialize<List<Product>>(json);
            Console.WriteLine("Restored products:");
            foreach (var p in restored!)
            {
                Console.WriteLine($"  {p.Id}: {p.Name} - ${p.Price:F2}");
            }
            
            var dict = new Dictionary<string, int>
            {
                { "apple", 5 }, { "banana", 3 }, { "orange", 8 }
            };
            string dictJson = JsonSerializer.Serialize(dict);
            Console.WriteLine($"Dictionary JSON: {dictJson}");
            
            Console.WriteLine("// Output: Collections serialized as JSON arrays/objects");
        }

        private static void DeserializationExamples()
        {
            Console.WriteLine("--- Deserialization Examples ---");
            
            string jsonString = @"" +
                "{\"name\":\"Alice\",\"age\":25,\"email\":\"alice@test.com\",\"isActive\":false}";
            
            var person = JsonSerializer.Deserialize<Person>(jsonString);
            Console.WriteLine($"From JSON string: {person?.Name}, Age: {person?.Age}");
            
            string jsonFile = "NN_data.json";
            File.WriteAllText(jsonFile, jsonString);
            
            using (StreamReader reader = new StreamReader(jsonFile))
            {
                var fromFile = JsonSerializer.Deserialize<Person>(reader.ReadToEnd());
                Console.WriteLine($"From file: {fromFile?.Name}, Email: {fromFile?.Email}");
            }
            
            File.Delete(jsonFile);
            Console.WriteLine("// Output: JSON string and file parsed into objects");
        }

        private static void PrettyPrintAndOptions()
        {
            Console.WriteLine("--- Pretty Print and Options ---");
            
            var settings = new AppSettings
            {
                Theme = "dark",
                MaxUsers = 100,
                Timeout = 30,
                Features = new[] { "auth", "logging", "caching" }
            };
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            string prettyJson = JsonSerializer.Serialize(settings, options);
            Console.WriteLine("Pretty-printed JSON:");
            Console.WriteLine(prettyJson);
            
            var readBack = JsonSerializer.Deserialize<AppSettings>(prettyJson, options);
            Console.WriteLine($"Read back: {readBack?.Theme}, timeout: {readBack?.Timeout}");
            Console.WriteLine("// Output: Formatted JSON with options applied");
        }

        private static void RealWorldExample_ConfigFile()
        {
            Console.WriteLine();
            Console.WriteLine("=== REAL-WORLD EXAMPLE: Application Config File ===");
            
            string configPath = "NN_config.json";
            
            var config = new AppConfig
            {
                Application = "MyApp v1.0",
                Database = new DatabaseConfig
                {
                    Host = "localhost",
                    Port = 5432,
                    Name = "myapp_db",
                    Username = "admin"
                },
                Logging = new LoggingConfig
                {
                    Level = "Info",
                    FilePath = "logs/app.log",
                    MaxFileSizeMB = 100
                },
                Features = new Dictionary<string, bool>
                {
                    { "darkMode", true },
                    { "notifications", true },
                    { "analytics", false }
                }
            };
            
            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            string configJson = JsonSerializer.Serialize(config, serializeOptions);
            File.WriteAllText(configPath, configJson);
            Console.WriteLine("Config file created:");
            Console.WriteLine(configJson);
            
            string loadedJson = File.ReadAllText(configPath);
            var loadedConfig = JsonSerializer.Deserialize<AppConfig>(loadedJson, serializeOptions);
            
            Console.WriteLine("Loaded config:");
            Console.WriteLine($"  App: {loadedConfig?.Application}");
            Console.WriteLine($"  DB Host: {loadedConfig?.Database?.Host}:{loadedConfig?.Database?.Port}");
            Console.WriteLine($"  Log Level: {loadedConfig?.Logging?.Level}");
            Console.WriteLine($"  Features: {string.Join(", ", loadedConfig?.Features?.Keys ?? Array.Empty<string>())}");
            
            File.Delete(configPath);
            Console.WriteLine("// Output: Application configuration saved/loaded from JSON");
        }

        private static void CleanupDemoFiles()
        {
            string[] files = { "NN_data.json", "NN_config.json" };
            foreach (string f in files)
            {
                if (File.Exists(f)) File.Delete(f);
            }
            Console.WriteLine("[Cleanup] Demo files removed");
        }
    }

    public class Person
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
    }

    public class AppSettings
    {
        public string Theme { get; set; } = "";
        public int MaxUsers { get; set; }
        public int Timeout { get; set; }
        public string[] Features { get; set; } = Array.Empty<string>();
    }

    public class AppConfig
    {
        public string Application { get; set; } = "";
        public DatabaseConfig? Database { get; set; }
        public LoggingConfig? Logging { get; set; }
        public Dictionary<string, bool>? Features { get; set; }
    }

    public class DatabaseConfig
    {
        public string Host { get; set; } = "";
        public int Port { get; set; }
        public string Name { get; set; } = "";
        public string Username { get; set; } = "";
    }

    public class LoggingConfig
    {
        public string Level { get; set; } = "";
        public string FilePath { get; set; } = "";
        public int MaxFileSizeMB { get; set; }
    }
}