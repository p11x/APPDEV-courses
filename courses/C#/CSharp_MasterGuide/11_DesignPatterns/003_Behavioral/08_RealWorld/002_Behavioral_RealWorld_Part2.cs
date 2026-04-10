/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Real-World Part 2
 * FILE      : 18_Behavioral_RealWorld_Part2.cs
 * PURPOSE   : More real-world Behavioral pattern examples
 * ============================================================
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._08_RealWorld
{
    /// <summary>
    /// More real-world Behavioral pattern examples
    /// </summary>
    public class BehavioralRealWorldPart2
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Behavioral Patterns Real-World Part 2 ===\n");

            // Example: Strategy - Sorting Algorithms
            Console.WriteLine("1. Strategy - Sorting:");
            var sorter = new Sorter();
            sorter.SetStrategy(new QuickSortStrategy());
            sorter.Sort(new[] { 5, 2, 8, 1, 9 });
            
            // Output: Sorted using QuickSort

            // Example: Command - Transaction
            Console.WriteLine("\n2. Command - Transaction:");
            var transaction = new Transaction();
            transaction.Execute(new DebitCommand(100));
            transaction.Execute(new CreditCommand(50));
            transaction.Undo();
            
            // Output: Debited, Credited, Undone

            // Example: Observer - Stock Price
            Console.WriteLine("\n3. Observer - Stock Price:");
            var stock = new Stock("AAPL");
            stock.Attach(new Investor("Alice"));
            stock.Attach(new Investor("Bob"));
            stock.Price = 150;
            
            // Output: Alice notified: AAPL now $150, Bob notified: AAPL now $150

            Console.WriteLine("\n=== Behavioral Real-World Part 2 Complete ===");
        }
    }

    // Strategy - Sorting
    public interface ISortStrategy
    {
        void Sort(int[] array);
    }

    public class QuickSortStrategy : ISortStrategy
    {
        public void Sort(int[] array) => Console.WriteLine("   Sorted using QuickSort");
    }

    public class MergeSortStrategy : ISortStrategy
    {
        public void Sort(int[] array) => Console.WriteLine("   Sorted using MergeSort");
    }

    public class Sorter
    {
        private ISortStrategy _strategy;
        
        public void SetStrategy(ISortStrategy strategy) => _strategy = strategy;
        
        public void Sort(int[] array) => _strategy.Sort(array);
    }

    // Command - Transaction
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    public class DebitCommand : ICommand
    {
        private readonly decimal _amount;
        
        public DebitCommand(decimal amount) => _amount = amount;
        
        public void Execute() => Console.WriteLine($"   Debited: ${_amount}");
        public void Undo() => Console.WriteLine($"   Undone debit");
    }

    public class CreditCommand : ICommand
    {
        private readonly decimal _amount;
        
        public CreditCommand(decimal amount) => _amount = amount;
        
        public void Execute() => Console.WriteLine($"   Credited: ${_amount}");
        public void Undo() => Console.WriteLine($"   Undone credit");
    }

    public class Transaction
    {
        private readonly Stack<ICommand> _commands = new();
        
        public void Execute(ICommand command)
        {
            command.Execute();
            _commands.Push(command);
        }
        
        public void Undo()
        {
            if (_commands.Count > 0)
            {
                var cmd = _commands.Pop();
                cmd.Undo();
            }
        }
    }

    // Observer - Stock
    public interface IObserver
    {
        void Update(string symbol, decimal price);
    }

    public class Stock
    {
        public string Symbol { get; }
        public decimal Price 
        { 
            get => _price;
            set 
            { 
                _price = value; 
                Notify(); 
            }
        }
        
        private decimal _price;
        private readonly List<IObserver> _observers = new();
        
        public Stock(string symbol) => Symbol = symbol;
        
        public void Attach(IObserver observer) => _observers.Add(observer);
        
        private void Notify()
        {
            foreach (var o in _observers)
                o.Update(Symbol, Price);
        }
    }

    public class Investor : IObserver
    {
        public string Name { get; }
        
        public Investor(string name) => Name = name;
        
        public void Update(string symbol, decimal price) => 
            Console.WriteLine($"   {Name} notified: {symbol} now ${price}");
    }
}