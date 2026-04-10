/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - State Pattern
 * FILE      : 01_State.cs
 * PURPOSE   : Demonstrates State design pattern in C#
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._04_State
{
    /// <summary>
    /// Demonstrates State pattern - object behavior based on state
    /// </summary>
    public class StatePattern
    {
        /// <summary>
        /// Entry point for State examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === State Pattern Demo ===
            Console.WriteLine("=== State Pattern Demo ===\n");

            // ── CONCEPT: State Pattern ─────────────────────────────────────────
            // Allow object to alter behavior when internal state changes

            // Example 1: Order Status
            // Output: 1. Order Status:
            Console.WriteLine("1. Order Status:");
            
            var order = new Order();
            order.Submit();
            // Output: Order submitted
            
            order.Process();
            // Output: Order being processed
            
            order.Ship();
            // Output: Order shipped

            // Example 2: Vending Machine
            // Output: 2. Vending Machine:
            Console.WriteLine("\n2. Vending Machine:");
            
            var machine = new VendingMachine();
            machine.InsertMoney(100);
            machine.SelectItem("Soda");
            machine.Dispense();
            
            // Output: Item dispensed

            // ── REAL-WORLD EXAMPLE: Document Workflow ─────────────────────────
            // Output: --- Real-World: Document Workflow ---
            Console.WriteLine("\n--- Real-World: Document Workflow ---");
            
            var doc = new Document();
            doc.Submit();
            // Output: Document submitted for review
            
            doc.Approve();
            // Output: Document approved
            
            doc.Publish();
            // Output: Document published

            Console.WriteLine("\n=== State Pattern Complete ===");
        }
    }

    /// <summary>
    /// Order state interface
    /// </summary>
    public interface IOrderState
    {
        void Submit(Order order);
        void Process(Order order);
        void Ship(Order order);
        void Deliver(Order order);
    }

    /// <summary>
    /// Submitted state
    /// </summary>
    public class SubmittedState : IOrderState
    {
        public void Submit(Order order) => Console.WriteLine("   Already submitted");
        public void Process(Order order) 
        { 
            Console.WriteLine("   Order being processed");
            order.State = new ProcessingState();
        }
        public void Ship(Order order) => Console.WriteLine("   Must process first");
        public void Deliver(Order order) => Console.WriteLine("   Must ship first");
    }

    /// <summary>
    /// Processing state
    /// </summary>
    public class ProcessingState : IOrderState
    {
        public void Submit(Order order) => Console.WriteLine("   Already submitted");
        public void Process(Order order) => Console.WriteLine("   Already processing");
        public void Ship(Order order) 
        { 
            Console.WriteLine("   Order shipped");
            order.State = new ShippedState();
        }
        public void Deliver(Order order) => Console.WriteLine("   Must ship first");
    }

    /// <summary>
    /// Shipped state
    /// </summary>
    public class ShippedState : IOrderState
    {
        public void Submit(Order order) => Console.WriteLine("   Already submitted");
        public void Process(Order order) => Console.WriteLine("   Already processed");
        public void Ship(Order order) => Console.WriteLine("   Already shipped");
        public void Deliver(Order order) 
        { 
            Console.WriteLine("   Order delivered");
            order.State = new DeliveredState();
        }
    }

    /// <summary>
    /// Delivered state
    /// </summary>
    public class DeliveredState : IOrderState
    {
        public void Submit(Order order) => Console.WriteLine("   Already completed");
        public void Process(Order order) => Console.WriteLine("   Already completed");
        public void Ship(Order order) => Console.WriteLine("   Already completed");
        public void Deliver(Order order) => Console.WriteLine("   Already delivered");
    }

    /// <summary>
    /// Order context
    /// </summary>
    public class Order
    {
        public IOrderState State { get; set; }
        
        public Order() => State = new SubmittedState();
        
        public void Submit() => State.Submit(this);
        public void Process() => State.Process(this);
        public void Ship() => State.Ship(this);
        public void Deliver() => State.Deliver(this);
    }

    /// <summary>
    /// Vending machine state
    /// </summary>
    public interface IVendingState
    {
        void InsertMoney(VendingMachine machine, int amount);
        void SelectItem(VendingMachine machine, string item);
        void Dispense(VendingMachine machine);
    }

    /// <summary>
    /// Waiting for money state
    /// </summary>
    public class WaitingForMoneyState : IVendingState
    {
        public void InsertMoney(VendingMachine machine, int amount)
        {
            machine.Balance += amount;
            Console.WriteLine($"   Money inserted: {amount}");
        }
        public void SelectItem(VendingMachine machine, string item) => 
            Console.WriteLine("   Insert money first");
        public void Dispense(VendingMachine machine) => Console.WriteLine("   Select item first");
    }

    /// <summary>
    /// Has money state
    /// </summary>
    public class HasMoneyState : IVendingState
    {
        public void InsertMoney(VendingMachine machine, int amount)
        {
            machine.Balance += amount;
            Console.WriteLine($"   Additional money: {amount}");
        }
        public void SelectItem(VendingMachine machine, string item)
        {
            machine.SelectedItem = item;
            Console.WriteLine($"   Selected: {item}");
        }
        public void Dispense(VendingMachine machine)
        {
            if (machine.SelectedItem != null)
            {
                Console.WriteLine($"   Item dispensed: {machine.SelectedItem}");
                machine.State = new WaitingForMoneyState();
            }
        }
    }

    /// <summary>
    /// Vending machine context
    /// </summary>
    public class VendingMachine
    {
        public IVendingState State { get; set; }
        public int Balance { get; set; }
        public string SelectedItem { get; set; }
        
        public VendingMachine() => State = new WaitingForMoneyState();
        
        public void InsertMoney(int amount) => State.InsertMoney(this, amount);
        public void SelectItem(string item) => State.SelectItem(this, item);
        public void Dispense() => State.Dispense(this);
    }

    /// <summary>
    /// Document states
    /// </summary>
    public interface IDocumentState
    {
        void Submit(Document doc);
        void Approve(Document doc);
        void Publish(Document doc);
    }

    /// <summary>
    /// Draft state
    /// </summary>
    public class DraftState : IDocumentState
    {
        public void Submit(Document doc) 
        { 
            Console.WriteLine("   Document submitted for review");
            doc.State = new ReviewState();
        }
        public void Approve(Document doc) => Console.WriteLine("   Submit first");
        public void Publish(Document doc) => Console.WriteLine("   Approve first");
    }

    /// <summary>
    /// Review state
    /// </summary>
    public class ReviewState : IDocumentState
    {
        public void Submit(Document doc) => Console.WriteLine("   Already submitted");
        public void Approve(Document doc) 
        { 
            Console.WriteLine("   Document approved");
            doc.State = new ApprovedState();
        }
        public void Publish(Document doc) => Console.WriteLine("   Approve first");
    }

    /// <summary>
    /// Approved state
    /// </summary>
    public class ApprovedState : IDocumentState
    {
        public void Submit(Document doc) => Console.WriteLine("   Already approved");
        public void Approve(Document doc) => Console.WriteLine("   Already approved");
        public void Publish(Document doc) 
        { 
            Console.WriteLine("   Document published");
            doc.State = new PublishedState();
        }
    }

    /// <summary>
    /// Published state
    /// </summary>
    public class PublishedState : IDocumentState
    {
        public void Submit(Document doc) => Console.WriteLine("   Already published");
        public void Approve(Document doc) => Console.WriteLine("   Already published");
        public void Publish(Document doc) => Console.WriteLine("   Already published");
    }

    /// <summary>
    /// Document context
    /// </summary>
    public class Document
    {
        public IDocumentState State { get; set; }
        
        public Document() => State = new DraftState();
        
        public void Submit() => State.Submit(this);
        public void Approve() => State.Approve(this);
        public void Publish() => State.Publish(this);
    }
}