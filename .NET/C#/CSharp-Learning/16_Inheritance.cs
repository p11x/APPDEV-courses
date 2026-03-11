/*
================================================================================
TOPIC 16: INHERITANCE
================================================================================

Inheritance allows a class to inherit properties and methods from another class.

TABLE OF CONTENTS:
1. What is Inheritance?
2. Base and Derived Classes
3. The base Keyword
4. Method Overriding
5. Sealed Classes
================================================================================
*/

namespace InheritanceExamples
{
    // ====================================================================
    // BASE CLASS
    // ====================================================================
    
    class Animal
    {
        public string Name { get; set; }
        public int Age { get; set; }
        
        public Animal(string name, int age)
        {
            Name = name;
            Age = age;
        }
        
        public void Eat()
        {
            Console.WriteLine($"{Name} is eating");
        }
        
        public virtual void MakeSound()  // Virtual for overriding
        {
            Console.WriteLine("Some sound");
        }
    }
    
    // ====================================================================
    // DERIVED CLASS - inherits from Animal
    // ====================================================================
    
    class Dog : Animal
    {
        public string Breed { get; set; }
        
        public Dog(string name, int age, string breed) : base(name, age)
        {
            Breed = breed;
        }
        
        // Override base method
        public override void MakeSound()
        {
            Console.WriteLine($"{Name} says: Woof! Woof!");
        }
        
        // New method specific to Dog
        public void Fetch()
        {
            Console.WriteLine($"{Name} fetches the ball!");
        }
    }
    
    class Cat : Animal
    {
        public bool IsIndoor { get; set; }
        
        public Cat(string name, int age, bool isIndoor) : base(name, age)
        {
            IsIndoor = isIndoor;
        }
        
        public override void MakeSound()
        {
            Console.WriteLine($"{Name} says: Meow!");
        }
        
        public void Scratch()
        {
            Console.WriteLine($"{Name} scratches!");
        }
    }
    
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Inheritance ===");
            
            Dog dog = new Dog("Buddy", 3, "Golden Retriever");
            dog.Eat();
            dog.MakeSound();
            dog.Fetch();
            
            Cat cat = new Cat("Whiskers", 2, true);
            cat.Eat();
            cat.MakeSound();
            
            // Polymorphism
            Console.WriteLine("\n=== Polymorphism ===");
            Animal[] animals = { dog, cat };
            foreach (Animal animal in animals)
            {
                animal.MakeSound();  // Different behavior!
            }
        }
    }
}

/*
KEY POINTS:
-----------
- Use : to inherit from a class
- base() calls parent constructor
- virtual/override for method overriding
- sealed prevents further inheritance
*/

// ================================================================================
// INTERVIEW QUESTIONS
// =============================================================================

/*
Q1: What is inheritance in C#?
A: A mechanism where a class acquires properties and methods from another class.

Q2: What is the base keyword?
A: Used to access members of the parent class from a derived class.

Q3: Can C# support multiple inheritance?
A: No, C# only supports single class inheritance but multiple interface inheritance.
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 17 covers Polymorphism.
*/
