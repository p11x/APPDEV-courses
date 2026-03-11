using System;

namespace CSharpFundamentals.Code
{
    /// <summary>
    /// Demonstrates enums and nullable types in C#
    /// </summary>
    
    // Basic enum
    public enum Day
    {
        Monday,    // 0
        Tuesday,   // 1
        Wednesday, // 2
        Thursday,  // 3
        Friday,    // 4
        Saturday,  // 5
        Sunday     // 6
    }
    
    // Enum with explicit values
    public enum Status
    {
        Pending = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4
    }
    
    // Flags enum
    [Flags]
    public enum Permissions
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4,
        Delete = 8
    }
    
    public class EnumsDemo
    {
        public static void Main()
        {
            // Basic enum usage
            Day today = Day.Wednesday;
            Console.WriteLine($"Today is {today}");
            Console.WriteLine($"Day number: {(int)today}");
            
            // Switch on enum
            switch (today)
            {
                case Day.Monday:
                case Day.Tuesday:
                case Day.Wednesday:
                case Day.Thursday:
                case Day.Friday:
                    Console.WriteLine("Weekday");
                    break;
                case Day.Saturday:
                case Day.Sunday:
                    Console.WriteLine("Weekend");
                    break;
            }
            
            // Enum with values
            Status currentStatus = Status.InProgress;
            Console.WriteLine($"\nStatus: {currentStatus} ({(int)currentStatus})");
            
            // Flags enum
            Permissions perms = Permissions.Read | Permissions.Write;
            Console.WriteLine($"\nPermissions: {perms}");
            Console.WriteLine($"Has Write: {perms.HasFlag(Permissions.Write)}");
            
            // Nullable types
            int? nullableInt = null;
            double? nullableDouble = 3.14;
            bool? nullableBool = null;
            
            Console.WriteLine($"\nNullable int: {nullableInt ?? 0}");
            Console.WriteLine($"Nullable double: {nullableDouble}");
            Console.WriteLine($"Nullable bool: {nullableBool ?? false}");
            
            // Null coalescing assignment
            nullableInt ??= 10;
            Console.WriteLine($"After ??=: {nullableInt}");
        }
    }
}
