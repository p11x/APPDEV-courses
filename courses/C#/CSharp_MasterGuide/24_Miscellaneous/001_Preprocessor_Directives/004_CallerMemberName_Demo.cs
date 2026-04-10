/*
 * ============================================================
 * TOPIC     : Miscellaneous
 * SUBTOPIC  : Caller Information Attributes
 * FILE      : CallerMemberName_Demo.cs
 * PURPOSE   : Using CallerMemberName attribute
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._24_Miscellaneous._01_Preprocessor_Directives
{
    /// <summary>
    /// CallerMemberName demonstration
    /// </summary>
    public class CallerMemberNameDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== CallerMemberName ===\n");

            // Output: --- Automatic Property Name ---
            Console.WriteLine("--- Automatic Property Name ---");

            var person = new Person2();
            person.Name = "Alice";
            // Output: Setting property: Name

            // Output: --- Logging ---
            Console.WriteLine("\n--- Logging ---");

            var logger = new Logger2();
            logger.Log("Information");
            // Output: [Info] Log called from Main

            // Output: --- INotifyPropertyChanged ---
            Console.WriteLine("\n--- INotifyPropertyChanged ---");

            var vm = new ViewModel();
            vm.Name = "Bob";
            // Output: PropertyChanged: Name

            Console.WriteLine("\n=== CallerMemberName Complete ===");
        }
    }

    /// <summary>
    /// Person with property tracking (C# 5+)
    /// </summary>
    public class Person2
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                Console.WriteLine($"   Setting property: {nameof(Name)}");
            }
        }
    }

    /// <summary>
    /// Logger with caller info
    /// </summary>
    public class Logger2
    {
        public void Log(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string caller = "")
        {
            Console.WriteLine($"   [{caller}] {message}");
        }
    }

    /// <summary>
    /// ViewModel
    /// </summary>
    public class ViewModel
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                Console.WriteLine($"   PropertyChanged: {nameof(Name)}");
            }
        }
    }
}