/*
================================================================================
TOPIC 12: OBJECT-ORIENTED PROGRAMMING (OOP) OVERVIEW
================================================================================

OOP is a programming paradigm that organizes code around objects rather
than functions. C# is a fully object-oriented language.

TABLE OF CONTENTS:
1. What is OOP?
2. The Four Pillars
3. Benefits of OOP
4. OOP Principles in C#
5. Classes vs Objects
6. Key OOP Terminology
================================================================================
*/

// ================================================================================
// SECTION 1: WHAT IS OOP?
// ================================================================================

/*
OBJECT-ORIENTED PROGRAMMING:
---------------------------
OOP is a way of organizing code that treats data and behavior as a single
unit called an "object".

REAL-WORLD ANALOGY:
-------------------
Think of a car:
- DATA (Properties): Color, speed, fuel level
- BEHAVIOR (Methods): Start(), Stop(), Accelerate()

The car is an object that combines both data and actions.

PROCEDURAL vs OOP:
------------------
Procedural:                         OOP:
- Functions + Data                 - Objects with Data + Methods
- Global data                       - Encapsulated data
- Hard to maintain                  - Modular and maintainable

C# IS FULLY OBJECT-ORIENTED:
-----------------------------
- Everything is an object (even strings!)
- Predefined types are classes
- You create custom types as classes
*/


// ================================================================================
// SECTION 2: THE FOUR PILLARS OF OOP
// ================================================================================

/*
THE FOUR PILLARS:
----------------

1. ENCAPSULATION
   - Bundling data and methods together
   - Controlling access through access modifiers
   - Hiding internal implementation details
   
   REAL-WORLD: A remote control - you press buttons (interface),
               you don't know how it works inside (hidden)

2. INHERITANCE
   - Creating new classes from existing ones
   - Reusing code and establishing relationships
   - Parent/Child relationship
   
   REAL-WORLD: A child inherits features from parents

3. POLYMORPHISM
   - Same interface, different implementations
   - One method can work with different types
   - Override behavior in derived classes
   
   REAL-WORLD: A "draw" method works differently for Circle vs Square

4. ABSTRACTION
   - Showing only essential features
   - Hiding complex implementation
   - Using interfaces/abstract classes
   
   REAL-WORLD: Driving a car - you use pedals and steering wheel,
               you don't need to know engine mechanics
*/


// ================================================================================
// SECTION 3: BENEFITS OF OOP
// ================================================================================

/*
WHY USE OOP?
------------

1. MODULARITY
   - Each object is self-contained
   - Easy to understand and debug
   - Work on one part without affecting others

2. REUSABILITY
   - Inheritance allows code reuse
   - Create libraries of reusable components
   - Don't repeat yourself (DRY)

3. FLEXIBILITY
   - Polymorphism allows flexibility
   - Different implementations can be swapped
   - Easy to extend functionality

4. SECURITY
   - Encapsulation protects data
   - Controlled access to internals
   - Prevents unauthorized modifications

5. MAINTAINABILITY
   - Organized code structure
   - Easy to modify without breaking
   - Clear separation of concerns
*/


// ================================================================================
// SECTION 4: OOP PRINCIPLES IN C#
// ================================================================================

/*
C# OOP IMPLEMENTATION:
----------------------

1. ENCAPSULATION:
   - Access modifiers: public, private, protected, internal
   - Properties (getters/setters)
   - Data hiding through private fields

2. INHERITANCE:
   - Class inheritance with :
   - Single inheritance only (one parent)
   - Interface inheritance (multiple)

3. POLYMORPHISM:
   - Method overriding (virtual/override)
   - Method overloading
   - Interface implementation

4. ABSTRACTION:
   - Abstract classes
   - Interfaces
   - Hiding implementation details
*/


// ================================================================================
// SECTION 5: CLASSES VS OBJECTS
// ================================================================================

/*
CLASS vs OBJECT:
---------------

CLASS = Blueprint/Template
- Defines what an object will have
- Doesn't take up memory
- Like a recipe

OBJECT = Instance of a class
- Created from the blueprint
- Takes up memory
- Like a cake made from recipe

EXAMPLE:
--------

CLASS (Blueprint):
------------------
class Person
{
    string Name;     // Field
    int Age;         // Field
    
    void Speak()    // Method
    {
        // speak logic
    }
}

OBJECT (Instance):
------------------
Person john = new Person();
john.Name = "John";
john.Age = 30;
john.Speak();

Person jane = new Person();  // Another object
jane.Name = "Jane";
*/


