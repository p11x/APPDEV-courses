/*
================================================================================
TOPIC 15: ENCAPSULATION
================================================================================

Encapsulation bundles data and methods while restricting access to internal state.

TABLE OF CONTENTS:
1. What is Encapsulation?
2. Access Modifiers
3. Data Hiding
4. Properties for Controlled Access
5. Benefits of Encapsulation
================================================================================
*/

namespace EncapsulationExamples
{
    // ====================================================================
    // BEFORE ENCAPSULATION - Poor design (don't do this!)
    // ====================================================================
    
    class BadBankAccount
    {
        public string accountNumber;  // Public - anyone can modify!
        public decimal balance;      // Public - no validation!
        
        // Anyone can set any value!
    }
    
    // ====================================================================
    // AFTER ENCAPSULATION - Proper design
    // ====================================================================
    
    class BankAccount
    {
        // Private fields - hidden from outside
        private string accountNumber;
        private decimal balance;
        private string ownerName;
        
        // Public properties - controlled access
        public string AccountNumber
        {
            get { return accountNumber; }
            private set { accountNumber = value; }  // Can only be set internally
        }
        
        public decimal Balance
        {
            get { return balance; }
        }
        
        public string OwnerName
        {
            get { return ownerName; }
            set { ownerName = value; }
        }
        
        // Constructor
        public BankAccount(string accountNumber, string ownerName, decimal initialBalance)
        {
            this.accountNumber = accountNumber;
            this.ownerName = ownerName;
            
            // Validation in constructor
            if (initialBalance < 0)
                balance = 0;
            else
                balance = initialBalance;
        }
        
        // Controlled methods
        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                balance += amount;
                Console.WriteLine($"Deposited: {amount:C}, New Balance: {balance:C}");
            }
            else
            {
                Console.WriteLine("Invalid deposit amount");
            }
        }
        
        public bool Withdraw(decimal amount)
        {
            if (amount > 0 && amount <= balance)
            {
                balance -= amount;
                Console.WriteLine($"Withdrew: {amount:C}, New Balance: {balance:C}");
                return true;
            }
            Console.WriteLine("Invalid withdrawal or insufficient funds");
            return false;
        }
    }
    
    // ====================================================================
    // AUTO-PROPERTIES (Simpler encapsulation)
    // ====================================================================
    
    class Person
    {
        // Full encapsulation with auto-property
        public string Name { get; set; }
        
        // Read-only property
        public int Id { get; private set; }
        
        // With validation in property
        private int _age;
        public int Age
        {
            get { return _age; }
            set 
            {
                if (value >= 0 && value <= 150)
                    _age = value;
            }
        }
        
        // Private setter
        public string Email { get; private set; }
        
        public Person(string name, int id, string email)
        {
            Name = name;
            Id = id;
            Email = email;
        }
    }
    
    class Program
    {
        static void Main()
        {
            // Good encapsulation
            BankAccount account = new BankAccount("12345", "John", 1000);
            
            Console.WriteLine($"Account: {account.AccountNumber}");
            Console.WriteLine($"Balance: {account.Balance:C}");
            
            // Controlled modifications
            account.Deposit(500);
            account.Withdraw(200);
            
            // Cannot do this with encapsulation:
            // account.balance = -1000000;  // ERROR - private!
            // account.AccountNumber = "99999"; // ERROR - private set!
            
            // Auto-properties
            Person p = new Person("Jane", 1, "jane@email.com");
            p.Name = "Jane Doe";
            p.Age = 30;
            Console.WriteLine($"\nPerson: {p.Name}, Age: {p.Age}, Email: {p.Email}");
        }
    }
}

/*
ACCESS MODIFIERS:
-----------------
public   - Accessible everywhere
private  - Only within same class (default for classes)
protected - Same class + derived classes
internal - Same assembly only
protected internal - Protected OR internal
*/

// ================================================================================
// INTERVIEW QUESTIONS
// =============================================================================

/*
Q1: What is encapsulation?
A: Bundling data with methods that operate on it, while restricting direct
   access to some components.

Q2: Why is encapsulation important?
A: Protects data from invalid values, maintains data integrity, and allows
   implementation changes without breaking other code.

Q3: What are access modifiers?
A: Keywords that control visibility: public, private, protected, internal.

Q4: What difference between a field is the and a property?
A: Field is storage, property provides controlled access to that storage.
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 16 covers Inheritance - creating new classes from existing ones.
*/
