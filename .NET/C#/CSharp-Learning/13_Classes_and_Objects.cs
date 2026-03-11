/*
================================================================================
TOPIC 13: CLASSES AND OBJECTS
================================================================================

Classes are the building blocks of OOP. This topic covers how to create
and use classes effectively in C#.

TABLE OF CONTENTS:
1. Class Declaration
2. Fields
3. Properties
4. Methods
5. Creating Objects
6. Static Members
7. Partial Classes
================================================================================
*/

// ================================================================================
// SECTION 1: CLASS DECLARATION
// ================================================================================

/*
CLASS STRUCTURE:
----------------
[access modifier] class ClassName
{
    // Fields (data)
    // Properties (smart fields)
    // Constructors (initialization)
    // Methods (behavior)
    // Events (notifications)
}

ACCESS MODIFIERS:
-----------------
public   - Accessible everywhere
private  - Accessible only in this class (default)
protected - Accessible in this class and derived classes
internal - Accessible in same assembly
protected internal - Protected OR internal
private protected - Private within same assembly

Example: public class Person { }
*/


// ================================================================================
// SECTION 2: FIELDS
// ================================================================================

namespace FieldsExample
{
    class Person
    {
        // ====================================================================
        // FIELD TYPES
        // ====================================================================
        
        // Private field (encapsulated)
        private string name;
        
        // Private field with initialization
        private int age = 0;
        
        // Readonly field (cannot change after initialization)
        private readonly string id;
        
        // Static field (shared across all instances)
        private static int personCount = 0;
        
        // Constant (compile-time constant)
        private const string species = "Human";
        
        // Public field (generally not recommended!)
        public string email;  // Avoid this!
        
        // Constructor to initialize readonly
        public Person(string name, string id)
        {
            this.name = name;
            this.id = id; // Can only set here
            personCount++;
        }
        
        public void Display()
        {
            Console.WriteLine($"Name: {name}, Age: {age}, ID: {id}");
            Console.WriteLine($"Species: {species}");
            Console.WriteLine($"Total persons: {personCount}");
        }
    }
    
    class Program
    {
        static void Main()
        {
            Person p1 = new Person("John", "ABC123");
            Person p2 = new Person("Jane", "DEF456");
            
            p1.Display();
            Console.WriteLine();
            p2.Display();
        }
    }
}

/*
FIELD BEST PRACTICES:
---------------------
1. Always make fields private (encapsulation)
2. Use properties to access fields
3. Use readonly for values that shouldn't change
4. Use const for compile-time constants
5. Initialize fields at declaration or in constructor
*/


// ================================================================================
// SECTION 3: PROPERTIES
// ================================================================================

namespace PropertiesExample
{
    class Person
    {
        // ====================================================================
        // AUTO-PROPERTIES (Simplest)
        // ====================================================================
        
        public string Name { get; set; }
        public int Age { get; set; }
        
        // ====================================================================
        // PROPERTY WITH BACKING FIELD
        // ====================================================================
        
        private string _ssn;
        public string SSN
        {
            get { return _ssn; }
            set 
            { 
                // Validation
                if (value?.Length == 9)
                    _ssn = value;
            }
        }
        
        // ====================================================================
        // READ-ONLY PROPERTY
        // ====================================================================
        
        private int _id;
        public int Id 
        { 
            get { return _id; } 
        }
        
        // ====================================================================
        // COMPUTED PROPERTY
        // ====================================================================
        
        public int BirthYear 
        { 
            get { return DateTime.Now.Year - Age; } 
        }
        
        // Constructor
        public Person(string name, int age, string ssn, int id)
        {
            Name = name;
            Age = age;
            SSN = ssn;
            _id = id;
        }
    }
    
    class Program
    {
        static void Main()
        {
            Person person = new Person("John", 30, "123456789", 1);
            
            // Using properties
            Console.WriteLine($"Name: {person.Name}");
            Console.WriteLine($"Age: {person.Age}");
            Console.WriteLine($"SSN: {person.SSN}");
            Console.WriteLine($"ID: {person.Id}");
            Console.WriteLine($"Birth Year: {person.BirthYear}");
            
            // Set values
            person.Age = 31;
            Console.WriteLine($"New Age: {person.Age}");
            
            // Invalid SSN (won't set)
            person.SSN = "123";  // Too short
            Console.WriteLine($"SSN after invalid: {person.SSN}");
        }
    }
}

/*
PROPERTY TYPES:
---------------
Auto-property:      public string Name { get; set; }
Read-only:           public int Id { get; private set; }
Write-only (rare):   public string Password { private get; set; }
Computed:            public int Age => DateTime.Now.Year - BirthYear;
*/


// ================================================================================
// SECTION 4: METHODS IN CLASSES
// ================================================================================

namespace ClassMethods
{
    class Calculator
    {
        // ====================================================================
        // INSTANCE METHODS
        // ====================================================================
        
        public int Add(int a, int b)
        {
            return a + b;
        }
        
        public double Add(double a, double b)
        {
            return a + b;  // Method overloading
        }
        
        // ====================================================================
        // STATIC METHODS
        // ====================================================================
        
        public static int Multiply(int a, int b)
        {
            return a * b;
        }
        
        // ====================================================================
        // REF AND OUT PARAMETERS
        // ====================================================================
        
        public void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
        
