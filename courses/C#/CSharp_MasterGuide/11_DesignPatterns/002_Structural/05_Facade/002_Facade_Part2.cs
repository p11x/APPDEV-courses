/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Facade Extended
 * FILE      : 02_Facade_Part2.cs
 * PURPOSE   : Extended Facade pattern examples
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._05_Facade
{
    /// <summary>
    /// Extended Facade examples
    /// </summary>
    public class FacadePatternExtended
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Facade Pattern Extended ===\n");

            // Example: Bank Account
            Console.WriteLine("1. Bank Account Facade:");
            var accountFacade = new BankAccountFacade("ACC-001", 1000);
            accountFacade.Deposit(500);
            accountFacade.Withdraw(200);
            
            // Output: Balance: 1300

            Console.WriteLine("\n=== Facade Extended Complete ===");
        }
    }

    /// <summary>
    /// Account subsystem
    /// </summary>
    public class AccountService
    {
        public bool VerifyAccount(string id) => true;
    }

    /// <summary>
    /// Security subsystem
    /// </summary>
    public class SecurityService
    {
        public bool Authenticate(string id, string pin) => true;
    }

    /// <summary>
    /// Transaction subsystem
    /// </summary>
    public class TransactionService
    {
        public void Deposit(string id, decimal amount) { }
        public void Withdraw(string id, decimal amount) { }
    }

    /// <summary>
    /// Bank account facade
    /// </summary>
    public class BankAccountFacade
    {
        private readonly AccountService _account;
        private readonly SecurityService _security;
        private readonly TransactionService _transaction;
        private decimal _balance;
        private readonly string _accountId;
        
        public BankAccountFacade(string accountId, decimal initialBalance)
        {
            _accountId = accountId;
            _balance = initialBalance;
            _account = new AccountService();
            _security = new SecurityService();
            _transaction = new TransactionService();
        }
        
        public void Deposit(decimal amount)
        {
            _transaction.Deposit(_accountId, amount);
            _balance += amount;
            Console.WriteLine($"   Deposited: {amount}, Balance: {_balance}");
        }
        
        public void Withdraw(decimal amount)
        {
            if (_balance >= amount)
            {
                _transaction.Withdraw(_accountId, amount);
                _balance -= amount;
                Console.WriteLine($"   Withdrawn: {amount}, Balance: {_balance}");
            }
        }
    }
}