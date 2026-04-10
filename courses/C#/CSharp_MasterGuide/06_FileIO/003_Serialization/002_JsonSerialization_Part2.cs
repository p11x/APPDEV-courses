/*
TOPIC: C# File I/O Operations
SUBTOPIC: Serialization
FILE: 02_JsonSerialization_Part2.cs
PURPOSE: More JSON - serialization options, attributes, custom converters
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CSharp_MasterGuide._06_FileIO._03_Serialization
{
    public class NN_02_JsonSerialization_Part2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== JSON Serialization Advanced Demo ===");
            Console.WriteLine();

            JsonAttributesDemo();
            Console.WriteLine();

            NamingPoliciesDemo();
            Console.WriteLine();

            IgnorePropertiesDemo();
            Console.WriteLine();

            CustomConverterDemo();
            Console.WriteLine();

            NullableAndDefaultDemo();
            Console.WriteLine();

            RealWorldExample_UserPreferences();
            
            CleanupDemoFiles();
        }

        private static void JsonAttributesDemo()
        {
            Console.WriteLine("--- JSON Attributes Demo ---");
            
            var user = new User
            {
                Id = 123,
                UserName = "john_doe",
                Password = "secret123",
                CreatedDate = new DateTime(2024, 1, 15, 10, 30, 0),
                LastLogin = DateTime.Now,
                Role = "Admin"
            };
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            string json = JsonSerializer.Serialize(user, options);
            Console.WriteLine("Serialized with attributes:");
            Console.WriteLine(json);
            
            Console.WriteLine("// Output: JsonPropertyName, JsonIgnore, JsonInclude applied");
        }

        private static void NamingPoliciesDemo()
        {
            Console.WriteLine("--- Naming Policies Demo ---");
            
            var person = new PersonDetails
            {
                FirstName = "Jane",
                LastName = "Smith",
                EmailAddress = "jane@test.com",
                PhoneNumber = "555-1234"
            };
            
            Console.WriteLine("Default (PascalCase):");
            Console.WriteLine(JsonSerializer.Serialize(person));
            
            var camelOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            Console.WriteLine("CamelCase:");
            Console.WriteLine(JsonSerializer.Serialize(person, camelOptions));
            
            var snakeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy()
            };
            Console.WriteLine("snake_case:");
            Console.WriteLine(JsonSerializer.Serialize(person, snakeOptions));
            
            Console.WriteLine("// Output: Different naming conventions applied");
        }

        private static void IgnorePropertiesDemo()
        {
            Console.WriteLine("--- Ignore Properties Demo ---");
            
            var employee = new EmployeeRecord
            {
                Id = 42,
                Name = "Alice",
                Salary = 75000,
                SSN = "123-45-6789",
                Department = "Engineering"
            };
            
            var optionsIgnoreNull = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            Console.WriteLine("Ignore null values:");
            Console.WriteLine(JsonSerializer.Serialize(employee, optionsIgnoreNull));
            
            var optionsIgnoreDefault = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };
            Console.WriteLine("Ignore default values:");
            Console.WriteLine(JsonSerializer.Serialize(employee, optionsIgnoreDefault));
            
            Console.WriteLine("// Output: Certain properties ignored based on conditions");
        }

        private static void CustomConverterDemo()
        {
            Console.WriteLine("--- Custom JSON Converter Demo ---");
            
            var settings = new SensorSettings
            {
                SensorId = "SENS-001",
                ReadingTime = new DateTime(2024, 1, 15, 10, 30, 0),
                Temperature = 23.5,
                Humidity = 65.0
            };
            
            var options = new JsonSerializerOptions
            {
                Converters = { new DateTimeConverter() }
            };
            
            string json = JsonSerializer.Serialize(settings, options);
            Console.WriteLine("Custom date format:");
            Console.WriteLine(json);
            
            var restored = JsonSerializer.Deserialize<SensorSettings>(json, options);
            Console.WriteLine($"Restored: {restored?.SensorId}, time: {restored?.ReadingTime}");
            Console.WriteLine("// Output: Custom converter changes date format");
        }

        private static void NullableAndDefaultDemo()
        {
            Console.WriteLine("--- Nullable and Default Handling ---");
            
            var sample = new NullableSample
            {
                RequiredField = "present",
                OptionalField = null,
                DefaultInt = 0,
                NullableInt = null
            };
            
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                IncludeFields = true
            };
            
            string json = JsonSerializer.Serialize(sample, options);
            Console.WriteLine($"Serialized: {json}");
            
            var deserialized = JsonSerializer.Deserialize<NullableSample>(json, options);
            Console.WriteLine($"Required: {deserialized?.RequiredField}");
            Console.WriteLine($"Optional: {deserialized?.OptionalField ?? "(null)"}");
            Console.WriteLine($"NullableInt: {deserialized?.NullableInt?.ToString() ?? "(null)"}");
            Console.WriteLine("// Output: Nullable properties handled appropriately");
        }

        private static void RealWorldExample_UserPreferences()
        {
            Console.WriteLine();
            Console.WriteLine("=== REAL-WORLD EXAMPLE: User Preferences Storage ===");
            
            string prefsPath = "NN_user_preferences.json";
            
            var userPrefs = new UserPreferences
            {
                UserId = "user_12345",
                DisplayName = "John's Workspace",
                Theme = ThemeMode.Dark,
                Language = "en-US",
                FontSize = 14,
                AutoSave = true,
                RecentFiles = new List<string>
                {
                    "document1.docx",
                    "spreadsheet.xlsx",
                    "notes.txt"
                },
                CustomColors = new Dictionary<string, string>
                {
                    { "primary", "#007bff" },
                    { "secondary", "#6c757d" },
                    { "accent", "#28a745" }
                },
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };
            
            string prefsJson = JsonSerializer.Serialize(userPrefs, options);
            File.WriteAllText(prefsPath, prefsJson);
            
            Console.WriteLine("Saved preferences:");
            Console.WriteLine(prefsJson);
            
            string loadedJson = File.ReadAllText(prefsPath);
            var loadedPrefs = JsonSerializer.Deserialize<UserPreferences>(loadedJson, options);
            
            Console.WriteLine("Loaded preferences:");
            Console.WriteLine($"  User: {loadedPrefs?.UserId}");
            Console.WriteLine($"  Theme: {loadedPrefs?.Theme}");
            Console.WriteLine($"  Language: {loadedPrefs?.Language}");
            Console.WriteLine($"  Recent files: {loadedPrefs?.RecentFiles?.Count}");
            Console.WriteLine($"  Colors: {loadedPrefs?.CustomColors?.Count} custom colors");
            
            File.Delete(prefsPath);
            Console.WriteLine("// Output: User preferences persisted to JSON file");
        }

        private static void CleanupDemoFiles()
        {
            if (File.Exists("NN_user_preferences.json"))
                File.Delete("NN_user_preferences.json");
            Console.WriteLine("[Cleanup] Demo files removed");
        }
    }

    public class User
    {
        public int Id { get; set; }
        
        [JsonPropertyName("username")]
        public string UserName { get; set; } = "";
        
        [JsonIgnore]
        public string Password { get; set; } = "";
        
        [JsonInclude]
        public DateTime CreatedDate { get; set; }
        
        public DateTime LastLogin { get; set; }
        
        [JsonPropertyName("role")]
        public string Role { get; set; } = "";
    }

    public class PersonDetails
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
    }

    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            
            var result = new System.Text.StringBuilder();
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (char.IsUpper(c) && i > 0)
                    result.Append('_');
                result.Append(char.ToLowerInvariant(c));
            }
            return result.ToString();
        }
    }

    public class EmployeeRecord
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Salary { get; set; }
        
        [JsonIgnore]
        public string SSN { get; set; } = "";
        
        public string Department { get; set; } = "";
    }

    public class SensorSettings
    {
        public string SensorId { get; set; } = "";
        public DateTime ReadingTime { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
    }

    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            return DateTime.Parse(value ?? "2000-01-01");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }

    public class NullableSample
    {
        public string RequiredField { get; set; } = "";
        public string? OptionalField { get; set; }
        public int DefaultInt { get; set; }
        public int? NullableInt { get; set; }
    }

    public enum ThemeMode { Light, Dark, System }

    public class UserPreferences
    {
        public string UserId { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public ThemeMode Theme { get; set; }
        public string Language { get; set; } = "";
        public int FontSize { get; set; }
        public bool AutoSave { get; set; }
        public List<string>? RecentFiles { get; set; }
        public Dictionary<string, string>? CustomColors { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
    }
}