        public bool Divide(int a, int b, out int result)
        {
            if (b == 0)
            {
                result = 0;
                return false;
            }
            result = a / b;
            return true;
        }
        
        // ====================================================================
        // PARAMETERS ARRAY
        // ====================================================================
        
        public int SumAll(params int[] numbers)
        {
            int sum = 0;
            foreach (int n in numbers)
                sum += n;
            return sum;
        }
    }
    
    class Program
    {
        static void Main()
        {
            Calculator calc = new Calculator();
            
            // Instance method
            Console.WriteLine($"Add(5,3): {calc.Add(5, 3)}");
            Console.WriteLine($"Add(5.5, 3.3): {calc.Add(5.5, 3.3)}");
            
            // Static method (no instance needed)
            Console.WriteLine($"Multiply(4, 5): {Calculator.Multiply(4, 5)}");
            
            // Ref parameters
            int x = 10, y = 20;
            Console.WriteLine($"Before swap: x={x}, y={y}");
            calc.Swap(ref x, ref y);
            Console.WriteLine($"After swap: x={x}, y={y}");
            
            // Out parameters
            int result;
            bool success = calc.Divide(10, 2, out result);
            Console.WriteLine($"Divide(10,2): {result}, Success: {success}");
            
            // Params array
            Console.WriteLine($"SumAll: {calc.SumAll(1,2,3,4,5)}");
        }
    }
}


// ================================================================================
// SECTION 5: CREATING OBJECTS
// ================================================================================

namespace CreatingObjects
{
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        
        // Default constructor
        public Person()
        {
            Name = "Unknown";
            Age = 0;
        }
        
        // Parameterized constructor
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
        
        public void Display()
        {
            Console.WriteLine($"Name: {Name}, Age: {Age}");
        }
    }
    
    class Program
    {
        static void Main()
        {
            // Using new keyword to create objects
            Person p1 = new Person();  // Default constructor
            Person p2 = new Person("John", 30);  // Parameterized
            
            p1.Display();
            p2.Display();
            
            // Object initializer (alternative syntax)
            Person p3 = new Person 
            { 
                Name = "Jane", 
                Age = 25 
            };
            p3.Display();
            
            // Anonymous type (no class definition needed!)
            var person = new { Name = "Bob", Age = 35 };
            Console.WriteLine($"Anonymous: {person.Name}, {person.Age}");
        }
    }
}

/*
OBJECT CREATION PROCESS:
-----------------------
1. new allocates memory (heap)
2. Constructor is called
3. Object reference is returned

USING NEW:
- Always use new to create objects
- Memory is managed automatically (garbage collector)
*/


// ================================================================================
// SECTION 6: STATIC MEMBERS
// ================================================================================

namespace StaticMembers
{
    class Counter
    {
        // Static field - shared across all instances
        private static int count = 0;
        
        // Instance field - unique per instance
        public int InstanceId { get; set; }
        
        // Static constructor (runs once)
        static Counter()
        {
            Console.WriteLine("Static constructor called");
        }
        
        // Constructor
        public Counter()
        {
            count++;
            InstanceId = count;
            Console.WriteLine($"Instance {InstanceId} created");
        }
        
        // Static method
        public static int GetCount()
        {
            return count;
        }
        
        // Static property
        public static int Count => count;
    }
    
    class Program
    {
        static void Main()
        {
            Console.WriteLine($"Count before: {Counter.GetCount()}");
            
            Counter c1 = new Counter();
            Counter c2 = new Counter();
            Counter c3 = new Counter();
            
            Console.WriteLine($"Count after: {Counter.Count}");
            
            // Cannot access instance members from static:
            // Counter.InstanceId  // ERROR!
        }
    }
}

/*
STATIC vs INSTANCE:
-------------------
STATIC:
- Shared across all objects
- Accessed via ClassName.Member
- No object needed to access

INSTANCE:
- Unique to each object
- Accessed via object.Member
- Requires object to access
*/


// ================================================================================
// SECTION 7: PRACTICE EXAMPLES
// =============================================================================

/*
EXERCISE 1: Rectangle Class
----------------------------
Create a Rectangle class with:
- Width and Height properties
- Area() and Perimeter() methods

EXERCISE 2: Book Class
----------------------
Create a Book class with:
- Title, Author, Price
- Discount property (0-100%)
- FinalPrice after discount

EXERCISE 3: Student Class
--------------------------
Create with static field:
- totalStudents (count)
- instance: name, grade
*/


// ================================================================================
// SECTION 8: INTERVIEW QUESTIONS
// =============================================================================

/*
Q1: What is the difference between a field and a property?
A: A field is a variable that stores data directly. A property is a 
   method-like construct that provides controlled access to a field.

Q2: What are static members in C#?
A: Static members belong to the class itself, not to instances.
   They are shared across all objects and accessed via ClassName.

Q3: What is object initialization in C#?
A: A syntax to set properties at creation time: new Person { Name = "John" }

Q4: What is the purpose of the 'new' keyword?
A: It allocates memory for an object on the heap and calls the constructor.

Q5: What are access modifiers in C#?
A: Keywords that define accessibility: public, private, protected, internal.
*/


// ================================================================================
// NEXT STEPS
// =============================================================================

/*
EXCELLENT! You now understand Classes and Objects.

WHAT'S NEXT:
In Topic 14, we'll learn about Constructors - special methods that
initialize objects when they're created.
*/