// ================================================================================
// SECTION 6: KEY OOP TERMINOLOGY
// ================================================================================

/*
IMPORTANT TERMS:
----------------

CLASS: Blueprint for creating objects
- Contains fields, properties, methods, events

OBJECT: Instance of a class
- Created using new keyword
- Has identity, state, behavior

FIELD: Variable inside a class
- Stores data
- Usually private

PROPERTY: Smart field
- Has get/set accessors
- Can have validation

METHOD: Function inside a class
- Defines behavior
- Can return values

CONSTRUCTOR: Special method
- Called when object is created
- Initializes the object

INTERFACE: Contract
- Defines method signatures
- No implementation

ABSTRACT: Cannot be instantiated
- May have abstract members
- Serves as base
*/


// ================================================================================
// SECTION 7: PRACTICAL EXAMPLES
// ================================================================================

namespace OOPBasics
{
    // ====================================================================
    // A CLASS - The blueprint
    // ====================================================================
    
    // This is a class definition - the blueprint
    class BankAccount
    {
        // Fields (private - encapsulated)
        private string accountNumber;
        private decimal balance;
        
        // Properties (public interface)
        public string AccountNumber
        {
            get { return accountNumber; }
            private set { accountNumber = value; }
        }
        
        public decimal Balance
        {
            get { return balance; }
        }
        
        // Constructor
        public BankAccount(string number, decimal initialBalance)
        {
            accountNumber = number;
            balance = initialBalance;
        }
        
        // Methods (behavior)
        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                balance += amount;
                Console.WriteLine($"Deposited {amount}. New balance: {balance}");
            }
        }
        
        public void Withdraw(decimal amount)
        {
            if (amount > 0 && amount <= balance)
            {
                balance -= amount;
                Console.WriteLine($"Withdrew {amount}. New balance: {balance}");
            }
            else
            {
                Console.WriteLine("Invalid withdrawal amount or insufficient funds");
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // OBJECTS - Instances of the class
            // ====================================================================
            
            // Create objects using new keyword
            BankAccount account1 = new BankAccount("12345", 1000);
            BankAccount account2 = new BankAccount("67890", 500);
            
            // Use the objects
            Console.WriteLine($"Account: {account1.AccountNumber}");
            Console.WriteLine($"Initial Balance: {account1.Balance}");
            
            // Call methods
            account1.Deposit(500);
            account1.Withdraw(200);
            
            // Different object
            account2.Deposit(1000);
            
            Console.WriteLine($"\n{account1.AccountNumber} Balance: {account1.Balance}");
            Console.WriteLine($"{account2.AccountNumber} Balance: {account2.Balance}");
        }
    }
}

/*
WHAT WE DEMONSTRATED:
---------------------
1. ENCAPSULATION: balance is private, accessed via properties/methods
2. ABSTRACTION: User doesn't know how deposit works internally
3. The same class can create multiple objects with different data
*/


// ================================================================================
// SECTION 8: INTERVIEW QUESTIONS
// ================================================================================

/*
Q1: What are the four pillars of OOP?
A: Encapsulation, Inheritance, Polymorphism, and Abstraction.

Q2: What is the difference between a class and an object?
A: A class is a blueprint/template; an object is an instance of a class
   that takes up memory.

Q3: Why is encapsulation important?
A: It protects data from unauthorized access, ensures data integrity,
   and allows implementation changes without affecting other code.

Q4: How does inheritance promote code reuse?
A: Child classes inherit fields and methods from parent classes,
   eliminating the need to rewrite code.

Q5: What is polymorphism in C#?
A: The ability of different classes to respond to the same method call
   in different ways, achieved through method overriding.
*/


// ================================================================================
// NEXT STEPS
// =============================================================================

/*
Now that you understand OOP concepts, you're ready to dive deeper!

WHAT'S NEXT:
In Topic 13, we'll learn about Classes and Objects in detail - how to
create and use them effectively in C#.
*/
