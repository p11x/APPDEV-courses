/*
TOPIC: C# File I/O Operations
SUBTOPIC: StreamReader and StreamWriter
FILE: 04_BinaryReader_Writer.cs
PURPOSE: Demonstrates BinaryReader and BinaryWriter for binary data handling
*/

using System;
using System.IO;
using System.Text;

namespace CSharp_MasterGuide._06_FileIO._02_StreamReaderWriter
{
    public class NN_04_BinaryReader_Writer
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== BinaryReader/BinaryWriter Demo ===");
            Console.WriteLine();

            BasicBinaryWriteRead();
            Console.WriteLine();

            VariousDataTypes();
            Console.WriteLine();

            WorkingWithByteArrays();
            Console.WriteLine();

            ReadingBinaryPosition();
            Console.WriteLine();

            RealWorldExample_SaveGameData();
            
            CleanupDemoFiles();
        }

        private static void BasicBinaryWriteRead()
        {
            Console.WriteLine("--- Basic Binary Write/Read ---");
            
            string filePath = "NN_binary_basic.bin";
            
            using (BinaryWriter writer = new BinaryWriter(File.Create(filePath)))
            {
                writer.Write("Hello Binary World");
                writer.Write(42);
                writer.Write(3.14159);
                writer.Write(true);
                writer.Write(255L);
            }
            
            Console.WriteLine("Written: string, int, double, bool, long");
            
            using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
            {
                string text = reader.ReadString();
                int intVal = reader.ReadInt32();
                double doubleVal = reader.ReadDouble();
                bool boolVal = reader.ReadBoolean();
                long longVal = reader.ReadInt64();
                
                Console.WriteLine($"Read values: \"{text}\", {intVal}, {doubleVal}, {boolVal}, {longVal}");
            }
            
            Console.WriteLine("// Output: Binary format is more compact than text");
            
            File.Delete(filePath);
        }

        private static void VariousDataTypes()
        {
            Console.WriteLine("--- Various Data Types ---");
            
            string filePath = "NN_binary_types.bin";
            
            using (BinaryWriter writer = new BinaryWriter(File.Create(filePath)))
            {
                writer.Write((byte)128);
                writer.Write((sbyte)-50);
                writer.Write((short)-1000);
                writer.Write((ushort)65000);
                writer.Write(100000);
                writer.Write((uint)4000000000);
                writer.Write(-9876543210L);
                writer.Write((ulong)18000000000000000000);
                writer.Write(12.5f);
                writer.Write(987.654);
                writer.Write('X');
                writer.Write(new byte[] { 1, 2, 3, 4, 5 });
            }
            
            Console.WriteLine("Written: byte, sbyte, short, ushort, int, uint, long, ulong, float, double, char, byte[]");
            
            using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
            {
                Console.WriteLine($"byte: {reader.ReadByte()}");
                Console.WriteLine($"sbyte: {reader.ReadSByte()}");
                Console.WriteLine($"short: {reader.ReadInt16()}");
                Console.WriteLine($"ushort: {reader.ReadUInt16()}");
                Console.WriteLine($"int: {reader.ReadInt32()}");
                Console.WriteLine($"uint: {reader.ReadUInt32()}");
                Console.WriteLine($"long: {reader.ReadInt64()}");
                Console.WriteLine($"ulong: {reader.ReadUInt64()}");
                Console.WriteLine($"float: {reader.ReadSingle()}");
                Console.WriteLine($"double: {reader.ReadDouble()}");
                Console.WriteLine($"char: {reader.ReadChar()}");
                byte[] bytes = reader.ReadBytes(5);
                Console.WriteLine($"byte[5]: [{string.Join(", ", bytes)}]");
            }
            
            Console.WriteLine("// Output: All primitive types supported");
            
            File.Delete(filePath);
        }

        private static void WorkingWithByteArrays()
        {
            Console.WriteLine("--- Working with Byte Arrays ---");
            
            string filePath = "NN_binary_bytes.bin";
            string text = "Binary data test";
            byte[] originalBytes = Encoding.UTF8.GetBytes(text);
            
            using (BinaryWriter writer = new BinaryWriter(File.Create(filePath)))
            {
                writer.Write(originalBytes.Length);
                writer.Write(originalBytes);
            }
            
            Console.WriteLine($"Original bytes (UTF8): {originalBytes.Length} bytes");
            Console.WriteLine($"Bytes: [{string.Join(", ", originalBytes)}]");
            
            using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
            {
                int length = reader.ReadInt32();
                byte[] readBytes = reader.ReadBytes(length);
                string decoded = Encoding.UTF8.GetString(readBytes);
                
                Console.WriteLine($"Read length: {length}");
                Console.WriteLine($"Decoded string: \"{decoded}\"");
            }
            
            Console.WriteLine("// Output: Byte arrays stored with length prefix");
            
            File.Delete(filePath);
        }

        private static void ReadingBinaryPosition()
        {
            Console.WriteLine("--- Binary Position and Seek ---");
            
            string filePath = "NN_binary_seek.bin";
            
            using (BinaryWriter writer = new BinaryWriter(File.Create(filePath)))
            {
                writer.Write("First");
                writer.Write("Second");
                writer.Write("Third");
            }
            
            using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
            {
                Console.WriteLine($"BaseStream.Position: {reader.BaseStream.Position}");
                Console.WriteLine($"BaseStream.Length: {reader.BaseStream.Length}");
                
                string first = reader.ReadString();
                Console.WriteLine($"Read '{first}', position: {reader.BaseStream.Position}");
                
                long savedPosition = reader.BaseStream.Position;
                string second = reader.ReadString();
                Console.WriteLine($"Read '{second}', position: {reader.BaseStream.Position}");
                
                reader.BaseStream.Seek(savedPosition, SeekOrigin.Begin);
                string secondAgain = reader.ReadString();
                Console.WriteLine($"Seek back, read '{secondAgain}', position: {reader.BaseStream.Position}");
                
                long endPosition = reader.BaseStream.Length;
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                Console.WriteLine($"Seek to start, position: {reader.BaseStream.Position}");
            }
            
            Console.WriteLine("// Output: Can navigate within binary stream");
            
            File.Delete(filePath);
        }

        private static void RealWorldExample_SaveGameData()
        {
            Console.WriteLine();
            Console.WriteLine("=== REAL-WORLD EXAMPLE: Save Game Data ===");
            
            string gameFile = "NN_savegame.dat";
            
            PlayerData player = new PlayerData
            {
                Name = "HeroKnight",
                Level = 25,
                Experience = 15750,
                Health = 100,
                MaxHealth = 150,
                Mana = 80,
                MaxMana = 100,
                Gold = 5430,
                PositionX = 123.45f,
                PositionY = 67.89f,
                PositionZ = 0.0f,
                Inventory = new[] { "Sword", "Shield", "Potion", "Map", "Key" },
                QuestsCompleted = 42
            };
            
            SaveGameData(gameFile, player);
            Console.WriteLine("[Setup] Game saved to binary file");
            
            PlayerData loadedPlayer = LoadGameData(gameFile);
            Console.WriteLine();
            Console.WriteLine("Loaded Player Data:");
            Console.WriteLine($"  Name: {loadedPlayer.Name}");
            Console.WriteLine($"  Level: {loadedPlayer.Level}");
            Console.WriteLine($"  Experience: {loadedPlayer.Experience}");
            Console.WriteLine($"  HP: {loadedPlayer.Health}/{loadedPlayer.MaxHealth}");
            Console.WriteLine($"  MP: {loadedPlayer.Mana}/{loadedPlayer.MaxMana}");
            Console.WriteLine($"  Gold: {loadedPlayer.Gold}");
            Console.WriteLine($"  Position: ({loadedPlayer.PositionX}, {loadedPlayer.PositionY}, {loadedPlayer.PositionZ})");
            Console.WriteLine($"  Inventory: [{string.Join(", ", loadedPlayer.Inventory)}]");
            Console.WriteLine($"  Quests: {loadedPlayer.QuestsCompleted}");
            
            FileInfo fi = new FileInfo(gameFile);
            Console.WriteLine($"  File size: {fi.Length} bytes");
            
            File.Delete(gameFile);
            Console.WriteLine($"[Cleanup] Deleted: {gameFile}");
            Console.WriteLine("// Output: Game state saved and loaded as binary");
        }

        private static void SaveGameData(string filePath, PlayerData player)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Create(filePath)))
            {
                writer.Write(player.Name);
                writer.Write(player.Level);
                writer.Write(player.Experience);
                writer.Write(player.Health);
                writer.Write(player.MaxHealth);
                writer.Write(player.Mana);
                writer.Write(player.MaxMana);
                writer.Write(player.Gold);
                writer.Write(player.PositionX);
                writer.Write(player.PositionY);
                writer.Write(player.PositionZ);
                writer.Write(player.Inventory.Length);
                foreach (string item in player.Inventory)
                {
                    writer.Write(item);
                }
                writer.Write(player.QuestsCompleted);
            }
        }

        private static PlayerData LoadGameData(string filePath)
        {
            PlayerData player = new PlayerData();
            
            using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
            {
                player.Name = reader.ReadString();
                player.Level = reader.ReadInt32();
                player.Experience = reader.ReadInt32();
                player.Health = reader.ReadInt32();
                player.MaxHealth = reader.ReadInt32();
                player.Mana = reader.ReadInt32();
                player.MaxMana = reader.ReadInt32();
                player.Gold = reader.ReadInt64();
                player.PositionX = reader.ReadSingle();
                player.PositionY = reader.ReadSingle();
                player.PositionZ = reader.ReadSingle();
                
                int inventoryCount = reader.ReadInt32();
                player.Inventory = new string[inventoryCount];
                for (int i = 0; i < inventoryCount; i++)
                {
                    player.Inventory[i] = reader.ReadString();
                }
                
                player.QuestsCompleted = reader.ReadInt32();
            }
            
            return player;
        }

        private static void CleanupDemoFiles()
        {
            string[] filesToClean = new[]
            {
                "NN_binary_basic.bin", "NN_binary_types.bin",
                "NN_binary_bytes.bin", "NN_binary_seek.bin",
                "NN_savegame.dat"
            };
            
            foreach (string file in filesToClean)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            Console.WriteLine("[Cleanup] All demo files removed");
        }
    }

    public class PlayerData
    {
        public string Name { get; set; } = "";
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Mana { get; set; }
        public int MaxMana { get; set; }
        public long Gold { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public string[] Inventory { get; set; } = Array.Empty<string>();
        public int QuestsCompleted { get; set; }
    }
}