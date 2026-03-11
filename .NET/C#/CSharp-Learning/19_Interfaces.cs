/*
================================================================================
TOPIC 19: INTERFACES
================================================================================

Interfaces define contracts that classes must implement.

TABLE OF CONTENTS:
1. What is an Interface?
2. Interface vs Abstract Class
3. Implementing Interfaces
4. Multiple Interfaces
================================================================================
*/

namespace InterfaceExamples
{
    // Define contract
    interface IAnimal
    {
        string Name { get; set; }
        void MakeSound();
        void Move();
    }
    
    interface IComparable
    {
        int CompareTo(object obj);
    }
    
    // Implement interface
    class Dog : IAnimal
    {
        public string Name { get; set; }
        
        public void MakeSound()
        {
            Console.WriteLine($"{Name} barks");
        }
        
        public void Move()
        {
            Console.WriteLine($"{Name} runs");
        }
    }
    
    class Bird : IAnimal
    {
        public string Name { get; set; }
        
        public void MakeSound()
        {
            Console.WriteLine($"{Name} chirps");
        }
        
        public void Move()
        {
            Console.WriteLine($"{Name} flies");
        }
    }
    
    // Multiple interfaces
    interface IReadable
    {
        void Read();
    }
    
    interface IWritable
    {
        void Write();
    }
    
    class FileHandler : IReadable, IWritable
    {
        public void Read()
        {
            Console.WriteLine("Reading file");
        }
        
        public void Write()
        {
            Console.WriteLine("Writing file");
        }
    }
    
    class Program
    {
        static void Main()
        {
            IAnimal animal = new Dog { Name = "Buddy" };
            animal.MakeSound();
            animal.Move();
            
            IAnimal bird = new Bird { Name = "Tweety" };
            bird.MakeSound();
            
            FileHandler file = new FileHandler();
            file.Read();
            file.Write();
        }
    }
}

/*
INTERFACE RULES:
---------------
- Cannot be instantiated
- All members are public and abstract
- Class can implement multiple interfaces
- Use I prefix for naming (IAnimal, IComparable)

WHY INTERFACES?
---------------
- Multiple inheritance
- Loose coupling
- Dependency injection
- Testability
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 20 covers Exception Handling.
*/
