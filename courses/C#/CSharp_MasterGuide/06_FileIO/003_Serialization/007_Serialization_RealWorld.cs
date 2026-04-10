/*
TOPIC: C# File I/O Operations
SUBTOPIC: Serialization
FILE: 07_Serialization_RealWorld.cs
PURPOSE: Real-world examples - config files, caching, data persistence
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace CSharp_MasterGuide._06_FileIO._03_Serialization
{
    public class NN_07_Serialization_RealWorld
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Real-World Serialization Examples ===");
            Console.WriteLine();

            RealWorldExample_ApplicationConfig();
            Console.WriteLine();

            RealWorldExample_SessionCache();
            Console.WriteLine();

            RealWorldExample_UserSettings();
            Console.WriteLine();

            RealWorldExample_DataBackup();
            Console.WriteLine();

            RealWorldExample_ApiResponse();
            
            CleanupDemoFiles();
        }

        private static void RealWorldExample_ApplicationConfig()
        {
            Console.WriteLine("=== REAL-WORLD: Application Configuration ===");
            
            string configPath = "NN_app_config.json";
            
            var config = new AppConfiguration
            {
                Application = new AppInfo
                {
                    Name = "MyDesktopApp",
                    Version = "2.1.0",
                    Environment = "Production"
                },
                Server = new ServerConfig
                {
                    Host = "api.example.com",
                    Port = 443,
                    UseSsl = true,
                    TimeoutSeconds = 30
                },
                Database = new DatabaseConfig
                {
                    ConnectionString = "Server=db.example.com;Database=myapp;User=app_user;Password=***",
                    MaxConnections = 50,
                    EnablePooling = true
                },
                Logging = new LoggingConfig
                {
                    Level = "Information",
                    FilePath = "logs/app.log",
                    MaxFileSizeMb = 100,
                    RetainDays = 30
                },
                Features = new Dictionary<string, bool>
                {
                    { "darkMode", true },
                    { "autoUpdate", true },
                    { "telemetry", false },
                    { "betaFeatures", false }
                }
            };
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
            };
            
            string json = JsonSerializer.Serialize(config, options);
            File.WriteAllText(configPath, json);
            
            Console.WriteLine("Configuration saved:");
            Console.WriteLine(json);
            
            string loadedJson = File.ReadAllText(configPath);
            var loaded = JsonSerializer.Deserialize<AppConfiguration>(loadedJson, options)!;
            
            Console.WriteLine("Loaded configuration:");
            Console.WriteLine($"  App: {loaded.Application.Name} v{loaded.Application.Version}");
            Console.WriteLine($"  Server: {loaded.Server.Host}:{loaded.Server.Port}");
            Console.WriteLine($"  DB: {loaded.Database.ConnectionString.Substring(0, 30)}...");
            Console.WriteLine($"  Features: {loaded.Features.Count} enabled");
            
            File.Delete(configPath);
            Console.WriteLine("// Output: Application configuration persisted to JSON");
        }

        private static void RealWorldExample_SessionCache()
        {
            Console.WriteLine("=== REAL-WORLD: Session Cache Storage ===");
            
            string cachePath = "NN_session_cache.json";
            
            var cache = new SessionCache
            {
                SessionId = Guid.NewGuid().ToString(),
                UserId = "user_12345",
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                Data = new Dictionary<string, object>
                {
                    { "userName", "John Doe" },
                    { "roles", new[] { "admin", "editor" } },
                    { "lastActivity", DateTime.UtcNow },
                    { "preferences", new { theme = "dark", language = "en" } }
                }
            };
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
            
            string json = JsonSerializer.Serialize(cache, options);
            File.WriteAllText(cachePath, json);
            
            Console.WriteLine($"Session cache saved: {cache.SessionId}");
            Console.WriteLine($"Expires: {cache.ExpiresAt}");
            
            string loadedJson = File.ReadAllText(cachePath);
            var loaded = JsonSerializer.Deserialize<SessionCache>(loadedJson, options)!;
            
            Console.WriteLine($"Session valid: {(loaded.ExpiresAt > DateTime.UtcNow ? "Yes" : "No")}");
            Console.WriteLine($"User: {loaded.UserId}");
            Console.WriteLine($"Data entries: {loaded.Data.Count}");
            
            File.Delete(cachePath);
            Console.WriteLine("// Output: User session cached as JSON");
        }

        private static void RealWorldExample_UserSettings()
        {
            Console.WriteLine("=== REAL-WORLD: User Preferences ===");
            
            string settingsPath = "NN_user_settings.xml";
            
            var settings = new UserSettings
            {
                UserId = "user_98765",
                DisplayName = "John's Workspace",
                Theme = "Dark",
                Language = "en-US",
                FontSize = 14,
                WindowBounds = new WindowBounds { X = 100, Y = 100, Width = 1200, Height = 800 },
                RecentFiles = new List<string>
                {
                    "document.docx",
                    "spreadsheet.xlsx",
                    "presentation.pptx"
                },
                CustomColors = new Dictionary<string, string>
                {
                    { "primary", "#007bff" },
                    { "secondary", "#6c757d" },
                    { "success", "#28a745" },
                    { "danger", "#dc3545" }
                },
                KeyboardShortcuts = new Dictionary<string, string>
                {
                    { "save", "Ctrl+S" },
                    { "open", "Ctrl+O" },
                    { "new", "Ctrl+N" },
                    { "undo", "Ctrl+Z" }
                },
                LastModified = DateTime.Now
            };
            
            XmlSerializer serializer = new XmlSerializer(typeof(UserSettings));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            
            using (StreamWriter writer = new StreamWriter(settingsPath))
            {
                serializer.Serialize(writer, settings, ns);
            }
            
            Console.WriteLine("User settings saved as XML:");
            Console.WriteLine(File.ReadAllText(settingsPath));
            
            UserSettings loaded = new UserSettings();
            using (StreamReader reader = new StreamReader(settingsPath))
            {
                loaded = (UserSettings)serializer.Deserialize(reader)!;
            }
            
            Console.WriteLine("Loaded settings:");
            Console.WriteLine($"  User: {loaded.UserId}");
            Console.WriteLine($"  Theme: {loaded.Theme}");
            Console.WriteLine($"  Window: {loaded.WindowBounds.Width}x{loaded.WindowBounds.Height}");
            Console.WriteLine($"  Recent files: {loaded.RecentFiles.Count}");
            Console.WriteLine($"  Shortcuts: {loaded.KeyboardShortcuts.Count}");
            
            File.Delete(settingsPath);
            Console.WriteLine("// Output: User preferences persisted as XML");
        }

        private static void RealWorldExample_DataBackup()
        {
            Console.WriteLine("=== REAL-WORLD: Data Backup ===");
            
            string backupPath = "NN_data_backup.json";
            
            var backup = new DataBackup
            {
                BackupId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                Source = "production_db",
                Records = new List<BackupRecord>
                {
                    new BackupRecord { Id = 1, Type = "customer", Data = "{\"name\":\"Acme Corp\",\"contacts\":5}" },
                    new BackupRecord { Id = 2, Type = "product", Data = "{\"sku\":\"PROD-001\",\"stock\":150}" },
                    new BackupRecord { Id = 3, Type = "order", Data = "{\"orderId\":\"ORD-123\",\"total\":299.99}" }
                },
                Checksum = "abc123def456"
            };
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
            };
            
            string json = JsonSerializer.Serialize(backup, options);
            File.WriteAllText(backupPath, json);
            
            Console.WriteLine($"Backup created: {backup.BackupId}");
            Console.WriteLine($"Records: {backup.Records.Count}");
            Console.WriteLine($"Checksum: {backup.Checksum}");
            
            string loadedJson = File.ReadAllText(backupPath);
            var loaded = JsonSerializer.Deserialize<DataBackup>(loadedJson, options)!;
            
            Console.WriteLine("Backup restored:");
            Console.WriteLine($"  ID: {loaded.BackupId}");
            Console.WriteLine($"  Created: {loaded.CreatedAt}");
            Console.WriteLine($"  Records: {loaded.Records.Count}");
            Console.WriteLine($"  Verified: {(loaded.Checksum != null ? "Yes" : "No")}");
            
            File.Delete(backupPath);
            Console.WriteLine("// Output: Data backup as JSON file");
        }

        private static void RealWorldExample_ApiResponse()
        {
            Console.WriteLine("=== REAL-WORLD: API Response Handling ===");
            
            string responsePath = "NN_api_response.json";
            
            var response = new ApiResponse
            {
                Status = "success",
                StatusCode = 200,
                Message = "Data retrieved successfully",
                Data = new Dictionary<string, object>
                {
                    { "users", new[] { 
                        new { id = 1, name = "Alice", email = "alice@example.com" },
                        new { id = 2, name = "Bob", email = "bob@example.com" }
                    }},
                    { "pagination", new { page = 1, totalPages = 5, totalItems = 50 } }
                },
                Timestamp = DateTime.UtcNow,
                RequestId = Guid.NewGuid().ToString()
            };
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
            
            string json = JsonSerializer.Serialize(response, options);
            File.WriteAllText(responsePath, json);
            
            Console.WriteLine("API response:");
            Console.WriteLine(json);
            
            string loadedJson = File.ReadAllText(responsePath);
            var loaded = JsonSerializer.Deserialize<ApiResponse>(loadedJson, options)!;
            
            Console.WriteLine("Parsed response:");
            Console.WriteLine($"  Status: {loaded.Status} ({loaded.StatusCode})");
            Console.WriteLine($"  Message: {loaded.Message}");
            Console.WriteLine($"  Request ID: {loaded.RequestId}");
            Console.WriteLine($"  Timestamp: {loaded.Timestamp}");
            
            File.Delete(responsePath);
            Console.WriteLine("// Output: API response serialized/deserialized");
        }

        private static void CleanupDemoFiles()
        {
            string[] files = { "NN_app_config.json", "NN_session_cache.json", "NN_user_settings.xml", "NN_data_backup.json", "NN_api_response.json" };
            foreach (string f in files)
            {
                if (File.Exists(f)) File.Delete(f);
            }
            Console.WriteLine("[Cleanup] Demo files removed");
        }
    }

    public class AppConfiguration
    {
        public AppInfo Application { get; set; } = new();
        public ServerConfig Server { get; set; } = new();
        public DatabaseConfig Database { get; set; } = new();
        public LoggingConfig Logging { get; set; } = new();
        public Dictionary<string, bool> Features { get; set; } = new();
    }

    public class AppInfo
    {
        public string Name { get; set; } = "";
        public string Version { get; set; } = "";
        public string Environment { get; set; } = "";
    }

    public class ServerConfig
    {
        public string Host { get; set; } = "";
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public int TimeoutSeconds { get; set; }
    }

    public class DatabaseConfig
    {
        public string ConnectionString { get; set; } = "";
        public int MaxConnections { get; set; }
        public bool EnablePooling { get; set; }
    }

    public class LoggingConfig
    {
        public string Level { get; set; } = "";
        public string FilePath { get; set; } = "";
        public int MaxFileSizeMb { get; set; }
        public int RetainDays { get; set; }
    }

    public class SessionCache
    {
        public string SessionId { get; set; } = "";
        public string UserId { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public Dictionary<string, object> Data { get; set; } = new();
    }

    [XmlRoot("UserSettings")]
    public class UserSettings
    {
        [XmlAttribute("userId")]
        public string UserId { get; set; } = "";
        
        [XmlElement("DisplayName")]
        public string DisplayName { get; set; } = "";
        
        [XmlElement("Theme")]
        public string Theme { get; set; } = "";
        
        [XmlElement("Language")]
        public string Language { get; set; } = "";
        
        [XmlElement("FontSize")]
        public int FontSize { get; set; }
        
        [XmlElement("WindowBounds")]
        public WindowBounds WindowBounds { get; set; } = new();
        
        [XmlArray("RecentFiles")]
        [XmlArrayItem("File")]
        public List<string> RecentFiles { get; set; } = new();
        
        [XmlArray("CustomColors")]
        [XmlArrayItem("Color")]
        public Dictionary<string, string> CustomColors { get; set; } = new();
        
        [XmlArray("KeyboardShortcuts")]
        [XmlArrayItem("Shortcut")]
        public Dictionary<string, string> KeyboardShortcuts { get; set; } = new();
        
        [XmlElement("LastModified")]
        public DateTime LastModified { get; set; }
    }

    public class WindowBounds
    {
        [XmlAttribute("x")]
        public int X { get; set; }
        
        [XmlAttribute("y")]
        public int Y { get; set; }
        
        [XmlAttribute("width")]
        public int Width { get; set; }
        
        [XmlAttribute("height")]
        public int Height { get; set; }
    }

    public class DataBackup
    {
        public string BackupId { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string Source { get; set; } = "";
        public List<BackupRecord> Records { get; set; } = new();
        public string? Checksum { get; set; }
    }

    public class BackupRecord
    {
        public int Id { get; set; }
        public string Type { get; set; } = "";
        public string Data { get; set; } = "";
    }

    public class ApiResponse
    {
        public string Status { get; set; } = "";
        public int StatusCode { get; set; }
        public string Message { get; set; } = "";
        public Dictionary<string, object> Data { get; set; } = new();
        public DateTime Timestamp { get; set; }
        public string RequestId { get; set; } = "";
    }
}