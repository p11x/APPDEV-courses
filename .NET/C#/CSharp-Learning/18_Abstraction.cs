/*
================================================================================
TOPIC 18: ABSTRACTION
================================================================================

Abstraction hides complex implementation details and shows only necessary features.

TABLE OF CONTENTS:
1. What is Abstraction?
2. Abstract Classes
3. Abstract Methods
================================================================================
*/

namespace AbstractionExamples
{
    // Abstract class - cannot be instantiated
    abstract class Animal
    {
        public string Name { get; set; }
        
        // Abstract method - must be implemented by derived classes
        public abstract void MakeSound();
        
        // Regular method
        public void Sleep()
        {
            Console.WriteLine($"{Name} is sleeping");
        }
    }
    
    class Dog : Animal
    {
        public override void MakeSound()
        {
            Console.WriteLine($"{Name} says: Woof!");
        }
    }
    
    class Cat : Animal
    {
        public override void MakeSound()
        {
            Console.WriteLine($"{Name} says: Meow!");
        }
    }
    
    class Program
    {
        static void Main()
        {
            // Cannot do: Animal a = new Animal();
            
            Animal dog = new Dog { Name = "Buddy" };
            dog.MakeSound();
            dog.Sleep();
            
            Animal cat = new Cat { Name = "Whiskers" };
            cat.MakeSound();
        }
    }
}

/*
ABSTRACT vs INTERFACE:
----------------------
Abstract class: Can have implementation, single inheritance
Interface: Contract only, multiple inheritance

Use abstraction to:
- Hide complex implementation
- Provide common base functionality
- Enforce method implementation
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 19 covers Interfaces.
*/
