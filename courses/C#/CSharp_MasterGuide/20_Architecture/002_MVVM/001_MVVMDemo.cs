/*
 * ============================================================
 * TOPIC     : Architecture
 * SUBTOPIC  : MVVM Pattern
 * FILE      : 01_MVVMDemo.cs
 * PURPOSE   : Demonstrates MVVM architecture in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._20_Architecture._01_MVVM
{
    public class MVVMDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== MVVM Demo ===\n");
            Console.WriteLine("1. Model-View-ViewModel:");
            var vm = new MainViewModel();
            vm.LoadData();
            Console.WriteLine($"   Data loaded: {vm.Items.Count} items");
            Console.WriteLine("\n=== MVVM Complete ===");
        }
    }

    public class MainViewModel
    {
        public List<string> Items { get; } = new List<string>();
        public void LoadData() => Items.Add("Item 1");
    }
}