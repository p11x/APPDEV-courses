/*
================================================================================
TOPIC 14: CONSTRUCTORS
================================================================================

Constructors are special methods that initialize objects when they're created.

TABLE OF CONTENTS:
1. What are Constructors?
2. Default Constructors
3. Parameterized Constructors
4. Constructor Overloading
5. Static Constructors
6. Private Constructors
7. Constructor Chaining
================================================================================
*/

namespace ConstructorExamples
{
    // ====================================================================
    // BASIC CLASS WITH MULTIPLE CONSTRUCTORS
    // ====================================================================
    
    class Person
    {
        // Fields
        private string name;
        private int age;
        private string email;
        
        // 1. Default Constructor (no parameters)
        public Person()
        {
            name = "Unknown";
            age = 0;
            email = "notset@example.com";
        }
        
        // 2. Parameterized Constructor (one parameter)
        public Person(string name)
        {
            this.name = name;
            age = 0;
            email = "notset@example.com";
        }
        
        // 3. Parameterized Constructor (two parameters)
        public Person(string name, int age)
        {
            this.name = name;
            this.age = age;
            email = "notset@example.com";
        }
        
        // 4. Full Constructor (all parameters)
        public Person(string name, int age, string email)
        {
            this.name = name;
            this.age = age;
            this.email = email;
        }
        
        // 5. Static Constructor
        static Person()
        {
            Console.WriteLine("Static constructor called");
        }
        
        // Method to display info
        public void Display()
        {
            Console.WriteLine($"Name: {name}, Age: {age}, Email: {email}");
        }
    }
    
    // ====================================================================
    // CONSTRUCTOR CHAINING EXAMPLE
    // ====================================================================
    
    class BankAccount
    {
        private string accountNumber;
        private decimal balance;
        private string accountType;
        
        // Primary constructor with all parameters
        public BankAccount(string accountNumber, decimal balance, string accountType)
        {
            this.accountNumber = accountNumber;
            this.balance = balance;
            this.accountType = accountType;
        }
        
        // Chain to primary constructor using : this()
        public BankAccount(string accountNumber, decimal balance) 
            : this(accountNumber, balance, "Checking")
        {
        }
        
        public BankAccount(string accountNumber) 
            : this(accountNumber, 0, "Checking")
        {
        }
        
        // Default calls primary with defaults
        public BankAccount() 
            : this("UNKNOWN", 0, "Checking")
        {
        }
        
        public void Display()
        {
            Console.WriteLine($"Account: {accountNumber}, Balance: {balance:C}, Type: {accountType}");
        }
    }
    
    // ====================================================================
    // PRIVATE CONSTRUCTOR EXAMPLE
    // ====================================================================
    
    class Singleton
    {
        private static Singleton _instance;
        
        // Private constructor - cannot create with 'new'
        private Singleton()
        {
            Console.WriteLine("Private constructor called");
        }
        
        public static Singleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Singleton();
            }
            return _instance;
        }
        
        public void DoSomething()
        {
            Console.WriteLine("Singleton method called");
        }
    }
    
    class Program
    {
        static void Main()
        {
            // Default constructor
            Console.WriteLine("=== Default Constructor ===");
            Person p1 = new Person();
            p1.Display();
            
            // Parameterized constructors
            Console.WriteLine("\n=== Parameterized Constructors ===");
            Person p2 = new Person("Alice");
            p2.Display();
            
            Person p3 = new Person("Bob", 30);
            p3.Display();
            
            Person p4 = new Person("Charlie", 25, "charlie@email.com");
            p4.Display();
            
            // Constructor chaining
            Console.WriteLine("\n=== Constructor Chaining ===");
            BankAccount b1 = new BankAccount("12345", 1000, "Savings");
            b1.Display();
            
            BankAccount b2 = new BankAccount("67890", 500);
            b2.Display();
            
            BankAccount b3 = new BankAccount("11111");
            b3.Display();
            
            BankAccount b4 = new BankAccount();
            b4.Display();
            
            // Private constructor (Singleton pattern)
            Console.WriteLine("\n=== Private Constructor ===");
            Singleton s1 = Singleton.GetInstance();
            Singleton s2 = Singleton.GetInstance();
            
            Console.WriteLine($"Same instance: {s1 == s2}");
        }
    }
}

/*
CONSTRUCTOR TYPES:
------------------
1. Default - No parameters
2. Parameterized - Has parameters
3. Static - Initializes static members
4. Private - Restricts object creation
5. Copy - Creates copy of object

KEY POINTS:
-----------
- Same name as class
- No return type (not even void)
- Called automatically when object is created
- Can be overloaded (multiple versions)
*/

// ================================================================================
// INTERVIEW QUESTIONS
// ================================================================================

/*
Q1: What is a constructor?
A: A special method called when an object is created, used to initialize it.

Q2: What is the difference between static and instance constructors?
A: Static runs once per type, instance runs for each object created.

Q3: What is constructor chaining?
A: One constructor calling another using 'this()' keyword.

Q4: Why use private constructor?
A: To prevent direct instantiation, often used in Singleton pattern.

Q5: Can constructors be overloaded?
A: Yes, just like methods - multiple constructors with different parameters.
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 15 covers Encapsulation - controlling access to class members.
*/
