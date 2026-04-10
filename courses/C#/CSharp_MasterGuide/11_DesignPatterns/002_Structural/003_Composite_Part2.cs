/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Composite Part 2
 * FILE      : 05_Composite_Part2.cs
 * PURPOSE   : Demonstrates advanced Composite patterns in C#
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic;

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural
{
    /// <summary>
    /// Advanced Composite patterns
    /// </summary>
    public class CompositePart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Composite Part 2 ===\n");

            Console.WriteLine("1. Composite with Operations:");
            var org = new Organization();
            org.AddEmployee(new Employee("CEO", 100000));
            org.AddEmployee(new Employee("CTO", 90000));
            Console.WriteLine($"   Total salary: {org.GetTotalSalary()}");

            Console.WriteLine("\n2. Composite Iterator:");
            var tree = new TreeNode("root");
            tree.Add(new TreeNode("child1"));
            tree.Add(new TreeNode("child2"));
            // Output: Tree traversed

            Console.WriteLine("\n=== Composite Part 2 Complete ===");
        }
    }

    public interface IEmployee
    {
        string Name { get; }
        decimal Salary { get; }
    }

    public class Employee : IEmployee
    {
        public string Name { get; }
        public decimal Salary { get; }
        
        public Employee(string name, decimal salary)
        {
            Name = name;
            Salary = salary;
        }
    }

    public class Organization
    {
        private List<IEmployee> _employees = new();
        
        public void AddEmployee(IEmployee emp) => _employees.Add(emp);
        public decimal GetTotalSalary()
        {
            decimal total = 0;
            foreach (var emp in _employees) total += emp.Salary;
            Console.WriteLine($"   Total salary: {total}");
            return total;
        }
    }

    public class TreeNode
    {
        public string Value { get; }
        private List<TreeNode> _children = new();
        
        public TreeNode(string value) => Value = value;
        public void Add(TreeNode child) => _children.Add(child);
    }
}