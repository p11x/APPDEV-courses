/*
TOPIC: C# File I/O Operations
SUBTOPIC: Serialization
FILE: 08_Serialization_RealWorld_Part2.cs
PURPOSE: More real-world examples - state management, persistence, interop
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using System.Linq;

namespace CSharp_MasterGuide._06_FileIO._03_Serialization
{
    public class NN_08_Serialization_RealWorld_Part2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Real-World Serialization Part 2 ===");
            Console.WriteLine();

            RealWorldExample_GameState();
            Console.WriteLine();

            RealWorldExample_WorkflowPersistence();
            Console.WriteLine();

            RealWorldExample_DataExportImport();
            Console.WriteLine();

            RealWorldExample_InteropWithLegacy();
            Console.WriteLine();

            RealWorldExample_AuditLog();
            
            CleanupDemoFiles();
        }

        private static void RealWorldExample_GameState()
        {
            Console.WriteLine("=== REAL-WORLD: Game State Management ===");
            
            string savePath = "NN_game_save.json";
            
            var gameState = new GameState
            {
                SaveId = Guid.NewGuid().ToString(),
                SaveName = "Chapter 3 - Boss Battle",
                CreatedAt = DateTime.Now,
                PlayTime = TimeSpan.FromHours(2.5),
                Player = new PlayerState
                {
                    Name = "HeroKnight",
                    Level = 25,
                    Experience = 15750,
                    Health = 850,
                    MaxHealth = 1000,
                    Mana = 450,
                    MaxMana = 500,
                    Position = new Vector3 { X = 123.5f, Y = 0.0f, Z = 456.7f },
                    Rotation = 180.0f,
                    Inventory = new List<InventoryItem>
                    {
                        new InventoryItem { Id = "sword_001", Name = "Steel Sword", Type = "Weapon", Quantity = 1, Level = 5 },
                        new InventoryItem { Id = "potion_hp", Name = "Health Potion", Type = "Consumable", Quantity = 10, Level = 1 },
                        new InventoryItem { Id = "armor_003", Name = "Iron Shield", Type = "Armor", Quantity = 1, Level = 3 }
                    },
                    Equipped = new[] { "sword_001", "armor_003" }
                },
                QuestProgress = new Dictionary<string, QuestState>
                {
                    { "main_q1", new QuestState { Name = "The Beginning", Status = "Completed", Progress = 100 } },
                    { "main_q2", new QuestState { Name = "The Journey", Status = "InProgress", Progress = 65 } },
                    { "side_q5", new QuestState { Name = "Treasure Hunt", Status = "Available", Progress = 0 } }
                },
                WorldState = new WorldState
                {
                    CurrentZone = "DarkForest",
                    TimeOfDay = "Night",
                    Weather = "Rain",
                    NPCsKilled = 47,
                    TreasuresFound = 12
                }
            };
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
            };
            
            string json = JsonSerializer.Serialize(gameState, options);
            File.WriteAllText(savePath, json);
            
            Console.WriteLine($"Game saved: {gameState.SaveName}");
            Console.WriteLine($"Player: {gameState.Player.Name} Level {gameState.Player.Level}");
            Console.WriteLine($"Play time: {gameState.PlayTime}");
            Console.WriteLine($"Inventory: {gameState.Player.Inventory.Count} items");
            
            string loadedJson = File.ReadAllText(savePath);
            var loaded = JsonSerializer.Deserialize<GameState>(loadedJson, options)!;
            
            Console.WriteLine("Game loaded:");
            Console.WriteLine($"  Save: {loaded.SaveName}");
            Console.WriteLine($"  Player HP: {loaded.Player.Health}/{loaded.Player.MaxHealth}");
            Console.WriteLine($"  Quests: {loaded.QuestProgress.Count}");
            Console.WriteLine($"  Zone: {loaded.WorldState.CurrentZone}");
            
            File.Delete(savePath);
            Console.WriteLine("// Output: Game state saved and restored from JSON");
        }

        private static void RealWorldExample_WorkflowPersistence()
        {
            Console.WriteLine("=== REAL-WORLD: Workflow Persistence ===");
            
            string workflowPath = "NN_workflow.xml";
            
            var workflow = new WorkflowInstance
            {
                WorkflowId = "WF-2024-001",
                Name = "Employee Onboarding",
                StartedAt = DateTime.Now.AddDays(-5),
                CurrentStep = "Step 3: IT Setup",
                Status = WorkflowStatus.InProgress,
                Initiator = "hr_manager",
                Assignee = "it_support",
                Steps = new List<WorkflowStep>
                {
                    new WorkflowStep { StepId = 1, Name = "HR Interview", Status = "Completed", CompletedAt = DateTime.Now.AddDays(-4), CompletedBy = "hr_manager" },
                    new WorkflowStep { StepId = 2, Name = "Background Check", Status = "Completed", CompletedAt = DateTime.Now.AddDays(-3), CompletedBy = "system" },
                    new WorkflowStep { StepId = 3, Name = "IT Setup", Status = "InProgress", AssignedTo = "it_support" },
                    new WorkflowStep { StepId = 4, Name = "Training", Status = "Pending", AssignedTo = "trainer" },
                    new WorkflowStep { StepId = 5, Name = "Equipment Assignment", Status = "Pending", AssignedTo = "it_support" }
                },
                Data = new Dictionary<string, string>
                {
                    { "employee_name", "John Smith" },
                    { "department", "Engineering" },
                    { "start_date", "2024-02-01" },
                    { "manager", "Jane Doe" }
                }
            };
            
            XmlSerializer serializer = new XmlSerializer(typeof(WorkflowInstance));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            
            using (StreamWriter writer = new StreamWriter(workflowPath))
            {
                serializer.Serialize(writer, workflow, ns);
            }
            
            Console.WriteLine($"Workflow: {workflow.Name}");
            Console.WriteLine($"Status: {workflow.Status}");
            Console.WriteLine($"Current step: {workflow.CurrentStep}");
            
            WorkflowInstance loaded = new WorkflowInstance();
            using (StreamReader reader = new StreamReader(workflowPath))
            {
                loaded = (WorkflowStep)serializer.Deserialize(reader)!;
            }
            
            int completed = loaded.Steps.Count(s => s.Status == "Completed");
            int total = loaded.Steps.Count;
            Console.WriteLine($"Progress: {completed}/{total} steps completed");
            
            File.Delete(workflowPath);
            Console.WriteLine("// Output: Workflow state persisted as XML");
        }

        private static void RealWorldExample_DataExportImport()
        {
            Console.WriteLine("=== REAL-WORLD: Data Export/Import ===");
            
            string exportPath = "NN_data_export.json";
            
            var export = new DataExport
            {
                ExportId = Guid.NewGuid().ToString(),
                ExportedAt = DateTime.Now,
                Source = "customer_database",
                FormatVersion = "1.0",
                RecordCount = 150,
                Records = new List<ExportedRecord>
                {
                    new ExportedRecord { Id = "C001", Type = "customer", Fields = new Dictionary<string, string> { { "name", "Acme Corp" }, { "contact", "John Doe" }, { "email", "john@acme.com" } } },
                    new ExportedRecord { Id = "C002", Type = "customer", Fields = new Dictionary<string, string> { { "name", "Tech Inc" }, { "contact", "Jane Smith" }, { "email", "jane@tech.com" } } },
                    new ExportedRecord { Id = "O001", Type = "order", Fields = new Dictionary<string, string> { { "customer", "C001" }, { "total", "1500.00" }, { "status", "shipped" } } }
                }
            };
            
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(export, options);
            File.WriteAllText(exportPath, json);
            
            Console.WriteLine($"Data exported: {export.RecordCount} records");
            Console.WriteLine($"Export ID: {export.ExportId}");
            
            string importPath = "NN_data_import.json";
            File.Copy(exportPath, importPath);
            
            string importJson = File.ReadAllText(importPath);
            var imported = JsonSerializer.Deserialize<DataExport>(importJson, options)!;
            
            var customers = imported.Records.Where(r => r.Type == "customer").ToList();
            var orders = imported.Records.Where(r => r.Type == "order").ToList();
            
            Console.WriteLine("Data imported:");
            Console.WriteLine($"  Customers: {customers.Count}");
            Console.WriteLine($"  Orders: {orders.Count}");
            Console.WriteLine($"  Export verified: {(imported.RecordCount == imported.Records.Count ? "Yes" : "No")}");
            
            File.Delete(exportPath);
            File.Delete(importPath);
            Console.WriteLine("// Output: Data exported to JSON and re-imported");
        }

        private static void RealWorldExample_InteropWithLegacy()
        {
            Console.WriteLine("=== REAL-WORLD: Interoperability with Legacy Systems ===");
            
            string legacyPath = "NN_legacy_data.xml";
            
            var legacyData = new LegacyCustomerData
            {
                CustomerCode = "CUST-12345",
                FirstName = "John",
                LastName = "Doe",
                AddressLine1 = "123 Main Street",
                AddressLine2 = "Apt 4B",
                City = "New York",
                State = "NY",
                ZipCode = "10001",
                PhoneAreaCode = "212",
                PhoneNumber = "555-1234",
                CreditLimit = 5000.00m,
                Balance = 1250.00m,
                AccountStatus = "Active",
                LastOrderDate = DateTime.Now.AddDays(-30)
            };
            
            XmlSerializer serializer = new XmlSerializer(typeof(LegacyCustomerData));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.ASCII,
                OmitXmlDeclaration = false
            };
            
            using (XmlWriter writer = XmlWriter.Create(legacyPath, settings))
            {
                serializer.Serialize(writer, legacyData, ns);
            }
            
            Console.WriteLine("Legacy XML format:");
            Console.WriteLine(File.ReadAllText(legacyPath));
            
            LegacyCustomerData loaded = new LegacyCustomerData();
            using (StreamReader reader = new StreamReader(legacyPath))
            {
                loaded = (LegacyCustomerData)serializer.Deserialize(reader)!;
            }
            
            string fullName = $"{loaded.FirstName} {loaded.LastName}";
            string fullAddress = $"{loaded.AddressLine1}, {loaded.City}, {loaded.State} {loaded.ZipCode}";
            
            Console.WriteLine("Parsed legacy data:");
            Console.WriteLine($"  Name: {fullName}");
            Console.WriteLine($"  Address: {fullAddress}");
            Console.WriteLine($"  Phone: ({loaded.PhoneAreaCode}) {loaded.PhoneNumber}");
            Console.WriteLine($"  Credit: ${loaded.CreditLimit}, Balance: ${loaded.Balance}");
            
            File.Delete(legacyPath);
            Console.WriteLine("// Output: Legacy system XML data handled");
        }

        private static void RealWorldExample_AuditLog()
        {
            Console.WriteLine("=== REAL-WORLD: Audit Log ===");
            
            string auditPath = "NN_audit_log.json";
            
            var auditLog = new AuditLog
            {
                LogId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                System = "Financial System",
                Environment = "Production",
                Entries = new List<AuditEntry>
                {
                    new AuditEntry { Timestamp = DateTime.UtcNow.AddMinutes(-30), Action = "LOGIN", UserId = "user_001", Result = "Success", Details = "User logged in from IP 192.168.1.100" },
                    new AuditEntry { Timestamp = DateTime.UtcNow.AddMinutes(-25), Action = "VIEW", UserId = "user_001", Result = "Success", Details = "Viewed account summary" },
                    new AuditEntry { Timestamp = DateTime.UtcNow.AddMinutes(-20), Action = "TRANSFER", UserId = "user_001", Result = "Success", Details = "Transferred $500 to account ****1234" },
                    new AuditEntry { Timestamp = DateTime.UtcNow.AddMinutes(-15), Action = "UPDATE", UserId = "user_002", Result = "Failed", Details = "Attempted to update profile - unauthorized" },
                    new AuditEntry { Timestamp = DateTime.UtcNow.AddMinutes(-10), Action = "LOGOUT", UserId = "user_001", Result = "Success", Details = "User logged out normally" }
                }
            };
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
            };
            
            string json = JsonSerializer.Serialize(auditLog, options);
            File.WriteAllText(auditPath, json);
            
            Console.WriteLine($"Audit log created: {auditLog.LogId}");
            Console.WriteLine($"Entries: {auditLog.Entries.Count}");
            
            string loadedJson = File.ReadAllText(auditPath);
            var loaded = JsonSerializer.Deserialize<AuditLog>(loadedJson, options)!;
            
            int successCount = loaded.Entries.Count(e => e.Result == "Success");
            int failCount = loaded.Entries.Count(e => e.Result == "Failed");
            
            Console.WriteLine("Audit log entries:");
            Console.WriteLine($"  Successful: {successCount}");
            Console.WriteLine($"  Failed: {failCount}");
            
            var failedActions = loaded.Entries.Where(e => e.Result == "Failed").Select(e => e.Action);
            Console.WriteLine($"  Failed actions: {string.Join(", ", failedActions)}");
            
            File.Delete(auditPath);
            Console.WriteLine("// Output: Audit log serialized to JSON");
        }

        private static void CleanupDemoFiles()
        {
            string[] files = { "NN_game_save.json", "NN_workflow.xml", "NN_data_export.json", "NN_data_import.json", "NN_legacy_data.xml", "NN_audit_log.json" };
            foreach (string f in files)
            {
                if (File.Exists(f)) File.Delete(f);
            }
            Console.WriteLine("[Cleanup] Demo files removed");
        }
    }

    public class GameState
    {
        public string SaveId { get; set; } = "";
        public string SaveName { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public TimeSpan PlayTime { get; set; }
        public PlayerState Player { get; set; } = new();
        public Dictionary<string, QuestState> QuestProgress { get; set; } = new();
        public WorldState WorldState { get; set; } = new();
    }

    public class PlayerState
    {
        public string Name { get; set; } = "";
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Mana { get; set; }
        public int MaxMana { get; set; }
        public Vector3 Position { get; set; } = new();
        public float Rotation { get; set; }
        public List<InventoryItem> Inventory { get; set; } = new();
        public string[] Equipped { get; set; } = Array.Empty<string>();
    }

    public class Vector3 { public float X { get; set; } public float Y { get; set; } public float Z { get; set; } }

    public class InventoryItem
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public int Quantity { get; set; }
        public int Level { get; set; }
    }

    public class QuestState
    {
        public string Name { get; set; } = "";
        public string Status { get; set; } = "";
        public int Progress { get; set; }
    }

    public class WorldState
    {
        public string CurrentZone { get; set; } = "";
        public string TimeOfDay { get; set; } = "";
        public string Weather { get; set; } = "";
        public int NPCsKilled { get; set; }
        public int TreasuresFound { get; set; }
    }

    [XmlRoot("Workflow")]
    public class WorkflowInstance
    {
        [XmlAttribute("id")]
        public string WorkflowId { get; set; } = "";
        
        [XmlElement("Name")]
        public string Name { get; set; } = "";
        
        [XmlElement("StartedAt")]
        public DateTime StartedAt { get; set; }
        
        [XmlElement("CurrentStep")]
        public string CurrentStep { get; set; } = "";
        
        [XmlElement("Status")]
        public WorkflowStatus Status { get; set; }
        
        [XmlElement("Initiator")]
        public string Initiator { get; set; } = ""
        
        [XmlElement("Assignee")]
        public string Assignee { get; set; } = "";
        
        [XmlArray("Steps")]
        [XmlArrayItem("Step")]
        public List<WorkflowStep> Steps { get; set; } = new();
        
        [XmlArray("Data")]
        [XmlArrayItem("Item")]
        public Dictionary<string, string> Data { get; set; } = new();
    }

    public enum WorkflowStatus { Pending, InProgress, Completed, Cancelled }

    public class WorkflowStep
    {
        [XmlAttribute("id")]
        public int StepId { get; set; }
        
        [XmlElement("Name")]
        public string Name { get; set; } = "";
        
        [XmlElement("Status")]
        public string Status { get; set; } = "";
        
        [XmlElement("CompletedAt")]
        public DateTime? CompletedAt { get; set; }
        
        [XmlElement("CompletedBy")]
        public string? CompletedBy { get; set; }
        
        [XmlElement("AssignedTo")]
        public string? AssignedTo { get; set; }
    }

    public class DataExport
    {
        public string ExportId { get; set; } = "";
        public DateTime ExportedAt { get; set; }
        public string Source { get; set; } = "";
        public string FormatVersion { get; set; } = "";
        public int RecordCount { get; set; }
        public List<ExportedRecord> Records { get; set; } = new();
    }

    public class ExportedRecord
    {
        public string Id { get; set; } = "";
        public string Type { get; set; } = "";
        public Dictionary<string, string> Fields { get; set; } = new();
    }

    [XmlRoot("Customer")]
    public class LegacyCustomerData
    {
        [XmlElement("CustomerCode")]
        public string CustomerCode { get; set; } = "";
        
        [XmlElement("FirstName")]
        public string FirstName { get; set; } = "";
        
        [XmlElement("LastName")]
        public string LastName { get; set; } = "";
        
        [XmlElement("AddressLine1")]
        public string AddressLine1 { get; set; } = "";
        
        [XmlElement("AddressLine2")]
        public string AddressLine2 { get; set; } = "";
        
        [XmlElement("City")]
        public string City { get; set; } = "";
        
        [XmlElement("State")]
        public string State { get; set; } = "";
        
        [XmlElement("ZipCode")]
        public string ZipCode { get; set; } = "";
        
        [XmlElement("PhoneAreaCode")]
        public string PhoneAreaCode { get; set; } = "";
        
        [XmlElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = "";
        
        [XmlElement("CreditLimit")]
        public decimal CreditLimit { get; set; }
        
        [XmlElement("Balance")]
        public decimal Balance { get; set; }
        
        [XmlElement("AccountStatus")]
        public string AccountStatus { get; set; } = "";
        
        [XmlElement("LastOrderDate")]
        public DateTime LastOrderDate { get; set; }
    }

    public class AuditLog
    {
        public string LogId { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string System { get; set; } = "";
        public string Environment { get; set; } = "";
        public List<AuditEntry> Entries { get; set; } = new();
    }

    public class AuditEntry
    {
        public DateTime Timestamp { get; set; }
        public string Action { get; set; } = "";
        public string UserId { get; set; } = "";
        public string Result { get; set; } = "";
        public string Details { get; set; } = "";
    }
}