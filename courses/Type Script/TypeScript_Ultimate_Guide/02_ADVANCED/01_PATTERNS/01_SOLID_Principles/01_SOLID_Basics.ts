/**
 * Category: ADVANCED
 * Subcategory: PATTERNS
 * Concept: SOLID_Principles
 * Purpose: Understanding SOLID principles in TypeScript
 * Difficulty: intermediate
 * UseCase: web, backend, enterprise
 */

/**
 * SOLID Principles - Comprehensive Guide
 * ========================================
 * 
 * 📚 WHAT: Five design principles for object-oriented programming
 * 💡 WHY: Creates maintainable, scalable, and testable code
 * 🔧 HOW: Interface segregation, dependency inversion, and more
 */

// ============================================================================
// SECTION 1: SINGLE RESPONSIBILITY PRINCIPLE (S)
// ============================================================================

// Example 1.1: Violating SRP
// -----------------------

// Bad: One class doing too much
class UserManager {
  createUser(data: any): void {
    // Creating user
    console.log("Creating user");
  }
  
  sendEmail(user: any): void {
    // Sending email - shouldn't be here
    console.log("Sending email");
  }
  
  generateReport(user: any): void {
    // Generating report - shouldn't be here
    console.log("Generating report");
  }
}

// Good: Separate classes with single responsibility
interface UserCreator {
  create(data: any): User;
}

interface UserRepository {
  save(user: User): void;
}

interface EmailService {
  sendEmail(to: string, subject: string, body: string): void;
}

class UserService implements UserCreator, UserRepository {
  create(data: any): User {
    return { name: data.name, email: data.email };
  }
  
  save(user: User): void {
    console.log("Saving user to database");
  }
}

interface User {
  name: string;
  email: string;
}

// ============================================================================
// SECTION 2: OPEN/CLOSED PRINCIPLE (O)
// ============================================================================

// Example 2.1: Violating OCP
// -----------------------

// Bad: Adding new shapes requires modifying existing code
class AreaCalculator {
  calculate(shape: any): number {
    if (shape.type === "circle") {
      return Math.PI * shape.radius ** 2;
    } else if (shape.type === "rectangle") {
      return shape.width * shape.height;
    }
    return 0;
  }
}

// Good: Open for extension, closed for modification
interface Shape {
  area(): number;
}

class Circle implements Shape {
  constructor(public radius: number) {}
  
  area(): number {
    return Math.PI * this.radius ** 2;
  }
}

class Rectangle implements Shape {
  constructor(public width: number, public height: number) {}
  
  area(): number {
    return this.width * this.height;
  }
}

function totalArea(shapes: Shape[]): number {
  return shapes.reduce((sum, shape) => sum + shape.area(), 0);
}

// ============================================================================
// SECTION 3: LISKOV SUBSTITUTION PRINCIPLE (L)
// ============================================================================

// Example 3.1: Violating LSP
// -----------------------

// Bad: Subclass changes behavior unexpectedly
class Bird {
  fly(): void {
    console.log("Flying");
  }
}

class Penguin extends Bird {
  fly(): void {
    throw new Error("Penguins can't fly!");
  }
}

// Good: Proper inheritance
abstract class AbstractBird {
  abstract move(): void;
}

class FlyingBird extends AbstractBird {
  move(): void {
    console.log("Flying");
  }
}

class PenguinBird extends AbstractBird {
  move(): void {
    console.log("Swimming");
  }
}

// ============================================================================
// SECTION 4: INTERFACE SEGREGATION PRINCIPLE (I)
// ============================================================================

// Example 4.1: Violating ISP
// -----------------------

// Bad: Large interface forcing unnecessary implementations
interface Worker {
  work(): void;
  eat(): void;
  sleep(): void;
}

class Robot implements Worker {
  work(): void { console.log("Working"); }
  eat(): void { /* Robot doesn't eat */ }
  sleep(): void { /* Robot doesn't sleep */ }
}

// Good: Segregated interfaces
interface Workable {
  work(): void;
}

interface Eatable {
  eat(): void;
}

interface Sleepable {
  sleep(): void;
}

class HumanWorker implements Workable, Eatable, Sleepable {
  work(): void { console.log("Working"); }
  eat(): void { console.log("Eating"); }
  sleep(): void { console.log("Sleeping"); }
}

class RobotWorker implements Workable {
  work(): void { console.log("Working"); }
}

// ============================================================================
// SECTION 5: DEPENDENCY INVERSION PRINCIPLE (D)
// ============================================================================

// Example 5.1: Violating DIP
// -----------------------

// Bad: High-level module depends on low-level module
class MySQLDatabase {
  connect(): void {
    console.log("Connecting to MySQL");
  }
  
  query(sql: string): void {
    console.log(`Querying: ${sql}`);
  }
}

class UserService {
  private db = new MySQLDatabase();
  
  getUsers(): void {
    this.db.connect();
    this.db.query("SELECT * FROM users");
  }
}

// Good: Both depend on abstractions
interface Database {
  connect(): void;
  query(sql: string): void;
}

class MySQL implements Database {
  connect(): void { console.log("MySQL"); }
  query(sql: string): void { console.log(sql); }
}

class PostgreSQL implements Database {
  connect(): void { console.log("PostgreSQL"); }
  query(sql: string): void { console.log(sql); }
}

class UserServiceGood {
  constructor(private db: Database) {}
  
  getUsers(): void {
    this.db.connect();
    this.db.query("SELECT * FROM users");
  }
}

console.log("\n=== SOLID Principles Complete ===");
console.log("Next: ADVANCED/PATTERNS/02_Creational_Patterns");