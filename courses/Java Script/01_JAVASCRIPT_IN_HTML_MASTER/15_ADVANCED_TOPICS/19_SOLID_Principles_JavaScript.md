# 🎯 SOLID Principles in JavaScript

## 📋 Overview

SOLID is a set of five design principles for writing maintainable, flexible, and scalable code.

---

## 🎯 S - Single Responsibility

```javascript
// ❌ Bad - multiple responsibilities
class UserManager {
    saveUser(user) { /* save to DB */ }
    sendEmail(user) { /* send email */ }
    generateReport(user) { /* generate report */ }
}

// ✅ Good - separated concerns
class UserRepository { saveUser(user) { } }
class EmailService { sendEmail(user) { } }
class ReportGenerator { generateReport(user) { } }
```

## 🎯 O - Open/Closed

```javascript
// Open for extension, closed for modification
class PaymentProcessor {
    process(payment) {
        if (payment.type === 'credit') {
            return this.processCredit(payment);
        } else if (payment.type === 'debit') {
            return this.processDebit(payment);
        }
    }
}

// Better: Use strategy pattern
class PaymentProcessor {
    constructor(strategy) {
        this.strategy = strategy;
    }
    
    process(payment) {
        return this.strategy.pay(payment);
    }
}
```

## 🎯 L - Liskov Substitution

```javascript
// Subtypes should be substitutable for base types
class Bird {
    fly() { }
}

class Penguin extends Bird {
    fly() { 
        throw new Error('Penguins cannot fly!'); // ❌ Violates Liskov
    }
}

// ✅ Fixed
class Bird { }

class FlyingBird extends Bird {
    fly() { }
}

class Penguin extends Bird {
    swim() { }
}
```

## 🎯 I - Interface Segregation

```javascript
// ❌ Fat interface
class Machine {
    print() { }
    scan() { }
    fax() { }
}

// Client only needs print
class SimplePrinter extends Machine {
    print() { }
    scan() { throw new Error('Not supported'); } // ❌
}

// ✅ Segregated interfaces
class Printer { print() { } }
class Scanner { scan() { } }
class Fax { fax() { } }

class AllInOnePrinter implements Printer, Scanner, Fax {
    print() { }
    scan() { }
    fax() { }
}
```

## 🎯 D - Dependency Inversion

```javascript
// ❌ High-level depends on low-level
class MySQLUserRepository {
    save(user) { /* MySQL logic */ }
}

// ✅ Depend on abstractions
class UserRepository {
    save(user) { } // Abstract method
}

class MySQLUserRepository extends UserRepository {
    save(user) { /* MySQL logic */ }
}

class UserService {
    constructor(userRepo) {
        this.userRepo = userRepo;
    }
    
    create(user) {
        this.userRepo.save(user);
    }
}
```

---

## 🔗 Related Topics

- [18_Design_Patterns_Complete.md](./18_Design_Patterns_Complete.md)

---

**Next: [JavaScript Architecture](./20_JavaScript_Architecture.md)**